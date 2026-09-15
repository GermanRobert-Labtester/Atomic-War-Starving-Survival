# ASHFALL Collectible Effect Contract

**Status:** ACTIVE · **Date:** 2026-09-13 · **Batch:** `TASKS-5-8-COLLECTIBLE-INTEGRATION`

The stable contract between the collectible layer and the campaign authorities it feeds. Runtime truth lives in `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`; this document pins the semantics the tests enforce.

## Runtime chain (one path, all sources)

```text
any acquisition (scavenging / purchase / quest reward / scripted grant)
→ Inventory.OnItemAdded (host feeder, Main.Collectibles.cs)
→ CollectibleEffectDispatcher.DispatchOnAcquire
→ { resolve definition → discovery idempotence gate → typed effect branch }
→ owning subsystem's public API
→ discovery registered ONLY on success
→ one OnCollectibleDiscovered event
→ normal save pipeline
```

Restore never fires `OnItemAdded`, so save/load never replays effects (§1.5). Discovery idempotence is defense-in-depth, not the primary mechanism.

## Effect routing

| effect_type | Authority (canonical API) | Semantics |
|---|---|---|
| `none` | — (discovery registered only) | catalog/vinyl-route collectibles; the vinyl acquisition map registers records **before** the dispatcher runs |
| `morale` | `NeedsSystem.Modify(Morale, value)` | bounded [0, 10], all living survivors |
| `knowledge` | `ResearchSystem.UnlockManual(target)` | **reveal only** — adds to `unlockedIds`/`isUnlocked`; completion stays behind `StartResearch` + prerequisite gate; never touches `completedIds`, active research, or progress |
| `journal_unlock` / `faction_info` | `JournalSystem.TryDiscoverKnowledge(target)` | entry + codex unlock; already-known = idempotent success |
| `location_clue` | `WastelandMapSystem.DiscoverSurvey(target, "collectible_clue", day)` | **Surveyed** fog state (High confidence, clue provenance); never `Visited`; never routes; never neighbors |

## Result contract (§4)

`Applied` / `AlreadyDiscovered` (idempotent success) / `EffectTypeUnknown` / `EffectTargetUnknown` (`effect_target_unknown:<id>`) / `MapNodeNotFound` (`map_node_not_found:<id>`) / `…_authority_unavailable` (retryable). A failed dispatch leaves the collectible **undiscovered** so a later fresh acquisition retries; partial effects are never half-recorded.

## Target validation (§3, §6.9, §7.11)

Runtime: `knowledge` targets must exist in the loaded research catalog; `location_clue` targets must resolve to real map nodes — failures are typed and retryable, never silent swallows.

Permanent gate: `CollectibleCatalogIntegrityValidator.Validate` runs inside `--data-integrity-selftest` and enforces, for all 40 definitions: item↔definition bijection (`item_collectible_*` both directions), category/rarity/effect-type vocabularies, knowledge → `research_knowledge.json`, `location_clue` → `wasteland_map_v1.json`, journal/faction → `journal_voice_prose.json`, ≥1 acquisition source across scavenging/quest/map/radio catalogs, deterministic finding order.

## Vinyl morale ownership (§8.2–§8.4)

**Model decision — Model A (individual playable records), already chosen by the repository.** The three vinyl collectibles are registered acquisition routes in `VinylRecordAcquisitionMap` mapping each physical item to real `record_id`s (chamber → `record_03` very_rare/scavenging, civil broadcast → `record_04` rare/expedition, folk compilation → `record_02` uncommon/barter). No new definitions were authored; no duplicate physical record items exist.

Vinyl collectibles delegate morale ownership to `VinylMoraleSystem`. Acquisition (the host feeder runs the acquisition map BEFORE the dispatcher) registers the record exactly once via `AcquireRecord` — ownership only, zero morale. The single morale authority is `ApplyDailyEffect` (once per day while the turntable plays). Collectible pickup applies **zero** independent morale; no double award exists between pickup and playback (pinned in `CollectibleVinylIntegrationTests.VinylPickup_AppliesZeroMorale_PlaybackAppliesExactlyOneDailyEffect`). Reacquisition follows the authored fallback: a second physical copy yields a NEW title, never the same record twice, and never a second morale effect.

The existing vinyl panel binds `VinylMoraleSystem` state → projection; no collectible-specific record-player UI exists or is needed (§8.9).

## Player-facing copy (§15)

First discovery emits `CollectibleDiscovered` (collectible id, display name, category, effect summary). Panels subscribe to subsystem state events (`OnResearchCompleted`-class, `OnNodeKnowledgeChanged`, vinyl events) — collectible code never manipulates panel rows directly.
