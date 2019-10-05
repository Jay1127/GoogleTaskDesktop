using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;

namespace GoogleTaskDesktop.Core
{
    internal class ServiceLoader
    {
        private static TasksService _service;
        private static UserCredential _credential;

        public static TasksService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new TasksService(new Google.Apis.Services.BaseClientService.Initializer()
                    {
                        HttpClientInitializer = Credential,
                        ApplicationName = "SimpleGoogleTaskDesktop"
                    });
                }

                return _service;
            }
        }

        public static UserCredential Credential
        {
            get
            {
                if (_credential == null)
                {
                    var authService = new GoogleAuthService();
                    _credential = authService.Authorize();
                }
                return _credential;
            }
        }

        public ServiceLoader()
        {
        }
    }
}