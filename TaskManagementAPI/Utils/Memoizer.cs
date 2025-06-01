using System.Collections.Concurrent;

namespace TaskManagementAPI.Utils
{
    public static class Memoizer
    {
        private static readonly ConcurrentDictionary<string, object> _cache = new();

        public static TResult Memoize<TParam, TResult>(TParam param, Func<TParam, TResult> func)
        {
            var key = $"{typeof(TParam).FullName}:{param?.GetHashCode()}";

            if (_cache.TryGetValue(key, out var result))
            {
                return (TResult)result;
            }

            var computed = func(param);
            _cache[key] = computed;
            return computed;
        }

        public static void Clear() => _cache.Clear();
    }
}