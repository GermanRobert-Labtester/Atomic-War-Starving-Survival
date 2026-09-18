# ASHFALL — Six Unblocked Partial-Integration Plans

**Document class:** Implementation plan (integration ledger companion, not a replacement for `INTEGRATION_PLANS.md`).
**Drafted:** 2026-09-18. **Basis:** `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `SESSION_HANDOFF.md`, `docs/plans/wave8_part2/C1_HANDOFF.md`, `docs/plans/PLAN_24_CLOSEOUT.md`, `docs/plans/C2_PLANINTEGRATION_2_CLOSURE_REPORT.md`, `docs/radio/DISTRESS_SIGNAL_FOLLOWUP_CONTRACT.md`, `docs/radio/SIGNAL_TRUST_CONTRACT.md`, `docs/radio/DISTRESS_SIGNAL_AUDIO_CONTRACT.md`.
**Scope:** Six plans that shipped **partially integrated** — mechanism, contract, or core system landed green, but a named element was blocked (missing dependency, unsigned decision, absent authority, or deferred content). Every blocker named in the ledgers has since been removed by a later wave. This document specifies the full integration of each, in claimable package form.

---

## 0. Executive Summary

| # | Package ID | Was partially integrated because | Now unblocked by | Estimated waves |
|---|---|---|---|---|
| P1 | `DISTRESS-SIGNALS-PR3-FOLLOWUP-AUDIO-CONTENT` | Wave 3/4 shipped follow-up chaining + audio cue mechanisms with **zero authored content** (mechanism-first, option-c) | Contract sealed, validator live, save V6 frozen | 3 |
| P2 | `PLAN-24-RESIDUALS-STAFFING-RECOVERY-RAMP` | Two signature-gated decision items deferred in Plan 24 closeout (ward staffing; affliction-specific recovery ramp) | Both are pure decision+implementation items; nothing upstream blocks them | 2 |
| P3 | `PLAN-31-SEMANTIC-KIND-AUTHORITY` (+ C2[2] deferred phases 17C-I, 17C-E, 17B matrix) | C2 Plan 17 closure classified these as real gaps needing their own packages; 17A-S vocabulary now exists as the foundation | `DayEventVocabulary` + parity matrix gate landed 2026-09-15 | 4 |
| P4 | `SIGNAL-TRUST-AVAILABILITY-CONSUMER` | `SignalTrustAvailability` shipped as tested policy with **no runtime selection pool** to consume it | Rescue-signal runtime + 12 missions + expansion catalog now provide a real candidate population | 3 |
| P5 | `MERCHANT-RESTOCK-PRIORITY` | Deferred with an authority question: "priority ordering needs a signed design" | Day-gated restock (Plan 147) and the dynamic economy v2 (`MarketSystem` shocks/pressure) provide the signed inputs this design needs | 2 |
| P6 | `PLAN-213-VEHICLE-ARMOR-DECORATION-SEAM` | Plan 213 D6 deferred vehicle armor because **no vehicle owner existed** | Plan 50 vehicle garage + expedition profile + component wear landed and self-tested (`--vehicle-garage-selftest` 19/19) | 2 |

**Recommended claim order:** P1 → P2 → P5 → P6 → P3 → P4. Rationale: P1 is content-only on a frozen contract (lowest risk, highest visible payoff); P2 closes an already-accepted plan's last items; P5 and P6 are small signed-seam packages; P3 is the broadest refactor (event vocabulary authority) and should land after the radio content wave so its parity matrix regenerates once; P4 adds a new selection mechanic and benefits from P1's richer signal population existing first.

**Cross-plan invariants (apply to every package):**

1. Every package claims exact paths in `WORKTREE_OWNERSHIP.md` under a fresh claim id (`claim-<package>-YYYY-MM-DD`) before touching the worktree; the shared dirty worktree is preserved untouched outside claims.
2. Build must remain **0 errors / 0 warnings** on all targets (`dotnet build Ashfall.csproj`, Godot host build).
3. `godot --headless --path . -- --data-integrity-selftest` must PASS with 0 errors; any new catalog file must be registered and walked clean (catalog registry regenerated and checked).
4. No new RNG streams. All randomness routes through existing seeded sub-streams (e.g. the `StableHash`-derived distress sub-stream); determinism fingerprints must be stable across `continuous == save/restore` replays.
5. Save-schema changes only via frozen-shape migration (pattern: `RadioSave` V4→V5→V6); old saves migrate to explicit neutral defaults; newer-shape loads throw.
6. Exactly-once semantics on every player-visible consequence (ClaimedReceipts pattern); idempotency proven by test.
7. UI changes respect the a11y gate: words never color-only, focus restored after commands, `--panel-bind-lifecycle-selftest` and `--ui-a11y-selftest` PASS.
8. Docs index regenerated (`python3 scripts/ci/generate-docs-index.py --check`); closeout document written to `docs/plans/`; `INTEGRATION_PLANS.md` row updated; `WORKTREE_OWNERSHIP.md` claim marked HANDED_OFF.

---

# P1 — `DISTRESS-SIGNALS-PR3-FOLLOWUP-AUDIO-CONTENT`

## P1.1 Identity and why it was partially integrated

The `DISTRESS-SIGNALS-9-12` flagship shipped Waves 1–5 fully green (Radio suite 321/321, full suite 11,170/11,170). Wave 3 delivered the complete follow-up-chaining **mechanism**: closed trigger grammar (`answered` / `rescue_success` / `rescue_failed` / `expired` / `trap_fallen_for`), self-contained payloads, exactly-once pending/fired ledgers, campaign-day scheduling, V5→V6 migration. Wave 4 delivered the complete audio-cue **mechanism**: pure resolver (stage override → signal default → text-only fallback), Intercept-gated playback, persisted `distress:{id}:{cue}` dedupe.

What did NOT land: **authored content**. Zero `follow_up_signals` entries exist in any catalog. Zero authored `audio_cue` values exist on any definition or fragment. The Wave 3 ledger records this explicitly: "no authored follow-up content yet (mechanism-first per user's Wave 1 option-c decision — content tranche deferred)". The PR2 hint tranche (2026-09-15) closed the `outcome_hint` gap only — 63 hints across 43 identities — and its own closeout names the remaining deferred items as exactly: follow-up payload content, authored audio_cue content.

**Blocker removal:** the blocker was a sequencing decision, not a technical dependency. The contract (`docs/radio/DISTRESS_SIGNAL_FOLLOWUP_CONTRACT.md`, `DISTRESS_SIGNAL_AUDIO_CONTRACT.md`), the validator rules in `CatalogIntegrityValidator.cs`, and the V6 save shape are all frozen and battle-tested. Nothing upstream remains. This is the single lowest-risk, highest-payoff package in the backlog.

## P1.2 Authority map (from sealed contracts — do not re-derive)

| Concern | Authority | File |
|---|---|---|
| Stage fragment selection | `DistressStageResolver` (sole authority; `message_fragments` IS the stage authority — no `stages` field) | `Assets/Ashfall.Core/Radio/DistressStageResolver.cs` |
| Follow-up scheduling | `DistressFollowUpScheduler` (trigger grammar, definitions DTO, exactly-once ledgers) | `Assets/Ashfall.Core/Radio/DistressFollowUpScheduler.cs` |
| Follow-up schema on signals | additive `follow_up_signals` list on definitions | `Radio/RadioDistressSystem.cs` |
| Audio cue resolution | `DistressAudioCueResolver` (pure, zero RNG) | `Assets/Ashfall.Core/Radio/DistressAudioCueResolver.cs` |
| Cue playback + dedupe | `RadioHostSession` (Intercept-gated; `playedBroadcastKeys` ledger) | `src/Host/RadioHostSession.cs` |
| Catalog validation | follow-up validation + structural cue-id rule | `Assets/Ashfall.Core/Radio/CatalogIntegrityValidator.cs` (in `Radio/`) |
| Signal trust deltas | `SignalTrustLedger` (+2 answered / +5 rescue / −2 ignored / −5 trap, bounded [0,100]) | `Assets/Ashfall.Core/Radio/SignalTrustLedger.cs` |
| Data catalogs | 43 unique identities across primary + expansion (primary-wins on the 5 overridden expansion rows — leave as dead data) | `Assets/StreamingAssets/Data/radio_distress_signals.json`, `radio_distress_signals_expansion.json` |

**Hard constraints from the contracts (must not violate):**

- Follow-up payloads are **self-contained** — catalog-backed references would require cycle validation before authoring; chains/cycles must remain structurally impossible. Do not introduce cross-signal references.
- Every trigger value must map to exactly one exactly-once mission transition. A follow-up defined on a trigger that cannot fire for that signal class (e.g. `trap_fallen_for` on a genuine-only signal) is a validator error, not a warning.
- Trap-class expiry is excluded from trust penalties ("ignoring a lure is not a failure"); follow-ups authored off trap expiry must not route trust deltas either.
- Audio: playback ONLY on the Intercept detection edge. Undiscovered signals are silent. Catalog load, eligibility checks, and save-restore never play cues. Missing cue ids log once and degrade to text — never crash.
- No new save fields. The V6 shape is frozen; follow-up pending/fired ledgers already persist.

## P1.3 Content design (what to author)

Target population: the 12 rescue missions (5 flagship + 7 authored scenarios) plus the 43-identity catalog where chaining is narratively legitimate. Author follow-ups in four content families, each exercising a different part of the grammar:

1. **Aftermath chains (`rescue_success`)** — a rescued survivor or grateful faction sends a secondary signal days later: thanks with a coordinates payload (a loot-site single-resolution event), a warning about a hazard the sender fled through (feeds the AnomalyHazard awareness vocabulary read-only), or a standing-record note. ~10 entries.
2. **Regret chains (`rescue_failed` / sender death)** — a final transmission, a recovery-team beacon over the sender's last position, or a Verdict-faction investigation signal when a body is found. These carry the emotional weight of the ignore-consequences system (Plan §7.4 vocabulary: `sender_death` / `faction_standing_loss` / `faction_ambush`) without re-implementing it — the follow-up is the *narrative echo*, the consequence engine already exists. ~8 entries.
3. **Lure escalation (`trap_fallen_for` suppression test + trap aftermath legitimacy)** — for trap-class signals the player fell for: a taunt follow-up, a second attempt from the same ambusher with degraded trust weighting (consume `SignalTrustAvailability` read-only via the bounded ‰ modifier — pure input, no new selection logic), and a `trap_fallen_for`-triggered Verdict bounty rumor. ~6 entries. The ignored/trap suppression and trap-aftermath legitimacy tests already prove these paths legal.
4. **Deadline echoes (`expired`)** — only for signals where ignoring has consequences per the sealed ignore-consequences rules; no-consequence signals keep legacy expiry and get NO `expired` follow-ups (validator should enforce this mapping as a new authored-content rule, see P1.4 Wave 2). ~5 entries.

Audio cues: author explicit cue ids for every definition (signal default) and every stage where the emotional register shifts (stage override). Register a minimal cue catalog in the host-side audio registry (existing registry is host-side; missing ids already degrade safely). Text-first: every cued signal must remain fully playable with audio off — the text-only fallback path is already tested.

## P1.4 Wave breakdown

**Wave 1 — Authoring + validator tightening (content + rules)**
- Paths: `Assets/StreamingAssets/Data/radio_distress_signals.json`, `radio_distress_signals_expansion.json` (additive `follow_up_signals` + `audio_cue` fields only); `CatalogIntegrityValidator.cs` (three new authored-content rules: (a) `expired` follow-ups forbidden on no-consequence signals, (b) trap-grammar triggers forbidden on genuine-never-hostile identities, (c) follow-up fragment text presence and clarity monotonicity mirroring the stage contract).
- Acceptance: all ~29 follow-up entries validate; new rules produce 0 errors on current content; the 5 primary-wins-overridden expansion rows remain untouched dead data; data-integrity selftest PASS with the catalog count incremented and walked clean.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/` (321+ baseline), `DistressFollowUpTests` extended with rule-violation fixtures (add negative fixtures under a new test file, not inline data), `godot --headless --path . -- --data-integrity-selftest`.

**Wave 2 — Scheduling correctness at population scale (tests only)**
- Paths: `Ashfall.Core.Tests/Radio/DistressFollowUpPopulationTests.cs` (new): for every authored follow-up × every trigger × days 0–60, assert fire-day determinism, exactly-once firing, same-day ordinal ordering, save/restore mid-chain parity (continuous == interrupted fingerprint), and ignored/trap suppression per authored class.
- Acceptance: full-population replay green; no fingerprint drift across runs; V6 codec round-trips every new pending/fired ledger state.
- Verify: new suite + `RadioSaveCodecTests` (V6 pin unchanged — no shape change), full `dotnet test Ashfall.Core.Tests`.

**Wave 3 — Audio wiring verification + closeout (host + docs)**
- Paths: `src/Host/RadioHostSession.cs` (no structural change expected — verify Intercept-edge gating still holds with real cue ids; register cue ids in the host audio registry); `Ashfall.Core.Tests/Radio/DistressAudioCuePopulationTests.cs` (every authored cue id resolves; dedupe keys stable; replay-after-reload silent).
- Docs: `docs/radio/DISTRESS_SIGNAL_PR3_CONTENT_TRANCHE.md` (content inventory + authoring rationale), closeout update, `INTEGRATION_PLANS.md` row, `WORKTREE_OWNERSHIP.md` claim.
- Acceptance: `--audio-selftest` PASS with real ids; content-utilization PASS (0 orphan follow-ups/cues — every authored row reachable); full suite green.

## P1.5 Risks and degenerate strategies

- **Follow-up spam / player fatigue:** bounded by design — each follow-up fires at most once per campaign (exactly-once fired ledger), and the scheduler's day-aware tick spaces chains. Content cap: max 2 follow-ups per parent signal; enforce in Wave 1 rule (d) (max-children rule).
- **Trust farming:** answering follow-ups must not become a trust pump. Constraint: follow-up answers route the SAME `+2 answered` delta as any answer — no bonus for chained answers. Document in the content tranche doc; add a population test asserting trust ceilings over a 30-day all-answer replay.
- **Audio noise floor:** stage overrides only where register genuinely shifts; target ≤ 2 overrides per signal. The text-only fallback must remain the primary channel.

## P1.6 Rollback

Content-only plus three validator rules: revert the claimed JSON files and validator diff as one package. No save schema touched; V6 migration unchanged; removing follow-up rows returns ledgers to empty defaults on next load (already the V6 neutral path). Rollback is byte-safe.

---

# P2 — `PLAN-24-RESIDUALS-STAFFING-RECOVERY-RAMP`

## P2.1 Identity and why it was partially integrated

Plan 24 (`C-integration-plans/C1_planintegration[5].md`, promoted by the user 2026-09-16) closed as **CLOSED-WITH-DEFERRALS** on 2026-09-17. Every originally open acceptance item closed with evidence — the nine-family needs migration, the duty-hour/overwork/skill-to-yield labor cluster, assignment UI, grief-to-needs, mourning vigil, ration journey, save/load journey parity, the 30-day simulation, needs baseline 15/15 — except exactly two **signature-gated decision items**:

1. **Ward staffing** — whether medical wards require assigned staff (a doctor/nurse duty) for treatment throughput, or whether wards run unstaffed at a throughput penalty.
2. **Affliction-specific recovery ramp** — whether recovery speed per affliction is a flat authored value or a per-affliction ramp curve (fast early recovery, slow tail vs. linear).

Both items have decision memos filed in the implementation log (`docs/plans/C1_planintegration[5]_IMPLEMENTATION_LOG.md`) but no foreman signature, hence no implementation. A third deferral (environment-blocked snapshot rebaseline) belongs to the D1 drift work, not this package.

**Blocker removal:** these were never technically blocked — they were waiting on a decision. This package specifies both options fully so the foreman can sign one line per item and the implementation proceeds immediately. Everything the items consume already exists and is tested: `DoseLedgerSystem` treatment log, affliction preflight, duty roster work-eligibility gates (Plan 19B), the nine-family needs migration, and the medical timeline UI.

## P2.2 Decision D-A: Ward staffing

**Option A1 (staffed throughput gate)** — wards contribute full treatment throughput only when the duty roster has ≥1 eligible medical specialist assigned during duty hours; unstaffed wards run at 40% throughput (authorable per facility). "Specialist" reuses the existing skill certification tier seam (skill tags already gate work eligibility elsewhere — no new skill system).
**Option A2 (soft penalty)** — wards always run; staffing adds a bounded morale/recovery-speed bonus (±15%), never a hard gate. Lower risk of soft-locking a small shelter with no medical specialist.

**Recommendation: A1 with a floor.** Hard gates create meaningful roster decisions (the plan's whole thesis is duty-hour labor as a real economy), but the 40% floor prevents death-spirals in specialist-poor shelters. The floor must be authored in the ward facility catalog row, not hardcoded.

### Implementation (either option)

- **Core:** `Assets/Ashfall.Core/Medical/` — extend the ward facility catalog DTO with `staffed_throughput_multiplier` (default 1.0, floor 0.4) and `staff_skill_tags` (default `["medical"]`); throughput computation routes through the existing duty-hour labor cluster query (the same seam the Plan 24 labor tests already pin).
- **Host:** duty roster assignment UI gains the ward as an assignable station (the assignment UI landed in Plan 24 — additive row, no new panel).
- **Data:** ward rows in the medical facilities catalog gain the two authored fields; data-integrity rule: multiplier ∈ [0.4, 1.0], tags non-empty.
- **Tests:** `Plan24WardStaffingTests.cs` (new, ~10 cases): staffed vs unstaffed throughput, floor enforcement, specialist-eligibility via tags, duty-hour boundary (throughput applies only during assigned hours — reuse existing duty-hour test fixtures), save/restore of roster assignments mid-treatment, 30-day deterministic simulation with a no-specialist shelter (assert no unrecoverable death spiral under A1-floor).
- **Acceptance:** all treatment throughput arithmetic sourced from catalog + roster state, zero hardcoded multipliers; adjacent `DoseLedgerSystem` and medical timeline suites unchanged and green.

## P2.3 Decision D-B: Affliction-specific recovery ramp

**Option B1 (per-affliction ramp curve)** — each affliction row authors a 2–3 point ramp (e.g. `[(0.0, 1.6), (0.5, 1.0), (1.0, 0.4)]` severity → speed multiplier): fast early recovery (encourages early treatment), slow tail (chronic lingering). Recovery day-computation stays deterministic and integer-scaled (bp, like the economy's bounded-lerp — no floats in persistence).
**Option B2 (flat authored per-affliction value)** — simplest; keeps the existing linear model, one authored constant per affliction.

**Recommendation: B1.** B2 wastes the affliction catalog's granularity; B1 makes *when* you treat a real decision (treat early → cheap; treat late → long tail), which is exactly the meaningful-decision criterion the repo's design language demands. Integer bp ramp with monotone-decreasing speed is trivially validatable.

### Implementation

- **Core:** `Assets/Ashfall.Core/Medical/` — affliction catalog DTO gains `recovery_ramp` (optional; absent → legacy flat path byte-identical, mirroring the RoomPowerProvider provider-unset pattern); recovery progression consumes the ramp via a pure function (severity-normalized interpolation, integer math, invariant culture).
- **Data:** author ramps for the affliction families with existing test coverage first (infection, fever, contamination-sickness); validator rule: ramp points ≥2, x ∈ [0,1] strictly increasing, multiplier bounded [0.1, 2.0], monotone-decreasing y (fast-to-slow) unless authored otherwise for chronic conditions with an explicit `chronic: true` flag.
- **Tests:** `Plan24RecoveryRampTests.cs` (new, ~12 cases): pure-function interpolation table, legacy-absence parity, exactly-once daily recovery application (already guarded), 30-day ramp-vs-flat comparison fingerprint, save/restore mid-recovery (persisted severity continues on the same ramp — no restart-of-ramp exploit), treatment-log timeline UI showing projected recovery days from the ramp (read-model only).
- **Acceptance:** zero changes to the treatment decision preflight; recovery values derivable and explained in the medical timeline UI (legibility: the UI must show the projected ramp, not just today's delta).

## P2.4 Wave breakdown and verification

- **Wave 1:** D-A Core + data + tests (either option; sign first, implement second).
- **Wave 2:** D-B Core + data + tests + UI read-model.
- Verify per wave: focused new suites; `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/` (325+ baseline); `--data-integrity-selftest` (catalog count +1 rule set); full `dotnet test Ashfall.Core.Tests`; build 0/0.
- Docs: update `docs/plans/PLAN_24_CLOSEOUT.md` (deferrals → closed), implementation log decision memos annotated SIGNED, `INTEGRATION_PLANS.md` row, claim in `WORKTREE_OWNERSHIP.md`.

## P2.5 Risks

- A1 hard gate could soft-lock specialist-poor shelters → mitigated by the 0.4 authored floor and the no-specialist 30-day simulation test.
- B1 ramps interacting with medicine treatment effects (dose effects on needs/radiation) could double-count recovery speed → constraint: ramp multiplies baseline recovery only; dose effects apply additively after, and one test pins the composition order.
- Save compatibility: both fields optional with byte-identical legacy defaults — old saves load unchanged; no migration needed.

## P2.6 Rollback

Both features are additive optional catalog fields + pure functions. Revert claimed paths as one package; legacy behavior resumes automatically (provider-unset pattern). No save migration to unwind.


---

# P3 — `PLAN-31-SEMANTIC-KIND-AUTHORITY` (+ C2[2] deferred legibility phases)

## P3.1 Identity and why it was partially integrated

The C2 Plan 17 legibility package executed **bounded** on 2026-09-15: 17A-S (no-silent-drop repair via `DayEventVocabulary` + builder default case + 8 tests), the parity matrix (`docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`, gate-enforced), owner-failure briefing visibility, radiation exposure-end lifecycle, and the guidance F2 toggle. Its closure report then classified the remainder into stale phases (already built by other waves — honored as erratum) and **real gaps needing their own packages**:

1. **Plan 31 semantic-kind authority** — the day-event vocabulary is a closed id list, but there is no single authority mapping each event id to a *semantic kind* (informational / warning / consequence / progression / world-state) that UI, audio, and briefing consumers all read. Today each consumer re-derives meaning from string shape or hardcoded lists — the exact pattern 17A-S was built to kill.
2. **17C Phase I — alert ducking/concurrency:** multiple same-tick alerts can overlap and bury each other; there is no ducking policy (priority-ordered, bounded concurrency) for alert-class events.
3. **17C Phase E — acquisition sweep:** acquisition-flavored events (item/stock/loot gains across shelters, expeditions, black market, foundry) are not consistently classified or surfaced; some routes are silent today.
4. **17B deep test matrix:** the route/visibility work shipped with a thin matrix; the deep matrix (every event kind × every surface: briefing, journal, panel strip, audio) was deferred.

**Blocker removal:** 17A-S's `DayEventVocabulary` and the gate-enforced parity matrix are the foundation these phases all presupposed. They landed green (17/17 focused, 58/58 adjacent). Since then, three more event-producing systems (distress follow-ups, market rumors via `FactionRadioTypes.MarketRumor=6`, exposure-end lifecycle) have joined the vocabulary — making the authority *more* urgent and its input set *complete enough* to seal once rather than churn.

## P3.2 Authority map and design

| Concern | Authority | Note |
|---|---|---|
| Closed event id vocabulary | `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs` | extend, never fork |
| Parity gate | `Tooling/DayEventParitySourceGateTests.cs` + matrix doc | gate-enforced; regenerate after each wave |
| Briefing builder | `Campaign/DailyBriefingReportBuilder.cs` | default case must remain the no-silent-drop guard |
| Audio routing | `src/Audio/AudioEventBridge.cs` | geiger wiring is the reference pattern |
| Alert presentation | host UI alert surface | today unordered; becomes kind+priority ordered |

**Semantic kind model (additive enum on the vocabulary):**

```
DayEventKind { Informational, Warning, Consequence, Progression, WorldState }
```

- One authoritative `KindFor(eventId)` lookup, data-driven from a new authored table (or attribute map inside `DayEventVocabulary` — recommend the in-file map: single authority file, no new catalog, validator rule keeps the matrix doc in sync).
- Every consumer (briefing sections, alert surface, audio bridge routing, journal tone) reads `KindFor` — zero consumer-side string matching. Source-gate test enforces this mechanically (regex sweep for banned patterns, same technique as the RNG source gate).
- Classification of the current full vocabulary is authored once in Wave 1 and frozen in the parity matrix; new events without kinds fail the data-integrity gate (closing the silent-growth hole permanently).

## P3.3 Wave breakdown

**Wave 1 — Kind authority + classification**
- Paths: `DayEventVocabulary.cs` (kind map + `KindFor`), `DailyBriefingReportBuilder.cs` (sections keyed by kind), `DayEventParitySourceGateTests.cs` (extend: unknown-kind failure), `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` (regenerate with kind column).
- Acceptance: 100% of current vocabulary classified; briefing output text byte-identical for existing days (kind is organizational, not rewording — pin with a golden-day test); source gate finds zero string-matching consumers after migration.

**Wave 2 — 17C Phase I alert ducking/concurrency**
- Paths: alert presentation host seam; new pure `AlertConcurrencyPolicy` in Core (bounded same-tick alert window, priority = Warning > Consequence > Informational within the window, cap N=3 visible per tick with overflow counted and summarized as one line — words, not just counters).
- Acceptance: deterministic ordering (stable sort by kind-priority then id — no RNG); overflow summary line passes a11y (text, not color); tests: 12-case suite incl. 20-same-tick stress, save/restore mid-alert-window, kind-priority inversions; `--ui-a11y-selftest` PASS.

**Wave 3 — 17C Phase E acquisition sweep**
- Paths: recon first (the repo rule: premise-correct before implement): enumerate every acquisition route — shelter scavenging, expedition salvage (incl. rescue salvage branch), black-market Buy, foundry forging COMPLETE, greenhouse/harvest, companion fetch, loot-site resolution — and classify current silence. Then: `KindFor` acquisitions tagged `Progression` sub-flavor `Acquisition`; silent routes get one event each through the existing vocabulary (no new systems); briefing gains an acquisitions digest line.
- Acceptance: acquisition sweep matrix doc (`docs/campaign/ACQUISITION_EVENT_SWEEP.md`) with route × surface × before/after; every route emits exactly-once per acquisition (idempotency tests); no double-emission where two systems meet (e.g. black-market settlement already emits stock deltas — dedupe rule documented).

**Wave 4 — 17B deep test matrix + closeout**
- Paths: `Campaign/DayEventSurfaceMatrixTests.cs` (new): generated-from-vocabulary matrix test — every event kind × surface (briefing/journal/panel/audio) has an asserted routing decision (shown/summarized/silent-by-design), so adding an event without deciding surfaces fails the build.
- Acceptance: matrix green across the full vocabulary (~200+ ids); closeout `docs/plans/PLAN_31_SEMANTIC_KIND_CLOSEOUT.md`; ledger rows updated.

## P3.4 Verification

Per wave: focused suites (`Campaign/`, `Tooling/`), `--data-integrity-selftest`, briefing golden-day pins, full `dotnet test Ashfall.Core.Tests`, build 0/0, docs index regen. Full suite run at Wave 4 close.

## P3.5 Risks

- **Reclassification churn:** every consumer reading `KindFor` means a misclassification is systemic. Mitigate: Wave 1 classification reviewed against the parity matrix's existing semantic notes before freeze; kind changes thereafter require a ledger note.
- **Briefing text drift:** golden-day pins catch accidental rewording; kind reorganization must not change wording, only grouping/order.
- **Alert window persistence:** alerts are presentation-only — persisting them would violate the no-presentation-state rule; the window is per-tick derived state (document, test via replay).

## P3.6 Rollback

Wave 1 is the keystone: reverting the kind map reverts consumers mechanically (source gate will fail if consumers are left dangling — rollback must be atomic). Waves 2–4 are independently revertible additive packages.

---

# P4 — `SIGNAL-TRUST-AVAILABILITY-CONSUMER`

## P4.1 Identity and why it was partially integrated

Wave 2 of `DISTRESS-SIGNALS-9-12` shipped `SignalTrustAvailability` — integer-permille bounded weighting (range [500,1500]‰, monotonic, order-preserving, integer-only) — as a **tested policy with no consumer**, because recon proved "no dynamic distress-signal selection pool exists in the runtime". The ledger entry is explicit: "Availability consumer (deferred with evidence): ... `SignalTrustAvailability` is the tested policy awaiting that future seam (documented in SIGNAL_TRUST_CONTRACT.md §5)."

**Blocker removal:** the rescue-signal runtime now provides everything the missing seam needed: a real candidate population (43 unique identities, 12 rescue missions, expansion catalog), per-signal persisted trust state (V5 ledger), discovery/eligibility machinery (Intercept detection edge, eligibility checks), and a day-aware tick. The "future seam" can now be authored against live, tested infrastructure instead of a hypothetical.

## P4.2 Design — the dynamic selection pool

The mechanic this unlocks: **trust-shaped radio traffic**. Low trust (player ignores signals) → fewer genuine distress calls reach the tuner (availability ‰ down-weight); high trust (player answers/rescues) → more calls, but the weighting also shapes *which* signals surface. This closes the loop the trust ledger only half-built: consequences for ignoring were runtime events; now reach itself shrinks.

**Architecture (three candidate models, one recommended):**

- **M1 — Eligibility weighting (recommended):** the existing per-day eligibility check for each undiscovered signal multiplies its qualification probability by the availability ‰ (bounded [500,1500], so a trusted player sees at most 1.5× and an ignorer at least 0.5× baseline). Integer math via the existing permille policy; order-preserving so deterministic day-rolls are stable; NO new selection pool object — the pool is simply "all eligible signals this day", which already exists implicitly.
- **M2 — Explicit pool with ranked draw:** build a daily candidate list ranked by availability, draw top-K. More controllable pacing but introduces a new owned structure and a new save surface — heavier, and the ledger explicitly avoided inventing scan mechanics.
- **M3 — Budget model:** daily "airtime budget" in ‰ consumed per surfaced signal. Interesting scarcity texture but couples to signal count, brittle to catalog growth.

**Choose M1.** It is the smallest honest consumer of the sealed policy, needs no new save fields (trust already persists in V5), and preserves determinism (same seeded sub-stream, weighting only scales qualification).

## P4.3 Wave breakdown

**Wave 1 — Core seam**
- Paths: `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs` (eligibility check consumes `SignalTrustAvailability` via optional provider — provider unset → legacy byte-identical, the RoomPowerProvider pattern), `SignalTrustAvailability.cs` (no change — already the tested policy), `SignalTrustLedger.cs` (no change).
- Acceptance: weighting applied only to undiscovered signals; discovered/resolved/dispatched signals exempt; genuine-never-hostile invariant untouched (weighting shapes *when*, never *what class*); determinism: same seed + same trust history → identical surface schedule.

**Wave 2 — Host wiring + persistence verification**
- Paths: `src/Host/RadioHostSession.cs` (bind ledger → availability provider into the system; capture/restore already exists for trust — verify round-trip replays the same schedule).
- Acceptance: continuous == save/restore fingerprint parity for a 30-day run at three trust profiles (ignored-all, answered-all, mixed); no replay-after-reload surfacing changes (the persisted ledger is the only input).

**Wave 3 — Player legibility + tests + closeout**
- Presentation (restrained, per repo cadence): one line on the RadioPanel trust strip — e.g. "signal traffic: thin / steady / busy" derived from the current availability band (words, three bands, a11y-safe); NO numeric player-facing ‰.
- Tests: `SignalTrustAvailabilityConsumerTests.cs` (~14 cases): band thresholds, monotonicity across trust sweeps, exemption classes, seed-stability, 30-day three-profile fingerprints, floor/ceiling behavior, interaction with follow-up suppression (ignored signals that fire `expired` must not double-punish availability).
- Docs: `docs/radio/SIGNAL_TRUST_AVAILABILITY_CONSUMER.md` (seam + balance rationale), contract §5 updated from "awaiting seam" to "consumed", closeout, ledger rows.

## P4.4 Balance rationale (why the numbers)

- Floor 500‰: an ignorer still sees half the traffic — radio silence must never fully lock the mechanic (the genuine-never-hostile philosophy: never remove the player's chance to redeem).
- Ceiling 1500‰: a rescuer gets busier, not flooded — the alert-concurrency cap (P3 Wave 2) is the downstream guard.
- Interaction with trust deltas (+2/+5/−2/−5, bounded [0,100]): availability is a *smoothed* function of trust (the sealed policy's monotone map), so one ignored trap does not spike traffic — no whiplash.

## P4.5 Risks and exploits

- **Deliberate ignorance as difficulty-lowering:** players who want quiet radio ignore everything. Counterplay is already authored: ignore-consequences (`sender_death`, standing loss, ambush) punish the *other* axes; availability only modulates opportunity. Document this as intended tension, not an exploit.
- **Trust farming then ignoring:** bounded ledger + monotone map means farming to 100 then coasting decays only via future ignores; acceptable (redemption arc), test pins the coasting curve.
- **Determinism regression:** any new RNG in the seam is banned; the existing sub-stream only. Source-gated.

## P4.6 Rollback

Optional provider pattern: revert Wave 1/2 host binding → legacy eligibility byte-identical. No save changes. Wave 3 presentation line is one panel strip revert.

---

# P5 — `MERCHANT-RESTOCK-PRIORITY`

## P5.1 Identity and why it was partially integrated

The economy flagship's deferred-items ledger closed three of four thin seams in `FOLLOWUPS-210-213-THINSEAMS` (sanitation power-grid feed, foundry forging buttons, market-rumor band bridge), and the black-market action surface was later sealed by `WAVE8-PART2-C1-BLACK-MARKET-ACTIONS`. One item remains: **merchant-restock priority**, deferred with an authority question — "restock already day-gated per Plan 147; a priority ordering needs a signed design". The Wave 8 Part 2 C1 handoff repeats it as a known remaining blocker: "merchant-restock priority remains unsigned" and "Do not promote merchant-restock priority without its decision memo."

**Blocker removal:** what the unsigned design needed as *inputs* now all exist and are tested: (a) Plan 147's day-gated restock cadence (the baseline behavior to order within), (b) the dynamic economy v2 — `MarketSystem` category indices, trade pressure (±3500bp, 0.75/day decay), shocks (`OnShockStarted`/`OnShockExpired`, severity/duration bounds), and `ExplainPrice` typed factor rows, and (c) the black-market stock/heat/trust state with signed settlement actions. A priority ordering can now be defined as a pure function of signed, persisted inputs rather than invented economics.

## P5.2 Design — the signed decision memo (drafted here for signature)

**Question:** when a merchant's restock day arrives and stock slots are bounded, in what order are goods replenished?

**Recommended contract — scarcity-weighted priority, pure and deterministic:**

1. **Priority score per good (integer bp, no floats):** `priority = w_demand × pressure_sign + w_scarcity × scarcity_depth + w_player_history × depletion_ratio`, with weights authored in the merchant/restock catalog row (defaults: demand 400, scarcity 400, player-history 200 — sum 1000).
   - `pressure_sign`: +1 if the category trade-pressure index is positive (players buying), −1 if negative (players dumping) — merchants restock what moves.
   - `scarcity_depth`: normalized distance of current price index from authored baseline bounds (`CommodityBaselineCatalog` bounds are the authority).
   - `depletion_ratio`: how empty the merchant's own stock slot is vs. its authored capacity — a merchant refills near-empty staples first.
2. **Stable tie-break:** priority score, then authored catalog id (lexicographic) — zero RNG, fully deterministic.
3. **Shock interaction:** an active shock of the matching category multiplies that good's priority by the shock severity clamped [1000, 3000]‰ — scarcity events push restock toward the shocked category (world reacts to its own crisis).
4. **Trust interaction (black-market merchants only):** low merchant trust caps luxury/high-value rows out of the restock set entirely (trust-gated assortment, not price change — pricing belongs to MarketSystem).
5. **Excluded from the design (explicit non-goals):** no priority-based price edits (MarketSystem owns prices); no player-facing priority UI (restock is world-side); no RNG; no new save section (restock results derive deterministically from persisted market + stock state each restock day).

**Alternative considered and rejected — FIFO/authoring-order restock:** simplest, but ignores the dynamic economy entirely and makes the v2 market invisible in merchant behavior; the signed v2 design's whole point was world-visible scarcity.

## P5.3 Wave breakdown

**Wave 1 — Core policy + catalog**
- Paths: `Assets/Ashfall.Core/Economy/MerchantRestockPriorityRules.cs` (new, pure — mirrors `EconomyMarketRumorRules` / `EconomyWeatherShockRules` pattern: Core-owned pure function, host thin adapter); merchant/restock catalog DTO gains optional `restock_priority` weights block (absent → legacy order, byte-identical); validator rules: weights ≥0 and sum ∈ [800, 1200]bp tolerance, capacity fields present when depletion_ratio used.
- Tests: `Plan147RestockPriorityTests.cs` (~12 cases): pure-function table (weights, pressure signs, shock interaction, tie-breaks), bounded-ness, legacy-absence parity, determinism (same inputs → identical order across 100 iterations), capacity-clamping.
- Acceptance: zero changes to restock *cadence* (Plan 147 day-gating untouched — priority only orders within a restock event); data-integrity PASS.

**Wave 2 — Host wiring + verification + closeout**
- Paths: the existing restock day-owner call site (where Plan 147 cadence fires — recon the exact host seam during implementation; expected `src/Main.Economy.cs` or the merchant host session): sort the replenishment set through the Core rules; black-market merchant path applies the trust gate before sorting.
- Tests: host wiring suite (~6 cases): market-bound catalog → ordering visible; catalog-unbound → legacy order (never hard-fail, mirroring the EconomyHostSession binding pattern); shock live during restock day → ordering shifts deterministically; save/restore across a restock day boundary → identical resulting stock (fingerprint parity); trust-gated assortment exclusion; no restock triggered by UI open/refresh (panel-open must never advance stock — same invariant the C1 black-market package proved).
- Docs: **the decision memo** (`docs/economy/MERCHANT_RESTOCK_PRIORITY_DECISION.md` — signature line for the foreman at top), contract doc `docs/economy/MERCHANT_RESTOCK_PRIORITY_CONTRACT.md`, Plan 147/211 closeout follow-up notes, `INTEGRATION_PLANS.md` row (economy follow-ups section: last deferred item → closed), claim.
- Acceptance: focused suites green (Economy 184+ baseline), full `dotnet test Ashfall.Core.Tests`, build 0/0, `--data-integrity-selftest`, docs index regen.

## P5.4 Balance rationale

- Weights 400/400/200: demand and scarcity lead (world logic), player history informs but never dominates — a single big purchase day shouldn't warp the merchant's whole assortment.
- Shock clamp [1000, 3000]‰: a crisis matters, but cannot crowd out staples entirely (the floor 1000 means shocked goods never rank *below* baseline).
- Trust-gated assortment rather than trust pricing: keeps one authority per axis (trust → access, MarketSystem → price), preserving the no-fork rule.

## P5.5 Risks and exploits

- **Player market manipulation to steer restock** (dump a category to suppress its restock, buy out a rival): bounded by the 0.75/day pressure decay and ±3500bp cap already signed in v2; a test pins the maximum restock-order swing achievable in one day of trading.
- **Staple starvation under chronic shocks:** the shock clamp plus depletion_ratio (refill near-empty first) protects staples; 30-day simulation test with a permanent-blizzard scenario asserts food-class goods never leave the restock set.
- **Determinism:** pure function + stable tie-break + no RNG; save/restore parity test is the guard.

## P5.6 Rollback

Optional catalog block + pure rules + one host sort call: revert as one package, legacy authoring-order restock resumes. No save schema change. If signed but later disputed, weights are authored data — retunable without code rollback.

---

# P6 — `PLAN-213-VEHICLE-ARMOR-DECORATION-SEAM`

## P6.1 Identity and why it was partially integrated

Plan 213 (metallurgy reconciliation) deferred vehicle armor as decision **D6** because, at authorization time, "no vehicle owner exists" — there was no system that owned vehicle state, so armor had nothing to decorate. The metallurgy side that *did* land is green: the Silent Foundry's deterministic forging pass (heat·shape·finish·inspect), the forging-button player surface (`FORGING PASS` strip), the CvdDiamond determinism fix, and the full foundry suite (73/73 at the time, since grown).

**Blocker removal:** the vehicle owner now exists and is self-tested: the Plan 50 vehicle system — `VehicleGaragePanel` over the campaign-owned vehicle state, expedition profile decoration seam, trip distance feeding component wear, immobilized vehicles refused dispatch, recovery advancing over campaign days (`--vehicle-garage-selftest` 19/19, landed 2026-09-17 as `WAVE8-B2-PLAYER-ROUTES` row 3). Armor now has a legitimate decoration target: the expedition profile and the vehicle's wear/damage model.

## P6.2 Design

**Scope: armor as a foundry-forged vehicle modification, decorated onto the expedition profile.** The repo's signed pattern for exactly this shape is the Plan 50 decoration seam: mod effects *decorate* the expedition profile; they do not fork the expedition math.

- **Armor item family:** forged through the existing deterministic pass (new recipes in the metallurgy catalog — plate grades from existing metal tiers; NO new materials, NO new item-id namespaces outside the existing foundry recipe catalog).
- **Decoration targets (bounded, typed):**
  1. `expedition_risk_mitigation` — armor plating reduces the vehicle-side damage component of expedition risk (bounded bp, e.g. −1000 to −2500bp by grade), applied only to vehicle-flagged risk events, never to survivor-side risk.
  2. `component_wear_shield` — armored vehicles convert a bounded fraction of trip-distance wear from hull to armor plate; armor plate itself wears and is re-forgable (a maintenance loop that feeds the foundry economy — armor is consumable, not permanent).
  3. Immobilization interaction: a destroyed armor plate cannot immobilize the vehicle (plates are sacrificial); hull wear retains the existing immobilization authority untouched.
- **Non-goals:** no new combat system; no armor on survivor bodies (bionics/AmputationSystem owns that axis); no UI beyond the garage panel's existing mod rows and the foundry recipe list.

## P6.3 Wave breakdown

**Wave 1 — Core armor model + recipes**
- Paths: vehicle system (campaign-owned vehicle state file — recon exact path at implementation; the Plan 50 claim records it) gains additive `armor_plates` state (typed list: grade, integrity bp, fitted day) with capture/restore through the existing vehicle save section (no new section); metallurgy catalog gains 3–4 plate recipes (grade tiers from existing metals); foundry deterministic pass consumes them unchanged (recipes are data).
- Tests: `Plan213VehicleArmorTests.cs` (~12 cases): plate fitting/removal through canonical inventory (transactional, exactly-once charge), plate wear per trip-distance (same wear authority Plan 50 signed), sacrificial conversion bounds, hull authority untouched, save/restore mid-wear fingerprint parity, re-forge restores integrity (never exceeds authored max).
- Acceptance: zero changes to dispatch refusal logic (immobilization rules read hull only); data-integrity PASS with new recipe rows walked clean; foundry suites unchanged and green.

**Wave 2 — Expedition decoration + garage UI + closeout**
- Paths: expedition profile decoration seam (the signed Plan 50 seam — add the risk-mitigation decoration alongside existing mod effects); `VehicleGaragePanel` mod rows list fitted plates with integrity as text (e.g. "armor plate (heavy): 62% — worn"); foundry panel recipe list shows plate recipes (data-driven, no code).
- Tests: decoration host wiring (~6 cases): profile risk computation with/without plates (bounded, typed factor in the existing explain row — legibility), immobilized-vehicle dispatch refusal unaffected by plate state, plate wear exactly-once per trip, 30-day expedition simulation fingerprint (continuous == save/restore), UI source gates (integrity shown as words+number, not color-only).
- Docs: `docs/plans/PLAN_213_VEHICLE_ARMOR_CLOSEOUT.md` (D6 deferral → closed, citing the Plan 50 owner as the unblocking evidence), foundry/economy closeout follow-up notes, `INTEGRATION_PLANS.md` row, claim.
- Acceptance: focused suites (Foundry + vehicle/expedition suites), full `dotnet test Ashfall.Core.Tests`, `--vehicle-garage-selftest` extended (19 → ~24), `--data-integrity-selftest`, a11y + panel lifecycle PASS, build 0/0.

## P6.4 Balance rationale

- Bounded bp mitigation (max −2500bp at top grade): armor must shift expedition *risk composition*, not delete risk — the expedition system's authored risk curves remain authoritative; plates only decorate the vehicle-flagged slice.
- Sacrificial wear: armor creates a foundry demand loop (forge → fit → wear → re-forge) that deepens the metallurgy economy without inflating the money supply — plates cost metals that must be scavenged/traded.
- Grade tiers from existing metals: no new scarcity source; heavy plates compete with other heavy-metal uses (the intended interlock).

## P6.5 Risks and exploits

- **Armor stacking:** fitting N plates must not stack linearly to invulnerability — cap total mitigation at the top-grade bound regardless of plate count (validator + test).
- **Plate cycling exploit** (refit fresh plates before each trip to dodge wear): re-forge costs canonical inventory metals; wear accrues per trip-distance regardless of when fitted — a fresh plate still absorbs that trip's wear. Test pins per-trip wear ordering.
- **Save growth:** plates are a bounded typed list (cap per vehicle authored, default 3); save bloat guarded by the cap.

## P6.6 Rollback

Additive typed state inside the existing vehicle save section + data recipes + decoration rows. Revert as one package; expedition math and immobilization authority never changed, so removal is behavior-neutral for unarmored vehicles (the legacy path). Save loads with unknown plate fields dropped on rollback — plates vanish, hull state intact (verify with a rollback-shape test before handoff).

---

# 7. Cross-Package Sequencing and Final Verification Protocol

## 7.1 Recommended claim order (with rationale)

1. **P1** (content-only on frozen contracts) — lowest risk, immediately visible; also enriches the population P4 will weight.
2. **P2** (two signed decisions) — closes an accepted plan; independent of all others.
3. **P5** (merchant restock) — small pure-rules package; independent.
4. **P6** (vehicle armor) — depends only on already-landed Plan 50; independent.
5. **P3** (semantic kind authority) — broadest consumer migration; do it after P1/P4 add their new events so the vocabulary is classified once, not twice.
6. **P4** (availability consumer) — benefits from P1's richer authored population; lands after P3 so its new traffic-band line inherits kind classification for free.

P2, P5, P6 are mutually disjoint and can be claimed in parallel by different agents (disjoint paths verified against `WORKTREE_OWNERSHIP.md`).

## 7.2 Unified final verification (every package, every wave)

```
dotnet build Ashfall.csproj                 # 0 errors, 0 warnings
bash scripts/run_test.sh <focused suite>    # package-specific
dotnet test Ashfall.Core.Tests              # full suite, zero regressions
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
godot --headless --path . -- --ui-a11y-selftest
python3 scripts/ci/generate-docs-index.py --check
```

## 7.3 Governance checklist per package

- [ ] Fresh claim id in `WORKTREE_OWNERSHIP.md` before first edit
- [ ] Exact paths listed; unrelated dirty-worktree changes untouched
- [ ] Determinism fingerprints stable (continuous == save/restore)
- [ ] No new RNG streams; no new save sections unless frozen-shape migrated
- [ ] Exactly-once semantics proven by test for every player-visible consequence
- [ ] Closeout doc in `docs/plans/`; `INTEGRATION_PLANS.md` row updated; claim marked HANDED_OFF
- [ ] Docs index regenerated and PASS

*End of plan. Total: six packages, 16 waves, every blocker named in the integration ledger now resolved.*