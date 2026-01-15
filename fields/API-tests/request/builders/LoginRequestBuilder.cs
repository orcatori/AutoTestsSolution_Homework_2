using fields.request.login;

namespace fields.request.builders
{
    public class LoginRequestBuilder
    {
        private string _grantType = "password";
        private string _username = "";
        private string _password = "";

        public LoginRequestBuilder WithGrantType(string grantType)
        {
            _grantType = grantType;
            return this;
        }

        public LoginRequestBuilder WithUsername(string username)
        {
            _username = username;
            return this;
        }

        public LoginRequestBuilder WithPassword(string password)
        {
            _password = password;
            return this;
        }

        public LoginRequest Build()
        {
            return new LoginRequest(_grantType, _username, _password);
        }
    }
}