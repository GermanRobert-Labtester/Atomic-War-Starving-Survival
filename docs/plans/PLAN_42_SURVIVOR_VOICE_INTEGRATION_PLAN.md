# Plan 42 — A Voice for Each of Them: Survivor Voice Integration Plan

**Package:** `C2[18]` / Plan 42 — Deterministic Survivor Voice, Delivery Contracts, and Social Speech
**Census status at authoring:** `AUDIT-PENDING` (`docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, row `C2[18]`)
**Source documents (read in full):** `Next-steps-plans/shipped_to_chat/Plan_42_A_Voice_For_Each_Of_Them.md`; `C-integration-plans/C2_planintegration[18].md`
**Conflict-resolution audit:** `docs/plans/wave12_part1_1/A1_PLAN49_PREREQUISITE_AUDIT.md` (`DECIDED-DEFERRED`; names Plan 42 as a hard prerequisite of `C1[16]`/Plan 49 and of the EN-01/EN-05 rescoping proposals)
**Authored:** 2026-09-19, planning role only — this document modifies no production code, data, or tests.
**Verification basis:** every factual claim below was re-checked against current source at `HEAD` of branch `Zcode_Branch` (top commit `65357b8a`). Where this document and the source plan disagree, current source wins and the disagreement is recorded inline.

---

# 1. Objective

Give ASHFALL's 129 authored survivors a deterministic voice: a data-authored,
keyed line catalog; a Core selection authority that picks a line as a pure
function of (speaker identity × canonical state × day event × knowledge class
× day) using the seeded RNG contract; and a delivery contract that routes each
selected line into surfaces that already exist (journal, briefing, survivor
detail panel, HUD) under explicit attention budgets, with a persisted
idempotency ledger so reloads never repeat a line.

Bounded outcome (what "done" means for this package):

1. `Assets/StreamingAssets/Data/survivor_voice_lines.json` and
   `survivor_voice_registers.json` exist, validate clean through a new
   `CatalogIntegrityValidator` tier, and are loaded by one Core loader.
2. `Ashfall.Core.Voice.SurvivorVoiceSystem` selects lines deterministically;
   same master seed + same event sequence + same save/restore boundary produce
   the same ordered line-id sequence (proven by a replay harness in the style
   of `DistressSignalTasks912ReplayTests`).
3. Every selected line is delivered to a declared sink or the boot-time port
   check fails (`--port-contract-selftest`); nothing is silently discarded.
4. Every delivered line of `event` class is archived in the journal through
   the existing `JournalSystem.TryAddRawEntry` knowledge-key dedup.
5. A narrow first content slice (the `survivor_perished` trigger family) is
   authored, tone-gated, and reachable in-game.

Non-goals (from the source plan, restated as binding constraints): no dialogue
tree, no conversation engine, no chat system, no new panel, no new audio
family or VO production (`DEC-11` stands), no hardcoded survivor prose in C#,
no per-character voice *skill progression* of any kind (`DEC-17` stands — see
§21), no line as the sole carrier of a mechanical warning, no line selected
from information the speaker could not plausibly know, no new knowledge/rumor
simulation (that is parallel plan 131's concern), and no UI interruption of an
active player decision.

---

# 2. Current Reality

This section is the collision check. Every existing survivor-facing text or
voice-adjacent system was read in current source before any design below was
written. The verdict column says what Plan 42 does with each: **EXTEND** (add
to it / consume it), **ADAPT** (copy its pattern, not its code), or
**LEAVE** (do not touch; named so nobody "fixes" it during this package).

## 2.1 `SleepNarrativeProjection` — the closest precedent — ADAPT (partially)

- `Assets/Ashfall.Core/Needs/SleepNarrativeProjection.cs`; host tick
  `src/Main.SleepNarrative.cs` (`TickSleepNarrative(int day)`); tests
  `Ashfall.Core.Tests/Needs/Plan177SleepNarrativeProjectionTests.cs`; debt row
  `KNOWN_DEBT.md` `DEBT-177-SLEEP-EVENT-CONSUMER` = **RETIRED** (the task is
  sealed; the system is live and must not be re-opened).
- Shape: a **read-only classifier** over the persisted
  `SurvivorMentalHealthRecord` (insomnia days, active crisis id + remaining
  days, active trauma ids, `stressPermille`). `Classify(record)` maps state to
  `Restful/Troubled/Nightmare`; `Project(survivorId, day)` returns a
  `SleepBeat` (kind, survivor id, journal key, text). It writes no state
  anywhere.
- Determinism: host forks `_campaignDay.Rng.Fork(CampaignStreamIds.Psychology,
  day, 11)` once per day and passes it in; line index = `rng.Next(0, lines.Length)`
  (fallback `day % lines.Length` when no RNG). Same day + seed ⇒ same line.
- Delivery: journal only, via `_journal.TryAddRawEntry(beat.JournalKey,
  beat.Text, null!, day)` with a per-survivor key (`sleep_nightmare:{id}`) so
  the journal's knowledge-key dedup bounds it to one entry per survivor per
  severity. Restful beats are never journaled.
- **Deviation that must not be copied:** the line text lives in three
  `private static readonly string[]` arrays of hardcoded English inside Core.
  Plan 42's source plan explicitly forbids this class of debt ("no new class
  of hardcoded English"; the corpus doc's success criterion 20: "No survivor
  prose is hardcoded in C#"). The sleep lines predate the string-key policy;
  Plan 42 must not extend them, translate them, or add a fourth array. The
  pattern to copy is the *shape* (pure projection + day-keyed fork + journal
  dedup), not the *storage* (C# string arrays).

## 2.2 `JournalSystem` + `JournalVoice` + `JournalVoiceProseCatalog` — EXTEND (as the archive)

- `Assets/Ashfall.Core/Journal/JournalSystem.cs`: `TryAddRawEntry(string
  knowledgeKey, string text, ISurvivorAuthor author, int day, float hour)` —
  deduped exactly-once per key by `KnowledgeBase.Discover(key)`; bounded
  `MaxEntries = 64` with oldest-first eviction; `JournalSave` persists entries,
  knowledge keys, and the sequence counter; restore rebuilds both. Host
  precedent for raw journal writes from a projection: `Main.Enrichment.cs`
  `CheckKeepsakeRecognition` (`keepsake_recognized:{sid}:{itemId}` keys).
- `Assets/Ashfall.Core/Journal/JournalVoice.cs` + `JournalVoiceProseCatalog.cs`
  + `Assets/StreamingAssets/Data/journal_voice_prose.json`: an existing
  **keyed prose catalog with per-speaker-trait variants** — each knowledge key
  maps to nine `RiskBiasTrait` prose variants plus a default. This is direct
  precedent that variant-per-identity prose in data works, but it is scoped to
  the player-journal persona (`RiskBiasTrait`), not to arbitrary survivors,
  and it is resolved at insert time, not stored as a key. Plan 42 does not
  reuse `JournalVoice` (wrong axis: 9 player biases vs. per-survivor
  registers) but copies its discipline: prose lives in JSON, code holds keys.
- `JournalCorpusAdapter`: an authored-corpus overlay that replaces body text
  for known producer keys. Voice lines must NOT be routed through it (it is a
  journal-discovery overlay, not a speech archive); the voice journal write is
  a plain `TryAddRawEntry` with a voice knowledge key.

## 2.3 `MoraleMarkSystem` — the only existing "bark" mechanism — ADAPT (idiom), LEAVE (code)

- `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs:90` —
  `GetLaterProse(id)`: "Inspect/bark sentence from `duty_roster_marks.json`,
  else the saved payload." Catalog: `DutyRosterCatalog.Marks` /
  `duty_roster_marks.json` (43 defs, one non-test `src/` consumer).
- This proves authored sentence-in-data selection works in this codebase, but
  it is scoped to roster marks (flags set by duty events), not to survivors or
  state-triggered speech. The source plan's evidence inventory confirms
  `GetBarkSentence` exists nowhere and the only `bark`/`quip`/`voiceline` hit
  is this one. Plan 42 copies the *catalog-bound prose lookup* idiom and
  builds the survivor-scoped selector it was never meant to be. The mark
  system itself is untouched.

## 2.4 `SurvivorRelationsSystem` — EXTEND (read-only consumer)

- `Assets/Ashfall.Core/SurvivorRelationsSystem.cs`. Pair ledger:
  `RelationshipEntry{dwellerA, dwellerB, affinity[-100..100], trust,
  resentment, grief, grief_since_day, bondType, recentCauses,
  lastInteractionDay}`. Conflicts: `TryTriggerConflict()` (10%/day on the
  injected `ISeededRng`, one active conflict max), `Mediate(...)`,
  `OnConflictStarted` / `OnConflictResolved` / `OnRelationsChanged` events.
  Read seams: `TryGetRelationship(a, b, out entry)` (non-creating),
  `RelatedIds(survivorId)` (ordinal-sorted).
- Voice relevance: this is the state authority behind "people" speech
  (grudges, bonds, grief). Voice **reads** `affinity`/`resentment`/`grief` for
  condition matching and subscribes to conflict/mediation events as triggers.
  It never writes affinity, never creates relationships, never infers a grudge
  that is not in the ledger (source-plan step "No invented interiority").

## 2.5 `SurvivorSocialCoordinator` — EXTEND (event source; not the voice owner)

- `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` owns the seven
  social systems (Leadership, IdeologicalFriction, RationConflict, TraumaBond,
  SkillAtrophy, Exercise, PersonalBelongings) plus `Relations`. One aggregate
  save section `survivor_social` (`SurvivorSocialSaveState`,
  `src/Host/SurvivorSocialSaveStore.cs`). Host composition:
  `src/Main.SurvivorSocial.cs` `SetupSurvivorSocial()`.
- Event surfaces already raised that voice consumes as triggers:
  `RationConflictSystem.OnRationConfrontation(resenterId, targetId)`,
  `OnRationsStolen(thiefId, victimId)`, `OnResentmentBuilt(id, target,
  level)`; `IdeologicalFrictionSystem.OnFrictionDetected(a, b, delta)`,
  `OnRoommateSynergy(a, b)`; `LeadershipSystem.OnLeaderStressIncreased`,
  `OnLeaderBreakRisk`, `CurrentLeaderId`, `GetLeaderStress(id)`.
- **Decision: the voice system is NOT added to this coordinator.** The
  coordinator is the social-mechanics authority; voice is a read-only
  selection/projection layer over many owners (needs, relations, memorial,
  roster, enrichment), exactly like `SleepNarrativeProjection`. Adding voice
  here would make the social coordinator the presentation-selection owner and
  would bloat its save section with a presentation concern. Voice gets its own
  small state + save section (§9, §12).

## 2.6 `MoraleContagionSystem` — ADAPT (the ports idiom), LEAVE (code)

- `Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs`. Notable for this
  plan: `MoraleContagionPorts` — a single DTO of narrow delegate ports
  (`AliveSurvivors`, `GetMorale`, `AreInSameRoom`, `GetDutyRole`,
  `GetBondStrength`, `TriggerBreakdown`, …) that the host binds to canonical
  owners. Zero RNG; fully deterministic buffered commit. This is the cleanest
  existing idiom for "a Core system that must read state owned by six other
  systems without referencing them" — the voice system adopts it verbatim as
  `SurvivorVoicePorts` (§10). Contagion itself is untouched.

## 2.7 `MemorialSystem` + `RelationsGriefSink` + `ProceduralEulogyEngine` — EXTEND (trigger + subject source)

- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`: idempotent
  `Memorialize(MemorialInput)` (first call wins; grief fires once);
  `OnMemorialized` / `OnMourned` events; `Mourn(deceasedId, day)` once-per-
  death via persisted `MournedDay`; optional `ProceduralEulogyEngine EulogyEngine`
  property composes a eulogy from a `DwellerLifeRecord` when no text is
  supplied — the existing "deterministic literary text from live state"
  precedent alongside the sleep projection.
- `SurvivorFateSystem` (§2.8) feeds it; grief lands in
  `SurvivorRelationsSystem` via `IGriefSink.ApplyDispersion` with quality
  scales (Peaceful 0.5 / Rushed 1.0 / Unattended 1.25).
- Voice relevance: `OnMemorialized`/`OnMourned` and the persisted grief on
  relationship entries are the subject matter and trigger for the first
  vertical slice (`survivor_perished` / grief lines). Voice reads; memorial
  owns.

## 2.8 `SurvivorFateSystem` — EXTEND (event consumer only)

- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`: the single death
  authority. One report per survivor id runs one cascade (roster dead → needs
  dead → assignments cleared → leadership stress + grief → final wish →
  memorial → journal `survivor_death_` key → consequence ledger →
  `survivor_perished` day event buffered for the briefing).
- Voice consumes the `survivor_perished` day event it emits (and the memorial
  events above). No change to the fate pipeline.

## 2.9 `PhantomMemoryEngine` — LEAVE (and a data-style warning)

- `Assets/Ashfall.Core/PhantomMemoryEngine.cs` +
  `Assets/StreamingAssets/Data/phantom_triggers.json`: background-id + item →
  motivation/breakdown outcomes with `descriptionKey`, `motivationText`,
  `breakdownText` **raw English fields in JSON**. This is pre-key-layer data
  debt, not a model for voice: Plan 42 lines carry `text_key` only (§11), and
  the plan does not "fix" phantom_triggers (out of scope, §22).

## 2.10 `CombatTraumaSystem` — EXTEND (flashback-class trigger source)

- `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`: per-survivor
  `hypervigilanceLevel`, night flags; events `OnHypervigilanceIncreased(id,
  level)`, `OnFalseAlarmTriggered(id)`, `OnShelterFalseAlarm(fraction)`.
  There is no literal "flashback" API in current source — the word exists only
  in the corpus plan and in phantom/vinyl planning docs. The source plan's
  "somatic/audio flashback machinery" resolves in current reality to: combat
  trauma records + false-alarm events + the mental-health record the sleep
  projection reads. **Premise correction recorded:** 42B's "flashback voice
  class" hangs off `OnFalseAlarmTriggered` / active-trauma state, not off a
  nonexistent flashback bus.

## 2.11 `DayEventVocabulary` + `DailyBriefingReportBuilder` + day-event stream — EXTEND

- `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs`: the Plan 31
  semantic-kind authority; every registered kind maps to exactly one
  `SemanticKind` (Heartbeat / Casualty / Hazard / Survivor / Shelter /
  Production / Expedition / Communication / Weather / Narrative), enforced by
  totality tests (`DayEventVocabularyTests`, `DayEventParitySourceGateTests`).
- `DailyBriefingReportBuilder.BuildFromDayEvents(int day, int buildSeed,
  IEnumerable<DayStateChangeEvent> events, int maxEntriesPerSection = 10)`:
  deterministic sectioned briefing; `DailyBriefingEntry` already carries
  **reserved `CauseId` and `ActorId` fields** (comment: "CauseId/ActorId are
  reserved for cause/actor"). Host call site: `src/Main.Campaign.cs`
  `ShowBriefingForDay(int day, DayAdvancedEventArgs args)` builds from
  `args.AllEvents()`.
- `DayStateChangeEvent` itself has `Kind`, `SourceOwnerId`, `PrimaryId`,
  `SecondaryId`, `Numeric` — **no `CauseId` field**. The source plan's 42C
  dependency "needs `causeId` from 31A step 5 to exist" is only half-landed:
  the briefing entry model reserves it; the day-event wire format does not
  carry it. This plan does NOT extend `DayStateChangeEvent` (that is 31A's
  owner decision); instead the voice cause handle is the tuple
  `(kind, sourceOwnerId, primaryId, day)` (§10, §13), which is already
  persisted in the events a day produces. The limitation is recorded in §21.
- Trigger-vocabulary evidence correction: the source plan's example triggers
  `ration_cut` and `season_changed` do not exist as day-event kinds. Current
  real kinds include `survivor_perished`, `child_lost`,
  `rationing_tier_changed`, `rationing_blocked`, `consumed_rations`,
  `portions_spoiled`, `season_changing`, `weather_condition`,
  `weather_unexpected_storm`, `memorial_checked`, `social_dispute_unresolved`,
  `social_dispute_mediated`, `duty_vacated`, `medical_admitted`,
  `medical_discharged`, `expedition_milestone`, `radio_intercept`, and the
  heartbeat set. The voice trigger vocabulary is defined as **a named subset
  of `DayEventVocabulary`-registered kinds** plus voice-local kinds for
  social-system events that are not day events (ration confrontation, friction
  ignition, false alarm), each registered in the voice catalog schema and
  validated (§11). New day-event kinds are NOT added by this plan; if a wanted
  trigger has no kind, that is a 31A follow-up, named in §21.

## 2.12 Identity layer: `ExpansionEnrichmentCatalog` + `SurvivorEnrichmentService` — EXTEND (the register seam)

- `Assets/Ashfall.Core/ExpansionEnrichmentCatalog.cs` (347 lines) + live host
  loader in `src/Main.Enrichment.cs` (`SetupEnrichment()`), data
  `expansion_survivor_fields.json` + `deep_lore_survivor_fields.json` +
  `antigravity_survivor_fields.json`: per-survivor `belief_profile_id`,
  `pre_war_profession_id`, `personal_keepsake_item_id`,
  `phantom_background_id`, `philosophical_stance`, `manifesto_law_code`.
- `Assets/Ashfall.Core/Survivors/SurvivorEnrichmentService.cs` (Plan 137):
  read-only `GetView(survivorId, def)` → `SurvivorEnrichmentView` with
  resolved labels and `IsEnriched`; already consumed by
  `src/UI/SurvivorDetailPanel.cs` (Name/Profession/Worldview/Keepsake rows).
- `survivors.json`: 129 definitions, fields `id, displayName, profession, bio,
  baseHealth, traitIds, activeQuestlineId`. There is **no `isChild` field** —
  the source plan's evidence inventory listed one; current authority for age
  class is `Assets/Ashfall.Core/Survivors/GenerationalSystem.cs`
  (`ChildDevelopment`, `DevelopmentPhase.{YoungChild, OlderChild, …}`,
  `GetCanonicalChildProfile(survivorId, currentDay)`). Premise correction
  recorded; the child/elder register rule reads `GenerationalSystem`, not a
  catalog flag.
- **C2[17]/40A status (the identity prerequisite):** partially landed.
  `SetupSurvivorSocial()` registers beliefs from authored enrichment, but the
  `InferBeliefProfile(def)` trait heuristic **still exists in production**
  (`src/Main.SurvivorSocial.cs`) as the unenriched fallback, and 40A's
  deletion/registration-discipline tasks are open. **Consequence for this
  plan:** voice only *reads* identity, and the read path
  (`SurvivorEnrichmentService`) is live and production-wired today; the 40A
  remainder is about write-path registration discipline and heuristics
  deletion, which voice never performs. Plan 42 therefore treats 40A's *read
  model* as available groundwork and 40A's *write path* as out of scope, with
  an explicit rule: when `IsEnriched == false` or belief is undeclared, the
  speaker resolves to the authored **default register** — voice never calls,
  reimplements, or depends on `InferBeliefProfile` (§6, §19 phase 0 gate).

## 2.13 Radio voice discipline — ADAPT (register matrix + dedup ledger), LEAVE (lanes)

- `docs/ui/FACTION_VOICE_MATRIX.md`: 12 faction voice registers (vocabulary,
  cadence, sample lines). The source plan's step "reuse the faction matrix
  vocabulary" is scoped down by current evidence: those 12 registers are
  *faction broadcast* registers; survivor registers are a new, small,
  reviewed vocabulary (§11) documented in a new `docs/voice/VOICE_LINE_SPEC.md`
  that *references* the matrix as canon style guidance without importing
  faction IDs into shelter speech (lanes stay separate — 42B rule).
- `src/Host/RadioHostSession.cs`: persisted `_playedBroadcastKeys` dedup
  ledger (`distress:{freq}:{cue}` keys; "an already-heard cue does not replay
  after a reload") — the exact idempotency-ledger precedent for voice
  delivery (§9, §12). `DistressAudioCueResolver`: pure cue resolution,
  explicit IDs, zero RNG, logged-once-missing fallback — the model for the
  optional voice cue field.

## 2.14 Localization key layer (25A/25C reality) — EXTEND

- `Assets/Ashfall.Core/Localization/LocalizationService.cs`: engine-free
  key→string service (registered-key count, `OnMissingKey`, locale change,
  pseudo-locale support); host bridge `src/Localization/AshfallLocalization.cs`
  (`Tr`, `TrFormat`, `TrNamed`) backed by `assets/l10n/strings.csv` (360
  lines, en/de) and Godot `TranslationServer`. This is the existing key layer
  the source plan calls 25A/25C. Voice lines store `text_key`; presentation
  resolves through `AshfallLocalization.Tr(text_key)` at render time; the
  journal stores key + args, not rendered prose (§12).
- `DEC-13` (localization pipeline expansion) and the string freeze (`D22`,
  decision-blocked) are **not** prerequisites: the key service is live; the
  freeze governs mass translation, not adding keyed rows. This plan adds keys
  under the `voice.` prefix and counts them in the new-string report, staying
  under the string-freeze decision's blast radius.

## 2.15 Port contract (36A/36B) — available groundwork, re-verified

- `KNOWN_DEBT.md` `DEBT-PLAN36-PORT-CONTRACT-CLOSURE`: **RETIRED, sealed
  2026-09-19** — "Policy 262 seams / 180 HOST_REQUIRED / **0 DEFERRED**";
  `--port-contract-selftest` registered and green; `PortContractGateTests`
  8/8. The Wave 12 audit's "partially-sealed" note refers to the census row's
  36C long-tail sweep remainder, not to the enforcement infrastructure. The
  retirement evidence holds at current source
  (`Assets/Ashfall.Core/Ports/PortContract.cs`,
  `docs/ci/port_contract_policy.json`, `src/Host/PortContractSelfTest.cs`).
  **Treated as available groundwork, not a blocker:** the voice delivery port
  is declared with `PortContractAttribute` and joins the policy file like any
  other seam.

## 2.16 RNG + save infrastructure — EXTEND

- `Assets/Ashfall.Core/Random/CampaignRngStream.cs`: `CampaignRngManager.
  Fork(streamId, day, actionIndex)`; `CampaignStreamIds` constants (snake_case,
  gated by `CampaignRngSourceGateTests`); StableHash derivation
  (`masterSeed*31337 + StableHash.Of(streamId)*1009 + day*37 + actionIndex`);
  positions capture/restore. No `survivor_voice` stream exists yet — one is
  added (§13).
- Save pattern (house style): registry row in
  `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (e.g. `survivor_social`,
  `journal`), a thin host store via `SaveStoreHub.Checksummed<T>(fileName,
  owner)` (e.g. `src/Host/SurvivorSocialSaveStore.cs`), capture/restore
  through `Main.SaveOrchestrator.cs`. Voice follows this pattern exactly.

## 2.17 Delivery surfaces (all exist today)

| Surface | File | Current role |
|---|---|---|
| Journal (archive) | `Assets/Ashfall.Core/Journal/JournalSystem.cs` + journal book UI | Knowledge-key-deduped entry log, 64-entry bound |
| Daily briefing | `src/Main.Campaign.cs` `ShowBriefingForDay` + `DailyBriefingReportBuilder` | Sectioned day report from `DayAdvancedEventArgs` |
| Survivor detail panel | `src/UI/SurvivorDetailPanel.cs` | Name/profession/worldview/keepsake/needs rows; refreshes on `Needs.OnNeedChanged` |
| HUD | `src/UI/GameHudOverlay.cs` (`HBoxContainer`) | Status strip; **no transient/murmur text surface exists** — the murmur lane is a small additive widget, not a panel (§15) |
| Audio | `src/Audio/AudioEventBridge.cs` | Existing cue families; voice may reference existing cue IDs only |

## 2.18 What does NOT exist (greenfield confirmed)

- No `Assets/Ashfall.Core/Voice/` directory; no `SurvivorVoiceSystem`,
  `VoiceLineCatalog*`, `GetBarkSentence`, or survivor-scoped line selection
  anywhere in `Assets/Ashfall.Core` or `src/` (grep-verified, matching the
  source plan's evidence row 3).
- No `docs/voice/` directory; no `survivor_voice_lines.json`.
- No per-survivor knowledge authority (32C geographic knowledge and 131 rumor
  network are unbuilt proposals; `JournalSystem.Knowledge` is *player*
  knowledge, not speaker knowledge). The knowledge gate therefore uses the
  closed, state-derived `knowledge_class` vocabulary defined in §11 — no new
  simulation.

---

# 3. Required Delta

From current reality to the objective, the minimum delta is:

1. **New Core voice authority** (`Assets/Ashfall.Core/Voice/`): catalog loader,
   read models, selection system, delivery DTOs, ports DTO, cooldown/delivered
   state with capture/restore. Pure C#, zero engine references (Rule 2).
2. **Two new data catalogs** with snake_case schema + `schema_version`
   envelope, validated by a new `CatalogIntegrityValidator` tier.
3. **One new RNG stream id** (`survivor_voice`) and one new save section
   (`survivor_voice`) with registry row and host store.
4. **One host adapter** (`src/Main.SurvivorVoice.cs`) that collects trigger
   events during the day, runs the single end-of-day selection pass in a fixed
   order, and dispatches deliveries to bound sinks; plus sink wiring into the
   journal, briefing, survivor detail panel, and a small HUD murmur widget.
5. **First content slice**: `survivor_perished` × registers × a bounded
   condition set (12–20 lines), keyed strings under `voice.*`.
6. **Verification**: focused test files, a determinism replay harness, an
   unused-line utilization report, integrity tier, port-contract registration.

Explicitly NOT in the delta: no changes to `SurvivorRelationsSystem`,
`SurvivorSocialCoordinator`, `MoraleContagionSystem`, `MemorialSystem`,
`SurvivorFateSystem`, `SleepNarrativeProjection`, `JournalSystem`,
`DayEventVocabulary` mappings, `DayStateChangeEvent`, radio systems, or the
phantom/vinyl catalogs (event subscriptions from the host adapter only).

---

# 4. Evidence

Every load-bearing claim, verified at current source:

| # | Claim | Evidence |
|---|---|---|
| E1 | No survivor line-selection system exists | grep for `voiceline|voice_line|bark|quip|dialogueTree` over `Assets/Ashfall.Core` + `src/`; only hit is `MoraleMarkSystem.GetLaterProse`; no `Assets/Ashfall.Core/Voice/` directory |
| E2 | Journal dedup is knowledge-key based, exactly-once | `JournalSystem.TryAddRawEntry` → `KnowledgeBase.Discover(key)`; `MaxEntries = 64`; `JournalSave` persists entries + knowledge + seq |
| E3 | Deterministic text-from-state precedent is read-only, journal-only, day-keyed fork | `SleepNarrativeProjection.Project`; `Main.TickSleepNarrative` forks `CampaignStreamIds.Psychology` per day; `DEBT-177-SLEEP-EVENT-CONSUMER` RETIRED (do not reopen) |
| E4 | Sleep projection hardcodes English in Core | `SleepNarrativeProjection` `NightmareLines`/`TroubledLines`/`RestfulLines` static arrays — the pattern to copy in shape, not storage |
| E5 | Roster marks prove data-authored barks | `MoraleMarkSystem.cs` `GetLaterProse`; `DutyRosterCatalog.Marks`; `duty_roster_marks.json` (43 defs) |
| E6 | Social systems already raise speech-worthy events | `RationConflictSystem.OnRationConfrontation/OnRationsStolen/OnResentmentBuilt`; `IdeologicalFrictionSystem.OnFrictionDetected/OnRoommateSynergy`; `LeadershipSystem.OnLeaderStressIncreased/OnLeaderBreakRisk`; `MemorialSystem.OnMemorialized/OnMourned`; `CombatTraumaSystem.OnFalseAlarmTriggered` |
| E7 | Identity read path is live in production | `Main.Enrichment.cs` loads `ExpansionEnrichmentCatalog`; `SurvivorEnrichmentService.GetView` consumed by `SurvivorDetailPanel` (Worldview/Keepsake rows); `SurvivorEnrichmentServiceTests` pin labels |
| E8 | 40A write path is NOT finished | `InferBeliefProfile` still in `src/Main.SurvivorSocial.cs`; C2[17] census row `AUDIT-PENDING` |
| E9 | `survivors.json` = 129 defs, no `isChild` | fields `id, displayName, profession, bio, baseHealth, traitIds, activeQuestlineId`; age authority = `GenerationalSystem` (`GetCanonicalChildProfile`) |
| E10 | Day events carry no `causeId` | `DayStateChangeEvent{Kind, SourceOwnerId, PrimaryId, SecondaryId, Numeric}`; `DailyBriefingEntry` reserves `CauseId`/`ActorId` unused |
| E11 | Real trigger kinds differ from the source plan's examples | `DayEventVocabulary` map: `survivor_perished`, `rationing_tier_changed`, `season_changing`, … (`ration_cut`, `season_changed` absent) |
| E12 | Semantic-kind map is totality-gated | `DayEventVocabularyTests`, `DayEventParitySourceGateTests` — new day-event kinds would fail gates; none are added |
| E13 | Port contract enforcement is sealed and live | `DEBT-PLAN36-PORT-CONTRACT-CLOSURE` RETIRED 2026-09-19; 262 seams / 180 HOST_REQUIRED / 0 DEFERRED; `--port-contract-selftest` green |
| E14 | Seeded RNG fork contract | `CampaignRngManager.Fork(streamId, day, actionIndex)`; StableHash v1 derivation; `CampaignStreamIds` snake_case gated |
| E15 | Persisted delivery-dedup precedent | `RadioHostSession._playedBroadcastKeys` (`distress:{freq}:{cue}`); survives reload |
| E16 | Keyed localization service is live | `LocalizationService` + `AshfallLocalization.Tr*`; `assets/l10n/strings.csv` (en/de/pseudo) |
| E17 | Optional cue pattern exists | `DistressAudioCueResolver`: explicit cue ids, zero RNG, missing cue logs once and text continues |
| E18 | Save section + store pattern | `SaveSectionRegistry` rows; `SurvivorSocialSaveStore` via `SaveStoreHub.Checksummed<T>` |
| E19 | DEC-17 retires character RPG skill trees for voice-adjacent work | `DECISION_REGISTER.md` DEC-17 RETIRED: "Radio station production operates through equipment and program production, not individual character RPG skill trees" |
| E20 | DEC-11 defers VO production | `DEC-11` DEFERRED-WITH-CONDITION: full VO deferred until string freeze + loudness calibration; voice lines stay text-first with optional existing cue ids |
| E21 | Narrow-ports idiom exists | `MoraleContagionPorts` delegate DTO, host-bound to canonical owners |
| E22 | The Plan 49 audit gates this package | `A1_PLAN49_PREREQUISITE_AUDIT.md`: Plan 42 `AUDIT-PENDING`; promotion of C1[16] requires a Plan 42 premise/claim decision — this document is that decision artifact |
| E23 | Journal capacity is bounded | `MaxEntries = 64`, oldest-first eviction — voice journal writes must be budgeted (§17 F-12) |
| E24 | HUD has no transient text lane | `GameHudOverlay : HBoxContainer` only; murmur lane is new additive UI (§15), not a panel |
| E25 | Social coordinator is the wrong owner for voice | `SurvivorSocialCoordinator` owns seven mechanics systems + one aggregate save; voice reads across owners like the sleep projection — separate small authority |

---

# 5. Existing Extension Seams

The plan extends these seams and no others:

1. **Identity read seam:** `SurvivorEnrichmentService.GetView(survivorId, def)`
   → register resolution input (profession id, belief profile id, phantom
   background id, `IsEnriched`). No new identity authority.
2. **Age-class seam:** `GenerationalSystem.GetCanonicalChildProfile(id, day)`
   (host-bound through the existing generational session) → child/elder
   register + topic restriction input.
3. **Social event seams:** host subscriptions to
   `RationConflictSystem.OnRationConfrontation/OnRationsStolen`,
   `IdeologicalFrictionSystem.OnFrictionDetected`,
   `LeadershipSystem.OnLeaderStressIncreased/OnLeaderBreakRisk`,
   `MemorialSystem.OnMemorialized/OnMourned`,
   `CombatTraumaSystem.OnFalseAlarmTriggered`. Read-only relations state via
   `SurvivorRelationsSystem.TryGetRelationship` / `RelatedIds`.
4. **Day-event seam:** the host adapter consumes the day-event batch
   (`DayAdvancedEventArgs.AllEvents()`, same batch the briefing consumes) at
   end of day; no day-owner insertion, no `DayStateChangeEvent` change.
5. **Journal seam:** `JournalSystem.TryAddRawEntry(knowledgeKey, text, author,
   day)` with voice keys `voice_line:{lineId}:{speakerId}:{day}` (event class)
   — dedup + archive for free.
6. **Briefing seam:** `DailyBriefingReportBuilder` gains one additive
   renderer (`AppendVoiceLines`) beside `AppendCrisisWarnings` precedent —
   new method only; existing switch cases untouched; voice entries carry
   `CauseId`/`ActorId` through the *reserved* fields, finally consuming them
   without a wire-format change.
7. **Detail-panel seam:** `SurvivorDetailPanel` gains one "recent remark" row
   fed by a read model query (latest delivered line for this survivor), beside
   the existing Worldview/Keepsake rows.
8. **RNG seam:** additive `CampaignStreamIds.SurvivorVoice = "survivor_voice"`.
9. **Save seam:** additive `survivor_voice` section + registry row + host
   store, house pattern (`SaveStoreHub.Checksummed`).
10. **Integrity seam:** additive `CatalogIntegrityValidator.ValidateVoiceCatalogs`
    static method following `ValidateDistressSignalStages` shape.
11. **Port seam:** `PortContractAttribute` declarations on the voice delivery
    port + one row set in `docs/ci/port_contract_policy.json`, validated by
    the existing `--port-contract-selftest`.
12. **Localization seam:** `voice.*` keys in `assets/l10n/strings.csv`
    resolved at presentation via `AshfallLocalization.Tr`.
13. **Audio seam (optional):** `audio_cue` field validated against the
    existing cue catalog; playback through the existing bridge only on
    first-delivery edges, deduped by the delivered ledger (E15 idiom).

---

# 6. Proposed Architecture

```text
                         ┌──────────────────────────── Core (engine-free) ───────────────────────────┐
 day events (batch) ───► │                                                                        │
 social/trauma events ─► │  SurvivorVoiceSystem                                                   │
 (buffered by host)      │   ├─ Trigger intake (closed vocabulary)                                 │
                         │   ├─ MatchCandidates(trigger, speaker, conditions)                    │
 identity view ─────────►│   ├─ Eligibility filters: alive → knowledge class → age class →        │
 (EnrichmentService)     │   │     conditions (needs bands, affinity bands, day window, leader)   │
 relations (read) ──────►│   ├─ Register specificity order: survivor_id > belief > profession >   │
 needs (read) ──────────►│   │     archetype > default                                            │
 memorial (read) ───────►│   ├─ Deterministic weighted pick over ordinal-sorted candidates        │
 roster (read) ─────────►│   ├─ Cooldown + no-immediate-repeat + family cooldown bookkeeping      │
                         │   └─ Emits VoiceLineSelection (line id, speaker, text_key, class, cue?) │
                         │                                                                        │
                         │  SurvivorVoiceCatalog (loaded JSON, immutable at runtime)               │
                         │  SurvivorVoiceState (cooldowns, delivered ledger) — save section        │
                         └────────────────────────────────────────────────────────────────────────┘
                                              │ VoiceLineDelivery (DTO, port-contracted)
              ┌───────────────────────────────┼────────────────────────────────┐
              ▼                               ▼                                ▼
      JournalSystem.TryAddRawEntry   DailyBriefingReportBuilder         HUD murmur widget /
      (event class, always archived  .AppendVoiceLines (additive,       SurvivorDetailPanel
       once per voice key)            budgeted)                          (murmur class)
                                              │
                                              ▼
                              optional audio cue (existing catalog ids only,
                              first-delivery edge only, never a gameplay input)
```

Architectural rules, binding on the implementation:

- **Voice is a deterministic projection of existing state** (source plan §1).
  It owns: catalog, matching, selection, cooldown, delivered ledger, delivery
  proposals. It does NOT own: relationships, grief, needs, knowledge creation,
  warnings, consequences, audio assets, dialogue choices (corpus §78).
- **One selection pass per day, one fixed order.** The host buffers trigger
  candidates as events arrive, then runs a single pass at end of day sorted by
  `(trigger intake ordinal)`, where the ordinal is assigned from a sort of
  `(kind, sourceOwnerId, primaryId, secondaryId)` — never from dictionary
  iteration order. This is what makes the replay proof possible (§13).
- **No selection at event time.** Events arrive in owner-tick order which is
  fixed but fragile; buffering + sorted end-of-day selection makes voice
  immune to future day-owner reordering.
- **Density budgets are data, not code.** Per-surface and per-speaker caps are
  rows in the voice register catalog's `attention_budget` block (§11).
- **Interruption class is structurally absent.** The `VoiceDeliveryClass` enum
  has exactly `Murmur` and `Event` members; adding an `Interrupt` member is a
  governance change, not a config change.
- **Register is static authored metadata, never progression** (DEC-17, §21).
- **SleepNarrativeProjection stays as-is.** Voice does not absorb sleep beats;
  they are a separate, sealed read model. If a future package wants sleep
  lines keyed, that is a localization-debt package, not this one.

---

# 7. Ownership Matrix

| Concern | Owner (existing) | Voice relationship |
|---|---|---|
| Survivor identity (profession/belief/keepsake/phantom) | `survivors.json` + `ExpansionEnrichmentCatalog` (C2[17] authority) | Read via `SurvivorEnrichmentService` |
| Age class | `GenerationalSystem` | Read via port |
| Needs/morale state | `NeedsSystem` | Read via port |
| Pair relationships/grievances | `SurvivorRelationsSystem` (inside coordinator) | Read via port; events subscribed |
| Ration conflict events | `RationConflictSystem` | Events subscribed |
| Friction events | `IdeologicalFrictionSystem` | Events subscribed |
| Leadership state | `LeadershipSystem` | Read via port; events subscribed |
| Deaths/memorial | `SurvivorFateSystem` → `MemorialSystem` | Day event + memorial events consumed |
| Trauma/false alarms | `CombatTraumaSystem` + mental-health record | Events/state read via port |
| Line catalog, selection, cooldown, delivered ledger | **`SurvivorVoiceSystem` (NEW, sole authority)** | Owns |
| Journal archive | `JournalSystem` | Sink via `TryAddRawEntry` |
| Briefing | `DailyBriefingReportBuilder` | Sink via additive renderer |
| HUD murmur / detail panel | host UI | Sink via delivery DTO |
| Audio cues | `AudioEventBridge` + cue catalog | Optional consumer of `audio_cue` id |
| Localization | `LocalizationService` / `AshfallLocalization` | Resolves `text_key` at presentation |
| Port validation | Plan 36 infrastructure | Validates voice delivery port |

Claim hygiene: at authoring time, `WORKTREE_OWNERSHIP.md` active claims
(`claim-xp-wave1-difficulty-2026-09-18`,
`claim-wave11-part2-execution-2026-09-18`) cover `Assets/Ashfall.Core/
Difficulty/`, endgame/completion-history paths, port-contract tooling, and
governance docs — **no overlap** with this package's file set (§20). The
shared integrator paths this package touches additively
(`CatalogIntegrityValidator.cs`, `CampaignRngStream.cs`,
`SaveSectionRegistry.cs`, `Main.CampaignOwners.cs` if a day-owner is used,
generated docs) must be claimed per the ledger before editing, per Rule 6.

---

# 8. Data Flow

```text
SETUP (once per session, idempotent):
  VoiceLineCatalogLoader.Load(dataDir)
    → CatalogIntegrityValidator.ValidateVoiceCatalogs (boot gate)
    → SurvivorVoiceSystem.BindCatalog(catalog)
    → SurvivorVoiceState restore (cooldowns + delivered ledger)
    → host binds SurvivorVoicePorts to canonical owners
    → host binds sinks (journal / briefing / murmur / detail)
    → port contract selftest asserts VOICE sinks bound

DURING A DAY (event intake, no selection):
  producers tick → day events collected by CampaignDayCoordinator
  social/trauma events fire → host adapter buffers VoiceTriggerCandidate
    {kind, speakerHint(s), cause tuple, intake payload refs} — no RNG, no selection

END OF DAY (one deterministic pass, host adapter):
  1. merge day-event-derived candidates + buffered social candidates
  2. stable-sort by (kind, sourceOwnerId, primaryId, secondaryId)
  3. for each candidate, in order, ordinal i:
       speakers = ResolveSpeakers(candidate)  // roster ∩ eligibility, ordinal-sorted
       for each speaker s (ordinal-sorted):
         rng = CampaignRngManager.Fork("survivor_voice", day, i * 256 + speakerOrdinal)
         selection = SurvivorVoiceSystem.TrySelect(candidate, s, ports, rng, state)
         if selection == null → continue
         delivery = BuildDelivery(selection, candidate, day)
         deliveredLedger check (lineId, speakerId, triggerKind, day) → skip if present
         dispatch to sinks by delivery_class under attention budgets
         journal archive (event class) → TryAddRawEntry(voice key)
         mark delivered ledger + cooldowns (voice state, dirty flag)
  4. emit one "survivor_voice_ticked" heartbeat day event (counts only:
     eligible/selected/delivered/suppressed) — classified Heartbeat, so it
     never appears in the briefing

SAVE: voice state captured into the campaign envelope as section survivor_voice
LOAD: restore voice state → delivered ledger suppresses re-delivery →
      rebind sinks → continue; journal re-renders history from its own save
```

Invariant: nothing in the flow mutates any state owned by another system.
The only writes are: voice state (own section), journal entries (via the
journal's public dedup API), and UI presentation state.

---

# 9. State Model

`SurvivorVoiceState` (persisted, the ONLY voice-owned mutable state):

```csharp
[Serializable] public sealed class SurvivorVoiceState
{
    public string systemId = SurvivorVoiceSystem.SystemId;   // "survivor_voice"
    public int schemaVersion = 1;
    // Per-survivor line cooldown: "survivorId|lineId" → last fired day.
    public List<VoiceCooldownEntry> cooldowns = new();        // sorted on capture
    // Per-survivor family cooldown: "survivorId|familyId" → last fired day.
    public List<VoiceCooldownEntry> familyCooldowns = new();  // sorted on capture
    // Delivery idempotency: "lineId|speakerId|triggerKind|day".
    public List<string> deliveredKeys = new();                // ordinal-sorted, bounded
    // Last delivered line per speaker (no-immediate-repeat across save/load).
    public List<VoiceLastLineEntry> lastLinePerSpeaker = new();
    // Bounded statistics for the utilization report (per line id).
    public List<VoiceLineStatsEntry> lineStats = new();       // eligible/selected/delivered/blocked counts
}
```

Field rules:

- `deliveredKeys` is bounded: entries older than a data-authored retention
  window (default 30 days; journal already holds the durable archive) are
  pruned on capture. The ledger exists to suppress re-delivery across
  reload/rebind, not to be history — history is the journal's job (Rule 5).
- All lists are captured ordinal-sorted; restore validates and re-sorts rather
  than trusting order; culture-invariant serialization per the save checksum
  contract (no `float` in the state at all — day numbers and counts only).
- `lineStats` is additive diagnostic state; it never gates selection. If a
  legacy save lacks it, it starts empty (documented, not migrated).
- Explicit non-goals for state: no per-line text, no resolved prose, no
  survivor identity copies, no relationship caches, no knowledge snapshots.
  Everything else is derived at selection time from live canonical state.

---

# 10. API/Contracts

All types engine-free, in namespace `Ashfall.Core.Voice`.

```csharp
// ── Catalog DTOs (JSON-bound, snake_case in data) ─────────────────────
public sealed class SurvivorVoiceCatalog
{
    public IReadOnlyList<VoiceLineDef> Lines { get; }
    public IReadOnlyDictionary<string, VoiceRegisterDef> Registers { get; }
    public VoiceAttentionBudget Budget { get; }
    public bool TryGetLine(string id, out VoiceLineDef line);
    public IReadOnlyList<VoiceLineDef> LinesForTrigger(string triggerKind); // pre-indexed, ordinal-sorted
}

public sealed class VoiceLineDef
{
    public string Id;                 // voice_* snake_case, globally unique
    public VoiceSpeakerSelector Speaker;   // survivor_id / belief_profile_id / profession_id / archetype_id (exactly one set; null = default)
    public string Trigger;            // closed trigger vocabulary (§11)
    public VoiceConditions Conditions;     // nullable families
    public string Register;           // register id; empty = any
    public string[] LexiconTags;      // closed lexicon vocabulary
    public string TextKey;            // voice.* localization key (never prose)
    public string FamilyId;           // repetition family (e.g. grief, ration_grievance)
    public int Weight;                // > 0, integer permille-style weighting
    public int CooldownDays;          // >= 0 per-speaker per-line
    public int FamilyCooldownDays;    // >= 0 per-speaker per-family
    public VoiceDeliveryClass DeliveryClass; // Murmur | Event
    public string KnowledgeClass;     // public_shelter | participant | relationship_pair | background | leader_only | self_state
    public string AgeClass;           // any | adult_only | child_only (child lines never reference mass-casualty triggers — validated)
    public string AudioCue;           // optional; must resolve in cue catalog
    public int DayMin, DayMax;        // optional window; 0 = unbounded
}

// ── Ports (MoraleContagionPorts idiom; host-bound, null-safe defaults) ──
public sealed class SurvivorVoicePorts
{
    public Func<IReadOnlyList<string>> AliveSurvivors;          // ordinal-stable roster order
    public Func<string, SurvivorEnrichmentView> IdentityView;   // SurvivorEnrichmentService
    public Func<string, bool> IsChild;                          // GenerationalSystem projection
    public Func<string, float> GetNeed;                         // NeedsSystem per-kind read (morale polarity documented)
    public Func<string, string, bool> TryRelationshipBand;      // Relations: affinity/resentment/grief band check
    public Func<string, bool> IsLeader;                         // LeadershipSystem.CurrentLeaderId
    public Func<string, float> GetLeaderStress;                 // LeadershipSystem
    public Func<string, bool> HasActiveTrauma;                  // mental-health record read
    public Func<string, string> GetDutyRole;                    // DutyRosterSystem
    public Func<string, bool> WasPresentAt;                     // participant check (expedition roster / room co-presence) — host truth
}

// ── Trigger intake ─────────────────────────────────────────────────────
public readonly struct VoiceTriggerCandidate
{
    public readonly string TriggerKind;     // closed vocabulary
    public readonly string SpeakerHintId;   // empty = resolve from roster
    public readonly string SecondaryId;     // e.g. the other party in a confrontation
    public readonly string CauseKey;        // (kind|sourceOwner|primaryId|day) tuple string — cause attribution
    public readonly int Day;
}

// ── Selection result + delivery DTO ────────────────────────────────────
public sealed class VoiceLineSelection
{
    public string LineId; public string SpeakerId; public string TextKey;
    public string Register; public string FamilyId;
    public VoiceDeliveryClass DeliveryClass; public string AudioCue; // may be empty
}

public sealed class VoiceLineDelivery
{
    public string LineId; public string SpeakerId; public string TriggerKind;
    public string CauseKey; public int Day;
    public VoiceDeliveryClass DeliveryClass; public string TextKey;
    public string AudioCue;                  // optional, existing ids only
    public string DeliveryKey;               // lineId|speakerId|triggerKind|day
}

// ── The system ─────────────────────────────────────────────────────────
public sealed class SurvivorVoiceSystem
{
    public const string SystemId = "survivor_voice";
    public event Action<VoiceLineDelivery> OnLineSelected;   // host dispatches to sinks

    public void BindCatalog(SurvivorVoiceCatalog catalog);
    // Pure selection for one candidate+speaker. Never mutates anything except
    // voice state bookkeeping (cooldown/last-line/stats), and only when a line
    // is actually selected.
    public bool TrySelect(in VoiceTriggerCandidate candidate, string speakerId,
                          SurvivorVoicePorts ports, ISeededRng rng,
                          out VoiceLineSelection selection);
    public SurvivorVoiceState CaptureState();
    public void RestoreState(SurvivorVoiceState state);       // non-operative; never re-fires events
    public VoiceUtilizationReport BuildUtilizationReport();   // per-line eligible/selected/delivered/blocked
}
```

Contracts:

- `TrySelect` is the sole selection entry point. Candidate order inside it:
  catalog trigger index → speaker selector specificity → eligibility filters →
  knowledge class → age class → cooldown/no-repeat → ordinal sort by line id →
  weighted pick by `rng.Next(0, totalWeight)` walk in that sorted order.
- `OnLineSelected` fires at most once per accepted candidate-speaker pair per
  day; the host's delivered-ledger check happens before dispatch, so the event
  never fires twice for the same `DeliveryKey` even across reload (the ledger
  is consulted inside the end-of-day pass, and `TrySelect` refuses a
  `DeliveryKey` already in the ledger).
- Ports are required for: `AliveSurvivors`, `IdentityView`. All others have
  null-safe defaults that **exclude** lines needing them (a line with an
  affinity condition simply never matches when `TryRelationshipBand` is
  unbound — fail-closed toward silence, and the integrity report counts
  unbound-port exclusions so they cannot rot silently).
- The briefing sink is a new additive static
  `DailyBriefingReportBuilder.AppendVoiceLines(DailyBriefingReport,
  IReadOnlyList<VoiceLineDelivery>, Func<string,string> resolveText, int maxEntries)`
  — mirrors the `AppendCrisisWarnings` precedent; no existing case changes.

---

# 11. Data Changes

Two new files in `Assets/StreamingAssets/Data/`. Both carry
`"schema_version": 1`, snake_case keys, and pass
`CatalogIntegrityValidator.ValidateVoiceCatalogs` (new tier).

## 11.1 `survivor_voice_lines.json`

```json
{
  "schema_version": 1,
  "lines": [
    {
      "id": "voice_perished_medic_clinical_01",
      "speaker": { "survivor_id": null, "belief_profile_id": null,
                   "profession_id": "nurse", "archetype_id": null },
      "trigger": "survivor_perished",
      "conditions": {
        "need_bands": { "morale_min": null, "morale_max": null },
        "affinity_with_deceased_min": 20,
        "day_min": null, "day_max": null,
        "leader_only": false
      },
      "register": "clinical",
      "lexicon_tags": ["medical"],
      "text_key": "voice.perished.medic.clinical_01",
      "family_id": "grief",
      "weight": 1000,
      "cooldown_days": 30,
      "family_cooldown_days": 3,
      "delivery_class": "event",
      "knowledge_class": "public_shelter",
      "age_class": "adult_only",
      "audio_cue": ""
    }
  ]
}
```

Field-by-field validation rules (the integrity tier — every rule names
catalog/file/line id/field/value/rule in its error, per the distress-stage
validator shape):

| Field | Rule |
|---|---|
| `id` | required, `voice_` prefixed, snake_case, globally unique across both voice files |
| `speaker` | object always present; at most one of the four selector ids non-null; `survivor_id` must resolve in `survivors.json`; `belief_profile_id` must resolve against the enrichment belief vocabulary; `profession_id` must resolve against authored professions |
| `trigger` | must be in the closed voice trigger vocabulary (§11.3); day-event-backed triggers must be registered in `DayEventVocabulary` with a non-Heartbeat kind |
| `conditions.*` | band mins ≤ maxes; affinity in [-100,100]; need bands reference only canonical `NeedKind` names; `day_min ≤ day_max` |
| `register` | must resolve in `survivor_voice_registers.json`; empty = any |
| `lexicon_tags` | each must be in the closed lexicon vocabulary declared in the register file |
| `text_key` | required, `voice.` prefixed, must resolve in `assets/l10n/strings.csv` (boot-checked via `LocalizationService.OnMissingKey`-backed audit; missing key = integrity error) |
| `family_id` | required, snake_case; families with identical id must share cooldown semantics |
| `weight` | integer > 0, ≤ 10000 |
| `cooldown_days` / `family_cooldown_days` | integer ≥ 0 |
| `delivery_class` | `murmur` or `event` only (no interruption value exists) |
| `knowledge_class` | one of the six closed values (§10); `leader_only` requires `conditions.leader_only` consistency |
| `age_class` | `any` / `adult_only` / `child_only`; **content-policy rule:** `child_only` lines may not use triggers in the mass-casualty set (`survivor_perished` is allowed for a child *reaction* line only when `register` is in the reviewed child-safe set — the slice authors adult-only for deaths) |
| `audio_cue` | empty or must resolve in the existing audio cue catalog (validated structurally like distress cues; semantic missing-cue policy = log once, text continues, per E17) |
| `day_min/day_max` | ≥ 0, min ≤ max |

Coverage rules (same tier):

- Every survivor in `survivors.json` resolves to at least the `default`
  register through the register map (§11.2) — enforced by a validator walk
  over the catalog, not by authoring 129 rows.
- Every line family has ≥ 2 lines or an explicit `"single_line": true`
  opt-out comment field (prevents dead no-repeat behavior).
- Every register used by any line exists; every register defined is used by at
  least one line or is marked `"reserved": true` (mirrors 40A's unused-profile
  rule).

## 11.2 `survivor_voice_registers.json`

```json
{
  "schema_version": 1,
  "lexicon_vocabulary": ["medical", "mechanical", "devotional", "bureaucratic",
                         "military", "domestic", "outdoor"],
  "registers": [
    { "id": "clipped",      "description_key": "voice.register.clipped.desc" },
    { "id": "bureaucratic", "description_key": "voice.register.bureaucratic.desc" },
    { "id": "devotional",   "description_key": "voice.register.devotional.desc" },
    { "id": "clinical",     "description_key": "voice.register.clinical.desc" },
    { "id": "weary",        "description_key": "voice.register.weary.desc" },
    { "id": "guarded",      "description_key": "voice.register.guarded.desc" },
    { "id": "plain",        "description_key": "voice.register.plain.desc", "is_default": true }
  ],
  "register_rules": [
    { "match": { "belief_profile_id": "religious_faith" },          "register": "devotional" },
    { "match": { "belief_profile_id": "military_discipline" },      "register": "clipped" },
    { "match": { "profession_id": "nurse" },                        "register": "clinical" },
    { "match": { "profession_id": "machinist" },                    "register": "clipped" },
    { "match": { "belief_profile_id": "atheist_rationalist" },      "register": "clinical" },
    { "match": { "belief_profile_id": "collectivist_solidarity" },  "register": "bureaucratic" }
  ],
  "child_register_override": "plain",
  "attention_budget": {
    "murmur_per_day_surface": 4,
    "event_per_day_briefing": 3,
    "event_per_day_journal": 6,
    "per_speaker_per_day": 2,
    "flashback_per_day": 2,
    "crisis_suppresses_murmurs": true
  }
}
```

Register resolution order (deterministic, documented in
`docs/voice/VOICE_LINE_SPEC.md`): exact `register_override` on the survivor's
enrichment row (future, not required for the slice) → first matching
`register_rules` row (rules are evaluated in **file order**, first match wins;
duplicate-match ambiguity is a validator error) → child override when
`IsChild` → `is_default` register. The vocabulary is deliberately small (7);
adding a register requires a `VOICE_LINE_SPEC.md` edit in the same commit —
the spec is the review gate, mirroring the faction matrix's role for radio.

The register vocabulary is **new**; it does not import the 12 faction ids from
`FACTION_VOICE_MATRIX.md`. The spec file cites the matrix as style canon and
states the lane rule: faction registers belong to the outside world's radio;
survivor registers belong to the shelter interior.

## 11.3 Closed voice trigger vocabulary (initial)

Day-event-backed (kind must exist in `DayEventVocabulary`, non-Heartbeat):
`survivor_perished`, `child_lost`, `rationing_tier_changed`,
`rationing_blocked`, `portions_spoiled`, `season_changing`,
`weather_unexpected_storm`, `medical_admitted`, `medical_discharged`,
`duty_vacated`, `social_dispute_unresolved`, `social_dispute_mediated`,
`expedition_milestone`, `memorial_checked`, `power_brownout_began`,
`sanitation_spill`, `hazard_warning`.

Social-event-backed (raised by existing systems, buffered by the host
adapter; names are voice-local constants, validated against a static map in
the catalog file): `ration_confrontation`, `ration_theft`,
`friction_ignited`, `roommate_synergy`, `leader_break_risk`,
`leader_designated`, `memorial_mourned`, `false_alarm`,
`belonging_lost`, `belonging_gifted`.

The first vertical slice authors content for exactly ONE trigger:
`survivor_perished` (12–20 lines across 4 registers × bounded conditions).
All other triggers exist in the vocabulary with zero lines until their
content phase (§19); the utilization report lists them as `authored=0`, which
is a report row, not an error.

## 11.4 String keys

English values for the slice are added to `assets/l10n/strings.csv` under
`voice.*` (key, en, de empty, source = `survivor_voice_lines.json`). No German
translation in this package (DEC-13 deferred); the CSV format tolerates empty
locale columns, and pseudo-locale rendering proves the key path end-to-end.

---

# 12. Save/Load

- **New section:** `survivor_voice` in `SaveSectionRegistry.cs`
  (`new("survivor_voice", "SaveSurvivorVoice", "SetupSurvivorVoice", "social",
  "Survivor voice selection cooldowns and delivered-line idempotency ledger")`)
  + `survivor_voice_save.json` row in `SectionFileNames`. The section-count
  gate and generated save-store matrix are regenerated by their owning
  generators, never hand-edited (Rule: generated outputs).
- **Host store:** `src/Host/SurvivorVoiceSaveStore.cs` — thin
  `SaveStoreHub.Checksummed<SurvivorVoiceState>` façade, identical in shape to
  `SurvivorSocialSaveStore`.
- **Capture/restore:** `Main.SurvivorVoice.cs` `SaveSurvivorVoice()` /
  `FlushSurvivorVoiceIfDirty()` beside the social equivalents;
  `Main.SaveOrchestrator.cs` gains the two calls in the same position as
  other sections (integrator path — claim before editing).
- **Legacy saves:** no voice section ⇒ empty state: no cooldowns, empty
  delivered ledger, empty stats. First post-load day may therefore select a
  line that "would have" been on cooldown pre-upgrade — documented one-time
  behavior, never a crash, never a duplicate *within* the post-upgrade session
  (the ledger starts recording immediately).
- **Newer saves on older builds:** the checksum envelope fails version
  negotiation per the house pattern (refuse, never guess).
- **Journal interaction:** voice journal entries persist inside the existing
  journal save (they are ordinary entries with `voice_line:*` knowledge keys).
  Deleting voice state does not delete the archive; restoring an old journal
  with voice keys but no voice state is coherent (the lines are history).
- **Restore is non-operative** (the `MoraleContagionSystem.RestoreState`
  contract): restoring cooldowns/ledger never re-fires `OnLineSelected`,
  never re-delivers, never writes journal entries.

---

# 13. Determinism

Full specification; this section is the contract the replay test pins.

1. **Stream:** additive `CampaignStreamIds.SurvivorVoice = "survivor_voice"`
   (snake_case; passes `CampaignRngSourceGateTests`). Seed derivation is the
   existing StableHash v1 formula: `masterSeed*31337 +
   StableHash.Of("survivor_voice")*1009 + day*37 + actionIndex` — the voice
   stream cannot perturb any other stream, and adding it changes no existing
   derived seed (dot-free snake_case namespace, same as the Plans 210–213
   streams).
2. **Fork scheme:** per selection attempt:
   `Fork(SurvivorVoice, day, candidateOrdinal * 256 + speakerOrdinal)`.
   `candidateOrdinal` is the index of the trigger candidate in the
   stable-sorted end-of-day list (sort key: `triggerKind`, then
   `SpeakerHintId`, then `SecondaryId`, then `CauseKey`, all ordinal string
   compares). `speakerOrdinal` is the speaker's index in the ordinal-sorted
   eligible-speaker list. The ×256 spacing leaves room for roster sizes
   (current cast 129) without collision; a roster over 256 is a validator
   warning, not silent collision.
3. **No RNG outside the fork:** candidate matching, filtering, and sorting use
   zero randomness; the weighted pick is the only `rng.Next` call, exactly one
   per accepted (candidate, speaker) pair, walking the ordinal-sorted
   candidate list with cumulative weights. Rejected candidates consume no RNG,
   so content edits that remove ineligible lines do not shift the sequence.
4. **Cooldown/read state:** cooldowns and no-immediate-repeat read persisted
   voice state only; they never read wall-clock time, `GetHashCode`, or
   dictionary enumeration order (all catalog indexes are built once at load
   into ordinal-sorted arrays).
5. **Save/load parity:** because selection state is (catalog, live canonical
   state, voice state, day, seed), and all are restored before the end-of-day
   pass runs, an interrupted run (save mid-day, restore, finish day) produces
   the identical delivered sequence as a continuous run. The one structural
   requirement: the host buffers trigger candidates into a list that is part
   of the **pre-day snapshot** pattern used by day owners, OR (simpler, chosen
   here) the adapter derives candidates **only** from sources that are
   themselves persisted/replayable — the day-event batch (rebuilt from owner
   reports) and social events (re-raised on the deterministic tick). Because
   the social systems re-raise the same events on the same day after a
   restore (their state is saved), and the voice adapter buffers by content
   (not by subscription order), the end-of-day candidate list is identical.
   The replay harness in §18 proves this with a mid-day save/restore.
6. **Restore ordering:** restore voice state after catalogs bind and before
   the first end-of-day pass; restoring after a partially-run day replays the
   whole day deterministically (the delivered ledger suppresses any line whose
   `DeliveryKey` was already recorded, so even a double-pass cannot
   double-deliver).
7. **Culture/formatting:** voice state contains ints and strings only; no
   float formatting anywhere, so the save checksum is culture-invariant by
   construction.
8. **Prohibited:** `System.Random`, `Guid.NewGuid`, `DateTime`,
   `Dictionary` iteration order, UI-frame-time randomness, audio-completion
   callbacks feeding selection.

---

# 14. System/Event Wiring

Host composition (new `src/Main.SurvivorVoice.cs`, partial `Main`):

```text
SetupSurvivorVoice()                      // idempotent, manifest-registered like peers
  ├─ VoiceLineCatalogLoader.Load(_dataDir)
  ├─ CatalogIntegrityValidator — boot tier already covers it; loader reuses diagnostics
  ├─ _survivorVoice = new SurvivorVoiceSystem(); BindCatalog(catalog)
  ├─ restore via SurvivorVoiceSaveStore.TryLoad()
  ├─ Bind ports:
  │    AliveSurvivors   ← _survivors.RosterState alive ids (existing order)
  │    IdentityView     ← _enrichmentService.GetView(id, def)
  │    IsChild          ← _generational.GetCanonicalChildProfile(id, _simDay) != null
  │    GetNeed          ← _survivors.Needs (read-only band query)
  │    TryRelationshipBand ← _survivorSocial.Relations.TryGetRelationship bands
  │    IsLeader/GetLeaderStress ← _survivorSocial.Leadership
  │    HasActiveTrauma  ← EnsureSurvivorMentalHealth().GetOrCreateRecord (read-only)
  │    GetDutyRole      ← _dutyRoster.Roster.GetRoleOf
  │    WasPresentAt     ← expedition participation + room co-assignment (existing owners)
  └─ Subscribe (buffer-only handlers):
       _survivorSocial.Ration.OnRationConfrontation   → buffer(ration_confrontation, a, b)
       _survivorSocial.Ration.OnRationsStolen         → buffer(ration_theft, thief, victim)
       _survivorSocial.Friction.OnFrictionDetected    → buffer(friction_ignited, a, b)
       _survivorSocial.Friction.OnRoommateSynergy     → buffer(roommate_synergy, a, b)
       _survivorSocial.Leadership.OnLeaderBreakRisk   → buffer(leader_break_risk, id)
       _survivorSocial.Leadership.OnLeaderDesignated  → buffer(leader_designated, id)
       _memorial.OnMemorialized / OnMourned           → buffer(memorial_mourned …)
       _combatTrauma.OnFalseAlarmTriggered            → buffer(false_alarm, id)
       _survivorSocial.Belongings events              → buffer(belonging_lost/gifted)
```

End-of-day pass: called from the day-advance completion path where
`ShowBriefingForDay` already consumes `DayAdvancedEventArgs` — the voice pass
runs **before** the briefing build so briefing voice entries render in the
same modal, and after all owners ticked (it reads the completed batch).
Day-event-backed triggers are derived by scanning `args.AllEvents()` for
kinds in the voice trigger map (no new day owner is required for the slice;
if a future phase needs mid-tick voice, a late-phase
`SurvivorVoiceDayOwner` is the upgrade path — deferred, §22).

Sink dispatch:

- `Event` class → `AppendVoiceLines` list for the briefing + always
  `_journal.TryAddRawEntry($"voice_line:{lineId}:{speakerId}:{day}",
  resolvedText, authorAdapter, day)`. Author adapter: a tiny
  `ISurvivorAuthor` implementation wrapping the speaker id + display name
  (the journal already supports survivor authors via `JournalCorpusAdapter`'s
  author map idiom).
- `Murmur` class → HUD murmur queue + detail-panel "recent remark" read
  model. Murmurs are additionally journaled only when their `family_id` is in
  the data-authored `journal_families` list (grief, confrontation); ambient
  murmurs stay ephemeral on screen. This is the source plan's "journal as
  archive" rule with an honest budget against E23 (64-entry cap).
- Crisis attenuation: when the crisis predictor or an active decision modal is
  up (host knows both), murmur dispatch is suppressed for the day; event-class
  lines still journal (record deferred, never discarded — corpus §42B-J).

---

# 15. Godot Integration

- **Briefing:** `DailyBriefingReportBuilder.AppendVoiceLines` (Core,
  additive) renders a "Voices" section between existing sections; entries
  carry `ActorId = speakerId`, `CauseId = causeKey` (finally consuming the
  reserved fields, E10), text resolved from keys at build time by the host's
  resolver delegate. Budget: `event_per_day_briefing` cap, deterministic
  overflow drop (lowest weight first, ties by line id) — dropped lines still
  journal (archive is not budgeted the same way; it is bounded by
  `event_per_day_journal`).
- **Survivor detail panel:** one additive row group "Last heard:" showing the
  latest delivered line for the selected survivor (from a read-only query on
  voice state: last `DeliveryKey` per speaker + text key resolution), plus
  register label only when the identity is player-known under the 40C
  knowledge rule — for the slice, register is **not shown** (no compatibility-
  meter-style leakage; the panel shows the words, not the wiring).
- **HUD murmur lane:** a small additive widget on `GameHudOverlay` — one line
  at a time, speaker display name + text, auto-timeout (~6s at 15 FPS), Esc/Enter
  dismiss, never steals focus, never overlaps the alert strip (it yields to
  the existing alerts lane). This is a widget, not a panel: no route, no
  registry row, no modal. Keyboard/controller behavior preserved (dismiss
  maps to the existing UI-cancel action).
- **Audio:** if `audio_cue` set, the host plays it through the existing
  bridge on the first-delivery edge only; the delivered ledger prevents
  replay after reload (E15 idiom). No new cue family, no VO (DEC-11).
- **Accessibility:** murmur and briefing entries are plain text through the
  existing theme constants; not color-only; text-scale respected; murmur
  never the sole carrier of a mechanical fact (§16 warning-parity rule,
  enforced by test §18).
- **Localization:** all display text via `AshfallLocalization.Tr(text_key)`;
  pseudo-locale run in the a11y selftest proves no hardcoded string.

---

# 16. Narrative/Content Integration

- **Spec first:** `docs/voice/VOICE_LINE_SPEC.md` is written in Phase 1
  before any line: schema, register vocabulary + resolution order, knowledge
  classes, age-class policy ("a child does not narrate a mass grave" —
  concretely: child-class lines may not select on mass-casualty triggers, and
  the validator rejects a child line whose trigger is in that set), tone
  rules (cold, tired, human, restrained, concrete; no exposition, no jokes,
  no moralizing, no tutorial-prose-as-dialogue), and the two-lane rule
  (radio = world, voice = shelter).
- **Vertical slice content:** `survivor_perished` × 4 registers
  (`clinical`, `clipped`, `devotional`, `weary`) × conditions
  (affinity-with-deceased band, leader-vs-not). 12–20 lines. Each line
  references only shelter-public facts (`knowledge_class: public_shelter`
  or `relationship_pair` when an affinity band is set — the pair relationship
  is the speaker's own knowledge by construction).
- **Register ↔ phrasing, not topic:** the medic and the fitter grieve
  differently in *word choice* (lexicon tags `medical`/`mechanical`), not in
  different facts — that is the 40A value the source plan asks voice to
  surface, encoded as `register` + `lexicon_tags` with zero state invention.
- **Pipeline:** content goes through `ashfall-write` (canon-aware drafting)
  and `ashfall-narrative-check` (tone/continuity) like any narrative content;
  the `survivor_voice_lines.json` row + `voice.*` string key are added in the
  same commit.
- **Warning parity:** if a line references a mechanical fact (allowed only in
  later families, not the slice), the canonical warning surface must already
  carry it; the test in §18 asserts the briefing/panel text exists for the
  referenced fact class. Voice humanizes warnings; it never originates them.
- **No invented interiority:** lines may reference only facts in state
  (E6/E7/E9 seams). The knowledge-class gate is the enforcement point; the
  contradiction soak (§18) catches authoring drift.

---

# 17. Failure Modes

| # | Failure | Behavior | Detection |
|---|---|---|---|
| F-1 | Survivor dies between trigger intake and end-of-day selection | Speaker eligibility re-checks `AliveSurvivors` at selection time (not intake time); a dead speaker is dropped; if the *subject* of a grief line died, the line still fires (that is its point) | unit test: kill speaker mid-day, assert no line by them |
| F-2 | All matching lines on cooldown for a speaker | `TrySelect` returns false; nothing delivered; stats count `blocked_cooldown`; no fallback to a wrong-register line | unit test: fire same trigger twice within cooldown |
| F-3 | Catalog has no line for a state combination | Fail-closed silence for that candidate; stats count `blocked_no_candidate`; utilization report surfaces the gap so content can be authored | soak report row; never an error |
| F-4 | Save/restore between trigger intake and selection | Candidates derive from persisted/replayed sources only (§13.5); restored run rebuilds the identical candidate list; delivered ledger suppresses any already-delivered key | replay harness Scenario B (continuous == interrupted) |
| F-5 | Restore after selection but before save (crash window) | Delivered ledger was captured in the same save as the lines it delivered (atomic envelope); a pre-save crash loses both together — consistent, never half-delivered | save-atomicity argument + journey test |
| F-6 | Journal full (64-cap eviction) | Eviction is oldest-first; voice event lines are ordinary entries and may be evicted — acceptable (they are not mechanical records); the voice delivered ledger (30-day retention) outlives journal eviction for idempotency | documented; eviction test with 64-entry journal |
| F-7 | Missing `text_key` in strings.csv | Boot integrity error (fail-fast in dev/CI); runtime fallback is the key itself rendered dim + `OnMissingKey` log — never a crash, never prose invention | integrity tier test with a missing-key fixture |
| F-8 | Unbound required port (`TryRelationshipBand`) | Lines needing it never match (fail-closed); integrity report counts unbound-port exclusions; port selftest fails if a `HOST_REQUIRED` voice port has no active caller | `--port-contract-selftest` |
| F-9 | Unbound sink (e.g. briefing builder not wired) | Port contract violation → boot selftest failure; in a degenerate runtime, `OnLineSelected` has no subscriber → host logs once per day + counts `undelivered` in the heartbeat event | wiring test: unbind sink, assert failure |
| F-10 | Duplicate delivery after session rebind | `DeliveryKey` ledger check precedes dispatch; rebind never re-delivers | rebind idempotency test (save → new session → same day) |
| F-11 | Register rule ambiguity (two rules match) | Validator error at load; file-order-first rule documented as the tiebreak but ambiguity is authored out | validator test with overlapping rules fixture |
| F-12 | Voice journal writes flood the 64-entry journal | `event_per_day_journal` budget (default 6) + murmur families journaled selectively; heartbeat counts suppressed-by-budget | budget test: 20 candidates, cap enforced |
| F-13 | One survivor dominates (grief spike day) | `per_speaker_per_day` cap (default 2) across all classes | budget test |
| F-14 | Child selects a death line | `age_class` filter + validator content policy; child survivors select only `child_only`/`any` lines with non-mass-casualty triggers | unit test + validator test |
| F-15 | Line contradicts state ("grateful for full rations" during `rationing_tier_changed` to half) | Conditions bind lines to bands; contradiction soak asserts every delivered line's conditions held at delivery; authoring review gate | soak test (§18) |
| F-16 | Locale switch mid-campaign | Journal stores key + args (§12); briefing/murmur resolve at render; history re-renders in the new locale | locale-switch test (en → pseudo) |
| F-17 | Weighted pick overflow (many lines) | Integer weight walk with `long` accumulator; weights ≤ 10000 validated; candidate count per trigger is content-bounded by review | edge test with 100-line fixture trigger |
| F-18 | Roster > 256 survivors breaks actionIndex spacing | Validator warning at load when alive roster exceeds 256 (impossible for current 129-def cast); documented | static assertion |
| F-19 | `audio_cue` id missing from cue catalog | Structural validation error at load; runtime policy (if catalog drifts): log once, text continues, no crash (E17) | validator test |
| F-20 | Day-event kind used as trigger becomes Heartbeat later | Validator cross-checks `DayEventVocabulary` at load; a reclassified kind fails the voice tier loudly, not silently | validator test with a heartbeat kind fixture |
| F-21 | Second-hand line about a survivor the speaker never met | `knowledge_class: relationship_pair` requires an existing `RelationshipEntry`; ambient "opinion of stranger" lines are not authorable in the slice vocabulary | knowledge-gate test |
| F-22 | Flashback line misread as a gameplay alert | `false_alarm`-triggered lines are `event` class, journal+briefing only, never the murmur lane, never the alert strip; register = fragmented/present-tense authored set | routing test |

---

# 18. Test Strategy

Per `TEST_POLICY.md`: focused files, run alone first via
`bash scripts/run_test.sh <file>`, each builder stays well under 100 cases.
No full-suite run by default. New test files live under
`Ashfall.Core.Tests/Voice/` (new directory).

**`VoiceLineCatalogLoaderTests.cs`** (~14 cases): schema envelope required;
snake_case enforcement; duplicate line ids rejected (within file); unknown
register rejected; unknown lexicon tag rejected; unknown trigger rejected;
trigger that maps to a Heartbeat kind rejected; `weight ≤ 0` rejected;
negative cooldown rejected; `day_min > day_max` rejected; `survivor_id`
selector not in survivors.json rejected; child line on mass-casualty trigger
rejected (content policy); missing `text_key` rejected; valid slice file
loads and indexes (ordinal order proven); both selector-set (two non-null
speaker ids) rejected.

**`SurvivorVoiceSelectionTests.cs`** (~18 cases): speaker selector specificity
order (exact survivor > belief > profession > archetype > default); candidate
ordinal sort before weighting; deterministic pick — same fork yields same line
across 3 repetitions; weight distribution sanity (weight-9000 line picked
over weight-1000 with a fixed seed); cooldown blocks re-selection until
`cooldown_days` elapsed; family cooldown independent of line cooldown;
no-immediate-repeat with ≥ 2 alternatives; single-candidate family exempt
from no-repeat (documented); age-class filter (child never selects
adult_only); knowledge-class gate (relationship_pair without relationship
entry → no match); leader_only without leadership binding → no match
(fail-closed port); unbound optional port excludes dependent lines;
`TrySelect` on dead speaker returns false (F-1); day window filter;
`DeliveryKey` already in ledger → false (F-10); stats counters increment per
outcome class.

**`SurvivorVoiceSaveTests.cs`** (~8 cases): capture/restore round-trip
(cooldowns, family cooldowns, delivered keys, last-line, stats); restore is
non-operative (no events fired); legacy null state → empty default;
ordinal-sorted capture (deterministic bytes); delivered-ledger retention
prune at the day window; restore-then-select equals uninterrupted sequence
(paired with replay harness); checksum envelope via the real store codec;
newer-version envelope refused.

**`SurvivorVoiceReplayTests.cs`** (~6 cases) — modeled directly on
`DistressSignalTasks912ReplayTests` (same World.Create harness shape, real
save codec, fingerprint strings):
- Scenario A — fixed-trace lifecycle: authored fixture catalog + scripted
  trigger sequence (death day 3, confrontation day 5, friction day 7) → exact
  delivered line-id sequence asserted.
- Scenario B — save/restore mid-day 5 → finish: delivered sequence, cooldown
  map, and stats byte-identical to the continuous run (fingerprint compare).
- Scenario C — cooldown pressure: same trigger daily for 10 days → distinct
  lines until exhaustion, then silence, never repetition, never wrong register.
- Scenario D — child/elder + knowledge gates: fixture child speaker never
  receives death-trigger lines; pair-knowledge line fires only after a
  relationship exists.
- Full-lifecycle fingerprint stable across two independent runs (same seed).
- Contradiction probe: every delivered line's conditions re-evaluated against
  the recorded state snapshot at delivery time (all true).

**`SurvivorVoiceDeliveryTests.cs`** (~10 cases, host-adjacent Core harness):
briefing renderer additive behavior (section present with entries, absent
when no lines, budget drop order deterministic); journal write deduped per
voice key (two dispatches of the same delivery → one entry); murmur budget
per surface; per-speaker budget; crisis suppression defers murmurs but still
journals event class; warning-parity probe (line family referencing
`hazard_warning` requires the matching day event present in the same batch —
fixture proves the guard fires when absent); locale re-resolution (store key,
render en then pseudo); unbound-sink port failure; delivered-ledger survives
simulated rebind.

**`VoiceUtilizationReportTests.cs`** (~4 cases): report shape (per-line
eligible/selected/delivered/blocked_cooldown/blocked_knowledge/
blocked_budget); 200-day seeded soak over the fixture cast produces the
report without error; a never-eligible line is reported (not crashed on);
report is deterministic across runs.

Focused verification commands (builder):
`bash scripts/run_test.sh Ashfall.Core.Tests/Voice/` (each new file alone
first); adjacent regression: `bash scripts/run_test.sh
Ashfall.Core.Tests/Needs/Plan177SleepNarrativeProjectionTests.cs`,
`Ashfall.Core.Tests/Campaign/DayEventVocabularyTests.cs`,
`Ashfall.Core.Tests/Journal/` slice; `godot --headless --path . --
--data-integrity-selftest` (voice tier); `--port-contract-selftest`;
`--audio-selftest` (cue ids); `--content-utilization-selftest` (catalog
consumed); `--panel-bind-lifecycle-selftest` (session wiring);
`dotnet build Ashfall.csproj` 0 errors 0 warnings. Gates to regenerate via
their owners: save-store matrix, architecture map, catalog registry, docs
index, port-contract policy.

---

# 19. Dependency-Ordered Phases

Execution order honors the source plan (`40A → 31A → 42A → 42B → 42C`) with
evidence-based corrections from §2: the 40A **read** path is live today
(E7); the 40A write-path remainder and 31A's `causeId` wire field are named
external dependencies, not re-scoped into this package; 36A is sealed
groundwork (E13).

**Phase 0 — Premise gate (audit-only, no production change).** Re-verify at
claim time: (a) `InferBeliefProfile` status in `Main.SurvivorSocial.cs` — if
40A has since deleted it, register resolution is unaffected (voice never used
it); (b) `DEBT-PLAN36-PORT-CONTRACT-CLOSURE` still RETIRED; (c) no new active
claim overlaps §20 paths; (d) `DayEventVocabulary` trigger kinds used by the
slice still registered. Record findings in the claim row; claim paths in
`WORKTREE_OWNERSHIP.md` before Phase 1. **C2[17] sequencing rule:** Plan 42
Phases 1–4 may proceed in parallel with 40A's remainder because voice reads
identity through the live enrichment service; if 40A changes the enrichment
schema, voice's loader/validator adapt in a follow-up (the read seam is one
method). Plan 42 must NOT ship its content slice claiming belief coverage
that the integrity tier cannot yet prove — the coverage gate (§11) reports
the gap honestly instead of failing the build on unenriched survivors (they
resolve to the default register).

**Phase 1 — Spec + schema + loader (42A core).** `VOICE_LINE_SPEC.md`;
`survivor_voice_registers.json` (vocabulary + rules + budget, zero lines
required); `survivor_voice_lines.json` (schema only + fixture minimal);
`VoiceLineCatalogLoader` + catalog read model; integrity tier; loader tests.
Gate: validator green, spec reviewed.

**Phase 2 — Selection system + state + save (42A system).**
`SurvivorVoiceSystem`, ports DTO, state + capture/restore, RNG stream id,
save section + registry row + host store (integrator-claimed), selection
tests, save tests. Gate: selection + save suites green alone.

**Phase 3 — Vertical slice content + replay harness (42A content).** 12–20
`survivor_perished` lines + `voice.*` keys; `ashfall-write`/
`ashfall-narrative-check` pass; replay harness Scenarios A–D + fingerprint.
Gate: replay green; slice lines all reachable in the harness.

**Phase 4 — Delivery (42B).** Host adapter + buffering + end-of-day pass;
journal sink; briefing renderer + host call; murmur widget + detail-panel
row; budgets; delivered-ledger idempotency; port registration + selftest row;
optional cue path; delivery tests; a11y/snapshot selftests green. Gate:
survivors audibly grieve a death in a headless journey, archived in journal,
visible in briefing, no duplicate after reload.

**Phase 5 — Social speech (42C).** Wire the buffered social triggers
(confrontation, theft, friction, synergy, leader events, mourning, false
alarm, belongings) to the same selection pass; author the grief-adjacent
families (ration grievance, friction pair, leader stress, false-alarm
fragment) under the same budgets; contradiction soak. Gate: each speech
object class (world/people/player-decision) has ≥ 1 reachable family; soak
asserts no contradictions over 200 seeded days.

**Phase 6 — Utilization + closeout.** 200-day utilization report committed;
unreachable-line ratchet documented; content-utilization selftest shows the
catalog consumed; closure report per the corpus template; census row update
handed to the foreman (the foreman, not this builder, marks `C2[18]`).

External dependency register (never silently absorbed): 31A `causeId` on
`DayStateChangeEvent` (would upgrade `CauseKey` from tuple-string to a typed
field — additive later); 40A write-path completion (removes the unenriched-
survivor default-register gap); 32C/131 knowledge network (would replace the
conservative knowledge classes with real per-survivor knowledge); `D22`
string freeze + `DEC-11`/`DEC-13` (govern later translation/VO, not this
package).

---

# 20. File Impact Map

New files (this package's claim):

| Path | Reason |
|---|---|
| `docs/voice/VOICE_LINE_SPEC.md` | Schema/register/tone authority; review gate for all content |
| `Assets/StreamingAssets/Data/survivor_voice_lines.json` | Line catalog (slice then families) |
| `Assets/StreamingAssets/Data/survivor_voice_registers.json` | Register vocabulary, resolution rules, attention budgets |
| `Assets/Ashfall.Core/Voice/VoiceLineDef.cs` | Catalog DTOs (engine-free) |
| `Assets/Ashfall.Core/Voice/VoiceLineCatalogLoader.cs` | Strict loader (collected diagnostics, house pattern) |
| `Assets/Ashfall.Core/Voice/SurvivorVoiceCatalog.cs` | Runtime indexed catalog |
| `Assets/Ashfall.Core/Voice/SurvivorVoicePorts.cs` | Narrow ports DTO (E21 idiom) |
| `Assets/Ashfall.Core/Voice/SurvivorVoiceSystem.cs` | Selection authority + state |
| `Assets/Ashfall.Core/Voice/VoiceTriggerCandidate.cs` + `VoiceLineDelivery.cs` | Intake + delivery DTOs |
| `src/Host/SurvivorVoiceSaveStore.cs` | Save façade (house pattern) |
| `src/Main.SurvivorVoice.cs` (+ `.cs.uid`) | Host composition, buffering, end-of-day pass, sink dispatch |
| `src/UI/HudMurmurWidget.cs` (+ `.cs.uid`) | Single-line transient murmur (not a panel) |
| `Ashfall.Core.Tests/Voice/*` (5 files, §18) | Focused tests + replay harness |
| `docs/plans/PLAN_42_SURVIVOR_VOICE_INTEGRATION_PLAN.md` | This document |

Additive edits (each with its reason; integrator/shared paths claimed per
ledger):

| Path | Change | Reason |
|---|---|---|
| `Assets/Ashfall.Core/Random/CampaignRngStream.cs` | +1 const `SurvivorVoice` | RNG stream (§13) |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | +`ValidateVoiceCatalogs` | Integrity tier (§11) |
| `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` | +1 row + file-name row | Save section (§12) |
| `Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs` | +`AppendVoiceLines` (new method only) | Briefing sink (§15) |
| `src/UI/SurvivorDetailPanel.cs` | +"Last heard" row group | Detail sink (§15) |
| `src/UI/GameHudOverlay.cs` | +murmur widget child | Murmur lane (§15) |
| `src/Main.Campaign.cs` | +1 call: run voice pass before briefing build | End-of-day pass placement (§14) |
| `src/Main.SaveOrchestrator.cs` | +setup/save/flush calls beside social | Save wiring (§12) |
| `docs/ci/port_contract_policy.json` | +voice port rows | Port contract (§5.11) |
| `assets/l10n/strings.csv` | +`voice.*` keys | Localization (§11.4) |
| Generated (`docs/INDEX.md`, catalog registry, save-store matrix, architecture map, `PORT_CONTRACT.md`) | regenerated by owning generators only | Rule: never hand-edit generated outputs |

Explicitly untouched: every system in §2 marked LEAVE, plus
`SleepNarrativeProjection`/`Main.SleepNarrative.cs`, radio hosts/panels,
`phantom_triggers.json`, `JournalVoice*`, `MoraleMarkSystem`,
`DayEventVocabulary` mappings, `DayStateChangeEvent`, all social system
cores (host subscribes to their events; their code is unmodified).

---

# 21. Risks

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Duplicate-architecture drift (re-implementing journal/radio/mark idioms badly) | Medium | High | §2 collision check is the contract; review gate compares each new file against its named precedent (E2/E5/E15/E21) |
| DEC-17 violation by scope creep ("voice skill up") | Low | High | Register is static authored data; no XP/level/progression fields exist in the schema by design; any future voice-progression proposal must re-litigate DEC-17 explicitly — stated in `VOICE_LINE_SPEC.md` |
| 40A schema drift breaks the read seam | Medium | Medium | Single-method seam (`GetView`); Phase 0 re-check; adaptation is one loader touchpoint |
| Content explosion before schema proof | High | Medium | Vertical-slice-only rule; utilization ratchet (report-first, not fail-first) per corpus §29 |
| Chatter overload on bad days | Medium | Medium | Attention budgets in data + crisis attenuation + per-speaker cap (F-12/F-13) |
| Nondeterminism from candidate ordering | Medium | High | Stable sort + ordinal speaker lists + fork scheme; replay harness proves continuous==interrupted |
| Journal eviction loses voice history | Medium | Low | Accepted (F-6); journal is a 64-entry window by design; memorial/chronicle systems own permanence |
| Knowledge gate too conservative (lines never fire) | Medium | Medium | Utilization report distinguishes `blocked_knowledge`; families tune `knowledge_class` in data |
| Warning-parity authoring drift | Low | High | Test guard (§18) + spec rule; voice never originates mechanical facts |
| Save bloat from delivered ledger | Low | Low | 30-day retention prune; ints/strings only |
| Briefing section noise vs. Plan 31 pins | Medium | Medium | Additive section after existing ones; `GenericSectionTitle` pins untouched; parity gate unaffected (no new day-event kinds) |
| Tone regression (quips, jokes) | Medium | High | `ashfall-write` + narrative check mandatory per content tranche; spec tone section; child-content validator rule |
| Unenriched survivors feel voiceless | Medium | Low | Default register + generic families; coverage report names the gap until 40A closes |

---

# 22. Out of Scope

- Dialogue trees, conversation engines, player-to-survivor chat, choice UI
  (`ResolveChoice` idioms already cover decisions).
- Per-character voice skill progression or any voice RPG mechanics (DEC-17).
- VO recording/synthesis, new audio cue families (DEC-11); radio content.
- Rumor/information-network simulation (parallel plan 131); per-survivor
  knowledge simulation (32C) — voice ships with the conservative
  knowledge-class gate and consumes a real knowledge authority if one lands.
- `DayStateChangeEvent` schema changes (31A's owner decision), new day-event
  kinds, semantic-kind re-grouping (D11 decision-blocked).
- 40A write-path work: deleting `InferBeliefProfile`, registration discipline,
  profession fork resolution (C2[17]'s package).
- Translating `SleepNarrativeProjection`'s hardcoded lines or absorbing sleep
  beats into voice (separate localization-debt package; DEBT-177 stays
  RETIRED).
- `phantom_triggers.json` raw-text migration to keys (data-hygiene package).
- New panels or routes; localization into German (DEC-13); the D22 string
  freeze itself.
- A `SurvivorVoiceDayOwner` mid-tick insertion (upgrade path if a future
  phase needs intra-day voice; the end-of-day pass suffices for all
  specified content).

---

# 23. Rollback Strategy

- The package is strictly additive: delete the new files + revert the
  additive edits listed in §20 to roll back. No existing system behavior
  changes when the voice catalog is absent — the host setup is idempotent and
  a missing catalog file binds an empty catalog (loader diagnostics warn;
  zero lines ⇒ zero selections ⇒ zero deliveries).
- Save compatibility on rollback: `survivor_voice` section is ignored by
  builds without the section reader only if the envelope reader tolerates
  unknown sections — verify against the current orchestrator behavior before
  rollback; otherwise the rollback build keeps the section reader and simply
  never acts on the state (preferred: keep the reader, remove the sinks).
- Data rollback: removing `voice.*` string keys leaves journal history
  entries rendering their key name dimly (the `OnMissingKey` path) — ugly but
  non-fatal; prefer keeping string keys for one release after content
  rollback.
- No migration is ever needed because no existing save section or data file
  is modified.

---

# 24. Definition of Done

- [ ] `VOICE_LINE_SPEC.md` written and reviewed before any line content.
- [ ] Both catalogs validate clean through the new integrity tier, including
      coverage (default register for all 129 survivors), content-policy
      (child), and reference (trigger/location-flag-free/profession/belief/
      cue/text-key) rules.
- [ ] `SurvivorVoiceSystem` selection is deterministic: replay harness
      Scenarios A–D + fingerprint green; continuous == mid-day-save/restore.
- [ ] Cooldown, family cooldown, no-immediate-repeat, per-speaker and
      per-surface budgets enforced by test.
- [ ] Knowledge classes enforced; a line never fires for a speaker who could
      not know its subject (test + soak).
- [ ] Delivery: every selected line reaches a declared sink or the port
      selftest fails; journal archives event-class lines exactly once
      (knowledge-key dedup); briefing shows a bounded Voices section;
      murmur + detail rows work; reload/rebind never double-delivers.
- [ ] No voice line is the sole carrier of a mechanical warning (guard test).
- [ ] First slice (survivor_perished family) authored, tone-gated via
      `ashfall-write` / `ashfall-narrative-check`, reachable in-game.
- [ ] 200-day seeded utilization report committed; unreachable lines reported
      as data, with the ratchet rule documented.
- [ ] Core remains engine-free (`Ashfall.Core` builds with zero Godot
      references in the new files); host builds 0 errors / 0 warnings.
- [ ] `--data-integrity-selftest`, `--port-contract-selftest`,
      `--audio-selftest`, `--content-utilization-selftest`,
      `--panel-bind-lifecycle-selftest` green; focused xUnit targets green
      per §18; generated gates regenerated and `--check` clean.
- [ ] No hardcoded survivor prose in C# (source-scan over the new files).
- [ ] DEC-17/DEC-11 honored; lanes (radio vs. shelter voice) separate.
- [ ] Handoff to foreman: census row `C2[18]` status recommendation +
      closure metrics (authored/eligible/selected/delivered/unused,
      delivery-missing = 0, duplicate deliveries = 0, knowledge violations =
      0, contradiction violations = 0).

---

# 25. Implementation Handoff

## MUST PRESERVE

- Godot-authoritative architecture; Core engine-free (no Godot/UnityEngine/
  engine-serialization references in `Assets/Ashfall.Core/Voice/*`).
- Every §2 system marked LEAVE — especially `SleepNarrativeProjection`
  (RETIRED-adjacent, sealed), `JournalVoice`/`JournalVoiceProseCatalog`,
  `MoraleMarkSystem`, radio hosts and the `_playedBroadcastKeys` ledger, the
  phantom/vinyl catalogs, and all seven social systems' cores.
- `DayEventVocabulary` totality and the pinned `GenericSectionTitle`
  behavior; `DayStateChangeEvent`'s current shape.
- The seeded RNG contract: only `CampaignRngManager.Fork("survivor_voice",
  day, actionIndex)`; never `System.Random`, wall-clock, or hash-order.
- Existing journal semantics (64-cap, knowledge-key dedup, save shape) and
  briefing structure (additive renderer only).
- Unrelated dirty worktree state (`Seal-steps/*`,
  `docs/plans/BLOCKED_PLANS_UNBLOCKER_PLAN_2026-09-19.md`) — never touch.

## MUST ADD

- `Assets/Ashfall.Core/Voice/` (loader, catalog, ports, system, DTOs, state)
  as the sole voice-selection authority.
- `survivor_voice_lines.json` + `survivor_voice_registers.json` with
  `schema_version: 1`, snake_case, and the §11 validation tier.
- `CampaignStreamIds.SurvivorVoice`; `survivor_voice` save section +
  registry row + `SurvivorVoiceSaveStore`; `Main.SurvivorVoice.cs` host
  adapter (buffering + one sorted end-of-day pass).
- Sinks: journal writes (voice knowledge keys), additive briefing renderer,
  HUD murmur widget, detail-panel row; port-contract rows.
- `docs/voice/VOICE_LINE_SPEC.md`; `voice.*` string keys; the five focused
  test files including the replay harness; the 200-day utilization report.

## MUST NOT DO

- No dialogue tree/chat/conversation engine; no new panel or route; no
  interruption-class delivery (the enum has no such member).
- No survivor prose in C# (the SleepNarrativeProjection line arrays are the
  anti-pattern — do not copy them); no raw English in the voice JSON
  (`text_key` only — phantom_triggers' `motivationText` style is debt, not
  precedent).
- No voice skill progression, XP, levels, or register growth (DEC-17 —
  RETIRED "Presenter Skill Tree"; a static authored register mapping is the
  whole design; anyone wanting progression must re-litigate DEC-17 by name).
- No VO production or new audio families (DEC-11); optional cue ids from the
  existing catalog only.
- No parallel knowledge/rumor/grief/relationship state; no second journal;
  no second identity authority; no use of `InferBeliefProfile`.
- No line as the sole carrier of a warning; no omniscient lines
  (knowledge-class gate); no child narration of mass-casualty content.
- No edits to generated files by hand; no full-suite test runs by default;
  no claims on paths outside §20 without updating `WORKTREE_OWNERSHIP.md`.

## VERIFY WITH

- `bash scripts/run_test.sh Ashfall.Core.Tests/Voice/<each new file>` (alone
  first), then the `Voice/` directory; adjacent pins:
  `Plan177SleepNarrativeProjectionTests`, `DayEventVocabularyTests`,
  `DayEventParitySourceGateTests`, journal slice.
- `godot --headless --path . -- --data-integrity-selftest` (voice tier),
  `--port-contract-selftest` (voice rows), `--audio-selftest`,
  `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest`.
- `dotnet build Ashfall.csproj` (0/0) and the Core test project build.
- Replay harness: continuous == save/restore-mid-day fingerprint equality.
- 200-day seeded soak: utilization report + zero contradiction violations.
- `ashfall-narrative-check` + `ashfall-write` review on every content
  tranche; pseudo-locale a11y pass on murmur/briefing/detail surfaces.
- Source scan: no `Text = "` / string-literal prose in the new Core/host
  files; `CampaignRngSourceGateTests` for the new stream id.

## FIRST SAFE IMPLEMENTATION STEP

Claim the Phase 0/1 paths in `WORKTREE_OWNERSHIP.md` (voice files +
`docs/voice/` + test directory; note the integrator-owned shared paths for
later phases), then run the Phase 0 premise re-verification: confirm
`DEBT-PLAN36-PORT-CONTRACT-CLOSURE` is still RETIRED, `InferBeliefProfile`'s
current status in `src/Main.SurvivorSocial.cs`, no active claim overlaps
§20, and the slice's trigger kinds are still registered non-Heartbeat in
`DayEventVocabulary`. Record the four answers in the claim row, then write
`docs/voice/VOICE_LINE_SPEC.md` and the two catalog schemas with the
integrity tier — before a single line of survivor dialogue is authored.
