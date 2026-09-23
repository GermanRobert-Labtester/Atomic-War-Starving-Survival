# PLAN-MOD-CONTENT-BOUNDARY-92 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-CONTENT-PIPELINE-QA-77`](../EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-CONTENT-PIPELINE-QA-77.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `CP-77A` | pipeline doc: one page per content type with the five stages and the exact commands. |
| `CP-77B` | per-type checklists in `docs/content/<type>.md` (items, recipes, quests, narrative, weather, factions, audio,  |
| `CP-77C` | PR template additions: author attests the five stages; CI verifies what is verifiable. |

## 2. Source inventory (38 files, Core + host)

| File | Lines |
|---|---:|
| `Campaign/CampaignCalendarReadModel.cs` | 63 |
| `Campaign/CampaignInitializationMode.cs` | 14 |
| `Campaign/CrisisPredictionModel.cs` | 374 |
| `Commitments/CommitmentReadModel.cs` | 71 |
| `Economy/CommodityBaselineCatalog.cs` | 252 |
| `Expeditions/ArmoredCrawlerModuleCatalog.cs` | 133 |
| `Inventory/ItemInspectionModel.cs` | 153 |
| `MathfCompat.cs` | 27 |
| `Mods/JsonModLayering.cs` | 647 |
| `Mods/ModCompatibilityEvaluator.cs` | 199 |
| `Mods/ModDataContract.cs` | 506 |
| `Narrative/Continuity/NarrativeContinuityModel.cs` | 185 |
| `Radio/RadioBroadcastModels.cs` | 360 |
| `Shelter/ShelterShieldingModel.cs` | 305 |
| `Spiritual/SpiritualModels.cs` | 121 |
| `Survivors/FitnessForDutyModel.cs` | 910 |
| `Survivors/NeedsModifierStack.cs` | 239 |
| `Textiles/GarmentLayeringThermalEngine.cs` | 230 |
| `UI/CollectiblePresentationModel.cs` | 178 |
| `UI/ModalStackController.cs` | 160 |
| `World/ModalTravelDispatchEngine.cs` | 200 |
| `World/WeatherGateContextModifier.cs` | 45 |
| `host:Host/HostCli.Mods.cs` | 100 |
| `host:Host/ModRuntime.cs` | 94 |
| `host:Muster/ApproachSelectionModal.cs` | 84 |
| `host:UI/ConfirmationModal.cs` | 141 |
| `host:UI/DailyBriefingModal.cs` | 246 |
| `host:UI/DailyBriefingModalContent.cs` | 7 |
| `host:UI/IModalPanel.cs` | 29 |
| `host:UI/ModalManager.cs` | 135 |
| `host:UI/MoralChoiceModal.cs` | 318 |
| `host:UI/NarrativeArcModal.cs` | 142 |
| `host:UI/OpeningProtocolModal.cs` | 239 |
| `host:UI/OpeningProtocolModalContent.cs` | 7 |
| `host:UI/SafeCrackModal.cs` | 215 |
| `host:UI/SafeCrackModalContent.cs` | 7 |
| `host:YearOfAsh/DoorEncounterModal.cs` | 177 |
| `host:YearOfAsh/QuestlineModal.cs` | 240 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `vehicle_modifications.json` | object(2 keys) |
| `armored_crawler_modules.json` | object(2 keys) |
| `commodity_baselines.json` | object(4 keys) |
| `vehicle_modules.json` | object(2 keys) |
| `mod_manifest_schema.json` | object(9 keys) |
| `engineering_mod_notes.json` | object(4 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupModsMBScaffold` / `SaveModsMBScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Mods/MB92ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class MB92ScaffoldTests
{
    [Fact] public void MCB92A_TODO() { /* precedence spec + layering tests over a two-layer fixture. */ }
    [Fact] public void MCB92B_TODO() { /* conflict report: structured list + one panel/log surface; bounded sample in test */ }
    [Fact] public void MCB92C_TODO() { /* manifest pinning: checksum recorded and verified on load; mismatch is a typed wa */ }
    [Fact] public void MCB92D_TODO() { /* safe disable: remove-after-load fixture; save still loads; missing ids flagged v */ }
    [Fact] public void MCB92E_TODO() { /* determinism gate: same-seed modded replay matches; banned-source scan covers `Mo */ }
    [Fact] public void MB92_AuthorityConformance_TODO() { /* pattern parity with PLAN-CONTENT-PIPELINE-QA-77 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Mods/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win over new ones; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
