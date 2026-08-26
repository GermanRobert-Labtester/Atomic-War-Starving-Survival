# ASHFALL — Direct Godot Asset Loads Audit

**Date:** 2026-08-26
**Scope:** Forensic inventory of all `GD.Load`, `ResourceLoader.Load`, and direct engine file access calls in `src/` outside of `AssetRegistry.cs`.
**Purpose:** Classify direct engine asset loading into **Intentional UI/System Loaders**, **Canonical Fallbacks**, and **Candidates for Later Consolidation** (Report-only; no code changes).

---

## 1. Executive Summary

Centralized asset resolution in ASHFALL is governed by [`src/Host/AssetRegistry.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/AssetRegistry.cs), which resolves item textures, survivor portraits, location backdrops, faction emblems, and procedural fallbacks.

Only **6 direct resource load calls** and **2 file access calls** exist outside of `AssetRegistry.cs` across the entire 200+ file Godot host. All 6 direct resource load calls are architectural intentional patterns (subsystem loaders, shared UI helpers, and component scene instantiations).

---

## 2. Direct Asset Load Inventory & Categorization

| # | Source File & Line | Invocation Expression | Loaded Asset Type | Category | Architecture Rationale |
|---|---|---|---|---|---|
| 1 | [`src/Audio/AudioManager.cs:319`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Audio/AudioManager.cs#L319) | `ResourceLoader.Load<AudioStream>(path)` | `AudioStream` (`.wav`, `.ogg`, `.mp3`) | **Intentional Audio Subsystem Loader** | Dedicated audio pipeline cache. Dynamically loads audio cues from `res://assets/audio/` with LRU caching. |
| 2 | [`src/UI/AshfallUiHelpers.cs:57`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/UI/AshfallUiHelpers.cs#L57) | `ResourceLoader.Load<FontFile>(path)` | `FontFile` (`.ttf`) | **Intentional UI Theme Loader** | Shared font loader for BarlowCondensed & ShareTechMono typography hierarchy with static caching. |
| 3 | [`src/UI/AshfallUiHelpers.cs:547`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/UI/AshfallUiHelpers.cs#L547) | `ResourceLoader.Load<Texture2D>(path)` | `Texture2D` (`.png`, `.jpg`) | **Intentional Shared UI Helper** | Low-level safe texture loader in `AshfallUiHelpers.TryLoadTexture()` with file existence guards. |
| 4 | [`src/UI/AshfallUiHelpers.cs:565`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/UI/AshfallUiHelpers.cs#L565) | `ResourceLoader.Load<Texture2D>(alt)` | `Texture2D` (`.png`, `.jpg`) | **Intentional Shared UI Helper** | Alternate path candidate loader in `AshfallUiHelpers.TryLoadTexture()`. |
| 5 | [`src/World/SurvivorActorView.cs:51`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/World/SurvivorActorView.cs#L51) | `GD.Load<Texture2D>(FallbackTexturePath)` | `Texture2D` (`.png`) | **Canonical Fallback Loader** | Loads default survivor sprite using centralized constant `AssetRegistry.FallbackSurvivorPath`. |
| 6 | [`src/World/WastelandMapView.cs:32`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/World/WastelandMapView.cs#L32) | `GD.Load<PackedScene>("res://src/World/MapLocationMarkerView.tscn")` | `PackedScene` (`.tscn`) | **Intentional Scene Component Loader** | Instantiates component node hierarchy for clickable map markers. |

---

## 3. Direct File Access Calls

| # | Source File & Line | Invocation Expression | Purpose | Category |
|---|---|---|---|---|
| 1 | [`src/Host/GodotFileIO.cs:38`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/GodotFileIO.cs#L38) | `FileAccess.Open(path, ModeFlags.Read)` | Godot adapter for Core `IFileIO` interface. | **Intentional Port/Adapter** |
| 2 | ~~`src/World/WastelandMapView.cs:85`~~ | `FileAccess.Open(jsonPath, Read)` | Consolidated into Core `WastelandMapCatalogLoader`. | **Resolved** |

---

## 4. Recommendations & Completed Consolidations

1. **Retain UI / Audio Loaders**: `AshfallUiHelpers`, `AudioManager`, and `AssetRegistry` form clean, domain-specific loading layers and do not require refactoring.
2. **Consolidation Completed**: `WastelandMapView` now reads from authoritative `WastelandMapSystem` (via `WorldHostSession`) and delegates catalog loading to engine-agnostic [`WastelandMapCatalogLoader.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs) instead of performing direct `FileAccess.Open` or JSON deserialization.
3. **Strict Policy Compliance**: Direct `GD.Load` or `ResourceLoader.Load` of texture assets outside of `AssetRegistry` and `AshfallUiHelpers` remains discouraged to preserve deterministic fallback handling.
