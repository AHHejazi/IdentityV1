// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableQueryableExtensions.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Extensions
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    #endregion

    /// <summary>
    ///     The enumerable queryable extensions.
    /// </summary>
    public static class EnumerableQueryableExtensions
    {
        /// <summary>
        /// Gets the paged.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="pageNum">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        /// <returns>
        /// The <see cref="PagedList{T}"/>.
        /// </returns>
        public static PagedList<T> AsPagedList<T>(this IEnumerable<T> query, int pageNum, int pageSize)
        {
            return new PagedList<T>(query, pageNum, pageSize);
        }

        /// <summary>
        /// The contains all items.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ContainsAllItems<T>(this List<T> a, List<T> b)
        {
            return !b.Except(a).Any();
        }

        /// <summary>
        /// Partitions the IEnumerable to sets of specified size.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
        {
            T[] array = null;
            var count = 0;
            foreach (var item in source)
            {
                if (array == null)
                {
                    array = new T[size];
                }

                array[count] = item;
                count++;
                if (count != size)
                {
                    continue;
                }

                yield return new ReadOnlyCollection<T>(array);
                array = null;
                count = 0;
            }

            if (array == null)
            {
                yield break;
            }

            Array.Resize(ref array, count);
            yield return new ReadOnlyCollection<T>(array);
        }

        /// <summary>
        /// The to dictionary.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <typeparam name="TK">
        /// </typeparam>
        /// <typeparam name="TV">
        /// </typeparam>
        public static IDictionary<TK, TV> ToDictionary<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> list)
        {
            return list.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static string ToCommaSeparatedString(this List<string> items, string separator = " ,")
        {
            return string.Join(separator, items);
        }
    }
}