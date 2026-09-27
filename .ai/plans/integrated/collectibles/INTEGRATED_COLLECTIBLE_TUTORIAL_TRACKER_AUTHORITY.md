# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-27** — production consumer wired, verified by per-file reachability.

---

## Integration update (2026-09-27) — `CollectibleTutorialTracker`

`CollectibleTutorialTracker` owned the first-time collectible introductions (Cultural
Artifacts, Reading and Discovering) with a full seen/queue model and save DTO, but had
**zero production consumers**: the tracker was never fed, so no tutorial ever queued.

**What shipped.**

- **`src/Main.Collectibles.cs`** — the live `CollectibleDispatchResult` from
  `DispatchOnAcquire` is now handed to `OnCollectibleDiscovered(result)`, so the queue is driven
  by a real first-discovery event, never by historical save state.
- **`Assets/Ashfall.Core/CollectibleDiscoveryState.cs`** — `CollectibleDiscoverySave` gained an
  **optional** `tutorials` field restoring as empty for saves written before it existed
  (historical discoveries still cannot trigger a tutorial).
- **`src/Main.Lifecycle.cs`** — tracker cleared with the rest of the collectible triad.

Mounted inside the existing `collectible_discovery` section: no parallel save store.

**Evidence.** `grep -rlw CollectibleTutorialTracker src/ --include=*.cs | grep -v HostCli`
→ `src/Main.Collectibles.cs`. `CollectibleTutorialIntegrationTests` and
`CollectibleCampaignSmokeTests` pass.

---
