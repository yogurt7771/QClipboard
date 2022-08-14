using q_clipboard_server.Services;
using q_clipboard_server.Models;
using q_clipboard_server.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace q_clipboard_server.Controllers;



[ApiController]
[Route("[controller]")]
public class ClipboardController : ControllerBase
{
    [HttpGet("{user}")]
    public ActionResult<ClipboardResponse> Get(string user, [FromHeader] HeaderParameters headers)
    {
        if (!AuthService.IsAuthenticated(headers.Token))
        {
            return Unauthorized();
        }
        var clipboard = ClipboardService.GetClipboard(user);
        var url = $"/{user}/download?token={headers.Token}";
        if (clipboard.Type == "text")
        {
            return new ClipboardResponse {
                Type = clipboard.Type,
                Content = clipboard.Content,
                Url = url
            };
        }
        else if (clipboard.Type == "image")
        {
            if (headers.Base64Image) {
                return new ClipboardResponse {
                    Type = clipboard.Type,
                    Content = clipboard.Content,
                    Url = url
                };
            }
            else
            {
                return new ClipboardResponse {
                    Type = clipboard.Type,
                    Content = "",
                    Url = url
                };
            }
        }
        else if (clipboard.Type == "file")
        {
            return new ClipboardResponse {
                Type = clipboard.Type,
                Content = Path.GetFileName(clipboard.Content),
                Url = url
            };
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet("{user}/download")]
    public ActionResult<IFormFile> Download(string user, string token)
    {
        if (!AuthService.IsAuthenticated(token))
        {
            return Unauthorized();
        }

        var clipboard = ClipboardService.GetClipboard(user);
        if (clipboard.Type == "text") {
            return new FileStreamResult(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(clipboard.Content)), "text/plain");
        }
        else if (clipboard.Type == "image") {
            var data = Convert.FromBase64String(clipboard.Content);
            return new FileStreamResult(new MemoryStream(data), "image/png");
        }
        else if (clipboard.Type == "file") {
            if (!System.IO.File.Exists(clipboard.Content)) {
                return NotFound();
            }
            var result = new PhysicalFileResult(clipboard.Content, "application/octet-stream");
            
            Response.Headers.Append("Content-Disposition", $"attachment; filename={System.Web.HttpUtility.UrlEncode(System.IO.Path.GetFileName(clipboard.Content))}");
            return result;
        }
        else {
            return BadRequest();
        }
    }

    [HttpPost("{user}/upload")]
    public IActionResult Upload(string user, [FromHeader] HeaderParameters headers, [FromForm] UploadParameters upload)
    {
        if (!AuthService.IsAuthenticated(headers.Token))
        {
            return Unauthorized();
        }
        if (upload.File == null)
        {
            return BadRequest();
        }
        else if (upload.Type == "image")
        {
            var stream = new BinaryReader(upload.File.OpenReadStream());
            var data = stream.ReadBytes((int)upload.File.Length);
            var bitmap = SkiaSharp.SKBitmap.Decode(data);
            var encodeData = bitmap.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100).ToArray();
            var content = Convert.ToBase64String(encodeData);
            var clipboard = new ClipboardModel { Type="image", Content = content };
            ClipboardService.SetClipboard(user, clipboard);
            return Ok();
        }
        else if (upload.Type == "file")
        {
            var tempDir = Path.GetTempPath();
            if (tempDir == null)
            {
                return BadRequest();
            }
            Directory.CreateDirectory(tempDir);
            var tempFile = Path.Join(tempDir, upload.File.FileName);
            var stream = new BinaryReader(upload.File.OpenReadStream());
            var data = stream.ReadBytes((int)upload.File.Length);
            System.IO.File.WriteAllBytes(tempFile, data);
            ClipboardService.SetClipboard(user, new ClipboardModel { Type="file", Content = tempFile });
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost("{user}")]
    public IActionResult Post(string user, [FromHeader] HeaderParameters headers, [FromBody] ClipboardModel clipboard)
    {
        if (!AuthService.IsAuthenticated(headers.Token))
        {
            return Unauthorized();
        }
        ClipboardService.SetClipboard(user, clipboard);
        return Ok();
    }
}
