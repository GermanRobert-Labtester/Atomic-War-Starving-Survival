# ASHFALL — Audit Issues 26–35 Closeout

**Branch:** `feat/asset-pipeline-flagship`
**Date:** 2026-09-05
**Scope:** Local post-PR #36 audit findings 26–35 (Medium). Uncommitted with issues 1–25 waves.

---

## Disposition Matrix

| # | Severity | Finding | Disposition |
|---|---|---|---|
| **26** | M | Dynamic questlines catalog awaiting host wiring | **FIXED** — `DynamicQuestlineCatalogLoader.LoadAndRegister` in `YearOfAshHostSession.Create` after YoA quests; removed from LoaderWiring allowlist; formerly-allowlisted fact pins production call. |
| **27** | M | Collectibles gameplay feeder incomplete; tests quarantined | **FIXED** — production `CollectibleEffectDispatcher` on Main; `Inventory.OnItemAdded` feeder + `MarkCollectiblesDirty`; gated by `CollectibleProductionFeederGateTests`. `CollectibleEffectTargetResolutionTests` stays Compile Remove (authored target/map/research id mismatches — data follow-up). **Deferred:** `UniqueItemClaimRegistry.TryClaim` production feeder (ledger is constructed/saved on Main; claim calls remain HostCli/test-only until `OnItemGenerationCommitted` is wired). |
| **28** | M | Setup/Save/Flush triad drift | **GATED** — `MainTriadDriftGateTests` allowlists intentional Setup-without-Save aliases; does not require Flush for every Save (SaveAll-only is by design). |
| **29** | M | `FlushEndgameIfDirty` missing from `_Process` | **ALREADY FIXED** — pinned by triad gate (`FlushMoralChoice` → `FlushEndgame` adjacency in `Main.Application.cs`). |
| **30** | M | MoralChoice/Fire journeys Core-only | **EXTENDED** — SaveEnvelopeHelper round-trips + SaveAll/`_Process` Flush enrollment source pins for both domains. |
| **31** | M | Main host sprawl (~19.8k / 74 partials) | **DISPOSITION** — AGENTS H7 updated; decomposition deferred (`ashfall-decompose-godot`). |
| **32** | M | IEventBus underused | **DISPOSITION** — AGENTS EVENT SYSTEM documents Verdict/Dive-only scope; typed C# events remain primary. |
| **33** | M | Mixed JSON camel/snake | **PINNED** — `JsonNamingMixPinTests` lists heavily mixed root catalogs; no mass rename. |
| **34** | M | ARCHITECTURE_TEST_MAP `survivor_fate` / `onboarding` GAP | **FIXED** — generator `host: ["Main"]`; regenerated map → Constructed 110/110; both sections PASS 6/6. Residual GAPs: `chemical_synthesis` / `power_subgrids` missing xUnit fixtures (out of this wave). |
| **35** | M | AGENTS stale H2 / H11 / Guid / FlagLedger | **FIXED** — Guid + FlagLedger Normalize/Ordinal marked RESOLVED; H2 RESOLVED via `Inventory.WornGear` consolidation (no `FromInventory`); H11 JournalSystemTests RESOLVED; H7 counts refreshed. |

---

## Tests Added / Extended

- `LoaderWiringGateTests` — DynamicQuestline production pin; allowlist pruned
- `MainTriadDriftGateTests` — Setup orphan allowlist + FlushEndgame/SaveAll enrollment
- `JsonNamingMixPinTests` — mixed catalog pin list
- `FireIncidentJourneyTests` — SaveEnvelope + host wiring
- `MoralChoiceJourneyTests` — SaveEnvelope + host wiring
- `CollectibleProductionFeederGateTests` — Main dispatcher + OnItemAdded wiring pin

---

## Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  --filter LoaderWiring|MainTriad|JsonNamingMix|FireIncidentJourney|MoralChoiceJourney|CollectibleEffectTarget
dotnet build Ashfall.csproj
godot --headless -- --data-integrity-selftest
godot --headless -- --bridge-selftest
python3 scripts/ci/generate-architecture-map.py
```

---

## Known Remaining (out of 26–35 scope)

- Collectible integrity/perf/replay/scavenge Compile Remove entries still quarantined.
- Unique claim ledger production `TryClaim` feeder deferred (constructed + saved only).
- Main decomposition (H7) deferred.
- IEventBus full merge deferred.
- Mass JSON snake_case migration deferred (heavily-mixed + below-threshold pin lists only).
- `chemical_synthesis` / `power_subgrids` architecture map test GAPs.

## Reviewer nits addressed (post csharp-reviewer)

- H2 AGENTS wording corrected to consolidation truth (no `FromInventory`).
- `JsonNamingMixPinTests` splits heavily-mixed vs below-threshold pins and asserts mix floors.
- UniqueClaims production claim feeder dispositioned as deferred in this closeout.
