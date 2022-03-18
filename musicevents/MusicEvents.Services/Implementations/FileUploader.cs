using Microsoft.Extensions.Logging;
using MusicEvents.Entities;
using MusicEvents.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace MusicEvents.Services.Implementations;

public class FileUploader : IFileUploader
{
    private readonly IOptions<AppSettings> _options;
    private readonly ILogger<FileUploader> _logger;

    public FileUploader(IOptions<AppSettings> options, ILogger<FileUploader> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(string base64String, string filePath)
    {
        try
        {
            if (base64String == null) return string.Empty;

            var bytes = Convert.FromBase64String(base64String);

            // D:\Servidor\MusicEventsPictures\concierto01.jpg
            var path = Path.Combine(_options.Value.StorageConfiguration.Path, filePath);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }

            // http://localhost/pictures/concierto01.jpg

            return $"{_options.Value.StorageConfiguration.PublicUrl}{filePath}"; 

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return string.Empty;
        }
    }
}