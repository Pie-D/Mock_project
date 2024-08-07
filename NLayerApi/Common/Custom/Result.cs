using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Custom
{
    public class Result
    {
        private Result(bool isSuccess, GeneralError generalError)
        {
            if (isSuccess && generalError != GeneralError.None ||
                !isSuccess && generalError == GeneralError.None)
            {
                throw new ArgumentException("Invalid error", nameof(generalError));
            }

            IsSuccess = isSuccess;
            GeneralError = generalError;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public GeneralError GeneralError { get; }

        public static Result Success() => new(true, GeneralError.None);

        public static Result Failure(GeneralError generalError) => new(false, generalError);


    }
}
