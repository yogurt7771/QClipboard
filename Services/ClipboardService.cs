using q_clipboard_server.Models;

namespace q_clipboard_server.Services;


public static class ClipboardService
{
    private static Dictionary<string, ClipboardModel> userClipboards { get; }

    static ClipboardService()
    {
        userClipboards = new Dictionary<string, ClipboardModel>();
    }

    public static ClipboardModel GetClipboard(string user)
    {
        return userClipboards.TryGetValue(user, out var clipboard) ? clipboard : new ClipboardModel();
    } 

    public static void SetClipboard(string user, ClipboardModel clipboard)
    {
        userClipboards[user] = clipboard;
    }
}
