# Plan 85 — Fragment Lifecycle

## Observed rule (ratified): Model B — discovery token

Damaged-map fragments are **not inventory items**. `fragment_id` is a catalog/state key. There are no fragment item definitions, no drop/sale/destroy semantics, and no duplicate-inventory ambiguity, because physical possession is not part of the model.

## Lifecycle

| Stage | Mechanism | Reversibility |
|---|---|---|
| Produced | weighted entry with `map_fragment_id` in a Plan 46 scavenging table resolves during an expedition's Looting phase (`ScavengingTableCatalog.RollLoot`, seeded `ISeededRng`) | repeatable source |
| Discovered/registered | `ExpeditionSystem.PerformLootRoll` forwards the token to `DamagedMapSystem.RegisterFragment`; the id is appended to `WastelandMapState.RegisteredMapFragments` | **permanent** (never un-registered) |
| Duplicate acquisition | `RegisterFragment` returns false; nothing is granted, nothing double-counts (§1.7 idempotence) | no-op |
| Duplicate roll economics | a fragment entry with an empty `item_id` yields no physical loot line, so post-registration duplicates resolve to nothing tangible — fragments cannot become a money farm (§85C.4) | no-op |
| Completion | derived (`IsZoneComplete`): every catalog fragment of the zone registered. Catalog reordering, reload, and duplicate registration cannot re-fire it (edge-triggered on the registering call only) | one-way |
| Reveal | `WastelandMapSystem.Discover(nodeId)` + `Unlock(nodeId)` — the installation node leaves the Locked state on the world map and the expedition destination passes the dispatch gate | one-way; persists in `Discovered`/`Unlocked` |
| Loss of access | impossible: registration is state, not possession. There is nothing to sell, drop, or destroy (§1.10 hard-lock risk eliminated by construction) | n/a |

## Possession-vs-knowledge statement (§1.3)

Map knowledge is permanent once registered. Selling, dropping, or destroying anything can never reduce zone progress, because fragments never enter the inventory. This is written into `DamagedMapSystem`'s contract and pinned by tests (`Reveal_PersistsThroughCaptureRestore_AndSurvivesReload`, `RegisterFragment_DuplicatesAndUnknowns_NeverDoubleCount`).

## Unknown / legacy fragment ids in old saves (§7.6)

Unknown ids inside `RegisteredMapFragments` are inert: completion only ever matches catalog fragment ids of the zone being checked. They are neither pruned nor reinterpreted on load — consistent with the project's existing save policy of tolerating stale ids in list-typed state (they cannot complete any zone and are overwritten by nothing). Pinned by `OldSave_OriginalZoneProgress_LoadsAndPreserves_UnderExpandedCatalog`.

## Save boundary coverage (§5.3 subset implemented)

Covered by tests: 0→1 registration, N-1→N completion, duplicate final fragment after completion, save at N-1 then restore and collect final, save after completion (reveal state round-trip), old-save fixture with original-zone progress under the expanded catalog. Expedition en-route / cache-open boundaries are the pre-existing expedition save flow and were not modified.
