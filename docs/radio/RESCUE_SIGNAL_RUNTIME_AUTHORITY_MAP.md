# Rescue Signal Runtime — Authority Map (Wave 0, evidence-based)

**Package:** `RESCUE-SIGNAL-RUNTIME-TASKS-1-4` (user-authorized flagship plan, Tasks 1–4)
**Date:** 2026-09-13 · **Builder:** this session · **Status:** IMPLEMENTED — verified, ready for foreman/integrator review

This map records the **current source evidence** for the rescue-signal plan and
corrects its premises where the flagship text assumed greenfield code.

## 1. Premise corrections (verified against source, 2026-09-13)

| Flagship-plan premise | Current evidence |
|---|---|
| "Create `src/Host/DistressSignalExpeditionBridge.cs`" | **Already exists as Core authority.** `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs` (staged lifecycle Heard→Identified→Dispatched→Reached→Terminal, deterministic deadline math, idempotent reward receipts) + host dispatch/arrival bridges in `src/Main.Expeditions.cs` (`BridgeDistressRescueOnDispatch` / `BridgeDistressRescueOnArrive`, ~lines 479–530) which call the **existing** `ExpeditionHostSession.StartExpedition(...)` path. |
| "Create `src/Host/DistressSignalDestinationResolver.cs`" | **Already exists as Core authority.** `Assets/Ashfall.Core/Radio/DistressDestinationResolver.cs` — precedence: direct canonical → versioned alias → `table_loot_*` owner → signal thematic → fail(ish). Required mappings verified: `loc_recovery_yard` canonical; `rural_gas_station` canonical; `table_loot_forestry_compound → loc_forestry_compound`. |
| "All five rescue quests" | Confirmed: `quest_distress_trapped_mechanic`, `quest_distress_injured_trader`, `quest_distress_family_shelter`, `quest_distress_raider_trap`, `quest_distress_military_patrol` — registered in `DistressRescueMissionManager.RegisterAuthoredRescueMissions` (authored mapping authority, tested by `DistressRescueMissionTests`) and present in `questline_master.json` entries 456–460 with `signalId` + `deadlineDays` + `rewardItems`. |
| "Quest completion too early" risk | Already honored: quest resolution happens on **arrival** (`RecordDestinationReached`), never at dispatch; rewards only on TerminalRescued/TerminalSurvived via `ClaimIdempotentRewards` (receipt-guarded, tested). |
| "Sender survival days / ignore_consequence" | **Real gap.** Definition DTO fields (`sender_survival_days`, `ignore_consequence`) exist in `DistressSignalDefinition` but have **zero consumers and zero authored data rows**. |
| "Authenticity detection runtime" | **Real gap.** Ground truth exists (`authenticity` field, `IsTrapOrDeception`/`IsGenuineRescue`/`IsGrimOrMemorial` predicates) but no survivor-skill-driven deterministic check, no persisted assessment, no dedicated RNG purpose. |

## 2. Authorities (one owner per concern — extended, never forked)

| Concern | Owner (this work extends) |
|---|---|
| Signal lifecycle / dispatch / expiry | `RadioDistressSystem` + `DistressRescueMissionManager` (Core) |
| Destination resolution | `DistressDestinationResolver` (Core) |
| Expedition creation / party / travel / weather / encounters | `ExpeditionHostSession.StartExpedition(...)` (unchanged) |
| Faction standing | Host `FactionStanceEngine.ModifyTrust` for mission faction tags (existing reward path); `FactionWarSystem.ModifyStanding` for the moral-choice ignore path |
| Quest catalog | `questline_master.json` + mission-manager quest ids |
| Radio save | `RadioSaveCodec` (V1→V2→V3 migration chain, checksummed) |
| Day tick | `RadioHostSession.SetDay` → `DistressSystem.TickDaily` + `RescueMissions.TickDaily` |

## 3. Silent gap sealed (found during Wave 0)

`DistressRescueMissionManager.CaptureState/RestoreState` existed with **zero
callers** — mission stage, deadline, expedition association, and claimed reward
receipts were silently lost on every reload. Fixed by persisting
`rescueMissions` inside `RadioSaveState` (additive V4 field, V3→V4 migration).

## 4. Contract decisions (frozen for this implementation)

- **C1 — One runtime state:** `DistressRescueMission` IS the per-signal runtime
  record (plan §2.1). Additive fields only; no parallel `DistressSignalRuntimeState`.
- **C2 — Deadline math unchanged:** `ExpiryDay = InterceptedDay + DeadlineDays`,
  initialized once at `RecordSignalHeard` (idempotent — Stage guard). Plan §7.2 satisfied.
- **C3 — Sender death day:** `SenderDeathDay = InterceptedDay + SenderSurvivalDays`
  (plan §2.5 contract, verbatim). Missions without a survival model (0 days)
  keep the legacy `currentDay > ExpiryDay` arrival check.
- **C4 — Arrival boundary:** with a live-sender model, arrival
  `currentDay >= SenderDeathDay` → dead branch (plan §8.6 case list). This is a
  deliberate, documented change from the legacy strictly-`>` deadline rule for
  survival-modeled missions (death day == deadline days for all authored
  rescue missions, so only the exact-boundary day changes).
- **C5 — Expiry semantics split (plan §7.13 test):** signals **without** an
  ignore consequence keep legacy `TickDaily → TerminalFailed` expiry (tested on
  family_shelter/raider_trap — unchanged). Signals **with** a consequence get
  `Expired=true` + consequence applied **once** and stay dispatchable so a later
  expedition resolves the remains/recovery branch (plan §7.6).
- **C6 — Consequence tokens authored:** `sender_death` for
  trapped_mechanic / injured_trader / military_patrol; military_patrol also
  carries `faction_standing_loss` (tag `military`); raider_trap and
  family_shelter carry none (legacy expiry preserved as the no-consequence case).
- **C7 — Core events expose facts:** consequences are Core facts
  (`OnIgnoreConsequence` event + Core-owned `SenderAlive` state); the host
  applies faction trust and journal lines. No standing mutation in Core bridge code.
- **C8 — Authenticity detection (Task 2):** pure Core evaluator
  (`SignalAuthenticityEvaluator`); skill lookup via the established
  `HasSkill(survivorId, skillId)` port pattern (DiplomaticSummitSystem precedent);
  skills `skill_signal_ear` (+15) / `skill_watchful` (+10) / `skill_cold_analysis`
  (+8); base deceptive-detection chance 35 (difficulty 65, plan §6.4 mapping);
  **genuine → never Trap/FalseFlag** (hard invariant, tested);
  stale detection = `authenticity == "stale"` AND trace complete; sub-stream seed
  derived via `StableHash` (never `GetHashCode`, never `new Random()`);
  first valid analysis persisted, repeat calls never reroll (anti-save-scum §6.9).
- **C9 — Idempotence guards:** `ArrivalResolved`, `IgnoreConsequenceApplied`,
  receipt set, dispatch-stage guard — each guarded once, persisted (plan §15).

## 5. Focused verification (executed 2026-09-13)

| Target | Result |
|---|---|
| `RescueSignalAuthenticityTests.cs` (Task 2) | 12/12 PASS |
| `RescueSignalIgnoreConsequenceTests.cs` (Task 3) | 12/12 PASS |
| `RescueSignalSenderSurvivalTests.cs` (Task 4) | 13/13 PASS |
| `RescueSignalRuntimePersistenceTests.cs` (save contract) | 5/5 PASS |
| Legacy `DistressRescueMissionTests.cs` | 7/7 PASS |
| Legacy `RadioSaveMigrationTests.cs` | 4/4 PASS |
| Full Radio test directory (30 files) | 220/220 PASS |
| `NewSaveStoreTriadTests.cs` (triad gate) | 6/6 PASS |
| `dotnet build Ashfall.csproj` | 0 errors / 0 warnings |
| `dotnet test Ashfall.Core.Tests` (full suite, explicit plan-§21 window) | 11,056/11,060 — 4 failures all PRE-EXISTING outside claimed paths (HostCliHelpContractTests ×2, CrossingEndings prose ×1, DeterminismGuard `CvdDiamondPanel.cs:104 DateTime.UtcNow` ×1 — the CLI help/prose/determinism findings reported in the closed economy batch) |

## 6. Test contracts retargeted (plan-mandated, documented)

1. `DistressRescueMissionTests.DeadlineMath_FailsMissionWhenArrivingPastExpiry` — the injured trader's Day-5 arrival now resolves through the sender-mortality authority (death day = first-heard + survival = 4), asserting `SenderAlive == false` and the dead-branch summary (plan §8.5/§8.6).
2. `RadioSaveMigrationTests` — migration target follows `CurrentSaveVersion` (now 4, rescue-signal runtime field); encode stamps V4.

## 8. Expansion wave (executed 2026-09-13)

| Piece | Authority touched | Result |
|---|---|---|
| Dead-arrival recovery/salvage (plan §8.5) | `ClaimIdempotentRewards` extended: `TerminalFailed` + sender dead + `ArrivalResolved` + survival model → salvage granted exactly once with **rep 0**; legacy deadline expiry (no expedition, no model) never grants | `RescueSignalSalvagePreflightTests` 13/13 |
| Authored salvage = the mission's own reward items (no new item ids, no data-integrity surface) | `DistressRescueMissionManager` | covered by the same suite |
| Structured dispatch preflight (plan §10) | NEW `RescueDispatchPreflight.cs` + `GetDispatchPreflight(signalId)` — truthful projection (analyzed/assessment/threat/sender/expired → DispatchRescue / DispatchWithWarning / RecoveryInvestigation / NotApplicable); pure projection, zero side effects; dispatch stays player agency | same suite |
| Host salvage routing | `RadioHostSession` stage hook claims on `TerminalFailed` too (eligibility stays Core-owned) | build + triad green |
| Same-day deadline/death boundary (plan §9) | patrol test: consequence + sender death each resolve exactly once on the boundary day; later recovery → salvage once | `RescueSignalDeterministicReplayTests` 6/6 |
| Trap-detected → dispatch → ambush (plan §17) | warning never gates dispatch; ambush → TerminalSurvived pipeline unchanged | same suite |
| Continuous == mid-reload replay (plan §14) | 7-step scenario fingerprint-equal across a mid-reload at day 3; analyzed-vs-unanalyzed divergence proves the fingerprint tracks runtime state; save-one-tick-before-deadline → fires once after restore; 3000-day undiscovered no-penalty | same suite |

**Contract updates from the expansion:** `RescueSignalSenderSurvivalTests` dead-arrival cases retargeted from "grants nothing" to "salvage once, rep 0" (plan §8.5 supersedes the first wave's stricter reading).

**Cumulative:** Radio directory 239/239 · build 0/0 · full suite 11,075/11,079 (+19 net passing; the 4 failures are the same pre-existing CLI-help/prose/CvdDiamond findings).

## 10. Presentation + content wave (executed 2026-09-13)

**Presentation (plan §10 surface):** `src/UI/RadioPanel.cs` gains a **RESCUE SIGNALS** strip bound to the live host session — one truthful line per registered rescue mission: stage (or `EXPIRED (unanswered)`), deadline day, sender window/death day, persisted analysis verdict (uppercase word, `DECEPTION FLAGGED` marker), and the structured preflight advisory; summary header counts actionable / deception warnings / unanswered. Words never color-only; no new modal/route, keyboard close path untouched.

**Content expansion (plan §24 — hostage calls, infected survivors, convoy SOS):** three new authored rescue scenarios through every existing authority (no new item ids, no new systems):

| Scenario | Signal | Quest | Destination | Deadline/Death | Consequence |
|---|---|---|---|---|---|
| Verity Motel Hostage | `freq_distress_726_5` | `quest_distress_hostage_call` + `quest_moral_distress_hostage_call` | `loc_motel_verity` | 4 / 4 | `sender_death` |
| Fever Ward Warden (almshouse east ward) | `freq_distress_609_4` | `quest_distress_infected_survivor` + `quest_moral_distress_infected_survivor` | `loc_st_brigids_almshouse` | 5 / 5 | `sender_death` |
| Salvage Crew Collapse | `freq_distress_455_7` | `quest_distress_convoy_sos` + `quest_moral_distress_convoy_sos` | `loc_warehouse_district` | 4 / 4 | `sender_death` |

Data files: `radio_distress_signals_expansion.json` (19 rows now; all three carry `moral_choice_id` + authored message fragments), `moral_choice_quests_distress.json` (+3 quests, canonical choice schema), `questline_master.json` (+3 quest rows, flagship-entry shape). Manager registers 3 new missions (8 total).

**Test contracts updated (content-additive, documented):** `AllFiveRescueMissions…` → `AllRescueMissions…` (8, +3 mapping asserts); catalog audit table 5→8 rows; `LandedCatalogLayers…` 25/16/36 → 25/19/39; layer contract 40 → 43 (39 JSON + 4 built-in fallbacks).

**Gates:** build 0/0 · Radio directory 239/239 · data-integrity selftest **PASS (325/325 catalogs)** · panel-bind-lifecycle PASS · ui-accessibility PASS · content-utilization PASS (CI + deep-chain) · triad 6/6 · full suite 11,075/11,079 (same 4 pre-existing failures).

## 11. Second content tranche (§24 continued, executed 2026-09-13)

Four more authored scenarios through the same four-catalog pattern; the consequence vocabulary gains **`faction_ambush`** (standing-affecting, distinct host-side journal future):

| Scenario | Signal | Quest | Destination | Deadline/Death | Consequence |
|---|---|---|---|---|---|
| Ransom Demand (Dentists' Row, river-nomad courier) | `freq_distress_555_0` | `quest_distress_ransom_demand` + moral choice | `loc_dentists_row` | 4 / 4 | `sender_death` + `faction_ambush` (`faction_river_nomads`) |
| Hijacked Evacuation Band (trap-class; heeding it is the ambush, ignoring is wise → no consequence, no moral choice) | `freq_distress_380_2` | `quest_distress_false_evacuation` | `collapsed_building` | 3 / — | none |
| Command Post Echo (Sergeant Brenner, Ordnance Shoulder) | `freq_distress_318_0` | `quest_distress_military_beacon` + moral choice | `loc_ordnance_shoulder` | 5 / 5 | `sender_death` |
| Winter Crossing (weather-gated Shallows route — existing weather authority governs the travel window) | `freq_distress_269_3` | `quest_distress_winter_crossing` + moral choice | `loc_the_shallows_market` | 4 / 4 | `sender_death` |

§24 checklist after both tranches: hostage calls ✅ · infected survivors ✅ · convoy SOS ✅ · ransom demands ✅ · false evacuations ✅ · military beacons ✅ · faction ambushes ✅ (as a consequence token) · weather-delayed rescues ✅ (emergent: far destination + existing weather gates) · radio triangulation ✅ (pre-existing DF → `RecordSignalIdentified` integration).

Data files: expansion signals 19→23 rows (all fragments validated day-contiguous, clarity strictly increasing); moral choices +3 (canonical schema); questline +4; missions 8→12.

**Gates:** build 0/0 · Radio directory 239/239 · data-integrity **PASS (325/325)** · content-utilization PASS (CI + deep-chain) · triad 6/6 · full suite 11,075/11,079 (same 4 pre-existing).

**Contract updates:** mission registration 8→12; audit table +4 rows; layer counts 19/39/43 → 23/43/47; false-evacuation `days_to_trace` corrected to 3 (fragment-contiguity contract caught the mis-dated third fragment).

## 13. Dispatch-association hardening (plan §5.8 sealed, executed 2026-09-13)

With 12 missions sharing the destination space, destination-only inference became the plan's own named latent risk. Matching rules moved into Core:

- `GetActiveMissionForDispatch(destinationId)` — dispatch selection: non-terminal missions at the destination, **Identified (triangulated) outranks Heard**, deterministic tiebreak (InterceptedDay → QuestId ordinal); already-dispatched missions are never re-selected.
- `GetMissionForArrival(destinationId, expeditionId)` — arrival resolution: **the persisted `ExpeditionId` is authoritative**; when any mission tied to the destination carries an association, an arrival whose expedition id does not match resolves **nothing** (plan §5.9 test 13 — unrelated arrivals can never resolve a rescue); the destination fallback applies only when no association exists (legacy saves keep the old behavior).
- Host bridges (`src/Main.Expeditions.cs`) are thin adapters over these rules; dispatch and arrival now derive the same `exp_{survivor}_{location}` id.

`RescueSignalDispatchAssociationTests` 8/8 (association-over-collision, unrelated-arrival, legacy fallback, association-through-restore, deterministic tiebreak).

**NPC-arc coupling (§24) — scoped out with evidence:** coupling the new scenarios to recurring characters requires authoring a new NPC into `holdfast_npcs.json` (10 canonical NPCs) plus an arc state machine in `npc_arcs.json` (24 arcs, precedence/terminal-state graph) and its expansion-quest choices — a cross-catalog narrative-continuity package (arc quests live in `narrative_encounters_npc_arcs.json`, consumed via `ExpansionQuestSystem` + the Plan 52 suppression filter). The extension points are proven (`npc_id` / `resolve_quest_id` / `NpcSignalSuppressionFilter`), but this is narrative-continuity work for a dedicated package with the narrative tooling, not a bolt-on here.

**Cumulative gates:** build 0/0 · Radio directory **249/249** · triad 6/6 · full suite 11,085/11,089 (+4 net passing; same 4 pre-existing failures).

## 12. Cumulative files (all waves)

**Core (`Assets/Ashfall.Core/Radio/`):**********
- `DistressRescueMissionManager.cs` — additive runtime fields (sender survival/death, ignore consequence, authenticity, arrival resolution), first-heard-once deadline init, arrival-day resolution + `ArrivalResolved` guard, ignore-consequence dispatch with idempotence + `OnIgnoreConsequence` fact event, fingerprinted save state, closed consequence-token vocabulary.
- `SignalAuthenticityEvaluator.cs` — NEW: deterministic skill-driven detection (genuine/trap/false_flag/stale), dedicated sub-stream RNG, persisted first result.
- `RadioSave.cs` — V4: `rescueMissions` field, frozen-V3 migration, mission fingerprint validated by the codec (property-based missions sit outside the field-walking checksum).
- `RescueDispatchPreflight.cs` — NEW: structured §10 preflight types (recommendation enum + projection DTO).

**Host (`src/`):**
- `Host/RadioHostSession.cs` — mission state captured/restored in the radio save; distress definitions wired into the mission manager; `TerminalFailed` salvage claim routing.
- `Main.Narrative.cs` — `OnIgnoreConsequence` host application: faction-trust loss via the canonical `FactionStanceEngine` + journal line.
- `UI/RadioPanel.cs` — RESCUE SIGNALS strip (wave 10).
- `Main.Expeditions.cs` — dispatch/arrival bridges moved onto the Core association rules (wave 13).

**Tests:** 7 new files (71 cases) under `Ashfall.Core.Tests/Radio/`. **Docs:** this map.
