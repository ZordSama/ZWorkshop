using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using z_workshop_server.BLL.DTOs;

namespace z_workshop_server.BLL.Helpers;

public class FileHelper
{
    protected static readonly string _basePath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot/files"
    );

    /// <summary>
    /// Asynchronously saves a file to the specified directory with the given filename.
    /// </summary>
    /// <param name="file">The file to save.</param>
    /// <param name="dir">The directory relative to the base path.</param>
    /// <param name="fileName">The name of the file without extension.</param>
    /// <returns>A ZServiceResult indicating success or failure with the saved file path.</returns>
    public static async Task<ZServiceResult<string>> SaveFileAsync(
        IFormFile file,
        string dir,
        string fileName
    )
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return ZServiceResult<string>.Failure("No file provided to save.", 400);
            }

            var path = Path.Combine(_basePath, dir);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); // Synchronous, but acceptable for one-time setup
            }

            string fileExtension = Path.GetExtension(file.FileName);
            string fileNameWithExtension = fileName + fileExtension;
            var filePath = Path.Combine(path, fileNameWithExtension);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return ZServiceResult<string>.Success(
                $"File saved successfully: {dir}/{fileNameWithExtension}",
                Path.Combine(dir, fileNameWithExtension).Replace("\\", "/")
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure($"Failed to save file: {ex.Message}");
        }
    }

    /// <summary>
    /// Asynchronously removes a file based on its relative path within the base file directory.
    /// </summary>
    /// <param name="relativePath">The relative path of the file to remove (e.g., "images/myimage.jpg"). Should use forward slashes.</param>
    /// <returns>A ZServiceResult indicating success or failure.</returns>
    public static async Task<ZServiceResult<string>> RemoveFileAsync(string relativePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return ZServiceResult<string>.Failure("Relative path cannot be empty.", 400);
            }

            // Normalize the relative path to use the system's directory separator
            string normalizedRelativePath = relativePath
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);

            // Prevent path traversal attacks
            if (normalizedRelativePath.Contains("..") || Path.IsPathRooted(normalizedRelativePath))
            {
                return ZServiceResult<string>.Failure("Invalid relative path.", 400);
            }

            var fullPath = Path.Combine(_basePath, normalizedRelativePath);

            if (!File.Exists(fullPath))
            {
                return ZServiceResult<string>.Failure(
                    $"File not found at path: {relativePath}",
                    404
                );
            }

            // Perform file deletion asynchronously
            await Task.Run(() => File.Delete(fullPath));

            return ZServiceResult<string>.Success(
                $"File removed successfully: {relativePath}",
                relativePath
            );
        }
        catch (DirectoryNotFoundException)
        {
            return ZServiceResult<string>.Failure(
                $"Directory not found for path: {relativePath}",
                404
            );
        }
        catch (IOException ioEx)
        {
            return ZServiceResult<string>.Failure($"IO error removing file: {ioEx.Message}");
        }
        catch (UnauthorizedAccessException)
        {
            return ZServiceResult<string>.Failure("Permission denied when removing file.");
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }
}
