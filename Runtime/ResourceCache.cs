using System.Collections.Generic;
using UnityEngine;

namespace Tesseract.Cache
{
    /// <summary>
    /// Generic LRU cache for Resources.Load with configurable max size.
    /// Uses LinkedList for O(1) cache hit reordering.
    /// </summary>
    public class ResourceCache<T> where T : Object
    {
        private readonly int _maxSize;
        private readonly Dictionary<string, LinkedListNode<KeyValuePair<string, T>>> _cache;
        private readonly LinkedList<KeyValuePair<string, T>> _lruList;

        public ResourceCache(int maxSize = 20)
        {
            _maxSize = Mathf.Max(1, maxSize);
            _cache = new Dictionary<string, LinkedListNode<KeyValuePair<string, T>>>(_maxSize);
            _lruList = new LinkedList<KeyValuePair<string, T>>();
        }

        /// <summary>
        /// Load a resource from cache or Resources folder.
        /// On cache hit, moves the item to the front (most recently used).
        /// </summary>
        public T Load(string path)
        {
            if (_cache.TryGetValue(path, out var node))
            {
                // Move to front (most recently used)
                _lruList.Remove(node);
                _lruList.AddFirst(node);
                return node.Value.Value;
            }

            T result = Resources.Load<T>(path);
            if (result == null)
            {
                Debug.LogWarning($"[ResourceCache] Failed to load: {path}");
                return null;
            }

            var newNode = _lruList.AddFirst(new KeyValuePair<string, T>(path, result));
            _cache[path] = newNode;
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
            _lruList.Clear();
        }

        /// <summary>
        /// Current number of cached items.
        /// </summary>
        public int Count => _cache.Count;

        private void Evict()
        {
            while (_cache.Count > _maxSize)
            {
                var last = _lruList.Last;
                _cache.Remove(last.Value.Key);
                _lruList.RemoveLast();
            }
        }
    }
}
