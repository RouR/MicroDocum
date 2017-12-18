using System;
using System.Collections.Generic;

namespace MicroDocum.Graphviz
{
    public static class Utils
    {
        public static string NormalizeGraphvizNames(this string id)
        {
            var ret = id //forbidden chars in Graphviz names
                    .Replace(".", "_")
                    .Replace("+", "__")
                    .Replace("-", "")
                ;
            if (char.IsDigit(ret[0]))
            {
                ret = "_" + ret;
            }
            return ret;
        }

        public static TV GetValueOrDefault<TK, TV>(this IDictionary<TK, TV> @this, TK key, Func<TV> @default)
        {
            return @this.ContainsKey(key) ? @this[key] : @default();
        }

        public static void SetValueIfExists<TK, TV>(this IDictionary<TK, TV> @this, TK key, TV value)
        {
            if (@this.ContainsKey(key))
            {
                @this[key] = value;
            }
        }

        public static void SetValueIfNotExists<TK, TV>(this IDictionary<TK, TV> @this, TK key, TV value)
        {
            if (!@this.ContainsKey(key))
            {
                @this[key] = value;
            }
        }

        public static void SetValue<TK, TV>(this IDictionary<TK, TV> @this, TK key, TV value)
        {
            if (@this.ContainsKey(key))
            {
                @this[key] = value;
            }
            else @this.Add(key, value);
        }
    }
}