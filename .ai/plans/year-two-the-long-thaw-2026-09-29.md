# Feature / Task Plan: ASHFALL — Year Two: The Long Thaw (Days 361–720)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> **Approval record.** User, 2026-09-29, in session: *"I authorise the each seperate plan!"* — approves this umbrella and each derived per-package plan (`.ai/plans/y2-*-2026-09-29.md`). Same message **overrules** two drafted decisions: the Reckoning day is per storyline (DEC-Y2-02 revised) and Year One's ending selection may differ per storyline (DEC-Y2-09 reversed). See §6 and prose plan §2.1 / §5.7.
> **What approval covers:** authoring and integrating the packages below under the stop conditions in §0. It does **not** waive Rule 10, the foreman's claim step, the shared-seam (`INT`) ownership rules, or the P0 confirmation of decisions not yet answered individually (DEC-Y2-01, -04…-08, -10, -11 are recommended defaults adopted by approving the plan text; **the user may revoke any of them**, and P0 restates them for a one-line confirm).
> **Nothing has been implemented, claimed, or committed by this document.**

**Umbrella plan.** This file is the program-level integration plan. Each package below is executed **one at a time** under its own derived plan file (`.ai/plans/y2-<package>-<date>.md`, copied from `.ai/plans/template.md`, `STATUS: APPROVED BY USER` set by the user) so that CLAUDE.md §8 (every code commit needs an approved plan) is satisfied per package, not once for the whole chapter.

**Prose companion (the story, the evidence table F1–F17, the cast, the endings):**
`docs/expansions/expansion_year_two_the_long_thaw_plan.md`
Read that first for *why*. This file is *how, in what order, and where it stops*.

**Author role:** Story Director (documentation only — no source, data, ledger, or claim edited in authoring this plan).
**Date:** 2026-09-29 · **Branch at authoring:** `integration/all-latest-2026-09-24` (dirty worktree preserved; not touched).

---

## 0. Foreman checklist before any builder starts

CLAUDE.md requires this read order; this plan has **not** edited any of these files (foreman/integrator-owned).

1. `INTEGRATION_PLANS.md` — current batch. **Authoring-time evidence:** its live head is the performance/host recovery close and the blocked Wave-20 readiness package; XP-WAVE1 is COMPLETE. Year Two is **not in the ledger**. A foreman entry is required (§9).
2. `WORKTREE_OWNERSHIP.md` — no live claim overlaps the paths below at authoring time (a search for `Main.Endgame.cs`, `OutpostSettlementSystem.cs`, `WaystationNetworkSystem.cs` found only historical, closed claims). **Re-check at each package start**; the file changes hourly.
3. `TEST_POLICY.md` — focused targets only; `bin/run-scoped-tests`; **no full suite** without the literal `RUN FULL TESTS`.
4. `KNOWN_DEBT.md` — Year Two adds debt rows (§10); it retires none.
5. `AI_AGENT_WORKFLOW.md` — package card format (§5), handoff format (§11).

**Stop conditions (Rule 10):** any package that (a) needs a new architecture decision not in §6, (b) overlaps a live claim, (c) would restore retired behavior, or (d) contradicts current evidence, **stops and reports STALE_PLAN / BLOCKED to the foreman.** It does not improvise.

---

---

## 0b. Prologue — The Long Thaw (texture; not a claim, not authority)

> *"The war ended the way a fever ends. Not with a morning. With a long, ambiguous week in which
> nobody could tell you whether the sweating had stopped or merely moved."*

**Editorial note.** This section and §1b are narrative texture and writing guidance only. They
change no authority, no claimed path, no acceptance criterion and no verification step. §14 is a
register of deliberately unanswered questions. Sample lines are content candidates for
`year_two_chapter.json` / `year_two_radio.json` rows; they belong in data, never in code.

Day 360 is a door and there are only two things to do with a door. **SEAL HERE** is not a failure
state — it is the game keeping its first promise, bit-identically, forever. **PLAY ON** is the
other thing, and it is not an epilogue. It is a decision to be present for a *thaw*, which is the
least dramatic and most demanding thing a world can do to the people living in it.

The Long Thaw is a year of paperwork, apprenticeship, distance and weather. Nine Chronicles, none
of which is an ending in the way Year One ended. Children become apprentices. Apprentices become
the people who decide things on the night of a Rite of Passage that the chapter stops just short
of. A second shelter acquires bunks and a real location. A road acquires cost. And Standing —
A, B, C, and the strange fourth grade called **D, The Late Call** — keeps a quarterly account of
what the shelter is now understood to be.

**Tone & register.** Domestic, institutional, quietly monumental. The vocabulary is the household
and the archive: *hearth, bunk, standing, reading, rite, charter, road, reckoning*. Prose should
feel like the minutes of a community that has survived something and now has to live with having
survived it. Never write a climax. The chapter's whole thesis is that there isn't one.

**Mystery & texture.** Three things are deliberately held back: what the Reckoning *was* in the
mechanics of the world rather than the story of the shelter; what Standing D is waiting for; and
what happens on the night after the last line the chapter writes. See §14.

**The second layer.** The Long Thaw is the least dramatic thing a world can do to the people in it,
and that is why it needs a whole program. Year One asked whether the shelter could be kept alive;
Year Two asks what the shelter agrees to *be* now that it is alive — and the program's answer is
entirely structural: twelve bunks bound to a place, four readings that disagree, apprentices who
are not rushed, a road that costs days, and a last page that describes instead of judging. The
bit-identical legacy promise is what licenses all of it. A game that keeps its first promise earns
the right to ask its players for a second year. And the asking is made in the smallest possible
voice — a bunk, a reading, a date on a wall — because a thaw is never announced; it is noticed.

---

## 1. Goal & Outcome

> *Design intent: Year One asks whether you can keep people alive. Year Two asks what they will
> agree to be, now that they are alive. There is no mechanical answer to that and the plan does
> not invent one.*

- **Goal:** Let a running campaign **play on after the Reckoning** into a second 360-day chapter (Days 361–720), with (I) a live *Standing* derived from the resolved verdict, (II) a *generations* arc that carries children to apprentices to acting successors and ends the night before the first Rite of Passage, and (III) an *outposts network* (a second shelter, waystations as the road, outposts as positions) in which distance costs something and defense is real — using **only existing owners** plus three nested additive state structures.

- **Observable outcome (whole program):** a headless 720-day run from a fresh seed reaches Day 360, the player chooses **PLAY ON**, the run reaches Day 720 with no exceptions, produces a deterministic Year Two Chronicle (one of nine), and seals once; saves round-trip at Days 359, 361, 450, 600, and 719; a Day-360 **SEAL HERE** is behaviorally identical to today.

- **Non-Goals (program level):**
  - No new save *section* (three nested additive structures inside `endgame`, the Verdict section, and one of `apprenticeship`/`genealogy` — §4.3).
  - No second population, food, inventory, ledger, settlement, or simulation. No power/water/air *simulator* for the second shelter.
  - No change to Year One behavior for Days ≤ 360 **under the legacy profile** (`profile_base_v1`; bit-identical, tested). Storyline profiles may deliberately differ — that is the point of P1B — but only for campaigns that select them; saves already in progress load as legacy.
  - No rewriting of authored Verdict content day gates: timing shifts through one boundary adapter (DEC-Y2-13).
  - No merge of `OutpostSettlementSystem`, `ColonySystem`, `WaystationSystem`/`WaystationNetworkSystem`, or `SettlementCatalog` (signed custody, `ORPHAN_SEAL_PRIORITY_W1_BOUNDARIES.md` §2).
  - No implementation of Expansions 44 / 82 / 46 as written; they are vocabulary references only.
  - No Chapter Three (Days 721+). No formal *ratified* successor from a raised child inside the chapter (impossible by the F7 age floor; by design).
  - No Unity. No new routed panel (DEC-Y2-08). No Python/shell tools for the eight Go-only concerns (CLAUDE.md tooling policy).
  - No restoring deprecated APIs. No graphic depiction of harm to children.

---

## 1b. Texture, Mystery & Voice

**Nine Chronicles and none is a verdict.**

The Chronicle is derived from the resolved verdict and the year's Standing, but it is a *portrait*,
not a score. Let each of the nine read as a paragraph someone could read aloud in a room without
anyone flinching. P7 owns the id and the determinism; the prose owns the mercy.

**Generations is the heart; the Rite is the horizon.**

The chapter ends **the night before the first Rite of Passage** and that stopping point is a
design decision with a sound structural justification (the F7 age floor makes a ratified successor
impossible by design) — but it is also the most literary thing in the program. A community is
always about to hand something over. Let that feeling stand unresolved.

**What the player is never told.**

- What Standing **D — The Late Call** means. It is a real grade (P3) that resolves into A/B/C. Until
  it resolves it is a piece of vocabulary the shelter is living inside.
- Whether the second shelter is a return or a departure. *Thirteen* binds bunks to a real location
  and stops there. Custody of meaning is not asserted.
- Why the Road costs what it costs. P6 gives supply runs, pressure and relief. The arithmetic is
  authored; its fairness is not discussed.
- What the Rite of Passage would have been. The chapter stops the night before. By design.

**Voice — sample fragments (content candidates for `year_two_chapter.json` / `year_two_radio.json`).**

> "Day 400. Standing: C. We are not being punished and we are not being praised. We are being
> described, which is harder."

> "The apprentice repaired the pump without asking. I have written it in the book because nobody
> else will and because in ten years the book is what there will be."

> "Quarterly reading, second of the year, in the second voice. The readings do not agree with each
> other. That is why there are four."

> "The road is open. The road costs. Both of those are true and only one of them is on the sheet."

**Design texture beats.**

- **Legacy profiles are bit-identical (§1 Non-Goals).** A Day-360 SEAL HERE must be *exactly* today's
  game. That is the program's honesty guarantee and its most reassuring promise.
- **Distance costs something.** The outposts network is the chapter's only new form of pain, and it
  must be paid in time and supply, never in invented resources.
- **No graphic harm to children, ever.** This is a hard rule (§Non-Goals) and it is also why the
  Generations arc works: it is about capability, not peril.
- **No Chapter Three.** The program ends at Day 720 with a seal. Resist every instinct to gesture
  beyond it — the silence after the last line is the point.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §14's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions.)*

**What the year leaves lying around.**

> "Standing sheet, third quarter: D. The word is on the sheet and in the corridor within the hour."

> "Rite of Passage, prepared and unheld. The chapter stops the night before, and the preparation is the ending."

> "Allocation 13, named in the ledger and called something else at the table."

**Scenes the player may piece together.**

> "The apprentice repaired the pump without asking. It is written in the book, because in ten years the book is what there will be."

> "Day 720. One seal. The paragraph is read aloud in a room where nobody flinches."

**Held silences (texture, not register rows).**

- Who keeps the second winter's almanac. P1 authors values, never a keeper; the weather office has no staff and must not acquire any. Texture only.
- Whether the hearth's folk name is ever said aloud twice. Folklore, not state (`definitions never persisted`); the repetition is the player's, and the game does not count it.

**Third pass — the long week (texture only; §14 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §14's register is unchanged; the
fragments below are content candidates for `year_two_chapter.json` / `year_two_radio.json` and
deliberate silences, not new recorded questions.)*

**The shape of the polish.** The register is the minutes of a community that has survived something
and now has to live with having survived it. Never a climax. The prose should carry its mysteries
as *dates*: what the Reckoning was, what Standing D waits for, and what happens after the last line
— three questions the program holds open by simply continuing to keep books.

**What the year leaves lying around.**

> "Day 362. Nobody has named it yet. The naming will happen later, in a kitchen, and the almanac
> will record it as weather."

> "Standing sheet, quarter four: the grade is unchanged. Nobody has ever written 'unchanged' on a
> sheet of this kind before."

> "Reading day, sealed copy. The paragraph is read aloud in a room where nobody flinches. The
> flinch was in the waiting."

**Held silences (texture, not register rows).**

- What the fever moved to. The prologue's metaphor is a week nobody can read (§0b); the sweating is
  not tracked and the metaphor must not be made clinical. Texture only.
- Who rings anything at all in Year Two. Year One ended without a bell; the second year has no
  ringer and the silence is the tone.

---

## 2. Premise evidence (CLAUDE.md Rule 7 — verified 2026-09-29; **re-verify per package**)

The full table with paths is in the prose plan §3 (F1–F17). Load-bearing rows:

| # | Fact | Where | Why it gates work |
|---|---|---|---|
| F1 | Host ends the chapter at Day 360 (`living==0 \|\| day>=360` → `TriggerEnding`; Chronicle offers SEAL). | `src/Main.Endgame.cs` `CheckAndTriggerEndgame`; `src/UI/ChroniclePanel.cs` | P2 must change trigger consequence. |
| F2 | Seal runs terminal side effects (SaveAll, unified ending rewrite, legacy archive, completion history append-only, meta-progression). | `src/Main.Endgame.cs` `OnCampaignSealed` | Only the **final** seal may run them. |
| F3 | Host ending ctx omits `TruthBroadcasted / VassalageAccepted / DominantFaction / ForceWinterFailure`. | `src/Main.Endgame.cs` L224–232 | Standing must read `BuildCampaignOutcomeSnapshot()`, not the ctx. |
| F4 | `YearOfAshTimelineSystem.AdvanceDay` clamps at 360; its temperature feeds thermal/radon/ice-road. | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs`; `src/YearOfAsh/YearOfAshHostSession.cs` L170–175 | **Blocker.** Days 361+ freeze at +4 °C. |
| F5 | 360 horizon also appears as data defaults/constants in Questline, Door encounters, Dose content, Caravan routes, Year-of-Ash catalog loader. | `grep` (see prose §3) | P1 census decides which are runtime clamps vs. inert defaults. |
| F6 | Almost no authored content exists past Day 360 (11+8+5 entries at 365, 1 at 390, 1 at 361). | data scan | Year Two content is greenfield (P8). |
| F7 | Child stages: Toddler 60, Child 180, Adolescent 500, YoungAdult 720 days; age floored at birth day 1 → earliest coming-of-age **Day 721**. | `ChildDevelopmentSystem.cs` L98–124 | Shapes P4 and the whole ending. |
| F8/F9 | Milestone ladder, `ApprenticeshipSystem`, `PerformSuccession` exist and are host-wired. | files named in prose §3 | P4 reuses; adds no ladder. |
| F10 | Outposts/Waystations/Colony/Settlement custody is **signed** and separate. | `docs/plans/ORPHAN_SEAL_PRIORITY_W1_BOUNDARIES.md` §2 | P5/P6 stay inside it. |
| F11 | `WorldDangerRatingForDay` returns 0 → outposts are never attacked in play. | `src/Main.OutpostSettlement.cs` L280–287 | P6 replaces the stub. |
| F12 | Outposts are fed free from inventory daily (no journey). | `src/Main.OutpostSettlement.cs` L216, L233–250 | P6 introduces supply runs. |
| F13 | `graph_node_id`s are defined nowhere else; validator checks costs only. | `outposts.json`; `CatalogIntegrityValidator.ValidateOutpostCostCatalog` | P5 binds outposts to real locations. |
| F16 | Allocations 11 / 12-B / 13, Pump Hatch, Blank Cellar, Nila Brant, Sela, Halvard Renn, Margit Sole already exist as lore + quests. | `duty_roster_*.json`, `holdfast_*.json`, `codex_entries.json` | Thirteen is re-opened, not invented. |
| F16b | Existing quest choice ("Copy it for Sole. Completeness opens 13 to a file and closes 11 to you.") is a live flag Year Two must **read**. | `duty_roster_quests.json` ~L898–914 | P0 must extract flag ids. |

**Stale references found while authoring (do not propagate):**
- `docs/endgame/ENDGAME_V1.md` cites host gate `--endgame-v1-selftest`; the CLI registry did **not** list it in this audit. Use `--unified-ending-selftest`, `--epilogue-selftest`, `--chronicle-summary-selftest`, `--epilogue-chronicle-selftest` after confirming names (P2 pre-flight).
- `ENDGAME_V1.md` says sealing halts simulation ticks; no `IsSealed` gate in `CommitAdvance` was found (only `EndgameHostSession` and `ChroniclePanel` read it). P2 pre-flight confirms actual behavior.
- Expansion 82's "no host reference / no save-section" premise is superseded by the `colony` section (ORPHAN-SEAL-W1).
- World bible says "Machine Reckoning at Day 360"; code resolves the Call at Day 240. **Resolved by the user (2026-09-29):** the Reckoning day is per storyline; the default profile keeps 240/360.

**Second-pass findings (after the storyline override; details in prose plan §3.0):**

| # | Fact | Where | Why it gates work |
|---|---|---|---|
| F18 | 45 authored faction branches (15 Military / 15 Rebel / 15 Independent), 3 endings each = 135 branch endings; each has a `flag_branch_*_ponr` and an `OnEndingResolved` event; `FactionBranchCoordinator` enforces mutual exclusion. | `military_/rebel_/independent_faction_branch.json`; `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` | P1B: branch endings become Year One endings and seed Standing modifiers. |
| F19 | Reckoning phase days are `const` in `ReckoningSystem`; no other source reads them; `Poll` compares the raw campaign day. | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` | P1B: optional thresholds/offset at one owner, default = today. |
| F20 | The Verdict can be **unresolved** at Day 360 (evidence gate; `DecideEnding` returns `null` below Counted). | `ReckoningSystem.Poll` L84–108; `VerdictEndingEvaluator.cs` L47–52 | Adds **Standing D — The Late Call** (P3). |
| F21 | `UnifiedEndingContext` already carries `factionBranchId`, `musterApproachId`, `holdfastEndingId`, `verdictEndingId`; populated in `src/Main.UnifiedEnding.cs` L95; resolver runs only at seal. | `UnifiedEndingResolver.cs` | P1B: Year One ending *selection* can adopt the unified source under a profile. |

---

## 3. Claimed Paths & Affected Files

**Nothing is claimed by this document.** Claims are made by the foreman in `WORKTREE_OWNERSHIP.md`, per package. The lists below are the **proposed** exact paths for each package's claim. `INT` = integrator-owned shared seam (builder may not edit; request via foreman).

### P0 — Premise audit & decision packet (read-only + 2 docs)
- `docs/plans/year_two/Y2_PREMISE_EVIDENCE.md` (new)
- `docs/plans/year_two/Y2_DECISION_PACKET.md` (new)

### P1 — Horizon Lift
- `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs`
- `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs` (additive only)
- `Assets/Ashfall.Core/YearOfAsh/YearTwoClimateCatalog.cs` (new loader)
- `Assets/StreamingAssets/Data/year_two_climate.json` (new)
- `src/YearOfAsh/YearOfAshHostSession.cs`
- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` — `INT` (baseline delegation only if DEC-Y2-03 selects it)
- Only the F5 files P0 proves are *runtime clamps*, each named in the package's own claim.
- Tests: `Ashfall.Core.Tests/YearOfAsh/YearTwoHorizonTests.cs` (new)

### P1B — Storyline Chapter Profiles & branch-aware Year One ending
- `Assets/StreamingAssets/Data/chapter_profiles.json` (new): `profile_base_v1` (legacy) + `base`, `military`, `rebel`, `independent`, `muster` family profiles; optional per-branch overrides
- `Assets/Ashfall.Core/Endgame/ChapterProfileCatalog.cs` (new loader + validator) and `ChapterProfileResolver.cs` (new, pure: storyline facts → profile)
- `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` (optional thresholds/offset; constants remain the defaults) and `ReckoningClock.cs` (new adapter `campaign day → verdict day`)
- `Assets/Ashfall.Core/Endgame/EndgameSystem.cs` (profile id in state; `EvaluateEnding` reads the profile's ending source; additive; legacy branch untouched)
- `Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs` (expose the year-one selection projection; additive)
- `src/Main.Endgame.cs` — `INT` co-sign (close rule; ctx population) and `src/Main.UnifiedEnding.cs`
- the Verdict host session that calls `Poll` (name VERIFY in P0) — pass verdict day through `ReckoningClock`
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — `INT` (profile rules: every branch ending has a Standing modifier or family default)
- Tests: `Ashfall.Core.Tests/Endgame/ChapterProfileTests.cs`, `Ashfall.Core.Tests/Verdict/ReckoningClockTests.cs` (new)

### P2 — Play On (chapter mechanism)
- `Assets/Ashfall.Core/Endgame/EndgameSystem.cs`
- `src/Host/EndgameHostSession.cs`, `src/Host/EndgameSaveStore.cs`
- `src/Main.Endgame.cs` — `INT` co-sign (ties to save orchestration and `OnCampaignSealed`)
- `src/UI/ChroniclePanel.cs`
- `Assets/StreamingAssets/Data/year_two_chapter.json` (new; constants only in this package)
- Tests: `Ashfall.Core.Tests/Endgame/YearTwoPlayOnTests.cs` (new); regression targets `EndgameSystemTests`, `Plan145UnifiedEndingHostIntegrationTests`, `Plan175MetaProgressionHostIntegrationTests`
- Host gate: `--year-two-chapter-selftest` (new descriptor in `HostCliRegistry.cs` — `INT`)

### P3 — Standing & quarterly Readings
- `Assets/Ashfall.Core/Endgame/YearTwoStanding.cs` (new, pure)
- `Assets/Ashfall.Core/Verdict/QuarterlyReadingLedger.cs` (new, nested in Verdict save)
- `Assets/Ashfall.Core/Verdict/VerdictSave.cs` (additive)
- the existing Verdict host session (name **VERIFY** in P0)
- `Assets/StreamingAssets/Data/year_two_chapter.json` (sentence tables), `year_two_radio.json` (new)
- `src/UI/ChroniclePanel.cs` (Standing Sheet page)
- Tests: `Ashfall.Core.Tests/Verdict/YearTwoStandingTests.cs`, `.../QuarterlyReadingLedgerTests.cs` (new)
- Day-owner hook: prefer extending the existing `narrative_quests_verdict` owner over a new registration (`src/Main.CampaignOwners.cs` is `INT`).

### P4 — Generations (sub-packages a–d, sequential)
- **4a Apprentice pipeline:** `Assets/StreamingAssets/Data/apprenticeship_catalog.json` (extend), `src/Host/ApprenticeshipHostSession.cs`, `src/UI/ApprenticeshipPanel.cs`
- **4b Elders:** `src/Host/AgingHostSession.cs`, `FinalWishSystem`/`HeirloomSystem` bindings (host sessions; exact files VERIFY in P0)
- **4c Council:** `Assets/Ashfall.Core/Survivors/SuccessionCouncilLedger.cs` (new; nested in the owner chosen by DEC-Y2-07), that owner's save DTO (additive), `src/Host/GenealogyHostSession.cs` or `ApprenticeshipHostSession.cs`, `src/UI/ApprenticeshipPanel.cs` (Council page)
- **4d Registration:** owner chosen in P0 (VERIFY: Standing Record / Voluntary Register / census)
- Tests: `Ashfall.Core.Tests/Survivors/YearTwoSuccessionTests.cs`, `Ashfall.Core.Tests/Generations/YearTwoApprenticeLadderTests.cs` (new)

### P5 — Thirteen (second shelter) and location binding
- `Assets/StreamingAssets/Data/outposts.json` (extend; additive fields; new positions)
- `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` (additive fields; DTO additive; definitions **never** persisted)
- `src/Host/OutpostSettlementHostSession.cs`
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — `INT` (location-binding rule)
- `src/UI/ShelterOperationsPanel.cs` — network tab only (`INT` co-sign; no new route)
- Tests: `Ashfall.Core.Tests/Settlements/YearTwoThirteenTests.cs` (new); regression `Plan58Outpost*Tests`, `OutpostAtomicBillTests`

### P6 — The Road (supply runs, pressure, relief)
- `Assets/Ashfall.Core/Settlements/OutpostPressureModel.cs` (new, pure)
- `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` (supply mode)
- `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs` (additive read: leg quality)
- `src/Main.OutpostSettlement.cs` (replace `WorldDangerRatingForDay` stub) — `INT` co-sign
- expedition dispatch host seam (files **VERIFY** in P0)
- `Assets/Ashfall.Core/Random/CampaignStreamIds.cs` (additive stream ids) — `INT`
- Tests: `Ashfall.Core.Tests/Settlements/YearTwoRoadTests.cs` (new)

### P7 — Year Two Chronicle & endings
- `Assets/Ashfall.Core/Endgame/YearTwoOutcomeEvaluator.cs` (new, pure)
- `Assets/Ashfall.Core/Endgame/EndgameSystem.cs` (chapter-2 evaluation)
- `Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs` (additive, chapter-aware)
- `Assets/StreamingAssets/Data/year_two_endings.json` (new)
- `Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs` (legacy traits from generations; additive)
- Tests: `Ashfall.Core.Tests/Endgame/YearTwoOutcomeTests.cs` (new)

### P8 — Content waves (W1 Q1 · W2 Q2 · W3 Q3 · W4 Q4)
- `Assets/StreamingAssets/Data/year_two_quests.json`, `year_two_radio.json`, `narrative_encounters_year_two.json` (new; wave by wave)
- Waystation keeper follow-ups (one line each) in the existing keeper data path (VERIFY)
- Tests: existing `NarrativeEncounterSystemTests`, data-integrity gate, content-utilization scan

### P9 — Governance close
- `docs/endgame/ENDGAME_V1.md` (fix stale claims), docs index refresh via the owning generator, `KNOWN_DEBT.md`/`INTEGRATION_PLANS.md` (foreman), plan header + archive.

---

## 4. Architecture rules (apply to every package)

### 4.1 Authority table (one owner per concern)
| Concern | Owner (extend, never replace) |
|---|---|
| Chapter close/continue/seal | `EndgameSystem` + `EndgameHostSession` |
| The count, lease, readings | `ReckoningSystem` + Verdict save section |
| Standing / outcome projection | `CampaignOutcomeEvaluator` via `BuildCampaignOutcomeSnapshot()` |
| Climate | `YearOfAshTimelineSystem` (catalog-extended) |
| Child stage & milestones | `ChildDevelopmentSystem` / `GenerationalSystem` |
| Mentor pairs, wills | `ApprenticeshipSystem` |
| Succession fact | `GenerationalLineageExtension.PerformSuccession` |
| Adult age & retirement | `AgingSystem` (30 d/yr; retire ≥ 65) |
| Outposts & secondary positions | `OutpostSettlementSystem` |
| Waystations | `WaystationNetworkSystem` |
| Supply-run travel | `ExpeditionSystem` |
| Rations/fuel/filters/water | canonical inventory |
| Grief/rites | `MemorialSystem`; rites are **not** evidence (W10) |

`GenerationalSuccessionEngine.inGameAgeYears` (365 d/yr) is **non-authoritative** for all Year Two decisions (logged debt, not fixed).

### 4.2 Determinism
Seeded streams forked from the campaign RNG via `CampaignStreamIds`, keyed by (day, slot). No `System.Random`, no wall-clock, no hash-order iteration. The quarterly reading is a **pure function of persisted state** and draws no RNG.

### 4.3 State (exactly three additive nested structures)
1. `endgame` section: `chapterIndex`, `chapters[]` (schema bump 1→2; **legacy default = chapter 1, empty list**).
2. Verdict section: `readings[]` (`quarter`, `day`, `filed`, `tier`).
3. Council ledger nested in the owner selected by DEC-Y2-07: `designations[]` (`role`, `candidateId`, `quarter`, `status`).

Definitions are re-derived from authored JSON on restore; they are **never** frozen into a save (same rule Plan 58 signed).

### 4.4 Data
New files under `Assets/StreamingAssets/Data/`: `year_two_chapter.json`, `year_two_climate.json`, `year_two_endings.json`, `year_two_quests.json`, `year_two_radio.json`, `narrative_encounters_year_two.json`. Extended: `outposts.json`, `apprenticeship_catalog.json`. All schema-valid snake_case, unique ids, cross-references validated through `CatalogIntegrityValidator`. **Presence in JSON is not reachability:** every catalog gets a consumer and an integration test in the same package.

### 4.5 UI
Extend existing routes: Chronicle panel (Reading, Standing Sheet, Year Two Chronicle pages), Apprenticeship panel (Council page), Shelter Operations panel (network tab). **No new routed panel** (would touch `PanelRegistryBootstrap`, `Main.UiPanels`, `PlayerSurfaceManifest`, all `INT`). Keyboard/controller back and focus restoration preserved; no color-only state; contrast at the current AA ratchet; the Standing Sheet is plain ordered text.

### 4.6 Ship-dark rule (R3)
Play On is **offered only if** `year_two_chapter.json`, `year_two_climate.json` **and** `chapter_profiles.json` loaded and validated (and the campaign's profile resolved). Missing/invalid → the Reading behaves exactly as today (SEAL only) with a logged warning. This is the feature flag; it is data, not a setting, so it cannot desync from content. Until content waves W1–W2 land, release builds may ship with the files absent.

---

## 5. Package cards (AI_AGENT_WORKFLOW package protocol)

Each card lists: outcome, premise, authority/save/host seam, acceptance (3–6 observable), verification. Test counts follow TEST_POLICY: **3–10 high-signal tests per behavior**, parameterized, deterministic seeds.

### P0 — Premise audit & decision packet · *Role: Sweep + Foreman* · **No code**
- **Outcome:** re-verify F1–F17 with `path:line`; census every 360-day constant/clamp; extract F16b flag ids; VERIFY the open items in §3; foreman signs DEC-Y2-01…14.
- **Acceptance:**
  1. `Y2_PREMISE_EVIDENCE.md` lists each F-row as CONFIRMED / STALE / CHANGED with `path:line`.
  2. Horizon census table: every 360 constant classified *runtime clamp* / *inert default* / *content window*.
  3. The Verdict host session, role owner, expedition-dispatch host, and "who is written" registration owner are each named with file paths.
  4. `Y2_DECISION_PACKET.md` carries a signed row per DEC.
  5. Ownership claims proposed for P1, P1B, P2 with exact paths.
  6. Verdict-day-gate census: every consumer of authored verdict day gates (quests, radio, evidence, accusation) listed with `path:line`, for the P1B `ReckoningClock` adapter.
  7. Per-storyline table drafted (families + the branch ids the user wants distinct first) with proposed Reckoning offset, close rule, ending source, and finale — for signature in P1B.
- **Verification:** static only. `git diff --check` on the two docs. No tests.

### P1 — Horizon Lift · *Role: Builder (Core) + Tester*
- **Outcome:** Days 361–720 have an honest climate; thermal, radon, ice road receive changing inputs; Days ≤ 360 unchanged.
- **Seam:** `YearOfAshTimelineSystem` reads `YearTwoClimateCatalog` for `day > 360`; `YearOfAshHostSession` unchanged in shape.
- **Acceptance:**
  1. **Bit-identical Year One:** a scripted 1→360 replay captures timeline state each day and matches a pre-change golden byte-for-byte.
  2. Days 361–720: ambient temperature, ash opacity, radon rate come from catalog phases; the four quarter phases are distinguishable; no freeze.
  3. `_deepFreeze`, `_radon`, `_iceRoad` receive **changing** values across 361–720 in a headless run.
  4. Save/restore mid-chapter round-trips; a legacy save with `currentDay ≤ 360` restores unchanged.
  5. Catalog fails validation on unknown phase, gap, or overlap in day ranges; loader failure disables Play On (§4.6), never silently freezes.
  6. Calendar baseline and timeline agree for Days 361+ (or the calendar read-model divergence is documented and the DEC-Y2-03 option recorded).
- **Verification:** `bin/run-scoped-tests` on the changed test file + `Ashfall.Core.Tests/YearOfAsh/`; `--year-of-ash-save-selftest`; host build 0 errors.

### P1B — Storyline Chapter Profiles & branch-aware Year One ending · *Role: Builder (Core + data) + Integrator co-sign*
- **Outcome:** each storyline has its own Reckoning days, Reading day/close rule, Year One ending source, and Standing modifier; the default profile reproduces today exactly.
- **Seam:** `ChapterProfileResolver` chooses a profile id once and persists it in the `endgame` section; `ReckoningClock` is the only place campaign day becomes verdict day; `EndgameSystem.EvaluateEnding` dispatches on `year_one_ending_source`.
- **Pre-flight (P0 outputs required):** the Verdict host session and every consumer of verdict day gates (quests/radio/evidence) census'd; `FactionBranchCoordinator.ActiveBranchId` and `OnEndingResolved` semantics confirmed; where a branch ending's text lives.
- **Acceptance:**
  1. **Legacy parity:** with `profile_base_v1`, Reckoning transitions on Days 160/210/240, close at Day 360, and the Day-360 ending equals today's `EvaluateEnding` for a fixed matrix of contexts (golden).
  2. With a shifted profile (e.g. offset −30), the Knowing/Culpable/Counted transitions occur on the shifted campaign days, `Poll` is idempotent, and authored verdict content still fires on its own authored (verdict) days.
  3. A faction-branch profile makes the branch's resolved ending row the Year One ending id; a branch with no resolved ending closes at its ceiling day with the family fallback and never before its floor day.
  4. **Every one of the 135 branch endings** resolves to a Standing modifier (override or family default); the integrity validator fails on an orphan.
  5. Profile selection is deterministic, persisted by id, and never changes mid-campaign; a save with no profile id loads as legacy.
  6. A deferred-Call profile (Standing D) keeps the Reckoning running past the Reading and honors `waive_evidence_gate_after_day`.
- **Verification:** `bin/run-scoped-tests` on `ChapterProfileTests`, `ReckoningClockTests`, `Ashfall.Core.Tests/Verdict/`, `EndgameSystemTests`, `Plan19EndingContinuityTests`, `Plan145UnifiedEndingIntegrationTests`; `--verdict-selftest`, `--unified-ending-selftest`; save round-trip; host build.

### P2 — Play On · *Role: Builder (Core + host) + Integrator co-sign*
- **Outcome:** Day 360 offers **PLAY ON** and **SEAL HERE**; Play On opens chapter 2 with no terminal effects.
- **Seam:** `EndgameSystem` gains chapter records and `ContinueChapter`; `Main.Endgame.CheckAndTriggerEndgame` reads chapter thresholds (360, 720); `OnCampaignSealed` runs only at final seal; `ChroniclePanel` gains a button and Year One page.
- **Pre-flight:** confirm what actually happens to day advance after `TriggerEnding`/`SealCampaign` today (stale-doc note in §2); confirm current endgame selftest names.
- **Acceptance:**
  0. All "Day 360 / 720" below mean the campaign's **profile** Reading day and chapter-two end day (defaults 360 / 720); the tests run against the legacy profile *and* one shifted profile.
  1. Day 360 → phase `Epilogue`; both verbs enabled (when §4.6 satisfied).
  2. PLAY ON → phase `Active`, `chapterIndex = 2`, Year One report kept as `chapters[0]`; **zero** completion-history appends, **zero** legacy archives, unified resolver **not** invoked, `SaveAll` behaves as a normal day save.
  3. SEAL HERE at Day 360 is **behaviorally identical to legacy** (golden: completion append count 1; legacy archive once; epilogue prose rewritten once).
  4. Terminal endings (extinction, frozen silence) never offer PLAY ON.
  5. Chapter-2 trigger at Day 720 (or extinction) produces a chapter-2 ending; final seal runs terminal effects exactly once; re-seal returns `AlreadyRecorded`.
  6. Schema-1 endgame saves load as chapter 1; reload between the Reading and the click preserves both options.
- **Verification:** `bin/run-scoped-tests` on `YearTwoPlayOnTests`, `EndgameSystemTests`, `Plan145UnifiedEndingHostIntegrationTests`, `Plan175MetaProgressionHostIntegrationTests`; `--unified-ending-selftest`; new `--year-two-chapter-selftest`; save round-trip gate; host build.

### P3 — Standing & quarterly Readings · *Role: Builder (Core + host)*
- **Outcome:** a pure Standing derivation, a Standing Sheet page, and four dated readings with three voices.
- **Seam:** `YearTwoStanding.Derive(ReckoningState, CampaignOutcomeSnapshot)` → standing + sheet sentence; `QuarterlyReadingLedger` nested in Verdict save; reading fired from the existing verdict day owner.
- **Acceptance:**
  1. Derivation is a **pure function**: same inputs → same output over ≥100 seeds; never mutates the verdict flags.
  2. Each of the **four** Standings yields its distinct instrument (Market Reading / Open Reading / Invoice / Approach) on **chapter-open day + 90k** (defaults 450/540/630/720); Standing D resolves into A/B/C on the Call and its earlier readings are recorded as Approach readings.
  3. Each reading fires **once**; replaying a day or reloading does not re-fire (ledger-idempotent).
  4. Standing C tiers (Full/Lapsed/Arrears) follow filed returns; no tier lowers a stat directly; effects are relay range and corridor availability via existing owners.
  5. Legacy/Dormant saves default to *no Standing*; Held-by-derivation (`evidence < 4`, no explicit choice) is read as Held without writing flags.
  6. Standing Sheet text contains only values from owners (no invented numbers).
- **Verification:** `bin/run-scoped-tests` on `YearTwoStandingTests`, `QuarterlyReadingLedgerTests`, `Ashfall.Core.Tests/Verdict/`; `--verdict-selftest`; save round-trip.

### P4 — Generations · *Role: Builder, four sequential sub-packages*
Shared rules: adult age from `AgingSystem`, child stage from `ChildDevelopmentSystem`; **no new ladder**; consent is real (a child can decline).

**4a — Apprentice pipeline**
- **Outcome:** an Adolescent with `vocational_apprenticeship` can be paired with a mentor through `ApprenticeshipSystem`, by consent.
- **Acceptance:** (1) pairing only offered at Adolescent+; (2) a declined pairing writes a record and does not re-prompt within the quarter; (3) pairing uses an existing `MentorshipDef` from the extended catalog (no parallel catalog); (4) `NotifyMentorDeath` transitions the apprentice to *acting-eligible*, not acting; (5) integrity validator passes.

**4b — Elders**
- **Outcome:** retirement eligibility surfaces from `AgingSystem`; a retiring founder can leave a Last Lesson (final wish) and a Handover object (heirloom).
- **Acceptance:** (1) eligibility = `age ≥ 65` under 30 d/yr, never from the 365 d/yr engine; (2) a Last Lesson grants one skill step to one named apprentice exactly once; (3) a Handover object moves through the existing heirloom/belongings owner, not a new list; (4) a retired elder can be posted to light duty at a waystation position.

**4c — Council**
- **Outcome:** on each reading day the Council presents candidates with evidence and three verbs (Ratify for the quarter / Extend / Decline).
- **Acceptance:** (1) candidate evidence is read from owners (education, milestones, mentor hours, kinship) with **no hidden rolls**; (2) Ratify writes a `designation` and calls the existing role/duty owner; a quarter later it lapses unless renewed; (3) `PerformSuccession` records the lineage fact for adult–adult successions; (4) a lapsed acting successor generates a memorial-free, ledger-only record; (5) determinism test across two runs; (6) legacy saves have an empty ledger.

**4d — Registration ("Do Not Write the Living")**
- **Outcome:** *write / leave unwritten* is a single command per child on the existing "who is written" owner (chosen in P0).
- **Acceptance:** (1) exactly one command path; (2) Standing A entitlement/callable, Standing C enrolled-dwelling count, Standing B diff **read** the same fact; (3) neither choice is penalized in stats; (4) the unwritten child's milestones and Rite still record in ChildDevelopment; (5) reversible only by an explicit second command with a journal line.

- **Verification (P4):** `bin/run-scoped-tests` on `YearTwoApprenticeLadderTests`, `YearTwoSuccessionTests`, `Ashfall.Core.Tests/Generations/`, `Plan217Genealogy*`, `ApprenticeshipIntegrationTests`; `--second-generation-milestones-selftest`, `--child-development-selftest`, `--aging-selftest`, `--genealogy-selftest`, `--apprenticeship-curriculum-selftest`.

### P5 — Thirteen & location binding · *Role: Builder + Integrator co-sign (validator, panel tab)*
- **Outcome:** a **12-bunk secondary position** (`tier: hearth`) bound to a real location (Allocation 13) plus the four existing outposts bound to real locations; Thirteen established via the existing command.
- **Seam:** additive `OutpostDef` fields: `tier`, `supply_mode` (`tether` default | `convoy`), `radio_warning_days`, `location_id`. Instance/DTO additive (`enrolled` for Standing C read).
- **Acceptance:**
  1. Thirteen definable and establishable through `TryEstablishOutpost` with cost from canonical inventory and garrison from canonical roster; capacity 12.
  2. A def with an unknown `location_id` **fails** the integrity validator; the four legacy outposts pass after retro-binding (locations chosen in P0).
  3. Children can be assigned a caregiver **at** Thirteen through the ChildDevelopment owner (no new list).
  4. Legacy `outpost_settlement` saves restore with new fields defaulted; definitions are not persisted.
  5. The F16b flag is read: an already-filed Thirteen opens filed; an unfiled one opens unfiled; neither is overwritten.
  6. Under Standing C, *enrolled/unenrolled* is readable from the instance and drives the relay/serve read model.
- **Verification:** `bin/run-scoped-tests` on `YearTwoThirteenTests`, `Plan58OutpostSettlementIntegrationTests`, `Plan58OutpostHostIntegrationTests`, `OutpostAtomicBillTests`; `--outpost-settlement-selftest`; data-integrity gate; save round-trip.

### P6 — The Road · *Role: Builder + Integrator co-sign (Main.OutpostSettlement, stream ids)*
- **Outcome:** supply is a journey; danger is real and comes from named sources; overrun has a relief journey.
- **Seam:** `OutpostPressureModel.Evaluate(inputs) → int danger` (pure); `Main.WorldDangerRatingForDay` becomes a thin host read of that model; `convoy` supply mode consumes only the outpost's **own reserve**; delivery arrives through an expedition and calls `TrySupplyOutpost`.
- **Acceptance:**
  1. A `convoy` outpost's reserve **never** increases from the daily free draw; a `tether` outpost behaves exactly as before (regression-pinned).
  2. A delivered manifest debits canonical inventory **once** and credits the reserve **once**; an interrupted run credits nothing and returns unspent cargo.
  3. Danger > 0 only from named sources (warlord doctrine, faction/territory state, route hazard, Standing); a quiet day draws **no RNG**; a fixed seed yields the same raid days across two runs (golden).
  4. A working relay gives at least one warning day before a raid; an isolated position gives none.
  5. Relief is a journey: an overrun position is restored only through a completed relief run (or the existing paid re-establish, which is retained), never a bare button.
  6. Grace window: no raid can strike Thirteen within N days of establishment (N from data), and a tutorial signal precedes the first raid.
  7. Waystation leg quality (watch present + filter healthy) measurably shortens a leg or reduces exposure; a dead filter with no watch does neither.
- **Verification:** `bin/run-scoped-tests` on `YearTwoRoadTests`, `Plan58Outpost*`, `Plan56Phase3Tests`, `Plan56Phase6Tests`; `--outpost-settlement-selftest`; waystation selftest (name VERIFY); `--expedition-selftest`; save round-trip.

### P7 — Year Two Chronicle & endings · *Role: Builder (Core + data)*
- **Outcome:** the nine-permutation Chronicle on Day 720, with Standing paragraphs and Year Two memorial names, feeding the final seal and cross-run legacy.
- **Acceptance:**
  1. All **nine** permutations reachable by fixtures; each yields a distinct id and non-empty text.
  2. Pure and deterministic given persisted state (same state → same ending, two runs).
  3. Memorial names come from `MemorialSystem` for deaths with `day > 360` only.
  4. Final seal invokes existing terminal effects **once**; `CampaignLegacy` receives ≥ 1 trait derived from generations (e.g., `heirs_named` → an inheritable trait).
  5. `heirs_none × one_door` ("The Sealed Count") never softens; no permutation contradicts the tone lock (reviewed).
- **Verification:** `bin/run-scoped-tests` on `YearTwoOutcomeTests`, `CampaignOutcomeEvaluatorTests`, `Plan19EndingContinuityTests`; `--unified-ending-selftest`, `--epilogue-chronicle-selftest`, `--campaign-legacy-selftest`.

### P8 — Content waves · *Role: Builder (prose/data) + Reviewer (tone)*
Four bounded waves, one per quarter, each **prose-led** and integrated only through existing loaders.
| Wave | Quarter | Minimum content | Depends on |
|---|---|---|---|
| **W1** | Q1 *Concessions* (361–450) | Five Days pool (≥12 lines); Standing Sheet sentence table; 3 quests (grief, mud roads, first reading); Reading radio ×3 standings; 6 encounters | P1, P2, P3 |
| **W2** | Q2 *Second Door* (451–540) | Thirteen chain (≥4 beats); waystation keeper follow-ups ×14 (one line each); 4 road encounters; Council #1 scene | P4c, P5 |
| **W3** | Q3 *Short Summer* (541–630) | *The Night the Relay Goes Dark* (3 causes); raid encounters ×4; bloom/late-snap events ×3; Council #2 | P6 |
| **W4** | Q4 *Handing Over* (631–720) | Second Winter events ×4; Council #3 & #4; elder Last Lesson scenes; *The Night Before* close | P4b/c, P7 |
- **Acceptance (per wave):** (1) data-integrity gate passes; (2) content-utilization scan shows a consumer for every new id; (3) headless `--year-two-quarter-selftest <n>` (new) plays the quarter without exception; (4) tone review sign-off (no explained idioms, no depicted harm to children, no real-world referents); (5) `minDay ≥ 361` on every entry and none beyond 720.
- **Verification:** `NarrativeEncounterSystemTests`, data-integrity, content-utilization, and the quarter selftest. Full-suite not run.

### P9 — Governance close · *Role: Foreman / Integrator*
- **Outcome:** stale docs fixed; debt rows filed; ledger entry written; plan sealed and archived.
- **Acceptance:** (1) `ENDGAME_V1.md` corrected (selftest name, sealing-halt claim, Play On described); (2) `KNOWN_DEBT.md` rows for F3 thin ctx, 365-day generational clock, `ColonySystem` deliberately separate, `graph_node_id` legacy binding; (3) `INTEGRATION_PLANS.md` entry (foreman only); (4) this plan's header replaced with the multi-line FULLY INTEGRATED banner and the file moved to `.ai/plans/integrated/<category>/` — **immediately**, same session, per CLAUDE.md §8; (5) docs index regenerated by its owning generator (`--check` mode passes).

---

## 6. Decision register (signatures required — Rule 10)

Foreman/user signature is required for each before the package that depends on it starts. **Defaults are recommendations, not approvals.**

| ID | Decision | Depends | Recommended default | Blocks |
|---|---|---|---|---|
| DEC-Y2-01 | **Canon extension:** Thirteen has been kept by a stale maintenance schedule (never a keeper, never a miracle). | canon | Approve | P5, W2 |
| DEC-Y2-02 | **REVISED by user 2026-09-29 — DECIDED:** Reckoning day and Reading day are **per storyline** via Chapter Profiles; default profile = Call 240 / Reading 360; Five Days interlude and 360/365 reconciliation apply to the default profile. | canon | **Decided (user)** | P1, P1B, P2 |
| DEC-Y2-03 | Climate authority: **timeline reads `year_two_climate.json`**; calendar baseline delegates or is read-model-only for year ≥ 2. Rejects both replaying −45 °C and freezing at +4 °C. | architecture | Approve | P1 |
| DEC-Y2-04 | Held + `TempestSterilization` conditions: live soft advisory or epilogue-only. | design | **Epilogue-only in v1** | P3 |
| DEC-Y2-05 | Late Presentation of a held count in Year Two. | design | **No in v1** | P3 |
| DEC-Y2-06 | The four legacy outposts stay on `tether` supply; only new positions use `convoy`. | compatibility | Yes | P5, P6 |
| DEC-Y2-07 | Council ledger lives in `apprenticeship` or `genealogy` section. | architecture | Decide in P0 audit (prefer the owner whose save DTO already carries designations/kinship) | P4c |
| DEC-Y2-08 | UI: pages on existing panels; **no new routed panel** in v1. | UI seams (`INT`) | Yes | P2–P4, P5 |
| DEC-Y2-09 | **REVERSED by user 2026-09-29 — DECIDED:** Year One ending selection is **per storyline** (F3 retired for new campaigns). Legacy profile keeps today's behavior for saves in progress. | compatibility | **Decided (user)** | P1B |
| DEC-Y2-12 | Chapter Profile catalog `chapter_profiles.json`: fields, selection, legacy migration, family defaults for 135 branch endings (prose §5.7). | architecture | Approve; values signed in P1B | P1B |
| DEC-Y2-13 | Verdict timing through one boundary adapter (`verdictDay = campaignDay − offset`); no rewrite of authored verdict day gates. | architecture | Approve | P1B |
| DEC-Y2-14 | Standing D (The Late Call) + per-profile evidence-gate waiver. | design | Approve | P3 |
| DEC-Y2-10 | Registration owner ("who is written") among Standing Record / Voluntary Register / census. | architecture | Decide in P0 audit | P4d |
| DEC-Y2-11 | `exodus_to_sea` at Day 360 is a Standing *modifier* (a chosen part of the roster sails as a friendly port; not a deletion, not terminal). | design | Approve | P3, P7 |

---

## 7. Dependency order and parallelism

```
P0 ─► P1 ─► P1B ─► P2 ─┬─► P3 ─────────────┐
                ├─► P4a─►P4b─►P4c─►P4d ─┤
                └─► P5 ─► P6 ────────┤
                                     └─► P7 ─► P9
Content: W1 after P1+P2+P3 · W2 after P4c+P5 · W3 after P6 · W4 after P4b/c+P7
```

- **Disjoint parallel builders** after P2: P3, P4, P5 touch disjoint paths (Verdict, Survivors/Apprenticeship, Settlements). P6 follows P5 (same owner file). P7 needs P3 + P4 + P6.
- **Shared seams stay integrator-only:** `Main.CampaignOwners.cs`, `Main.SaveOrchestrator.cs`, `SaveSectionRegistry.cs`, `HostCliRegistry.cs`, `PanelRegistryBootstrap`, `Main.UiPanels`, `PlayerSurfaceManifest`, `CatalogIntegrityValidator.cs`, `CampaignStreamIds.cs`, `CampaignCalendar.cs`.
- **Minimum shippable slice:** P0–P3 + W1 = *the Reading, the Standing Sheet, one honest quarter.* If schedule collapses, this alone converts "epilogue" into "play on" for Q1. The §4.6 ship-dark rule keys on catalog presence, **not** content completeness, so a build that ships only this slice must either (a) also ship a minimal Q2–Q4 filler set from W2–W4's first beats, or (b) have the foreman sign a documented "Chapter Two is Q1-only" cut with the Chronicle closing at Day 450. Do not ship PLAY ON into a chapter that goes silent after Day 450.

---

## 8. Implementation steps (program-level, max 60–100)

1. **P0.1** Re-verify F1–F17 at `path:line`; record CONFIRMED / STALE / CHANGED.
2. **P0.2** Census 360-day constants; classify each.
3. **P0.3** Locate: Verdict host session; role owner; expedition dispatch host; registration owner; F16b flag ids; retro-binding locations for the four outposts.
4. **P0.4** Write `Y2_PREMISE_EVIDENCE.md` and `Y2_DECISION_PACKET.md`; foreman signs DEC-Y2-01…14.
5. **P0.5** Foreman: ledger entry + claims for P1 and P2 (exact paths from §3).
6. **P1.1** Author `year_two_climate.json` (four phases; tuning knobs; validated ranges).
7. **P1.2** `YearTwoClimateCatalog` loader (strict; unknown/overlap/gap = error).
8. **P1.3** Extend timeline for `day > 360` reading the catalog; leave ≤ 360 code path untouched.
9. **P1.4** Capture 1→360 golden **before** editing; commit the golden fixture with the tests.
10. **P1.5** Additive `YearOfAshSave` fields; legacy default; round-trip test.
11. **P1.6** Calendar reconciliation per DEC-Y2-03.
12. **P1.7** Fix only the F5 constants P0 proved to be runtime clamps (one claim each).
13. **P1.8** Headless 361→720 run: thermal/radon/ice-road change; assertions.
13b. **P1B.1** Author `chapter_profiles.json` (legacy + 5 family profiles); loader + validator.
13c. **P1B.2** `ChapterProfileResolver` (pure) + persist profile id in `endgame`; legacy default for old saves.
13d. **P1B.3** `ReckoningSystem` optional thresholds; `ReckoningClock` adapter at the Verdict host boundary; legacy-parity golden **before** editing.
13e. **P1B.4** Profile-driven `EvaluateEnding` dispatch (`legacy_context` untouched); branch-ending source from `FactionBranchCoordinator`.
13f. **P1B.5** Standing-modifier coverage rule for the 135 branch endings in the integrity validator.
13g. **P1B.6** Close rules: fixed day or ending-resolved + settle window with floor/ceiling.
14. **P2.1** `EndgameSaveState` schema 2 + `chapters[]`; legacy default; schema-gated restore.
15. **P2.2** `EndgameSystem.ContinueChapter`, chapter thresholds (360, 720), terminal-ending guard.
16. **P2.3** `Main.CheckAndTriggerEndgame` reads the profile's close rule and chapter thresholds; Year One ending source from the profile (`legacy_context` remains byte-compatible).
17. **P2.4** `OnCampaignSealed` runs only when `chapter is final` or player chose SEAL HERE.
18. **P2.5** `ChroniclePanel`: PLAY ON / SEAL HERE + Year One page; focus/back preserved.
19. **P2.6** Golden-test the legacy SEAL HERE path *before* editing `OnCampaignSealed`.
20. **P2.7** `--year-two-chapter-selftest` descriptor and body.
21. **P2.8** Enforce §4.6 ship-dark rule; test the flag-off path equals today.
22. **P3.1** `YearTwoStanding.Derive` (pure) + tests.
23. **P3.2** Sentence tables and Standing definitions in `year_two_chapter.json`.
24. **P3.3** `QuarterlyReadingLedger` nested in `VerdictSave` (additive).
25. **P3.4** Reading firing in the existing verdict day owner; idempotency test.
26. **P3.5** Lease tiers (Full/Lapsed/Arrears) → relay/corridor read model.
27. **P3.6** Standing Sheet page on `ChroniclePanel`.
28. **P3.7** `year_two_radio.json` — three voices × four readings.
29. **P4a.1** Extend `apprenticeship_catalog.json`; pairing offer at Adolescent+; consent; decline record.
30. **P4a.2** `NotifyMentorDeath` → acting-eligible; tests.
31. **P4b.1** Retirement eligibility from `AgingSystem`; UI line.
32. **P4b.2** Last Lesson via final-wish owner; Handover via heirloom owner.
33. **P4b.3** Light-duty posting for retired elders (waystation/Thirteen).
34. **P4c.1** Council ledger nested per DEC-Y2-07; capture/restore.
35. **P4c.2** Council page: candidates, evidence, three verbs.
36. **P4c.3** Ratify → role/duty owner + `PerformSuccession`; quarterly lapse.
37. **P4c.4** Determinism and legacy-save tests.
38. **P4d.1** Registration command on the chosen owner; journal line.
39. **P4d.2** Wire Standing A/B/C instruments to read it.
40. **P4-x** Read-only clock-consistency check (no retired 30-year-olds, no adult children before Day 721).
41. **P5.1** Extend `OutpostDef`/DTO additively; validator location-binding rule.
42. **P5.2** Retro-bind the four legacy outposts; regression tests unchanged-green.
43. **P5.3** Add Thirteen (12 bunks, `tier: hearth`, `convoy`), read F16b flag.
44. **P5.4** Nursery/caregiver assignment at Thirteen via ChildDevelopment owner.
45. **P5.5** Enrolled/unenrolled read model for Standing C.
46. **P5.6** Shelter Operations panel network tab (integrator co-sign).
47. **P6.1** `OutpostPressureModel` (pure) + tests.
48. **P6.2** Replace `WorldDangerRatingForDay` stub; grace window from data.
49. **P6.3** `convoy` supply mode in `OutpostSettlementSystem`; `tether` regression pin.
50. **P6.4** Supply run through `ExpeditionSystem`; manifest debit/credit exactly-once tests.
51. **P6.5** Waystation leg-quality read; radio warning day; relief journey.
52. **P6.6** New `CampaignStreamIds` entries (integrator).
53. **P7.1** `YearTwoOutcomeEvaluator` (pure) + nine-fixture tests.
54. **P7.2** `year_two_endings.json` (nine + Standing paragraphs).
55. **P7.3** Chapter-aware `UnifiedEndingResolver` (additive); final seal once.
56. **P7.4** `CampaignLegacy` traits from generations.
57. **P8.W1–W4** Content waves per §5, each with its own approved plan and gates.
58. **P9.1** Fix `ENDGAME_V1.md`; regenerate docs index; add KNOWN_DEBT rows.
59. **P9.2** Foreman ledger entry; **replace this file's header with the FULLY INTEGRATED banner and move it to `.ai/plans/integrated/<category>/`.**

---

## 9. Proposed ledger entry (for the foreman — not applied)

> **YEAR TWO — THE LONG THAW (PROPOSED PROGRAM):** story-director plan `docs/expansions/expansion_year_two_the_long_thaw_plan.md`, integration plan `.ai/plans/year-two-the-long-thaw-2026-09-29.md`. Packages P0–P9; decisions DEC-Y2-01…14 unsigned. Play on after the Reckoning (Days 361–720): Standing from the verdict, generations arc ending the night before the first Rite (Day 720; earliest coming-of-age Day 721 by the F7 age floor), outposts network with a second shelter (Allocation 13) under signed custody. No new save section; three nested additive structures. **Not authorized until STATUS: APPROVED BY USER.**

**Proposed claim shapes** (exact paths per §3): `claim-year-two-p0-audit-…`, `claim-year-two-p1-horizon-…`, `claim-year-two-p2-play-on-…`. One package per claim; shared paths listed as integrator-only.

---

## 10. Debt this program will file (P9) and known limitations

| Item | Nature |
|---|---|
| F3 — host ending ctx omits four inputs; only 3 of 8 endings reachable from the host trigger. | Pre-existing; **retired for new campaigns by P1B** (DEC-Y2-09 reversed); legacy profile retains the thin context for saves in progress — close the row when the legacy profile is retired. |
| `GenerationalSuccessionEngine` ages at 365 d/yr; `AgingSystem` at 30 d/yr; children by stage-days. | Pre-existing three clocks; Year Two reads two, ignores one. |
| `outposts.json` `graph_node_id` legacy values. | Retro-bound in P5; the old field is left in place for compatibility. |
| No chapter 3. | Intentional; state left ready. |
| Held-branch live "sweep advisory" and Late Presentation. | Deferred by DEC-Y2-04/05. |
| Full-scale 720-day runtime-scale budget (current gates: 30/180/360). | Add a bounded 720-day workload in P9 or a follow-up; **do not** run unbounded. |

---

## 11. Handoff format (AI_AGENT_WORKFLOW)

Every package returns:

```text
Package:
Outcome:
Files changed:
Current contract used:
Verification commands and results:
Tests reused / added / aggregated:
Known limitation or debt:
Shared files intentionally untouched:
Ready for sweep: yes/no
```

Sweep findings use: `finding_id | severity | confidence | path:line | current evidence | expected contract | proposed owner`. A finding without path-level evidence is discarded.

---

## 12. Pre-flight Checks

- [ ] `bin/ashfall-dev validate-config` / `bin/validate-config` passes on current data **before** any Year Two data lands.
- [ ] No equivalent existing system found — **searched 2026-09-29:** no `year_two*` catalog, no chapter concept in `EndgameSystem`, no post-360 timeline, no supply-run/convoy for outposts, no hostile-pressure source (F11), no council/designation ledger. Prior proposals 44/82/46/12 are documentation-only and unapproved.
- [ ] P0 has re-verified every F-row and signed DEC-Y2-01…14.
- [ ] `WORKTREE_OWNERSHIP.md` re-read; no live claim on the package's exact paths.
- [ ] Working tree: unrelated dirty changes preserved; nothing mass-formatted.
- [ ] `.ai/state.md` read before acting; updated at the end of each package (files, tests, remaining errors).

## 13. Verification (program level)

- [ ] Scoped tests pass via `bin/run-scoped-tests` (< 30 s per package; medium tier before a feature commit). **Never** the full suite without the literal `RUN FULL TESTS`.
- [ ] Max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md` for a bug validator.
- [ ] Host build 0 errors; changed selftests green; save round-trip gate green.
- [ ] **Program acceptance (after P7 and W1–W4):**
  1. Headless 720-day run from a fresh seed: Day 360 → PLAY ON → Day 720 → Chronicle → single seal, no exceptions.
  2. Round-trip at Days 359, 361, 450, 600, 719.
  3. Two runs, same seed → identical Chronicle id.
  4. `SaveSectionRegistry` section count unchanged (three nested additive structures only).
  5. Day-360 SEAL HERE golden identical to pre-change.
  6. Days ≤ 360 timeline golden identical to pre-change.
- [ ] Godot runtime session (if needed) at **15 FPS** unless the user requests otherwise. No Unity.
- [ ] Do not commit until the user asks; when committing, end the message with the attribution line required by the session reminder.

## 14. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep the Long Thaw larger than the nine Chronicles that meter it. Any future plan that answers one
must name the signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| Y2-OM-1 | What is Standing **D — The Late Call** waiting for? | P3 makes it a real grade that resolves into A/B/C. Until it resolves it is vocabulary the shelter is living inside, and that is the design. | Never — locked by the program's own structure. |
| Y2-OM-2 | What happens the night after the chapter's last line? | The program ends at Day 720 with a single seal (§1 Non-Goals: no Chapter Three). The silence after the last line is the point. | Never — scope-locked. |
| Y2-OM-3 | What would the first Rite of Passage have been? | The chapter stops **the night before** it. By design (F7 age floor). Writing the rite would spend the program's best silence. | Never — a rule, not a gap. |
| Y2-OM-4 | Is the second shelter a return or a departure? | P5 binds 12 bunks to a real location (Allocation 13) and stops there. Custody of meaning is not asserted. | *The Record Keepers*, if a hearth is ever archived as a Place. |
| Y2-OM-5 | Why four quarterly readings in four voices? | P3 authors four voices and never reconciles them. The disagreement is the instrument. | Never — texture by omission. |
| Y2-OM-6 | What was the Reckoning? | It is a boundary the game crosses and a day the player chose. Its meaning in the world's mechanics, as opposed to the shelter's story, is never authored. | Never — a rule, not a gap. |
