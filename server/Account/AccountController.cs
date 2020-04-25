using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace server.Account
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private AccountService accountService;
        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<SignupResponse>> SignupAsync(SignupRequest request)
        {
            
            var response = await accountService.CreateUser(request.FirstName, request.LastName, request.Email, request.Password);

            return response;
        }
    }
}