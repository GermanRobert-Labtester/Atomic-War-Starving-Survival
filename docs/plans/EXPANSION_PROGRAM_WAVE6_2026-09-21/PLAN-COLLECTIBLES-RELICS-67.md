# PLAN-COLLECTIBLES-RELICS-67 — Sets, Provenance, Display & Collector Economy

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ASSET-PIPELINE-19, PLAN-CREATIVE-WORKS-66.
**Implementation scaffold:** [`PLAN-COLLECTIBLES-RELICS-67_APPENDIX-A_SCAFFOLD.md`](PLAN-COLLECTIBLES-RELICS-67_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no loot boxes, no real coins/stamps/brands; all collectibles
fictional.

## Outcome
Collectibles already exist in code and data: `CollectibleCatalog.cs`,
`CollectibleDiscoveryState.cs`, `Collectibles/`, `UniqueItemClaimRegistry`,
`collectibles matrix` generator, `--collectibles-selftest`, plus relic
families across `relic_recipes.json` (39), `verdict_items.json`, and
`deep_lore_locations.json`. This plan turns collecting into a **knowledge and
display loop**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Discovery | `CollectibleDiscoveryState`, expedition loot | find, trade | set progress, lore |
| Identity | `UniqueItemClaimRegistry` | register a unique find | provenance, no duplicates |
| Sets | catalog sets | complete a set | morale/reputation, unlock lore |
| Display | museum/quarters | exhibit | visitor interest, identity |
| Provenance | item lore (`ItemLoreSystem`) | research an item | story, value |
| Economy | market/black market | sell, appraise | price by rarity + provenance |
| Preservation | condition system | store, restore | value retained |
| Knowledge | field guide/codex | record | unlock hints for remaining items |

## Evidence
- Core: `CollectibleCatalog`, `CollectibleDiscoveryState`, `Collectibles/` (dir), `UniqueItemClaimRegistry`, `Inventory/ItemLoreSystem` (orphan, 11 tests).
- Data: `collectibles` catalogs + `artifacts/` collectibles matrix; `relic_recipes.json` 39 relics; `verdict_items.json`.
- Sealed prior: `--collectibles-selftest`, Plan 87 relic restoration, Plan 190/`DEC-29` provenance inspection.
- Contracts: one inventory; unique-claim registry prevents duplicates; no parallel collection store.

## Packages
- **CR-67A** set model: authored sets with completion effects and lore unlocks; progress visible.
- **CR-67B** discovery integration: finds come from canonical loot/excavation/maritime tables; no bespoke spawner.
- **CR-67C** provenance: item lore research reveals origin; provenance affects value and display.
- **CR-67D** display/exhibition: museum/quarters display gives identity + visitor interest (ties Plans 30/66).
- **CR-67E** collector economy: appraisals and trades through canonical market/black market; rarity premiums authored.
- **CR-67F** preservation: condition degradation and restoration for display pieces.
- **CR-67G** content volumes: +8 sets (48 items), +12 lore entries, +6 appraisal rows; fictional.

## Acceptance & verification
- Set completion effects bounded; provenance changes value explainably; no duplicate uniques.
- `godot --headless --path . -- --collectibles-selftest`; `bash scripts/run_test.sh Ashfall.Core.Tests/Collectibles/`; economy suites.

## Risks
Collectible grind → small sets, discovered through play, tradeable; no random-only drops.

---

## 6. Expanded census (3 files · 687 lines)

Scope: `Assets/Ashfall.Core/Collectibles/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CollectibleEffectDispatcher.cs` | 352 | Support | — | 0 | 0 | 0 |
| `CollectibleMapProjector.cs` | 153 | Support | — | 0 | 0 | 0 |
| `CollectibleTutorialTracker.cs` | 182 | Support | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `collectibles.json` | object[2 keys] |
| `relic_recipes.json` | object[2 keys] |
| `relic_provenance_dossiers.json` | object[3 keys] |

**State surfaces:** `CollectibleTutorialTracker.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Collectibles/` |
| Test references | 15 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 3. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CR-67A` | no name match — resolve at claim time |
| `CR-67B` | no name match — resolve at claim time |
| `CR-67C` | no name match — resolve at claim time |
| `CR-67D` | no name match — resolve at claim time |
| `CR-67E` | no name match — resolve at claim time |
| `CR-67F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **3** · Test files: **19** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/CollectibleEffectDispatcher.cs`, `src/Host/HostCli.Collectibles.cs`, `src/Main.Collectibles.cs` |
| Tests (`Ashfall.Core.Tests/`) | 19 | `Ashfall.Core.Tests/CollectibleCatalogTests.cs`, `Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryStateTests.cs`, `Ashfall.Core.Tests/CollectibleItemPresentationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **4**; isolated: **1**.

| From | → To |
|---|---|
| `CollectibleEffectDispatcher` | `CollectibleCatalog` |
| `CollectibleEffectDispatcher` | `CollectibleDiscoveryState` |
| `CollectibleMapProjector` | `CollectibleCatalog` |
| `CollectibleMapProjector` | `CollectibleDiscoveryState` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/collectibles.json` |
| `Assets/StreamingAssets/Data/discovery_consequences.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (11 files, 72 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Collectibles` | 11 | 72 |

**Verdict:** 72 cases sit under matching regions — run those first (`Collectibles`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **5**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/CollectibleEffectDispatcher.cs` |
| `src/Host/HostCli.Collectibles.cs` |
| `src/Main.Collectibles.cs` |
| `src/UI/TutorialPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `collectible_discovery` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `breach_obstacle_secondary_effect` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `collectibles.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 1 (laddered 0) · RNG streams 1 · host files 6 · catalogs 4 · test regions 1 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-COLLECTIBLES-RELICS-67
wave: 6
status: PROPOSED — foreman claim required
packages: CR-67A, CR-67B, CR-67C, CR-67D, CR-67E, CR-67F, CR-67G
claim paths:
  - src/Host/CollectibleDiscoverySaveStore.cs  # §19 candidate host surface
  - src/Host/CollectibleEffectDispatcher.cs  # §19 candidate host surface
  - src/Host/HostCli.Collectibles.cs  # §19 candidate host surface
  - src/Main.Collectibles.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/collectibles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/discovery_consequences.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Collectibles/
dependencies:
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
