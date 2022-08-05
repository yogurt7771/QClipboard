using Microsoft.AspNetCore.Mvc;

namespace q_clipboard_server.ViewModels;


public class HeaderParameters
{
    [FromHeader]
    public string Token { get; set; } = "";
    [FromQuery]
    public bool Base64Image { get; set; } = true;
}

public class UploadParameters
{
    public string Type { get; set; } = "";
    public IFormFile? File { get; set; } = null;
}

public class ClipboardResponse
{
    public string Type { get; set; } = "";
    public string Content { get; set; } = "";
    public string Url { get; set; } = "";
}

