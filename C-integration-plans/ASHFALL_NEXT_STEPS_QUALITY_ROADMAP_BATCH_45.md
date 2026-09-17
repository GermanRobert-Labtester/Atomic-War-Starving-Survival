# Ashfall Next Steps Quality Roadmap — Batch 45

**Plan file:** `/home/robertsrff/Desktop/luna)plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_45.md`<br>
**Source inventory:** `/home/robertsrff/Desktop/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_45.md`<br>
**Scope:** Steps 705–720<br>
**Prerequisite:** Batches 43–44, especially accessibility/read models, resource accounting, communication, transport, water/soil observations, medical host boundaries, and deterministic save contracts.

## 1. Purpose and exit condition

Batch 45 makes the settlement more accessible, medically safer, maritime-capable, and scientifically calibrated. It begins with sign language because later education, culture, work, social care, and records must be able to represent partial communication and access needs. It then builds maritime maintenance and food safety, integrates a bath house with the current medical surface, and adds observation-heavy science without pretending uncertain measurements are facts.

The exit condition is a reusable accessibility capability, a finite maritime/contamination model, a clinical-care adapter over existing medical systems, and a calibrated observation pipeline that later ecology and endgame features can trust.

## 2. Review findings and constraints

The current medical host surface is `src/Host/MedicalHostSession.cs`, centered on `ChemicalDependencySystem` (`Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`) and `VigilStateMachine` (`Assets/Ashfall.Core/Medical/VigilStateMachine.cs`) — both confirmed to exist at these paths with save DTOs `ChemicalDependencyLedgerState` and `VigilSaveState` respectively; no new general `MedicalSystem` should be introduced. Batch 44's resource and transport work must remain the authority for water, labor, maintenance, and route access. Batch 43's provenance/observation system is the proper owner for rock, meteorite, ice, globe, and territory evidence. The Godot host must remain thin, and every system needs a `CaptureState`/`RestoreState` path plus a `src/Main.cs` registration entry.

**Blocking dependency risk (not previously called out):** as of this review, Batches 43 and 44 exist only as plan documents (`ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_43.md` / `_44.md`) — none of their systems (accessibility/read models, resource accounting, communication records, transport rules, water/soil observation records) exist in the actual codebase yet. This batch's "Prerequisite: Batches 43–44" is therefore a hard blocker on unimplemented plans, not a completed foundation. Before starting any Batch 45 step, confirm Batches 43–44 have actually landed in code (not just been reviewed as documents) — if they haven't, Batch 45 cannot "reuse Batch 43's communication record" or "Batch 44's resource-flow contract" because those don't exist to reuse, and steps described below as "extend X" would in practice mean "build X from scratch," which is a different (larger) task than this document describes.

Accessibility is not a cosmetic checkbox. It must affect communication, teaching, participation, and action availability while preserving consent and avoiding a "deficit" score. Maritime and medical risks must produce legible partial/failure states rather than hidden random penalties.

**Risk callout — medical surface is sensitive.** `ChemicalDependencySystem` and `VigilStateMachine` are both save-bearing, already-shipped clinical systems (Expansion 07 vigil mechanics per the class's own doc comments). Slice 45.2 (Step 708, bath house) explicitly integrates with both. Any change to these two classes' public surface, save DTO shape, or effect application order is a medium-risk change to existing player-facing save compatibility, not a greenfield addition — treat it with the same one-commit-per-behavioral-change discipline called for elsewhere in this project's save-contract work, and add a rollback note to Step 708 specifically (see Slice 45.2 below).

## 3. Non-goals

- No second medical, hygiene, water, map, territory, sport, or message authority.
- No medical diagnosis or professional advice encoded as a real-world claim; this is game-state simulation with bounded fictional rules.
- No free harvest, perfect navigation, or guaranteed scientific discovery.
- No real-world countries, wars, people, surveillance, or glorified violence.
- No direct UI writes into Core state, and no use of Unity legacy files.

## 4. Contracts to lock before feature work

### 4.1 Accessibility capability

Represent communication access as capability and context: preferred mode, available interpreter/teacher, literacy or symbol support where relevant, time/cost, partial understanding, degraded channel, consent, refusal, and withdrawal. The host read model should expose what is accessible now and why an action is unavailable. Data should allow multiple valid modes without declaring one culturally correct.

### 4.2 Calibrated observation

Extend Batch 43 records with instrument/observer calibration, units, detection limits, environmental conditions, sample custody, confidence, uncertainty, failed/ambiguous results, and revision. Every science feature must specify the authoritative unit and conversion. Do not use floating-point display rounding as scientific correctness; store the game value in a documented canonical unit and format it in the host.

### 4.3 Maritime and clinical safety

Maritime systems need hull/seal integrity, capacity, weather, contamination, maintenance, labor, route access, spoilage, and rescue/abort states. Clinical features need assessment, consent, contraindication/interaction, duration, rest, referral, adverse outcome, and no-treatment states routed through existing medical/needs effects.

## 5. Delivery sequence

### Slice 45.0 — Sign language and communication access

- **Step 705 — Sign language:** add data and Core state for signed communication learning/use, interpreter or peer support, teaching time, access, partial message fidelity, fatigue, consent, and degraded communication. Integrate it with Batch 43's communication record and Batch 44's cultural/education read models.
- Add accessible status to all new Batch 45 read models, not just a single panel. An inaccessible lecture, atlas, bath-house instruction, or emergency signal must show an alternative or a reason it cannot be used.

**Done when (concrete):** a new `CaptureState`/`RestoreState` pair exists for the sign-language/communication-access state with a `[Serializable]` DTO, registered in `src/Main.cs`'s setup/save/flush matrix; `dotnet test --filter "SignLanguage"` (or the equivalent test-class filter chosen at implementation time — record the actual filter string here once named) passes with at least the 7 scenarios listed below as distinct test methods, not narrative prose; and every other Slice 45.1–45.5 read model that claims "accessible status" (bath house, atlas, globe, calibrated-science displays) has a corresponding boolean/enum field sourced from this slice's capability model, verified by grep for the accessibility type name across those slices' files.

**Tests (as concrete methods, one assertion focus each):** no-interpreter path returns an explicit "unavailable + reason" result (not silent failure); partial-interpreter path returns a fidelity-degraded result distinct from full/none; consent withdrawal mid-session halts without corrupting state; degraded channel is represented as data, not a random-chance boolean; teaching interruption preserves partial progress across a save/restore cycle; deterministic message interpretation (same seed + same inputs → same output, verified with `ISeededRng`, never `System.Random`); privacy (a signed conversation record is not visible to an unrelated read model); save migration (a save captured before this slice's DTO existed loads without throwing, defaulting to "no accessibility state recorded" rather than crashing).

**Risk:** Low-Medium. This slice is the foundation every later slice's "show accessible status" language depends on; if its capability data shape changes after Slices 45.1+ start consuming it, that is a breaking change to already-written code. Freeze the capability DTO shape before starting Slice 45.1, and if it must change afterward, treat that as its own reviewed step, not a silent edit.

### Slice 45.1 — Maritime maintenance and safe harvest

- **Step 706 — Shipwright caulking:** model hull material, seam condition, caulking inputs, labor/time, tool wear, quality, leak risk, water exposure, maintenance schedule, interruption, and launch/abort conditions. Reuse Batch 44 transport/route capacity; a patched hull is not automatically safe.
- **Step 719 — Shellfish:** model location/season, water-sample evidence, species/harvest limits, contamination, handling, spoilage, labor, and health consequences through existing medical/needs effects. A sample with unknown or outdated calibration must not authorize a clean harvest.

**Ordering note:** Step 719 is listed after Step 706 in this slice despite its number (719) being far outside the 705–712 numeric neighborhood that most of this batch's other steps occupy. If the source inventory's step numbers reflect a specific dependency or authoring order, confirm 719 does not have an undeclared dependency on a step from a later batch before starting it out of numeric sequence; if it has no such dependency, keep this ordering (706 before 719) since caulking's hull-safety state is a reasonable precondition for a harvest expedition, but state that reasoning explicitly rather than leaving the numbering unexplained.

**Done when (concrete):** `dotnet test` includes a `ShipwrightCaulking` test class and a `Shellfish` test class, each with `CaptureState`/`RestoreState` round-trip coverage; a caulked hull with a documented leak-risk value below a defined safety threshold allows launch, and above it blocks launch with an explicit reason string (not a silent false); a shellfish harvest attempt against a water sample whose calibration timestamp exceeds a defined staleness window is rejected with an explicit "stale calibration" result, not silently treated as clean.

**Tests:** Use the water-sampling provenance chain from Batch 43 and the resource-flow contract from Batch 44 — **only if those exist in code already** (see the Section 2 blocking-dependency risk); otherwise this slice must define a local minimal water-quality/calibration record now and note it as provisional pending Batch 43/44 landing. Tests must cover bad seals, weather closure, contaminated water, sample age, spoilage, overharvest, and rescue/abort state — each as a distinct test method with an explicit pass/fail assertion, not a prose description.

**Risk:** Medium. Maritime abort/rescue states and shellfish contamination gating both affect survivor health outcomes (poisoning risk). Add an explicit rollback plan: ship both systems behind data flags that default to "harvest disabled" / "launch disabled" until their respective test suites are green, so a mid-development build cannot silently expose an unfinished safety gate as if it were complete.

### Slice 45.2 — Clinical care adapter

- **Step 708 — Bath house:** provide facilities, clean water, heat/power, staffing, hygiene effects, rest, chemical-dependency interactions, vigil-state interactions, consent, privacy, accessibility, and adverse/empty states. Integrate with `ChemicalDependencySystem`, `VigilStateMachine`, and current medical host wiring instead of creating a second medical model.

The bath house is not a cure-all. It should return a typed treatment/result object listing accepted effects, refused effects, unavailable resources, and risks.

**Done when (concrete):** a `BathHouseSession` (or equivalently named) class exists that takes `ChemicalDependencySystem` and `VigilStateMachine` as constructor dependencies (adapter pattern — no duplicated dependency/vigil logic inside the new class); the typed result object has distinct fields/cases for accepted, refused, and unavailable effects (not a single free-text string); `dotnet test` includes a dedicated test class with the 8 scenarios below as separate methods; and a diff review confirms `ChemicalDependencySystem.cs` and `VigilStateMachine.cs` themselves gained zero new public methods beyond what's needed to query existing state (the adapter reads their existing surface, it does not require them to grow bath-house-specific methods, which would start blurring the "adapter, not second medical model" boundary this step exists to prevent).

**Tests:** no water, no heat, refusal, dependency conflict, illness/contamination, partial session, save/restore, and checksum mutation — 8 distinct test methods.

**Risk — HIGH, elevated from the original document's silence on this point.** This is the one step in Batch 45 that directly touches two already-shipped, save-bearing clinical systems (`ChemicalDependencySystem`, `VigilStateMachine`) rather than adding new isolated state. A mistake here can corrupt existing player saves or change dependency/vigil behavior for players who never interact with a bath house. Rollback plan: implement the bath-house adapter as strictly read + apply-effect calls through each system's existing public API (confirmed real methods: `ChemicalDependencySystem.CaptureState`/`RestoreState`, `VigilStateMachine.CaptureState`/`RestoreState`, plus whatever domain methods those classes already expose — enumerate them before writing the adapter rather than assuming) and add zero new fields to `ChemicalDependencyLedgerState` or `VigilSaveState`. If the bath house needs to persist its own state (e.g. facility condition, staffing), give it its own new DTO and save-store slot rather than extending the two existing DTOs — this keeps a revert of the bath-house feature from requiring a save-format migration of the two upstream systems.

### Slice 45.3 — Calibrated field science

- **Step 707 — Geomancy study:** treat the feature as a culturally situated observation/landscape study. Store hypotheses and confidence; do not make unverifiable claims a hidden truth. Connect any practical consequence only to measured location/soil/water evidence.
- **Step 714 — Rock glacier:** record location, ice/rock observations, melt/season, route hazard, calibration, and uncertainty. Use weather and travel safety contracts.
- **Step 716 — Meteorites:** record search area, specimen custody, classification confidence, contamination, mass/condition, and provenance. Unknown classification remains unknown.
- **Step 718 — Ice cores:** record extraction equipment, depth/units, sample integrity, cold-chain failure, chronology confidence, and interpretation uncertainty.

**Scope-creep flag:** this slice bundles four independent features (707, 714, 716, 718) with only one shared sentence of guidance ("must use one observation registry and explicit units") and no per-step acceptance criteria. Four features sharing one registry is a reasonable design constraint, but "implement all four in one slice" risks the registry's contract being locked in based on only the first feature implemented and retrofitted awkwardly onto the other three. Recommend implementing Step 707 first as the registry's proving ground, writing its Done-when tests, and only then treating 714/716/718 as three separate follow-on tasks that consume the now-stable registry — do not implement all four in parallel against a registry contract that hasn't been exercised by a real feature yet.

**Done when (concrete, per feature):**
- Step 707: a hypothesis/confidence record type exists with at least "unverified," "supported by evidence," and "contradicted by evidence" states; a hypothesis with no linked measured evidence cannot transition out of "unverified" (enforced by a test, not just documentation).
- Step 714: a rock-glacier observation record round-trips through `CaptureState`/`RestoreState`; a route-hazard value derived from a stale (past a defined staleness window) observation is flagged as such in the read model rather than presented as current.
- Step 716: a meteorite specimen with contaminated custody (a documented custody-chain break) cannot reach "classified" confidence regardless of its measured properties — enforced by test.
- Step 718: an ice-core sample that fails cold-chain (a documented temperature-excursion event) has its chronology-confidence value reduced by a deterministic, testable rule, not a random penalty.

**Tests:** no sample, damaged sample, calibration drift, conflicting readings, deterministic sample IDs (via `ISeededRng`, never `Guid.NewGuid()` or `System.Random`, per Invariant 4), custody loss, and correction history — each covered once per feature (7 scenarios × 4 features, or fewer if a scenario is inapplicable to a given feature — record which scenarios were skipped and why rather than silently omitting them).

**Risk:** Medium. The shared-registry design is sound but under-specified; the main risk is the registry contract being designed too narrowly around whichever of the four features is implemented first. Mitigate by writing the registry's own unit tests (independent of any of the four features) before wiring any feature to it.

### Slice 45.4 — Craft, food economy, and material culture

- **Step 711 — Kvass economy:** model grain/water/starter/fuel/labor, batch quality, contamination, fermentation time, spoilage, storage, exchange, and price/ledger effects through the existing economy. Do not create a second market or free beverage output.
- **Step 713 — Carpet:** use fiber/dye/tool/labor inputs, quality, pattern/provenance, repair, and accessibility; record finished work through the shared artifact contract.
- **Step 715 — Needlework:** use thread/cloth/tool/labor, quality, repairability, interruption, and cultural/medical utility where already supported by existing systems.
- **Step 717 — Gourds:** model crop/season/curing, contamination, durability, storage capacity, and craft transformation with byproducts.
- **Step 709 — Niello:** model metal/inlay/heat/skill/tool wear, quality, toxicity/safety, and provenance. It is a material craft extension, not a new economy.

**Scope-creep flag:** five independent crafting features in one slice, same structural issue as Slice 45.3. Unlike 45.3 these five do not obviously share a single registry the way the science steps do — "the existing economy/inventory rules" is a much looser shared contract, since each of grain-fermentation (711), textile-dye (713), thread (715), crop-curing (717), and metal-inlay (709) has materially different inputs and failure modes. Recommend treating each as its own independently reviewable commit even within this slice, rather than one combined "craft, food economy, and material culture" commit — a bug in kvass spoilage math should not block or get tangled with a niello toxicity bug in code review.

**Done when (concrete, per feature):** each of the 5 features has its own `CaptureState`/`RestoreState` pair and its own test class; each feature's material-conservation test asserts that total input mass/quantity consumed equals what the recipe declares (no silent free resources) via an explicit before/after inventory-count assertion; Niello specifically must have a toxicity/safety test asserting an unsafe-process attempt is blocked or flagged (not silently succeeding), since it's the one feature in this slice with an explicit safety claim in its one-line description.

**Tests:** material conservation, failed/partial batches, contamination, price updates from real ledger changes, deterministic quality (via `ISeededRng`), and record provenance — 6 scenarios per feature (30 total test methods across the slice, or fewer where a scenario doesn't apply, documented per feature).

**Risk:** Low-Medium. Individually low risk (isolated crafting systems), but the bundling itself is the risk — five unrelated systems reviewed as one unit increases the chance a real defect in one is missed while reviewing the other four. Mitigate by keeping the five as separate PRs/commits even though they're grouped in one slice here.

### Slice 45.5 — Sport, geography, and territory read models

- **Step 712 — Wrestling/sumo:** define a generic safe contest/training capability with participant consent, venue, ruleset, preparation, fatigue, injury boundaries, accessibility, officiating, and cultural record. Do not glorify violence or create a combat progression system.
- **Step 710 — Globe:** present canonical locations, routes, observations, and uncertainty. It must not own coordinates or duplicate the world map.
- **Step 720 — Territory atlas:** provide a read-only historical/geographic view over canonical location/territory records with revision, privacy, and contested/unknown boundaries. Do not create political "correctness" or an independent territorial authority.

The globe and atlas should consume accessible read models and show stale/unknown data. The contest should persist participation/results through the shared culture/provenance contract.

**Done when (concrete):** Step 712's injury-boundary rule is enforced by a test that attempts to exceed the defined boundary and asserts the contest stops or refuses rather than continuing past it; Steps 710 and 720 have zero write methods in their public API surface (grep-verifiable — a "read-only" claim should be checked by absence of mutator methods, not just documentation) and both explicitly render a "stale" or "unknown" indicator when backing data is missing, verified by a test that supplies deliberately incomplete backing data and asserts the indicator appears rather than a default/fallback value being silently substituted.

**Content-safety note (elevated, not previously called out):** Step 712 (wrestling/sumo) sits closest to this project's "no glorified violence" rule among all of Batch 45's steps. Its Done-when should include an explicit reviewer check — not just automated tests — that the implemented feature reads as consented athletic training/contest rather than combat mechanics (no damage-over-time framing, no "defeat" language implying harm beyond a safe contest's ordinary bounds). Automated tests can verify data shape but not tone; this needs a human read of the implemented copy/text before merge.

**Risk:** Low for 710/720 (pure read models over — hypothetically — already-canonical data, contingent on Batch 43/44 actually existing per the Section 2 blocking risk). Medium for 712 due to the content-tone risk above.

## 6. Likely implementation surfaces

- `Assets/Ashfall.Core/` — accessibility, maritime maintenance/harvest, clinical adapters, calibrated observations, craft/economy, safe contests, and geographic read models.
- `Assets/Ashfall.Core/Medical/` or existing medical namespaces — only extensions that fit current systems; inspect before adding a file.
- `Assets/StreamingAssets/Data/` — signed-language/accessibility, maritime, species/contamination, calibration, craft, contest, location, and territory data with schema versions.
- `src/Host/MedicalHostSession.cs` and related host sessions — wiring/read models only; preserve the existing medical authority.
- `src/Main.cs` — setup/tick/save/restore/flush matrix.
- `src/Host/`, `src/Host/HostCli.cs`, and panel tests — headless smoke commands and accessible UI-state checks.
- `Ashfall.Core.Tests/` — capability, clinical, maritime, measurement, contamination, conservation, migration, checksum, and deterministic replay tests.

## 7. Cross-feature failure matrix

Every feature must explicitly test: no access/interpreter, refusal or withdrawn consent, stale/uncalibrated data, contamination, weather closure, maintenance failure, resource shortage, partial completion, custody loss, ambiguous result, privacy filtering, and save interruption. A measurement cannot silently become a boolean unlock. A medical or harvest action cannot silently bypass water quality. A map cannot silently convert an uncertain observation into fact.

## 8. Required verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

**Runnability correction:** the "focused self-tests" mentioned below are not yet defined as concrete CLI verbs anywhere in this document. `src/Host/HostCli.cs` confirms the pattern for adding one (existing verbs follow `--<domain>-selftest`, e.g. the confirmed `--medical-selftest`, `--expedition-selftest`, `--data-integrity-selftest`). Before claiming "run focused self-tests for X," each slice above must actually register a new `--<name>-selftest` verb in `HostCli.cs` (or an equivalent `dotnet test --filter` target) as part of its own Done-when criteria — a plan cannot verify against a command that doesn't exist yet. The list below is the set of verbs/filters this batch should produce, to be confirmed present before Batch 45 is considered complete:

- `dotnet test --filter "SignLanguage"` (Slice 45.0)
- `dotnet test --filter "ShipwrightCaulking|Shellfish"` (Slice 45.1)
- `dotnet test --filter "BathHouse"` (Slice 45.2)
- `dotnet test --filter "GeomancyStudy|RockGlacier|Meteorite|IceCore"` (Slice 45.3)
- `dotnet test --filter "Kvass|Carpet|Needlework|Gourd|Niello"` (Slice 45.4)
- `dotnet test --filter "Wrestling|Globe|TerritoryAtlas"` (Slice 45.5)

(Exact class/filter names above are placeholders matching this document's step names in PascalCase — replace with the actual test class names chosen at implementation time, and update this list to match rather than leaving it aspirational.)

Test a cross-host save tree for one accessibility state, one maritime partial repair, one contaminated sample, and one clinical refusal — meaning: capture a save containing all four of these states, and confirm the same save loads correctly under both the Godot host path and the plain `dotnet test` in-memory path (per Invariant 3, cross-host save compatibility), not just that it loads once. Have an independent tool review any implementation that introduces multiple coupled safety variables — per this project's Cross-Tool QA Rule, this specifically applies to Step 708 (bath house, ≥2 coupled variables: hygiene effect × dependency state × vigil state) and should be stated as a hard requirement for that step, not a general suggestion.

## 9. Batch acceptance checklist

- [ ] Step 705 establishes reusable communication access and is consumed by later features — verified by grep for the capability type name across Slices 45.1–45.5's files (not just narrative agreement).
- [ ] Steps 706 and 719 share transport, water-quality, contamination, maintenance, and provenance authorities — verified by both referencing the same shared record type, not independently duplicated ones.
- [ ] Step 708 extends current medical systems without a duplicate `MedicalSystem` — verified by the diff-review criterion in Slice 45.2 (zero new public methods added to `ChemicalDependencySystem`/`VigilStateMachine` beyond querying existing state).
- [ ] Steps 707, 714, 716, and 718 share calibrated observations, units, custody, and uncertainty — verified by all four referencing one shared observation-registry type, built and unit-tested independently per the Slice 45.3 ordering note.
- [ ] Steps 709, 711, 713, 715, and 717 conserve materials and use the existing economy/inventory rules — verified per-feature by the material-conservation test described in Slice 45.4, not a single blanket assertion.
- [ ] Steps 710 and 720 are read-only geographic views over canonical world data — verified by absence of mutator methods in their public API (grep-checkable).
- [ ] Step 712 is consented, accessible, bounded training/contest state, not combat progression — verified by both the injury-boundary test and the human tone-read called out in Slice 45.5.
- [ ] All stateful work has migration/checksum/determinism/event coverage and explicit host registration in `src/Main.cs`.
- [ ] Core, Godot, data, bridge, and focused headless checks pass or failures are recorded — using the actual verb/filter names registered per slice (see Section 8), not placeholder names.
- [ ] The Section 2 blocking-dependency risk has been resolved or explicitly waived in writing before any step claims to "reuse" a Batch 43/44 system.

## 10. Handoff to Batch 46

Batch 46 may begin when accessibility capabilities, calibrated observation, maritime resource state, contamination rules, clinical adapters, and a generic safe contest/culture contract are stable. Batch 46 should use these foundations for education, ecology, identity, and finite machinery rather than creating parallel education, map, sport, or ecology authorities.

## Review Notes (Corrected)

This file was adversarially reviewed against the actual repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Findings:

1. **Verified factual claims were accurate but under-cited.** `src/Host/MedicalHostSession.cs`, `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`, and `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` all exist exactly as described, with confirmed save DTOs `ChemicalDependencyLedgerState` and `VigilSaveState`. Added these exact class/DTO names to Section 2 so the implementer doesn't have to re-derive them, and confirmed `--medical-selftest` is a real registered CLI verb in `src/Host/HostCli.cs`.

2. **Unstated blocking dependency risk.** Batches 43 and 44, listed as prerequisites, exist only as plan documents today — none of their systems are implemented in the repository. The original document phrased this batch's dependencies as settled ("Batch 44's resource and transport work must remain the authority...") when in fact there is no such authority in code yet. Added an explicit blocking-risk note to Section 2 so this isn't discovered mid-implementation.

3. **No Done-when criteria anywhere in the original document.** Every one of the 16 steps was described only in prose ("model X, Y, Z..."), with zero testable pass/fail assertions. This is the most significant testability gap found. Added a concrete "Done when" block to every slice (45.0 through 45.5) with specific, checkable assertions (e.g. "a route-hazard value derived from a stale observation is flagged... enforced by a test," not "handle staleness").

4. **Scope-creep in Slices 45.3 and 45.4.** Four independent science features (707/714/716/718) and five independent craft features (709/711/713/715/717) were each bundled into a single slice with one shared paragraph of guidance and no per-feature acceptance criteria — structurally similar to the "196 files in one step" risk pattern this review was specifically asked to check for, just at a smaller scale (4-5 features instead of 196 files). Added explicit scope-creep flags to both slices recommending independent review per feature even though they remain grouped as slices, plus per-feature Done-when criteria.

5. **No risk/rollback notes anywhere in the original document**, despite Step 708 (bath house) directly modifying integration points on two already-shipped, save-bearing clinical systems (`ChemicalDependencySystem`, `VigilStateMachine`). Added an explicit HIGH risk rating and rollback plan to Slice 45.2 (keep the bath house's own state in its own new DTO rather than extending the two upstream save DTOs, so a revert doesn't require migrating existing saves).

6. **Unrunnable verification claims.** Section 8 said to "run focused self-tests for accessible communication, ship repair, shellfish safety..." without naming any actual CLI verb or test filter — none of these exist yet, so the instruction wasn't executable as written. Added a concrete list of `dotnet test --filter` targets per slice and made explicit that each slice's Done-when criteria must include registering the corresponding verb/filter before the batch can be considered verified.

7. **Missing content-safety call-out for Step 712 (wrestling/sumo).** The project's tone rules explicitly forbid glorified violence; this step sits closest to that line of anything in the batch, and the original document's one line ("do not glorify violence") had no enforcement mechanism. Added an explicit human-review requirement (tone cannot be verified by automated tests alone) to Slice 45.5.

8. **Unexplained step-number ordering.** Step 719 appears in Slice 45.1 and Step 712 appears in Slice 45.5 despite both having step numbers well outside the 705–718 range most of the batch occupies, with no explanation for why they're sequenced where they are rather than in numeric order. Added an explicit ordering note to Slice 45.1 requiring confirmation that 719's placement doesn't hide an undeclared dependency, and reasoned through why 706-before-719 makes sense structurally (hull safety as a precondition for a harvest expedition) rather than leaving the numbering unexplained.

9. **Tightened the Section 9 acceptance checklist** from broad-agreement bullets ("Steps 707, 714, 716, and 718 share calibrated observations...") into checklist items with a stated verification method (e.g. "verified by all four referencing one shared observation-registry type"), consistent with the per-slice Done-when criteria added above.
