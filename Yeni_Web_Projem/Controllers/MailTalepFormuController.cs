using Microsoft.AspNetCore.Mvc;
using System;

namespace Yeni_Web_Projem.Controllers
{
    public class MailTalepFormuController
    {

        [ApiController]
        [Route("api/[controller]")]
        public class RandomController : ControllerBase
        {
            private static readonly Random _rnd = new Random(); // Tek bir Random nesnesi

            [HttpGet("number")]
            public IActionResult GetRandomNumber()
            {
                int number = _rnd.Next(); // _rnd.Next(1, 101) ---> 1-100 arasında rastgele sayı
                String result = number.ToString();
                return Ok(new { RandomNumber = number });
            }

            
        }
    }
}
