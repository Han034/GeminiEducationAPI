namespace GeminiEducationAPI.API.Models
{
	public class ErrorResponse
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public string? Details { get; set; } // ? nullable olduğunu belirtir
	}
}
/*
 StatusCode: HTTP status kodunu temsil eder (Örn: 400, 404, 500).
Message: Hata mesajını içerir.
Details: Hata hakkında ek bilgiler içerebilir (Örn: Stack Trace).
 */