# Feature / Task Plan: The Plague Year — outbreaks, quarantine politics and zoonotic vectors

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_plague_year_plan.md`. Family index: `docs/expansions/expansion_world_moves_without_you_index.md`.
> Not a claim. Fictional pathogens only (the four already authored). No clinical instruction content.

## 1. Goal & Outcome
- **Goal:** Take disease out of the walls: a **Regional Outbreak Watch** (Rumour → Confirmed → Spreading → Waning → Ended) fed by seeded zoonotic spillover, reaching the shelter through four existing-pattern vectors (gate, wagon, berth, table); one persisted **Gate Protocol** dial (Open/Screen/Sealed); outbreak-triggered cordons through the existing embargo authority; one strain per season across a full year.
- **Outcome (observable):** on a fixed seed, a deep-winter spillover becomes a Rumour, then Confirmed in a region; a petition carrying the strain arrives; with Protocol *Screen* the case is offered isolation and consumes the existing daily care burden; a cordon blocks a route and adds a decaying medical price shock; save/load mid-outbreak preserves every stage.
- **Non-Goals:** no second disease/medical/quarantine authority; no new pathogen in v1; no new save section; no new routed panel; no real-world analogues; no child-endangerment set-pieces; no change to `DiseaseSystem` infection/immunity rules; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | 20 diseases, 4 vectors, 5 exposure sources; catalog schema 3. | `disease_catalog.json`; `Disease/DiseaseSystem.cs` | LIVE |
| E2 | `disease` save section, `DiseaseSystemState` version 2 (per-disease outbreak counters, immunities, seed, rng position). | `DiseaseSystem.cs` L169–235; `Save/SaveSectionRegistry.cs` L108, L479 | LIVE |
| E3 | 4 strains: ash_fever, red_lung, frost_rot, glass_cough; cure projects default 10 days; mutation under radiation dose. | `pathogens.json`; `Disease/PathogenStrainSystem.cs` | LIVE |
| E4 | `IDiseaseOutbreakSource` contract + `DiseaseSystem.TriggerOutbreak(source, …)` rejecting diseases outside the contract. | `Disease/IDiseaseOutbreakSource.cs`; `src/Host/DiseaseOutbreakHostAdapter.cs` L145; `src/Main.EcologicalInfestations.cs` L167–174 | LIVE |
| E5 | Isolation beds; daily care burden (1 water, 1 food, 1 kit/bandage); quality 0.10–1.0; containment bonus. | `Disease/DiseaseQuarantineCoordinator.cs` `TickDaily` L254–305 | LIVE |
| E6 | Spread is intra-shelter; no region/settlement/harbour/caravan outbreak. | `DiseaseSystem.TickDaily` L737; grep | GAP |
| E7 | Zoonotic exposure sources + trapping-catalog disease fields. | `disease_catalog.json`; `wildlife_trapping_catalog.json` | LIVE |
| E8 | Reservoir density simulated (packs, rabid, ecology, global ratio); no disease read. | `World/WildlifeEcosystemSystem.cs`; `EvolvingWorldDayOwner` | LIVE / VERIFY |
| E9 | Embargoes are weather-triggered only (14 rules; region/category/price/caravan_blocked/slow/decay). | `trade_embargoes.json`; `Economy/TradeEmbargoSystem.cs` | GAP |
| E10 | Diagnosis: `is_diagnosed`, `tell`, `tell_secondary`, `timing_clue`; microfluidic diagnostics. | `Medical/DiagnosisKnowledgeStore.cs`; `MicrofluidicDiagnosticEngine.cs` | LIVE |
| E11 | Contagion (grief) system, sanatorium, sick list. | `contagion_events.json`; `Sanatorium/PsychologicalSanatoriumSystem.cs`; `SickListSystem.cs` | LIVE |
| E12 | Selftests `--disease-selftest`, `--disease-expansion-selftest`. | `src/Host/HostCli*.cs` | LIVE (VERIFY args) |
| E13 | Gate/door owner — **found in a later pass:** `AirlockSecuritySystem` already has **Inspect** and **Quarantine** decisions (`ResolveIncident`), with `DoorEncounterSystem` (authored knocks) and `VisitorIntegrationSystem` (the stay); the Gate Protocol dial should bias these existing verbs, not add a new gate. Still to locate: campaign difficulty hook for outbreak severity; where `DiseaseSystem.TickDaily` is invoked in the day tick; whether an apprentice/medic role exists. | `Assets/Ashfall.Core/AirlockSecuritySystem.cs` L30, L98; `YearOfAsh/DoorEncounterSystem.cs`; `Visitors/VisitorIntegrationSystem.cs` | **VERIFY (P0)** — agree **one shared gate adapter** with *The Living Region* (LR-P6) and *The Quiet War* (QW-P0) |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Infection, immunity, stages, treatment | `DiseaseSystem` (`disease`) | nothing to rules; nested watch + protocol DTO — **DEC-PY-02** |
| Strains/cures | `PathogenStrainSystem` | consumer; sample bonus via existing cure-project days if approved (DEC-PY-08) |
| Isolation | `DiseaseQuarantineCoordinator` + medical ward | consumer only |
| Arrival | `IDiseaseOutbreakSource` adapters | 4 new adapters (gate, wagon, berth, table) with authored contracts |
| Embargo/cordon | `TradeEmbargoSystem` | additive trigger kind (weather default) — **DEC-PY-05** |
| Region ids | *Living Region* vocabulary map | consumer |
| Standing | existing standing record | consumer |
| Deaths | survivor death/legacy | consumer |
| Grief/sanatorium | existing systems | consumer |
| Prices | market shock seam (Living Region E3) | calls only |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Disease/RegionalOutbreakWatch.cs` (new, pure), `Disease/SpilloverModel.cs` (new, pure, seeded), `Disease/GateProtocol.cs` (new, pure policy + effect table), `Disease/DiseaseSystem.cs` (additive nested state only; **no rule change**), `Economy/TradeEmbargoSystem.cs` (additive trigger field), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `plague_year.json` (season→strain map, stage thresholds, spillover coefficients, vector contracts, protocol effects), `trade_embargoes.json` (additive outbreak rules), `plague_year_lines.json`
**Host:** `src/Host/DiseaseOutbreakHostAdapter.cs` (new adapters or a sibling file), `src/Main.CampaignOwners.cs` (`INT`, one day-owner registration after `world_evolution`), gate adapter (file from P0)
**Presentation:** existing gate/door surface, existing medical/radio/journal surfaces (**DEC-PY-07**)
**Tests:** `Ashfall.Core.Tests/Disease/RegionalOutbreakWatchTests.cs`, `SpilloverModelTests.cs`, `GateProtocolTests.cs`, `Ashfall.Core.Tests/Save/PlagueYearSaveTests.cs`; extend existing disease/embargo tests

## 5. Packages

### PY-P0 — Premise audit (Auditor; read-only)
- Close E13 (gate owner, difficulty hook, `TickDaily` call site, medic role); verify the four adapters can each be expressed as an `IDiseaseOutbreakSource`; confirm the embargo catalog can take an additive trigger field without breaking its integrity rules; foreman signs DEC-PY-01…10.
- **Accept:** each VERIFY closed or a named blocker; adapter feasibility recorded with `path:line`.

### PY-P1 — Regional Outbreak Watch (Core, pure + nested DTO)
- Watch entries `{ strainId, regionId, cause, stage, since }`; stage transitions by authored thresholds + hysteresis; nested `watch[]` in the `disease` DTO (additive, default empty).
- **Accept:** table-driven transitions; round-trip; old saves load; no new section; ship-dark parity for `--disease-selftest`.

### PY-P2 — Spillover model (Core, pure, seeded)
- Reservoir pressure (wildlife density × season × rabid share) × contact pressure (trapping/butchery/scavenging) → daily seeded hazard → **Rumour** only. Stream via a `CampaignStreamIds` fork keyed `(day, region)` (`INT`); no `System.Random`.
- **Accept:** deterministic replay; hazard never exceeds authored cap; quiet days consume no RNG draws beyond the fork's fixed shape.

### PY-P3 — Vector adapters (Host)
- Four `IDiseaseOutbreakSource` adapters with authored `AuthoredDiseaseIds`: gate, wagon, berth, table. Each fires only when the corresponding upstream plan's hook exists (ship-dark otherwise).
- **Accept:** each adapter rejected outside its contract (existing behaviour); results routed to journal/radio; no direct disease-state writes.

### PY-P4 — Gate Protocol (Core policy + host)
- One persisted dial (`Open/Screen/Sealed`, change-day) nested in the `disease` DTO; `Screen` uses diagnosis knowledge and isolation capacity; `Sealed` blocks petitions and freight dwell through read-only flags other plans consume.
- **Accept:** Screen offers isolation and consumes the existing care burden; Sealed produces standing consequences only through existing standing seams; dial survives save/load.

### PY-P5 — Cordons & faction responses (Core data + embargo extension)
- Additive `trigger_kind` (default `weather`) on embargo rules; outbreak rules block caravans, slow routes, add a decaying medical price shock; faction positions expressed as data (who enforces, who profiteers).
- **Accept:** existing 14 weather rules behave identically; new rules decay to exactly neutral; integrity validator rejects unknown regions/categories.

### PY-P6 — Bulletins & graded truth (Presentation)
- Rumour/Told/Seen (shares the *Living Region* grades); ward log; the Count. Focus/back behaviour preserved on any touched surface.
- **Accept:** presenter tests prove Rumour never exposes stage exactly; Seen equals state.

### PY-P7 — Strain seasons & cure arc (content + wiring)
- Season→strain map from data; four movements as quest/journal chains; sample-bonus (if DEC-PY-08) implemented as a bounded reduction to `requiredDays` through the cure project owner's public API.
- **Accept:** each movement has start gate, end marker, journal line; cure project days never below authored floor.

### PY-P8 — Aftermath hooks (read-only)
- Feed the Count; expose per-region "emptied by the Year" to the Living Region Board; grief/sanatorium use existing systems unchanged.
- **Accept:** no writes to contagion, sanatorium, or sick-list authorities.

### PY-P9 — Content waves W1–W5 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Determinism: identical spillover/stage sequences on replay.
3. Save round-trip mid-outbreak and mid-quarantine.
4. Ship-dark parity: no `plague_year.json` → `--disease-selftest`, `--disease-expansion-selftest` unchanged.
5. Weather embargoes bit-identical after the additive trigger field.
6. Every infection introduced by a vector passes `TriggerOutbreak` under its contract (rejections counted as today).
7. Every disease death has a Count line.

## 7. Cross-plan boundaries
- **The Living Region:** Health pillar reads Confirmed/Spreading; gate petitions are the *gate* vector; Board consumes "emptied by the Year".
- **The Long Line: Freight:** quarantine is a leg condition; cordon blocks a route; wagon vector is an adapter here, not a freight-resolver change.
- **The Drowned Coast:** yellow-flag berths; the ledger exposes a read-only `closed` flag.
- **Year Two:** children/apprentices read immunity only; outposts as isolation sites are a Year Two custody decision; no change here.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-PY-01 | Season→strain map (Red Lung/Frost Rot/Glass Cough/Ash Fever ↔ deep winter/thaw/dry heat/ash winds). | design | Yes |
| DEC-PY-02 | Watch + protocol nest in `disease` (not `pathogen_strains`). | architecture | Yes; decide in P0 |
| DEC-PY-03 | Regional layer is data-only; infection still through `DiseaseSystem`. | architecture | Yes |
| DEC-PY-04 | Spillover creates **Rumour** only, never Confirmed. | design | Yes |
| DEC-PY-05 | Embargo rules gain an additive trigger kind. | architecture | Decide in P0 |
| DEC-PY-06 | Gate Protocol is one persisted dial with change-day. | design | Yes |
| DEC-PY-07 | No new routed panel. | UI | Yes |
| DEC-PY-08 | Sample bonus for cure projects (autopsy/consenting survivor), bounded with a floor. | design | Decide; moral weight is authored, not stated |
| DEC-PY-09 | Severity respects the difficulty authority. | compatibility | Yes; hook found in P0 |
| DEC-PY-10 | No new pathogen in v1. | scope | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `RegionalOutbreak`, `GateProtocol`, `Spillover`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing disease/embargo/quarantine/pathogen tests (list from P0 selector)
- [ ] `--disease-selftest`, `--disease-expansion-selftest` (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a vector cannot be expressed as an `IDiseaseOutbreakSource`; embargo extension requires a new authority; the gate has no existing owner; any change alters `DiseaseSystem` infection/immunity rules; any path overlaps a live claim.
