# Feature / Task Plan: Radio Free Ashfall — run your own broadcast

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_radio_free_ashfall_plan.md`. Family index: `docs/expansions/expansion_new_ways_to_play_index.md`.
> Not a claim. Soft dependencies on *The Living Region* (canonical regions, graded news) ship dark until they exist.

## 1. Goal & Outcome
- **Goal:** Give the shelter one persistent broadcast identity (call sign, frequency, power), a schedule grid, per-region audience affinity, a Voice Trust ledger, a broadcast Signature that hostile direction-finding can act on, and a listener mailbag — all on top of the existing program-production, PsyOps, triangulation and door/visitor systems.
- **Outcome (observable):** on a fixed seed the player books a bulletin into their own slot; a region's affinity rises; a false-graded bulletin is later revealed and Voice Trust falls; Signature crosses a faction's resolve threshold and a probe (jamming or visitor) arrives through the existing PsyOps/door path; save/load mid-schedule preserves everything.
- **Non-Goals:** no edits to the six authored stations or their slots; no second propaganda/PsyOps system; no new save section; no new routed panel; no real-time audio; no change to distress-signal trust; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | 6 NPC stations with persona/reliability/schedule; catalog loader. | `radio_stations.json`; `Radio/RadioStationCatalog.cs`, `RadioStationCatalogLoader.cs` | LIVE |
| E2 | `RadioProgramProductionSystem`: `StartPrep`, `CancelJob`, `TryDeliver`, `CalculateAudienceResponse`, `ResolveFollowUpHook`, `SlotExists`; jobs, follow-up hooks; host session; save section `radio_program_production`. | `Radio/RadioProgramProductionSystem.cs` L123–364; `src/Host/RadioProgramProductionHostSession.cs`; `Save/SaveSectionRegistry.cs` L166 | LIVE |
| E3 | 2 authored programs. | `radio_programs.json` | LIVE (thin) |
| E4 | PsyOps campaigns/jamming/counter-propaganda; section `psyops`. | `Radio/PsyOpsSystem.cs`; `Save/SaveSectionRegistry.cs` L165 | LIVE |
| E5 | `SignalTrustLedger` bounded 0–100 with constants (distress only). | `Radio/SignalTrustLedger.cs` L22–40 | LIVE |
| E6 | Triangulation / DF / authenticity for incoming signals. | `Radio/SignalTriangulationSystem.cs`, `DirectionFindingCatalog.cs`, `SignalAuthenticityEvaluator.cs` | LIVE |
| E7 | Shelter station, propagation, recording, schedule coordinator; sections `radio_station`, `radio`. | `Radio/*.cs`; `Save/SaveSectionRegistry.cs` L91, L182 | LIVE |
| E8 | Door encounters/visitors as the arrival path for consequences. | `YearOfAsh/DoorEncounterSystem.cs`; `Visitors/VisitorIntegrationSystem.cs` | LIVE (gate owner = P0 VERIFY, shared with LR/PY/QW) |
| E9 | No own-station identity, audience ledger, mailbag, or outbound signature. | grep `Radio/` | GAP |
| E10 | Panels: `RadioPanel`, `RadioIntelligencePanel`; program-production surface unconfirmed. | `src/UI/` | VERIFY |
| E11 | Comms array tier/power available as transmitter input. | `Communications/CommunicationsSystem.cs`; comms_targets | LIVE (VERIFY power query) |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Player program jobs | `RadioProgramProductionSystem` (`radio_program_production`) | nested `station` DTO (additive) — **DEC-RF-02** |
| NPC stations | `RadioStationCatalog` | none |
| Campaign pressure/jamming | `PsyOpsSystem` | consumer: signature→jam probe triggers an existing PsyOps path |
| Detection | `SignalTriangulationSystem` | reused *pattern*; signature model is pure, separate |
| Arrival of consequences | door/visitor owner (E8) | adapter only |
| Distress trust | `SignalTrustLedger` | untouched |
| Voice trust | — | `VoiceTrustLedger` (pure), same bounded pattern, nested in the station DTO |
| Region vocabulary/news | *Living Region* | consumer (soft) |
| Transmitter power | comms array | consumer |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Radio/PlayerStation.cs` (new, pure state + rules), `Radio/AudienceLedger.cs` (new, pure), `Radio/VoiceTrustLedger.cs` (new, pure), `Radio/BroadcastSignatureModel.cs` (new, pure), `Radio/ListenerMailbag.cs` (new, pure, seeded), `Radio/RadioProgramProductionSystem.cs` (additive nested state only), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `radio_programs.json` (additive templates), `player_station.json` (call signs, slot grid, signature curve, trust constants), `listener_mail.json`, `radio_free_lines.json`
**Host:** `src/Host/RadioProgramProductionHostSession.cs`, `src/Main.RadioProgramProduction.cs` (`INT`), one day-owner registration (`src/Main.CampaignOwners.cs`, `INT`), door/visitor adapter (file from P0)
**Presentation:** `src/UI/RadioPanel.cs` (existing; DEC-RF-06)
**Tests:** `Ashfall.Core.Tests/Radio/PlayerStationTests.cs`, `AudienceLedgerTests.cs`, `VoiceTrustLedgerTests.cs`, `BroadcastSignatureModelTests.cs`, `Ashfall.Core.Tests/Save/RadioFreeAshfallSaveTests.cs`

## 5. Packages

### RF-P0 — Premise audit (Auditor; read-only)
- Re-verify E1–E11; find the live door/visitor owner; confirm where program delivery targets a slot and whether a *player-owned* slot can exist without editing the authored catalog; locate the transmitter power query; foreman signs DEC-RF-01…10.
- **Accept:** each VERIFY closed or a named blocker.

### RF-P1 — Station identity & schedule grid (Core + host)
- One persisted station (call sign, frequency, power tier, grid of bookings); frequency must not collide with authored stations; nested in the production save DTO (additive; default empty).
- **Accept:** round-trip; old saves load; two stations cannot share a frequency; ship-dark parity for the program-production selftest.

### RF-P2 — Audience ledger (Core, pure)
- Per canonical region: reach (power/propagation/weather/terrain inputs) and persisted affinity; missed slots decay affinity.
- **Accept:** table-driven; same inputs → same output; no RNG; regions resolve through the Living Region vocabulary or a fixed fallback when absent.

### RF-P3 — Voice Trust & truth policy (Core, pure)
- Bounded 0–100, constants like `SignalTrustPolicy`; script truth grade (true/spun/false) set at booking; reveal event moves trust; spin decays after a short window.
- **Accept:** trust never leaves [0,100]; distress ledger untouched (write-count test).

### RF-P4 — Broadcast Signature & consequences (Core + host)
- Signature from power × duration × repetition, overnight decay; thresholds per faction from data; crossing emits *one* warning line then a probe through existing PsyOps jamming or a door visitor. Relay option reads waystation/outpost as the transmit site (custody untouched).
- **Accept:** deterministic; every consequence uses an existing path; warning always precedes action.

### RF-P5 — Listener mailbag (Core, seeded)
- Daily letter draw only when affinity > 0; deterministic stream; each letter maps to existing choice resolution.
- **Accept:** same seed → same mail; no letter with an unresolved option; no RNG on quiet stations.

### RF-P6 — Program catalog & reactions (content + data)
- Add categories (bulletin, story, messages, classifieds, health, coded) as additive templates; faction reaction rows.
- **Accept:** validator passes; no edit to existing 2 templates.

### RF-P7 — Presentation
- Extend `RadioPanel`: station header, grid, audience by region (graded), Signature band, mailbag. Focus/back preserved.
- **Accept:** presenter tests; panel holds no gameplay authority.

### RF-P8 — Cross-plan hooks (dark until both ends exist)
- Bulletin reads Living Region Board; classifieds feed freight demand read-only; Health hour reads Plague Year outbreak watch; Year Two children's hour reads apprenticeship roster.
- **Accept:** each hook absent → neutral.

### RF-P9 — Content waves W1–W5 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Determinism: identical mail, signature and probe sequences on replay.
3. Save round-trip mid-schedule.
4. Ship-dark parity: no `player_station.json` → program-production, PsyOps and radio selftests unchanged.
5. No write to `SignalTrustLedger`, authored stations, or PsyOps state except through their public APIs.
6. Every probe has a preceding warning line.

## 7. Cross-plan boundaries
- **The Living Region:** provides regions and news grades; the station is a *consumer* and an *influence* (affinity), never a writer of Pulse state.
- **The Plague Year:** Health-hour truth affects Voice Trust; the outbreak watch is read-only.
- **The Long Line: Freight:** classifieds are demand hints only.
- **The Quiet War:** the "signal interceptor" agent and Signature share the detection story; QW owns agents, RF owns the station.
- **Shelter Governance:** broadcast content policy is a governance scope (read).
- **Crews and Companions:** party check-ins are broadcasts/short-range comms (read-only).
- **Year Two:** children's hour uses apprenticeship roster read-only.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-RF-01 | The player's station is an *additional* identity, never edits authored stations. | scope | Yes |
| DEC-RF-02 | Station state nests in `radio_program_production`. | architecture | Yes; confirm in P0 |
| DEC-RF-03 | Separate Voice Trust, same bounded pattern as distress trust. | architecture | Yes |
| DEC-RF-04 | Truth grade is set by the player's script, revealed later by the world. | design | Yes |
| DEC-RF-05 | Signature consequences reuse PsyOps jamming and door visitors only. | architecture | Yes |
| DEC-RF-06 | Extend `RadioPanel`; no new routed panel. | UI | Yes |
| DEC-RF-07 | Audience is per canonical region (Living Region vocabulary; fixed fallback). | design | Yes |
| DEC-RF-08 | Relay broadcasting from outposts/waystations is read-only custody. | compatibility | Yes |
| DEC-RF-09 | Mailbag only when affinity > 0. | design | Yes |
| DEC-RF-10 | No real-time audio, no music generation. | scope | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `PlayerStation`, `Audience`, `VoiceTrust`, `Signature`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing radio/program-production/PsyOps tests (list from P0 selector)
- [ ] Program-production selftest and radio/PsyOps selftests (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: an own-station slot requires editing the authored station catalog; Voice Trust would need writes into the distress ledger; consequences would need a new arrival system; any path overlaps a live claim.
