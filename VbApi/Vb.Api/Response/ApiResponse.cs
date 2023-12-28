using FluentValidation.Results;
using System.Net;

namespace Vb.Api.Response
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }
        private ApiResponse(bool succeeded, HttpStatusCode httpStatusCode, T result, IEnumerable<T> resultList, ICollection<ValidationFailure> errors)
        {
            Succeeded = succeeded;
            HttpStatusCode = httpStatusCode;
            Result = result;
            ResultList = resultList;
            Errors = errors;
        }

        public bool Succeeded { get; set; }

        public string SuccessMessage => Succeeded ? "The operation has been successfully completed." : "An error occurred while processing the operation.";

        public HttpStatusCode HttpStatusCode { get; set; }
        public int ProcessCode => (int)HttpStatusCode;
        public T Result { get; set; }

        public IEnumerable<T> ResultList { get; set; }

        public ICollection<ValidationFailure> Errors { get; set; }

        public static ApiResponse<T> Success()
        {
            return new ApiResponse<T>(true, HttpStatusCode.OK, default, null, null);
        }

        public static ApiResponse<T> Success(T result)
        {
            return new ApiResponse<T>(true, HttpStatusCode.OK, result, null, null);
        }

        public static ApiResponse<T> Success(IEnumerable<T> resultList)
        {
            return new ApiResponse<T>(true, HttpStatusCode.OK, default, resultList, null);
        }

        public static ApiResponse<T> Failure(ICollection<ValidationFailure> errors)
        {
            return new ApiResponse<T>(false, HttpStatusCode.BadRequest, default, null, errors);
        }

        public static ApiResponse<T> NotFound(string entityName, int? id)
        {
            return new ApiResponse<T>(false, HttpStatusCode.NotFound, default, null, new List<ValidationFailure>
            {
                new ValidationFailure($"{entityName}NotFound", $"{entityName} with ID {id} not found.")
            });
        }
    }
}
