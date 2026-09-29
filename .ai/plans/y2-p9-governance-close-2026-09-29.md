# Feature / Task Plan: Year Two — P9 — Governance close

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P9); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

> **Editorial polish (prose pass, non-contractual):** the Prologue and texture notes below are
> narrative texture and writing guidance only. They change no acceptance criterion, no claimed path,
> no decision and no verification step. All binding criteria remain in the umbrella
> (`year-two-the-long-thaw-2026-09-29.md`) §5/§6/§8.

---

## 0. Prologue — Sealing

> *"A plan that is never closed is not a plan. It is a rumour about work."*

P9 writes no code. It fixes stale documents, files debt rows where they belong, writes the ledger
entry, and then does the one irreversible administrative act in the whole program: it marks the
family **FULLY INTEGRATED** and moves every derived plan into the archive — immediately, even when
the work was finished in the same session that wrote the plan.

That last instruction is the interesting one. Same-session archiving is not bureaucracy; it is the
refusal to leave a finished thing sitting in the active pile where the next agent will mistake it
for work. A sealed plan becomes **evidence**. An unsealed one stays a claim.

**Tone & register.** Clerkly, final, quietly proud. The vocabulary is the registry: *stale, debt
row, ledger, seal, archive, generator, --check*. Prose should sound like someone closing a file
they are not going to reopen, and meaning it.

**Mystery & texture.** `docs index (via owning generator)` is the card's one line of real
discipline: generated outputs are never edited by hand. The index of everything this program
learned is itself a generated artefact, which means the record of the work is not authored by the
people who did it. That is correct, and it is also slightly strange. §6 leaves it there.

---

## 1. Goal & Outcome

> *Design intent: end cleanly. A finished plan that is still in the active pile is a plan that will
> be re-done.*

- **Goal:** Stale docs fixed, debt rows filed, ledger entry written, plan sealed and archived.
- **Non-Goals:** No new code.
- **Start gate:** P7 and W1–W4 accepted

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- docs/endgame/ENDGAME_V1.md
- docs index (via owning generator)
- KNOWN_DEBT.md / INTEGRATION_PLANS.md (foreman only)
- this plan family: header + move to integrated

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P9.1–P9.2 per umbrella §8; mark FULLY INTEGRATED and move every derived plan immediately.

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P9 all met
- [ ] generator `--check` mode passes; `git diff --check` on edited docs.
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated

## 6. Open Mysteries & Deliberate Silence (lore register — texture only, not acceptance criteria)

These questions are **intentionally unanswered** — not gaps, not TODOs. All binding criteria remain
in the umbrella §5/§6/§8. Any future plan that answers one must name the signed decision.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| P9-OM-1 | What does a *stale* document remember? | P9 fixes stale docs and files no account of what was in them. The correction is the record. | Never — a rule, not a gap. |
| P9-OM-2 | Who reads the debt rows? | They are filed to `KNOWN_DEBT.md`, which is foreman-owned. The card files and does not follow up. | The foreman, in a later batch. |
| P9-OM-3 | Why is the docs index generated and not written? | `docs index (via owning generator)` and `--check` mode. The record of the work is not authored by the people who did it — correct, and slightly strange. | Never — tooling policy. |
| P9-OM-4 | What does sealing a plan *do* to it? | It marks it FULLY INTEGRATED and moves it to the archive immediately. Whether it becomes evidence or merely history is left unsaid. | Never — texture by omission. |
| P9-OM-5 | Is there a plan to close the closers? | P9 closes this family. Nothing closes the process that produces families. | Never — a rule, not a gap. |
