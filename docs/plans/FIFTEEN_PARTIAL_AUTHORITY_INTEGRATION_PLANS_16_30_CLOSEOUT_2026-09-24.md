# Fifteen partial-authority integration plans, items 16–30 — planning closeout

**Date:** 2026-09-24. **Scope:** prose, source audit, integration framework, editorial polishing, and architecture handoff. This wave changes documentation only. The historical text remains above each dated addendum; it is neither erased nor silently revalidated. No Core, Godot, JSON gameplay catalog, save codec, or runtime route was changed.

## Delivered sections

All fifteen requested subjects have their own addendum above 250,000 characters. Fourteen existing documents carry them; the Plan 11 World Exploration QA matrix carries both Cipher Quest Chain and World Evolution. Each addendum gives a bounded first outcome, current Core/API premise, one-authority map, data/host/save path, non-goals, phased implementation gates, a C# integration sketch, a large **non-canon** case register, a separate source-specific editorial review, and a finished planning handoff. The master expansion document informed the C3–C17 narrative directions, content information flow, and anti-duplication rules; current source overrode its dated examples where the tree changed.

| Subject | Updated document | Architecture disposition |
|---|---|---|
| SafeCrackingSystem | [Maritime save compatibility](../maritime/PLAN23_SAVE_COMPATIBILITY.md) | Host, modal, and save exist; reconcile Core's loot-transfer flag with canonical inventory receipt. |
| SaltMineExtractionSystem | [Production save migration](../production/PRODUCTION_SAVE_MIGRATION.md) | Foundry host and expansion-hub save exist; bind real vein/workforce/power and settled treaty/material transactions. |
| ApicultureSystem | [Production content utilization](../production/PRODUCTION_CONTENT_UTILIZATION.md) | Greenhouse host, panel, and nested save exist; validate feed transaction and use real climate/radiation inputs. |
| SeasonalEventSystem | [Seasonal resource swings](../world/SEASONAL_RESOURCE_SWINGS.md) | Weather-intelligence coordinator, day owner, and save exist; connect paid mitigation and actual resource consumers. |
| LatentExpertAwakeningSystem | [Skill catalog migration](../progression/SKILL_CATALOG_MIGRATION.md) | Core progress/save exist without a live host; ensure a valid actor and successful skill grant before awakened state commits. |
| RadioRecordingSystem | [Radio recording contract](../radio/RADIO_RECORDING_CONTRACT.md) | Radio host saves metadata; recording/replay commands and physical tape transaction are absent from production call sites. |
| CipherQuestChainEngine | [World Exploration QA matrix](../world/PLAN_11_WORLD_EXPLORATION_QA_MATRIX.md) | Three static Core chains and CLI probe exist; bind actual heard/key/decode/map facts through one quest/map save owner. |
| WorldEvolutionEngine | [World Exploration QA matrix](../world/PLAN_11_WORLD_EXPLORATION_QA_MATRIX.md) | Core and CLI probe exist; bind a current event to existing world owners and select one world-envelope save path. |
| RadioScheduleCoordinator | [Radio content utilization](../radio/RADIO_CONTENT_UTILIZATION.md) | Live radio host resolves schedules; define dynamic-alert lifetime, priority, and playback receipt. |
| EcologicalInfestationSystem | [Plan 28 sign-off](../ecology/PLAN28_PHASE8_SIGN_OFF.md) | Main day owner, disease/food consumers, and save exist; expose truthful discovery/response through the current owner. |
| YearOfAshIceRoadSystem | [Year of Ash truth plan](EXPANSION_PROGRAM_WAVE12_2026-09-21/PLAN-YEAR-OF-ASH-TRUTH-146.md) | Direct host absent; resolve its overlap with the active Holdfast IceRoadSystem before any Year-of-Ash attachment. |
| CombatBreachingEngine | [Plan 86 authority map](../combat/PLAN_86_AUTHORITY_MAP.md) | Core is indirectly integrated through TacticalCombatSystem.Breaching; verify production barrier/catalog/action route. |
| NarrativeContinuityEngine | [Wave 13 README](EXPANSION_PROGRAM_WAVE13_2026-09-21/README.md) | Read-only analyzer with HostCli diagnostic; integrate as a bounded content-acceptance gate, with no campaign state. |
| UvCoronaDetectionEngine | [UV closeout](../radio/PLAN_119_UV_CORONA_DETECTION_CLOSEOUT.md) | Core, catalog, and CLI probe exist; production scan needs inventory, fault inputs, campaign RNG, save, and a reader. |
| GroundPenetratingRadarEngine | [GPR closeout](../world/PLAN_121_GPR_CARTOGRAPHY_CLOSEOUT.md) | Core, catalog, and CLI probe exist; production survey needs terrain/target input, power, save, and map-lead receipt. |

## Finished integration architecture for this planning wave

```text
authored JSON or a fact from the current campaign owner
    → one engine-free Core system / pure analyzer
    → one Godot command, day adapter, or bounded tooling invocation
    → existing save owner for accepted mutable state (none for pure diagnostics)
    → truthful panel, map, radio, inventory receipt, or report
```

The architecture is **specified**, not implemented. Its stop decisions are explicit:

1. **Inventory transactions:** Safe loot and cassette recording must settle real inventory alongside their Core state. Hive feeding and seasonal mitigation must reject before spending. Salt-mine output and GPR/UV battery use must reconcile with the canonical inventory owner. None of these may use a panel-owned count.
2. **Overlapping authorities:** The active Holdfast `IceRoadSystem` already decides travel. `YearOfAshIceRoadSystem` cannot become a second road verdict without a signed retirement/projection/migration choice. `CombatBreachingEngine` already runs through `TacticalCombatSystem`; its next step is a reachability audit, not a second breaching host. `NarrativeContinuityEngine` remains tooling only.
3. **Fact and save custody:** Cipher decoding enters the existing radio, inventory, quest, flag, and map owners one way. World evolution mutates the existing world instance and saves its triggered IDs in an approved world envelope. Latent awakening must coordinate its state with a successful skill grant. Radio scheduling holds no separate persistent schedule ledger; alert reconstruction must be defined at restore.
4. **Campaign time and replay:** Seasonal events, infestations, production, weather, and world evolution use the canonical day coordinator; UV/GPR surveys and safe attempts use approved seeded streams or persisted RNG state. No UI refresh advances a tick, spends a resource, or emits a success fact.
5. **Content promotion:** Candidate case prose remains **non-canon** until a current JSON ID or owner fact, eligibility rule, loader, host command, save outcome, and reader are evidenced. The master expansion document's scale-honesty clause explicitly forbids treating bulk unverified prose as finished gameplay content.

## Promotion order

Start with premise corrections and transaction gaps that can misreport outcomes: safe loot, hive feeding, seasonal mitigation, radio cassette stock, and latent skill grant. Then resolve the Year-of-Ash road boundary and verify Combat Breaching's existing tactical path. Promote one reachable route each for Cipher Quest, World Evolution, UV Corona, and GPR through current host owners. Polish radio schedules, infestation UI, and Salt Mine material/treaty presentation after their canonical inputs are verified. The continuity analyzer should enter the content-acceptance toolchain independently of campaign runtime. Every implementation package still requires a foreman path claim under `WORKTREE_OWNERSHIP.md` and current acceptance in `INTEGRATION_PLANS.md`.

## Polishing and verification record

The separate polishing script inserted a source-specific correction and first-package acceptance into every subject. It called out live hosts hidden by old direct-name reference counts; missing inventory receipts; a possible awakening-without-skill transition; demo or fixed environmental inputs; an unbound radio record command; and the difference between runtime systems and diagnostics. Static document checks cover 15 distinct marked sections over 250,000 characters each, original-prefix preservation, balanced code fences, source-path existence, valid closeout links, clean whitespace, and reproducible generator-plus-polish output. No xUnit or Godot runtime tests were run for this documentation-only wave.

**Closeout status:** the requested prose and integration-framework planning architecture is complete. Gameplay integration and canon promotion remain subject to the named owner and foreman gates above.
