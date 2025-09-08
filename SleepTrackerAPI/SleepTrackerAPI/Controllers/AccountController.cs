using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    public AccountController(IMapper mapper, ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _mapper = mapper;
        _accountService = accountService;

    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAccount([FromRoute] int userId)
    {
        try
        {
            var accountServiceResult = await _accountService.GetAccount(userId);
            if (accountServiceResult == null)
            {
                return NotFound();
            }
            var accountResult = Result<UserAccountDTO>.Success(accountServiceResult);

            if (!accountResult.Successful)
            {
                var errorResponse = new Response(accountResult);
                return BadRequest(errorResponse);
            }
            var accountData = accountResult.GetResult(_mapper.Map<UserAccountDTO>);
            var response = new DataResponse<UserAccountDTO>(accountData, accountResult);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] UserAccountDTO userAccount)
    {
        try
        {
            var accountServiceResult = await _accountService.CreateAccount(userAccount);
            var accountResult = Result<UserAccountDTO>.Success(accountServiceResult);

            if (!accountResult.Successful)
            {
                var errorResponse = new Response(accountResult);
                return BadRequest(errorResponse);
            }
            var accountData = accountResult.GetResult(_mapper.Map<UserAccountDTO>);
            var response = new DataResponse<UserAccountDTO>(accountData, accountResult);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    public async Task<IActionResult> EditAccount([FromBody] UserAccountDTO userAccount)
    {
        try
        {
            var accountServiceResult = await _accountService.EditAccount(userAccount);
            var accountResult = Result<UserAccountDTO>.Success(accountServiceResult);

            if (!accountResult.Successful)
            {
                var errorResponse = new Response(accountResult);
                return BadRequest(errorResponse);
            }
            var accountData = accountResult.GetResult(_mapper.Map<UserAccountDTO>);
            var response = new DataResponse<UserAccountDTO>(accountData, accountResult);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
    
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateSleepGoal([FromRoute] int userId, [FromBody] int sleepGoal)
    {
        try
        {
            var accountServiceResult = await _accountService.UpdateSleepGoal(userId, sleepGoal);
            if (accountServiceResult == null)
            { 
                return NotFound();
            }
            var accountResult = Result<UserAccountDTO>.Success(accountServiceResult);

            if (!accountResult.Successful)
            {
                var errorResponse = new Response(accountResult);
                return BadRequest(errorResponse);
            }
            var accountData = accountResult.GetResult(_mapper.Map<UserAccountDTO>);
            var response = new DataResponse<UserAccountDTO>(accountData, accountResult);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] int userId)
    {
        try
        {
            var accountServiceResult = await _accountService.DeleteAccount(userId);
            var accountResult = accountServiceResult ? Result.Success() : Result.Fail(ResultReason.NotFound, "Account not found.");

            if (!accountResult.Successful)
            {
                var errorResponse = new Response(accountResult);
                return BadRequest(errorResponse);
            }
            return Ok(accountResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}