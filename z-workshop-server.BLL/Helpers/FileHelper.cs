using z_workshop_server.BLL.DTOs;

namespace z_workshop_server.BLL.Helpers;

public class FileHelper
{
    public static string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/file");

    public static ZServiceResult<string> SaveFile(IFormFile file, string dir, string fileName)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return ZServiceResult<string>.Failure("No file are saved");
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
            return ZServiceResult<string>.Success("File saved thành công:" + dir + "/" + fileName);
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }
}
