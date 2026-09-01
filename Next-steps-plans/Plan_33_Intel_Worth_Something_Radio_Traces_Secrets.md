# Plan 33 — Intel Has To Be Worth Something: Radio, Traces, and Secrets

> **Wave:** Continuity Wave 4 — *The World Beyond the Gate*
> **Depends on:** 31 (intercepts must be reportable), 32C (reveals must unlock travel), 23 (a
> receiver needs watts), 26A (radio catalogs must resolve through the shipped data path).
>
> **Theme:** 118 authored broadcasts, a live tuner with triangulation, a faction radio corpus loaded
> from JSON, `days_to_trace` distress signals with clarity-graded fragments — and **no outcome**.
> `radio_distress_signals.json` has no loader at all, `knowledge_points` has zero consumers, and the
> one real result the radio produces (`OnLocationRevealed`) unlocks nothing. Listening is not an
> activity in this game; it is a screensaver with static.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | Radio infrastructure is real | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` (`OnLocationRevealed` at `:83`, `:188`), tuner + `freq_` ids, `RadioHostSession.cs`, `TriangulationPanel.cs`, `Main.Narrative.cs:206` subscriber; 4 radio cues wired in 7B (tune/signal-lock/morse/static) |
| 2 | Broadcast volume is large | `radio.json` (50) + `year_of_ash_radio.json` (50) + `verdict_radio.json` (13) + `radio_distress_signals.json` (5) = **118**; 10 authored broadcasts already carry `audio_cue` VO references, 10 VO assets produced (7B) |
| 3 | **The traceable-signal catalog is dead content** | `radio_distress_signals.json`: `{schema_version, radio_broadcasts[]}` with `frequency_id: "freq_distress_217_4"`, `frequency_mhz: "217.4"`, `source_name: "Checkpoint Kilo Automated Beacon"`, `outcome_type: "survivor_community"`, `days_to_trace: 4`, `message_fragments[] {day, clarity (0.25…), text}` — `grep -rln "radio_distress_signals"` outside the scanner → **nothing loads it** |
| 4 | **`knowledge_points` has no consumer** | `grep -rn "knowledge_points\|KnowledgePoints" Assets/Ashfall.Core src/` → **0 hits**; the field exists in authored data only |
| 5 | Reveals are labels | `RadioHostSession.cs:60` sets `LastEvent = $"Location discovered: {id}"`; `TriangulationPanel.cs:53` forwards to a UI callback; expedition targeting validates *known locations*, not revealed ones (`ExpeditionHostSession.cs:225,376`) |
| 6 | Interception already reports | `radio_intercept` is one of the 7 day-event kinds the briefing actually renders (`DailyBriefingReportBuilder.cs` case) — the reporting seam exists and works |
| 7 | Reliability is modelled in parts | device `Battery`/`Calibration` persist in inventory slots (`Inventory.cs:978`-region `SlotSave.hasDevice/battery/calibration`), `geiger_calibration` route exists, `AudioConditionSystem` exists, and weather/EMP states exist (20C) — but nothing multiplies them into signal clarity |
| 8 | Radio equipment is unserviceable | `tropospheric_radio_relay` is one of Wave 1's 30 unbacked consoles (`16A`), and the antenna/relay idea is exactly what this plan should give authority instead of deleting |
| 9 | Listening costs nothing | no power draw, no time, no duty slot, and no exposure to consequence; radio is free information in a game whose economy is scarcity |
| 10 | Secrets are authored and inert | `narrative_arc_events.json` (15) + `environmental_texts_expansion_05.json` (36) + `audio_logs_expansion_05.json` (30) + `moral_choice_quest_stubs.json` (10) sit in `exempt_no_source_evidence` (Wave 1's 18B), including the multi-stage cipher-broadcast idea the registry recommends ("Create multi-stage cipher broadcast ARG") |

**Reading:** radio is the game's cheapest source of *want* — a reason to go somewhere you didn't
know existed. Wave 4's plans 30 and 32 create the world; this plan is what makes the player learn
about it **instead of being told**.

---

## Task 33A — Distress signals become objectives you can act on

**Goal:** turn `radio_distress_signals.json` (5 authored signals with trace windows and outcomes)
into live, traceable, resolvable events — the smallest possible end-to-end proof that intel pays.

**Files:** new `Assets/Ashfall.Core/Radio/DistressSignalCatalogLoader.cs` +
`DistressSignalSystem.cs`, `SignalTriangulationSystem.cs`, `RadioHostSession.cs`,
`src/UI/RadioPanel.cs` / `TriangulationPanel.cs`, `SaveSectionRegistry`, `src/Main.Narrative.cs`,
`Ashfall.Core.Tests/DistressSignalTests.cs` (new).

### Substeps

1. **Read the data before designing** (it already specifies the mechanic): `days_to_trace`, the
   per-`day` `message_fragments` with `clarity`, `outcome_type` (`survivor_community`, …),
   `frequency_id`/`frequency_mhz`, `source_name`. Model the system on those fields, not the
   reverse.
2. **Loader** in Core using `SystemTextJsonSerializer` + the `CatalogDiagnostics.Warn(path, shape,
   ex)` pattern (H4's fix), so a malformed catalog is loud instead of silently empty.
3. **Register the section** with `SaveSectionRegistry` + a `SaveStore<T>` façade through
   `SaveStoreHub` (Initiative #41), so the new state cannot ship unchecksummed — and add it to the
   campaign envelope so a mid-trace save resumes exactly.
4. **Emit the day-event kinds** (31's vocabulary): `signal_detected`, `trace_progress`,
   `signal_resolved`, `signal_lost` so the briefing and the diagnostics both see it.
5. **Trace over days**: clarity accumulates per the authored fragments; the trace can stall or be
   lost (weather, power, equipment condition) — a trace must be *maintainable*, not automatic.
6. **Wire outcomes per `outcome_type`**: `survivor_community` → a new map node revealed
   (32A/32C) and a possible population addition through the existing visitor/census path
   (`AirlockSecurityHostSession` visitor triage + `VoluntaryRegisterSystem`/`CensusClaimSystem`);
   caches → loot at a node via the existing expedition target list; threats → encounter risk (30B).
   One handler per outcome type, registered from data.
7. **Cost the attention**: tracing occupies a duty slot or a radio operator's hours (24's labour
   ledger) and draws power (23) — intel competes with everything else, which is the point.
8. **Make the tuner feel like an instrument**: keep the existing cue set (tune/static/signal-lock/
   morse) and use clarity to drive signal quality; do **not** add a new audio family.
9. **Remove the mystery meat**: no hidden RNG the player can't influence — reliability is a function
   of the visible facts from 33B.
10. **Tests**: load + malformed-warn, trace progression per authored day, loss on storm/power,
    outcome → node reveal, outcome → population/loot, save mid-trace, determinism, and a content
    test asserting every authored `freq_distress_*` resolves to a playable chain.
11. **Delete the exemption** for `radio_distress_signals.json` from the content-utilization
    exemption list once it has a consumer (Wave 1's 18B step 12 discipline).
12. **Docs**: `docs/radio/SIGNALS.md` (or extend `docs/narrative/`) with the trace rules and outcome
    table, citing file:line per Wave 3's 29B.
13. **Run the checklist** + `--data-integrity-selftest` + `--content-utilization-selftest`.

**DoD:** five authored distress signals become five playable "we heard something — do we go?"
decisions with outcomes.

---

## Task 33B — Reliability: a signal can lie, and that's the interesting part

**Goal:** make reception a function of equipment, environment, and expertise, so the player must
decide how much to trust what they heard — the information economy the atlas already diagrams
(§12) with the mechanics to back it.

**Files:** `Assets/Ashfall.Core/AudioConditionSystem.cs`, `SignalTriangulationSystem.cs`,
`RadioHostSession.cs`, `WeatherSystem` (20C), `PowerGridSystem` (23), `RadiationSystem` /
`EquipmentConditionSystem` (21), inventory device state (`Battery`/`Calibration`),
`geiger_calibration` route, `src/UI/RadioPanel.cs`, reliability data catalog.

### Substeps

1. **Define one reception quality value** (0..1) in Core: device condition × battery × calibration
   × antenna/relay state × weather/EMI × operator skill — one function, one place, testable in
   isolation.
2. **Expose it as a breakdown, not a number**: "signal degraded — calibration off, storm overhead"
   via 31's attribution, so a false reading is explainable after the fact.
3. **Model misreading honestly**: below a threshold, fragments resolve to *wrong* or *ambiguous*
   text/coordinates (from the authored fragment `clarity`), and the correction is a re-trace, not a
   save-reload — determinism preserved via `ISeededRng`.
4. **Jam and outage as states**: an EMP-class storm, a powered-down shelter, or a broken relay should
   *remove* channels temporarily (20C's weather effects, 23's load shedding), so the radio is a
   system with dependencies, not an island.
5. **Decoding as a knowledge axis**: `knowledge_points` (0 consumers today) becomes the reward for
   partial decrypts — routed into the existing knowledge/research progression, not a new currency.
6. **Cipher arcs**: implement or retire the registry's "multi-stage cipher broadcast" recommendation
   using the existing `narrative_arc_events.json` (15 defs, currently `exempt_no_source_evidence`):
   a chain of broadcasts → a location → a real outcome. If it isn't going to happen this wave,
   downgrade the registry row (29B).
7. **Operator identity**: who listens matters (24A's fitness, 24B's skill) — a tired night watch
   misses things, which converts radio into scheduling rather than menu-diving.
8. **Persist device state** through the inventory section that already saves battery/calibration,
   and prove a reloaded campaign's receiver is still mis-calibrated.
9. **Counter-information**: with 30A running, factions can broadcast **false** news — a hostile
   faction's corpus content should sometimes mislead, discoverable only via a second source (radio
   vs caravan vs scout). This is the highest-value link in the plan.
10. **Corroborate**: the UI should be able to show two independent reports of the same fact and mark
    confidence rising when they agree — knowledge, not omniscience (32C step 9).
11. **Cost**: listening burns power/time/battery; the panel shows what a night of watching the
   dial costs.
12. **Tests**: reception quality per factor, wrong-resolution determinism, jamming states,
    knowledge award path, false-intel detectability, save round-trip.
13. **Run the checklist.**

**DoD:** the radio can be wrong, the player can tell how wrong, and finding out costs something.

---

## Task 33C — The radio network as infrastructure you maintain

**Goal:** give `tropospheric_radio_relay` (and the network idea it gestures at) a real Core
authority, so comms are assets with coverage, upkeep, and failure — and so the relay console stops
being one of Wave 1's 30 fake affordances.

**Files:** new `Assets/Ashfall.Core/Radio/CommsNetworkSystem.cs`,
`Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` (`tropospheric_radio_relay`),
`src/Main.PlayerSurfaces.cs` (route binding), `src/UI/TroposphericRadioRelayPanel.cs`,
`WaystationNetwork*`, `ExpeditionSystem` (field comms), `SaveSectionRegistry`, comms data catalog.

### Substeps

1. **Start from 16A's verdict**: this console is shelved until it has an authority. This task is
   that authority — if capacity is short, delete the route instead and leave the network idea on the
   backlog; a working plan that ends in deletion is still a win.
2. **Model relays as graph nodes** (32A) with coverage radius, line-of-sight modifiers from terrain
   and weather, and a condition that degrades (21's ledger) and is repairable by a crew shift (24).
3. **Coverage gates three things**: intel fidelity (33B), expedition contact (a sortie out of
   coverage cannot be retasked or warned — the storm payoff from 20C), and caravan arrival
   notification (30C).
4. **Author it in data**: relay definitions, coverage curves, upkeep intervals, and failure modes in
   a new `comms_network.json` (`schema_version`, snake_case, ids gated), not in C#.
5. **Build/repair costs** through the existing construction/recipe path so the network competes with
   every other shelter project.
6. **Adversary interaction**: with 30A running, a hostile faction can suppress or seize a relay —
   resolve through territory control (30B), not a bespoke event.
7. **Bind the panel to the live authority** (Wave 1's 16B pattern; `ReferenceEquals` assertion, no
   `new CommsNetworkSystem()` at bind time) and give it at least one mutating action that 15C's
   liveness gate can see.
8. **Emit events** (`relay_built`, `relay_degraded`, `relay_lost`, `coverage_reduced`) into 31.
9. **Persist** via a new section in `SaveSectionRegistry` + `SaveStoreHub` façade; add to the
   save-store contract matrix so a checksummed envelope is required from birth.
10. **Tests**: coverage math, upkeep/failure, retask-gating for out-of-coverage sorties, seizure by
    territory change, save round-trip, determinism, liveness gate satisfied.
11. **Update the registry row** ("Analog Shortwave Radio Tuner" / relay recommendation) with the
    evidence pointer, and re-run the atlas docs pass (29B).
12. **Snapshots** for the panel in three network states.
13. **Run the checklist** + `--audio-selftest` (radio cues share this surface).

**DoD:** comms coverage is an asset you build, maintain, and lose — and the console behind it is
real.

---

## Cross-Task Dependencies

```
32A/32C (place + reveal) ◄──── 33A step 6 (outcomes need places to open)
31A (kinds)              ◄──── 33A/33B/33C event emission
23 (power) + 21 (condition) ◄── 33B steps 1,4 and 33C step 2
30A (world autonomy)     ◄──── 33B step 9 (false intel) and 33C step 6 (suppression)
        │
33A (signals → outcomes) ──► 33B (can I trust it?) ──► 33C (how do I keep hearing at all?)
```

**Execution order:** 33A → 33B → 33C, and inside Wave 4: **31A → 32A → 30A → 33A**. 33A without
32A/32C has nowhere to point a revealed signal; 33B without 30A has nothing worth lying about.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --audio-selftest                 # radio cues intact
7. godot --headless --path . -- --content-utilization-selftest   # distress catalog: consumed
8. bash scripts/ci/generate-save-store-matrix.sh --check         # new section (33C)
9. ashfall-dialog-graph-lint + ashfall-narrative-continuity      # broadcast reachability
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 33A | 2 new | 2 | 1 (already authored) | 1 | 10–13 | Medium | LOW (additive, one dead file revived) |
| 33B | 2–3 | 2 | 1 | 1 | 10–14 | Medium–High | MEDIUM (untrusted info can frustrate — tune thresholds) |
| 33C | 1 new | 2 | 1 new | 1 | 8–12 | Medium | LOW (route already shelved by 16A) |

**Guardrails:** no new radio mechanic beyond what `radio_distress_signals.json` already specifies;
no new audio production batch; no procedural "signals" the player can't reason about; no intel
without a channel (the atlas's §12 rule); and no lie the player can never disprove — misdirection
must be investigable, or it reads as the game cheating.
