using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Scellecs.Collections;

namespace Rollin.LeoEcs.Extensions
{
    public static class CollectionsExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FastList<T> ToFastList<T>(this ICollection<T> collection)
        {
            var result = new FastList<T>(collection.Count);

            foreach (T value in collection)
            {
                result.Add(value);
            }

            return result;
        }
    }
}