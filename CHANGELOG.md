# Changelog

## [1.0.0] - 2026-02-14
### Added
- ResourceCache<T>: Generic LRU cache for Resources.Load with configurable max size
- ResourceCacheExtensions: String extension methods (LoadPrefab, LoadIcon, LoadAudioClip, InstantiatePrefab)
- Automatic eviction when cache exceeds max size
