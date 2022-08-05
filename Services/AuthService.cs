namespace q_clipboard_server.Services;


public static class AuthService
{
    private static string Token { get; }

    static AuthService()
    {
        Token = ConfigurationService.Configuration.Token;
    }

    public static bool IsAuthenticated(string token) => token == Token;
}
