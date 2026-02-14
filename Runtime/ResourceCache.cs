using System.Collections.Generic;
using UnityEngine;

namespace Tesseract.Cache
{
    /// <summary>
    /// Generic LRU cache for Resources.Load with configurable max size.
    /// </summary>
    public class ResourceCache<T> where T : Object
    {
        private readonly int _maxSize;
        private readonly Dictionary<string, T> _cache;
        private readonly Queue<string> _evictionQueue;

        public ResourceCache(int maxSize = 20)
        {
            _maxSize = maxSize;
            _cache = new Dictionary<string, T>();
            _evictionQueue = new Queue<string>();
        }

        /// <summary>
        /// Load a resource from cache or Resources folder.
        /// </summary>
        public T Load(string path)
        {
            if (_cache.TryGetValue(path, out T result))
                return result;

            result = Resources.Load<T>(path);
            if (result == null)
            {
                Debug.LogWarning($"[ResourceCache] Failed to load: {path}");
                return null;
            }

            _cache.Add(path, result);
            _evictionQueue.Enqueue(path);
            Evict();
            return result;
        }

        /// <summary>
        /// Check if a resource is cached.
        /// </summary>
        public bool Contains(string path) => _cache.ContainsKey(path);

        /// <summary>
        /// Clear all cached resources.
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
            _evictionQueue.Clear();
        }

        /// <summary>
        /// Current number of cached items.
        /// </summary>
        public int Count => _cache.Count;

        private void Evict()
        {
            while (_evictionQueue.Count > _maxSize)
            {
                string oldestKey = _evictionQueue.Dequeue();
                _cache.Remove(oldestKey);
            }
        }
    }
}
