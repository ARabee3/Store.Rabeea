namespace Store.Rabeea.Api.ErrorsModels;

public class ValidationError
{
    public string Field { get; set;}
    public IEnumerable<string> Errors { get; set; }

}