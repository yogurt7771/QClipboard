namespace q_clipboard_server.Models;


public record ClipboardModel
{
    public string Type { get; set; } = "text";
    public string Content { get; set; } = "";
}
