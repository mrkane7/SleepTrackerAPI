

public interface ILoginUseCase
{
    Task<Result<string>> LoginUser(LoginDTO requestedUser);
}
public class LoginUseCase : ILoginUseCase
{
    private readonly IAccountService _accountService;
    private readonly TokenProvider _tokenProvider;

    public LoginUseCase(IAccountService accountService, TokenProvider tokenProvider)
    {
        _accountService = accountService;
        _tokenProvider = tokenProvider;
    }
    public async Task<Result<string>> LoginUser(LoginDTO requestedUser)
    {
        var user = await _accountService.GetAccountByEmail(requestedUser.Email);
        if (user == null)
        {
            return Result<string>.Fail(ResultReason.NotFound, "User not found");
        }

        if (user.Password != requestedUser.Password)
        {
            return Result<string>.Fail(ResultReason.Failure, "Invalid Password");
        }

        string token = _tokenProvider.Create(user);

        return Result<string>.Success(token);
    }
}