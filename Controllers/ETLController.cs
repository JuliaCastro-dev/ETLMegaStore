namespace ETLMegaStore.Controllers
{
    public class ETLController
    {
        private readonly IConfiguration configuration;

        public ETLController(IConfiguration config)
        {
            configuration = config;

        }
    }
}
