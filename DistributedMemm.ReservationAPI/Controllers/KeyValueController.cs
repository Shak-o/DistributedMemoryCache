using DistributedMemm.ReservationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DistributedMemm.ReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeyValueController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public KeyValueController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] KeyValuePair<string, string> keyValue)
        {
            if (keyValue.Key == null || keyValue.Value == null)
            {
                return BadRequest();
            }

            await _cacheService.SaveKeyValueAsync(keyValue.Key, keyValue.Value);
            return Ok();
        }
    }

}