# ASHFALL: DEEP EXPANSION CONTEXT & INTEGRATION ATLAS
**Authoritative Architectural Blueprint, Connective Seams, Temporal Dynamics & Creative Design Context**

---

## 1. PURPOSE / RELATIONSHIP TO IMPLEMENTED REGISTRY

This document is the **deep structural and design-context companion** to [`docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md).

```mermaid
graph LR
    subgraph Registry["Implemented Canon Registry"]
        A["Answers: WHAT ALREADY EXISTS?<br/>• 318 Core Systems<br/>• 296 JSON Catalogs<br/>• 678 Items, 261 Locations<br/>• Content Counts & Inventories"]
    end

    subgraph Atlas["Expansion Context Atlas (This Document)"]
        B["Answers: HOW DOES IT FIT TOGETHER & WHERE CAN IT EXPAND?<br/>• System Connectivity & High-Leverage Hubs<br/>• Extension Seams & Content Grammars<br/>• Temporal Pacing & Information Economy<br/>• Emergent Story Machinery & White Space"]
    end

    Registry -.->|Foundational Baseline| Atlas
```

### Core Distinction for AI Systems & Designers
* The **Implemented Canon Registry** prevents duplicate mechanic proposals by establishing a hard inventory of implemented systems.
* This **Expansion Context Atlas** guides creative AI models (Gemini, Claude, Perplexity) on **how to build upon ASHFALL's existing connective tissue**. It details:
  1. Where existing hooks, delegates, interfaces, and JSON schemas allow instant expansion without new C# engine code.
  2. How state cascades across systems (e.g. how radiation moves from wasteland weather into personal dosimeters, clinical ARS pathology, sick lists, and survivor grief).
  3. Which systems are isolated islands requiring integration rather than more standalone content.
  4. What practical constraints (determinism, save checksums, JSON authority, dual namespaces) govern new designs.

---

## 2. AUDIT SCOPE & COMMIT

* **Document Version:** 1.0.0 (Authoritative Design & Context Atlas)
* **Audit Timestamp:** 2026-08-20T20:48:08+03:00
* **Audited Git Commit SHA:** `c900210cf6f39442975b8a36ed10322a6ab0d4ef`
* **Target Host / Engine:** Godot Engine v4.7.1 (.NET 8 C# / C# 12)
* **Core Runtime:** `netstandard2.1` / `net8.0` Engine-Agnostic C# (`Assets/Ashfall.Core/` — 318 files, 65,923 lines)
* **Data Authority:** `Assets/StreamingAssets/Data/` (296 JSON catalogs, 59,133 lines)
* **Host Presentation & UI:** `src/` (203 files, 58,545 lines, 27 HostSessions, 60+ UI panels)
* **Verification Standard:** `dotnet test` (all unit tests passing, 0 failures) + `godot --headless` CLI selftests

---

## 3. ARCHITECTURE CONTEXT

To design features that are immediately implementation-ready, creative agents must understand the four distinct architectural tiers of ASHFALL:

```mermaid
graph TD
    subgraph Tier1["Tier 1: Data Authority (Assets/StreamingAssets/Data/)"]
        Data["296 snake_case JSON Files<br/>(Definitions for items, locations, quests, diseases, radio, events)"]
    end

    subgraph Tier2["Tier 2: Single Source of Truth (Assets/Ashfall.Core/)"]
        CoreSystems["Plain C# Simulation Systems<br/>(Zero Godot/Unity Engine References)<br/>Implements CaptureState() / RestoreState()"]
        CorePorts["Ports & Interfaces (Ports.cs)<br/>IJsonSerializer, IFileIO, ILog, IClock, ISeededRng"]
    end

    subgraph Tier3["Tier 3: Presentation & Session Host (src/)"]
        HostSessions["27 HostSessions (src/Host/)<br/>(Binds Core events/state to Godot lifecycle)"]
        UIOverlays["60+ Godot UI Panels & Controls (src/UI/)<br/>(Render views, capture user input, emit intents)"]
        SaveStores["30 Checksummed Save Stores (src/Host/)<br/>(Versioned envelope serialization, SHA-256 integrity)"]
    end

    subgraph Tier4["Tier 4: Automated Verification (Ashfall.Core.Tests/)"]
        Tests["2,194 xUnit Automated Unit Tests<br/>(Determinism, schema validation, save roundtrips)"]
    end

    Tier1 -->|Deserialized via SystemTextJsonSerializer| Tier2
    Tier2 -->|Subscribed to by| Tier3
    Tier3 -->|User Actions Dispatched to| Tier2
    Tier2 -.->|Verified by| Tier4
```

### Critical Architectural Boundaries
1. **Never Propose Core Engine References:** `Ashfall.Core` cannot reference `Godot`, `Node`, `Vector2`, `Resource`, or `UnityEngine`. New gameplay mechanics must be pure C# classes operating on primitive types, domain DTOs, and ports.
2. **Never Invent Dual Data Sources:** Unity ScriptableObjects and `.meta` files in `_quarantine_legacy/` are dead. `StreamingAssets/Data/*.json` is the sole authority.
3. **The Setup-Save-Flush Triad in Main.cs:** `src/Main.cs` manages 38 Setup, 30 Save, and 18 Flush methods. Any new stateful system must implement a matching triad (`SetupXxx`, `SaveXxx`, `FlushXxxIfDirty`) to persist across sessions without state leaks.

---

## 4. PLAYER EXPERIENCE MODEL

ASHFALL is designed around a continuous cognitive loop of information gathering, desperate triage, resource commitment, and delayed fallout.

```mermaid
graph LR
    A["1. OBSERVE<br/>(Weather forecast, dosimeters, vitals, radio signals, pantry stocks)"] 
    --> B["2. INTERPRET<br/>(Weigh crisis urgency: freezing vs starvation vs raider toll)"]
    --> C["3. PRIORITIZE<br/>(Allocate power, water, shift rosters, medical beds)"]
    --> D["4. COMMIT<br/>(Dispatch expedition, tap foundry cupola, enforce strict rationing)"]
    --> E["5. PAY COST<br/>(Burn diesel, consume filters, inflict survivor fatigue/guilt)"]
    --> F["6. RECEIVE CONSEQUENCE<br/>(Immediate: scavenged loot, casualty; Delayed: sickness, strike, raid)"]
    --> G["7. ADAPT<br/>(Rebalance bunk assignments, mediate disputes, treat ARS)"]
    --> A
```

### Hierarchy of Decision Horizons

| Decision Level | Information Player Receives | Resources Manipulated | Core Uncertainty | Immediate vs. Delayed Consequence | Primary Executing Systems |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Minute-to-Minute (UI Interaction)** | Vitals gauges, room temperature, crafting queue progress, sound cues. | Active survivor assignment, crafting station focus, radio frequency dial. | Exact item yield, ambient audio hazard trigger. | Immediate: Craft queued, station assigned. Delayed: Fatigue accumulation. | `NeedsSystem`, `CraftingSystem`, `RadioTuner`. |
| **Shift-Based (Morning / Evening)** | Shift roster slots, sickness band reports, intake filter clogging. | Morning/Night shift labor, water filtration jobs, greenhouse watering. | Work accident rolls, somatic flashback triggers. | Immediate: Power allocated, water treated. Delayed: Refractory firebrick wear in foundry. | `DutyRosterSystem`, `PowerGridSystem`, `WaterTreatmentSystem`. |
| **Daily Cycle (Sim Day Advance)** | Daily briefing modal, weather transition, radio distress intercepts, food stocks. | Food/water ration tier (Feast vs Starvation), dosimeter clearance, medical procedures. | Night raid occurrence, sickness incubation manifestation. | Immediate: Rations consumed, health adjusted. Delayed: Guilt insomnia, cholera spread. | `StartingLevelSystem`, `RationConflictSystem`, `DiseaseSystem`. |
| **Expedition Cycle (Multi-Day)** | Wasteland map danger tier, route distance, weather forecast, vehicle fuel. | Party roster (scouts/guards/medics), stance, supplies (ammo, water, filters, fuel). | Encounter threat level, loot roll randomness, vehicle breakdown risk. | Immediate: Travel hours spent, supplies burned. Delayed: Radiation ARS phases, trauma bonds formed. | `ExpeditionSystem`, `WastelandMapSystem`, `TacticalCombatSystem`, `RadiationPhaseProgression`. |
| **Strategic / Faction (Weekly / Monthly)** | Warlord tribute demand notices, market price shock reports, debt statements. | Food/ammo tribute payments, promissory loan contracts, treaty quotas. | Faction retaliation doctrine shift, merchant caravan arrival schedules. | Immediate: Debt contracted, tribute delivered. Delayed: Raider breach assault, regional trade embargo. | `WarlordDoctrineSystem`, `LedgerDebtSystem`, `MarketSystem`, `SilentFoundrySystem`. |
| **Endgame / Saga (Seasonal / Annual)** | Forensic evidence documents enrolled, machine log reckonings, regional census. | Grand Treaty ratification, machine evidence submissions, debt ledger burning. | Automated Machine tribunal verdict, surviving lineage viability. | Immediate: Narrative climax choice. Delayed: 32-permutation whole-saga epilogue matrix resolution. | `ReckoningSystem`, `VerdictEndingEvaluator`, `EpilogueMatrixRuntime`, `MusterSystem`. |

---

## 5. TIME & PACING ATLAS

ASHFALL operates on discrete temporal layers, from real-time UI interactions to multi-year generational arcs.

```mermaid
timeline
    title Temporal Architecture of ASHFALL
    Frame / Real-Time : UI Rendering : Audio Event Bridge : Radio Frequency SNR Dial
    Hourly Simulation : Work Shift Ticks : Pharma Distillation Phases : Underwater Dive Oxygen Drain
    Daily Tick : Weather Check : Needs Decay : Sickness Progression : Ration Consumption : Save Flush
    Multi-Day Window : Expedition Transit : Crop Growth Stages : Warlord Tribute Cycles (7 Days)
    Seasonal Stage : Weather Seasons (Nuclear Winter to Thaw) : Second Winter Roster Lock
    Endgame Milestone : The Machine Reckoning (Day 360) : Epilogue Matrix Evaluation (Day 360 to 3650)
```

### Temporal Pressure & Pacing Matrix

| Time Scale | Active Core Systems | Typical Player Pressure | Narrative Expansion Opportunity |
| :--- | :--- | :--- | :--- |
| **Real-Time / Session** | `RadioTuner`, `TradeScreenSeam`, `TacticalCombatSystem`, `StealthDiveInstance` | Audio static decoding, barter negotiation fairness, combat lane repositioning, dive acoustic noise limits. | Interactive physical puzzles (circuit rewiring, manual cipher wheels, safe-cracking). |
| **Hourly (Sub-Day)** | `PharmaLabSystem`, `SilentFoundrySystem`, `ExpeditionSystem`, `SomaticFlashbackSystem` | Chemical distillation temperature curves, foundry metal pouring windows, travel hour depletion, panic duration. | Emergency crisis interventions (fixing blowout leaks, stabilizing critical surgery patients). |
| **Daily (Sim Day Tick)** | `SimClock`, `NeedsSystem`, `DiseaseSystem`, `PowerGridSystem`, `GuiltInsomniaSystem` | Ration allocation, rolling blackout load shedding, triage bed management, sleep fatigue recovery. | Daily dweller interaction dialogues, personal confessions, found diary entries. |
| **Multi-Day (3–14 Days)** | `GreenhouseSystem`, `WarlordDoctrineSystem`, `LedgerDebtSystem`, `RadiationPhaseProgression` | Crop harvest cycles, weekly warlord tribute deadlines (7 days), compound debt interest accrual, ARS latent-to-manifest phase transitions. | Multi-stage expedition journeys, caravan arrival events, escalating labor strikes. |
| **Seasonal / Phase (30–90 Days)** | `WeatherSystem`, `DutyRosterSystem` (Second Winter), `District8DeepCoastSystem` | Shifting from toxic ash rains to sub-zero blizzards, freezing canal locks, roster burn audits. | Faction war territorial shifts, major migration waves, seasonal trade fairs. |
| **Yearly / Endgame (Day 360+)** | `ReckoningSystem`, `VerdictCensusBroadcast`, `EpilogueMatrixRuntime`, `MusterSystem` | Enrolling 4+ forensic evidence items before tribunal deadline, surviving long-term demographic collapse. | Final saga trials, regional federation founding, generational succession. |

### Temporal Gaps & Design Opportunities
1. **The Mid-Winter Slump (Days 90–180):** High survival pressure early on often settles into routine once water and power stabilize. Opportunity: Introduce mid-game faction conscription levies, crop blight epidemics, or deep-strata seismic cave-ins.
2. **Delayed Narrative Callbacks:** Many early-game moral choices in `DoorEncounterSystem` lack multi-month delayed callbacks. Seam: Store encounter choice flags in `IFlagLedger` to spawn vengeful survivors or grateful traders 100 days later.

---


## 6. SYSTEM CONNECTIVITY GRAPH

ASHFALL's gameplay emerges from a tightly coupled network of simulation hubs and domain-specific satellites.

```mermaid
graph TD
    subgraph CoreHubs["Primary High-Connectivity Hubs"]
        SurvivorState["HUB 1: Survivor State & Vitals<br/>(NeedsSystem, SurvivorCatalog)"]
        InventoryHub["HUB 2: Physical Inventory & Goods<br/>(Inventory.cs, GoodsCatalog)"]
        TimeClock["HUB 3: SimClock & Calendar<br/>(SimClock.cs, CampaignDayCoordinator)"]
        WeatherHub["HUB 4: Weather & Atmosphere<br/>(WeatherSystem, WeatherKind)"]
        RadiationHub["HUB 5: Radiation & Dosimetry<br/>(RadiationSystem, DoseLedgerSystem)"]
        FactionHub["HUB 6: Faction Standing & Stance<br/>(FactionStanceEngine, WarlordDoctrine)"]
    end

    subgraph ProductionSubsystems["Operational Satellites"]
        PowerSat["PowerGridSystem"]
        WaterSat["WaterTreatmentSystem"]
        GreenhouseSat["GreenhouseSystem"]
        FoundrySat["SilentFoundrySystem"]
        PharmaSat["PharmaLabSystem"]
        WorkshopSat["WorkshopReverseEngineering"]
    end

    subgraph WorldSubsystems["Exploration & Narrative Satellites"]
        ExpeditionSat["ExpeditionSystem"]
        WastelandMapSat["WastelandMapSystem"]
        TacticalCombatSat["TacticalCombatSystem"]
        RadioSat["RadioTuner & FactionRadio"]
        VerdictSat["Verdict Reckoning & MachineLog"]
        MusterSat["MusterSystem"]
    end

    subgraph Islands["Low-Connectivity Islands"]
        VinylIsland["VinylMoraleSystem"]
        TrappingIsland["WildlifeTrappingSystem"]
        SkyArmorIsland["SkyLayerArmorSystem"]
        LineageIsland["CohortSystem & Lineage"]
    end

    SurvivorState <--> InventoryHub
    TimeClock --> SurvivorState
    TimeClock --> WeatherHub
    WeatherHub --> RadiationHub
    RadiationHub --> SurvivorState
    SurvivorState <--> ProductionSubsystems
    InventoryHub <--> ProductionSubsystems
    InventoryHub <--> ExpeditionSat
    ExpeditionSat <--> WastelandMapSat
    ExpeditionSat <--> TacticalCombatSat
    FactionHub <--> ExpeditionSat
    FactionHub <--> RadioSat
    FactionHub <--> VerdictSat
    SurvivorState -.->|Weak Link| VinylIsland
    ProductionSubsystems -.->|Weak Link| TrappingIsland
    WeatherHub -.->|Weak Link| SkyArmorIsland
    SurvivorState -.->|Weak Link| LineageIsland
```

---

## 7. HIGH-CONNECTIVITY HUBS

High-connectivity hubs are foundational systems whose state mutations ripple across multiple gameplay domains.

### 1. HUB: Survivor Physiological & Psychological State
* **Anchor Classes:** [`NeedsSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Survivors/NeedsSystem.cs), [`SurvivorCatalog.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs), [`GuiltInsomniaSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs).
* **Inputs:** Daily ration quality/quantity, ambient room temperature, medical disease infections, combat trauma events, witnessed dweller deaths.
* **Internal State:** 8 vitals (Hunger, Thirst, Fatigue, Warmth, Morale, Health, Hygiene, Rads), active guilt records, insomnia severity, somatic flashback susceptibility.
* **Outputs:** Survivor alive/dead status, work efficiency multipliers (0.0 to 1.5x), cognitive refusal flags, autonomous Utility AI action priorities.
* **Dependents:** `DutyRosterSystem` (shift manning), `SilentFoundrySystem` (workforce), `ExpeditionSystem` (party health), `MedicalWardSystem` (admissions), `EpilogueMatrixRuntime` (demographic survival).
* **Events Raised:** `OnNeedsChanged`, `OnSurvivorDied`, `OnGuiltRecorded`, `OnFlashbackTriggered`.
* **Save State:** `SurvivorRosterState`, `NeedsProfile`, `GuiltInsomniaSaveState`.
* **Extension Seams:** Add new trait modifiers in `SurvivorDefinition.traits`, register custom guilt triggers in `guilt_sources.json`, or bind custom response curves in `utility_actions.json`.

### 2. HUB: Physical Inventory & Supply Chain
* **Anchor Classes:** [`Inventory.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Inventory/Inventory.cs), [`ItemDefinitions.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Inventory/ItemDefinitions.cs), [`MarketSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Economy/MarketSystem.cs).
* **Inputs:** Scavenged expedition loot, harvested greenhouse crops, tapped foundry ingots, synthesized pharmaceuticals, caravan barter acquisitions.
* **Internal State:** Grid slot items, item quantities, durability/wear values, 11 equipped gear items per survivor, 4-tier spoilage states.
* **Outputs:** Resource availability for crafting, medical procedures, generator fuel, expedition supply checks, barter purchasing power.
* **Dependents:** `CraftingSystem`, `PharmaLabSystem`, `PowerGridSystem`, `TradeScreenSeam`, `ExpeditionSystem`.
* **Events Raised:** `OnInventoryChanged`, `OnItemAdded`, `OnItemRemoved`, `OnItemSpoiled`.
* **Save State:** `InventorySaveState`, `EquippedGearData`.
* **Extension Seams:** Add items in `items.json`, define custom scrap yields in `ItemDefinitions.ScrapYield`, author crafting recipes in `recipes.json`.

### 3. HUB: Simulation Clock & Calendar
* **Anchor Classes:** [`SimClock.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/HostDefaults.cs), [`CampaignDayCoordinator.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs).
* **Inputs:** Day advance calls triggered from UI or host orchestrator.
* **Internal State:** Current integer Day (1 to 3650+), fractional hour (0.0 to 24.0), seasonal phase window.
* **Outputs:** Daily simulation ticks dispatched to all 38 Core subsystems.
* **Dependents:** Every stateful system in the game.
* **Events Raised:** `OnDayAdvanced`, `OnHourTicked`, `OnSeasonTransition`.
* **Save State:** `CampaignDayPersistenceAdapter`.
* **Extension Seams:** Hook daily listener actions into `Main.cs:TickSimDay()`.

### 4. HUB: Atmospheric Weather & Nuclear Environment
* **Anchor Classes:** [`WeatherSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/World/WeatherSystem.cs), [`WeatherKind.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/WeatherKind.cs).
* **Inputs:** Multi-day procedural weather generation seeded by `ISeededRng`.
* **Internal State:** Active weather state (22 `WeatherKind` enums), exterior ambient temperature (°C), atmospheric rads/hr, particulate density.
* **Outputs:** Shelter insulation heating demands, air intake filter clogging rates, expedition travel hazards, greenhouse solar illumination.
* **Dependents:** `VentilationSystem`, `PowerGridSystem`, `NeedsSystem`, `ExpeditionSystem`, `GreenhouseSystem`.
* **Events Raised:** `OnWeatherChanged`, `OnForecastGenerated`.
* **Save State:** `WorldWeatherState`.
* **Extension Seams:** Author seasonal profiles in `weather_seasons.json`.

### 5. HUB: Radiation & Dosimetry
* **Anchor Classes:** [`RadiationSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Radiation/RadiationSystem.cs), [`RadiationPhaseProgression.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs), [`DoseLedgerSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/DoseLedgerSystem.cs).
* **Inputs:** Environmental rad fields, radioactive dust inhalation, contaminated food/water ingestion, high-dose maintenance tasks.
* **Internal State:** Personal cumulative mSv, active ARS clinical phase (Healthy to Fibrosis), dosimeter calibration state, 4 dose bands.
* **Outputs:** Physical stamina debuffs, clinical ARS symptom emergence, triage bed assignment requirements.
* **Dependents:** `NeedsSystem`, `SickListSystem`, `MedicalWardSystem`, `FinalWishSystem`.
* **Events Raised:** `OnDoseChanged`, `OnPhaseAdvanced`, `OnLedgerCalibrated`.
* **Save State:** `DoseLedgerSystemState`, `PhaseProgressionSaveState`.
* **Extension Seams:** Add high-rad exploration nodes or author bespoke chelation drugs in `PharmaLabSystem`.

### 6. HUB: Faction Standing & Geopolitics
* **Anchor Classes:** [`FactionStanceEngine.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Economy/FactionStanceEngine.cs), [`WarlordDoctrineSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs).
* **Inputs:** Tribute payments, quest resolutions, door encounter choices, combat engagements, treaty signings.
* **Internal State:** Faction standing score (-100 to +100), active warlord doctrine (`Toll`, `Raiding`, `Fortification`, `Tribute`), treaty compliance quotas.
* **Outputs:** Barter price multipliers (0.5x to 6.0x), trader dialogue tell lines, radio propaganda themes, raider assault frequencies.
* **Dependents:** `TradeScreenSeam`, `FactionRadioEngine`, `AirlockSecuritySystem`, `SilentFoundrySystem`, `VerdictEndingEvaluator`.
* **Events Raised:** `OnStandingChanged`, `OnDoctrineShifted`, `OnTributeDemanded`.
* **Save State:** `FactionStandingRecord`, `WarlordDoctrineState`.
* **Extension Seams:** Author new trade tells in `trade_tell_lines.json` or add warlord responses in `warlord_doctrines.json`.

---

## 8. LOW-CONNECTIVITY ISLANDS

Islands are fully implemented Core systems that currently operate with minimal cross-system feedback. They represent **prime opportunities for systemic integration**.

```mermaid
graph LR
    subgraph Islands["Low-Connectivity Islands"]
        Vinyl["VinylMoraleSystem<br/>(Turntable Music Buffs)"]
        Trapping["WildlifeTrappingSystem<br/>(Snares, Butchery, Wild Rads)"]
        SkyArmor["SkyLayerArmorSystem<br/>(Overhead Roof Armor)"]
        Cohort["CohortSystem & Lineage<br/>(Child Maturation & Schooling)"]
    end

    subgraph IntegrationTargets["Prime Integration Targets"]
        Radio["Connect Vinyl to Radio Broadcasts<br/>(Broadcast Music across Wasteland)"]
        Disease["Connect Trapping to Disease Vectors<br/>(Zoonotic Flu from Bushmeat)"]
        Weather["Connect Sky Armor to Extreme Storms<br/>(RadHail & GlassStorm Roof Breaches)"]
        Skills["Connect Cohort to Skill Apprenticeships<br/>(Foundry/Medical Youth Training)"]
    end

    Vinyl --> Radio
    Trapping --> Disease
    SkyArmor --> Weather
    Cohort --> Skills
```

### Detailed Island Breakdown & Integration Roadmap

| System Island | Current Functionality | Why It Is Isolated | Recommended High-Value Integration Seam |
| :--- | :--- | :--- | :--- |
| **`VinylMoraleSystem.cs`** | Plays scavenged vinyl records in common room for static +15 morale buff. | Only connects to `PowerGridSystem` (power on/off) and `NeedsSystem` (morale). | **Broadcast Music via Radio:** Hook vinyl audio playback into `FactionRadioEngine` to broadcast cultural morale signals across the sector, improving regional survivor recruitment. |
| **`WildlifeTrappingSystem.cs`** | Background ticks calculate deadfall snares and wild game butchery yields. | Runs in isolation; output drops generic meat/hide into inventory without social/medical friction. | **Zoonotic Epidemic Vector:** Route wild carcass butchery through `DiseaseSystem` (contracting `disease_zoonotic_flu` from tainted meat) and `GuiltInsomniaSystem` (poaching sacred cult beasts). |
| **`SkyLayerArmorSystem.cs`** | Models cell-grid roof armor against kinetic penetration. | Rarely evaluated outside of rare orbital telemetry alerts. | **Severe Storm Breaches:** Couple roof armor cell degradation directly to `WeatherKind.GlassStorm` and `WeatherKind.RadHail`, forcing emergency rooftop shoring during storms. |
| **`CohortSystem.cs` / `GenerationalLineageExtension.cs`** | Tracks child dweller baselines and generational maturation. | Operates mostly as background state container; children do not participate in daily labor or education. | **Technical Apprenticeships:** Hook teenage dwellers into `SkillProgressionSystem` as apprentices under master crafters in the Silent Foundry and Medical Ward. |

---

## 9. EXTENSION-SEAM ATLAS

This atlas documents the exact code interfaces, delegates, and JSON hook points where new gameplay content can attach without requiring new C# infrastructure.

### 1. Extension Seam: Door Encounter Choice Trees
* **Existing Capability:** Multi-option moral and transactional dilemmas at the shelter threshold.
* **How New Content Attaches:** Add new entry objects to `Assets/StreamingAssets/Data/door_encounters.json`.
* **What It Can Read:** `minDay`, `maxDay`, `threatLevel`, `visitorFaction`, `requiredItemId`, `requiredItemQuantity`, `requiredTrait`.
* **What It Can Change:** `baseMoraleDelta`, `baseGuiltDelta`, `targetFaction`, `factionStandingDelta`, item inventory additions/removals, survivor roster additions.
* **Constraints:** Must use valid snake_case IDs from `items.json` and `faction_lore.json`.
* **Relevant Files:** `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs`, `door_encounters.json`.
* **Good Expansion Uses:** Asylum seekers fleeing the Iron Garrison, desperate plague victims seeking quarantine, traveling gunsmiths offering weapon repairs.

### 2. Extension Seam: Trade Dialogue Tells & Faction Stances
* **Existing Capability:** Dynamic trader dialogue lines reflecting trust bands and transaction fairness.
* **How New Content Attaches:** Add dialogue objects under trust band keys in `Assets/StreamingAssets/Data/trade_tell_lines.json`.
* **What It Can Read:** Faction trust band (`Hostile`, `Wary`, `Neutral`, `Trusted`, `Allied`), transaction fairness score (`Robbery` to `Gift`), regional scarcity tier.
* **What It Can Change:** Player narrative intelligence, hints about upcoming weather crises, discovered location map markers.
* **Relevant Files:** `Assets/Ashfall.Core/Economy/TradeTellEngine.cs`, `trade_tell_lines.json`.
* **Good Expansion Uses:** Faction-specific trade gossip revealing hidden smuggling routes or impending military offensives.

### 3. Extension Seam: Pharmaceutical Recipe Compounding
* **Existing Capability:** 7-phase laboratory compounding state machine with purity and addiction risk rolls.
* **How New Content Attaches:** Add recipe definitions to `PharmaLabSystem:RegisterRecipe` or `pharma_recipes.json`.
* **What It Can Read:** Chemist skill evaluator (`Func<string, float>`), input reagent item IDs and quantities, station heating requirements.
* **What It Can Change:** Output item ID, output quantity scaled by purity (0.1–1.0), chemical dependency addiction triggers.
* **Relevant Files:** `Assets/Ashfall.Core/PharmaLabSystem.cs`, `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`.
* **Good Expansion Uses:** Synthesizing radiation-clearing chelators, high-potency surgical anesthetics, and anti-psychotic mood stabilizers.

### 4. Extension Seam: Heavy Metallurgy Foundry Casting Molds
* **Existing Capability:** Cupola melting, crucible pouring, refractory firebrick wear, and labor dispute triggers.
* **How New Content Attaches:** Add product definitions in `Assets/StreamingAssets/Data/foundry_production.json` and patterns in `CrucibleFoundryCatalog.cs`.
* **What It Can Read:** Metal charge weights (scrap iron, coke, limestone flux), heat stages (`Cold` to `Annealing`), labor strike status.
* **What It Can Change:** Produced heavy ordnance, reinforced structural armor plates, tool wear, labor dispute severity.
* **Relevant Files:** `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`, `foundry_production.json`.
* **Good Expansion Uses:** Casting custom artillery shells, reinforced airlock blast doors, and high-tensile railway tracks.

### 5. Extension Seam: Shortwave Radio Broadcasts & Distress Signals
* **Existing Capability:** Analog frequency tuning, SNR calculations, signal lock, and played deduplication.
* **How New Content Attaches:** Add broadcast definitions in `Assets/StreamingAssets/Data/radio.json` and `year_of_ash_radio.json`.
* **What It Can Read:** Broadcast frequency (MHz), signal band (`AM`, `FM`, `Shortwave`), minimum day, required world flags.
* **What It Can Change:** Surfaces distress quests, unlocks hidden map nodes, broadcasts Morse code ciphers.
* **Relevant Files:** `Assets/Ashfall.Core/Radio/RadioTuner.cs`, `Assets/Ashfall.Core/Radio/FactionRadioEngine.cs`.
* **Good Expansion Uses:** Multi-part numbers station ARG ciphers, emergency SOS beacons from stranded expeditions, pirate radio propaganda.

---


## 10. CONTENT GRAMMAR

This section reverse-engineers the design language and schema grammar of ASHFALL's content families, explaining how they encode meaning and where their structural limitations lie.

### 1. Door Encounter Grammar (`door_encounters.json`)
* **Structure:** `encounterId`, `visitorName`, `visitorFaction`, `description`, `minDay`, `maxDay`, `threatLevel`, `choices[]` (`choiceId`, `text`, `requiredTrait`, `requiredItemId`, `requiredItemQuantity`, `baseMoraleDelta`, `baseGuiltDelta`, `targetFaction`, `factionStandingDelta`, `outcomeDescription`).
* **Triggers:** Evaluated on morning day ticks when shelter airlock is unoccupied. Filtered by day range and threat level.
* **Inputs:** Survivor traits (`requiredTrait`), item inventory (`requiredItemId`).
* **Outputs:** Delta to `NeedsSystem` morale, `GuiltInsomniaSystem` guilt, `FactionStanceEngine` standing, inventory transfers.
* **Branching:** Flat 2-to-3 choice trees with conditional availability locks.
* **Persistence:** Outcome text logged to events history; permanent stat mutations applied to inventory/factions.
* **Current Limitations:** Cannot natively spawn multi-day follow-up encounters without setting manual world flags in `IFlagLedger`.

### 2. Narrative Questline Grammar (`QuestlineSystem.cs` & `year_of_ash_quests.json`)
* **Structure:** `questId`, `title`, `description`, `stages[]` (`stageIndex`, `briefingText`, `requiredConditions[]`, `choices[]` (`choiceText`, `nextStageIndex`, `rewardItemIds[]`, `reputationDeltas[]`)).
* **Triggers:** Gated by day numbers, previously completed quest IDs, or reaching specific faction standing thresholds.
* **Inputs:** Inventory items, survivor presence at specific location map nodes, world flags.
* **Outputs:** Unlocks subsequent quest stages, mutates regional faction standings, rewards items, uncovers map nodes.
* **Branching:** Multi-stage directed acyclic graphs (DAGs) with branching forks and failure states.
* **Persistence:** Active stage, completed stages, and choice history serialized into `YearOfAshSave`.
* **Current Limitations:** Quests cannot easily evaluate multi-variable math expressions (e.g. checking if total shelter food > 50 AND power > 1000W simultaneously) without dedicated code condition validators.

### 3. Location Schema Grammar (`locations.json` & `holdfast_locations.json`)
* **Structure:** `id`, `displayName`, `description`, `dangerLevel` (1–10), `travelHours`, `baseRadsPerHour`, `region`, `inspect` (flavor text), `overlay_on_unlock`.
* **Triggers:** Discovered through radio intercepts, quest completions, or physical map proximity.
* **Inputs:** Referenced by `ExpeditionSystem` for travel time calculations and loot table lookups.
* **Outputs:** Scavenged container yields, radiation doses delivered to expedition party, ambient encounter pools.
* **Branching:** Static geography; overlay rooms unlocked through `LocationLayoutSystem` in expansions.
* **Persistence:** Scavenged container depletion states, visit timestamps, and discovered echoes persist in `ProceduralScavengeSave`.
* **Current Limitations:** Base locations cannot dynamically change their danger level based on weather storms without active host script recalculations.

### 4. Disease Definition Grammar (`disease_catalog.json`)
* **Structure:** `id`, `display_name`, `vector` (`water`, `air`, `blood`, `spore`), `lethality` (0–1), `incubation_days`, `illness_days`, `infectivity` (0–1), `spread_interval_days`, `spread_radius`, `countermeasure_item_id`, `guidance`, `source_note`.
* **Triggers:** Contracted via environmental exposure (drinking untreated water, inhaling unmasked spores, unsterilized surgery).
* **Inputs:** Evaluated against survivor hygiene, worn gas masks/hazmat suits, and water treatment filtration mode.
* **Outputs:** Incubation countdowns, clinical illness symptoms, mortality rolls, triage bed demands in `SickListSystem`.
* **Persistence:** Individual survivor infection records persist in `DiseaseSystemState`.
* **Current Limitations:** Diseases cannot currently mutate into secondary strains dynamically at runtime.

---

## 11. STATE & CONSEQUENCE ATLAS

This atlas maps the flow of persistent simulation state across ASHFALL's systems, highlighting high-leverage state, hidden state, and potential orphan data.

```mermaid
graph TD
    subgraph StateTypes["Simulation State Flow"]
        DoseState["DoseLedger / ARS State<br/>(Written: Scavenging/Weather | Read: Medical/Vitals)"]
        GuiltState["Guilt Records & Insomnia<br/>(Written: Moral Choices/Deaths | Read: Sleep/Fatigue)"]
        FoundryState["Firebrick & Heat State<br/>(Written: Production | Read: Incident Rolls/Strikes)"]
        WarlordState["Warlord Tribute & Doctrine<br/>(Written: Payment/Shortage | Read: Raid Assigner)"]
        EvidenceState["Evidence Ledger Records<br/>(Written: Exploration | Read: Machine Reckoning)"]
    end

    subgraph HighLeverage["High-Leverage State (Multi-System Impact)"]
        DoseState -->|Dictates| SickList["SickList Triage & ICU Beds"]
        GuiltState -->|Drives| SleepAccidents["Sleep Quality & Foundry Explosions"]
        WarlordState -->|Triggers| AirlockBreaches["Airlock Gun-Port Defense"]
        EvidenceState -->|Determines| WholeSaga["32-Permutation Epilogue Matrix"]
    end
```

### Master State Flow Matrix

| Persistent State Type | Written By | Read By | Persists? | Player Visibility | Narrative Potential | Architectural Classification |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **Survivor Vitals (Hunger/Rads)** | `NeedsSystem`, `RadiationSystem` | `Main.cs`, `MedicalWardSystem`, `UtilityAiSystem` | YES (`.json` Save) | High (HUD Bars & Gauges) | Immediate survival stakes; starvation/ARS crisis. | **High-Leverage State** |
| **Guilt Records & Insomnia Score** | `DoorEncounterSystem`, `MemorialSystem` | `GuiltInsomniaSystem`, `NeedsSystem` | YES | Medium (Insomnia status visible; guilt source text hidden) | Moral weight of past atrocities; psychological haunting. | **High-Leverage State** |
| **Dosimeter Cumulative mSv** | `DoseLedgerSystem`, `ExpeditionSystem` | `SickListSystem`, `RadiationPhaseProgression` | YES | High (Dosimeter Ledger UI Panel) | Institutional exposure limits; radiation sacrifice. | **High-Leverage State** |
| **Warlord Doctrine & Shortage Debt**| `WarlordDoctrineSystem` | `TradeScreenSeam`, `AirlockSecuritySystem` | YES | Medium (Collector demand notes) | Escalating extortion, armed retribution, siege. | **High-Leverage State** |
| **Forensic Evidence Dossiers** | `JournalSystem`, `ExpeditionSystem` | `EvidenceLedger`, `VerdictEndingEvaluator` | YES | High (The Machine's Register Panel) | Deciding the final political fate of the sector. | **High-Leverage State** |
| **Foundry Refractory Wear & Slag** | `SilentFoundrySystem` | `SilentFoundrySystem` | YES | Low (Foundry Maintenance Screen) | Industrial accidents; catastrophic crucible blowouts. | **Domain-Specific State** |
| **Child Maturation Baseline** | `CohortSystem` | `CohortSystem` | YES | Low (Sub-panel text) | Generational continuity; youth education. | **Orphan State (Underconnected)**|
| **Location Strata Inscriptions** | `LocationMemorySystem` | `StandingRecordEngine` | YES | Low (Inspect popups) | Environmental lore echoes from past scavengers. | **Hidden State (Weak Feedback)** |

---

## 12. INFORMATION ECONOMY & UNCERTAINTY

ASHFALL deliberately weaponizes partial, delayed, and unreliable information to force players into making high-stakes decisions under uncertainty.

```mermaid
graph LR
    A["Information Source<br/>(Static Radio, Trader Tell, Dosimeter Reading, Autopsy Log)"]
    --> B["Reliability Evaluation<br/>(Noise SNR, Trader Trust Band, Sensor Calibration)"]
    --> C["Player Interpretation<br/>(Weigh threat: impending storm vs raider attack)"]
    --> D["Operational Decision<br/>(Reinforce roof, pay tribute, send emergency scout)"]
    --> E["Systemic Consequence<br/>(Safe survival vs catastrophic blowout)"]
```

### Information Channels & Uncertainty Mechanics

| Information Channel | Underlying System | Noise / Unreliability Factor | Player Decision Created | Systemic Consequence |
| :--- | :--- | :--- | :--- | :--- |
| **Shortwave Radio Intercepts** | `RadioTuner.cs` & `FactionRadioEngine.cs` | Signal-to-Noise Ratio (SNR); atmospheric static; deliberate faction propaganda deception. | Deciding whether to dispatch an expedition to investigate a distant distress signal. | Rescue viable survivors vs walking into a pre-planned raider ambush. |
| **Trader Gossip & Barter Tells** | `TradeTellEngine.cs` | Trust bands (`Hostile` to `Allied`); low-trust traders lie or withhold critical scarcity warnings. | Stockpiling specific resources (e.g. ammo or filters) ahead of predicted price shocks. | Shielding shelter economy from 6.0x inflation vs wasting scarce barter credits. |
| **Personal Dosimetry Readings** | `DoseLedgerSystem.cs` | Dosimeters can go out of calibration if dropped or submerged, reading falsely low. | Sending a dweller into a damaged reactor crawlspace based on sensor readings. | Safe maintenance repair vs dweller contracting lethal ARS Latent Phase illness. |
| **Medical Diagnostic Autopsies** | `MedicalWardSystem.cs` & `DwellerMedicalCatalog.cs` | Symptoms overlap between bacterial cholera and chemical toxin poisoning. | Choosing whether to administer scarce broad-spectrum antibiotics or clean water flushes. | Halting epidemic spread vs wasting the shelter's last medical ampoule. |
| **Weather Forecast Telemetry** | `WeatherStationSystem.cs` | Barometric sensors provide 70% accuracy for 48h forecasts; sudden cold fronts can shift early. | Choosing whether to leave greenhouse glass open for natural sunlight or sealing shutters. | Maximum crop yield vs complete frostbite crop destruction. |

---

## 13. NARRATIVE DELIVERY CHANNELS

ASHFALL distributes storytelling across 10 distinct presentation channels rather than relying solely on linear exposition.

```mermaid
graph TD
    subgraph DirectChannels["Direct Interactive Channels"]
        EventModal["Door Encounter Choice Modals<br/>(Threshold Visitor Dilemmas)"]
        QuestDAG["Multi-Stage Quest Dialogues<br/>(Branching Faction Missions)"]
        RadioDial["Analog Radio Tuner Audio<br/>(Voice/Morse/Numbers Transmissions)"]
    end

    subgraph EnvironmentalChannels["Environmental & Archival Channels"]
        CodexDocs["196 Forensic Codex Documents<br/>(Diaries, Autopsies, Repair Manuals)"]
        StrataEchoes["Location Strata Graffiti & Echoes<br/>(Tool Markings, Scavenger Inscriptions)"]
        ItemFlavor["Item Inscriptions & Relic Provenance<br/>(Heirloom Memento Descriptions)"]
    end

    subgraph SystemicChannels["Systemic & Behavioral Storytelling"]
        DwellerActions["Utility AI Behavior & Refusals<br/>(Trauma Panics, Caregiving Vigils)"]
        MemorialEulogies["Wall Carvings & Eulogies<br/>(Procedural Death Memorials)"]
        TradeTells["Trust-Reactive Trader Dialogue<br/>(Dynamic Barter Reactions)"]
        TribunalReadouts["The Machine's Census Readouts<br/>(Automated Judicial Audits)"]
    end

    DirectChannels --> SystemicChannels
    EnvironmentalChannels --> SystemicChannels
```

### Multi-Channel Narrative Chains
A single narrative thread in ASHFALL can organically propagate across multiple delivery systems:
1. **Radio Intercept:** Player tunes into a garbled Morse code transmission on 104.2 MHz (`RadioTuner.cs`).
2. **Found Document:** Scavengers uncover a waterlogged dispatch log at `loc_substation_nine` confirming a secret military bunker (`JournalSystem.cs`).
3. **Threshold Visitor:** An injured deserter arrives at the airlock seeking asylum, claiming knowledge of the bunker access codes (`DoorEncounterSystem.cs`).
4. **Survivor Reaction:** An older survivor with the `ex_military` trait experiences a somatic flashback upon hearing the deserter's unit name (`SomaticFlashbackSystem.cs`).
5. **Faction Retaliation:** The Central Garrison issues a radio ultimatum demanding the deserter's surrender, shifting warlord standing (`WarlordDoctrineSystem.cs`).

---

## 14. QUEST CAPABILITY MAP

This section analyzes the exact mechanical capabilities of ASHFALL's quest engines ([`QuestlineSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs), [`HoldfastQuestSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/HoldfastQuestSystem.cs), [`DutyRosterSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs)).

### Quest Engine Feature Matrix

| Quest Capability | Supported? | Underlying Mechanism | Evidence / Code Hook | Practical Limitations |
| :--- | :--- | :--- | :--- | :--- |
| **Prerequisites & Day Gates** | **YES** | `minDay` integer checks; prerequisite quest ID arrays. | `QuestlineSystem.cs:CanStartQuest()` | Day gates are static; cannot easily evaluate dynamic weather seasons without custom code. |
| **Multi-Stage DAG Progression** | **YES** | `QuestStage` array with `nextStageIndex` pointers. | `QuestStage.cs`, `year_of_ash_quests.json` | Requires manual index wiring in JSON; cycles must be avoided. |
| **Branching Moral Choices** | **YES** | `QuestChoice` array with distinct outcome descriptions and deltas. | `QuestChoice.cs` | Choices are local to stage; cannot dynamically branch based on external survivor traits without condition hooks. |
| **Item Delivery Requirements** | **YES** | `requiredItemId` and `requiredItemQuantity` checks. | `QuestCondition.cs` | Checks inventory count; automatically consumes or verifies items. |
| **Location Exploration Goals** | **YES** | `targetLocationId` verification upon expedition return. | `ExpeditionSystem.cs` | Location must exist in location catalogs. |
| **Faction Standing Deltas** | **YES** | `factionStandingDelta` applied to target faction ID. | `FactionWarSystem.cs` | Must use valid faction systems IDs. |
| **Timed Failure Conditions** | **YES** | `expiryDays` counter decremented on daily tick. | `QuestlineSystem.cs:TickDaily()` | Failure transitions to designated failure stage or cancels quest. |
| **Mutually Exclusive Outcomes**| **YES** | Selecting one choice permanently locks alternative branch paths. | `QuestChoiceResult.cs` | Serialized into `ActiveQuestlineRecord`. |
| **Recurring / Daily Bounties** | **PARTIAL**| Supported in `DutyRosterSystem` daily shift tasks; not in base questline engine. | `DutyRosterSystem.cs` | Base questlines are one-shot completions. |

### Quest Design Space Enabled by Current Architecture
Without writing any new C# code, designers can author:
* **Multi-Week Survival Expeditions:** 5-stage quests requiring players to stockpile rations, travel to distant industrial ruins, repair a generator on-site, and extract heavy machine parts.
* **Espionage & Infiltration Chains:** Gaining trust with The Underwrite, stealing financial ledgers, and deciding whether to blackmail the faction or burn the records at the Crossing.
* **Medical Containment Arcs:** Quests triggering when a disease is diagnosed, requiring players to harvest fungal antibiotics, isolate infected dwellers, and synthesize a vaccine in the Pharma Lab.

---


## 15. EMERGENT STORY CAPABILITY

Emergent storytelling in ASHFALL arises when multiple discrete simulation systems collide without scripted intervention.

```mermaid
graph LR
    subgraph TriggerChain["Emergent Tragedy Chain"]
        A["1. Scarcity & Cold<br/>(Blizzard + Low Diesel)"]
        --> B["2. Prioritization Sacrifice<br/>(Cut Power to Clinic)"]
        --> C["3. Pathology Progression<br/>(ARS Manifest Illness Spikes)"]
        --> D["4. Dweller Mortality<br/>(Doctor Dies in Darkness)"]
        --> E["5. Social Shockwave<br/>(Caregiver Guilt Insomnia)"]
        --> F["6. Production Accident<br/>(Crucible Blowout in Foundry)"]
    end
```

### Deep Emergent Story Scenarios

#### Scenario 1: The Conscientious Sabotage
* `RationConflictSystem` records that the shelter leader gave double meat rations to expedition guards.
* A malnourished botanist accumulates high grievance score (> 75).
* During the night shift, the botanist's `UtilityAiSystem` scores sabotage higher than tending crops.
* The botanist contaminates the hydroponic water intake line with spore mold.
* `GreenhouseSystem` triggers a crop blight; `WaterTreatmentSystem` detects biological pathogen.
* The player is forced to hold a tribunal interrogation or execute the saboteur, triggering `GuiltInsomniaSystem` across all living dwellers.

#### Scenario 2: The Sentry's Panic Flashback
* A guard with high `CombatTrauma` hypervigilance is assigned to the night sentry shift.
* `WeatherSystem` rolls `WeatherKind.AshLightning`, generating loud atmospheric static and thunderclaps.
* `AudioEventBridge` emits sound cue; `SomaticFlashbackSystem` triggers a severe panic episode.
* The guard abandons the airlock gun-port to seek emotional grounding from their trauma-bonded partner.
* A passing raider scouting party discovers the unmanned airlock and breaches the outer hatch, triggering an emergency close-quarters firefight in the entrance corridor.

### Prematurely Terminating Chains & How to Bridge Them
* **Foundry Accidents to Medical Drama:** When a crucible explodes in `SilentFoundrySystem`, it currently logs an incident and injures workers, but does not automatically spawn a multi-stage emergency burn surgery quest in `MedicalWardSystem`. *Bridge:* Emit a clinical trauma event that creates a 24-hour surgical stabilization timer.
* **Warlord Tribute to Dweller Desertion:** When tribute is shorted in `WarlordDoctrineSystem`, raiders attack, but dwellers do not currently consider deserting to join the Warlords. *Bridge:* Allow low-morale, ideologically aligned militarist dwellers to defect during tribute confrontations.

---

## 16. RESOURCE PRESSURE NETWORK

ASHFALL's survival economy is governed by interconnected resource flows where every asset serves competing life-or-death functions.

```mermaid
graph TD
    subgraph CoreResources["Resource Flows & Competing Pressures"]
        Water["POTABLE WATER<br/>• Direct Survivor Hydration<br/>• Greenhouse Hydroponic Crops<br/>• Medical Burn Irrigation<br/>• Steam Boiler Generation"]
        Diesel["DIESEL FUEL<br/>• Power Grid Generator Watts<br/>• Expedition Vehicle Engines<br/>• Foundry Preheating Burners"]
        Filters["CHARCOAL / HEPA FILTERS<br/>• Shelter Intake Air Quality<br/>• Scout Gas Masks on Expeditions<br/>• Quarantine Isolation Wards"]
        Antibiotics["ANTIBIOTICS<br/>• Epidemic Outbreak Treatment<br/>• Post-Surgical Sepsis Prevention<br/>• Threshold Visitor Bribes"]
    end

    Water -->|Shortage| Dehydration["Dehydration / Crop Blight / Sepsis"]
    Diesel -->|Shortage| Blackout["Rolling Blackouts / Frozen Halftracks"]
    Filters -->|Shortage| Fibrosis["Respiratory Fibrosis / ARS Inhalation"]
    Antibiotics -->|Shortage| Lethality["45% Blood Fever Mortality"]
```

### Resource Supply & Trade-off Matrix

| Resource Category | Primary Acquisition Source | Competing Uses & Allocation Dilemmas | Shortage Consequence | Recovery & Substitution Methods |
| :--- | :--- | :--- | :--- | :--- |
| **Potable Water** | `WaterTreatmentSystem` (Sand/Ozone) & `BrineWaterSystem` (Distillation). | Drinking vs. Hydroponic crop beds vs. Medical wound irrigation vs. Steam turbine power. | Dehydration vitals decay (fatal in 3 days); withered greenhouse crops; cholera spread. | Boiling raw water over stoves (burns fuel); scavenging sealed pre-war bottles. |
| **Diesel Fuel** | Scavenging industrial depots & purchasing from caravans. | Electrical generator watts vs. Motorized halftracks vs. Foundry cupola preheating. | Complete shelter blackouts (heaters fail); immobilized vehicles; halted metallurgy. | Converting ethanol from `Distillery`; burning wood/coal scrap in thermal boilers. |
| **Air Intake Filters** | Crafting at ChemStation (charcoal) & scavenging military silos. | Shelter air intake louvers vs. Expedition gas masks vs. Isolation ward scrubbers. | Rapid ash dust accumulation; pulmonary degeneration; radioactive spore inhalation. | Washing coarse mesh filters (low efficiency); recycling active charcoal in kilns. |
| **Pharmaceuticals** | `PharmaLabSystem` compounding & hospital scavenging. | Treating clinical epidemics vs. Emergency surgery vs. Bribing visiting warlords. | Uncontrolled disease spread; high surgical mortality; failed warlord appeasement. | Brewing crude herbal extracts (low purity); enforcing strict isolation quarantine. |
| **Scrap Metal & Ingots**| Scavenging ruins & melting scrap in `SilentFoundrySystem`. | Repairing weapon condition vs. Casting structural armor plates vs. Crafting tools. | Jammed firearms in combat; fragile airlock doors; inability to build new beds. | Tearing down non-essential bunker lockers and bunks for structural rebar. |

### Systemic Resource Bottlenecks
* **Clean Water is the Master Bottleneck:** It directly caps maximum shelter population, agricultural output, and medical capacity simultaneously.
* **Underconnected Multi-Use Item: Salt (`item_salt`):** Produced by `BrineWaterSystem`, currently used for food curing and tanning, but underused in chemical electrolyte synthesis or road de-icing.

---

## 17. SURVIVOR AGENCY & SOCIAL MODEL

ASHFALL survivors are complex psychological actors governed by traits, trauma history, ideological affinities, trade mastery, and autonomous Utility AI.

```mermaid
graph TD
    subgraph Identity["Static Survivor Identity"]
        Traits["Innate Traits (survivors.json)<br/>(Resilient, Claustrophobic, Ex-Chemist)"]
        Beliefs["Ideological Profile<br/>(Militarist, Communal, Fatalist, Technocrat)"]
        Trade["Trade Specialty<br/>(Gunsmith, Apothecary, Distiller, Metallurgist)"]
    end

    subgraph DynamicState["Dynamic Psychological History"]
        Trauma["Trauma & Hypervigilance (CombatTraumaSystem)"]
        Guilt["Guilt Insomnia Log (GuiltInsomniaSystem)"]
        Bonds["Trauma Bonds & Rivalries (SurvivorRelationsSystem)"]
        Moral["Moral Hardening State (MoralBranchingSystem)"]
    end

    subgraph Agency["Autonomous Agency & Execution"]
        UtilityScorer["Utility AI Action Scorer<br/>(Evaluates Eat, Rest, Work, Socialize, Panic)"]
        WorkEff["Work Efficiency & Refusal Engine<br/>(0.0x to 1.5x Performance Multiplier)"]
    end

    Identity --> DynamicState
    DynamicState --> Agency
```

### What Makes Two Survivors Behave Differently?
1. **Response to Crises:** A dweller with the `fatalist` belief and high `GuiltInsomnia` will succumb to despair and refuse shifts when a companion dies, whereas an `altruistic` survivor with `MoralBranching` will take double shifts in the clinic to tend the sick.
2. **Roommate Compatibility:** Two militarists sharing a bunk room gain +10% rest quality; pairing a militarist with a penitent cultist triggers nocturnal ideological brawls and sleep deprivation.
3. **Combat Behavior:** A traumatized scout with `CombatTrauma` hypervigilance gains +20% reaction speed on overwatch but breaks into uncontrollable panic if suppressed by automatic rifle fire.

### Mechanical vs. Purely Descriptive Survivor Fields

| Survivor Field | Mechanical Status | Exact Systemic Impact | Expansion Potential |
| :--- | :--- | :--- | :--- |
| **`traits[]`** | **Mechanically Live** | Directly gates encounter choices, modifies medical infection risks, and scales skill XP gain rates. | Add 20+ specialized wasteland survival traits (e.g. `lead_stomach`, `night_eyes`). |
| **`belief_profile`** | **Mechanically Live** | Modifies bunk compatibility in `IdeologicalFrictionSystem` and dictates strike behavior. | Add faction-specific religious dogmas and ideological conversion mechanics. |
| **`skill_levels[]`** | **Mechanically Live** | Scales crafting speed, pharma purity, foundry defect rates, and combat accuracy. | Define tier-10 master perks and special craft recipes. |
| **`backstory`** | **Descriptive / Narrative** | Displayed in survivor inspect UI and referenced in memorial eulogies. | Hook backstory keywords into `PhantomMemoryEngine` heirloom trigger rules. |
| **`voice_type`** | **Descriptive / Audio** | Determines audio bark sound files during UI interaction. | Add unique dialogue vocalizations for traumatic breakdowns. |

---

## 18. FACTION CAPABILITY MODEL

Factions in ASHFALL are dynamic regional entities that exert economic pressure, broadcast radio propaganda, enforce territorial tolls, and launch armed raids.

```mermaid
graph LR
    A["Player Action<br/>(Pay Tribute / Sabotage Aqueduct / Harbor Deserter)"]
    --> B["Faction Standing Mutation<br/>(-100 Hostile to +100 Allied)"]
    --> C["Systemic Economic Reaction<br/>(Prices Multiplied 0.5x to 6.0x | Trade Tells Shift)"]
    --> D["Operational World Response<br/>(Warlord Shifts Doctrine to Raiding | Airlock Assault)"]
    --> E["Long-Term Narrative Climax<br/>(Epilogue Matrix Regional Fate Resolution)"]
```

### Faction Systemic Response Channels

| Faction State Dimension | Executing System | Systemic Gameplay Impact | Narrative Content Reaction |
| :--- | :--- | :--- | :--- |
| **Trust Band (`Hostile` to `Allied`)**| `TradeScreenSeam.cs` & `TradeTellEngine.cs` | Dictates barter exchange rates (up to 6.0x price penalties); locks high-tier goods. | Traders deliver hostile threats, neutral bargaining tells, or allied trade secrets. |
| **Adaptive Warlord Doctrine** | `WarlordDoctrineSystem.cs` | Shifts between `Toll` (taxation), `Raiding` (assaults), `Fortification`, and `Tribute`. | Warlord broadcast ultimatums on radio; collector demand notes delivered to door. |
| **Radio Broadcast Control** | `FactionRadioEngine.cs` | Controls propaganda channels; triggers silence events when stations are wiped out. | Dynamic shortwave news reports praising or condemning the player's shelter. |
| **Treaty Compliance Quota** | `SilentFoundrySystem.cs` & `RegionalTreatySystem.cs` | Enforces monthly metal ingot and munitions export quotas under threat of embargo. | Faction inspectors arrive at airlock to audit production manifests. |
| **Endgame Tribunal Evidence** | `ReckoningSystem.cs` & `EvidenceLedger.cs` | Uncovering declassified faction war crimes influences The Machine's final census verdict. | Faction leaders lobby or threaten the player to suppress incriminating archives. |

### The Dual Faction-ID Namespace Problem
Creative designers MUST be aware of ASHFALL's dual faction-ID namespace:
* **Lore / UI Namespaces:** `iron_garrison`, `ash_militia`, `cult_of_ash_sign`, `warlords_sector_4`.
* **Systems / Save Namespaces:** `faction_central_garrison`, `faction_ash_militia`, `faction_ash_sign`, `faction_silent_foundry`.
* **Design Rule:** When authoring quests, encounters, and trader definitions, always verify the exact system ID against `faction_lore.json` and `HostDefaults.cs` to prevent silent reference failures.

---

## 19. LOCATION CAPABILITY MODEL

Locations in ASHFALL are multi-dimensional environmental nodes capable of housing layered archaeological strata, changing danger states, and dynamic container scavenging.

```mermaid
graph TD
    subgraph LocationProperties["Location Data & State Dimensions"]
        BaseStats["Static Geography (locations.json)<br/>• Danger Level (1-10)<br/>• Travel Hours<br/>• Base Rads/Hour"]
        ScavengeState["Scavenge Memory (ProceduralScavengeSystem)<br/>• Container Depletion States<br/>• Scavenged Visit Timestamps<br/>• Scavenger Perk Modifiers"]
        StrataRooms["Subterranean Strata (LocationLayoutSystem)<br/>• Overlay Rooms<br/>• Inspected Keyholes<br/>• Scraped Wall Inscriptions"]
    end

    BaseStats --> ExpeditionExecution["Expedition Resolution"]
    ScavengeState --> ExpeditionExecution
    StrataRooms --> ExpeditionExecution
```

### Location Classification by Mechanical Behavior

| Location Archetype | Mechanical Dynamics | Examples in Repository | Best Expansion Use |
| :--- | :--- | :--- | :--- |
| **Static Resource Nodes** | Standard travel time and fixed danger level; deterministic container loot tables with diminishing returns. | `rural_gas_station`, `abandoned_pharmacy`, `scrap_yard`. | Quick scavenging runs for basic materials, fuel dregs, and scrap metal. |
| **Dynamic High-Hazard Zones**| Extreme baseline radiation (> 50 rads/hr); ambient weather storms multiply exterior exposure. | `loc_reactor_core`, `ground_zero_crater`, `sulfur_trench`. | High-tier pre-war tech salvage requiring hazmat suits and lead aprons. |
| **Multi-Strata Complex Ruin** | Contains sub-level rooms, locked blast gates, and inspected keyholes unlocked through `LocationLayoutSystem`. | `loc_substation_nine`, `loc_vault_eighty_eight`, `loc_iron_works`. | Deep multi-stage exploration quests uncovering forensic evidence documents. |
| **Submerged Maritime Hulks** | Gated by diving gear; tracks oxygen depletion and acoustic noise thresholds in `StealthDiveInstance`. | `loc_black_flotilla_flagship`, `loc_submerged_keel`. | High-stakes underwater stealth salvage in flooded Cold War warships. |
| **Faction Strongholds** | Controlled by territorial warlords; requires valid transit passes or paying armed tolls. | `loc_toll_house`, `loc_garrison_hq`, `loc_scale_crossing`. | Diplomatic negotiations, smuggling infiltrations, and faction assault missions. |

---


## 20. UI & PLAYER FEEDBACK

A creative feature in ASHFALL is only implementation-ready if the player can clearly perceive its state and make informed decisions through Godot's UI layer.

```mermaid
graph TD
    subgraph FullyVisible["1. Fully Visible & Dedicated UI Screens"]
        SurvivorsUI["SurvivorsPanel.cs & Needs Gauges<br/>(8 Vitals, Health, Trait Badges)"]
        PowerUI["PowerGridPanel.cs<br/>(Watt Meter, Battery Wh, Priority Toggles)"]
        RadioUI["RadioPanel.cs<br/>(Frequency Dial, Signal SNR Lock, Audio)"]
        FoundryUI["SilentFoundryPanel.cs<br/>(Heat Stages, Crucible Melt, Pour Buttons)"]
        CombatUI["CombatPanel.cs<br/>(5 Lanes, 7 Stances, Jam Condition)"]
    end

    subgraph UnderCommunicated["2. Under-Communicated / Hidden State"]
        GuiltSources["Guilt Sources Log<br/>(Only Insomnia score shown; specific guilt crimes hidden)"]
        RadonDeep["Radon PPM in Deep Shafts<br/>(Accumulates quietly without distinct meter)"]
        FoundryRefractory["Firebrick Refractory Wear %<br/>(Hidden until crucible blowout occurs)"]
        TraderFairness["Exact Barter Fairness Ratio<br/>(Communicated only via cryptic dialogue tells)"]
    end
```

### UI Surface & Communication Audit

| UI Surface / Screen Family | Underlying Host Session | What State Is Clearly Visible | What Information Is Obscured / Weak | Recommended UI Extension |
| :--- | :--- | :--- | :--- | :--- |
| **Survivors & Needs HUD** | `SurvivorsHostSession.cs`, `SurvivorsPanel.cs` | 8 vital progress bars (Hunger, Thirst, Warmth, etc.), health percentage, active trait badges. | Detailed psychological guilt records and specific somatic trigger histories are obscured. | Add a "Psychological Dossier" sub-tab detailing past traumas and guilt sources. |
| **Power Grid & Fuel Console** | `PowerGridHostSession.cs`, `PowerGridPanel.cs` | Total generation watts, load consumption, battery storage Wh, 5 priority load shedding buttons. | Power line transmission degradation and circuit breaker fatigue are not explicitly visualized. | Add an interactive circuit breaker schematic showing line overload risks. |
| **Radio Receiver Terminal** | `RadioHostSession.cs`, `RadioPanel.cs` | Frequency dial slider, AM/FM/SW band selectors, signal SNR meter, play/stop audio controls. | Historical frequency catalog and decoded Morse transcripts require manual journal review. | Add an auto-logging transcription window that decodes Morse signals in real-time. |
| **Tactical Combat Overlay** | `CombatHostSession.cs`, `CombatPanel.cs` | 5 combat range lanes, 7 tactical stance toggles, weapon jam alerts, cover integrity bars. | Exact ballistic penetration math and bullet ricochet angles operate under the hood. | Add tactical combat prediction tooltips showing hit probability and armor deflection chance. |
| **Trade & Barter Screen** | `TradeScreenGodotPanel.cs`, `TradeScreenSeam.cs` | Player item grid, trader item grid, trade tell dialogue speech bubble, commit button. | Underlying transaction fairness numeric ratio is hidden to encourage intuitive barter reading. | Maintain dialogue tell immersion; add subtle visual trader facial expression cues. |

---

## 21. PRESENTATION HOOKS

ASHFALL's presentation architecture connects simulation events to audio barks, ambient lighting, particle effects, and illustrative codex art.

### Available Presentation Hook Channels
1. **Audio Event Bridge (`AudioEventBridge.cs`):**
   * Emits reactive audio cues for ambient sirens, storm gales, weapon gunshots, steam pipe ruptures, and Geiger clicks.
   * *Creative Hook:* Trigger somatic flashback events in traumatized dwellers whenever specific industrial audio cues play.
2. **Dynamic Atmospheric Shaders (`WeatherAtmosphereMap.cs`):**
   * Renders full-screen ash particulate fall, green radiation glows, blizzard whiteouts, and heavy black rain.
   * *Creative Hook:* Visually tint shelter interior lighting red during emergency blackout generator trips.
3. **Illustrated Narrative Codex (`JournalBookUI.cs`):**
   * Renders parchment and terminal-style document pages for autopsies, blueprints, and historical records.
   * *Creative Hook:* Attach authentic Cold War technical diagrams and handwritten survivor marginalia to newly authored codex entries.
4. **Survivor Portrait Badges (`UiAssetManifest.cs`):**
   * Displays character portraits dynamically badged with status icons (gas mask equipped, bandages, ARS pallor, hypothermia frost).

---

## 22. FAILURE & RECOVERY DESIGN

In ASHFALL, failure is rarely a binary "Game Over" screen. Instead, failure is an engine for generating secondary narrative drama and emergency triage gameplay.

```mermaid
graph TD
    subgraph FailureTaxonomy["Failure & Recovery Architecture"]
        TerminalFail["1. TERMINAL FAILURES<br/>• All Survivors Deceased (Total Extinction)<br/>• Day 360 Machine Sterilization Verdict"]
        
        RecoverableFail["2. RECOVERABLE CRISES (Creates Secondary Story)<br/>• Power Grid Total Blackout (Heaters freeze; force emergency battery repairs)<br/>• Sickness Outbreak (40% infected; convert common room to quarantine)<br/>• Crucible Blowout (Foundry destroyed; emergency burn surgery)<br/>• Lost Expedition (Party captured; launches rescue mission)"]
        
        PunitiveDeadEnds["3. PUNITIVE DEAD-ENDS (To Be Avoided)<br/>• Unwarned instant death from clicking un-scouted map node<br/>• Irreversible soft-locks from running out of water on Day 2"]
    end
```

### Cascade Mechanics & Anti-Snowball Design
* **The Failure Cascade Loop:** Power loss → Heaters fail → Hypothermia sets in → Dwellers refuse work shifts → Water pumps shut down → Severe dehydration.
* **Built-In Anti-Snowballing Safety Valves:**
  1. *Wasteland Scavenger Pity Spawns:* When food drops to zero, wandering merchants or friendly scavengers appear at the airlock offering emergency nutrient paste.
  2. *Warlord Shortage Grace Periods:* Shorting tribute does not instantly spawn an un-winnable siege; the Warlord issues a 3-day warning ultimatum and offers alternative concession quests.
  3. *Convalescence Morale Rebounds:* Surviving a deadly disease outbreak grants remaining dwellers a permanent "Survivor's Resolve" morale buff (+20).

---

## 23. CURRENT FEEDBACK LOOPS

Understanding ASHFALL's systemic feedback loops prevents designers from introducing runaway economic surpluses or inescapable death spirals.

### 1. Verified Negative (Dampening) Feedback Loops
```text
REFINED METALLURGY DAMPENER:
High Foundry Production 
→ Refractory Firebrick Wear Increases 
→ Worker Fatigue Spikes 
→ Labor Strike Dispute Probability Escalates 
→ Foundry Automatically Shuts Down for Repairs.
(Prevents infinite weapon/armor stockpiling).
```

### 2. Verified Positive (Reinforcing) Feedback Loops
```text
SICKNESS SPIRAL (Reinforcing):
Contaminated Water Consumed 
→ Doctor Contracts Cholera 
→ Medical Ward Capacity Drops 
→ Secondary Patients Suffer Sepsis 
→ More Labor Incapacitated 
→ Less Clean Water Purified.
(Creates desperate emergency triage moments).
```

---

## 24. SATURATION × CONNECTIVITY MATRIX

This matrix crosses **Content Saturation** (how many items/quests/definitions exist) with **System Connectivity** (how deeply the system is wired to other domains) to identify optimal design strategies.

```mermaid
quadrantChart
    title Domain Saturation vs. Connectivity
    x-axis Low Connectivity --> High Connectivity
    y-axis Low Content --> High Content
    quadrant-1 "MATURE & CORE (Extend with Care)"
    quadrant-2 "INTEGRATE & CONNECT (Bridge to Core)"
    quadrant-3 "FOUNDATIONAL (Requires Core Design)"
    quadrant-4 "HIGH VALUE FOR CONTENT (Prime Expansion)"
    "Survivor Needs & Vitals": [0.9, 0.9]
    "Epidemic Diseases & ARS": [0.85, 0.85]
    "Heavy Metallurgy (Foundry)": [0.8, 0.8]
    "Atmospheric Weather": [0.85, 0.75]
    "Pharma Lab Compounding": [0.75, 0.3]
    "Pre-War Relic Workshop": [0.7, 0.25]
    "Turntable Vinyl Morale": [0.25, 0.7]
    "Wildlife Trapping": [0.3, 0.65]
    "Sky Layer Roof Armor": [0.35, 0.6]
    "Cohort Generational Lineage": [0.2, 0.4]
    "Tactile Mini-Game Wiring": [0.1, 0.1]
```

### Strategic Action by Quadrant
1. **High Content + High Connectivity (Mature):** *Survivor Needs, ARS Radiation, Power Grid, Heavy Metallurgy.* → Do NOT build new systems; add rich situational events and narrative dilemmas.
2. **Low Content + High Connectivity (Prime Target for Content):** *Pharma Lab, Relic Reverse Engineering, Expedition Vehicles.* → Add dozens of recipes, blueprints, and vehicle variants; the underlying engine is ready.
3. **High Content + Low Connectivity (Prime Target for Integration):** *Turntable Vinyl Records, Wildlife Trapping, Codex Manuals.* → Wire these systems into radio broadcasting, epidemic vectors, and expedition morale.
4. **Low Content + Low Connectivity (Foundational):** *Tactile Mini-Games, Interior Decor.* → Requires creating both new C# engine ports and data definitions.

---


## 25. UNDERUSED CROSS-SYSTEM PAIRINGS

These system pairings expose compatible state and APIs but currently have little interaction in the codebase. Bridging them unlocks deep systemic gameplay with minimal engineering effort.

```mermaid
graph LR
    subgraph Pairing1["Pairing 1: Weather × Warlord Logistics"]
        W1["WeatherSystem<br/>(Blizzard / Ash Lightning)"]
        <-->|Delays & Ambush Hazards| F1["WarlordDoctrineSystem<br/>(Collector Patrols & Raids)"]
    end

    subgraph Pairing2["Pairing 2: Survivor Trade Specialty × Radio Intel"]
        S2["TradeSpecialtySystem<br/>(Master Gunsmith / Distiller)"]
        <-->|Broadcasts Custom Barter Signals| R2["FactionRadioEngine<br/>(Attracts High-Tier Caravans)"]
    end

    subgraph Pairing3["Pairing 3: Medical Pathology × Barter Negotiation"]
        M3["DiseaseSystem<br/>(Quarantine Outbreak)"]
        <-->|Emergency Medical Concessions| T3["TradeScreenSeam<br/>(Desperate Price Gouging)"]
    end
```

### High-Value Cross-System Pairing Proposals

#### 1. Atmospheric Weather × Warlord Logistics (`WeatherSystem` × `WarlordDoctrineSystem`)
* **Compatible State:** `WeatherSystem.CurrentWeather` exposes extreme storm states (`Blizzard`, `FalloutStorm`, `RadHail`); `WarlordDoctrineSystem` schedules tribute collection visits.
* **Why Weakly Connected:** Warlord collector arrival ticks run on a static day interval without checking storm severity.
* **Bridging Mechanics:** Severe blizzards strand collector patrols in the wasteland, creating an emergency rescue dilemma (save the freezing extortionists for diplomatic goodwill vs letting them freeze and seizing their weapon crates).
* **Engineering Required:** Pure content event / minor wiring in `WarlordDoctrineSystem:Tick()`.

#### 2. Survivor Trade Specialty × Shortwave Radio Broadcasting (`TradeSpecialtySystem` × `RadioTuner`)
* **Compatible State:** `TradeSpecialtySystem` tracks dweller mastery tiers (Master Gunsmith, Master Apothecary); `RadioTuner` handles outbound broadcast signals.
* **Why Weakly Connected:** Radio currently acts primarily as a receiver; dweller crafting mastery does not broadcast outward.
* **Bridging Mechanics:** Transmitting custom radio advertisements detailing the shelter's master goods attracts specialized merchant caravans with exclusive pre-war blueprints.
* **Engineering Required:** Minor host session wiring connecting `TradeSpecialtySystem` to `FactionRadioEngine`.

#### 3. Clinical Pathology × Desperation Barter (`DiseaseSystem` × `TradeScreenSeam`)
* **Compatible State:** `DiseaseSystem` tracks active infected dwellers; `TradeScreenSeam` evaluates transaction fairness and item worth.
* **Why Weakly Connected:** Visiting traders currently evaluate barter value based on static scarcity tiers without checking active shelter epidemics.
* **Bridging Mechanics:** Visiting unscrupulous traders notice sick dwellers coughing in the airlock, triggering predatory price-gouging (10x price multiplier on antibiotics) or forcing players to trade rare weapons for emergency medicine.
* **Engineering Required:** Hook `DiseaseSystem.HasActiveInfection()` into `TradeScreenSeam:CalculateItemWorth()`.

---

## 26. IMPLEMENTATION COST CONTEXT

When formulating feature proposals, creative AI models must categorize additions into these five engineering complexity tiers to maintain architectural feasibility.

```mermaid
graph TD
    T1["1. DATA-ONLY<br/>(0 C# changes; pure JSON catalog authoring)"]
    --> T2["2. DATA + MINOR WIRING<br/>(Hooking existing Core event into HostSession/UI)"]
    --> T3["3. CORE SYSTEM EXTENSION<br/>(Adding new methods/state DTO fields to existing system)"]
    --> T4["4. CROSS-SYSTEM INTEGRATION<br/>(Wiring multi-system dependencies across 3+ domains)"]
    --> T5["5. FOUNDATIONAL ARCHITECTURE<br/>(New abstraction requiring new ports, codecs & save stores)"]
```

### Detailed Implementation Complexity Framework

| Complexity Class | Typical Files & Domains Touched | Save State Implications | Verification & Testing Burden | Determinism & Risk Profile |
| :--- | :--- | :--- | :--- | :--- |
| **`DATA-ONLY`** | `Assets/StreamingAssets/Data/*.json` (items, locations, quests, diseases, radio). | Zero save schema changes; uses existing DTO containers. | `CatalogIntegrityValidator` cross-reference test; schema validation. | Zero determinism risk; 100% safe. |
| **`DATA + MINOR WIRING`** | JSON catalogs + 1 HostSession (`src/Host/`) + 1 UI Panel (`src/UI/`). | Minor UI state caching; no core save schema mutations. | Host boot smoke test; UI panel binding test. | Low risk; preserves core Invariant 1. |
| **`CORE EXTENSION`** | 1 Core System (`Assets/Ashfall.Core/`) + DTO state class. | Requires bumping `schema_version` in save DTO and updating `CaptureState/RestoreState`. | xUnit unit tests; save/load round-trip codec test; determinism check. | Medium risk; must use `ISeededRng` strictly. |
| **`CROSS-SYSTEM FEATURE`** | 2–3 Core Systems + `Main.cs` Orchestrator + 2 Save Stores. | Multiple save stores updated; must maintain Setup-Save-Flush triads. | Integration selftest flag in `HostCli.cs`; end-to-end simulation tests. | High risk; potential cross-system event cascading. |
| **`FOUNDATIONAL`** | `Ports.cs` + Host Adapters + Core Domain + New Save Store. | Creates new versioned save envelope with `SaveChecksum` validation. | Full test suite across xUnit, headless CLI, and host UI binding. | Extreme risk; requires deep architectural review. |

---

## 27. TESTABILITY ATLAS

Every proposed expansion MUST be architected with an explicit automated verification pathway following ASHFALL's testing conventions.

```mermaid
graph TD
    subgraph TestingPathways["Automated Verification Hierarchy"]
        xUnit["1. xUnit Automated Unit Tests (Ashfall.Core.Tests/)<br/>• Pure logic & state machine execution<br/>• Determinism tests (Same seed = identical outcome)<br/>• Save/Load roundtrip & checksum stability"]
        
        HeadlessCLI["2. Godot Headless CLI Selftests (src/Host/HostCli.cs)<br/>• End-to-end headless gameplay simulation<br/>• 70+ automated CLI flags (e.g. --data-integrity-selftest)<br/>• Zero-graphics CI test pipeline"]
        
        IntegrityTests["3. Catalog Referential Integrity (CatalogIntegrityValidator.cs)<br/>• Validates all 200+ snake_case ID prefixes<br/>• Checks recipe-to-item, quest-to-location links"]
    end
```

### Verification Blueprint by Feature Type

| Proposed Feature Type | Primary Verification Tool | Required Test Assertions | Example Test File in Repo |
| :--- | :--- | :--- | :--- |
| **New Questline / Storyline** | `dotnet test` + Data Integrity Selftest | Referential integrity of item IDs, location IDs, and faction IDs; quest stage progression DAG validity. | [`HoldfastQuestSystemTests.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Ashfall.Core.Tests/HoldfastQuestSystemTests.cs) |
| **New Stateful Core System** | xUnit Test Class (`Ashfall.Core.Tests/`) | Initial state baseline; discrete tick mutations; `CaptureState()` / `RestoreState()` identical hash roundtrip; determinism. | [`SilentFoundrySystemTests.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Ashfall.Core.Tests/SilentFoundrySystemTests.cs) |
| **New Chemical / Medical Drug** | `dotnet test` (Pharma Lab suite) | Recipe validation, purity scaling curve math, addiction trigger probability bounds. | [`MedicalWardSystemTests.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Ashfall.Core.Tests/MedicalWardSystemTests.cs) |
| **New Tactical Combat Feature** | xUnit Test Class + Combat Demo Flag | Ballistic trajectory math, armor deflection angles, jam rate under extreme fouling, stance modifiers. | [`CombatBallisticsTests.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Ashfall.Core.Tests/CombatBallisticsTests.cs) |
| **New Host UI Panel** | `godot --headless -- <flag>` | Panel instantiation without null refs, signal binding/unbinding without memory leaks, clean teardown. | [`HostCli.PanelTests.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/HostCli.PanelTests.cs) |

---

## 28. CHANGE-IMPACT MAP

This map highlights high-risk architectural surfaces where seemingly minor changes can trigger catastrophic breaking regressions across the entire game.

| High-Risk Surface | Why It Is Sensitive | Dependent Systems & Downstream Impact | Safe Extension Pattern |
| :--- | :--- | :--- | :--- |
| **`items.json` ID Renaming** | Referenced by over 200 JSON files, crafting recipes, combat catalogs, and loot tables. | Renaming an ID without global migration breaks `CatalogIntegrityValidator` and corrupts saves. | **NEVER rename IDs.** Deprecate old IDs gracefully and add new IDs using snake_case. |
| **`SaveChecksum.cs` Hashing** | Computes SHA-256 hash across all serialized system state fields. | Altering field order or formatting in state DTOs invalidates existing save files. | Follow versioned codec migrations (`V1toV2`, `V2toV3`) with backward-compatible fallbacks. |
| **`Ports.cs` Interfaces** | Core abstraction boundary implemented by host adapters. | Changing interface signatures requires updating both Core defaults and Godot host adapters. | Prefer default interface methods or create specialized secondary interfaces (e.g. `IFactionRadioProvider`). |
| **`ISeededRng` PRNG Calls** | Governs deterministic replayability across hosts. | Introducing `System.Random` or unseeded `GetHashCode()` breaks cross-host determinism tests. | Strictly inject and consume `ISeededRng` (xorshift64*) throughout all simulation classes. |
| **Faction ID Constants** | Competing dual namespaces (`iron_garrison` vs `faction_central_garrison`). | Referencing the wrong namespace in quest triggers results in permanently locked choices. | Check `faction_lore.json` and use the canonical systems ID prefix `faction_*.` |

---

## 29. DATA DEPENDENCY GRAPH

ASHFALL's 296 JSON catalogs form a dense web of referential dependencies validated mechanically by `CatalogIntegrityValidator.cs`.

```mermaid
graph TD
    Items["items.json (678 Items)<br/>Master Commodity & Gear Root"]
    Locations["locations.json (261 Locations)<br/>Master Geographic Root"]
    Factions["faction_lore.json (19 Factions)<br/>Master Political Root"]
    Survivors["survivors.json (174 Characters)<br/>Master Character Root"]

    Recipes["recipes.json / relic_recipes.json"] -->|Consumes & Produces| Items
    Foundry["foundry_production.json"] -->|Consumes Scrap & Produces| Items
    Pharma["chemical_dependency_items.json"] -->|References Drug| Items
    
    Quests["questline_master.json / year_of_ash_quests.json"] -->|Requires & Rewards| Items
    Quests -->|Targets Destination| Locations
    Quests -->|Modifies Standing| Factions
    Quests -->|Involves Actor| Survivors

    Encounters["door_encounters.json / crossing_encounters.json"] -->|Checks Items| Items
    Encounters -->|Checks Faction| Factions
    Encounters -->|Spawns Characters| Survivors

    Radio["radio.json / faction_radio_corpus.json"] -->|Broadcasts for| Factions
    Radio -->|Unlocks Target| Locations
    Radio -->|References Lore| Items
```

### Key Catalog Referential Rules
1. **The Item ID Rule:** Any item referenced in `requiredItemId`, `rewardItemIds`, or `input_ids` MUST exist as a definition in `items.json` or an active expansion catalog.
2. **The Location ID Rule:** All quest navigation nodes and expedition targets MUST resolve against `locations.json` or expansion location files.
3. **The Prefix Convention:** All IDs must follow snake_case prefix conventions (`item_`, `loc_`, `faction_`, `quest_`, `recipe_`, `event_`, `disease_`, `radio_`).

---


## 30. CONTENT AUTHORING CONSTRAINTS

Creative AI systems expanding ASHFALL must strictly obey these narrative, mechanical, and technical authoring rules.

### 1. Hard Narrative & World Constraints
* **Grounded Post-Nuclear Realism:** ASHFALL is grounded survival fiction. There are **NO magic spells, supernatural entities, psychic mutations, alien artifacts, or fantasy monsters**. Mutations are strictly biological (e.g. tumorous blindness, hair loss, stunted limbs, fungal spore growths).
* **Fictional Geopolitics Only:** The pre-war world is composed of fictional alliances and nations (*The Meridian Compact*, *The Northern Coalition*, *The Inland Accord*). **Never reference real-world nations (e.g. USA, USSR, China, NATO)**. Gated by `DataRuleComplianceTests.cs`.
* **Atmospheric Tone:** Cold, gritty, desperate, industrial, and bureaucratic. People barter in lead washers, salted fat, and calibrated millisieverts. Bureaucracy survives the apocalypse (e.g. The Office's axle ledgers and The Machine's automated census).

### 2. Hard Technical & Systemic Constraints
* **snake_case ID Conventions:** All identifiers must be lowercase snake_case with established prefixes (`item_`, `loc_`, `quest_`, `faction_`, `recipe_`, `disease_`, `event_`, `radio_`).
* **JSON Authority:** All static content belongs in `Assets/StreamingAssets/Data/*.json`. Never hardcode narrative prose, item stats, or location descriptions directly in C# source files.
* **Deterministic Simulation:** Never invoke `System.Random`, `Guid.NewGuid()`, or `GetHashCode()` in gameplay logic. All randomness must consume the injected `ISeededRng`.
* **Save Compatibility:** Never delete or reorder fields in existing serialized DTO classes without providing versioned codec migration paths.

---

## 31. WORLD & LORE CAUSAL MODEL

Every gameplay mechanic and narrative conflict in ASHFALL is directly caused by the physical, ecological, and institutional fallout of the atomic exchange.

```mermaid
graph TD
    Cause["1. ROOT CAUSE<br/>High-Altitude Nuclear Airbursts & Orbital Defense Failures"]
    --> Condition["2. PRESENT ECOLOGICAL CONDITION<br/>Atmospheric Nuclear Winter, Dust Inversion & Aquifer Contamination"]
    --> Consequence["3. SYSTEMIC GAMEPLAY CONSEQUENCE<br/>Acute Water Scarcity, Rolling Blackouts, Crop Blights & ARS Radiation"]
    --> Story["4. NARRATIVE & STORY POSSIBILITY<br/>Tribunal Reckonings, Warlord Extortion, Deserter Asylum & Roster Burns"]
```

### Causal World Mechanics Breakdown

| Pre-War Cause / Historical Fact | Present Wasteland Condition | Systemic Gameplay Consequence | Rich Story / Quest Possibility |
| :--- | :--- | :--- | :--- |
| **High-Altitude EMP Airbursts** | Civil power grids and integrated microelectronics completely destroyed. | Electronics are ultra-rare; shelter utilities rely on robust vacuum tubes and steam turbines. | Scavenging an intact Cold War vacuum tube amplifier to repair the shelter's long-range radio transmitter. |
| **Pulverized Silica & Topsoil Inversion** | Perpetual ash precipitation (*The Ashfall*) and toxic particulate fog. | Dwellers contract `RespiratoryDegeneration`; intake filters clog rapidly during storms. | Excavating a buried limestone quarry to produce pozzolan mortar for sealing leaking air shafts. |
| **Automated Cold War Defense AI (*The Machine*)** | Automated census transmitters and orbital platforms remain active without human commanders. | Shelter must submit forensic compliance logs to avoid orbital sterilization during the Year 1 Reckoning. | Infiltrating an abandoned relay bunker to falsify the shelter's dweller mortality register before the census broadcast. |
| **Heavy Brine Infiltration in Aquifers** | Surface water is contaminated with radioactive particulates; deep wells are hyper-saline. | Raw water cannot be consumed directly; requires slow sand filtration and brine evaporation pans. | Sabotaging the Hydro Barons' fortified brine aqueduct to redirect clean drinking water to famine-stricken settlements. |
| **Fragmented Military Remnants** | Professional army units broke into authoritarian garrisons and predatory toll cartels. | Warlords enforce weekly food/ammo tribute; expeditions face heavily armed patrols. | Negotiating a treaty accord with the Silent Foundry to supply cast iron mortar shells in exchange for military protection. |

---

## 32. MYSTERY ARCHITECTURE

ASHFALL features multi-layered Cold War mysteries and institutional secrets distributed across wasteland ruins and shortwave radio signals.

```mermaid
graph TD
    subgraph MysteryStatus["Mystery Knowledge Taxonomy"]
        ActiveMysteries["1. ACTIVE / DISCOVERABLE MYSTERIES<br/>• The Origin of The Machine's Census Directive<br/>• The Fate of the Northern Continental Convoy<br/>• The Secret Purpose of Substation Nine's Cable"]
        
        Ambiguous["2. DELIBERATELY AMBIGUOUS LORE<br/>• Who Fired the First Warhead? (Unanswerable)<br/>• The True Nature of the Horizon Flash (No Supernatural Truth)"]
        
        Reserved["3. RESERVED EXPANSION SEEDS<br/>• The Deep Submerged Keel of the Black Flotilla<br/>• The Sovereign Shelf Sub-Strata Vaults"]
    end
```

### Mystery Guidelines for Creative AI
1. **Never Reveal the First-Strike Instigator:** The nuclear exchange was an automated, multi-lateral cascade failure. Retaining geopolitical ambiguity reinforces the themes of bureaucratic tragedy and senseless ruin.
2. **Ground All Anomalies in Physics:** Strange radio signals are atmospheric ducting, numbers station loops, or automated telemetry beacons—never supernatural voices from beyond.
3. **Forensic Evidence Rewards:** Discovering lost documents should provide actionable intelligence (unlocking new coordinates or granting evidence items for The Machine's tribunal).

---

## 33. EXPANSION COLLISION ZONES

Certain design areas are already densely crowded. Adding features here risks redundant complexity, UI clutter, or system collisions.

```mermaid
graph TD
    subgraph CollisionZones["High-Collision Design Zones"]
        Zone1["Collision Zone 1: Generic Survival Meters<br/>(NeedsSystem already has 8 vitals; adding 'stress' or 'sanity' creates clutter)"]
        Zone2["Collision Zone 2: Parallel Faction Organizations<br/>(19 factions already exist; creating new raider gangs dilutes existing warlords)"]
        Zone3["Collision Zone 3: Duplicate Crafting Stations<br/>(6 stations already exist; adding 'apothecary bench' collides with ChemStation/PharmaLab)"]
    end

    subgraph SafeAlternative["Recommended Safe Alternatives"]
        Alt1["Deepen Psychological States<br/>(Hook into GuiltInsomnia or SomaticFlashbacks)"]
        Alt2["Deepen Existing Factions<br/>(Create splinter factions under Sector 4 Warlords)"]
        Alt3["Expand Existing Station Recipes<br/>(Add recipes to ChemStation or PharmaLab)"]
    end

    Zone1 ==> Alt1
    Zone2 ==> Alt2
    Zone3 ==> Alt3
```

---

## 34. CREATIVE WHITE SPACE

This section documents genuine, high-value architectural white space where ASHFALL's existing systems can support powerful new mechanics without creating redundant infrastructure.

### White Space 1: Tactical Interactive Physical Mini-Games
* **Evidence:** `RadioTuner.cs` implements frequency tuning sliders, and `PowerGridSystem.cs` tracks discrete room watt loads, but both operate primarily through standard Godot buttons.
* **Why Not Already Covered:** Existing UI panels present abstract numerical readouts rather than tactile, mechanical interactions.
* **Supporting Systems:** `RadioTuner`, `PowerGridSystem`, `AudioEventBridge`, `WorkshopReverseEngineeringSystem`.
* **Expansion Type:** `DATA + MINOR WIRING` (Host UI controls & audio feedback).
* **Concept:** Interactive physical interfaces: Manual circuit breaker rerouting boards during electrical surges, oscilloscopes for cleaning radio static, and mechanical tumblers for cracking pre-war safes.

### White Space 2: Surface Atmospheric Countermeasures & Cloud Seeding
* **Evidence:** `WeatherSystem.cs` implements 22 weather states, and `SilentFoundrySystem.cs` casts heavy mortar ordnance, but weather is currently purely passive and uncontrollable by the player.
* **Why Not Already Covered:** Weather is strictly an input; players currently have no tools to actively modify local atmospheric conditions.
* **Supporting Systems:** `WeatherSystem`, `SilentFoundrySystem`, `PharmaLabSystem`, `PowerGridSystem`.
* **Expansion Type:** `CORE EXTENSION` (Adding weather dispersion methods).
* **Concept:** Constructing a roof-mounted pneumatic mortar to launch chemical silver-iodide and lime dispersion shells into approaching fallout clouds, clearing ash storms or neutralizing acid rain over the shelter for 48 hours.

### White Space 3: Shelter Interior Visual Customization & Trophies
* **Evidence:** `ShelterAssignmentSystem.cs` manages room occupants, and `MemorialSystem.cs` tracks dead dwellers, but shelter walls lack interactive decorative customization.
* **Why Not Already Covered:** Shelter presentation is focused on operational workstation panels rather than spatial interior decoration.
* **Supporting Systems:** `ShelterAssignmentSystem`, `MemorialSystem`, `Inventory.cs`, `Theme.cs`.
* **Expansion Type:** `DATA + MINOR WIRING` (UI room view decor slots).
* **Concept:** Allowing players to hang scavenged propaganda posters, mount salvaged locomotive nameplates, and carve customized memorial plaques onto bunk walls to grant localized room morale buffs.

---


## 35. "ASK THESE QUESTIONS BEFORE DESIGNING" CHECKLIST

Future AI Game Mechanics & Narrative Brainstorming Gems must run every proposed concept through this 12-point design filter:

```text
ASHFALL EXPANSION DESIGN GATEWAY:

1. WHAT EXISTING STATE DRIVES IT?
   Does it read survivor vitals (NeedsSystem), dosimeter mSv (DoseLedger), weather kind (WeatherSystem), or faction standing (FactionStanceEngine)?
   -> If it invents a new standalone float or counter, STOP. Connect to an existing hub.

2. WHICH CORE SYSTEM OWNS THIS LOGIC?
   Is it an engine-agnostic C# system in Assets/Ashfall.Core/?
   -> If it puts gameplay rules in Godot UI nodes or scripts, STOP. Refactor to Core.

3. CAN EXISTING JSON SCHEMAS EXPRESS IT?
   Can this be authored as a door encounter, questline DAG, recipe, or trade tell?
   -> If yes, author it in StreamingAssets/Data/ without writing C# code.

4. WHAT PLAYER DECISION DOES IT CREATE?
   Does it force an agonizing trade-off between life support, labor productivity, and moral guilt?
   -> If it is a purely passive stat buff or trivial choice, redesign it.

5. WHAT EXISTING RESOURCE DOES IT PRESSURE?
   Does it burn clean water, diesel fuel, charcoal filters, medical antibiotics, or survivor health?
   -> If it introduces an unneeded 15th abstract currency, map it to existing physical items.

6. WHICH OTHER SYSTEMS CONSUME ITS OUTPUTS?
   Does a failure in this system cascade into sickness, power loss, labor strikes, or raider sieges?
   -> If its outputs terminate in an isolated dead-end, connect it to a high-leverage hub.

7. WHAT PERSISTS ACROSS SESSIONS?
   Does it implement CaptureState() and RestoreState() with versioned DTOs and SaveChecksum validation?
   -> Ensure state is cleanly serialized in JSON save envelopes.

8. HOW DOES THE PLAYER PERCEIVE IT IN GODOT?
   Is there a dedicated HUD progress bar, terminal text window, audio event cue, or modal dialog?
   -> Never create hidden mechanics that leave the player confused without clear UI feedback.

9. IS IT DETERMINISTIC UNDER ISeededRng?
   Does it avoid System.Random, Guid.NewGuid(), and unseeded GetHashCode()?
   -> Ensure all randomness is injected and deterministic.

10. DOES IT RESPECT THE GROUNDED TONE?
    Is it gritty, industrial, desperate, and strictly non-supernatural?
    -> Ensure mutations and tech are grounded in real-world physics and biology.

11. DOES IT AVOID REAL-WORLD GEOPOLITICS?
    Does it use fictional alliances (The Meridian Compact, The Northern Coalition)?
    -> Never reference real countries or alliances.

12. IS THIS AN EXTENSION RATHER THAN A DUPLICATION?
    Have you verified that ASHFALL_IMPLEMENTED_CANON_REGISTRY.md does not already implement this?
    -> Extend and deepen underused systems (§20) instead of inventing parallel mechanics.
```

---

## 36. AI RETRIEVAL KEYWORDS INDEX

This index provides optimized semantic search keywords for AI client retrieval across major gameplay categories.

```text
SURVIVOR PSYCHOLOGY & VITALS:
Retrieval Keywords: hunger, thirst, fatigue, warmth, morale, health, hygiene, radiation, vitals, needs, ptsd, trauma, shell shock, hypervigilance, guilt, insomnia, sleeplessness, conscience, somatic flashback, sensory trigger, acoustic trauma, panic, mental break, final wish, deathbed request, caregiving, bedside vigil, convalescence, roommate compatibility, ideological friction, grief cascade, mediation, ration conflict, food theft, mutiny, skill tree, xp progression, skill atrophy, trade specialty, trauma bond, leader stress, moral alignment, hardening, phantom memory, memento, heirloom, dosimetry, dosimeter, dose ledger, triage, sick list, palliative care, turntable, vinyl records, utility ai, response curves.

MEDICAL PATHOLOGY & PHARMA:
Retrieval Keywords: disease, cholera, zoonotic flu, blood fever, spore blight, epidemic, vector, waterborne, airborne virus, sepsis, fungal spore, acute radiation syndrome, ars, radiation sickness phases, prodromal, latent phase, manifest illness, chronic fibrosis, respiratory degeneration, black lung, chemical dependency, opioid addiction, morphine, raw opium, painkiller withdrawal, sedative abuse, alcoholism, stimulant dependency, pharma lab, drug synthesis, distillation phases, purity target, contamination risk, medical ward, hospital beds, surgical clinic, isolation ward, chelation therapy, edta, prussian blue.

SHELTER ENGINEERING & HEAVY INDUSTRY:
Retrieval Keywords: power grid, generator watts, battery storage, rolling blackouts, load shedding priorities, water purification, slow sand filtration, schmutzdecke, ozone tower, chlorine titration, activated carbon, brine extraction, salt pans, ventilation, air filters, hepa filter, radon venting, silent foundry, cupola furnace, metallurgy, crucible casting, firebrick repair, slag leaching, labor strike, greenhouse, hydroponics, crop cultivation, century seed, airlock security, decontamination interlock, blast door, excavation, tunneling, timber shoring, cave-in, material shielding, sky armor, roof fortification, kinetic harrow, workshop, reverse engineering, relic blueprints, duty roster, shift schedule, ink vs pencil, roster burn.

EXPEDITIONS, COMBAT & THE WASTELAND:
Retrieval Keywords: expedition stances, cautious, aggressive, stealth, scavenge stance, wasteland map, danger tiers, black zone, route distance, deep coast, marine survey, stealth dive, underwater salvage, diving oxygen, acoustic noise, expedition vehicles, scrap rig, armored scout, steam crawler, waystations, forward base, traveling caravans, weather states, fallout storm, black rain, acid snow, bio fog, emp storm, rad hail, tactical combat, combat lanes, close range, overwatch stance, ballistics, bullet penetration, ricochet, armor deflection, weapon fouling, gun jamming, clearing jams, combat perks.

FACTIONS, NARRATIVE & ENDGAME:
Retrieval Keywords: iron garrison, central garrison, ash militia, cult of the ash sign, warlords of sector 4, the tollman, toll doctrine, raiding doctrine, silent foundry, the scale, the underwrite, the compact, the office, the cutters, the fleet, the overlay, hydro barons, radio tuner, frequency dial, signal lock, snr meter, numbers station, faction radio, silence events, journal codex, forensic documents, autopsies, the machine tribunal, reckoning phases, evidence ledger, memorial eulogies, wall carvings, epilogue matrix, 32 permutations, regional fate, holdfast endings, verdict endings, muster epilogues.
```

---

## 37. EVIDENCE INDEX & REPOSITORY AUDIT DIRECTORY

### Authoritative Repository File Manifest

#### 1. Core Simulation Systems (`Assets/Ashfall.Core/` — 318 Files)
* **Foundation & Ports:** `Ports.cs`, `SaveChecksum.cs`, `SaveWireContract.cs`, `HostDefaults.cs`, `StableHash.cs`, `MathfCompat.cs`.
* **Survivor Simulation:** `Survivors/NeedsSystem.cs`, `Survivors/SurvivorCatalog.cs`, `Survivors/CombatTraumaSystem.cs`, `Survivors/GuiltInsomniaSystem.cs`, `Survivors/SomaticFlashbackSystem.cs`, `Survivors/FinalWishSystem.cs`, `Survivors/CaregivingSystem.cs`, `Survivors/IdeologicalFrictionSystem.cs`, `Survivors/LeadershipSystem.cs`, `Survivors/MoralBranchingSystem.cs`, `Survivors/RationConflictSystem.cs`, `Survivors/SkillProgressionSystem.cs`, `Survivors/SkillAtrophySystem.cs`, `Survivors/TradeSpecialtySystem.cs`, `Survivors/TraumaBondSystem.cs`, `SurvivorRelationsSystem.cs`, `CohortSystem.cs`, `GenerationalLineageExtension.cs`, `PhantomMemoryEngine.cs`, `DoseLedgerSystem.cs`, `SickListSystem.cs`, `VinylMoraleSystem.cs`.
* **Medical & Pathology:** `Medical/MedicalWardSystem.cs`, `Medical/ChemicalDependencySystem.cs`, `Medical/RespiratoryDegenerationSystem.cs`, `Medical/VigilStateMachine.cs`, `Disease/DiseaseSystem.cs`, `Disease/DiseaseCatalog.cs`, `Radiation/RadiationSystem.cs`, `Radiation/RadiationPhaseProgression.cs`, `PharmaLabSystem.cs`.
* **Shelter Engineering:** `Shelter/PowerGridSystem.cs`, `Shelter/ShelterAssignmentSystem.cs`, `Shelter/MaterialShieldingSystem.cs`, `Shelter/SkyLayerArmorSystem.cs`, `Greenhouse/GreenhouseSystem.cs`, `Foundry/SilentFoundrySystem.cs`, `DutyRoster/DutyRosterSystem.cs`, `StartingLevel/StartingLevelSystem.cs`, `AirlockSecuritySystem.cs`, `BrineWaterSystem.cs`, `ExcavationSystem.cs`, `VentilationSystem.cs`, `WaterTreatmentSystem.cs`, `WorkshopReverseEngineeringSystem.cs`, `WildlifeTrappingSystem.cs`.
* **Expeditions & Combat:** `Expeditions/ExpeditionSystem.cs`, `Expeditions/ExpeditionEncounterBridge.cs`, `Expeditions/ScavengeEngine.cs`, `ExpeditionVehicleSystem.cs`, `District8DeepCoastSystem.cs`, `Maritime/StealthDiveInstance.cs`, `Maritime/ProceduralScavengeSystem.cs`, `WaystationSystem.cs`, `TravelingCaravanSystem.cs`, `Combat/TacticalCombatSystem.cs`, `Combat/BallisticsSystem.cs`, `Combat/WeaponConditionSystem.cs`, `Combat/CombatPerks.cs`.
* **World & Environment:** `World/WastelandMapSystem.cs`, `World/WeatherSystem.cs`, `World/WeatherAtmosphereMap.cs`, `WeatherStationSystem.cs`, `OrbitalHarrowTelemetrySystem.cs`, `WeatherKind.cs`.
* **Economy & Trade:** `Inventory/Inventory.cs`, `Inventory/ItemDefinitions.cs`, `Economy/MarketSystem.cs`, `Economy/HardcoreEconomyTuning.cs`, `Economy/TradeScreenSeam.cs`, `Economy/TradeTellEngine.cs`, `Economy/CaravanAtomicTrader.cs`, `LedgerDebtSystem.cs`, `HoldfastTradeSession.cs`.
* **Factions & Warlords:** `Warlords/WarlordDoctrineSystem.cs`, `Warlords/WarlordDoctrineCatalog.cs`, `Warlords/WarlordResponseActions.cs`, `Economy/FactionStanceEngine.cs`, `RegionalTreatySystem.cs`.
* **Narrative, Radio & Endgame:** `Radio/RadioTuner.cs`, `Radio/FactionRadioEngine.cs`, `Journal/JournalSystem.cs`, `Verdict/MachineLogSystem.cs`, `Verdict/ReckoningSystem.cs`, `Verdict/EvidenceLedger.cs`, `Verdict/VerdictEndingEvaluator.cs`, `Muster/MusterSystem.cs`, `Endgame/EpilogueMatrixRuntime.cs`, `Endgame/EpilogueChronicleBuilder.cs`, `HoldfastEndings.cs`, `Memorial/MemorialSystem.cs`, `YearOfAsh/DoorEncounterSystem.cs`, `YearOfAsh/QuestlineSystem.cs`, `YearOfAsh/FactionWarSystem.cs`.

#### 2. Godot Presentation Host (`src/` — 203 Files)
* `Main.cs`: Monolithic host orchestrator (7,014 lines).
* `Host/HostCli.cs` & `HostCli.PanelTests.cs` & `HostCli.SelfTests.cs`: Headless CLI selftest suite.
* `Host/`: 27 HostSessions and 30 Checksummed Save Stores.
* `UI/`: 60+ Godot Control Panels and Overlays.

#### 3. Authoritative Data Catalogs (`Assets/StreamingAssets/Data/` — 296 Catalogs)
* Master Catalogs: `items.json` (499), `locations.json` (105), `survivors.json` (102), `recipes.json` (32), `events.json` (77), `radio.json` (50), `door_encounters.json` (68), `questline_master.json` (194), `world_history.json` (79), `faction_lore.json` (19).
* Narrative Codex: 196 specialized technical and lore files in `narrative/*.json`.

#### 4. Automated Verification Suite (`Ashfall.Core.Tests/` — 213 Files)
* 2,194 automated xUnit unit tests verifying simulation logic, schemas, determinism, and save roundtrips.

---

## 38. AUDIT COMPLETION STATEMENT

* **Audited Git Commit SHA:** `c900210cf6f39442975b8a36ed10322a6ab0d4ef`
* **Directories Inspected:** Full sweep of `Assets/Ashfall.Core/`, `src/`, `Ashfall.Core.Tests/`, `Assets/StreamingAssets/Data/`, `_quarantine_legacy/`, root configuration, and documentation.
* **Core Systems Traced:** 318 C# engine-agnostic systems.
* **Godot Host Systems Traced:** 203 C# presentation, session, and save store files.
* **JSON Catalogs Inspected:** 296 authoritative data catalogs (59,133 lines).
* **Extension Seams Documented:** 5 primary architectural hook categories with concrete code bindings.
* **Persistent State Flows Mapped:** 8 major state categories across 30 save stores.
* **Cross-System Relationships Mapped:** 7 deep emergent causal chains and 3 high-value underused pairings.
* **Creative White Spaces Identified:** 3 verified architectural white space domains.
* **Deliberate Omissions:** System inventory lists and raw content counts were deliberately omitted to prevent duplication with `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md`.
* **Audit Status:** **OFFICIALLY SIGNED OFF & CERTIFIED AS THE EXPANSION CONTEXT ATLAS.**
