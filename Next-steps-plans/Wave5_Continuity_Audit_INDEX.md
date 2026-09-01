# Continuity Wave 5 — Audit Index (Plans 35–39): *The Human Interface*

**Snapshot audited:** `ccac926e` (branch `main`, 95 uncommitted paths) · **Date:** 2026-08-31
**Gates re-run:** `dotnet build Ashfall.csproj` 0/0 · `dotnet test` **5303 passed / 0 failed** ·
`--data-integrity-selftest` **PASS 138 catalogs / 5563 ids / 0 errors** · `triad-drift-gate` PASS ·
`warning-baseline-gate` PASS · `doc-link-gate` PASS · still red from Wave 3: `agent_rulebooks_sync`,
`docs_index_drift`, `agent_skills_catalog_drift`.

Prior waves: [W1 story](Wave1_Continuity_Audit_INDEX.md) · [W2 physics](Wave2_Continuity_Audit_INDEX.md)
· [W3 ship](Wave3_Continuity_Audit_INDEX.md) · [W4 world](Wave4_Continuity_Audit_INDEX.md).

Waves 1–4 covered *does it connect*. Wave 5 asks the two remaining player-facing questions:
**does what you make arrive**, and **can a human actually operate this thing** — with hands, with
eyes, over hundreds of hours, without losing a run.

---

## The headline number this wave

Core exposes **147** integration-shaped public methods (`Bind* / Set* / Wire* / Register* / Apply* /
Enable* / Configure*`). **74 have no caller in `src/`.** Triage of the named ones:

| Method | Core refs | Test refs | Meaning |
|---|---:|---:|---|
| `ApplyGrief` | 1 (itself) | **3** | **only tests invoke grief** — `IGriefSink`/`DeathQuality` landed in `b48b4494` and the game never calls it |
| `ApplySedative` | 1 | **3** | a medical behaviour proven in xUnit, unreachable in play |
| `ApplyChoice` | 1 | 0 | declared, referenced by nothing |
| `RegisterFactions` | 1 | 0 | a catalog seam nobody uses |
| `ApplyOpenWindowNeeds` | 1 | 0 | a ventilation→needs effect with no producer |
| `ApplyTreatment` | **7** | 0 | reached through Core's medical pipeline — **live**, must be classified, not deleted |

This single table is the mechanical explanation for everything Waves 1–4 kept finding: `ServeMeal`,
`SetHunterSkill`, `SetCellar/SetRefrigeration`, `ConsumeRation`, `OnInventoryConsumeClicked`,
`TryResolveMoralChoice`, `SimulateDailyFriction`, null `Consume` callbacks, `DegradeRate = 0f` —
**Core seams the host never plugged in, with nothing standing between them and a green build.**
The fix already exists in this codebase (`CombatHostSession.ValidatePorts`, `:145–165`); Plan 36
generalises it.

### Audit self-correction (recorded, per Wave 3's 29B)
My first pass at the input finding read "15 of 16 hotkeys dead" from a constant-usage grep. That was
wrong — `AshfallInputActions.cs:138–207` wraps actions in `Is*` predicates, 10 of 13 of which **are**
called. Plan 37 states the verified version: 3 predicates uncalled, **4 directional nav actions with
no handler at all**, `FocusMode`/`MoveFocus` set in **0** of 164 UI files, `GrabFocus` in 2 files, and
**0** gamepad bindings. Same spirit as the Wave 4 erratum against 17A: check the mechanism before
condemning the symptom.

---

## Wave 5 findings: the 10 highest-impact gaps

| # | Gap | Category | Severity | Why it matters to the player | Smallest action | Deps | Timing |
|---|---|---|---|---|---|---|---|
| 1 | **74 of 147 Core integration seams have no host caller** (incl. `ApplyGrief`, `ApplySedative` — test-only) | technical architecture / testing | **critical** | It's the root cause of four waves of "authored but inert"; features exist, are unit-tested, and never happen in play | declare ports per subsystem + a generated `--check` gate (copy `CombatHostSession.ValidatePorts`) | 35A | **first** |
| 2 | **Trapping yields never become goods** — `hasCatch`/`catchSpecies`/`carcassYield`/`hidePreserved` are recorded (`WildlifeTrappingSystem.cs:326–349,409`) and the host session has **no inventory at all**; the only reader is a panel string (`WildlifeTrappingPanel.cs:102` "CATCH READY") | system connection | **critical** | The hunt produces text, not food — and the player learns this the expensive way | deliver via the bill/sink API (`TryConsumeBill` already exists) | 36A, 22A | before any hunting content (parallel 136) |
| 3 | **Water exists twice** — litres in the plant (`AddWater/GetWater(WaterType)`) vs `clean_water` items (starting supplies add 12); the host bridge is **nullable** (`WaterTreatmentHostSession.cs:16`), and `ConsumeRation` (`:471`) has **0 callers** | system connection | **critical** | Thirst can be solved by a system that doesn't talk to the one that stores water | an ADR + one conversion (`draw`/`pour`), non-nullable dependency | 36A | with 2 |
| 4 | **No producer respects storage or capacity** — `capacity`/`maxWeight` exist on inventory; nothing checks them, and there's no mass-balance assertion anywhere | balance / technical | **important** | Overproduction is free and overflow invisible, so provisioning has no meaning | `IOutputSink` refusal + a 200-day mass-balance test | 35A | during |
| 5 | **Seasons are cosmetic** — `GetSeasonForDay` has one consumer (`WeatherPanel.cs:184`) and only biases weather weights; `weather_seasons.json` has **3** windows; the chapter/year clock exists only inside status strings (`ExpansionHostSession.cs:389–404`, `CenturySeedPanel.cs:173`) | core loop / progression | **critical** | *Nuclear winter* is a mood, not a mechanic: the greenhouse (`0` season/temperature reads), heat, travel, spoilage and illness never notice the year | one `CampaignCalendar` read-model, then a season matrix | 20C, 31 | before next content wave |
| 6 | **One deadline exists and was never generalised** — warlord tribute works (`SettleWarlordTribute(…, out next)`, `TickDaily`, collector lines, raid assignment); ledger debt, treaties, census filings and delivery contracts have no shared obligation mechanism | progression / content | **important** | Without scheduled pressure the game is a series of days with no narrative spine | extract the warlord pattern into `CommitmentSystem` + data | 38A | during |
| 7 | **Keyboard/controller navigation doesn't exist** — 21 actions declared, 4 nav actions with **no handler**, `FocusMode`/`MoveFocus` in **0** UI files, `GrabFocus` in 2, **0** `InputEventJoypad` bindings | UX / production | **important** | A 135-route management game is mouse-only; accessibility and pad users are excluded, and advertised hotkeys mislead | focus policy in the shared UI factory + handlers for the bound actions + an input-map gate | 17B, 25A | before store readiness |
| 8 | **No legibility controls** — font sizes are per-widget overrides (`Theme.FontSizeBody` sprinkled across panels); no text/UI scale setting, no reduce-motion, no captions for the VO produced in 7B | UX / production | **important** | Players who need larger text or calmer motion can't adjust; store pages increasingly expect this | theme-level scale + settings + caption policy | 37A | with 7 |
| 9 | **Existing release probes aren't gates** — `SevenDayDeterministicSmokeSelfTest` is a CLI verb (`HostCli.cs:122,375`) absent from the 46-gate manifest; perf sampling runs **n=5** and is labelled `"advisory"` (`PerformanceSelfTest.cs:47`; median 0.609 s, p95 1.145 s) | testing / production | **important** | The strongest evidence the game holds together is optional, and its statistics are meaningless at n=5 | register as gates, raise n, budget honestly | 26B | immediately (cheap) |
| 10 | **Save durability edge cases untested and unexplained to the player** — atomic writes + `.bak` rotation + slot routing exist and are gated for *shape*, but interrupted writes, disk-full, quit-mid-save (`Main.Application.cs:531`), cross-slot writes, and recovery UX are unproven | technical architecture / UX | **important** | Losing a 200-hour run is the one bug a player never forgives | a durability matrix (corrupt/truncate/kill-mid-write) + a player-facing slot model | 39A | before release |

---

## Plans in this wave

| Plan | Title | Closes | DoD in one line |
|---|---|---|---|
| [35](Plan_35_Goods_Must_Arrive_Production_Provisioning_Chain.md) | Goods Must Arrive | 2, 3, 4 | Every producer delivers or refuses with a reason, and a 200-day mass balance closes. |
| [36](Plan_36_Port_Contract_Unbound_Effects_Fail_CI.md) | The Port Contract | 1 | A Core seam the host forgets to plug in fails the build. |
| [37](Plan_37_Hands_On_The_Wheel_Input_Focus_Controller.md) | Hands on the Wheel | 7, 8 | A full campaign day is completable by keyboard or pad, at a text size the player chose. |
| [38](Plan_38_The_Year_Turns_Seasons_Deadlines_Clock.md) | The Year Turns | 5, 6 | Winter is a different game, and promises have dates the calendar can prove. |
| [39](Plan_39_Session_Durability_Slots_Soak_Release_Gate.md) | Session Durability | 9, 10 | One command can refuse to ship, and no run is ever lost quietly. |

---

## Five waves, one picture

| Wave | Question | Plans | Headline |
|---|---|---|---|
| 1 — Story machine | Does choosing matter? | 15–19 | Ending hardcoded; choices unmakeable; 30 fake consoles |
| 2 — Bunker machine | Does doing matter? | 20–24 | Dose a literal; gear immortal; eating a no-op; power decorative |
| 3 — Ship it intact | Can we build/test/describe it? | 25–29 | 3 red gates; `GameBootstrap` instruction; unbooted artifacts; no coverage |
| 4 — World beyond the gate | Is anything else going on out there? | 30–34 | War never ticked; 20/27 event kinds dropped; 6-node map; tuning applied empty |
| 5 — Human interface | Can a person operate this for 200 hours? | 35–39 | 74/147 seams unplugged; hunting yields vanish; no keyboard nav; no calendar; no soak gate |

**One sentence covering all five:** *the systems exist and the seams don't.* Twenty-five plans,
seventy-five tasks — but the same six verbs recur: bind the port, emit the transition, deliver the
goods, read one authority, gate the claim, and boot the artifact.

**Cross-wave execution order (the three highest-value tasks per wave):** 15A · 19A · 16A → 22A · 24A ·
20A → 29A · 26B · 27A → 31A · 30A · 34B.1 → **36A · 35A · 38A**.

## Metrics to report at wave close

1. Core seams with no host caller: **74 → 0** (each remaining row classified `LIVE_VIA_CORE`)
2. Producers delivering to inventory: **greenhouse only → every row of the 35A audit table**, with a mass-balance assertion
3. Water authorities: **2 → 1** (+ explicit packaging conversion)
4. Season/chapter clock consumers: **1 display → ≥6 mechanical** (crops, heat, travel, spoilage, illness, migration)
5. Obligations with due dates: **1 (warlord tribute) → the full commitments ledger**
6. Input actions declared vs handled: **21 vs ~10 → equal**, 0 gamepad bindings → bound + rebinding UI
7. Panels with focus order: **~0 of 164 → the top 10, then all live routes**
8. Release probes that are gates: **0 → 3** (7-day soak, export boot, perf budget at honest n)
9. Save durability scenarios covered by tests: **shape only → shape + interrupted write + disk full + cross-slot + quit-mid-save + recovery UX**

## Deferred to Wave 6 → **now planned**

**[Continuity Wave 6 — Plans 40–44, *The People In It*](Wave6_Continuity_Audit_INDEX.md)** picked these up and found the same disease in the inner-life layer: 72 authored survivor identity fields with zero consumers (the host invents beliefs instead), a 103-line `ProceduralEulogyEngine` referenced by *nothing*, `DesignateLeader`/`OnCrisisEvent`/`TryMaturation` with no non-test caller, and affinity computed by three systems and read by zero. Read it before running any parallel inner-life content plan (132/144/147/148/150/154/159).

Original candidates:

* **Inner life of the holdfast** — survivor voice, memory, relationships, autonomy, romance/family, ideology and governance; sequence *after* 24A/24B/36A so each new behaviour binds to a port instead of becoming another seam nobody plugs in (parallel plans 132/144/147/148/150/154/159 propose this content).
* **Outposts, vehicles as mobile base, working animals, espionage, black market, shelter renovation** (parallel 151/152/153/155/156/160) — each needs Wave 4's graph/intel and Wave 5's production rails to be more than a new panel.
* **Mod/creator surface** — `ashfall-mod-contract` as a real boundary, content packs on Wave 3's overlay pattern.
* **First-hour funnel at scale** — 20+ seeded playthroughs using Wave 2's event stream and Wave 5's calendar to place the difficulty cliff.
* **Release candidates as an art form** — changelog/version discipline, patch/lane hygiene, save-compatible hotfix path (`ashfall-hotfix-rollback`, `ashfall-release-captain`).
