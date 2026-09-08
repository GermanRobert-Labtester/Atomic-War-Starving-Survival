# Plan 73 — Faction Radio Corpus Expansion — Implementation Report & Closeout

**Status: COMPLETE (verification + reconciliation pass).** The 30-broadcast roster was already
implemented and committed (`719545c4`); this pass verified the full acceptance matrix against
repository truth, reconciled the quarantined contract test with later flagship-stream drift,
and unquarantined it after verified compile+pass.

## Repository facts

| Item | Finding |
|---|---|
| Corpus schema | `{schema_version, $schema, version, description, silence_events[12], factions{8}, broadcasts[35]}` |
| Consumer classification | **R1** — `RadioBroadcastCatalog.LoadFactionRadioCorpusJson` is the existing mechanical bridge (genre/reliability/priority derived from `type`; cross-refs carried as `intel:`/`distress:`/`telemetry:`/`quest:` tags). `FactionRadioEngine` provides the independent faction-chatter dual path. No runtime changes needed. |
| ID namespace | Global: canonical roster `radio_faction_*`, integration hooks `radio_patrol_*` (from the Plans 60–63 patrol-radio-hooks stream). No collision with `radio.json` / `year_of_ash_radio.json` / `verdict_radio.json` / `faction_war_radio.json` (test-pinned). |
| RadioTuner authority | `RadioTuner` owns tuning in **KHz** (`TuneTo`/`TuneBy`, clamp ≥0), signal lock via `Evaluate` (5 KHz tolerance, strength = broadcast signal × proximity falloff, VU = strength × (1 − noise floor), `ISeededRng` deterministic). Station coverage spans 14.487–142.85 MHz; faction corpus uses 88.4–142.85 MHz. |
| Signal-strength semantics | 1–9 S-units; in the engine path it is computed from tuning precision (offset falloff); in the schedule path it is carried on the broadcast record. Not decorative. |
| Schedule authority | `RadioScheduleCoordinator.Resolve` — eligible set filtered by frequency/day, sorted by priority (Emergency > Urgent > Important > Routine), deterministic rng pick among ties. Faction-corpus type→priority map: distress Urgent; patrol/military/dead-hand Important; others Routine. |
| Save/history owner | Radio save store — `--radio-selftest` pins history/frequency/played-dedup across save/load with tamper rejection. |
| Quest hooks | `quest_hook` is an **intel tag** (`quest:<id>`), not a mechanical trigger. The three hook IDs (`quest_patrol_bounty`, `quest_missing_caravan`, `quest_orphaned_stock`) are reserved runtime vocabulary in `CatalogIntegrityValidator.KnownRuntimeIds` ("Radio rumor / broadcast quest hooks"). |

## Broadcast roster (35 = 30 canonical + 5 integration hooks)

Canonical Plan 73 roster — 3 per type across all 10 required types (patrol check-in/missing/border,
supply medicine/fuel/filter, propaganda work-order/mutual-aid/closed-gates, distress
patrol-ambush/clinic/convoy, encrypted repeating/short-burst/key-change, military
corridor/checkpoint/withdrawal, civilian water/family/market, dead-hand readiness/orbital/command,
weather ash/cold/runoff, inventory fuel/food-medical/tools) — all IDs, factions (12 distinct),
frequencies (88.4–142.85), signal strengths (2–9), and day gates (1–9999) resolve.
Silence events: 12 retained. The 5 `radio_patrol_*` entries were added later by the patrol-radio
hooks stream (type `patrol_report`, unscheduled, day 1–300).

## Required integrations

- **8 scheduled** (`scheduled: true`): patrol north-culvert, supply clinic, propaganda work-order,
  distress patrol-ambush, encrypted repeating-groups, military convoy-corridor, dead-hand
  readiness-check, weather ash-front — all eligibility-pinned through `RadioScheduleCoordinator`.
- **5 patrol/territory**: the 3 canonical patrol reports + military checkpoint-reinforce +
  military withdrawal-order (loc intel refs resolve).
- **3 quest hooks**: as reserved runtime quest IDs (see facts table).
- **3 distress / 3 cipher / 3 telemetry**: distress calls are urgent-priority corpus entries;
  encrypted bursts tagged cipher-traffic; dead-hand pings carry no living faction and map to
  `AutomatedLoop`/`Automated`. Radio is delivery, not crisis/cipher/telemetry authority.

## Drift reconciliation (the 4 quarantined-test failures)

1. **30 vs 35 count** — later stream added 5 hook broadcasts. Test now asserts 30 canonical +
   5 `radio_patrol_*` hooks explicitly.
2. **patrol_report 8 vs 3** — same cause; test asserts 3 canonical per type + 8 patrol total.
3. **Quest hooks vs `dynamic_questlines.json`** — the test read `quests[]`/`questlineId`, but the
   file (both then and now) uses `questlines[]`/`quest_id` and contains only 2 unrelated
   questlines; the quarantined assertion could never have passed. Reconciled to the actual
   authority: hooks resolve against `CatalogIntegrityValidator.KnownRuntimeIds`. Radio-triggered
   quest START remains owned by the questline system (deferred grammar).
4. **142.85 relay-band winner** — the later radio-authority stream added a permanent Urgent
   automated teletype (radio.json, day 20–300) plus battery-countdown (80–365) / continuity-roll
   (200–365) / Year-of-Ash traffic (dayTrigger 180+, never closing). Dead-hand pings (Important)
   can no longer deterministically WIN resolution on the band. Test updated to pin the true
   contract: all three pings remain **eligible** through the canonical catalog, the resolved
   broadcast is always within the eligible set, resolution is deterministic, and the
   `FactionRadioEngine` dual path keeps the band reachable as chatter.

## Verification

| Command | Result |
|---|---|
| `dotnet test --filter FactionRadioBroadcastExpansion` | **22/22 PASS** (unquarantined) |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **9461/9461 PASS** |
| `godot --headless -- --data-integrity-selftest` | **PASS** — 0 findings, 298 catalogs |
| `godot --headless -- --content-utilization-selftest` | **PASS** — CI gate |
| `godot --headless -- --radio-selftest` | **PASS** — history/frequency/dedup persist; tamper rejected |
| `dotnet build Ashfall.csproj` | **PASS** — 0 errors |

Headless patrol/distress/cipher/dead-hand journeys (§64) are covered by the schedule-resolution
and dual-path facts inside the 22-test contract (frequency eligibility, priority surfacing,
determinism, engine bridge); no additional verbs were invented.

## Deviations from the roadmap

1. Roadmap assumed "corpus dominated by silence events, no broadcast collection." Evidence: a full
   30-broadcast roster already shipped (`719545c4`). Adaptation: verification/reconciliation pass
   instead of authoring. Data-first architecture preserved.
2. Roadmap §47 wanted broadcasts to "start real questlines." Evidence: quest authority has no
   radio-trigger grammar; `quest_hook` is a tag. Adaptation: hooks pinned as reserved runtime
   vocabulary; quest-start integration deferred to the questline owner.
3. Roadmap §50 wanted dead-hand pings surfaceable on the relay band. Evidence: later canonical
   automated traffic permanently outranks them there. Adaptation: eligibility + determinism pinned;
   band priority documented.

No data files required changes; changed files are the test csproj (unquarantine) and the
contract test (reconciliation).
