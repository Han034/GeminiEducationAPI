using GeminiEducationAPI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace GeminiEducationAPI.Infrastructure.Files
{
	public class FileService : IFileService
	{
		public async Task<string> UploadFileAsync(IFormFile file, string path)
		{
			if (file == null || file.Length == 0)
			{
				throw new ArgumentException("File is empty.");
			}

			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException("Path is empty.");
			}

			// Dosya yolunun sonuna \ eklenmemiş ise ekliyoruz.
			if (!path.EndsWith("\\"))
			{
				path += "\\";
			}

			// Dosyayı asenkron olarak diske kopyalar.
			var fileName = GetValidFileName(file.FileName);
			var filePath = Path.Combine(path, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return fileName; // Dosya adını döndürüyoruz, kaydedilirken bu bilgi kullanılabilir.
		}

		public async Task<List<string>> UploadFilesAsync(IFormFileCollection files, string path)
		{
			if (files == null || files.Count == 0)
			{
				throw new ArgumentException("Files collection is empty.");
			}

			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException("Path is empty.");
			}

			// Dosya yolunun sonuna \ eklenmemiş ise ekliyoruz.
			if (!path.EndsWith("\\"))
			{
				path += "\\";
			}

			// Dosyaları asenkron olarak diske kopyalar.
			var fileNames = new List<string>();
			foreach (var file in files)
			{
				var fileName = GetValidFileName(file.FileName);
				var filePath = Path.Combine(path, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				fileNames.Add(fileName);
			}

			return fileNames; // Dosya adlarını döndürüyoruz, kaydedilirken bu bilgi kullanılabilir.
		}

		public async Task<byte[]> DownloadFileAsync(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				throw new ArgumentException("File path is empty.");
			}

			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("File not found.", filePath);
			}

			return await File.ReadAllBytesAsync(filePath);
		}

		public string GetValidFileName(string fileName)
		{
			// Dosya adındaki geçersiz karakterleri temizle
			string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			string cleanedFileName = Regex.Replace(fileName, "[" + Regex.Escape(invalidChars) + "]", "");

			// Boşlukları ve özel karakterleri - ile değiştir
			cleanedFileName = Regex.Replace(cleanedFileName, @"\s+", "-");
			cleanedFileName = Regex.Replace(cleanedFileName, "[^a-zA-Z0-9\\.\\-]", "-");

			// Dosya adını küçük harfe dönüştür
			cleanedFileName = cleanedFileName.ToLowerInvariant();

			// Uzun dosya adlarını kısalt (isteğe bağlı)
			if (cleanedFileName.Length > 255)
			{
				cleanedFileName = cleanedFileName.Substring(0, 255);
			}

			return cleanedFileName;
		}
	}
}
/*
 UploadFileAsync: Tek bir dosyayı path ile belirtilen dizine yükler. Yüklenen dosyanın adını döndürür.
UploadFilesAsync: Birden fazla dosyayı path ile belirtilen dizine yükler. Yüklenen dosyaların adlarını List<string> olarak döndürür.
DownloadFileAsync: filePath ile belirtilen dosyanın içeriğini byte dizisi olarak indirir.
GetValidFileName: Verilen dosya adını SEO uyumlu hale getirir.
Geçersiz dosya adı karakterlerini kaldırır.
Boşlukları ve özel karakterleri tire (-) ile değiştirir.
Dosya adını küçük harfe dönüştürür.
İsteğe bağlı olarak, uzun dosya adlarını kısaltır.
 */
