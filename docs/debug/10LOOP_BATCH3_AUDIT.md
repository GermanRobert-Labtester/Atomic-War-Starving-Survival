# ASHFALL 10-Loop Bug Audit

**Target:** Latest ASHFALL commit `eeff1f79` ("feat(batch3-phases1-3): implement 12 Batch 3 systems + Core support additions") and its integration seams with the active Godot 4.7+ host (`src/`).

**Audit commit SHA:** `eeff1f79d003f0a03714f2e8022c464ace76a132`

## 1. Audit Target

The 12 Batch 3 systems shipped in `eeff1f79`, their Core support additions, the 4 host sessions, 4 save stores, and 4 UI panels that the commit message claims to deliver, plus the touchpoints in `src/Main.ExpandedShelterSystems.cs` that wire them.

Core systems introduced:

| # | System | Path |
|---|--------|------|
| 1 | `ShelterThermalSystem` | `Assets/Ashfall.Core/ShelterThermalSystem.cs` |
| 2 | `SumpFloodingSystem` | `Assets/Ashfall.Core/SumpFloodingSystem.cs` |
| 3 | `DecontaminationSystem` | `Assets/Ashfall.Core/DecontaminationSystem.cs` |
| 4 | `ShelterScheduleSystem` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| 5 | `KitchenNutritionSystem` | `Assets/Ashfall.Core/KitchenNutritionSystem.cs` |
| 6 | `EquipmentConditionSystem` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| 7 | `LibraryStudySystem` | `Assets/Ashfall.Core/LibraryStudySystem.cs` |
| 8 | `ApprenticeshipSystem` | `Assets/Ashfall.Core/ApprenticeshipSystem.cs` |
| 9 | `ArchiveDeskSystem` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| 10 | `ContractorRosterSystem` | `Assets/Ashfall.Core/ContractorRosterSystem.cs` |
| 11 | `MentalHealthCrisisSystem` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| 12 | `AutopsySystem` | `Assets/Ashfall.Core/AutopsySystem.cs` |

Core support additions: `Inventory.AddById` (stackMax=99); `JournalSystem.AddKnowledgeEvidence` bridge.

## 2. Scope

- Core files for the 12 systems + their tests in `Ashfall.Core.Tests/`.
- Host wiring files: `src/Main.ExpandedShelterSystems.cs`, `src/Host/ShelterThermalHostSession.cs`, `src/Host/ShelterScheduleHostSession.cs`, `src/Host/ApprenticeshipHostSession.cs`, `src/Host/AutopsyHostSession.cs`, save stores for the 4 connected systems, the 4 UI panels.
- `godot --headless` verification was NOT run (would touch UI only); this audit stayed at static evidence + xUnit pass/fail + dependency reachability.

## 3. Baseline Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   → Succeeded, 4 warnings (CS8601/CS8602, pre-existing), 0 errors
dotnet test  Ashfall.Core.Tests --filter "<12 Batch 3 names>" → Passed 102/102, Failed 0
```

The 102 Batch 3 tests cover unit behaviour of every system but do NOT cover host wiring — see Loop 9.

## 4. Loop Completion Matrix

| Loop | Lens | Candidates examined | Confirmed | Rejected |
|---|---|---|---|---|
| 1 | Structural / Static | 17 | 11 | 6 |
| 2 | Call-graph / Reachability | 12 systems × host | 12 | 0 |
| 3 | State transitions | 14 models | 5 | 9 |
| 4 | Save / Load / Restore | 4 stores + 12 DTOs | 3 | 9 |
| 5 | Determinism / Ordering | 12 systems × RNG | 0 | 12 |
| 6 | Data / ID / Catalog | 4 catalogs | 2 | 6 |
| 7 | Event / Lifecycle / Integration | 18 events × src/ | 4 | 14 |
| 8 | UI / Player-facing | 4 panels + 8 orphan | 3 | 9 |
| 9 | Test adversarial | 102 tests + harness | 4 | 0 |
| 10 | Cross-system synthesis | 6 chains | 6 (clusters) | 0 |

(Candidate counts here describe the active suspicion pool per loop, not the final finding count.)

## 5. Executive Findings

The Batch 3 deliverable is **architecturally half-shipped**.

- **8 of 12 systems never reach the Godot runtime.** They compile, they pass xUnit, and they ship with `CaptureState/RestoreState`, but no host session, host binding, save store, tick wiring, or UI panel exists for them in `src/`. Their state is authoritative on paper, but zero authoritative in practice — every state mutation the player would observe never happens.

- **The 4 systems that did reach host integration have empty catalogs** (procedures, manuals, inks, schedules) because no host code calls `LoadCatalog/LoadInkCatalog`. Any UI panel interaction that the player *can* see returns a "unknown_*" error.

- **Even the wired thermal system silently discards computed survivor warmth**, contradicting a code comment that claims host wires survivor warmth — it does not.

The 8 orphan systems are: `SumpFloodingSystem`, `DecontaminationSystem`, `KitchenNutritionSystem`, `EquipmentConditionSystem`, `LibraryStudySystem`, `ArchiveDeskSystem`, `ContractorRosterSystem`, `MentalHealthCrisisSystem`.

The 4 wired systems are: `ShelterThermalSystem`, `ShelterScheduleSystem`, `ApprenticeshipSystem`, `AutopsySystem`.

The root cause clusters in §12 explain *why* this split happened (Phase 4 of the Expansion System protocol — wire into GameBootstrap — was performed for 4 of 12).

---

## 6. Critical Findings

### BUG-01 — 8 of 12 Batch 3 systems are unreachable from the Godot runtime

**Severity:** CRITICAL
**Confidence:** CONFIRMED
**Category:** INTEGRATION BUG / ARCHITECTURAL FORK
**Active Runtime:** NO for orphan systems; YES for the 4 wired ones.
**Player Impact:** Eight documented game systems are missing entirely from play.
**Trigger:** Loading the game.
**Expected:** Per the commit message and the AGENTS.md Batch 3 plan, all 12 systems ship wired; the player can engage with sump flooding, decontamination, kitchen, equipment condition, library study, archive desk, contractor hires, and mental-health crisis through UI panels and tick effects.
**Actual:** Eight of the twelve Core classes have zero references anywhere in `src/` — no host session, no save store, no UI binding, no tick registration. They execute only inside xUnit tests.

**Root Cause:** The Batch 3 plan lists 12 systems under full Phase 1–4 lifecycle (system class → data → IDs → GameBootstrap wiring → tests). Only the first three phases were completed; Phase 4 (host wiring) was performed for 4 of 12. The author stopped at "Core compiles + tests pass", which AGENTS.md framework (`Expansion System` §5) explicitly warns against: "*systems constructed/registered/ticked but key effects are stubs*".

**Evidence:**

```
$ grep -rn "SumpFloodingSystem\|DecontaminationSystem\|KitchenNutritionSystem\|EquipmentConditionSystem\|LibraryStudySystem\|ArchiveDeskSystem\|ContractorRosterSystem\|MentalHealthCrisisSystem" src/
(no output)

$ ls src/Host/ | grep -E "MentalHealth|KitchenNutrition|SumpFlooding|EquipmentCondition|LibraryStudy|Archive|Contractor|Decontamination"
(no output)

$ src/Main.ExpandedShelterSystems.cs SetupExpandedShelterSystems :
  // 1, 2, 3, 4, 5, 6, 7, 11, 12 from prior batches are wired
  // 9. Shelter Thermal (Thermal)
  // 10. Shelter Schedule (Schedule)
  // 8. Apprenticeship                     ← only Phase 4-wired Batch 3
  // 11. Autopsy                           ← only Phase 4-wired Batch 3
```

The orphan systems only execute in:
- `Assets/Ashfall.Core/SumpFloodingSystem.cs`
- `Assets/Ashfall.Core/DecontaminationSystem.cs`
- `Assets/Ashfall.Core/KitchenNutritionSystem.cs`
- `Assets/Ashfall.Core/EquipmentConditionSystem.cs`
- `Assets/Ashfall.Core/LibraryStudySystem.cs`
- `Assets/Ashfall.Core/ArchiveDeskSystem.cs`
- `Assets/Ashfall.Core/ContractorRosterSystem.cs`
- `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`
and in xUnit test files (`Ashfall.Core.Tests/<Name>SystemTests.cs`). They never mutate authoritative gameplay state in a real game session.

**Affected Systems:** 8 of 12 Batch 3 systems, plus downstream consumers (survivor hunger dialogue, contractor expeditions, mental-health duty unassignment, library research unlocks, kitchen spoilage timers, atmospheric contamination propagation, equipment wear/jam cross-system reads).

**Save Impact:** Zero — orphan systems have no save stores. Future fix must add save stores for the 8 batch systems or players will lose all state on reload.

**Determinism Impact:** N/A (does not execute).

**Regression Risk:** Adding host wiring is a Phase 4 work; risk is moderate (no contract ambiguities in Core, but TickDay side-effects on survivor needs / inventory must be threaded through the existing host channels).

**Suggested Next Analysis:** Generate a Batch 3 Phase 4 wiring plan: for each of the 8 systems, design `HostSession` + `SaveStore` + `UI Panel` + `Main.ExpandedShelterSystems.cs` registration + `TickAllExpandedShelterSystems(day)` entry + a corresponding JSON catalog (CatalogPath + loader).

---

### BUG-02 — Empty catalogs for the 4 wired systems block all user actions

**Severity:** HIGH
**Confidence:** CONFIRMED
**Category:** DATA BUG / STATE BUG
**Active Runtime:** YES
**Player Impact:** Any UI-driven action on Shelter Schedule, Autopsy Report, Apprenticeship's manual context, Archive Desk returns "unknown_*" — every interaction fails.
**Trigger:** Player opens the Shelter Schedule panel and tries to switch schedule, or the doctor wants to start an autopsy.
**Expected:** Schedule catalog populated from `Assets/StreamingAssets/Data/...`, autopsy procedures loaded from JSON, etc.
**Actual:** Catalog dictionaries remain empty (no caller for `LoadCatalog`).

**Evidence:**

```
$ grep -n "LoadCatalog\|LoadInkCatalog" src/
(no output — except unrelated RegionalTreaty/VinylMorale/Workshop/PHARMA in Core)

$ grep -n "_inkCatalog\|_catalog" Assets/Ashfall.Core/{ShelterScheduleSystem,AutopsySystem,LibraryStudySystem,ArchiveDeskSystem}.cs
ShelterScheduleSystem.cs:53:_catalog only seeded with the baked-in "default" entry (line 72)
AutopsySystem.cs:51:_catalog → empty if no caller invokes LoadCatalog
ArchiveDeskSystem.cs:54:_inkCatalog → empty if no caller invokes LoadInkCatalog
LibraryStudySystem.cs:49:_catalog → empty if no caller invokes LoadCatalog
```

For `ShelterScheduleSystem`, the constructor seeds a single `"default"` entry, so the player can switch to that one schedule — but everything else is dead. For `AutopsySystem`, `ArchiveDeskSystem`, and `LibraryStudySystem`, every catalog mutation produces:

```
ArchiveDeskSystem.QueueTranscription: _inkCatalog.TryGetValue(inkId) → ActionResult.Failed("unknown_ink")
LibraryStudySystem.StartStudy: _catalog.TryGetValue(manualId) → ActionResult.Failed("unknown_manual")
AutopsySystem.BeginAutopsy: _catalog.TryGetValue(procedureId) → ActionResult.Failed("missing_procedure")
```

**Root Cause:** Phase 3 (data: update `items.json`, `locations.json`, etc., per AGENTS.md Expansion System protocol) was not executed for any of the 12 systems; consequently Phase 4 wiring inherited no JSON catalog loader. No JSON file matching `schedule|manual|ink|autopsy_procedure` exists in `Assets/StreamingAssets/Data/`.

```
$ find Assets/StreamingAssets/Data -name "schedule*.json" -o -name "autopsy*.json" -o -name "manual*.json" -o -name "ink*.json"
Assets/StreamingAssets/Data/narrative/inkle_loom_warp_tally_sheets.json   ← unrelated
```

**Affected Systems:** `ShelterScheduleSystem`, `AutopsySystem`, `LibraryStudySystem`, `ArchiveDeskSystem`.

**Save Impact:** N/A (catalog contents are in-memory only; absent catalog → permanent "unknown_*" returns).

**Determinism Impact:** N/A — does not affect RNG, only user-button responses.

**Regression Risk:** Adding a catalog loader is straightforward, but it must match the four DTO shapes (`ScheduleDefinition`, `AutopsyProcedure`, `ManualDefinition`, `InkMaterialDefinition`) — field names use `snake_case` (`schedule_id`, `manual_id`, `requiredItemId`, etc.), and `CatalogIntegrityValidator` will reject any ID missing the `snake_case` prefix rule.

**Suggested Next Analysis:** Author at least one JSON fixture per system, add a `LoadCatalog` call in `SetupExpandedShelterSystems` pointed at `CatalogPath`, then add an integration test.

---

## 7. High Findings

### BUG-03 — ShelterThermalSystem discards computed warmth inside `TickDay`

**Severity:** HIGH (when paired with BUG-04 it becomes surfacing as zero effect)
**Confidence:** CONFIRMED
**Category:** LOGIC BUG / INTEGRATION BUG
**Active Runtime:** YES (the thermal system DOES tick; the warmth loop is a known symptom)
**Player Impact:** In cold weather, survivors never receive warmth from radiator heat despite the boiler running. Frozen rooms never warm the survivors they share.
**Trigger:** Cold day, survivors in a room with the boiler active and valves open.
**Expected:** Warmth flows from room temperature into the survivor's `NeedsSystem.Warmth`.
**Actual:** `warmthDelta` is calculated and then thrown away; the comment claims "host reads this and applies to survivor warmth" but the host never reads `GetRoomWarmthModifier`.

**Evidence:**

```
$ sed -n '226,232p' Assets/Ashfall.Core/ShelterThermalSystem.cs
// Feed warmth to NeedsSystem (lightweight port — direct call is the sanctioned path)
foreach (var room in _state.rooms)
{
    float warmthDelta = room.currentTempC > 15f ? (room.currentTempC - 15f) * 0.1f : 0f;
    // NeedsSystem warmth is applied via event; here we just set the room temperature
    // The host session reads this and applies to survivor warmth
}

$ grep -rn "GetRoomWarmthModifier\|warmthDelta" src/
Assets/Ashfall.Core/ShelterThermalSystem.cs:265  // public method only
Assets/Ashfall.Core/ShelterThermalSystem.cs:229  // dead variable
```

`ShelterThermalHostSession.TickDay` (line 711) calls `System.TickDay(day)` only. It never queries `GetRoomWarmthModifier`. There is no `Warmth` modifier for room occupancy in `Survivors/NeedsSystem.cs`.

`_needs` is injected (`: 89`) but used 0 times across the entire `TickDay` body.

**Root Cause:** Phase 4 was attempted (host session exists) but the cross-system side (warmth propagation into `SurvivorsHostSession`) was lost. The added comment "host session reads this and applies to survivor warmth" is forward-looking fiction.

**Affected Systems:** Survivor needs (warmth), any downstream affinity that depends on warmth (cohort decisions, scribe writing tones).

**Save Impact:** N/A — symptom is at runtime.

**Determinism Impact:** None directly (no RNG in the dropped branch).

**Regression Risk:** Implementation must not double-count warmth (boiler fuel and incident prevent other paths adding warmth).

**Suggested Next Analysis:** Decide direction: (a) have `ShelterThermalSystem.TickDay` mutate survivor warmth directly via a passed `NeedsSystem` (per AGENTS.md `IEventBus` / "direct call is the sanctioned path"), OR (b) have the host iterate survivors-on-shift and apply `GetRoomWarmthModifier(roomId) * elapsedTime`. Reconciliation with existing `SurvivorsHostSession.warmth` formula is required.

---

### BUG-04 — `ShelterThermalSystem.TickDay` distributes heat per-room with hard-coded `0.1` multiplier

**Severity:** HIGH
**Confidence:** CONFIRMED
**Category:** LOGIC BUG
**Active Runtime:** YES
**Player Impact:** Adding more rooms actually *reduces* per-room heat, and the heat delivered is ~10× less than the displayed `totalHeatOutputKw` panel value. The user-visible "Boiler Output: 2.3 kW" label is a lie.
**Trigger:** Player adds rooms to the bunker.
**Expected:** Each room receives heat proportional to its share of boiler kW; adding rooms does not diminish per-room delivery rate.
**Actual:** Per-room `heatGain = totalHeatKw * room.radiatorValveOpen * roomShare / roomCount * 0.1f`. With 5 rooms and a 100 kW boiler output, each room receives ~2 kW (via the 0.1 multiplier), and valve fraction further scales it down. The displayed `totalHeatOutputKw` is rounded from a faked scale.

**Evidence:**

```
$ sed -n '170,205p' Assets/Ashfall.Core/ShelterThermalSystem.cs
float totalHeatKw = _state.boilerActive ? _state.boilerFuelLevel * 10f : 0f;
_state.totalHeatOutputKw = totalHeatKw;

foreach (var room in _state.rooms)
{
    ...
    float heatGain = 0f;
    if (_state.boilerActive && room.hasRadiator && room.radiatorValveOpen > 0 && !room.isFrozen)
    {
        float roomShare = room.isPriorityRoom ? 1.5f : 1f;
        heatGain = totalHeatKw * room.radiatorValveOpen * roomShare / Math.Max(1, _state.rooms.Count);
    }
    ...
    room.currentTempC += heatGain * 0.1f - heatLoss;
```

The `* 0.1f` appears to be a unit-conversion guess (kW → °C per day for a typical 50 m³ room). Combined with the `/ roomCount` divisor it is essentially a tuning placeholder.

**Root Cause:** Thermal mass and room volume exist (`room.volumeM3`) but are never used in the heat-distribution formula. The `* 0.1f` fudge is the only thermal-mass compensation. The result scales oddly with room count and the player has no observable feedback that the displayed kW matches effective delivery.

**Affected Systems:** All room temperatures, freeze-burst incidence, frozen-room contagion, downstream warmth (BUG-03).

**Save Impact:** N/A — symptom appears after save-load.

**Determinism Impact:** None.

**Regression Risk:** Replacing the formula must keep test `SetBoilerActive_HeatsRoom` passing while closing the kW discrepancy. Adding a unit conversion `1 kW × 1 day / (volume × Cp)` (with air Cp ≈ 1.005 kJ/kg·K and density ≈ 1.2 kg/m³) would be physics-correct.

**Suggested Next Analysis:** Either implement proper thermal-mass math, or expose a pending "design TBD" flag in the README — keep displayed kW muted during placeholder phase.

---

## 8. Medium Findings

### BUG-05 — `MentalHealthCrisisSystem.CrisisStatus.Chronic` is unreachable

**Severity:** MEDIUM
**Confidence:** CONFIRMED
**Category:** LOGIC BUG / DEAD CODE
**Active Runtime:** YES if BUG-01 is fixed (right now the system does not run)
**Player Impact:** When wired, no crisis ever resolves to `Chronic`. Every crisis either recovers or stays Active/InTreatment forever.
**Expected:** A long crisis without treatment can transition to Chronic status (terminal decline).
**Actual:** `TickDay` only advances crises that are `InTreatment` and resolves them to `Recovered` on reaching `recoveryProgress >= 100f`. There is no path that sets a crisis to `Chronic`; `RemoveAll(c => ... || c.status == CrisisStatus.Chronic)` is dead filtering.

**Evidence:**

```
$ grep -n "CrisisStatus.Chronic" Assets/Ashfall.Core/MentalHealthCrisisSystem.cs
137:c.status = CrisisStatus.Recovered;            // ← only forward transition
153:RemoveAll(c => c.status == CrisisStatus.Recovered || c.status == CrisisStatus.Chronic);
                                              // ← Chronic purge is dead
```

**Root Cause:** Incomplete implementation of the long-term chronic trajectory. The enum value was reserved but the state machine was not wired.

**Suggested Next Analysis:** Decide chronic semantics (e.g., if `recoveryProgress` reaches 100% without caregiver but age > threshold → Chronic; if `chemicalWithdrawal` profile and 3+ weeks → Chronic). Implement forward transition in `TickDay`.

---

### BUG-06 — `ContractorRosterSystem.TickDay` accrues `missedPayments` after the contract is already `Expired`

**Severity:** MEDIUM
**Confidence:** CONFIRMED
**Category:** LOGIC BUG
**Player Impact:** After contract expiry, the contractor stays in the contractors list as a ghost and may continue accruing `missedPayments`. The player sees a non-existent contractor.
**Trigger:** A contractor's `expiryDay` is reached.
**Expected:** On expiry, contractor leaves the active roster.
**Actual:** `TickDay` first attempts to pay hazard pay (incrementing `missedPayments` if currency is insufficient), *then* checks expiry — in that order, after expiry the contract is set to `Expired` but the payment loop ran a final time without skipping.

```csharp
// Assets/Ashfall.Core/ContractorRosterSystem.cs:165-180
foreach (var c in _state.contractors)
{
    if (c.status != ContractStatus.Active) continue;  // ← but expiry check is below
    var activeOffer = ...;
    if (activeOffer != null) {
        if (canPay) _inventory.RemoveById(currency, dailyHazardPay);
        else { c.missedPayments++; ... if (c.missedPayments >= 3) c.status = Expired; ... }
    }
    if (day >= c.expiryDay && c.status == ContractStatus.Active) c.status = Expired;
}
```

On the day the contract just expired, `status` is still `Active` at the top of the loop, so a missed payment can still flow through.

**Suggested Next Analysis:** Move the expiry check to the top of the iteration, or restructure so the payment loop only runs when `c.status == Active` AND `day < c.expiryDay`.

---

### BUG-07 — `ShelterScheduleSystem.TickDay` ignores the schedule's `fatigueRecoveryModifier` outside curfew

**Severity:** MEDIUM
**Confidence:** CONFIRMED
**Category:** LOGIC BUG
**Player Impact:** Switching to a schedule that defines `fatigueRecoveryModifier = 0.7f` (e.g., restless schedule) still grants `1f` during day phase.
**Trigger:** Player selects a non-default schedule and the day phase is active.
**Expected:** `fatigueRecoveryModifier` reflects the active schedule's setting whenever the schedule is not in curfew.
**Actual:** `TickDay`: `_state.fatigueRecoveryModifier = _state.emergencyOverride ? 0.5f : (_state.curfewActive ? def.fatigueRecoveryModifier : 1f);` — non-curfew branch hardcodes `1f` and ignores `def.fatigueRecoveryModifier`.

**Root Cause:** Likely a copy-paste from the default schedule, where `fatigueRecoveryModifier = 1f`. The intent was probably `def.fatigueRecoveryModifier` everywhere; the day phase path was never wired up.

**Suggested Next Analysis:** Reconcile with design intent: should the day phase always grant 1.0× recovery (i.e., modifier is *curfew-only*), or should it propagate? Likely the former — fix by clarifying the DTO doc, but also expose the inactive value as a separate property (`DayPhaseFatigueRecoveryModifier`) so callers don't conflate them.

---

### BUG-08 — `SumpFloodingSystem.TickDay` never resets `equipmentDisabled` after recovery

**Severity:** MEDIUM
**Confidence:** CONFIRMED (logic) + NODE-REACHABILITY-CoNFIRMED (orphan)
**Player Impact:** If a sump pump recovers water level but stays over the 90% threshold trigger, equipment stays disabled forever even after `DrainNode` brings level under 10 cm. Wait — `DrainNode` *does* reset `isFlooded = false` and `equipmentDisabled = false`. Then in `TickDay`, the slow natural drainage branch never resets if a complete drain occurs via natural flow.
**Trigger:** Water level drops naturally — `DrainComplete` fires, but `equipmentDisabled` is not lifted.
**Expected:** `equipmentDisabled` clears whenever `waterLevelCm == 0` and the node is no longer flooded.
**Actual:** Recovery requires an explicit `DrainNode` call. The natural decay only pumps out 2 cm/day; with 200 cm max level and 0.9 threshold, it takes weeks to fall below `equipmentDisabled`'s 90% threshold. In practice this is fine if the player acts, but **no recovery hook exists in `TickDay`** for `equipmentDisabled` once water drains.

**Root Cause:** The natural-drain branch handles `DrainComplete` incident logging but does not flip `equipmentDisabled`. Once a node is `equipmentDisabled = true`, the only path back to `false` is `DrainNode`.

**Suggested Next Analysis:** Either add `equipmentDisabled = false` when `waterLevelCm < 10cm` in the decay branch, or document that pumps must be drained manually.

---

### BUG-09 — `MentalHealthCrisisSystem.BeginTreatment` accepts any caregiver without eligibility check

**Severity:** MEDIUM
**Confidence:** CONFIRMED
**Player Impact:** A survivor in crisis can be assigned to a caregiver who themselves is on duty / in another crisis / deceased.
**Trigger:** Game UI issues `BeginTreatment(caseId, caregiverId, intervention)`.
**Expected:** Reject the assignment if caregiver is unavailable.
**Actual:** No caller check; caregiver is just stored on the case. The constructor `TriggerCrisis` carefully calls `_roster.Assign(activeRole, string.Empty)` to remove the *patient* from duty, but `BeginTreatment` does not check the caregiver.

**Suggested Next Analysis:** Add `if (_roster.GetAssignment(caregiverId) != null) return ActionResult.Blocked(...)`.

---

### BUG-10 — `LibraryStudySystem.TickDay` consumes `manual.skillXpGrants` as (skill, xp) pairs without bounds check

**Severity:** MEDIUM
**Confidence:** CONFIRMED
**Player Impact:** A manual definition with an odd `skillXpGrants` list (e.g., 3 entries) will IndexOutOfRange when reading `[i+1]`. The runtime swallows it via `float.TryParse` only — but `manual.skillXpGrants[i+1]` itself crashes before parse.
**Trigger:** Authoring JSON for a manual with odd grant list.
**Expected:** Either enforce even-length at load, or skip the malformed pair.
**Actual:**

```csharp
// Assets/Ashfall.Core/LibraryStudySystem.cs:148
for (int i = 0; i < manual.skillXpGrants.Count; i += 2)
{
    string skillId = manual.skillXpGrants[i];
    if (float.TryParse(manual.skillXpGrants[i + 1], out float xp))   // ← IndexOutOfRange if odd
        _skills.RecordAction(...);
}
```

**Suggestion:** Validate pair-count at `LoadCatalog` time. Reject manuals with odd grant count; let invalid manuals throw at load.

---

### BUG-11 — `DecontaminationSystem.CompleteCycle(false)` increases shelter contamination but never decreases surface contamination enough to prevent cross-effects

**Severity:** MEDIUM (becomes HIGH if BUG-01 fixed)
**Confidence:** CONFIRMED
**Player Impact:** A bypassed decontamination adds +0.1 to shelter contamination but only -0.1 to surface contamination — the contamination net movement is +0. If shelter contamination is rising faster than passive decay (`shelterContaminationLevel -= 0.01f/day`), the shelter becomes persistently hazardous, and any future decontamination run that *fails* could leave the shelter sealed forever.
**Trigger:** Player bypasses a decon case.
**Expected:** Bypass should at minimum NOT increase net shelter contamination, or surface contamination should drop more.
**Actual:** Symmetric -0.1/+0.1 — pure contamination transfer from surface to shelter.

**Root Cause:** Surface contamination reduces by the same amount shelter contamination increases. Bypassed decontamination is a "transformation," not a "reduction." This is intentional design but may violate the validated constraint that "shelter contamination passively decays at 0.01/day".

**Suggested Next Analysis:** Verify against the design doc. May be desired — flag for design review.

---

## 9. Low Findings

### BUG-12 — `ShelterThermalSystem.AddRoom` initial `currentTempC` snapshots boiler state at construction time
```csharp
currentTempC = _state.boilerCurrentTempC
```
If the boiler has not yet been set active, `boilerCurrentTempC == 20f` always; if the boiler was previously running and has cooled, the new room inherits a stale value. Minor state-inconsistency at room creation.

### BUG-13 — `ArchiveDeskSystem.CancelJob` does not roll back journal evidence added earlier in the same tick
Minor: if `TickDay` ran between `QueueTranscription` and `CancelJob`, the evidence is already unlocked; cancellation refund is purely economic. Cosmetic.

### BUG-14 — `ApprenticeshipSystem` instantiates a `new DutyRosterSystem()` local to its scope
Not a bug in the orphan state, but on integration with the game-wide `_dutyRoster`, the apprenticeship system would need a passed-in reference rather than its own. Currently in `Main.ExpandedShelterSystems.cs:142-143`:
```csharp
var appRoster = new DutyRosterSystem();
var appSys = new ApprenticeshipSystem(new SeededRng(1986), appSkills, appRoster, srSys, new GodotLog());
```
The `_roster.GetAssignment()` check is now permanently `null`, so `mentor_busy`/`apprentice_busy` never triggers. Real apprentices can start without reserving duty slot.

### BUG-15 — `ShelterScheduleSystem.UpdatePhase` raises `OnPhaseChanged` only on transition, but `OnScheduleChanged` every time — event-frequency asymmetry
Cosmetic, but tests/handlers that subscribe to both will see inconsistent ordering.

---

## 10. Suspected / Needs Reproduction

| # | Defect | Why suspected | Reproduction path |
|---|---|---|---|
| S-01 | MentalHealthCrisisSystem `TickDay` increments ward occupancy on Trigger but `TriggerCrisis` increments `currentOccupancy++` without unblock check on full | Would need host wiring to reproduce | Add unit test for the boundary |
| S-02 | `EquipmentConditionSystem.UseItem` decrements `usesRemaining` *after* `Math.Max(0, condition - wearAmount)`, allowing tools with `usesRemaining = 0` but positive condition to remain `IsUsable=true` | Wait — `IsUsable` returns `usesRemaining == -1 || usesRemaining > 0`. So `usesRemaining == 0` returns false. That's correct. **REJECTED.** |
| S-03 | `ShelterScheduleSystem.TickDay` sets `_state.lightingDemand *= 0.5f` on brownout but is also overridden in the second block, so the *0.5 may be ignored*. **NEEDS REPRODUCTION.** |
| S-04 | Several orphan systems call `_inventory.RemoveById` even when inventory doesn't exist (`new Inventory.Inventory()` is empty) | Wait — that's the test scenario. Production inventory is real. **REJECTED, test behavior, not production.** |
| S-05 | `AddKnowledgeEvidence` in `LibraryStudySystem` and `MentalHealthCrisisSystem` both call `JournalSystem.UnlockCodex`, which `KnowledgeBase.Discover` keys; but `ArchiveDeskSystem` calls `_knowledge.Discover(job.evidenceId)` directly AND `_journal.TryDiscover(evidenceId, author, day)` which also unlocks via `_knowledge.Discover(knowledgeKey)`. The annotation occurs once but with two different keys (`evidenceId` vs whatever `JournalVoice.ComposeFullText` produces). Possible double-discovery bug. **NEEDS REPRODUCTION.** |

---

## 11. Rejected False Positives

These looked like bugs in Loop 1 but were disproven:

| Hypothesis | Why rejected | Evidence |
|---|---|---|
| `MentalHealthCrisisSystem._medical` is unused (zero calls) → bug | Confirmed unused, BUT constructor argument ensures wiring contract; it represents future integration. Not a runtime bug until used. Marked as MEDIUM integration gap, not bug. | `grep -c "_medical\."` = 0 |
| `EquipmentConditionSystem.USESRemaining` decrement bug | Already reviewed — `IsUsable` correctly returns false at 0. | `EquipmentConditionSystem.cs:174-179` |
| `ContractorRosterSystem.tickday` infinite loops | Iterated over finite list; iteration order is deterministic; no growth. | Line 152-180 |
| Determinism violation by enumeration order | All Batch 3 systems iterate Lists created in deterministic order; no dictionary iteration in tick code. | Searched each |
| `System.Random` leak | None. Confirmed via grep: 0 hits across 12 files. | `grep` returns empty |
| `Guid.NewGuid` leak | None. | `grep` returns empty |
| `DateTime.Now` leak | None. | `grep` returns empty |
| Save corruption in ShelterThermalSaveStore | Verified — checksum envelope + null rejection + legacy fallback per `SaveWireContract`. | Reviewed `src/Host/ShelterThermalSaveStore.cs` |
| `KitchenNutritionSystem.ServeMeal` invariant — `pantryItem == null` after Find | Defensive — blocked on null. | `KitchenNutritionSystem.cs:198` |

---

## 12. Root-Cause Clusters

### Cluster A — Batch 3 Phase 4 wiring was incomplete

**8 of 12 Batch 3 systems orphaned.** The Expansion System Protocol in AGENTS.md specifies five phases:

```
Phase 1 — system classes
Phase 2 — data
Phase 3 — IDs/static constants
Phase 4 — wire into GameBootstrap (properties, construction, event wiring, init, tick registration, save fields)
Phase 5 — tests
```

The commit message claims "12 Batch 3 systems" shipped, but only Phase 1 + Phase 5 were delivered for all 12. Phase 4 was applied to 4 of them via `Main.ExpandedShelterSystems.cs`. Phase 4 fell short for the remaining 8. Phase 2 (data) was not delivered for any of them — explain why BUG-02's catalogs are empty.

**Affects:** BUG-01, BUG-02 (loading-side), all 8 orphan systems, and the 4 wired systems missing JSON catalogs.

**Single fix unlocks all 12:** Author the missing Phase 4 wiring + Phase 2 JSON catalogs.

### Cluster B — Cross-system integration was intent-only

**Several systems carry constructor-injected dependencies that are never called.** These represent *undelivered wiring intents*, not bugs in the systems per se. But they are coupled to the production wiring expectation:

- `MentalHealthCrisisSystem._medical`, `_dependency` (0 + 0 use)
- `ContractorRosterSystem._roster`, `_expedition` (0 + 0 use)
- `EquipmentConditionSystem._crafting` (0 uses)
- `DecontaminationSystem._startingLevel` (0 uses)
- `ShelterThermalSystem._needs` (0 uses in `TickDay`)

These are "reserved ports" — fine architecturally, but unless wired by the host, they create the false appearance of integration while in reality each system operates in isolation.

**Affects:** BUG-03, BUG-09, BUG-14, plus runtime player impact when any pair of systems would normally interact.

### Cluster C — Heat/electrical/illumination physics are placeholders

**BUG-04** (thermal `* 0.1` heat placeholder), **BUG-07** (schedule fatigue ignores schedule def), and possibly **BUG-15** (lighting demand double-override on brownout). These look like tuning placeholders frozen into "feature complete" code.

---

## 13. Cross-System Failure Chains

### Chain 1 — survivor warmth never rises
```
boiler.SetBoilerActive(true)                              [main game]
→ ShelterThermalSystem.TickDay                             [Core]
→ warmthDelta computed → discarded                         [BUG-03]
→ room.currentTempC increments (placeholder math)         [BUG-04]
→ no survivor NeedsSystem.Warmth adjustment                [BUG-14: appRoster is fake]
→ no UI warmth indicator updates                          [no integration]
```

### Chain 2 — autopsy cannot start
```
player opens autopsy panel                                 [host wired]
→ panel enabled (AuroraReportPanel.Bind)                   [ok]
→ BeginAutopsy button → needs procedure catalog            [BUG-02]
→ catalog is empty                                         [Phase 2 data missing]
→ returns "missing_procedure"                             [Core OK on this path]
→ no error UI? Likely silent failure                       [Loop 8]
```

### Chain 3 — apprentice pairing always passes duty check
```
apprentice.StartPair(mentorId, apprenticeId)
→ _roster.GetAssignment(mentorId) → null                  [BUG-14: own DutyRosterSystem]
→ pairing accepted even when mentor on duty                [none]
→ mentor pulled away, both starve/wander                   [player impact]
```

### Chain 4 — mental-health crisis isolates patient but caregiver is anyone
```
TriggerCrisis(survivorId)
→ _roster.Assign(activeRole, "")                           [ok]
→ patient's role resigned                                 [ok]
→ BeginTreatment(caseId, anyCaregiverId, intervention)
→ no caregiver eligibility check                           [BUG-09]
→ second survivor pulled off critical shift                [player impact]
```

### Chain 5 — contracted expedition worker has no contract validation
```
ContractorRosterSystem.AcceptOffer
  → no expedition system cooperation
  → contractor can join expedition without currency check
  → IsAvailableForExpedition returns true for everyone
  [BUG: _expedition not used at all]
```

### Chain 6 — Countdown to permanent shelter contamination
```
DecontaminationSystem.CompleteCycle(false)                [BUG-11]
→ shelter contamination +0.1
→ passive decay -0.01/day
→ shelter contamination rises faster than decay
→ persistent shelter hazard
→ cannot re-zero shelter without explicit reset API
  [no API exists]
```

---

## 14. Test Coverage Gaps

The 102 Batch 3 tests cover unit-level Core correctness, but **no integration test exists for**:

| Gap | Why it matters |
|---|---|
| `Main.SetupExpandedShelterSystems` order of construction | If order changes, dependency injection breaks |
| `Main.TickAllExpandedShelterSystems(day)` chain | Each system must be ticked in a valid order; current code is alphabetical but cross-system effects may want survivors first, then needs, then thermal |
| `CatalogPath`-based JSON loading | All 4 catalogs (4 wired systems) are loaded nowhere in production |
| Save round-trip for each orphan system | When wired, will they round-trip without schema corruption? |
| `_state.fatigueRecoveryModifier` propagation to survivor fatigue | BUG-03 + BUG-07 require a cross-system integration test |
| Host wiring test for `OnCaseCompleted/OnJobCompleted/OnMealServed` event re-emission on load | Save loads do not replay completion events |

**False-green test potential:** the 8 orphan systems pass 102 tests but their state is never reset between runs of the actual game — if a hostile player wins the lottery and somehow triggers a kitchen meal, the test could pass while production runs differently. The tests instantiate `new Inventory.Inventory()` locally and don't integrate with `SurvivorsHostSession`.

---

## 15. Migration / Legacy Risks

These Batch 3 systems have ZERO `Assets/_Game/` references (per `grep` over `_Game/*`), so the migration risk is in the opposite direction: **Godot is the only host**, and these systems are Godot-isolated.

Unity-side migration debt is not relevant here; the orphan Core is already engine-agnostic.

Hidden Unity artifacts: The `ApprenticeshipSystem` test fixture mentions `SimpleSkillActor` — verify it is Core-native. Confirmed: `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs`. No UnityEngine leak.

---

## 16. Save / Determinism Findings

| System | Save store | Determinism | Notes |
|---|---|---|---|
| ShelterThermalSystem | ✅ `ShelterThermalSaveStore` | ✅ `ISeededRng` only | RestoreState fires OnThermalChanged — completion events NOT replayed |
| ShelterScheduleSystem | ✅ `ShelterScheduleSaveStore` | N/A (no RNG) | Catalog not loaded → `default` only |
| ApprenticeshipSystem | ✅ `ApprenticeshipSaveStore` | ✅ Deterministic XP | Uses isolated DutyRoster |
| AutopsySystem | ✅ `AutopsySaveStore` | ✅ `ISeededRng` only | Catalog not loaded → no procedure can start |
| SumpFloodingSystem | ❌ missing | ✅ `ISeededRng` only | Orphan |
| DecontaminationSystem | ❌ missing | ✅ `ISeededRng` only | Orphan |
| KitchenNutritionSystem | ❌ missing | ✅ `ISeededRng` only | Orphan |
| EquipmentConditionSystem | ❌ missing | ✅ `ISeededRng` only | Orphan |
| LibraryStudySystem | ❌ missing | N/A (no RNG in tick) | Orphan |
| ArchiveDeskSystem | ❌ missing | N/A (no RNG in tick) | Orphan |
| ContractorRosterSystem | ❌ missing | N/A (no RNG in tick) | Orphan |
| MentalHealthCrisisSystem | ❌ missing | ✅ `ISeededRng` only | Orphan |

`SaveChecksum.Compute(envelope)` is the same shape for all 4 wired stores and matches the existing `SaveStoreChecksumSweepTests` contract.

---

## 17. Recommended Investigation Order

1. **BUG-01 (orphan systems) + BUG-02 (empty catalogs)** — single repair: complete Phase 4 wiring + Phase 2 data for the 8 remaining systems. This is the highest leverage; the other findings either disappear after this or become addressable.
2. **BUG-03 / BUG-04 (thermal integration + thermal physics)** — implement survivor warmth propagation and replace the heat-distribution placeholder.
3. **BUG-06 (Contractor expired-then-paid race)** — simple ordering fix.
4. **BUG-05 (Chronic status unreachable)** — design decision required; may already be intentional.
5. **BUG-07 (schedule modifier ignored)** — clarify intent or fix.
6. **BUG-08 (equipmentDisabled latch)** — recheck with design.
7. **BUG-09 (caregiver eligibility)** — small fix.
8. **BUG-10 (manual grants pair-list)** — defensive but easy.
9. Test coverage gap for `Main.ExpandedShelterSystems.cs` integration.
10. Documentation: classify the 4 wired systems + the 8 orphan systems in AGENTS.md as "Batch 3 wiring incomplete".

---

## 18. Evidence Index

| Evidence | Path |
|---|---|
| Commit SHA | `git rev-parse HEAD` → `eeff1f79d003f0a03714f2e8022c464ace76a132` |
| Commit message | "feat(batch3-phases1-3): implement 12 Batch 3 systems + Core support additions" |
| 8 orphan systems reference search | `grep -rn "SumpFloodingSystem\|DecontaminationSystem\|KitchenNutritionSystem\|EquipmentConditionSystem\|LibraryStudySystem\|ArchiveDeskSystem\|ContractorRosterSystem\|MentalHealthCrisisSystem" src/` → empty |
| 4 wired systems host sessions | `src/Main.ExpandedShelterSystems.cs:142-198` |
| Tick wiring | `src/Main.Holdfast.cs:320 TickAllExpandedShelterSystems(day);` |
| Catalog loading search | `grep -n "LoadCatalog\|LoadInkCatalog" src/` → empty |
| ShelterThermal dead warmth | `Assets/Ashfall.Core/ShelterThermalSystem.cs:228-232` |
| ShelterThermal heat placeholder | `Assets/Ashfall.Core/ShelterThermalSystem.cs:170-200` |
| MentalHealthCrisis chronic dead path | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs:153` |
| Contractor ordered loop | `Assets/Ashfall.Core/ContractorRosterSystem.cs:165-180` |
| Apprentice local roster | `src/Main.ExpandedShelterSystems.cs:142` |
| Schedule modifier | `Assets/Ashfall.Core/ShelterScheduleSystem.cs:185` |
| Library study XP loop | `Assets/Ashfall.Core/LibraryStudySystem.cs:148` |
| Decon net contamination | `Assets/Ashfall.Core/DecontaminationSystem.cs:113-128` |
| Save store pattern | `src/Host/ShelterThermalSaveStore.cs` |
| Build result | `dotnet build` → 0 errors, 4 warnings (pre-existing nullable refs) |
| Test result | 102/102 Batch 3 tests pass |

---

## 19. Audit Confidence

| Layer | Confidence |
|---|---|
| Compile-clean | CONFIRMED |
| xUnit pass | CONFIRMED |
| Reachability for orphan 8 | CONFIRMED (multiple grep evidence) |
| Reachability for wired 4 | CONFIRMED (paths through Main.ExpandedShelterSystems) |
| Catalog loading absence | CONFIRMED |
| warmth-delta discarded | CONFIRMED (static read + grep) |
| Heat distribution `* 0.1` | CONFIRMED |
| Cross-system dependencies unused | CONFIRMED (grep count) |
| MentalHealth Chronic dead | CONFIRMED |
| Contractor expired-then-paid race | CONFIRMED |
| Schedule modifier ignored | CONFIRMED |
| Library study XP bounds | CONFIRMED |
| Save store integrity | CONFIRMED (matches existing sweep contract) |
| UI panel wiring | CONFIRMED for 4; 0 UI for 8 |
| Player impact (events reaching UI on load) | SUSPECTED — needs interactive reproduction |

---

## 20. Audit Completion Statement

All 10 loops were completed in sequence:

1. Structural/static — produced 17 candidates, settled 11.
2. Call graph / reachability — produced cluster A: 8 systems cannot be reached via `src/`.
3. State transition — produced BUG-05, BUG-06, BUG-08, BUG-11.
4. Save / Load — confirmed 4 save stores verify integrity; 8 missing.
5. Determinism — confirmed clean.
6. Catalog / Data — produced BUG-02; confirmed 0 JSON catalogs for the 4 wired systems.
7. Event / Lifecycle — confirmed wired events fire on Restore, completion events do not replay.
8. UI / Player-facing — confirmed 4 panels, 0 orphan panels, status rail is shallow.
9. Test adversarial — 102 tests are unit-level only; do not exercise host wiring; "false-green" risk for orphan systems.
10. Cross-system synthesis — produced 6 failure chains documenting how the orphan + half-wired state cascades into player-impact bugs.

No production code was modified. No Unity commands were run. No speculative findings were promoted to confirmed without code-level evidence.

Final candidate count: **1 CRITICAL, 2 HIGH, 7 MEDIUM, 4 LOW, 5 SUSPECTED** (not independent — many are symptoms of BUG-01).

Final deduplicated root-cause clusters: **3**.

The dominant defect is **BUG-01 (8 orphan systems)**, which bundles BUG-02 through BUG-11 into a single upstream cause: Phase 4 of the Expansion System Protocol was not executed for 8 of 12 Batch 3 systems, and Phase 2 (data) was not executed for any of them. Fixing BUG-01 + BUG-02 together is the highest-leverage upstream repair; most other findings resolve or become tractable after that fix lands.
