using System.Reflection;

namespace HotelSearch.UnitTests;

public static class UnitTestReflectionExtensions
{
    public static void SetPrivateProperty<T>(this T obj, string propertyName, object value)
    {
        var prop = typeof(T).GetProperty(propertyName,
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        
        if (prop == null)
            throw new ArgumentException($"Property '{propertyName}' not found on {typeof(T)}");

        prop.SetValue(obj, value);
    }

}