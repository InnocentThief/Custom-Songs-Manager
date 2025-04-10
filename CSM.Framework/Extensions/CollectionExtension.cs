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
    }
}
