# Feature / Task Plan: Year Two — P0 — Premise audit & decision packet

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P0); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

> **Editorial polish (prose pass, non-contractual):** the Prologue and texture notes below are
> narrative texture and writing guidance only. They change no acceptance criterion, no claimed path,
> no decision and no verification step. All binding criteria remain in the umbrella
> (`year-two-the-long-thaw-2026-09-29.md`) §5/§6/§8.

---

## 0. Prologue — The Ground Beneath

> *"Nothing is built in this package. Everything else depends on it. That is what a foundation is."*

P0 is the least glamorous card in the program and the only one nobody may skip. It writes two
documents and edits no source. Its whole product is the difference between a plan that knows where
it is standing and a plan that is guessing in a file that changed an hour ago.

F1–F21 come back **CONFIRMED**, **STALE**, or **CHANGED**. Three words, and the third one is the
one that matters. A premise that has changed is not a failure of the earlier audit; it is the
repository being alive, and the only sin would be to build on the old shape anyway.

**Tone & register.** Scrupulous, plain, unglamorous. The vocabulary is the audit: *premise, path,
line, census, constant, clamp, inert, window*. Prose should sound like someone checking a list
they have checked before and will check again. Never dramatise verification. Its drama is that
nobody notices it when it works.

**Mystery & texture.** P0.2's census of 360-day constants sorts them into *runtime clamp*, *inert
default*, and *content window* — three kinds of number that look identical in source and mean
three entirely different things. That taxonomy is the card's quiet intellectual payload. §6 keeps
what it cannot classify.

---

## 1. Goal & Outcome

> *Design intent: measure the ground before you promise to build on it. This card is the reason
> every other card's stop conditions are meaningful.*

- **Goal:** Re-verify F1–F21 at path:line; census 360-day constants and verdict day-gate consumers; locate host owners; draft per-storyline profile table; restate open decisions for one-line confirm.
- **Non-Goals:** No source/data/ledger edits. No code. No tests.
- **Start gate:** None (predecessor of all)

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- docs/plans/year_two/Y2_PREMISE_EVIDENCE.md (new)
- docs/plans/year_two/Y2_DECISION_PACKET.md (new)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P0.1 Re-verify F1–F21 (CONFIRMED/STALE/CHANGED).
2. P0.2 Census 360-day constants; classify runtime clamp / inert default / content window.
3. P0.3 Name: Verdict host session, role owner, expedition dispatch host, registration owner, F16b flag ids, four-outpost retro-binding locations, verdict day-gate consumers.
4. P0.4 Draft storyline table (family + first distinct branches) for P1B signature.
5. P0.5 Write both docs; foreman confirms DEC-Y2-01,-04…-08,-10,-11 defaults.

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P0 all met
- [ ] static only; `git diff --check` on the two docs (no tests).
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated

## 6. Open Mysteries & Deliberate Silence (lore register — texture only, not acceptance criteria)

These questions are **intentionally unanswered** — not gaps, not TODOs. All binding criteria remain
in the umbrella §5/§6/§8. Any future plan that answers one must name the signed decision.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| P0-OM-1 | What happens to a premise marked **CHANGED**? | P0.2 classifies constants and P0.1 classifies rows; neither authors a consequence story. The word does its work and stops. | Never — a rule, not a gap. |
| P0-OM-2 | Who counted the 360-day constants in the first place? | The census is performed now (P0.2). Their origin is not recorded and the card declines to speculate. | Never — the artefact reads as history. |
| P0-OM-3 | What is an *inert default* for? | The taxonomy distinguishes it from a clamp and a window. Why the constant exists at all is not the census's business. | The owning subsystem, if it ever documents it. |
| P0-OM-4 | Why F1–F21 and not more? | The umbrella authors 21 premises and stops. Whether the list is complete is not asserted. | Never — texture by omission. |
| P0-OM-5 | Does a one-line decision confirm mean the same as signing it? | P0.5 asks for confirmation "for one-line confirm". The card treats it as binding and does not philosophise about consent. | Never — a rule, not a gap. |
