using System.Threading.Tasks;
using z_workshop_server.BLL.DTOs;

namespace z_workshop_server.BLL.Helpers;

public class FileHelper
{
    protected static readonly string _basePath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot/files"
    );

    public static async Task<ZServiceResult<string>> SaveFile(
        IFormFile file,
        string dir,
        string fileName
    )
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return ZServiceResult<string>.Failure("No file are saved", 400);
            }
            var path = Path.Combine(_basePath, dir);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileExtension = Path.GetExtension(file.FileName);
            string fileNameWithExtension = fileName + fileExtension;

            var filePath = Path.Combine(path, fileNameWithExtension);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return ZServiceResult<string>.Success(
                "File saved thành công:" + dir + "/" + fileNameWithExtension,
                Path.Combine(dir, fileNameWithExtension).Replace("\\", "/")
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }
}
