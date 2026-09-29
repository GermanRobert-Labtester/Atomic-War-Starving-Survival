# Feature / Task Plan: Crews and Companions — expeditions become parties with named crew and animals

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_crews_and_companions_plan.md`. Family index: `docs/expansions/expansion_new_ways_to_play_index.md`.
> Not a claim. `ExpeditionSystem` is used in many places; this plan **adds a coordinator beside it** and does not change its one-expedition-per-survivor rule.

## 1. Goal & Outcome
- **Goal:** A **party** = up to 4 survivors + up to 2 companions travelling together, implemented as *N ordinary expeditions plus a small party coordinator*: shared destination/pace/outcome, roles, a cohesion number, seeded injury/loss distribution, camp rituals, and road-bond writes through existing social systems. Provide one **crew contract** other plans read.
- **Outcome (observable):** on a fixed seed a party of three plus a hound dispatches to a catalog location; exactly **one** encounter is rolled per tick for the party (not per member); a medic role changes an injury outcome; camp raises cohesion; an event writes a trauma bond through the existing system; a lost companion produces the existing grief effect; a party of one behaves identically to today; save/load mid-trip preserves everything.
- **Non-Goals:** no change to `ExpeditionSystem`'s one-per-survivor rule or start path; no new stamina/inventory/health authority; no new save section; no new routed panel; no boats/wagons (crew *contract* only); no squad tactics; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; solo-expedition parity holds; handoff lists untouched shared paths.

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | One expedition per survivor; `_active` keyed by `survivorId`; `Start(...)` refuses a second. | `Expeditions/ExpeditionSystem.cs` L400–454 | LIVE |
| E2 | Query hooks: stamina-drain multiplier, survivor speed multiplier, pack-capacity bonus, encounter-chance multiplier (all `Func<string,float>`). | `ExpeditionSystem.cs` L349–387 | LIVE |
| E3 | Camp API: `EnterCamp`, `ReserveCampSupplies`, `CampTick`, `ResolveCampEncounter`. | `ExpeditionSystem.cs` L842–1050 | LIVE |
| E4 | Companion system: 5 species, roles Guard/Pack/Morale, one handler, bond, care, sickness, guard/pack/morale/grief queries; section `companion_animals`. | `Ecology/CompanionAnimalSystem.cs`; `companion_animals.json`; `Save/SaveSectionRegistry.cs` L289 | LIVE |
| E5 | Host flips companion `on_expedition` when its handler has an active expedition. | `src/Main.Companion.cs` L214–225 | LIVE |
| E6 | `crew_min/crew_max` on naval vessel defs unread. | `naval_vessels.json`; grep | GAP |
| E7 | Trauma bonds, relationship decay, survivor roles, skills, social coordinator. | `Survivors/TraumaBondSystem.cs`, `RelationshipDecaySystem.cs`, `SurvivorRoleSystem.cs`, `SurvivorSocialCoordinator.cs` | LIVE (VERIFY write APIs) |
| E8 | Encounter choice resolution per survivor via bridge. | `Expeditions/ExpeditionEncounterBridge.cs`; `src/Host/ExpeditionHostSession.cs` L1155 | LIVE (VERIFY choice API, how encounters are rolled per tick) |
| E9 | Duty roster governs availability. | `DutyRoster/DutyRosterSystem.cs` | LIVE |
| E10 | 75 expedition locations. | `expeditions.json` | LIVE |
| E11 | Whether N simultaneous solo expeditions to the same location produce N encounter rolls per tick (the double-count risk). | `ExpeditionSystem.TickHours` L751 | **VERIFY (P0, critical)** |
| E12 | Panels: `ExpeditionPanel`, `ExpeditionCampPanel`. | `src/UI/` | LIVE |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Expedition state (per survivor) | `ExpeditionSystem` | none; N ordinary starts |
| Companion state/care/grief | `CompanionAnimalSystem` | read; role at party level stored in party DTO |
| Relationships/trauma bonds | existing social systems | writes only via their public APIs |
| Availability | duty roster | consumer |
| Party grouping, roles, cohesion, seeded loss draw | — | `PartyCoordinator` (pure Core), nested `parties[]` in the `expeditions` save DTO — **DEC-CC-02** |
| Encounter roll suppression for followers | via E2 hook | a per-survivor query that returns 0 for followers |
| Crew contract | — | `CrewContract` value type, read-only for DC/LF |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Expeditions/PartyCoordinator.cs` (new, pure), `Expeditions/PartyRoles.cs` (new), `Expeditions/PartyCohesion.cs` (new), `Expeditions/CrewContract.cs` (new), `Expeditions/ExpeditionSystem.cs` (additive nested DTO field only; **no logic change**), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `party_roles.json`, `camp_rituals.json`, `party_quarrels.json`, `companion_party_moments.json`, `expedition_encounter_variants.json` (additive role-gated options)
**Host:** `src/Host/ExpeditionHostSession.cs` (`INT`, dispatch + encounter routing), `src/Main.Companion.cs` (`INT`, presence sync), one day-owner registration if needed (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** existing `ExpeditionPanel`, `ExpeditionCampPanel` (**DEC-CC-06**)
**Tests:** `Ashfall.Core.Tests/Expeditions/PartyCoordinatorTests.cs`, `PartyCohesionTests.cs`, `CrewContractTests.cs`, `Ashfall.Core.Tests/Save/PartySaveTests.cs`; extend expedition and companion tests

## 5. Packages

### CC-P0 — Premise audit (Auditor; read-only) — **critical: E11**
- Prove or disprove E11 (N solo expeditions → N encounter rolls); enumerate every caller of `ExpeditionSystem.Start`, `Active`, and the four query hooks (who already sets them); confirm relationship/trauma-bond write APIs (E7); confirm encounter-choice API per survivor (E8); foreman signs DEC-CC-01…10.
- **Accept:** E11 answered with a test or `path:line`; the hook owner conflict (if any) identified before design.

### CC-P1 — Party model (Core, pure + nested DTO)
- `Party { partyId, name, memberIds[≤4], companionIds[≤2], leadId, roles, cohesion, startedDay }`; validation (free on roster, handler present for each companion, one party per survivor); nested in the `expeditions` DTO, additive, default empty.
- **Accept:** round-trip; old saves load; a party of one writes nothing extra; validation rejects all invalid parties with reasons.

### CC-P2 — Dispatch as N ordinary starts (Host, `INT`)
- Dispatch calls `ExpeditionSystem.Start` once per member with the same definition and coordinated stance/night/vehicle; followers get an encounter-chance multiplier of 0 via the existing hook; lead's multiplier scaled by party size (data).
- **Accept:** exactly one encounter roll per tick for the party (fixed-seed test); a party of one identical to a solo start (parity test); no change to `ExpeditionSystem` logic.

### CC-P3 — Roles & query providers (Core)
- Roles from skills/role system; roles feed the *existing* hooks: pathfinder → speed, hauler/goat → pack-capacity, watch → encounter multiplier, medic → injury outcome.
- **Accept:** each role's effect is bounded and table-driven; roles absent → neutral multipliers.

### CC-P4 — Cohesion & camp rituals (Core + content)
- Cohesion derived from relationships + shared history; camp rituals (data) with cohesion deltas via camp choice machinery; low cohesion enables quarrels.
- **Accept:** deterministic; cohesion bounded 0–100; rituals never bypass camp authority.

### CC-P5 — Party encounters, injury & loss (Core + host)
- Encounter resolved once at party level with role-gated options (additive variants); injury/loss target chosen by a **seeded** weighted draw (stream via `CampaignStreamIds` fork keyed `(day, partyId, tick)`); death → survivor legacy; companion loss → existing grief.
- **Accept:** same seed → same target; medic changes outcome; survivor and companion conservation (members before = members after + recorded losses).

### CC-P6 — Companions as members (Core + host)
- Companion party presence (role, terrain fit, temper, bond-gated loyalty); `on_expedition` continues to be driven by handler presence; companion risk is party-level only.
- **Accept:** a companion never starts an expedition alone; handler absent → not in party; bond-gated behaviour deterministic.

### CC-P7 — Road bonds (Core via existing APIs)
- Extreme events write to trauma-bond/relationship systems through their public APIs; ledger line for each write.
- **Accept:** no direct mutation of relationship state; each write has one journal line.

### CC-P8 — Crew contract (Core, read-only)
- `CrewContract` produced from a party or a roster subset; consumed by *The Drowned Coast* (boat crew_min/max) and *The Long Line: Freight* (driver/escort) as read-only.
- **Accept:** no consumer writes party state; contract fields validated against vessel/wagon requirements.

### CC-P9 — Presentation
- Extend `ExpeditionPanel` (party builder, role picks, cohesion) and `ExpeditionCampPanel` (rituals). Focus/back preserved.
- **Accept:** presenter tests; panels hold no authority.

### CC-P10 — Content waves W1–W5 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Solo parity: a party of one → `--expedition-selftest` and expedition/companion tests unchanged.
3. One encounter roll per party per tick.
4. Determinism: identical loss targets and quarrels on replay.
5. Save round-trip mid-trip with a party, a companion, and cohesion.
6. Conservation of members and companions across every outcome.
7. No writes to relationship/trauma/companion/legacy state except through public APIs.

## 7. Cross-plan boundaries
- **The Drowned Coast:** boat crews read the crew contract (`crew_min/crew_max`); DC owns hulls/berths.
- **The Long Line: Freight:** wagon crews read the crew contract (driver, escort); LF owns runs.
- **The Quiet War:** a returning party may bring a stranger through the shared gate adapter.
- **Radio Free Ashfall:** party check-ins are short broadcasts; silence past a threshold raises alarm (read-only).
- **The Plague Year:** party illness through existing disease exposure; parties returning from an outbreak region trigger Screen.
- **Shelter Governance:** crew conscription is a policy scope; parties never override duty law.
- **The Reconstruction Tree:** expedition finds route as fragment sources through existing seams; a party's Pathfinder/Medic disciplines count as bearers only via skills.
- **Year Two:** apprentices may join as *trainee* (non-lead); children never.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-CC-01 | A party is N ordinary expeditions + a coordinator; one-per-survivor stays. | architecture | Yes |
| DEC-CC-02 | Party state nests in the `expeditions` DTO. | architecture | Yes; confirm in P0 |
| DEC-CC-03 | Party cap 4 survivors + 2 companions. | scope | Yes |
| DEC-CC-04 | Encounters roll once per party (lead), followers suppressed via existing hook. | design | Yes; depends on E11 |
| DEC-CC-05 | Loss target via seeded weighted draw; medic role changes outcome. | design | Yes |
| DEC-CC-06 | No new routed panel; extend expedition/camp panels. | UI | Yes |
| DEC-CC-07 | Companions never start alone; handler presence required. | rule | Yes |
| DEC-CC-08 | One crew contract shape for boats and wagons. | architecture | Yes |
| DEC-CC-09 | Road bonds only through existing social APIs. | architecture | Yes |
| DEC-CC-10 | Children never join; apprentices as trainees only (if Year Two present). | rule | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Party`, `Crew`, `Cohesion`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim (expedition and companion paths are heavily shared)
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing expedition/companion/trauma-bond tests (list from P0 selector)
- [ ] `--expedition-selftest` and companion selftests (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: E11 shows follower encounter suppression cannot be done through the existing hooks; hook ownership conflicts with another setter; a party would require a second start path or a change to `ExpeditionSystem` logic; any path overlaps a live claim.
