# Plan 17 — Legibility: Cause, Effect, and Guidance

> **Wave:** Continuity Wave 1
> **Depends on:** 16B for stable live panels; benefits directly from 15A/15B (a choice with no
> readable consequence is still an unreadable game).
>
> **Theme:** ASHFALL simulates ~128 Core systems across 19 day-advance owners, and the player
> can't see *why anything happened*. The typed cause/effect channel is already built and almost
> nobody writes to it. The teaching layer is built and unreachable. The confirmation layer
> (sound) is 14-of-20 closed but still silent on ordinary panel actions. This plan spends
> existing plumbing rather than new systems.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| Finding | Evidence | Verified how |
|---|---|---|
| The typed day-event channel exists in Core | `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs:355` — `DayStateChangeEvent { Kind, SourceOwnerId, PrimaryId, SecondaryId, Numeric }`; `IDayAdvanceOwner.TickDay(int, List<DayStateChangeEvent>)` at `:320`; owners collect into `DayOwnerReport` (`:377–86`) and surface via `DayAdvancedEventArgs` | source read |
| **Exactly one** Core system emits into it | grep for `DrainDayEvents` / `new DayStateChangeEvent` → `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs:129,348,359` and a perf harness only | grep |
| 19 owners are registered, 18 are mute | `grep -rhoE '_campaignDay\.Register\("[a-z_]+"' src/*.cs` → `crafting_production, duty_roster, economy_market, expeditions_caravans, greenhouse_foundry, holdfast_core, host_events, maritime_deep_coast, medical_disease, memorial, narrative_quests_verdict, phase0_psychology, power_grid, shelter_facilities, starting_level_rations, survivors_needs, survivor_social, weather_world, world_evolution` | grep |
| So the briefing runs on its **fallback** branch | `src/Main.Campaign.cs:225` `if (args != null && args.AllEvents().Any())` → real path; `:229+` hand-rolled fallback reads *levels* ("X is hungry (73%)", "Canned Food in stock: 4") | source read |
| Fallback is hardcoded to 3 items + 3 survivors' bars | same region: `canned_food`, `clean_water`, `fuel_canister`, thresholds `80/40`, `50/15`, `60/90` — no cause, no attribution to the player's act | source read |
| The guidance overlay exists and cannot be opened | BUG-UI-004: `OnboardingHintPanel._Ready()` sets `Visible = false`; `SetupOnboarding()` constructs + persists it; grep for any `Visible`/open/show route → **0 hits**. `src/Main.Onboarding.cs:72` wires `OnShowMeWhereRequested → OpenPlayerPanel`, but nothing opens the panel itself | grep + audit |
| UI feedback is still partial | `docs/audio/SILENCE_AUDIT.md` §9: 14/20 closed, **6 PARTIAL** — #2 UI panels, #3 ambience, #4 music, #13 shelter door, #17 item pickup, #18 danger; 17 `PlayCue` call sites across **164** files in `src/UI` | grep + audit |
| One audio item is blocked by a Core gap, not wiring | `rad_geiger_loop` cannot stop: "Core has no explicit exposure-end signal" (`docs/audio/AUDIO_QA_REPORT.md`, `SILENCE_AUDIT.md` §12) | audit |
| Alert stacking is unresolved | SILENCE_AUDIT §8: fallout storm can stack 3–4 Alerts-bus cues inside 5 s; no bus ducking | audit |
| Snapshot safety net exists | 29 golden UI targets, `docs/ui/SNAPSHOT_COVERAGE.md`, `ashfall-snapshot-diff` skill | docs |

**Central reading:** three separate channels — *cause* (`DayStateChangeEvent`), *guidance*
(`OnboardingJourney`), *confirmation* (cue catalog) — are each built and each left half-connected.
None needs a new system. All three need producers, one route, and ducking.

---

## Task 17A — Attribution: make the daily briefing say "you did X, so Y"

> ### ⚠️ ERRATUM (found while auditing Wave 4 — read before executing this task)
> The premise below ("the typed `DayStateChangeEvent` channel … has exactly one producer, so 18 of
> 19 owners are mute and the briefing runs on its fallback") is **wrong**. Re-measured at
> `ccac926e`: all **19** owner classes emit (26 sites in `src/Main.CampaignOwners.cs`), and
> `src/Main.Campaign.cs:225` takes the **primary** branch every day. The real defect is vocabulary
> drift: **20 of 27** emitted kinds hit no `case` in `DailyBriefingReportBuilder.cs:49–115`, which
> has **no `default:`** (silent drop), while **6 handled kinds are never emitted**
> (`survivor_condition`, `resource_delta`, `shelter_consequence`, `weather_condition`,
> `radio_transmission`, `crafting_production`).
> **Execute [Plan 31](Plan_31_Event_Layer_Semantic_Kinds_No_Silent_Drops.md) instead of this task** —
> same goal, correct diagnosis, less work (transport + consumer already exist). 17B/17C stand.
> Full details: [`Wave4_Continuity_Audit_INDEX.md`](Wave4_Continuity_Audit_INDEX.md#-erratum--this-wave-disproved-one-of-my-own-wave-1-findings).

**Goal:** convert the briefing from a state readout into a consequence report by giving the 18
mute day-advance owners a small, uniform emission contract, then displaying it.

**Files:** `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs` (read-only first),
`src/Main.CampaignOwners.cs`, `src/Main.Campaign.cs` (`ShowBriefingForDay`),
`DailyBriefingReportBuilder` (Core), the 19 owner implementations,
`Ashfall.Core.Tests/DailyBriefing*Tests.cs`.

### Substeps

1. **Freeze the contract in writing** before touching code: pick the canonical `Kind` vocabulary
   (`need_worsened`, `ration_cut`, `dose_added`, `fire_started`, `structure_wore`,
   `trade_settled`, `quest_advanced`, `unit_deployed`, `crop_failed`, …). snake_case, listed in a
   Core constants file so nobody invents per-owner strings.
2. **Add `DayEventKinds` to Core** as the single authority, with a doc-comment per kind naming
   the producer and the meaning of `PrimaryId`/`SecondaryId`/`Numeric`. `DayStateChangeEvent`
   already has the right shape — do not add fields unless a display genuinely needs one.
3. **Adopt the existing drain pattern**: copy `SurvivorFateSystem`'s
   `_pendingDayEvents` / `DrainDayEvents(target)` idiom (`:129`, `:348`, `:359`) into a small
   reusable Core helper (e.g. `DayEventSink`) so 18 owners don't each hand-roll a list.
4. **Emit from the highest-signal owners first** (stop after 6, verify, then continue):
   `survivors_needs`, `starting_level_rations`, `power_grid`, `shelter_facilities`,
   `economy_market`, `expeditions_caravans`.
5. **Rule of thumb per owner:** emit on *transitions and decisions*, never on steady state. A
   ration cut is an event; "hunger is 73%" is not. This is what makes the report read as cause.
6. **Cap volume deterministically**: at most N events per owner per day, aggregated
   (`3 survivors became starving`), chosen by severity — not by dictionary order. Aggregation
   must be order-stable (Invariant 4).
7. **Verify the real branch now fires**: after step 4–6, confirm
   `src/Main.Campaign.cs:225` `args.AllEvents().Any()` is true in a normal session, and that the
   fallback branch is now the exception (headless/no-campaign) rather than the rule.
8. **Do not delete the fallback** — it is the headless-test path. Instead, extract its hardcoded
   thresholds into the same `DayEventKinds` vocabulary so both branches speak one language.
9. **Group the report by attribution, not by severity**: sections
   `YOUR DECISIONS → CONSEQUENCES → NEEDS ATTENTION → UNCHANGED`, so the first thing a player
   reads is what *they* caused. Reuse the existing `DailyBriefingEntry` category mechanism.
10. **Link each line to a surface**: every briefing entry already carries an entity id
    (`PrimaryId`); add "open the relevant panel" behaviour using the existing
    `OpenPlayerPanel(route)` seam already proven in `src/Main.Onboarding.cs:72`. This is the
    single highest-value cross-system link in this plan: consequence → the screen that fixes it.
11. **Journal parity**: the same events should feed the events log (`events_log` route) so the
    history is queryable, not just a modal that vanishes on close.
12. **Fail-closed discipline**: the coordinator already records `DayOwnerReport.FailureMessage`
    and `HasFailures` (`:382–388`); surface a *visible* briefing warning when an owner fails, so
    a silently skipped tick never reads as "nothing happened today".
13. **Tests:** contract test per kind (producer exists, ids resolvable), aggregation stability
    test, briefing-content test for a scripted day, and a determinism test — same seed, same
    briefing text.
14. **Run the five-step verification checklist.**

**DoD:** in a normal day the briefing explains at least three player-caused outcomes with
clickable routes, and a failing owner is visible instead of silent.

---

## Task 17B — Guidance: a reachable, reopenable `GUIDANCE` overlay

**Goal:** the first session teaches the loop instead of dumping 134 panels on the player, and
the help is always one key away.

**Files:** `src/Main.Onboarding.cs`, `src/UI/OnboardingHintPanel.cs`,
`src/UI/GameDashboardPanel.cs`, `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`,
`project.godot` `[input]`, `Assets/Ashfall.Core/Onboarding/*` (read-first),
`docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`, `ashfall-tutorial-review` skill output.

### Substeps

1. **Read `OnboardingJourney` first** — enumerate its stages, its persisted fields, the
   `SetDay`/`OnJourneyChanged`/`SetOnboardingAssistance` API already used in
   `src/Main.Onboarding.cs:56–76`. The state machine is done; do not redesign it.
2. **Prove the defect** in a runtime probe: assert `OnboardingHintPanel` is never `Visible`
   during a session. Commit the failing assertion first.
3. **Add the route**: register/reuse `help` (already `PanelGroup.Dashboard`) as the reopen
   affordance, and add a `GUIDANCE` nav entry beside the existing 30 `AddNavButton` rows in
   `GameDashboardPanel.cs:390–409`. One new button, not a new panel.
4. **Add the keybinding**: an `ui_guidance` InputMap action (F1) handled in the existing input
   path; respect the project's rebinding conventions and run the `ashfall-input-map-audit`
   checklist so the new key doesn't collide with the existing 12 actions.
5. **Fix the visibility bug at the source**: `_Ready()` sets `Visible = false`; make the panel's
   visibility owned by the host route (`Open()`/`Close()` in `ConfigureActions`) instead of by
   the constructor.
6. **Wire "show me where" fully**: `OnShowMeWhereRequested` already closes the overlay and opens
   the target panel. Add the missing half — mark the journey step advanced on that action so the
   hint doesn't nag.
7. **Gate on real progress, not on the clock**: advance steps from `DayStateChangeEvent`s
   (17A) and from actual player actions (a completed craft, a returned expedition), reusing the
   `_onboardingFailedActions` / `_onboardingLastInteractionSeconds` fields that already exist
   (`src/Main.Onboarding.cs:28–29`) for idle nudges.
8. **Surface the status bar path**: `RefreshOnboardingStatusBar()` is already called at
   `src/Main.Onboarding.cs:60,64`; confirm it is legible and dismissible, and that dismissing a
   hint never blocks reopening the overlay.
9. **Day-1 to Day-3 teaching order**: rations → water/filter → craft → expedition → advance day
   → dose. Cross-check against `ashfall-tutorial-review` ("teach vs demand") and confirm the
   first three days never *demand* an untaught action.
10. **Accessibility**: text alternatives for colour cues, readable contrast, keyboard-only path
    to every taught screen; run `ashfall-ui-access` on the overlay. Guidance must work with the
    assistance toggle off (no forced hand-holding for veterans).
11. **Snapshot target** for the overlay following `docs/ui/SNAPSHOT_FIXTURE_POLICY.md`; add it to
    `SNAPSHOT_COVERAGE.md` as a real COVERED target, since it is now player-facing.
12. **Tests:** route test (guidance opens and reopens), journey persistence across save/load
    (`OnboardingSaveStore` already exists — prove the reopen path after load), and a journey test
    in `Main.UiTests` that walks Day 1 steps without getting stuck.
13. **Re-test the load path**: `src/Main.Lifecycle.cs:290–294` tears the panel down on session
    swap; assert a fresh panel is constructed and re-bound on `New Game` and `Load`.
14. **Run the checklist.**

**DoD:** a new player can reach guidance from the HUD and F1 at any moment, and the overlay
advances because of what the player did.

---

## Task 17C — Confirmation: finish the feedback layer (UI cues, ambience, ducking, geiger stop)

**Goal:** close the 6 partial silence gaps and the one Core gap blocking them, so ordinary acts
feel real without adding a single new asset family.

**Files:** `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioCueCatalog.cs`,
`src/Audio/ShelterAudioController.cs`, `src/UI/AshfallUiHelpers.cs`, host sessions
(inventory, expedition, shelter), `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`,
`docs/audio/SILENCE_AUDIT.md`, `docs/audio/AUDIO_QA_REPORT.md`.

### Substeps

1. **Re-run the audit as a gate, not prose**: `--audio-selftest` (last recorded 245/245, 70
   cues) and `generate-audio-catalog.py --check`; capture the current numbers before any change.
2. **UI cue coverage through the shared helper**: `AshfallUiHelpers` is the single button
   factory used across the 164 UI files — extend it so `MakeActionButton` plays `ui_click`,
   confirm/invalid/warning map to their existing cues, and tab/modal cues fire from the dashboard
   shell. One change, whole-game effect; no per-panel scattering.
3. **Ambience state machine (7C item 2)**: drive `amb_bunker` vs `amb_surface` from *campaign*
   state — which screen the player is on, `WeatherSystem.Current`, and `PowerGridSystem` load tier
   (the inputs the silence audit already names). Follow `ShelterAudioController`'s existing
   loop-lifecycle discipline (start once, stop on session replacement).
4. **Music on transitions**: menu ↔ gameplay ↔ ending, reusing the two existing music cues, and
   re-check the path bug class fixed in 7B (extension mismatch `.wav` vs `.ogg`) by validating
   every music path through the catalog rather than by literal string.
5. **Item-pickup parity**: `action_item_pickup` currently fires only via expedition completion;
   move it to the inventory mutation seam so any acquisition path (craft output, trade, gift,
   scavenging, autopsy) is audible exactly once.
6. **Danger cues from real hazards**: wire `danger_explosion` / `danger_glass_break` /
   `danger_debris` to existing hazard events (foundry incident, sump flooding, hatch defense,
   vault breach) rather than inventing new ones — every one of those systems is already in a
   day-advance owner.
7. **Core gap: exposure-end signal.** Add an explicit end-of-exposure event to
   `RadiationSystem` so `rad_geiger_loop` can stop safely (the recorded blocker in
   `AUDIO_QA_REPORT.md`). Type-safe C# event, capture/restore unaffected, no new RNG.
8. **Deduplicate shared assets** (audit §5): `rad_contamination` vs `weather_black_rain`
   currently share one file; `weather_alert` vs `danger_alarm_klaxon` share a klaxon;
   `shelter_pipe_clang` vs `day_transition`. Produce or assign distinct assets for these four
   pairs — sound must not blur two different threats.
9. **Bus ducking (7C item 1)**: when an Alerts cue fires, duck Ambience/SFX ~6 dB for its
   duration; also enforce a per-bus concurrency cap so the storm + dose + klaxon stack degrades to
   the loudest single alert instead of a mush.
10. **Prune or use dead topology**: 5 buses were unused at audit time and now carry cues
    (Generator, Ventilation, Medical, Surface) — verify each named bus has ≥1 routed cue and that
    `AudioSelfTest` validates **all 12** buses, not the original 7.
11. **Settings parity**: every newly used bus needs a visible volume slider and a recovery path —
    run `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md`.
12. **Regression tests:** a host-side probe that each UI cue fires exactly once per act (guards
    the double-fire class 16C also fixes), plus a determinism-neutral check that adding audio
    wiring cannot change simulation output (assert identical `SaveChecksum` with audio disabled).
13. **Update `docs/audio/SILENCE_AUDIT.md` status column** to 20/20 or list what remains with a
    reason — stale audit prose is how the next agent re-does this work.
14. **Run the checklist**, `--audio-selftest`, `--data-integrity-selftest`.

**DoD:** every ordinary player act has exactly one audible confirmation; alert pile-up is
controlled; the geiger loop stops when exposure ends.

---

## Cross-Task Dependencies

```
17A (cause events) ──► 17B (guidance reacts to real progress)
        │                     │
        ├──► 15B feeds choice consequences into the same channel
        │                     │
        └──► 17C (confirmation on real events) ◄── 16C (no double-fire)
```

**Execution order:** 17A → 17C → 17B. Build the event vocabulary first; guidance and audio both
consume it. 17B last because its step-advance conditions come from 17A's event kinds.
Hard prerequisite: **16B** — ambience/ducking driven by `Weather`/`PowerGrid` state is worthless
while panels can bind to throwaway instances of those systems.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --audio-selftest                 # 0 silent paths, all buses validated
7. bash scripts/ci/triad-drift-gate.sh
8. bash scripts/ci/verify-fast.sh
9. manual: docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md Day 1 → Day 2
```

---

## Estimated Effort & Risk

| Task | Owners touched | Host | UI | Core | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 17A | 19 (6 first) | 3 | 1 | 2 | 10–14 | Medium | MEDIUM (briefing text churn → snapshot review) |
| 17B | 0 | 2 | 2 | 1 (route) | 4–6 | Low–Med | LOW (additive; project.godot input edit) |
| 17C | 0 | 5 | 1 | 1 (exposure-end) | 5–8 | Medium | MEDIUM (audio-only, but bus + settings parity) |

**Guardrails:** no new panels, no new buses beyond existing names, no procedural audio
families beyond the four de-duplication replacements, no new briefing framework — reuse
`DailyBriefingReportBuilder`. If an event kind can't be attributed to a player act, it doesn't
earn a line in the briefing.
