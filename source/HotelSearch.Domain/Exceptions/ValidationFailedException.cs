using FluentValidation.Results;

namespace HotelSearch.Domain.Exceptions;

public class ValidationFailedException: Exception
{
    /// <summary>
    /// Dictionary that contains list of validation errors for each key.
    /// </summary>
    public readonly Dictionary<string, List<string>> ValidationErrors;

    public ValidationFailedException(string message, ValidationResult validationResult):base(message)
    {
        ValidationErrors = validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToList());
    }
}