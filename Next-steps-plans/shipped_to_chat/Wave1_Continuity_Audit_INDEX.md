# Continuity Wave 1 — Audit Index (Plans 15–19)

**Snapshot audited:** `ccac926e` (branch `main`, 95 uncommitted paths) · **Date:** 2026-08-31
**Baseline gates I ran myself:** `dotnet build Ashfall.csproj` → 0 errors / 0 warnings ·
`dotnet test Ashfall.Core.Tests` → **5303 passed, 0 failed** ·
`godot --headless --path . -- --data-integrity-selftest` → **PASS, 138 catalogs, 5563 ids, 0 errors**.

The game is *not* short of systems, content, or polish. It is short of **connections**. Every gap
below is a missing link between things that already exist.

---

## The 10 highest-impact continuity gaps

| # | Gap | Category | Severity | Closes in |
|---|---|---|---|---|
| 1 | **The ending does not read the campaign** — epilogue bound to `0, true, true, true, true, true` (`src/Main.GameFlow.cs:444`, `src/Main.PlayerSurfaces.cs:246`); no gameplay path ever derives `EpilogueEvaluationContext` | core loop | **critical** | 19A |
| 2 | **Moral choices cannot be made** — `TryResolveMoralChoice` (`src/Main.MoralChoice.cs:91`) has **0 call sites**; 215 authored moral-choice quests sit at "queried, no effect" | core loop / system connection | **critical** | 15A, 15B |
| 3 | **30 player-routable consoles are false affordances** — 30 files with `IsBound … = true`; buttons that only print text (`AnaerobicBiogasDigesterPanel.cs:88–100`) | UX / system connection | **critical** | 16A |
| 4 | **Routed panels bound to throwaway systems** — `new ShelterFireHazardSystem()` / `new FactionStanceEngine()` ×2 / `new SkillProgressionSystem()` / `new WeatherHostSession(...)` at bind time, plus literal fixture ids (`"inc_default"`, `"tag_1"`, `"sig_distress"`, `"sv_cohort_demo"`) | system connection | **critical** | 16B |
| 5 | **Authored content is causally inert** — 411 catalogs / 4,808 definitions: 272 codex-only, 300 with zero consumers (2,067 defs), **only 4 catalogs reach `EFFECT_PRODUCED`**; 26 catalogs / 429 defs sit in `exempt_no_source_evidence` | content / progression | **critical** | 18A, 18B |
| 6 | **Guidance is unreachable** — `OnboardingHintPanel` is constructed and persisted but no route or key can open it (no `Visible`/open/show hit anywhere in `src/`) | UX | **critical** (playability) | 17B |
| 7 | **The briefing reports levels, not consequences** — the typed `DayStateChangeEvent` channel exists but exactly **one** producer (`SurvivorFateSystem`), so 18 of 19 day-advance owners fall through to a hand-rolled "X is hungry (73%)" readout | UX / system connection | **important** | 17A |
| 8 | **Reopened panels drift** — 4 panels unsubscribe a *freshly allocated* lambda (`TriangulationPanel.cs:44/:52/:187`), so handlers accumulate and survive session swaps | technical / testing | **important** | 16C |
| 9 | **Feedback layer half-connected** — 6 of 20 silence gaps still partial (UI cues across 164 UI files, ambience/music loops, pickup, danger); alert stacking undecided; `rad_geiger_loop` blocked on a missing Core exposure-end event | UX / balance | **important** | 17C |
| 10 | **Dead knobs and drifting truth** — `regional_supply` has 0 C# consumers; 15 dangling `.cs.uid` sidecars (incl. a lost `EpilogueContextFactory`); four audit docs each contain a now-false "known issue" claim; `.claude/worktrees/` (143 MB) excluded only locally | technical architecture / production | **important** (root cause) | 18C, 19C |

**Root cause that ties 1–5 and 10 together:** every existing gate measures *presence*
(route registered, loader named, descriptor exists) and none measures *liveness*
(action mutates the campaign authority). `--content-utilization-selftest` reports
`Actionable Priorities: 0,0,0,0,0` while 4 of 411 catalogs produce effects.

---

## Plans in this wave

| Plan | Title | Closes | DoD in one line |
|---|---|---|---|
| [15](Plan_15_Moral_Choice_Decision_Spine.md) | The Decision Spine | 2, 5(partial), 10 | A player can commit a moral choice, it moves four other systems, and a new dead panel can no longer ship. |
| [16](Plan_16_Honest_Navigation_Console_Triage.md) | Honest Navigation | 3, 4, 8 | Openable panels == acting panels; no panel binds a throwaway system; reopening a panel cannot drift. |
| [17](Plan_17_Legibility_Cause_Effect_Guidance.md) | Legibility | 6, 7, 9 | The day explains what you caused, guidance is always one key away, ordinary acts are audible once each. |
| [18](Plan_18_Living_Content_Codex_To_Consequence.md) | Living Content | 5, 10 | One content family becomes playable end to end, the no-evidence bucket is retired, and authored fields must be load-bearing. |
| [19](Plan_19_Ending_Continuity_Derived_Campaign.md) | Ending Continuity | 1, plus years/session/repo truth | Three play styles produce three endings that name what actually happened. |

Predecessor (unrelated, still open): [Plan 14](Plan_14_Economy_Weather_Shelter_Loop.md) — note
its premise that the weather forecast "is never surfaced" is **stale**
(`src/UI/WeatherForecastPanel.cs` exists and is routed); re-verify before executing it.

**Successor:** [Continuity Wave 2 — Plans 20–24, *The Bunker Machine*](Wave2_Continuity_Audit_INDEX.md)
— the same audit applied to physical causality (dose, gear wear, food, power, labour). Wave 2's
17A prerequisite means these two waves interleave rather than run in sequence.

---

## If capacity allows only three tasks

**15A → 19A → 16A.** One playable choice, one ending that remembers you, one menu that stops
lying. That is the difference between "a lot of systems" and "a game".

## Metrics to report at wave close

1. `EFFECT_PRODUCED` catalogs: **4 → ?**
2. `exempt_no_source_evidence` catalogs / defs: **26 / 429 → 0**
3. Player-routable panels vs live-bound panels: **135 → N = N**
4. Moral choices resolvable in-session: **0 → 215 authored**
5. Distinct epilogue outcomes reachable from real state: **1 → up to 32**
6. Fresh-system binds in `src/Main.PlayerSurfaces.cs`: **5 → 0**
7. Dangling `.cs.uid` sidecars: **15 → 0**, gated

---

## Numbering / co-existence note

`Next-steps-plans/` also contains **Wave 14 (Plans 131–135)** — `Plan_131_Wasteland_Information_Rumor_Network`,
`Plan_132_Survivor_Hidden_Agendas_Betrayal_Arc`, `Plan_133_Expedition_Discovery_Persistent_World_Consequences`,
`Plan_134_Dynamic_Faction_Territory_Supply_Lines`, `Plan_135_Weather_Deep_Gameplay_Cascade`,
plus `Wave_14_Summary.md`. That wave was authored in parallel with this one and proposes **new
systems**; this wave closes **missing links** between systems that already exist.

Both are valid, but they are not independent: Wave 14 would add Rumor, Betrayal, Territory,
Supply-Line and Weather-cascade authorities into the same tree where 30 routed panels act on
nothing, 18 of 19 day-advance owners emit no events, and the epilogue inputs are literal
constants. **Execute Wave 1 (15A, 19A, 16A) first**, or Wave 14's new systems will inherit these
gaps on arrival.

Numbering is deliberately non-overlapping (`15–19` here, `131–135` there, `14` pre-existing), but
if a future renumber is wanted, keep this wave's plans contiguous below 100 so "1xx" stays reserved
for expansion waves. One correction to Wave 14's own evidence, offered in the same spirit: its
claim that no information-flow capability exists understates what is already built — the radio
tuner, `TriangulationSystem`, `WeatherIntelligenceCoordinator`, trader "tells", and
`MoralChoiceGossipRuntime` are all live pieces of an information economy already mapped in
`docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` §12. Link those before adding a sixth authority.
