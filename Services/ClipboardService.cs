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
        var containsUser = userClipboards.TryGetValue(user, out var oldClipboard);
        if (containsUser && oldClipboard != null)
        {
            if (oldClipboard.Type == "file") {
                if (!string.IsNullOrEmpty(oldClipboard.Content)) {
                    if (System.IO.File.Exists(oldClipboard.Content)) {
                        System.IO.File.Delete(oldClipboard.Content);
                    }
                    var tempDir = Path.GetDirectoryName(oldClipboard.Content);
                    if (!string.IsNullOrEmpty(tempDir) && System.IO.Directory.Exists(tempDir)) {
                        System.IO.Directory.Delete(tempDir);
                    }
                }
            }
        }

        userClipboards[user] = clipboard;
    }
}
