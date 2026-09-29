# Feature / Task Plan: Year Two — P8 — Content waves W1–W4 (one quarter each)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P8); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

> **Editorial polish (prose pass, non-contractual):** the Prologue and texture notes below are
> narrative texture and writing guidance only. They change no acceptance criterion, no claimed path,
> no decision and no verification step. All binding criteria remain in the umbrella
> (`year-two-the-long-thaw-2026-09-29.md`) §5/§6/§8.

---

## 0. Prologue — Four Quarters

> *"Content is what a year feels like from the inside. Mechanics only decide when."*

One wave per quarter, four quarters, and every one of them **prose-led**: quests, radio, encounters,
keeper follow-ups. Nothing here is a new mechanic. Everything here is a new *sentence*, placed
where a year would have had one.

The integration rule is the hard part and the whole craft: *only through existing loaders*. Content
that cannot be reached through a loader that already exists is content that does not ship. That
constraint is what keeps four waves of writing from becoming four waves of architecture.

**Tone & register.** Seasonal, patient, writerly. The vocabulary is the working day of a content
designer: *wave, quarter, quest, radio, encounter, follow-up, loader, minDay*. Prose should sound
like a schedule kept by someone who knows that the fifth quarter does not exist and must never be
improvised.

**Mystery & texture.** `No explained idioms` is a Non-Goal with real teeth: the writing may use
phrases, names and customs the player is never given a glossary for. §6 keeps open what the waves
are not permitted to explain.

---

## 1. Goal & Outcome

> *Design intent: make a year feel lived-in, and never explain an idiom that only needs to be
> overheard.*

- **Goal:** Prose-led quests, radio, encounters, keeper follow-ups per quarter, integrated only through existing loaders.
- **Non-Goals:** No new mechanics; nothing outside `minDay ≥ profile chapter-open day` and ≤ chapter end; no explained idioms; no depicted harm to children.
- **Start gate:** W1: P1+P1B+P2+P3 · W2: P4c+P5 · W3: P6 · W4: P4b/c+P7

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- Assets/StreamingAssets/Data/year_two_quests.json, year_two_radio.json, narrative_encounters_year_two.json (new, wave by wave)
- keeper follow-up data path (from P0)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. Each wave is its own claim/handoff; content minimums per umbrella §5 P8 table.

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P8 all met
- [ ] data-integrity gate; content-utilization scan; `NarrativeEncounterSystemTests`; new `--year-two-quarter-selftest <n>` (INT descriptor).
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated

## 6. Open Mysteries & Deliberate Silence (lore register — texture only, not acceptance criteria)

These questions are **intentionally unanswered** — not gaps, not TODOs. All binding criteria remain
in the umbrella §5/§6/§8. Any future plan that answers one must name the signed decision.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| P8-OM-1 | What are the unexplained idioms? | `No explained idioms` is a Non-Goal. The writing may use phrases the player is never given a glossary for. | Never — locked by scope. |
| P8-OM-2 | Why one wave per quarter and not four per year? | The cadence is authored. A wave is a delivery schedule that reads as a season. | Never — the artefact reads as custom. |
| P8-OM-3 | What does a keeper follow-up follow up *from*? | The data path is "from P0" and the content is prose. Its antecedent is not authored here. | *The Record Keepers*, if a follow-up is ever a record. |
| P8-OM-4 | Who is on the radio in Year Two? | `year_two_radio.json` is new and voice is content. P3's four voices are never reconciled and may or may not overlap. | Never — texture by omission. |
| P8-OM-5 | What happens in the fifth quarter? | Non-Goals bounds content to `minDay ≥ profile chapter-open day` and ≤ chapter end. There is no fifth quarter and there must not be one. | Never — a rule, not a gap. |
