# Feature / Task Plan: Shelter Governance — the Assembly: laws, courts and internal opposition

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_shelter_governance_plan.md`. Family index: `docs/expansions/expansion_new_ways_to_play_index.md`.
> Not a claim. Reader-and-router over five existing governance owners; none is merged or replaced.

## 1. Goal & Outcome
- **Goal:** Make five governance authorities feel like one **Assembly**: one scope vocabulary; a statute book the shelter enacts/repeals; trials with named roles that shape verdict *reception*; an announced **opposition ladder** (murmur → petition → walkout → sit-in → schism or challenge) driven by existing bloc grievance; precedents; a legitimacy-versus-fear ledger.
- **Outcome (observable):** on a fixed seed, enacting an opposed statute raises a bloc's grievance; crossing a threshold announces then triggers a walkout that reduces named duty output through the duty roster; a concession lowers grievance; a schism removes named survivors as a new faction; state survives save/load.
- **Non-Goals:** no new political/legal authority; no change to trial verdict rules; no new save section; no new routed panel; no real-world ideology labels; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `PoliticsSystem` state (approval, legitimacy, mode, coup risk, elections) + commands; section `settlement_politics`; 6 political policies. | `Narrative/PoliticsSystem.cs`; `political_policies.json`; `src/Main.Politics.Integration.cs` | LIVE |
| E2 | Daily politics call passes `guardDeficiency` = 0. | `src/Main.SubsystemComposition.cs` L488 | GAP / VERIFY |
| E3 | `PolicySystem` (scopes: `rations`, `curfew`, `emergency_override`; options; costs; proposer rules; decision method). | `Governance/PolicySystem.cs`; `policies.json` | LIVE (thin) |
| E4 | `ShelterGovernanceEngine`: blocs, membership, consent, grievance, disputes, stability rating, tick; section `shelter_governance`; daily owner phase 5. | `Governance/ShelterGovernanceEngine.cs`; `src/Main.CampaignOwners.cs` L177 | LIVE |
| E5 | Bloc scope vocabulary (~21 words) vs policy scopes (3); exact-string match in `EvaluatePolicyConsent`. | `shelter_governance_blocs.json`; `policies.json`; engine L367–410 | GAP (VERIFY by test) |
| E6 | `JusticeSystem`: incident, evidence, `HoldTrial(TrialDecision)`; 3 verdicts; 6 punishments; 6 crime types. Host-wired. | `Narrative/JusticeSystem.cs`; `src/Main.Justice.Integration.cs` | LIVE |
| E7 | 4 laws; Hoarding, Desertion have none. | `wasteland_laws.json` | LIVE / GAP |
| E8 | `LeadershipSystem` (5 policies, elect/designate/step down/successor/deputy/`ResolveChallenge`). | `Survivors/LeadershipSystem.cs`; `leadership_policies.json` | LIVE (VERIFY daily integration) |
| E9 | Grievance/stability have no action outputs. | grep `Governance/` | GAP |
| E10 | Reputation dimensions/tags. | `Reputation/ShelterReputationSystem.cs` | LIVE |
| E11 | Year Two Council of Succession (`designations[]`) proposed. | Year Two umbrella P4c / DEC-Y2-07 | PROPOSED elsewhere |
| E12 | Duty roster owns work assignment/refusal; where a "work refusal" penalty can be applied through its public API. | `DutyRoster/DutyRosterSystem.cs` | **VERIFY (P0)** |
| E13 | Panels: `JusticeTribunalPanel`, `PoliticsUI`; governance surface unconfirmed. | `src/UI/` | VERIFY |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Approval/legitimacy/elections/martial law/coup | `PoliticsSystem` | read; writes only via public commands |
| Policy options/scopes | `PolicySystem` | read/enact via `SetPolicy` |
| Blocs/grievance/disputes/stability | `ShelterGovernanceEngine` | read; `AdjustBlocGrievance` via public API |
| Trials/evidence/punishments | `JusticeSystem` | read; role assignment *around* a trial; reception shaping |
| Leadership/succession/challenge | `LeadershipSystem` | read; call `ResolveChallenge` only on a schism-challenge |
| Work refusal | duty roster public API (E12) | consumer |
| Scope vocabulary | — | `scope_map.json` (data) |
| Statute list, opposition rung/leader, precedent tags | — | nested `assembly` DTO in `shelter_governance` state — **DEC-SG-02** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Governance/AssemblyScopeMap.cs` (new, pure), `Governance/StatuteBook.cs` (new, pure), `Governance/OppositionLadder.cs` (new, pure), `Governance/CourtRoles.cs` (new, pure), `Governance/PrecedentLedger.cs` (new, pure), `Governance/ShelterGovernanceEngine.cs` (additive nested state only), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`, for juror/leader draws)
**Data:** `assembly_scope_map.json`, `statutes.json` (starter book = 4 laws + additions), `opposition_ladder.json`, `precedents.json`, `assembly_lines.json`; `wasteland_laws.json` (additive rows — owner-file, `INT`)
**Host:** `src/Main.ShelterGovernance.cs` (`INT`), `src/Host/ShelterGovernanceHostSession.cs`, `src/Main.Justice.Integration.cs` (`INT`, role hooks), one day-owner registration (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** existing `JusticeTribunalPanel`/`PoliticsUI` (**DEC-SG-06**)
**Tests:** `Ashfall.Core.Tests/Governance/AssemblyScopeMapTests.cs`, `StatuteBookTests.cs`, `OppositionLadderTests.cs`, `CourtRolesTests.cs`, `Ashfall.Core.Tests/Save/ShelterAssemblySaveTests.cs`

## 5. Packages

### SG-P0 — Premise audit (Auditor; read-only)
- Prove E5 with a failing consent test on a current policy; confirm E2; confirm E8 daily integration and E12 refusal API; list every reader/writer of grievance and coup risk; foreman signs DEC-SG-01…10.
- **Accept:** E5 shown by test evidence or downgraded; each VERIFY closed or a named blocker.

### SG-P1 — Scope map (Core + data)
- Map every bloc scope word to ≥1 policy scope (or to `none`); consent evaluation consults the map first, then exact match (backward compatible).
- **Accept:** table-driven; a previously zero-consent policy now shows real supporters/opponents; no bloc or policy file edited.

### SG-P2 — Statute book (Core + data)
- Statutes wrap laws + attention cost + repeal cooldown + doctrine; starter book = 4 authored laws; +2 for Hoarding/Desertion, +8 others; enact/repeal via `PolicySystem`/`PoliticsSystem` public commands.
- **Accept:** enacting an opposed statute raises grievance by the engine's own projection; repeal cooldown enforced; unknown statute rejected.

### SG-P3 — Opposition ladder (Core, pure + nested DTO)
- Per-bloc rung derived from grievance thresholds with hysteresis; leader = named survivor; **announce a sitting ahead** before any action; walkout/sit-in effects go through the duty-roster/facility public APIs.
- **Accept:** same inputs → same rung; announcement always precedes action; concession reduces grievance through `AdjustBlocGrievance`; ship-dark parity.

### SG-P4 — Schism & challenge bridge (Core + host)
- ≥90% rung: schism (survivors depart as a faction — through the existing faction/survivor departure paths) or leadership challenge if the active leadership policy permits (call existing `ResolveChallenge`).
- **Accept:** schism conserves survivors (departed + remaining = before); challenge only under a policy with a threshold; both paths logged.

### SG-P5 — Court roles (Core, pure)
- Roles (presiding, advocate, prosecutor, jurors, defendant) assigned from survivors; roles **shape reception** (legitimacy/fear deltas) of a verdict the existing `HoldTrial` produced; never alter the verdict.
- **Accept:** the same incident + evidence yields the same verdict with and without roles (invariance test); reception differs by juror bloc mix.

### SG-P6 — Precedent ledger (Core, pure)
- Tags recorded from resolved cases; bounded consent modifiers for similar statutes/verdicts.
- **Accept:** modifiers bounded and decay; deterministic.

### SG-P7 — Legitimacy/fear ledger & consistency probe (Core + presentation)
- Two-line ledger from law/verdict/policy deltas; a per-sitting probe compares approval, legitimacy, stability and warns on divergence beyond a threshold.
- **Accept:** probe never writes; numbers reproduce from event log.

### SG-P8 — Cross-plan scopes (dark until both ends exist)
- Register scopes for `gate_protocol` (*Plague Year*), `open_admission` (*Living Region* petitions), `broadcast_content` (*Radio Free Ashfall*), `crew_conscription` (*Crews and Companions*), `informant_use` (*The Quiet War*).
- **Accept:** each scope only appears when its owner plan is present; consent shown through SG-P1 map.

### SG-P9 — Presentation (Assembly agenda)
- Extend existing tribunal/politics surfaces with an agenda list; focus/back preserved.
- **Accept:** presenter tests; no authority in panels.

### SG-P10 — Content waves W1–W5 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no assembly data → policy, politics, governance, justice and leadership selftests unchanged.
3. Trial verdict invariance under roles.
4. Save round-trip with a bloc at Walkout, a statute in cooldown, and a precedent.
5. Survivor conservation across schism.
6. No writes to `PoliticsSystem`, `PolicySystem`, `ShelterGovernanceEngine`, `JusticeSystem`, or `LeadershipSystem` except through public commands.

## 7. Cross-plan boundaries
- **Year Two — Generations:** Council of Succession decides *who*; Assembly decides *acceptance*; no shared ledger (DEC-SG-09).
- **The Plague Year:** the Gate Protocol becomes a policy scope with real consent; Plague Year still owns the dial's effects.
- **The Living Region:** admission of refugee waves is an `open_admission` scope.
- **The Quiet War:** infiltrator trials use its evidence; false accusation raises grievance through the ladder.
- **Radio Free Ashfall:** content policy is a scope.
- **Crews and Companions:** conscription/crew rules are a scope; party law stays advisory in v1.
- **The Reconstruction Tree:** the Heritage bloc's influence is a read-only teaching multiplier via data.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-SG-01 | Assembly is a reader/router; no authority merged. | architecture | Yes |
| DEC-SG-02 | Assembly state nests in `shelter_governance`. | architecture | Yes; confirm in P0 |
| DEC-SG-03 | Scope map is data; neither bloc nor policy files renamed. | compatibility | Yes |
| DEC-SG-04 | Roles shape verdict *reception*, never the verdict. | design | Yes |
| DEC-SG-05 | Every opposition rung is announced one sitting ahead. | design | Yes |
| DEC-SG-06 | No new routed panel; extend tribunal/politics UI. | UI | Yes |
| DEC-SG-07 | Statute book starts from the 4 authored laws. | data | Yes |
| DEC-SG-08 | Assembly may supply the real `guardDeficiency` to `AdvanceDailyPolitics` (fixing E2). | architecture | Decide; touches shared seam (`INT`) |
| DEC-SG-09 | Year Two council ledger and Assembly never share state. | boundary | Yes |
| DEC-SG-10 | Execution remains a rare authored path; no new lethal punishment. | tone | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Assembly`, `Statute`, `Opposition`, `Precedent`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim (esp. Plans 53/159/193 and Year Two P4c)
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing governance/justice/politics/leadership tests (list from P0 selector)
- [ ] Shelter-governance/justice/politics selftests (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: the scope map cannot be expressed without editing bloc or policy files; a trial's verdict changes under roles; schism cannot conserve survivors through existing departure paths; the ladder needs a new duty/faction authority; any path overlaps a live claim.
