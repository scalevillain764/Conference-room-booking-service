using _interfaces;
using System.Data.SqlTypes;
namespace _result
{
    public class Result<T> where T: class
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMSG { get; set; }
        public Result(bool IsSuccess, T? Data, string? ErrorMSG)
        {
            this.IsSuccess = IsSuccess;
            this.Data = Data;
            this.ErrorMSG = ErrorMSG;
        }
        public static Result<T> Success(T data) => new Result<T>(true, data, null);
        public static Result<T> Error(string msg) => new Result<T>(false, null, msg);
    }
}