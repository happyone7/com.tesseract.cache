# Changelog

## [1.1.0] - 2026-02-14
### Fixed
- LRU implementation: Replaced Queue (FIFO) with LinkedList for proper O(1) cache hit reordering
- Domain Reload: Added [RuntimeInitializeOnLoadMethod] to reset static caches on play mode enter
- maxSize validation: Clamped to minimum 1 via Mathf.Max
- Eviction: Evicts from tail (least recently used) instead of head

## [1.0.0] - 2026-02-14
### Added
- ResourceCache<T>: Generic LRU cache for Resources.Load with configurable max size
- ResourceCacheExtensions: String extension methods (LoadPrefab, LoadIcon, LoadAudioClip, InstantiatePrefab)
- Automatic eviction when cache exceeds max size
