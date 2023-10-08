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
        public async Task<IActionResult> Post([FromBody] KeyValuePair<string, object> keyValue)
        {
            if (keyValue.Key == null || keyValue.Value == null)
            {
                return BadRequest();
            }

            await _cacheService.SaveToCacheKeyValueAsync(keyValue.Key, keyValue.Value);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 100)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Invalid page or pageSize value");
            }

            var keyValues = await _cacheService.GetKeyValuesAsync(page, pageSize);
            return Ok(keyValues);
        }

    }

}