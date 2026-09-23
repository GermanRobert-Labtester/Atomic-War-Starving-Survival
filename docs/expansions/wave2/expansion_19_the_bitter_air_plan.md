# ASHFALL — Expansion 19 Design Bible
# THE BITTER AIR
### Wave 2 · Chemical and Biological Hazards, Quarantine, Decontamination, and Toxic Legacy

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Combat` (ChemWarfare), `Ashfall.Core.Disease` (Pathogen, Quarantine, DiseaseSystem), `Ashfall.Core` (Decontamination), `Ashfall.Core.Crafting` (ChemicalSynthesis), `Ashfall.Core.Medical` (ChemicalDependency)
**Proposed host owner:** `BitterAirHostSession` (extends `ChemWarfareSaveStore` + `DecontaminationHostSession` + `PathogenStrainSaveStore`)
**Existing save sections:** `chem_warfare`, `decontamination`, `disease`, `chemical_synthesis`, `chemical_recon`, `chemical_dependency`
**Existing CLI verbs:** `--chemical-recon-selftest`, `--decon-airlock-selftest`, `--disease-selftest`, `--pathogen-strain-selftest`, `--chemical-dependency-save-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; fictionalized hazards only; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already models hazards that travel through air and people. `ChemWarfareSystem`
(Plan 198) loads fictional toxic agents from `chemical_weapons.json` with density,
persistence, filter wear, and exposure severity, and tracks tactical hazard zones.
`DecontaminationSystem` runs a decon queue, shelter contamination, effluent tanks,
filters, sludge, and manual overrides. `PathogenStrainSystem` (Plan 155) merges
fictional strains into the canonical `DiseaseSystem`, mutates them deterministically,
couples them to radiation, and runs cure projects. `DiseaseQuarantineCoordinator`
handles quarantine. `ChemicalReconEngine`, `ChemicalSynthesisSystem`, and
`ChemicalDependencySystem` cover detection, production, and dependency. But the
authored content is thin: **a handful of agents, a handful of pathogens, and small
protocol tables.**

**The Bitter Air** turns that machinery into a persistent environmental and social
hazard: plumes that drift across the map, air quality in the shelter, filter
logistics, quarantine as a way of life, sites that stay poisonous for years, and the
moral weight of keeping — or using — weapons that poison the air.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The Exchange did not only burn. It also leaked, spilled, vented, and buried. The
wasteland's air is not just radioactive; it carries the dissolved remains of a
chemical war that ended before anyone agreed it had begun. The shelter breathes
through filters, drinks through treatment, and walks with a mask on its belt.

**The Bitter Air** is the expansion about that inheritance. It adds drifting hazard
plumes driven by live weather, shelter air quality as a managed resource, gas masks
and filter logistics, quarantine as a recurring social institution, contaminated
sites that can be remediated or abandoned, and a sealed strain vault that raises the
oldest question in the wasteland: is a weapon you never use still a weapon?

The expansion is deliberately uncomfortable. Chemical and biological weapons are
presented as deterrent, taboo, and catastrophe — never as a fun combat option, never
as a power fantasy, and never using real-world agents, properties, or designations.

### 1.2 The five loops it adds

```
    Source ──► Plume drift ──► Exposure ──► Triage/Decon ──► Quarantine
       │            │              │             │              │
       ▼            ▼              ▼             ▼              ▼
   Site cleanup  Weather    Masks/filters   Effluent       Society:
   or abandon    coupling   logistics       management     trust & fear
       │
       ▼
   Toxic legacy ──► soil/water ──► farms, wells, shelter intake
       │
       ▼
   Strain vault ──► cure research ──► deterrence debate
```

### 1.3 What the player manages

1. **Air.** `PowerGridSystem` already powers `room_air_filtration` with a
   `fx_filtration_off` failure. The expansion adds air quality: dust, spores,
   vapors, and the filter states that keep them out.
2. **Plumes.** Weather-driven hazard clouds that cross the region, using live wind
   and precipitation. They force shelter lockdowns and reroute expeditions.
3. **Masks and filters.** A `filter_wear_permille` field already exists on toxic
   agents. The expansion adds mask fit, canister inventory, pump units, and the
   quiet horror of wearing a mask for weeks.
4. **Decontamination.** The live decon queue, effluent tank, and sludge become a
   managed system with real discharge consequences.
5. **Quarantine.** `DiseaseQuarantineCoordinator` gains zones, lockdown policy,
   supply rationing, and escape attempts.
6. **Toxic legacy.** Contaminated soil, wells, and buildings persist for years;
   remediation costs materials and lives.
7. **The vault.** Sealed strain and agent samples become a research and deterrence
   asset — and a moral decision.

### 1.4 What it is not

- Not a real-world CBRN simulator. All agents and strains are fictional.
- Not a second disease system. Everything routes through `DiseaseSystem` and the
  strain layer.
- Not a second hazard model. Gas/flood/collapse/rad route through live owners.
- Not a combat buff. Using chemical weapons in combat produces lasting social and
  environmental costs, never a clean win.
- Not a second save authority.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Combat/ChemWarfareSystem.cs` | Toxic agents, hazard zones, density, persistence, filter wear | `LIVE` |
| `Assets/Ashfall.Core/DecontaminationSystem.cs` | Decon queue, shelter contamination, effluent, sludge, overrides | `LIVE` |
| `Assets/Ashfall.Core/Disease/PathogenStrainSystem.cs` | Strain merging, mutation, rad coupling, cures | `LIVE` |
| `Assets/Ashfall.Core/Disease/PathogenStrainCatalog.cs` | Strain definitions | `LIVE` |
| `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs` | Quarantine authority | `LIVE` |
| `Assets/Ashfall.Core/Disease/` (10 files) | Canonical disease engine | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/ChemicalReconEngine.cs` | Detection and recon | `LIVE` |
| `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` | Synthesis | `LIVE` |
| `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | Dependency | `LIVE` |
| `src/Host/ChemWarfareSaveStore.cs` etc. | Persistence | `LIVE` |
| `src/UI/ChemUI.cs`, `src/UI/ChemicalLabPanel.cs` | UI | `LIVE` |
| `src/Host/DecontaminationHostSession.cs` | Host | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Entries | Notes |
|---|---|---|
| `chemical_weapons.json` | small | fictional toxic agents |
| `pathogens.json` | 2.7 KB | strain definitions |
| `contagion_events.json` | 2.8 KB | outbreak events |
| `toxic_chemical_catalog.json` | 9.2 KB | industrial toxics |
| `decontamination_protocol_catalog.json` | 9.3 KB | decon methods |
| `chemical_syntheses.json` | 7 KB | synthesis routes |
| `chemical_dependency_items.json` | 2.8 KB | dependency items |
| `narcotics.json` | 6.4 KB | narcotics |
| `disease_catalog.json` | **54 KB** | canonical diseases |
| `autopsy_procedures.json` | 7.6 KB | pathology |

### 2.3 Confirmed gaps

- **GAP-19-1 — No plume system.** Hazard zones exist in combat lanes only; nothing
  models a cloud crossing the map on wind.
- **GAP-19-2 — No shelter air quality.** Filtration is on/off power; there is no
  dust, spore, or vapor load, and no partial-failure model.
- **GAP-19-3 — No mask logistics.** Filter wear exists per agent; there is no mask,
  canister, fit, or supply system.
- **GAP-19-4 — Quarantine is coordination, not society.** No zones, no lockdown
  policy, no supply pressure, no escape, no social cost.
- **GAP-19-5 — No toxic legacy.** Contaminated sites do not persist on the map or in
  soil/water.
- **GAP-19-6 — No strain vault or deterrence.** Weapons exist as data; there is no
  custody, taboo, or diplomatic system around them.
- **GAP-19-7 — Effluent discharge is undermodeled.** Tanks and sludge exist but no
  outfall consequence to water or soil.
- **GAP-19-8 — Few agents and strains.** The catalogs are demonstrations.
- **GAP-19-9 — No hazard locations or NPCs.** No toxic marsh, sealed lab, filter
  works, or quarantine camp.

### 2.4 Non-duplication statement

This expansion will **not** add a second disease, hazard, decon, synthesis,
dependency, weather, or save system. It extends the live owners and consumes
`WeatherSystem`, `WaterTreatmentSystem`, `PowerGridSystem`, `NeedsSystem`, and
`MedicalPipelineCoordinator`. All content is fictionalized; no real agent, strain,
precursor, or process is modeled.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Air is a commons.** A plume does not care about walls, factions, or
treaties. It forces cooperation and exposes hypocrisy.

**Pillar 2 — Protection is logistics.** Masks, filters, pumps, seals, and clean
rooms are a supply chain, not a stat. The shelter is only as safe as its weakest
canister.

**Pillar 3 — Contamination is patient.** Poisoned ground stays poisoned. Cleanup is
slow, expensive, and incomplete.

**Pillar 4 — Quarantine is a social institution.** Locking people away saves lives
and costs trust. The expansion makes both true.

**Pillar 5 — The weapon is the taboo.** The expansion never rewards chemical or
biological use. Custody, deterrence, and disposal are the interesting decisions.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A mask | Fogged lenses, a voice made flat | Tactical cool |
| A plume | A smell, a bird falling, a closed valve | Green gas clouds |
| Quarantine | A line on the floor, a family on both sides | Prison drama |
| Decon | Water, soap, waiting, paperwork | Sci-fi sterilization |
| Cleanup | Lime, ash, wheelbarrows, years | Instant purge |
| The vault | A cold cabinet and a signature | Doomsday trophy room |

### 3.3 Content limits

- All agents, strains, and precursor routes are fictional and abstract. No real
  chemical weapon, pathogen, toxin, or synthesis route is described.
- No weapon-use power fantasy; use produces catastrophe, not victory.
- No torture, no medical horror for shock, no exploitation of suffering.
- No real-world chemical, biological, or military designation is used.

---

## 4. THE BITTER AIR WORLD

### 4.1 Interior rooms

- **`room_air_lock`** — a proper two-door airlock with wash-down.
- **`room_filter_works`** — canister assembly, media, and testing.
- **`room_clean_ward`** — positive-pressure isolation care.
- **`room_quarantine_block`** — segregated bunks and a guarded line.
- **`room_decon_bay`** — showers, tanks, and effluent routing.
- **`room_air_lab`** — sampling, assay, and air-quality monitoring.
- **`room_agent_vault`** — sealed custody for agents and strains.
- **`room_mask_store`** — masks, canisters, pumps, and fit kits.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_gas_works` | The Gas Works | 7 | Industrial toxic salvage; plume source |
| `loc_sealed_lab` | The Sealed Lab | 8 | Pre-war research; strains and records |
| `loc_toxic_marsh` | The Bitter Marsh | 6 | Persistent contamination; remediation site |
| `loc_filter_factory` | The Filter Works | 5 | Canister manufacturing salvage |
| `loc_quarantine_camp` | The Wire Camp | 5 | A ruined quarantine settlement |
| `loc_spill_field` | The Spill | 7 | Chemical spill zone; soil legacy |
| `loc_burn_pits` | The Burn Pits | 6 | Waste disposal; air hazard |
| `loc_spring_head` | The Spring Head | 4 | A clean spring; contested water |
| `loc_siren_tower` | The Siren Tower | 5 | Regional warning network |
| `loc_border_checkpoint` | The Wash Line | 5 | A crossing with a decon gate |

All locations require valid item references and scanner registration.

### 4.3 The plume map

Plumes are not new map nodes. They are moving hazard volumes with a source, a shape,
a density, and a direction derived from live weather. The existing map shows them as
weather overlays; the existing weather authority owns wind and rain. The expansion
owns only the plume model.

---

## 5. MAIN STORYLINE — "WHAT DRIFTED IN"

### 5.1 Central conflict

A siren tower starts broadcasting a warning no one can interpret: a plume is moving
across the region, and it is not radioactive. A settlement to the east is in its
path. The shelter has 300 masks — and 460 people. The choice is not whether to help,
but who, and how many, and what happens to the ones who are turned away.

The plume originates from a pre-war gas works that the Foundry has been scavenging
for months. The quartermaster wants the salvage. The physician wants the site sealed.
And in the Sealed Lab, behind a door that has held for decades, there is a strain
cabinet and a custody ledger: the old world's biological deterrents, still viable,
still signed for, and now owned by whoever is standing there.

The expansion's question: **when the air is the weapon, who gets to breathe?**

### 5.2 Theme (unspoken)

**You cannot own the air. You can only decide who you warn, who you protect, and
what you do with what you find.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_physician_asa_vell` | Dr. Asa Vell | Shelter physician | Quarantine authority; exhaustion and duty |
| `npc_warden_koval` | Warden Koval | Quarantine warden | Enforces the line; not unkind |
| `npc_chemist_ren_basil` | Ren Basil | Chemist | Detection and synthesis; ethically slippery |
| `npc_quartermaster_lowe` | Lowe | Quartermaster | Wants the salvage; counts the living |
| `npc_guardian_mira_shen` | Mira Shen | Mask sergeant | Supplies and fit; quartermaster of safety |
| `npc_outlander_tam_wick` | Tam Wick | Quarantine escapee | Represents the cost of the line |
| `npc_archivist_strain_doss` | Doss | Lab archivist | Custody ledger; guardian of the vault |
| `npc_child_clean_junip` | Junip | Clean ward child | The human stake in the vault decision |

### 5.4 Story beats (15)

1. **The Siren.** An uninterpretable warning crosses the region.
2. **The Model.** Ren models the plume with the shelter's weather data.
3. **The Line.** The east camp asks for masks; supply is short.
4. **The Works.** The plume source is identified at the Gas Works.
5. **The Seal.** The physician demands the works be closed; the quartermaster refuses.
6. **The First Cough.** An early exposure; triage begins.
7. **The Airlock.** Decon is overloaded; effluent spikes.
8. **The Line Holds.** Quarantine is declared; a family is split.
9. **The Escape.** Tam Wick breaks quarantine.
10. **The Sealed Lab.** The vault is found; the custody ledger names the dead.
11. **The Debate.** What to do with the strains: keep, destroy, or trade.
12. **The Deterrence.** A rival learns of the vault and makes a demand.
13. **The Second Plume.** A larger plume; the shelter must choose who to warn.
14. **The Clean Ground.** Remediation begins or the site is abandoned.
15. **What Breathes.** Final disposition of the vault, the line, and the ground.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Mask distribution | shelter-first / share / sell | self vs. region |
| Gas Works | seal / salvage / burn | safety vs. gain |
| Quarantine | strict / humane / open | control vs. trust |
| Escapee | punish / exile / pardon | order vs. mercy |
| Vault | keep / destroy / trade | deterrence vs. taboo |
| Deterrence demand | refuse / bluff / comply | war vs. peace |
| Second plume warning | warn all / warn allies / stay silent | commons vs. interest |
| Ground | remediate / fence / abandon | commitment vs. cost |

### 5.6 Endings (5 + fade)

1. **The Common Air** — regional cooperation; the siren network becomes a shared institution.
2. **The Clean Line** — the shelter survives through discipline; the region does not forgive.
3. **The Empty Vault** — the strain cabinet is destroyed; some deterrence is lost and some future is bought.
4. **The Bitter Ground** — remediation succeeds slowly; the marsh becomes a meadow in a decade.
5. **The Open Cabinet** — the vault is traded; the region's balance shifts toward fear.
6. **Fade** — the plume passes; nothing is decided.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_air_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (15)

`quest_air_siren`, `quest_air_model`, `quest_air_the_line`, `quest_air_the_works`,
`quest_air_the_seal`, `quest_air_first_cough`, `quest_air_airlock`,
`quest_air_line_holds`, `quest_air_the_escape`, `quest_air_sealed_lab`,
`quest_air_the_debate`, `quest_air_deterrence`, `quest_air_second_plume`,
`quest_air_clean_ground`, `quest_air_what_breathes`.

### 6.2 Side quests (28)

**Air quality (5)**
- `quest_air_filter_change` — a filter change under load
- `quest_air_pressure_test` — find a leak in the clean ward
- `quest_air_dust_watch` — dust load monitoring
- `quest_air_spore_season` — a seasonal spore surge
- `quest_air_co2_buildup` — crowding raises CO2 in a sealed room

**Masks and filters (5)**
- `quest_air_mask_fit` — fit masks for odd faces
- `quest_air_canister_line` — assemble canisters by hand
- `quest_air_pump_unit` — powered respirators need power
- `quest_air_mask_theft` — masks are stolen
- `quest_air_child_mask` — masks for children

**Plume and weather (4)**
- `quest_air_wind_read` — interpret wind for a plume path
- `quest_air_rain_wash` — rain scrubs a plume and concentrates it
- `quest_air_reroute` — move an expedition around a plume
- `quest_air_false_alarm` — the siren is wrong; trust cost

**Decontamination (4)**
- `quest_air_decon_queue` — triage the decon line
- `quest_air_effluent_spike` — the tank is full
- `quest_air_sludge_disposal` — where the sludge goes
- `quest_air_manual_override` — skip a wash step and pay for it

**Quarantine (5)**
- `quest_air_zone_setup` — build the quarantine block
- `quest_air_supply_line` — feed people behind the line
- `quest_air_family_split` — a child on the wrong side
- `quest_air_escape_watch` — watch for a break
- `quest_air_release_day` — the end of quarantine and its aftermath

**Toxic legacy (3)**
- `quest_air_soil_test` — test contaminated ground
- `quest_air_lime_ground` — begin remediation
- `quest_air_well_test` — a well may be poisoned

**Vault and deterrence (2)**
- `quest_air_custody_ledger` — read the ledger
- `quest_air_vault_choice` — keep, destroy, or trade

### 6.3 Repeatable quests (8)

`quest_air_repeat_filter`, `quest_air_repeat_air`, `quest_air_repeat_decon`,
`quest_air_repeat_quarantine`, `quest_air_repeat_siren`,
`quest_air_repeat_sample`, `quest_air_repeat_remediate`,
`quest_air_repeat_vault_watch`.

### 6.4 Dynamic hooks

Live systems emit exposure, filter-wear, decon, contamination, infection, mutation,
and dependency events. The generator attaches authored follow-ups without a new bus.

### 6.5 Constraints

- No quest may require or reward using a chemical or biological weapon.
- No agent or strain may be described with real-world properties or names.
- Quarantine consequences route through `NeedsSystem`, `MoraleContagionSystem`, and
  `GuiltInsomniaSystem`; no parallel trust meter.
- Effluent discharge routes to the water authority; no parallel pollution model.
- Exposure routes through the medical pipeline; no parallel harm model.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `HazardPlumeSystem` (new, `Ashfall.Core.Combat`)

**Owns:** plume source, shape, density, drift, deposition, and decay.
**Consumes:** `WeatherSystem` wind/rain, `ChemWarfareSystem` agent definitions, map
cells. **Data:** `hazard_plumes.json`.
**Rules:** a plume moves with wind and loses density with rain and distance; exposed
cells gain a lingering contamination; the system never invents weather.

### 7.2 `AirQualitySystem` (new, `Ashfall.Core.Shelter`)

**Owns:** shelter air load (dust, spores, vapor, CO2), filtration efficiency, and
room air state. **Consumes:** `PowerGridSystem` filtration room, weather, plume
proximity, occupancy. **Data:** `air_quality_profiles.json`.
**Rules:** filtration reduces load at a rate proportional to power and filter
condition; a partial failure degrades air rather than switching it off.

### 7.3 `RespiratorSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** masks, canisters, fit quality, pump units, and filter wear.
**Consumes:** `Inventory`, `ChemWarfareSystem.filter_wear_permille`, `PowerGridSystem`.
**Data:** `masks_filters.json`.
**Rules:** protection is bounded and consumable; a fitted mask protects far more than
a loose one; canisters deplete and cannot be refilled without media.

### 7.4 `QuarantineZoneSystem` (new, `Ashfall.Core.Disease`)

**Owns:** quarantine zones, population assignment, lockdown policy, supply draw,
escape risk, and release. **Consumes:** `DiseaseQuarantineCoordinator`,
`DiseaseSystem`, `NeedsSystem`, `MoraleContagionSystem`. **Data:**
`quarantine_zones.json`.
**Rules:** quarantine reduces spread and raises social cost; the system never
duplicates the coordinator's infection logic.

### 7.5 `ToxicLegacySystem` (new, `Ashfall.Core.World`)

**Owns:** persistent contaminated sites in soil, water, and structures, and their
remediation. **Consumes:** `HazardPlumeSystem`, `WaterTreatmentSystem`, `Farming`
soil, `Excavation` ground. **Data:** `contaminated_sites.json`.
**Rules:** contamination decays slowly or not at all; remediation consumes lime,
ash, labor, and time; crops and wells can be damaged.

### 7.6 `StrainVaultSystem` (new, `Ashfall.Core.Disease`)

**Owns:** custody of fictional agents and strains, access control, and disposition.
**Consumes:** `PathogenStrainSystem`, `ChemWarfareSystem`, `FactionStanceEngine`.
**Data:** `strain_vault.json`.
**Rules:** the vault produces no benefit except research, deterrence, and standing;
using it in the field is possible and catastrophic. Core never resolves use; it
emits a typed event and the host applies consequences.

### 7.7 `DeterrenceSystem` (new, thin, `Ashfall.Core.Factions`)

**Owns:** the diplomatic weight of holding a vault, rival demands, bluffing, and
taboo effects. **Consumes:** `FactionStanceEngine`, `StrainVaultSystem`.
**Data:** `deterrence_doctrines.json`.
**Rules:** deterrence is political, not mechanical power; a bluff can be called.

### 7.8 Systems explicitly not added

- No second disease, hazard, decon, water, or diplomacy system.
- No real-world chemical or biological content.
- No weapon-use reward loop.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `chemical_weapons.json` (extend)

Existing schema preserved (`id`, `display_name`, `hazard_class`,
`base_density_permille`, `persistence_ticks`, `filter_wear_permille`,
`exposure_severity`, `visual_profile_id`, `description`). New fictional agents span
classes: irritant, choking, blister, nerve-analogue (abstract), smoke, and
incapacitant. All names and properties are invented and abstract.

### 8.2 `pathogens.json` (extend)

Strain rows with fictional names, transmission class, incubation, outcome class, and
mutation siblings. No real pathogen is referenced.

### 8.3 `contagion_events.json` (extend)

Outbreak event rows: trigger, severity, spread modifiers, quarantine interaction.

### 8.4 `toxic_chemical_catalog.json` (extend)

Industrial toxics for sites, spills, and salvage.

### 8.5 `decontamination_protocol_catalog.json` (extend)

Methods: dry brush, wet wash, soap, oxidizer, adsorption, isolation, disposal.

### 8.6 `hazard_plumes.json` (new)

```json
{
  "schema_version": 1,
  "plumes": [
    {
      "plume_id": "plume_gas_works_cloud",
      "display_name": "Works Drift",
      "agent_id": "agent_irritant_a",
      "source_location_id": "loc_gas_works",
      "base_density_permille": 600,
      "release_rate": 0.35,
      "wind_coupling": 0.9,
      "rain_washout": 0.4,
      "deposition_rate": 0.15,
      "decay_per_day": 0.08,
      "warning_profile": "siren_2",
      "tags": ["drift", "industrial", "persistent_source"]
    }
  ]
}
```

### 8.7 `air_quality_profiles.json` (new)

Room profiles: baseline load, filtration response, safe thresholds, failure effects.

### 8.8 `masks_filters.json` (new)

Mask types, fit classes, canister types, media, wear rates, power needs.

### 8.9 `quarantine_zones.json` (new)

Zone rows: capacity, separation, supply draw, escape risk, morale cost, release
criteria.

### 8.10 `contaminated_sites.json` (new)

Site rows: linked location, contaminant class, soil/water/structure load, decay
rate, remediation recipe, crop/well effects.

### 8.11 `strain_vault.json` (new)

Vault inventory: agent/strain id, viability, custody signature, research value,
deterrence weight, disposal options.

### 8.12 `deterrence_doctrines.json` (new)

Doctrine rows: posture, demand response, bluff success model, taboo effects.

### 8.13 Items

New items: `item_mask_cloth`, `item_mask_rubber`, `item_canister_charcoal`,
`item_canister_oxidizer`, `item_filter_media`, `item_pump_respirator`,
`item_decon_soap`, `item_decon_oxidizer`, `item_sealant_paste`,
`item_air_sample_tube`, `item_lime_sack`, `item_effluent_test_kit`,
`item_strain_case`, `item_custody_seal`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Existing stores: `ChemWarfareSaveStore`, `DecontaminationHostSession`,
`PathogenStrainSaveStore`, chemical synthesis/recon/dependency stores. New
sub-objects are additive inside the existing envelopes.

### 9.2 State to persist

- Active plumes and deposited contamination.
- Shelter air load and filter condition.
- Mask/canister inventory and wear.
- Quarantine zones, assignments, and release timers.
- Contaminated sites and remediation progress.
- Vault inventory, access, and disposition.
- Deterrence posture and rival demands.

### 9.3 Determinism

- Plume drift is a pure function of live weather plus authored rates.
- Exposure, infection, mutation, and cure remain in the live systems' seeded paths.
- No wall clock; no `System.Random`.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with no plumes, no air load, no zones, no vault, and neutral
posture. Existing disease, decon, and dependency state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille for density, load, wear, and
contamination.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `ChemUI` (extend) | Agents, hazards, protection status | `BitterAirHostSession` |
| `AirQualityPanel` (new) | Shelter air load, filtration, rooms | same |
| `PlumePanel` (new) | Active plumes, drift, ETA, warnings | same |
| `MaskPanel` (new) | Masks, canisters, fit, wear, pumps | same |
| `DeconPanel` (extend) | Queue, effluent, sludge, overrides | same |
| `QuarantinePanel` (new) | Zones, population, supplies, release | same |
| `ToxicLegacyPanel` (new) | Sites, contamination, remediation | same |
| `VaultPanel` (new) | Custody, research, deterrence, disposal | same |
| `SirenNetworkPanel` (new) | Regional warnings and trust | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Hazard status uses text plus icon, never color alone.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Irreversible actions (disposal, weapon use, vault destruction) require explicit
  confirmation with stated consequences.
- Plume ETA and uncertainty are shown honestly; forecasts can be wrong.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: siren rising, mask seal, canister click,
air hiss, decon spray, quarantine latch, vault seal. No cue is required; text
carries meaning. The vault has no triumphant cue.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ChemWarfareSystem` | Agent definitions and hazard zones extended |
| `DecontaminationSystem` | Queue, effluent, sludge, overrides consumed |
| `PathogenStrainSystem` | Strains and cures; vault consumes, never duplicates |
| `DiseaseSystem` | Canonical infection and spread |
| `DiseaseQuarantineCoordinator` | Zones consume the coordinator |
| `ChemicalReconEngine` | Detection and sampling |
| `ChemicalSynthesisSystem` | Decon and canister chemistry |
| `ChemicalDependencySystem` | Exposure and dependency routing |
| `WeatherSystem` | Plume drift and washout |
| `PowerGridSystem` | Filtration, pumps, air lab |
| `WaterTreatmentSystem` | Effluent and well contamination |
| `Farming` | Soil contamination and crop damage |
| `NeedsSystem` | Quarantine morale and supply pressure |
| `MoraleContagionSystem` | Fear and rumor |
| `GuiltInsomniaSystem` | Quarantine and weapon-use guilt |
| `FactionStanceEngine` | Vault deterrence and warning trust |
| `MedicalPipelineCoordinator` | Exposure triage and care |
| `MemorialSystem` | Hazard deaths |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `ChemWarfareSystem`, `DecontaminationSystem`,
`PathogenStrainSystem`, `DiseaseQuarantineCoordinator`, `ChemicalReconEngine`,
`ChemicalSynthesisSystem`, `ChemicalDependencySystem`, save stores, and panels.
Record file:line; change nothing.

**Phase 1 — Data + validators.** Extend agents, pathogens, contagion events, toxics,
decon protocols; author plumes, air profiles, masks, zones, sites, vault, doctrines.
Register validators and scanner.

**Phase 2 — Pure Core.** `HazardPlumeSystem`, `AirQualitySystem`,
`RespiratorSystem`, `QuarantineZoneSystem`, `ToxicLegacySystem`,
`StrainVaultSystem`, `DeterrenceSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `BitterAirHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 90/180-day soak including plume seasons and quarantine cycles.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Toxic agents | 20 new |
| Pathogen strains | 20 new |
| Contagion events | 20 new |
| Industrial toxics | 30 new |
| Decon protocols | 20 new |
| Hazard plumes | 12 |
| Air quality profiles | 12 |
| Mask/canister types | 20 |
| Quarantine zones | 8 |
| Contaminated sites | 20 |
| Vault entries | 15 |
| Deterrence doctrines | 6 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 28 |
| Repeatable | 8 |
| Items | 14 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Real-world CBRN content leak | Critical | Fictional-only review gate |
| Weapon-use reward | Critical | Catastrophic consequences; no buffs |
| Second disease system | High | Route through live owners |
| Plume model drifts from weather | High | Read live weather only |
| Quarantine abused as a feel-bad | Medium | Humane options with real costs |
| Effluent pollution breaks water | Medium | Coupled to live water authority |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §22 |

---

## 13. TEST AND VERIFICATION PLAN

### 13.1 New test files

- `Ashfall.Core.Tests/Combat/HazardPlumeTests.cs`
- `Ashfall.Core.Tests/Shelter/AirQualityTests.cs`
- `Ashfall.Core.Tests/Shelter/RespiratorTests.cs`
- `Ashfall.Core.Tests/Disease/QuarantineZoneTests.cs`
- `Ashfall.Core.Tests/World/ToxicLegacyTests.cs`
- `Ashfall.Core.Tests/Disease/StrainVaultTests.cs`
- `Ashfall.Core.Tests/Factions/DeterrenceTests.cs`
- `Ashfall.Core.Tests/Combat/BitterAirSaveRoundTripTests.cs`
- `Ashfall.Core.Tests/Combat/BitterAirDeterminismTests.cs`
- `Ashfall.Core.Tests/Content/HazardCatalogIntegrityTests.cs`

### 13.2 Required assertions

- Plume drift is a pure function of live weather; no invented wind.
- Rain washes a plume out and deposits contamination.
- Air quality degrades on partial filtration failure, never silently.
- Mask protection is bounded by fit and canister wear.
- Quarantine reduces spread and raises morale/supply cost.
- Effluent routes to the water authority; no parallel pollution.
- Contaminated ground damages crops/wells and remediates slowly.
- Vault grants no combat power; weapon use is catastrophic and typed.
- Deterrence can be bluffed and called.
- Round-trip restores plumes, air, masks, zones, sites, vault.
- Legacy loads neutral; paired replay hash equality.

### 13.3 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
bash scripts/run_test.sh Ashfall.Core.Tests/Disease/
godot --headless --path . -- --chemical-recon-selftest
godot --headless --path . -- --decon-airlock-selftest
godot --headless --path . -- --disease-selftest
godot --headless --path . -- --chemical-dependency-save-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
dotnet build Ashfall.csproj --no-restore
```

---

## 14. ACCEPTANCE CRITERIA

Core authority documented and engine-free; data canonical with valid schema and
passing integrity; persistence round-trips with neutral legacy and Triad parity;
determinism proven; host reachable by a real route/event; player can observe the
outcome; focused tests green; docs updated; fictional-content audit passed.
Compile-green is not acceptance.

---

## 15. CROSS-EXPANSION HOOKS

| Expansion | Hook |
|---|---|
| 12 The Second Generation | Children in quarantine; mask fit for small faces |
| 13 The Faithful | Clean-air purity movement; quarantine as rite |
| 14 Above the Ash | Plume vs. flight windows; aerial sampling |
| 15 The Deep Root | Contaminated soil; remediation and crop loss |
| 16 The Rebuilt Body | Respirator prosthetics; sealed-suit work |
| 17 The Long Evening | Sirens interrupt festivals; quarantine songs |
| 18 The Underneath | Gas leached from deep; sealed strata |
| 20 The Quiet Hand | Vault espionage; stealing a strain |
| 21 The Grid | Filtration load; decon power demand |

---

## 16. LORE AND CONTINUITY CHECK

### 16.1 Must not contradict

- The live agent schema and `filter_wear_permille`.
- The decon queue, effluent, and sludge state.
- The strain layer's merge/mutation/cure contract.
- `DiseaseSystem` as the sole infection authority.
- The fictional-content rule (critical).

### 16.2 New canon

- The siren network and the uninterpretable warning.
- The Gas Works as a persistent source.
- The Sealed Lab and the custody ledger.
- The Wash Line and quarantine as a regional institution.

### 16.3 Provenance

Registered in `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when integrated.

---

## 17. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `chemical_weapons.json` | +20 | 4,000 |
| `pathogens.json` | +20 | 4,000 |
| `contagion_events.json` | +20 | 3,000 |
| `toxic_chemical_catalog.json` | +30 | 5,000 |
| `decontamination_protocol_catalog.json` | +20 | 4,000 |
| `hazard_plumes.json` | 12 | 2,500 |
| `air_quality_profiles.json` | 12 | 2,000 |
| `masks_filters.json` | 20 | 3,000 |
| `quarantine_zones.json` | 8 | 2,000 |
| `contaminated_sites.json` | 20 | 4,000 |
| `strain_vault.json` | 15 | 3,000 |
| `deterrence_doctrines.json` | 6 | 2,000 |
| Quest objectives | 51 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 14 | 2,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~69,000** |

---

## 18. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R19-1 | Real CBRN leak | Low | Critical | Fictional-only audit |
| R19-2 | Weapon reward loop | Low | Critical | Catastrophe only |
| R19-3 | Second disease system | Low | High | Route through owners |
| R19-4 | Plume model drifts | Med | High | Live weather only |
| R19-5 | Quarantine feel-bad | Med | Med | Humane options |
| R19-6 | Effluent breaks water | Med | Med | Coupled water authority |
| R19-7 | Determinism | Low | High | Live seeded paths |
| R19-8 | Content overrun | Med | Med | Budget §17 |
| R19-9 | Vault trivializes diplomacy | Med | Med | Posture, not power |
| R19-10 | Mask logistics tedious | Med | Med | Automation-friendly UI |

---

## 19. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Are plumes visible on the map?** Recommended: yes, as weather-style overlays
   with honest uncertainty.
2. **Can a plume kill without intervention?** Recommended: yes, through exposure and
   the medical pipeline, never as a scripted wipe.
3. **Does the vault ever provide a mechanical benefit?** Recommended: research and
   standing only.
4. **Can quarantine be escaped by the player's own survivors?** Recommended: yes,
   with real consequences.
5. **Is weapon use possible at all?** Recommended: yes, but only through an explicit
   irreversible event with catastrophic standing, morale, and environmental costs —
   and never as a tactical advantage.

---

## 21. APPENDIX D — FICTIONAL AGENT CLASS TABLE

All names are invented and abstract. No real agent, class, or property is represented.

| Class | Behavior | Persistence | Filter wear | Severity | Use |
|---|---|---|---|---|---|
| Irritant A | eye and airway sting | low | 40 | 1 | area denial |
| Irritant B | throat burn | low | 45 | 2 | crowd control |
| Choking A | deep lung | med | 70 | 3 | trench legacy |
| Choking B | delayed edema | med | 80 | 4 | rare |
| Blister A | skin lesions | high | 90 | 4 | taboo |
| Blister B | delayed wounds | high | 95 | 5 | taboo |
| Nerve-analogue A | abstract paralysis | med | 110 | 5 | forbidden |
| Nerve-analogue B | abstract seizure | med | 120 | 5 | forbidden |
| Smoke A | visibility | low | 30 | 1 | screening |
| Smoke B | heat and particles | low | 35 | 2 | screening |
| Incapacitant A | drowsiness | low | 50 | 1 | capture |
| Incapacitant B | disorientation | low | 55 | 2 | capture |
| Vomit agent | nausea | low | 40 | 1 | crowd |
| Tear agent | tearing | low | 35 | 1 | crowd |
| Caustic mist | burns | med | 85 | 4 | industrial |
| Solvent vapor | abstract CNS effect | low | 60 | 3 | industrial |
| Acid aerosol | tissue damage | med | 95 | 4 | industrial |
| Ammonia cloud | airway | low | 65 | 3 | industrial |
| Chlorine-analogue | airway | low | 75 | 4 | industrial |
| Unknown agent | unknown | unknown | unknown | unknown | the vault |

Agent behavior is abstract and the class names are deliberately generic. The
expansion never specifies a real synthesis route or a real-world effect.

---

## 22. APPENDIX E — PATHOGEN CLASS TABLE

| Class | Transmission | Incubation | Outcome | Mutation sibling |
|---|---|---|---|---|
| Airborne fever A | air | 3–7 | moderate | sister B |
| Airborne fever B | air | 4–9 | severe | sister C |
| Waterborne flux | water | 1–3 | moderate | sister |
| Blood fever | blood | 5–12 | severe | sister |
| Spore lung | spore | 7–21 | severe | sister |
| Skin rot | contact | 2–6 | moderate | sister |
| Nerve fever | vector | 4–10 | severe | sister |
| Gut worm | food | 10–20 | mild | sister |
| Coughing sickness | air | 2–5 | mild | sister |
| Quiet fever | blood | 8–16 | severe | sister |
| Marsh flux | water | 2–6 | moderate | sister |
| Ash lung | spore | 14–30 | severe | sister |
| Rust pox | contact | 5–10 | moderate | sister |
| Cold sweat | air | 1–4 | mild | sister |
| Long cough | air | 10–25 | moderate | sister |
| Blood flux | water | 3–7 | severe | sister |
| Bone ache | vector | 6–14 | moderate | sister |
| Vault strain A | unknown | unknown | severe | unknown |
| Vault strain B | unknown | unknown | severe | unknown |
| Vault strain C | unknown | unknown | unknown | vault only |

All strains are fictional and merge into the canonical disease engine. No real
pathogen is named or described, and the vault strains are deliberately unspecified.

---

## 23. APPENDIX F — MASK AND FILTER TABLE

| Mask | Fit class | Protection | Wear rate | Power | Note |
|---|---|---|---|---|---|
| Cloth Wrap | poor | 0.15 | high | none | last resort |
| Improvised Filter | poor | 0.30 | high | none | charcoal |
| Rubber Half-Mask | med | 0.55 | med | none | canisters |
| Full-Face Mask | good | 0.75 | med | none | eye protection |
| Powered Respirator | high | 0.90 | low | 40 W | pumps |
| Sealed Suit | very high | 0.97 | low | 60 W | rare |
| Child Mask | med | 0.50 | med | none | fit |
| Improvised Positive | med | 0.70 | med | 25 W | fan |
| Exhale-Only Valve | med | 0.45 | med | none | simple |
| Rebreathing Unit | high | 0.85 | low | 50 W | closed |
| Industrial Hood | med | 0.60 | high | none | bulky |
| Emergency Escape | poor | 0.40 | one-use | none | escape |
| Surgical Mask | very poor | 0.10 | high | none | not protection |
| Vault Suit | extreme | 0.99 | low | 80 W | custody |
| Filter Media Coarse | — | — | — | — | reusable |
| Filter Media Fine | — | — | — | — | scarce |
| Filter Media Oxidizer | — | — | — | — | specific |
| Filter Media Charcoal | — | — | — | — | broad |
| Filter Media Mixed Bed | — | — | — | — | best |
| Filter Media Unknown | — | — | — | — | vault |

Protection is bounded by both fit and media. A perfect canister on a loose mask is
false confidence, and the expansion says so plainly.

---

## 24. APPENDIX G — DECONTAMINATION PROTOCOL TABLE

| Protocol | Stage | Consumes | Removes | Residue |
|---|---|---|---|---|
| Dry Brush | 1 | labor | loose dust | dust |
| Wet Wash | 2 | water | surface agent | effluent |
| Soap Wash | 3 | soap, water | oils and agent | effluent |
| Oxidizer | 4 | oxidizer | reactive agent | sludge |
| Adsorption | 5 | media | dissolved agent | media waste |
| Isolation | 6 | room | nothing | contained |
| Heat | 7 | power | volatile agent | vapor |
| Freeze | 8 | power | some agents | condensate |
| Disposal | 9 | transport | gear | burial site |
| Override | — | nothing | nothing | future harm |

Manual override exists in the live system; the expansion gives it content and a
reckoning. Skipping a stage is faster, and it goes into the file.

---

## 25. APPENDIX H — QUARANTINE ZONE TABLE

| Zone | Capacity | Separation | Supply draw | Escape risk | Morale |
|---|---|---|---|---|---|
| Ward Annex | 6 | med | low | low | −3 |
| Segregated Block | 20 | high | med | med | −6 |
| Wire Camp | 60 | very high | high | high | −10 |
| Clean Ward | 4 | extreme | low | none | −1 |
| Home Isolation | 1 | low | low | low | −2 |
| Work Pod | 12 | med | med | low | −4 |
| Arrival Line | 30 | high | med | high | −5 |
| Hard Lock | any | extreme | high | none | −14 |

Quarantine reduces spread through the coordinator; it never replaces infection
logic. Supply draw and morale cost are real and appear in the shelter's daily math.

---

## 26. APPENDIX I — CONTAMINATED SITE TABLE

| Site | Class | Soil | Water | Structure | Decay | Remediation |
|---|---|---|---|---|---|---|
| Gas Works | industrial | high | high | high | very slow | seal and lime |
| Bitter Marsh | persistent | high | high | none | none | lime, years |
| The Spill | chemical | high | med | low | slow | dig and burn |
| Burn Pits | ash | med | low | none | slow | cap |
| Sealed Lab | bio | med | med | high | none | seal |
| Wash Line | mixed | med | med | low | fast | wash |
| Spring Head | water | low | high | none | med | filter |
| Old Depot | industrial | med | low | med | slow | remove |
| Metro Sump | water | low | high | low | slow | drain |
| Filter Works | dust | high | low | med | med | vacuum |
| Siren Tower | none | none | none | none | — | — |
| Quarry Pool | water | low | high | none | slow | lime |

Contaminated sites persist across save/load and are remediated with real materials
and labor. The Deep Root expansion's soil chemistry consumes these loads.

---

## 27. APPENDIX J — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_air_siren` | 3 | Decode or fail to decode the warning |
| `quest_air_model` | 4 | Model drift with shelter weather data |
| `quest_air_the_line` | 4 | Ration masks; choose who is turned away |
| `quest_air_the_works` | 5 | Reach the source; assess salvage and danger |
| `quest_air_the_seal` | 4 | Seal the works or keep pulling from it |
| `quest_air_first_cough` | 4 | First exposure; triage and trace |
| `quest_air_airlock` | 5 | Decon overload; effluent spike |
| `quest_air_line_holds` | 4 | Declare quarantine; split a family |
| `quest_air_the_escape` | 4 | Break the line; catch or lose the escapee |
| `quest_air_sealed_lab` | 5 | Open the lab; read the custody ledger |
| `quest_air_the_debate` | 5 | Keep, destroy, or trade the vault |
| `quest_air_deterrence` | 4 | Answer a rival demand; bluff or comply |
| `quest_air_second_plume` | 5 | Warn all, warn allies, or stay silent |
| `quest_air_clean_ground` | 5 | Remediate or fence the ground |
| `quest_air_what_breathes` | 3 | Final disposition; epilogue |

---

## 28. APPENDIX K — NPC DOSSIERS (BRIEF)

**Dr. Asa Vell** — physician. Runs quarantine because someone must, and she is tired
in a way sleep will not fix. She is not a tyrant; she is a person making triage
decisions at the edge of her competence, and she knows it. Her arc is about asking
for help before she breaks.

**Warden Koval** — quarantine warden. Enforces the line with as much kindness as
the rule allows. Believes order is a form of care. The escape quest forces him to
choose between the rule and the person.

**Ren Basil** — chemist. Sees hazards as problems to solve and occasionally forgets
that solutions have owners. The one who wants to open the vault. Not evil;
curiosity without brakes.

**Lowe** — quartermaster. Counts the living and the masks, and the numbers do not
match. Would take the risk at the Gas Works because the shelter needs the salvage.
The expansion's pressure.

**Mira Shen** — mask sergeant. Owns the supply chain of safety: fit, media,
inspection, and repair. The most practical person in the expansion and its quiet
hero.

**Tam Wick** — quarantine escapee. Broke the line for a reason the player may or
may not accept. Represents the human cost of quarantine policy.

**Doss** — lab archivist. Keeps the custody ledger and will not let it be rewritten.
Believes the vault is a trust, not a stockpile.

**Junip** — clean-ward child. The person the vault decision is really about: a child
who has only ever breathed filtered air and wants to run outside.

---

## 29. APPENDIX L — LOCATION DETAIL

- **The Gas Works** — pipes, valves, and a plume that does not stop. Salvage is
  everywhere and every piece of it is poisoned.
- **The Sealed Lab** — a door with a wheel, dry air, and a cabinet with a ledger.
- **The Bitter Marsh** — water with a sheen, reeds that will not grow straight, and
  birds that stopped nesting.
- **The Filter Works** — press molds, media dust, and a machine that once made
  thousands of canisters.
- **The Wire Camp** — a quarantine settlement that never got the release order.
- **The Spill** — cracked drums and soil that hurts to touch without gloves.
- **The Burn Pits** — black ground that still smolders near the edges.
- **The Spring Head** — clean water rising from rock; the region's contested prize.
- **The Siren Tower** — a horn, a hill, and a receiver that hears things nobody
  sends.
- **The Wash Line** — a decon gate built from shower stalls and tarps.

---

## 30. APPENDIX M — PLUME MODEL DETAIL

A plume is defined by six fields and four couplings:

| Field | Meaning |
|---|---|
| `source_location_id` | where it emits |
| `base_density_permille` | starting concentration |
| `release_rate` | daily emission |
| `wind_coupling` | how strongly wind moves it |
| `rain_washout` | how much rain scrubs it |
| `deposition_rate` | how much settles on ground |
| `decay_per_day` | natural loss |

| Coupling | Reads | Effect |
|---|---|---|
| Weather | wind, rain | drift and washout |
| Shelter | filtration, sealing | interior load |
| Sites | deposition | contamination |
| Population | exposure | triage and decon |

The model is a pure function of live weather plus authored rates. It never invents
a wind direction and it never resolves harm directly; it produces exposure facts
that the medical pipeline consumes.

---

## 31. APPENDIX N — AIR QUALITY MODEL DETAIL

Each room carries an air load in four channels:

| Channel | Sources | Effects |
|---|---|---|
| Dust | ash, excavation, weather | lungs, filters, visibility |
| Spore | marsh, compost, disease | infection risk |
| Vapor | agents, solvents, fuel | toxicity |
| CO2 | crowding, combustion | fatigue, cognitive penalty |

Filtration reduces load at `efficiency * power_fraction`. If power drops, efficiency
drops; if the filter clogs, efficiency drops; if both, the room degrades. There is
never a silent pass: every degradation writes a typed event and a UI state.

---

## 32. APPENDIX O — WORKED 180-DAY HAZARD SCENARIO

**Days 1–30.** The siren starts. Ren models a plume crossing the eastern approach.
The shelter rations masks: 300 for 460. The east camp receives 120.

**Days 31–60.** The plume source is found at the Gas Works, where the Foundry has
been pulling pipe for weeks. Exposure cases appear. The decon queue backs up; the
effluent tank reaches 80% and the outfall is redirected twice.

**Days 61–90.** A family is split by quarantine. Tam Wick escapes; the warden asks
for a decision. The Bitter Marsh shows elevated soil load along the plume's path.

**Days 91–120.** The Sealed Lab opens. The custody ledger names pre-war staff and
the strains they kept. The vault debate begins. Ren wants to keep everything; Doss
wants it sealed; Lowe wants it as leverage.

**Days 121–150.** A rival demands access. Deterrence posture is chosen. The second
plume is larger; the warning decision determines regional standing and deaths.

**Days 151–180.** Remediation begins on the marsh (lime, ash, labor) or the ground is
fenced and abandoned. The vault is kept, destroyed, or traded. The epilogue records
who breathed and who did not.

This scenario is the expansion's intended shape. Every branch must be survivable and
consequential.

---

## 33. APPENDIX P — VIGNETTE (TONE SAMPLE)

> The siren is not a scream. It is a low, patient note that carries across the ash,
> and everyone in the yard stops to listen because none of them have heard it used
> before. Ren has the wind chart on the table inside and he is already drawing a
> cone, and the cone points east, and the east is where people live.
>
> Mira checks the mask racks one by one, opening each and looking inside the
> canister seat, the way you check a weapon you may have to trust. Three hundred
> masks. She counts twice, because the number does not change but she hopes it will.
>
> In the clean ward, Junip presses both palms against the glass and watches the
> plume move, and asks whether they can go outside when it is gone, and nobody
> answers, because the answer is not a promise anyone can keep yet.

This sets the register: quiet, procedural, and honest about fear. All Bitter Air
prose should match it.

---

## 34. APPENDIX Q — CONTENT REVIEW CHECKLIST

- [ ] No real chemical weapon, pathogen, toxin, or precursor is named or described.
- [ ] No real synthesis route, dosage, or effect is specified.
- [ ] Chemical and biological use is never rewarded or tactically advantageous.
- [ ] Agent and strain names are invented and abstract.
- [ ] Infection routes only through `DiseaseSystem` and the strain layer.
- [ ] Decon routes through the live decon system.
- [ ] Quarantine does not duplicate infection logic.
- [ ] Effluent and toxic sites route to the water and soil authorities.
- [ ] Mask protection is bounded by fit and wear.
- [ ] Plumes are driven only by live weather.
- [ ] Irreversible actions require confirmation and state consequences.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 35. APPENDIX R — GLOSSARY

- **Plume** — a moving hazard volume driven by live weather.
- **Air load** — a room's dust, spore, vapor, and CO2 levels.
- **Fit** — how well a mask seals; it governs real protection.
- **Canister** — a consumable filter unit with media.
- **Quarantine zone** — a segregated area with supply and morale costs.
- **Toxic legacy** — persistent soil, water, or structure contamination.
- **Custody ledger** — the vault's record of agents, strains, and signatures.
- **Deterrence posture** — the political use of holding a vault.
- **Wash line** — a crossing with a decontamination gate.
- **Siren network** — the regional warning system the expansion builds on.

---

## 36. APPENDIX S — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ChemWarfareSystem` | agents | zones | disease state |
| `DecontaminationSystem` | queue | decon state | water inventory |
| `PathogenStrainSystem` | strains | strain state | human health |
| `DiseaseSystem` | infection | infection | decon state |
| `DiseaseQuarantineCoordinator` | zones | quarantine | needs |
| `WeatherSystem` | wind, rain | — | — |
| `PowerGridSystem` | load | — | — |
| `WaterTreatmentSystem` | effluent | water state | — |
| `Farming` | soil load | crop state | — |
| `NeedsSystem` | scarcity | morale | — |
| `MoraleContagionSystem` | fear | contagion | — |
| `GuiltInsomniaSystem` | choices | guilt | — |
| `FactionStanceEngine` | posture | standing | — |
| `MedicalPipelineCoordinator` | exposure | treatment | — |
| `MemorialSystem` | deaths | memorials | — |

---

## 38. APPENDIX T — DATA SCHEMA DETAIL (NEW CATALOGS)

**`hazard_plumes.json`** — `plume_id`, `display_name`, `agent_id`,
`source_location_id`, `base_density_permille`, `release_rate`, `wind_coupling`,
`rain_washout`, `deposition_rate`, `decay_per_day`, `warning_profile`, `tags`.

**`air_quality_profiles.json`** — `profile_id`, `display_name`, `room_tags[]`,
`baseline_load[]` (dust, spore, vapor, co2), `filtration_response`,
`safe_threshold`, `danger_threshold`, `failure_effect_id`, `tags`.

**`masks_filters.json`** — `mask_id`, `display_name`, `fit_class`, `protection`,
`wear_rate`, `power_watts`, `canister_slots`, `child_fit`, `media_tags[]`, `tags`.

**`quarantine_zones.json`** — `zone_id`, `display_name`, `capacity`,
`separation_class`, `supply_draw`, `morale_cost`, `escape_risk`, `release_criteria`,
`linked_rooms[]`, `tags`.

**`contaminated_sites.json`** — `site_id`, `display_name`, `location_id`,
`contaminant_class`, `soil_load`, `water_load`, `structure_load`, `decay_rate`,
`remediation_recipe[]`, `crop_effect`, `well_effect`, `tags`.

**`strain_vault.json`** — `vault_entry_id`, `display_name`, `source_id`,
`viability`, `custody_signature`, `research_value`, `deterrence_weight`,
`disposal_options[]`, `tags`.

**`deterrence_doctrines.json`** — `doctrine_id`, `display_name`, `posture`,
`demand_response`, `bluff_success_bp`, `taboo_effect`, `standing_effect`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing or
duplicate IDs, invalid location/room/item references, or out-of-range numbers.

---

## 39. APPENDIX U — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_air_filter_change` | 3 | Change a filter mid-surge without killing the room |
| `quest_air_pressure_test` | 3 | Trace a clean-ward leak with smoke and tape |
| `quest_air_dust_watch` | 2 | Log dust load across a storm |
| `quest_air_spore_season` | 4 | A seasonal spore surge and its crop |
| `quest_air_co2_buildup` | 3 | Uncrowd a sealed room before the air turns |
| `quest_air_mask_fit` | 4 | Fit masks for children and odd faces |
| `quest_air_canister_line` | 4 | Assemble canisters by hand; test each |
| `quest_air_pump_unit` | 3 | Power the respirators through a blackout |
| `quest_air_mask_theft` | 4 | Recover stolen masks without breaking the shelter |
| `quest_air_child_mask` | 3 | Build a child mask that actually seals |
| `quest_air_wind_read` | 3 | Read wind and commit to a plume path |
| `quest_air_rain_wash` | 4 | Rain scrubs the plume and concentrates it downhill |
| `quest_air_reroute` | 3 | Move an expedition around a plume |
| `quest_air_false_alarm` | 4 | A false siren and the trust it costs |
| `quest_air_decon_queue` | 4 | Triage a decon line under pressure |
| `quest_air_effluent_spike` | 4 | The tank is full; find somewhere for the water |
| `quest_air_sludge_disposal` | 4 | Move sludge without making a new site |
| `quest_air_manual_override` | 3 | Skip a wash step; live with the record |
| `quest_air_zone_setup` | 5 | Build and staff the quarantine block |
| `quest_air_supply_line` | 4 | Feed people behind the line without breaking the line |
| `quest_air_family_split` | 4 | A child on the wrong side; choose |
| `quest_air_escape_watch` | 3 | Watch for a break; decide the response |
| `quest_air_release_day` | 4 | Release quarantine; handle the aftermath |
| `quest_air_soil_test` | 3 | Sample and classify contaminated ground |
| `quest_air_lime_ground` | 5 | Begin remediation; years of work |
| `quest_air_well_test` | 3 | Test a well before the shelter drinks |
| `quest_air_custody_ledger` | 3 | Read the ledger; understand the trust |
| `quest_air_vault_choice` | 5 | Keep, destroy, or trade; live with it |

---

## 40. APPENDIX V — SHELTER AIR NETWORK

| Room | Load profile | Filtration | Failure |
|---|---|---|---|
| Air Filtration | source | primary | `fx_filtration_off` |
| Clinic | low | shared | clinic-off |
| Clean Ward | positive | dedicated | ward contaminated |
| Quarantine Block | medium | negative | cross-flow |
| Decon Bay | high | wash | effluent backup |
| Greenhouse | medium | shared | spore ingress |
| Foundry | high dust | local | dust ingress |
| Living Quarters | medium CO2 | shared | fatigue |
| Deep Barracks | high CO2 | ducted | deep air loss |
| Mask Store | low | sealed | media damage |

Air is a building-wide system, not a per-room switch. Doors, ducts, pressure, and
filter condition matter, and the player manages them together.

---

## 41. APPENDIX W — REGIONAL WARNING NETWORK

| Tier | Source | Lead time | Trust |
|---|---|---|---|
| Local alarm | shelter sensor | minutes | high |
| Siren tower | fixed installation | hours | medium |
| Radio warning | broadcast | hours | medium |
| Rider warning | courier | days | low |
| Rumor | travelers | inconsistent | very low |

Warnings are a commons problem: the shelter can warn everyone (and lose advantage),
warn allies (and create obligation), or stay silent (and lose trust when the truth
comes out). The network routes through the live radio authority for transmission.

---

## 42. APPENDIX X — EFFLUENT CHAIN

| Stage | Owner | Input | Output |
|---|---|---|---|
| Wash water | DecontaminationSystem | water, soap | effluent |
| Tank | DecontaminationSystem | effluent | stored effluent |
| Filter | DecontaminationSystem | effluent | filtered water |
| Sludge | DecontaminationSystem | solids | sludge |
| Discharge | WaterTreatmentSystem | filtered effluent | downstream water |
| Disposal | ToxicLegacySystem | sludge | burial site |
| Reckoning | World/faction | discharge | well and crop effects |

The chain makes decontamination honest: every wash produces something that has to
go somewhere, and somewhere is always downstream of someone.

---

## 43. APPENDIX Y — TABOO AND DIPLOMACY

| Posture | Rival reaction | Trade effect | Morale effect |
|---|---|---|---|
| Denied | pressure, probes | neutral | + cohesion |
| Admitted | fear, respect | cautious | − trust |
| Bluffed | testing, espionage | volatile | − guilt |
| Traded | dependence, resentment | access | − cohesion |
| Destroyed | relief, contempt | neutral | + cohesion |

The vault is never a weapon in the mechanical sense. It changes how others calculate
risk, and that calculation is the whole game. Expansion 20 (espionage) is the natural
partner for a stolen or copied vault entry.

---

## 44. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Plume events per campaign | pacing | HazardPlumeSystem |
| Exposure cases | hazard reach | MedicalPipeline |
| Mask shortfall | tension | RespiratorSystem |
| Quarantine days | social cost | QuarantineZoneSystem |
| Effluent discharged | environmental cost | DecontaminationSystem |
| Sites remediated | long-term play | ToxicLegacySystem |
| Vault posture | diplomatic weight | DeterrenceSystem |
| Siren false alarms | trust | warning network |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score. It exists so the team can tell whether the expansion is actually scary or
merely loud.

---

## 45. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Should plumes be visible from the start, or discovered by sensors?
2. Should filters be repairable or strictly consumable?
3. Should the vault have a research payoff that changes cure speed?
4. Should quarantine zones be built by the player or only assigned?
5. Should contaminated sites ever fully heal without intervention?
6. Should the siren network include rival-owned towers?
7. Should air quality affect sleep, work, or both?
8. Should decon override be a one-time crime or a persistent record?

None of these may be decided unilaterally; each changes the expansion's tone.

---

## 47. APPENDIX AB — LORE: THE QUIET WAR

The pre-war world ran two wars in parallel. The loud one used fire, steel, and
radiation. The quiet one used chemistry and biology, was never declared, and is only
now being discovered. The expansion's fiction:

- **The Quiet War** was fought through proxies, laboratories, and industrial
  accidents that were not accidents. No nation is named.
- **The Gas Works** was a legitimate industrial facility that was repurposed late in
  the war to produce agents it was never designed for. Its plume is a physical
  record of that decision.
- **The Sealed Lab** was a research station with a cooperative charter: several
  parties shared custody of dangerous strains to prevent any one of them from using
them. The cooperative failed. The ledger survives.
- **The Wire Camp** was a quarantine settlement built after the Exchange by survivors
  who knew about the quiet war and tried to screen arrivals. Its fence is still up.
- **The Bitter Marsh** is where the region's runoff collected. It is the conflict's
  unintended monument.

No real nation, agency, program, or event is referenced. The Quiet War is entirely
fictional and exists to explain why the wasteland has pre-war hazards that radiation
alone cannot explain.

---

## 48. APPENDIX AC — SHELTER SEALING AND PRESSURE

When a plume passes, the shelter has three options:

| Option | Method | Effect | Cost |
|---|---|---|---|
| Seal | close vents, overpressure | near-zero interior load | power, heat, CO2 rising |
| Filter | run filtration at full | low interior load | power, filter wear |
| Vent | leave open | high interior load | people, health |

Sealing is not free: a sealed shelter heats up, CO2 climbs, and people cannot leave.
The expansion makes sealing a temporary emergency measure rather than a permanent
solution. Pressure management is a room-level operation: the clean ward maintains
positive pressure; the quarantine block maintains negative pressure; the decon bay
balances both. Pressure mismatches create cross-flow events, which are exactly how
quarantine failures happen.

---

## 49. APPENDIX AD — WORKPLACE HAZARD TABLE

Hazards follow workers to their jobs. The expansion ties exposure to the live duty
roster:

| Workplace | Hazard | Protection | Monitoring |
|---|---|---|---|
| Foundry | metal fume, dust | respirator | air sample |
| Greenhouse | spores, humidity | mask, ventilation | spore trap |
| Decon bay | splash, vapor | suit, eye cover | wash log |
| Mine | dust, gas | respirator, canary | gas meter |
| Kitchen | smoke, CO | hood, ventilation | CO monitor |
| Clinic | bioaerosol | mask, isolation | swab |
| Salvage | unknown agents | mask, gloves | assay |
| Farm | dust, pesticide | mask, gloves | soil test |
| Latrine | gas, biofilm | ventilation | air check |
| Filter works | media dust | respirator | dust count |

Each workplace has an authored protection standard; failing it produces exposure
events. This gives the mask logistics a clear purpose: the shelter decides which jobs
are protected first when supplies are short.

---

## 50. APPENDIX AE — CAMPAIGN ARC TIMELINE

| Phase | Days | Theme | Decision |
|---|---|---|---|
| First warning | 1–30 | uncertainty | mask rationing |
| First exposure | 31–60 | triage | decon discipline |
| Quarantine | 61–90 | social cost | line policy |
| The lab | 91–120 | morality | vault posture |
| Deterrence | 121–150 | politics | demand response |
| Second plume | 151–180 | commons | warning choice |
| Reckoning | 181–240 | permanence | ground and vault |

The arc is authored over roughly 240 days, but scales down for shorter campaigns.
Each phase changes the shelter's relationship to air, trust, and its own history.

---

## 51. APPENDIX AF — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Filter failure | interior load rises | replace media, seal |
| Mask shortage | unprotected work | ration, improvise, trade |
| Quarantine breach | spread event | re-zone, trace contacts |
| Effluent overflow | downstream contamination | stop discharge, filter |
| Plume surprise | mass exposure | triage, decon, memorial |
| Siren trust loss | ignored warnings | demonstrate reliability |
| Vault theft | deterrence loss | recover or deny |
| Site recontamination | crop/well loss | re-zone, remediate |
| Panic | morale collapse | leadership, ritual, rest |

No failure is a game over. Every failure has a recovery path, and every recovery
costs something proportional to the failure.

---

## 52. APPENDIX AG — CONTENT CROSS-REFERENCE

The Bitter Air reuses existing anchors rather than inventing parallel content:

| Existing anchor | Reused as |
|---|---|
| `chemical_weapons.json` | agent definitions |
| `pathogens.json` | strain definitions |
| `contagion_events.json` | outbreak events |
| `toxic_chemical_catalog.json` | industrial toxics |
| `decontamination_protocol_catalog.json` | decon methods |
| `chemical_syntheses.json` | decon and media chemistry |
| `chemical_dependency_items.json` | dependency supplies |
| `disease_catalog.json` | canonical disease engine |
| `autopsy_procedures.json` | pathology of exposure |
| `disease_save` | infection state |
| `decontamination` save | decon state |
| `chem_warfare` save | agent state |
| `pathogen_strain` save | strain layer |
| `power_grid.json` (air filtration room) | filtration load |
| `water_quality` logs | effluent testing |
| `weather_effects.json` | plume weather coupling |

This reuse is what keeps the expansion from creating a parallel disease, hazard,
water, or weather model.

---

## 53. APPENDIX AH — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the fictional-only audit.
- [ ] Phase 7 soak shows plume seasons, quarantine cycles, and recovery paths.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel disease, hazard, water, weather, or save system exists.

---

## 54. APPENDIX AI — CLOSING VIGNETTE

> The shelter runs on its own air for eleven days. The vents are sealed, the
> filtration plant is loud, and the CO2 board in the corridor climbs one line a day.
> People sleep badly and argue about nothing and Mira walks the racks checking masks
> that were checked that morning.
>
> On the twelfth day the plume is gone, and the first person out is Junip, who walks
> to the middle of the yard and stands there with her mask off, breathing, while the
> adults pretend not to watch her closely.
>
> Somewhere east, the marsh is still poisonous. Somewhere below, the vault is still
> cold. Somewhere in the shelter, the ledger still has the old names in it, and
> nobody has decided yet what that means.

---

## 55. CLOSING STATEMENT

ASHFALL already models toxic agents, decontamination, strains, quarantine, and
dependency with real authority splits and deterministic discipline. What it lacks is
the weather of poison: air that moves, ground that stays dirty, masks that run out,
lines that split families, and a vault that asks what kind of people the shelter is.
The Bitter Air adds that world without adding a second disease or hazard system, and
without ever turning chemical or biological weapons into a reward. It adds a siren,
a filter, a ledger, and a choice about who gets to breathe.

> Wave 2 note: this plan is one of five Wave 2 expansion bibles (17–21). Each is
> self-contained; none requires another to ship. The shared Wave 2 index lives at
> `docs/expansions/wave2/WAVE2_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible.