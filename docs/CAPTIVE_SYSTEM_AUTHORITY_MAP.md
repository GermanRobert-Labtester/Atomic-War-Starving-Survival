# ASHFALL Plan 63 — Raider Captives, Interrogation & Penal Labor Authority Map

**Subsystem:** Raider Captives, Interrogation & Penal Labor Shifts
**Core Authority:** `Assets/Ashfall.Core/Shelter/ShelterPrisonerSystem.cs`
**Data Authority:** `Assets/StreamingAssets/Data/captive_interrogations.json` (`schema_version: 1`)
**Save Store:** `src/Host/ShelterPrisonerSaveStore.cs` (`shelter_prisoners`)
**Host & Presentation:** `src/UI/CaptiveManagementPanel.cs`, `assets/ui/panels/CaptiveManagementPanel.tscn`, `src/Main.Plans62_65.cs`

---

## 1. Subsystem Architecture

Plan 63 introduces captive management without corrupting the canonical survivor roster:
1. **Separation of Concerns:** Captives are stored in `ShelterPrisonerSystem` as `PrisonerRecord` instances. They do not appear in `SurvivorAggregate` or `SurvivorRosterSystem` until legally paroled and integrated.
2. **Containment & Guarding:**
   - Captives occupy secure shelter containment (`room_ward_quarantine` or `room_storage_secure`). Max capacity = room count × base capacity.
   - Assigned guards provide security suppression. Guard deficit increases daily unrest, riot risk, and escape attempts.
3. **Interrogation & Psychological Inquiry:**
   - Strictly non-graphic: interrogation consists of psychological rapport building, evidentiary confrontation, strategic bargaining, or firm isolation.
   - Interrogation outcomes: extraction of map cache markers, raider attack warning clocks, faction cryptographic codes, or willingness to cooperate.
4. **Penal Labor Shifts:**
   - Prisoners can be assigned to grueling infrastructure shifts (quarrying rubble, slurry shoveling, ventilation filter cleaning).
   - Generates production output but increases fatigue, resistance, and community moral strain.
5. **Rehabilitation & Parole:**
   - When compliance reaches 100 and hostility drops to 0, player can offer parole/citizenship.
   - Converts the prisoner into a full `SurvivorRecord` in `SurvivorEntityStore` with inherited traits and skills.

---

## 2. Catalog Schema (`captive_interrogations.json`)

Each interrogation archetype and topic definition includes:
- `id`: Unique string key (e.g., `captive_raider_scout`, `captive_mercenary_engineer`, `captive_zealot_infiltrator`).
- `faction_origin`: Origin faction identifier.
- `base_resistance`: 0–100 initial resistance against cooperation.
- `base_hostility`: 0–100 initial hostility towards the shelter.
- `interrogation_topics`: Array of revealable topics (e.g., `topic_arms_cache`, `topic_ambush_route`, `topic_cipher_key`).
- `intel_reward_type`: `map_location`, `faction_reputation`, `warning_timer`, `tech_fragment`.
- `intel_reward_id`: Target ID of the reward.
- `parole_morale_impact`: Community morale effect upon granting citizenship.

---

## 3. Core Domain Classes

```csharp
namespace Ashfall.Core.Shelter
{
    public enum PrisonerStatus
    {
        Detained = 0,
        Interrogating = 1,
        PenalLabor = 2,
        Paroled = 3,
        Escaped = 4,
        Deceased = 5
    }

    public enum InterrogationMethod
    {
        RapportBuilding = 0,
        MaterialBargaining = 1,
        FirmPressure = 2,
        SensoryIsolation = 3
    }

    public sealed class PrisonerRecord
    {
        public string PrisonerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FactionOrigin { get; set; } = string.Empty;
        public PrisonerStatus Status { get; set; }
        public float Resistance { get; set; }
        public float Hostility { get; set; }
        public float Compliance { get; set; }
        public float Fatigue { get; set; }
        public List<string> ExtractedTopics { get; set; } = new List<string>();
        public int DaysInCaptivity { get; set; }
    }
}
```

---

## 4. Invariants & Determinism

- Guard security ratio = `GuardsAssigned * GuardSkillFactor / Max(1, PrisonerCount)`.
- Risk of revolt/escape checked daily via `ISeededRng` with purpose `captive_interrogate`.
- Captives consume standard food rations daily (`RationTier.Standard` or `RationTier.Half`).
- Zero UnityEngine or Godot dependencies in Core.
