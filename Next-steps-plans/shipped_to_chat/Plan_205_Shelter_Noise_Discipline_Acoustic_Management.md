# Plan 205 — Shelter Noise Discipline & Acoustic Management

## Goal

Create a shelter noise discipline and acoustic management system where the shelter generates sound from activities, machinery, and survivors — and where excessive noise attracts external threats (raiders, factions, wildlife) while noise discipline (soundproofing, quiet hours, acoustic dampening) reduces detection risk. Currently `MaritimeDiveSystem.cs` tracks noise level for underwater diving, `SafeCrackingSystem.cs` tracks noise for safe cracking, and `SignalTriangulationSystem.cs` tracks radio noise — but these are domain-specific. There is no general shelter noise system, no acoustic propagation, no noise discipline management, no sound-based detection by external threats, no soundproofing mechanics. The shelter is acoustically invisible. This plan adds noise as a stealth/survival layer.

## Why

**Repository evidence:** Grep for `ShelterNoise`, `NoiseDiscipline`, `SoundPropagation`, `AcousticSystem`, `NoiseDetection`, `Soundproofing`, `NoiseRadius`, `AcousticSignature` in Core returns ZERO matches. `MaritimeDiveSystem.cs` tracks `NoiseLevel` (0-100) for diving — but this is domain-specific to underwater. `SafeCrackingSystem.cs` tracks `NoiseLevel` for safe cracking — domain-specific. `SignalTriangulationSystem.cs` tracks `noiseLevel` for radio signals — domain-specific. No general shelter noise system exists. Plan 138 (Shelter Defense) mentions "noise discipline" in passing but doesn't implement.

**What is missing:** No shelter noise system. No acoustic propagation through rooms. No noise discipline management. No soundproofing mechanics. No noise-based detection by external threats. No quiet hours enforcement. No acoustic dampening. The shelter generates noise but nothing tracks it or consequences from it.

**Why existing plans don't solve it:** Plan 138 (shelter defense) mentions noise discipline but doesn't implement. Plan 156 (shelter expansion) covers construction but not acoustics. Plan 71 (power grid) covers power but not noise from generators. No plan addresses shelter noise as a system.

**Player value:** Creates stealth gameplay (manage noise to avoid detection), adds strategic depth (soundproofing, quiet hours), generates emergent tension (noise attracts threats), and makes shelter management more immersive (the shelter has an acoustic footprint).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` — domain-specific noise (reference pattern)
- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` — generator noise source
- `Assets/Ashfall.Core/VentilationSystem.cs` — ventilation noise source
- `Assets/Ashfall.Core/Shelter/ShelterThermalSystem.cs` — shelter environment
- `Assets/Ashfall.Core/ExpeditionSystem.cs` — external threat detection
- NEW: `Assets/Ashfall.Core/Shelter/ShelterNoiseSystem.cs`
- NEW: `Assets/StreamingAssets/Data/noise_sources.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterNoiseSystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `ShelterNoiseState` DTO: `overallNoiseLevel` (0-100, shelter-wide acoustic signature), `noiseByRoom` (dict of room_id → noise level), `noiseSources` (list of active noise sources), `soundproofingLevel` (0-100, overall shelter soundproofing), `noiseDisciplineActive` bool (quiet hours enforced), `detectionRisk` (0-100, how likely external threats detect shelter), `lastNoiseEvent` (day), `noiseEvents` (list of noise events), `acousticSettings` (noise propagation enabled bool, detection enabled bool)
3. Define `NoiseSource` DTO: `sourceId`, `sourceType` (machinery/human_activity/industrial_process/alarm/ventilation/generator/construction/music/argument), `roomId` (room where source is active), `noiseOutput` (0-100, base noise level), `frequency` (low/medium/high — affects propagation), `duration` (hours per day or continuous), `isActive` bool, `canBeSoundproofed` bool
4. Define `NoisePropagation` DTO: `propagationId`, `sourceRoomId`, `targetRoomId`, `distance` (rooms between), `wallSoundproofing` (0-100, wall dampening), `propagatedNoise` (0-100, noise level after propagation), `frequency` (low/medium/high), `path` (list of room_ids sound travels through)
5. Define `Soundproofing` DTO: `soundproofingId`, `roomId`, `wallTreatment` (0-100, wall soundproofing), `floorTreatment` (0-100, floor soundproofing), `doorTreatment` (0-100, door soundproofing), `overallSoundproofing` (0-100, composite), `installedDay`, `condition` (0-100, degrades over time)
6. Define `NoiseEvent` DTO: `eventId`, `eventType` (noise_spike/quiet_hours_violation/noise_discipline_started/soundproofing_installed/external_detection/noise_complaint), `day`, `roomId`, `description`, `severity` (mild/moderate/severe/critical), `noiseLevel` (0-100), `consequences` (list of effects)
7. Define `NoiseDiscipline` DTO: `disciplineId`, `isActive` bool, `quietHoursStart` (hour 0-23), `quietHoursEnd` (hour 0-23), `enforcedRooms` (list of room_ids), `violations` (list of violations), `complianceLevel` (0-100, how well survivors comply)
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define noise source types (9+ types):
   - **Machinery**: generators, pumps, processors (continuous, high noise)
   - **Human Activity**: conversation, movement, work (variable, medium noise)
   - **Industrial Process**: welding, cutting, forging (intermittent, very high noise)
   - **Alarm**: emergency alerts, sirens (intermittent, very high noise)
   - **Ventilation**: air circulation, fans (continuous, low-medium noise)
   - **Generator**: power generation (continuous, high noise)
   - **Construction**: building, renovation (intermittent, very high noise)
   - **Music/Recreation**: entertainment, social gatherings (variable, medium noise)
   - **Argument/Conflict**: interpersonal conflicts (intermittent, variable noise)
10. Define noise propagation mechanics:
    - Noise propagates from room to room
    - Propagation affected by: distance, wall soundproofing, frequency
    - Low frequency (bass, machinery): propagates far, hard to block
    - High frequency (voices, alarms): propagates less, easier to block
    - Soundproofing reduces propagation
    - Open doors: noise propagates freely
    - Closed doors: noise reduced
11. Define soundproofing mechanics:
    - Rooms can be soundproofed (walls, floors, doors)
    - Soundproofing materials: mass-loaded vinyl, acoustic foam, double walls
    - Soundproofing reduces noise propagation
    - Soundproofing degrades over time, requires maintenance
    - Soundproofing has cost (resources, time)
12. Define noise discipline mechanics:
    - Player can enforce quiet hours (specific time range)
    - During quiet hours: noise sources reduced, survivors speak quietly
    - Noise discipline violations: survivors who don't comply
    - Noise discipline reduces overall shelter noise
    - Noise discipline improves stealth
13. Define external detection mechanics:
    - Shelter noise creates acoustic signature
    - External threats (raiders, factions, wildlife) can detect shelter
    - Detection risk based on: overall noise level, distance to threats, terrain
    - High noise: increased detection risk
    - Detection consequences: raids, surveillance, attacks
14. Define noise consequences:
    - **Low noise (0-20)**: minimal detection risk, stealth maintained
    - **Moderate noise (21-50)**: low detection risk, some acoustic signature
    - **High noise (51-80)**: moderate detection risk, audible from distance
    - **Critical noise (81-100)**: high detection risk, very audible, attracts threats
15. Add deterministic seeding: noise events use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupShelterNoise`, `TickShelterNoise`, `SaveShelterNoise`

## Main Task 2 — Implementation / Sources / Propagation / Soundproofing / Discipline / Detection / UI

1. Implement noise sources:
   - Each activity/machine generates noise
   - Noise sources have type, output, frequency, duration
   - Noise sources active in specific rooms
   - Noise sources can be turned off/managed
   - Noise sources logged
2. Implement noise propagation:
   - Noise propagates from room to room
   - Propagation affected by distance, walls, frequency
   - Sound travels through open doors freely
   - Sound travels through closed doors (reduced)
   - Soundproofing reduces propagation
   - Propagation logged
3. Implement soundproofing:
   - Rooms can be soundproofed
   - Soundproofing has level (0-100)
   - Soundproofing reduces noise propagation
   - Soundproofing degrades over time
   - Soundproofing requires maintenance
   - Soundproofing logged
4. Implement noise discipline:
   - Player can set quiet hours
   - During quiet hours: noise reduced
   - Survivors comply (or don't)
   - Violations logged
   - Compliance affects overall noise
5. Implement external detection:
   - Shelter noise creates acoustic signature
   - External threats detect shelter based on noise
   - Detection risk calculated from noise level
   - Detection triggers events (raid, surveillance)
   - Detection logged
6. Implement noise management:
   - Player can turn off noise sources
   - Player can schedule noisy activities (outside quiet hours)
   - Player can invest in soundproofing
   - Player can enforce noise discipline
   - Management logged
7. Implement shelter noise UI:
   - Noise panel: overall noise level, noise by room
   - Source detail: noise sources, output, status
   - Soundproofing panel: room soundproofing levels
   - Discipline panel: quiet hours, compliance
   - Detection panel: detection risk, external threats
   - Noise map: show noise levels per room
   - Alerts: high noise, quiet hours violation, detection risk
8. Create noise events:
    - "The Noise Spike" — sudden loud noise
    - "The Quiet Hours" — quiet hours enforced
    - "The Violation" — noise discipline violated
    - "The Detection" — external threat detected shelter
    - "The Soundproofing" — room soundproofed
    - "The Complaint" — survivor noise complaint
    - "The Silence" — shelter very quiet
    - "The Raid" — raid attracted by noise
9. Add noise quest hooks:
    - "The Silent Shelter" — maintain noise below 20 for 100 days
    - "The Soundproof" — soundproof 10 rooms
    - "The Discipline" — maintain 90%+ noise discipline compliance
    - "The Ghost" — never be detected by external threats
    - "The Librarian" — enforce quiet hours for 50 days
    - "The Acoustic Engineer" — install complete soundproofing
    - "The Silent Running" — operate shelter at minimal noise for 30 days
10. Implement noise tutorial: first noise spike explains system
11. Add noise tooltips: hover over room shows noise level
12. Create noise source definitions in data file (15+ source types)
13. Implement noise persistence: noise state saved with shelter state
14. Integrate with `PowerGridSystem`: generators/machines are noise sources

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `PowerGridSystem`: generators/machines generate noise
2. Connect to `VentilationSystem`: ventilation generates noise
3. Integrate with `ShelterThermalSystem`: thermal systems generate noise
4. Connect to `ExpeditionSystem`: external threats detect shelter noise
5. Wire into `CombatSystem`: noise affects combat detection
6. Connect to `InterpersonalConflictSystem` (Plan 202): arguments generate noise
7. Implement old-save compatibility: existing saves get no noise tracking, moderate baseline
8. Add deterministic seeding: noise events use `ISeededRng`
9. Create exploit prevention: noise is physics-based, can't be gamed
10. Add tests: noise sources, propagation, soundproofing, discipline, detection, save round-trip
11. Verify all noise source types work correctly
12. Test edge cases: no noise (silent shelter), extreme noise (very loud shelter)
13. Verify headless behavior: noise processes correctly without UI
14. Add data-integrity-selftest: noise validates against shelter/room catalogs
15. Create `--shelter-noise-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-noise-selftest
```

## Risk

**LOW** — Shelter noise is straightforward with clear inputs (noise sources) and outputs (propagation, detection). Risk of noise management feeling like another meter. Mitigation: make detection consequences real (raids), show clear cause-effect, and ensure noise discipline feels like meaningful stealth gameplay.

## Definition of Done

- `ShelterNoiseSystem.cs` exists with full `CaptureState/RestoreState`
- 9+ noise source types (machinery, human activity, industrial, alarm, ventilation, generator, construction, music, argument)
- Noise propagation mechanics (room-to-room, distance, walls, frequency)
- Soundproofing mechanics (wall/floor/door treatment, degradation, maintenance)
- Noise discipline (quiet hours, compliance, violations)
- External detection (acoustic signature, detection risk, threat attraction)
- Noise consequences (detection risk levels, raids)
- Noise events and quest hooks
- Save/load round-trip tested
- Deterministic noise events verified
- Old saves load with no noise tracking, moderate baseline
- Noise source definitions in data authority (15+ types)
- UI noise panel, source detail, soundproofing panel, discipline panel, detection panel, noise map, alerts
- Cross-system integration (power grid, ventilation, thermal, expedition, combat, interpersonal conflict)

## Follow-On Opportunities

- Noise specialization (survivors become expert acoustic engineers)
- Noise legacy (famous noise incidents remembered)
- Noise quests (specific noise management goals)
- Noise events (massive noise catastrophe, silent running operation)
- Noise trading (trade acoustic technology with other settlements)
