using z_workshop_server.DTOs;

namespace z_workshop_server.Helpers;

public class FileService
{
    public static string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/file");

    public static ZActionResult SaveFile(IFormFile file, string dir, string fileName)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return new ZActionResult(false, "No file are saved");
            }
            var path = Path.Combine(_basePath, dir);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filePath = Path.Combine(path, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return new ZActionResult(true, "File saved successfully:" + dir + "/" + fileName);
        }
        catch (Exception ex)
        {
            return new ZActionResult(false, ex.Message);
        }
    }
}
