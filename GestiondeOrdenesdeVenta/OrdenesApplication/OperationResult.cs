using System;

namespace OrdenesApplication
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static OperationResult Ok(string message = "") => new OperationResult { Success = true, Message = message };
        public static OperationResult Fail(string message) => new OperationResult { Success = false, Message = message };
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }

        public static OperationResult<T> Ok(T data, string message = "") => new OperationResult<T> { Success = true, Data = data, Message = message };
        public static new OperationResult<T> Fail(string message) => new OperationResult<T> { Success = false, Message = message };
    }
}
