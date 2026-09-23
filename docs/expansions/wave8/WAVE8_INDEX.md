# ASHFALL — Expansion Wave 8 Index (Expansions 47–51)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-24
**Purpose:** Index and evidence summary for the five Wave 8 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin or absent around a working seam, and propose
expansions that attach to existing owners. They are design bibles: they do not
claim paths, change code, or authorize implementation. Any implementation must
later pass through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and
`TEST_POLICY.md`.

---

## The wave theme: keeping

Waves 1–7 built a shelter that survives, works, reaches, and remembers. Wave 8
is about what a community **keeps**: the fire it contains, the evening it makes
worth having, the light it speaks with, the culture it copies forward, and the
machines it maintains. Each plan takes a live system that already simulates
something real — fire, downtime, signaling, decay, machines — and gives it the
practice around the simulation: prevention, culture, patience, stewardship,
care.

| Expansion | Live seam | Current content | Gap |
|---|---|---|---|
| 47 · The Brigade | `ShelterFireHazardSystem` (473 lines: zones, dampers, smoke, CO, heat, brigade, extinguishers; save `shelter_fire`; `FireIncidentPanel`) | **no catalog at all**; live ignition passes an **empty zone list**; zero extinguisher items in `items.json` | zones, loads, inspections, permits, roles, equipment, drills, causes, reviews |
| 48 · The Pastime | `SurvivorDowntimeSystem` (318 lines: `HobbyDef`, sessions, profiles, brawl risk; save `recreation`; `SurvivorDowntimePanel`) | `recreation.json` = **3,726 B / 6 hobbies** | hobbies, materials, clubs, leagues, ensembles, craft and reading circles, field days, records |
| 49 · The Mirror | `HeliographSystem` (270 lines: stations, messages, `MinimumVisibility01`, map and distress ports; save `heliograph`; `Plans94To97Panel`) | `heliograph.json` = **290 B / 2 stations** | sight lines, codes, windows, chains, acks, exposure, lamps, care, logs |
| 50 · The Vault | `CulturalArchiveVaultSystem` (674 lines: degradation clock, restoration, transcription, microfiche, recordings, salons, chronicles; save `cultural_archives`) | `cultural_archive_tomes.json` = **12 tomes**; no vault panel | accessions, conservation, climate, fiche, recording, salons, exhibits, volumes |
| 51 · The Machine | `RoboticsSystem` (366 lines: units, directives, charge, EMP, rogue roll below 250 logic; save `robotics`; `RoboticsWorkshopPanel`) | `robotics.json` = **5 units**; only `directive_idle` exists | directives, tasks, duty, docks, maintenance, protocols, envelopes, names, retirement |

The strongest authority discipline in the wave: fire content extends the live
sim instead of replacing it; recreation never becomes a second morale system;
the mirror never touches radio; the vault never touches research or records;
and the machine bay never gains an autonomous weapon, a needs model, or a power
authority.

---

## The five plans

1. `expansion_47_the_brigade_plan.md` — 70,673 chars.
   Prevention, inspection, permits, brigade roles, equipment, drills,
   evacuation, causes, and blameless review around the live fire sim. Points
   the live empty-zone ignition path at authored data.
2. `expansion_48_the_pastime_plan.md` — 70,744 chars.
   Twenty-four new hobbies, clubs, leagues with handicaps and rest days,
   ensembles, craft and reading circles, field days, and a record cabinet.
   Every effect routes through `NeedsSystem.Modify`.
3. `expansion_49_the_mirror_plan.md` — 70,852 chars.
   Station siting and sight lines, codebooks and countersigns, weather windows,
   relay chains, acknowledgements, exposure ethics, night lamps, and logs.
   Never touches the radio family; honors the live visibility gate.
4. `expansion_50_the_vault_plan.md` — 72,671 chars.
   Accessions with consent and seals, conservation cards, fiche redundancy,
   recordings with consent, salons, exhibits and loans, chronicle volumes. The
   live degradation clock finally has meaningful work beside it.
5. `expansion_51_the_machine_plan.md` — 73,242 chars.
   Safe reactivation, fail-safe directives, duty rotas with retraining plans,
   grid-respecting docks, preventive maintenance, a calm malfunction protocol,
   names, and retirement with parts records. No lethal authority anywhere.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot or Unity reference in Core
  logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with
  integer `schema_version`, validated by `CatalogIntegrityValidator` and
  registered with `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains a non-duplication statement
  and an integration-seam table. No second fire, needs, morale, radio, records,
  research, power, roster, workshop, or machine system is introduced.
- **Deterministic.** Live seeded paths only: the fire tick, hobby sessions, the
  weather-gated visibility check, the vault's daily degradation remainder, and
  the live charge/drain/rogue path. Paired replay hashes must match.
- **Persistence.** New state is additive inside existing owners
  (`ShelterFireSaveState`, `RecreationState`, `HeliographState`,
  `CulturalArchiveVaultSave`, `RoboticsSaveState`). Legacy saves load neutral;
  the Triad drift gate must pass.
- **Tone.** Restrained, human, fictional. No real protocols, texts, songs,
  media, landmarks, or institutions copied.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus
  `--data-integrity-selftest` and `--content-utilization-selftest`. A
  compile-green result is not acceptance.

---

## Ethical and content contracts specific to Wave 8

| Plan | Hard contract |
|---|---|
| 47 Brigade | No arson or sabotage reward; no spectacle; no graphic burns; entry requires pairs and training; children never fight fires; apprentices 16+ and supervised; reviews name conditions, never people |
| 48 Pastime | No wagers or currency; no humiliation, handicaps mandatory; no child labor; no literacy shaming; allocation of rooms respects quiet hours; rest is not a reward to be earned |
| 49 Mirror | Messages are visibly public by construction; countersigns replace secrets; no jamming, spycraft, or interrogation content; invented codes only; lamps obey fire rules; nothing flashed that the shelter would not read aloud |
| 50 Vault | Consent for testimony and personal material; seals are never peeked; accession has provenance and may be refused; no real-world texts; no grave goods; loss is recorded honestly |
| 51 Machine | No autonomous lethal authority; the sentry is sensor-only or dismantled; malfunction is calm engineering, never horror; no personhood or cruelty; displacement requires a retraining plan; EMP is a hazard, never a weapon |

---

## Cross-wave hooks (summary)

- **Within Wave 8:** brigades protect the vault's paper and the machines'
  docks; pastimes and salons share the same week and the same quiet owner; the
  mirror carries the vault's copies as gifts; the bay hauls for the brigade and
  the network.
- **With earlier waves:** the core and grid power the docks; the workshop and
  metrology repair machines and instruments; the press prints codebooks, cards,
  and labels; the school borrows primers; the ward borrows the surgery primer
  and hosts the medical assistant; the watch shares the horizon and the sentry
  feed; the wild and weather supply field days and windows; the quiet governs
  night lamps and salon evenings; the long road carries walkers and convoys.

Each plan is self-contained; none requires another to ship.

---

## How to promote a Wave 8 plan

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
reversible.

---

## Open decisions common to the wave

- Content volume budgets (each plan lists an authoring estimate; the full wave is
  roughly 300,000–330,000 words of new prose if all five are authored).
- Save placement (additive sub-objects versus sibling sections) per domain. Each
  plan recommends additive sub-objects.
- Whether any Wave 8 expansion adds a new headless selftest verb or extends an
  existing one. Each plan recommends extending panel lifecycle, data integrity,
  and content utilization verbs, with a domain verb only if none exists.
- Priority order. Recommended: 47 (The Brigade) first because the live ignition
  path can already start fires with an empty zone list and no prevention
  content; 50 (The Vault) second because the live degradation clock is actively
  losing content; 49 (The Mirror) third because it is infrastructure with two
  stations and no practice; 48 (The Pastime) fourth because it is quality of
  life and depends on nothing; 51 (The Machine) fifth because it is the most
  self-contained and the most sensitive to tone review.

---

## Evidence anchors (file references used across the plans)

- `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` —
  `FireZoneState`, `FireIncidentState`, spread/smoke/CO/heat constants,
  `Ignite`, `RaiseAlarm`, `AssignBrigade`, `SetDamper`, `DeployExtinguisher`,
  `EvacuateZone`, `Tick`, `CaptureState`; `src/Host/ShelterFireHostSession.cs`;
  `src/Host/ShelterFireSaveStore.cs` (`shelter_fire`); `src/UI/FireIncidentPanel.cs`;
  `src/Main.Plans162_165.cs` (live ignition, empty zone list); zero extinguisher
  items in `items.json`.
- `Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs` — `HobbyDef`,
  `ActiveHobbySession`, `SurvivorHobbyProfile`, `RecreationState`,
  `StartSession`, `CompleteSession`, `NeedsSystem.Modify`, brawl handling,
  events; `recreation.json` (3,726 B, 6 hobbies); `RecreationSaveStore`;
  `SurvivorDowntimePanel`.
- `Assets/Ashfall.Core/HeliographSystem.cs` — `HeliographStationDefinition`,
  `HeliographStationState`, `HeliographCatalog`, `HeliographMessageState`,
  `HeliographState`, `MinimumVisibility01 = 0.35f`, `RegisterStation`,
  `SetStationCondition`, `LoadCatalog`, `Transmit`, `CaptureState`,
  `RestoreState`, events; map reveal and distress dispatch ports;
  `HeliographHostSession`; `HeliographSaveStore` (`heliograph`);
  `heliograph.json` (290 B, two stations).
- `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs` —
  `ArchiveDocumentState`, `ArchiveProjectState`, `ArchiveRecordingState`,
  `SalonState`, `ArchiveChronicleEntry`, `CulturalArchiveVaultSave`,
  `RestorationReliefPermille = 350`, `LegibilityLimitPermille = 900`,
  `LostThresholdPermille = 1000`, `BaseDailyDegradationPermille = 2f`,
  `SalonDefaultDurationDays = 5`, `SalonCooldownDays = 10`, all vault events;
  `src/Main.FlagshipInstitutions.cs` (`EnsureCulturalArchive`); `ArchiveDeskSystem`;
  `CulturalArchiveSaveStore` (`cultural_archives`); `cultural_archive_tomes.json`
  (9,530 B, 12 tomes).
- `Assets/Ashfall.Core/Crafting/RoboticsSystem.cs` — `RobotDefinition`,
  `RobotUnitState`, `RoboticsSaveState`, `LoadCatalog`, `ReactivateRobot`,
  `ProgramDirective`, `ApplyEmpShock`, `TickLabor`, `RepairRobot`,
  `CaptureState`, `RestoreState`, events, rogue roll gated on
  `LogicIntegrity < 250`; `RoboticsSaveStore` (`robotics`);
  `RoboticsWorkshopPanel`; `robotics.json` (4,431 B, 5 units).
- Data and governance: `items.json`, `architecture` map and save-store matrix,
  `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`.
- Wave indexes: `docs/expansions/wave1/WAVE1_INDEX.md` through
  `docs/expansions/wave7/WAVE7_INDEX.md`.