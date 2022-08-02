using Microsoft.AspNetCore.Mvc;

namespace TakeBlipChatBotAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositoryController : ControllerBase
    {
        private readonly ILogger<RepositoryController> _logger;

        public RepositoryController(ILogger<RepositoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetRepositoriesFromUser")]
        public IEnumerable<Repository> Get()
        {
            User user = new User("takenet", 5, "C#");
            return user.repositories;
        }
    }
}