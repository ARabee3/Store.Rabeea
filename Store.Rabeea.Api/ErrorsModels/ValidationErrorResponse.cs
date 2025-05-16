namespace Store.Rabeea.Api.ErrorsModels;

public class ValidationErrorResponse
{
    public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
    public string ErrorMessage { get; set; } = "Validation Error";
    public IEnumerable<ValidationError> Errors { get; set;  }
}
