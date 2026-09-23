# ASHFALL Expansion & Integration Program — Wave 13 (2026-09-21)

Fifteen new plans (161–175), selected by a **type-level audit**: of 454 Core
authority types, **135 reachable authorities were never addressed in any plan
body**. This wave takes the fifteen largest distinct ones (430–830 lines each).

**None is a claim.** Foreman owns `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md`.

## Plan 1, expanded again (S–U)

| File | Size | Content |
|---|---:|---|
| `…_APPENDIX-S_TEST_REGIONS.md` | ~9 KB | inverse index: each test region and the orphans it touches — **50 of 109 regions** reference at least one, giving natural claim bundles |
| `…_APPENDIX-T_WORKED_EXEMPLARS.md` | ~5 KB | four fully worked seal specs (top-risk, island, stateful-with-data, small evaluator) |
| `…_APPENDIX-U_DATA_REFERENCES.md` | ~2 KB | hardcoded JSON literals verified against the recursive data tree; **`ModSupportSystem` references `factions.json` / `quests.json`, which do not exist** |
| **Plan 1 total (A–U)** | ~386 KB | |

## Wave 13 (161–175)

| # | Plan | Unaddressed authority (lines) |
|---|---|---|
| 161 | Espionage System Truth | `Factions/EspionageSystem.cs` (734) — field operations vs Plan 41's tradecraft |
| 162 | Morale Contagion Truth | `Survivors/MoraleContagionSystem.cs` (770) — the link between Plans 64 and 129 |
| 163 | Aquaponics Truth | `Shelter/AquaponicsSystem.cs` (813) + `HydroponicBiomeSystem.cs` (441) — mass-balanced food loop |
| 164 | Aquifer Monitoring Truth | `Shelter/AquiferPiezometerEngine.cs` (829) — wells as a stock with recharge/drawdown |
| 165 | Perimeter Defense Truth | `Defense/PerimeterDefenseSystem.cs` (796) — the layer raids actually meet |
| 166 | Trade Embargo Truth | `Economy/TradeEmbargoSystem.cs` (709) — policy with enforcement and smuggling pressure |
| 167 | Pharmaceutical Truth | `Medical/PharmaceuticalTabletEngine.cs` (721) — production, dosage, dependency |
| 168 | Weather Sonde Truth | `World/WeatherSondeSystem.cs` (692) — measurement, error bands, forecast lead time |
| 169 | Cultural Archive Truth | `Culture/CulturalArchiveVaultSystem.cs` (675) — deposits, access, loss events |
| 170 | Narrative Continuity Truth | `Narrative/Continuity/NarrativeContinuityEngine.cs` (671) — runtime contradiction checks |
| 171 | Faction Branch Truth | `Factions/FactionBranchCoordinator.cs` (668) — splits, inheritance, reunification |
| 172 | Metrology Truth | `Shelter/PrecisionMetrologySystem.cs` (671) — standards, tolerances, calibration |
| 173 | Leadership Truth | `Survivors/LeadershipSystem.cs` (663) — authority, compliance, succession |
| 174 | Rationing Truth | `Economy/ResourceRationingSystem.cs` (631) — allocation tiers, fairness, escalation |
| 175 | Workshop Truth | `Shelter/ShelterWorkshopSystem.cs` (634) — bench capacity, queue, shift coupling |

## Two premise corrections carried in this wave

- **Plan 116** (noise) claimed the `_Game` file was the only noise system —
  corrected: `Shelter/ShelterNoiseSystem.cs` (560 lines, reachable) also
  exists; the plan now reconciles **two authorities** before Plan 94's decision.
- **Plan 119** (maintenance) now cites `EquipmentConditionSystem.cs` (555
  lines, Core root) as the likely existing condition owner to extend.

## Programme state (171 plans)

| Waves | Plans | Chars |
|---|---|---|
| 1 | 6 + Appendices A–U | ~386 KB |
| 2–10 | 10/10/10/10/20/15/15/15/15 | ~570 KB |
| 11 | 15 | ~58 KB |
| 12 | 15 | ~66 KB |
| 13 | 15 | this wave |

Counts are indicative; Plan 100 replaces README tables with a generated index.

## Ordering

- **170** first: a runtime validator for authored story is the highest-value structural catch.
- **162** pairs with Plans 64/129; **173** pairs with 141/69.
- **163, 164** both couple to Plan 46's water owner — land together.
- **165** supplies Plan 61's breach input; **168** supplies Plan 80's lead time.
- **166, 174** route effects through Plan 96's ledger; no local multipliers.
- **167, 172** read Plan 112's quality tiers rather than defining their own.
- **171** must resolve treaties through Plan 151; **169** reads Plan 141's roles.
- **161** is the runtime consumer of Plan 41's sealed engines.
- **175** consumes Plans 101/112/125/93 — land after them.

## Standing rules

Core engine-free; one authority per concern; JSON authority with live
consumers; seeded RNG with replay equality; typed errors and visible failure;
provenance for content; focused verification only; fictional original content.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
