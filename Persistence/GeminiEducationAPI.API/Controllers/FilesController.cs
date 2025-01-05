using GeminiEducationAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeminiEducationAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FilesController : ControllerBase
	{
		private readonly IFileService _fileService;
		private readonly string _uploadFolder;

		public FilesController(IFileService fileService)
		{
			_fileService = fileService;
			_uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads"); // Yükleme klasörü

			// Klasör yoksa oluştur
			if (!Directory.Exists(_uploadFolder))
			{
				Directory.CreateDirectory(_uploadFolder);
			}
		}

		[HttpPost("Upload")]
		public async Task<IActionResult> UploadFile(IFormFile file)
		{
			try
			{
				var fileName = await _fileService.UploadFileAsync(file, _uploadFolder);
				return Ok(new { fileName });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("UploadMultiple")]
		public async Task<IActionResult> UploadFiles(IFormFileCollection files) // List<IFormFile> yerine IFormFileCollection
		{
			try
			{
				var fileNames = await _fileService.UploadFilesAsync(files, _uploadFolder);
				return Ok(new { fileNames });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("Download/{fileName}")]
		public async Task<IActionResult> DownloadFile(string fileName)
		{
			try
			{
				var filePath = Path.Combine(_uploadFolder, fileName);
				var fileBytes = await _fileService.DownloadFileAsync(filePath);
				return File(fileBytes, "application/octet-stream", fileName); // application/octet-stream, herhangi bir dosya türünü indirmek için kullanılabilir.
			}
			catch (FileNotFoundException)
			{
				return NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
/*
 FilesController, IFileService'i constructor'ında enjekte eder.
_uploadFolder: Yüklenen dosyaların kaydedileceği klasörü belirtir. wwwroot/uploads klasörünü kullanır. Siz চাইলে farklı bir klasör belirtebilirsiniz.
UploadFile: POST /api/Files/Upload endpoint'i, tek dosya yüklemek için kullanılır. IFormFile tipinde file parametresi alır.
UploadFiles: POST /api/Files/UploadMultiple endpoint'i, çoklu dosya yüklemek için kullanılır. List<IFormFile> tipinde files parametresi alır.
DownloadFile: GET /api/Files/Download/{fileName} endpoint'i, dosya indirmek için kullanılır. fileName parametresi ile indirilecek dosyanın adını alır.
File(fileBytes, "application/octet-stream", fileName): İndirilecek dosyayı ve dosya adını belirtir.
 */
