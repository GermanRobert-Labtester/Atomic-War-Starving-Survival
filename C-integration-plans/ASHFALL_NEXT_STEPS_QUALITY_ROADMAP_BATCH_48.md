# Ashfall Next Steps Quality Roadmap — Batch 48

**Plan file:** `/home/robertsrff/Desktop/luna)plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_48.md`<br>
**Source inventory:** `/home/robertsrff/Desktop/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_48.md`<br>
**Scope:** Steps 753–768<br>
**Prerequisite:** Batches 43–47, especially water/soil/observation, ecology, accessibility, welfare read models, finite resources/machinery, provenance/legacy, safe travel, and the existing endgame authority.

## 1. Purpose and exit condition

Batch 48 is the synthesis batch. It should make resilience visible through water security, ecological stewardship, mutual welfare, practical craft, safe transport, culture, and bounded science. It must not become a second campaign, score, save, or ending system. The final "Second Renaissance" feature is a derived epilogue/read-model extension over existing evidence and `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`.

The exit condition is a settlement whose water, land, care, food, transport, culture, and utility systems can be interrupted, maintained, audited, and saved without data loss, and whose final presentation explains evidence and uncertainty rather than declaring a hidden moral winner.

**Blocking dependency risk (verified, not previously called out):** `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs` exists today (149 lines, confirmed by direct read) but its actual shape does **not** match what this batch assumes. The real class has three `Evaluate*` methods (`EvaluateRegionalFate`, `EvaluateDemographics`, `EvaluateMoralStanding`) that each take a single flat `EpilogueEvaluationContext` DTO (8 boolean/int fields: `totalDaysSurvived`, `livingDwellerCount`, `totalDeathsRecorded`, `grandTreatySigned`, `tempestDecommissioned`, `debtLedgersBurned`, `childrenSurvived`, `velSecretExposed`) and a `GenerateEpilogueNarrative` method that switches on the three enum results to concatenate hardcoded prose blocks. There is **no evidence/criteria registration hook, no confidence/timestamp/missing-data concept, and no extensibility mechanism of any kind** in the class as it exists today — every enum branch and every prose paragraph is a hardcoded `switch` case. Section 4.4 and Step 768 below describe extending this class with "evidence/criteria hooks... timestamps, confidence, missing-data handling" as if such a mechanism already exists to extend; it does not. Step 768 is therefore not an extension of an existing extensibility contract — it requires **designing that extensibility contract from scratch** inside (or alongside) `EpilogueMatrixRuntime`, which is a materially larger task than "register new evidence through its existing contract" implies. This is corrected in Section 4.4 and Slice 48.4 below.

Additionally, this batch's stated Prerequisite is "Batches 43–47," which, as of this review, exist only as plan documents — none of the systems this batch depends on (water/soil/observation, ecology, accessibility, welfare read models, finite resources/machinery, provenance/legacy, safe travel) exist in the repository yet. Every "reuse Batch 4X's Y system" instruction below is contingent on that prerequisite batch having actually landed in code first, not merely having been planned or reviewed as a document.

## 2. Review findings and constraints

Batches 43–47 establish the records, accessibility, resource flow, calibrated observation, finite ecology/machinery, industrial capacity, and legacy contracts needed here. The project’s known risks remain relevant: `src/Main.cs` requires explicit setup/save/flush registration; save stores must use portable checksummed envelopes; JSON under `Assets/StreamingAssets/Data/` is authoritative; and the active verification path is `dotnet` plus `godot --headless`. Existing needs, medical, water, ecology, economy, and endgame authorities must be extended rather than forked.

The tone must remain cold, human, and restrained. Social work, insurance, food, ceremony, poetry, and communal sports should represent consent, limits, and care. The ceremonial sword is a provenance/ritual object, not a weapon progression item. The Second Renaissance is an interpreted, evidence-backed milestone, not an automatic victory trigger.

## 3. Non-goals

- No new ending selector, victory writer, narrative save envelope, morale score, map, ecology, water, insurance, social-work, sport, or crafting authority.
- No free water, food, energy, transport, or cultural output.
- No glorified violence or weapon mechanics for the ceremonial sword.
- No surveillance, real-world country/war/person references, or unbounded physics/energy simulation.
- No Unity-side gameplay changes or nondeterministic simulation state.

## 4. Shared resilience contracts

### 4.1 Water and land stewardship

Use existing calibrated sampling and ecology records. A water source needs location, recharge/season, capacity, quality, contamination, treatment, access, maintenance, and uncertainty. A garden or rewilding system needs habitat, species/functional group, carrying capacity, interventions, labor, water, contamination, recovery, and failure states.

### 4.2 Welfare and mutual aid

Insurance mutual and social work must model membership/consent, coverage or case scope, resources/reserves, claims/referrals, privacy, care capacity, fraud/uncertainty without punitive assumptions, interruption, and closure. They must consume existing needs/health/economy records through explicit adapters and never reveal private records in a public index.

### 4.3 Practical production and culture

Craft, food, transport, and events use the shared resource/process contract: inputs, labor, time, capacity, quality, maintenance, spoilage, safety, accessibility, consent, and provenance. Cultural events create records/read-model effects through the shared culture system, not a new morale ledger.

### 4.4 Endgame evidence

`EpilogueMatrixRuntime` remains the only ending authority. **Correction:** as verified in Section 1, the class today has no evidence/criteria registration mechanism — it is three small enum-classifying methods over one flat context DTO, feeding a hardcoded prose generator. The final feature must *design and add* an evidence/criteria contract (not merely "register through its existing contract," since no such contract exists yet) that:
- Accepts evidence bundles with a source, a timestamp, a confidence value, and an explicit "missing/unknown" representation (the current `EpilogueEvaluationContext` has no field type that can represent "unknown" — every field is a non-nullable `bool`/`int`, so "missing evidence" today silently defaults to `false`/`0`, which is indistinguishable from "confirmed false/zero." This must be fixed as part of designing the new contract, e.g. via nullable fields or an explicit per-field confidence/presence flag).
- Produces a derived presentation additively, without modifying the three existing `Evaluate*` methods' behavior for old saves that don't carry the new evidence (regression risk: today's three methods are pure functions of the flat context; adding new fields to that context or wrapping it must not change the enum outcome for a context that only populates the original 8 fields).
- May not write a competing ending state — corrected framing: since there is no existing save envelope for `EpilogueEvaluationContext` today (it appears to be constructed fresh per evaluation, not persisted), "may not create a new save envelope" (Slice 48.4 below) means specifically that the new evidence contract must not introduce its own independent save file; if evidence needs to persist across a session, it must be captured through the systems that already own that data (water, ecology, welfare, etc.) via their own `CaptureState`, not duplicated into a new endgame-specific store.

## 5. Delivery sequence

### Slice 48.0 — Water, land, welfare, and shared infrastructure

- **Step 754 — Well/spring caps:** model source location, recharge/capacity, cap construction/material, access, maintenance, water quality, contamination, seasonal failure, and treatment. Reuse Batch 43 sampling and the existing water authority; a cap does not create water or guarantee purity.
- **Step 755 — Botanical garden:** use Batch 46's ecology base and Batch 43/45 observation records. Model species/seed provenance, habitat, water, labor, season, contamination, carrying capacity, accessibility, preservation, and failed/partial growth.
- **Step 756 — Insurance mutual:** model voluntary membership, coverage/claim categories, contributions/reserves, evidence, waiting/processing state, privacy, capacity, default/shortfall, appeal, and closure. It must not become a hidden wealth or wellbeing score.
- **Step 759 — Social work:** integrate with existing needs, health, civic, accessibility, and communication records. Model consent, case scope, worker capacity, referral, privacy, refusal, safety, follow-up, and unresolved/partial care. Do not create a second medical or quest system.
- **Step 761 — Rewilding:** extend the reusable ecology state from Batch 46, linking land/water observations, interventions, habitat limits, contamination, labor, season, recovery, and uncertainty. It must be distinct from a decorative garden and must not grant unlimited resources.
- **Step 758 — Tidal mill:** reuse PowerGrid/resource/machinery contracts — **confirmed real:** `PowerGridSystem` exists at `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`. Model tide window, generation/capacity, mechanical wear, maintenance, flooding, storage, outages, safety, and byproducts. It is a finite utility, not an infinite energy source.

**Scope-creep flag:** six independently substantial systems (water caps, garden, insurance, social work, rewilding, tidal mill) in a single slice with no per-step acceptance criteria, and — unlike Batch 45's science slice — no shared registry contract binding them; the only unifying element is "resilience/welfare theme." Recommend treating this as six separately reviewable deliverables even though grouped as one slice, and sequencing Step 758 (tidal mill) first since `PowerGridSystem` is the one dependency in this slice confirmed to already exist in code — the other five depend on unimplemented Batch 43/46 ecology/observation systems per the Section 1 blocking risk.

**Done when (concrete, per step):**
- 754: a cap's water-quality read model reflects the linked sample's actual contamination/staleness state — a cap over a stale or contaminated sample must not report "safe" — enforced by a test with a deliberately stale sample.
- 755/761: the ecology-carrying-capacity limit is enforced by a test that attempts to exceed it and asserts growth/recovery is capped, not silently unbounded; a test asserts rewilding and the botanical garden use the *same* underlying ecology state type (not two independently duplicated ecology models, since both claim to extend "Batch 46's ecology base").
- 756: a claim against an exhausted reserve is rejected with an explicit shortfall result, not silently paid from nowhere; a test asserts a non-member's case is inaccessible (privacy boundary) via the read model, not just via convention.
- 759: a refused case is represented as a distinct terminal state (not indistinguishable from "not yet started"), and a private case's details are not present in any read model reachable without the case-owner's access rights — tested by attempting to read it from an unrelated read model and asserting absence.
- 758: a tidal outage (tide window closed) test asserts zero generation during the outage window, not a fallback default output.

**Tests should cover** recharge failure, contaminated source, cap maintenance, ecological carrying capacity, insurance reserve exhaustion, private case access, social-work refusal, rewilding failure, tide outage, and deterministic recovery — as distinct test methods per affected step, not one combined narrative test.

**Risk:** Medium. Welfare/privacy systems (756, 759) handle simulated sensitive-case data; a privacy leak between read models — even in a fictional game-state sense — is the kind of defect this project's "never reveal private records in a public index" rule (Section 4.2) exists to prevent. Treat any privacy-boundary test failure in 756/759 as a blocking defect, not a follow-up.

### Slice 48.1 — Safe transport and practical craft

- **Step 757 — Birch canoes:** model material/provenance, construction, hull condition, capacity, sealing, labor, weather, route risk, repair, cargo, and abort/rescue. Reuse Batch 45 shipwright and Batch 44 transport rules; do not duplicate maritime state.
- **Step 762 — Rowing:** extend the safe contest/travel capability with crew consent, equipment, water/weather, fatigue, route, rescue, accessibility, and nonparticipation. It must not be a combat or forced-labor system.
- **Step 763 — Smoked fish:** use fish/ecology/harvest limits, water quality, fuel/smoke, labor, time, contamination, spoilage, storage, welfare, and byproducts. Reuse charcuterie/food rules from Batch 47.
- **Step 760 — Tinderboxes:** model material, assembly, fuel/ignition, durability, maintenance, storage, fire risk, and accessibility. Outputs are finite and must interact with shelter/camp safety.
- **Step 765 — Candles/batik:** represent both candle and dye/batik processes using shared material, fuel, water, labor, quality, contamination, fire, storage, and provenance rules. If the source inventory treats them as one step, keep one feature boundary with two explicit process recipes rather than a free-form crafting system.

**Scope-creep flag:** same pattern as Slice 48.0 — five substantial features in one slice. Step 757 (canoes) explicitly depends on Batch 45's shipwright work (Step 706 in the Batch 45 plan reviewed alongside this one), which is itself not yet implemented — this is a plan-depends-on-plan chain two levels deep (48 depends on 45 depends on 43/44), each unverified in code. Do not start Step 757 until Batch 45 Step 706 has landed and its hull/seal/maintenance types are confirmed to exist with the exact names this step expects to reuse.

**Done when (concrete, per step):**
- 757: an abort/rescue test asserts a canoe trip in a hazard condition beyond a defined route-risk threshold is blocked or forced into rescue state, not silently completed.
- 762: a nonparticipation test asserts a crew member who withdraws consent is removed from the active contest without corrupting the remaining participants' state.
- 763: a contamination test asserts fish from a contaminated water source cannot produce a "safe" smoked-fish record — mirroring the shellfish safety test pattern from Batch 45 Step 719 for consistency across the two similar features.
- 760: a fire-risk test asserts a tinderbox past its durability/maintenance threshold either fails to ignite or raises an explicit fire-risk flag consumed by shelter/camp safety, not silently succeeding.
- 765: both the candle and batik recipes are covered by independent conservation tests (material in vs. material out), and a test confirms they share the underlying process type rather than being copy-pasted into two parallel implementations — "one feature boundary with two explicit process recipes," as this step's own text requires, should be enforced by a shared base type or shared recipe contract, checkable in code review.

**Tests cover** hull damage, weather abort, capacity, fire risk, contaminated fish, spoilage, dye/water waste, partial craft work, and save/restore of maintenance state — as distinct test methods per feature.

**Risk:** Medium for 757/762 (physical hazard/rescue states, similar water-safety risk profile to Batch 45's maritime work); Low for 760/763/765 (isolated crafting).

### Slice 48.2 — Culture, access, and participation

- **Step 753 — Poetry slam:** use the shared cultural participation record with venue, contributors, consent, accessibility, time/labor, privacy, moderation/safety, and provenance. Do not assign a universal artistic score or force publication.
- **Step 766 — Morris/ceilidh:** use the same venue/event/culture contract with instruments, participation, accessibility, consent, fatigue, injury boundaries, and cultural attribution. It must not create a second sport or morale system.

Both events must support no venue, no accessible facilitator, refusal, partial attendance, correction, and private/public record scope. Their effects should be transparent read-model contributions to existing culture/wellbeing evidence.

**Done when (concrete):** a test asserts a poetry-slam submission has no numeric "artistic quality" field feeding any wellbeing/score system (only participation/attendance/accessibility data) — this directly enforces the "do not assign a universal artistic score" non-goal, which otherwise has no verification mechanism; a Morris/ceilidh injury-boundary test mirrors Batch 45 Step 712's pattern (attempt to exceed the boundary, assert refusal/stop); both events' "no accessible facilitator" path returns an explicit reason string via the accessibility capability from Batch 45 Step 705 (if implemented) rather than silently hiding the event from the read model.

**Risk:** Low. Two structurally similar, low-hazard culture events; main risk is the "no score" non-goal being silently violated by a later batch that wires these into a wellbeing metric without re-reading this constraint — the test above exists specifically to catch that.

### Slice 48.3 — Bounded science and ritual artifact

- **Step 767 — Van de Graaff:** use the finite machinery/education contract. Model materials, power, insulation, charge/safety state, maintenance, capacity, demonstration duration, failure, and accessibility. It is a bounded educational/scientific device, not an infinite power source or weapon.
- **Step 764 — Ceremonial sword:** implement only as a nonviolent ritual/provenance artifact. Model custody, consent, storage, condition, attribution, ceremony, privacy, and safety. It must not provide attack stats, combat progression, intimidation bonuses, or a weapon economy. If a proposed design needs those effects, reject it and preserve the artifact/ritual interpretation.

**Content-safety note (elevated, matching the same concern raised for Batch 45 Step 712):** Step 764 is the highest content-tone risk in this entire batch — a "ceremonial sword" is one implementation decision away from becoming exactly the "glorified violence" / "weapon progression item" this project's tone rules forbid (Section 3 non-goals explicitly call this out twice). This needs a mandatory human tone-and-data-shape review before merge, not just automated tests: specifically, a reviewer must confirm the shipped data schema for this item has **no** field resembling `damage`, `attack`, `intimidation_bonus`, or similar, by direct inspection of the JSON schema — not by trusting the implementer's description.

**Done when (concrete):**
- 767: a demonstration-duration test asserts the device stops generating charge after its defined duration/capacity limit, and a safety-shutdown test asserts an over-capacity attempt is blocked rather than producing unbounded output; an accessibility test asserts an inaccessible demonstration has a stated alternative or reason.
- 764: a schema-inspection check (automatable: grep the item's JSON definition and its C# DTO for combat-related field names) confirms zero attack/combat/intimidation fields exist; a consent-withdrawal test asserts custody can be revoked without corrupting the artifact's condition/attribution history.

Tests must assert finite energy, safe shutdown, maintenance failure, inaccessible demonstration alternatives, artifact custody, consent withdrawal, and absence of combat fields/effects — the last of these ("absence of combat fields") should be a concrete grep-based or reflection-based test, not a manual claim, given how easy this constraint is to accidentally violate in a later edit.

**Risk:** Low for 767 (isolated finite-machinery pattern, similar to `PowerGridSystem`'s existing finite-resource model). Medium-High for 764 specifically due to the content-tone risk above — flag this step for the mandatory human review described.

### Slice 48.4 — Second Renaissance, last

- **Step 768 — Second Renaissance:** extend `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs` with evidence/criteria hooks that reference existing water, ecology, education, welfare, industry, culture, accessibility, and provenance read models. Define criteria as transparent evidence bundles with timestamps, confidence, missing-data handling, and correction behavior.

**Corrected scope (per Section 1's finding):** this step is not adding a feature on top of an existing extensibility contract — it is *building* the evidence/criteria contract that Section 4.4 describes, from scratch, against a class whose current three `Evaluate*` methods are hardcoded switch statements over 8 non-nullable primitive fields with no representation for "unknown/missing." Concretely, this step must:
1. Decide how new evidence coexists with the existing `EpilogueEvaluationContext` (extend it with nullable/optional fields, or wrap it in a new outer type that also carries the original context) — and prove the choice doesn't change `EvaluateRegionalFate`/`EvaluateDemographics`/`EvaluateMoralStanding`'s output for any context built the old way (a regression test using only the original 8 fields, comparing output before and after this step's changes, byte-for-byte).
2. Add the actual evidence-bundle type (source, timestamp, confidence, presence/absence) — this type does not exist anywhere in the codebase today under any name; do not assume one can be found and reused.
3. Add a new presentation path (the "Second Renaissance" narrative) that consumes the new evidence type without modifying `GenerateEpilogueNarrative`'s existing three switch statements, so old-save/no-new-evidence playthroughs keep producing byte-identical prose to today's output.

The presentation may say that a particular pattern of recovery is supported, uncertain, or incomplete. It must never select a player's moral worth, overwrite source records, or create a new save envelope. Existing endings must remain stable when the new evidence is absent.

**Done when (concrete):**
- A regression test proves `EvaluateRegionalFate`, `EvaluateDemographics`, and `EvaluateMoralStanding` produce identical enum results for a context populated only with the 8 original fields, run before and after this step's changes.
- A test proves `GenerateEpilogueNarrative`'s output string is byte-identical for a no-new-evidence context, before and after this step's changes (the concrete form of "existing endings must remain stable when the new evidence is absent").
- A test proves the new presentation path performs zero writes to any source system's state (read-only) — e.g. by asserting no `CaptureState` on any consumed system changes its returned value across two calls to the new presentation logic in sequence.
- A test proves a missing/absent evidence bundle produces an explicit "incomplete" presentation state, not a silent default value indistinguishable from "confirmed absent" (this directly targets the non-nullable-primitive gap identified in Section 1).
- A test proves no new save file/envelope is created by this step (grep or file-system check in the test for any new `*SaveStore` class or new save-file constant introduced) — enforcing "must never create a new save envelope" as a checkable fact, not a promise.

Test older saves, missing evidence, contradictory evidence, corrected evidence, privacy-filtered evidence, deterministic ordering, and no state mutation during presentation — each as its own test method matching one of the concrete assertions above.

**Risk:** High. This is the capstone step of the entire batch, depends on every other slice's read models existing and being stable, touches the game's ending/epilogue system (maximum narrative-stakes surface), and — per this correction — is a from-scratch design task rather than the "extend the existing hook" task the original document implied, meaning it is likely to take longer and carry more design risk than its single-step listing suggests. Recommend explicitly re-scoping this as its own reviewed mini-design (interface/DTO shape agreed before implementation starts) rather than treating it as a same-size unit of work as the other 15 steps in this batch.

## 6. Likely implementation surfaces

- `Assets/Ashfall.Core/` — water/source adapters, ecology extension, mutual aid/social work adapters, tidal utility, transport/craft, culture events, finite science, ritual artifact, and endgame evidence hooks.
- Existing `PowerGridSystem`, food/material/economy, maritime/transport, needs/medical, accessibility, ecology, provenance, and event systems — extend through ports.
- `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs` — only endgame integration authority.
- `Assets/StreamingAssets/Data/` — source, habitat, species, recipes, materials, events, culture, artifact, and endgame evidence definitions with `schema_version`.
- `src/Host/`, `src/Main.cs`, and `src/Host/HostCli*.cs` — thin wiring, read models, setup/save/restore/flush, and headless smoke paths.
- `Ashfall.Core.Tests/` — resilience, water/ecology, welfare privacy, transport safety, food conservation, culture access, machine bounds, artifact safety, and endgame compatibility.

## 7. Cross-feature failure matrix

Every feature must cover: missing or stale observation, contamination, capacity/recharge limits, season/weather/tide outage, maintenance, staffing/labor shortage, accessibility barrier, consent refusal/withdrawal, privacy filtering, interruption/partial progress, spoilage/fire/safety risk, rescue/abort, correction, and old-save migration. Derived reports and the Second Renaissance presentation must be idempotent and read-only.

## 8. Required verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

**Runnability correction:** the five commands above are the project's real, confirmed canonical gate. The "focused headless tests" named below it are not yet concrete CLI verbs or test filters — as with Batch 45's equivalent section, each slice's Done-when criteria above should produce the actual filter/verb name, and this list should be updated to match once implementation starts rather than left as an aspirational description:

- `dotnet test --filter "WellSpring|BotanicalGarden|InsuranceMutual|SocialWork|Rewilding|TidalMill"` (Slice 48.0)
- `dotnet test --filter "BirchCanoe|Rowing|SmokedFish|Tinderbox|CandleBatik"` (Slice 48.1)
- `dotnet test --filter "PoetrySlam|MorrisCeilidh"` (Slice 48.2)
- `dotnet test --filter "VanDeGraaff|CeremonialSword"` (Slice 48.3)
- `dotnet test --filter "SecondRenaissance|EpilogueMatrix"` (Slice 48.4)

(Placeholder names in PascalCase matching this document's step names — replace with actual test class names at implementation time.)

Run a migrated-save smoke path and prove the final presentation does not change the save checksum — concretely: capture a `SaveChecksum` before invoking the Step 768 presentation logic and again after, and assert equality; this is the direct test for "no state mutation during presentation" from Slice 48.4's Done-when criteria, not a separate manual check. Require independent review of coupled water/ecology/welfare/endgame changes — per this project's Cross-Tool QA Rule, this specifically means Steps 754/755/756/759/761 (all touching the shared, still-hypothetical ecology/welfare state) and Step 768 (touching endgame evidence with ≥2 coupled new variables: confidence × missing-data handling) must each be implemented by one tool/session and reviewed by a different one — state this as a hard requirement per those specific steps, not a general aspiration.

## 9. Batch acceptance checklist

- [ ] Steps 753–768 are all represented and sequenced by shared resilience contracts — verified per-slice against the per-step Done-when criteria above, not narrative agreement alone.
- [ ] Well/spring caps, botanical garden, rewilding, and tidal mill use finite water/ecology/utility state — verified by the Slice 48.0 carrying-capacity and outage tests specifically.
- [ ] Insurance mutual and social work enforce consent, privacy, capacity, partial care, and existing needs/health authority — verified by the Slice 48.0 privacy-boundary tests (treat any failure here as blocking, per Slice 48.0's risk note).
- [ ] Canoes, rowing, smoked fish, tinderboxes, and candles/batik conserve inputs and handle safety, weather, maintenance, and spoilage — verified by the per-feature conservation tests in Slice 48.1.
- [ ] Poetry and Morris/ceilidh reuse accessibility, venue, culture, consent, and provenance records — verified by the Slice 48.2 no-score and injury-boundary tests.
- [ ] Van de Graaff is finite, safe, educational, and non-weaponized — verified by the Slice 48.3 demonstration-duration and safety-shutdown tests.
- [ ] Ceremonial sword is a nonviolent ritual/provenance artifact with no combat mechanics — verified by the mandatory human tone review AND the automated schema-inspection test from Slice 48.3, both required, neither sufficient alone.
- [ ] Second Renaissance extends `EpilogueMatrixRuntime` and does not create a competing ending writer or victory selector — verified by the Slice 48.4 regression tests proving old-context/no-new-evidence output is byte-identical before and after this batch's changes.
- [ ] Core, host, save, data, determinism, checksum, Godot, bridge, and focused endgame checks pass or failures are recorded — using actual registered verb/filter names (see Section 8), not the placeholder names listed there.
- [ ] The Section 1 blocking-dependency risk (Batches 43–47 existing only as plans, not code) has been resolved or explicitly waived in writing before any step claims to "reuse" a prerequisite-batch system.

## 10. Final handoff and release review

After Batch 48, run a repository-wide authority audit: one writer per mutable field, no new legacy-host logic, no duplicate save envelopes, no new serializer violations, no unregistered setup/save/flush systems, and no unreviewed privacy or safety surfaces. Re-run the complete canonical gate on a clean checkout and compare current results—not historical report numbers—to the Batch 43 baseline. The release review must include a human-readable list of unresolved uncertainty, inaccessible content, incomplete evidence, and known non-goals rather than hiding them behind a final score.

## Review Notes (Corrected)

This file was adversarially reviewed against the actual repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Findings:

1. **The central factual claim about `EpilogueMatrixRuntime` was wrong.** The document repeatedly describes it as having an existing "evidence/criteria" contract that Step 768 merely "registers through" — read the full 149-line file: it is three small enum-classifying methods (`EvaluateRegionalFate`, `EvaluateDemographics`, `EvaluateMoralStanding`) over one flat 8-field `EpilogueEvaluationContext` DTO, feeding a `GenerateEpilogueNarrative` method built entirely from hardcoded `switch`/prose blocks. There is no evidence-bundle type, no confidence/timestamp concept, and no extensibility hook anywhere in the class. This is corrected in Section 1, Section 4.4, and Slice 48.4, which now treat Step 768 as designing that contract from scratch (with explicit regression tests proving old behavior is unchanged) rather than extending something that already exists.

2. **Unstated blocking dependency risk**, same pattern as Batch 45: this batch's stated prerequisite, Batches 43–47, exist only as plan documents today. No ecology system (`EcologySystem` or equivalent), water-authority system, or accessibility read model was found anywhere in `Assets/Ashfall.Core/` during verification — only `PowerGridSystem` (`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`, referenced by Step 758) was confirmed to actually exist among this batch's dependencies. Added this as an explicit blocking-risk note in Section 1 so every "reuse Batch 4X's Y" instruction is understood as conditional on that batch having actually landed in code.

3. **No Done-when criteria anywhere in the original document**, identical gap to Batch 45 — all 16 steps were prose-only. Added concrete, testable Done-when criteria to every slice (48.0–48.4).

4. **Scope-creep across every slice except 48.4**: Slice 48.0 bundled 6 systems (water caps, garden, insurance, social work, rewilding, tidal mill), Slice 48.1 bundled 5 (canoes, rowing, smoked fish, tinderboxes, candles/batik), with no per-step acceptance criteria and, unlike Batch 45's Slice 45.3, no shared registry contract to justify the grouping. Flagged both slices explicitly and recommended independent review per feature within each slice.

5. **Cross-plan dependency chain identified**: Step 757 (birch canoes) explicitly depends on "Batch 45 shipwright" (Step 706 in the Batch 45 document reviewed alongside this one), which itself depends on unimplemented Batch 43/44 systems — a three-level unverified dependency chain. Added an explicit warning against starting Step 757 before Batch 45 Step 706 has landed with confirmed type names.

6. **Missing content-safety call-outs.** Step 764 (ceremonial sword) and, to a lesser extent, Step 762 (rowing, "not a combat or forced-labor system") sit closest to this project's explicit "no glorified violence" / "no weapon progression" non-goals (stated twice in Section 3). The original document's mitigation was descriptive ("if a proposed design needs those effects, reject it") with no concrete enforcement mechanism. Added a mandatory human tone-and-schema review requirement plus an automatable grep/reflection-based test asserting zero combat-related fields on the ceremonial sword's data shape — this is the same class of gap flagged for Batch 45's Step 712.

7. **Unrunnable "focused headless tests" verification claims**, same defect found in Batch 45 Section 8 — named test categories with no corresponding CLI verb or filter. Added a concrete placeholder `dotnet test --filter` list per slice with instructions to replace placeholders with real names at implementation time, and made the save-checksum-stability claim in Section 8 concrete (capture checksum before/after presentation, assert equality) instead of leaving it as an unverified assertion.

8. **Elevated Step 768's risk rating to High** (was implicitly treated as same-weight as the other 15 steps) given the corrected understanding from finding 1 — this step is now understood to be a from-scratch contract design against the game's ending system, not a same-size feature addition, and the document now recommends treating it as its own reviewed mini-design rather than one line item in Slice 48.4's step list.

9. **Tightened the Section 9 acceptance checklist** to reference specific verification methods per bullet (e.g. "verified by the mandatory human tone review AND the automated schema-inspection test... both required, neither sufficient alone" for the ceremonial sword), consistent with the per-slice Done-when criteria added throughout.
