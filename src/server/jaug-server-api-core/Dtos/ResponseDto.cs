
namespace jaug_server_api_core.Dtos
{
    public class ResponseDto<T>
    {
        public ResponseDto()
        {
        }
        public ResponseDto(T dtoData)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = dtoData;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}
