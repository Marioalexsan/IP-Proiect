using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting;
public static class TestUtility
{
    /// <summary>
    /// A helper extension similar to Kotlin's let.
    /// </summary>
    public static void Let<T>(this T x, Action<T> action) => action(x);
}
