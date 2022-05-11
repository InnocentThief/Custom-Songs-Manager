using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Framework.Extensions
{
    public static class CollectionExtension
    {
        /// <summary>
        /// Adds multiple elements to a collection of the same type.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the collection.</typeparam>
        /// <param name="collection">The current collection elements will be added to.</param>
        /// <param name="elements">The elements to be added to the current collection.</param>
        /// <returns>A referrence to the current collection.</returns>
        public static ICollection<TSource> AddRange<TSource>(this ICollection<TSource> collection, IEnumerable<TSource> elements)
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
