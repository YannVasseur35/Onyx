namespace Onyx.Core.Common
{
    public class OperationResult<T> : Operation
    {
        public T? Model { get; set; }

        public OperationResult<T> Build(
            T model,
            string? message = null,
            bool isOperationSuccess = true,
            OperationErrorType? errorType = null)
        {
            Model = model;
            base.Build(message, isOperationSuccess, errorType);
            return this;
        }
    }
}