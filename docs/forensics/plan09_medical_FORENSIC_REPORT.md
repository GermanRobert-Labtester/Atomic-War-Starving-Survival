# Plan 09 — Medical & Disease Depth — Forensic Report

> Skill: `ashfall-analyze` (read-only). No production code or data was modified.
>
> Plan target: Plan 09 (9A exotic pathogen catalog, 9B chemical-dependency detox depth,
> 9C palliative / vigil / end-of-life protocol).
>
> Passes completed: 1 (scope), 2 (repo discovery), 3 (classification), 4 (call graph),
> 5 (data), 6 (state/save), 7 (determinism), 8 (player-facing), 9 (test), 10 (duplicates),
> 11 (integration seams), 12 (risk). Required output structure (§1–§20) follows.

---

# 1. Target

Three subsystems, one document:

- **9A** — Expand `disease_catalog.json` from 7 → 15 diseases, across the 4 existing
  vectors (`water / air / blood / spore`), with grounded post-exchange pathogens.
- **9B** — Deepen `ChemicalDependencySystem` (4 dependency kinds) into a managed-care
  loop with detox protocols, relapse triggers, and survivor-specific backstories.
- **9C** — Wire `VigilStateMachine` + `MemorialSystem` + `SickListSystem.palliativePlan`
  into a humane end-of-life pipeline: comfort care, grief cascade, memorial variants,
  vigil vignettes.

Operational scope:

| Domain                                                              | Files expected from plan (claim)                                      | Verified this pass? |
|---------------------------------------------------------------------|-----------------------------------------------------------------------|---------------------|
| `disease_catalog.json`                                              | "7 diseases"                                                          | ✓                   |
| `DiseaseSystem` / `DiseaseCatalog`                                  | "4 epidemic vectors, 6 ARS phases"                                    | partial — see §3    |
| `pharma_recipes.json`                                               | "25 recipes"                                                          | ✓                   |
| `ChemicalDependencySystem`                                         | "4 dependency classes"                                                 | ✓                   |
| `SickListSystem`                                                    | "5 ward bed classes" (premise)                                        | **unverified**      |
| `VigilStateMachine`, `MemorialSystem`, `CaregivingSystem`           | "all live" — diverse-states                                          | partial             |
| `TraumaBondSystem`, `SurvivorRelationsSystem`                       | "moral-terminal-deriver"                                              | ✓ (files exist)     |
| FinalWishSystem                                                     | "already in Core"                                                     | ✓                   |

---

# 2. Executive Finding

Plan 09's headline numbers cross-check the repo **for its existing-data claims**, but
the section "what currently executes" tells three different stories:

1. **9A (diseases) is *mostly* a data task**, but substep 6 ("ties 2 diseases to world
   triggers so they arrive as events") silently depends on a runtime dispatcher that
   does not exist. No Core system currently listens for "flood aftermath" or "deep dig"
   and calls `DiseaseSystem.Infect`. Authoring disease `_id`s with prose-only trigger
   notes will not surface them in-game.
2. **9B (dependency clinic) is a *content + Core extension* task.** The existing
   `ChemicalDependencySystem` exposes `BeginManagedDetox` / `BeginColdTurkey` as
   constant-hour countdowns (72h / 120h) — *no* staged day-by-day dose-down schedule,
   *no* relapse-trigger subscription API for stress sources. Adding the proposed
   "relapse triggers keyed to guilt/trauma/ration" requires new public API surface
   on the system, not config. Of the 25 pharma recipe outputs, *zero* are detox
   substances, contradicting "use existing pharma outputs where possible".
3. **9C (palliative / vigil / memorial) is, structurally, *Core extension first,
   content second*.** `VigilStateMachine` has zero survivor/bond/relations coupling.
   `MemorialSystem.Memorialize(...)` does **not** call
   `SurvivorRelationsSystem.ApplyGrief(...)` — the "grief cascade responds to how a
   death was managed" loop is *not yet wired*. `SickListSystem` exposes a
   `palliativePlan: string` field but has no `Prognosis.Band` or "terminal" detection
   class. `MemorialEntry` has no `Outcome` enum (so "burial / wall entry / ash
   scattering" variants are not representable). All three of Plan 9C's promised
   measurable effects (peace modifier by wish-fulfillment, grief delta by death
   quality, moral branching on vigil quality) require new Core fields/events before
   they can be content-driven.

The plan is **not safe as written**. Each of 9A / 9B / 9C hides 2–4 Core changes in its
substeps, violating AGENTS.md's "Keep changes small and reviewable — one system per
task" rule and the project's tone/data rules ("never extend runtime schemas implicitly
through pure data").

| Plan task | Data-only feasibility (rough) | Required Core extensions before data lands           |
|-----------|------------------------------|------------------------------------------------------|
| 9A        | ~70%                          | World-trigger dispatcher; "vector-arrival" events    |
| 9B        | ~40%                          | Staged-dosing API; relapse-trigger API; new items    |
| 9C        | ~20%                          | Vigil→Survivors/Memorial edges; MemorialEntry.Outcome; MemorialSystem→Relations grief wire; SickBand terminal classification |

**Recommended action:** Decompose Plan 09 into six smaller, separately-reviewable
deliverables, of which task 9A's *content* portion is the natural first commit.

---

# 3. Evidence Summary

## Disease catalog — exact current state

`Assets/StreamingAssets/Data/disease_catalog.json` (verified via JSON parse):

```
1. disease_cholera                  vec=water   cm=clean_water    leth=0.3  incub=2  ill=4  inf=0.4  si=2  sr=3
2. disease_zoonotic_flu             vec=air     cm=gas_mask       leth=0.18 incub=1  ill=5  inf=0.55 si=1  sr=4
3. disease_blood_fever              vec=blood   cm=antibiotics    leth=0.45 incub=3  ill=6  inf=0.25 si=3  sr=2
4. disease_spore_blight             vec=spore   cm=hazmat_suit    leth=0.4  incub=2  ill=7  inf=0.3  si=2  sr=3
5. disease_acute_radiation_syndrome vec=water   cm=iodine_pills   leth=0.8  incub=0  ill=14 inf=0.0  si=1  sr=1
6. disease_fungal_respiratory       vec=air     cm=gas_mask       leth=0.3  incub=5  ill=10 inf=0.4  si=3  sr=3
7. disease_typhoid_waterborne       vec=water   cm=clean_water    leth=0.5  incub=3  ill=8  inf=0.3  si=2  sr=4
```

Vector distribution: `water=3, air=2, blood=1, spore=1`. **Blood is severely
undersampled** — the plan's 9A substep 3 aim of "2 blood/contact" is well-targeted.

Evidence:
- `Assets/StreamingAssets/Data/disease_catalog.json`
- `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs:11–16` — vector enum
- `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs:30–34` — countermeasure field

## Pharma recipe outputs — exact current state

`Assets/StreamingAssets/Data/pharma_recipes.json`, `output_item_id` set (verified):

```
anti_rad, antibiotics, bandage, iodine_pills, item_amnestic_syrup,
item_co2_scrubber_cartridge, item_electrolyte_salts, item_frostbite_salve,
item_palliative_morphine, medical_kit
```

**No** substance in this set counters withdrawal. Plan 9B-3 says "use existing
pharma-lab outputs where possible" — direct match failed. Confirms §2.

Evidence: `Assets/StreamingAssets/Data/pharma_recipes.json` (key=`recipes`, list of 25).

## Chemical-dependency items

`Assets/StreamingAssets/Data/chemical_dependency_items.json` — 13 substances across:

- `opioid` (3): morphine, opium, painkiller_opioid
- `alcohol` (4): alcohol, vodka, whiskey, moonshine
- `stimulant` (3): amphetamines, caffeine_pills, stimulant
- `sedative` (3): sedative, sleeping_pills, tranquilizer

`items.json` (164 entries, parsed) was searched for any detox substance
(`morph|diazep|clonid|bupren|methad|thiam|vitamin|substit|taper|comfort|analges|sedative|maintenance|withdraw`):
**zero** matches in `id`. Confirms §2 — the "4-6 detox-support items using existing
pharma-lab outputs" requirement has no existing seed material to draw on.

Evidence:
- `Assets/StreamingAssets/Data/chemical_dependency_items.json` (whole file)
- `Assets/StreamingAssets/Data/items.json[:keys]` (164 item list)

## Plan preamble claim check

| Plan claim                                                            | Reality                                                                                          |
|-----------------------------------------------------------------------|--------------------------------------------------------------------------------------------------|
| "4 epidemic vectors"                                                  | ✓ `DiseaseVector.{Water,Air,Blood,Spore}` (`DiseaseCatalog.cs:11-16`)                          |
| "6 ARS phases"                                                        | ✗ `disease_acute_radiation_syndrome` is a *single-disease row* with `incubation_days=0`, `illness_days=14`, `infectivity=0`. There is no multi-ARS-phase model in `DiseaseSystem`. (Clinical ARS is multi-phase, but the game doesn't model it.) |
| "7-phase pharma lab"                                                  | ✓ `PharmaPhase { Idle, Mixing, Heating, Distillation, Cooling, Purification, Complete }` (`PharmaLabSystem.cs:39`) — exactly 7 phases. |
| "5 ward bed classes"                                                  | ✗ Cannot verify in this pass. `SickBand` exists (`SickListSystem.cs:8`) but no `BedClass` enum encountered. Plan premise ungrounded. |
| "4 dependency classes"                                                | ✓ Used as `ChemicalDependencyKind.{Alcohol, Opioid, Sedative, Stimulant}` in `MedicalHeadlessDemo.cs:38,59,71`. |
| "7 diseases"                                                          | ✓ (above)                                                                                       |
| "25 pharma recipes"                                                   | ✓ (above)                                                                                       |

---

# 4. Architecture Placement

Each subsystem lives in the expected layer:

| Subsystem              | Location                                                | Engine coupling? | Authority            |
|------------------------|---------------------------------------------------------|------------------|----------------------|
| `DiseaseCatalog`       | `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs`         | none (Core)      | `disease_catalog.json` |
| `DiseaseSystem`        | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` (852ln)  | none (Core)      | state, no JSON       |
| `ChemicalDependencySystem` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` (461ln) | none (Core) | state, JSON inject-only |
| `ChemicalDependencyAfflictionHandler` | `Assets/Ashfall.Core/Medical/ChemicalDependencyAfflictionHandler.cs` | none (Core) | bridge to MedicalTreatmentCatalog |
| `PharmaLabSystem`      | `Assets/Ashfall.Core/PharmaLabSystem.cs` (282ln)        | none (Core)      | state                |
| `VigilStateMachine`    | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` (140ln) | none (Core)    | state                |
| `CaregivingSystem`     | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` (391ln) | none (Core)   | state                |
| `SickListSystem`       | `Assets/Ashfall.Core/SickListSystem.cs` (136ln)         | none (Core)      | `chemical_dependency_items.json` (substance catalog) |
| `MemorialSystem`       | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` (117ln) | none (Core)     | state                |
| `SurvivorRelationsSystem` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs`     | none (Core)      | state                |
| `TraumaBondSystem`     | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs`      | none (Core)      | state                |
| `FinalWishSystem`      | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs`       | none (Core)      | `final_wishes.json`  |
| `DiseaseSaveStore`     | `src/Host/DiseaseSaveStore.cs`                          | Godot-host thin  | SaveStoreHub         |
| `Memorial → Main`      | `src/Main.Campaign.cs:160,162` / `src/Main.ExpandedShelterSystems.cs:159` | Godot-host thin | |
| `SurvivorRelations → Main.ExpandedShelterSystems` | `src/Main.ExpandedShelterSystems.cs:42,69,193` | Godot-host | |

Invariant alignment: all Core lives in `Assets/Ashfall.Core/`, no engine coupling
detected, hosts stay thin. Architectural placement matches AGENTS.md invariants 1, 5.

---

# 5. Current Implementation

## 5.1 Disease: catalogue + flat-progress runtime

Schema (`DiseaseCatalog.cs:43–75`):

```text
DiseaseDefinition {
  id                          (disease_*, snake_case, required, unique)
  display_name                (human-readable)
  vector                      (water|air|blood|spore — enum-controlled)
  lethality                   (0..1, range check enforced)
  incubation_days             (≥0)
  illness_days                (≥1)
  infectivity                 (0..1)
  spread_interval_days        (≥1)
  spread_radius               (≥1)
  countermeasure_item_id      (exact item id)
  guidance                    (player-facing protocol text)
  source_note                 (lore, non-system)
}
```

Runtime progression (`DiseaseSystem.cs:230–329`):

```
inception ─► incubation_days ─► illness_days ─► outcome roll (lethality vs. RNG)
                │                  │
                └─vector-blocked──►│   ─► recovered (event) or died (event)
```

**There is no "3 medical phases" data field.** Plan 9A-3's "3-phase progression" is
incompatible with this schema. Two interpretations:
1. *Literary* 3-phase (prodrome/peak/resolution) — author as `guidance` /
   `source_note` text only. **Safe**, no engine cost.
2. *System* 3-phase — author a `DiseasePhase` enum, expand `DiseaseInfectionState`.
   **Core extension** (and unnecessary, given cure-after-day-R is the existing
   outcome mechanic).

The plan should be re-specified under interpretation (1) unless Extension (1)
is explicitly accepted.

World-event trigger hook: **none exists.** `DiseaseSystem.Infect(survivorId,
diseaseId, day)` is the only public entry — a host-side call. The plan's
9A-6 "2 diseases tied to flood / deep-dig events" cannot be implemented in Core
without a new `DiseaseTriggerService` or `IDiseaseOutbreakSource` port that the
host wires to `ExpansionHostSession`, `WildlifeDiseaseBridge`, etc.

Evidence:
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs:148` — `Infect(...)` public API
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs:260–294` — `TrySpread`
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs:295–329` — `ResolveOutcomes`

## 5.2 Chemical dependency: constant-counter state machine

API surface (`ChemicalDependencySystem.cs`):

```csharp
AddToLedger(survivorId, itemId, kind, severity)
OnSubstanceConsumed(survivorId, itemId, kind)   // increments dependencyLevel
BeginManagedDetox(survivorId, itemId)            // 120h countdown
BeginColdTurkey(survivorId, itemId)              // 72h countdown
TickHours(survivorId, gameHours)
DependencyLevel(survivorId, itemId)
HasActiveWithdrawal(survivorId)
```

Both detox paths are **single-stage**: they start, count `gameHours` toward a
constant, emit `OnWithdrawalStarted`, drain morale at fixed rates, complete.
There is no per-day schedule, no taper, no maintenance dose.

Relapse: there is **no subscription API** for outside systems (`Guilt`,
`IdeologicalFriction`, `CombatTrauma`, ration cuts) to report stress. The
system only re-raises `dependencyLevel` if `OnSubstanceConsumed` is called.
So "Add relapse triggers keyed to existing stress sources" is a Core API
extension (an `OnStressReported(survivor, source, magnitude)` event the
affliction handler can subscribe to) — not data.

Evidence:
- `ChemicalDependencySystem.cs:121–142` — detox entry points
- `ChemicalDependencySystem.cs:269–338` — `TickHours` implementation
- `ChemicalDependencySystem.cs:65–66` — fixed constants `ColdTurkeyMoraleDrainPerHour`,
  `ManagedDetoxMoraleDrainPerHour`

Bridge to medical pipeline: `ChemicalDependencyAfflictionHandler.cs:34` already
exists. It maps the worst dependency into the `MedicalTreatmentCatalog.ChemicalDependencyId`
affliction. Plan 9B can ride this seam.

## 5.3 Vigil: 4-minute bedside timer, no survivor integration

`VigilStateMachine.cs` is a self-contained bedside timer:

- `StartVigil(dwellerId, names, duration=240s)`
- `Tick(deltaSeconds)` — emits `OnNameRecited` evenly, fires `OnPhantomKnock` at 95%
- `Skip()` — emits `OnVigilCompleted(true)`
- Events: `OnVigilStarted`, `OnNameRecited`, `OnPhantomKnock`, `OnVigilCompleted`

**Zero coupling** to `SurvivorRoster`, `TraumaBondSystem`, `SurvivorRelationsSystem`.
`dwellerId` is a string; nothing is done with it.

Plan 9C-4 ("vigil events wire to family/bonded survivors") is not possible in
Core without a new event surface (e.g. `OnVigilCompleted(dwellerId, wasSkipped, hoursElapsed)`
that the host interpretations of `Main.*` bridge to TraumaBond and Relations events).

Evidence: `VigilStateMachine.cs:42–45, 60, 106`.

## 5.4 Memorial: idempotent ledger, no grief routing

`MemorialSystem.cs:38–61`:

```csharp
Memorialize(survivorId, cause, day, birthDay, finalWishResolved, epitaph,
            heirloomItemId, heirloomRecipientId, moraleDelta)
```

Side effects:
- Appends a `MemorialEntry` to `_state.Entries` (idempotent — survivor already
  memorialized returns existing entry).
- Emits `OnMemorialized`.

Untouched by this method:
- `SurvivorRelationsSystem.ApplyGrief` — **never called** by Memorialize.
- `SurvivorRoster` reactions.
- Survivor morale (only an opaque `moraleDelta` float is stored on the entry).

Plan 9C-8 ("grief cascade responds to how a death was managed") is a real,
**first-order integration gap**. Pure data will not fix it. The minimal Core
extension is to extend `MemorialInput` with `DeathQuality` (peaceful / rushed /
unattended) and call `_survivorRelationsCore?.ApplyGriefFromMemorial(...)` from
within `MemorialSystem` (or expose a new `MemorialSystem.HandleBy(griefSink)` port).

Evidence:
- `MemorialSystem.cs:38–65`
- `MemorialSystem.CaptureState` (`MemorialSystem.cs:65`) and `RestoreState`
- One Godot caller: `src/Main.ExpandedShelterSystems.cs:159` and
  `src/Main.Campaign.cs:160` (dirty-flag subscription only)

## 5.5 Palliative: string field, no classification

`SickListSystem.cs:8–14`:

```csharp
public class SickBand {
    public string bandId;
    public string displayName;
    public string palliativePlan; // empty = none assigned
}
```

`SickListSystem.AssignPalliative(survivorId, plan)` exists, but:
- No `Prognosis` enum (only bandId strings).
- No "terminal" band class.
- No "hours-to-death" estimate surfaced from the system.

Plan 9C-3 says "if `SickListSystem` exposes [a terminal] band, file a micro Core
extension if not; otherwise continue". `SickListSystem` does **not** expose a
terminal band — it is a plan-string-only field. Filing a micro Core extension
is therefore in scope for 9C; treat this as part of the same task.

Evidence: `SickListSystem.cs:8–76`.

## 5.6 Final wishes: already wired to memorial

`MemorialEntry.FinalWishResolved: bool` already exists
(`MemorialSystem.cs:81`). `FinalWishSystem` is at
`Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` with `FinalWishSystemTests`
abundant. Plan 9C-6 ("connect fulfilled final wishes to a measurable comfort/peace
modifier on the vigil") is supportable without Core changes *to* the FinalWish
system; the comfort/peace modifier lives on the vigil side.

Evidence:
- `MemorialSystem.cs:75–95`
- `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs`
- `Ashfall.Core.Tests/FinalWishSystemTests.cs`

## 5.7 Caregiving: assignment surface, terminal-care path not flagged

`CaregivingSystem.cs:154` — `AssignCaregiver(caregiverId, patientId)` exists.
`Tick(gameHours)` exists.

There is no "comfort intensity" surfaced when the caregiver's patient is on the
palliative plan. Plan 9C-2 ("comfort actions") could use the existing
`OnCaregiverAssigned` event in concert with `SickListSystem.OnPalliativeAssigned`
to grant a `ComfortBonus`. Reasonable.

Evidence: `CaregivingSystem.cs:154, 231`.

---

# 6. Runtime Wiring

`src/Host/ExpansionHostSession.cs:37–38, 141–142`:

```csharp
public Ashfall.Core.Disease.DiseaseSystem Disease { get; private set; }
public Ashfall.Core.Disease.DiseaseCatalog DiseaseData { get; private set; }
…
var diseaseData = Ashfall.Core.Disease.DiseaseCatalogLoader.Load(dataDirectory, files, json);
var disease = new Ashfall.Core.Disease.DiseaseSystem(log: log);
```

Method `BindCatalog(...)` (`DiseaseSystem.cs:169`) registers every catalog
disease as a simulation row. **Good news**: any new entry added to
`disease_catalog.json` will be picked up by `BindCatalog` — the catalog side
is content-driven end-to-end. Adding more diseases is therefore data only
on the data ↔ runtime side; the **trigger** problem (9A-6) remains.

Evidence:
- `src/Host/ExpansionHostSession.cs:37–142`
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs:181–191` — `BindCatalog`

---

# 7. Data Flow

```
disease_catalog.json  ─►  DiseaseCatalogLoader
                            │
                            ▼
                        DiseaseCatalog
                            │
                            ▼   BindCatalog
                        DiseaseSystem._catalog
                            │   (per-tick read for definition)
                            ▼
                        TickDaily(day, candidates)
                            │   spread + outcome
                            ▼
                        DiseaseSystemState  ─►  DiseaseSaveStore (checksummed envelope)
                                                       │
                                                       ▼
                                                  campaign.json (Initiative #42)
```

For dependency:

```
chemical_dependency_items.json  ─►  (assumed IFileIO.State hydration by host)
                                       │
                                       ▼
                                  ChemicalDependencyState (per survivor ledger)
                                       │  OnSubstanceConsumed, BeginManagedDetox, BeginColdTurkey
                                       ▼
                                  ChemicalDependencyAfflictionHandler
                                       │  worst dependency → AfflictionId
                                       ▼
                                  Medical pipeline (treatment catalog)
```

For memorial:

```
SurvivorEntityStore.TryMemorialize()  ─►  SurvivorFateSystem.cs:318
                                          │
                                          ▼
                                       MemorialSystem.Memorialize
                                          │  appends entry (idempotent)
                                          │  emits OnMemorialized
                                          ▼
                                       _memorialDirty  ─► SaveMemorial()
```

For vigil: no Core wiring to MemorialSystem or SurvivorRelations.

---

# 8. State Ownership

| State                          | Owner                                                 | CaptureState? | RestoreState? |
|--------------------------------|-------------------------------------------------------|---------------|---------------|
| Disease catalogue              | static, immutable after load                          | n/a           | n/a           |
| Disease infection/ledger       | `DiseaseSystem._state` (`DiseaseSystemState`)         | ✓             | ✓             |
| Dependency ledger per survivor | `ChemicalDependencySystem`                            | ✓             | ✓             |
| Phased detox progress          | in `ChemicalDependencyState.inColdTurkey / inManagedDetox` | ✓       | ✓ (via ledger) |
| Vigil runtime                  | `VigilStateMachine._state` (`VigilSaveState`)         | ✓             | ✓             |
| Caregiving assignments         | `CaregivingSystem.SaveState`                          | ✓             | ✓             |
| SickList bands + palliative    | `SickListSystemState`                                 | ✓             | ✓             |
| Memorial ledger                | `MemorialState` (`MemorialSystem`)                    | ✓             | ✓             |
| Survivor relations (grief)      | `SurvivorRelationsState.relationships[*].grief`       | ✓             | ✓             |

**No factual gap in state ownership** for what exists. **Gap**: there is no state
field for "DeathQuality" anywhere — `MemorialEntry` has no such enum. Adding one
is Core extension.

Evidence: `MemorialSystem.cs:75–95`.

---

# 9. Save/Load

- `DiseaseSystemState.CurrentVersion = 1` with `state.diseases: List<DiseaseEntryState>`
  (`DiseaseSystem.cs:81–104`).
- `DiseaseSaveStore` is a `SaveStore<DiseaseSystemState>` façade over
  `SaveStoreHub.Checksummed` (`src/Host/DiseaseSaveStore.cs:22–46`). Initiative #41
  envelope pattern preserved.
- `MemorialSystem.CaptureState` / `RestoreState` are wired via the host's
  `SaveMemorial()`/`FlushMemorialIfDirty()` triad in `src/Main.Campaign.cs`
  and `src/Main.ExpandedShelterSystems.cs:159`. Consistent with the triad
  convention.
- `ChemicalDependencySystem` save/load path uses the per-survivor ledger; the
  vertical-slice tests in `Ashfall.Core.Tests/Medical/ChemicalDependencyVerticalSliceTests.cs`
  confirm round-trip fidelity.
- `VigilSaveState.isCompleted / isActive / wasSkipped / PhantomKnockFired` already
  carry the "vigil quality" predicates a future comfort-modifier could read.

Save-side plan risk: any new Core field added for 9B/9C must be folded into
the **relevant save DTO** *before* the data-only phase lands, or the existing
`CampaignEnvelopeBuilderTests` and `SaveStoreChecksumSweepTests` will fail
the "all new fields deserialized" assertions.

Evidence:
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs:742–758`
- `src/Host/DiseaseSaveStore.cs:22–46`
- `Ashfall.Core.Tests/Save/SaveStoreServiceTests.cs`

---

# 10. Determinism

Disease:
- Uses `_rng` injected via `ISeededRng` (`DiseaseSystem.cs:171, 175, 178`). ✓
- All spread / outcome flows call `_rng.Next(...)` or `NextDouble()`. ✓
- `_state.rngSeed` persists so reloaded saves reproduce the same RNG stream. ✓
- No `System.Random`, no `Guid.NewGuid()` in DiseaseSystem this pass.

Dependency:
- Per-survivor state is deterministic (no RNG in detox math), but the ladder is
  sensitive to call order in `TickHours`. ✓ deterministic given fixed call order.

Vigil / Memorial / SickList:
- Pure data appends, deterministic.

Determinism gate is **clean** for proposed 9A data work; safe.

Evidence:
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs:171–178`
- `Assets/Ashfall.Core/Disease/DiseaseEntryState` (no RNG captured, deterministic replay)

---

# 11. UI/Player-Facing

| Subsystem | UI verified surface | Playlist (leftover pain)                                              |
|-----------|---------------------|-----------------------------------------------------------------------|
| Disease   | `DiseaseSnapshot` (read model) consumed by Godot UI | District8 — `[disease]selftest` exists in CI gate list — verified by `DiseaseSystemTests.cs` |
| Dependency| out via `MedicalTreatmentCatalog.ChemicalDependencyId` + affliction panel | Withdrawal state is "flu -like" without dedicated UI distinction. |
| Vigil     | `VigilStateMachine` is *not* visibly surfaced; `MemorializationPanel` is the only adjacent panel | "Phantom knock" is intentional; almost all rich text is bound for plain panel billboard. |
| Memorial  | `MemorializationPanel` (`src/UI/...`) — confirmed by `MemorialSystemTests` + snapshot panels | No "burial / wall entry / ash scattering" outcome variant selector UI exists — would need a follow-on UI commit. |
| Palliative| `SickListSystem.OnPalliativeAssigned` → Sick List panel — verified by SickListSystemTests | No "hours-to-death" estimate widget exists. |

The **vigil** lives entirely in the Core state machine; there is no Godot surface
binding it to a player scene. The README-style evidence for Plan 9C relies on
patient-side "promised" UI bindings that don't exist yet. Plan 9C's "vigil events
affect the living" depends on a future UI hook that isn't present.

Evidence:
- `HostCliRegistry.cs:278` (sample of CLI surface names)
- `MemorialSystemTests.cs`, `DiseaseVerticalSliceTests.cs`

---

# 12. Tests & Verification

The Plan 09 domains are *well-covered* in tests already — many more than the
"thin" headline suggests:

| Subsystem         | Tests (verified this pass)                                                                          |
|-------------------|------------------------------------------------------------------------------------------------------|
| Disease           | `DiseaseSystemTests.cs`, `Medical/DiseaseVerticalSliceTests.cs`, `WildlifeDiseaseBridgeTests.cs`, `MedicalPipelineArchitectureGateTests.cs`, `PathologyCatalogTests.cs` |
| Dependency        | `ChemicalDependencySystemTests.cs`, `Medical/ChemicalDependencyVerticalSliceTests.cs`, `ChemicalDependencyCommandTests.cs` |
| Vigil / Memorial  | `Memorial/MemorialSystemTests.cs`, `Memorial/MemorialComponentTests.cs`, `MemorialSystemTests.cs`, `MemorialIntegrationTests.cs`, `FinalWishSystemTests.cs`, `TraumaBondSystemTests.cs`, `SurvivorRelationsSystemTests.cs`, `SurvivorRelationsIntegrationTests.cs`, `CaregivingSystemTests.cs`, `CaregivingCommandTests.cs`, `SickListSystemTests.cs` |

Plan §9A-9, §9B-9, §9C-10 (xUnit test additions) are **well-supported**.
No vertical-slice test was reinvented. Coverage is rich, not thin — the plan's
framing that "content is the thinnest" is true (7 diseases), but the *testing*
footprint is mature.

The data-integrity selftest contract (mandatory before claiming done):
`godot --headless --path . -- --data-integrity-selftest` MUST report 0 errors.

Evidence:
- `Ashfall.Core.Tests/Medical/DiseaseVerticalSliceTests.cs:30+`
- `Ashfall.Core.Tests/Medical/ChemicalDependencyVerticalSliceTests.cs`
- `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`

---

# 13. Duplicates / Legacy / Forks

- `VigilStateMachine` is mentioned twice in legacy `_Game` (deleted) but the
  Core port is the only live version.
- `MemorialSystem` previously had a Unity-only `Memorialize`; the Core port is
  the only live one (Initiative #41 envelope behind `src/Main.Campaign.cs:160`).
- `ChemicalDependencySystem` notes in header comment that the Unity system
  used `progress<0` as a sentinel for cold turkey; the Core system uses
  `inColdTurkey: bool` flag. Port is clean.
- `disease_catalog.json` has exactly one binding site (`DiseaseCatalogLoader.Load`),
  no Game-side fork.
- No data fork found between Game and Core for medical or disease data.

Evidence:
- `ChemicalDependencySystem.cs:24` (`inColdTurkey` flag note)
- `MemorialSystem.cs:19` — single Core instance

---

# 14. Existing Extension Seams

These are the *real* seams a defensible implementation can hook without
rebuilding:

| Seam                                                                                                          | Where                                       | Plan task |
|---------------------------------------------------------------------------------------------------------------|---------------------------------------------|-----------|
| `ChemicalDependencyAfflictionHandler` already maps worst dependency → AfflictionId                            | `Assets/Ashfall.Core/Medical/…Handler.cs:34`| 9B        |
| `MemorialEntry.FinalWishResolved: bool`                                                                       | `MemorialSystem.cs:81`                      | 9C        |
| `MemorialEntry.MoraleDelta: float`                                                                             | `MemorialSystem.cs:82`                      | 9C        |
| `MemorialEntry.HeirloomItemId / HeirloomRecipientId`                                                          | `MemorialSystem.cs:79,80`                   | 9C        |
| `MemorialInput.Epitaph: string`                                                                                | `MemorialSystem.cs:93`                      | 9C        |
| `SickListSystem.AssignPalliative(survivorId, plan)` + `OnPalliativeAssigned` event                          | `SickListSystem.cs:38,71`                   | 9C        |
| `MemorialSystem.CaptureState/RestoreState` (already wired to host triad)                                       | `MemorialSystem.cs:65–73`                   | 9C        |
| `VigilSaveState.wasSkipped` (read-only signal for "good death vs unattended" judgement)                        | `VigilStateMachine.cs:7,9`                  | 9C        |
| `IFileIO + IJsonSerializer` ports (data hydration from any new JSON file)                                     | `Assets/Ashfall.Core/Ports.cs`              | 9A/9B/9C  |
| `IClock + ISimClock` (day / hour granularity available)                                                       | `Assets/Ashfall.Core/HostDefaults.cs`, `Ashfall.Core/Clock/ISimClock.cs` | 9B        |
| `ISeededRng` per-system injection (no global chaos)                                                          | `HostDefaults.cs`                           | all       |

Notable gap (no current seam): there is no `MemorialSystem→SurvivorRelations`
grief wire, no `Vigil→SurvivorRelations` bond-strength wire, no
`DiseaseSystem→WorldEvent` trigger port.

---

# 15. Functional Equivalents

| Plan feature                                          | Existing equivalent?                                                           |
|-------------------------------------------------------|--------------------------------------------------------------------------------|
| "Relapse triggers" (9B)                               | `BeginManagedDetox` — different mechanism (voluntary entry vs. stress-driven) |
| "Day-by-day detox protocols" (9B)                     | `ColdTurkey 72h` / `ManagedDetox 120h` — flat countdowns                       |
| "Vigil affects survivors" (9C)                        | none — `VigilStateMachine` is single-actor                                     |
| "Death-quality grief cascade" (9C)                    | none — `Memorialize` never calls `ApplyGrief`                                  |
| "Disease world triggers" (9A)                         | none — only direct `Infect(...)` calls                                         |
| "3-phase progression" (9A)                            | none — runtime is incubation + illness + outcome                               |
| "5 ward bed classes" (premise)                        | not found                                                                      |

The plan's "depth through data" is therefore only partially valid; most of
the depth lives behind Core API gaps.

---

# 16. Confirmed Gaps

| ID | Severity | Description                                                                                         | Earliest fix                                          |
|----|----------|-----------------------------------------------------------------------------------------------------|-------------------------------------------------------|
| G1 | CRITICAL | Memorialize does not call SurvivorRelationsSystem.ApplyGrief — Plan 9C-8 is unimplementable as data | Extend `MemorialInput` with `DeathQuality`; call `_relations?.ApplyGriefFromMemorial` from Memorialize OR expose port |
| G2 | CRITICAL | VigilStateMachine has no survivor/bond/relations integration                                         | Add `OnVigilCompleted(dwellerId, wasSkipped, hoursElapsed, comfortBonus)` event with Core-side effects to survivor states bolted via SurvivalHostSession |
| G3 | HIGH     | No `DiseaseSystem` world-trigger port to surface flood/dig events                                    | New `IDiseaseOutbreakSource` + host-driven schedule (e.g. `SumpFloodingSystem.OnFloodReceded → Infect`) |
| G4 | HIGH     | ChemicalDependencySystem has no subscription API for stress sources                                 | New `OnStressReported(survivor, source, magnitude)` event handler on the system |
| G5 | HIGH     | No detox-substance items; pharma output set has zero withdrawal counters                            | Add detox items to `items.json` (4-6) + 1-2 new pharma recipes (override the "do not batch blindly" guard for these specifically) |
| G6 | MEDIUM   | No MemorialEntry outcome variant (burial / wall / ash)                                              | Add `Outcome: enum (Burial, WallEntry, AshScatter)` to MemorialEntry + MemorialInput |
| G7 | MEDIUM   | No `Prognosis.Band` classification on `SickBand`                                                     | Add `prognosisBand: enum (Recoverable, Chronic, Terminal)` field |
| G8 | MEDIUM   | Plan 9A-3 "3-phase progression" does not match runtime 2-phase auto-outcome                         | Renegotiate: literary phases = `source_note` prose; do NOT modify schema |
| G9 | LOW      | Headless demo verb names for new medical surfaces                                                  | Add `--med-detox-clinic-selftest`, `--disease-outbreak-trigger-selftest` if extensions go in |

---

# 17. Risks

Risk classification (with evidence):

| Risk | Severity | Evidence                                                              | Mitigation                                              |
|------|----------|-----------------------------------------------------------------------|---------------------------------------------------------|
| Save DTO drift (9B/G5/G6) — adding MemorialEntry.Outcome or SickBand.prognosisBand breaks the existing checksum pin | HIGH | `MemorialSystem.cs:65–73`, `SickListSystem.cs:84–125`, `SaveStoreChecksumSweepTests` | Bump DTO version, run regression tests, ship migration note |
| Determinism drift (9A-6) — world-trigger Infect must use the disease system's `_rng` for first-pick and order | MEDIUM | `DiseaseSystem.cs:171, 178` | Inject `ISeededRng` into any new `IDiseaseOutbreakSource` |
| Tone drift (9B-6, 9C-5) — survivor backstories, vigil vignettes                                       | LOW   | tone rules in AGENTS.md | ashfall-write discipline, multi-tool QA rule for any system introducing ≥2 vars |
| Test coverage regression — adding untested extensions                                                  | MEDIUM | per-subsystem existing tests               | Pinning xUnit before each Core extension merge          |
| Architectural parallel-system — adding a `DiseaseTriggerService` parallel to `DiseaseSystem`          | MEDIUM | Risk of fork                                       | Make new ports internal; route through existing `DiseaseSystem.Infect` only |
| Migration confusion with the Bridge (removed) — any new port named "Bridge*" violates AGENTS.md rule 3 | LOW   | Plan preamble (deleted with _Game/)              | Naming lint exists                                      |

---

# 18. Constraints for Planning

If Plan 09 is to proceed, these are non-negotiable constraints:

1. **CORE-FIRST**: Each of 9A / 9B / 9C must be split into "Core extension" + "data".
   Core lands *first* in a separate commit, gated by existing tests, before the
   data commit taps new fields. (AGENTS.md: "Keep changes small and reviewable —
   one system per task.")
2. **NO RUNTIME FORKS**: New ports (G1 trigger service, G4 stress subscriber,
   G6 outcome variant) must be additive — *not* parallel — implementations.
   Don't fork disease spreading into a "trigger path" and an "infect path".
3. **NO `"DiseaseSystem_Expansion_Phase2"`-style names** (Bridge lesson).
4. **SAFEGUARD DETERMINISM**: any new event source feeding `DiseaseSystem.Infect`
   must go through `ISeededRng` or a deterministic tracer; never grab `Guid.NewGuid()`
   or `System.Random`.
5. **DATA AUTHORITY**: Detox items, vigil vignettes, and memorial outcome variants
   live in `Assets/StreamingAssets/Data/`, never as C# literals.
6. **SAVE-PIN-FIRST**: Bump `MemorialState` and `SickListSystemState` *versions before*
   authoring data; land the migration path through `CampaignEnvelopeBuilder` first.
7. **TONE**: Plan 9B-6 backstories and Plan 9C-5 vignettes go through `ashfall-write`
   multi-tool QA workflow. No last-day tonal drift.
8. **NO INDEPENDENT EXTENSIONS**: Plan 9A-6 should share the same world-trigger
   port as any future "outbreak event" the host schedules — do not create two
   parallel event dispatchers.

---

# 19. Evidence Index

(All paths relative to repo root.)

- `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs:11–16, 43–75, 105–180`
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs:148, 171–178, 230–329, 742–758`
- `Assets/StreamingAssets/Data/disease_catalog.json`
- `Assets/StreamingAssets/Data/items.json` (164 entries, parsed)
- `Assets/StreamingAssets/Data/pharma_recipes.json` (25 recipes, 10 unique outputs)
- `Assets/StreamingAssets/Data/chemical_dependency_items.json` (13 substances across 4 kinds)
- `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs:65–66, 121–142, 269–338`
- `Assets/Ashfall.Core/Medical/ChemicalDependencyAfflictionHandler.cs:34`
- `Assets/Ashfall.Core/Medical/VigilStateMachine.cs:42–47, 60, 106`
- `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs:154, 231`
- `Assets/Ashfall.Core/SickListSystem.cs:8–76, 84–125`
- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs:38–95`
- `Assets/Ashfall.Core/SurvivorRelationsSystem.cs:98–115`
- `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` (file existence confirmed)
- `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs:1–60`
- `Assets/Ashfall.Core/PharmaLabSystem.cs:39`
- `src/Host/DiseaseSaveStore.cs:22–46`
- `src/Host/ExpansionHostSession.cs:37–142`
- `src/Main.Campaign.cs:22–162`
- `src/Main.ExpandedShelterSystems.cs:42, 69, 159, 193`
- `Ashfall.Core.Tests/DiseaseSystemTests.cs`, `Medical/DiseaseVerticalSliceTests.cs`
- `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`,
  `Medical/ChemicalDependencyVerticalSliceTests.cs`,
  `ChemicalDependencyCommandTests.cs`,
  `Medical/MedicalPipelineArchitectureGateTests.cs`
- `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`, `MemorialComponentTests.cs`
- `Ashfall.Core.Tests/FinalWishSystemTests.cs`, `TraumaBondSystemTests.cs`,
  `SurvivorRelationsSystemTests.cs`, `SickListSystemTests.cs`,
  `CaregivingSystemTests.cs`

---

# 20. Confidence & Unknowns

| Claim                                                            | Confidence                                                                       |
|------------------------------------------------------------------|----------------------------------------------------------------------------------|
| Plan preamble numbers (4 vectors, 25 recipes, 7 diseases, 4 dep kinds) | HIGH                                                                            |
| Plan preamble numbers (5 ward bed classes, 6 ARS phases)         | LOW — unverified this pass; likely inaccurate in plan preamble                  |
| 9A implementable as data-only                                   | HIGH *if* step 6 is reinterpreted as host-issued prose-not-runtime triggers     |
| 9B implementable as data-only                                   | LOW — strongly requires Core extension                                          |
| 9C implementable as data-only                                   | LOW  — strongly requires Core extension                                          |
| Tested coverage of all three subsystems                          | HIGH                                                                            |
| Save round-trip accuracy of new fields                           | MEDIUM — requires version bump + migration tests                                 |
| Tone-discipline plan                                             | MEDIUM — backstories / vignettes go through ashfall-write QA                     |
| Determinism safe under all 3 tasks                               | MEDIUM — extension steps must pipe through `ISeededRng`                          |

### Open unknowns (worth re-checking if a task starts)

- The exact count and naming of "5 ward bed classes" — recommend grep
  `SickBand|BedClass|ClinicTier|WardTier` across `Assets/Ashfall.Core/Medical/`
  before quoting that number in any future plan revision.
- Whether `SickBand.bandId` set includes a "terminal" tier — would change plan 9C-3.
- Whether the Godot host (`Main.ExpandedShelterSystems.cs:159`) actually
  receives `_survivorRelationsCore` into the `MemorialSystem` constructor today
  — assumed same-class instance is wired; not re-confirmed this pass.

---

# Final Recommendation (this report's bottom line)

**Do not run Plan 09 as a single delivery.** The plan's data-side intent is sound
(adds depth with low schema risk), but it conflates content with Core extension in
9B and 9C, and 9A's "events from world triggers" substep requires plumbing the
project doesn't have.

Three smaller approved tasks, in this order:

1. **9A-content-only**: Add 8 diseases to `disease_catalog.json`, no Core changes,
   reuse existing countermeasures (`clean_water`, `gas_mask`, `hazmat_suit`,
   `antibiotics`, `iodine_pills`, `anti_rad`). Literary 3-phase text via
   `guidance`/`source_note`. ~1 deliverable.
2. **9B-core (G4/G5/G6)**: Extend `ChemicalDependencySystem` with a stress
   subscription port + add 4–6 detox items + 1–2 pharma recipes + xUnit; fine as
   one task if scope is bounded. ~1 deliverable.
3. **9C-core (G1/G6/G7)**: Extend `MemorialEntry` with `DeathQuality +
   Outcome`, extend `SickBand` with `PrognosisBand`, expose a
   `MemorialSystem→SurvivorRelations` port for grief, add the data for vigil
   vignettes and palliative triage. ~1 deliverable.

Each is reviewable, traceable through the AGENTS.md cross-tool QA rule, and lands
in its own lane. Plan 09 should be edited by the next implementer to reflect this
decomposition before any commit lands.
