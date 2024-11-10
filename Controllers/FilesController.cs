using Azure_Blob_Storage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly BlobService _blobService;

    public FilesController(BlobService blobService)
    {
        _blobService = blobService;
    }

    // Endpoint to upload file
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        using (var stream = file.OpenReadStream())
        {
            await _blobService.UploadFileAsync(file.FileName, stream);
        }

        return Ok("File uploaded successfully.");
    }

    // Endpoint to download file
    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        var fileStream = await _blobService.DownloadFileAsync(fileName);

        if (fileStream == null)
        {
            return NotFound();
        }

        return File(fileStream, "application/octet-stream", fileName);
    }
}
