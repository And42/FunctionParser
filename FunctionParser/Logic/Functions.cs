using System.Collections.Generic;

namespace FunctionParser.Logic
{
    public static class Functions
    {
        internal static T As<T>(this object obj) => (T) obj;

        internal static T LastElement<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }
    }
}
