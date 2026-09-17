# Ashfall Next Steps Quality Roadmap — Batch 47

**Plan file:** `/home/robertsrff/Desktop/luna)plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_47.md`<br>
**Source inventory:** `/home/robertsrff/Desktop/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_47.md`<br>
**Scope:** Steps 737–752<br>
**Prerequisite:** Batches 43–46, especially provenance/legacy records, finite ecology/material/machinery, communications, observation, safe travel, accessibility, and transparent derived metrics.

## 1. Purpose and exit condition

Batch 47 builds productive capacity and a durable legacy layer while placing clear safety boundaries around defense-adjacent infrastructure. Industrial work comes first because it supplies the material, food, medical, communication, and maintenance capacity that the heritage surfaces depend on. Heritage and communications then consume the same provenance/observation contracts. Longbow towers and hornwork are allowed only as bounded shelter/perimeter/observation infrastructure after a safety review; they must not become combat optimization or surveillance mechanics.

The exit condition is a finite, maintainable industrial chain; consented, privacy-aware legacy records; bounded astronomy/computation and health/ecology links; safe mountaineering; and an explicit rejection or recast path for unsafe combat/surveillance interpretations.

## 2. Review findings and constraints

Batch 46 supplies a reusable finite machinery contract and ecology base — this review could not independently verify Batch 46's actual merged state since no Batch 46 plan file was in scope for this pass; confirm Batch 46's real deliverables before treating its contracts as stable, following the same caution flagged for Batch 44's reliance on Batch 43. Batch 43 supplies provenance, correction, consent, privacy, and calibrated observation (same caveat applies). Existing `EpilogueMatrixRuntime` (confirmed: `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`, `public sealed class EpilogueMatrixRuntime`, with `EpilogueEvaluationContext` DTO and enums `RegionalFate`/`DemographicOutcome`/`MoralStanding`) is the future endgame authority; this batch's memoirs, documentaries, vision statements, and philosophical legacy must feed it as evidence/read models rather than write a new ending. The host orchestration risk in `src/Main.cs` applies especially to industrial systems with many fields, so a setup/save/restore/flush matrix is mandatory.

Satellite tracking is acceptable only as astronomy/observation of objects or signals without identifying or following people. Longbow towers and hornwork must be represented as safe infrastructure, historical artifacts, or route/visibility systems with storage, inspection, consent, and misidentification safeguards. If a requested mechanic depends on enemy targeting, mass surveillance, or glorified violence, it is out of scope and must be replaced with neutral observation, weather, route, or shelter information.

**New finding — same ID-prefix divergence risk as Batch 44.** This batch introduces new IDs for industrial (pig iron, seed oil, spinning, porcelain, charcuterie), scientific (difference engine, kelp/iodine, satellite), and legacy/heritage (murals, memoirs, documentary photography, vision statement, philosophical legacy) systems. Per the same verification performed for Batch 44: AGENTS.md's prose "known prefixes" list includes `expansion_`, `skill_`, `ending_`, `article_`, `sector_`, none of which exist in the actual enforced `IdPrefixes` array in `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`. Any of this batch's new IDs using an AGENTS.md-only prefix will fail `--data-integrity-selftest`. Check the actual array, not the prose summary, before authoring data files.

## 3. Non-goals

- No generic crafting/industrial ledger that bypasses existing inventory, economy, power, ecology, or food authorities.
- No person tracking, enemy-position system, weapon progression, combat optimization, or real-world military content.
- No second narrative, memoir, archive, or ending writer.
- No free industrial output, perfect material quality, or guaranteed scientific discovery.
- No Unity-side gameplay code, `JsonUtility`, `System.Random`, or `Guid.NewGuid()`.

## 4. Shared industrial and legacy contracts

### 4.1 Industrial process

Every process must declare material/fuel/water inputs, labor/skill, power/heat, capacity, time, quality, waste/byproducts, maintenance, safety, contamination, storage, and interruption. Reserve inputs before work, consume them according to actual progress, and preserve partial work on interruption. Outputs must be traceable to source batches where quality or health depends on them.

### 4.2 Legacy/provenance record

Extend Batch 43’s record with creator/subject consent, privacy scope, attribution, revision, withdrawal, custody, publication status, and access mode. Memoirs and photographs may contain conflicting or private accounts. A documentary or vision statement must show its source and uncertainty rather than silently becoming objective history.

### 4.3 Observation and communication

Difference-engine results, satellite observations, iodine/kelp measurements, and mountaineering route data should use calibrated units and the existing observation/message contracts. Communications must not expose private identity or become a tracking system.

## 5. Delivery sequence

### Slice 47.0 — Industrial/material capacity

- **Step 745 — Pig iron:** model ore/feedstock, furnace, fuel/heat, labor/skill, capacity, temperature/quality, slag/byproducts, maintenance, safety, contamination, and partial smelt. Reuse power/material/economy authorities. **Done when:** a unit test starts a smelt, interrupts it at a fixed percent complete, saves/reloads, and asserts the resumed job continues from the same percent-complete with the same reserved-but-not-yet-consumed input quantities (not restarted, not silently completed).
- **Step 742 — Seed oil:** model seed input, crop provenance, pressing/extraction, labor, equipment wear, yield, contamination, storage, spoilage, and food/medical use. Do not duplicate seed-saving or agriculture state. **Done when:** a unit test asserts the seed-oil process reads crop provenance through the existing agriculture/seed-saving system's API (name the specific existing type once confirmed during implementation) rather than storing its own duplicate seed-lot record.
- **Step 746 — Spinning:** model fiber, preparation, spindle/tool condition, labor, quality, waste, output, maintenance, and accessibility. Reuse Batch 46's cultural/material records (unverified in this review — confirm Batch 46 actually shipped a cultural/material record type before assuming it exists). **Done when:** a unit test degrades spindle/tool condition below a defined threshold and asserts output quality decreases by a specific, asserted amount, and a second test asserts continued use below a "broken" threshold blocks further output until repaired (maintenance is enforced, not decorative).
- **Step 751 — Porcelain:** model clay/mineral/fuel/water, kiln capacity, firing quality, breakage, labor, contamination, heat, and repair. Use the finite machinery contract (unverified — confirm Batch 46's "finite machinery contract" exists with a concrete interface/DTO name before this step's implementation begins, since the plan currently references it only by description). **Done when:** a unit test fires a kiln batch under an insufficient-fuel condition and asserts a defined breakage/failure rate applies (not a guaranteed-success default), with the failure outcome deterministic under a fixed `ISeededRng` seed.
- **Step 740 — Charcuterie:** model meat/animal provenance, salt/smoke/fuel/water, labor, time, contamination, spoilage, storage, welfare, and byproducts through existing food/health systems. **Done when:** a unit test ages a charcuterie batch past its defined safe-storage window and asserts it is flagged contaminated/unsafe rather than remaining silently consumable, and asserts the contamination flag is queryable by the same health-effect system that already handles irradiated/contaminated food (no parallel contamination enum).

Build these as individual Core adapters over shared process/resource ports, not a monolithic factory. Tests must cover heat/fuel shortage, maintenance, quality variation, contamination, spoilage, waste, capacity contention, partial firing/smelt, deterministic replay, and save migration.

**Scope-creep flag (missing in original plan):** five industrial chains in one slice, each with its own material/heat/labor/contamination model, is comparable in scope to Batch 44's Slice 44.2 flag above. Split into separate implementation commits per the project's "one system per task" rule even if planned as one slice.

### Slice 47.1 — Finite science, health, and communications

- **Step 741 — Difference engine:** model finite materials, construction/repair, power or labor, computation capacity, input validation, output provenance, maintenance, and failure. It is a bounded calculation/education capability, not a general arbitrary-code runtime. **Done when:** a unit test feeds an out-of-range or malformed computation input and asserts a defined rejection/failure response is returned (not an exception escaping to the caller, and not silent truncation), and a second test asserts there is no code path in this feature that accepts or executes user-supplied logic/expressions beyond the bounded calculation set the design defines — this is the concrete test for "not a general arbitrary-code runtime," which the original plan stated as a constraint but never as a checkable assertion.
- **Step 748 — Kelp/iodine:** connect species/ecology, water/contamination observations, harvesting limits, processing, dosage/health effects, storage, and uncertainty to existing health/food systems. Avoid medical claims beyond the game's defined rules; an uncalibrated or contaminated batch cannot be treated as safe. **Done when:** a unit test asserts an uncalibrated-observation or contaminated kelp batch cannot be marked "safe to consume" by any code path (the batch's safety status must derive from the observation/contamination data, not be settable independently of it), and a dosage-bounds test asserts exceeding a defined dose threshold triggers the same health-consequence path as other overdose/toxicity effects in the game, not a bespoke one.
- **Step 747 — Satellite tracking:** restrict the feature to astronomical object/signal observation, orbital/visibility windows, instrument condition, weather/occlusion, uncertainty, and communication delay. It must not track survivors, "enemies," individuals, or private movement. Store observations through provenance with privacy-safe identifiers. **Done when:** a unit test asserts no public method on this feature's API accepts a survivor ID, location, or any individual-identifying parameter as an *input* used to produce a tracking result — the type signatures themselves should make person-tracking impossible to call, not just discouraged by convention. This is a safety-boundary step; per Section 7 below, the test suite must assert this by inspecting the actual method signatures, not merely by testing that nobody happened to call it that way.

Tests must include invalid computation input, machine breakdown, satellite occlusion/ambiguous signal, communication delay, contaminated kelp, dosage bounds, withdrawal/recall, and privacy filtering.

### Slice 47.2 — Heritage and legacy records

- **Step 737 — Murals:** model materials, labor, venue/access, participants, consent, attribution, preservation, damage, revision, and provenance. Avoid a single official cultural meaning. **Done when:** a unit test records two conflicting attributions/interpretations for the same mural and asserts both are retrievable (no single "canonical" field overwrites the other), matching the "avoid a single official cultural meaning" goal as an enforced data shape, not prose.
- **Step 744 — Memoirs:** provide private/public scope, author consent, subject privacy, draft/revision, contradictory accounts, withdrawal, custody, and accessibility. Memoirs must not automatically become official history. **Done when:** a unit test withdraws a published memoir's consent and asserts its content becomes inaccessible through public read APIs while the record itself is preserved (not deleted) for custody/audit purposes, and asserts no automatic write-through from a memoir into `EpilogueMatrixRuntime` or any other "official" record exists (grep-verifiable: no call from the memoir feature into endgame-authority types).
- **Step 749 — Documentary photography:** model camera/media/lighting resources, subject consent, privacy, metadata, custody, selection, correction, and publication. Do not create surveillance or an all-seeing map. **Done when:** a unit test asserts photograph records require an explicit subject-consent field to be set before the record can be marked "published" (unpublished/no-consent photos remain in a restricted-access state), and — mirroring Step 747's test — asserts no method signature in this feature accepts a live-location or tracking-style query across multiple photos to reconstruct someone's movement.
- **Step 750 — Vision statement:** model contributors, consent, revisions, publication, disagreement, and source links. It is a civic/cultural record, not a hidden ideology score. **Done when:** a unit test asserts there is no numeric "ideology" or "alignment" score derived from vision statements that any other system (quests, factions, endings) reads — grep-verifiable: no cross-reference from vision-statement data into faction/quest/ending logic.
- **Step 752 — Philosophical legacy:** aggregate provenance-backed teaching/records/interpretations with plurality, access, and uncertainty. It must feed existing legacy/endgame evidence only through a read model. **Done when:** a unit test asserts calling the philosophical-legacy aggregation twice with no underlying data changes produces identical output (idempotent read model) and triggers no writes to `EpilogueMatrixRuntime`'s `EpilogueEvaluationContext` or any other endgame state.

Tests cover private records, consent withdrawal after publication, correction, conflicting accounts, missing attribution, inaccessible media, deterministic archive ordering, and no mutation during report generation.

### Slice 47.3 — Safe travel and infrastructure review

- **Step 739 — Mountaineering:** use existing route, weather, equipment, medical, rescue, and shelter contracts. Model route risk, visibility, load, training, fatigue, equipment wear, weather closure, rescue capacity, abort, and partial success. Do not guarantee discovery or treat injury as entertainment. **Done when:** a unit test runs a climb under a weather-closure condition and asserts the climb is aborted with a defined partial-progress result (not a binary success/fail), and an injury test asserts injury applies through the existing medical/health system rather than a bespoke injury stat.
- **Step 738 — Longbow towers:** require a safety design review before implementation. The allowed scope is shelter/perimeter/visibility infrastructure or historical observation: location, structural condition, safe storage, access control, maintenance, training/consent, line-of-sight information at a neutral level, weather, and misidentification. No weapon progression, attack resolution, enemy targeting, or glorified violence. **Done when:** the safety design review document (per Section 7 below) is written and explicitly approved *before* any code is merged — this must be a hard gate, not a parallel task — and once implemented, a unit test asserts no method on this feature's API resolves an "attack," accepts an enemy/target parameter, or returns a damage/combat-outcome value. If the review concludes the intended mechanic cannot be built without those elements, the feature must be rejected or recast per Section 7's "alternate design" requirement — the plan must record which outcome occurred, not silently drop the step.
- **Step 743 — Hornwork:** use the same bounded infrastructure/heritage contract for earthwork, shelter, route control, maintenance, safe access, and historical interpretation. It may expose cover, route capacity, or observation quality, not combat advantage or enemy positions. **Done when:** same safety-review gate and same no-combat-API test as Step 738, applied to this feature's method signatures independently (do not assume passing Step 738's test suite also covers this feature — they are separate classes and need separate assertions).

The two infrastructure features must be disabled pending explicit safety acceptance if their proposed behavior requires targeting or violence. Tests cover unsafe structure, missing maintenance, misidentification, restricted access, weather, consent/training, nonviolent failure, and safe teardown.

**Ordering note (correction):** the original plan lists Mountaineering (739) before Longbow towers (738) and Hornwork (743) within the slice, but gives no explicit reason for that order, and the two infrastructure steps are the only ones in this entire batch requiring a pre-implementation safety review gate. Recommend resolving the Step 738/743 safety review *first*, in parallel with but not blocking Step 739's implementation, so the batch doesn't stall entirely if the review concludes a design must be rejected/recast — Mountaineering has no such dependency and can proceed independently.

## 6. Likely implementation surfaces

- `Assets/Ashfall.Core/` — industrial process adapters, finite computation, ecology/health links, legacy/provenance records, mountaineering, and bounded infrastructure.
- Existing inventory/economy/power/food/ecology/observation/medical systems — reuse ports and state owners rather than copying them.
- `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs` — consume legacy evidence only through existing endgame APIs; do not add an ending writer.
- `Assets/StreamingAssets/Data/` — material/process, food, ecology, observation, legacy, accessibility, safety, and infrastructure definitions with schema versions.
- `src/Host/`, `src/Main.cs`, and host self-test surfaces — thin wiring, read models, registration, save/restore, and safety rejection paths.
- `Ashfall.Core.Tests/` — industrial conservation, provenance/privacy, observation, mountaineering safety, infrastructure boundaries, migration, checksum, and deterministic replay.

## 7. Safety and failure gates

Before any longbow/hornwork implementation is accepted, the plan must document: intended nonviolent player value, safe storage/access, maintenance and structural failure, consent/training, accidental harm/misidentification, privacy boundaries, no-targeting behavior, and an alternate design if the mechanic cannot satisfy those conditions. The test suite must assert that no command exposes enemy/person tracking or performs attack resolution. **Done when (tightened from prose-only original):** this review document exists as a committed artifact (not just a discussion) before Step 738/743 code is merged, and the no-targeting assertion is a compiled test that inspects the feature's public method signatures/parameter types for enemy/target/damage-shaped parameters, not merely a manual code-review checklist item.

All industrial and legacy features must cover interruption, outage, maintenance, contamination, spoilage, privacy, withdrawal, correction, ambiguous evidence, and partial completion. Derived reports must be read-only and idempotent.

## 8. Required verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

All five commands/verbs confirmed runnable against the current repository (same verification as Batch 44: `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` and `Ashfall.csproj` exist; `--data-integrity-selftest`/`--bridge-selftest` dispatched from `src/Host/HostCli.cs`). As with Batch 44, `--bridge-selftest` is a stable no-op post-migration verb and its passing does not validate any of this batch's new systems.

Add focused headless tests for industrial process replay, contaminated kelp/iodine, satellite privacy, legacy consent, mountaineering abort/rescue, and defense-adjacent safety rejection. Run cross-host save tests for partial furnace work and withdrawn memoir/photo records. A different tool/agent must review any implementation containing multiple coupled industrial or infrastructure variables.

## 9. Batch acceptance checklist

- [ ] Steps 737–752 are all represented and sequenced by dependency (see corrected ordering note in Slice 47.3: Step 738/743's safety review should start before or alongside 739, not strictly after).
- [ ] Pig iron, seed oil, spinning, porcelain, and charcuterie conserve inputs and expose quality, maintenance, waste, contamination, and spoilage, each verified by the specific Done-when test named in Slice 47.0 above (not just a design-intent checkbox).
- [ ] Difference engine is finite and bounded (verified by a rejection test for out-of-range input, not just documented as a constraint); kelp/iodine uses calibrated ecology/health data; satellite tracking cannot track people (verified by a method-signature-level test, per Slice 47.1's Step 747 Done-when).
- [ ] Murals, memoirs, documentary photography, vision statement, and philosophical legacy are consented, privacy-aware, revisable provenance records.
- [ ] Mountaineering uses travel/weather/medical/rescue contracts with abort and partial success.
- [ ] Longbow towers and hornwork are nonviolent, safety-reviewed infrastructure or are explicitly rejected/recast — the safety review document itself must exist as a committed artifact, and its outcome (approved / rejected / recast) must be recorded, not silently assumed.
- [ ] Every new data ID's prefix is checked against `CatalogIntegrityValidator.cs`'s actual `IdPrefixes` array (not AGENTS.md's prose summary — see Section 2 finding).
- [ ] Existing `EpilogueMatrixRuntime` remains the only endgame authority.
- [ ] State, data, save, checksum, determinism, event, host, and Godot gates pass or failures are recorded.

## 10. Handoff to Batch 48

Batch 48 may begin when the industrial process contract, legacy/provenance records, observation/privacy rules, ecology/health links, safe travel, and approved infrastructure boundaries are stable. Batch 48 should use them to build water, welfare, resilience, culture, and the final derived endgame presentation; it must not add a new campaign or ending system.

## Review Notes (Corrected)

This file was adversarially reviewed against the actual repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Like Batch 44, this batch describes entirely prospective/unimplemented work — none of the 16 named features (pig iron, seed oil, spinning, porcelain, charcuterie, difference engine, kelp/iodine, satellite tracking, murals, memoirs, documentary photography, vision statement, philosophical legacy, mountaineering, longbow towers, hornwork) exist anywhere in the current codebase, confirmed by repository-wide search. Verification here focused on (a) confirming the systems this plan says to reuse actually exist with the claimed shape, and (b) attacking testability, risk coverage, and ordering, since there is no existing implementation to check for bugs.

1. **Confirmed accurate:** `EpilogueMatrixRuntime` (`Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`, `public sealed class EpilogueMatrixRuntime`, with `EpilogueEvaluationContext` DTO and `RegionalFate`/`DemographicOutcome`/`MoralStanding` enums) exists and matches the "future endgame authority" role this plan assigns it. Added its exact class/type names inline in Section 2.

2. **Unverifiable claims flagged, not silently trusted.** The original plan states as fact that "Batch 46 supplies a reusable finite machinery contract and ecology base" and "Batch 43 supplies provenance, correction, consent, privacy, and calibrated observation." Neither Batch 46 nor Batch 43's plan files were in scope for this review, so these claims could not be verified against actual merged code. Rather than silently accepting them (as the original plan does) or silently rejecting them, added explicit caveats in Section 2 and inline in Slice 47.0 (Steps 746, 751) instructing implementers to confirm these dependencies exist with concrete type names before relying on them.

3. **Same ID-prefix divergence risk as Batch 44 (new finding).** Verified via the same `CatalogIntegrityValidator.cs` read performed for Batch 44: `expansion_`, `skill_`, `ending_`, `article_`, `sector_` are in AGENTS.md's prose "known prefixes" list but not in the actual enforced array. This batch introduces at least 16 new feature areas' worth of data IDs, so the risk of a prefix mismatch here is at least as high as in Batch 44. Added the same finding and checklist item.

4. **Zero testable Done-when criteria (same core issue as Batch 44).** Every one of the 16 steps in Sections 5.0–5.3 was scope prose with no acceptance test. This is a more serious gap here than in Batch 44 because two of the steps (738, 743 — longbow towers, hornwork) are explicit safety-boundary features where "no targeting/tracking" was stated only as a design constraint, not as a compiled, checkable assertion. Added a concrete Done-when test to all 16 steps, and for the two safety-boundary steps (738, 743) plus the two person-tracking-adjacent steps (747 satellite, 749 documentary photography), specified that the test must inspect method *signatures* for disallowed parameter shapes, not just behavior under the tests someone happened to write — signature-level prevention is a stronger guarantee than behavioral testing alone for a safety boundary like this.

5. **Ordering gap — safety review sequencing not addressed.** The original plan places Mountaineering (739) first in Slice 47.3's step list with Longbow towers (738) and Hornwork (743) after, but gives no rationale, and Section 7's safety-review requirement for 738/743 is stated separately from the delivery sequence with no explicit link to when it should happen relative to 739. Added an explicit ordering note recommending the safety review for 738/743 start in parallel with (not strictly gated behind) Mountaineering's implementation, since Mountaineering has no dependency on the infrastructure-safety outcome and delaying it unnecessarily would stall the whole batch if the review takes time.

6. **Missing explicit outcome-recording requirement for the safety review.** The original plan says longbow/hornwork "must be disabled pending explicit safety acceptance if their proposed behavior requires targeting or violence" but never says what happens if the review concludes rejection is necessary — is the step dropped from the batch, deferred, or recast? Per this project's own rule ("dropping a requested feature or requirement is a last resort... explain the deviation and confirm before proceeding"), added an explicit requirement that the review's outcome (approved / rejected / recast) be recorded as a committed artifact, and that the batch acceptance checklist reflect whichever outcome actually occurred rather than assuming approval.

7. **Scope-creep flag — Slice 47.0 bundles five industrial chains**, mirroring the same issue found in Batch 44's Slice 44.2. Added the same recommendation to split into separate implementation commits.

8. **Verification section — confirmed all 5 commands/verbs are real and runnable**, and added the same `--bridge-selftest` no-op caution as in Batch 44's Review Notes, since both files share the identical verification block and the same false-confidence risk.
