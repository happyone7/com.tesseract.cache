using UnityEngine;

namespace Tesseract.Cache
{
    /// <summary>
    /// String extension methods for cached resource loading.
    /// </summary>
    public static class ResourceCacheExtensions
    {
        private static readonly ResourceCache<GameObject> _prefabs = new ResourceCache<GameObject>(10);
        private static readonly ResourceCache<Sprite> _sprites = new ResourceCache<Sprite>(10);
        private static readonly ResourceCache<AudioClip> _audioClips = new ResourceCache<AudioClip>(20);

        /// <summary>
        /// Load a prefab with LRU caching.
        /// </summary>
        public static GameObject LoadPrefab(this string path) => _prefabs.Load(path);

        /// <summary>
        /// Load a sprite with LRU caching.
        /// </summary>
        public static Sprite LoadIcon(this string path) => _sprites.Load(path);

        /// <summary>
        /// Load an audio clip with LRU caching.
        /// </summary>
        public static AudioClip LoadAudioClip(this string path) => _audioClips.Load(path);

        /// <summary>
        /// Load and instantiate a prefab.
        /// </summary>
        public static GameObject InstantiatePrefab(this string path, Transform parent = null)
        {
            GameObject prefab = path.LoadPrefab();
            if (prefab == null)
            {
                Debug.LogError($"[ResourceCache] Failed to load prefab: {path}");
                return null;
            }
            return Object.Instantiate(prefab, parent);
        }

        /// <summary>
        /// Clear all static caches.
        /// </summary>
        public static void ClearAllCaches()
        {
            _prefabs.Clear();
            _sprites.Clear();
            _audioClips.Clear();
        }
    }
}
