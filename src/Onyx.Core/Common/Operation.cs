namespace Onyx.Core.Common
{
    public class Operation
    {
        public bool IsOperationSuccess { get; set; } = true;

        public string? ErrorMessage { get; set; }

        public OperationErrorType? ErrorType { get; set; }

        public Operation Build(string? message = null, bool isOperationSuccess = true, OperationErrorType? errorType = null)
        {
            IsOperationSuccess = isOperationSuccess;
            ErrorMessage = message;
            ErrorType = errorType ?? (!isOperationSuccess ? OperationErrorType.Technical : null);
            return this;
        }
    }
}