# ASHFALL — Expansion Wave 9 Index (Expansions 52–56)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-24
**Purpose:** Index and evidence summary for the five Wave 9 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin or absent around a working seam, and propose
expansions that attach to existing owners. They are design bibles: they do not
claim paths, change code, or authorize implementation. Any implementation must
later pass through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and
`TEST_POLICY.md`.

---

## The wave theme: the household

Waves 1–8 built a shelter that survives, works, reaches, keeps, and remembers.
Wave 9 is about the part of life that is neither survival nor celebration but
the ordinary week: the heat that comes out of the ground, the mail that
arrives, the pests that must be managed, the neighbors you sleep beside, and
the dates a community sets aside. Each plan takes a live system that already
simulates something real — a drilling campaign, two letter systems, an
infestation lifecycle, social friction, a ceremony engine — and gives it the
practice around the simulation.

| Expansion | Live seam | Current content | Gap |
|---|---|---|---|
| 52 · The Warm Ground | `GeothermalAquiferSystem` + `GeothermalOrcSystem` (depth, bit, casing, steam, scaling, loops, turbine, reserve; save `geothermal_aquifer`; host session exists) | **5 drilling strata + 3 loop strata**; 2.1 KB + 1.5 KB | crews, rig, bits, casing, venting, descaling, thermal uses, records |
| 53 · The Post | `LetterDeliverySystem` + `SurvivorLetterDeliverySystem` (states, morale, address, withhold, unanswered) | **both systems unwired** — no host, no save section, no panel; rich narrative letter corpora | post office, sorting, addresses, couriers, tubes, dead letters, withholding board |
| 54 · The Uninvited | `EcologicalInfestationSystem` (trigger, clear, tolerate, `MaxFoodLossPerDay = 3`, `RecurrenceCooldownDays = 5`; save `ecological_infestation`) | **10 designed infestations** (6 location, 4 shelter) | rounds, standards, traps, clearance, quarantine, tolerance, guardians, records |
| 55 · The Quarter | `ShelterSocialDynamicsSystem` (events, outcomes, privacy fatigue 0..1000, incidents, mediation; save `shelter_social_dynamics`; panel exists) | **8 social events**; 7.4 KB | assignments, privacy builds, house rules, mediation, agreements, neighborhoods, reviews |
| 56 · The Calendar | `CeremonySystem` (prep days, required items, morale, truce, disaster pool; save `ceremony`; panel exists) | **5 ceremonies** (founding, vigil, solstice, fair, harvest) | the civic year, preparation, feasts, guests, truces, observances, disasters, roll |

The strongest authority discipline in the wave: geothermal sends generation
into the grid and heat into the thermal owner; the post never touches radio,
intelligence, or the vault; pests never become an ecology or a poison system;
the quarter never becomes justice, noise, or a rating; and the calendar never
becomes faith or compulsion.

---

## The five plans

1. `expansion_52_the_warm_ground_plan.md` — ~71,000 chars.
   Deep drilling crews, rigs, bits, casing, pressure venting, descaling, ORC
   loops, thermal uses, and the well stone, around the live drilling and ORC
   systems.
2. `expansion_53_the_post_plan.md` — ~71,000 chars.
   A post office, sorting and addresses, courier rounds and receipts,
   pneumatic tubes, a dead-letter shelf, a withholding board with two
   reviewers and a timer, and letter-writing culture. Wires two orphaned Core
   systems into host, save, and UI.
3. `expansion_54_the_uninvited_plan.md` — ~71,000 chars.
   Pest rounds, storage standards, trap lines, clearance teams, quarantine,
   tolerance with boundaries, guardian animals, site histories, and a loss
   stone. No poisons, no cruelty, tolerance first-class.
4. `expansion_55_the_quarter_plan.md` — ~71,000 chars.
   Dorm assignments, buildable privacy, posted house rules, mediation ending
   in agreements, shared-space rotas, corridor neighborhoods, quarterly
   reviews, and a stone with no names on it.
5. `expansion_56_the_calendar_plan.md` — ~70,000 chars.
   A civic year of twelve observances with preparations, feasts, guests,
   truces read and witnessed, vigils with consent, disasters with recovery,
   a roll, year volumes, and lessons kept year over year.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot or Unity reference in Core
  logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with
  integer `schema_version`, validated by `CatalogIntegrityValidator` and
  registered with `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains a non-duplication statement
  and an integration-seam table. No second power, warmth, water, radio, ecology,
  food, disease, justice, noise, faith, memory, or diplomacy system is
  introduced.
- **Deterministic.** Live seeded paths only: the geothermal day tick, the
  infestation trigger/recurrence path, the social event roll,
  `EvaluateRoomDynamics`, `TickDay` for ceremonies, and campaign-day timers for
  mail. Paired replay hashes must match.
- **Persistence.** New state is additive inside existing owners
  (`GeothermalAquiferState`, `ShelterSocialSave`, `CeremonySaveState`,
  `ecological_infestation`). The Post proposes additive letters state inside the
  `narrative` envelope with a dedicated section flagged as a foreman decision.
  Legacy saves load neutral; the Triad drift gate must pass.
- **Tone.** Restrained, human, fictional. No real sites, holidays, services,
  protocols, or species claims copied.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus
  `--data-integrity-selftest` and `--content-utilization-selftest`. A
  compile-green result is not acceptance.

---

## Ethical and content contracts specific to Wave 9

| Plan | Hard contract |
|---|---|
| 52 Warm Ground | No blowout spectacle; failures costly and contained; no free energy (reserve/scaling); power via grid only; heat via thermal owner; no drilling of graves or sacred ground |
| 53 Post | No opening sealed letters, ever; no interception or surveillance; withholding needs two reviewers and a timer; dead letters kept and indexed; no postage pricing; the service never gossips |
| 54 Uninvited | No poison or fumigation; mechanical, checked traps only (sticky boards banned in data); tolerance is first-class with boundaries, keepers, and reviews; guardians owned and vetted elsewhere; people are never quarantined |
| 55 Quarter | No ratings, scores, or surveillance; no forced reconciliation (distance is valid); no punishment authority; no names in trend data; quiet hours stay with their owner |
| 56 Calendar | No real holidays or doctrine; attendance and labor voluntary; no feast may gut the stores; truces have terms, witnesses, and honest violation reports; vigils require consent and offer a private option |

---

## Cross-wave hooks (summary)

- **Within Wave 9:** the well's warm rooms make the quarter livable; the post
  carries the calendar's invitations; pests are caught in the granary and the
  farm; the calendar's blocks meet in the quarter.
- **With earlier waves:** the grid takes the geothermal generation; the thermal
  owner takes its heat; the road carries couriers and guests; the frontier
  carries the fair's truce; the ward handles the post's bad news and the
  calendar's outbreak; the vault binds year volumes and preserves songs; the
  wild supplies guardian animals; the press prints rolls, programs, labels, and
  cards; the quiet owner governs vigils and night rounds.

Each plan is self-contained; none requires another to ship.

---

## How to promote a Wave 9 plan

1. Pick exactly one plan and one phase. Do not start two.
2. Re-audit the premise against current source and data; a plan is not proof an
   API or catalog still exists (`AGENTS.md` Rule 7).
3. Claim exact paths in `WORKTREE_OWNERSHIP.md`; confirm no overlap.
4. Add the package row to `INTEGRATION_PLANS.md` with owner, acceptance, and
   focused verification.
5. Implement data first, then pure Core, then persistence, then host/UI, then
   content.
6. Verify with focused tests and the data/content selftests; record limitations.
7. Update the live ledger only if you are the foreman or named integrator.

Until a foreman signature exists, these plans remain proposals. The safe
pre-signature work is Phase 1 (schemas and validators), which is additive and
reversible. For The Post, note that its Phase 3 includes a genuinely new
wiring job — two Core systems with no host or save — so its package plan must
name the envelope decision explicitly.

---

## Open decisions common to the wave

- Content volume budgets (each plan lists an authoring estimate; the full wave is
  roughly 300,000–330,000 words of new prose if all five are authored).
- Save placement (additive sub-objects versus sibling sections) per domain. Four
  plans recommend additive sub-objects; The Post recommends additive inside
  `narrative` and flags a dedicated section as a foreman option.
- Whether any Wave 9 expansion adds a new headless selftest verb or extends an
  existing one. Each plan recommends extending panel lifecycle, data integrity,
  and content utilization verbs, with a domain verb only if none exists.
- Priority order. Recommended: 53 (The Post) first because two live Core
  systems are orphaned and it is the only wave plan with a missing host/save
  spine; 52 (The Warm Ground) second because the campaign is long and wants the
  most runway; 54 (The Uninvited) third because prevention protects the food
  the other plans depend on; 56 (The Calendar) fourth because it binds the
  year; 55 (The Quarter) fifth because it is the most self-contained.

---

## Evidence anchors (file references used across the plans)

- `Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs` —
  `GeothermalAquiferState` (`currentDepthMeters`, `drillBitCondition`,
  `steamPressurePsi`, `mineralScaling`, `activeTurbineOutput`,
  `currentStrataId`, `installedCasingDepth`, `turbineCommissioned`,
  `aquiferTapped`, `pressureReliefState`, `generatorHealth`,
  `crossedStrataIds`), constants `NominalTurbineOutputKw = 1500f`,
  `DescalingChemicalCost = 2f`, methods `StartDrilling`, `CommissionTurbine`,
  `Descale`, `TapAquifer`, `VentPressure`, `InstallCasing`, `TickDay`;
  `GeothermalOrcSystem` (`GeothermalStratumDefinition`, `GeothermalStrataCatalog`,
  `GeothermalLoopState`); `src/Host/GeothermalAquiferHostSession.cs`;
  `GeothermalAquiferSaveStore` (`geothermal_aquifer`);
  `geothermal_drilling_depths.json` (2,112 B, 5 strata),
  `geothermal_strata_catalog.json` (1,547 B, 3 strata).
- `Assets/Ashfall.Core/Narrative/LetterDeliverySystem.cs` —
  `LetterDeliveryRecord`, `AddressLetter`, `DeliverLetter`, `WithholdLetter`,
  `MarkUnanswered`, `RestoreState`; `SurvivorLetterDeliverySystem.cs` —
  `not_found`/`found`/`addressed`/`delivered`/`withheld`/`unanswered`,
  `SurvivorLetterRecordState`, `SurvivorRef`, `DefaultDeliveryMoraleBonus = 8f`;
  narrative corpora `letters_expansion.json`,
  `survivor_letters_lost_kin.json`, `unsent_letters_batch_2.json`,
  `pneumatic_carrier_capsule_logs.json`; confirmed: no host wiring, no
  save section, no mail panel.
- `Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs` (`SystemId` =
  "ecological_infestation", `RecurrenceCooldownDays = 5`,
  `MaxFoodLossPerDay = 3`, `TryTrigger`, `TryClear`, `TryTolerateAndHarvest`,
  `TickDay`); `EcologicalInfestationDefs.cs`; `EcologicalInfestationCatalog.cs`;
  `ecological_infestations.json` (six location and four shelter infestations);
  `EcologicalInfestationSaveStore` (`ecological_infestation`).
- `Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs` (`SystemId` =
  "shelter_social_dynamics", `SocialOutcome`, `SocialEventDefinition`,
  `SurvivorPrivacyProfile` with `PrivacyFatiguePermille` 0..1000 and
  `LastSolitaryRestDay`, `SocialIncidentRecord` with `IsMediated`/`MediatorId`/
  `Resolved`, `ShelterSocialSave`, `BindMediatorSkillProvider`,
  `RegisterSurvivorRoom`, `EvaluateRoomDynamics`, events);
  `ShelterSocialSaveStore`; `ShelterSocialPanel`; `shelter_social_events.json`
  (7,377 B, 8 events).
- `Assets/Ashfall.Core/Narrative/CeremonySystem.cs` (`SystemId` =
  "ceremony_system", `CeremonyDefinition` with `PreparationDays`,
  `RequiredRoomId`, `MoraleBoost` default 25f, `StressRelief` default 20f,
  `TruceDurationDays`, `TruceEligible`, `DisasterPool`; `CeremonySaveState`
  with `CompletedCeremonyIds`, `TotalCeremoniesHeld`,
  `TotalDisastersEncountered`; `ScheduleCeremony`, `ContributeResource`,
  `InviteFaction`, `TickDay(outcomeSummary)`); `CeremonySaveStore` (`ceremony`);
  `CeremonyFestivalPanel`; `ceremonies.json` (5 ceremonies, two truce-eligible,
  two-entry disaster pools).
- Data and governance: `items.json`, architecture map and save-store matrix,
  `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`.
- Wave indexes: `docs/expansions/wave1/WAVE1_INDEX.md` through
  `docs/expansions/wave8/WAVE8_INDEX.md`.