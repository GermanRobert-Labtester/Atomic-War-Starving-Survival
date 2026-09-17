# ASHFALL Quality Roadmap — Batch 49
## Theme: Main.cs Monolith Decomposition (Phase 1 — Domain Partials)

**Priority:** HIGH (H7)
**Risk:** Low — refactor only, no behavioral changes
**Prerequisite:** None

---

## Rationale

`src/Main.cs` is currently **7,014 lines** (verified by direct line count; AGENTS.md's "6,640" figure is stale) — a single `partial class Main : Control` that orchestrates **38 Setup methods, 30 Save methods (plus `SaveAll`), and 17 Flush methods** across every game domain (verified by grep count against the real file; the previously-cited 31/24/14 counts are stale). No `Main.*.cs` partial files exist yet — this is a from-scratch split, not a continuation of an existing pattern. The file is too large for safe navigation, review, or AI-assisted work. The internal structure is already organized by triads (Setup/Save/Flush per domain), making extraction mechanical, but the exact field names below have been corrected against the real declarations.

---

## Step 1 — Extract Expedition Domain Partial

**Goal:** Move `SetupExpeditions`, `SaveExpeditions`, `FlushExpeditionIfDirty`, and all expedition-related fields/events to `src/Main.Expeditions.cs`.

**Implementation:**
1. Create `src/Main.Expeditions.cs` with `partial class Main`.
2. Move the `_expeditions` (type `ExpeditionHostSession`) and `_expeditionDirty` fields (verified at `src/Main.cs` field-declaration block; the field is named `_expeditions`, **not** `_expeditionHost`).
3. Move `SetupExpeditions()` (line ~3378), `SaveExpeditions()` (line ~3387), `FlushExpeditionIfDirty()` (line ~2083).
4. Move any expedition-specific event handlers wired in the Setup method (e.g. `_expeditions.StateChanged += () => _expeditionDirty = true;`).
5. Ensure `Main.cs` retains the call sites in `ContinueGame()`, `SaveAll()` (line ~6227), `_Process()` (line ~517), and `TickSimDay()` (line ~1643) — they call into the partial seamlessly. Note `FlushExpeditionIfDirty()` is invoked from a central flush dispatcher near line 550, not scattered inline — moving the method leaves that call site in `Main.cs` unchanged since C# partials share one namespace.

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings
- `godot --headless --path . -- --bridge-selftest` — exits 0
- Manual/headless smoke: game boots, expedition panel opens, day advance ticks expeditions (state before/after `TickSimDay` unchanged for a fixed seed)

**Risk / rollback:** Low risk (mechanical move, no logic change), but C# partial classes require every moved field/method to keep the exact same accessibility and no accidental duplicate declaration — a copy-paste that leaves the original in `Main.cs` will fail to compile with a duplicate-member error, which is an easy, safe failure mode. Rollback is a single `git checkout -- src/Main.cs src/Main.Expeditions.cs` (no data/save-format impact since this only reorganizes source files, not DTOs).

**Done when:** `Main.Expeditions.cs` compiles, `dotnet build Ashfall.csproj` reports the same warning count as before the change, a `git diff --stat` shows zero net line change across both files (pure move), and Main.cs is measurably shorter by the moved line count (~40–60 lines for this domain — see corrected Summary table).

---

## Step 2 — Extract Economy Domain Partial

**Goal:** Move `SetupEconomy`, `SaveEconomy`, `SetupCaravans`, `SaveCaravans`, `FlushCaravanIfDirty`, and economy/trade fields to `src/Main.Economy.cs`.

**Implementation:**
1. Create `src/Main.Economy.cs` with `partial class Main`.
2. Move `_economy` (type `EconomyHostSession`), `_caravans` (type `TravelingCaravanHostSession`), `_economyDirty`, `_caravansDirty` fields (verified names — the plan's `_economyHost`, `_caravanHost`, `_caravanDirty` do not exist in `src/Main.cs`; the caravan dirty flag is plural: `_caravansDirty`). Also move `_economyPanel` (`EconomyMarketPanel`) if it is only referenced from economy setup/save code — confirm with `find_references` before moving, since UI wiring in `BuildUserInterface()` may also touch it.
3. Move `SetupEconomy()` (line ~2542), `SaveEconomy()` (line ~2602), `FlushEconomyIfDirty()` (line ~2612 — note the plan omitted this method; it exists and must move alongside `SaveEconomy`), `SetupCaravans()` (line ~3641), `SaveCaravans()` (line ~3649), `FlushCaravanIfDirty()` (line ~2108).
4. Move economy-related event handlers (e.g. `_economy.StateChanged += () => _economyDirty = true;`).
5. Keep call sites in Main.cs intact (they remain valid across partials).

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass
- Economy panel and caravan panel functional (headless smoke via `--bridge-selftest` plus a manual day-advance to confirm dirty-flush still fires)

**Risk / rollback:** Low risk, but this step touches two sub-domains (economy + caravans) that share the file — if the split is later found to be too large, it can be divided into `Main.Economy.cs` and `Main.Caravans.cs` without any behavior change. Rollback: revert both new/changed files via git; no save-format or DTO change means no player-facing risk even if reverted mid-task.

**Done when:** `Main.Economy.cs` compiles cleanly with 0 warnings, `FlushEconomyIfDirty` is confirmed present in the moved code (not dropped), and Main.cs shrinks by the actual moved line count (~90–120 lines for economy + caravans combined — see corrected Summary table; the original ~250-line estimate was not derived from a real measurement).

---

## Step 3 — Extract Medical Domain Partial

**Goal:** Move `SetupMedical`, `SaveMedical`, `FlushMedicalIfDirty`, `SetupMedicalWard`, `SaveMedicalWard`, `LoadMedicalWard`, and medical fields to `src/Main.Medical.cs`.

**Implementation:**
1. Create `src/Main.Medical.cs` with `partial class Main`.
2. Move `_medical` (type `MedicalHostSession`), `_medicalWard` (type `Ashfall.Core.Medical.MedicalWardSystem`), `_medicalDirty`, `_medicalWardDirty` fields (verified names — the plan's `_medicalHost`/`_medicalWardHost` do not exist; there is also no separate `_medicalWardHost` session type, `_medicalWard` is the Core system reference held directly).
3. Move `SetupMedical()` (line ~3507), `SaveMedical()` (line ~3519), `FlushMedicalIfDirty()` (line ~2093), `SetupMedicalWard()` (line ~3743), `SaveMedicalWard()` (line ~3766), and `LoadMedicalWard()` (line ~3787 — the plan omitted this method; it is called from `SetupMedicalWard()` and must move with it).
4. Move any disease/respiratory setup that lives alongside medical — **verified false lead:** `SetupDisease()` (line ~3992) does **not** belong here. It depends on `SetupExpansions()` / `_expansions.Disease` and marks `_expansionHubDirty`, not `_medicalDirty`; it saves through the expansion-hub envelope, not the medical envelope. Do not move it into `Main.Medical.cs`; leave it with the expansions partial (see Step 6/handoff note) or a future dedicated step.

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors
- Medical panel renders, chemical dependency and vigil state machine functional (verify via existing `Ashfall.Core.Tests` coverage for `ChemicalDependencySystem`/`VigilStateMachine` plus a headless boot)

**Risk / rollback:** Medium risk relative to the other steps — medical ward introduces a third method (`LoadMedicalWard`) not mentioned in the original plan. Rollback: revert the two files; no DTO/save-format change.

**Done when:** `Main.Medical.cs` compiles, `LoadMedicalWard` is present in the moved code, `SetupDisease` is confirmed left in `Main.cs` (not moved), and Main.cs shrinks by the actual moved line count (~80–100 lines for medical + medical ward — see corrected Summary table).

---

## Step 4 — Extract Combat Domain Partial

**Goal:** Move `SetupCombat`, `SaveCombat`, `FlushCombatIfDirty`, `SetupExpeditionCombatHandoff`, and combat fields to `src/Main.Combat.cs`.

**Implementation:**
1. Create `src/Main.Combat.cs` with `partial class Main`.
2. Move `_combat` (type `CombatHostSession`), `_combatDirty` (verified names — the plan's `_combatHost` does not exist), and combat event handlers (`_combat.StateChanged += () => _combatDirty = true;`).
3. Move `SetupCombat()` (line ~3399), `SaveCombat()` (line ~3417), `FlushCombatIfDirty()` (line ~3427 — the plan omitted this method entirely; it exists and must move with the rest of the triad), and `SetupExpeditionCombatHandoff(CombatHostSession combat)` (line ~3438) — this method is called directly from inside `SetupCombat()`, so it must move in the same step or `Main.cs` will fail to compile on a missing-reference error across partials (it will still compile since partials share scope, but leaving it behind would defeat the purpose of the split — keep the combat/expedition handoff together since it's combat-owned).
4. `_combatPanel.Bind(_combat)` (line ~2891) is called from UI wiring, not from `SetupCombat()` — confirm whether `_combatPanel` itself should also move, or stays in `Main.cs`/a UI partial. Do not assume; check with `find_references` on `_combatPanel`.

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors
- `--combat-selftest` exists and is the correct headless verb (verified in `src/Host/HostCli.cs:159-160` and `src/Host/HostCli.SelfTests.cs:442`, `RunCombatSelfTest`): run `godot --headless --path . -- --combat-selftest` and confirm it still exits 0 after the split (catalog, ballistics, weapon condition, determinism, save round-trip)

**Risk / rollback:** Low-medium risk — `SetupExpeditionCombatHandoff` creates a real cross-domain coupling between Combat and Expeditions (Step 1). Extract Combat only after Expeditions is confirmed stable, and re-run the Expeditions verification after this step to catch any partial-ordering regression. Rollback: revert both files; no DTO/save-format change.

**Done when:** `Main.Combat.cs` compiles cleanly, `FlushCombatIfDirty` and `SetupExpeditionCombatHandoff` are both present in the moved code, and Main.cs shrinks by the actual moved line count (~50–70 lines for this domain — see corrected Summary table).

---

## Step 5 — Extract Narrative Domain Partial

**Goal:** Move `SetupNarrative`, `SaveNarrative`, `FlushNarrativeIfDirty`, `SetupEncounterChoice`, and narrative fields to `src/Main.Narrative.cs`.

**Implementation:**
1. Create `src/Main.Narrative.cs` with `partial class Main`.
2. Move `_narrative` (type `NarrativeHostSession`), `_narrativeDirty`, `_encounterChoice` (type `Ashfall.Core.Expeditions.EncounterChoiceResolver`), and `_encounterChoiceDirty` fields (verified names — the plan's `_narrativeHost`/`_encounterChoiceHost` do not exist, and the encounter-choice dirty flag is separate from `_narrativeDirty`, not shared with it).
3. Move `SetupNarrative()` (line ~3482), `SaveNarrative()` (line ~3490), `FlushNarrativeIfDirty()` (line ~2088), `SetupEncounterChoice()` (line ~3871).
4. **Verified finding:** `_encounterChoiceDirty` is set (`_encounterChoice.OnResolved += _ => _encounterChoiceDirty = true;`) but there is **no** `SaveEncounterChoice()` or `FlushEncounterChoiceIfDirty()` method anywhere in `src/Main.cs` — the flag is currently dead/unconsumed, or encounter-choice state is captured incidentally inside another system's `CaptureState()` (e.g. narrative or expeditions) without being routed through this flag. This is a pre-existing gap, not something to fix silently during the partial extraction — move the field and flag as-is, note the gap in the partial file's header comment, and raise it as a candidate follow-up ticket rather than papering over it with a new Save method invented during a "refactor only" batch.

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors
- Narrative encounters fire on day advance, save/load round-trips (verify via existing narrative save-store tests plus a headless boot)

**Risk / rollback:** Low risk. Rollback: revert both files; no DTO/save-format change.

**Done when:** `Main.Narrative.cs` compiles, the encounter-choice save/flush gap above is documented (not silently fixed — that is out of scope for a "refactor only, no behavioral changes" batch per this plan's own risk rating), and Main.cs shrinks by the actual moved line count (~60–90 lines — see corrected Summary table).

---

## Step 6 — Extract World/Weather Domain Partial

**Goal:** Move `SetupWorld`, `SaveWorld`, `FlushWorldIfDirty`, and world/weather fields to `src/Main.World.cs`. Treat `SetupWastelandMap` as a **separate, independently-owned** system in the same step only if a save/flush method for it is confirmed to exist.

**Implementation:**
1. Create `src/Main.World.cs` with `partial class Main`.
2. Move `_world` (type `WorldHostSession`), `_worldDirty` fields (verified names — the plan's `_worldHost` does not exist).
3. Move `SetupWorld()` (line ~3544), `SaveWorld()` (line ~3571), `FlushWorldIfDirty()` (line ~2098).
4. `SetupWastelandMap()` (line ~3845) is **not** coupled to `SetupWorld()`/`_worldDirty` — it constructs its own `_wastelandMap` field (a raw `Ashfall.Core.World.WastelandMapSystem`, not a `WorldHostSession`) with no dirty-flag wiring visible at its definition. Before moving it into this partial, verify with `grep -n "_wastelandMap" src/Main.cs` whether it has its own save/flush pair elsewhere in the file (it may belong in its own future partial rather than World). Do not assume co-location from the original plan's grouping.

**Verification:**
- `dotnet build Ashfall.csproj` — 0 errors
- Weather ticks, wasteland map navigable, seasonal fallout storms trigger (verify via `Ashfall.Core.Tests/World/WastelandMapSystemTests.cs` plus a headless boot)

**Risk / rollback:** Low risk for World; the Wasteland Map coupling is unverified and should be confirmed or split out before this step is marked done. Rollback: revert changed files; no DTO/save-format change.

**Done when:** `Main.World.cs` compiles, the Wasteland Map ownership question above is resolved (either moved here with evidence, or explicitly left in `Main.cs`/split into a later step), and Main.cs total line count is reported from an actual `wc -l` after the change rather than assumed.

---

## Step 7 — Validate Triad Completeness

**Goal:** After extraction, audit that every Setup has a matching Save (and Flush if stateful). Identify any triad drift introduced by the split.

**Implementation:**
1. Write a shell script or xUnit test that extracts all `private void SetupXxx()` method names from `src/Main.cs` and its new partials via regex (e.g. `grep -ohP 'private void Setup\K\w+' src/Main.cs src/Main.*.cs`) and asserts a `SaveXxx` counterpart exists for each, except a documented exception list.
2. Document any Setup methods that intentionally have no Save. Confirmed example: `SetupUtilityAi()` (line ~2516) has no `SaveUtilityAi()` anywhere in `src/Main.cs` — verified stateless. Do not assume other Setup-only methods are also intentionally stateless without the same grep-and-confirm step; treat each as a candidate bug until checked individually.
3. Add a comment header in each partial file listing its triads (Setup/Save/Flush method names actually present in that file).

**Verification:**
- Script/test passes with 0 unmatched triads (or an explicit, reviewed exception list committed alongside the script)
- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings
- Full verification checklist (all 5 steps in AGENTS.md's Verification Checklist) passes

**Risk / rollback:** Low risk — this step is additive (a script/test) and does not change `Main.cs` itself. If the audit surfaces genuine triad drift (a Setup with no Save that should have one), do **not** fix it inside this batch — file it as a separate follow-up with its own risk review, since this batch is scoped "refactor only, no behavioral changes."

**Done when:** the triad-audit script/test runs and exits 0 (or lists only the pre-reviewed exceptions) against the post-split `src/Main.cs` + all six new partial files, and all 5 items in the canonical verification checklist report PASS.

---

## Summary

| Step | Partial File | Lines Moved (estimate — re-measure with `wc -l` after each step) | Cumulative Reduction |
|------|-------------|-------------------|---------------------|
| 1 | Main.Expeditions.cs | ~40–60 | ~50 |
| 2 | Main.Economy.cs | ~90–120 | ~165 |
| 3 | Main.Medical.cs | ~80–100 | ~255 |
| 4 | Main.Combat.cs | ~50–70 | ~315 |
| 5 | Main.Narrative.cs | ~60–90 | ~390 |
| 6 | Main.World.cs | ~40–60 (World only; Wasteland Map ownership TBD per Step 6) | ~440 |
| 7 | (audit) | 0 | ~440 |

The original plan's per-step estimates (200/250/150/100/200/200 lines, totaling 1,100) were not derived from measuring the actual method bodies and significantly overstate the reduction — the six target methods plus their directly-associated fields and handlers span roughly 400–450 lines total based on the verified line ranges cited in Steps 1–6 above, not 1,100. Treat all estimates in this table as approximate; the authoritative number is whatever `wc -l src/Main.cs` reports after each step, not this table.

**End state:** Main.cs is currently **7,014 lines** (verified, not 6,640). After Steps 1–6 it will drop to roughly **6,550–6,600 lines**, not ~5,500 — the original target was based on both a wrong starting count and overstated per-step reductions. The remaining core (game state machine, `TickSimDay` orchestration at line ~1643, `BuildUserInterface` at line ~640, `ContinueGame`/`StartNewGame` at lines ~5551/~5426) stays in Main.cs. Further batches can extract Survivors, DutyRoster, YearOfAsh, Holdfast, etc. — but each of those must go through the same verify-actual-field-names process used in this corrected plan, not be assumed from method-name pattern matching alone.

## Review Notes (Corrected)

This batch plan was adversarially reviewed against the real repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The following issues were found and fixed in place:

1. **Wrong Main.cs line count.** Plan stated 6,640 lines; actual `wc -l src/Main.cs` reports **7,014 lines**. Corrected throughout.
2. **Wrong method-count claims.** Plan stated "31 Setup methods, 24 Save methods, and 14 Flush methods." Actual verified counts by grep: **38 Setup, 30 Save (+ `SaveAll`), 17 Flush.** Corrected in Rationale.
3. **Wrong field names throughout every step.** The plan invented `_expeditionHost`, `_economyHost`, `_caravanHost`, `_medicalHost`, `_medicalWardHost`, `_combatHost`, `_narrativeHost`, `_encounterChoiceHost`, `_worldHost` — none of these exist. The real fields are `_expeditions`, `_economy`, `_caravans` (note plural), `_medical`, `_medicalWard`, `_combat`, `_narrative`, `_encounterChoice`, `_world`. Also `_caravanDirty` should be `_caravansDirty` (plural), matching the field it flags. All step implementations were corrected with verified field names and line numbers.
4. **Missing methods in each triad.** The plan omitted `FlushEconomyIfDirty` (Step 2), `LoadMedicalWard` (Step 3), `FlushCombatIfDirty` and `SetupExpeditionCombatHandoff` (Step 4). All are now called out explicitly since omitting them from a "move" step would leave orphaned methods behind in `Main.cs`, silently defeating the extraction.
5. **False domain grouping: Disease is not Medical.** Step 3 originally said to move "any disease/respiratory setup that lives alongside medical" — verified `SetupDisease()` actually depends on `SetupExpansions()`/`_expansions.Disease` and flags `_expansionHubDirty`, not `_medicalDirty`. This was corrected to explicitly exclude Disease from the Medical partial.
6. **False assumption: Wasteland Map is not coupled to World.** Step 6 originally bundled `SetupWastelandMap()` with `SetupWorld()`/`SaveWorld()`. Verified `SetupWastelandMap()` constructs an independent `_wastelandMap` field with no visible dirty-flag wiring at its definition and no confirmed Save/Flush pair alongside `SaveWorld()`. Flagged as an open ownership question rather than silently kept in the grouping.
7. **Real gap surfaced, not invented:** `_encounterChoiceDirty` is set on `_encounterChoice.OnResolved` but there is no `SaveEncounterChoice()`/`FlushEncounterChoiceIfDirty()` anywhere in `src/Main.cs` — this is a pre-existing bug/gap unrelated to the refactor. Documented as an explicit non-goal for this "refactor only" batch rather than silently fixed or silently ignored.
8. **Verified `--combat-selftest` exists** (`src/Host/HostCli.cs:159-160`, `RunCombatSelfTest` in `HostCli.SelfTests.cs:442`) — Step 4's verification now cites the real, runnable command instead of a vague "headless self-test."
9. **Inflated/unsourced line-reduction estimates.** The original per-step estimates (200/250/150/100/200/200, summing to 1,100 lines and landing at "~5,500 lines") do not match the actual size of the referenced methods and fields. Replaced with ranges tied to the verified line numbers for each method, and the end-state target was corrected to ~6,550–6,600 lines.
10. **Vague Done-when criteria tightened.** Step 7's original "Triad audit passes, all 5 verification steps green" gave no way to fail deterministically. Replaced with a concrete script/regex approach, an explicit exception-list mechanism, and instructions not to silently fix drift discovered mid-audit (which would violate the batch's own "Low risk — refactor only" premise).
11. **Added risk/rollback notes.** The original plan (Priority HIGH, Risk Low) had zero rollback guidance for any step despite editing a single 7,000+ line file relied on by the entire game loop. Each step now states the actual coupling risk (e.g. Combat↔Expeditions handoff, Medical↔Disease non-coupling, World↔WastelandMap ownership) and a concrete rollback path (git revert of the two files involved, no DTO/save-format impact since this is a pure source reorganization).
