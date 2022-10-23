using System.Text.Json.Serialization;

namespace Instruction.Domain.Core
{
    public class BaseResponseDto<T>:BaseDto
    {
        public T Data { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }


        public List<String> Errors { get; set; }


        public static BaseResponseDto<T> Success(int statusCode, T data)
        {
            return new BaseResponseDto<T> { Data = data, StatusCode = statusCode };
        }
        public static BaseResponseDto<T> Success(int statusCode)
        {
            return new BaseResponseDto<T> { StatusCode = statusCode };
        }

        public static BaseResponseDto<T> Fail(int statusCode, List<string> errors)
        {
            return new BaseResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }

        public static BaseResponseDto<T> Fail(int statusCode, string error)
        {
            return new BaseResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}
