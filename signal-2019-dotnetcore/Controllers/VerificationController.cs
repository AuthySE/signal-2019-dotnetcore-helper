using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Twilio.Rest.Verify.V2.Service;

namespace signal_2019_dotnetcore.Controllers
{
    [ApiController]
    [Route("api/verification")]
    public class VerificationController : Controller
    {
        private readonly TwilioOptions _options;

        public VerificationController(IOptions<TwilioOptions> twilioOptions)
        {
            var options = twilioOptions.Value;

            _options = options;
        }

        [HttpPost]
        [Route("start")]
        public IActionResult StartVerification([FromBody] StartRequest request)
        {
            var verification = VerificationResource.Create(
                to: $"+{request.CountryCode}{request.PhoneNumber}",
                channel: request.Channel,
                locale: request.Locale,
                pathServiceSid: _options.VerifyServiceSid
            );

            return Ok(verification);
        }

        [HttpPost]
        [Route("verify")]
        public IActionResult CheckVerification([FromBody] CheckRequest request)
        {
            var verificationCheck = VerificationCheckResource.Create(
                to: $"+{request.CountryCode}{request.PhoneNumber}",
                code: request.Code,
                pathServiceSid: _options.VerifyServiceSid
            );

            return Ok(verificationCheck);
        }

        public class StartRequest
        {
            [Required, JsonProperty(PropertyName = "country_code")]
            public string CountryCode { get; set; }

            [Required, JsonProperty(PropertyName = "phone_number")]
            public string PhoneNumber { get; set; }

            [Required, JsonProperty(PropertyName = "via")]
            public string Channel { get; set; }

            [Required, JsonProperty(PropertyName = "locale")]
            public string Locale { get; set; }
        }

        public class CheckRequest
        {
            [Required, JsonProperty(PropertyName = "country_code")]
            public string CountryCode { get; set; }

            [Required, JsonProperty(PropertyName = "phone_number")]
            public string PhoneNumber { get; set; }

            [Required, JsonProperty(PropertyName = "token")]
            public string Code { get; set; }
        }
    }
}
