namespace Helpdesk.WebApi.Helpers;

public static class CollectionExtension
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable == null || !enumerable.Any();
    }
}