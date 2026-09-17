# D1 Flagship Integration Plan [7]
## Plan 157 — Communications & Radio Network Infrastructure

> **Purpose:** turn ASHFALL radio from a receive-only content surface into a deterministic, power-aware, buildable communications infrastructure layer that supports reception, transmission, interception, encryption, jamming, settlement/outpost networking, and typed intelligence handoffs without replacing existing radio, shelter, expedition, faction, or espionage authorities.
>
> **Primary source:** Plan 157 — Communications & Radio Network Infrastructure.
>
> **Core defect:** `FactionRadioEngine.cs`, `RadioTuner.cs`, and `VerdictRadioSystem.cs` already provide broadcast/tuning content, but the player owns no physical communications infrastructure. There is no canonical antenna state, network topology, transmitter authority, interception pipeline, jamming effect, encryption/key state, or persistent communications capacity.
>
> **Implementation posture:** data-driven, deterministic, power-coupled, topology-aware, save-compatible, headless-testable, and integration-first. The system should make communications strategically useful while avoiding frequency-management busywork.


---

## 1. Problem Statement and Architectural Intent

The source plan correctly distinguishes **radio content** from **communications infrastructure**. The current project can emit authored broadcasts and let the player tune to them, but this does not answer whether the shelter has an antenna, whether the antenna has power, whether a distant station is reachable, whether a secure channel can be intercepted, or whether the player can transmit anything back.

Plan 157 therefore introduces one authoritative communications layer:

```text
Shelter construction / power / antenna hardware
                  ↓
          CommunicationsSystem
                  ↓
     link budget + channel/network state
       ├─ reception capability
       ├─ transmission capability
       ├─ interception opportunities
       ├─ encryption/decode state
       └─ jamming/interference state
                  ↓
 existing systems consume explicit outputs
   ├─ FactionRadioEngine / RadioTuner
   ├─ VerdictRadioSystem
   ├─ EspionageSystem
   ├─ Expedition / outpost systems
   ├─ faction/diplomacy systems
   └─ UI / journal / quest hooks
```

The communications layer owns infrastructure and signal reachability. Existing radio engines remain content authorities. Espionage remains intelligence-operation authority. Shelter power remains power authority.


---

## 2. Flagship Success Criteria

The implementation is complete only when all of these are true:

1. `CommunicationsSystem.cs` owns persistent communications infrastructure state.
2. Antenna definitions are catalog-backed and validated.
3. Antenna instances have location, condition, power demand, capabilities, and installation identity.
4. Communications networks have stable IDs, member stations, channels/frequencies, encryption policy, and operational state.
5. Reception depends on actual powered infrastructure rather than only tuner selection.
6. Transmission is an explicit Core action with range, power, channel, and recipient/reachability checks.
7. Interception generates typed intercepted-message records and does not merely unlock prose.
8. Decode state is persisted and deterministic.
9. Jamming produces a real typed communications impairment consumed by supported systems.
10. Encryption has defined key/compatibility semantics and does not become an arbitrary 0–100 magic wall.
11. Existing radio broadcasts continue functioning through a compatibility adapter.
12. Old saves with no communications block load safely.
13. Mid-network and mid-decode save/load is stable.
14. No per-frame full-network simulation is required.
15. `--communications-selftest` verifies headless behavior.
16. All antenna/network/message/faction/station references validate.
17. UI renders a projection and does not calculate signal truth independently.
18. A campaign with no antennas behaves intentionally and does not crash radio content paths.
19. Power loss, damage, weather interference, and jamming all have deterministic effects.
20. The system remains optional enough that players not specializing in communications can still play the campaign.


---

## 3. Repository Reconnaissance Before Editing

Create `docs/communications/COMMUNICATIONS_INTEGRATION_AUDIT.md` before implementation. Inspect at minimum:

- `Assets/Ashfall.Core/Radio/FactionRadioEngine.cs`
- `Assets/Ashfall.Core/Radio/RadioTuner.cs`
- `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs`
- radio frequency/broadcast catalogs and validators
- shelter construction/room/installable systems
- shelter electrical/power-grid authority
- weather/interference systems
- expedition/outpost/location topology
- faction standing and faction communication hooks
- Plan 153 espionage/intelligence contracts if implemented
- survivor task assignment and decoding/research skill systems
- save capture/restore
- seeded RNG authority
- journal/notification/event bus
- UI panel architecture
- localization and asset registries

For every candidate integration, record canonical owner, stable ID, persistence contract, event contract, and whether communications may mutate it directly. The expected answer should usually be “consume through adapter/sink,” not direct field writes.


---

## 4. Scope Boundaries

**In scope:** antennas, stations, channels, networks, link reachability, communications capacity, reception gating, transmission, interception records, decode workflow, jamming effects, encryption/key compatibility, condition/damage, power coupling, deterministic interference, save migration, UI projection, journal/events, CI validation, and typed handoffs to radio/espionage/expedition/faction systems.

**Not in the first cut:** satellite simulation, autonomous RF spectrum AI, detailed electromagnetic propagation physics, procedural voice generation, full cyberwarfare, cross-campaign upgrades, platform networking, multiplayer communications, or a second faction-intelligence system.


---

## 5. Canonical Domain Types

Create explicit Core DTOs, adjusted to project conventions:

```csharp
AntennaDefinition
- id
- antennaType
- rangeKm
- sensitivity
- channelCapacity
- supportedBands
- transmitPowerClass
- baseCondition
- powerRequirement
- weatherTolerance
- buildRecipeId / installableId if canonical

AntennaInstance
- instanceId
- definitionId
- locationId
- condition
- enabled
- orientation/targetSector if directional
- assignedPowerCircuitId if supported

CommunicationsNetwork
- networkId
- displayNameKey
- channelId / frequencyId
- encryptionProfileId
- memberStationIds
- status

InterceptedMessage
- messageId
- sourceStationId
- sourceFactionId
- channelId
- encryptionProfileId
- interceptedDay
- decodeState
- intelligenceValueBand
- payloadRef

CommunicationsState
- schemaVersion
- antennaInstances
- networkInstances
- interceptedMessages
- decodeJobs
- encryptionKeys
- jammingEffects
- transmissionLedger / idempotency markers
```

Avoid storing player-facing strings as state authority.


---

## 6. Ownership Matrix

`CommunicationsSystem` owns physical communications instances, signal/link computation, network membership, transmission scheduling, interception records, decode state, jamming state, and encryption/key state.

It does **not** own shelter resources, construction costs, power generation, survivor health, faction standing, expedition travel, espionage suspicion, radio broadcast content, or quest completion. Those systems remain canonical owners and communicate through explicit commands/events.


---

## 7. Antenna Catalog and Instance Split

Definitions describe capabilities; instances describe installed hardware. Never mutate catalog definitions at runtime to represent damage or power state. A `directional` antenna definition may support orientation, while each installed instance stores its current target sector or link target. This keeps data reusable and save state compact.


---

## 8. Antenna Type Baseline

The source proposes basic, directional, parabolic, and phased-array categories. Preserve these as capability families, but move exact range/channel/sensitivity values into data. Do not hardcode 5/20/50/100 km as permanent constants until world-scale audit proves those numbers meaningful.

Recommended capability progression:
- **basic:** omnidirectional, low sensitivity, one/few channels, cheap power;
- **directional:** improved link budget in selected bearing/target, moderate channel capacity;
- **parabolic:** high gain, narrow beam, long-distance point links, greater orientation constraints;
- **phased array:** late-game adaptive high-capacity array, high power and maintenance cost.


---

## 9. Ten-Antenna Target Strategy

The source requests ten antenna types. Do not create ten cosmetic copies. Reach ten only through meaningful capability variants, for example damaged/scavenged field sets, mast-mounted basic, directional Yagi, high-gain array, parabolic dish, compact outpost relay, hardened military set, weather-resistant array, phased array, and specialized intercept array—only where distinct downstream mechanics exist.

Generate an antenna coverage report and reject definitions whose only difference is arbitrary stat inflation.


---

## 10. Station Model

Introduce a stable station identity separate from antenna identity. A station can represent shelter, outpost, allied settlement, relay site, or faction transmitter. One station may host multiple antennas. Networks connect stations, not individual antennas. This prevents topology from collapsing when an antenna is repaired or replaced.


---

## 11. Frequency and Channel Authority

Audit whether `radio_frequencies.json` actually exists and is authoritative. Prefer stable `channelId` or `frequencyId` references over raw floating-point MHz in save state. If raw frequencies are required for tuner UX, normalize them through a catalog to avoid precision mismatch.

Example:
```text
channel.faction_garrison_command -> 91.3 MHz
channel.shelter_emergency -> 102.7 MHz
```

The tuner can still display MHz while Core logic uses stable IDs.


---

## 12. Link-Budget Model

Do not simulate full radio physics. Use a deterministic abstract link score:

```text
linkScore = transmitterPower
          + txAntennaGain
          + rxSensitivity
          + relayBonus
          - distanceLoss
          - terrain/weatherInterference
          - antennaDamagePenalty
          - jammingPenalty
          - powerInstabilityPenalty
```

Compare against capability thresholds: receive, intercept, transmit-return, reliable-network. Keep coefficients data-driven and testable.


---

## 13. Distance and World Topology

If the world already has coordinates/routes, derive station distance from canonical topology. Do not create a second map inside communications. If only logical regions exist, use region-distance bands. The communications layer must consume topology; it must not own geography.


---

## 14. Reception Capability

`RadioTuner` should ask a communications capability provider whether a selected channel is receivable at the current shelter/station. Existing broadcasts remain authored by radio systems. Communications determines whether infrastructure can hear them and at what quality band.

Define fallback compatibility for legacy campaigns before antennas exist: either the shelter starts with a baseline receiver, or existing radio content is treated as using built-in hardware. Document the product decision so Plan 157 does not accidentally delete established gameplay.


---

## 15. Reception Quality Bands

Use deterministic bands such as unavailable, marginal, readable, strong. Quality may affect static, transcript completeness, interception reliability, or tuner UI. Avoid making low quality randomly drop critical quest information unless quest systems support recovery/repeat semantics.


---

## 16. Transmission Contract

Create an explicit action:

```csharp
TryTransmit(TransmissionRequest request) -> TransmissionResult
```

Request includes source station, channel/network, message template/payload ID, transmission type, encryption profile, and intended recipient scope. Validate powered transmitter, supported band, network membership, range, and cooldown before commit.


---

## 17. Transmission Types

The source proposes alerts, trade offers, propaganda, and distress calls. Treat these as typed message intents with downstream consumers. Do not permit arbitrary prose to silently trigger game effects. A player-facing authored message can reference a `TransmissionIntent` such as `distress`, `trade_offer`, `allied_alert`, or `propaganda`.


---

## 18. Transmission Reachability

For targeted network traffic, compute whether each member station is reachable through direct or relay links. For broadcasts, compute recipient set from stations/factions listening to the band. Persist stable transmission IDs so duplicate day ticks or reloads do not deliver twice.


---

## 19. Communications Network Topology

A network is a logical grouping of stations sharing channel and encryption policy. Keep topology simple in v1: star/direct links and optional relays. Do not implement internet-style dynamic routing until actual outpost scale requires it.

Network health can be derived from reachable member percentage rather than a magic stored score.


---

## 20. Network Creation Validation

Validate:
- unique network ID;
- source station exists;
- channel is supported;
- at least one powered antenna can transmit/receive;
- member stations exist;
- encryption profile/key compatibility exists;
- concurrent channel capacity not exceeded;
- network name/localization policy valid.


---

## 21. Network Membership

Members should be stations, not factions. An allied settlement can join if diplomacy/outpost systems authorize it. Communications records the member link; diplomacy remains owner of whether the settlement agrees to join.


---

## 22. Relay Stations

Outposts can act as relays only if they have compatible communications equipment and power. Relay benefits should come from topology, not a global “network range” stat that ignores the actual member layout.


---

## 23. Capacity and Channels

The source mentions bandwidth/channels. Model a small discrete channel-capacity budget per station/antenna. Avoid continuous telecom simulation. Capacity can limit simultaneous network service, jamming, decoding receiver use, or broadcast operations if this creates useful decisions.


---

## 24. Power Integration

Every powered communications function must consume the shelter/outpost power authority. Recommended operation modes:
- receive idle draw;
- active transmit draw;
- jamming high draw;
- high-grade encryption/decode compute draw if compute systems exist.

Do not subtract abstract “communications power” from a second resource pool. Use the canonical grid API.


---

## 25. Brownout and Power-Loss Behavior

On insufficient power:
- receivers may downgrade quality or go offline;
- transmissions fail before sending;
- jamming stops;
- decode work pauses if computing power is required;
- network status changes deterministically.

Power restoration resumes eligible services without duplicating messages.


---

## 26. Antenna Construction

Construction should route through the existing building/crafting/shelter expansion authority. Communications receives an installed antenna event/instance reference after construction commits. It should not directly remove inventory resources or labor.


---

## 27. Antenna Condition and Damage

Condition 0–100 may be stored per instance if the project uses durability. Degradation sources can include weather, use, combat/raid damage, and neglected maintenance. Use canonical durability/maintenance systems if available. Otherwise keep v1 degradation minimal and deterministic.


---

## 28. Weather and Atmospheric Interference

Communications consumes weather state and maps it to interference profiles. Avoid random noise per frame. Compute stable day/segment modifiers from weather event IDs or seeded periods. Important radio story broadcasts should have design-safe fallback behavior if temporarily unreadable.


---

## 29. Interception Opportunity Model

Interception should not mean scanning every frequency every frame. Define explicit opportunities when:
- a faction transmission event occurs;
- the player has a powered receiver covering that band;
- signal quality exceeds intercept threshold;
- channel capacity is available;
- optional intercept mode is enabled.

This is event-driven and cheap.


---

## 30. Intercepted Message State

An intercepted message stores metadata and a payload reference. It should not automatically expose canonical hidden truth. Fields should include source (or unknown source), channel, encryption profile, acquisition day, signal quality, decode state, confidence/intelligence value, and payload/fact reference.


---

## 31. Espionage Handoff

Plan 153 owns intelligence interpretation/espionage consequences. Communications should expose a decoded/intercepted message through a typed bridge such as:

```text
CommunicationsDecodedIntercept
 -> Espionage/Intelligence intake
```

Do not duplicate faction suspicion, intelligence reports, or covert-operation state if Plan 153 already owns those concepts. Where Plan 153 is absent, communications may keep minimal intercepted-message state until a consumer exists.


---

## 32. Decode Workflow

Create a deterministic decode job:
```text
intercepted -> queued -> decoding -> decoded | failed/corrupted
```

Audit survivor skill/task systems and computing infrastructure. Assigning a decoder should reserve a survivor through canonical task authority. Persist start day, completion day, assigned survivor, message ID, and deterministic seed if failure is probabilistic.


---

## 33. Decode Difficulty and Encryption

Do not reduce encryption to one arbitrary `0-100` field if richer semantics are needed. Prefer named encryption profiles with strength, key requirements, compute cost, and decode difficulty. Example tiers: none, field cipher, rotating codebook, hardened cipher, one-time-pad-like “not breakable without key” profile.


---

## 34. Encryption Keys

Networks using encryption need key identity and distribution semantics. Store stable `encryptionKeyId` references. Network members must possess compatible keys. Key rotation can be deferred. Never imply “encryption prevents interception” absolutely; it prevents readable content, while metadata interception may still occur.


---

## 35. Own-Transmission Encryption

Before sending encrypted traffic, validate key availability for intended recipients. If one member lacks the key, either exclude it, block the send, or downgrade only with explicit player confirmation. No silent plaintext fallback.


---

## 36. Failed Decoding

The source says messages may be lost/corrupted. Prefer bounded failure semantics: failure may produce partial metadata, require more time/resources, or permanently corrupt only when design supports it. Do not allow repeated free rerolls. Seed once when decode starts and persist that seed/result intent.


---

## 37. Jamming Contract

Jamming is a high-power, detectable communications effect. Represent it as a typed effect targeting a channel, station, network, or faction band. Validate transmitter capability, power, range, and target before activation.


---

## 38. Jamming Effects

Supported effects may include reduced signal quality, disabled long-range link, delayed transmissions, or reduced interception quality. Every jamming template must have a real consumer. Avoid generic “enemy communications effectiveness -20%” if no faction operation system reads it.


---

## 39. Jamming Detection and Consequences

Jamming should be hard to hide. Emit a detectable electromagnetic incident that faction/espionage systems may attribute. Diplomatic or military consequences belong to their authorities. This makes jamming strategically distinct from quiet interception.


---

## 40. Counter-Jamming Hook

Prepare, but do not overbuild, counter-jamming support: frequency shift, stronger signal, directional antenna, hardened encryption/network routing, or destroy jammer. The initial system can expose a `JammingDetected` event and supported mitigation tags.


---

## 41. FactionRadioEngine Integration

Preserve `FactionRadioEngine` as content/broadcast authority. Communications adds a capability filter and metadata capture layer. It should not rewrite faction broadcast corpus ownership.

Compatibility path:
```text
FactionRadioEngine emits broadcast candidate
 -> Communications evaluates reception/intercept capability
 -> Radio presentation receives playable/readable broadcast
```


---

## 42. RadioTuner Integration

`RadioTuner` should consume available channels and signal quality from communications infrastructure. Tuner selection remains player-facing input; it no longer assumes every signal is physically receivable.


---

## 43. VerdictRadioSystem Integration

Audit whether Verdict radio content is critical-path. If so, define required infrastructure/fallback semantics before gating it. The communications layer should enrich Verdict radio rather than accidentally make the ending inaccessible because a player failed to build an optional antenna.


---

## 44. Distress Signal Integration

Existing distress-signal content can become two-way: receive distress, optionally acknowledge/respond, and dispatch expedition through the proper bridge. The communications system emits a transmission acknowledgement; quest/expedition systems decide consequences.


---

## 45. Expedition and Outpost Integration

Outposts may host stations, relay traffic, send status messages, and request help. Communications consumes outpost identity/location/power. Expedition remains owner of travel and outpost establishment.


---

## 46. Faction/Diplomacy Integration

Transmissions such as trade offers or propaganda should emit typed intents to faction/diplomacy systems. Communications verifies delivery; diplomacy decides whether the recipient cares, accepts, or becomes hostile.


---

## 47. Plan 153 Espionage Integration Boundary

Communications owns physical interception and decoded communications artifacts. Espionage owns covert operation suspicion, intelligence use, sabotage missions, and agent risk. Shared types should be narrow and immutable to prevent two intelligence authorities.


---

## 48. Shelter Expansion Integration

If Plan 156 installs rooms/modules, antennas should be installables anchored to shelter locations or exterior mounts. Communications stores the installed instance, while shelter construction owns placement validity and build transactions.


---

## 49. Communications Events

Support stable domain events such as:
- `AntennaInstalled`
- `NetworkEstablished`
- `TransmissionSent`
- `TransmissionDelivered`
- `InterceptCaptured`
- `InterceptDecoded`
- `JammingStarted`
- `JammingDetected`
- `NetworkWentOffline`
- `NetworkRestored`

Quest/journal/UI systems subscribe instead of polling communications internals.


---

## 50. Quest Hooks

The source proposes “The Spy,” “The Messenger,” “The Jammer,” “The Codebreaker,” “The Broadcast,” “The Network,” and “The Silence.” Expose event hooks first. Quest authoring may follow separately. Do not hardwire quest IDs inside communications logic.


---

## 51. Journal Integration

Log major player-meaningful events: first long-range link, important decoded intercept, network outage/restoration, intentional broadcast, major jamming event. Do not log every background signal-strength recalculation.


---

## 52. Tutorial and Guidance

First-use guidance should teach three ideas only: hardware determines reach, power determines availability, and secure/intercepted traffic may need decoding. Defer advanced jamming/encryption guidance until the player unlocks those capabilities.


---

## 53. UI Projection Contract

Provide a read-only model containing installed antennas, operational status, network list, coverage/reachability, intercepted messages, decode jobs, active jamming, and available actions. UI must not independently compute range, power sufficiency, or decode outcomes.


---

## 54. Communications Panel Layout

Recommended tabs or sections:
1. **Infrastructure** — antennas, condition, power, location.
2. **Networks** — members, channel, encryption, reachability.
3. **Intercepts** — raw/decoded messages and decode jobs.
4. **Transmit** — permitted message intents and target network.
5. **Spectrum/Status** — optional compact signal/jamming overview.

Keep advanced RF detail behind tooltips/secondary panels so the main surface remains strategic rather than technical clutter.


---

## 55. Player-Facing Signal Clarity

Use clear labels: Offline, Weak, Readable, Strong, Jammed, Key Missing, Decode Required. Do not rely on color alone. If exact ranges are shown, ensure world-distance units are meaningful and consistent.


---

## 56. Accessibility

Support keyboard/controller navigation where globally supported, text scaling, reduced motion, status icons plus text, captions/transcripts for audio content, and no critical information available only by waveform/color. Intercepted audio should have textual transcript once decodable if the game’s accessibility policy requires it.


---

## 57. Save State

Persist infrastructure instances, networks, station links, messages, decode jobs, keys, jamming effects, and stable transaction/event IDs. Do not persist derived signal-strength values if they can be recomputed from canonical world state; persist only inputs needed for deterministic restoration.


---

## 58. Old-Save Migration

Missing communications state initializes schema v1 safely. Product decision required: provide baseline built-in receiver so old radio gameplay remains accessible, or migrate an initial basic antenna installation. Do not silently disable legacy broadcasts in old campaigns.


---

## 59. Mid-Decode Save/Load

Start decode, save halfway, reload, continue, and assert identical completion day/result. Any seeded decode failure/success seed is assigned before save and restored exactly.


---

## 60. Mid-Transmission and Delivery Idempotency

Every transmission gets a stable ID. Delivery consumers use that ID to suppress duplicates if a save is restored near a delivery boundary. Reopening the panel must never resend a message.


---

## 61. Network Failure and Restoration

A network can become offline because of power, damaged antenna, destroyed relay, jamming, or missing key compatibility. Derive a reason set and expose it to UI. Restoring the cause should recompute network health without manually toggling unrelated flags.


---

## 62. Data Catalogs

Use data authorities such as:
- `communications_antennas.json`
- `communications_network_templates.json`
- `communications_channels.json` only if no canonical frequency catalog exists
- `communications_encryption_profiles.json`
- `communications_interference_profiles.json`

Do not create duplicate catalogs when an existing radio catalog already owns the concept.


---

## 63. Fifteen-Network Template Strategy

The source requests fifteen network templates. Treat templates as authored starting configurations/use cases—not fifteen prebuilt runtime networks. Examples may include shelter emergency net, ally trade net, outpost relay net, secure command net, expedition support net, and faction liaison net. Reach fifteen only if each has distinct membership/encryption/channel/capability semantics.


---

## 64. Catalog Validation

Validate unique IDs, supported bands, legal ranges, positive power requirements, channel capacity, construction references, encryption profiles, station/member targets, localization keys, weather profiles, jamming capability, and network-template dependencies. Fail definitions promising unsupported gameplay effects.


---

## 65. Deterministic RNG

Use `ISeededRng` only for outcomes that actually need uncertainty: decode corruption, detection of interception, atmospheric transient events, or similar. Link reachability itself should normally be deterministic arithmetic. Seed from campaign seed + stable event/message/job ID, never wall-clock time.


---

## 66. Interception Detection Risk

Passive reception may be undetectable; active probing, decryption attempts, or direction-finding can be detectable depending on design. Do not assume all interception magically alerts factions. Define explicit operation profiles and route detection consequences to Plan 153/faction systems.


---

## 67. Exploit Prevention

Prevent: free repeated decode rerolls, save-scummed interception detection, infinite broadcast spam, jamming without power, simultaneous use beyond channel capacity, duplicate network membership, keyless encrypted delivery, construction without canonical cost, relay links through offline stations, and repeated delivery after reload.


---

## 68. Performance Budget

Communications should be event/day-driven. Cache station topology and recompute only on relevant changes: antenna install/damage, power state change, station move, weather profile change, jamming start/stop, network membership change. Do not run all-pairs signal calculations every frame.


---

## 69. Data-Integrity Self-Test

Extend the standard integrity gate to validate every antenna, network template, channel, encryption profile, station reference, construction reference, effect sink, localization key, and supported band. Verify no duplicate frequency IDs and no template references an unimplemented capability.


---

## 70. Dedicated `--communications-selftest`

The selftest should:
1. load catalogs;
2. instantiate baseline station + basic antenna;
3. verify reception in range and failure out of range;
4. establish a network;
5. send and deliver a transmission once;
6. capture an intercept;
7. save/reload during decode;
8. decode deterministically;
9. start jamming and verify supported effect;
10. cut power and verify outage;
11. restore power and verify recovery;
12. test damaged antenna quality;
13. validate old-save migration;
14. prove UI scenes are unnecessary;
15. exit non-zero on mismatch.


---

## 71. Unit Test Matrix

**Antenna:** load, install, invalid location, damage, repair, power off/on, band compatibility.

**Network:** create, duplicate ID, member add/remove, unreachable member, relay path, key mismatch, channel-capacity limit.

**Reception:** strong/weak/unavailable, weather penalty, damage penalty, jammer penalty.

**Transmission:** successful delivery, out-of-range, power failure, encrypted recipient missing key, duplicate-delivery prevention.

**Interception:** eligible signal, no hardware, signal too weak, encrypted capture, decode, deterministic failure/partial result.

**Jamming:** capability requirement, power requirement, range, start/stop, detection event, stacking policy.

**Persistence:** old save, active network, queued decode, active jammer, message ledger.


---

## 72. Golden Scenario Fixtures

Create fixed scenarios:
1. shelter with no antenna;
2. baseline receiver/basic antenna;
3. directional link to nearby outpost;
4. parabolic long-range ally link;
5. phased-array multi-network setup;
6. weather-degraded reception;
7. brownout outage;
8. faction message intercepted but encrypted;
9. successful decode and espionage handoff;
10. false/failed decode path if supported;
11. player distress transmission delivered;
12. encrypted ally network;
13. jamming blocks one channel;
14. relay restores distant connectivity;
15. old-save compatibility.


---

## 73. Property and Fuzz Tests

Properties: signal quality stays in legal bands; identical topology/state produces identical reachability; network members are unique; offline stations cannot relay; message IDs are unique; one transmission delivers at most once per recipient; no decode job references missing message/survivor; power demand is non-negative; jamming cannot improve enemy signal; old saves never produce invalid references.


---

## 74. Observability

Structured logs:
```text
AntennaInstalled instance=<id> type=<id> station=<id>
NetworkEstablished network=<id> members=<n>
CommunicationsLinkChanged from=<station> to=<station> state=<band>
TransmissionSent id=<id> type=<id>
TransmissionDelivered id=<id> station=<id>
InterceptCaptured id=<id> source=<station-or-unknown>
InterceptDecoded id=<id>
JammingChanged target=<id> active=<bool>
```
Avoid logging hidden decoded content in ordinary diagnostics if it would spoil gameplay.


---

## 75. Failure Handling

- Missing antenna definition: fail data validation, not silent fallback.
- Missing power sink/source: infrastructure remains offline with diagnostic.
- Missing station: block network creation or mark migrated orphan explicitly.
- Message payload missing: preserve metadata and surface invalid-content diagnostic.
- Encryption profile missing on old save: migrate through compatibility map.
- Downstream intent consumer absent: transmission may still be delivered as narrative only only if definition explicitly declares no gameplay effect; otherwise fail catalog validation.


---

## 76. Security and Data Hygiene

Catalogs are declarative. No dynamic code, reflection paths, shell commands, arbitrary file access, or executable expressions in antenna/network/message definitions. Rich text from intercepted content should use the project’s normal sanitization/localization pipeline.


---

## 77. Implementation Phase A — Audit and Contracts

Tasks: audit radio/power/shelter/topology/save/RNG; document baseline receiver behavior; create station/antenna/network/message/state DTOs; create catalog loaders/validators; add schema versioning and empty migration.

Exit: Core types compile and catalogs load headlessly with no behavior changes.


---

## 78. Implementation Phase B — Antennas and Power

Tasks: installation adapter, instance state, powered/offline state, condition, signal capability, basic link score, baseline receiver compatibility, tests.

Exit: physical infrastructure deterministically gates reception/transmission capability.


---

## 79. Implementation Phase C — Network Topology

Tasks: station model, member management, direct links, optional relay, channel capacity, encryption compatibility, network status derivation, save round-trip.

Exit: shelter/outpost/allied stations can form persistent networks.


---

## 80. Implementation Phase D — Transmission

Tasks: typed transmission request, validation, reachability, delivery ledger, faction/quest/distress intents, idempotent events, UI action projection.

Exit: the player can send messages whose delivery is a real Core event.


---

## 81. Implementation Phase E — Interception and Decode

Tasks: intercept opportunities, intercepted-message DTO, encryption metadata, decode job, survivor assignment, deterministic outcome, Plan 153 handoff, journal and tests.

Exit: faction communications can become persistent, decodable strategic artifacts.


---

## 82. Implementation Phase F — Jamming and Interference

Tasks: jamming capability, power demand, target scope, effect registry, detection event, weather interference integration, countermeasure hooks, tests.

Exit: electronic disruption changes actual communications reachability without creating a second military simulator.


---

## 83. Implementation Phase G — UI and Guidance

Tasks: infrastructure/network/intercept/transmit projections, status labels, risk/capability tooltips, first-use guidance, keyboard/controller support, text scaling, transcripts/captions, reduced motion.

Exit: UI is a renderer/controller over Core state.


---

## 84. Implementation Phase H — Catalog Expansion

Tasks: author meaningful antenna variants toward ten, meaningful network templates toward fifteen, generate reachability/consumer coverage report, reject dead definitions, add localization and art/icon references.

Exit: requested content volume is reached only with mechanically distinct supported entries.


---

## 85. Implementation Phase I — CI Hardening

Tasks: `--communications-selftest`, data-integrity extension, golden fixtures, fuzz/property tests, migration tests, performance sanity, documentation, full repository regression.

Exit: headless CI proves infrastructure, network, intercept, transmission, jamming, power, and save behavior.


---

## 86. Communications Coverage Report

Generate `docs/communications/COMMUNICATIONS_COVERAGE.md` including antenna families, supported bands, powered capabilities, network templates, station types, transmission intents, interception consumers, encryption profiles, jamming effects, fixtures, and any definitions with no real consumer.


---

## 87. Risk Register

**Busywork risk:** too many frequencies/settings. Mitigate with stable channel presets, sensible defaults, and network templates.

**Radio-content regression:** infrastructure gating hides existing critical broadcasts. Mitigate with baseline receiver compatibility and critical-content policy.

**Fake infrastructure:** antennas only change UI numbers. Mitigate by requiring real radio/intercept/network consumers.

**Power dominance:** communications becomes impossible under normal grid pressure. Tune idle draw modestly; reserve high draw for transmit/jam/advanced arrays.

**Topology complexity:** relay graph becomes hard to understand. Start with direct/star links and visible reachability.

**Cross-system duplication:** intercepted intelligence duplicates Plan 153. Keep physical capture in communications and intelligence interpretation in espionage.


---

## 88. Definition of Done — Flagship

- [ ] `CommunicationsSystem.cs` exists with schema-versioned capture/restore.
- [ ] antennas are catalog-backed instances, not literals.
- [ ] basic/directional/parabolic/phased-array families work.
- [ ] ten antenna definitions only if mechanically meaningful.
- [ ] station/network model exists.
- [ ] network membership and reachability work.
- [ ] reception is infrastructure-aware.
- [ ] player transmission works and is idempotent.
- [ ] intercepted messages persist.
- [ ] decode workflow works and round-trips.
- [ ] encryption/key compatibility works.
- [ ] jamming has real effects and power cost.
- [ ] weather/power/damage affect communications deterministically.
- [ ] FactionRadioEngine, RadioTuner, Verdict radio compatibility preserved.
- [ ] Plan 153 handoff is typed and non-duplicative.
- [ ] outpost/expedition hooks exist where supported.
- [ ] old saves load safely.
- [ ] panel is projection-only.
- [ ] data-integrity gate passes.
- [ ] `--communications-selftest` passes.


---

## 89. Follow-On Task 157-A — Communications Warfare

Build advanced electronic warfare only after base jamming works: direction finding, frequency hopping, hardened links, jammer localization, counter-jamming, deception beacons, and military-operation modifiers. Reuse the same link/effect contracts rather than creating a second EW simulator.


---

## 90. Follow-On Task 157-B — Satellite Communications

Late-game satellite/long-haul communications can add extreme-range station types, orbital-pass windows if desired, specialized power/research prerequisites, and unique interception opportunities. Keep it as an advanced station/antenna family layered on the same infrastructure authority.


---

## 91. Follow-On Task 157-C — Propaganda Broadcasting

Add authored mass-broadcast campaigns with faction/moral consumers. Communications owns delivery/reach; faction systems own opinion/standing effects; moral system owns ethical consequences. Avoid generic “propaganda power” stats without consumers.


---

## 92. Follow-On Task 157-D — Communications Legacy / Epilogue

Expose notable communication facts to Plan 145: first long-range alliance link, famous distress broadcast, decisive intercept, communications blackout, major jammer operation. Ending prose remains outside communications.


---

## 93. Follow-On Task 157-E — Network Operations and Maintenance

If playtests show strategic value, add scheduled maintenance, spare parts, technician assignment, relay redundancy, and graceful degradation. Do not add maintenance chores unless failures create understandable decisions rather than repetitive clicks.


---

## 94. Immediate Next Substeps

1. Verify whether `radio_frequencies.json` exists and is canonical.
2. Audit current radio behavior when no explicit hardware exists.
3. Decide baseline-receiver compatibility policy.
4. Identify canonical shelter power API.
5. Identify station/location coordinate authority.
6. Define minimal antenna/network/message DTOs.
7. Implement catalog validator before content expansion.
8. Build one basic antenna + one two-station network fixture.
9. Wire reception capability into `RadioTuner` behind compatibility test.
10. Add save/restore before transmission/interception complexity.
11. Implement one typed transmission end-to-end.
12. Implement one faction intercept end-to-end.
13. Only then add jamming/encryption breadth.


---

## 95. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj

godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --communications-selftest
```

Also run radio-specific, save, power-grid, expedition/outpost, and UI regression gates that exist in the repository.


---

## 96. Final Guardrails

- No receive-only compatibility regression.
- No raw floating-frequency save authority when stable channel IDs exist.
- No per-frame all-pairs signal simulation.
- No transmission without power/capability.
- No jamming without power and a real effect sink.
- No duplicate delivery after reload.
- No decode reroll by save/reload.
- No plaintext fallback when encryption/key compatibility fails.
- No fake interception that only unlocks prose.
- No duplicate intelligence authority beside Plan 153.
- No shelter construction cost subtraction inside communications.
- No faction-standing mutation as a side effect of mere RF delivery.
- No critical Verdict/quest radio becoming accidentally unreachable without explicit design approval.
- No ten-antenna/fifteen-network vanity padding.
- No UI-owned signal math.

When complete, radio becomes a genuine shelter infrastructure and strategic network: hardware and power determine what the shelter can hear, reach, intercept, protect, and disrupt; outposts and allies can form real links; intercepted traffic can feed the espionage/intelligence layer; and every message or outage is backed by deterministic Core state rather than decorative UI fiction.

---

## Annex A — Acceptance Scenario Matrix

### A1 — Legacy receiver compatibility
An old save created before Plan 157 loads with no communications block. Existing mandatory radio content remains available through the documented baseline receiver policy. The panel shows the migrated baseline state without fabricating advanced antennas.

### A2 — Basic powered shelter antenna
The shelter installs one basic antenna through the construction authority. With adequate power, nearby broadcasts become readable; removing power changes status to offline and tuner capability updates without deleting authored broadcasts. Restoring power restores capability deterministically.

### A3 — Directional outpost link
A directional antenna is oriented toward an outpost. Link score exceeds the reliable threshold in that direction but not to an unrelated station. Saving and reloading preserves orientation/network membership.

### A4 — Encrypted allied network
Shelter and allied station possess the same key. A targeted transmission reaches both endpoints. A third station lacking the key may detect metadata but cannot read the payload. No silent plaintext fallback occurs.

### A5 — Faction intercept
FactionRadioEngine emits an intercept-eligible secure communication. The shelter has sufficient receive capability and an available channel, creating one intercepted-message record. The ordinary broadcast corpus remains owned by the radio engine.

### A6 — Decode job
A survivor is assigned through the canonical task system. The message enters decoding, is saved halfway, restored, and completes on the same day with the same seeded result. Reopening the panel cannot reroll it.

### A7 — Espionage handoff
A decoded message emits a stable typed intelligence artifact. Plan 153 consumes it once. Communications does not create a second suspicion or intelligence-report authority.

### A8 — Jamming
A compatible powered antenna starts jamming a supported channel. Target signal quality drops through the real link/effect model. Power loss ends the jammer. A detectable-jamming event is emitted for faction/espionage consumers.

### A9 — Weather interference
A severe weather profile reduces link quality deterministically. The same campaign day/weather event produces identical quality after reload. Critical-content fallback policy is honored.

### A10 — Relay resilience
Shelter cannot directly reach a distant ally, but a powered outpost relay creates a valid path. Turning the relay off breaks connectivity and the UI reports the actual cause.

### A11 — Damaged antenna
Antenna condition falls below a configured capability threshold. Range/sensitivity degrade according to data; the instance remains the same stable installed asset. Repair through the canonical maintenance authority restores capability.

### A12 — Channel-capacity limit
An antenna supporting a bounded number of channels cannot simultaneously host more active services than its capability allows. The start action is rejected deterministically with a clear reason rather than silently oversubscribing.

---

## Annex B — Detailed Communications Integration Acceptance Matrix

This annex converts the architecture into execution-grade integration scenarios. Each scenario must be represented by either a unit fixture, a headless selftest case, or a documented manual verification where presentation is involved.

### B1. Baseline radio preservation

Given a campaign save created before the communications schema exists, restoring that campaign must preserve the previously available radio experience. The migration may materialize a built-in receiver capability or a baseline installed antenna depending on the repository's shelter model, but it must not silently require the player to construct new infrastructure before hearing critical pre-existing broadcasts. The migration decision must be documented, covered by a fixture, and treated as compatibility policy rather than an implementation accident.

Acceptance assertions:
- no communications block is tolerated;
- baseline receiver state is deterministic;
- critical broadcast reachability matches approved legacy behavior;
- no advanced transmitter/interceptor capabilities are granted accidentally;
- opening the new communications panel does not mutate the migration state.

### B2. Construction-to-runtime handoff

A basic antenna is constructed through the canonical shelter construction system. Inventory/resource subtraction, labor, build time, placement, and room/exterior-slot validation remain owned by shelter/construction. Only after construction commits does communications receive the installed antenna identity. If construction fails or is cancelled, no antenna instance exists in `CommunicationsState`.

This scenario proves that communications infrastructure is integrated rather than becoming a second building system.

### B3. Power-grid dependency

A powered basic antenna provides its configured reception capability. When the electrical grid sheds that circuit, communications recomputes the antenna as offline or degraded according to the approved power profile. No radio link should continue pretending that power exists. When power returns, the antenna resumes capability without duplicating transmissions, re-decoding messages, or recreating network membership.

### B4. Directional antenna orientation

A directional antenna linked toward Outpost A should receive its directional gain only for the configured target/bearing/sector. Outpost B at similar distance but outside the configured direction should not receive the same bonus. The orientation must round-trip through save/load and must be validated if the target outpost disappears.

### B5. Multi-antenna station arbitration

A shelter with several antennas should expose a deterministic best-supported path for each communications action. Reception can choose the best eligible receiver; transmission can select a compatible transmitter; interception can reserve a channel. Selection order must never depend on collection insertion/hash ordering. Tie-break using explicit priority, capability, then stable antenna instance ID.

### B6. Direct network link

Create a two-station secure network between shelter and a nearby allied settlement. Both stations have compatible antennas, channel support, power, and encryption keys. The network reports active. Removing one endpoint's power changes the network reason to endpoint-offline rather than deleting the network. Restoring power reactivates it.

### B7. Relay network link

A distant station is unreachable directly but reachable through one powered relay outpost. The system computes a valid route through supported topology. Turning off or destroying the relay breaks the path. This must not require full dynamic routing: a bounded deterministic relay graph is sufficient.

### B8. Encrypted network key mismatch

Two stations share a network definition but one lacks the active key. The system must expose `key_missing` or equivalent and prevent readable encrypted delivery. It must not silently transmit plaintext. Metadata reception may still occur if the design supports it.

### B9. Key rotation safety

If key rotation is implemented later, active networks must transition atomically: old key validity, new key distribution, and member mismatch must be explicit. For the initial implementation, do not ship key rotation unless the state machine is tested. This acceptance case may remain a deferred guardrail recorded in coverage.

### B10. Faction broadcast reception

`FactionRadioEngine` emits an authored broadcast candidate. Communications determines whether the current station can receive the signal. Radio content remains unchanged. The same broadcast, antenna topology, weather, and power state yields the same reception quality after reload.

### B11. Critical broadcast fallback

A story-critical Verdict or campaign-ending broadcast must follow the approved critical-content policy. If infrastructure requirements can make it unavailable, a recovery path must exist—repeat window, alternative channel, journal transcript, mandatory baseline receiver, or other explicit design. Never let an infrastructure feature silently soft-lock critical progression.

### B12. Player distress transmission

The player sends a distress transmission using a compatible powered transmitter. Communications validates the channel, power, capability, range, and cooldown; creates a stable transmission ID; computes reachable recipients; and emits delivery events once. Quest/faction/expedition systems decide what any recipient does with the message.

### B13. Trade-offer transmission

A trade offer delivered to a faction is not itself a trade transaction. Communications only proves delivery. The economy/faction system evaluates whether the faction responds and creates any trade session. This protects transaction authority.

### B14. Propaganda transmission

A propaganda message can be transmitted if the template and recipient conditions are satisfied. Communications reports delivered recipient set and detectable source metadata. Any standing, morale, suspicion, or political consequence belongs to the faction/moral/espionage systems.

### B15. Interception without decoding

A secure faction communication occurs within receive range. The player has interception capability. Communications creates an intercepted-message record containing metadata and encrypted payload reference, but the player does not automatically learn the hidden intelligence. This proves physical interception is distinct from interpretation.

### B16. Deterministic decoding

A survivor is assigned to decode a captured message. Start day, completion day, decoder ID, input message ID, encryption profile, compute/power requirement, and seeded outcome are persisted. Saving and loading halfway produces the same completion/result. Restarting or reopening UI cannot reroll the outcome.

### B17. Decoding interrupted by power loss

If decoding requires computing power, a power outage pauses progress rather than silently completing or resetting the job. Resume semantics must be deterministic. If the repository's task system uses accumulated work units instead of days, use that authority rather than inventing a separate timer.

### B18. Decoded intelligence handoff

When decoding completes, communications emits a typed artifact once. If Plan 153 exists, espionage/intelligence consumes that artifact. Communications must not also create duplicate faction suspicion, strategic buffs, or research unlocks unless explicitly delegated through other canonical bridges.

### B19. Jamming activation

A jamming-capable antenna with sufficient power starts a jammer on a supported target channel. The active jammer consumes power and creates a typed impairment. Signal/link calculation incorporates the impairment. Starting the jammer twice does not stack unless the stacking policy explicitly permits it.

### B20. Jamming attribution

Because jamming is active emission, it can produce a detection/attribution event. Communications reports the incident; faction/espionage systems decide diplomatic or covert consequences. The jamming system itself must not write faction hostility directly.

### B21. Atmospheric interference

A deterministic weather/interference profile lowers specific bands or general communications quality. The same weather event ID/day produces the same result after save/load. The system must not use random per-frame static to decide reachability.

### B22. Damaged antenna degradation

Reducing antenna condition changes capabilities according to the catalog profile. Repair is performed through the canonical maintenance/durability system. Communications consumes the new condition and recomputes links. It does not invent repair materials or labor.

### B23. Channel-capacity exhaustion

If all supported channels/resources on an antenna are reserved by active communications services, a new operation is rejected or queued according to explicit policy. The result includes a stable failure reason. No hidden over-allocation.

### B24. Network member removal

When an outpost is abandoned/destroyed, communications removes or marks its station unavailable through a topology event. Existing network definitions remain interpretable; orphan links do not crash save restore. A migration/cleanup function handles stale member IDs deterministically.

### B25. Maximum infrastructure scale

Construct a stress fixture with the maximum expected number of shelter antennas, outpost relays, network members, intercept records, and active channels. Day tick and topology recomputation remain within the project's CI/performance budget and allocate predictably.

---

## Annex C — Communications Data and Validation Schema Guidance

The exact JSON schema should follow the repository serializer, but every definition family needs stable IDs and explicit references.

### Antenna definition requirements

Each antenna definition must specify identity, display/localization keys, supported bands/channels, receive sensitivity, transmission capability, channel capacity, range/link-budget parameters, power requirements, weather tolerance, and optional jamming/interception capabilities. Build/recipe/installable references must point to canonical catalogs if construction is enabled.

### Network template requirements

A network template should describe a useful starting configuration: intended station roles, supported channel/frequency identity, encryption profile, minimum required capabilities, UI description, and optional doctrine/purpose tags. Templates must not pre-own runtime members that do not exist in the current campaign.

### Encryption profile requirements

Each encryption profile defines whether a key is required, decode difficulty, compute/time requirements, metadata visibility, and whether brute-force decoding is allowed. Some profiles may explicitly require a stolen/shared key and be non-bruteforceable. This is more meaningful than a raw “encryption level 100.”

### Interference profile requirements

Profiles map weather, damage, and electronic effects to link-score penalties by band/capability. They must be deterministic and bounded. Stacking rules must be explicit.

### Transmission template requirements

Player-selectable transmissions require stable intent IDs, localization keys, allowed target kinds, channel/security requirements, cooldown policy, and a registered downstream consumer when a gameplay consequence is promised. Decorative/log-only transmissions must be marked as such.

### Validator output

The validator should produce actionable diagnostics including file, definition ID, broken field, referenced ID, and expected catalog/type. Do not emit only generic “communications data invalid.”

---

## Annex D — Release and Regression Checklist

Before merge/release:

1. Run Core build and all tests.
2. Run data-integrity selftest.
3. Run communications selftest.
4. Run radio tuner/faction radio regression tests.
5. Run Verdict/critical-radio regression tests.
6. Run shelter construction tests.
7. Run power-grid/brownout tests.
8. Run save migration and mid-decode round trips.
9. Run expedition/outpost topology tests where integrated.
10. Run Plan 153 espionage handoff tests where available.
11. Verify panel keyboard/controller navigation.
12. Verify text scaling and captions/transcripts.
13. Generate communications coverage report.
14. Verify no antenna/network template has zero gameplay consumers.
15. Verify no critical radio content is newly unreachable without approved policy.
16. Verify old saves with no communications block boot and remain playable.
17. Verify transmission delivery is idempotent.
18. Verify decode result is seed-stable.
19. Verify jamming cannot run without power.
20. Verify no raw UI signal math duplicates Core calculations.

The release close-out should record catalog revision, communications schema version, baseline-receiver compatibility decision, number of antenna definitions, number of network templates, number of supported transmission intents, and all active cross-system sinks.
