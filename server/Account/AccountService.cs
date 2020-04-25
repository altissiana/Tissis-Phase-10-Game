using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace server.Account
{
    public class AccountService
    {
        private HttpClient client;
        private ILogger<AccountService> logger;
        private static string userRoute = "users?activate=true";
        public AccountService(HttpClient client, ILogger<AccountService> logger)
        {
            this.client = client;
            this.logger = logger;
        }  

        public async Task<SignupResponse> CreateUser(string firstName, string lastName, string email, string password)
        {

            string payload = JsonConvert.SerializeObject(new {
                profile = new {
                    firstName = firstName,
                    lastName = lastName,
                    email = email,
                    login = email 
                },
                credentials = new {
                    password = new {
                        value = password
                    }
                }
                
            });
            StringContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(userRoute, content);
            logger.LogInformation("response", response);

            if (response.IsSuccessStatusCode)
            {
                return new SignupResponse(SignupResponseType.Created);
            } 
            else 
            {
                string result = await response.Content.ReadAsStringAsync();
                OktaSignupResponse oktaSignupResponse = JsonConvert.DeserializeObject<OktaSignupResponse>(result);
                if (oktaSignupResponse.ErrorSummary.Contains("email") || oktaSignupResponse.ErrorSummary.Contains("login"))
                {
                    return new SignupResponse(SignupResponseType.Email); 
                }
                else if (oktaSignupResponse.ErrorSummary.Contains("password"))
                {
                    return new SignupResponse(SignupResponseType.Password);
                }
                else 
                {
                    return new SignupResponse(SignupResponseType.Unknown);
                }
                
            }
        }
    }
}