# Muster Faction Runtime Contract (Plan 25 · 25A.1)

> Verified 2026-09-01 against the working tree. All paths relative to repo root.
> This document is the capability matrix Plan 25 extends. It records what exists today; authored Plan 25 behavior is layered on top without altering these contracts.

## 1. The four Muster faction systems

All live in `Assets/Ashfall.Core/Muster/`, namespace `Ashfall.Core.Muster`. All are plain C# state machines: **no JSON loading, no action catalogs, no seeded RNG, no day clocks of their own.** Host wiring: `src/Host/MusterHostSession.cs` (ctor L38–76, `Create` L78–100, day tick `Escalate` L115–125); Godot: `src/Main.Muster.cs`; persistence: `src/Host/MusterSaveStore.cs` (`MusterHostSave`, section `muster`, file `muster_save.json`, self-checksummed envelope, also captured into the campaign envelope as section `muster`).

| | ScavengerGuildSystem | HydroBaronsSystem | IronRaidersSystem | CoalitionCampSystem |
|---|---|---|---|---|
| State | `claimedSiteIds` List, `blacklistedShelterIds` HashSet (Ordinal), `trust` float (floor 0, **no ceiling**) | `queuePosition` 0..100, `trust` float (**no upper clamp**), `rateCardRevised`, `plantSeized`, `adminReform`, `approach` | `aggressionLevel` 0..1, `raidsThisSeason`, `shelterVisibility` (floor 0.1) — **no trust** | `formed`, `formedDay`, `membersRallied` (base 9), `chosenStrategy` A–D, `garrisonLockoutRisk` 0..100, `holdingGroundId`, `vaskWithCamp` |
| Input deps | none (optional state ctor) | none | none | none |
| Existing actions | `ClaimSite` (+1 trust), `RecordOverStrip` (−4, permanent blacklist), `ApprenticeOverstripCheck` | `ResolveApproach(QuestApproach)` one-shot (A/B/D trust +, C seizes plant, queue 0), `AdvanceQueue` (blocked after lock) | `ExecuteRaid()`, `EvaluateRaidChance()` (pure: `aggression*0.6 + visibility*0.25`; **host never rolls; `SetAggressionLevel` has no production caller**) | `Form(day)` gated `day >= 260`, `RallyDeserter()`, `SetStrategy(QuestApproach)` one-shot (lockout −5/+15/−10/zero) |
| Standing read for Plan 25 | `trust` | `trust` (+ queue/approach) | aggression/visibility band | formed/members/lockout |
| Day gating | none | none | none | Form ≥ Day 260 only |
| War/treaty gating | none | none | none | none |
| Cooldowns | one-shot blacklist only | approach lock (per-save) | none | strategy lock (per-save) |
| Events | `OnStateChanged`, `OnClaimed`, `OnBlacklisted` | `OnStateChanged`, `OnApproachResolved` | `OnStateChanged`, `OnRaidExecuted`, `OnFortified` | `OnStateChanged`, `OnCampFormed`, `OnStrategySet`, `OnLockoutShifted` |
| Save | `MusterHostSave.ScavengerGuildState`; capture sorts lists Ordinal (deterministic) | `MusterHostSave.HydroBaronsState`; plain copy | `MusterHostSave.IronRaidersState`; plain copy | `MusterHostSave.Camp`; plain copy |
| Determinism | sorted save; no RNG | field copy; no RNG | pure formula; no RNG | field copy; no RNG |

### Plan 25 standing bands (used by FactionActionBoard variants)

| Band | Guild/Hydro `trust` | Raiders (aggression a, visibility v) | Camp |
|---|---|---|---|
| hostile | ≤ 0 | a ≥ 0.75 | lockout ≥ 60 or strategy D |
| poor | 0 < t < 4 | 0.5 ≤ a < 0.75 | lockout 30–59 |
| neutral | 4 ≤ t < 9 | 0.25 ≤ a < 0.5 | formed, lockout < 30 |
| good | 9 ≤ t < 15 | a < 0.25, v < 0.3 | members ≥ 12 |
| allied | ≥ 15 | a < 0.1 and a shelter fortification event | members ≥ 15 |

Bands read each system's **own** scalar; no new standing store. Additive `AdjustTrust(float)` seam added to guild/hydro (clamped, event-raising, save-safe).

## 2. Faction JSON catalogs — actual shapes

### holdfast_factions.json — ALIVE (Holdfast terminal UI only)
`{schema_version: 1, actions: [...]}` — `actions` is a **misnomer**: entries are dossiers `{id, display_name, alignment, home_region, is_active, trust, wants[], offers[], signature_quote, access_rule, badge_asset_id}`. 3 entries: `faction_the_office`, `faction_the_cutters`, `faction_the_fleet` (`is_active: false`). Loader `HoldfastCatalogLoader` (`Assets/Ashfall.Core/HoldfastCatalog.cs:138,167,276`); catalog `HoldfastFactionsCatalog` (insertion-ordered List + Ordinal dict). NOT consumed by any Muster system.

### standing_record_factions.json — DEAD
Same dossier schema, 1 entry `faction_the_overlay`. **No loader.** Referenced only by `LocationLayoutSystemTests.cs:246-254` (raw content assertion) and `FactionIconCatalog.cs:72` (icon comment). Plan 25 does not author here.

### foundry_faction.json — ALIVE (Silent Foundry)
Flat object `{schema_version, faction_id: "faction_silent_foundry", display_name, short_name, identity, icon_path, internal_divisions[6], relationships[], tags[]}`. Loader `SilentFoundryCatalog.LoadFaction` (`Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs:124,185`). Divisions are display-only (`src/UI/FactionsPanel.cs:226`).

### currents.json — the Muster factions' authored presence
17 faction dossiers incl. `faction_scavenger_guild`, `faction_iron_raiders`, `faction_hydro_barons`, `faction_deserter_coalition`. Loaded by `CurrentsCatalogLoader` at `src/Host/MusterHostSession.cs:87`. Presentation-only. Plan 25 faction actions reference these ids.

### faction_lore.json — codex lore (23 entries, ids WITHOUT `faction_` prefix, e.g. `iron_garrison`)
Loaded for canonical-id validation (`CombatCatalog.cs:610`, `WarlordDoctrineCatalog.cs:328`) and codex UI (`src/UI/FactionsNarrativePanel.cs:75`, `src/UI/FactionMatrixPanel.cs:78`). The four Muster factions are **not** in it; Plan 25 culture content goes to a new `muster_faction_culture.json` (25E) rather than mixing id conventions.

## 3. Capability matrix — what Plan 25 adds per faction

| Faction | Actions (authored in `muster_faction_actions.json`) | Standing-sensitive | Grievance producer | Culture codex |
|---|---|---|---|---|
| Scavenger Guild | A1 salvage claim, A2 arbitration dispute | trust bands | `flag_grievance_scavenger_*` | C1, C2 |
| Hydro Barons | A3 purification toll, A4 emergency water appeal | trust bands | `flag_grievance_hydro_*` | C3, C4 |
| Iron Raiders | A5 parley, A6 passage demand | aggression/visibility band | `flag_grievance_raider_*` | C5 |
| Coalition Camp | A7 mediation request, A8 shared-supply appeal | camp state band | `flag_grievance_coalition_*` | C6 |

Runtime owner: `FactionActionBoard` (see `docs/plans/PLAN_25_FACTION_ECOLOGY_INTEGRATION_PLAN.md` §S1). No guild currency; item effects use existing `item_*` ids; all produced flags have a named consumer.

## 4. Tests that pin these contracts today

- `Ashfall.Core.Tests/MusterCurrentSystemsTests.cs` — guild ×4 (L162–190), raiders ×3, hydro ×6, ColdCount/Provisioned/LongWalk.
- `Ashfall.Core.Tests/CoalitionCampSystemTests.cs` — 11 tests (day gate, strategies, snapshot isolation, checksum-stable roundtrip, clamps).
- `Ashfall.Core.Tests/MusterContentCatalogTests.cs` — currents/witness/epilogue catalog loads; **pins witness count 3 (L52–55) — updated by Seam S2**.
- `Ashfall.Core.Tests/MusterSystemTests.cs` — trigger/approach/ending keys.
