public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public string AllowedHosts { get; set; }
    public Logging Logging { get; set; }
    public JwtSettings Jwt { get; set; }
    public string API_KEY { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}

public class Logging
{
    public LogLevel LogLevel { get; set; }
}

public class LogLevel
{
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
}

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationInMinutes { get; set; }
}
