# CF-P1-DISTRESS-CONTENT-SEAL — Integration Plan (Plan 02 / "Plan 01" of the 2026-09-19 execution program)

> **Status:** SEALED — executed and verified 2026-09-19 under
> `claim-cf-p1-distress-content-seal-2026-09-19` (`WORKTREE_OWNERSHIP.md`).
> **This plan document was not updated after execution; the sections below
> retain their original forward-looking wording exactly as authored, and
> §26 (appended 2026-09-21, during this batch's review pass) records the
> actual closeout evidence.** Read §26 first if you are deciding whether
> this package still needs a claim — it does not.
> **Program source:** `Seal-steps/ashfall-eight-unblocked-plans-completion-first-execution-program-2026-09-19.md` §B.2 row 01, §C.1 (Plan 01).
> **Origin rule phrasing:** `Seal-steps/ashfall-six-unblocked-partial-integration-plans-full-implementation-plan.md` (P1); `Seal-steps/ashfall-p1-follow-up-content-authored-market-board-panel-design-census-route-19-review-and-further-expansions.md` lines 19–20.
> **Evidence date:** 2026-09-19, branch `Zcode_Branch` @ `65357b8a` (plus listed untracked files, none touching this package's paths).
> **Premise-audit verdict (program §B.3 protocol):** READY — every premise re-verified against current source; divergences recorded in §4.4.

---

# 1. Objective

Seal the distress-signal follow-up and audio-cue **content** that landed in the
two authoritative catalogs after the mechanism was sealed (Waves 1–5 of
`DISTRESS-SIGNALS-9-12`), by adding the three deferred validator rules, proving
every authored follow-up chain end-to-end through the real V6 save codec,
verifying every referenced audio-cue id against the host-side cue registry,
re-confirming the content-utilization gate at 0 orphans over the distress
layer, and publishing the PR3 closeout document.

This package is explicitly **verify-and-seal**, not new mechanism and not new
content. The distress mechanism — stage resolution, follow-up scheduling,
signal trust, audio-cue resolution, V6 persistence — is complete, tested, and
sealed. What is missing is the seal tail: the three validator rules named in
the origin documents, a population-level replay over the shipped content (not
fixtures), a permanent host-side cue-registry cross-reference, and the
governance record that the package is sealed.

**Bounded outcome:**

1. `CatalogIntegrityValidator` rejects, inside the permanent
   `--data-integrity-selftest` gate: (a) `expired`-trigger follow-ups on
   no-consequence signals, (b) trap-grammar follow-ups on non-trap identities,
   (c) more than two `follow_up_signals` entries on one signal.
2. The shipped catalogs are remediated so the gate passes with **0 errors and
   exactly the 5 pinned primary-wins warnings** (dead-data register unchanged).
3. A population replay test drives **every authored follow-up entry** through
   its trigger transition via the real `RadioSaveCodec` V6 path and asserts
   fire day, exactly-once ledgers, trust delta, and resolved cue id — with a
   stable two-run fingerprint.
4. `--audio-selftest` permanently cross-references every distress-catalog cue
   id against `AudioCueCatalog`; `--content-utilization-selftest` passes with
   0 orphans.
5. `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` records the census, the rules,
   the remediations, the replay results, the registry verification, the
   utilization result, and the known reachability limitation; the integrator
   adds the `INTEGRATION_PLANS.md` seal row.

**Non-goals:** no new follow-up/stage/cue content; no new missions; no runtime
scheduler, trust, audio, or save changes; no save-version bump; no panel work;
no second registry; no rule relaxations to make data pass.

---

# 2. Current Reality

Everything below was re-read at HEAD on 2026-09-19. Line citations are to the
files as they exist now.

## 2.1 Mechanism (sealed, do not rebuild)

| Concern | Authority | Evidence |
|---|---|---|
| Stage model | `message_fragments` on `DistressSignalDefinition` (absolute-day thresholds, strictly ascending, clarity non-decreasing) | `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs:170-171`; contract `docs/radio/DISTRESS_SIGNAL_STAGE_CONTRACT.md` §1–2 |
| Sole stage resolver | `DistressStageResolver.Resolve/ResolveStageIndex` (pure, deterministic; `RadioTuner` + `RadioPropagation` delegate) | `Assets/Ashfall.Core/Radio/DistressStageResolver.cs:53-90` |
| Follow-up trigger grammar | `SignalFollowUpTriggers` — closed set: `answered`, `rescue_success`, `rescue_failed`, `expired`, **`ambush_encountered`** | `Assets/Ashfall.Core/Radio/DistressFollowUpScheduler.cs:14-44` |
| Follow-up scheduler | `DistressFollowUpScheduler` — campaign-day scheduling (`SetDay` before mission tick), exactly-once pending + persisted fired ledgers, ordinal same-day order `(dueDay, parentSignalId, followUpId)`, removed-definition pending entries expire silently | `DistressFollowUpScheduler.cs:113-330` |
| Follow-up event source | `DistressRescueMissionManager.OnStageChanged` / `OnIgnoreConsequence` only; scheduler binds via `BindToMissionEvents()` | `DistressFollowUpScheduler.cs:202-243`; `DistressRescueMissionManager.cs:180-190` |
| Mission registry | **Exactly 12 authored missions** registered in the manager constructor; additional missions arrive only via save restore. No dynamic runtime registration API exists | `DistressRescueMissionManager.cs:192-201` (`RegisterAuthoredRescueMissions()`), `:828-910` (the 12 fixtures), `:818-823` (restore path) |
| Signal trust | `SignalTrustLedger` + `SignalTrustPolicy` — integer [0,100], neutral 50, deltas +2 answered / +5 rescue / −2 ignored / −5 ambush, exactly-once per signal | `Assets/Ashfall.Core/Radio/SignalTrustLedger.cs:22-40, 82-138` |
| Trust availability | `SignalTrustAvailability` — **dormant/retired as a runtime consumer** (Wave 9 Part 2 Option B); retained as a tested math pin only | `docs/radio/SIGNAL_TRUST_CONTRACT.md` §5 retirement note |
| Audio cue resolution | `DistressAudioCueResolver` — stage override → signal default → empty (text-only); explicit ids, zero RNG | `Assets/Ashfall.Core/Radio/DistressAudioCueResolver.cs:30-63` |
| Playback gate | Intercept edge only; dedupe key `distress:{signalId}:{cueId}` rides the persisted `playedBroadcastKeys` ledger; missing cue logs once, text continues | `src/Host/RadioHostSession.cs:283-296`; `docs/radio/DISTRESS_SIGNAL_AUDIO_CONTRACT.md` §4, §6 |
| Save codec | `RadioSaveCodec` **V6** (`CurrentSaveVersion = 6`); frozen shapes V1–V5; `MigrateV5` → empty follow-up scheduler default; checksum hard-reject on tamper; mission fingerprint validation | `Assets/Ashfall.Core/Radio/RadioSave.cs:170-186` (state), `:189-262` (codec), `:434-480` (MigrateV5) |
| Catalog validation | `CatalogIntegrityValidator.ValidateDistressSignalStages` — identity, stage contract, follow-up shape, closed trigger grammar, structural cue-id rule | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:1901-1911` (entry), `:1913-2144` (per-file), `:2059-2137` (follow-up block), `:2235-2246` (`IsValidAudioCueId`) |

## 2.2 Shipped content census (verified by direct JSON parse, 2026-09-19)

**Catalog rows:** `radio_distress_signals.json` = **25** broadcasts;
`radio_distress_signals_expansion.json` = **23** broadcasts → 48 rows, **43
unique effective identities** (5 cross-file duplicates resolved by the
documented primary-wins load order: expansion first, primary last —
`src/Host/RadioHostSession.cs:170-184`), plus 4 builtin-only fragment-less
fallbacks = 47 runtime-registered signals. Matches the Wave 0 baseline PC-1
correction and the dead-data register (5 pinned warnings).

**Follow-up content (landed post-PR2, in commits `660cb595` / `8f72ee62`,
without its own seal — the gap this package closes):**

- **24 signals carry `follow_up_signals`** (17 primary + 7 expansion rows).
- **43 total follow-up entries** (30 primary + 13 expansion). The program
  doc's "17+7 follow-ups" / "24 authored follow-up entries" counts the
  *signals*; the entry count is 43. (Refinement R1, §4.4.)
- Trigger distribution: `answered` ×17, `rescue_success` ×16,
  `ambush_encountered` ×7, `expired` ×3, `rescue_failed` ×0.
- No follow-up entry authors `outcome_hint` (all 43 `hint: absent`).
- 21 entries carry a non-empty `audio_cue`; 22 entries are explicitly
  text-only (empty cue).

**Full census table** (columns: catalog P=primary/E=expansion; mission =
signal has a registered `DistressRescueMissionManager` mission; cue empty =
text-only):

| # | Cat | Signal | Follow-up id | Trigger | Delay | Cue | Mission |
|---:|:--:|---|---|---|---:|---|:--:|
| 1 | P | freq_distress_217_4 | fu_217_4_answered | answered | 2 | radio_distress_beacon | no |
| 2 | P | freq_distress_217_4 | fu_217_4_rescue_success | rescue_success | 4 | radio_distress_beacon | no |
| 3 | P | freq_distress_148_2 | fu_148_2_trap_fallen_for | ambush_encountered | 1 | radio_static | no |
| 4 | P | freq_distress_55_1 | fu_55_1_answered | answered | 2 | radio_vinyl_broadcast | no |
| 5 | P | freq_distress_55_1 | fu_55_1_expired | expired | 3 | radio_static | no |
| 6 | P | freq_distress_401_9 | fu_401_9_answered | answered | 2 | radio_distress_beacon | no |
| 7 | P | freq_distress_401_9 | fu_401_9_rescue_success | rescue_success | 4 | radio_distress_beacon | no |
| 8 | P | freq_distress_88_3 | fu_88_3_answered | answered | 2 | *(text-only)* | yes |
| 9 | P | freq_distress_88_3 | fu_88_3_rescue_success | rescue_success | 5 | *(text-only)* | yes |
| 10 | P | freq_distress_156_8 | fu_156_8_answered | answered | 2 | *(text-only)* | yes |
| 11 | P | freq_distress_156_8 | fu_156_8_rescue_success | rescue_success | 4 | *(text-only)* | yes |
| 12 | P | freq_distress_203_1 | fu_203_1_answered | answered | 2 | radio_distress_beacon | no |
| 13 | P | freq_distress_203_1 | fu_203_1_rescue_success | rescue_success | 4 | radio_distress_beacon | no |
| 14 | P | freq_distress_311_5 | fu_311_5_answered | answered | 2 | radio_distress_beacon | no |
| 15 | P | freq_distress_311_5 | fu_311_5_rescue_success | rescue_success | 4 | radio_distress_beacon | no |
| 16 | P | freq_distress_445_2 | fu_445_2_answered | answered | 2 | *(text-only)* | yes |
| 17 | P | freq_distress_445_2 | fu_445_2_rescue_success | rescue_success | 5 | *(text-only)* | yes |
| 18 | P | freq_distress_192_4 | fu_192_4_trap_fallen_for | ambush_encountered | 1 | *(text-only)* | yes |
| 19 | P | freq_distress_410_7 | fu_410_7_trap_fallen_for | ambush_encountered | 1 | radio_static | no |
| 20 | P | freq_distress_288_1 | fu_288_1_trap_fallen_for | ambush_encountered | 1 | radio_static | no |
| 21 | P | freq_distress_333_6 | fu_333_6_trap_fallen_for | ambush_encountered | 1 | radio_static | no |
| 22 | P | freq_distress_478_2 | fu_478_2_trap_fallen_for | ambush_encountered | 1 | radio_static | no |
| 23 | P | freq_distress_812_5 | fu_812_5_answered | answered | 2 | radio_distress_beacon | no |
| 24 | P | freq_distress_812_5 | fu_812_5_rescue_success | rescue_success | 4 | radio_distress_beacon | no |
| 25 | P | freq_distress_812_5 | fu_812_5_expired | expired | 3 | radio_static | no |
| 26 | P | freq_distress_867_9 | fu_867_9_answered | answered | 2 | radio_distress_beacon | no |
| 27 | P | freq_distress_867_9 | fu_867_9_rescue_success | rescue_success | 4 | radio_distress_beacon | no |
| 28 | P | freq_distress_867_9 | fu_867_9_expired | expired | 3 | radio_static | no |
| 29 | P | freq_distress_901_2 | fu_901_2_answered | answered | 2 | *(text-only)* | yes |
| 30 | P | freq_distress_901_2 | fu_901_2_rescue_success | rescue_success | 5 | *(text-only)* | yes |
| 31 | E | freq_distress_726_5 | fu_726_5_answered | answered | 2 | *(text-only)* | yes |
| 32 | E | freq_distress_726_5 | fu_726_5_rescue_success | rescue_success | 4 | *(text-only)* | yes |
| 33 | E | freq_distress_609_4 | fu_609_4_answered | answered | 2 | *(text-only)* | yes |
| 34 | E | freq_distress_609_4 | fu_609_4_rescue_success | rescue_success | 5 | *(text-only)* | yes |
| 35 | E | freq_distress_455_7 | fu_455_7_answered | answered | 2 | *(text-only)* | yes |
| 36 | E | freq_distress_455_7 | fu_455_7_rescue_success | rescue_success | 4 | *(text-only)* | yes |
| 37 | E | freq_distress_555_0 | fu_555_0_answered | answered | 2 | *(text-only)* | yes |
| 38 | E | freq_distress_555_0 | fu_555_0_rescue_success | rescue_success | 4 | *(text-only)* | yes |
| 39 | E | freq_distress_380_2 | fu_380_2_trap_fallen_for | ambush_encountered | 1 | *(text-only)* | yes |
| 40 | E | freq_distress_318_0 | fu_318_0_answered | answered | 2 | *(text-only)* | yes |
| 41 | E | freq_distress_318_0 | fu_318_0_rescue_success | rescue_success | 5 | *(text-only)* | yes |
| 42 | E | freq_distress_269_3 | fu_269_3_answered | answered | 2 | *(text-only)* | yes |
| 43 | E | freq_distress_269_3 | fu_269_3_rescue_success | rescue_success | 4 | *(text-only)* | yes |

**The 12-mission registry** (the only signals whose follow-up triggers can
fire at runtime today; from `RegisterAuthoredRescueMissions`,
`DistressRescueMissionManager.cs:828-910`):

| # | Quest id | Signal | Deadline | Consequence tokens | Trap |
|---:|---|---|---:|---|:--:|
| 1 | quest_distress_trapped_mechanic | freq_distress_88_3 | 5 | sender_death | no |
| 2 | quest_distress_injured_trader | freq_distress_156_8 | 3 | sender_death | no |
| 3 | quest_distress_family_shelter | freq_distress_445_2 | 4 | *(none — legacy expiry)* | no |
| 4 | quest_distress_raider_trap | freq_distress_192_4 | 5 | *(none)* | yes |
| 5 | quest_distress_military_patrol | freq_distress_901_2 | 5 | sender_death, faction_standing_loss | no |
| 6 | quest_distress_hostage_call | freq_distress_726_5 | 4 | sender_death | no |
| 7 | quest_distress_infected_survivor | freq_distress_609_4 | 5 | sender_death | no |
| 8 | quest_distress_convoy_sos | freq_distress_455_7 | 4 | sender_death | no |
| 9 | quest_distress_ransom_demand | freq_distress_555_0 | 4 | sender_death, faction_ambush | no |
| 10 | quest_distress_false_evacuation | freq_distress_380_2 | 3 | *(none)* | yes |
| 11 | quest_distress_military_beacon | freq_distress_318_0 | 5 | sender_death | no |
| 12 | quest_distress_winter_crossing | freq_distress_269_3 | 4 | sender_death | no |

**Reachability consequence (finding F1, §4.4):** 12 of the 24
follow-up-carrying signals — 217_4, 148_2, 55_1, 401_9, 203_1, 311_5, 410_7,
288_1, 333_6, 478_2 (no mission, no moral-choice link) and 812_5, 867_9
(moral-choice link `quest_moral_distress_child_school` /
`quest_moral_distress_siblings_clinic`, but **no registered mission** — the
moral-choice quest system is location-triggered and does not register
`DistressRescueMission`s) — carry **21 of the 43 entries** that cannot fire at
runtime today, because every follow-up trigger is produced exclusively by
mission-manager lifecycle events. This is a *content-reachability* finding for
the PR3 closeout (decision-needed routing), **not** a defect this
verify-and-seal package repairs (see §6.6, §17, §22).

**Audio-cue census:**

- `audio_cue` occurrences: 66 (primary) + 36 (expansion) = 102 total across
  signal-default, fragment-override, and follow-up levels.
- Exactly **7 distinct cue ids** referenced: `radio_static`, `radio_morse`,
  `radio_numbers_station`, `radio_ebs_alert`, `radio_dead_hand_pulse`,
  `radio_distress_beacon`, `radio_vinyl_broadcast`.
- All 7 exist as named constants in the host registry
  (`src/Audio/AudioCueCatalog.cs:229-237`). Registry *membership* is therefore
  expected to pass; what has never run is the permanent cross-reference gate
  (membership + resource/fallback resolution per referenced id) — §6.4.
- Follow-up-level cues use only 3 ids: `radio_distress_beacon` (×14),
  `radio_static` (×6), `radio_vinyl_broadcast` (×1).

**Hint census (context only):** `outcome_hint` occurrences 82 (primary) + 71
(expansion) = 153 raw. PR2 authored 63 effective stage hints (37 primary + 26
expansion-effective) and deliberately left 28 stage positions on the 5 dead
primary-wins rows untouched. One legal omission exists on a live row:
`freq_distress_88_9` stage day 5 carries no hint (field absent — contract-legal;
`DISTRESS_SIGNAL_STAGE_CONTRACT.md` §2 makes hints optional). PR2's "fully
hinted across all stages" line is therefore slightly overstated but
contract-clean. No action; recorded for the closeout's accuracy.

## 2.3 What does NOT exist yet (grep-verified 2026-09-19)

1. The three semantic content rules — no `expired`-consequence rule, no
   trap-grammar identity rule, no follow-up count cap — anywhere in
   `CatalogIntegrityValidator.cs` (the follow-up block at `:2059-2137` contains
   only shape/grammar rules).
2. Any population-level replay over the *shipped* catalogs — the Wave 5
   harness (`DistressSignalTasks912ReplayTests.cs`) drives two synthetic
   fixture signals, not the 43 authored entries.
3. Any host-side cross-reference from distress-catalog cue ids to
   `AudioCueCatalog` — `src/Audio/AudioSelfTest.cs:43-96` validates the
   registry internally (coverage, resolution, stream load) but never reads the
   distress catalogs.
4. `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` (only the Wave 5 closeout and
   the PR2 tranche doc exist).
5. Any seal row for this package in `INTEGRATION_PLANS.md` / claim row in
   `WORKTREE_OWNERSHIP.md`.

## 2.4 Governance state

- `INTEGRATION_PLANS.md` rows for Waves 1–5 (lines ~194–199): all DONE/HANDED_OFF.
- `INTEGRATION_PLANS.md` "Next waves" line ~208: "PR 2 content tranche — DONE
  2026-09-15 … Remaining deferred: follow-up payload content, authored
  audio_cue content." — **STALE**: both content classes have since landed
  (commits `660cb595`/`8f72ee62`). The PR3 closeout + ledger row must correct
  this line (finding F4).
- `WORKTREE_OWNERSHIP.md` rows 42–46: the five wave claims, closed. No active
  claim covers this package's paths. No conflicting claim found for
  `CatalogIntegrityValidator.cs`, the two distress JSONs,
  `Ashfall.Core.Tests/Radio/`, `src/Audio/AudioSelfTest.cs`, or `docs/radio/`.

---

# 3. Required Delta

| # | Delta | Type | Owner layer |
|---|---|---|---|
| D1 | Validator rule `distress_followup_expired_requires_consequence` | additive Core validation | `CatalogIntegrityValidator.cs` |
| D2 | Validator rule `distress_followup_trap_only_on_lures` | additive Core validation | `CatalogIntegrityValidator.cs` |
| D3 | Validator rule `distress_followup_max_two` | additive Core validation | `CatalogIntegrityValidator.cs` |
| D4 | Data remediation: remove 3 shipped entries that violate D1/D3 (§11) | data edit, same commit as the rules | `radio_distress_signals.json` |
| D5 | Population replay test over the shipped catalogs through the real V6 codec | new focused test file | `Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs` |
| D6 | Validator-rule unit cases (fixtures + real-catalog pin) | extend existing test file | `Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs` |
| D7 | Distress cue-registry cross-reference section | additive host selftest section | `src/Audio/AudioSelfTest.cs` |
| D8 | Utilization seal run + recorded output | verification only (scanner already covers both catalogs) | — |
| D9 | PR3 closeout document | new doc | `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` |
| D10 | Ledger row + stale-line correction; claim open/close rows | governance (integrator for the ledger row) | `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md` |

Everything not listed here is out of scope (§22). No save-version change, no
schema addition, no runtime behavior change.

---

# 4. Evidence

## 4.1 Mechanism evidence (file:line, re-read 2026-09-19)

- Trigger grammar values: `DistressFollowUpScheduler.cs:15-36` —
  `Answered = "answered"`, `RescueSuccess = "rescue_success"`,
  `RescueFailed = "rescue_failed"`, `Expired = "expired"`,
  `AmbushEncountered = "ambush_encountered"`; `All[]` + `IsValid` (ordinal-
  ignore-case) at `:38-50`.
- Trigger production mapping: `DistressFollowUpScheduler.cs:214-243` —
  `Dispatched→answered`, `TerminalRescued→rescue_success`,
  `TerminalAmbush→ambush_encountered`, `TerminalFailed` → `rescue_failed` when
  `ArrivalResolved` else `expired`; `OnIgnoreConsequence` → `expired`.
- Expiry paths: `DistressRescueMissionManager.cs:594-638` (`TickDaily`):
  consequence-bearing missions (`IgnoreConsequenceTokens.Count > 0`) →
  `Expired = true` + `ApplyIgnoreConsequence` (+ `RecordIgnored` when
  `!mission.IsTrap`); legacy path (no tokens) → `TerminalFailed` (+
  `RecordIgnored` when `!mission.IsTrap`) + `OnStageChanged`. **Both paths
  reach the scheduler's `expired` trigger; trap-class missions are excluded
  only from the trust delta, not from the stage event.**
- `ApplyIgnoreConsequence` idempotence and token application:
  `DistressRescueMissionManager.cs:707-727`; closed token vocabulary
  `{sender_death, faction_standing_loss, faction_ambush}` at `:204`.
- Trap classification: `RadioDistressSystem.cs:221-226` — `IsTrapOrDeception`
  ⇔ `authenticity ∈ {trap, false_flag, bait_trap}` (case-insensitive) ∨
  `outcome_type == bait_trap` ∨ `deceptive_faction_id` non-empty.
  `IsGenuineRescue` at `:239-246`; hardcoded `IsAutomated` id list at
  `:230-236` (freq_distress_392_7 / 512_4 / 623_8 / 701_3).
- Deadline/consequence DTO dual binding: `RadioDistressSystem.cs:103-114`
  (`deadline_days` snake, `deadlineDays` camel; **fallback
  `DeadlineDays ⇒ DaysToTrace` when neither is authored**), `:129-140`
  (`ignore_consequence` / `ignoreConsequence`, default empty). The DTO
  `IgnoreConsequence` string has **no Core runtime consumer** (grep:
  definition-only at `RadioDistressSystem.cs:136`); consequence tokens live on
  the mission fixtures. The validator rules therefore treat the JSON fields as
  *authored declarations of expirability*, not as runtime-wired state.
- Audio resolution precedence: `DistressAudioCueResolver.cs:36-63`.
- Follow-up cue field: `DistressFollowUpScheduler.cs:86-92`
  (`SignalFollowUpDefinition.AudioCue`).
- V6 codec: `RadioSave.cs:170-186` (`signalFollowUps` field),
  `RadioSaveCodec.CurrentSaveVersion = 6`, `MigrateV5` at `:449-480`,
  `EnsureCollections` at `:482-503`.
- Host wiring: `RadioHostSession.cs:115-121` (manager + scheduler
  construction and event binding), `:121-128` (fired-follow-up presentation +
  cue), `:170-184` (expansion-first/primary-last load order), `:258`
  (`RecordSignalHeard` on intercept), `:283-296` (Intercept-gated playback +
  dedupe), `:567-568` (sorted persisted played keys), `:610+` (restore).
- Expedition bridge: `src/Main.Expeditions.cs:611`
  (`RecordExpeditionDispatched`), `:636` (`RecordDestinationReached`).
- Cue registry constants: `src/Audio/AudioCueCatalog.cs:229-237`
  (`RadioStatic`, `RadioMorse`, `RadioNumbersStation`, `RadioEbsAlert`,
  `RadioDeadHandPulse`, `RadioDistressBeacon`, `RadioVinylBroadcast`).
- Audio selftest internal gates: `src/Audio/AudioSelfTest.cs:43-96` (catalog
  coverage, per-cue resolution, stream load), dispatch at
  `src/Main.Application.cs:537-538`.
- Utilization coverage: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:77`
  (both distress catalogs in the known list), `:426-427` (consumer map →
  `SignalTriangulationSystem`); gate regression/orphan rules at
  `Assets/Ashfall.Core/Content/ContentUtilizationGate.cs:59-112`; runner at
  `src/Host/ContentUtilizationSelfTest.cs:15-86`, dispatch at
  `src/Main.Application.cs:564-565`.
- Existing replay harness pattern to extend (public-API mission injection via
  `CaptureState`/`RestoreState` + `RefreshFingerprint`):
  `Ashfall.Core.Tests/Radio/DistressSignalTasks912ReplayTests.cs:87-120`.

## 4.2 Contract documents (sealed authorities this plan must not contradict)

`DISTRESS_SIGNAL_STAGE_CONTRACT.md` (Wave 1), `SIGNAL_TRUST_CONTRACT.md`
(Wave 2, §5 retirement note), `DISTRESS_SIGNAL_FOLLOWUP_CONTRACT.md` (Wave 3 —
its §3 table already names `ambush_encountered`),
`DISTRESS_SIGNAL_AUDIO_CONTRACT.md` (Wave 4 — §6: the cue registry is
host-side; Core must not reference it), `DISTRESS_SIGNAL_TASKS_9_12_CLOSEOUT.md`
(Wave 5), `DISTRESS_SIGNAL_TASKS_9_12_SAVE_CONTRACT.md`,
`DISTRESS_SIGNAL_TASKS_9_12_TEST_MATRIX.md`,
`DISTRESS_SIGNAL_DEAD_DATA_REGISTER.md` (5 pinned warnings; "a delta is a new
finding"), `DISTRESS_SIGNAL_PR2_HINT_TRANCHE.md` (PR2; its "still deferred"
list is the pre-content-landing snapshot).

## 4.3 Program requirements (binding scope)

From the execution program §C.1.2: the three validator rules (data-shape only,
no runtime behavior change); population replay "for each of the 24 authored
follow-up entries, drive its trigger transition and assert the fire day, the
exactly-once ledger, and the resolved audio cue id (or explicit text-only)";
audio-cue registry verification per the existing fallback policy; utilization
seal 0 orphans; PR3 closeout + ledger row routed through the ledger owner or
an authorized transfer. §C.1.6 failure-mode rule: "A validator rule that fires
on currently-shipped rows … resolve by fixing the data or amending the rule's
boundary with a documented reason, never by deleting the rule." §C.1.10: rule
ids, error shapes, fixture cases, and the ordering note (semantic rules run
after the existing structural validation). §C.1.12: the eight mandated
closeout contents. This plan implements those requirements; where current
evidence refines them, §4.4 says so explicitly.

## 4.4 Evidence refinements and contradictions of the briefing/program

| # | Claim (source) | Current evidence | Resolution in this plan |
|---|---|---|---|
| R1 | "for each of the 24 authored follow-up **entries**" (program §C.1.2) | 24 = signals carrying follow-ups; entries = **43** (30+13) | Plan uses 43-entry census (§2.2); replay drives all post-remediation entries (40) |
| R2 | Trigger "`trap_fallen_for`" (program §C.1.1/§C.1.10; briefing) | Grammar value is **`ambush_encountered`** (`DistressFollowUpScheduler.cs:27-30`); `trap_fallen_for` survives only inside authored follow-up *id strings* (`fu_*_trap_fallen_for`) | Rules are specified against `ambush_encountered`; id-string naming noted as legacy cosmetics, not renamed (no content churn) |
| R3 | PR2 closeout: "Follow-up payload content — mechanism live, no authored content"; `INTEGRATION_PLANS.md` ~208 same | 43 entries + 21 cued entries shipped in `660cb595`/`8f72ee62` | This package *is* the seal for that content; ledger correction in D10 |
| R4 | "Max 2 follow-ups per parent signal" (origin docs) vs current data | `freq_distress_812_5` and `freq_distress_867_9` ship **3** entries each | Rule D3 fires on shipped rows → remediation per §11 (anticipated by program §C.1.6) |
| R5 | (implicit) expired follow-ups are wired wherever authored | `fu_55_1_expired` sits on a signal with **no `deadlineDays`, no `ignoreConsequence`, no mission** — it can never fire | Rule D1 catches it; removal per §11 |
| R6 | (implicit) all shipped follow-ups are runtime-reachable | 21/43 entries sit on signals with no registered mission (F1) | Out of scope to fix (needs data/architecture authority); documented as decision-needed in PR3 closeout; replay proves content-mechanism correctness via injected harness missions |
| R7 | PR2: "43/43 unique effective identities now fully hinted across all stages" | `freq_distress_88_9` day-5 stage legally omits the hint | Contract-legal; recorded for closeout accuracy; no action |
| R8 | Dead-data register pins "333 catalogs"; Wave 5 closeout pins "325" | Catalog count grows between waves; the pin that matters for this package is **0 errors + exactly 5 named warnings** | DoD states error/warning pins, not the catalog total |

**Premise-audit verdict: READY.** No dependency is blocked; no claim overlap;
the save/determinism surface is unchanged; every gap above is re-verified at
HEAD.

---

# 5. Existing Extension Seams

Every delta lands on an existing, owned seam. No new system is created.

| Delta | Seam being extended | Why it is the right seam |
|---|---|---|
| D1–D3 rules | `ValidateDistressSignalStagesFile` follow-up loop (`CatalogIntegrityValidator.cs:2059-2137`) and the per-broadcast scope | The Wave 1/3/4 distress rules already live here; the new rules are the same class of authored-content check, run in the same pass, with the same error-format conventions (`{file}:{signalId}.follow_up_signals[{i}] '{id}': …`) |
| D4 data fix | The two authoritative catalogs | JSON is the data authority; removals are content-authority edits recorded in the closeout |
| D5 replay | `DistressSignalTasks912ReplayTests.cs` harness pattern (World fixture, public capture/restore mission injection, day-level trace + `StableHash` fingerprint) | Wave 5 sealed this harness shape; the population replay generalizes it from 2 fixture signals to the shipped corpus without touching the sealed file |
| D6 validator cases | `Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs` | Its Wave 3 brief already includes "validator rules" (contract §8); TEST_POLICY: extend an existing file when diagnostics stay clear |
| D7 registry check | `src/Audio/AudioSelfTest.cs` (new section after "Cue Catalog Coverage") | The audio contract §6 assigns semantic cue resolution to the host audio selftest; `AudioSelfTest` already gates registry resolution and runs in CI headless |
| D8 utilization | `--content-utilization-selftest` (unchanged) | Both catalogs are already registered (`ContentUtilizationScanner.cs:77,426-427`); the gate already fails on new orphans/regressions |
| D9 closeout | `docs/radio/` contract-doc family (Wave 5 closeout as structural precedent) | Program §C.1.12 mandates mirroring it |
| D10 ledger | `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md` | Integrator-owned seams; builder only opens/closes its own claim row |

Deliberately **not** used: `SignalTrustAvailability` (retired consumer — Wave 9
Part 2 Option B), any panel (`RadioPanel` strips are presentation-only and
already surface fired follow-ups via the session event line), any new save
section, any new selftest action (avoids `HostCli.SelfTestManifest.cs` churn).

---

# 6. Proposed Architecture

## 6.1 Rule D1 — `distress_followup_expired_requires_consequence`

**Normative statement.** An `expired`-triggered follow-up narrates the
aftermath of an unanswered response window. It is therefore forbidden unless
the parent signal's expiry can actually carry a consequence:

1. the broadcast authors an **explicit positive response window** —
   `deadline_days` or `deadlineDays` present, integer, and `> 0`; and
2. the broadcast is **not trap-class** (mirror of `IsTrapOrDeception`,
   §6.5) — trap expiry is authored as *wisdom* and is trust-excluded
   (`DistressRescueMissionManager.cs:619-630`: `if (!mission.IsTrap)
   _signalTrust?.RecordIgnored(...)` on both expiry branches), so a trap
   `expired` follow-up would narrate a consequence-free path.

Rationale for requiring an *explicit* window: the DTO falls back
`DeadlineDays ⇒ DaysToTrace` (`RadioDistressSystem.cs:110-114`), so field
absence does not mean "no deadline" to the rescue-mission mechanic — but an
`expired` follow-up is a *consequence declaration*, and the origin rule
("`expired` follow-ups forbidden on no-consequence signals") targets rows that
declare no response semantics of their own. The `−2` ignored trust delta then
supplies the guaranteed consequence for every non-trap windowed signal (both
expiry branches record it), satisfying the program's "a trust/standing delta
is already implied by the grammar" clause without inventing a new consequence
field.

**Pseudocode (per follow-up entry, after the Wave 3 grammar check passes):**

```csharp
// inside the follow_up_signals loop, after SignalFollowUpTriggers.IsValid(trigger)
if (string.Equals(trigger, "expired", StringComparison.OrdinalIgnoreCase))
{
    bool hasWindow = TryReadIntEither(broadcast, "deadline_days", "deadlineDays", out int window)
                     && window > 0;
    if (!hasWindow)
    {
        report.Error($"{followUpPath} '{followUpId}': trigger=expired but the signal authors " +
            "no positive response window (deadline_days/deadlineDays) — expiry cannot carry a " +
            "consequence; expired follow-ups on no-consequence signals are forbidden " +
            "[distress_followup_expired_requires_consequence]");
    }
    else if (IsTrapClassBroadcast(broadcast))
    {
        report.Error($"{followUpPath} '{followUpId}': trigger=expired on trap-class signal — " +
            "letting a lure expire is authored as no-consequence wisdom (trust-excluded); " +
            "expired follow-ups on trap-class signals are forbidden " +
            "[distress_followup_expired_requires_consequence]");
    }
}
```

**Edge cases (each becomes a test row in §18):**

- `deadline_days` absent *and* `deadlineDays` absent → no window → error.
- Present but `0` or negative → no *positive* window → error.
- Present but non-integer (string/float) → treated as no window → error (the
  message still names the missing positive window; no separate shape rule is
  added in this package — candidate noted in §22).
- Both casings present → snake wins (mirrors `DeadlineDaysSnake ??
  DeadlineDaysCamel`); tested.
- Trap-class with a positive window (e.g. a lure with `deadlineDays: 5`) →
  second branch error.
- Non-trap with positive window and empty `ignoreConsequence` → **legal**
  (Family-Shelter legacy-expiry precedent: the trust delta is the
  consequence).
- Automated/stale/narrative signals (no window authored) → caught by branch 1.
- Trigger casing `EXPIRED`/`Expired` → matched (OrdinalIgnoreCase, consistent
  with `SignalFollowUpTriggers.IsValid` and the scheduler).

**Current-data verdict:** fires exactly once — `fu_55_1_expired`
(freq_distress_55_1: no window, no consequence, narrative class). Remediation
§11. `fu_812_5_expired` and `fu_867_9_expired` pass (window 4, genuine).

## 6.2 Rule D2 — `distress_followup_trap_only_on_lures`

**Normative statement.** Trap-grammar follow-ups (`ambush_encountered`) may
attach only to trap-class identities. A genuine-only signal can never resolve
`TerminalAmbush`, so trap aftermath on a genuine identity is dead, misleading
content (origin: "trap-grammar triggers forbidden on genuine-never-hostile
identities").

```csharp
if (string.Equals(trigger, "ambush_encountered", StringComparison.OrdinalIgnoreCase)
    && !IsTrapClassBroadcast(broadcast))
{
    report.Error($"{followUpPath} '{followUpId}': trigger=ambush_encountered (trap grammar) on a " +
        "signal whose identity is not trap-class (authenticity/outcome_type/deceptive_faction_id) " +
        "— trap grammar is forbidden on genuine-only identities " +
        "[distress_followup_trap_only_on_lures]");
}
```

**Edge cases:** authenticity casing (`"Trap"`, `"FALSE_FLAG"`) → matched
case-insensitively (mirror); `outcome_type: "bait_trap"` with empty
authenticity → trap-class (mirror); `deceptive_faction_id` non-empty →
trap-class (mirror); genuine by `authenticity: "genuine"` or by
`IsGenuineRescue` signals (moral-choice/sender-faction/survivor-*) without any
trap marker → error if carrying the trap trigger. Reciprocal direction
(trap-class signal carrying `rescue_success`) stays **legal**: the contract
seals that such entries simply never fire from the trap path
(`DISTRESS_SIGNAL_FOLLOWUP_CONTRACT.md` §3; Scenario D proves suppression) —
grammar reachability is not identity confusion.

**Current-data verdict:** clean — all 7 `ambush_encountered` entries sit on
trap-class rows (148_2/192_4/410_7/288_1 = `trap`; 333_6/478_2 = `false_flag`;
380_2 = `false_flag`). The rule is preventive at zero remediation cost.

## 6.3 Rule D3 — `distress_followup_max_two`

**Normative statement.** At most 2 `follow_up_signals` entries per signal
definition (origin: "Max 2 follow-ups per parent signal", restated as the
content cap in both predecessor plans; anti-fatigue + trust-pump bounding:
exactly-once ledgers already cap each entry at one fire per campaign, and the
count cap bounds per-signal answer-chaining texture).

```csharp
// per broadcast, before the per-entry loop
if (broadcast.TryGetProperty("follow_up_signals", out var followUps)
    && followUps.ValueKind == JsonValueKind.Array)
{
    int count = 0;
    foreach (var _ in followUps.EnumerateArray()) count++;
    if (count > MaxFollowUpsPerSignal) // const int MaxFollowUpsPerSignal = 2;
        report.Error($"{fileName}:{signalId}.follow_up_signals: count={count} exceeds the " +
            $"maximum of {MaxFollowUpsPerSignal} authored follow-ups per signal " +
            "[distress_followup_max_two]");
}
```

**Edge cases:** counts *all* entries regardless of trigger, reachability, or
primary-wins shadowing (corpus-uniform, matching the existing duplicate-id
pass); empty array → count 0 → legal (and the existing block simply skips);
the cap is on *authored entries*, not on fires (firing is already
exactly-once per entry).

**Current-data verdict:** fires twice — `freq_distress_812_5` (3) and
`freq_distress_867_9` (3). Remediation §11.

## 6.4 Audio-cue registry verification (D7)

Permanent, host-side, additive section in `src/Audio/AudioSelfTest.cs` (after
the "Cue Catalog Coverage" block), honoring the audio contract §6 boundary
(Core never references the registry; the registry check never lives in Core):

```text
[AudioSelfTest] --- Distress Catalog Cue Cross-Reference ---
  load radio_distress_signals.json + radio_distress_signals_expansion.json
    from the data dir (read-only; same repo-root resolution pattern as
    ContentUtilizationSelfTest)
  collect every non-empty audio_cue at signal / message_fragments[] /
    follow_up_signals[] level, with its source path
  for each distinct id:
    Check(AudioCueCatalog.Contains(id))            — membership
    Check(resource file exists OR valid fallback)  — resolution, mirroring
                                                     section 1's policy
  print the text-only summary (signals/fragments/follow-ups with empty cue)
  failure lines name catalog:signalId:level:index:cueId
```

Expected current result: 7 distinct ids, all members
(`AudioCueCatalog.cs:229-237`), resolution per the section-1 policy; 22
text-only follow-ups + uncued fragments reported as the text-only set. A
referenced-but-unregistered id is a **FAIL** (authoring error), distinct from
the runtime missing-cue fallback (which remains: log once, text continues,
never crash — the fallback is a runtime safety net, not an authoring license).

## 6.5 Shared validator helpers (additive, private, inside `CatalogIntegrityValidator`)

```csharp
// Reads an integer authored under either casing; snake wins on conflict —
// mirrors DistressSignalDefinition.DeadlineDaysSnake/Camel binding order.
private static bool TryReadIntEither(JsonElement row, string snake, string camel, out int value)

// Mirrors RadioDistressSystem.IsTrapOrDeception (RadioDistressSystem.cs:221-226)
// on raw JSON — the validator must not construct runtime definitions:
//   authenticity ∈ {trap, false_flag, bait_trap} (OrdinalIgnoreCase)
//   ∨ outcome_type == bait_trap (OrdinalIgnoreCase)
//   ∨ deceptive_faction_id non-empty
private static bool IsTrapClassBroadcast(JsonElement broadcast)
```

Both helpers are pure, engine-free, culture-invariant, and live beside
`IsValidAudioCueId` (`CatalogIntegrityValidator.cs:2235`). **No new warning is
emitted by any rule** — errors only — so the dead-data register's pinned
5-warning count is untouched.

**Rule ordering (program §C.1.10 note):** D3 runs at broadcast scope before
the per-entry loop; D1/D2 run inside the loop *after* the existing Wave 3
structural checks for that entry (grammar, id uniqueness, delay, text,
clarity, hint shape, cue shape), so semantic rules never double-report a row
whose grammar already failed.

## 6.6 Population replay harness (D5) — full design walk-through

New file `Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs`.
It extends the sealed Wave 5 harness pattern; it does **not** modify
`DistressSignalTasks912ReplayTests.cs`.

**World construction (mirrors `RadioHostSession` load order exactly):**

```csharp
var distress = new RadioDistressSystem();
distress.LoadFromJson(File.ReadAllText(Path.Combine(dataDir,
    "radio_distress_signals_expansion.json")));   // expansion FIRST
distress.LoadFromJson(File.ReadAllText(Path.Combine(dataDir,
    "radio_distress_signals.json")));             // primary LAST (primary-wins)
var trust    = new SignalTrustLedger();
var missions = new DistressRescueMissionManager(null, distress, trust); // 12 authored fixtures
var followUps = new DistressFollowUpScheduler(distress, missions);
followUps.BindToMissionEvents();
```

`dataDir` resolution reuses the existing test-project convention for locating
`Assets/StreamingAssets/Data` (same repo-relative approach used by the
real-catalog cases in `DistressAudioCueTests`).

**Mission coverage for unmissioned signals (finding F1).** Twelve
follow-up-carrying signals have no authored mission. The harness registers
harness missions for exactly those, through the *public* capture/restore seam
— the same pattern the Wave 5 harness uses
(`DistressSignalTasks912ReplayTests.cs:99-120`), so no production API is
added:

```csharp
var state = missions.CaptureState();
foreach (var signalId in FollowUpCensus.SignalsWithoutMissions)
{
    var def = distress.GetDefinition(signalId)!;
    state.Missions.Add(new DistressRescueMission
    {
        QuestId           = "quest_replay_" + signalId,
        SignalId          = signalId,
        DestinationId     = "loc_replay_" + signalId,
        DaysToTrace       = def.DaysToTrace,
        DeadlineDays      = Math.Max(1, def.DeadlineDays),
        SenderSurvivalDays = def.SenderSurvivalDays,
        IsTrap            = def.IsTrapOrDeception,
        IgnoreConsequenceTokens = ConsequenceTokensFor(def), // sender_death when JSON declares it
        SenderAlive       = true
    });
}
state.RefreshFingerprint();
missions.RestoreState(state);
```

This is a *test harness* accommodation, recorded as such: it proves the
authored content fires correctly through the mechanism when a mission exists.
It does not assert production reachability — that gap is F1, routed to the
closeout.

**Census table in code.** A static readonly table mirrors §2.2's
post-remediation state (40 entries × {signalId, followUpId, trigger, delay,
expectedCue-or-empty}). A census-drift guard test reloads the real catalogs
and asserts: 24 follow-up-carrying signals, 40 entries, class counts
(answered 15 / rescue_success 16 / ambush_encountered 7 / expired 2), and
exact table equality — any future content edit fails loudly with the row that
drifted (aggregation-compliant per-row messages).

**Drive patterns per trigger class** (campaign-day only; `SetDay` before
mission tick, tick order `Distress.TickDaily → Missions.TickDaily →
FollowUps.TickDaily`, mirroring the host contract):

| Class | Drive | Expected |
|---|---|---|
| answered | day 1 `Intercept` + `RecordSignalHeard`; day D `RecordExpeditionDispatched` | pending due D+delay; fires on first tick ≥ due; trust +2 once |
| rescue_success | dispatch, then `RecordDestinationReached` at a day ≤ deadline **and** ≤ senderDeathDay (survival-model missions) | `TerminalRescued`; due arrivalDay+delay; trust +5 once (total +7 with the answer) |
| expired (consequence) | never dispatch; tick to expiryDay on a token-bearing mission | `ApplyIgnoreConsequence` → due expiryDay+delay; trust −2 once |
| expired (legacy) | same on a token-less mission (445_2 pattern) | `TerminalFailed` (no arrival) → same trigger; trust −2 once |
| ambush_encountered | dispatch into `IsTrap` mission; `RecordDestinationReached` | `TerminalAmbush`; due arrivalDay+delay; trust −5 (net −3 with the answer); no other trigger class schedules for that signal |

**Per-entry assertions (all 40):**

1. Fire day == trigger-event day + authored `delay_days` (exact).
2. Fired-key ledger contains `parent:followUpId` exactly once; re-ticking the
   due day and ticking beyond never re-fires.
3. Save (real `RadioSaveCodec.Encode`) between schedule and due, restore into
   a fresh world, tick → fires exactly once, same day, same payload
   (interrupted == continuous, the Scenario B property per entry).
4. `LastFired` / event payload `Text` equals the authored catalog text
   byte-for-byte (content-binding proof, not fixture text).
5. `DistressAudioCueResolver`-equivalent resolution of the follow-up payload
   equals the authored `audio_cue` (or empty ⇔ the entry is in the documented
   text-only set).
6. Trust deltas match policy per class; trap-class expiry (none authored
   post-remediation) and undiscovered signals produce zero trust movement.

**Population-level properties:**

- Same-day multi-fire ordering across parents is the ordinal
  `(dueDay, parentSignalId, followUpId)` sort (construct two harness missions
  whose follow-ups share a due day; assert order).
- Full-population fingerprint: day-level trace lines over days 1–30 across
  all 24 signals (stage index, cue id, trust score + four counters, pending
  keys, fired keys — the Wave 5 trace format), hashed with
  `Ashfall.Core.StableHash`; **two consecutive runs produce identical
  fingerprints** (the program's determinism guard for the harness itself).
- One mid-lifecycle save/load over the whole population produces a
  byte-identical tail trace (population Scenario B).

`rescue_failed` has **0 authored entries**; it remains grammar-only content
and stays covered by the existing mechanism tests
(`DistressFollowUpTests`). The population replay asserts that invariant
(census guard: no `rescue_failed` row exists) rather than fabricating content.

## 6.7 Utilization seal (D8)

Run `--content-utilization-selftest`; record the distress-relevant output:
both catalogs remain `GAMEPLAY_CONSUMED` (consumer
`SignalTriangulationSystem`), 0 orphans, 0 regressions vs baseline. No scanner
or baseline edit — the catalogs were registered before this package. If the
gate reports a distress-layer orphan, that is a **stop-and-report** finding
(Rule 10), not something to fix silently.

## 6.8 Collision check (mandatory before design — results)

- Rules: no existing expired-consequence / trap-identity / count rule
  (grep-verified); the checks extend the one existing distress validator pass.
- Replay: `DistressSignalTasks912ReplayTests` (fixtures, 5 cases) is not
  duplicated — D5 is population-over-shipped-content, a disjoint brief; both
  files coexist.
- Registry check: `AudioSelfTest` section 1 validates the registry internally;
  no distress-catalog consumer exists yet — D7 is additive, not a second
  audio system (the AudioManager path is reused untouched).
- No alternate "content seal" mechanism exists under another name; the
  content-utilization gate is catalog-granular and does not subsume the
  per-entry replay.
- Current JSON can express everything needed; **no additive field or schema
  change is required anywhere** (removals only).

---

# 7. Ownership Matrix

| Concern | Owner (file) | This package |
|---|---|---|
| Follow-up scheduling / exactly-once / fired ledger | `Assets/Ashfall.Core/Radio/DistressFollowUpScheduler.cs` | unchanged |
| Stage selection | `Assets/Ashfall.Core/Radio/DistressStageResolver.cs` | unchanged |
| Audio cue resolution (Core, pure) | `Assets/Ashfall.Core/Radio/DistressAudioCueResolver.cs` | unchanged |
| Trust policy + ledger | `Assets/Ashfall.Core/Radio/SignalTrustLedger.cs` | unchanged (consumed by replay assertions only) |
| Mission lifecycle events | `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs` | unchanged |
| Radio persistence (V6) | `Assets/Ashfall.Core/Radio/RadioSave.cs` | unchanged — **no version bump** |
| Catalog validation | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | **extended (D1–D3 + 2 helpers)** — the single validation authority for the distress catalogs |
| Authored content | `Assets/StreamingAssets/Data/radio_distress_signals.json` (+ `_expansion.json`) | **remediated (D4)** — remains the data authority |
| Cue registry + audio selftest | `src/Audio/AudioCueCatalog.cs` / `src/Audio/AudioSelfTest.cs` | registry unchanged; selftest **extended (D7)** |
| Playback dedupe / host presentation | `src/Host/RadioHostSession.cs` | unchanged |
| Content utilization | `Assets/Ashfall.Core/Content/*` | unchanged (run-only) |
| Seal record | `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` | **new (D9)** |
| Governance ledger | `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md` | ledger row by integrator/authorized transfer (D10) |

Ambiguity check: the JSON `ignoreConsequence` declaration vs. mission-fixture
`IgnoreConsequenceTokens` disagree for `freq_distress_445_2` (JSON declares
`sender_death`; the fixture deliberately ships no tokens — "Family Shelter —
no authored consequence: legacy expiry semantics",
`DistressRescueMissionManager.cs:864-865`). Runtime authority is the fixture;
the JSON field has no Core consumer. This pre-existing dual declaration is
**not** reconciled in this package (it is presentation-adjacent and changing
either side is a runtime/data-authority decision); it is recorded in the PR3
closeout as a candidate future consistency rule. Flagged here per the
ownership-ambiguity rule rather than left vague.

---

# 8. Data Flow

**Author-time gate (changed by this package):**

```text
author edits radio_distress_signals*.json
  → CatalogIntegrityValidator.Validate (data-integrity selftest / CI)
      → Wave 1 stage rules → Wave 3 follow-up shape/grammar rules
      → Wave 4 structural cue rule
      → NEW D3 count cap (per broadcast)
      → NEW D1/D2 semantic rules (per entry, after grammar passes)
  → 0 errors + exactly 5 pinned primary-wins warnings ⇒ PASS
```

**Runtime flow (unchanged — asserted, not modified, by the replay):**

```text
catalogs → RadioDistressSystem (expansion first, primary last)
  → tuner Intercept edge → RecordSignalHeard → host plays resolved cue (dedupe)
  → expedition bridge → RecordExpeditionDispatched / RecordDestinationReached
  → mission TickDaily expiry paths
  → DistressFollowUpScheduler (SetDay → schedule → TickDaily → OnFollowUpFired)
  → host: event line + signal log + follow-up cue (played once)
  → SignalTrustLedger deltas at the guarded transitions
  → RadioSaveCodec V6 capture/restore
```

**Verification-time flow (changed by this package):**

```text
--audio-selftest
  → existing registry-internal gates
  → NEW: distress catalogs × AudioCueCatalog cross-reference (D7)
--content-utilization-selftest
  → scanner (both catalogs registered) → gate vs baseline → 0 orphans (D8)
dotnet test (focused)
  → D6 validator fixtures + real-catalog pin
  → D5 population replay through the real V6 codec
```

---

# 9. State Model

No state is added, renamed, or removed. The complete mutable state of the
distress follow-up/audio area remains:

| State | Shape | Owner | Change |
|---|---|---|---|
| Pending follow-ups | `SignalFollowUpSaveState.pending` (parent, followUpId, dueDay — sorted) | `DistressFollowUpScheduler` | none |
| Fired ledger | `SignalFollowUpSaveState.firedKeys` (sorted) | `DistressFollowUpScheduler` | none |
| Trust ledger | `SignalTrustSaveEntry` (4 counters, score, 4 id lists) | `SignalTrustLedger` | none |
| Mission states | `DistressMissionSaveState` (+ fingerprint) | `DistressRescueMissionManager` | none |
| Playback dedupe | `RadioSaveState.playedBroadcastKeys` (`distress:{id}:{cue}`) | `RadioHostSession` | none |
| Scheduler `CurrentDay` | transient, host-set, never persisted | `DistressFollowUpScheduler` | none |
| Stage index / resolved cue | **derived** (definition + campaign day), never persisted | resolvers | none |

`RadioSaveState.saveVersion` stays **6**; `RadioSaveStateFrozenV1..V5` stay
byte-frozen; `MigrateV1..MigrateV5` untouched. The data remediation (D4)
removes *authored content*, not saved state: pending/fired ledgers key on
follow-up ids, and the scheduler already expires pending entries whose
definition vanished ("definition removed — pending entry expires silently",
`DistressFollowUpScheduler.cs:284-285`), so a pre-fix save carrying
`fu_55_1_expired` / `fu_812_5_answered` / `fu_867_9_answered` in `pending` or
`firedKeys` loads and continues without error: restored pending entries for
the removed ids no-op at fire time; fired keys simply never recur. This
graceful-degradation path is asserted by a dedicated replay case (§18 T-17).

---

# 10. API/Contracts

**No public API is added, changed, or removed.** The package's contracts are:

1. **Validator rule contracts (new, data-shape only):** rule ids
   `distress_followup_expired_requires_consequence`,
   `distress_followup_trap_only_on_lures`, `distress_followup_max_two`;
   error messages follow the house format `{catalog}:{signalId}.
   follow_up_signals[{i}] '{followUpId}': <field/value> — <rule explanation>
   [<rule_id>]` (and the per-broadcast variant for D3). Rules are error-level
   only. Ordering: D3 at broadcast scope; D1/D2 per entry after Wave 3
   structural checks.
2. **Trigger-grammar contract (unchanged, re-pinned):** the closed set of five
   values; the rules reference `ambush_encountered` as the trap-grammar value
   and document that authored id *strings* containing `trap_fallen_for` are
   legacy cosmetics with no semantic load.
3. **Replay harness contract (test-side):** population census drift guard +
   per-entry fire-day / exactly-once / codec / cue / trust assertions +
   two-run fingerprint stability. The harness uses public Core APIs only
   (`LoadFromJson`, `CaptureState`/`RestoreState` + `RefreshFingerprint`,
   `RecordSignalHeard`, `RecordExpeditionDispatched`,
   `RecordDestinationReached`, `TickDaily`, `SetDay`,
   `RadioSaveCodec.Encode/TryDecode`, `StableHash`).
4. **Audio cross-reference contract (host-side):** every distress-catalog
   cue id resolves in `AudioCueCatalog` (membership + resource/fallback) or
   the entry is empty (documented text-only). Failure is an authoring error,
   not a runtime branch.
5. **Seal-record contract:** the PR3 closeout contains the eight mandated
   sections (program §C.1.12) in the Wave 5 closeout's section order.

---

# 11. Data Changes

Additive fields: **none.** Schema: **unchanged.** The only data edits are
three entry removals from `Assets/StreamingAssets/Data/radio_distress_signals.json`,
applied **in the same commit as the rules that require them** (house rule:
"Shipped-row remediation … is a data fix in the same commit as the rule —
never a rule relaxation").

## 11.1 R1 — remove `fu_55_1_expired` (violates D1)

`freq_distress_55_1` ("The Pianist's Last Broadcast", `outcome_type:
narrative`) authors no `deadlineDays`/`deadline_days`, no `ignoreConsequence`,
no moral-choice link, and has no registered mission — the entry is unreachable
dead content *and* consequence-free. Remove exactly this object from its
`follow_up_signals` array:

```json
{
  "id": "fu_55_1_expired",
  "trigger_condition": "expired",
  "delay_days": 3,
  "clarity": 0.75,
  "text": "Total silence on 55.1 MHz. The concert hall frequency has fallen permanently dark.",
  "audio_cue": "radio_static"
}
```

Remaining on the signal: `fu_55_1_answered` (kept; its own reachability is the
F1 finding, not a D1–D3 violation). The removal is runtime-invisible today (no
mission → the entry could never fire) and the narrative beat it carried is
already expressed by the signal's own late-stage fragment text.

## 11.2 R2 — remove `fu_812_5_answered` (violates D3; recommended option)

`freq_distress_812_5` ships 3 entries. Remove:

```json
{
  "id": "fu_812_5_answered",
  "trigger_condition": "answered",
  "delay_days": 2,
  "clarity": 0.85,
  "text": "Petar's voice through static: 'I hear footsteps in the hallway outside the gym. I am tapping on the radiator pipe.'",
  "audio_cue": "radio_distress_beacon"
}
```

## 11.3 R3 — remove `fu_867_9_answered` (violates D3; recommended option)

`freq_distress_867_9` ships 3 entries. Remove:

```json
{
  "id": "fu_867_9_answered",
  "trigger_condition": "answered",
  "delay_days": 2,
  "clarity": 0.85,
  "text": "Ana whispers on 867.9: 'We heard three knocks on the basement pipe. We are staying behind the steel locker.'",
  "audio_cue": "radio_distress_beacon"
}
```

## 11.4 Remediation decision record (for the closeout)

For both D3 violators the choice is which single entry to drop:

| Option | Keeps | Drops | Verdict |
|---|---|---|---|
| **A** | answered + rescue_success | expired | Rejected — zeroes the `expired` class's authored coverage (55_1's entry is already removed by D1), weakening the class seal and the replay's class matrix |
| **B (recommended)** | rescue_success + expired | answered | **Recommended** — preserves trigger-class diversity (both terminal poles); the answered mid-mission texture is the most redundant of the three (the rescue_success entry at delay 4 already narrates the aftermath in the same voice) |
| C | (amend rule to max-3) | — | Rejected — contradicts the cap stated in both origin documents without a new authority signature |

Option B leaves both child_voice signals narratively coherent: the stage
fragments carry the in-crisis voice; the two retained follow-ups narrate the
two endings. All three removals are unreachable at runtime today (F1), so no
player-observable behavior changes; the edit is data hygiene ahead of any
future mission registration, recorded with before/after in the PR3 closeout.
If the content authority prefers Option A at execution time, D3's
implementation is unchanged — only the removed ids and the replay census pins
swap (§19 P1/P2 coupling note).

**Post-remediation census (pinned by the replay drift guard):** 24 signals,
**40 entries** — answered ×15, rescue_success ×16, ambush_encountered ×7,
expired ×2; `follow_up_signals` block count unchanged at 24; audio_cue
occurrences become 64 (primary) + 36 (expansion) = 100.

---

# 12. Save/Load

- **No save-version bump.** `RadioSaveCodec.CurrentSaveVersion` remains 6; no
  frozen shape is added or touched; `MigrateV5` and below are untouched.
- The replay exercises the *real* codec: `RadioSaveCodec.Encode` →
  `TryDecode` → restore into fresh instances, mid-lifecycle (between schedule
  and due) and population-wide. Expected properties (already sealed at
  mechanism level; now proven per authored entry):
  - pending entries restore with absolute due days; firing after restore is
    exactly-once (persisted fired ledger);
  - a restored save whose catalog no longer contains the follow-up definition
    (the three removed ids) expires the pending entry silently at fire time —
    no crash, no event, no error (T-17);
  - interrupted == continuous traces, byte-for-byte, including the save
    payload fingerprint (Scenario B property generalized);
  - tampered payloads still hard-reject (`TryDecode` false) — covered by
    existing `RadioSaveCodecTests` / `RadioSaveMigrationTests`, not re-run
    here beyond the focused Radio directory pass.
- Migration behavior for old saves is unchanged (V1–V5 → V6 paths frozen);
  the removals are catalog data, not save data.

---

# 13. Determinism

- The delta introduces **zero RNG**: validators are pure structural checks;
  the replay harness drives fixed campaign-day schedules; the audio
  cross-reference enumerates static JSON.
- No `System.Random`, no wall-clock, no hash-iteration-order dependence:
  scheduler order is the documented ordinal sort; capture sorts pending/fired
  lists; trace lines use `StableHash` and `CultureInfo.InvariantCulture`
  formatting (mirroring the Wave 5 harness).
- The two-run fingerprint gate (§6.6) is the harness's own determinism proof;
  any nondeterminism is a defect to surface, never a flake to pin (program
  §C.1.6).
- Trust arithmetic is integer-only and clamped; replay assertions use exact
  integer equality (e.g. neutral 50 → 52 after one answer; → 57 after the
  rescue; → 48 after an ignore; → 47 net on the trap path).

---

# 14. System/Event Wiring

No wiring changes. The replay and the validators consume the existing wiring:

- `DistressRescueMissionManager.OnStageChanged` →
  `DistressFollowUpScheduler.OnMissionStageChanged` (Dispatched /
  TerminalRescued / TerminalAmbush / TerminalFailed with the ArrivalResolved
  split).
- `DistressRescueMissionManager.OnIgnoreConsequence` →
  `DistressFollowUpScheduler.OnIgnoreConsequence` (expired).
- Host order contract (asserted in the harness): `FollowUps.SetDay(day)`
  **before** mission ticks; `FollowUps.TickDaily(day)` after — day-aware
  scheduling uses absolute due days, so restore never double-fires.
- `RadioHostSession` presentation of fired follow-ups (event line + cue) is
  untouched; panels are untouched.

---

# 15. Godot Integration

The only Godot-side (host) change is the additive `AudioSelfTest` section
(D7, §6.4): read-only catalog parsing, registry membership + resolution
checks, standard `Check(...)` accounting, no playback (headless
`PlayCueDef` is a no-op by existing design — the gate is structural:
membership + resolvable resource/fallback + dedupe-key policy documented).
Dispatch is unchanged (`src/Main.Application.cs:537-538`); no new CLI action,
so `HostCli.SelfTestManifest.cs` and the CLI catalog generator are untouched.
No `.tscn`, panel, or UI-tree change. If a Godot runtime session is used for
verification, it is the standard headless selftest invocation (15 FPS default
applies to any interactive session, which this package does not need).

---

# 16. Narrative/Content Integration

- The removed texts (§11) are not re-authored elsewhere; the signals remain
  coherent: 812_5/867_9 keep their stage-fragment voice plus the two terminal
  follow-ups; 55_1 keeps its answered follow-up and its dark-ending stage
  text.
- House voice rules from the sealed contracts continue to bind any *future*
  content (observational radio-intelligence lines; no hidden-truth exposure in
  hints; no real places/wars/people); this package authors nothing.
- The 5 primary-wins dead rows remain untouched (dead-data register rules:
  no new content, no deletion without signature, exactly-5-warnings pin).
- Tone check on removals: no tone impact — removals only.

---

# 17. Failure Modes

Complete enumeration for the touched surface. "Gate" = where the behavior is
proven (V = validator case in D6, R = population replay D5, E = existing
suite, S = selftest, C = closeout record).

## 17.1 Signal class × event matrix (runtime truth the content must respect)

| Signal class | answered | rescue_success | rescue_failed | expired | ambush_encountered |
|---|---|---|---|---|---|
| Genuine + mission (88_3, 156_8, 445_2, 901_2, 726_5, 609_4, 455_7, 555_0, 318_0, 269_3) | fires once, +2 (R) | fires once, +5 (R) | reachable (late arrival); 0 authored rows (E) | fires once, −2, both expiry branches (R) | N/A — never resolves TerminalAmbush; D2 forbids authoring it (V) |
| Genuine, moral link, **no mission** (812_5, 867_9) | unreachable today; harness proves content correctness (R, F1→C) | same | same | same (D1 passes: window authored) | D2 forbids (V) |
| Genuine, no mission, no link (217_4, 401_9, 203_1, 311_5) | unreachable today (R, F1→C) | unreachable (F1→C) | unreachable | no window authored → D1 forbids authoring it (V) | D2 forbids (V) |
| Trap + mission (192_4, 380_2) | would fire +2 if authored (none authored) | suppressed from trap path by construction (E: Scenario D) | reachable; none authored | trust-excluded ("wisdom"); D1 forbids authoring it (V) | fires once, −5 net −3 with answer (R) |
| Trap, no mission (148_2, 410_7, 288_1, 333_6, 478_2) | unreachable (F1→C) | unreachable | unreachable | D1 forbids (V) | unreachable today; harness proves content (R, F1→C) |
| Stale / narrative / knowledge / encrypted / automated (55_1, 129_6, 278_3, 367_9, 392_7, 512_4, 623_8, 701_3, 756_1, …) | unreachable without mission (55_1's kept entry — F1→C) | unreachable | unreachable | no window → D1 forbids (V; removes fu_55_1_expired) | D2 forbids (V) |
| Builtin fragment-less fallbacks (108_9, 134_5, 162_1, 124_7) | no fragments, no follow-ups; stage resolver returns null (E) | — | — | — | — |

## 17.2 Event × save-state matrix

| Scenario | Expected | Gate |
|---|---|---|
| Fire due, no save | fires once on first tick ≥ due; fired ledger persisted | R (all 40) |
| Save between schedule and due, restore, tick | fires exactly once, same day, same text/cue | R (per entry) |
| Save after fire, restore, tick | never re-fires (persisted fired ledger) | R + E |
| Explicit moral-choice ignore, later deadline expiry on same signal | exactly one ignored record (first event wins); one expired scheduling | E (SignalTrustTests overlap cases) + R |
| Re-tick due day repeatedly / tick far past due | exactly-once | R |
| Two entries same due day | ordinal `(dueDay,parent,id)` order | R |
| `delay_days: 0` | due == event day; fires on the same-day tick that follows the event (host order) | V (shape) + E (mechanism) |
| Pending entry whose follow-up id was removed from the catalog (the 3 remediated ids in an old save) | silent expiry at fire time; no crash/event | R (T-17) |
| Parent *signal* removed from catalog entirely | pending entry expires silently (same branch) | E (Wave 3 case) |
| Tampered save payload | `TryDecode` false; nothing restores | E |
| V5 save → V6 | empty scheduler default; nothing retroactively fires | E (migration pin) |
| Restore fires no events during restore itself | replayed presentation only via tick | E + R |

## 17.3 Validator failure modes

| Case | Expected | Gate |
|---|---|---|
| Rule fires on shipped rows pre-remediation | exactly 3 errors, naming 55_1 (D1), 812_5 (D3), 867_9 (D3) — captured as P1 evidence | V + S |
| Rule text naming | catalog, signal, follow-up path, id, field/value, rule id | V |
| Grammar-invalid row also hits semantic rule | grammar error only (rules run after; no double-report) | V |
| Case variants (`Expired`, `AMBUSH_ENCOUNTERED`, `Trap`) | matched case-insensitively | V |
| `deadline_days: 0` / negative / string | treated as no positive window → D1 error | V |
| Both deadline casings present | snake wins | V |
| Trap with positive window + expired entry | D1 second-branch error | V |
| 3+ entries on a primary-wins **dead row** | D3 still fires (corpus-uniform) — none today; policy note in C | V (synthetic) |
| Warning-count regression | any new warning breaks the dead-data register pin — rules emit errors only | S (5-warning pin) |

## 17.4 Verification-harness failure modes

| Case | Expected |
|---|---|
| Future content edit without re-census | census drift guard fails, naming the drifted row |
| Nondeterministic harness ordering | two-run fingerprint mismatch — treated as defect, never pinned |
| Referenced cue id renamed/removed from registry | D7 section fails naming catalog:signal:level:id |
| Utilization gate reports distress orphan/regression | stop and report (Rule 10); not auto-fixed |

---

# 18. Test Strategy

Per TEST_POLICY: focused targets only; new file runs alone first; no
full-suite run by default (the Wave 5 full-suite pass was a dedicated window
and is not repeated here).

## 18.1 D6 — validator-rule cases (extend `Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs`)

Synthetic mini-catalogs written to the test's temp data dir (existing file's
established pattern), each case asserting the exact error substring including
the rule id:

| # | Case | Rule | Expected |
|---|---|---|---|
| V-01 | expired entry, no deadline fields, non-trap | D1 | error: no positive response window |
| V-02 | expired entry, `deadlineDays: 0` | D1 | error (no positive window) |
| V-03 | expired entry, `deadline_days: -2` | D1 | error |
| V-04 | expired entry, `deadlineDays: "five"` (non-integer) | D1 | error |
| V-05 | expired entry, `deadlineDays: 4`, `authenticity: "trap"` | D1 | error (trap-class second branch) |
| V-06 | expired entry, `deadlineDays: 4`, `deceptive_faction_id: "x"` | D1 | error |
| V-07 | expired entry, `deadline_days: 3`, genuine, empty consequence | D1 | **pass** (trust-implied consequence) |
| V-08 | expired entry, both casings present, snake `0`, camel `4` | D1 | error (snake wins) |
| V-09 | trigger `Expired` (casing) on windowless signal | D1 | error (case-insensitive) |
| V-10 | ambush entry, `authenticity: "genuine"` | D2 | error (genuine-only) |
| V-11 | ambush entry, empty authenticity, `outcome_type: "survivor_community"` | D2 | error |
| V-12 | ambush entry, `authenticity: "Trap"` (casing) | D2 | pass |
| V-13 | ambush entry, `outcome_type: "bait_trap"`, empty authenticity | D2 | pass |
| V-14 | ambush entry, `deceptive_faction_id` non-empty only | D2 | pass |
| V-15 | 3 follow-up entries on one signal | D3 | error naming `count=3`, `maximum of 2` |
| V-16 | 2 entries | D3 | pass |
| V-17 | 1 entry / absent block | D3 | pass |
| V-18 | grammar-invalid trigger + expired semantics on windowless row | order | exactly one error — the Wave 3 grammar error; no D1 double-report |
| V-19 | **real-catalog pin:** both shipped catalogs pass D1–D3 with 0 errors post-remediation | all | pass (mirrors the Wave 4 "real-catalog cleanliness" pattern) |

## 18.2 D5 — population replay (`DistressFollowUpPopulationReplayTests.cs`, runs alone first)

| # | Case | Content | Key assertions |
|---|---|---|---|
| T-01 | Census drift guard | both real catalogs | 24 signals / 40 entries / class counts 15-16-7-2 / table equality; per-row diff messages |
| T-02 | answered-class population | 15 entries | fire day = dispatch day + delay; text byte-equality; +2 once per signal; cue == authored |
| T-03 | rescue_success-class population | 16 entries | arrival ≤ deadline & ≤ death day; TerminalRescued; due = arrival + delay; +5 once; text/cue equality |
| T-04 | expired-class population | 2 entries (812_5, 867_9) | legacy-expiry drive (no tokens) → TerminalFailed → due = expiry + delay; −2 once; fires once |
| T-05 | expired-class, consequence path | 1 harness-token drive (812_5 with `sender_death`) | ApplyIgnoreConsequence → same due day; idempotent (second expiry never re-applies) |
| T-06 | ambush-class population | 7 entries | TerminalAmbush → due = arrival + delay; −5 (net −3 with the answer); **no other trigger class schedules for any trap signal** (population suppression invariant) |
| T-07 | Exactly-once × codec | all 40 | save between schedule and due → restore → fires once; save after fire → restore → never re-fires |
| T-08 | Same-day ordering | 2 harness collisions | ordinal `(dueDay,parent,id)` fire order |
| T-09 | Cue/text-only partition | all 40 | non-empty cue ids ∈ {authored set}; empty ⇔ entry ∈ text-only set (22→20 post-remediation: 812_5/867_9 answered had cues; text-only set is unchanged at 22 minus 0 = 22… **recompute at P0: removed entries carried cues, so text-only stays 22 → pin 22 text-only / 18 cued**) |
| T-10 | Undiscovered silence | sample of 5 signals | no intercept → no trust, no scheduling, no cue |
| T-11 | Two-run fingerprint | full population, days 1–30 | identical `StableHash` fingerprints across two runs |
| T-12 | Population interrupted == continuous | save at day 6 → restore → tail trace byte-identical | Scenario B property at population scale |
| T-17 | Removed-id save tolerance | hand-built V6 payload referencing `fu_55_1_expired` pending/fired | restore clean; pending entry expires silently; no event |

Case count stays within the builder norm (<100); per-class aggregation uses
per-row failure messages naming the signal/follow-up id (TEST_POLICY
aggregation rule: homogeneous rows, clear per-row diagnostics; lifecycle and
codec cases T-05/T-07/T-12/T-17 stay independent facts).

## 18.3 Focused verification commands (the package's gate set)

```bash
# build gate
dotnet build Ashfall.csproj                                  # 0 errors

# new file alone first (TEST_POLICY)
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs

# extended validator cases
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs

# focused region
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/           # full radio directory

# headless gates
godot --headless --path . -- --data-integrity-selftest       # 0 errors, exactly 5 pinned warnings
godot --headless --path . -- --audio-selftest                # PASS incl. new distress section
godot --headless --path . -- --content-utilization-selftest  # PASS, 0 orphans, 0 regressions
```

Pre-remediation evidence capture (P1): run `--data-integrity-selftest` after
implementing rules but before the data fix; expect **exactly 3 errors**
(`fu_55_1_expired` D1; `freq_distress_812_5` D3; `freq_distress_867_9` D3) —
archive the output in the closeout as the rules-fire proof.

---

# 19. Dependency-Ordered Phases

Each phase has a hard gate; a failed gate stops the line (no improvisation).

## P0 — Census (read-only)

1. Re-run the census (JSON parse of both catalogs): signals, entries,
   triggers, delays, cues, hints, window/consequence fields, trap
   classification; reproduce §2.2's tables at execution HEAD.
2. Re-derive the mission-registry coverage (`RegisterAuthoredRescueMissions`
   vs. follow-up-carrying signals) and the F1 unreachable set.
3. Enumerate distinct cue ids and diff against `AudioCueCatalog` constants.
4. Produce the P0 artifact: the census table with per-row expected rule
   verdicts (fires/passes) and the replay's post-remediation pins (40/24,
   class counts, text-only set).
5. Open the claim row in `WORKTREE_OWNERSHIP.md`
   (`claim-cf-p1-distress-content-seal-<date>`, package
   `CF-P1-DISTRESS-CONTENT-SEAL`, paths = §20 table).

**Gate:** the census table exists, matches this plan or names every drift;
any drift (new signals/entries/cues since 2026-09-19) re-baselines the pins
*before* P1. Read-only otherwise.

## P1 — Validator rules + data remediation (single commit)

1. Add `MaxFollowUpsPerSignal = 2` const, `TryReadIntEither`,
   `IsTrapClassBroadcast` helpers, and the three rule blocks to
   `ValidateDistressSignalStagesFile` (D3 before the entry loop; D1/D2 inside
   it after the Wave 3 checks), per §6.1–6.5.
2. Extend `DistressFollowUpTests.cs` with V-01…V-19; run the file alone.
3. Run `--data-integrity-selftest` → capture the expected 3 errors (evidence).
4. Apply the three removals (§11) to `radio_distress_signals.json` — same
   commit.
5. Re-run the selftest → **0 errors, exactly the 5 pinned warnings**; re-run
   V-19; run the Radio directory.

**Gate:** build 0 errors; `DistressFollowUpTests` green; data-integrity PASS
with the pinned warning set; no other validator output delta. If remediation
content choice differs from Option B (content-authority call), swap the
removed ids in the same commit and update the P2 pins accordingly — never
relax a rule instead.

## P2 — Population replay

1. Create `DistressFollowUpPopulationReplayTests.cs` per §6.6 (census table +
   drift guard + drive patterns + assertions T-01…T-17).
2. Run the new file alone; then the Radio directory.

**Gate:** all 40 post-remediation entries exercised; every assertion green;
two-run fingerprints identical; interrupted == continuous. Any nondeterminism
halts the line as a defect.

## P3 — Registry cross-reference + utilization

1. Add the D7 section to `src/Audio/AudioSelfTest.cs` (read-only JSON parse;
   membership + resolution checks; text-only summary print).
2. Run `--audio-selftest` → PASS including the new section; record the 7-id
   resolution table and the text-only set.
3. Run `--content-utilization-selftest` → PASS, 0 orphans; record the
   distress-catalog classifications.

**Gate:** both selftests PASS with outputs archived for the closeout.

## P4 — Closeout + governance (integrator touchpoints flagged)

1. Write `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` per §10.5/§24 contents,
   Wave 5 section order.
2. **Integrator (or authorized transfer):** add the
   `CF-P1-DISTRESS-CONTENT-SEAL` row to `INTEGRATION_PLANS.md` and correct the
   stale "Next waves" PR2 line (~208) to record that follow-up payload and
   audio-cue content landed in `660cb595`/`8f72ee62` and is sealed here.
3. Close the `WORKTREE_OWNERSHIP.md` claim row (DONE + verification summary).
4. Regenerate the owned docs indexes via their generators' `--check` modes
   (docs INDEX / catalog registry — never hand-edit generated outputs).
5. Final focused pass: Radio directory + the three headless gates.

**Gate:** closeout published; ledger row merged; checks in sync; Radio suite
green.

**Phase coupling notes:** P1 before P2 (replay pins post-remediation census);
P2 before P4 (closeout quotes replay evidence); P3 independent of P2 but
before P4. P0 is strictly read-only and may run the moment the claim opens.

---

# 20. File Impact Map

| File | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | MODIFY (additive: 1 const, 2 helpers, 3 rule blocks inside the existing distress pass) | D1–D3 | low — additive, error-only, pinned warning count untouched |
| `Assets/StreamingAssets/Data/radio_distress_signals.json` | MODIFY (3 entry removals, §11) | D4 remediation | low — unreachable entries; silent-expiry path proven (T-17) |
| `Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs` | MODIFY (additive V-01…V-19) | D6 | low |
| `Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs` | NEW (~13 facts, aggregated per-row) | D5 | low — public APIs only |
| `src/Audio/AudioSelfTest.cs` | MODIFY (one additive section) | D7 | low — read-only, headless-safe |
| `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` | NEW | D9 | none |
| `INTEGRATION_PLANS.md` | MODIFY (row + stale-line correction) | D10 | integrator-owned; routed per program |
| `WORKTREE_OWNERSHIP.md` | MODIFY (claim open/close) | D10 | standard claim hygiene |
| `docs/INDEX.md` + generated registries | REGENERATE via owning generators `--check` | D10 hygiene | generator-owned; never hand-edited |

**Explicitly untouched (with reasons):** all five wave-sealed Core radio files
(mechanism complete); `RadioSave.cs` (no save change); `RadioHostSession.cs` /
`Main.Expeditions.cs` / `RadioPanel.cs` (wiring/presentation already correct);
`AudioCueCatalog.cs` (all 7 ids exist); `ContentUtilization*`
(coverage pre-existing); `radio_distress_signals_expansion.json` (no
violations); the 5 dead-data rows (register policy);
`DistressSignalTasks912ReplayTests.cs` (sealed Wave 5 artifact — extended by a
new sibling, never edited); `moral_choice_quests_distress.json` (no
interaction).

---

# 21. Risks

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Content authority disagrees with the Option-B removals | medium | P1 gate holds | §11.4 decision record presents A/B/C with rationale; swap is one-commit local; rules never relax |
| Census drifted between 2026-09-19 and execution (new entries/cues) | medium | wrong pins | P0 re-census gate re-baselines pins before any edit; drift guard test keeps them honest afterwards |
| F1 (21 unreachable entries) pressures scope expansion into mission registration | medium | package creep | F1 is documented decision-needed in the closeout and routed to the foreman; this package stays verify-and-seal (Rule 10) |
| Validator pass order regresses an unrelated catalog | low | gate noise | rules are scoped to the distress pass; Radio dir + data-integrity gate prove no collateral output |
| AudioSelfTest runtime cost / headless fragility | low | CI flake | section is read-only JSON + dictionary lookups; existing selftest already does heavier stream loads |
| Two-run fingerprint flakes from dictionary iteration | low | false defect | harness sorts all ledger reads ordinally (existing scheduler capture pattern); a flake is treated as a real defect per program §C.1.6 |
| Closeout mis-quotes counts | low | governance debt | every number in the closeout is produced by the P0 census artifact / gate outputs, not by hand |
| Ledger row edited by non-owner | low | process breach | program routes the row through the ledger owner or an authorized transfer; the builder flags it in handoff |

---

# 22. Out of Scope

- Any new follow-up, stage, hint, or cue **content** (no authoring tranche).
- Mission registration for the 12 unmissioned follow-up-carrying signals, and
  any runtime change to make F1 entries reachable (data/architecture
  authority decision; routed via the closeout).
- Save-version bump, new save fields, migration changes.
- Scheduler/trust/resolver/manager code changes of any kind.
- A second audio registry, playback changes, panel/UI work, localization.
- The 445_2 JSON-vs-fixture `ignoreConsequence` consistency question
  (recorded; candidate future rule).
- `rescue_failed` content (0 authored rows; grammar stays mechanism-tested).
- Renaming the `fu_*_trap_fallen_for` id strings (cosmetic churn on stable
  dedupe identities; documented instead).
- Full-suite runs, performance sweeps, and anything named in the program's
  "deliberate exclusions".
- A `deadline_days` shape/type rule beyond D1's needs (candidate, noted).

---

# 23. Rollback Strategy

- The package lands as **one commit for P1** (rules + remediation together —
  required so the gate never goes red) plus separate commits for P2 tests, P3
  selftest section, and P4 docs/governance.
- Rollback of any phase = `git revert` of its commit. No save migration exists
  to unwind (V6 unchanged); no player save can be harmed by the data removals
  (silent-expiry path, T-17).
- If a rule proves mis-specified after merge: revert the P1 commit (rules +
  removals atomically), record the finding, and re-spec through the foreman —
  never hot-patch the rule to pass silently (program §C.1.6).
- If the D7 section misbehaves in CI: revert the P3 commit; the runtime
  missing-cue fallback is unaffected either way.
- Governance rows revert with their commit; the dead-data register and its
  5-warning pin are never touched by rollback (rules emit no warnings).

---

# 24. Definition of Done

Mapped to the program's §C.1.2 delta and §C.1.12 closeout outline; every box
names its evidence. **Checkboxes below are left exactly as authored
(unchecked, forward-looking) because this section predates execution; see §26
for the actual post-execution evidence, which confirms every item below was
met.**

- [ ] **D1–D3 live in the permanent gate** — `ValidateDistressSignalStages`
  emits the three rule ids; V-01…V-19 green; pre-remediation evidence shows
  exactly the 3 expected shipped-row errors; post-remediation
  `--data-integrity-selftest` = **0 errors, exactly 5 pinned warnings**.
- [ ] **Remediation landed** — the 3 entries removed in the rules' commit;
  before/after quoted in the closeout; post-fix census 24 signals / 40
  entries (15/16/7/2).
- [ ] **Population replay green** — every post-remediation entry driven through
  the real V6 codec with fire-day, exactly-once, codec, text, cue, and trust
  assertions; T-01…T-17 pass; two-run fingerprints identical; interrupted ==
  continuous.
- [ ] **Registry verified** — `--audio-selftest` distress section PASS: all
  referenced cue ids resolve (7 ids at plan time) or are in the documented
  text-only set; output archived.
- [ ] **Utilization sealed** — `--content-utilization-selftest` PASS, 0
  orphans, 0 regressions over the distress layer; classifications recorded.
- [ ] **PR3 closeout published** — scope + rules with fixture counts; the P0
  census verbatim; replay results (chains, fingerprints, two-run stability);
  registry table + text-only set; utilization result; remediations with
  before/after; the ledger-row diff; limitations (F1 reachability finding
  routed as decision-needed; the 445_2 declaration mismatch; un-authored
  future tranches; 88_9's legal hint omission).
- [ ] **Governance** — `INTEGRATION_PLANS.md` row (integrator) marks P1 sealed
  and corrects the stale PR2 deferral line; claim row closed; generated
  indexes `--check`-clean.
- [ ] **Quality gates** — build 0 errors; Radio directory green; no warning
  count change; no full-suite run claimed as evidence; handoff per
  `AI_AGENT_WORKFLOW.md`.

---

# 25. Implementation Handoff

## MUST PRESERVE

- The five sealed mechanism files and their contracts (stage resolver,
  follow-up scheduler + closed grammar, audio cue resolver, trust ledger +
  policy, V6 codec + frozen shapes + migrations).
- The exactly-once pending/fired ledgers, the Intercept-gated playback edge,
  and the `playedBroadcastKeys` dedupe contract.
- The dead-data register: exactly 5 named primary-wins warnings; no new
  validator warnings (rules are error-only); no edits to the 5 dead rows.
- `DistressSignalTasks912ReplayTests.cs` byte-for-byte (sealed Wave 5
  artifact); the expansion catalog (no violations); all unrelated dirty
  worktree state.
- TEST_POLICY discipline: focused targets, new file alone first, no
  full-suite claim.

## MUST ADD

- Three validator rules with the exact rule ids
  (`distress_followup_expired_requires_consequence`,
  `distress_followup_trap_only_on_lures`, `distress_followup_max_two`), house
  error format, specified ordering, and the two shared helpers.
- The three data removals (§11) in the same commit as the rules.
- `DistressFollowUpPopulationReplayTests.cs` with the census drift guard and
  T-01…T-17.
- V-01…V-19 in `DistressFollowUpTests.cs`.
- The `AudioSelfTest` distress cue cross-reference section.
- `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` with the eight mandated
  contents, including the F1 decision-needed finding.

## MUST NOT DO

- No rule relaxation or boundary amendment to make shipped data pass without
  the documented decision record; no deleting a rule because it fires.
- No save-version bump, new save field, or migration; no scheduler/trust/
  resolver/manager edits; no new API.
- No Unity anything; no engine reference in Core; no `System.Random`; no
  wall-clock seeding.
- No mission registrations, no reachability "fixes", no new content, no panel
  work, no second registry, no renaming of shipped follow-up ids.
- No edits to generated indexes by hand; no edits to `INTEGRATION_PLANS.md`
  by the builder (integrator/authorized transfer only); no full-suite runs.

## VERIFY WITH

```bash
dotnet build Ashfall.csproj                                               # 0 errors
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs   # alone first
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/                        # focused region
godot --headless --path . -- --data-integrity-selftest                    # 0 errors + exactly 5 pinned warnings
godot --headless --path . -- --audio-selftest                             # PASS incl. distress section
godot --headless --path . -- --content-utilization-selftest               # PASS, 0 orphans
```

Plus the P1 mid-step evidence run (rules in, remediation pending) showing
exactly the 3 expected shipped-row errors.

## FIRST SAFE IMPLEMENTATION STEP

**P0 census — read-only, one bounded pass.** Open the claim row, re-run the
JSON census and mission-registry diff at execution HEAD, reproduce (or
drift-correct) the §2.2 tables and the post-remediation pins, and publish the
census artifact as the P0 gate output. It is immediately useful, touches
nothing, and de-risks every later phase. (Program §C.1 handoff names the same
first step.)

---

*Plan authored 2026-09-19 against branch `Zcode_Branch` @ `65357b8a`. All
counts, line citations, and reachability findings were produced by direct
reads/parses of the files listed in §4.1; re-verify at P0 before editing.*

---

# 26. Execution closeout — added 2026-09-21 during batch review

This section was appended during a review pass of this plan batch, after
direct evidence showed the package above was already executed the same day it
was authored. §1–§25 are preserved unedited above as the original
forward-looking plan; this section reconciles them against current reality so
a future reader does not re-claim or re-implement sealed work.

## Evidence of completion

- `WORKTREE_OWNERSHIP.md`, claim row `claim-cf-p1-distress-content-seal-2026-09-19`:
  *"**DONE 2026-09-19:** validator 39/39; population replay 46/46; build 0/0;
  data-integrity 0 errors + 5 pinned warnings; audio selftest 645/645;
  content-utilization CI/deep-chain PASS."* This exact evidence set matches
  §24's checkbox list item-for-item (validator rules live, population replay
  green, registry verified, utilization sealed, quality gates green).
- `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` exists on disk and opens with
  *"Status: **SEALED — CF-P1-DISTRESS-CONTENT-SEAL (2026-09-19).**"* Its §1
  documents the three validator rules with the exact rule ids this plan's §25
  `MUST ADD` specified
  (`distress_followup_expired_requires_consequence`,
  `distress_followup_trap_only_on_lures`, `distress_followup_max_two`), each
  with fixture coverage, and records the extended validator file passing
  **39/39** — matching §24's D1–D3 checkbox exactly.
- The sealing work landed in commit `d04f964ecb37f96af3f236520293840af400ad9a`
  ("feat: implement distress content seal, vehicle armor grades, bootstrap
  path parity, and integration plans") — the same commit that authored this
  plan file — which added
  `Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs` (532
  lines, new), extended `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`
  (+371 lines), trimmed `Assets/StreamingAssets/Data/radio_distress_signals.json`
  (the three remediation removals §11/§24 called for), and added
  `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` (195 lines, new). `git
  merge-base --is-ancestor d04f964e HEAD` confirms this commit is in the
  current branch's history.

## What this means for a future reader

- **Do not** open a new claim for `CF-P1-DISTRESS-CONTENT-SEAL`. It is sealed.
- **Do not** re-add the three validator rules, the population replay tests, or
  the PR3 closeout document — all three already exist and are green.
- The §25 `MUST NOT DO` list (no rule relaxation, no save-version bump, no
  scheduler/trust/resolver/manager edits, no second registry) remains the
  correct constraint set for anyone touching this area *later*, e.g. if a new
  follow-up entry is authored and needs to pass the now-permanent validator
  rules — it is preserved as a boundary reference, not as pending work.
- If a future audit finds the validator counts have drifted from 39/39 (new
  content added without corresponding fixture coverage, for example), that is
  new work with its own evidence and its own plan — not a reason to reopen
  this package.
