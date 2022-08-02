namespace TakeBlipChatBotAPI
{
    public class Repository
    {
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Repository(string avatar, string name, string description)
        {
            Avatar = avatar;
            Name = name;
            Description = description;
        }
    }
}