# ASHFALL Disease Quarantine Policy Architecture (Plan 63 / B4)

**Document ID:** DOC-MED-P63-002
**Status:** Canonical Architecture Specification
**Authority:** `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`
**Associated Systems:** `MedicalWardSystem`, `DutyRosterSystem`, `DiseaseSystem`, `ResearchSystem`

---

## 1. Architectural Mission

The quarantine policy elevates disease containment from a simple passive UI flag to an active, systemic management loop. Prior to Plan 63, marking a patient as quarantined was a zero-cost toggle that immediately eliminated transmission without checking bed capacity, imposing labor or resource costs, or interacting with duty rosters.

Under Plan 63, quarantine operates as an orchestrated shelter policy:

```text
[ Inbound Infection / Outbreak ]
               ↓
    [ Quarantine Coordinator ]
   ↙           ↓            ↘
MedicalWard  DutyRoster   Care Supplies
(Bed check)  (Separation) (Water/Food/Meds)
   ↘           ↓            ↙
     Transmission Reduction
     (85–95% reduction)
               ↓
       Deterministic Arc
```

---

## 2. Core Invariants & Boundaries

1. **No Bed Ownership in DiseaseSystem:** `MedicalWardSystem` is the sole authority for bed allocation and physical occupancy (`Category == Isolation` or `Isolation == true`). `DiseaseSystem` receives isolation context (`isIsolated`, `isolationQuality01`), not bed state.
2. **Duty Roster Separation:** Quarantined survivors cannot work communal shifts (Mess Hall, Ventilation Maintenance, Water Reclamation, Field Expeditions). When isolation is executed, conflicting duty assignments are atomically cleared. While isolated, `DutyRosterSystem.IsSurvivorReservedExternally` blocks new assignments.
3. **Bed-Capacity Honesty:** If no isolation beds exist or all are occupied, `PreviewAssignIsolation` returns `CanExecute = false` with an honest reason (`no_isolation_beds_available`). There is no silent success or overflow into imaginary beds.
4. **Real Care Burden:** Maintaining a patient in isolation imposes a non-trivial daily resource drain:
   - Clean Water: 1 unit/day
   - Food Rations: 1 unit/day
   - Medical Consumables / Bandages: 1 unit/day
   - Monitoring Labor: 2.0 hours/day
   If care requirements cannot be met, isolation quality degrades, increasing shedding and breakthrough risk.
5. **Bounded Transmission Reduction (No Magic Cures):** Quarantine reduces effective contagiousness by 85%–95% (depending on ward quality and research capabilities), but does not guarantee 0% transmission. Secondary infections can still breach containment if care is neglected.
6. **Zero Direct Mutation from Research:** `knowledge_pathogen_containment` projects a typed `ContainmentCapability` context with bounded bonuses (`EfficacyBonus`, `CareEfficiencyBonus`, `MonitoringBonus`). Research never directly mutates disease state or bed records.

---

## 3. Command API: Preview & Execute Pattern

The coordinator exposes atomic command pairs:

### Assign Isolation
- `PreviewAssignIsolation(string survivorId)`: Evaluates eligibility, locates an available isolation bed in `MedicalWardSystem`, detects conflicting duty roles, projects isolation quality, and details daily care costs.
- `ExecuteAssignIsolation(string survivorId, int day)`: Atomically admits the patient to the isolation bed, clears any active duty assignment, applies quarantine to all active infections in `DiseaseSystem`, registers the policy burden, and emits `OnQuarantineAssigned`.

### Release Isolation
- `PreviewReleaseIsolation(string survivorId)`: Validates that the survivor is currently isolated and calculates discharge feasibility.
- `ExecuteReleaseIsolation(string survivorId, int day)`: Discharges the patient from `MedicalWardSystem`, lifts quarantine flags in `DiseaseSystem`, releases external duty reservations, and emits `OnQuarantineReleased`.

---

## 4. Daily Simulation Loop (`TickDaily`)

Each simulation day, `DiseaseQuarantineCoordinator.TickDaily(int day)`:
1. Audits all currently isolated patients.
2. Deducts daily care consumables via `TryConsumeItem`.
3. Assesses isolation quality based on supply availability and `ContainmentCapability`.
4. Feeds isolation state and quality into `DiseaseSystem.TickDaily`.
5. Emits daily telemetry tracking bed-days, consumed resources, and containment status.
