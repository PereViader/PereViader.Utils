using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Results
{
    public readonly struct Result<TResult, TError> : IEquatable<Result<TResult, TError>>
    {
        public bool IsSuccess { get; }
        private readonly TResult _result;
        private readonly TError _error;

        private Result(bool isSuccess, TResult result, TError error)
        {
            IsSuccess = isSuccess;
            _result = result;
            _error = error;
        }

        public static Result<TResult, TError> Success(TResult result) => new(true, result, default!);
        
        public static Result<TResult, TError> Failure(TError error) => new(false, default!, error);
        
        public bool TryGetResult(out TResult result)
        {
            result = _result;
            return IsSuccess;
        }

        public bool TryGetError(out TError error)
        {
            error = _error;
            return !IsSuccess;
        }
        
        public void Deconstruct(out bool isSuccess, out TResult result, out TError error)
        {
            isSuccess = IsSuccess;
            result = _result;
            error = _error;
        }

        public bool Equals(Result<TResult, TError> other) =>
            IsSuccess == other.IsSuccess
            && EqualityComparer<TResult>.Default.Equals(_result, other._result)
            && EqualityComparer<TError>.Default.Equals(_error, other._error);

        public override bool Equals(object? obj) => obj is Result<TResult, TError> other && Equals(other);
        
        public static bool operator ==(Result<TResult, TError> left, Result<TResult, TError> right) => left.Equals(right);
        
        public static bool operator !=(Result<TResult, TError> left, Result<TResult, TError> right) => !left.Equals(right);
        
        public override int GetHashCode() => HashCode.Combine(IsSuccess, _result, _error);
        
        public override string ToString() => IsSuccess ? $"success {_result}" : $"error {_error}";
    }
}