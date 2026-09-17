# D1 Flagship Integration Plan [19]
## Plan 205 — Shelter Noise Discipline & Acoustic Management

> **Canonical filename:** `D1_planintegration[19].md`
>
> **Previous:** `D1_planintegration[18].md`
>
> **Next:** `D1_planintegration[20].md`
>
> **Purpose:** Build one authoritative shelter-acoustics layer that turns machinery, work, construction,
> ventilation, alarms, recreation, and conflict into a persistent acoustic signature; propagates that sound
> through the shelter topology; lets physical soundproofing and operational noise discipline reduce exposure;
> and exports a bounded, deterministic external-detection signal to raid, faction, wildlife, combat, and
> expedition authorities without becoming a second threat-spawn system.
>
> **Primary source:** Plan 205 — Shelter Noise Discipline & Acoustic Management.
>
> **Core repository problem:** ASHFALL already contains several isolated notions of "noise"—underwater noise in
> `MaritimeDiveSystem`, safe-cracking noise, and radio noise in `SignalTriangulationSystem`—but there is no
> shelter-wide acoustic authority. `PowerGridSystem`, ventilation, thermal equipment, construction, industrial
> work, recreation, and arguments can all plausibly generate noise, yet nothing combines them into one
> shelter signature or exposes that signature to external detection systems.
>
> **Implementation posture:** event- and schedule-driven, room-graph aware, deterministic, source-adapter based,
> physically interpretable at game scale, bounded against per-frame graph traversal, save-safe, and explicitly
> subordinate to canonical threat, raid, combat, wildlife, faction, room, power, ventilation, and construction
> systems.
>
> **Critical guardrail:** `ShelterNoiseSystem` computes and exposes acoustic state. It does **not** independently
> spawn raids, wildlife, faction patrols, or combat encounters. It produces a typed detection-pressure/acoustic
> exposure signal; the systems that already own threat generation decide what that pressure means.
---

## 1. Source Problem Statement

The source identifies a clear systemic gap:

- no `ShelterNoise`, `NoiseDiscipline`, `SoundPropagation`, `AcousticSystem`, `NoiseDetection`, `Soundproofing`,
  `NoiseRadius`, or `AcousticSignature` authority exists;
- `MaritimeDiveSystem` noise is underwater-specific;
- `SafeCrackingSystem` noise is safe-cracking-specific;
- `SignalTriangulationSystem` radio noise is RF-domain-specific;
- Plan 138 mentions noise discipline but does not implement a system;
- Plan 71 power covers generators, but not acoustic output;
- Plan 156 shelter expansion covers construction, but not acoustic propagation;
- the shelter is effectively acoustically invisible.

The desired architecture is:

```text
authoritative shelter activity systems
 ├─ power / generators / machines
 ├─ ventilation / thermal equipment
 ├─ workshop / industrial processes
 ├─ construction
 ├─ alarms
 ├─ human activity
 ├─ recreation / music
 └─ interpersonal conflict
                ↓
        Noise Source Adapters
                ↓
         ShelterNoiseSystem
   ┌────────────┼──────────────┐
   ↓            ↓              ↓
source state  room field   acoustic envelope
   ↓            ↓              ↓
soundproofing / doors / topology / frequency
                ↓
        external acoustic signature
                ↓
        IAcousticExposureProvider
     ┌──────────┼───────────┬──────────┐
     ↓          ↓           ↓          ↓
raid/threat  factions    wildlife   combat/stealth
 systems      systems      systems      systems
```

The shelter-noise system owns the acoustic model. It does not own the consequences.
---

## 2. Flagship Success Criteria

Implementation is complete only when all of the following are true:

1. `ShelterNoiseSystem.cs` exists with schema-versioned capture/restore for truly persistent acoustic state.
2. Noise sources are keyed by stable IDs and tied to real upstream systems.
3. A generator contributes noise only while it is actually running.
4. Ventilation contributes according to real operating state.
5. Construction/industrial work contributes only during real scheduled activity.
6. Human/recreation/conflict sources derive from actual events or schedule state.
7. Noise aggregation uses a bounded acoustic model rather than naive linear summation.
8. Room-to-room propagation uses the canonical shelter topology.
9. Door/opening state affects propagation only if the room/door system actually exposes it.
10. Wall/floor/door treatments modify propagation through real construction/shelter upgrade data.
11. Soundproofing is physical state, not one shelter-wide magic percentage.
12. Quiet hours are an operational policy, not a global multiplicative silence switch.
13. Quiet hours can suppress or reschedule only sources that are actually controllable.
14. Emergency alarms, critical medical actions, raids, fires, and other mandatory events can override quiet hours.
15. Noise-discipline "compliance" is grounded in scheduler/autonomy/behavior facts if implemented; otherwise no
    fake random disobedience layer is added.
16. Acoustic source and room outputs are deterministic.
17. External acoustic signature is deterministic for the same shelter state and schedule.
18. Detection chance, if probabilistic, uses stable exposure windows and `ISeededRng`.
19. Save/load cannot reroll an already-evaluated acoustic detection window.
20. UI toggling cannot lower noise without changing real source state.
21. Soundproofing cannot be toggled to reroll external detection.
22. No per-frame all-pairs room propagation exists.
23. No external-threat scan runs once per rendered frame.
24. Threat systems receive normalized acoustic exposure through a typed adapter.
25. Raid/faction/wildlife systems remain authoritative over actual consequences.
26. The source's 0–100 noise bands are presentation/normalized outputs, not the sole internal physics.
27. Old saves initialize from their real active shelter sources on first evaluation; they do not receive an
    unexplained arbitrary "moderate baseline" that ignores actual machinery.
28. Noise events are retained selectively, not logged for every evaluation.
29. Noise-source catalog mappings validate against real source types/room IDs/capabilities.
30. `--shelter-noise-selftest` validates source integration, propagation, attenuation, policy, detection
    exposure, migration, idempotency, save round-trip, and headless behavior.
---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/shelter_noise/SHELTER_NOISE_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`
- safe-cracking noise implementation
- `SignalTriangulationSystem.cs`
- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`
- generator/machine runtime state
- `Assets/Ashfall.Core/VentilationSystem.cs`
- `Assets/Ashfall.Core/Shelter/ShelterThermalSystem.cs`
- workshop/industrial processing systems
- construction/build queues
- alarm/emergency systems
- recreation/music/social event systems
- `InterpersonalConflictSystem` from Plan 202 if implemented
- shelter room/topology graph
- door/opening state
- wall/room upgrade/material definitions
- shelter defense / raid systems
- faction awareness/scouting systems
- wildlife/ecology attraction systems
- combat detection/stealth APIs
- expedition proximity/threat APIs
- campaign clock/scheduler
- save schema/migrations
- deterministic RNG
- UI shelter overview / room map
- achievement/quest/epilogue consumers

Build an authority matrix:

| Acoustic input/consequence | Canonical owner | Noise-system role |
|---|---|---|
| generator running | PowerGrid | read source state |
| fan/vent active | Ventilation | read source state |
| furnace/pump active | Thermal/Power | read source |
| workshop action | Workshop/Crafting | read scheduled source |
| construction | Construction | read scheduled source |
| argument | Conflict system | consume event |
| music | Recreation/Culture | consume activity |
| room graph | Shelter topology | read propagation graph |
| door state | Shelter topology | read attenuation state |
| raid | Raid/Defense | expose detection pressure only |
| faction scouting | Faction/World | expose acoustic visibility |
| wildlife attraction | Ecology | expose acoustic cue |

Do not copy domain-specific RF or underwater noise mechanics into shelter acoustics merely because the field is
also named "NoiseLevel."
---

## 4. Scope Boundary

### In scope

- shelter acoustic source registry;
- room-level acoustic propagation;
- frequency bands;
- soundproofing/attenuation;
- quiet-hour policy;
- source scheduling hooks;
- acoustic signature at shelter boundary;
- external exposure/detection pressure;
- noise alerts/events;
- room-noise UI/map;
- save/load;
- old-save migration;
- tests/performance.

### Explicitly out of scope for first pass

- real-time audio-engine occlusion;
- ray-traced acoustics;
- reverberation rendering;
- waveform/DSP simulation;
- RF noise;
- underwater acoustics;
- weapon sound propagation across the whole world;
- autonomous raid spawning;
- autonomous faction patrol spawning;
- autonomous wildlife spawning;
- exact real-world decibel simulation;
- psychoacoustics;
- cross-campaign acoustic legacy.

This is a strategic shelter-management model, not an audio middleware replacement.

---
## 5. One Acoustic Authority
- Use ShelterNoiseSystem only for shelter-domain airborne/structural acoustics.
- Do not merge RF noise, underwater noise, or safe-cracking local-alert meters into one universal number.
- Adapters may normalize domain events into shelter sources only when they physically occur inside/near the shelter.
- Repository comments should explicitly distinguish acoustic noise from signal noise.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 6. Internal Units vs 0–100 UI
- Keep source-facing and UI-facing normalized values separate.
- The source's 0–100 `overallNoiseLevel` is appropriate for presentation and threshold tuning, but naive addition of 70+70 should not simply clamp to 100.
- Internally use a stable intensity-like fixed-point value or normalized energy score.
- Convert to 0–100 presentation bands at the boundary.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 7. Acoustic Combination Model
- Multiple simultaneous sources should combine with diminishing/log-like behavior rather than linearly.
- Recommended implementation can use a deterministic lookup/approximation instead of floating-point logarithms if project determinism requires it.
- Two moderately loud machines should be louder than one, but not necessarily double the normalized UI level.
- Document the exact combination rule and test associativity/order independence where intended.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 8. Frequency Bands
- Retain low/medium/high bands from the source as coarse gameplay abstractions.
- Low-frequency machinery can propagate further through structure; high-frequency voices/alarms can be attenuated more by doors/walls.
- Do not pretend these bands are literal full acoustical spectra.
- Every source definition chooses one primary band or a small band-weight profile.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 9. Noise Source Definition
- Create `noise_sources.json` with source profiles, not runtime source instances.
- Definition fields can include sourceTypeId, baseOutput, frequency profile, controllability, persistence, room-coupling, exterior-coupling, and quiet-hours policy.
- Runtime source IDs come from real machines/activities.
- Do not create one catalog entry per individual generator instance.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 10. Runtime Noise Source
- Recommended fields: sourceInstanceId, definitionId, roomId, ownerSystemId, sourceObjectId, activeState, outputScale, scheduleWindow, and sourceEvent/root transaction ID.
- Do not persist transient sources that can be reconstructed from authoritative machine/activity state unless necessary for idempotency.
- Event sources such as alarms or arguments may need explicit start/end timestamps.
- Stable source IDs prevent duplicate adapters from double-counting the same machine.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 11. Source Ownership
- PowerGrid owns generator state; NoiseSystem only reads it.
- Ventilation owns fans; Thermal owns heaters/pumps if applicable.
- Workshop/Construction own work schedules.
- NoiseSystem must not turn equipment on/off directly except through an explicit command routed to the owning subsystem.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 12. Generator Source Adapter
- Map generator model/rated load/operating mode to acoustic output using data.
- Noise may scale with active load only if PowerGrid exposes load and design wants the extra detail.
- Generator source activates/deactivates from actual power state.
- Quiet hours may recommend shutdown, but PowerGrid decides whether life-critical loads require it.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 13. Ventilation Source Adapter
- Fans/blowers become continuous low/medium sources while active.
- Emergency ventilation may override quiet policy.
- Do not duplicate air-flow simulation.
- If ventilation speed levels exist, map them to output scale.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 14. Thermal Source Adapter
- Heaters, pumps, compressors, or thermal machinery can contribute where actual components exist.
- Passive thermal state is silent.
- Do not create generic 'heat noise' just because ShelterThermalSystem is active.
- Source mapping must target real powered/mechanical equipment.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 15. Industrial Process Adapter
- Welding, cutting, forging, milling, press work, and similar activities emit high/intermittent sources only while jobs execute.
- Crafting/Workshop supplies start/end/room/job ID.
- Noise output may vary by recipe/process profile.
- Do not scan crafting queues every frame.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 16. Construction Adapter
- Construction/renovation emits high intermittent noise during active labor windows.
- Building placement alone is not noise.
- Construction jobs should have a noise profile in data or map to one registered source category.
- Scheduling noisy construction outside quiet hours is a player strategy.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 17. Alarm Adapter
- Emergency sirens/alarms are high-priority, high-output sources.
- Quiet hours must never suppress a life-safety alarm merely to preserve stealth.
- Alarm duration follows the emergency system.
- External detection pressure can rise sharply during alarms.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 18. Human Activity Adapter
- Do not emit one noise source per walking survivor.
- Model ordinary human activity as room occupancy/activity aggregate at coarse intervals.
- Use schedule/room activity groups where available.
- Low-intensity everyday human noise should rarely dominate machinery.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 19. Music/Recreation Adapter
- Plan 161/170/178 activities can emit room-local social/music sources during real events.
- Volume/intensity comes from activity template if available.
- Quiet-hours scheduling can postpone optional recreation.
- Do not create continuous background music noise from UI audio.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 20. Conflict/Argument Adapter
- Plan 202 interpersonal conflict may emit an argument event with room/time/intensity.
- NoiseSystem records a transient source.
- Do not independently roll arguments.
- Conflict consequences remain owned by relations/psychology.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 21. Medical/Emergency Activity
- Emergency treatment may be noisy but should not be blocked by quiet policy.
- MedicalPipeline may optionally expose equipment/procedure source profiles.
- Do not add medical noise unless there are meaningful machines/actions to model.
- Survival-critical activity outranks stealth.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 22. Source Lifecycle
- Continuous sources are derived from machine state.
- Scheduled sources are active during their work/activity window.
- Transient event sources carry explicit start/end or decay duration.
- Expired transient sources are removed deterministically.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 23. Room Topology Authority
- Use the canonical shelter room graph.
- Rooms are nodes; doors/openings/walls are edges or boundary properties depending repository design.
- NoiseSystem must not maintain a second room topology.
- Save references stable room IDs.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 24. Propagation Graph
- Precompute or cache acoustic adjacency from shelter topology.
- Invalidate only when construction, door topology, or soundproofing changes.
- Do not recompute all possible paths every frame.
- For modest shelter sizes, bounded graph traversal from active source rooms per evaluation window is sufficient.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 25. Propagation State
- `NoisePropagation` should primarily be a computed/debug projection, not a giant persisted list of source→target paths.
- Persist source/soundproofing state; recompute propagation after restore.
- Debug tools may expose selected paths.
- This prevents save bloat and stale path data.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 26. Distance Attenuation
- Each room transition reduces acoustic energy according to a configurable base attenuation.
- Frequency band modifies attenuation.
- Do not use raw room count as literal meters unless topology has distances.
- If edge lengths exist, use them.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 27. Wall Attenuation
- Wall/material properties contribute acoustic attenuation if shelter construction data exposes them.
- Do not hardcode material names in ShelterNoiseSystem.
- Map construction material tags to acoustic coefficients in data.
- Upgraded double walls can legitimately reduce transmission.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 28. Door State
- Open doors reduce attenuation; closed doors increase it.
- Only model this if door/opening state is canonical and performant to query.
- Door toggling invalidates local propagation cache.
- Do not let rapidly toggling a door reroll already-evaluated external detection windows.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 29. Floor/Ceiling Transmission
- Only model vertical transmission if shelter topology has floors/vertical adjacency.
- Floor treatment belongs to edge/boundary attenuation.
- Do not invent vertical geometry when shelter is effectively flat.
- Source DTO may retain future compatibility without unused live calculations.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 30. Structural vs Airborne Simplification
- Low-frequency machinery can receive a reduced wall-attenuation factor to approximate structural transmission.
- Do not create a separate vibration physics subsystem in v1.
- One low/medium/high frequency model is enough.
- Document approximation.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 31. Room Acoustic Field
- Compute one normalized current noise field per room from active sources and propagated contributions.
- Store/cache only current evaluation result and timestamp if needed.
- UI uses room field.
- Do not persist every contribution separately.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 32. Overall Shelter Acoustic Signature
- Overall internal noise and exterior signature are different.
- A loud but deeply isolated machine room can be noisy internally yet quiet outside.
- Compute exterior-coupled signature from boundary leakage or designated exterior paths.
- Do not simply use `max(noiseByRoom)` as detection risk.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 33. Exterior Leakage Model
- Identify rooms/boundaries coupled to outside: entrances, vents, thin walls, surface structures, exhausts.
- Apply boundary attenuation and source frequency.
- Aggregate leaked energy into directional or scalar exterior signature.
- V1 may use scalar shelter signature if world directionality is unsupported.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 34. Directional Signature — Optional
- If the world map/threat system supports bearings, expose north/east/south/west or boundary-specific leakage.
- Otherwise keep scalar signature.
- Do not create directionality solely for the noise system.
- Follow-on can add more spatial sophistication later.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 35. Noise Bands
- Retain source presentation bands: Low 0–20, Moderate 21–50, High 51–80, Critical 81–100.
- These are UI/risk labels, not direct raid probabilities.
- Detection systems consume normalized exposure and context.
- Thresholds live in data/config.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 36. Acoustic Exposure Contract
- Create an interface such as `IAcousticExposureProvider`.
- Inputs: observer/threat context if supported—distance, terrain, weather/ambient masking, line-of-world region.
- Output: deterministic exposure strength/signature, not a spawned event.
- Threat owners apply their own awareness/detection logic.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 37. Distance to External Threats
- NoiseSystem should not maintain the world's enemy positions.
- Threat/faction/ecology system supplies observer distance/context when querying exposure.
- Alternatively world system samples shelter signature and applies its own distance model.
- Choose the dependency direction that preserves world authority.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 38. Terrain Attenuation
- Terrain effects belong to world/route/environment data.
- NoiseSystem may accept a normalized attenuation context from the threat system.
- Do not independently classify every world tile unless no external acoustic model exists.
- Examples: dense ruins, hills, open water, underground shelter.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 39. Weather Masking
- Wind, storm, rain, or heavy environmental noise may mask shelter sound if WeatherSystem exposes a suitable modifier.
- Do not create a second weather simulation.
- Masking should be bounded and not guarantee invisibility.
- Weather effects apply at exposure evaluation, not room propagation unless shelter openings require it.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 40. Ambient World Noise
- If world systems model battle fronts, storms, industrial settlements, etc., they can provide masking/noise-floor context.
- Otherwise defer.
- Do not invent global ambient sound every tick.
- Keep v1 understandable.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 41. Detection Window
- Do not roll external detection every frame or every minute.
- Evaluate stable windows such as hourly, 6-hourly, or per threat-scan cadence.
- Accumulate acoustic exposure over the window.
- High short spikes and long moderate exposure can be distinguished.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 42. Exposure Accumulator
- Recommended inputs: peak signature, time-weighted average, duration above threshold, exceptional spike tags.
- Persist enough state to survive save/load mid-window.
- At window close, create one stable exposure result ID.
- Threat system consumes result or queries aggregate.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 43. Detection RNG
- Only the consequence owner or a dedicated detection resolver should roll probabilistic detection.
- Seed from campaign seed + observer/threat ID + exposure window ID + shelter ID.
- Persist/derive outcome deterministically.
- Reload cannot reroll.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 44. Threat Consequence Boundary
- RaidSystem owns raids.
- FactionSystem owns surveillance/hostility response.
- Wildlife/Ecology owns attraction.
- Combat detection owns tactical consequences.
- NoiseSystem emits exposure/notice facts, never directly creates all of them.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 45. Detection Event Idempotency
- `external_detection` history event is recorded only after a canonical threat system confirms detection.
- Do not log 'detected' merely because risk was high.
- Use source detection event ID.
- One detection cannot generate duplicate history through raid + journal summary.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 46. Quiet Hours Policy
- Represent quiet hours as a shelter policy with one or more clock windows.
- Policy applies to controllable optional sources only.
- Schedule boundaries should support windows crossing midnight.
- Store policy settings, not daily duplicated occurrences.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 47. Quiet Hours Schedule
- Use campaign clock/scheduler.
- At scheduling time, optional noisy work can be moved outside quiet hours where possible.
- During execution, mandatory source state remains truthful.
- Do not simply multiply all noise by 0.5 between 22:00 and 06:00.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 48. Source Quiet-Hours Policy
- Each source definition can specify `quietHoursBehavior`: unaffected, reduced, reschedulable, prohibited-if-optional, emergency-override.
- Generators may be reducible only if power policy permits.
- Construction may be reschedulable.
- Alarms are unaffected.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 49. Noise Discipline Compliance
- Do not invent a generic random compliance score unless survivor autonomy/policy systems support compliance behavior.
- Preferred: compliance is derived from the share of optional scheduled activities obeying policy and explicit violations.
- Plan 144 autonomy may later add individual refusal/behavior.
- V1 can use deterministic policy adherence.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 50. Quiet Hours Violation
- A violation requires a real controllable source running contrary to policy.
- Examples: optional workshop job, music event, non-emergency construction.
- Life-critical generator use is an override, not misconduct.
- Record one violation per source episode/window, not per tick.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 51. Player Controls
- Player can configure quiet hours, schedule noisy tasks, modify machine operating policy where owning systems permit, and install soundproofing.
- NoiseSystem provides commands that route to owners or policy services.
- Do not directly mutate foreign subsystem booleans.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 52. Emergency Override
- Fire, raid, medical crisis, ventilation hazard, and critical power events can override quiet policy.
- UI explains why noise remains high.
- Override should not count as compliance failure unless design explicitly says so.
- Survival priority is higher than acoustic stealth.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 53. Soundproofing Authority
- Physical soundproofing should live in shelter construction/upgrades if that architecture exists.
- NoiseSystem reads acoustic treatment state.
- If no generic upgrade framework exists, NoiseSystem may own sparse acoustic treatment records keyed by room/boundary.
- Do not duplicate wall/floor/door condition.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 54. Soundproofing Components
- Retain source concepts: wall treatment, floor treatment, door treatment.
- Only expose components supported by actual shelter topology.
- `overallSoundproofing` is derived presentation, not an independently editable field.
- Different frequency bands may receive different attenuation.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 55. Soundproofing Materials
- Source examples include mass-loaded vinyl, acoustic foam, and double walls.
- Use setting-appropriate item/material IDs that actually exist in ASHFALL.
- Do not add modern branded construction materials merely as text if inventory has no such items.
- Recipes/upgrades consume canonical resources.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 56. Soundproofing Installation
- Installation is a real construction/work transaction.
- Validate room/boundary, materials, labor, prerequisites, and build time.
- Commit treatment only on completion.
- NoiseSystem then invalidates propagation cache.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 57. Soundproofing Degradation
- Challenge the source's blanket degradation assumption.
- Passive wall treatments should not require frequent arbitrary maintenance.
- Only degradable elements—damaged doors, seals, improvised panels—need condition if shelter-damage systems support it.
- Do not create maintenance chores solely to force a resource sink.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 58. Soundproofing Maintenance
- If condition is modeled, maintenance should be low frequency and event/damage driven.
- Damage from raid/fire/construction can reduce treatment.
- Repair uses construction authority.
- No daily 1% decay.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 59. Acoustic Upgrade Tiers
- Optional data tiers: basic baffling, reinforced door seals, isolated machinery mount, full acoustic room.
- Each must correspond to real costs/effects.
- Do not use one shelter-wide soundproofing bar that hides where leakage occurs.
- UI can summarize overall effectiveness.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 60. Machine Isolation
- Low-frequency generator/pump noise may benefit more from isolated mounts/enclosures than wall foam.
- Represent as source-local attenuation upgrade if construction system supports machine upgrades.
- Do not over-model engineering details.
- This gives meaningful counterplay to low-frequency sources.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 61. Vent Silencers
- Ventilation may leak sound directly outside.
- An acoustic baffle/silencer upgrade can reduce exterior coupling while preserving airflow if VentilationSystem supports the tradeoff.
- Do not reduce ventilation performance unless explicitly modeled.
- Excellent follow-on integration with shelter engineering.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 62. Internal Comfort Consequences — Optional
- Room noise could affect sleep/stress later, but the source focuses on external detection.
- Do not add Needs penalties in v1 unless a clear owner/contract exists.
- Acoustic management should first prove stealth value.
- Follow-on can add sleep/work concentration.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 63. Noise Complaints
- The source proposes complaints.
- Only implement if relations/needs/autonomy can provide a real complaint trigger.
- Do not randomly generate complaints simply because room noise exceeds a threshold.
- Complaint is narrative/relationship output, not acoustic authority.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 64. Silence Event
- `The Silence` should be a milestone/quest state such as sustained very-low signature, not a daily event.
- Use duration threshold.
- No repeated notification while shelter remains quiet.
- Achievement/quest system owns rewards.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 65. Noise Spike Event
- A spike is a high transient source or rapid signature increase with stable event ID.
- Examples: explosion, alarm, construction accident, loud industrial action.
- Do not classify every generator start as a major spike.
- Use threshold + source tag.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 66. Noise Event Retention
- Persist/selectively retain policy changes, major spikes, violations, confirmed detections, and soundproofing milestones.
- Do not persist all propagation evaluations.
- Aggregate routine statistics separately if quests need them.
- This keeps save/history compact.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 67. State DTO Design
- Persist quiet-hours policy, any system-owned soundproofing state, detection-window accumulator if needed, processed event IDs, and selected history.
- Do not persist `noiseByRoom` if fully derivable from active sources/topology on restore.
- Do not persist propagated paths.
- `overallNoiseLevel` and `detectionRisk` should usually be derived projections.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 68. Source Reconstruction on Restore
- Continuous machine sources should rebuild from PowerGrid/Ventilation/Thermal state after restore.
- Scheduled work sources should rebuild from scheduler/jobs.
- Transient sources active across save must persist end time or source event state.
- Restore order must avoid a one-tick silent shelter while threat checks run.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 69. Old-Save Migration
- The source suggests 'no noise tracking, moderate baseline.' Refine this.
- Old save gets default quiet-hours policy off, no noise history, no special soundproofing unless construction data already indicates it.
- Current acoustic field is computed from real restored machines/activities.
- Do not inject an arbitrary persistent moderate noise value.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 70. Legacy Soundproofing Migration
- If Plan 138/156 or old room data already contains acoustic/soundproofing upgrade tags, map them exactly.
- Otherwise do not fabricate treatment.
- Migration is silent.
- Record migration provenance only for diagnostics.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 71. GameBootstrap Wiring
- Setup order: topology/construction → power/ventilation/thermal → scheduler/activities → ShelterNoiseSystem → threat adapters → UI.
- Restore persistent policy/treatments before first acoustic evaluation.
- Reconstruct sources after owner systems restore.
- Register selftest command.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 72. No Generic Per-Frame Tick
- Use source-change events plus fixed simulation acoustic evaluation cadence.
- Continuous sources can be cached until state changes.
- Room propagation recalculates only on source, topology, door, treatment, or scheduled evaluation change.
- Threat exposure uses bounded windows.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 73. Acoustic Evaluation Cadence
- Recommended: evaluate room field when dirty and at simulation clock checkpoints.
- Do not recompute on every rendered frame.
- For UI animation, interpolate display from last computed state without changing Core.
- Benchmark large shelters.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 74. Dirty-Set Optimization
- Track changed source rooms/boundaries.
- Recompute only affected propagation regions if complexity warrants it.
- With small shelters, full graph traversal on dirty evaluation may be simpler and adequate.
- Profile before over-optimizing.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 75. Propagation Cache
- Cache per-room edge attenuation by frequency band.
- Invalidate on topology/door/soundproofing change.
- Do not cache source amplitude into topology coefficients.
- Cache keys must be deterministic.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 76. Large Shelter Performance
- Test 100+ rooms, 100 active sources, multiple frequency bands, door changes, and soundproofing.
- Target evaluation well below day-tick budget.
- No O(sources × rooms²) all-pairs calculation if avoidable.
- Use adjacency traversal.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 77. No-Noise Edge Case
- All sources inactive and no transient event: interior and exterior signature approach floor/baseline.
- Do not force an arbitrary nonzero 'moderate baseline.'
- Optional human ambient floor may exist only if explicitly modeled.
- Detection pressure should be minimal.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 78. Extreme Noise Edge Case
- Many machines + construction + alarm must remain numerically bounded.
- UI clamps to 100 presentation while internal model retains useful differences if needed.
- Threat exposure saturates rather than overflowing.
- No NaN/overflow.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 79. All Doors Open Edge
- Open doors produce lower attenuation across relevant edges.
- Propagation remains deterministic.
- Closing one door affects only subsequent exposure/evaluations.
- Existing detection result is not rerolled.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 80. Full Soundproofing Edge
- No physically plausible treatment should produce perfect zero leakage unless an explicitly sealed/buried architecture supports it.
- Set attenuation caps/floors.
- Low-frequency leakage may remain.
- This prevents invulnerable stealth.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 81. Quiet-Hours All-Day Edge
- Allow policy if design permits, but optional work/recreation availability should reveal the tradeoff.
- Life-critical sources continue.
- Do not grant a free all-day global noise multiplier.
- Scheduler may accumulate delayed noisy work.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 82. Quiet-Hours Midnight Wrap
- Support windows such as 22:00–06:00.
- Test boundary minutes/hours.
- No off-by-one daily duplicate violations.
- Timezone is irrelevant; use campaign clock.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 83. Acoustic Signature UI
- Show current overall exterior signature, trend, and band.
- Explain top contributing sources and leakage paths.
- Do not present 'Raid chance 37%' unless threat system truly exposes that probability.
- Prefer exposure/visibility labels.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 84. Room Noise Map
- Color/heat-map room field if UI supports it, with numeric/text labels for accessibility.
- Selecting room shows local sources and top propagated contributors.
- Map reads projection only.
- No graph calculation in UI.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 85. Source Detail UI
- List source, owner system, current output band, frequency, room, controllability, schedule, and quiet-hours behavior.
- Provide action links only through owning subsystem commands.
- Explain why critical sources cannot be silenced.
- Do not fake an `isActive` toggle for immutable event noise.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 86. Soundproofing UI
- Show room/boundary treatments, expected attenuation by band, installation cost, condition if real, and major leakage points.
- Use construction upgrade workflow.
- Do not expose one meaningless global percent alone.
- Provide comparison before/after where possible.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 87. Discipline UI
- Configure quiet hours and optional source policies.
- Show scheduled conflicts and emergency overrides.
- If compliance exists, explain how it is derived.
- Do not display a random compliance bar without actionable cause.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 88. Detection UI
- Show acoustic visibility/exposure and known contributing context.
- Known threats may display whether they are within plausible hearing range only if world systems expose that info.
- Do not leak hidden enemy positions through noise UI.
- Confirmed detection is shown only after threat authority confirms it.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 89. Alerts
- Actionable alerts: critical acoustic spike, refrigeration/generator? no—only acoustic-specific issues, quiet-policy conflict, damaged soundproofing, confirmed external detection.
- Aggregate repeated violations.
- Use cooldown/hysteresis.
- No per-tick high-noise spam.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 90. Tutorial
- First meaningful noise spike explains sources, room propagation, exterior signature, quiet hours, and soundproofing.
- Keep tutorial action-oriented.
- Do not teach hidden equations.
- Demonstrate top contributor and mitigation.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 91. Quest Hooks
- Source hooks include Silent Shelter, Soundproof, Discipline, Ghost, Librarian, Acoustic Engineer, Silent Running.
- Use sustained-duration and unique-upgrade metrics, not UI toggles.
- `never detected` must define whether story-mandated/known detections invalidate it.
- QuestSystem owns progress/rewards.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 92. Achievement Integration
- Plan 149 may observe sustained low signature, complete acoustic upgrades, or surviving a detection event.
- Do not reward intentionally causing raids merely to farm noise events.
- Use stable event IDs and duration facts.
- NoiseSystem exports read-only facts.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 93. Epilogue/Archive Integration
- Plan 145/archive may consume famous silent-running periods, catastrophic noise incidents, or detection caused by a specific event.
- Only confirmed consequences count.
- Export structured facts.
- Do not let routine source logs enter epilogue.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 94. Shelter Defense Integration
- Plan 138 can query acoustic exposure as one stealth/detection input.
- Defense owns patrol, raid, concealment, fortification outcomes.
- Noise discipline is no longer a comment-only concept.
- Do not duplicate defense alert levels.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 95. Expedition Integration
- Clarify source statement that ExpeditionSystem handles external threat detection.
- If expeditions simulate enemies near shelter, they may consume acoustic signature.
- Do not make distant expedition parties hear shelter across arbitrary map distances.
- Use world proximity/context.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 96. Combat Detection Integration
- Tactical combat can consume local shelter acoustic signature if combat occurs near/in shelter.
- CombatSystem owns awareness/initiative/stealth.
- NoiseSystem provides current acoustic context.
- Do not retroactively modify completed combat.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 97. Faction Integration
- Faction scouting/intelligence may use acoustic exposure to increase chance of discovering shelter.
- Faction system owns knowledge/standing/hostility.
- Repeated acoustic detections should feed canonical awareness, not spawn duplicate faction memory.
- Stable detection IDs.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 98. Wildlife Integration
- Ecology/wildlife may treat high acoustic signature as attraction, avoidance, or disturbance depending species.
- Do not assume all wildlife is attracted.
- Species response belongs to ecology data.
- NoiseSystem provides cue intensity.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 99. Weather Integration
- Weather can mask or alter external propagation if relevant.
- Use one weather adapter/context.
- Do not modify internal shelter room propagation with outside weather unless openings make it meaningful.
- Storm masking should not guarantee stealth.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 100. Interpersonal Conflict Integration
- Plan 202 owns arguments/conflict resolution.
- Noise adapter consumes start/end/intensity/room.
- Quiet-hour violation can be logged without changing relationship outcome.
- No random argument creation here.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 101. Culture/Recreation Integration
- Plan 170/178 events can provide source profile and schedule.
- Concert in soundproofed room may have high internal but lower external signature.
- This creates meaningful event-planning tradeoffs.
- Do not double count a celebration and its music as separate identical sources.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 102. Shared Event Deduplication
- Use one root activity/event ID across celebration, music, argument, construction, alarm, and summary fan-out where appropriate.
- NoiseSystem creates one source per physical source, not per observing system.
- Summaries do not re-add noise.
- Mandatory test.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 103. Source Catalog Validation
- Validate 15+ source definitions only after mappings exist.
- Each definition needs source type, frequency profile, output, control policy, and localization.
- Flag definitions with no upstream emitter.
- Do not pad the catalog with dead source types.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 104. Topology Validation
- Every room ID resolves.
- Edges are valid and symmetric/directional according to shelter topology.
- Door and boundary references resolve.
- Soundproofing applies to a real room/boundary.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 105. Attenuation Validation
- All coefficients are bounded and nonnegative.
- No edge amplifies sound unless an explicit resonance mechanic exists—defer that in v1.
- Frequency profiles normalize correctly.
- Exterior leakage remains bounded.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 106. Policy Validation
- Quiet-hour start/end valid.
- Enforced rooms exist.
- Source policy enums registered.
- No duplicate overlapping policies unless multiple schedules are intentionally supported.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 107. Detection Contract Validation
- Threat integrations must consume one normalized exposure interface.
- No direct calls from NoiseSystem to `StartRaid()` or similar.
- Search repository for forbidden consequence calls.
- CI architecture test can enforce dependency direction.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 108. Deterministic RNG
- Acoustic physics itself should be deterministic arithmetic.
- `ISeededRng` is used only for external probabilistic detection/event variation where the owning system needs it.
- No wall clock.
- No nondeterministic collection ordering.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 109. Save State
- Persist policy, system-owned treatments, transient source timing if unreconstructable, exposure-window accumulator, processed event IDs, and selected noise history.
- Derive active machine sources and room fields.
- Version state.
- Keep save compact.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 110. Save/Load Mid-Noise Spike
- Transient alarm/argument source persists remaining duration or root event state.
- Reload resumes the same acoustic exposure.
- Already-closed detection window does not reroll.
- One event ID.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 111. Save/Load Mid-Quiet Hours
- Campaign clock determines whether policy is active.
- No duplicate start/stop event after reload.
- Scheduled work remains consistent.
- Emergency overrides persist through owning system state.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 112. Save/Load After Door Change
- Door state owned elsewhere restores first.
- Noise propagation cache invalidates/rebuilds.
- Do not persist stale paths.
- Same result as uninterrupted simulation.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 113. Save/Load After Soundproofing
- Construction upgrade/treatment state restores.
- NoiseSystem recomputes attenuation.
- No double application.
- Installation event not replayed.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 114. Anti-Toggle Exploit
- Turning an optional source off lowers future signature; that is legitimate.
- Repeated on/off toggling cannot erase exposure already accumulated in current window.
- Detection-window accumulator integrates actual time active.
- No 'check only at midnight' exploit.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 115. Anti-Schedule Exploit
- Moving a noisy job after it already ran cannot retroactively remove noise.
- Scheduler changes affect future intervals.
- Source events include actual execution time.
- History remains immutable.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 116. Anti-Door Exploit
- Door toggles alter future propagation.
- Already accumulated exterior exposure stays.
- Rapid toggling may be rate-limited by actual door mechanics, not noise system.
- No detection reroll.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 117. Anti-Soundproofing Exploit
- Install completion timestamp defines when attenuation begins.
- Do not apply new treatment retroactively to earlier exposure in same window.
- Construction transaction is idempotent.
- Removing/reinstalling cannot farm quests.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 118. Anti-Quiet-Hours Exploit
- Enabling quiet hours after a loud event cannot erase it.
- Quest duration counts real compliant intervals.
- Policy toggle spam does not accumulate compliance.
- Use stable schedule periods.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 119. Exposure Window Persistence
- Persist start time, integrated exposure measure, peak, and exceptional spike tags when needed.
- After restore, continue from exact simulation time.
- At close, emit one result.
- Then clear accumulator.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 120. Noise Statistics
- Optional aggregate metrics: time in each band, peak daily signature, confirmed detections, violations, source contribution totals.
- Use them for quests/UI trends.
- Do not persist every sample.
- Downsample long histories.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 121. Acoustic Trend UI
- Show recent daily/weekly exterior signature trend if useful.
- Aggregate peak/average per period.
- Do not make trends a second authority.
- Bound retention.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 122. Performance Benchmark
- Benchmark representative shelter: 100 rooms, 100 sources, three frequency bands, 20 door changes, multiple treatment upgrades.
- Measure propagation evaluation time, allocations, and dirty-cache invalidation.
- Also benchmark exposure-window updates.
- Document budget.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 123. Allocation Budget
- Reuse adjacency/cache structures.
- Avoid allocating path lists for every propagation during normal play.
- Debug path reconstruction can allocate on demand.
- Use pooled/stack structures only if profiling shows need.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 124. Property Tests
- Adding attenuation cannot increase propagated noise.
- Closing a door cannot increase target noise under normal rules.
- Deactivating a source cannot increase total acoustic energy.
- Soundproofing cannot increase exterior signature.
- Same source/topology state yields same output.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 125. Graph Fuzz Tests
- Generate valid connected/disconnected shelter graphs, door states, treatment coefficients, and source rooms.
- Assert bounded outputs and no infinite traversal.
- Disconnected rooms receive no propagated contribution unless structural coupling explicitly connects them.
- Deterministic ordering.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 126. Frequency Tests
- Low-frequency source should propagate at least as far as high-frequency under configured default profiles if source policy intends that.
- Treatments can attenuate bands differently.
- No missing frequency definition.
- Band-combination output bounded.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 127. Detection Fuzz Tests
- Randomize exposure strengths, durations, observer distances/context, and stable seeds.
- Threat-side detection result is deterministic.
- NoiseSystem never creates direct raid in these tests.
- Save/load mid-window preserves outcome.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 128. Old-Save Tests
- Restore old save with generator on, ventilation on, construction active.
- Expected first acoustic evaluation reflects those real sources.
- No arbitrary 'moderate baseline' state persists.
- No migration notification flood.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 129. UI Snapshot Tests
- Silent shelter.
- Generator-heavy shelter.
- Construction spike.
- Quiet hours conflict.
- Soundproofed machine room.
- Confirmed external detection.
- High text scale / keyboard focus.
- Noise map with several rooms.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 130. Data Integrity Selftest
- Validate source profiles, room/topology references, soundproofing definitions, frequency bands, source adapters, quiet-hours policies, localization.
- Validate every base source definition has at least one emitter or is marked reserved.
- Validate threat adapter registration.
- Fail CI on orphan mappings.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 131. Dedicated Selftest
- `--shelter-noise-selftest` builds a tiny room graph and controlled source set.
- Turn generator on/off, open/close door, install treatment, start quiet hours, run construction, trigger alarm, accumulate external exposure, save/reload, and confirm deterministic results.
- Verify detection handoff is emitted as exposure, not direct raid.
- Exit non-zero on mismatch.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 132. Verification Commands
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`.
- `dotnet build Ashfall.csproj`.
- `godot --headless --path . -- --data-integrity-selftest`.
- `godot --headless --path . -- --shelter-noise-selftest`.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 133. Implementation Phase A — Authority Audit
- Map all source emitters, room graph, construction/soundproofing, and threat consumers.
- Freeze source IDs and dependency direction.
- Document old-save behavior.
- Exit when no duplicate acoustic authority remains.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 134. Implementation Phase B — Acoustic Core
- Implement source definitions, runtime source registry, normalized energy model, frequency bands, room aggregation, and deterministic projection.
- Add source lifecycle and idempotency.
- Exit with generator/ventilation/human fixtures.
- No detection yet.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 135. Implementation Phase C — Propagation
- Wire shelter topology, edge attenuation, door state, frequency-dependent propagation, exterior leakage, cache invalidation, and graph tests.
- Do not persist paths.
- Exit when room/noise map is correct headlessly.
- Benchmark.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 136. Implementation Phase D — Soundproofing
- Wire construction/upgrades, wall/door/floor/source isolation where supported, installation transactions, optional condition/damage, and cache invalidation.
- Reject fake daily degradation.
- Exit with before/after attenuation fixtures.
- Add material validation.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 137. Implementation Phase E — Quiet Hours
- Implement policy schedule, source behavior classes, scheduler integration, legitimate violations, emergency overrides, and duration statistics.
- Do not use blanket multipliers.
- Exit with midnight-wrap and override tests.
- Add UI policy projection.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 138. Implementation Phase F — External Exposure
- Implement exterior acoustic signature, exposure windows, threat-query interface, context inputs, deterministic handoff, and confirmed-detection record ingestion.
- Keep consequence ownership external.
- Exit with raid/faction/wildlife adapter contracts.
- Add save/load mid-window tests.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 139. Implementation Phase G — Cross-System Sources
- Wire generators, ventilation, thermal machinery, industrial work, construction, alarms, recreation, culture, and Plan 202 arguments.
- Use root event IDs to deduplicate.
- Validate all 15+ source profiles against real emitters.
- Remove dead/filler sources.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
## 140. Implementation Phase H — UI
- Build overall signature, room map, source detail, soundproofing, quiet-hours discipline, detection exposure, alerts, trend summary, and tooltips.
- Use projection DTOs only.
- Hide unknown threat positions.
- Add accessibility snapshots.

Implementation consequence: this section must be proven through a concrete source adapter, state transition, topology/acoustic calculation, downstream interface, or CI fixture. Avoid leaving acoustics as descriptive metadata that never affects the authoritative shelter or world loop.

---
