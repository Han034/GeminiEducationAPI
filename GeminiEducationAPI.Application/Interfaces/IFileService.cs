using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Interfaces
{
	public interface IFileService
	{
		Task<string> UploadFileAsync(IFormFile file, string path); // Dosya yükleme metodu
		Task<List<string>> UploadFilesAsync(IFormFileCollection files, string path); // Çoklu dosya yükleme metodu
		Task<byte[]> DownloadFileAsync(string filePath); // Dosya indirme metodu
		string GetValidFileName(string fileName); // SEO uyumlu dosya ismi oluşturma metodu
	}
}
