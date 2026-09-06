# PLANS 72–75 AUTHORITY MAP
## Campaign Strategic Information & Faction Conflict Architecture

**Document Version:** 1.0.0
**Scope:** Plan 72 (Muster Warfare), Plan 73 (Wasteland Cartography), Plan 74 (Codex Knowledge Surface), Plan 75 (Daily Briefing)
**Status:** Authoritative Architectural Mapping & Invariant Register
**Primary Directive:** Simulation systems own facts; presentation surfaces aggregate, attribute, order, and display them. No presentation surface may own or forge campaign truth.

---

## 1. Executive Mission & Cross-Domain Truth

Plans 72–75 establish the strategic information and faction warfare spine of ASHFALL. Every piece of information displayed to the player must trace cleanly to an authoritative producer.

```text
 Faction / Military       Exploration / Radio       Research / Field Guide       Life Support / Hazards
         │                         │                          │                            │
         ▼                         ▼                          ▼                            ▼
   Plan 72: Muster        Plan 73: Wasteland Map        Plan 74: Codex          Plan 75: Daily Briefing
 (Forces, Roster, War)     (Fog, Route, Survey)       (Knowledge Projection)      (Dawn Intelligence)
         │                         │                          │                            │
         └─────────────────────────┼──────────────────────────┴────────────────────────────┘
                                   │
                                   ▼
                   Campaign Information Spine
            (Typed Provenance · Confidence · Day · Source ID)
```

---

## 2. Core Authority Matrix

| Domain | Authoritative State Owner | Producer / Mutation API | Strategic Consumers | Save File & Store | Save Shape |
|---|---|---|---|---|---|
| **Muster Warfare** (Plan 72) | `MusterSystem` & `MusterState` / `MusterWarfareState` | `Mobilize()`, `CommitSupplies()`, `ResolveEngagement()`, `Withdraw()` | Muster UI, Daily Briefing, Faction War, Escalation | `muster_save.json` via `MusterSaveStore` | Checksummed Envelope (`MusterState`) |
| **Wasteland Map & Fog** (Plan 73) | `WastelandMapSystem` & `WastelandMapState` | `Discover()`, `Survey()`, `RecordObservation()`, `SetLocked()` | Wasteland Map View, Expedition System, Codex, Briefing | `wasteland_map_save.json` via `WastelandMapSaveStore` | Checksummed Envelope (`WastelandMapState`) |
| **World Evolution** (Host / World) | `DynamicWorldSystem` & `WorldState` | `AdvanceDay()`, `EvolveLocations()` | Wasteland Map, Expeditions, Briefing | `world_save.json` via `WorldSaveStore` | Checksummed Envelope (`WorldSaveState`) |
| **Radio Intercepts** | `RadioDistressSystem` & `RadioSave` | `Tune()`, `DecryptSignal()`, `Triangulate()` | Wasteland Map (rumor nodes), Daily Briefing | `radio_save.json` via `RadioSaveStore` | Checksummed Envelope (`RadioSave`) |
| **Field Guide Ledger** | `FieldGuideCatalog` & `FieldGuideState` | `UnlockEntry()`, `RecordObservation()` | Codex Knowledge Surface, Wasteland Map (geology/flora traits) | `field_guide_save.json` via `FieldGuideSaveStore` | Checksummed Envelope (`FieldGuideState`) |
| **Research Progression** | `ResearchSystem` & `ResearchState` | `CompleteResearch()`, `AdvanceResearch()` | Codex Knowledge Surface, Daily Briefing, Crafting | `research_save.json` via `ResearchSaveStore` | Checksummed Envelope (`ResearchState`) |
| **Journal Knowledge** | `JournalSystem` & `KnowledgeBase` | `TryDiscoverKnowledge()`, `AddKnowledgeEvidence()` | Codex Knowledge Surface, Journal Book | `journal_save.json` via `JournalSaveStore` | Checksummed Envelope (`JournalSave`) |
| **Daily Briefing** (Plan 75) | **DERIVED ONLY** (`DailyBriefingReport`) | `DailyBriefingReportBuilder.BuildFromDayEvents()` / typed collectors | Daily Briefing Modal UI | `daily_briefing_save.json` via `DailyBriefingSaveStore` | **Cadence & Dismissal Markers ONLY** (no report body) |
| **Codex Surface** (Plan 74) | **DERIVED ONLY** (`CodexEntryProjection`) | `CodexProjectionBuilder.Build()` | Codex Atlas Panel UI | **ZERO PERSISTENCE** (pure functional projection) | N/A (Derived) |

---

## 3. Pillar-by-Pillar Authority Details

### 3.1 Plan 72: Muster Warfare & Faction Rally Depth
- **Branch Authority:** `FactionBranchCoordinator` owns the player's primary faction alignment (`MilitaryBranchCatalog`, `RebelBranchCatalog`, `IndependentBranchCatalog`). Committing to a branch locks out competing branches via Point-of-No-Return (PoNR).
- **Doctrine Authority:** `warlord_doctrines.json` via `WarlordDoctrineSystem` and typed doctrine modifiers (`MusterDoctrineModifier`). Modifiers adjust recruitment cost, supply burn, risk tolerance, and retreat thresholds.
- **Roster Authority:** Shelter survivors (`ISurvivorRoster`). Combatants assigned to a muster force MUST be real living survivors, present in the shelter, not incapacitated, and free of conflicting duty assignments. Ghost soldiers are strictly prohibited.
- **Supply Authority:** Shelter inventory (`InventorySystem`). Munitions, food, medicine, and equipment are reserved upon mobilization and consumed/returned upon engagement resolution via atomic inventory transactions.
- **Engagement Authority:** Combat resolution delegates to the established combat and pressure authority (`TacticalCombatSystem` / `CombatSimulationSession`). Muster does NOT create a parallel combat resolution math engine.
- **Consequence Routing:**
  - Fallen combatants → `MemorialSystem` and survivor roster death.
  - Wounded combatants → `MedicalSystem` (trauma, infection, injury).
  - Geopolitical impact → `FactionWarSystem` (standing, territorial control, tension).
  - Historical record → `JournalSystem` entry.

### 3.2 Plan 73: Strategic Wasteland Cartography & Fog Depth
- **Catalog & Nodes:** `wasteland_map_v1.json` defines nodes and route edges.
- **Fog of War States:**
  - `Unknown`: Node is uncharted; no coordinates or details rendered.
  - `Rumored`: Sourced from radio intercept, trader gossip, or distant sighting. Degraded precision: approximate region, generic category, fuzzy danger rating. NEVER reveals exact loot tables or precise route travel duration.
  - `Surveyed`: Sourced from cartographic recon or specialized tech (e.g. seismic fault mapping). Exact coordinates, confirmed danger band, and geological/hydrological traits unlocked.
  - `Visited`: Confirmed by survivor expedition arrival. Full ground truth unlocked.
- **Route Estimation:** Map UI route projections MUST delegate directly to `ExpeditionSystem.Estimate` to guarantee 100% parity with live expedition dispatch calculations.
- **World Evolution Separation:** World changes (e.g. fallout migration, outpost destruction) belong to `DynamicWorldSystem`. The map maintains last-known intelligence with explicit observation timestamps ("Confirmed Day 14").

### 3.3 Plan 74: Codex & Field-Guide Knowledge Surface
- **Pure Derived Surface:** The Codex possesses zero save-state. An entry is visible if and only if at least one authoritative source verifies it.
- **Knowledge Sources:**
  1. `FieldGuideState`: Flora, fauna, ecology discoveries.
  2. `ResearchState`: Completed technologies and active research ("Studying").
  3. `JournalSystem.Knowledge`: Visited sites, met survivors, witnessed events.
  4. `Manuals & Autopsies`: `manual_*` and `autopsy_*` discoveries logged in knowledge base.
  5. `Narrative Articles`: Authored articles (`article_*`) unlocked via specific game achievements or discoveries.
- **Deduplication & Union:** Facts sharing a stable `KnowledgeFactId` merge into a single entry with unioned provenance tags and the earliest discovery day preserved.
- **Ordering:** Deterministic sort by Category Ordinal → State (Known > Studying) → Title Key → Stable Fact ID.

### 3.4 Plan 75: Daily Briefing System & Dawn Intelligence
- **Taxonomy:**
  - `CRITICAL`: Life-support failure, lethal disease outbreak, impending raid, structural collapse.
  - `WARNING`: Resource deficit, weather degradation, minor dispute, machine wear.
  - `INTEL`: Faction movements, map rumors, completed research, intercepted broadcasts.
  - `FLAVOR`: Sourced atmospheric observations from survivors.
- **Typed Collector Contract:** Systems implement `IBriefingFactCollector` or expose `IReadOnlyList<BriefingFact> CollectBriefingFacts(int day)`.
- **Cadence & Deduplication:**
  - First occurrence triggers reporting.
  - Unchanged persistent states are suppressed after initial report.
  - Severity escalation or recovery status triggers re-reporting.
  - Critical conditions re-alert on a configured cadence (e.g., every 3 days).
- **Deep-Links:** Every actionable briefing fact provides a typed navigation route verified against `PanelRegistry`.

---

## 4. Invariant & Boundary Checklist

- [x] Invariant 1: Zero engine coupling in Core (`Assets/Ashfall.Core/`).
- [x] Invariant 2: Ports and adapters used for all IO, logging, clock, and RNG.
- [x] Invariant 3: Checksummed save envelopes for all persisted systems.
- [x] Invariant 4: Deterministic simulation using `ISeededRng` and ordinal sorting.
- [x] Invariant 5: Godot host nodes handle only UI presentation, user input, and wiring.
- [x] Invariant 6: JSON data files in `Assets/StreamingAssets/Data/` remain authoritative.
