using System;
using System.Collections.Generic;
using System.Linq;

namespace CarMD.Fleet.Core.Utility.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// To the string with separator.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>System.String.</returns>
        public static string ToStringWithSeparator(this IList<string> list, string separator = ",")
        {
            return list.Count == 0 ? string.Empty : string.Join(separator, list.ToArray());
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                                     Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}