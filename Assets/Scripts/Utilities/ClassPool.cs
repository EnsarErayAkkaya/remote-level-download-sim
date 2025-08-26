using System.Collections.Generic;

namespace EEA.Utilities
{
    public static class ClassPool<T>
        where T : class
    {
        private static List<T> cache = new List<T>();

        // This will either return a pooled class instance, or null
        // You can also specify a match for the exact class instance you're looking for
        // You can also specify an action to run on the class instance (e.g. if you need to reset it)
        // NOTE: Because it can return null, you should use it like this: ClassPool<Whatever>.Spawn(...) ?? new Whatever(...)
        public static T Spawn()
        {
            // Get the matched index, or the last index
            var index = cache.Count - 1;
            // Was one found?
            if (index >= 0)
            {
                // Get instance and remove it from cache
                var instance = cache[index];

                cache.RemoveAt(index);

                return instance;
            }

            // Return null?
            return null;
        }

        // This allows you to desapwn a class instance
        // You can also specify an action to run on the class instance (e.g. if you need to reset it)
        public static void Despawn(T instance)
        {
            // Does it exist?
            if (instance != null)
            {
                // Add to cache
                if (!cache.Contains(instance))
                    cache.Add(instance);
            }
        }

        public static void Clear()
        {
            cache.Clear();
        }
    }
}