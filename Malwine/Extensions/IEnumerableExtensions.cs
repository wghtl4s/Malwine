using System;
using System.Collections.Generic;
using System.Linq;

namespace Malwine.Extensions;

static class IEnumerableExtensions
{
  public static bool All<T1, T2>(this IEnumerable<(T1, T2)> source, Func<T1, T2, bool> predicate)
  {
    return source.All(s => predicate(s.Item1, s.Item2));
  }
}