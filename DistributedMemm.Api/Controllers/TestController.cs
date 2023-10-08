using DistributedMemm.Lib.Infrastructure.Models;
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
    public void Add(string key, string value, PriorityLevel priority)
    {
        _distributedMemm.Add(key, value, priority);
    }

    [HttpGet("GetCache")]
    public string Get(string key)
    {
        return _distributedMemm.GetString(key);
    }
}