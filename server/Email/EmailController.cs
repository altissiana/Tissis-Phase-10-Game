using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace server.Email
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private EmailService emailService;
        public EmailController(EmailService emailService)
        {
            this.emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<ActionResult> Send(SendEmailRequest request)
        {
            await emailService.SendEmailAsync(request.Sender, request.Message);
            return Ok();
        }

    }
}