# Plan 30 — The War Runs Without You: An Autonomous Outside World

> **Wave:** Continuity Wave 4 — *The World Beyond the Gate* (Plans 30–34)
> **Predecessors:** [W1](Wave1_Continuity_Audit_INDEX.md) story,
> [W2](Wave2_Continuity_Audit_INDEX.md) physics, [W3](Wave3_Continuity_Audit_INDEX.md) ship.
> **Depends on:** 31 (the world's doings must be reportable), 28A (registration so a new day owner
> cannot forget its save/flush half).
>
> **Theme:** ASHFALL's factions have standing, territorial control, decrees, war tension, and
> artillery strikes — and a method called `SimulateDailyFriction(day)`. **Nothing in the game ever
> calls it.** The world outside the hatch is a frozen tableau that only moves when the player
> pushes it. For a game about being small in a large catastrophe, that is the last missing wall.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | The faction-war model is real and rich | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` — `FactionStandingRecord { standing −100..+100, territorialControlPercent 0..100, isHostile, isAllied }`, `FactionWarSystemState { factions, activeWarTension = 50, dominantFactionId = "faction_central_garrison", enactedDecrees, totalArtilleryStrikesLogged }`, and three events: `OnFactionStandingChanged` (`:40`), `OnDecreeEnacted` (`:41`), `OnTerritorialClashOccurred` (`:42`) |
| 2 | **It has a daily simulator that nobody calls** | `FactionWarSystem.cs:87 SimulateDailyFriction(int day)` — `grep -rn "SimulateDailyFriction"` returns the declaration + **only test files** (`Ashfall.Core.Tests/YearOfAshTests.cs:161,381,385,389`, `WarlordDoctrineTests.cs:487`). Zero game-path callers |
| 3 | Its owner is live but idle | `src/YearOfAsh/YearOfAshHostSession.cs:29,47` exposes/constructs `FactionWar`; `TickDay(int day)` (`:136–142`) ticks timeline, deep freeze, radon, and **`TickWarlord(day)`** — and never touches `_factionWar` |
| 4 | The war-chain runner is save-only | `FactionWarChainRunner` appears in `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs:34,113,148` (state fields) and its own file — **no construction in `src/`**; `FactionWarChainRunner.ResolveChoice` (`:349`) therefore has no caller in the host |
| 5 | Presentation already expects live data | `src/YearOfAsh/FactionWarMapWidget.cs:12` — "Thin presentation only: queries `FactionWarSystem` and `YearOfAshTimelineSystem`"; it renders a world state that never changes |
| 6 | The player's own acts do land — one directionally | `src/Host/HostCli.SelfTests.cs:209–215` asserts standing consequences land in `FactionWarSystem` — i.e. input exists, ambient output does not |
| 7 | Faction data is authored and consumed elsewhere | `--data-integrity-selftest`: 138 catalogs / 5563 ids; the Factions family is 15 catalogs, all gameplay-consumed per `artifacts/content-utilization.md`; `faction_radio_corpus.json` is loaded into `FactionRadioEngine` (`src/Main.Economy.cs:190–192`) — so a radio channel keyed on factions already exists to receive news |
| 8 | Price shocks are modelled but only panel-side | `src/Economy/TradeScreenGodotPanel.cs:546` — `PriceShockKind.{PriceShockKind.PlumePassing, ConvoyAmbush, FactionWar, WinterDeepens}` used for icons/presentation; `HardcoreEconomyTuning` is applied with **three `Array.Empty` collections** live (`src/Main.Economy.cs:194–199`), so no authored shock rule runs (see Wave 4, Plan 34) |
| 9 | The day loop has room | 19 registered owners in 5 phases (`src/Main.CampaignOwners.cs:42–46` shows phase 4/5 registrations); a `world_politics` owner in phase 4 is the natural home |
| 10 | The atlas promised this | `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` §11 lists *Warlord Doctrine & Shortage Debt* as High-Leverage → "Triggers → Airlock Breaches"; doctrine is ticked (`:175 _warlord.TickDaily`), the war beside it is not |

**Reading:** unlike Waves 1–3, this is not a broken link — it is an **unlived simulation**. The
Core method already exists, is already tested, and is already deterministic. Wiring it in is small;
designing consequences for it is the work.

---

## Task 30A — Put the world on the clock: a `world_politics` day owner

**Goal:** faction standing, tension, territory, and decrees advance every day from their own rules,
persist correctly, and are attributable in the briefing (via Plan 31's event kinds).

**Files:** `src/Main.CampaignOwners.cs`, `src/Main.World.cs`, `src/YearOfAsh/YearOfAshHostSession.cs:136–142`,
`Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`, `FactionWarChainRunner.cs`,
`YearOfAshSaveStore`/`SaveSectionRegistry`, new `docs/systems/WORLD_POLITICS.md`.

### Substeps

1. **Read `SimulateDailyFriction` before anything else** — what it mutates, which events it raises,
   what its determinism depends on (`YearOfAshTests.cs` pins 4 call patterns; use them as the spec).
2. **Call it from `YearOfAshHostSession.TickDay`**, beside `TickWarlord`, with the session's own
   `ISeededRng` stream — never a fresh RNG and never `System.Random` (Invariant 4).
3. **Register a `world_politics` day owner** (or extend `narrative_quests_verdict`/`world_evolution`
   if phase ordering already fits — prefer extending over adding) so the war advances inside the
   campaign envelope rather than on a side timer.
4. **Instantiate `FactionWarChainRunner` in the host** and join its `ResolveChoice` to the
   encounter-resolution idiom already used three times elsewhere
   (`ExpeditionHostSession.cs:432`, `Main.YearOfAsh.cs:300`, `DutyRosterHostSession.cs:151`) — the
   chain state exists in the save shape today and is currently never driven.
5. **Decrees need a source**: either data (`faction_*.json` decree definitions with effects and
   windows) or the existing `enactedDecrees` list fed by chain outcomes — do not hardcode decree ids
   in the owner.
6. **Report every transition as a semantic event** (Plan 31's vocabulary):
   `standing_shifted`, `tension_stage_changed`, `territory_changed`, `decree_enacted`,
   `clash_occurred`, `dominance_changed` — the three existing Core events map one-to-one onto these.
7. **Persist and prove it**: `YearOfAshSaveStore` already carries war state; add a round-trip test
   asserting standing/tension/territory survive save → load → 30 more days identically, and that the
   checksum changes when the war changes.
8. **Rate-limit the noise**: at most N political lines per day in the briefing, aggregated by
   significance, so a 20-faction drift doesn't drown the ration report.
9. **Determinism gate**: paired-seed 180-day replay
   (`ashfall-seed-replay`) asserting the war state digest is identical; territory percentages must
   not depend on collection order (ordinal iteration, documented).
10. **Performance**: the daily political pass must be negligible against the W3 budget
    (`day_advance_30d` median 0.609 s) — measure, don't assume.
11. **Backfill from the calendar**: on load of an old save, catch the war up from
    `lastAdvancedDay` to today in one deterministic loop rather than skipping days.
12. **Tests**: friction per tension stage, decree effects, dominance handover, chain-runner
    resolution, save round-trip, replay determinism.
13. **Run the five-step verification checklist** + `bash scripts/ci/triad-drift-gate.sh`.

**DoD:** on day 90 the world is not where the player left it, and the briefing says why.

---

## Task 30B — Let the war reach the player: prices, radio, encounters, the map

**Goal:** political state must change what the player experiences, using channels that already
exist — radio corpus, price shocks, expedition encounter risk, map control overlay, airlock
defense. No new "world events" framework.

**Files:** `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs`,
`Economy/MarketSystem.cs`, `HardcoreEconomyTuning.cs` (+ 34A), `Radio/FactionRadioEngine.cs`,
`src/Main.Economy.cs:188–205`, `Expeditions/ExpeditionSystem.cs` (risk),
`src/YearOfAsh/FactionWarMapWidget.cs`, `src/UI/MapAtlasPanel.cs`,
`AirlockSecuritySystem.cs`, trade/faction data catalogs.

### Substeps

1. **Define the reach matrix** first: for each faction-war output (standing band, tension stage,
   territorial control %, dominant faction, active decrees) name the channel it should move and the
   magnitude — then implement only rows with an existing consumer.
2. **Prices**: faction stance + tension → the `PriceShockKind.FactionWar` and `ConvoyAmbush` paths
   already referenced in the trade screen, driven from the tuning bundle (coordinate with 34A, which
   loads the real `hardcore_economy_tuning.json` instead of three empty arrays).
3. **Radio**: `FactionRadioEngine.LoadFromJson(faction_radio_corpus.json)` is live at
   `src/Main.Economy.cs:190–192` — gate/weight broadcast selection by standing band and tension
   stage, so the wireless reports the war the player is in. Defer new prose; the corpus exists.
4. **Encounter risk and disposition**: `ExpeditionSystem` risk should read the *territory* owner of
   the route (30C's graph) — a sector controlled by a hostile faction changes both odds and who
   appears, reusing `ExpeditionEncounterBridge` rather than a new encounter type.
5. **Trade availability**: a faction at Blood Feud refuses or embargoes; wire this through
   `FactionStanceEngine`'s existing stance bands (the live campaign engine, per Wave 1's 16B — not a
   fresh instance).
6. **Gate defense pressure**: `AirlockSecuritySystem` already models sentries and incidents; the
   atlas says warlord debt "triggers airlock breaches" — connect tension stage to that existing
   path so politics arrives with consequences at the hatch.
7. **Map overlay**: `FactionWarMapWidget` should show control shifts as they happen, with the
   change (not only the current state) surfaced — click a territory → the decree/clash that caused
   it (17A's attribution pattern).
8. **Refugee and census ripple**: `AirlockSecurityHostSession` mentions visitor triage; population
   pressure from a neighbouring war is the cheapest way to make the outside *arrive* — use
   `VoluntaryRegisterSystem`/`CensusClaimSystem` (both live inside `DoseLedgerHostSession`) so new
   mouths are accounted by Wave 2's food and duty systems.
9. **No surprise without a trace**: every externally-caused change (price, closure, raid, refusal)
   must have a briefing line, a journal entry, or a radio source — never a silent value mutation.
10. **Tone discipline**: this is where prose can creep into mechanics. Route authored text through
    `ashfall-write` and the Wave 3 25A/25C string layer so nothing new is hardcoded in C#.
11. **Balance**: `ashfall-balance-sim` with war-tension as a swept parameter; a hostile world must
    tighten the screws, not make the campaign unwinnable. Record the curves.
12. **Tests**: one test per reach-matrix row, plus an integration test that a war shift on day 60
    changes a day-65 price, a day-65 radio item, and a day-65 encounter.
13. **Run the checklist** + `--content-utilization-selftest` (corpus catalogs should gain runtime
    evidence).

**DoD:** the player can name, from evidence in-game, who is winning the war and what it costs them.

---

## Task 30C — Neighbours act: make the other settlements have their own days

**Goal:** extend autonomy beyond faction arithmetic to the places and people the player trades with —
caravans, waystations, deep coast, flotilla — so the outside has a schedule of its own.

**Files:** `TravelingCaravanSystem.cs`, `Waystation*` (per `waystation_network` route),
`District8DeepCoastSystem.cs`, `Maritime/*`, `WildlifeMigrationSystem.cs`,
`LocationEvolutionSystem.cs`, `LandmarkDegradationSystem.cs`, `WeatherIntelligenceCoordinator.cs`,
day-owner registrations, weather/trade data catalogs.

### Substeps

1. **Audit the "already autonomous" set**: `world_evolution` already ticks location evolution,
   wildlife migration, and landmark degradation (it emits 7 event sites, incl. landmark collapse) —
   start by listing which systems self-advance and which only react, so this task adds only what's
   missing.
2. **Caravans get intentions**: `TravelingCaravanSystem` currently arrives on a schedule; give it
   origin/destination chosen from live political + weather state (30B reach, 32's graph), so a
   trader showing up is a world fact rather than a timer.
3. **Waystations as world nodes**: tie `waystation_network` state into the travel graph (32) so an
   abandoned station is a consequence of the war, not a static description.
4. **Wildlife responds to the world, not only to the calendar** — migration already exists; let
   contamination (20A) and territory (30B) bias routes so hunting grounds move for legible reasons.
5. **Deep coast / flotilla autonomy**: `District8DeepCoastSystem` and maritime systems should
   respond to the same political inputs (blockade, tribute demand) so the coast isn't a separate
   theme park.
6. **Propagate information, not teleportation**: what the player learns about distant facts should
   pass through the intel channels (33) — a distant raid is not visible until radio, a caravan, or a
   scout carries it. That single rule turns autonomy into drama instead of noise.
7. **Budget the daily pass**: every autonomous system already runs inside a day owner; keep total
   day-advance cost inside the W3 budget and add per-owner timing to the existing advisory output.
8. **Deterministic ordering** across autonomous owners (documented phase order; ordinal iteration) —
   otherwise two systems mutating the same faction read differently per run.
9. **Semantic events** for each autonomous change (31) so nothing happens off-camera.
10. **Persistence**: prove each autonomous system's state round-trips inside the campaign envelope,
    and that a save loaded 40 days later catches up consistently (30A step 11's mechanism, shared).
11. **Player-visible calendar**: a compact "known happenings" view — where did the caravan go, who
    holds the sector, what collapsed — assembled from briefing/journal data, not a new truth.
12. **Tests**: per-system autonomy assertions (state advances with zero player input, verified by a
    no-action N-day run), cross-system propagation tests, information-gating tests (unrevealed facts
    are not actionable), determinism and save round-trips.
13. **Run the checklist** + `--expansions-selftest` + `--expedition-selftest`.

**DoD:** leave the hatch alone for thirty days and the world has stories to tell you when you
return — delivered through channels you have to maintain.

---

## Cross-Task Dependencies

```
31 (semantic event kinds) ──► 30A step 6 (report transitions) ──► 30B (reach the player)
28A (manifest)              ──► 30A step 3 (an owner that can't forget save/flush)
34A (real tuning bundle)    ──► 30B step 2 (price shocks need authored rules)
32 (travel graph)           ──► 30B step 4, 30C steps 2–3 (routes and territories)
33 (intel channels)         ──► 30C step 6 (knowing about it)
```

**Execution order:** 31 → 30A → 30B → 30C. Landing 30A without 31 gives a briefing full of
`market_ticked`-style noise and no politics; landing 30B before 30A gives consequences with no
cause. Wave-level: 31 must precede 30 and is the corrective successor to Wave 1's 17A (see the
erratum in `Wave4_Continuity_Audit_INDEX.md`).

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --expansions-selftest            # YoA/faction surfaces
7. ashfall-seed-replay: 180-day war-state digest identical
8. ashfall-balance-sim: tension-swept price/raid curves
9. no-action 30-day run: autonomous state advances, briefing reports it
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 30A | 1–2 | 3 | 0–1 | 0 | 8–12 | Low–Med (the API exists) | MEDIUM (daily drift changes all baselines) |
| 30B | 2–3 | 3 | 1–2 | 2 | 10–14 | Medium–High | MEDIUM (balance-wide) |
| 30C | 3–4 | 3 | 1 | 1 | 12–16 | **High** | MEDIUM–HIGH (ordering/determinism) |

**Guardrails:** no new faction, no new war mechanic, no new world-event framework, no new prose in
code. The world's autonomy must be **experienced through information** (33), never by handing the
player a live omniscient map — the game's premise is scarcity of knowledge, not abundance.
