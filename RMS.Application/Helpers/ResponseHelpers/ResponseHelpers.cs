namespace RMS.Application.Helpers.ResponseHelpers
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }

        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public string? Token { get; set; }
    }



    public class ResponseDto
    {
        public bool Success { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;
    }

    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
