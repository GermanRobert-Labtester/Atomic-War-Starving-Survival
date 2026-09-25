# ASHFALL Plans 50–53 Master Authority Map
## Overland Vehicle Garage · Faction Espionage · Survivor Mental Health · Subterranean Acoustic Direction

**Status:** Implementation authority map
**Scope:** Plans 50, 51, 52, 53
**Target:** `Ashfall.Core` simulation, data catalogs, persistence, host projection, Godot audio & presentation

---

## 1. Architectural Invariants Matrix

| Subsystem | Authority Invariant | Data Catalog (Authority) | Core Simulation System | Save Store & Section | Presentation Node |
|---|---|---|---|---|---|
| **Plan 50: Vehicle Garage** | Invariant 1 (Zero Engine References) & Invariant 6 (JSON Authority) | `vehicle_modifications.json` | `VehicleGarageSystem` (decorates & extends `ExpeditionVehicleSystem`) | `VehicleGarageSaveStore` (`vehicle_garage`) | `GarageDetailPanel` |
| **Plan 51: Faction Espionage** | Invariant 4 (Deterministic PRNG) & Invariant 3 (Checksummed Saves) | `faction_intelligence.json` | `ShelterEspionageSystem` | `ShelterEspionageSaveStore` (`faction_espionage`) | `FactionDetailPanel` & `DailyBriefingModal` |
| **Plan 52: Mental Health** | Invariant 1 (Zero Engine References) & Invariant 4 (Seeded PRNG) | `psychological_trauma.json` | `SurvivorMentalHealthSystem` | `SurvivorMentalHealthSaveStore` (`survivor_mental_health`) | `SurvivorDetailPanel` & `AfflictionsPanel` |
| **Plan 53: Acoustic Director** | Invariant 5 (Thin Presentation Projection) & Invariant 1 (Headless Safe) | `shelter_audio_cues.json` | `ShelterAcousticDirector` | Reconstructed from live state | `AudioManager` & `ShelterAudioController` |

---

## 2. Cross-System Interaction Matrix

```text
[Vehicle Travel / Expeditions]
        │
        ├── Distance & Wear ──────────────► [VehicleGarageSystem]
        │                                         │
        ├── Breakdown & Stranded ◄────────────────┘
        │          │
        │          └── Recovery Mission ──► [ExpeditionSystem]
        │
[Faction Hostility / Diplomacy]
        │
        └── Hostility Threshold ──────────► [ShelterEspionageSystem]
                                                  │
                                                  ├── Leaks & Sabotage (Abstract)
                                                  ├── Sleeper Infiltration & Interrogation
                                                  └── Dead Drops ────────► [ExpeditionSystem]
                                                                                │
[Trauma & Crises] ◄──────── Critical Casualties / Betrayal ─────────────────────┘
        │
        ▼
[SurvivorMentalHealthSystem]
        │
        ├── Stress & Trauma Tokens (Insomnia / Nightmares)
        ├── Quiet-Room Decompression (room_reading_quiet_room)
        ├── Journal Catharsis & Resilience Unlocks ──► [JournalSystem]
        └── Non-Stigmatizing Crises (Hoarding, Refusal, Panic)
        │
        ▼
[Shelter Environmental & Survival State] (Power, Radiation, Excavation Risk, Radio, Bulkheads)
        │
        ▼
[ShelterAcousticDirector] (Core Semantic Director)
        │
        ▼
[AudioManager] (Godot Audio Stream Players, Bus EQ, Mixing & Ducking)
```

---

## 3. Determinism & Save Safety Strategy

- **RNG Substreams:** Dedicated forks from `ISeededRng`:
  - `garage_breakdown` (Plan 50)
  - `shelter_espionage` (Plan 51)
  - `survivor_mental_health` (Plan 52)
  - `shelter_acoustic` (Plan 53 - zero RNG, pure state evaluation)
- **Save Isolation:** Checksummed envelopes via `SaveStoreHub` and `SchemaVersionedEnvelope<T>`.
- **Legacy Compatibility:** Absent sections in legacy saves default to safe unmodded vehicles, zero sleeper agents, baseline calm mental states, and fresh acoustic reconstruction.

---

# EXPANSION 2026-09-25 — Plans 50–53: Full Integration Framework & Code Architecture

**Expansion status:** Documentation-only expansion of this authority map. The original
map text above is preserved byte-for-byte; everything below the separator is the
2026-09-25 re-verification and integration framework.
**Evidence date:** 2026-09-25 (static inspection of working tree; no builds, no test
runs, no Godot sessions were executed for this document).
**Scope of edit:** `docs/PLANS_50_53_AUTHORITY_MAP.md` only. No code, data, or save
surfaces were touched. Unrelated dirty worktree changes were left alone.
**Honesty rules used throughout:** every claim below is labeled with one of three
statuses — `VERIFIED (code)` (read directly from the working tree, with file path),
`VERIFIED (data)` (read directly from `Assets/StreamingAssets/Data/`), or
`UNVERIFIED (map text)` (asserted by the original 2026-09-06 map but not confirmed
against the current tree, or contradicted by it). Where the tree contradicts the
map, the contradiction is stated explicitly rather than smoothed over.

---

## Part I — Preamble: What Landed, In One Table

The original map is an *implementation authority map* dated 2026-09-06: it declares
which authority owns which concern for four subsystems. This expansion answers the
question the map leaves open: **did each declared authority actually land, and does
the map's description still match the tree?**

Short answer: all four Core systems, all four JSON catalogs, and three of the four
save stores exist today. Two of the four presentation bindings named in the map are
name-drifted or bound to different authorities than the map implies, one RNG fork
name in the map does not exist in code, and the "zero-RNG acoustic director" claim
is not literally true. Details per row:

| Plan | Core system | Data catalog | Save section | RNG fork (map) | RNG fork (actual) | Presentation (map) | Presentation (actual) | Landed status |
|---|---|---|---|---|---|---|---|---|
| 50 Vehicle Garage | `VehicleGarageSystem` — VERIFIED (code) `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` | `vehicle_modifications.json` — VERIFIED (data) | `vehicle_garage` — VERIFIED (code) `SaveSectionRegistry.cs:271` | `garage_breakdown` — UNVERIFIED (map text); **string absent from the entire repo** | `vehicle_garage` (`src/Main.Plans50_53.cs:42`) | `GarageDetailPanel` — UNVERIFIED (map text); **no such type exists** | `VehicleGaragePanel` (`src/UI/VehicleGaragePanel.cs`, 635 lines) + `src/Main.VehicleGarage.cs` + `src/Host/HostCli.VehicleGarage.cs` | **LANDED** (Core, save, panel, CLI, tests) |
| 51 Faction Espionage | `ShelterEspionageSystem` — VERIFIED (code) `Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs` | `faction_intelligence.json` — VERIFIED (data) | `faction_espionage` — VERIFIED (code) `SaveSectionRegistry.cs:272` | `shelter_espionage` — VERIFIED (code) | `shelter_espionage` (`src/Main.Plans50_53.cs:112`) | `FactionDetailPanel` & `DailyBriefingModal` — types exist — VERIFIED (code), but **neither binds the espionage system** | `FactionDetailPanel` binds holdfast diplomacy; `DailyBriefingModal` binds `DailyBriefingReport`; **zero `src/` consumers of `ShelterEspionageSystem` outside the wire partial** | **LANDED (Core+save); presentation UNBOUND** |
| 52 Mental Health | `SurvivorMentalHealthSystem` — VERIFIED (code) `Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs` | `psychological_trauma.json` — VERIFIED (data) | `survivor_mental_health` — VERIFIED (code) `SaveSectionRegistry.cs:273` | `survivor_mental_health` — VERIFIED (code) | `survivor_mental_health` (`src/Main.Plans50_53.cs:158`) | `SurvivorDetailPanel` & `AfflictionsPanel` — types exist — VERIFIED (code), but **they bind Survivors/Medical sessions, not the mental-health system**; live consumer is `SleepNarrativeProjection` | see Part II trauma-authority finding | **LANDED (Core+save+tick); UI projection INDIRECT** |
| 53 Acoustic Director | `ShelterAcousticDirector` — VERIFIED (code) `Assets/Ashfall.Core/Audio/ShelterAcousticDirector.cs` | `shelter_audio_cues.json` — VERIFIED (data) | none (reconstructed from live state) — VERIFIED (code), map claim holds | `shelter_acoustic` (zero RNG) — **BOTH halves drifted**: fork string is `shelter_acoustics` (`src/Main.Plans50_53.cs:204`); director holds a seeded `ISeededRng` used for spontaneous one-shots | `shelter_acoustics` | `AudioManager` & `ShelterAudioController` — VERIFIED (code) `src/Audio/AudioManager.cs`, `src/Audio/ShelterAudioController.cs`; connected through `ShelterAcousticBridge` | bridge wired in `Main.Plans50_53.cs:222-226`; but JSON cue ids (`acue_*`) are **not registered in the host `AudioCueCatalog`**, and the bridge never calls `StartLoop`, so loop updates currently no-op | **LANDED (Core+bridge); playback surface PARTIAL** |

Reading the table as a whole: the Core/save/data tier of all four plans is real,
consistent, and matches the map's authority assignments almost everywhere. The gaps
concentrate in two places: (1) the *presentation tier* — the map names panels that
either do not exist under that name or bind different sessions, and (2) the
*determinism narrative* — the map's RNG fork names were written from design intent,
not from the fork strings the host actually passes. Neither gap is a correctness bug
in the shipped code; both are documentation drift that this expansion corrects.

### 1.1 Status vocabulary

Because this document mixes map text with tree evidence, every substantive claim in
Parts II–VIII carries one of:

- **VERIFIED (code)** — the named symbol, file, or behavior was read in the working
  tree on 2026-09-25. A path is cited.
- **VERIFIED (data)** — the named catalog entry, id, or field was read in
  `Assets/StreamingAssets/Data/` on 2026-09-25. A file is cited.
- **UNVERIFIED (map text)** — the claim exists only in this document's original
  2026-09-06 map layer (or in repo memory / older docs) and was *not* confirmed in
  the tree. These are quoted, then either corroborated or contradicted.
- **DRIFT** — the tree contradicts the map text. The map text is retained above for
  history; the tree wins. Corrections are stated as findings, not silently patched
  into the original layer.

### 1.2 How to read the rest of this document

- **Part II** re-audits the matrix row by row and resolves the three open questions
  the map cannot answer: the trauma-authority relationship, the numbering collision
  with `PLANS_51_54_INTEGRATION_REPORT.md`, and the true RNG fork inventory.
- **Part III** states the integration framework: the decorate-don't-duplicate
  principle, per-plan invariants, event and command flows, save capture/restore,
  and the fork strategy as actually implemented.
- **Part IV** is the code architecture: module map, component specs with API and DTO
  examples, failure modes, and four full sequence walkthroughs.
- **Part V** is the bulk: one chapter per plan, then the interaction diagram from
  the original map expanded into dependency specifications.
- **Part VI** is the cross-system matrix and emergent-consequence design.
- **Part VII** is verification and acceptance: per-plan test matrices, the gate
  ladder, and rollback strategy.
- **Part VIII** is appendices: glossary, ID and stream vocabulary, scenario
  walkthroughs, and open questions.
- **Part IX** is the reference layer: public API index, data dictionary,
  verified citation index, the consolidated map-vs-tree correction ledger
  (Appendix H), and the per-plan quick-reference for what to run.
- **Part X** is design analysis: why each system is shaped the way it is, the
  edge-case compendium, the determinism deep-dive, state-size notes, the tone
  and accessibility contract, and the method note describing how this
  expansion was produced.
- **Part XI** is the integration playbooks: one bounded, non-authorizing
  preparation sketch per open item drawn from the debt ledger (7.5), Appendix
  D, and Appendix M.

Numbering and register convention: the 2026-09-06 map layer at the top of this
file uses bare numbered sections (`1.`–`3.`); everything below the separator
uses Roman `Part N` headings with decimal `N.x` subsections, so a reference
like `2.1` always means Part II subsection 2.1, never map section 2. The
single-letter register prefixes are disjoint by design and mean only what this
list says: `I<plan>.<n>` are invariants (3.2); `E1`–`E14` are dependency-edge
specifications (54.2); `S1`–`S4` are shared-infrastructure edges (54.3);
`W-A`–`W-D` are sequence walkthroughs (4.6); `C.1`–`C.4` are the player-visible
scenario arcs (Appendix C); `P1.`–`P12.` are playbooks (Part XI); `D<n>` are
Appendix D open questions; and `Appendix A`–`O` are lettered appendices. Two
near-collisions are therefore safe by convention: "Appendix E" (the public API
index) is a different register from edge `E11`, and the test file
`PlanE1_29VehicleEspionageTests.cs` carries an unrelated historical plan-series
label, not an edge id. Plans themselves are always written out ("Plan 50"–
"Plan 53"), never as a bare `P<nn>`.

---

## Part II — Current Authority Audit (2026-09-25)

This part re-verifies the map's invariants matrix row by row against the working
tree, then resolves the three questions the original map leaves open.

### 2.1 Plan 50 — Vehicle Garage: row re-verified

| Matrix cell (map) | Finding | Evidence |
|---|---|---|
| Authority invariant: Zero engine refs + JSON authority | **HOLDS.** `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` has no `Godot`/`UnityEngine` using; it loads `VehicleGarageCatalog` / `VehicleArmorGradeCatalog` from JSON through `VehicleGarageCatalogLoader` / `VehicleArmorGradeCatalogLoader`. | VERIFIED (code) |
| Data catalog: `vehicle_modifications.json` | **PRESENT.** 4,882 bytes, `schema_version: 1`, 8 modifications across slots `cargo`, `protection` (x2), `utility` (x3), `mobility`, `engine`. A second, later catalog `vehicle_armor_grades.json` feeds `LoadArmorCatalog` (added by the CF-P6 armor-grades package — see 2.9). | VERIFIED (data) |
| Core system: `VehicleGarageSystem` decorates `ExpeditionVehicleSystem` | **HOLDS, verbatim.** The decoration seam is `VehicleGarageSystem.DecorateProfile(ExpeditionVehicleProfile?)` (line 212), whose doc comment says "The garage decorates; the expedition core stays decoupled and owns travel." `ExpeditionVehicleSystem.CreateExpeditionProfile` (in `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`, line ~283) builds the stock profile; the garage then mutates cargo, speed, fuel, and breakdown-chance fields additively. Production call site: `src/Host/ExpeditionHostSession.cs:935` (`Garage?.DecorateProfile(profile)`); CLI probes call it from `src/Host/HostCli.VehicleGarage.cs:80,111,138`. | VERIFIED (code) |
| Save store: `VehicleGarageSaveStore` (`vehicle_garage`) | **HOLDS.** `src/Host/VehicleGarageSaveStore.cs` is a 35-line facade over `SaveStoreHub.Checksummed<VehicleGarageState>("vehicle_garage_save.json", ..., allowLegacyBareState: false)`. The section id `vehicle_garage` is registered in `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:271` with methods `SaveVehicleGarage` / `SetupVehicleGarage`, domain `expeditions`. | VERIFIED (code) |
| Presentation: `GarageDetailPanel` | **DRIFT — name.** No type named `GarageDetailPanel` exists anywhere in the repo. The actual panel is `VehicleGaragePanel` (`src/UI/VehicleGaragePanel.cs`, 635 lines), a Godot `Control` implementing `IBindablePanel` with `Bind(VehicleGarageSystem, ExpeditionVehicleSystem, Inventory)`. Its own doc comment: "Presentation only: renders the Core garage read model ... and forwards player commands to `VehicleGarageSystem`." Host wiring lives in `src/Main.VehicleGarage.cs`. | VERIFIED (code) |

Additional verified facts the map does not record:

- **The garage's own `ISeededRng` is effectively inert.** `VehicleGarageSystem`
  accepts an optional `ISeededRng` (default `new SeededRng(1337)`) but no code path
  in the file draws from it: wear accumulation, immobilization at 1,000 permille,
  service costs, and recovery-mission progress are all pure deterministic math. The
  only probabilistic breakdown mechanic on the vehicle path lives in
  `ExpeditionSystem.TickHours(hours, ISeededRng rng)` — "Mid-route vehicle
  breakdown: one seeded roll per travel tick" (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:773-780`)
  — which rolls the *expedition* RNG against `vehicleBreakdownChancePerTick`, a
  field the garage's `DecorateProfile` only *modifies* (armor mitigation scales it
  down). Implication: `garage_breakdown` as a named fork is a design-time fiction;
  the honest statement is *garage state transitions are deterministic; breakdown
  rolls belong to the expedition stream*.
- **Recovery missions are garage-owned, not `ExpeditionSystem`-owned.** The map's
  interaction diagram draws `Recovery Mission ──► [ExpeditionSystem]`, but the
  authoritative mission model is `VehicleRecoveryMission` inside
  `VehicleGarageState.activeRecoveries`, advanced by `AdvanceRecoveries(deltaTicks)`
  and completed by the player command `CompleteRecoveryMission(missionId, ...)`.
  `ExpeditionSystem` is a *consumer* of the decorated profile, not the recovery
  authority. DRIFT in the diagram's arrow ownership; corrected in Part V.
- **State capture is JSON round-trip deep copy.** `CaptureState()` serializes
  `VehicleGarageState` through `SystemTextJsonSerializer` and deserializes it back —
  so callers always receive a detached copy, and `RestoreState(null)` resets to a
  fresh unmodded state (the map's "legacy defaults" row for Plan 50).

### 2.2 Plan 51 — Faction Espionage: row re-verified

| Matrix cell (map) | Finding | Evidence |
|---|---|---|
| Authority invariant: deterministic PRNG + checksummed saves | **HOLDS.** All stochastic branches draw from the injected `ISeededRng` (default `SeededRng(4242)`); persistence goes through `SaveStoreHub.Checksummed<ShelterEspionageState>`. | VERIFIED (code) |
| Data catalog: `faction_intelligence.json` | **PRESENT.** 3,884 bytes, `schema_version: 1`, 8 `operations` (`fop_*`) and 4 `dead_drop_templates` (`fdrop_*`). Full contents tabulated in Part V Chapter 51. | VERIFIED (data) |
| Core system: `ShelterEspionageSystem` | **PRESENT** at `Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs` (340 lines). Public surface: `EnrollSleeperAgent`, `IsSleeperAgent`, `GetSleeperRecord`, `AddSuspicion`, `InvestigateSuspect`, `AttemptTurnDoubleAgent`, `SetSecurityCounterIntelScore`, `TickDay`, `SpawnRandomDeadDrop`, `InterceptDeadDrop`, `CaptureState`, `RestoreState`. | VERIFIED (code) |
| Save store: `ShelterEspionageSaveStore` (`faction_espionage`) | **HOLDS.** `src/Host/ShelterEspionageSaveStore.cs` (35 lines), same `SaveStoreHub.Checksummed` pattern, section registered at `SaveSectionRegistry.cs:272` (domain `factions`), persisted file `faction_espionage_save.json`. | VERIFIED (code) |
| Presentation: `FactionDetailPanel` & `DailyBriefingModal` | **BOTH TYPES EXIST; NEITHER BINDS ESPIONAGE.** `src/UI/FactionDetailPanel.cs` (155 lines) binds `HoldfastFactionEntry`, `HoldfastTradeSession`, `MusterHostSession`, `ExpansionHostSession` — holdfast diplomacy, not `ShelterEspionageSystem`. `src/UI/DailyBriefingModal.cs` (245 lines) renders `Ashfall.Core.Campaign.DailyBriefingReport` (built by `DailyBriefingReportBuilder`, which has an "Intelligence & Recon" section composed from `BriefingFact` inputs — no direct espionage-system input found). A repo-wide search for consumers of `ShelterEspionageSystem` under `src/` finds **only** the wire partial `src/Main.Plans50_53.cs`. | VERIFIED (code); map cell is aspirational → DRIFT |

Status summary for Plan 51: **Core + catalog + save + daily tick: landed. Live UI
read path: not bound.** The system ticks every day via
`Main.TickPlans50To53` → `ShelterEspionageSystem.TickDay(day, inventory)` and its
state is captured on flush, but no panel or modal currently renders sleeper
rosters, dead drops, or incidents. `Ashfall.Core.Tests/Factions/ShelterEspionageSystemTests.cs`
(6 cases) and `PlanE1_29VehicleEspionageTests.cs` (3 cases) exercise the Core
contract directly.

### 2.3 Plan 52 — Survivor Mental Health: row re-verified

| Matrix cell (map) | Finding | Evidence |
|---|---|---|
| Authority invariant: zero engine refs + seeded PRNG | **HOLDS.** `Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs` (444 lines) is engine-free; stochastic branches (therapy breakthrough, per-trauma insomnia) draw from the injected `ISeededRng` (default `SeededRng(5252)`). | VERIFIED (code) |
| Data catalog: `psychological_trauma.json` | **PRESENT.** 5,373 bytes, `schema_version: 1`, 6 `trauma_types`, 3 `recovery_actions`, 5 `crisis_events`. Full contents tabulated in Part V Chapter 52. | VERIFIED (data) |
| Core system: `SurvivorMentalHealthSystem` | **PRESENT** with a larger and more defensive API than the map implies: id normalization (`NormalizeId`, ordinal-ignore-case), state normalization on both capture and restore, cloned read models (`State`, `Traumas`, `Therapies`, `Crises` all return copies), saturating counters, and a monotonic `TickDay` guard (`currentDay <= lastTickDay` is a no-op). | VERIFIED (code) |
| Save store: `SurvivorMentalHealthSaveStore` (`survivor_mental_health`) | **HOLDS.** `src/Host/SurvivorMentalHealthSaveStore.cs` (35 lines), section at `SaveSectionRegistry.cs:273` (domain `psychology`), persisted file `survivor_mental_health_save.json`. Corruption/migration coverage exists in `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`. | VERIFIED (code) |
| Presentation: `SurvivorDetailPanel` & `AfflictionsPanel` | **BOTH TYPES EXIST; BOTH BIND OTHER AUTHORITIES.** `src/UI/SurvivorDetailPanel.cs` (435 lines) binds `SurvivorsHostSession` plus a `SurvivorEnrichmentService` (needs, traits, status, fitness). `src/UI/AfflictionsPanel.cs` (488 lines) binds `MedicalHostSession`, `SurvivorsHostSession`, `InventoryHostSession`, `RespiratoryDegenerationSystem`, `MedicalTextCatalog`. Neither references the mental-health system. | VERIFIED (code); map cell is aspirational → DRIFT |

**Plan 52 status: Core + catalog + save + daily tick: landed. UI projection:
indirect only** (see 2.7 for who actually reads the state).

### 2.4 Plan 53 — Acoustic Director: row re-verified

| Matrix cell (map) | Finding | Evidence |
|---|---|---|
| Authority invariant: thin presentation projection + headless safe | **HOLDS with a nuance.** `ShelterAcousticDirector` (147 lines) is engine-free Core. `ShelterAcousticBridge` (`src/Audio/ShelterAcousticBridge.cs`, 77 lines) is a pure delegate adapter ("Thin presentation adapter with zero gameplay or simulation logic"). `AudioManager` (`src/Audio/AudioManager.cs`, 1,068 lines) guards all Godot-specific work behind `_headless`. | VERIFIED (code) |
| Data catalog: `shelter_audio_cues.json` | **PRESENT.** 4,738 bytes, `schema_version: 1`, 11 `cues` and 4 `mix_profiles`. Full contents tabulated in Part V Chapter 53. | VERIFIED (data) |
| Core system: `ShelterAcousticDirector` | **PRESENT.** Input `UpdateSimulationFacts(AcousticSimulationFacts)`, semantic trigger `TriggerSemanticCue(cueId)`, output `EvaluateSnapshot() → AcousticSnapshot { activeMixProfileId, continuousLayerIntensities, pendingOneShotCues }`. | VERIFIED (code) |
| Save store: reconstructed from live state | **HOLDS.** No save store, no section, no envelope exists for the director; `Main.Plans50_53.cs` constructs it fresh and binds a new `ShelterAcousticBridge` each session. `AcousticSimulationFacts` is plain state the host would re-populate from live systems. | VERIFIED (code) |
| "Zero RNG, pure state evaluation" | **DRIFT — partially false.** The director holds `_rng` (default `SeededRng(5353)`) and draws from it inside `EvaluateSnapshot()`: spontaneous `acue_structural_creak` when `structuralStressPermille > 600` (30% per evaluation) and `acue_falling_dust` when `> 800` (40%). Everything else in the evaluation is pure arithmetic. The honest statement: *the director is deterministic given facts plus a seeded spontaneous-cue stream; it is not zero-RNG.* | VERIFIED (code) |
| Presentation: `AudioManager` & `ShelterAudioController` | **TYPES EXIST AND ARE RICHER THAN THE MAP SAYS.** `ShelterAudioController` (208 lines) is a separate presentation-only lifecycle that follows `PowerGridSystem` and `StartingLevelSystem` events for generator/ventilation loops and a one-shot air-filter hazard alert — it does not consume the director. The director→manager connection is `ShelterAcousticBridge`, which the map omits. | VERIFIED (code) |

**The Plan 53 playback gap (important):** `ShelterAcousticBridge.SyncAcoustics()`
dispatches one-shots via `_playCue(cueId)` and loops via
`_updateLoop("shelter_acoustic", busId, volumeDb, 1f)`. But:

1. The bridge never calls `StartLoop`, so `AudioManager.UpdateLoop` finds no
   registered player under the loop key and silently does nothing.
2. A repo-wide search finds **no** `acue_` string in `src/` — the JSON cue ids
   (`acue_generator_hum_60hz`, `acue_structural_creak`, ...) are not entries of the
   host-side static `AudioCueCatalog`, so `PlayCue("acue_structural_creak")` would
   hit `LogMissingOnce` rather than play.

Net: the Core director evaluates snapshots every day tick
(`Main.TickPlans50To53` → `_shelterAcousticBridge?.SyncAcoustics()`), but audible
output from this specific path is currently a structural stub unless/until the
bridge starts loops and the `acue_*` ids are bridged into the host cue catalog.
This is exactly the kind of "compile-green is not integration" fact the foreman
rules require be written down. Related facts, also verified:

- `UpdateSimulationFacts` has **no production caller** under `src/` (only tests in
  `Ashfall.Core.Tests/Audio/ShelterAcousticDirectorTests.cs` and
  `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs` construct
  facts). In the live host the director therefore evaluates against the default
  facts object (`activeZoneOrRoom = "inner_vault"`, zero loads, no unread radio).
- The map's stated fork `shelter_acoustic` (singular) does not exist; the host
  forks `"shelter_acoustics"` (plural) (`src/Main.Plans50_53.cs:204`).

### 2.5 The RNG fork inventory, as actually implemented

The original map claims dedicated forks `garage_breakdown`, `shelter_espionage`,
`survivor_mental_health`, and `shelter_acoustic` "from `ISeededRng`". The verified
mechanism is different in three ways:

1. Forks are taken from the campaign day's `ICampaignRngStream` (the manager's
   `Fork(streamId, day, actionIndex)` seam in
   `Assets/Ashfall.Core/Random/CampaignRngStream.cs`), which derives a
   `StableHash`-based seed per stream id: `masterSeed * 31337 + StableHash.Of(id) * 1009 + day * 37 + actionIndex`.
2. The fork ids for these four plans are **raw strings passed at the host wire**,
  *not* entries of `CampaignStreamIds`. The constants class contains
   `Psychology`, `Expedition`, etc., but none of the four plan-local ids; adding
   them as constants is optional because `Fork(string)` accepts any id.
3. Two of the four map names are wrong:

| Plan | Map fork name | Actual fork expression | Where | Draws RNG? |
|---|---|---|---|---|
| 50 | `garage_breakdown` | `_campaignDay.Rng.Fork("vehicle_garage")` (fallback `SeededRng(50)`) | `src/Main.Plans50_53.cs:42` | **No** — the system never consumes the injected rng; all garage math is deterministic. Vehicle breakdown rolls use the expedition stream inside `ExpeditionSystem.TickHours` |
| 51 | `shelter_espionage` | `_campaignDay.Rng.Fork("shelter_espionage")` (fallback `SeededRng(51)`) | `src/Main.Plans50_53.cs:112` | Yes — investigation, sabotage, dead-drop spawn/pick |
| 52 | `survivor_mental_health` | `_campaignDay.Rng.Fork("survivor_mental_health")` (fallback `SeededRng(52)`) | `src/Main.Plans50_53.cs:158` | Yes — therapy breakthrough and target, per-trauma insomnia |
| 53 | `shelter_acoustic` (zero RNG) | `_campaignDay.Rng.Fork("shelter_acoustics")` (fallback `SeededRng(53)`) | `src/Main.Plans50_53.cs:204` | Yes — spontaneous structural creak / falling dust one-shots |

Fallback constants `SeededRng(50/51/52/53)` apply only when `_campaignDay` is not
yet constructed (pure-bootstrap or early-host paths), preserving determinism by
using fixed seeds rather than wall-clock entropy — consistent with the repo's
determinism contract.

### 2.6 Save-section registry evidence

`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` registers all three persistent
sections together, in one block, with matching file names:

```text
line 271: new("vehicle_garage",        "SaveVehicleGarage",        "SetupVehicleGarage",        "expeditions", "Plans 50-53 — expedition overland vehicle modifications and garage maintenance state")
line 272: new("faction_espionage",     "SaveShelterEspionage",     "SetupShelterEspionage",     "factions",    "Plans 50-53 — shelter faction espionage, sleeper assets, counter-intel, and sabotage state")
line 273: new("survivor_mental_health","SaveSurvivorMentalHealth", "SetupSurvivorMentalHealth", "psychology",  "Plans 50-53 — survivor psychological trauma, stress levels, catharsis, and mental health crises")
line 561-563 (file map): vehicle_garage → vehicle_garage_save.json
                         faction_espionage → faction_espionage_save.json
                         survivor_mental_health → survivor_mental_health_save.json
```

All three stores are built on `SaveStoreHub.Checksummed<T>(fileName, ownerName,
allowLegacyBareState: false)` — checksummed envelopes with legacy bare-state
*rejected*, which tightens the map's "legacy compatibility" bullet: legacy saves do
not get their old bare payloads silently accepted for these sections; an absent
section deserializes to the system's fresh default state (unmodded vehicles, no
sleeper agents, calm records), matching the map's intent through a different
mechanism (absence → default, not legacy-format tolerance). `SchemaVersionedEnvelope<T>`
exists at `Assets/Ashfall.Core/Save/SchemaVersionedEnvelope.cs` as the generic
versioned wrapper the map names.

### 2.7 The trauma-authority relationship (repo-memory question resolved)

Repo memory says the trauma authorities are `CombatTraumaSystem`,
`SomaticFlashbackSystem`, and `GuiltInsomniaSystem`. Verified relationship:
**those three and `SurvivorMentalHealthSystem` are four independent, parallel
authorities with no code references between them.** `SurvivorMentalHealthSystem` is
not a wrapper, not a duplicate that was merged, and not absent — it is the
catalog-driven Plans 50–53 wave system, and the other three are the earlier
"engine-agnostic port" wave. Specifics:

| System | Namespace / file | Catalog input | Save shape | Constructed by | Randomness |
|---|---|---|---|---|---|
| `SurvivorMentalHealthSystem` | `Ashfall.Core.Needs` / `Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs` | `psychological_trauma.json` → `PsychologicalTraumaCatalog` | `SurvivorMentalHealthState` in the `survivor_mental_health` checksummed section | `Main.Plans50_53.EnsureSurvivorMentalHealth` (live game path) | Fork `survivor_mental_health` |
| `CombatTraumaSystem` | `Ashfall.Core.Survivors` / `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` | none (constant-driven: `HypervigilancePerCombat = 0.05f` etc.) | `CombatTraumaSaveState` (own capture/restore) | `src/Host/Phase0HostSession.cs` (Phase 0 host surface) | Host-injected `Rng` callback |
| `SomaticFlashbackSystem` | `Ashfall.Core.Survivors` / `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` | none (constant-driven: `BaseFlashbackChancePerNoise = 0.15f` etc.) | `SomaticFlashbackSaveState` | `Phase0HostSession` | Host-injected |
| `GuiltInsomniaSystem` | `Ashfall.Core.Survivors` / `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | none (constant-driven: `SleepQualityPenaltyPerSeverity = 0.50f` etc.) | `GuiltInsomniaSaveState` | `Phase0HostSession` | Host-injected (via callbacks) |
| `MentalHealthCrisisSystem` | (host-batch wave) | needs/medical/chem-dep/roster inputs | `MentalHealthState` via `MentalHealthCrisisSaveStore` | `Main.ShelterBatch3.SetupMentalHealthCrisis` (live game path) | `Fork(CampaignStreamIds.Psychology, 0, 15)` |

Cross-reference searches confirm: no file under `Assets/Ashfall.Core/Needs`,
`Factions`, `Expeditions`, or `Audio` mentions the three `Survivors/` systems, and
no file under `Survivors/` mentions `SurvivorMentalHealth*`. The one verified
*consumer bridge* for the Plans 50–53 system is
`Assets/Ashfall.Core/Needs/SleepNarrativeProjection.cs`, which takes a
`SurvivorMentalHealthSystem` in its constructor and classifies sleep beats from
`SurvivorMentalHealthRecord` (insomnia, active traumas) — this is the projection
that feeds the sleep-narrative presentation wave (Plans 177-family tests:
`SleepNarrativePhantomPainTests`, `Plan177SleepNarrativeProjectionTests`).

So the accurate one-line answer to "duplicate, wrapper, or absent?" is: **a
parallel, still-live authority.** It owns stress floors, crises, and therapy for
the catalog wave; the Phase-0 trio owns combat hypervigilance, noise-triggered
flashbacks, and guilt insomnia for their wave; `MentalHealthCrisisSystem` owns the
needs-driven crisis loop on the shelter-batch wave. They coexist by scope, not by
delegation. Any future consolidation is a foreman decision, not something this
document can authorize.

### 2.8 The `PLANS_51_54_INTEGRATION_REPORT.md` numbering collision

`docs/PLANS_51_54_INTEGRATION_REPORT.md` (2026-09-10) exists, but its plan numbers
denote a **different plan series** than this map's. Verified by reading it:

- Its "Plan 51" = expedition playtest selftest (`--expedition-playtest-selftest`,
  30-day deterministic campaign, 10 sorties, estimate/actual parity).
- Its "Plan 52" = air-filter maintenance in `StartingLevelSystem`.
- Its "Plan 53" = economy `PriceExplanation`.
- Its "Plan 54" = medical oxygen scheduled treatment.

None of the report's four plan numbers denotes the same work as this map's
Plans 51–53, and its Plan 54 has no counterpart in this map at all (Chapter 54
below is an interaction-diagram chapter, not a plan). The report is therefore
**not** a
post-map status update for espionage/mental-health/acoustics. It *is* useful
adjacent evidence for Plan 50: its expedition leg recorded "breakdown transition"
as exercised, "same-seed byte equality, different-seed divergence, and mid-sortie
save/load equality", and "No `vehicles.json` tune was justified or applied" — i.e.
the vehicle travel math the garage decorates was validated end-to-end after this
map was written, without changing vehicle baselines. Its validation ledger (build
PASS; full Core suite 10,828 passed / 0 failed; several Godot headless selftests
PASS; `run-gates.py --tier fast` 47/47) is the most recent full-suite green on
record cited in docs, and predates any of this expansion's claims.

### 2.9 Plan 50 addendum: the CF-P6 armor-grades extension (already landed)

The queue in `AGENTS.md` lists `CF-P6-VEHICLE-ARMOR-GRADES` as available; the tree
shows the additive armor state **already present** on the Plan 50 seam:

- `VehicleCustomizationRecord` carries `armorGradeId`, `armorIntegrityPermille`,
  `armorIntegrityMaxPermille`, `armorMaterialProfileId`, `armorPurity`, with a
  comment that missing fields in old saves deserialize to stock defaults (legacy
  save tolerance at the DTO level — consistent with the envelope-level strictness
  in 2.6; the envelope rejects bare payloads, the DTO tolerates absent fields).
- `VehicleGarageSystem` has the armor API: `LoadArmorCatalog`,
  `CanInstallArmorGrade`, `InstallArmorGrade` (foundry-purity-stamped integrity
  pool, old-plate 50% `scrap_metal` refund), `ReforgeArmorPlate`,
  `GetArmorProfile`, `ArmorConditionBand` (nominal ≥75%, worn ≥25%, critical below,
  depleted at 0), and `DecorateProfile` applies armor speed/fuel deltas plus
  breakdown-mitigation scaling `(1000 - mitigation) / 1000`.
- Data: `Assets/StreamingAssets/Data/vehicle_armor_grades.json` (loader
  `VehicleArmorGradeCatalogLoader`, file name carried by the loader; referenced in
  `ContentUtilizationScanner` row `vehicle_armor_grades.json`).
- Host: armor material quality handoff via `TryGetArmorMaterialQuality` delegate
  bound in `Main.Plans50_53.BindVehicleGarageArmorMaterialQuality()`; terrain gate
  via `VehicleTerrainResolver` reading `ExpeditionVehicleSystem` definitions.
- Tests: `VehicleArmorGradesTests` (5), `Plan213VehicleArmorGradeTests` (21).

Implication for the queue: the CF-P6 row in `AGENTS.md` predates this tree state;
whoever integrates that package should treat it as a premise audit
(`AGENTS.md` rule 7) rather than a green-field build.

### 2.10 Catalog reachability and integrity wiring

All four catalogs are registered in the Core integrity/utilization layer:

- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` rows 519–523 map each
  JSON file to its loader + system pair (`vehicle_modifications.json` →
  `VehicleGarageCatalogLoader`/`VehicleGarageSystem`, `faction_intelligence.json`
  → `FactionIntelligenceCatalogLoader`/`ShelterEspionageSystem`,
  `psychological_trauma.json` → `PsychologicalTraumaCatalogLoader`/
  `SurvivorMentalHealthSystem`, `shelter_audio_cues.json` →
  `ShelterAudioCueCatalogLoader`/`ShelterAcousticDirector`), so data-integrity
  selftests can report authored-but-unconsumed fields.
- `Assets/Ashfall.Core/CatalogIntegrityRules.cs:305` and
  `CatalogIntegrityValidator.cs:465` list the four plans' field vocabulary
  (`slot_type`, `compatible_vehicle_tags`, `operation_class`, `target_subsystem`,
  `risk_level`, `trigger_tags`, `journal_entry_key`, `bus_id`, `playback_mode`,
  `ducking_group`, `attenuation_profile`) as recognized catalog columns — presence
  in these tables is what makes the JSON fields schema-legitimate rather than
  tolerated typos.

---

## Part III — Integration Framework

### 3.1 The decorate-don't-duplicate principle (Plan 50's template)

Plan 50 is the wave's architectural exemplar because it solves the oldest problem
in survival-management codebases: how to add a modification/maintenance layer on
top of an existing vehicle owner without forking it. The verified pattern:

1. **The base owner keeps travel.** `ExpeditionVehicleSystem` owns vehicle
   definitions, ownership state, condition, track gear, and builds a stock
   `ExpeditionVehicleProfile` via `CreateExpeditionProfile(vehicleId, kmPerTravelTick)`.
   It knows nothing about modifications.
2. **The decorator holds deltas, not copies.** `VehicleGarageSystem` never stores
   cargo capacity, speed, or fuel. It stores *installed slot ids* and *wear
   permille*, and computes effective deltas on demand
   (`GetEffectiveCargoCapacityDelta`, `GetEffectiveSpeedMultiplierDelta`,
   `GetEffectiveFuelConsumptionMultiplier`, `GetEffectiveWearRateMultiplier`,
   `GetEffectiveRadiationProtectionPermille` — each a fold over
   `installedSlots` values looked up in the catalog).
3. **Decoration is a single read-only seam.** `DecorateProfile(profile)` mutates
   the *profile struct* (cargo `+=`, speed `*= (1 + delta)`, fuel `*=`, breakdown
   chance scaled by armor mitigation). It is read-only over persisted garage state
   and documented as zero-RNG. The expedition simulation consumes the decorated
   profile and remains decoupled.
4. **Commands stay in Core; presentation forwards.** Install/uninstall/service/
   armor/reforge/recovery are Core methods with `out string reason` failures and
   optional `IPlayerInventoryPort` for atomic bill consumption. `VehicleGaragePanel`
   only renders read models and forwards commands; `HostCli.VehicleGarage.cs`
   proves the same commands work headless.

The same shape recurs across the wave: **catalog-driven Core system + checksummed
facade store + thin Godot adapter**, with the delta that Plans 51/52 own mutable
simulated state (sleepers, records) rather than pure decoration, and Plan 53 owns
no state at all beyond facts + pending one-shots.

### 3.2 Per-plan invariants (restated with verified anchors)

**Plan 50 invariants**

- I50.1 — `vehicle_modifications.json` (+ `vehicle_armor_grades.json`) is the only
  source of modification and armor definitions. VERIFIED (data).
- I50.2 — The garage never assigns travel; it only decorates profiles and holds
  wear. VERIFIED (code, `DecorateProfile` doc + `ExpeditionHostSession.cs:935`).
- I50.3 — Wear/immobilization/recovery are deterministic; no garage-local RNG
  draws. VERIFIED (code, zero `_rng.Next` in `VehicleGarageSystem.cs`).
- I50.4 — Inventory consumption is atomic (`TryConsumeBill`) or refused; refunds
  are explicit and fractional-safe (`Math.Max(1, amount / 2)`). VERIFIED (code).
- I50.5 — Immobilized vehicles refuse modification, armor install, reforge, and
  wear accrual (`RecordTripWear` early-returns), and clear only through service
  (all three subsystems < 900) or recovery completion. VERIFIED (code).
- I50.6 — Persistence round-trips through the checksummed `vehicle_garage`
  section; `RestoreState(null)` = factory reset. VERIFIED (code).

**Plan 51 invariants**

- I51.1 — `faction_intelligence.json` is the only source of operation and
  dead-drop template definitions. VERIFIED (data).
- I51.2 — All sleeper progression is seeded: the fork `shelter_espionage`
  supplies every draw (`InvestigateSuspect` roll, sabotage gate and prevention
  roll, spontaneous dead-drop gate and template pick). VERIFIED (code).
- I51.3 — Sabotage is abstract: the ledger records an incident with a summary and
  a target subsystem string; the only resource effect is a bounded 2×
  `scrap_metal` siphon on unprevented storage leaks. No faction-specific content
  leak, no real-world reference. VERIFIED (code + data).
- I51.4 — Counter-intel is a single scalar authority
  (`securityCounterIntelScore`, default 100, clamp 0..1000) set through
  `SetSecurityCounterIntelScore`; it gates both prevention (>150 to attempt, roll
  against score) and discovery (>120 for spontaneous dead-drop spawns). VERIFIED
  (code).
- I51.5 — Sleeper identity is hidden state: `isIdentified` flips only through
  `InvestigateSuspect`, and turning requires prior identification. VERIFIED (code).
- I51.6 — Persistence in the checksummed `faction_espionage` section. VERIFIED (code).

**Plan 52 invariants**

- I52.1 — `psychological_trauma.json` is the only source of trauma, recovery, and
  crisis definitions; ids are normalized (trim + ordinal-ignore-case) at load and
  re-pointed (`crisis_affinity` resolved to canonical crisis id). VERIFIED (code).
- I52.2 — Stress is a single scalar per survivor (0..1000, default 200) with a
  trauma-derived **floor**: `ReduceStress` cannot go below
  `sum(trauma.stress_floor_permille)` capped at 750. VERIFIED (code,
  `GetStressFloor`).
- I52.3 — Crises trigger at stress ≥ 750 through `AddStress`, require the crisis's
  own `threshold_stress_permille`, prefer trauma `crisis_affinity`, and fall back
  to `crisis_withdrawal` then `crisis_panic`. One active crisis per survivor.
  VERIFIED (code).
- I52.4 — Crisis resolution is time-boxed (`duration_days` countdown in `TickDay`,
  +100 stress relief on expiry); therapy resolution is stochastic (seeded) and
  trauma-resolving. VERIFIED (code).
- I52.5 — Insomnia is the bridge to the sleep domain: per-trauma daily rolls set
  `insomniaDaysRemaining ≥ 1`; active insomnia adds a flat +200 productivity
  penalty; `SleepNarrativeProjection` reads the same record. VERIFIED (code).
- I52.6 — Tone contract: crises are **non-stigmatizing operational events** —
  hoarding, withdrawal, shift refusal, panic, barricade — each with a productivity
  penalty and a `journal_entry_key`, never a "madness" meter or a moral failing
  label. VERIFIED (data, catalog descriptions).
- I52.7 — Persistence in the checksummed `survivor_mental_health` section with
  normalize-on-capture and normalize-on-restore (unknown ids dropped, duplicates
  collapsed, negative counters clamped). VERIFIED (code).

**Plan 53 invariants**

- I53.1 — `shelter_audio_cues.json` is the only source of cue definitions and mix
  profiles. VERIFIED (data).
- I53.2 — The director is a pure fact→snapshot evaluator except the seeded
  spontaneous one-shot draws; it never mutates gameplay state, never writes saves,
  and holds no Godot types. VERIFIED (code).
- I53.3 — Presentation stays thin: `ShelterAcousticBridge` translates a snapshot
  into `PlayCue`/`UpdateLoop` delegate calls; `AudioManager` owns buses, streams,
  pooling, DSP, and headless guards. VERIFIED (code).
- I53.4 — No persistence: the director is reconstructed from live state each
  session (`EnsureShelterAcoustics`); a stale director cannot survive a save/load
  because it is never captured. VERIFIED (code, absence of store + wire).
- I53.5 — Headless safety: `AudioManager` gates stream/player work behind
  `_headless`, so Core-driven snapshots are computable in CI without audio
  hardware. VERIFIED (code, `_headless` guards + `AudioSelfTest`).
- I53.6 — Known gap (not an invariant, a recorded limitation): JSON cue ids are
  not registered in the host `AudioCueCatalog` and the bridge never starts loops;
  snapshots are computed and dispatched but do not yet reach audible output on
  this path. VERIFIED (code, absence of `acue_` in `src/`; see 2.4).

### 3.3 Event and command flows

The wave follows the repo rule *Core events expose facts; host adapters apply
presentation and persistence effects.* Concretely, per plan:

- **Plan 50** — no C# events; the panel polls read models on refresh, and the host
  marks `_vehicleGarageDirty` wherever a mutation command succeeded
  (`src/Main.Plans50_53.cs`), flushing through `CaptureSection` into the
  save-section hub. Wear accrual is *pulled* by expedition execution
  (`RecordTripWear`), not pushed by an event.
- **Plan 51/52** — same dirty-flag pattern (`_shelterEspionageDirty`,
  `_survivorMentalHealthDirty`), set after each `TickDay` and cleared only on
  successful `CaptureSection`. No Core events are raised; the systems are
  command/tick-driven.
- **Plan 53** — the only *event-like* flow in the wave:
  `TickPlans50To53(currentDay)` → `ShelterAcousticBridge.SyncAcoustics()` →
  `EvaluateSnapshot()` → delegate calls into `AudioManager`. One-shot lists are
  drained per evaluation (fire-once semantics); continuous layers are re-stated
  every tick (level semantics).
- The older `Survivors/` trio, by contrast, *does* use C# events
  (`OnHypervigilanceIncreased`, `OnFlashbackTriggered`, `OnGuiltRecorded`, ...);
  if the two waves ever converge, event-style exposure is the direction the
  foreman rules favor for host reaction.

### 3.4 Save capture/restore contract

```text
Player tick / command
  └─ Main partial marks <system>Dirty = true
       └─ FlushPlans50To53()
            ├─ SaveVehicleGarage()        → VehicleGarageSystem.CaptureState()
            │    → VehicleGarageSaveStore.TryCapturePersisted(state)
            │    → Main.CaptureSection("vehicle_garage", payload)
            ├─ SaveShelterEspionage()     → same shape, "faction_espionage"
            └─ SaveSurvivorMentalHealth() → same shape, "survivor_mental_health"

Session boot
  └─ Ensure<System>()
       ├─ fork deterministic rng (or fixed-seed fallback)
       ├─ load catalog JSON from Assets/StreamingAssets/Data/
       └─ <Store>.TryLoad() → RestoreState(saved) | fresh default
```

Properties that hold (all VERIFIED by reading the wire and store code):

- **Section isolation:** each plan writes exactly its own section; the registry's
  `SaveSectionRegistry` block (2.6) is the single declaration point.
- **Checksummed envelopes:** `SaveStoreHub.Checksummed<T>` with
  `allowLegacyBareState: false` — corrupted or tampered payloads fail the checksum
  and the store returns null → the wire falls back to fresh default state.
- **Deep-copy discipline:** `CaptureState` round-trips through JSON in all three
  systems, so callers cannot mutate live state through a captured reference.
- **Normalize-on-boundary (Plan 52):** capture and restore both pass through
  `NormalizeState`, so malformed ids, negative counters, orphan crisis ids, and
  duplicate records cannot survive a save/load cycle.
- **Plan 53's absence-of-save is itself the contract:** reconstruct-from-live-state
  is safe precisely because the snapshot is a pure function of facts; there is
  nothing to restore that live systems cannot recompute.

### 3.5 RNG fork strategy (as implemented)

- The campaign day exposes a stream manager (`ICampaignRngManager`); each plan
  takes a **long-lived fork per system**, not per draw: one
  `_campaignDay.Rng.Fork("<plan-local-id>")` at ensure-time, held for the session.
  This differs from the fork-per-day discipline used by newer waves (see the
  Plans 122–125 and 162–165 comments in `CampaignStreamIds`), and is the main
  determinism caveat for the wave: **draw order within a session affects the
  sequence**, so replay equality depends on identical command/tick order, not just
  the same seed. Mid-session save/load is nevertheless stable because system state
  (not RNG position) is what persists; resuming continues the fork from wherever
  the session left it — acceptable for these systems because their draws gate
  flavor/probability, not resource conservation.
- The fixed-seed fallbacks (`SeededRng(50|51|52|53)`) guarantee that headless and
  early-bootstrap paths are reproducible even without a campaign day.
- `CampaignStreamIds` does not (yet) declare the four ids as constants. If a later
  package promotes them, the promotion must keep the exact strings
  `vehicle_garage`, `shelter_espionage`, `survivor_mental_health`,
  `shelter_acoustics` — the seed derivation hashes the string, so any rename is a
  replay-breaking change.

### 3.6 Integrity pipeline integration

- Authored fields are legitimized by `CatalogIntegrityRules` /
  `CatalogIntegrityValidator` vocabularies (2.10).
- Unconsumed-field detection is owned by `ContentUtilizationScanner` rows per
  catalog (2.10); adding a JSON field without a consumer shows up in
  data-integrity selftests rather than silently rotting.
- Catalog ids referenced cross-system (therapy `required_room` values like
  `room_bunks`, `room_main`, `room_clinic`; dead-drop `target_location` values
  like `loc_recovery_yard`) are string references validated only as far as the
  generic integrity rules go — the quiet-room example (`room_reading_quiet_room`,
  which exists in `shelter_rooms.json:345` and `shelter_construction.json:59` but
  is *not* one of the three `required_room` values in `psychological_trauma.json`)
  shows the map's diagram rounded the room names for narrative effect. DRIFT,
  cosmetic.

---

## Part IV — Code Architecture

### 4.1 Module map (verified file inventory)

```text
Assets/Ashfall.Core/                                (netstandard2.1, engine-free)
├── Expeditions/VehicleGarageSystem.cs        864 ln   Plan 50 authority (+ DTOs,
│                                                      armor API, recovery model)
├── Factions/ShelterEspionageSystem.cs        340 ln   Plan 51 authority (+ DTOs)
├── Needs/SurvivorMentalHealthSystem.cs       444 ln   Plan 52 authority (+ DTOs,
│                                                      normalization layer)
├── Needs/PsychologicalTraumaCatalog.cs        — ln    Plan 52 catalog types
├── Needs/SleepNarrativeProjection.cs          — ln    Plan 52 consumer bridge
├── Audio/ShelterAcousticDirector.cs          147 ln   Plan 53 authority (+ facts,
│                                                      snapshot DTOs)
├── ExpeditionVehicleSystem.cs                base     decorated owner (Plan 50)
├── Expeditions/ExpeditionSystem.cs           travel   breakdown roll owner
├── Save/SaveSectionRegistry.cs               271-273, 561-563  section declarations
├── Save/SaveStoreHub.cs / SchemaVersionedEnvelope.cs  envelope infrastructure
├── Random/CampaignRngStream.cs               fork derivation + manager
├── Content/ContentUtilizationScanner.cs      519-523, 1187-1191  utilization rows
└── CatalogIntegrityRules.cs / CatalogIntegrityValidator.cs  field vocab

Assets/StreamingAssets/Data/                        (authoritative JSON)
├── vehicle_modifications.json              4,882 B   8 modifications
├── vehicle_armor_grades.json                 — B     CF-P6 armor grades (add-on)
├── faction_intelligence.json              3,884 B   8 operations + 4 dead drops
├── psychological_trauma.json              5,373 B   6 traumas + 3 recoveries + 5 crises
└── shelter_audio_cues.json                4,738 B   11 cues + 4 mix profiles

src/                                                (net8.0, Godot host)
├── Main.Plans50_53.cs                       267 ln   the wave wire: ensure/setup/
│                                                      save/flush/tick for all four
├── Main.VehicleGarage.cs                      — ln   Plan 50 panel lifecycle glue
├── Host/VehicleGarageSaveStore.cs            35 ln   checksummed facade (P50)
├── Host/ShelterEspionageSaveStore.cs         35 ln   checksummed facade (P51)
├── Host/SurvivorMentalHealthSaveStore.cs     35 ln   checksummed facade (P52)
├── Host/ExpeditionHostSession.cs             :935    DecorateProfile call site
├── Host/HostCli.VehicleGarage.cs              — ln   headless CLI probes (P50)
├── UI/VehicleGaragePanel.cs                 635 ln   Plan 50 panel (IBindablePanel)
├── UI/FactionDetailPanel.cs                 155 ln   holdfast dossier (not espionage)
├── UI/DailyBriefingModal.cs                 245 ln   DailyBriefingReport modal
├── UI/SurvivorDetailPanel.cs                435 ln   survivor needs/traits (not MH)
├── UI/AfflictionsPanel.cs                   488 ln   medical afflictions (not MH)
├── Audio/ShelterAcousticBridge.cs            77 ln   director→manager adapter
├── Audio/ShelterAudioController.cs          208 ln   power/ventilation loop lifecycle
└── Audio/AudioManager.cs                  1,068 ln   buses, streams, DSP, ducking

Ashfall.Core.Tests/                                 (net9.0, xUnit)
├── Expeditions/VehicleGarageSystemTests.cs        6 cases
├── Expeditions/Plan50VehicleGarageIntegrationTests.cs  5 cases
├── Expeditions/VehicleArmorGradesTests.cs         5 cases
├── Expeditions/Plan213VehicleArmorGradeTests.cs  21 cases
├── Expeditions/PlanE1_29VehicleEspionageTests.cs  3 cases
├── Factions/ShelterEspionageSystemTests.cs        6 cases
├── Needs/SurvivorMentalHealthTests.cs            14 cases
├── Audio/ShelterAcousticDirectorTests.cs          4 cases
└── Campaign/Plans50_53_SharedIntegrationTests.cs  3 cases (cross-plan)
```

Case counts are `[Fact]`/`[Theory]` occurrences counted on 2026-09-25; theory
row-expansion means executed-case counts are higher than file counts.

### 4.2 Plan 50 component spec — `VehicleGarageSystem`

**State DTOs** (all `[Serializable]`, snake-ish C# field names, JSON round-tripped):

```csharp
public sealed class VehicleCustomizationRecord {
    public string vehicleId;
    public Dictionary<string,string> installedSlots;   // slot_type -> mod id
    public int chassisStressPermille;                  // 0..1000
    public int engineFoulingPermille;                  // 0..1000
    public int transmissionWearPermille;               // 0..1000
    public bool isImmobilized;
    public string immobilizedReason;
    // CF-P6 additive armor state (defaults preserve legacy saves)
    public string armorGradeId;  public int armorIntegrityPermille;
    public int armorIntegrityMaxPermille;
    public string armorMaterialProfileId;  public string armorPurity;
}
public sealed class VehicleRecoveryMission {
    public string missionId;      // "recov_{n}_{vehicleId}"
    public string strandedVehicleId;  public string locationId;
    public int requiredFuelUnits = 10;
    public int progressTicks;     public int requiredTicks = 120;
    public bool isComplete;
}
public sealed class VehicleGarageState {
    public string systemId;                                   // "vehicle_garage"
    public Dictionary<string, VehicleCustomizationRecord> vehicleRecords;
    public Dictionary<string, VehicleRecoveryMission> activeRecoveries;
    public int nextRecoveryCounter = 1;
}
```

**Command surface** (each returns `bool` + `out string reason`; inventory optional
so pure-Core callers can probe):

| Command | Guard highlights | Effect |
|---|---|---|
| `InstallModification(vehicleId, slotType, modId, inv)` | catalog id exists; slot matches `modDef.slot_type`; not immobilized; materials sufficient | atomically consumes install bill; `installedSlots[slotType] = modId` (one mod per slot) |
| `UninstallModification(vehicleId, slotType, inv)` | record and slot occupied | removes mod; refunds `max(1, cost/2)` per line |
| `CanInstallArmorGrade` / `InstallArmorGrade(vehicleId, gradeId, inv)` | grade exists; not stock/default; not already worn; not immobilized; terrain compatible; materials | consumes bill, refunds 50% of displaced plate's scrap, stamps integrity pool `grade.integrity_pool_permille × armorBp × purityBp` clamped to [50%, 130%] of authored pool |
| `ReforgeArmorPlate(vehicleId, inv)` | plate fitted, not immobilized, integrity < max | consumes reforge bill; integrity → max |
| `ServiceChassis/Engine/Transmission(vehicleId, inv, repairPermille)` | wear > 0; cost = ceil(permille/50) scrap for chassis, ceil(permille/100) parts for engine/trans | consumes, reduces wear, may clear immobilization (all < 900) |
| `RegisterRecoveryMission(vehicleId, locationId, fuel)` | vehicle immobilized; no existing mission | creates `recov_*` mission |
| `AdvanceRecoveryMission(missionId, ticks)` / `AdvanceRecoveries(delta)` | mission exists, incomplete | progress += ticks; auto-completes at 120 |
| `CompleteRecoveryMission(missionId, inv)` | mission complete | wear clamped to ≤ 800 (garage-serviceable), immobilization cleared, mission removed |
| `RecordTripWear(vehicleId, km, roughness)` | not immobilized, km > 0 | see wear math below |
| `DecorateProfile(profile)` | profile non-null, has vehicleId | applies mod + armor deltas |

**Wear math** (all deterministic, VERIFIED):

```text
wearMult    = GetEffectiveWearRateMultiplier(v) × max(0.5, roadRoughnessMultiplier)
baseWear    = round(distanceKm × 2 × wearMult)
armorAbsorb = min(armorIntegrity, round(baseWear × wearAbsorptionPermille / 1000))
chassis  += baseWear − armorAbsorb          (clamp 0..1000)
engine   += round(baseWear × 0.8)           (clamp 0..1000)
trans    += round(baseWear × 0.9)           (clamp 0..1000)
immobilize if any component reaches 1000:
    reason = "Critical component catastrophic failure during overland transit."
```

**Failure modes and their handling:** unknown mod/slot/vehicle → typed reason
strings, no state change; inventory shortfalls → pre-checked with `HasSufficient`
before any consumption (atomic bills); immobilized vehicle → all modification
commands refuse with the same reason text (UI can surface verbatim); recovery
completion before progress → refused with tick counts in the reason; `RestoreState(null)`
→ factory state; armor catalog absent → `GetArmorProfile` returns the default
"Stock Plating" profile so decoration degrades to no-op.

**Panel contract:** `VehicleGaragePanel.Bind(garage, vehicles, inventory)`; labels
for detail/vehicle/mod/armor/armor-cost/maintenance/recovery text; three selectors
(vehicle, mod, armor); nine command buttons including `_completeRecoveryButton`;
`LastFeedback` exposes the last Core reason string; `OnClose` for keyboard/back
handling.

### 4.3 Plan 51 component spec — `ShelterEspionageSystem`

**State DTOs:**

```csharp
public sealed class SleeperAgentRecord {
    public string survivorId;  public string factionId;
    public int loyaltyPermille = 500;   // allegiance to the faction (field
                                        // default; EnrollSleeperAgent's parameter default is 600 — 51.2)
    public int suspicionPermille;       // shelter-side evidence, 0..1000
    public bool isIdentified;  public bool isTurnedDoubleAgent;
    public int leaksCommittedCount;
}
public sealed class ActiveDeadDrop {
    public string dropId;      // "drop_{n}_{templateId}"
    public string templateId;  public string targetLocationId;
    public int remainingDays;  public int intelYield;  public bool isIntercepted;
}
public sealed class SabotageIncident {
    public string incidentId;  // "inc_{n}"
    public string operationId; public string targetSubsystem;
    public int dayRecorded;    public bool preventedByCounterIntel;
    public string summary;
}
public sealed class ShelterEspionageState {
    public string systemId;                                  // "shelter_espionage"
    public Dictionary<string, SleeperAgentRecord> sleeperAgents;
    public List<ActiveDeadDrop> activeDeadDrops;
    public List<SabotageIncident> recentIncidents;
    public int securityCounterIntelScore = 100;
    public int totalIntelPoints;  public int nextDropCounter = 1;
    public int nextIncidentCounter = 1;
}
```

**Key mechanics (VERIFIED math):**

- *Investigation:* `threshold = 1000 − (suspicion/2 + investigatorSkill × 5)`,
  floored at 200; unmask on `rng.Next(0,1000) ≥ threshold`. Non-suspects are
  cleared with an explicit "no hostile handler links" report — investigating an
  innocent is a safe, quiet operation.
- *Turning:* requires prior `isIdentified` and 5 `scrap_metal` (the "security
  payoff / relocated family guarantee"); on success
  `loyalty ← clamp(1000 − loyalty + 200, 400, 1000)` and +25 intel points. The
  turned agent then produces +2 intel/day in `TickDay` instead of leaking.
- *Daily sleeper tick:* unturned agents gain +5 suspicion and one leak count;
  if `loyalty > 600` a sabotage gate rolls (15% per day); a rolled sabotage is
  prevented when `counterIntel > 150` and `rng.Next(0,1000) < counterIntel`;
  unprevented storage leaks siphon up to 2 `scrap_metal`.
- *Dead drops:* spawn spontaneously only when counter-intel > 120, fewer than 3
  active, and a 25% daily roll succeeds; template picked uniformly from the
  catalog; each expires after `expiry_days` unless intercepted; interception
  awards `reward_intel_points` exactly once.

**Failure modes:** enrolling an existing sleeper → false; turning an unidentified
or already-turned agent → refused with reason; intercepting an expired/collected
drop → refused with report; `TickDay` is idempotent per invocation but has **no
day guard** (unlike Plan 52) — the host must call it exactly once per day, which
`TickPlans50To53` does.

### 4.4 Plan 52 component spec — `SurvivorMentalHealthSystem`

**State DTOs:**

```csharp
public sealed class SurvivorMentalHealthRecord {
    public string survivorId;
    public int stressPermille = 200;
    public List<string> activeTraumaIds;
    public int insomniaDaysRemaining;
    public string currentCrisisId;   public int crisisDaysRemaining;
    public int therapySessionCount;
}
public sealed class SurvivorMentalHealthState {
    public string systemId;                                  // "survivor_mental_health"
    public Dictionary<string, SurvivorMentalHealthRecord> survivorRecords;
    public int totalCatharsisBreakthroughs;
    public int lastTickDay = -1;                             // monotonic tick guard
}
```

**Key mechanics (VERIFIED math):**

- *Stress floor:* active traumas push a floor = min(750, Σ `stress_floor_permille`);
  `ReduceStress` clamps to `[floor, 1000]`, so trauma cannot be out-relaxed away —
  it must be resolved.
- *Trauma infliction:* `InflictTrauma` rejects duplicates, adds the id, and adds
  the trauma's floor as stress. *Resolution* (`ResolveTrauma`, also reached via
  therapy breakthrough) removes the id, saturating-increments
  `totalCatharsisBreakthroughs`, and relieves 150 stress.
- *Therapy:* `PrescribeTherapy(survivorId, actionId, hasCounselor)` always counts
  a session and applies `stress_reduction_permille (+ counselor_bonus if
  counselor)`; then a seeded breakthrough roll:
  `chance = clamp(daily_resolution_chance_permille + counselor_bonus × 2, 0, 1000)`;
  on success a random active trauma resolves with an explicit breakthrough outcome
  string.
- *Crisis trigger:* at stress ≥ 750 with no active crisis, the first trauma with a
  resolvable `crisis_affinity` picks the crisis; fallback order
  `crisis_withdrawal` → `crisis_panic`; the crisis must still satisfy its own
  `threshold_stress_permille` (750–950 range in data — hoarding is the lowest
  at 750, barricade the highest at 950 — so the affinity pick can defer the
  crisis until stress climbs further).
- *Day tick (guarded):* `TickDay(day)` no-ops for `day ≤ lastTickDay`; decrements
  crisis timer (expiry → clear + 100 relief), decrements insomnia, rolls each
  active trauma's `insomnia_chance_permille` (100–400 in data) to set/extend
  insomnia ≥ 1 day.
- *Productivity:* crisis penalty (`productivity_penalty_permille`, 350–1000 in
  data) + flat 200 for insomnia, clamped 0..1000.

**Failure modes:** unknown trauma/therapy id → typed refusal; empty/whitespace
survivor id → argument exception on `GetOrCreateRecord`; capture/restore both
normalize (unknown ids dropped, case-collapsed, duplicates merged, negative
counters clamped, crisis-without-days repaired), so no malformed state can
persist.

### 4.5 Plan 53 component spec — director, bridge, manager

**DTOs:**

```csharp
public sealed class AcousticSimulationFacts {
    public int generatorWattage;  public int generatorMaxWattage = 1000;
    public int ventilationLoadPermille;   // 0..1000
    public int waterPumpLoadPermille;     // 0..1000
    public float ambientRadiationMillisieverts;
    public int structuralStressPermille;  // 0..1000
    public bool hasUnreadRadioBroadcast;
    public string activeZoneOrRoom = "inner_vault";
}
public sealed class AcousticSnapshot {
    public string activeMixProfileId = "acue_prof_inner_vault";
    public Dictionary<string,int> continuousLayerIntensities;  // layer -> 0..1000
    public List<string> pendingOneShotCues;
}
```

**Evaluation rules (VERIFIED):** profile selection by substring match on
`activeZoneOrRoom` (surface/outpost/gate → `acue_prof_surface`; airlock/decon →
`acue_prof_airlock`; excavation/mine/stope → `acue_prof_deep_excavation`; else
`acue_prof_inner_vault`, also the fallback if the profile id is missing).
Layer intensities: generator = `wattage × 1000 / maxWattage` (guard divide-by-zero);
ventilation and machinery = the permille facts verbatim; radiation =
`clamp(mSv × 100, 0, 1000)`; structural = the fact verbatim; radio = 600 with an
unread broadcast else 50. One-shots: queued semantic cues are appended and the
queue drains; then the two seeded spontaneous draws (creak > 600 @ 30%, dust
> 800 @ 40%).

**Bridge contract:** `SyncAcoustics()` plays each pending one-shot through
`PlayCue(cueId)` and restates every continuous layer as
`UpdateLoop("shelter_acoustic", busId, volumeDb, 1f)` with
`volumeDb = permille <= 0 ? −80 : −30 + (permille/1000) × 24`. Delegates are
constructor-injected, so tests can run the bridge without Godot (the 4-case
director test plus the shared integration tests exercise the Core half; the
bridge's delegate seam makes it host-testable the same way).

**Manager responsibilities (VERIFIED):** 15-bus topology (`Music, Ambience, SFX,
UI, Voice, Alerts, Generator, Ventilation, Radio, Medical, Surface, Machinery,
ShelterSocial, Subterranean` + Master), pooled one-shot players with per-cue
cooldowns, stream caching, DSP effects (low-pass occlusion on `Surface`, band-pass
+ Atan distortion on `Radio`), tweened cutoff changes, accessibility ducking
offsets, `_headless` short-circuits. Cue resolution goes through the static
`AudioCueCatalog`; ids absent from it are logged once and dropped
(`LogMissingOnce`) — which is exactly what happens today to `acue_*` ids
(2.4, I53.6).

### 4.6 Sequence walkthroughs

**W-A. Breakdown → garage repair → recovery mission**

```text
1. Expedition launch: ExpeditionHostSession builds stock profile
   (CreateExpeditionProfile) and calls Garage.DecorateProfile(profile)
   → cargo/speed/fuel adjusted; armor scales breakdownChancePerTick down.
2. During travel: ExpeditionSystem.TickHours rolls the expedition RNG once per
   tick against vehicleBreakdownChancePerTick (line ~773-780); a hit breaks the
   sortie's vehicle transit.
3. Wear accrual along the route: RecordTripWear(km, roughness) raises component
   permille; a component hitting 1000 sets isImmobilized + reason string.
4. Garage availability: panel refuses install/uninstall/armor/reforge on the
   immobilized vehicle (uniform reason string).
5. Player registers recovery: RegisterRecoveryMission(vehicleId, locationId, fuel)
   → mission recov_N_<id> with requiredTicks = 120.
6. Days advance: TickPlans50To53 → (host advances recoveries) progress grows;
   mission auto-marks complete at ≥ 120 ticks.
7. Player completes: CompleteRecoveryMission → wear clamped ≤ 800, immobilization
   cleared; service commands can now finish the job
   (chassis: ceil(permille/50) scrap; engine/trans: ceil(permille/100) parts).
8. Flush: _vehicleGarageDirty → vehicle_garage section captured (checksummed).
```

**W-B. Hostility threshold → sleeper/dead-drop cycle**

```text
1. Setup: a faction operation list (fop_*) describes what hostile actors do;
   the shelter holds enrolled sleepers (EnrollSleeperAgent) hidden in the
   survivor roster.
2. Daily: TickPlans50To53 → ShelterEspionageSystem.TickDay(day, inventory).
3. Each unturned sleeper: suspicion +5, leaks +1; loyalty > 600 → 15% sabotage
   gate; counter-intel > 150 rolls prevention (rng < score); failure siphons
   ≤ 2 scrap_metal and records a SabotageIncident with a restrained summary.
4. Counter-intel > 120 and < 3 active drops → 25% chance a dead drop spawns from
   fdrop_* templates (culvert, tree knot, pylon base, silo cavity).
5. Player path A (security): InvestigateSuspect(survivor, skill) — threshold
   math unmasks the agent → isIdentified.
6. Player path B (recruitment): AttemptTurnDoubleAgent — costs 5 scrap_metal,
   flips loyalty, +25 intel; the agent now yields +2 intel/day.
7. Player path C (fieldwork): InterceptDeadDrop(dropId) before expiry →
   intelYield into totalIntelPoints.
8. Flush: faction_espionage section captured. No UI binds this state yet
   (2.2): the honest present-day read model is the save file and tests.
```

**W-C. Trauma → crisis → quiet-room/journal arc**

```text
1. Cause (upstream): a raid casualty or betrayal event adds stress
   (AddStress) and possibly InflictTrauma("trauma_survivor_guilt") — the
   catalog's trigger_tags (death/sacrifice/abandonment) document intent for
   callers; the tagging hookup is the caller's contract, not the system's.
2. Floor rises: the record's stress floor grows by 200; ordinary relief cannot
   drop stress below it.
3. Escalation: stress ≥ 750 and no active crisis → TriggerPotentialCrisis
   picks crisis_withdrawal (affinity) once stress ≥ its 800 threshold.
4. Daily: TickDay decrements crisisDaysRemaining (3 days for withdrawal);
   insomnia rolls may stack sleep damage; productivity penalty = 600 (+200 if
   insomnia).
5. Player response: PrescribeTherapy("therapy_journal_catharsis",
   hasCounselor) — required_room "room_clinic" documents where it happens;
   stress −80 (−40 more with a counselor); each day a 140‰ (+80‰ counselor)
   seeded breakthrough roll can resolve a trauma outright.
6. Resolution: ResolveTrauma → +1 catharsis breakthrough, −150 stress.
   Crisis expiry → −100 stress. The crisis wrote journal_entry_key
   "journal_crisis_withdrawal" for the journal layer to render.
7. Flush: survivor_mental_health section captured, normalized both directions.
```

**W-D. Environment change → acoustic cue → bus mix change**

```text
1. Facts change (intended path): shelter systems update loads — generator
   wattage, ventilation load, pump load, ambient mSv, structural stress,
   unread radio, current room — and call UpdateSimulationFacts.
   NOTE (verified gap): no production caller exists yet; live host evaluates
   against default facts.
2. Semantic event (optional): TriggerSemanticCue("acue_bulkhead_heavy_slam")
   queues a one-shot for the next evaluation.
3. Daily tick: TickPlans50To53 → ShelterAcousticBridge.SyncAcoustics() →
   EvaluateSnapshot():
     - room "excavation_sector_04" → profile acue_prof_deep_excavation
       (reverb 0.85, lowpass 5500 Hz, −6 dB).
     - generator at 640/1000 W → layer "generator" = 640.
     - structural stress 820 → creak draw (30%) and dust draw (40%).
4. Bridge: one-shots → AudioManager.PlayCue; layers → UpdateLoop with
   volumeDb = −30 + 0.024 × permille (−80 dB at zero).
5. AudioManager: bus topology already exists (Generator, Ventilation, Radio,
   Machinery, Surface, Subterranean...); DSP shapes the Surface bus
   (low-pass occlusion) and Radio bus (band-pass + distortion); accessibility
   duck offsets apply on top.
6. Known gap (I53.6): PlayCue resolves through AudioCueCatalog, which has no
   acue_* entries, and the bridge never StartLoops — so today the snapshot is
   computed and dispatched but silent on this path. Closing the gap = register
   the JSON cues host-side + start loops keyed "shelter_acoustic".
```

---

## Part V — Full Chapters

### Chapter 50 — Overland Vehicle Garage

**Authority statement.** `VehicleGarageSystem` (`Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`,
`SystemId = "vehicle_garage"`) is the single authority for vehicle modification
installs, component wear, immobilization, garage service, armor plates, and
recovery missions. `ExpeditionVehicleSystem` remains the single authority for
vehicle ownership, definitions, and travel. `vehicle_modifications.json` and
`vehicle_armor_grades.json` are the single data authorities. The
`vehicle_garage` checksummed save section is the single persistence authority.
`VehicleGaragePanel` is the single presentation surface. All VERIFIED (code/data).

#### 50.1 The modification catalog, exhaustively

Eight authored modifications (`Assets/StreamingAssets/Data/vehicle_modifications.json`,
`schema_version: 1`). Effects schema: `cargo_capacity_delta` (kg), `speed_multiplier_delta`
(fraction), `fuel_consumption_multiplier` (×), `wear_rate_multiplier` (×),
`radiation_protection_permille` (0..950 clamp in code). Costs are `(item_id, amount)`
bills; `install_labor_ticks` is authored but **not consumed by the current Core
code** (a scanner-visible authored field reserved for the duty-roster labor
integration — treat as UNVERIFIED (map text) that it is scheduled anywhere).

| id | slot | cargo Δ | speed Δ | fuel × | wear × | rad ‰ | install cost | labor | tags |
|---|---|---|---|---|---|---|---|---|---|
| `vmod_expanded_flatbed` | cargo | +60 | −0.05 | 1.05 | 1.10 | 0 | 4 mech parts, 8 scrap | 240 | cargo, hauler, flatbed |
| `vmod_lead_lined_cab` | protection | −20 | −0.10 | 1.10 | 1.05 | +400 | 12 scrap, 3 mech parts | 300 | protection, radiation, armor |
| `vmod_reinforced_bullbar` | protection | 0 | −0.02 | 1.00 | 0.85 | +50 | 6 scrap, 2 mech parts | 180 | protection, chassis, collision |
| `vmod_aux_fuel_rack` | utility | −10 | 0.00 | 0.95 | 1.00 | 0 | 4 scrap, 2 mech parts | 120 | utility, fuel, range |
| `vmod_traction_cleats` | mobility | 0 | +0.15 | 1.08 | 0.90 | 0 | 5 scrap, 3 mech parts | 200 | mobility, traction, rough_terrain |
| `vmod_stretcher_mount` | utility | −15 | 0.00 | 1.00 | 1.00 | +100 | 3 scrap, 4 bandage | 150 | utility, medical, rescue |
| `vmod_heavy_winch` | utility | −10 | −0.02 | 1.02 | 0.90 | 0 | 4 mech parts, 5 scrap | 180 | utility, recovery, towing |
| `vmod_engine_supercharger` | engine | 0 | +0.35 | 1.20 | 1.25 | 0 | 6 mech parts, 4 scrap | 280 | engine, speed, performance |

Design reads (all derivable from the table): the flatbed trades speed and wear for
the largest cargo jump; the lead-lined cab is the only meaningful radiation answer
(+400‰, versus a 950 total clamp) at a stiff mobility price; the bullbar is the
only wear *reducer* among protection options; the aux fuel rack is the only fuel
*saver*; traction cleats are the only speed gain without an engine slot; the
supercharger is the pure glass-cannon (fastest, thirstiest, fastest-wearing).
Slot exclusivity (one mod per slot key per record) means a truck must choose
between bullbar and lead cab — both are `protection`.

`compatible_vehicle_tags` gate by vehicle tag sets (`road`, `rough`, `quad`,
`bike`, `truck`, `hauler`); note the Core `InstallModification` guard checks
slot/catalog/immobilization/materials but the *tag* compatibility is enforced at
the data/presentation layer via selector filtering — the panel shows only fitting
mods for the selected vehicle. (UNVERIFIED (map text) that a Core-side tag check
exists; the verified Core guard set is the one listed in 4.2.)

#### 50.2 Wear, breakdown, and immobilization math

Wear enters only through `RecordTripWear(vehicleId, distanceKm, roadRoughnessMultiplier)`:

```text
wearMult = Π(mod.wear_rate_multiplier) × max(0.5, roughness)
baseWear = round(km × 2 × wearMult)                       // 2 permille per km base
armor absorption (CF-P6): absorbed = min(plateIntegrity, round(baseWear × wearAbsorption‰/1000))
chassisStress    += baseWear − absorbed        // clamp 0..1000
engineFouling    += round(baseWear × 0.8)      // clamp 0..1000
transmissionWear += round(baseWear × 0.9)      // clamp 0..1000
```

Consequences worth internalizing:

- **Chassis is the canary.** It accumulates fastest, so chassis service is the
  routine maintenance action; engine and transmission lag at 0.8× and 0.9× but
  still hit 1000 eventually on unmodded trucks (base wear multiplier 1.0).
- **A ~500 km unmodded chassis clamp** is the rough order of magnitude: at
  2 permille/km and roughness 1.0 the chassis reaches the 1000 clamp in 500 km
  (engine lags at 0.8× to 625 km, transmission at 0.9× to ~556 km); on the
  0.5 roughness floor the same clamp takes 1,000 km. With the supercharger's
  1.25× wear those distances shrink proportionally; with the bullbar's 0.85×
  they stretch. The design consequence: distance kills, service resurrects —
  a truck's lifetime is set by the maintenance cadence, not the odometer.
- **Immobilization is a hard tripwire at exactly 1000 permille** on any component,
  recorded with a single restrained reason string. There is no random component —
  a vehicle never "suddenly" breaks from garage state alone; the mid-route
  *breakdown* roll is the expedition layer's probability over
  `breakdownChancePerTick`, which itself derives from vehicle condition
  (`(100 − condition)/100 × 0.15 × gear risk multiplier`) and is only *reduced*
  by garage armor.
- **Wear stops while immobilized** (`RecordTripWear` early-return) — a stranded
  vehicle does not rot further, which keeps recovery missions meaningful rather
  than futile.

#### 50.3 Service economics

| Action | Cost formula | Currency | Notes |
|---|---|---|---|
| Service chassis by N permille | `ceil(N / 50)` | `scrap_metal` | 50 permille per scrap at any N |
| Service engine by N permille | `ceil(N / 100)` | `mechanical_parts` | |
| Service transmission by N permille | `ceil(N / 100)` | `mechanical_parts` | |
| Full recovery (post-mission) | free at completion | — | wear clamped to ≤ 800 |
| Uninstall mod | refund `max(1, cost/2)` per bill line | any item | always full refund of half |
| Replace armor plate | refund 50% of displaced plate's `scrap_metal` lines | `scrap_metal` | other-line items not refunded |
| Reforge armor plate | authored `reforge_cost` bill | per grade | restores integrity to max |

Immobilization clears when *all three* components drop below 900 permille —
service amounts are chosen by the caller, so a player who services only the
broken-at-1000 component stays immobilized if the others sit at 950. This is the
system's quiet lesson: catastrophic failure is a *fleet-state* problem, not a
single-part problem.

#### 50.4 Recovery missions

Model: one active mission per vehicle (`RegisterRecoveryMission` refuses a
duplicate), id `recov_{counter}_{vehicleId}`, authored location string,
`requiredFuelUnits` (default 10 — authored on the record but the Core progress
math is tick-based, so fuel is a declared requirement for the caller to enforce),
`requiredTicks = 120`. Two advancement seams exist: `AdvanceRecoveryMission(id, ticks)`
(targeted) and `AdvanceRecoveries(delta)` (all missions — the natural host tick).
Completion is *always* an explicit player command; auto-complete only flips the
`isComplete` flag. On completion the vehicle returns garage-serviceable (≤ 800
wear) rather than pristine — recovery rescues the asset, it does not rebuild it.
All VERIFIED (code).

#### 50.5 Expedition integration and the profile decoration order

`ExpeditionHostSession.cs:935` decorates at profile-build time; the decorated
profile then flows into `ExpeditionSystem.Start`/`Estimate` paths, which clamp
`breakdownChancePerTick` to [0,1], adopt `cargoCapacityKg` as the loot cap, and
use `speedMultiplier`/`fuelPerTravelTick` in the discrete travel-step math
characterized by the 51–54 report's expedition leg ("estimate/actual ticks"
parity, rounded `DiscreteTravelStep` on both estimate and execution). Because
`DecorateProfile` is read-only over garage state and zero-RNG, estimate and
execution see identical decoration — the property that makes the playtest's
0-mismatch result possible in the vehicle's presence.

The radiation field is the one decoration the expedition travel path does not
consume directly in the verified code; `GetEffectiveRadiationProtectionPermille`
exists as a garage read model (clamped 0..950) for callers that expose cab
protection (encounter/radiation resolution layers). UNVERIFIED (map text) that a
specific consumer binds it today; the API is the verified fact.

#### 50.6 Armor grades (CF-P6) in depth

Four-tier additive plating on the same seam. Definition source
`vehicle_armor_grades.json` via `VehicleArmorGradeCatalogLoader` (loader carries
`FileName`, error list, and a `HasErrors` gate the host checks before
`LoadArmorCatalog`). Fields per grade: `id`, `display_name`, `tier`,
`is_default`, `integrity_pool_permille`, `mitigation_permille` (clamp 0..400 in
`GetArmorProfile`), `wear_absorption_permille` (clamp 0..500), `speed_multiplier_delta`,
`fuel_consumption_multiplier`, `compatible_terrain_types`, `install_cost`,
`reforge_cost`, `default_grade_id` at catalog level.

Integrity stamping on install:

```text
armorBp   = clamp(foundryQuality.ArmorModifierBp, 500, 2000)   // 1000 when no foundry
purityBp  = Poor 850 | Standard 1000 | High 1100 | Exceptional 1200
stamped   = round(pool × armorBp/1000 × purityBp/1000)          // AwayFromZero
clamped   = clamp(stamped, ceil(pool × 0.5), ceil(pool × 1.3))
```

Condition bands (static `ArmorConditionBand`): `depleted` at 0, `nominal` ≥ 75%,
`worn` ≥ 25%, `critical` below; `none` when no max. Depleted plates keep their
mass penalties (speed/fuel deltas still apply in `DecorateProfile`) but lose
mitigation and absorption — armor fails *progressively*, and the reforge bill is
the recovery path. Terrain gating reads the vehicle owner's `terrain_type`
through the injected resolver; unknown vehicles stay install-neutral in pure-Core
probes and are rejected in the live garage.

#### 50.7 The panel, as built

`VehicleGaragePanel` (635 lines) is a chrome-in-code `Control` implementing
`IBindablePanel` with an `AshfallDashboardShell` + `AshfallStatusRail` frame. It
keeps three `OptionButton` selectors (vehicle, mod, armor grade) and nine command
buttons; every command's `out string reason` lands in `LastFeedback` and the
`_commandResult` label, so Core failure text is the player-facing text (one
authority for wording). Read models rendered per refresh: fitted mods, the three
wear permille lines, immobilization banner, armor plate block (grade, band,
integrity/max, purity, material profile) with install/reforge cost previews,
maintenance costs, and the recovery mission block with progress. The panel's doc
comment records the integration claim explicitly: displayed deltas are the deltas
the sortie actually uses, because they come from the same `DecorateProfile` math.
`OnClose` plus the shell's keyboard/back handling preserve the accessibility
contract; `Unbind()` detaches for lifecycle hygiene.

#### 50.8 Plan 50 test matrix (focused)

| Test file | Cases | Covers |
|---|---|---|
| `VehicleGarageSystemTests.cs` | 6 | install/uninstall economics, effective-delta folds, wear math |
| `Plan50VehicleGarageIntegrationTests.cs` | 5 | catalog load → decorate → expedition profile parity |
| `VehicleArmorGradesTests.cs` | 5 | armor catalog, stamping clamps, condition bands |
| `Plan213VehicleArmorGradeTests.cs` | 21 | CF-P6 grade matrix, terrain gates, reforge, mitigation scaling |
| `Plans50_53_SharedIntegrationTests.cs` (P50 legs) | shared | cross-plan boot/tick/save cycle |

#### 50.9 Restraint notes for future editors

- Do not add a second wear ledger; `VehicleCustomizationRecord` *is* the ledger.
- Do not move breakdown probability into the garage; the expedition roll's
  stream ownership is load-bearing for replay equality (2.1, 3.5).
- Do not consume `install_labor_ticks` without a duty-roster premise audit —
  it is authored-but-dormant, which is exactly the state the utilization
  scanner is designed to report.

---

### Chapter 51 — Faction Espionage

**Authority statement.** `ShelterEspionageSystem`
(`Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs`, `SystemId = "shelter_espionage"`)
is the single authority for sleeper agents, counter-intelligence, sabotage
incidents, dead drops, and intel points. `faction_intelligence.json` is the
single data authority. The `faction_espionage` checksummed section is the single
persistence authority. The map's named presentation nodes exist but do not bind
this system (2.2) — there is **no live presentation surface today**. All code and
data claims VERIFIED; the presentation gap is a verified absence.

#### 51.1 The intelligence catalog, exhaustively

Eight faction operations (`fop_*`) describe what hostile external actors attempt
against the shelter; four dead-drop templates (`fdrop_*`) describe where intel
can be physically recovered. Operation schema: `display_name`, `target_faction`,
`operation_class` (`intelligence_leak | sabotage | counter_intel | theft |
infiltration | ambush_prep`), `target_subsystem`, `suspicion_per_tick`,
`cooldown_days`, `detection_difficulty`, `tags`. Dead-drop schema:
`display_name`, `target_location`, `reward_intel_points`, `risk_level`
(`low | medium | high`), `expiry_days`.

| operation id | class | target subsystem | faction | suspicion/tick | cooldown | detection |
|---|---|---|---|---|---|---|
| `fop_inventory_leak` | intelligence_leak | storage | warlords_sector_4 | 5 | 4 | 40 |
| `fop_fuel_sabotage` | sabotage | fuel | iron_garrison | 15 | 8 | 55 |
| `fop_water_tampering` | sabotage | water | cult_of_ash_sign | 20 | 7 | 60 |
| `fop_power_interruption` | sabotage | power | iron_garrison | 18 | 6 | 50 |
| `fop_misinformation` | counter_intel | radio | ash_militia | 8 | 5 | 35 |
| `fop_patrol_theft` | theft | patrol | warlords_sector_4 | 12 | 9 | 45 |
| `fop_sleeper_activate` | infiltration | personnel | iron_garrison | 25 | 12 | 70 |
| `fop_convoy_interception` | ambush_prep | storage | warlords_sector_4 | 10 | 6 | 50 |

| dead drop id | target location | intel | risk | expiry |
|---|---|---|---|---|
| `fdrop_culvert_ruins` | `loc_recovery_yard` | 25 | medium | 5 days |
| `fdrop_hollow_tree` | `loc_grain_silo` | 15 | low | 7 days |
| `fdrop_rusted_pylon` | `loc_radio_relay_mast` | 35 | high | 4 days |
| `fdrop_abandoned_silo` | `loc_diesel_tank_farm` | 40 | high | 3 days |

Reading the data: sabotage targets map onto the shelter's real resource
subsystems (fuel, water, power, storage) without modeling any specific
infrastructure — the incident is one ledger row and (if unprevented) one small
resource siphon. Faction ids (`warlords_sector_4`, `iron_garrison`,
`cult_of_ash_sign`, `ash_militia`) are fictional wasteland factions already owned
by the faction data authority; this catalog only *references* them. Risk on dead
drops correlates with yield and expiry (high risk = more intel, less time), a
compact risk/reward curve.

#### 51.2 Sleeper agents: the hidden-state model

`SleeperAgentRecord` is keyed by survivor id. Defaults encode the design: loyalty
starts at 600 on enrollment override (600 default parameter, clamped 0..1000),
suspicion at 0, unidentified, unturned, no leaks. The record is *the* truth about
who is a mole; nothing else in the repo may duplicate that judgment (rule 5).
Three progression axes:

- **Suspicion (evidence):** grows passively +5/day per unturned agent, and
  callers may add more via `AddSuspicion` for event-driven evidence. It is
  shelter-side knowledge — it makes investigation *stronger* but never exposes
  the agent by itself.
- **Loyalty (allegiance):** drives sabotage appetite (> 600 gate) and is
  scrambled by turning (`1000 − loyalty + 200`, clamped 400..1000 — the 600
  enrollment default lands exactly at 600, a fanatical 900-loyalty operative
  flips to the 400 clamp, and a half-convinced 400-loyalty one lands hot at
  800). The post-turn value is inert to the tick: turned agents `continue`
  past the leak/sabotage branch entirely, so the scrambled number is flavor
  for read models, not a live risk.
- **Identification (knowledge):** flips only through `InvestigateSuspect`; only
  identified agents can be turned. Leaks are counted per agent for the case file.

#### 51.3 The investigation roll

```text
threshold = 1000 − (suspicionPermille / 2 + investigatorSkill × 5)
threshold = max(200, threshold)                 // never certain
unmask    = rng.Next(0, 1000) >= threshold
```

Properties: integer division on suspicion halves its weight (200 permille of
suspicion adds 100 to the threshold); each point of investigator skill adds 5;
the floor of 200 means even a maximally suspicious, maximally skilled
investigation fails 20% of the time — no certainty without turning. Innocent
survivors get a clean, non-accusatory report ("cleared of all covert suspicion")
and no state change: investigating the wrong person is quiet and free of stigma,
consistent with the wave's tone contract. All VERIFIED (code).

#### 51.4 Turning a double agent

Preconditions: identified, not already turned, 5 `scrap_metal` available (the
"security payoff / relocated family guarantee" — the cost is deliberately
material, not moral). Effect: loyalty flip per 51.2, +25 intel, and a permanent
`isTurnedDoubleAgent` flag after which `TickDay` pays +2 intel/day instead of
running the leak/sabotage branch. The turned agent stays in the roster as an
asset the player knows about — the system never removes survivors, it reassigns
their allegiance. VERIFIED (code).

#### 51.5 Sabotage incidents and counter-intelligence

Daily, per unturned agent with loyalty > 600: a 15% sabotage gate; on gate hit,
prevention requires `securityCounterIntelScore > 150` **and** a draw below the
score on a 0..1000 scale (so a 400 score prevents ~40% of attempts; a 900 score
~90%). Unprevented storage-class leaks siphon up to 2 `scrap_metal` (bounded —
espionage can nibble, never hollow out, the stockpile). Every attempt lands in
`recentIncidents` with id `inc_{n}`, operation `fop_inventory_leak` (the only
operation the current tick emits — the other seven catalog rows are authored
surface for callers/future legs; UNVERIFIED (map text) that they are wired to
day ticks), target subsystem string, day, prevention flag, and a restrained
summary. The summaries carry the tone contract: "Counter-intelligence intercepted
covert siphon attempt by an unidentified infiltrator" / "Storage depot manifest
tampered with; minor supplies reported missing" — no nationalities, no gore, no
real-world echo.

`securityCounterIntelScore` is a caller-owned scalar (default 100): the system
exposes `SetSecurityCounterIntelScore` and clamps 0..1000, but never mutates the
score itself. Whatever drives shelter security posture (guard duty, decoded
radio, hardened doors) must *push* the score; espionage only *reads* it. That is
the one-authority rule applied to counter-intel.

#### 51.6 Dead drops

Spawning is defensive: only when counter-intel > 120 (a competent security
posture *finds* drops — higher vigilance yields more fieldwork, not less),
fewer than 3 active, 25% daily gate, uniform template pick. Each drop counts
down `expiry_days` in `TickDay` and silently evaporates at zero (the informant
moved on; nothing dramatic happens). `InterceptDeadDrop` pays
`reward_intel_points` once, removes the drop, and writes a factual report string.
`totalIntelPoints` is the system's single cumulative score: +25 per turned
agent, +2/day per turned agent, +yield per intercept — a currency for a future
faction-standing or research spender (no spender exists in this system;
UNVERIFIED (map text) that intel points purchase anything today).

#### 51.7 The briefing modal and faction panel — what they actually are

- `DailyBriefingModal` (245 lines) renders `DailyBriefingReport` — the day's
  authoritative briefing built by `DailyBriefingReportBuilder` from
  `BriefingFact` inputs, with an "Intelligence & Recon" section among its
  sections. It is a *display and acknowledgment* surface (Enter/Space/Tab
  contract, typewriter cadence, deep-link callbacks) with **no espionage-system
  binding**: no verified code path feeds `ShelterEspionageState` into the
  builder's facts. The map's pairing of the two is aspiration, not wiring.
- `FactionDetailPanel` (155 lines) renders a holdfast faction's dossier —
  doctrine, treaties, trade commodities, trust, event logs — bound to
  `HoldfastFactionEntry` and friends. Intel *logs* in the dossier come from
  holdfast data, not from `ShelterEspionageState`.

Closing the presentation gap (if a foreman signs a package for it) is a
projection-only change: both surfaces exist, both refresh patterns are
established, and the read model is already JSON-clean. Nothing in this document
authorizes that work.

#### 51.8 Determinism and save shape

All draws come from the `shelter_espionage` fork; the same session command
sequence from the same master seed replays identically. State is small (a
dictionary of sleepers, two short lists, four scalars) and round-trips through
the checksummed `faction_espionage` envelope; `RestoreState(null)` = no sleepers,
no drops, no incidents, score 100. The corruption/migration suite covers the
store shape (`ComprehensiveSaveStoreCorruptionAndMigrationTests`).

#### 51.9 Plan 51 test matrix (focused)

| Test file | Cases | Covers |
|---|---|---|
| `ShelterEspionageSystemTests.cs` | 6 | enrollment, investigation math, turning, tick branches |
| `PlanE1_29VehicleEspionageTests.cs` | 3 | espionage × vehicle cross-plan premises |
| `Plans50_53_SharedIntegrationTests.cs` (P51 legs) | shared | boot/tick/save cycle |
| `VersionReportContractTests.cs` | (leg) | section presence in version reporting |

#### 51.10 Restraint notes

- Do not add a "paranoia meter" or shelter-wide suspicion aura; suspicion is
  per-agent evidence, by design.
- Do not wire the seven dormant `fop_*` rows into `TickDay` without a premise
  audit of what each subsystem sabotage would actually touch — the current
  single-branch tick is the verified contract.
- Any intel-point spender belongs to the economy or faction authority, not here.

---

### Chapter 52 — Survivor Mental Health

**Authority statement.** `SurvivorMentalHealthSystem`
(`Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs`, `SystemId = "survivor_mental_health"`)
is the single authority for catalog-driven psychological trauma, per-survivor
stress with trauma floors, therapy prescriptions, and crisis events *for the
catalog wave*. It coexists — without delegation — with three constant-driven
`Survivors/` systems (combat trauma, somatic flashbacks, guilt insomnia) and one
host-batch crisis engine (`MentalHealthCrisisSystem`); the full relationship map
is 2.7. `psychological_trauma.json` is the single data authority. The
`survivor_mental_health` checksummed section is the single persistence authority
for its state. All code/data claims VERIFIED.

#### 52.1 Trauma types, exhaustively

Six authored traumas. Schema: `display_name`, `description`, `stress_floor_permille`,
`insomnia_chance_permille` (daily, per active trauma), `trigger_tags` (for
callers), `crisis_affinity` (resolved to canonical crisis id at load).

| id | floor ‰ | insomnia ‰/day | trigger tags | crisis affinity |
|---|---|---|---|---|
| `trauma_combat_shock` | 250 | 300 | combat, explosion, injury | `crisis_panic` |
| `trauma_survivor_guilt` | 200 | 250 | death, sacrifice, abandonment | `crisis_withdrawal` |
| `trauma_claustrophobia` | 150 | 150 | confinement, vault, darkness | `crisis_barricade` |
| `trauma_nightmares` | 180 | 400 | radiation, sleep, flash | `crisis_shift_refusal` |
| `trauma_radiation_dread` | 220 | 200 | rads, fallout, geiger | `crisis_hoarding` |
| `trauma_bereavement` | 160 | 180 | loss, family, grief | `crisis_withdrawal` |

Design reads: combat shock is the heaviest floor (250) — violence leaves the
deepest baseline; nightmares have the highest insomnia pressure (400‰ daily) —
the mind keeps the schedule; withdrawal is the shared affinity of both grief
traumas, so bereavement plus survivor's guilt converges on the same crisis.
Descriptions do the tone work: "Nerve-rending hyperarousal", "Crushing
existential remorse", "Terrifying dreams of peeling skin" — specific, bodily,
fictional, never clinical labels applied to the *person*; the crisis events
(52.3) carry the non-stigmatizing framing.

#### 52.2 Recovery actions, exhaustively

Three authored therapies. Schema: `display_name`, `description`, `required_room`,
`stress_reduction_permille`, `daily_resolution_chance_permille`,
`counselor_bonus_permille`.

| id | required room | stress −‰ | resolution ‰/day | counselor bonus ‰ |
|---|---|---|---|---|
| `therapy_quiet_room` | `room_bunks` | 70 | 120 | +30 |
| `therapy_support_circle` | `room_main` | 90 | 150 | +50 |
| `therapy_journal_catharsis` | `room_clinic` | 80 | 140 | +40 |

Notes: the map's interaction diagram names `room_reading_quiet_room` as the
quiet-room anchor; the catalog actually requires `room_bunks` for
`therapy_quiet_room`. `room_reading_quiet_room` does exist as a shelter room
(`shelter_rooms.json:345`, canonical in `shelter_construction.json:59`), so the
diagram conflated the room registry with the therapy requirement — cosmetic
DRIFT, recorded in 3.6. The support circle is the strongest therapy on every
axis (people help), but requires the main hall; the quiet room is the
lowest-intensity option for sensory-overloaded survivors; journaling sits
between, in the clinic. `required_room` is authored documentation for callers —
the Core `PrescribeTherapy` does not itself check room occupancy (UNVERIFIED
(map text) that any caller enforces it today; the verified contract is the field
plus the caller-supplied `hasCounselor` boolean).

#### 52.3 Crisis events, exhaustively

Five authored crises. Schema: `display_name`, `description`, `threshold_stress_permille`,
`duration_days`, `productivity_penalty_permille`, `journal_entry_key`.

| id | threshold ‰ | days | productivity −‰ | journal key |
|---|---|---|---|---|
| `crisis_withdrawal` | 800 | 3 | 600 | `journal_crisis_withdrawal` |
| `crisis_hoarding` | 750 | 4 | 350 | `journal_crisis_hoarding` |
| `crisis_shift_refusal` | 850 | 2 | 1000 | `journal_crisis_shift_refusal` |
| `crisis_panic` | 900 | 2 | 800 | `journal_crisis_panic` |
| `crisis_barricade` | 950 | 2 | 900 | `journal_crisis_barricade` |

These five are the implementation of the non-stigmatizing crisis contract
(I52.6): every crisis is a *behavior the shelter can work around* — withdrawal,
hiding biscuits in bunk slats, refusing one rotation, trembling at plumbing
clatter, jamming a storage latch — each with a bounded duration, a productivity
cost the duty roster can absorb, and a journal key so the event is narrated as
the survivor's experience rather than flagged as a malfunction. None of the
descriptions pathologize; all are concrete and domestic. This is the wave's most
important data-authored tone artifact.

#### 52.4 Stress mechanics: floors, ceilings, and the 750 line

Stress is 0..1000, default 200. The two structural rules:

- **Floor:** `min(750, Σ active trauma floors)` — a survivor with combat shock
  and radiation dread cannot rest below 470. Relief efforts below the floor are
  wasted; trauma *resolution* (which removes the floor term) is the only way to
  lower the floor.
- **Crisis line:** `AddStress` triggers `TriggerPotentialCrisis` the moment
  stress ≥ 750 with no active crisis. The crisis chosen is the first
  affinity-resolvable trauma's crisis, *then gated by that crisis's own
  threshold* — so a nightmares-only survivor at 800 stress is *not* yet in
  shift-refusal (threshold 850); they cross into crisis later, and the fallback
  (`crisis_withdrawal` at 800, then `crisis_panic` at 900) only applies when no
  trauma has a resolvable affinity. The threshold gate is the system's pacing
  mechanism: heavier crises arrive later and shorter.

#### 52.5 Therapy math and the breakthrough roll

```text
reduction = clamp(stress_reduction_permille + counselor? counselor_bonus : 0, 0, 1000)
stress   -= reduction                      // clamped to floor
if active traumas > 0:
    chance = clamp(daily_resolution_chance_permille + counselor_bonus × 2, 0, 1000)
    if rng.Next(0,1000) < chance:
        resolve random active trauma       // +1 breakthrough, −150 stress
```

Counselors are worth more on resolution than on relief (bonus ×2 in the roll)
— the designed message is that accompanied therapy *resolves* rather than
merely soothes. Breakthrough resolution also fires the same −150 relief as
manual resolution, so a successful session can double-dip (reduction + relief);
that is the authored generosity, not an accident. Every prescription counts a
session (`therapySessionCount`) regardless of outcome — attendance is recorded
even when nothing breaks through. VERIFIED (code).

#### 52.6 The day tick and insomnia

`TickDay(currentDay)` is monotonic (`day ≤ lastTickDay` no-ops), making it safe
against double-tick host bugs. Per survivor per day: crisis countdown (expiry →
clear + 100 relief), insomnia countdown, and per-active-trauma insomnia rolls
(the floor ‰/day chances); a success sets `insomniaDaysRemaining = max(current, 1)`
— insomnia is a *condition gate* for the sleep domain, refreshed daily while
traumas are active, and `SleepNarrativeProjection` reads exactly this record to
classify sleep beats (restless, nightmare-marked, phantom-pain-adjacent). The
productivity penalty read model (`GetProductivityPenaltyPermille`) sums crisis
penalty + 200 insomnia flat, clamped 0..1000, for duty-roster consumers.

#### 52.7 Catharsis, the journal, and cross-layer hooks

The map's diagram draws `Journal Catharsis & Resilience Unlocks ──► [JournalSystem]`.
What the tree verifies is narrower and honest: crises and therapy author
`journal_entry_key` strings into the catalog and state; the mental-health system
normalizes those keys but does not itself write journal entries, and no
`JournalSystem` type was found consuming them in the verified searches (the keys
appear in `CatalogIntegrityRules`/`Validator` vocabularies and in the catalog
DTO). The catharsis *mechanic* that is fully real is
`totalCatharsisBreakthroughs` + `ResolveTrauma`'s −150 relief — resilience as a
counter, available to any future projection. The diagram's arrow is design
intent; UNVERIFIED (map text) as wiring. (This is the same pattern as 51.7: the
wave ships Core truth plus authored keys, and leaves presentation seams for
signed packages.)

#### 52.8 Relationship to the canonical trauma systems (the full picture)

2.7 tabulates the four authorities; the design-level summary:

- **`SurvivorMentalHealthSystem` (this chapter)** — catalog-driven, wave-scoped,
  stress/floor/crisis/therapy, saved in `survivor_mental_health`, ticked by the
  Plans 50–53 wire, consumed by `SleepNarrativeProjection`. The live game path
  for *its* concerns.
- **`CombatTraumaSystem` / `SomaticFlashbackSystem` / `GuiltInsomniaSystem`
  (`Ashfall.Core.Survivors`)** — the earlier constant-driven trio, event-rich
  (`OnHypervigilanceIncreased`, `OnFlashbackTriggered`, `OnGuiltRecorded`, ...),
  host-callback RNG, captured in their own save shapes, constructed on the
  Phase 0 host surface (`src/Host/Phase0HostSession.cs:235-237,351,367`). Their
  concerns (raid-derived hypervigilance with false alarms, noise-triggered
  flashbacks with companion grounding, guilt-weighted sleep penalties with
  sedative/dialogue compensation) do not overlap the catalog wave's
  stress/crisis/therapy concerns; they *adjacent* them through the same
  survivors.
- **`MentalHealthCrisisSystem` (host batch wave)** — needs/medical/chem-dep/roster
  driven crisis loop on the live path, forked from `CampaignStreamIds.Psychology`
  action 15, own `MentalHealthCrisisSaveStore`. The needs-driven crisis engine
  where this chapter's system is the trauma-driven one.

Consolidation (if ever) is a foreman-signed architecture package: the four do
not currently disagree about any state, they merely coexist, and the fastest way
to break three working systems would be a premature merge. This document
records the boundary; it does not authorize crossing it.

#### 52.9 Plan 52 test matrix (focused)

| Test file | Cases | Covers |
|---|---|---|
| `SurvivorMentalHealthTests.cs` | 14 | floors, crisis gating, therapy rolls, normalization, tick guard |
| `SleepNarrativePhantomPainTests.cs` | (leg) | projection × record contract |
| `Plan177SleepNarrativeProjectionTests.cs` | (leg) | sleep-beat classification |
| `Plans50_53_SharedIntegrationTests.cs` (P52 legs) | shared | boot/tick/save cycle |
| `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | (leg) | `survivor_mental_health` store corruption/migration |

#### 52.10 Restraint notes

- Never add stigmatizing labels, "sanity" meters, or player-facing diagnosis
  strings; the catalog's crisis names and the panel-facing copy stay behavioral
  and domestic.
- Do not let relief sources bypass the trauma floor silently — floor bypass
  would erase the design's central trade (rest vs. resolution).
- `MentalHealthCrisisSystem` (needs-driven) and this system (trauma-driven) must
  not be merged by convenience; each has its own save section and tick owner.
- Trauma infliction call sites should map real campaign events to
  `trigger_tags` before calling `InflictTrauma`; the system deliberately does
  not crawl events itself.

---

### Chapter 53 — Subterranean Acoustic Direction

**Authority statement.** `ShelterAcousticDirector`
(`Assets/Ashfall.Core/Audio/ShelterAcousticDirector.cs`) is the single Core
authority for translating shelter simulation facts into a semantic audio
snapshot. `shelter_audio_cues.json` is the single data authority for cue
definitions and mix profiles. There is deliberately **no save store** — the
director is reconstructed from live state each session. Presentation is split:
`ShelterAcousticBridge` (translation), `AudioManager` (buses, streams, DSP,
pooling), `ShelterAudioController` (a *separate* power/ventilation lifecycle
that does not consume the director). All code/data claims VERIFIED; the
playback-registration gap and the unpopulated-facts gap are verified absences
(2.4, I53.6).

#### 53.1 The cue catalog, exhaustively

Eleven cues. Schema: `display_name`, `bus_id`, `playback_mode`
(`loop | one_shot | procedural_density`), `base_volume_db`, `pitch_jitter_permille`,
`ducking_group`, `description`.

| id | bus | mode | base dB | jitter ‰ | duck group |
|---|---|---|---|---|---|
| `acue_generator_hum_60hz` | generator | loop | −6.0 | 20 | machinery |
| `acue_ventilation_rush` | ventilation | loop | −8.0 | 30 | machinery |
| `acue_water_pump_clatter` | machinery | loop | −10.0 | 40 | machinery |
| `acue_geiger_clicks_low` | radiation | procedural_density | −12.0 | 150 | none |
| `acue_geiger_clicks_high` | radiation | procedural_density | −8.0 | 120 | none |
| `acue_structural_creak` | structural | one_shot | −4.0 | 80 | none |
| `acue_falling_dust` | structural | one_shot | −7.0 | 60 | none |
| `acue_radio_static_burst` | radio | one_shot | −9.0 | 100 | radio |
| `acue_radio_voice_intercept` | radio | one_shot | −6.0 | 50 | radio |
| `acue_bulkhead_heavy_slam` | bulkhead | one_shot | −3.0 | 40 | dialogue |
| `acue_hydraulic_door_hiss` | bulkhead | one_shot | −5.0 | 30 | dialogue |

Descriptions are the world-building layer: a diesel flywheel drone, duct rush
"responding to scrubbing load", piston chatter and sump vibration, ionization
cascades, "deep resonant groaning of reinforced concrete", grit showers from
micro-tremors, ionospheric skip noise, "distorted military communique or coded
wasteland chatter fragments", blast-door pressure latches. Note the two ducking
groups that protect *speech*: `dialogue` (bulkhead impacts duck under it) and
`radio` (self-ducking) — the accessibility contract encoded in data.

Four mix profiles carry the space acoustics:

| id | space | reverb room size | lowpass cutoff | attenuation |
|---|---|---|---|---|
| `acue_prof_surface` | surface outpost & sentry post | 0.10 | 20,000 Hz | 0.0 dB |
| `acue_prof_airlock` | decontamination airlock | 0.40 | 14,000 Hz | −2.0 dB |
| `acue_prof_inner_vault` | reinforced living habitation | 0.60 | 9,000 Hz | −4.0 dB |
| `acue_prof_deep_excavation` | deep subterranean stope | 0.85 | 5,500 Hz | −6.0 dB |

The gradient is the game's depth language: the deeper you go, the smaller and
duller the world sounds (more reverb, less top end, quieter).

#### 53.2 The zero-RNG claim, corrected

The director's evaluation is *deterministic arithmetic over facts* for all six
continuous layers and the profile pick. Exactly two draws exist, both in the
spontaneous one-shot block of `EvaluateSnapshot()`: a structural-creak chance
(30% when stress > 600) and a falling-dust chance (40% when stress > 800), from
the seeded `shelter_acoustics` fork. Correct statements for future docs:

- "The director's continuous mix is a pure function of simulation facts." TRUE.
- "The director is zero-RNG." FALSE — spontaneous hazard one-shots are seeded
  draws (and this is *good* design: creaks that occur on a fixed schedule stop
  being information; the seed keeps them replayable while making them feel
  unstaged).
- Replay property: given identical facts and identical evaluation counts, the
  spontaneous block replays identically — determinism survives; purity does not.

#### 53.3 Snapshot evaluation, line by line

```text
profile pick (first match wins, else inner_vault, else literal fallback string):
  room contains surface|outpost|gate      → acue_prof_surface
  room contains airlock|decon             → acue_prof_airlock
  room contains excavation|mine|stope     → acue_prof_deep_excavation
  default                                  → acue_prof_inner_vault
layers:
  generator  = clamp(wattage×1000/maxWattage, 0, 1000)   // 0 when maxWattage ≤ 0
  ventilation= clamp(ventilationLoadPermille, 0, 1000)
  machinery  = clamp(waterPumpLoadPermille, 0, 1000)
  radiation  = clamp(mSv × 100, 0, 1000)                  // 10 mSv saturates
  structural = clamp(structuralStressPermille, 0, 1000)
  radio      = hasUnreadRadioBroadcast ? 600 : 50         // presence, not volume
one-shots:
  queue drained (semantic cues, fire-once)
  stress > 600 and rng < 30% → acue_structural_creak
  stress > 800 and rng < 40% → acue_falling_dust
```

The radio layer deserves a note: 600 vs 50 encodes *attention*, not loudness —
an unread broadcast sits forward in the mix; a silent set still hums faintly.
And the radiation curve saturating at 10 mSv means the geiger layer is a
*warning instrument* with a ceiling, not a dosimeter readout.

#### 53.4 The bridge contract

`ShelterAcousticBridge` (77 lines) is deliberately dumb:

```text
SyncAcoustics():
  snapshot = director.EvaluateSnapshot()
  foreach cue in snapshot.pendingOneShotCues: PlayCue(cue)
  foreach (layer, permille) in snapshot.continuousLayerIntensities:
      volumeDb = permille <= 0 ? −80.0 : −30.0 + (permille/1000)×24.0
      UpdateLoop("shelter_acoustic", layer, volumeDb, 1.0)
```

The volume curve maps 0..1000 permille onto −30..−6 dB (with −80 as a hard mute)
— a compressed range that keeps every layer audible-but-behind without per-layer
tuning. All four manager operations are constructor-injected delegates, so the
bridge is testable without Godot and `IDisposable` detaches the director.
Verified gap: `UpdateLoop` only restates *existing* loop players keyed
`"{ownerKey}:{cueId}"`, and nothing calls `StartLoop("shelter_acoustic", layer)`
— so every `UpdateLoop` today is a silent no-op, and `PlayCue("acue_*")` hits
the catalog miss path (`LogMissingOnce`). The Core half of the pipeline is live
every day tick; the audible half awaits cue registration and loop starts
(2.4). This is the single most important open item for any Plan 53 follow-up
package.

#### 53.5 AudioManager: buses, DSP, ducking, headless safety

The manager owns 15 buses (`AudioCueCatalog.AudioBusNames`): `Master, Music,
Ambience, SFX, UI, Voice, Alerts, Generator, Ventilation, Radio, Medical,
Surface, Machinery, ShelterSocial, Subterranean`. The director's six layer names
(`generator, ventilation, machinery, radiation, structural, radio`) overlap
four of those buses case-insensitively (`Generator`, `Ventilation`,
`Machinery`, `Radio`); `radiation` and `structural` have **no dedicated
buses**, and the JSON cues' one-shot `bulkhead` bus id has no host counterpart
either — a naming-mismatch detail any loop-start or cue-registration fix must
resolve (map layers and cue buses onto existing buses or add buses; either is
a presentation decision).

Verified DSP shape: a `AudioEffectLowPassFilter` occlusion on the `Surface` bus
(default occluded, tweened cutoff ≈ 0.4 s transitions), `AudioEffectBandPassFilter`
+ Atan-mode `AudioEffectDistortion` on the `Radio` bus (the catalog's "distorted
military communique" character is produced here, not in the samples), pooled
one-shot players with per-cue cooldowns and random resource-path selection,
stream caching, accessibility duck offsets (`_accessibilityDuckDb`), and `_headless`
short-circuits on all playback work — `AudioSelfTest` exercises the manager
without audio hardware. Ducking groups from the JSON (`machinery`, `radio`,
`dialogue`) are authored intent; the manager's ducking is offset-based; the
per-group routing from JSON ducking_group to manager behavior is part of the
same open registration work as 53.4 (UNVERIFIED (map text) that a per-group
duck table binds JSON groups to bus behavior today).

`ShelterAudioController` (208 lines), for completeness, is the *other* shelter
sound loop: it subscribes to `PowerGridSystem.OnPowerChanged/OnTickSummary` and
`StartingLevelSystem.OnStateChanged`, runs a `ReactiveAmbienceEvaluator`, keeps
generator/ventilation loops truthful to infrastructure, and plays exactly one
alert cue on air-filter hazard transitions. It shares `AudioManager` but not
state with the director — two complementary ambience authorities on one bus
topology, both presentation-only by contract.

#### 53.6 Headless and CI safety

Because the director is engine-free and the bridge's delegates are injectable,
the whole acoustic chain is computable in CI: `ShelterAcousticDirectorTests`
(4 cases) drive facts and assert profiles/layers; the shared integration tests
construct facts for inner-vault and deep-excavation rooms and assert the save
cycle around them; `AudioManager`'s `_headless` guard plus `AudioSelfTest` cover
the host half. No Godot runtime session is needed to verify director semantics —
consistent with the repo's 15-FPS rule being unnecessary here.

#### 53.7 Plan 53 test matrix (focused)

| Test file | Cases | Covers |
|---|---|---|
| `ShelterAcousticDirectorTests.cs` | 4 | profile selection, layer math, one-shot queue |
| `Plans50_53_SharedIntegrationTests.cs` (P53 legs) | shared | facts→snapshot inside the cross-plan cycle |
| `AudioSelfTest` / `AudioSettingsRecoveryTests` / accessibility suites | (host legs) | bus setup, recovery, accessibility offsets |

#### 53.8 Restraint notes

- Do not persist acoustic state; the reconstruct-from-live-state contract is
  what makes save/load and headless modes free.
- Do not add gameplay reads of the snapshot (e.g., "loud shelter attracts
  raids") without a foreman signature — the director is a one-way projection
  by invariant.
- The cue-registration fix (53.4) must map all 11 JSON ids and the three bus
  names the manager does not define — the continuous layers `radiation` and
  `structural`, plus the one-shot `bulkhead` bus — in one reviewed change; a
  partial mapping would leave silent layers or dropped one-shots that look
  like bugs.

#### 53.9 Chapter summary (Plan 53)

| Concern | Owner | Status |
|---|---|---|
| Cue + mix profile definitions | `shelter_audio_cues.json` | VERIFIED (data), 11 cues / 4 profiles |
| Fact→snapshot evaluation | `ShelterAcousticDirector` | VERIFIED (code); deterministic except 2 seeded spontaneous draws |
| Snapshot→playback translation | `ShelterAcousticBridge` | VERIFIED (code); loop-start + catalog-registration gap open |
| Bus topology, DSP, pooling, headless | `AudioManager` | VERIFIED (code), 15 buses |
| Power/ventilation ambience (separate) | `ShelterAudioController` | VERIFIED (code), does not consume director |
| Persistence | none (by design) | VERIFIED absence |

---

### Chapter 54 (of this map) — The Interaction Diagram, Expanded into Dependency Specifications

The original map's ASCII diagram compresses eight causal chains into one picture.
This chapter restates each arrow as a dependency spec: producer, consumer,
transport, contract, and current wiring status. Where the diagram's arrow does
not match verified ownership, the spec corrects it (this is the corrected
diagram the original could not verify in 2026-09-06).

#### 54.1 Corrected diagram

```text
[ExpeditionVehicleSystem]  ── stock profile ──►  [VehicleGarageSystem.DecorateProfile]
        ▲   owned by: vehicle data authority             │ decorated profile
        │ definitions/ownership                          ▼
   [vehicles.json / armor grades]            [ExpeditionSystem travel + breakdown roll]
                                                 │           │
                                     wear accrual │           │ stranded/immobilized
                                    (RecordTripWear)          ▼
                                                 │   [VehicleGarageSystem recovery
                                                 │    missions: register→advance→complete]
                                                 ▼
                                        [garage wear state] ──► vehicle_garage save section

[Faction standing authorities (holdfast/diplomacy)] ── posture (caller-owned) ──►
        [ShelterEspionageSystem]  ── daily tick ──► sleeper leaks / sabotage incidents
                ▲     counter-intel score (SetSecurityCounterIntelScore)
                │     dead drops: spawn(>120 counter-intel) ──► InterceptDeadDrop
                │     intel points: +25 turn, +2/day turned, +yield intercept
                └── NO VERIFIED UI CONSUMER (FactionDetailPanel/DailyBriefingModal
                    bind other authorities) ──► faction_espionage save section

[Campaign events: raids, casualties, betrayals]  ── caller maps trigger tags ──►
        [SurvivorMentalHealthSystem]
                │  stress + trauma floors
                │  crisis at ≥750 gated by crisis threshold (750–950)
                │  therapy: quiet room / support circle / journal catharsis
                │  journal_entry_key strings (keys authored; journal binding unverified)
                ▼
   [SleepNarrativeProjection] ── sleep beats ──► sleep/needs presentation
                │
                └──► survivor_mental_health save section
        (coexists, no delegation, with CombatTrauma/SomaticFlashback/GuiltInsomnia
         on the Phase 0 surface and MentalHealthCrisisSystem on the shelter batch)

[Shelter systems: power, ventilation, pumps, radiation, structure, radio, room]
        │  AcousticSimulationFacts  (NO VERIFIED production caller — default facts)
        ▼
[ShelterAcousticDirector] ── AcousticSnapshot ──► [ShelterAcousticBridge]
        ▼
[AudioManager buses/DSP]  ── (loop-start + cue-registration gap) ──► speakers
        ▲
[PowerGridSystem + StartingLevelSystem] ── events ──► [ShelterAudioController]
        (separate ambience lifecycle, same buses, no director dependency)
```

#### 54.2 Edge specifications

**E1. Stock profile → decoration (Plan 50, hard dependency).**
Producer `ExpeditionVehicleSystem.CreateExpeditionProfile`; consumer
`VehicleGarageSystem.DecorateProfile`; transport: direct in-process call from
`ExpeditionHostSession` (production) or CLI probes. Contract: decorator mutates
only additive fields; read-only over persisted garage state; zero-RNG. Status:
WIRED. Failure mode: absent garage (null) → stock profile proceeds unchanged
(`Garage?.DecorateProfile`).

**E2. Decorated profile → travel math (Plan 50, hard dependency).**
Producer: garage-decorated `ExpeditionVehicleProfile`; consumer:
`ExpeditionSystem` start/estimate paths; contract: speed/fuel/cargo/breakdown
fields are consumed exactly as decorated (clamped [0,1] on breakdown chance);
estimate and execution share the discrete-step math. Status: WIRED; parity
characterized by the 51–54 report's expedition leg.

**E3. Travel ticks → wear (Plan 50, hard dependency).**
Producer: expedition execution; consumer: `RecordTripWear`; contract: km and
roughness per trip segment; deterministic permille accrual; immobilization at
1000. Status: WIRED at the API level; the exact production call-site cadence
belongs to the expedition execution owner (call sites verified in tests and CLI
probes; UNVERIFIED (map text) that every production sortie path calls it — the
wear ledger shows real values only if callers do).

**E4. Immobilization → recovery mission (Plan 50, player-gated).**
Producer: garage state; consumer: same system's mission registry; transport:
player commands via panel/CLI; contract: one mission per vehicle, 120 ticks,
explicit completion, ≤ 800 wear on return. Status: WIRED.

**E5. Faction posture → counter-intel score (Plan 51, caller contract).**
Producer: whatever owns shelter security posture; consumer:
`SetSecurityCounterIntelScore`; contract: 0..1000 clamp, caller-owned cadence.
Status: SEAM EXISTS, production pusher UNVERIFIED (map text) — the score sits at
its default 100 unless a caller sets it, which gates dead-drop spawns (needs
>120) *off* by default. This is a quiet, important integration fact: with the
wire as shipped, spontaneous dead drops do not occur in a default game.

**E6. Daily tick → espionage progression (Plan 51, hard dependency).**
Producer: `Main.TickPlans50To53(currentDay)`; consumer:
`ShelterEspionageSystem.TickDay(day, inventory)`; contract: once per day, no
internal day guard (unlike Plan 52) — the host owns exactly-once. Status: WIRED.

**E7. Campaign events → trauma infliction (Plan 52, caller contract).**
Producer: raid/casualty/betrayal authorities; consumer: `AddStress` /
`InflictTrauma`; contract: callers map events to catalog `trigger_tags` and ids;
system never crawls events itself. Status: SEAM EXISTS; production event
mapping UNVERIFIED (map text) — verified call sites are tests. Consequence:
with the wire as shipped, records are created lazily on first interaction and
traumas arrive only if a caller inflicts them.

**E8. Daily tick → mental-health progression (Plan 52, hard dependency).**
Producer: `TickPlans50To53`; consumer: `SurvivorMentalHealthSystem.TickDay`
(monotonic guard). Status: WIRED.

**E9. Mental-health record → sleep narrative (Plan 52, verified consumer).**
Producer: `SurvivorMentalHealthRecord`; consumer: `SleepNarrativeProjection`
(constructor-injected system); contract: read-only classification of
insomnia/trauma into sleep beats. Status: WIRED on the needs/sleep wave
(Plan 177 tests).

**E10. Crisis → journal (Plan 52, authored keys only).**
Producer: crisis definitions' `journal_entry_key`; consumer: journal layer.
Status: KEYS AUTHORED, binding UNVERIFIED (map text) — no consuming
`JournalSystem` type found; treat as an open seam, not a dependency.

**E11. Shelter facts → acoustic director (Plan 53, caller contract).**
Producer: power/ventilation/pump/radiation/structure/radio/room systems;
consumer: `UpdateSimulationFacts`; contract: full-facts object replace
(not a merge). Status: SEAM EXISTS, production caller ABSENT (verified) — the
live host evaluates default facts (inner vault, zero loads). The snapshot is
therefore currently constant per session except spontaneous draws.

**E12. Daily tick → snapshot dispatch (Plan 53, hard dependency).**
Producer: `TickPlans50To53`; consumer: `ShelterAcousticBridge.SyncAcoustics` →
`AudioManager` delegates. Status: WIRED; audible output blocked by the
registration gap (2.4, 53.4).

**E13. Power/ventilation events → controller ambience (separate axis).**
Producer: `PowerGridSystem`, `StartingLevelSystem`; consumer:
`ShelterAudioController`; contract: event-subscription lifecycle, detached on
session swap; one filter-hazard alert. Status: WIRED (independent of E11/E12).

**E14. Cross-plan: espionage betrayal → trauma (diagram's
"Critical Casualties / Betrayal" arrow).**
The map draws trauma fed by "Critical Casualties / Betrayal" from the
expedition/espionage column. Verified reality: no code connects
`ShelterEspionageSystem` incidents (or unmasked sleepers) to `AddStress`/
`InflictTrauma`. Status: DESIGN INTENT ONLY (UNVERIFIED (map text)); the honest
dependency today runs through *callers* that own both facts (E7). Any future
wiring must decide which authority emits the betrayal fact — that is a foreman
decision per rule 10.

#### 54.3 Shared infrastructure edges

- **S1. Save hub:** all three persistent sections meet at
  `Main.CaptureSection(sectionName, payload)` and the registry block (2.6).
  One write path, three section names, no cross-writes.
- **S2. RNG manager:** one `ICampaignRngManager` per campaign day; four plan
  forks + the psychology-action fork (`MentalHealthCrisisSystem`) + the
  expedition stream (breakdown rolls) all derive from the same master seed
  (`StableHash` derivation, 3.5). Adding a fork never shifts another fork's
  seed — the property the dot/snake naming discipline in `CampaignStreamIds`
  protects.
- **S3. Integrity pipeline:** four catalogs, two integrity vocabulary tables,
  one utilization scanner (2.10). Adding JSON fields without consumers is a
  *visible* event in data-integrity selftests.
- **S4. Dirty-flag flush:** three dirty flags, one `FlushPlans50To53`, ordered
  garage → espionage → mental health; acoustic state needs no flush (by
  design).

---

## Part VI — Cross-System Matrix and Emergent-Consequence Design

### 6.1 The full cross-system matrix

Rows are producers of pressure; columns are systems that receive it. Each cell
names the verified transport (or the seam that exists without a verified
producer). "—" means no verified relationship.

| Producer ↓ / Receiver → | VehicleGarage | ShelterEspionage | SurvivorMentalHealth | ShelterAcousticDirector |
|---|---|---|---|---|
| **Expedition travel** | wear via `RecordTripWear` (E3); immobilization | — | — | — |
| **Vehicle breakdown** | recovery missions (E4) | — | — | — |
| **Garage service/economy** | scrap/parts bills (atomic) | — | — | — |
| **Faction posture owners** | — | `SetSecurityCounterIntelScore` (E5) | — | — |
| **Espionage incidents** | — | self (ledger) | — | — |
| **Campaign casualty/betrayal events** | — | — | `AddStress`/`InflictTrauma` (E7, caller contract) | — |
| **Mental-health record** | — | — | self (floors, crises) | — |
| **Sleep domain** | — | — | reads record (`SleepNarrativeProjection`) | — |
| **Power grid / ventilation / pumps** | — | — | — | facts via `UpdateSimulationFacts` (E11, caller absent) |
| **Structural stress** | — | — | — | layers + spontaneous one-shots |
| **Radio state** | — | — | — | 600/50 attention layer |
| **Room/zone state** | — | — | — | mix profile pick |
| **Duty roster (labor)** | `install_labor_ticks` authored, unconsumed | — | productivity penalty read model | — |

Read diagonally, the matrix shows the wave's real integration story: **every
strong edge is intra-system or via the shared wire (tick/flush/save)**; every
cross-system edge is a *caller contract* seam, four of which (E5, E7, E10, E11)
await production producers or consumers. That is not incompleteness by accident
— it is the repo's discipline of not letting a new system reach into another's
state without a signed package.

### 6.2 Emergent consequences the data already implies

These are consequences *derivable from verified data and math*, written
restrained and concrete. None requires new code; each is a play pattern the
current numbers produce once callers feed the seams.

1. **The grease economy (Plan 50 × economy).** A truck wearing the flatbed
   (wear ×1.10) and supercharger (×1.25) multiplies to ×1.375 wear — 1,000 km
   of rough route produces ~2,750 chassis permille of abuse, nearly three
   clamp-widths. The tripwire itself stops the clock: the truck immobilizes
   once at 1000 and the route's remaining wear goes unbilled until service
   releases it. `scrap_metal` stops being a crafting
   residue and becomes the fleet's lifeblood: chassis service alone consumes it
   at 1 per 50 permille, and every armor reforge bills more. Players will feel
   maintenance as a *route-planning* decision (roughness multiplier min 0.5
   floor rewards smooth corridors), which is exactly the overland-logistics
   fantasy Plan 50 authored.
2. **The bullbar compromise.** Because protection is one slot, the bullbar
   (wear ×0.85, rad +50) and the lead cab (rad +400, wear ×1.05) are mutually
   exclusive. A radiation-heavy corridor run forces a choice: plate up and
   service more, or shrug off wear and eat the rads. Emergent variety, zero new
   content.
3. **Recovery is a day-cost, not a resource-cost.** 120 required ticks and
   tick-based advancement mean a stranded vehicle is out of the fleet for real
   calendar time even with fuel in the tank. Fleets of one feel this hardest —
   the data quietly argues for a two-vehicle shelter once expeditions go long.
4. **Depleted plates still drag (Plan 50 CF-P6).** Armor mitigation dies at
   integrity 0 but speed/fuel deltas persist — a worn plate is dead weight.
   Reforge bills create a natural "service before the deep run" ritual, and the
   foundry purity stamp (Poor 850bp .. Exceptional 1200bp) ties vehicle readiness
   to foundry quality: a shelter with an exceptional-purity foundry fields
   measurably tougher trucks. Cross-plan consequence, verified math both sides.
5. **Dead drops in culverts and silo cavities (Plan 51 × map).** The four
   templates anchor intel pickups to drainage culverts, petrified tree knots,
   pylon bases, and silo drainage cavities — infrastructure the expedition map
   already names (`loc_recovery_yard`, `loc_grain_silo`, `loc_radio_relay_mast`,
   `loc_diesel_tank_farm`). Fieldwork therefore routes players past *industrial
   ruins*, not faction camps: espionage stays quiet, logistic, and slightly
   melancholic — the restrained tone the wave requires. The 3-day expiry on the
   40-intel silo cavity drop makes high-yield work a scheduling puzzle.
6. **The turned agent's quiet dividend (Plan 51).** +25 once, +2/day forever,
   versus an unturned sleeper's −(up to 2 scrap)/day leak risk. Over a 60-day
   arc the turned asset is worth ~145 intel and stops the bleeding; the math
   makes recruitment strictly better than surveillance-only play, which is the
   humane message the system wants: turn people, don't just catch them.
7. **Counter-intel is a threshold game.** 120 unlocks discovery, 150 enables
   prevention, and prevention strength *is* the score. A shelter that invests
   nothing sits at 100: sabotage still gets attempted (15% gates don't check
   the score) and always lands. Security investment flips the fraction of
   attempts intercepted — a legible, honest defense curve.
8. **Night terrors have a schedule (Plan 52 × sleep).** `trauma_nightmares`'
   400‰ daily insomnia roll against `SleepNarrativeProjection`'s beat
   classification means a nightmare-afflicted survivor is restless most nights
   *and* carries a 180 floor — the projection can narrate the same week of bad
   nights the math guarantees. When the crisis threshold finally crosses at 850,
   shift refusal (productivity −1000 for 2 days) lands on the roster like a
   fact, not a punishment: the duty board simply shows a gap.
9. **Hoarding as inventory rumor (Plan 52).** Crisis-hoarding's description
   ("concealment of ration biscuits and medical bandages in bunk slats") plus
   its low 350 penalty make it the mildest crisis — the design invites a future
   presentation where the *only* observable is a small inventory drift and a
   journal key, handled by a quiet conversation rather than confiscation. The
   data authors restraint; presentations should keep it.
10. **Pipes ticking under load (Plan 53 × shelter).** The machinery layer is
    `waterPumpLoadPermille` verbatim, and the pump cue's description is piston
    chatter and sump drainage. A shelter running night irrigation therefore
    *sounds* different at 02:00 — not because anyone wrote a night script, but
    because the facts are honest. The same is true of the geiger layer's 10 mSv
    saturation: past the ceiling the shelter sounds no *louder*, only no safer.
11. **The deep room dampens everything (Plan 53).** `acue_prof_deep_excavation`
    cuts ~14.5 kHz of top end and −6 dB relative to surface. A survivor walking
    from the gate to the stope hears the shelter close behind them — pure
    consequence of one profile pick, no scripted transition.
12. **Unread radio pulls attention (Plan 53).** The 600-vs-50 radio layer means
    the shelter's soundscape literally leans toward the set when a broadcast
    waits. Combined with the radio bus's band-pass + distortion DSP, intercept
    nights are *texturally* distinct before any content plays.

### 6.3 Consequence routing rules (for future packages)

- A consequence is *legal* only if every step of its causal chain is an
  authority-to-consumer edge from Part IV/54 — no new ledgers, no polling
  another system's private state (rule 5).
- Presentation consequences (audio, journal, briefing copy) may be added freely
  behind existing seams; gameplay consequences (resources, survival odds) need
  the receiving authority's owner to expose a command or event first.
- Tone check for every new consequence string: concrete, domestic, restrained,
  fictional (2.3's I52.6, 51.5's summaries are the calibration samples).

---

## Part VII — Verification and Acceptance

### 7.1 Per-plan test matrices (consolidated)

**Plan 50 — Vehicle Garage**

| Layer | Target | Cases (file count) | Kind |
|---|---|---|---|
| Core unit | `VehicleGarageSystemTests.cs` | 6 | install/uninstall, deltas, wear |
| Core integration | `Plan50VehicleGarageIntegrationTests.cs` | 5 | catalog→decorate→profile |
| Armor unit | `VehicleArmorGradesTests.cs` | 5 | grades, stamping, bands |
| Armor deep | `Plan213VehicleArmorGradeTests.cs` | 21 | CF-P6 matrix |
| Cross-plan | `Plans50_53_SharedIntegrationTests.cs` (P50 legs) | shared | boot/tick/save |
| Host CLI | `HostCli.VehicleGarage.cs` probes | — | headless decorate/legacy probes |

**Plan 51 — Faction Espionage**

| Layer | Target | Cases | Kind |
|---|---|---|---|
| Core unit | `ShelterEspionageSystemTests.cs` | 6 | enroll/investigate/turn/tick |
| Cross-plan | `PlanE1_29VehicleEspionageTests.cs` | 3 | espionage×vehicle premises |
| Cross-plan | `Plans50_53_SharedIntegrationTests.cs` (P51 legs) | shared | boot/tick/save |
| Save contract | `VersionReportContractTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | legs | section presence, corruption/migration |

**Plan 52 — Survivor Mental Health**

| Layer | Target | Cases | Kind |
|---|---|---|---|
| Core unit | `SurvivorMentalHealthTests.cs` | 14 | floors, crises, therapy, normalization, tick guard |
| Consumer | `SleepNarrativePhantomPainTests.cs`, `Plan177SleepNarrativeProjectionTests.cs` | legs | projection contract |
| Cross-plan | `Plans50_53_SharedIntegrationTests.cs` (P52 legs) | shared | boot/tick/save |
| Save contract | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | leg | store corruption/migration |

**Plan 53 — Acoustic Director**

| Layer | Target | Cases | Kind |
|---|---|---|---|
| Core unit | `ShelterAcousticDirectorTests.cs` | 4 | profiles, layers, one-shots |
| Cross-plan | `Plans50_53_SharedIntegrationTests.cs` (P53 legs) | shared | facts→snapshot in cycle |
| Host | `AudioSelfTest`, `AudioSettingsRecoveryTests`, accessibility suites | legs | buses, recovery, duck offsets |

Case counts are `[Fact]`/`[Theory]` declarations counted 2026-09-25; theories
expand to more executed cases. Per `TEST_POLICY.md`, any focused re-run should
take the specific file(s) for the package being changed — never the full suite.

### 7.2 Acceptance gates (per plan)

A Plan 50–53 package is *integrated* only when all of its applicable rows hold:

| Gate | 50 | 51 | 52 | 53 |
|---|---|---|---|---|
| Core authority compiles engine-free in `Ashfall.Core` | ✔ (verified) | ✔ | ✔ | ✔ |
| Catalog loads through its loader; integrity vocabulary accepts fields | ✔ | ✔ | ✔ | ✔ |
| Focused Core tests green (`scripts/run_test.sh <files>`) | ✔ | ✔ | ✔ | ✔ |
| Save section captured/restored through the hub (corruption leg) | ✔ | ✔ | ✔ | n/a (no save) |
| Legacy absence → safe defaults | ✔ | ✔ | ✔ | n/a |
| Deterministic under fixed seed; no `System.Random` | ✔ (no draws) | ✔ | ✔ | ✔ (2 seeded draws) |
| Host wire: ensure/flush/tick present and idempotent per day | ✔ | ✔ | ✔ | ✔ |
| Presentation binds the live system (not a parallel one) | ✔ panel | ✖ unbound (2.2) | ✖ indirect (2.3) | ✖ partial (2.4) |
| Headless/selftest path exists | ✔ CLI | ✔ | ✔ | ✔ |

The three unchecked presentation cells are the wave's honest acceptance debt —
recorded here so no future audit has to rediscover them.

### 7.3 The gate ladder (how to verify a change here)

1. **Static:** grep the touched system's id string (`vehicle_garage`,
   `shelter_espionage`, `survivor_mental_health`, `shelter_acoustics`) to find
   every consumer before renaming anything — the fork strings are replay-
   bearing (3.5).
2. **Focused tests:** the plan's unit file first, alone; then the shared
   integration file; then the save-contract leg if persistence changed.
   `bash scripts/run_test.sh <file>` per `TEST_POLICY.md`; builders stay under
   100 cases.
3. **Data integrity:** if a JSON catalog changed, run the data-integrity
   selftest target so utilization rows re-scan (2.10).
4. **Godot headless:** only when the change touches the Godot runtime path
   (panel, bridge, manager); 15 FPS target per repo rules; the audio path's
   `_headless` guard makes acoustic changes testable without a session.
5. **Ledger:** only the foreman or named integrator updates
   `INTEGRATION_PLANS.md`/`KNOWN_DEBT.md`; builders hand off per
   `AI_AGENT_WORKFLOW.md`.

### 7.4 Rollback strategy

- **Code:** each system is one Core file + one host wire block + (for 50–52)
  one 35-line store facade; a revert is proportionally small and cannot strand
  the others because the wire partial constructs each independently
  (`SetupPlans50To53` calls four independent ensure methods).
- **Saves:** sections are independent envelopes; a rollback that stops writing
  one section leaves the other two intact, and absent sections default safely
  (2.6). Never hand-edit saved section files; they are checksummed and the
  stores reject tampering.
- **Data:** catalog reverts are safe downward (fields the new code reads are
  optional-defaulted in the DTOs — the CF-P6 comment in
  `VehicleCustomizationRecord` is the pattern); upward reverts need the
  integrity selftest to confirm no authored-but-unconsumed regressions.
- **Acoustic:** rollback is trivial by construction — no persistence, thin
  bridge, `_headless` guards.

### 7.5 Known open items (acceptance debt ledger, as of 2026-09-25)

1. `GarageDetailPanel` name does not exist; `VehicleGaragePanel` is the panel
   (documentation fix only — this expansion is that fix).
2. Plan 51 live presentation unbound; `FactionDetailPanel`/`DailyBriefingModal`
   bind other authorities. Needs a signed UI package; read model is ready.
3. Plan 52 live presentation indirect (`SleepNarrativeProjection` only);
   `SurvivorDetailPanel`/`AfflictionsPanel` bind other sessions.
4. Plan 53 cue registration + loop starts (2.4, 53.4) and the manager-missing
   bus names (layer buses `radiation` and `structural`; cue bus `bulkhead`).
5. `UpdateSimulationFacts` production caller (E11) — without it the director's
   mix is constant.
6. Counter-intel score production pusher (E5) — without it dead drops never
   spawn (score defaults to 100 < 120).
7. Trauma infliction production event mapping (E7) — without it no records
   gain traumas outside tests.
8. `install_labor_ticks` consumption (duty-roster premise audit required).
9. `JournalSystem` binding for authored `journal_entry_key` values (E10).
10. Fork-id promotion into `CampaignStreamIds` constants, if desired, keeping
    exact strings (3.5).

---

## Part VIII — Appendices

### Appendix A — Glossary

| Term | Meaning in this map (verified usage) |
|---|---|
| **Acoustic layer** | One named continuous sound channel in an `AcousticSnapshot` (`generator`, `ventilation`, `machinery`, `radiation`, `structural`, `radio`); intensity 0..1000. |
| **Armor band** | Static condition class of a fitted plate: `none / depleted / critical / worn / nominal` from `ArmorConditionBand`. |
| **Breakdown (vehicle)** | Mid-route transit failure rolled by `ExpeditionSystem.TickHours` against `vehicleBreakdownChancePerTick` on the *expedition* RNG stream — distinct from immobilization. |
| **Breakthrough** | A seeded therapy-roll trauma resolution; increments `totalCatharsisBreakthroughs` and relieves 150 stress. |
| **Capture/restore** | The JSON round-trip deep-copy pair every Core system exposes for persistence (`CaptureState` / `RestoreState`). |
| **Catharsis** | The wave's name for trauma resolution and its counter; resilience is expressed as a count, never a personality score. |
| **Checksummed envelope** | `SaveStoreHub.Checksummed<T>` store shape: versioned payload + checksum, `allowLegacyBareState: false` for these sections. |
| **Crisis** | A time-boxed non-stigmatizing behavioral event (5 authored) triggered at stress ≥ 750 and gated by the crisis's own 750–950 threshold. |
| **Dead drop** | A physical intel pickup generated from an `fdrop_*` template; expires silently; pays intel once on interception. |
| **Decoration** | The Plan 50 pattern of mutating an `ExpeditionVehicleProfile` additively at build time instead of duplicating vehicle state. |
| **Dirty flag** | Host-side bool (`_vehicleGarageDirty`, ...) set on mutation, cleared only after a successful `CaptureSection` flush. |
| **Fact (acoustic)** | One field of `AcousticSimulationFacts`; the complete typed input of the director. |
| **Fork** | A deterministic sub-stream from the campaign RNG manager (`Fork(streamId, day, actionIndex)`); seed = `masterSeed × 31337 + StableHash(id) × 1009 + day × 37 + actionIndex`. |
| **Floor (stress)** | `min(750, Σ trauma floors)`; relief cannot cross it. |
| **Fork string** | The exact stream id passed to `Fork(...)`; replay-bearing, must never be renamed casually. |
| **Immobilization** | Garage-owned state at any component = 1000 permille; blocks modification; clears only via service (all < 900) or recovery completion. |
| **Intel points** | The espionage system's single cumulative score (+25 turn, +2/day turned, +yield intercept). |
| **Mix profile** | Authored space acoustics (`acue_prof_*`): reverb room size, lowpass cutoff, attenuation. |
| **One-shot (acoustic)** | A fire-once cue, either semantic (`TriggerSemanticCue`) or spontaneous (seeded draws). |
| **Recovery mission** | `recov_*` garage mission, 120 ticks, explicit completion, wear clamped ≤ 800 on return. |
| **Sleeper agent** | A survivor with a `SleeperAgentRecord`; hidden until `isIdentified`; may be turned into a double agent. |
| **Snapshot** | `AcousticSnapshot`: active profile + layer intensities + pending one-shots; the director's entire output. |
| **Stress** | Per-survivor 0..1000 scalar (default 200) in the `survivor_mental_health` record. |
| **Trauma floor** | Authored per-trauma `stress_floor_permille`; summed (cap 750) into the stress floor. |
| **Turned double agent** | An identified sleeper flipped via `AttemptTurnDoubleAgent`; leaks become intel income. |
| **Utilization row** | `ContentUtilizationScanner` mapping of a JSON file to its loader/system pair; powers unconsumed-field reporting. |
| **Wire partial** | `src/Main.Plans50_53.cs` — the single host construction/save/flush/tick seam for all four systems. |

### Appendix B — ID and stream vocabulary

**Save sections (exact strings):** `vehicle_garage` (`vehicle_garage_save.json`),
`faction_espionage` (`faction_espionage_save.json`), `survivor_mental_health`
(`survivor_mental_health_save.json`). Plan 53 has none.

**RNG fork strings (exact, replay-bearing):** `vehicle_garage`,
`shelter_espionage`, `survivor_mental_health`, `shelter_acoustics` (note plural),
plus the adjacent `psychology` stream action 15 used by
`MentalHealthCrisisSystem` and the `expedition`-owned rolls inside
`ExpeditionSystem`. None of the four is a `CampaignStreamIds` constant today.

**System ids:** `vehicle_garage`, `shelter_espionage`, `survivor_mental_health`
(const `SystemId` on each Core class; also the `systemId` field of each state DTO).

**Slot types:** `cargo`, `protection`, `mobility`, `engine`, `utility`
(consts `VehicleGarageSystem.Slot*`).

**Modification ids:** `vmod_expanded_flatbed`, `vmod_lead_lined_cab`,
`vmod_reinforced_bullbar`, `vmod_aux_fuel_rack`, `vmod_traction_cleats`,
`vmod_stretcher_mount`, `vmod_heavy_winch`, `vmod_engine_supercharger`.

**Faction operation ids:** `fop_inventory_leak`, `fop_fuel_sabotage`,
`fop_water_tampering`, `fop_power_interruption`, `fop_misinformation`,
`fop_patrol_theft`, `fop_sleeper_activate`, `fop_convoy_interception`.

**Dead-drop template ids:** `fdrop_culvert_ruins`, `fdrop_hollow_tree`,
`fdrop_rusted_pylon`, `fdrop_abandoned_silo`.

**Trauma ids:** `trauma_combat_shock`, `trauma_survivor_guilt`,
`trauma_claustrophobia`, `trauma_nightmares`, `trauma_radiation_dread`,
`trauma_bereavement`.

**Recovery-action ids:** `therapy_quiet_room` (room `room_bunks`),
`therapy_support_circle` (room `room_main`), `therapy_journal_catharsis`
(room `room_clinic`).

**Crisis ids:** `crisis_withdrawal`, `crisis_hoarding`, `crisis_shift_refusal`,
`crisis_panic`, `crisis_barricade` (journal keys `journal_crisis_*`).

**Acoustic cue ids:** `acue_generator_hum_60hz`, `acue_ventilation_rush`,
`acue_water_pump_clatter`, `acue_geiger_clicks_low`, `acue_geiger_clicks_high`,
`acue_structural_creak`, `acue_falling_dust`, `acue_radio_static_burst`,
`acue_radio_voice_intercept`, `acue_bulkhead_heavy_slam`,
`acue_hydraulic_door_hiss`.

**Mix-profile ids:** `acue_prof_surface`, `acue_prof_airlock`,
`acue_prof_inner_vault`, `acue_prof_deep_excavation`.

**Acoustic bus names (host):** `Master`, `Music`, `Ambience`, `SFX`, `UI`,
`Voice`, `Alerts`, `Generator`, `Ventilation`, `Radio`, `Medical`, `Surface`,
`Machinery`, `ShelterSocial`, `Subterranean`. Director layers without a
same-named bus: `radiation`, `structural`. The JSON cues additionally carry a
`bulkhead` bus id with no host counterpart (one-shots only).

**Missions/incidents/drops runtime id shapes:** `recov_{n}_{vehicleId}`,
`inc_{n}`, `drop_{n}_{templateId}`.

**Referenced external ids (owned elsewhere, referenced here):** vehicle tags
(`road`, `rough`, `quad`, `bike`, `truck`, `hauler`); locations
(`loc_recovery_yard`, `loc_grain_silo`, `loc_radio_relay_mast`,
`loc_diesel_tank_farm`); rooms (`room_bunks`, `room_main`, `room_clinic`,
`room_reading_quiet_room`); foundry purity names (`Poor`, `Standard`, `High`,
`Exceptional` via `FoundryPurityNames`); items (`scrap_metal`,
`mechanical_parts`, `bandage`).

### Appendix C — Scenario walkthroughs (player-visible arcs)

**C.1 "The haul that cost a week."** A hauler truck with flatbed + cleats runs a
rough 120 km corridor. Decoration: cargo 60 kg up, speed ×1.10, fuel ×1.134,
wear ×0.99. Roughness floors at 0.5 only on paved grades; at 0.9 the trip adds
~214 chassis permille each way. Third run of the corridor trips the 1000 line
mid-route — the expedition layer's breakdown roll meanwhile makes the *return*
leg nervy. Back home: immobilized, reason "Critical component catastrophic
failure during overland transit". Register recovery, four days of ticks to 120,
complete (wear clamped to 800), then ceil(800/50) = 16 scrap for the chassis and
8+8 parts for engine/transmission below 900. The truck runs again — heavier,
slower, thirstier than the day it was bought, exactly as authored.

**C.2 "The culvert envelope."** Security score raised to 200 after two supply
depot incidents. Three days later counter-intel finds a concrete drainage culvert
drop (25 intel, 5-day expiry). The player routes a foot sortie through
`loc_recovery_yard`, intercepts on day two, and logs the yield. A week later the
same vigilance unmasks a low-loyalty sleeper (suspicion 45 + skill 8 →
45/2 = 22 plus 40 → threshold 938; the roll lands). Five scrap and a hard
conversation later the agent is turned and pays +2 intel/day. No panel shows
any of this today (2.2) — the arc
is real in Core and save, pending its signed UI package.

**C.3 "The quiet room week."** A raider dies covering the retreat. The caller
inflicts `trauma_survivor_guilt` (floor 200) and adds 300 stress on top of the
record's 200 → 500. Two bad nights (250‰ nightly insomnia rolls both land).
Support circle in `room_main` (−90, counselor −50 more) twice → 500 − 280 =
220, held no lower than the 200 floor; journaling in the clinic on the fifth
day with a counselor (−80 −40, again floor-clamped at 200) rolls the
breakthrough at 140+80 = 220‰ and it lands — the guilt resolves, the floor
dies with it, and the −150 relief takes the record to 50;
`totalCatharsisBreakthroughs` ticks to 1. `SleepNarrativeProjection` narrates
the first full night's sleep. Nothing stigmatized happened; a person was
accompanied.

**C.4 "Night shift in the stope."** Excavation opens `excavation_sector_04`;
the room string's `excavation` token flips the mix profile to deep excavation
(reverb 0.85, 5.5 kHz, −6 dB). The pump draws 700 permille on irrigation nights,
so machinery sits at −13.2 dB; at 03:00 a structural stress spike to 820 rolls
the creak draw and loses, then the dust draw and wins — a soft grit fall under
the pump chatter, seeded, replayable, unscripted. (Audible pending 53.4; in Core
terms the snapshot already contains it.)

### Appendix D — Open questions (for the foreman queue, not for improvisation)

1. **Presentation packages for Plans 51/52** — the read models exist; which
   surface hosts them (faction dossier sections vs a dedicated security tab vs
   briefing entries)? Owner decision required.
2. **Cue registration path for Plan 53** — extend the static `AudioCueCatalog`
   with the 11 JSON ids, or generate a bridge catalog from the JSON at load?
   Generator + `--check` mode is the repo-preferred shape for catalog codegen.
3. **Bus naming** — add `Radiation`/`Structural` buses or map the director's
   layers onto `Ambience`/`Subterranean`? Affects save-irrelevant but
   accessibility-relevant mixing.
4. **Facts producer** — which partial owns composing `AcousticSimulationFacts`
   each tick (power + ventilation + radiation + structure + radio + room are
   five different owners)?
5. **Counter-intel pusher** — guard duty, radio intercepts, or hardening
   research? The score is a caller contract awaiting its caller.
6. **Trauma event mapping** — which campaign event authority maps casualty and
   betrayal facts to `trigger_tags` and calls `InflictTrauma`?
7. **`install_labor_ticks`** — duty-roster integration or formal removal from
   the authored schema (with a scanner note)?
8. **Journal binding** — does a journal authority consume `journal_entry_key`
   strings, and if so which one, or is a new seam required?
9. **Fork-id promotion** — add the four fork strings to `CampaignStreamIds`
   as constants (compile-time safety) while preserving exact strings?
10. **Intel spender** — `totalIntelPoints` has no consumer; economy or faction
    standing owner signs any future spender.
11. **Consolidation review (long-term)** — the four mental-health authorities
    (2.7, 52.8) coexist cleanly; a convergence proposal would need a foreman
    signature, a save-migration plan for four shapes, and a determinism audit.
12. **Garage labor and fuel on recovery** — `requiredFuelUnits` is authored but
    the completion math is tick-based; either enforce fuel at completion (a
    caller contract) or document it as flavor.

---

## Part IX — Reference Appendices

### Appendix E — Public API index (one-line contracts, verified signatures)

**`VehicleGarageSystem`** (`Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`)

| Member | Contract |
|---|---|
| `const string SystemId` | `"vehicle_garage"`; also the state DTO's `systemId` default. |
| `const string SlotCargo/SlotProtection/SlotMobility/SlotEngine/SlotUtility` | The five slot keys; one installed mod per slot per record. |
| `TryGetArmorMaterialQuality? ArmorMaterialQualitySource` | Optional foundry handoff delegate; unset/false → neutral stamp (bp 1000, Standard purity). |
| `Func<string, string?>? VehicleTerrainResolver` | Optional vehicle→terrain classifier; unset keeps pure-Core probes neutral. |
| `VehicleGarageSystem(VehicleGarageCatalog?, ISeededRng?)` | Rng defaults `SeededRng(1337)` (currently unconsumed — 2.1). |
| `void LoadCatalog(VehicleGarageCatalog)` | Replaces the modification index; skips empty ids. |
| `bool HasModification(string)` / `GetModification(string)` / `GetAllModifications()` | Catalog reads; the getter returns null for unknown ids. |
| `void LoadArmorCatalog(VehicleArmorGradeCatalog)` | Replaces grade index + default grade id. |
| `bool HasArmorGrade(string)` / `GetArmorGrade(string)` / `GetAllArmorGrades()` | Grade reads; `HasArmorGrade` false for empty string. |
| `VehicleArmorProfile GetArmorProfile(string)` | Effective plate profile (default "Stock Plating" when absent/depleted/unknown); clamps mitigation ≤ 400, absorption ≤ 500. |
| `static string ArmorConditionBand(int, int)` | `none/depleted/critical/worn/nominal` from integrity and max. |
| `VehicleCustomizationRecord? GetRecord(string)` | Read-only record or null. |
| `bool IsImmobilized(string)` | Convenience over the record flag. |
| `IReadOnlyDictionary<string, VehicleRecoveryMission> ActiveRecoveries` | Live mission map (read-only view). |
| `IReadOnlyDictionary<string, string> GetInstalledSlots(string)` | Slot→mod view; shared empty map when no record. |
| `void DecorateProfile(ExpeditionVehicleProfile?)` | THE decoration seam: cargo `+=`, speed `×(1+Δ)`, fuel `×`, armor second pass incl. breakdown scaling; read-only, zero-RNG. |
| `int AdvanceRecoveries(int deltaTicks)` | Advances all incomplete missions; returns count advanced. |
| `VehicleCustomizationRecord GetOrCreateRecord(string)` | Creates or returns the live record; throws on empty id. |
| `bool HasVehicleRecord(string)` | Record existence. |
| `bool CanInstallArmorGrade(...)` / `InstallArmorGrade(...)` | Full guard set (2.1/50.6) then atomic bill + stamp; refunds displaced scrap at 50%. |
| `bool ReforgeArmorPlate(...)` | Reforge-bill consumption; integrity → max. |
| `bool CanInstallModification(...)` / `InstallModification(...)` | Slot-match + immobilization + materials; atomic bill; slot write. |
| `bool UninstallModification(...)` | Removes slot; refunds `max(1, cost/2)` per line. |
| `float GetEffectiveCargoCapacityDelta(string)` / `GetEffectiveSpeedMultiplierDelta(string)` | Additive folds over installed mods. |
| `float GetEffectiveFuelConsumptionMultiplier(string)` / `GetEffectiveWearRateMultiplier(string)` | Multiplicative folds; floor 0.1. |
| `int GetEffectiveRadiationProtectionPermille(string)` | Additive fold; clamp 0..950. |
| `void RecordTripWear(string, float km, float roughness = 1)` | The wear math (50.2); no-op when immobilized or km ≤ 0. |
| `bool ServiceChassis/Engine/Transmission(string, IPlayerInventoryPort?, int repairPermille, out string)` | Cost formulas (50.3); may clear immobilization. |
| `bool RegisterRecoveryMission(string vehicleId, string locationId, int fuel, out string missionId, out string reason)` | Requires immobilized; one mission per vehicle. |
| `bool AdvanceRecoveryMission(string missionId, int deltaTicks, out bool completed)` | Targeted advance; true on known mission. |
| `bool CompleteRecoveryMission(string missionId, IPlayerInventoryPort?, out string)` | Requires complete; wear ≤ 800; clears immobilization; removes mission. |
| `VehicleGarageState CaptureState()` / `void RestoreState(VehicleGarageState?)` | JSON round-trip deep copy; null restore = factory state. |

**`ShelterEspionageSystem`** (`Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs`)

| Member | Contract |
|---|---|
| `const string SystemId` | `"shelter_espionage"`. |
| `ShelterEspionageSystem(FactionIntelligenceCatalog?, ISeededRng?)` | Rng defaults `SeededRng(4242)`. |
| `void LoadCatalog(FactionIntelligenceCatalog)` | Indexes operations + dead-drop templates. |
| `IReadOnlyDictionary<...> Operations` / `DeadDropTemplates` | Catalog views. |
| `bool EnrollSleeperAgent(string survivorId, string factionId, int initialLoyalty = 600)` | One record per survivor; loyalty clamped. |
| `bool IsSleeperAgent(string)` / `SleeperAgentRecord? GetSleeperRecord(string)` | Hidden-state reads (record is mutable — treat as internal). |
| `void AddSuspicion(string, int amount)` | Evidence add; ignores non-positive; clamp 0..1000. |
| `bool InvestigateSuspect(string, int investigatorSkill, out bool unmasked, out string report)` | Roll math (51.3); clears innocents with a clean report. |
| `bool AttemptTurnDoubleAgent(string, IPlayerInventoryPort?, out string outcome)` | Requires identified + unturned + 5 scrap; loyalty flip; +25 intel. |
| `void SetSecurityCounterIntelScore(int)` | Caller-owned scalar; clamp 0..1000. |
| `void TickDay(int currentDay, IPlayerInventoryPort? inventory = null)` | Drop countdowns, sleeper progression, sabotage gates, spontaneous spawns; **no day guard**. |
| `ActiveDeadDrop? SpawnRandomDeadDrop()` | Uniform template pick; appends to active list. |
| `bool InterceptDeadDrop(string dropId, out int intelPointsAwarded, out string report)` | Single-payout; removes drop. |
| `ShelterEspionageState CaptureState()` / `RestoreState(...)` | JSON round-trip; null = fresh state. |

**`SurvivorMentalHealthSystem`** (`Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs`)

| Member | Contract |
|---|---|
| `const string SystemId` | `"survivor_mental_health"`. |
| `SurvivorMentalHealthSystem(PsychologicalTraumaCatalog?, ISeededRng?)` | Rng defaults `SeededRng(5252)`. |
| `SurvivorMentalHealthState State` | Cloned capture (property form). |
| `IReadOnlyDictionary<...> Traumas / Therapies / Crises` | Cloned catalog views. |
| `void LoadCatalog(PsychologicalTraumaCatalog)` | Normalize ids (trim, ordinal-ignore-case), resolve `crisis_affinity`. |
| `SurvivorMentalHealthRecord GetOrCreateRecord(string, int initialStress = 200)` | Cloned record; throws on empty id. |
| `bool HasRecord(string)` | Record existence (normalized). |
| `int GetStressFloor(string)` | `min(750, Σ trauma floors)`; 0 without record. |
| `void AddStress(string, int permilleDelta, string triggerTag = "")` | Ignores non-positive; triggers crisis path at ≥ 750. |
| `void ReduceStress(string, int permilleDelta)` | Clamped to the trauma floor. |
| `bool InflictTrauma(string, string traumaId, out string reason)` | Duplicate-refusing; adds floor as stress. |
| `bool ResolveTrauma(string, string traumaId, out string reason)` | Removes trauma; +1 breakthrough; −150 stress. |
| `bool PrescribeTherapy(string, string recoveryActionId, bool hasCounselor, out string outcome)` | Always counts a session; reduction + seeded breakthrough roll (52.5). |
| `void TickDay(int currentDay)` | Monotonic guard; crisis countdown, insomnia rolls/decay. |
| `int GetProductivityPenaltyPermille(string)` | Crisis penalty + 200 insomnia; clamp 0..1000. |
| `SurvivorMentalHealthState CaptureState()` / `RestoreState(...)` | Both normalize (unknown ids dropped, dedup, clamp). |

**`ShelterAcousticDirector`** (`Assets/Ashfall.Core/Audio/ShelterAcousticDirector.cs`)

| Member | Contract |
|---|---|
| `ShelterAcousticDirector(ShelterAudioCueCatalog?, ISeededRng?)` | Rng defaults `SeededRng(5353)` (spontaneous draws only). |
| `void LoadCatalog(ShelterAudioCueCatalog)` | Indexes cues + mix profiles. |
| `IReadOnlyDictionary<...> Cues / Profiles` | Catalog views. |
| `void UpdateSimulationFacts(AcousticSimulationFacts)` | Full-facts replace; null → default facts. |
| `void TriggerSemanticCue(string cueId)` | Queues a known cue id for the next evaluation; unknown ids ignored. |
| `AcousticSnapshot EvaluateSnapshot()` | Profile pick + layer math + queue drain + 2 seeded spontaneous draws (53.3). |

**Host facades and adapters**

| Type | Contract |
|---|---|
| `VehicleGarageSaveStore` (src/Host) | `SectionName "vehicle_garage"`, `FileName "vehicle_garage_save.json"`; `SaveStoreHub.Checksummed`, legacy bare rejected; `TryLoad/TrySave/TryCapturePersisted/TryCaptureDirect/TryRestoreDirect`. |
| `ShelterEspionageSaveStore` (src/Host) | Same shape; section `faction_espionage`, file `faction_espionage_save.json`. |
| `SurvivorMentalHealthSaveStore` (src/Host) | Same shape; section `survivor_mental_health`, file `survivor_mental_health_save.json`. |
| `Main` partial `Plans50_53` | `EnsureVehicleGarage/EnsureShelterEspionage/EnsureSurvivorMentalHealth/EnsureShelterAcoustics` (idempotent construction, fork-or-fixed-seed rng, catalog load, restore); `SetupPlans50To53/FlushPlans50To53/TickPlans50To53`; public system accessors; `ShelterAcousticBridge` property. |
| `ShelterAcousticBridge` | `BindDirector`, `SyncAcoustics` (53.4 curve), `IDisposable`; delegate-injected manager ops. |
| `ShelterAudioController` | `Subscribe(PowerGridSystem?, StartingLevelSystem?)`; event-driven loops + one filter-hazard alert; `IDisposable`; `AmbienceEvaluator` exposure. |
| `VehicleGaragePanel` | `Bind(garage, vehicles, inventory)`, `Unbind`, `IsBound`, `LastFeedback`, `OnClose`; command buttons forward Core reasons verbatim. |
| `SleepNarrativeProjection` | Constructor takes `SurvivorMentalHealthSystem` (+optional rng); `Classify(record, hasPhantomPain)` beat classification. |

### Appendix F — Data dictionary (authoritative JSON shapes)

**Common envelope conventions:** every catalog carries `schema_version: 1`;
field names snake_case; list-typed fields are authoritative order (dead-drop
template pick indexes this order through the dictionary's insertion order —
Core preserves load order in the `StringComparer.Ordinal` dictionaries, so the
"uniform" template pick is stable across runs on the same build).

**`vehicle_modifications.json`**

| Field | Type | Constraint (verified consumer behavior) |
|---|---|---|
| `schema_version` | int | 1 |
| `modifications[].id` | string | catalog key; empty ids skipped at load |
| `modifications[].slot_type` | string | must equal the command's slot (case-insensitive compare) |
| `modifications[].compatible_vehicle_tags` | string[] | selector/data-layer gate (Core command does not check — 50.1) |
| `modifications[].install_cost[]` | `{item_id, amount}` | folded into the atomic bill; refund `max(1, amount/2)` |
| `modifications[].install_labor_ticks` | int | authored, unconsumed (50.1, D7) |
| `modifications[].effects.cargo_capacity_delta` | number | additive fold |
| `modifications[].effects.speed_multiplier_delta` | number | additive fold, then `×(1+Δ)`, floor 0.01 |
| `modifications[].effects.fuel_consumption_multiplier` | number | multiplicative fold, product floor 0.1 |
| `modifications[].effects.wear_rate_multiplier` | number | multiplicative fold, product floor 0.1 |
| `modifications[].effects.radiation_protection_permille` | int | additive fold, clamp 0..950 |
| `modifications[].tags` | string[] | data-layer labeling |

**`vehicle_armor_grades.json`** (loader-carried file name; CF-P6): `grades[].id`,
`display_name`, `tier`, `is_default`, `integrity_pool_permille` (stamp base),
`mitigation_permille` (clamped 0..400 at read), `wear_absorption_permille`
(clamped 0..500 at read), `speed_multiplier_delta`, `fuel_consumption_multiplier`,
`compatible_terrain_types[]` (checked against the resolver's terrain),
`install_cost[]`, `reforge_cost[]`; catalog-level `default_grade_id`.

**`faction_intelligence.json`**: `operations[].{id, display_name, target_faction,
operation_class, target_subsystem, suspicion_per_tick, cooldown_days,
detection_difficulty, tags[]}` and `dead_drop_templates[].{id, display_name,
target_location, reward_intel_points, risk_level, expiry_days}` — the tick
currently consumes only template fields of dead drops and none of the operation
rows beyond `fop_inventory_leak` as a label (51.5).

**`psychological_trauma.json`**: `trauma_types[].{id, display_name, description,
stress_floor_permille, insomnia_chance_permille, trigger_tags[], crisis_affinity}`;
`recovery_actions[].{id, display_name, description, required_room,
stress_reduction_permille, daily_resolution_chance_permille, counselor_bonus_permille}`;
`crisis_events[].{id, display_name, description, threshold_stress_permille,
duration_days, productivity_penalty_permille, journal_entry_key}`. Load-time
normalization trims ids/tags, clamps chances to 0..1000, floors to ≥ 0, and
resolves each trauma's `crisis_affinity` to the canonical crisis id.

**`shelter_audio_cues.json`**: `cues[].{id, display_name, bus_id, playback_mode,
base_volume_db, pitch_jitter_permille, ducking_group, description}`;
`mix_profiles[].{id, display_name, reverb_room_size, lowpass_cutoff_hz,
attenuation_db}`. The Core director consumes ids and profile ids; `base_volume_db`,
`pitch_jitter_permille`, `ducking_group`, and the profile DSP numbers are
authored for the presentation layer (currently reachable only after the 53.4
registration work).

**Save state shapes** (`vehicle_garage` / `faction_espionage` /
`survivor_mental_health` payloads): the three state DTOs of 4.2/4.3/4.4 wrapped
in the checksummed envelope; each carries its `systemId` literal, enabling
store-level sanity checks; Plan 52's payload additionally guarantees
normalized records on both write and read (52.9).

### Appendix G — Verified citation index

Everything this expansion cites, with the fact each citation supports.

| Citation | Supports |
|---|---|
| `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` (864 ln) | Plan 50 authority, DTOs, wear math, armor API, recovery model, capture/restore, `SystemId`, slots, `DecorateProfile` (212), armor decoration (224-235), wear (624-652), service (654-748), immobilization clear (750-760), recovery (762-843), `SeededRng(1337)` default (79) |
| `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` (~283-310) | Stock profile build; base breakdown formula `(100−condition)/100 × 0.15 × gear` |
| `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` (77, 97, 442, 676, 751, 773-780) | `vehicleBreakdownChancePerTick` adoption, estimate parity fields, `TickHours` breakdown roll on injected rng |
| `Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs` (340 ln) | Plan 51 authority and all math (51.2-51.6), `SeededRng(4242)` (68) |
| `Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs` (444 ln) | Plan 52 authority and all math (52.4-52.6), normalization, tick guard, `SeededRng(5252)` (47) |
| `Assets/Ashfall.Core/Needs/SleepNarrativeProjection.cs` (40, 58, 61, 140-145) | Plan 52 consumer bridge; beat classification |
| `Assets/Ashfall.Core/Audio/ShelterAcousticDirector.cs` (147 ln) | Plan 53 authority, facts/snapshot DTOs, evaluation rules, spontaneous draws (135-142), `SeededRng(5353)` (41) |
| `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (271-273, 561-563) | Section declarations + file names |
| `Assets/Ashfall.Core/Save/SchemaVersionedEnvelope.cs` (24, 36, 49) | Envelope type existence and shape |
| `src/Host/VehicleGarageSaveStore.cs` / `ShelterEspionageSaveStore.cs` / `SurvivorMentalHealthSaveStore.cs` (35 ln each) | Checksummed facade pattern, section names, legacy bare rejection |
| `src/Main.Plans50_53.cs` (267 ln) | The wire: forks at 42/112/158/204, catalog loads, terrain resolver, restore, capture/flush/tick, bridge bind (222-226) |
| `src/Host/ExpeditionHostSession.cs` (935) | Production decoration call site |
| `src/Host/HostCli.VehicleGarage.cs` (80, 111, 138) | Headless decorate probes |
| `src/UI/VehicleGaragePanel.cs` (635 ln) | Plan 50 panel (50.7) |
| `src/UI/FactionDetailPanel.cs` (155 ln) | Holdfast binding (51.7) |
| `src/UI/DailyBriefingModal.cs` (245 ln) | Briefing report modal contract (51.7) |
| `src/UI/SurvivorDetailPanel.cs` (435 ln) | Survivors binding (2.3) |
| `src/UI/AfflictionsPanel.cs` (488 ln) | Medical binding (2.3) |
| `src/Audio/ShelterAcousticBridge.cs` (77 ln) | Bridge contract and volume curve (53.4) |
| `src/Audio/ShelterAudioController.cs` (208 ln) | Separate ambience lifecycle (53.5) |
| `src/Audio/AudioManager.cs` (1,068 ln; 21-33, 111-160, 371-397, 1021-1057, 609) | 15-bus topology, DSP, PlayCue miss path, loop API, duck offset |
| `src/Audio/AudioCueCatalog.cs` (10-34) | `AudioBusNames`, `AudioCueDef`, static resolution |
| `src/Host/Phase0HostSession.cs` (235-237, 351, 367) | Trauma trio construction surface (2.7) |
| `src/Main.ShelterBatch3.cs` (266-284) | `MentalHealthCrisisSystem` fork + store (2.7) |
| `Assets/Ashfall.Core/Random/CampaignRngStream.cs` (whole) | Fork derivation, manager, capture/restore of positions, no plan-local constants (2.5, 3.5) |
| `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` (519-523, 1187-1191) | Utilization rows for all four catalogs + armor grades |
| `Assets/Ashfall.Core/CatalogIntegrityRules.cs` (305) / `CatalogIntegrityValidator.cs` (465) | Field vocabularies incl. `journal_entry_key`, `bus_id`, `ducking_group` |
| `Assets/StreamingAssets/Data/vehicle_modifications.json` (4,882 B) | 8-mod catalog (50.1) |
| `Assets/StreamingAssets/Data/faction_intelligence.json` (3,884 B) | 8 ops + 4 drops (51.1) |
| `Assets/StreamingAssets/Data/psychological_trauma.json` (5,373 B) | 6 traumas + 3 recoveries + 5 crises (52.1-52.3) |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` (4,738 B) | 11 cues + 4 profiles (53.1) |
| `Assets/StreamingAssets/Data/shelter_rooms.json` (345), `shelter_construction.json` (59) | `room_reading_quiet_room` existence (3.6, 52.2) |
| `docs/PLANS_51_54_INTEGRATION_REPORT.md` (whole, 2026-09-10) | Numbering collision + adjacent expedition evidence (2.8) |
| `Ashfall.Core.Tests/...` (the nine files of 4.1) | Test matrices (7.1), case counts |
| `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`, `SomaticFlashbackSystem.cs`, `GuiltInsomniaSystem.cs` | Trio constants, events, save DTOs (2.7, 52.8) |
| Absence searches: `garage_breakdown` (0 hits), `acue_` in src (0 hits), `UpdateSimulationFacts` src callers (0), `ShelterEspionageSystem` src consumers outside wire (0), `SurvivorMentalHealthSystem` src consumers outside wire (0), `GarageDetailPanel` (0 hits) | The verified absences of 2.1-2.4, 2.7, 53.4, E5, E7, E11 |

### Appendix H — Map-vs-tree correction ledger (consolidated)

Every place the 2026-09-06 map layer (top of this file) and the 2026-09-25 tree
disagree, in one table. The original text above is intentionally unmodified;
this table is the correction of record.

| # | Map says | Tree says (verified) | Severity |
|---|---|---|---|
| 1 | Plan 50 presentation node is `GarageDetailPanel` | Type does not exist; `VehicleGaragePanel` (635 ln) + `Main.VehicleGarage.cs` + `HostCli.VehicleGarage.cs` | Cosmetic (doc name) |
| 2 | Plan 50 RNG fork `garage_breakdown` | String absent repo-wide; fork is `vehicle_garage`; garage draws nothing (deterministic); breakdown rolls live in the expedition stream | Moderate (determinism narrative) |
| 3 | Plan 51 presentation = `FactionDetailPanel` + `DailyBriefingModal` | Both exist; neither binds `ShelterEspionageSystem`; zero live consumers outside the wire | Structural (acceptance debt #2) |
| 4 | Plan 52 presentation = `SurvivorDetailPanel` + `AfflictionsPanel` | Both exist; they bind Survivors/Medical sessions; live consumer is `SleepNarrativeProjection` | Structural (acceptance debt #3) |
| 5 | Plan 53 is "zero RNG, pure state evaluation" | Deterministic except two seeded spontaneous one-shot draws (creak 30% > 600, dust 40% > 800) | Moderate (determinism narrative) |
| 6 | Plan 53 fork `shelter_acoustic` | Fork string is `shelter_acoustics` (plural) | Cosmetic (string) |
| 7 | Diagram: `Recovery Mission ──► [ExpeditionSystem]` | Recovery missions are garage-owned end-to-end; `ExpeditionSystem` only consumes decorated profiles | Moderate (ownership) |
| 8 | Diagram: `Journal Catharsis ──► [JournalSystem]` | `journal_entry_key` strings authored; no consuming journal type found | Structural (open seam D8) |
| 9 | Diagram: quiet-room anchored to `room_reading_quiet_room` | `therapy_quiet_room` requires `room_bunks`; `room_reading_quiet_room` exists elsewhere in room data | Cosmetic |
| 10 | "Legacy Compatibility: absent sections default..." | Holds via absence→default; checksummed stores additionally *reject* legacy bare payloads (`allowLegacyBareState: false`) | Clarification |
| 11 | RNG forks "from `ISeededRng`" | Forks are `ICampaignRngStream.Fork(string)` long-lived per system; none are `CampaignStreamIds` constants | Clarification |
| 12 | (implied) acoustic director state is live | True, and additionally `UpdateSimulationFacts` has no production caller → live mix is default-facts | Structural (acceptance debt #5) |

### Appendix I — Quick-reference: what to run when touching each plan

```text
Plan 50 (garage/armor/expedition profile):
  bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs
  bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs
  (+ armor files if grades touched; + shared integration file if the wire changed)

Plan 51 (espionage):
  bash scripts/run_test.sh Ashfall.Core.Tests/Factions/ShelterEspionageSystemTests.cs
  (+ PlanE1_29VehicleEspionageTests.cs for cross-plan premises)

Plan 52 (mental health):
  bash scripts/run_test.sh Ashfall.Core.Tests/Needs/SurvivorMentalHealthTests.cs
  (+ SleepNarrative files if the projection contract changed)

Plan 53 (acoustic):
  bash scripts/run_test.sh Ashfall.Core.Tests/Audio/ShelterAcousticDirectorTests.cs
  (+ shared integration file; Godot headless audio selftest only if the
   manager/bridge path changed)

Any save-shape change:
  the matching ComprehensiveSaveStoreCorruptionAndMigrationTests leg
  (through scripts/run_test.sh with that file)

Any catalog JSON change:
  the data-integrity selftest target so utilization rows re-scan
```

Nothing above authorizes running the full suite; `TEST_POLICY.md` and
`AGENTS.md` targeted-testing rules still govern.

---

*End of the core expansion (Parts I–IX). The design-analysis appendices and the
integration playbooks follow in Parts X–XI; the document's single closing note
sits at the true end of the file.*

---

## Part X — Design Analysis Appendices

*(These appendices are analysis of the implemented shapes, written from the
verified code. They record why the shipped design is coherent and which
alternatives its structure forecloses; they do not report unrecorded design
meetings and they authorize nothing.)*

### Appendix J — Why each system is shaped the way it is

**J.1 Why garage wear is permille-tripwire rather than probabilistic.**
The implemented wear model is a pure accumulator with a hard 1000 permille
immobilization line. A probabilistic "part fails on any trip" model was
available to the designers (the expedition layer even has one for mid-route
breakdowns), so the choice of determinism in the garage is legible: *randomness
belongs to the journey; decay belongs to the machine.* The split gives players
two different relationships with risk — they can fully control wear through
route choice, roughness and service cadence (no RNG to blame), while the
mid-route breakdown roll preserves uncertainty where the player is committed
and cannot turn back. Making the garage deterministic also means recovery
missions have a fixed, schedulable cost (120 ticks), which the duty roster and
expedition planner can reason about — a probabilistic recovery would leak
uncertainty into every fleet plan.

**J.2 Why one mod per slot.**
`installedSlots` is a `Dictionary<string,string>` (slot→mod), not a list. This
single type choice implements build *identity*: a truck is defined by five
mutually exclusive choices, so comparing two vehicles is comparing two vectors,
not two piles. It also makes uninstall/refund bookkeeping total (one slot, one
refund) and prevents the degenerate "stack six bullbars" strategy without any
extra validation code. The cost is that some authored mods compete (both
`protection` mods, three `utility` mods) — which is exactly where the
interesting builds come from (6.2 items 1–2).

**J.3 Why sleeper suspicion grows passively.**
Every unturned sleeper gains +5 suspicion/day automatically. Implemented that
way, investigation becomes *easier the longer a mole operates* — the shelter's
evidence accumulates whether or not anyone is looking. The design consequence:
there is no punishment for slow security response except a longer window of
leaks, and no reward for paranoid early sweeps except catching leaks sooner.
The 200‰ investigation floor keeps even maxed-out evidence from certainty,
which preserves the drama of the turn decision (51.3). A suspicion-only-through-
events design would have made espionage swing on scripted moments; the passive
drip makes it a background pressure instead — appropriate for a survival
management game where most days are logistics.

**J.4 Why the counter-intel score is caller-owned.**
`ShelterEspionageSystem` never raises its own defense score; it clamps and
reads one. This is the one-authority rule producing good emergent design: the
score's *meaning* (whatever makes the shelter hard to spy on — guards, radios,
hardened hatches) stays with those authorities, so espionage cannot grow into
a self-contained mini-game with its own upgrade tree. It also produces the
verified default-game behavior that dead drops never spawn at score 100
(6.2 item 7, D5): espionage content is *opt-in through security investment*,
which is a defensible pacing decision for a game whose core loop is shelter
economy.

**J.5 Why the stress floor exists.**
Without the floor, any cheap repeated relief action would cycle stress to zero
and trauma would be cosmetic. The floor converts trauma into a *ratchet*:
relief still matters day-to-day (mood, productivity thresholds), but the
baseline only moves when a trauma actually resolves. The 750 floor cap is the
second half of the same idea — even a survivor with every catalog trauma keeps
250 permille of genuine downward range, so the system never hard-locks a
survivor into permanent crisis (the crisis gate at 750 would otherwise be
unreachable-or-permanent). The cap plus the 750 trigger line means trauma-
heavy survivors live *at* the crisis boundary — which is the intended drama.

**J.6 Why crises are behaviors, not bars.**
The five crises are all things a shelter manager handles operationally
(6.2 item 8, 52.3): a gap in the duty roster, hidden biscuits, a jammed latch.
The alternative framing — a sanity stat with degenerating labels — would have
made the player *manage a meter* instead of *accommodate a person*, and would
have required stigmatizing copy the tone contract forbids. The productivity-
penalty-only implementation (no health damage, no death, no event cascade)
means the cost of a crisis is precisely bounded and the recovery path is
always social (therapy, time, accompaniment). This is the wave's most
deliberate piece of design writing, and it is entirely in data.

**J.7 Why the acoustic director outputs a snapshot instead of playing.**
An alternative shape — director holds Godot nodes — would have violated the
Core boundary and made headless testing impossible. The implemented shape
gives three properties worth naming: (1) the *evaluation* is testable and
replayable without hardware (53.6); (2) the *interpretation* (volume curves,
bus routing) lives in one thin bridge that can be retuned without touching
simulation code; (3) multiple consumers can read the same snapshot later
(e.g., a future accessibility visualizer for "what is the shelter sounding
like right now") without re-evaluating anything. The verified cost of the
shape is the registration gap (53.4) — the seam that makes property (1)
possible is the same seam that must be completed for audibility.

**J.8 Why the director's spontaneous cues are seeded, not scheduled.**
A deterministic "creak every 40 ticks" schedule would be information: players
learn the shelter's rhythms and stop listening. The seeded draws keep creaks
replay-stable (a determinism audit can reproduce them exactly) while making
them unpredictable to a player — the same compromise the repo's fork
discipline makes everywhere else. The thresholds (600/800) tie the frequency
curve to real structural state, so a damaged shelter is audibly more restless
than a sound one, which is the entire communicative point of the layer.

**J.9 Why three 35-line save facades instead of one generic store.**
Each facade pins the section name, file name, owner label, and payload type at
one type per concern. A generic "Plans 50-53 store" would have shared all four
concerns across three systems — exactly the coupling the save-section
registry's per-section declaration avoids. The duplication is deliberate and
trivially reviewable; the registry block (2.6) is the only place the three
sections meet.

### Appendix K — Edge-case compendium (verified behaviors)

Every row below is behavior readable from the cited code; these are the cases
focused tests and future fixes should preserve.

**Plan 50**

| Case | Verified behavior |
|---|---|
| `RecordTripWear` with km ≤ 0 | No-op (guard on distanceKm). |
| `RecordTripWear` on immobilized vehicle | No-op — stranded vehicles do not decay. |
| Roughness < 0.5 | Clamped up to 0.5 (`Math.Max(0.5f, ...)`). |
| Wear landing above 1000 | Clamped to 1000; single immobilization with the catastrophic-failure reason. |
| Service with `repairPermille` larger than wear | Repairs only the remaining wear (`Math.Min`); cost scales with the *requested* permille, not the applied amount — callers should request min(remaining, desired). |
| Service at zero wear | Refused, "already at nominal condition", nothing consumed. |
| Install on unknown mod id / wrong slot | Refused with catalog-aware reason; no consumption. |
| Install while immobilized | Refused (mods and armor both). |
| Uninstall on slot with no mod | Refused, state untouched. |
| Refund when cost amount is 1 | `Math.Max(1, 1/2)` = 1 — refunds never round to zero. |
| Armor install with foundry absent | Neutral stamp: bp 1000, Standard purity, empty material profile. |
| Armor stamp out of authored bounds | Clamped to [50%, 130%] of the authored pool. |
| `GetArmorProfile` for depleted plate | Band `depleted`, mitigation/absorption 0, speed/fuel deltas still applied in decoration. |
| Recovery completion before 120 ticks | Refused with progress/threshold in the reason. |
| Second recovery registration for same vehicle | Refused; existing mission id returned. |
| `RestoreState(null)` | Factory state — no records, no missions. |
| `DecorateProfile(null)` / empty vehicleId | No-op. |
| Missing armor catalog | Default "Stock Plating" profile; decoration no-op for armor. |

**Plan 51**

| Case | Verified behavior |
|---|---|
| `EnrollSleeperAgent` for existing sleeper | False — one record per survivor, silently. |
| Empty survivor/faction id at enrollment | False. |
| `InvestigateSuspect` on a non-sleeper | True (operation succeeded), `unmasked = false`, clean exoneration report. |
| Investigation roll vs floor | Threshold floored at 200 — 20% miss ceiling at maximum evidence+skill. |
| Turn attempt on unidentified agent | Refused — no confrontation before unmasking. |
| Turn attempt without 5 scrap | Refused with the payoff reason; nothing consumed. |
| `TickDay` called twice in one day | Applies twice — **no day guard**; host owns exactly-once (E6). |
| Sabotage with counter-intel exactly 150 | Prevention gate requires strictly > 150; at 150 nothing prevents. |
| Dead-drop spawn with score exactly 120 | Requires strictly > 120; at 120 no spawns. |
| Fourth drop while three active | Spawn gate blocked at `< 3`. |
| `InterceptDeadDrop` on expired drop | Refused ("not found or already expired") — expiry removes the drop in `TickDay`. |
| Double intercept | Refused ("already collected"); the first call removed it anyway. |
| Turned agent in `TickDay` | +2 intel, skips leak/sabotage entirely. |
| `RestoreState(null)` | Fresh state: no sleepers/drops/incidents, score 100, counters reset. |

**Plan 52**

| Case | Verified behavior |
|---|---|
| `TickDay` with `currentDay <= lastTickDay` | No-op — replay/double-tick safe. |
| Negative day | `ArgumentOutOfRangeException`. |
| `AddStress` with delta ≤ 0 | Ignored (only AddStress exists for increases; no negative stress injection). |
| `ReduceStress` below floor | Clamped to floor. |
| `InflictTrauma` duplicate | Refused ("already suffers"). |
| `InflictTrauma` unknown id | Refused with catalog-aware reason. |
| Crisis trigger while a crisis is active | Blocked (`currentCrisisId` non-empty check). |
| Crisis affinity whose threshold not yet met | No crisis yet; retried on the next `AddStress` crossing. |
| No resolvable affinity | Fallback `crisis_withdrawal`, then `crisis_panic`. |
| Crisis expiry | `currentCrisisId` cleared, −100 stress, then insomnia decay continues. |
| Therapy with unknown action id | Refused with reason; no session counted. |
| Therapy with no active traumas | Relief only; no roll. |
| Breakthrough roll succeeds | Random trauma resolves (uniform index from the seeded rng); −150 additional relief; breakthrough counter increments (saturating). |
| Insomnia roll success | `insomniaDaysRemaining = max(current, 1)` — never extends beyond re-rolling daily. |
| Save with unknown trauma ids / orphan crisis id | Dropped/normalized on capture and restore; duplicates case-collapsed. |
| Negative counters in a hand-edited payload | Clamped ≥ 0 during normalization. |
| `RestoreState(null)` | Fresh state; `lastTickDay = -1`. |

**Plan 53**

| Case | Verified behavior |
|---|---|
| `UpdateSimulationFacts(null)` | Replaced with default facts (no crash, mix resets). |
| `generatorMaxWattage = 0` | Generator layer 0 (divide-by-zero guarded via the > 0 check). |
| Room string casing | `ToLowerInvariant` before substring match — mixed-case room ids work. |
| Unknown room string | Falls through to `acue_prof_inner_vault`. |
| Missing profile id in catalog | Snapshot falls back to the literal `"acue_prof_inner_vault"` string even if unresolvable downstream. |
| `TriggerSemanticCue` with unknown id | Ignored (queued only if cataloged). |
| Two `EvaluateSnapshot` calls in a row | First drains the queue; second sees none — fire-once semantics. |
| Repeated evaluation at stress 650 | Each evaluation re-rolls the 30% creak — frequency scales with evaluation cadence, so tick cadence is part of the mix design. |
| Bridge with null director | `SyncAcoustics` no-op. |
| Bridge after `Dispose` | No-op; double-dispose safe. |
| Manager `_headless` | All playback work short-circuits; snapshot math unaffected upstream. |

**Shared wire**

| Case | Verified behavior |
|---|---|
| `Ensure*` called twice | Same instance returned (idempotent). |
| `_campaignDay` absent at ensure | Fixed-seed fallback rngs (50/51/52/53) — reproducible bootstrap. |
| Catalog JSON missing on disk | System constructed empty; loader never invoked; garage additionally skips armor load. |
| Catalog JSON malformed | Loader exception caught at the host wire (`GD.PrintErr`), system continues empty (partial or absent catalog) — visible in logs, not fatal. |
| `FlushPlans50To53` with nothing dirty | No writes. |
| `CaptureSection` failure | Dirty flag stays set; next flush retries. |

### Appendix L — Determinism deep-dive for the wave

**L.1 The derivation, concretely.** `CampaignRngStream.DeriveSeed` (version 1)
computes `masterSeed × 31337 + StableHash.Of(streamId) × 1009 + day × 37 +
actionIndex`, coerced non-zero (0 → 1986). For the wave's forks with
`day = 0, actionIndex = 0`, each system's base seed is a fixed function of the
master seed and the fork string. Consequences:

- Two shelters with the same master seed derive identical garage/espionage/
  mental-health/acoustic streams *forever* — the fork strings are the only
  thing that can break that, hence 3.5's rename warning.
- Adding a fifth fork for a future system cannot shift these four — the hash
  mixes the *string*, not a registration ordinal. This is the property the
  `CampaignStreamIds` naming comments (Plans 122–125, 162–165) call
  "fork-per-day, cannot shift others"; the wave enjoys it at fork-per-system
  granularity.
- Version 0 legacy mappings (`MoralChoice`/`Radio`/`Economy` → 2026) do not
  touch the wave — these forks always derive through the StableHash path.

**L.2 What is and is not replay-stable.**

| Property | Status | Reason |
|---|---|---|
| Same seed + same command/tick order → identical state | HOLDS for all four systems | All draws from seeded forks; everything else pure math. |
| Same seed + different command order → identical state | DOES NOT HOLD for Plans 51/52/53 | The long-lived forks advance by draw order (3.5); reordering a therapy before an investigation changes every later draw. |
| Same seed + different command order → identical garage state | HOLDS for Plan 50 | The garage consumes no draws at all. |
| Save → load → continue ≈ uninterrupted run (state equality) | HOLDS | Systems persist state, not RNG positions; their draws gate flavor/probability, not conservation (3.5). The 51–54 report proved the analogous property for the expedition leg (mid-sortie save/load equality). |
| Save → load → continue ≈ uninterrupted (bit-identical subsequent draws) | DOES NOT HOLD | Position is not captured for these forks (manager `CapturePositions` exists but the wire does not use it for them). Acceptable per 3.5; worth revisiting only if a future consumer makes a resource-conserving roll on these streams. |
| Default-game dead-drop / trauma appearances | Deterministically ABSENT | E5 (score 100 < 120) and E7 (no inflictor) — the content gates are closed until callers open them. |

**L.3 The acoustic cadence caveat.** The spontaneous draws fire per
`EvaluateSnapshot()` call, and the wire calls it once per `TickPlans50To53`.
If a future package moves to per-frame evaluation for smoother mixes, the
effective creak/dust rates multiply by the frame rate — the thresholds are
per-evaluation, not per-day. Any cadence change must divide the chances by the
new evaluations-per-day to preserve the authored feel (a premise check for that
package, per AGENTS.md rule 7).

**L.4 Fixed-seed fallback discipline.** The four fallback seeds (50, 51, 52,
53) are distinct and unlikely to collide with the derived-seed space in any
coordinated way, but the important property is simpler: bootstraps without a
campaign day are *identical across machines and runs*, so selftests and
first-boot sessions are reproducible. The plan-numbered constants also make
the fallback source obvious in a stack trace.

**L.5 `System.Random` audit.** A repo-wide convention (AGENTS.md rule 4)
forbids `System.Random` in deterministic Core behavior; none of the four wave
systems constructs one, and their constructors force every randomness through
the injected `ISeededRng` with deterministic defaults. The one
`GD.Randi()` in the verified surface (`AudioManager.PlayCueDef` random
resource-path choice among variants) is presentation-layer, non-stateful, and
correctly outside the determinism contract.

### Appendix M — State-size and performance notes

All sizes are upper bounds derivable from the DTO shapes; none of these
systems is plausibly a performance concern at any conceivable survivor/vehicle
count, but recording the shape helps save reviewers.

| Section | Growth driver | Practical bound |
|---|---|---|
| `vehicle_garage` | One record per owned vehicle; one mission per immobilized vehicle | Tens of records; missions self-removing on completion |
| `faction_espionage` | Sleepers (≤ roster), drops (spawn-gated `< 3`), incidents (append-only) | Incidents are the only unbounded list — a years-long campaign accrues a few hundred small rows; a retention trim is a future owner decision, not an emergency |
| `survivor_mental_health` | One record per survivor who ever had any interaction; trauma ids per record (catalog-bounded at 6) | Roster-sized; normalization dedups |
| Acoustic | Nothing (no persistence) | Snapshot is 6 integers + a string + a short list, per tick, garbage-light |

CPU shape: garage folds are O(slots) per call over ≤ 5 slots; espionage and
mental-health ticks are O(sleepers) and O(survivors × traumas) per day; the
acoustic evaluation is O(1) with two RNG draws. No system allocates per-frame;
only the daily tick allocates (cloned read models on capture).

### Appendix N — Tone and accessibility contract for the four surfaces

The repo's UI rules (restrained, human, keyboard-safe, contrast-safe) bind any
future presentation work on this wave. The wave's own data already models the
contract:

- **Language register.** Every authored player-facing string in the four
  catalogs is concrete, domestic, and non-clinical: "Storage depot manifest
  tampered with; minor supplies reported missing" (espionage), "Obsessive
  concealment of ration biscuits" (crisis), "Isolated sensory rest in
  sound-dampened quarters" (therapy), "Deep resonant groaning of reinforced
  concrete" (acoustic). No slur, no real-world polity, no diagnosis label, no
  gore. New strings should calibrate against these, not against genre
  defaults.
- **Failure text is Core-owned.** `VehicleGaragePanel.LastFeedback` surfaces
  the Core `out string reason` verbatim — one voice, one authority for why a
  command failed. Future surfaces (espionage, mental health) should inherit
  the same rule rather than paraphrasing into panel-local strings.
- **Sound as information, with speech protected.** The cue catalog's ducking
  groups put `dialogue` above bulkhead impacts and let the radio bus
  self-duck; the manager layers accessibility duck offsets on top. Any loop
  registration work (53.4) must preserve both — the mix is a readability
  surface for players who cannot watch the screen, which is the accessibility
  argument for finishing that work at all.
- **No punishment framing.** Crisis durations end; insomnia decays; therapy
  always counts attendance; investigation exonerates the innocent with a
  clean report; turning an enemy is priced in scrap, not in humiliation.
  Presentations must not add shame mechanics the data declines to model.
- **Keyboard/back and lifecycle.** `VehicleGaragePanel` implements
  `IBindablePanel` with `OnClose`, `Unbind`, and shell-integrated focus;
  `DailyBriefingModal` documents its Enter/Space/Tab acknowledgment contract
  explicitly; `ShelterAudioController` detaches old Core sessions before
  rebinding to prevent stale-save control of audio. New surfaces copy the
  nearest of these three, per the UI rules in AGENTS.md.

### Appendix O — How this expansion was produced (method note)

1. Read `AGENTS.md` and the target file (3,677 chars, preserved byte-for-byte
   at the top of this document).
2. Grep/read the four systems, three save stores, six UI types, three audio
   types, the wire partial, the RNG manager, the save registry, the integrity
   and utilization layers, and the four catalogs in full or in the sections
   cited in Appendix G.
3. Ran *absence* searches for every name the map asserts that could not be
   found (`GarageDetailPanel`, `garage_breakdown`, `acue_` in `src/`,
   production callers of `UpdateSimulationFacts`, live consumers of the
   espionage and mental-health systems, the trauma-trio cross-references).
4. Read `docs/PLANS_51_54_INTEGRATION_REPORT.md` in full to resolve its
   relationship to this map (2.8) — different plan series.
5. Recounted `[Fact]`/`[Theory]` occurrences in the nine test files (4.1).
6. Wrote this expansion as append-only chunks; ran `wc -m` after each chunk.
   No builds, no test executions, no Godot sessions, no other files touched.

Limitations of this method, stated plainly: line numbers drift as files change;
the case counts are declaration counts, not executed-case counts; the
"UNVERIFIED (map text)" labels mark claims *this pass* did not verify, not
claims proven false; and the working tree contained unrelated concurrent
changes that were treated as read-only context throughout.

---

## Part XI — Integration Playbooks (one per open item)

*(Each playbook states the bounded outcome, the exact seams to extend, the
focused verification, and the non-goals. Each requires a foreman claim per
`WORKTREE_OWNERSHIP.md` before anyone edits; the playbooks are preparation,
not authorization. Anchors: P1–P9 map one-to-one onto debt-ledger items 2–10
of 7.5; P10 and P12 are Appendix D questions 12 and 11; P11 comes from the
incident-retention note in Appendix M. Debt item 1 needs no playbook — this
expansion is that fix. `P<n>` here names playbooks, never plans; plans are
always written out as "Plan 50"–"Plan 53".)*

### P1. Playbook — Bind espionage read models to a live surface (debt #2)

- **Bounded outcome:** one surface renders sleeper roster, active dead drops,
  incident ledger, counter-intel score, and intel points read-only; zero new
  gameplay authority.
- **Seams:** extend `FactionDetailPanel` with an espionage section (it already
  renders intel *logs* for holdfasts) or add a section to `DailyBriefingModal`'s
  report pipeline via `DailyBriefingReportBuilder` facts. Read model access
  already exists: `Main.ShelterEspionageSystem`.
- **Contract to respect:** sleeper identity stays hidden until `isIdentified`
  — the read model shown to players must filter on `isIdentified ||
  isTurnedDoubleAgent` for named rows, or aggregate unidentified sleepers as a
  count. Rendering raw records would leak the answer the investigation roll
  protects.
- **Verification:** panel selftest bindings (`SceneBindingSelfTest`,
  `UiAccessibilitySelfTest` patterns); Core unchanged → no new Core tests
  required beyond any read-model helper's unit test.
- **Non-goals:** no new commands (interrogate/turn/intercept UI), no score
  auto-tuning, no drop-spawn UI.

### P2. Playbook — Bind mental-health read models (debt #3)

- **Bounded outcome:** per-survivor stress band, active traumas (by display
  name), insomnia flag, active crisis name + days remaining, and therapy
  availability appear in the survivor surface.
- **Seams:** `SurvivorDetailPanel` (already per-survivor, already refresh-
  based) fed from `Main.SurvivorMentalHealthSystem.GetOrCreateRecord` — note
  the API is get-or-create, so a read-only path should prefer `HasRecord` +
  `GetOrCreateRecord` pair to avoid materializing records for untouched
  survivors, or add a read-only getter to the Core system (small, signed
  change).
- **Tone guardrail (from Appendix N):** trauma/crisis names render as authored
  (`display_name` only); no severity adjectives beyond the authored copy.
- **Verification:** `SurvivorMentalHealthTests` unchanged; UI selftests for
  binding; manual contrast check against `Ashfall.Core.UI.Theme`.
- **Non-goals:** no therapy command UI, no stress editing, no cross-link into
  `MentalHealthCrisisSystem` state.

### P3. Playbook — Acoustic cue registration + loop starts (debt #4)

- **Bounded outcome:** all 11 `acue_*` ids resolve in the host; the bridge's
  six continuous layers start loops once and update thereafter; one-shots
  play.
- **Seams:** either extend the static `AudioCueCatalog` (repo-preferred:
  generator + `--check` mode if a codegen pattern exists for catalogs) or add
  a small runtime loader that registers the JSON cues with the manager at
  `EnsureShelterAcoustics` time. Then extend `ShelterAcousticBridge.SyncAcoustics`
  to `StartLoop("shelter_acoustic", layer)` on first sighting of each layer
  and `StopLoop` on zero-permille if desired (or hold the loop at −80 dB —
  simpler, avoids start/stop churn; the −80 path is already implemented).
- **Bus mapping decision (open item D3):** `radiation` → `Ambience` or new
  `Radiation` bus; `structural` → `Subterranean` or new `Structural` bus. One
  reviewed decision; do not mix strategies per layer. The one-shot cues
  carrying the JSON-only `bulkhead` bus id need the same style of decision at
  registration time (map onto an existing bus unless a new bus is signed).
- **Verification:** `AudioSelfTest`; `ShelterAcousticDirectorTests` (unchanged);
  a Godot headless audio selftest pass; manual listen on a Godot runtime
  session at 15 FPS per repo rule.
- **Non-goals:** no ducking-group table changes, no new cues, no director
  math changes.

### P4. Playbook — Facts producer (debt #5)

- **Bounded outcome:** one owner composes `AcousticSimulationFacts` per day
  tick from live systems and calls `UpdateSimulationFacts` before
  `SyncAcoustics`.
- **Seams:** the composing partial already exists as a natural home —
  `Main.TickPlans50To53` — with inputs from power (`PowerGridSystem`),
  ventilation/starting level (`StartingLevelSystem` state), radiation
  (radiation authority), structure (shelter structure/stress owner), radio
  (radio owner), room (current room tracker). Each input needs its owner's
  existing read API; if any value has no read API, that sub-item is a
  separate claim — do not add caches (rule 5).
- **Verification:** extend `Plans50_53_SharedIntegrationTests` with a
  facts-populated evaluation assertion; director tests unchanged.
- **Non-goals:** no per-frame evaluation (see L.3), no new facts fields.

### P5. Playbook — Counter-intel pusher (debt #6)

- **Bounded outcome:** at least one shelter posture source writes
  `SetSecurityCounterIntelScore` on a defined cadence.
- **Seams:** candidate owners are the guard/duty-roster authority (posted
  watch) and the radio authority (intercepts). The API takes a scalar; the
  *composition* (e.g., base 100 + posted-watch bonus + intercept bonus,
  clamped) belongs to the pushing owner, not to espionage.
- **Verification:** pushing owner's focused tests; espionage tick tests
  unchanged (the system only clamps/reads).
- **Non-goals:** no score decay model inside espionage, no events from
  espionage about the score.

### P6. Playbook — Trauma event mapping (debt #7)

- **Bounded outcome:** campaign events that should wound minds call
  `AddStress`/`InflictTrauma` with catalog ids chosen by `trigger_tags`.
- **Seams:** the event owners (combat/casualty authority for deaths and
  betrayal facts; expedition authority for abandoned-ally outcomes). Mapping
  table lives with the caller (one static table per owner or a small shared
  mapper in Core — a foreman call on placement).
- **Verification:** caller's tests assert the mapping (event → expected
  system calls); `SurvivorMentalHealthTests` unchanged.
- **Non-goals:** no automatic event crawling, no new trauma ids without a
  data package (`ashfall-data-add` skill territory).

### P7. Playbook — Journal binding (debt #9)

- **Bounded outcome:** crisis `journal_entry_key` strings produce journal
  entries on crisis start (or end) through the existing journal authority.
- **Seams:** the journal authority consumes keys; the mental-health system
  exposes the active crisis id → the caller (or a projection like
  `SleepNarrativeProjection`) can resolve the key from
  `psychological_trauma.json` without new Core state.
- **Verification:** journal authority's tests plus one integration assertion.
- **Non-goals:** player-authored journal text, catharsis reward loops beyond
  the existing −150/−100 relief.

### P8. Playbook — Labor ticks decision (debt #8)

- **Bounded outcome:** either `install_labor_ticks` is consumed by the
  duty-roster authority at install time (a real scheduled task with atomic
  completion), or the field is removed from the authored schema with a
  scanner-visible data migration note.
- **Seams:** duty-roster owner for consumption; catalogs + validators for
  removal. Both paths end with the data-integrity selftest re-scan.
- **Non-goals:** half-implementations (a progress bar that gates nothing).

### P9. Playbook — Fork-id promotion (debt #10)

- **Bounded outcome:** four `public const string` entries appear in
  `CampaignStreamIds` (`VehicleGarage = "vehicle_garage"`, `ShelterEspionage =
  "shelter_espionage"`, `SurvivorMentalHealth = "survivor_mental_health"`,
  `ShelterAcoustics = "shelter_acoustics"`), snake_case per the
  `CampaignRngSourceGateTests` naming discipline, and the wire partial
  references the constants.
- **Verification:** the RNG source gate tests plus a string-equality guard
  (the constants must equal the old literals byte-for-byte; a drifted rename
  is a replay break, caught by comparing against the literal in a test).
- **Non-goals:** changing derivation, adding day/action offsets to existing
  forks.

### P10. Playbook — Recovery fuel enforcement decision (D12)

- **Bounded outcome:** `requiredFuelUnits` is either enforced at completion
  (`CompleteRecoveryMission` consumes through the inventory port — a small
  Core change with a save-safe failure mode: refusal leaves the mission
  intact) or documented as flavor with a catalog comment.
- **Verification:** `VehicleGarageSystemTests` gains one refusal case and one
  consumption case; existing five integration cases untouched.
- **Non-goals:** fuel logistics for the recovery *travel itself* (that would
  be an expedition package).

### P11. Playbook — Incident retention (from M)

- **Bounded outcome:** `recentIncidents` gets an authored retention rule
  (e.g., keep last N or last K days) *only if* a presentation surface needs
  history; otherwise leave append-only and note the bound in this document.
- **Seams:** espionage Core (small), with a save-compat note: trimming on
  capture is save-version-safe because incidents are narrative, not
  resource-bearing (the scrap siphon applies at tick time, not from history).
- **Non-goals:** retroactive trimming of other lists.

### P12. Playbook — Consolidation review (D11, long-term)

- **Bounded outcome:** a foreman-signed audit document comparing the four
  mental-health authorities (2.7) across: state overlap, save migration cost,
  determinism exposure, presentation contracts, and event seams — concluding
  in either a staged convergence plan or a recorded decision to keep the
  boundaries.
- **Non-goals:** any code change inside this playbook; the audit precedes any
  package.

---

*End of document. Original 2026-09-06 map text preserved byte-for-byte above
the separator; all 2026-09-25 findings cited to working-tree files and authored
JSON as of the evidence date. This document is documentation-only: no code,
data, save, or generator outputs were modified. The authoritative sources
remain the code, the catalogs, and the governance files named in `AGENTS.md`.
Where this document and the tree ever disagree again, re-run the method in
Appendix O and amend Appendix H.*
