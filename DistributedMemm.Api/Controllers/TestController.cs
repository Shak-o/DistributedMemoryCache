using DistributedMemm.Lib.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedMemm.Api.Controllers;

[ApiController]
public class TestController : ControllerBase
{
    private readonly IDistributedMemm _distributedMemm;

    public TestController(IDistributedMemm distributedMemm)
    {
        _distributedMemm = distributedMemm;
    }

    [HttpPost("Add")]
    public void Add(string key, string value)
    {
        _distributedMemm.Add(key, value);
    }

    [HttpGet("GetCache")]
    public string Get(string key)
    {
        return _distributedMemm.GetString(key);
    }
}