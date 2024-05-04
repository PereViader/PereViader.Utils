using System;
using PereViader.Utils.Common.Optionals;

namespace PereViader.Utils.Common.Results
{
    public static class ResultExtensions
    {
        public static TResult? GetNullableResult<TResult, TError>(this Result<TResult, TError> result) where TResult : class 
            => result.TryGetResult(out var resultValue) ? resultValue : null;
        
        public static TResult? GetNullableResultValue<TResult, TError>(this Result<TResult, TError> result) where TResult : struct 
            => result.TryGetResult(out var resultValue) ? resultValue : null;
        
        public static TError? GetNullableError<TResult, TError>(this Result<TResult, TError> result) where TError : class 
            => result.TryGetError(out var errorValue) ? errorValue : null;
        
        public static TError? GetNullableErrorValue<TResult, TError>(this Result<TResult, TError> result) where TError : struct 
            => result.TryGetError(out var errorValue) ? errorValue : null;
        
        public static TResult GetResultOrDefault<TResult, TError>(this Result<TResult, TError> result, TResult defaultResult) 
            => result.TryGetResult(out var resultValue) ? resultValue : defaultResult;
 
        public static TResult GetResultOrDefault<TResult, TError>(this Result<TResult, TError> result, Func<TResult> defaultResultFunc) 
            => result.TryGetResult(out var resultValue) ? resultValue : defaultResultFunc();

        public static TResult GetResultOrThrow<TResult, TError>(this Result<TResult, TError> result,
            Func<TError, Exception> exceptionProvider)
        {
            var (isSuccess, success, error) = result;
            return isSuccess 
                ? success 
                : throw exceptionProvider(error);
        }
        
        public static TResult GetResultOrThrow<TResult, TError>(this Result<TResult, TError> result)
        {
            var (isSuccess, success, error) = result;
            return isSuccess 
                ? success 
                : throw new InvalidOperationException($"Could not extract result from [{nameof(Result<TResult, TError>)}] with error [{error}]");
        }
        
        public static TError GetErrorOrThrow<TResult, TError>(this Result<TResult, TError> result,
            Func<TResult, Exception> exceptionProvider)
        {
            var (isSuccess, success, error) = result;
            return isSuccess 
                ? throw exceptionProvider(success) 
                : error;
        }
        
        public static TError GetErrorOrThrow<TResult, TError>(this Result<TResult, TError> result)
        {
            var (isSuccess, success, error) = result;
            return isSuccess 
                ? throw new InvalidOperationException($"Could not extract error from [{nameof(Result<TResult, TError>)}] with success [{success}]") 
                : error;
        }
        
        public static void Match<TResult, TError>(this Result<TResult, TError> result, Action<TResult> onSuccess)
        {
            if (result.TryGetResult(out var resultValue))
            {
                onSuccess(resultValue);
            }
        }

        public static void Match<TResult, TError>(this Result<TResult, TError> result, Action<TResult> onSuccess, Action<TError> onFailure)
        {
            var (isSuccess, success, error) = result;
            if (isSuccess)
            {
                onSuccess(success);
            }
            else
            {
                onFailure(error);
            }
        }
        
        public static void Match<TResult, TError>(this Result<TResult, TError> result, Action<TError> onFailure)
        {
            if (result.TryGetError(out var error))
            {
                onFailure(error);
            }
        }

        public static Result<TResultOut, TError> MapSuccess<TResult, TError, TResultOut>(this Result<TResult, TError> result,
            Func<TResult, TResultOut> mapSuccessFunc)
        {
            var (isSuccess, success, error) = result;
            return isSuccess
                ? Result<TResultOut, TError>.Success(mapSuccessFunc(success))
                : Result<TResultOut, TError>.Failure(error);
        }

        public static Result<TResultOut, TError> MapSuccess<TResult, TError, TResultOut>(this Result<TResult, TError> result,
            Func<TResult, Result<TResultOut, TError>> mapSuccessFunc)
        {
            var (isSuccess, success, error) = result;
            return isSuccess
                ? mapSuccessFunc(success)
                : Result<TResultOut, TError>.Failure(error);
        }
        
        public static Result<TResultOut, TErrorOut> MapSuccess<TResult, TError, TResultOut, TErrorOut>(this Result<TResult, TError> result,
            Func<TResult, Result<TResultOut, TErrorOut>> mapSuccessFunc, Func<TError, TErrorOut> mapErrorFunc)
        {
            var (isSuccess, success, error) = result;
            return isSuccess
                ? mapSuccessFunc(success)
                : Result<TResultOut, TErrorOut>.Failure(mapErrorFunc(error));
        }

        public static Optional<TResult> ToOptionalSuccess<TResult, TError>(this Result<TResult, TError> result)
        {
            var isSuccess = result.TryGetResult(out var success);
            return Optional<TResult>.Maybe(success, isSuccess);
        }
        
        public static Optional<TError> ToOptionalError<TResult, TError>(this Result<TResult, TError> result)
        {
            var isError = result.TryGetError(out var error);
            return Optional<TError>.Maybe(error, isError);
        }
    }
}