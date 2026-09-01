# Plan 157 — Communications & Radio Network Infrastructure

## Goal

Create a communications infrastructure system where players build antennas, establish radio networks, intercept faction communications, and broadcast their own messages. Currently radio systems (`FactionRadioEngine.cs`, `RadioTuner.cs`) provide content (broadcasts, distress signals) but there is no infrastructure layer — no antenna construction, no network building, no signal interception, no transmission capability. This plan adds a communications infrastructure that makes radio a two-way tool rather than just a receiver.

## Why

**Repository evidence:** `FactionRadioEngine.cs` handles deterministic faction broadcasts with frequency tuning and callsigns. `RadioTuner.cs` provides tuner abstraction. `VerdictRadioSystem.cs` handles Verdict-specific radio. But all radio is receive-only — players listen to broadcasts but cannot transmit, intercept, or build communications infrastructure. Plan 24 (radio signals/airwaves) and Plan 73 (faction radio corpus) add content but not infrastructure.

**What is missing:** Players cannot build antennas to improve reception. They cannot establish communication networks with other settlements. They cannot intercept faction communications. They cannot broadcast their own messages. Radio is a passive experience — content plays, player listens.

**Why existing plans don't solve it:** Plan 24 (radio signals) adds broadcast content but not infrastructure. Plan 50/107 (radio distress signals) adds distress content but not player transmission. Plan 73 (faction radio corpus) adds faction broadcasts but not interception. Plan 153 (espionage) adds intelligence gathering but not communications infrastructure. No plan addresses antenna construction, network building, or two-way communications.

**Player value:** Creates strategic infrastructure (build antennas, establish networks), adds active radio gameplay (intercept, transmit, jam), provides intelligence opportunities (intercept faction comms), and makes communications a meaningful shelter system rather than background noise.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Radio/FactionRadioEngine.cs` — faction broadcasts
- `Assets/Ashfall.Core/Radio/RadioTuner.cs` — tuner abstraction
- `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` — Verdict radio
- `Assets/Ashfall.Core/Shelter/` — shelter systems
- `Assets/StreamingAssets/Data/radio_frequencies.json` (VERIFY) — frequency definitions
- NEW: `Assets/Ashfall.Core/Communications/CommunicationsSystem.cs`
- NEW: `Assets/StreamingAssets/Data/communications_networks.json`

## Main Task 1 — Foundation / System Contract

1. Create `CommunicationsSystem.cs` in `Assets/Ashfall.Core/Communications/`
2. Define `Antenna` DTO: `antennaId`, `antennaType` (basic/directional/parabolic/phased_array), `range` (km), `sensitivity` (0-100), `bandwidth` (channels), `condition` (0-100), `locationId`, `powerRequirement`
3. Define `CommunicationsNetwork` DTO: `networkId`, `name`, `frequency` (MHz), `encryptionLevel` (0-100), `members` (list of station IDs), `range`, `status` (active/jammed/offline)
4. Define `InterceptedMessage` DTO: `messageId`, `sourceFactionId`, `frequency`, `encryptionLevel`, `content` (string), `interceptDay`, `decoded` bool, `intelligenceValue` (0-100)
5. Define `CommunicationsState` DTO: list of antennas, list of networks, list of intercepted messages, communications capacity, signal strength
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define antenna types:
   - **Basic antenna**: short range (5km), low sensitivity, 1 channel
   - **Directional antenna**: medium range (20km), focused sensitivity, 3 channels
   - **Parabolic antenna**: long range (50km), high sensitivity, 5 channels
   - **Phased array**: very long range (100km), adaptive sensitivity, 10 channels
8. Define communications capabilities:
   - **Reception**: receive broadcasts on available frequencies
   - **Interception**: intercept encrypted faction communications
   - **Transmission**: broadcast messages on owned frequencies
   - **Jamming**: jam enemy frequencies (reduce their communication)
   - **Encryption**: encrypt own communications (prevent interception)
9. Define network mechanics:
   - Networks connect multiple stations (shelter, outposts, allies)
   - Networks have frequencies and encryption
   - Networks can be jammed by enemies
   - Networks provide communication between members
   - Network range depends on antenna capabilities
10. Define interception mechanics:
    - Intercepted messages are encrypted (require decoding)
    - Decoding requires skill, time, and computing power
    - Decoded messages provide intelligence (faction plans, movements)
    - Failed decoding: message lost or corrupted
    - Detection risk: faction may detect interception attempt
11. Add deterministic seeding: communications outcomes use `ISeededRng`
12. Wire into `GameBootstrap`: `SetupCommunications`, `TickCommunications`, `SaveCommunications`
13. Create `AntennaCatalogLoader` for antenna definitions
14. Create `CommunicationsNetworkCatalogLoader` for network definitions
15. Create UI hook: communications panel showing antennas, networks, messages

## Main Task 2 — Implementation / Antennas / Networks / Interception / Transmission

1. Implement antenna construction:
   - Player builds antenna (requires resources, labor, location)
   - Antenna provides reception/transmission capabilities
   - Antenna condition degrades with weather/use
   - Antenna requires power to operate
   - Multiple antennas can be networked
2. Implement network establishment:
   - Player establishes communication network
   - Network connects shelter to outposts/allies
   - Network has frequency and encryption settings
   - Network members can communicate
   - Network range limited by antenna capabilities
3. Implement message interception:
   - Antenna intercepts faction communications
   - Intercepted messages are encrypted
   - Player assigns survivor to decode (skill check)
   - Decoded messages reveal faction intelligence
   - Failed decode: message lost
   - Detection risk: faction may notice interception
4. Implement message transmission:
   - Player broadcasts messages on owned frequencies
   - Messages can be: alerts, trade offers, propaganda, distress calls
   - Transmission range depends on antenna
   - Encrypted transmissions prevent interception
   - Broadcasts can be received by network members
5. Implement jamming:
   - Player can jam enemy frequencies
   - Jamming reduces enemy communication effectiveness
   - Jamming requires power and antenna capability
   - Enemy can counter-jam or relocate frequency
   - Jamming is detectable (enemy knows you're jamming)
6. Implement encryption:
   - Player encrypts own communications
   - Encryption level prevents enemy interception
   - Higher encryption requires more computing power
   - Enemy can attempt to break encryption
   - Encryption keys must be shared with network members
7. Implement communications events:
   - "The Signal" — intercept mysterious encrypted message
   - "The Broadcast" — transmit important message to allies
   - "The Jamming" — enemy jams your communications
   - "The Decode" — successfully decrypt enemy intelligence
   - "The Network" — establish new communication network
   - "The Antenna" — build advanced antenna array
   - "The Interference" — atmospheric interference disrupts comms
8. Add communications quest hooks:
   - "The Spy" — intercept enemy communications network
   - "The Messenger" — establish communication with isolated settlement
   - "The Jammer" — counter enemy jamming operations
   - "The Codebreaker" — decrypt heavily encrypted message
   - "The Broadcast" — transmit propaganda to enemy territory
   - "The Network" — build shelter-wide communications network
   - "The Silence" — restore communications after outage
9. Implement communications integration:
   - Communications integrate with espionage (Plan 153)
   - Intercepted messages provide intelligence
   - Networks connect outposts (Plan 155)
   - Transmissions coordinate faction operations
   - Jamming supports military operations
10. Add UI: communications panel showing antennas, networks, messages
11. Create communications journal: automatic log of intercepted/ transmitted messages
12. Implement communications tutorial: first antenna explains system
13. Add communications tooltips: hover over antenna shows capabilities
14. Create 10 antenna types and 15 network templates in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `FactionRadioEngine`: communications system intercepts faction broadcasts
2. Connect to `RadioTuner`: tuner integrates with antenna capabilities
3. Integrate with `VerdictRadioSystem`: Verdict radio uses communications infrastructure
4. Connect to `EspionageSystem` (Plan 153): intercepted messages provide intelligence
5. Wire into `ShelterExpansionSystem` (Plan 156): antennas installed in shelter rooms
6. Connect to `ExpeditionSystem`: outposts connect to communications network
7. Implement old-save compatibility: existing saves get empty communications state
8. Add deterministic seeding: communications use `ISeededRng`
9. Create exploit prevention: interception has detection risk, jamming requires power
10. Add tests: antenna construction, interception, transmission, jamming, save round-trip
11. Verify catalog integrity: all antenna/network IDs resolve
12. Test edge cases: no antennas (no communications), max antennas (full coverage)
13. Verify headless behavior: communications process correctly without UI
14. Add data-integrity-selftest: antenna/network definitions validate against catalogs
15. Create `--communications-selftest` verb for CI validation

## State / System Interaction Model

```text
Communications infrastructure
├─ Antenna construction
│  ├─ Build antenna (resources, labor, location)
│  ├─ Antenna provides capabilities (range, sensitivity, channels)
│  ├─ Antenna requires power
│  └─ Antenna condition degrades
├─ Network establishment
│  ├─ Create network (frequency, encryption)
│  ├─ Add members (stations, outposts, allies)
│  ├─ Network provides communication
│  └─ Network can be jammed
├─ Message interception
│  ├─ Antenna intercepts faction comms
│  ├─ Messages encrypted (require decoding)
│  ├─ Decode with skill/time
│  ├─ Decoded = intelligence
│  └─ Detection risk (faction notices)
├─ Message transmission
│  ├─ Broadcast on owned frequency
│  ├─ Alerts, trade, propaganda, distress
│  ├─ Range depends on antenna
│  └─ Encryption prevents interception
├─ Jamming
│  ├─ Jam enemy frequencies
│  ├─ Reduces enemy comms
│  ├─ Requires power/antenna
│  └─ Detectable by enemy
└─ Integration
   ├─ Espionage: intercepted messages = intelligence
   ├─ Outposts: network connects colonies
   ├─ Factions: transmissions coordinate operations
   └─ Military: jamming supports operations
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --communications-selftest
```

## Risk

**MEDIUM** — Communications complexity can overwhelm players if too many frequencies, networks, and encryption levels exist. Risk of communications feeling like busywork rather than strategic tool. Mitigation: start with basic antenna, unlock advanced capabilities gradually, make interception optional (can play without it), and provide clear UI showing communications status.

## Definition of Done

- `CommunicationsSystem.cs` exists with full `CaptureState/RestoreState`
- 4 antenna types implemented (basic, directional, parabolic, phased array)
- Network establishment and management functional
- Message interception and decoding working
- Message transmission capabilities
- Jamming and encryption mechanics
- Communications events and quest hooks
- Save/load round-trip tested
- Deterministic communications outcomes verified
- Old saves load without error
- 10 antenna types + 15 network templates in data authority
- UI panel shows communications infrastructure
- Cross-system integration (radio, espionage, shelter, expeditions, factions)

## Follow-On Opportunities

- Satellite communications (late-game advanced tech)
- Communications warfare (electronic warfare specialization)
- Propaganda broadcasts (psychological warfare)
- Communications legacy (famous broadcasts remembered in epilogue)
- Communications quests (intercept enemy codes, build network)
