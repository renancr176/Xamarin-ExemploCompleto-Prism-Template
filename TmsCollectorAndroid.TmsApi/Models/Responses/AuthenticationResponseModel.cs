using System;

namespace TmsCollectorAndroid.TmsApi.Models.Responses
{
    public class AuthenticationResponseModel
    {
        public bool IsAuthenticated { get; private set; }
        public string Message { get; private set; }

        public AuthenticationResponseModel(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            Message = String.Empty;
        }

        public AuthenticationResponseModel(bool isAuthenticated, string message)
            : this(isAuthenticated)
        {
            Message = message;
        }
    }
}