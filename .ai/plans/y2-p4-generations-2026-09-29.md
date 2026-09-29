# Feature / Task Plan: Year Two — P4 — Generations (4a apprentices · 4b elders · 4c council · 4d registration)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P4); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

## 1. Goal & Outcome
- **Goal:** Children→apprentices→acting successors; elder handover; quarterly Council; register/leave-unwritten choice. No new ladder; consent is real.
- **Non-Goals:** No ratified successor from a raised child before Day 721; GenerationalSuccessionEngine 365 d/yr clock is not read; no new role ledger.
- **Start gate:** P2 accepted (4c also needs P0 DEC-Y2-07; 4d needs DEC-Y2-10)

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- Assets/StreamingAssets/Data/apprenticeship_catalog.json (extend)
- src/Host/ApprenticeshipHostSession.cs, src/UI/ApprenticeshipPanel.cs
- src/Host/AgingHostSession.cs and final-wish/heirloom host bindings (files from P0)
- Assets/Ashfall.Core/Survivors/SuccessionCouncilLedger.cs (new; nested in owner per DEC-Y2-07) + that owner's save DTO (additive)
- src/Host/GenealogyHostSession.cs or ApprenticeshipHostSession.cs
- registration owner (DEC-Y2-10)
- Ashfall.Core.Tests/Survivors/YearTwoSuccessionTests.cs, Ashfall.Core.Tests/Generations/YearTwoApprenticeLadderTests.cs (new)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P4a.1–P4a.2, P4b.1–P4b.3, P4c.1–P4c.4, P4d.1–P4d.2, P4-x per umbrella §8; sub-packages sequential, each a separate handoff.

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P4 all met
- [ ] `bin/run-scoped-tests` on YearTwoApprenticeLadderTests, YearTwoSuccessionTests, Ashfall.Core.Tests/Generations/, Plan217Genealogy*, ApprenticeshipIntegrationTests; `--second-generation-milestones-selftest`, `--child-development-selftest`, `--aging-selftest`, `--genealogy-selftest`, `--apprenticeship-curriculum-selftest`.
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated
