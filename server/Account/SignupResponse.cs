namespace server.Account
{
    public class SignupResponse
    {
        public SignupResponseType signupResponseType{ get; set; }
        public SignupResponse(SignupResponseType signupResponseType)
        {
            this.signupResponseType = signupResponseType;
        }
    }
}