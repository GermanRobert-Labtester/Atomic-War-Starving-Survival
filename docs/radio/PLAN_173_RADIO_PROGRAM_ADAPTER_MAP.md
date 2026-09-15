# Plan 173 — Radio program production adapter map

**Status:** SEALED — map ACCEPTED + Phases 1–3 implemented 2026-09-12.  
**Package:** `DEBT-173-RADIO-PROGRAM-*` (ADAPTER / PRODUCTION / HOST / PANEL all RETIRED)  
**Batches:** `BATCH-2026-09-12-DEBT-173-RADIO-ADAPTER` + `…-PRODUCTION` + `…-HOST` + `…-PANEL` (closed)  
**Rebase source:** `RADIO-PROGRAM-ADAPTER-MAP` in `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`  
**Date:** 2026-09-12  
**Sign-off:** Approve all five §4 defaults (`RadioProgramSlot.SlotId`; `ScheduledBroadcastResult` delivery; `PsyOps.StartCampaign`; dedicated production save; no second schedule/receiver/corpus/network).  
**Historical proposal (non-authority):** `Next-steps-plans/Plan_173_Radio_Station_Content_Creation.md`

---

## 1. Premise (current evidence)

Player-authored shelter radio **programs** do not exist. There is no
`RadioProgramProductionSystem`, no `radio_programs.json`, and no save section
for program-production history.

Adjacent live authorities already cover airwaves, listening, and influence:

| Concern | Current owner | Path / data | Save section |
|---|---|---|---|
| Station identity + authored schedule slots | `RadioStationCatalog` + `radio_stations.json` | `Radio/RadioStationCatalog.cs`, `RadioProgramSlot` | station state overrides via radio save path |
| Schedule resolution / airtime evaluation | `RadioScheduleCoordinator` | `Radio/RadioScheduleCoordinator.cs` → `ScheduledBroadcastResult` | none owned by coordinator (stateless + transient alerts) |
| Appointment / cadence bulletins | `AppointmentProgramDefinition` list inside coordinator | hardcoded registration in coordinator today | not a player-production store |
| Listening composition | `RadioHostSession` | `src/Host/RadioHostSession.cs` | `radio` |
| Intercept reception / tune / decrypt | `ShelterRadioStationSystem` | `Radio/ShelterRadioStationSystem.cs`, `radio_intercepts.json` | `radio_station` |
| Faction corpus listening | `FactionRadioEngine` | composed by `RadioHostSession` | under radio composition |
| Outbound propaganda campaigns + jamming + pressure | `PsyOpsSystem` | `Radio/PsyOpsSystem.cs`, `propaganda_campaigns.json` | `psyops` |
| Record / log of heard broadcasts | `RadioRecordingSystem` / `RadioSignalLog` | consume `ScheduledBroadcastResult` | via radio composition |
| Printing-press leaflet UI | `UndergroundPrintingPressPanel` | prototype, unbound | **not** a consequence authority (`docs/content/PLAN156_*`) |

Historical Plan 173 assumed Plan 24 schedule, Plan 157 reach/jamming, and Plan 168
propaganda. Current source maps those intents to **station schedule +
`RadioScheduleCoordinator`**, **`PsyOpsSystem` transmitter/jamming/reach**, and
**`PsyOpsSystem.StartCampaign` / loyalty delegate** — not to new parallel systems.

---

## 2. Required adapter trio (rebase acceptance)

The map must name exactly one of each:

### 2.1 Schedule-slot reference (proposed)

**`RadioProgramSlot.SlotId`** on a station definition from `radio_stations.json`,
resolved through `RadioStationCatalog.GetCurrentSlot` /
`RadioHostSession.GetCurrentSlot`.

- Stable authored id a player program may bind to (airtime window +
  `broadcast_pool_id` / `program_type` stay schedule-owned).
- **Not** `AppointmentProgramDefinition.ProgramId`: those are coordinator-owned
  world bulletins (`prog_morning_weather`, …), not player production targets.
- A future production system stores only the **slot id string** (+ station id);
  it never copies frequency tables or rebuilds `Resolve`.

### 2.2 Delivery / reception fact (proposed)

**`ScheduledBroadcastResult`** produced by `RadioScheduleCoordinator.Resolve`
(fields: `HasTransmission`, `IsJammed`, `IsSilence`, `StationId`, `BroadcastId`,
`Priority`).

- This is the existing fact that a frequency/day evaluation did or did not
  deliver airtime (including jammed / dead-air outcomes).
- Recording and signal-log already consume this type
  (`RadioRecordingSystem.RecordBroadcast`, `RadioSignalLog.LogIntercept`).
- **Outbound reach gate** for influence stays on `PsyOpsSystem.TransmitterReady`
  and `EffectiveReach` — production may *read* those predicates; it must not
  fork jamming or invent a second receiver.
- `ShelterRadioStationSystem.ScanFrequency` → `RadioScanResult` remains the
  **intercept-listening** owner; player program production does not take it over.

### 2.3 Propaganda consequence input (proposed)

**`PsyOpsSystem.StartCampaign(string campaignId, int day)`** against ids in
`propaganda_campaigns.json`.

- Sole typed input for faction-targeted broadcast influence today.
- Daily pressure / loyalty shifts stay inside PsyOps (`TickCampaigns`,
  `LoyaltyShiftRequested`); production must not reimplement pressure math.
- Counter-propaganda / jamming remain PsyOps APIs (`StartCounterPropaganda`,
  `StartJamming`) — callable from espionage/host, not duplicated.
- `UndergroundPrintingPressPanel` is explicitly out of authority.

---

## 3. Ownership table (signed)

| Concern | Authority | Boundary |
|---|---|---|
| Station ids, frequencies, authored slots | `RadioStationCatalog` / `radio_stations.json` | Production stores slot/station **references only** |
| Airtime evaluation | `RadioScheduleCoordinator.Resolve` → `ScheduledBroadcastResult` | Production does not own Resolve or dynamic alert injection for world events |
| Intercept tune/decrypt | `ShelterRadioStationSystem` | Listen-side only; not program production |
| Faction corpus | `FactionRadioEngine` | Passive-only corpus; not player templates |
| Propaganda campaigns / pressure / jamming | `PsyOpsSystem` + `propaganda_campaigns.json` | Production may start a catalog campaign id after a successful delivery gate; no parallel loyalty ledger |
| Player program templates + prep jobs + history | **Future `RadioProgramProductionSystem`** (implement package only) | Owns templates, presenter/cost, production job state, follow-up hooks; consumes the three adapters above |
| Program template data | Future `radio_programs.json` | Templates only — **no** frequency, station list, or schedule authority |
| Program production persistence | Future dedicated save section (e.g. `radio_program_production`) | History + unresolved follow-ups only; do not merge into `radio` / `radio_station` / `psyops` |
| Host listening UI | `RadioHostSession` / `RadioPanel` | May surface production outcomes later; must not become gameplay authority |

---

## 4. Signed defaults

1. **Schedule bind target:** `RadioProgramSlot.SlotId` (+ `station_id`); not appointment `ProgramId`.  
2. **Delivery fact:** consume `ScheduledBroadcastResult` from existing `Resolve` (and PsyOps `TransmitterReady` / reach only as outbound gate reads).  
3. **Propaganda input:** `PsyOpsSystem.StartCampaign` + catalog campaign ids only.  
4. **Production save scope:** new section for program history/follow-ups only; never take over frequencies, stations, intercepts, or psyops pressure.  
5. **Non-goals for any follow-on implement:** second schedule, second receiver, second faction corpus, second radio network, unbound printing-press morale theater, unmapped morale/reputation writes outside PsyOps/existing relation owners.

---

## 5. Contract sketch (implement package only after sign-off)

Not authorized by this map alone.

1. **Data:** `radio_programs.json` — template id, display, required equipment/skills,
   prep cost, bound `station_id` + `slot_id`, optional `psyops_campaign_id`.  
2. **Core:** production system starts/cancels prep → marks delivered when host
   confirms airtime (`ScheduledBroadcastResult.HasTransmission` and not jammed)
   and optional `StartCampaign` when template names a campaign.  
3. **Audience/morale:** only through existing PsyOps loyalty delegate / documented
   host consumers — no local reputation counter.  
4. **Save:** `CaptureState`/`RestoreState` for production jobs + history only.  
5. **Tests:** cancelled prep, missing equipment, unknown slot id blocked,
   deterministic campaign start idempotency, save round-trip, assert no new
   schedule owner.

---

## 6. Exact paths (this map package)

| Path | Role |
|---|---|
| `docs/radio/PLAN_173_RADIO_PROGRAM_ADAPTER_MAP.md` | This contract |
| `KNOWN_DEBT.md` | `DEBT-173-RADIO-PROGRAM-ADAPTER-MAP` |
| `INTEGRATION_PLANS.md` | Active batch row |
| `WORKTREE_OWNERSHIP.md` | `claim-debt-173-radio-adapter-2026-09-12` |

**Read-only evidence (not claimed for edit):**

- `Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs`
- `Assets/Ashfall.Core/Radio/RadioBroadcastModels.cs` (`RadioProgramSlot`, `AppointmentProgramDefinition`, `ScheduledBroadcastResult`)
- `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`
- `Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs`
- `Assets/Ashfall.Core/Radio/PsyOpsSystem.cs` / `PsyOpsCatalog.cs`
- `Assets/StreamingAssets/Data/radio_stations.json`
- `Assets/StreamingAssets/Data/propaganda_campaigns.json`
- `src/Host/RadioHostSession.cs` / `src/Host/PsyOpsHostSession.cs`
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (`radio`, `radio_station`, `psyops`)
- `docs/radio/RADIO_STATION_AUTHORITY_AUDIT.md`
- `docs/foreman/PLAN_170_199_FORENSIC_AUDIT.md`
- `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`

---

## 7. Non-goals (this package and immediate follow-on)

- Creating a second `BroadcastSchedule`, station catalog, or frequency plan.  
- Taking over `ShelterRadioStationSystem` intercept reception.  
- Extending `FactionRadioEngine` corpus as player programs.  
- Replacing or forking `PsyOpsSystem` pressure/jamming math.  
- Wiring `UndergroundPrintingPressPanel` as radio consequence authority.  
- Implementing `RadioProgramProductionSystem` before §4 sign-off.

---

## 8. Acceptance for this map package

- [x] Gap documented: no program production system/data/save.  
- [x] One schedule-slot reference named.  
- [x] One delivery/reception fact named.  
- [x] One propaganda consequence input named.  
- [x] Program-production save scope bounded.  
- [x] **User/foreman signed §4 defaults (2026-09-12).**  
- [x] Map ACCEPTED; implement remains a separate claimed package (`DEBT-173-RADIO-PROGRAM-PRODUCTION`).

---

## 9. Phase 1 implement result

**Sealed 2026-09-12** (`DEBT-173-RADIO-PROGRAM-PRODUCTION`).

| Deliverable | Evidence |
|---|---|
| Templates | `Assets/StreamingAssets/Data/radio_programs.json` + `RadioProgramCatalog` |
| Production owner | `RadioProgramProductionSystem` — prep / cancel / tick / deliver / follow-ups |
| Slot bind | Validates `station_id` + `slot_id` against `RadioStationCatalog.Schedule` |
| Delivery gate | `TryDeliver` requires `ScheduledBroadcastResult.HasTransmission`, not jammed/silent, matching `StationId` |
| Propaganda | Optional `StartPropagandaCampaign` delegate → `PsyOpsSystem.StartCampaign` |
| Persistence | `CaptureState` / `RestoreState` on production DTO (host save section deferred) |

**Verify:** `Plan173RadioProgramProductionTests` 7/7.

## 10. Phase 2 host/save result

**Sealed 2026-09-12** (`DEBT-173-RADIO-PROGRAM-HOST`).

| Deliverable | Evidence |
|---|---|
| Host session | `src/Host/RadioProgramProductionHostSession.cs` |
| Save store | `src/Host/RadioProgramProductionSaveStore.cs` → `radio_program_production_save.json` |
| Registry | `SaveSectionRegistry` section `radio_program_production` + `SectionFileNames` |
| Main wire | `src/Main.RadioProgramProduction.cs` — catalog load, PsyOps `StartCampaign`, schedule `Resolve` delivery |
| Daily tick | `RadioProgramProductionDayOwner` (phase 4, after psyops) |
| Content claim | `ContentUtilizationScanner` maps `radio_programs.json` → `RadioProgramCatalog` / production system |

**Verify:** `Plan173RadioProgramProductionTests` 7/7; `Plan173RadioProgramProductionHostWiringTests` 1/1; `MainTriadDriftGateTests` 7/7; `CampaignEnvelopeBuilderTests` 10/10; `PersistentFilenameRegistryGateTests` 4/4; `dotnet build Ashfall.csproj` 0/0.

## 11. Phase 3 panel + equipment result

**Sealed 2026-09-12** (`DEBT-173-RADIO-PROGRAM-PANEL`).

| Deliverable | Evidence |
|---|---|
| Equipment gate | Template `required_equipment_item_ids` — possess (not consume); failure `missing_equipment` |
| Prep cost gate | Optional `prep_cost_item_id` / `prep_cost_count` consume on StartPrep; failure `missing_prep_cost` |
| Authored data | Both programs require `radio_headset`; morning bulletin also consumes `aa_batteries`×1 |
| Host inventory wire | `Main.RadioProgramProduction` → `HasRequiredEquipment` / `TryConsumePrepCost` via inventory `CountById` / `TryConsumeById` |
| RadioPanel strip | PROGRAM PRODUCTION: templates START PREP, active jobs CANCEL, LastEvent; `BindProduction` |
| Bind sites | `Main.GameFlow`, `Main.PlayerSurfaces`, `Main.UiHandlers.OpenRadioPanel`, player-panel uitest |
| Architecture map | `radio_program_production` UI cite → `RadioPanel` |

**Verify:** `Plan173RadioProgramProductionTests` (equipment + cost cases); `Plan173RadioProgramProductionHostWiringTests`; `dotnet build Ashfall.csproj`.

**Still deferred:** presenter skill tree / roster economy beyond default `presenter_shelter_desk`; dedicated CLI selftest for production.
