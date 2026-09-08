# Phantom Background Integration & Trauma Binding

## 1. Overview
In ASHFALL, the psychological toll of the atomic war manifests through memories, flashbacks, and hallucinations managed by `PhantomMemoryHostSession`. The survivor enrichment layer binds each survivor's authored `phantom_background_id` to provide narrative context for trauma reactions.

---

## 2. Phantom Background Taxonomy

| Phantom Background ID | Count | Narrative Archetype | Trauma Context & Trigger Seams |
|---|---|---|---|
| `child_refugee` | 6 | Evacuee youth | Sirens, separated family memories, cold shelters, toy relics |
| `driver` | 4 | Convoy / transport | Burning fuel smells, blocked highways, gridlock accidents, wreckage |
| `electrician` | 8 | Grid engineer | High-voltage sparks, blackouts, fuse box failures, ozone smell |
| `former_soldier` | 14 | Military conscript / veteran | Gunfire echoes, perimeter alarms, trench cold, radiation burns |
| `generic` | 16 | Civilian survivor | Falling ash, siren wails, bunker overcrowding, rationing |
| `machinist` | 8 | Industrial factory worker | Heavy machinery screeches, structural collapses, steam vents |
| `miner` | 4 | Subterranean worker | Cave-ins, asphyxiation, darkness, seismic rumbles |
| `nurse` | 8 | Pre-war medical staff | Screaming patients, triage decisions, antiseptic smells, blood loss |
| `teacher` | 4 | School educator | Lost children, school evacuation failures, burning books |

---

## 3. Host Wiring & Binding Seams

### Binding Seam (`src/Main.Phase0.cs`)
During world bootstrap, `Main.SetupPhantomMemory()` binds survivor definitions and their enrichment fields into the phantom memory session:

```csharp
private void SetupPhantomMemory()
{
    // ...
    if (_phantomMemory != null && _survivors != null && _enrichment != null)
    {
        _phantomMemory.BindSurvivors(_survivors.Roster, _enrichment);
    }
}
```

### Event Resolution Flow
1. **Trigger Check:** When a high-stress event occurs (e.g. bunker radiation spike, generator explosion, medical emergency), the trauma system queries the affected survivor's `phantom_background_id`.
2. **Contextual Flashpoint:** Rather than generic sanity drop text, the event text and audio cues align with the survivor's background:
   - Nurse: Remembers overflowing triage wards during the first strike.
   - Former Soldier: Experiences auditory flashbacks of perimeter breach alarms.
   - Electrician: Reacts to sparks from short-circuiting control panels.
3. **No Duplicate Stat Authorities:** The enrichment field acts purely as a narrative discriminator. It does NOT create a second sanity or morale meter.

---

## 4. Unenriched Survivor Safety
If a survivor definition lacks an entry in `expansion_survivor_fields.json`, `SurvivorEnrichmentService` and `PhantomMemoryHostSession` gracefully assign the fallback key `generic`. The simulation never throws or encounters null reference exceptions.
