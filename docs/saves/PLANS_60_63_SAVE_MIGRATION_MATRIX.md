# Plans 60–63 Save Migration Matrix

**Plan ID:** AF-B1-B4-P60-P63-SAVES
**Coverage:** Radio Station Authority (Plan 60) × Library Manuals & Research Discovery (Plan 61) × Tactical Combat Lifecycle (Plan 62) × Disease Quarantine Policy (Plan 63)
**Authority:** Core `SaveStore<T>` / `SaveEnvelopeHelper` with reflection-based `SaveChecksum`
**Compatibility Policy:** Backward compatible with V1/legacy bare saves; atomic envelope persistence via `campaign.json` or standalone `.json`.

---

## 1. System Save Envelope & Schema Overview

| Pillar | Domain / Store | Current Schema Version | Envelope Format | Atomic Write | Legacy Fallback | Checksum Gated |
|---|---|---|---|---|---|---|
| **B1 (Plan 60)** | Radio (`RadioSaveStore` / `RadioHostSession`) | `v1` | `{ State, Checksum }` | Yes (`SaveStore<T>`) | Preserved (bare dictionary) | Yes (`SaveChecksum`) |
| **B2 (Plan 61)** | Library Study (`LibraryStudySaveStore`) | `v1` | `{ State, Checksum }` | Yes (`SaveStore<T>`) | Supported | Yes (`SaveChecksum`) |
| **B3 (Plan 62)** | Tactical Combat (`TacticalCombatSaveStore`) | `v1` | `{ State, Checksum }` | Yes (`SaveStore<T>`) | Supported | Yes (`SaveChecksum`) |
| **B4 (Plan 63)** | Disease System (`DiseaseSaveStore`) | `v2` (bumped from v1) | `{ State, Checksum }` | Yes (`SaveStore<T>`) | V1 legacy supported | Yes (`SaveChecksum`) |
| **B4 (Plan 63)** | Medical Ward (`MedicalWardSaveStore`) | `v1` | `{ State, Checksum }` | Yes (`SaveStore<T>`) | Supported | Yes (`SaveChecksum`) |

---

## 2. Detailed Schema Contracts & Version Bumps

### 2.1 Pillar B1 — Radio Authority (`radio_stations.json` + `RadioSaveStore`)
- **Data Authority:** `Assets/StreamingAssets/Data/radio_stations.json` (schema_version 1).
- **State Capture:** Stations loaded from JSON data authority. `RadioSystemState` tracks tuned frequencies, signal strength contexts, and station states (`Normal`, `Jammed`, `Silent`, `Offline`).
- **Migration & Unknown Preservation:**
  - If a save references a station ID not in the local catalog (e.g. from an expansion or mod), the state is preserved rather than dropped.
  - Default stations fall back to data authority definitions without silent overwrites.

### 2.2 Pillar B2 — Library Manuals & Research Discovery (`LibraryStudyState`)
- **State Fields:**
  - `activeManualId`: string (currently studied manual, or empty).
  - `activeReaderSurvivorId`: string (survivor currently committed to study).
  - `studyProgress`: float (accumulated comprehension progress).
  - `completedManualIds`: `List<string>` (set of manuals fully comprehended).
  - `discoveredKnowledgeIds`: `List<string>` (knowledge nodes unlocked/revealed via `UnlockManual`).
- **Invariants:**
  - Manual study discovers and unlocks knowledge nodes (`UnlockManual`); it **never** auto-completes research (`CompleteResearch()`).
  - Active reader survivor has bidirectional reservation with `DutyRosterSystem`. On reload, `Rehydrate` re-attaches the external reservation.

### 2.3 Pillar B3 — Tactical Combat Mid-Encounter Lifecycle (`TacticalCombatState`)
- **State Fields:**
  - `EncounterId`, `ExpeditionId`, `LocationId`, `LocationDisplayName`, `Day`, `Seed`.
  - `Phase`: `NotStarted`, `PlayerTurn`, `EnemyTurn`, `Victory`, `Defeat`, `Fled`.
  - `TurnNumber`: int.
  - `Roster`: `List<CombatantState>` (Health, Armor, StatusEffects, Morale).
  - `Weapons`: `List<WeaponInstanceState>` (ConditionPct, AmmoRemaining, Jams).
  - `ActionPointPool`: AP distribution per actor.
  - `Aftermath`: `CombatAftermath` record populated once at encounter end (guaranteed exactly-once application).
  - `CombatResolutionId`: string unique resolution token preventing duplicate consequence application.
- **Mid-Encounter Save/Restore Determinism:**
  - A save captured mid-encounter and subsequently restored produces byte-identical resolution when seeded with identical PRNG continuation.

### 2.4 Pillar B4 — Disease Catalog & Quarantine Policy (`DiseaseSystemState` v2)
- **Version Bump:** `v1` → `v2`.
- **New Fields in V2:**
  - `DiseaseInfectionState.current_stage`: string (one of the 8 canonical stages: `Incubating`, `Prodromal`, `Acute`, `Severe`, `Critical`, `Convalescent`, `Chronic`, `Recovered`).
  - `DiseaseInfectionState.stage_entered_day`: int (tracking clinical stage progression).
  - `immunities`: `Dictionary<string, List<DiseaseImmunityRecord>>` keyed by `survivorId`.
- **Immunity Record Shape:**
  ```csharp
  public sealed class DiseaseImmunityRecord
  {
      public string DiseaseId;
      public int AcquiredDay;
      public int ExpiryDay;
      public float Strength; // 1.0f = full immunity
  }
  ```
- **Legacy V1 Migration Fallback:**
  - When loading V1 saves missing `current_stage`, `DiseaseSystem` maps `days_sick` against the catalog phase definitions to deduce the active clinical stage.
  - If `immunities` is missing or null, it initializes an empty immunity registry.
  - Pre-v2 bare saves load cleanly through the `allowLegacyBareState` adapter in `SaveEnvelopeHelper`.

---

## 3. Atomic Persistence & Checksum Integrity

1. **Atomic Write Pipeline:**
   - All host session stores write via `SaveStore<T>.Save(state, slot)` or `CampaignEnvelopeBuilder`.
   - File writes target `<filename>.tmp` before atomic rename to prevent corruption on crash/power loss.
2. **Checksum Verification:**
   - `SaveChecksum.Calculate(state)` generates a deterministic culture-invariant hash across all public serialized properties.
   - On load, `SaveEnvelopeHelper.Unwrap` validates that the payload matches the recorded checksum. If corrupted, load fails explicitly (`checksum mismatch`) rather than producing undefined simulation state.
3. **Multi-Section Campaign Envelope Integration:**
   - When running the integrated 30-day campaign pipeline, each system captures state to in-memory persisted bytes via `SaveStore<T>.CapturePersisted()`, which `CampaignEnvelopeBuilder` seals into `campaign.json` (manifestVersion 2).

---

## 4. Verification Evidence

- **Unit Tests:**
  - `Ashfall.Core.Tests/SaveStoreChecksumSweepTests.cs`: 100% pass across all envelopes.
  - `Ashfall.Core.Tests/BareSaveStoreSealTests.cs`: Pre-checksum legacy fallback verified.
  - `Ashfall.Core.Tests/Save/SaveStoreServiceTests.cs`: Byte-preservation and atomic write verified.
- **Integration Tests:**
  - `Ashfall.Core.Tests/Integration/Plans60To63ThirtyDayIntegrationTests.cs`: Mid-campaign Day 20 full save and restore verified with exact immunity and infection history preservation.
