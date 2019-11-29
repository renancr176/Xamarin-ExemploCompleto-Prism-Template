using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using TmsCollectorAndroid.TmsApi.Extensions;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Services
{
    public class ServiceBase : IServiceBase
    {
        public static int CompanyId { get; private set; }
        public static string Username { get; private set; }
        public static string Password { get; private set; }

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
            Authenticate().Wait();
        }

        public void SetApiUrl(IEnumerable<string> baseUrls)
        {
            var timeout = FlurlHttp.GlobalSettings.Timeout;
            FlurlHttp.GlobalSettings.Timeout = TimeSpan.FromSeconds(5);

            foreach (var baseUrl in baseUrls)
            {
                var apiUrl = new Url(baseUrl);

                if (apiUrl.IsValid())
                {
                    try
                    {
                        apiUrl.AppendPathSegment("ws/web-service-version")
                            .AllowAnyHttpStatus()
                            .GetAsync()
                            .Wait();
                        ApiUrl = new Url(baseUrl);
                        break;
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

        private async Task Authenticate()
        {
            if (ApiUrl != null && ApiUrl.IsValid() 
            && (Token == null || !Token.IsValid())
            && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                Token = await ApiUrl.AppendPathSegment("token")
                    .PostUrlEncodedAsync(
                        new
                        {
                            CompanyId,
                            Username,
                            Password,
                            grant_type = "password"
                        }
                    )
                    .ReceiveJson<ApiTokenModel>();

                Token.EmployeeViewInfo = await ApiUrl.AppendPathSegment("ws/me")
                    .WithOAuthBearerToken(Token.AccessToken)
                    .GetJsonAsync<EmployeeViewInfoModel>();
            }
        }

        public async Task<AuthenticationResponseModel> Authenticate(int companyId, string username, string password)
        {
            if (ApiUrl != null && ApiUrl.IsValid())
            {
                try
                {
                    var result = await ApiUrl.AppendPathSegment("token")
                        .AllowAnyHttpStatus()
                        .PostUrlEncodedAsync(
                            new
                            {
                                companyId,
                                username,
                                password,
                                grant_type = "password"
                            }
                        )
                        .HttpRequestResult<ApiTokenModel>();

                    if (result.IsSuccessStatusCode && result.Response != null)
                    {
                        Token = result.Response;

                        Token.EmployeeViewInfo = await ApiUrl.AppendPathSegment("ws/me")
                            .WithOAuthBearerToken(Token.AccessToken)
                            .GetJsonAsync<EmployeeViewInfoModel>();

                        CompanyId = companyId;
                        Username = username;
                        Password = password;

                        return new AuthenticationResponseModel(true);
                    }
                    else
                    {
                        return new AuthenticationResponseModel(false, result.Error?.ErrorDescription);
                    }
                }
                catch (FlurlHttpException) { }
                catch (Exception) { }
            }

            return new AuthenticationResponseModel(false);
        }

        public async Task FinalizeSession()
        {
            if (Token?.AccessToken != null)
            {
                await ApiUrl.AppendPathSegment("ws/finalize-session")
                    .WithOAuthBearerToken(Token.AccessToken)
                    .GetAsync();

                Token = null;
                CompanyId = 0;
                Username = String.Empty;
                Password = String.Empty;
            }
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

        #region Http Operations

        protected async Task<HttpRequestResult<TResponse>> GetAsyncAnonimouslyData<TResponse>(string pathSegment)
        {
            try
            {
                return await ApiUrl.AppendPathSegment(pathSegment)
                    .GetAsync()
                    .HttpRequestResult<TResponse>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<TResponse>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<TResponse>(HttpStatusCode.BadRequest);
        }

        protected async Task<HttpRequestResult<TResponse>> GetAsyncAuthenticatedData<TResponse>(string pathSegment)
        {
            try
            {
                return await ApiUrl.AppendPathSegment(pathSegment)
                    .WithOAuthBearerToken(Token.AccessToken)
                    .GetAsync()
                    .HttpRequestResult<TResponse>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<TResponse>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<TResponse>(HttpStatusCode.BadRequest);
        }

        protected async Task<HttpResponseMessage> GetAsync(string pathSegment)
        {
            try
            {
                return await ApiUrl.AppendPathSegment(pathSegment)
                    .WithOAuthBearerToken(Token.AccessToken)
                    .GetAsync();
            }
            catch (FlurlHttpException e)
            {
                return new HttpResponseMessage(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        protected async Task<HttpRequestResult<TResponse>> PostAsyncReceiveData<TResponse, TPost>(string pathSegment, TPost data)
        {
            try
            {
                return await ApiUrl.AppendPathSegment(pathSegment)
                    .WithOAuthBearerToken(Token.AccessToken)
                    .PostJsonAsync(data)
                    .HttpRequestResult<TResponse>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<TResponse>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<TResponse>(HttpStatusCode.BadRequest);
        }

        protected async Task<HttpResponseMessage> PostAsync<TPost>(string pathSegment, TPost data)
        {
            try
            {
                return await ApiUrl.AppendPathSegment(pathSegment)
                    .WithOAuthBearerToken(Token.AccessToken)
                    .PostJsonAsync(data);
            }
            catch (FlurlHttpException e)
            {
                return new HttpResponseMessage(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        protected async Task<HttpRequestResult<TResponse>> PutAsyncReceiveData<TResponse, TPut>(string pathSegment, TPut data)
        {
            try
            {
                return await ApiUrl.AppendPathSegment(pathSegment)
                    .WithOAuthBearerToken(Token.AccessToken)
                    .PutJsonAsync(data)
                    .HttpRequestResult<TResponse>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<TResponse>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<TResponse>(HttpStatusCode.BadRequest);
        }

        protected async Task<HttpResponseMessage> PutAsync<TPost>(string pathSegment, TPost data)
        {
            try
            {
                return await ApiUrl.AppendPathSegment(pathSegment)
                    .WithOAuthBearerToken(Token.AccessToken)
                    .PutJsonAsync(data);
            }
            catch (FlurlHttpException e)
            {
                return new HttpResponseMessage(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        protected async Task<HttpRequestResult<TResponse>> DeleteAsyncReceiveData<TResponse>(string pathSegment)
        {
            try
            {
                return await ApiUrl.AppendPathSegment(pathSegment)
                    .WithOAuthBearerToken(Token.AccessToken)
                    .DeleteAsync()
                    .HttpRequestResult<TResponse>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<TResponse>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<TResponse>(HttpStatusCode.BadRequest);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string pathSegment)
        {
            try
            {
                return await ApiUrl.AppendPathSegment(pathSegment)
                    .WithOAuthBearerToken(Token.AccessToken)
                    .DeleteAsync();
            }
            catch (FlurlHttpException e)
            {
                return new HttpResponseMessage(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}