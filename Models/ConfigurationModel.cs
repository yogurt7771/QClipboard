namespace q_clipboard_server.Models;


public record ConfigurationModel
{
    public int Port { get; set; } = 5000;
    public string Token { get; set; } = "qclipboard";
}
