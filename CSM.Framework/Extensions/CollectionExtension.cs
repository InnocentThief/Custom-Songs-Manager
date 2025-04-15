namespace CSM.Framework.Extensions
{
    internal static class CollectionExtension
    {
        public static ICollection<TSource>? AddRange<TSource>(this ICollection<TSource>? collection, IEnumerable<TSource> elements)
        {
            if (collection == null || elements == null)
            {
                return collection;
            }
            foreach (var t in elements)
            {
                collection.Add(t);
            }
            return collection;
        }

        public static void ForEach<TSource>(this IEnumerable<TSource>? collection, Action<TSource> action)
        {
            if (collection == null || action == null)
            {
                return;
            }
            foreach (var t in collection)
            {
                action(t);
            }
        }
    }
}
