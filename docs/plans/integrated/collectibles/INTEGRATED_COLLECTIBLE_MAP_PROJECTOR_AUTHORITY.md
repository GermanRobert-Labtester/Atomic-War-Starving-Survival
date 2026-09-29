# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-27** — verified zero production consumers before wiring; built, gated, and reachability-proved.

---

## `CollectibleMapProjector`

`CollectibleMapProjector` maps live discovery state into map markers and
location clusters, explicitly excluding hidden effect targets and location
clues. It had **zero production consumers** — collectible discoveries were
recorded but never projected anywhere.

**What shipped.**

- **`src/Main.Collectibles.cs`** — `GetCollectibleMapMarkers()` and
  `GetCollectibleMapClusters()` project from the live `CollectibleDiscoveryState`
  and the loaded `CollectibleCatalog`. Pure projection: records nothing, mutates
  nothing, so it is safe at any map/UI lifecycle boundary. Markers sort ordinally
  by `MarkerId`, so output is deterministic across platforms and runs.

**Evidence.** `grep -rlw CollectibleMapProjector src/ --include=*.cs | grep -v HostCli`
→ `src/Main.Collectibles.cs`. `CollectibleMapIntegrationTests` pass (36s).

**Rule 5:** discovery state and the catalog stay the sole authorities; the
projector only reads them.

---
