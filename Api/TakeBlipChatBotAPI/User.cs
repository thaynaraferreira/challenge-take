using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TakeBlipChatBotAPI
{
    public class User
    {
        public IEnumerable<Repository> repositories { get; set; }

        public User(string user, int numberOfRepositories, string language)
        {
            if (numberOfRepositories > 100) throw new Exception("You can only get 100 repositories at a time");
            this.repositories = GetRepositoriesFromUser(user, numberOfRepositories, language).Result;
        }

        public async Task<IEnumerable<Repository>> GetRepositoriesFromUser(string user, int numberOfRepositories, string language)
        {
            using (HttpClient client = new HttpClient())
            {
                List<Repository> repositoriesFromUser = new List<Repository>();
                int page = 1;

                while (repositoriesFromUser.Count() < numberOfRepositories)
                {
                    var content = new Dictionary<string, string?>
                    {
                        { "sort", "created" },
                        { "direction", "asc" },
                        { "per_page", numberOfRepositories.ToString() },
                        { "page", page.ToString() }
                    };

                    client.DefaultRequestHeaders.Add("User-Agent", "TakeBlipChatBotAPI");
                    client.DefaultRequestHeaders.Add("accept", "application/vnd.github+json");
                    var request = QueryHelpers.AddQueryString("https://api.github.com/users/" + user + "/repos", content);

                    HttpResponseMessage response = await client.GetAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<JObject> repositoryObjects = JsonConvert.DeserializeObject<List<JObject>>(responseBody);
                        
                        for (int i = 0; i < numberOfRepositories && repositoriesFromUser.Count() < numberOfRepositories; i++)
                        {
                            var repository = repositoryObjects[i];
                            string avatar = repository["owner"]["avatar_url"].ToString();
                            string name = repository["name"].ToString();
                            string description = repository["description"].ToString();
                            if (repository["language"].ToString() == language)
                                repositoriesFromUser.Add(new Repository(avatar, name, description));
                            else
                                page++;
                        }
                    }
                    else
                    {
                        throw new Exception("Error");
                    }
                }

                return repositoriesFromUser;
            }
        }
    }
}