using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using Twilio.Rest.Lookups.V1;

namespace signal_2019_dotnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : Controller
    {

        [HttpPost]
        public object Post([FromBody] LookupRequest request)
        {
            var lookupData = PhoneNumberResource.Fetch(
                type: new List<string> { "carrier" },
                pathPhoneNumber: new Twilio.Types.PhoneNumber($"{request.CountryCode}{request.PhoneNumber}")
            );

            return Ok(new { info = lookupData });
        }

        public class LookupRequest
        {
            [Required, JsonProperty(PropertyName = "phone_number")]
            public string PhoneNumber { get; set; }

            [Required, JsonProperty(PropertyName = "country_code")]
            public string CountryCode { get; set; }
        }
    }
}
