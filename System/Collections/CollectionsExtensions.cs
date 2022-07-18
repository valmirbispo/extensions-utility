namespace ExtensionsUtility.System.Collections
{
    public static class CollectionsExtensions
    {
        [Obsolete("Will be replaced by IsNullOrEmpty()")]
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
