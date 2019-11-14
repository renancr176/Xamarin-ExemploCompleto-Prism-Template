using Flurl;
using Flurl.Http;
using TmsCollectorAndroid.Api_Old.Interfaces.Services;
using TmsCollectorAndroid.Api_Old.Models;

namespace TmsCollectorAndroid.Api_Old.Services
{
    public class ServiceBase : IServiceBase
    {
        public static int CompanyId { get; private set; }
        public static string Username { get; private set; }
        public static string Password { get; private set; }
        public static bool SetToClose { get; private set; }

        private static Url _apiUrl;
        public Url ApiUrl
        {
            get { return ((_apiUrl != null) ? new Url(_apiUrl) : _apiUrl); }
            private set { _apiUrl = value; }
        }
        protected static ApiTokenModel Token { get; private set; }

        public EmployeeViewInfoModel EmployeeViewInfo => Token.EmployeeViewInfo;

        public ServiceBase()
        {
            Authenticate();
        }

        public void SetApiUrl(IEnumerable<string> baseUrls)
        {
            var timeout = FlurlHttp.GlobalSettings.Timeout;
            FlurlHttp.GlobalSettings.Timeout = TimeSpan.FromSeconds(1);

            foreach (var baseUrl in baseUrls)
            {
                var apiUrl = new Url(baseUrl);

                if (apiUrl.IsValid())
                {
                    try
                    {
                        apiUrl.AppendPathSegment("ws/check-version")
                            .GetAsync()
                            .Wait();
                        ApiUrl = new Url(baseUrl);
                        break;
                    }
                    catch (FlurlHttpException e)
                    {
                        if (e.Call.Completed)
                        {
                            ApiUrl = new Url(baseUrl);
                            break;
                        }
                    }
                    catch (Exception) { }
                }
            }

            FlurlHttp.GlobalSettings.Timeout = timeout;
        }

        public async Task<bool> TestConection()
        {
            if (ApiUrl != null && ApiUrl.IsValid())
            {
                try
                {
                    await ApiUrl.GetAsync();
                    return true;
                }
                catch (FlurlHttpException e)
                {
                    return e.Call.Completed;
                }
            }

            return false;
        }

        private void Authenticate()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                Authenticate(CompanyId, Username, Password, SetToClose).Wait();
        }

        public async Task<bool> Authenticate(int companyId, string username, string password, bool setToClose)
        {
            if (ApiUrl != null && ApiUrl.IsValid() && (Token == null || !Token.IsValid()))
            {
                try
                {
                    Token = await ApiUrl.AppendPathSegment("token")
                        .PostUrlEncodedAsync(
                            new
                            {
                                companyId,
                                username,
                                password,
                                setToClose,
                                grant_type = "password"
                            }
                        )
                        .ReceiveJson<ApiTokenModel>();

                    Token.EmployeeViewInfo = await ApiUrl.AppendPathSegment("ws/me")
                        .GetJsonAsync<EmployeeViewInfoModel>();

                    CompanyId = companyId;
                    Username = username;
                    Password = password;
                    SetToClose = setToClose;

                    return true;
                }
                catch (FlurlHttpException) { }
                catch (Exception) { }
            }

            return false;
        }

        public async Task<string> CheckVersion()
        {
            try
            {
                return await ApiUrl.AppendPathSegment("ws/check-version")
                    .GetStringAsync();
            }
            catch (FlurlHttpException) { }
            catch (Exception) { }

            return String.Empty;
        }

        public async Task<string> GetWebServiceVersion()
        {
            try
            {
                return await ApiUrl.AppendPathSegment("ws/web-service-version")
                    .GetStringAsync();
            }
            catch (FlurlHttpException) { }
            catch (Exception) { }

            return String.Empty;
        }
    }
}