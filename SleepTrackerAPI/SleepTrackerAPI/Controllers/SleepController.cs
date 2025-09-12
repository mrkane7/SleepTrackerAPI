
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SleepController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<SleepController> _logger;
    private readonly ISleepUseCase _sleepUseCase;

    public SleepController(IMapper mapper, ILogger<SleepController> logger, ISleepUseCase sleepUseCase)
    {
        _logger = logger;
        _mapper = mapper;
        _sleepUseCase = sleepUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSleep()
    {
        var sleepResult = await _sleepUseCase.GetAllSleep();
        if (!sleepResult.Successful)
        {
            var errorResponse = new Response(sleepResult);
            return BadRequest(errorResponse);
        }

        var sleepData = sleepResult.GetResult(_mapper.Map<IEnumerable<SleepDTO>>);
        var response = new DataResponse<IEnumerable<SleepDTO>>(sleepData, sleepResult);
        return Ok(response);
    }
    
    [HttpGet("byUser/{userId}")]
    public async Task<IActionResult> GetUsersSleep([FromRoute] int userId)
    {
        var sleepResult = await _sleepUseCase.GetUsersSleep(userId);
        if (!sleepResult.Successful)
        {
            var errorResponse = new Response(sleepResult);
            return BadRequest(errorResponse);
        }

        var sleepData = sleepResult.GetResult(_mapper.Map<IEnumerable<SleepDTO>>);
        var response = new DataResponse<IEnumerable<SleepDTO>>(sleepData, sleepResult);
        return Ok(response);
    }

    [HttpGet("byUser/stats/{userId}")]
    public async Task<IActionResult> GetUserSleepStats([FromRoute] int userId)
    {
        var sleepStatsResult = await _sleepUseCase.GetUserSleepStats(userId);
        if (!sleepStatsResult.Successful)
        {
            var errorResponse = new Response(sleepStatsResult);
            return BadRequest(errorResponse);
        }

        var sleepStats = sleepStatsResult.GetResult(_mapper.Map<SleepStatsDTO>);
        var response = new DataResponse<SleepStatsDTO>(sleepStats, sleepStatsResult);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddSleep([FromBody] SleepDTO sleep)
    {
        var sleepResult = await _sleepUseCase.AddSleep(sleep);
        if (!sleepResult.Successful)
        {
            var errorResponse = new Response(sleepResult);
            return BadRequest(errorResponse);
        }
        var sleepData = sleepResult.GetResult(_mapper.Map<SleepDTO>);
        var response = new DataResponse<SleepDTO>(sleepData, sleepResult);
        return Ok(sleepResult);

    }
    [HttpPut]
    public async Task<IActionResult> EditSleep([FromBody] SleepDTO sleep)
    {
        var sleepResult = await _sleepUseCase.EditSleep(sleep);
        if (!sleepResult.Successful)
        {
            var errorResponse = new Response(sleepResult);
            return BadRequest(errorResponse);
        }
        return Ok(sleepResult);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSleep( [FromRoute] int id)
    {
        var sleepResult = await _sleepUseCase.DeleteSleep(id);
        if (!sleepResult.Successful)
        {
            var errorResponse = new Response(sleepResult);
            return BadRequest(errorResponse);
        }
        return Ok(sleepResult);
    }
}

