using System.Collections.Generic;
using System.Linq;

namespace Doppler.BigQueryMicroservice.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
