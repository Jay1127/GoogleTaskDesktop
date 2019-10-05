using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

namespace GoogleTaskDesktop.Core
{
    /// <summary>
    /// 구글 인증 서비스
    /// </summary>
    public class GoogleAuthService
    {
        /// <summary>
        /// Credential파일 위치
        /// </summary>
        public string CredentialPath
        {
            get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "credentials.json");
        }

        /// <summary>
        /// 토큰 정보 저장 위치
        /// </summary>
        public string TokenStoragePath
        {
            get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SimpleGoogleTaskDesktop", "tokens");
        }

        /// <summary>
        /// 인증 요청 범위
        /// </summary>
        public string[] Scope = new[]
        {
            TasksService.Scope.Tasks
        };

        /// <summary>
        /// 인증하기
        /// </summary>
        /// <returns>사용자 인증 정보</returns>
        public UserCredential Authorize()
        {
            using (var stream = new FileStream(CredentialPath, FileMode.Open, FileAccess.Read))
            {
                var store = new FileDataStore(TokenStoragePath);

                return GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, 
                                                                    Scope, "user0", CancellationToken.None, store).Result;
            }
        }
    }
}