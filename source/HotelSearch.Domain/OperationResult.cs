namespace HotelSearch.Domain;

/// <summary>
/// Used to notify about result of an action.
/// </summary>
/// <typeparam name="T"></typeparam>
public class OperationResult<T>
{
    public string? Error { get; init; }
    public bool IsSuccess { get; init; }
    
    /// <summary>
    /// Contains entity identity value in a case of creation.
    /// </summary>
    public T? Value { get; init; }


    public static OperationResult<T> Failure(string error)
        => new() { IsSuccess = false, Error = error };
    
    public static OperationResult<T> Success(T value = default)
        => new() { IsSuccess = true, Value = value };

}
