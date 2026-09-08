# Continuity Wave 8 — Audit Index (Plans 50–54): *The Presented Game*

**Snapshot audited:** `ccac926e` (branch `main`, 95 uncommitted paths, 0 git tags) · **Date:** 2026-08-31
**Gates I ran this wave:** `dotnet build Ashfall.csproj` 0/0 · `dotnet test` **5303 passed / 0 failed** ·
`--data-integrity-selftest` **PASS 138 catalogs / 5563 ids** · `--asset-registry-selftest` **PASS,
`checked=50 passed=50 missing=0 (unique=6, duplicate_fallback_requests=3)`** · `triad-drift-gate`,
`doc-link-gate`, `warning-baseline-gate` PASS · Wave 3's three doc gates still red.

Prior waves: [W1 story](Wave1_Continuity_Audit_INDEX.md) · [W2 physics](Wave2_Continuity_Audit_INDEX.md)
· [W3 ship](Wave3_Continuity_Audit_INDEX.md) · [W4 world](Wave4_Continuity_Audit_INDEX.md) ·
[W5 interface](Wave5_Continuity_Audit_INDEX.md) · [W6 people](Wave6_Continuity_Audit_INDEX.md) ·
[W7 rails](Wave7_Continuity_Audit_INDEX.md).

Waves 1–7 made the simulation connected, operable, and measurable. Wave 8 asks whether any of it is
**seen and heard** — and whether the project can stop growing in directions it can't finish.

---

## Wave 8 findings: the 10 highest-impact presentation & governance gaps

| # | Gap | Category | Severity | Why it matters | Smallest action | Deps | Timing |
|---|---|---|---|---|---|---|---|
| 1 | **The asset gate checks 50 of 5,563 ids (0.9 %) and passes** — `IsValid => Texture != null && (Loaded \|\| FallbackUsed)`, so it prints `missing=0` *while reporting* `unique missing assets: 6, duplicate_fallback_requests: 3` | testing / production | **critical** | "All assets resolve" is unmeasured optimism — the same presence-vs-liveness bug as every prior wave, now applied to art | make `FallbackUsed` fail a strict tier; measure real coverage per family | 45A | **first** |
| 2 | **1,189 art files and 148/217 icons are referenced by nothing** (conservative substring test → lower bound), across a 114 MB tree (`assets/art` 74 MB, `assets/ui` 32 MB, `assets/sprites` 8.3 MB, `assets/audio` 10 MB) | production | **important** | Clone weight, import cache, and a false sense of art completeness | reconcile: wire (a), archive (c/d), delete (e) with a receipt | 50A | with 1 |
| 3 | **No id→asset mapping** — resolution is stem convention (`ResolveStemCandidates` × `PortraitSearchPaths`); portraits are display-named (`elena_vasquez.png`, 105 files) while the resolver queries `survivor_*`/`npc_*`; `assets/sprites/Characters/` holds **1 file, the placeholder** | technical architecture | **critical** | Whether a portrait appears is luck; art work can't be targeted because nobody knows the gap list | `asset_registry.json` manifest as authority; convention demoted to fallback | 50A, 47B | first |
| 4 | **`AGENTS.md` still describes a ~2,080-file Unity asset tree** — `Assets/art`, `Assets/sprites`, `Assets/ui`, `Assets/audio` now contain **1 file each (0 bytes)** | technical architecture | **later** (but cheap) | Agents keep "finishing" a migration that's done and skip real gaps (this is the **eighth** disproved doc claim this audit series has logged) | correct the row, regenerate the 12 rulebooks | 29A | with 3 |
| 5 | **There is no presented world**: 0 `.gdshader` files, **0 `CreateTween` calls**, 0 `TileMap` nodes, and all four scenes are orphans (`scenes/Main.tscn` = 1 node; `HoldfastInterior.tscn` = 5 nodes with `texture = null` and empty containers; nothing loads them). `src/World/WastelandMapView.cs` (193 lines) has **0 references**; `MapLocationMarkerView.cs` (150) is referenced only by that dead view; the 430-line interior view renders **inside a UI panel** | UX / technical architecture | **critical** | The game shows spreadsheets while simulating a bunker and a wasteland; the work that would make it feel like a game is written and unwired | mount the interior properly; wire or delete the map views | 50A, 32A, 23A, 20A | during |
| 6 | **Sound is half-wired**: surface ambience is never started in play (only `StartBunkerAmbience` at `Main.GameFlow.cs:98`); **no ducking or bus-volume code exists anywhere in `src/Audio/`**, so the documented 3–4-alert pile-up stands; 8 of 22 weather kinds unmapped; `rad_geiger_loop` still can't stop (no Core exposure-end event); 3 music cues total; and `Silence`/`SilentSpring`/`FalseSpring` are *authored weather states with no sound* | UX / balance | **important** | A nuclear-winter game that is silent at the moments it wrote silence into the fiction | one ambience state machine (extend `ShelterAudioController`) → ducking/priority → 3–5 musical pieces | 20C, 23A, 38A, 17C | during |
| 7 | **The backlog is ungoverned**: **115** plan files in `Next-steps-plans/` (38 continuity + **77** in the 1xx expansion series), **132** in `piagentsplans/`, **119** docs; three numbering schemes; no `STATUS`/`PREMISE_VERIFIED_AT` anywhere; no design-pillar document at all (`ls docs \| grep -iE "design\|pillar\|vision"` → none) | production / technical architecture | **critical** | Seven waves prove the failure mode is breadth without connection; 77 new-system plans with no intake reproduces it at scale | generate a plan register + write the pillars + an intake gate | 29A, 29C, 45A | immediately |
| 8 | **Human testing has been replaced by machine checks**: every row of `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md` reads `None (PASS)` citing a selftest; the repo owns a deterministic 7-day smoke harness (`7day_smoke_selftest`) that **is not a gate**; there is no demo preset and no playtest protocol or session record | testing / production | **critical** | Nothing in the project knows whether a new player can survive day 3 | cut a frozen 7-day slice + demo build; run 6–10 consented sessions | 46A/46B, 17B, 34B, 26B | **the wave's last word** |
| 9 | **Visual QA asserts loading, not seeing**: 30 snapshots vs 135 routes (post-16A: ~30 live), no populated-state fixtures, no placeholder-absence assertion, no scale/contrast variants | testing | **important** | A screen can be pixel-clean and visually empty | snapshot the live set with populated fixtures + a no-undeclared-placeholder gate | 15C/16A, 50A, 27A | after 1 |
| 10 | **No intake rule prevents the known failure**: a panel can be routed without an authority (Wave 1's 30 fake consoles), content authored before rails exist (Wave 7's 452 defs), or a system added beside a live capability (the 131/147/159 duplicates) | production | **important** | Each of those mistakes cost a wave to find | an intake form + `plan-intake-check.sh` refusing a panel without authority/rails | 53A/53B | after 7 |

---

## Plans in this wave

| Plan | Title | Closes | DoD in one line |
|---|---|---|---|
| [50](Plan_50_Asset_Truth_What_Actually_Renders.md) | Asset Truth | 1, 2, 3, 4, 9 | Every rendered pixel traces to a declared mapping, coverage is a published number, and 1,189 orphans are reclaimed or gone. |
| [51](Plan_51_The_Presented_Game_Interior_Map_Light_Motion.md) | The Presented Game | 5 | The shelter is a room you look at, the map is a place you route through, and ≤6 shaders each prove a mechanic. |
| [52](Plan_52_Sound_of_Scarcity_Ambience_Music_Silence.md) | The Sound of Scarcity | 6 | Ambience follows your situation, the mix stays legible under a storm, and the quiet moments are authored. |
| [53](Plan_53_Ambition_Audit_Expansion_Intake.md) | Ambition Audit & Intake | 7, 10 | One roadmap, one not-now list with reasons, and a gate that refuses a system whose rails don't exist. |
| [54](Plan_54_Seven_Day_Slice_Playtest_Instrument.md) | The Seven-Day Slice | 8, 9 | A stranger can finish day 7; the first week has a scorecard; releases are refused when it regresses. |

---

## Eight waves, one sentence

| Wave | Question | Plans | Root finding |
|---|---|---|---|
| 1 — Story machine | Does choosing matter? | 15–19 | Ending hardcoded; choices unmakeable |
| 2 — Bunker machine | Does doing matter? | 20–24 | Dose a literal; gear immortal; eating a no-op |
| 3 — Ship it intact | Can we build/test/describe it? | 25–29 | Red gates; instructions citing a dead class |
| 4 — World beyond the gate | Is anything else going on? | 30–34 | The war never ticked; 20/27 event kinds dropped |
| 5 — Human interface | Can a person run it for 200 hours? | 35–39 | 74/147 seams unplugged; no keyboard nav |
| 6 — The people in it | Is anybody in it? | 40–44 | Personality inferred, not authored; affinity read by nobody |
| 7 — Rails & measurement | Will it stay fixed? | 45–49 | 452 defs unreachable; balance unattributed; 0 tags |
| 8 — The presented game | Can anyone perceive it — and should we build more? | 50–54 | Asset gate checks 0.9 %; no shaders/motion/ambience; 77 ungoverned plans; no human ever tested it |

**Forty plans, 120 tasks, and the finding never changed:** *the systems exist and the seams don't —
and almost nothing measured whether a person could perceive or understand them.*

**Highest-value tasks across all eight waves:**
**19A · 22A · 24A · 29A · 31A · 34B.1 · 36A · 40A · 44A · 45A · 48A · 50A · 53B · 54A.**
Thirteen tasks in eight waves — of which exactly **zero** are new features. Two of them (*53B* write
the pillars, *54A* let a stranger finish week one) exist specifically to stop the project from
needing more.

## Metrics to report at wave close

1. Asset gate coverage: **50 of 5,563 ids → 100 % of ids in shipped catalogs**, with fallbacks failing the strict tier
2. Unreferenced art files: **1,189+148 → 0** (wired, archived, or deleted with receipts)
3. Portraits/icons resolving by manifest rather than stem luck: **0 → all rendered families mapped**
4. Motion/shader/tile layer: **0 shaders, 0 tweens, 4 orphan scenes → ≤6 shaders, a shared motion vocabulary, 0 orphan scenes**
5. Ambience coverage: surface ambience started in play **no → yes**; weather kinds mapped **14/22 → 22/22**; ducking rules **absent → enforced and measurable**
6. Backlog governance: **247 plan documents, 0 statuses → 100 % registered, statused, premise-dated**, intake gate live with proven rejection
7. Design pillars: **absent → `docs/design/PILLARS.md`, cited by every roadmap decision**
8. First-week instrument: **0 human sessions, 1 ungated 7-day harness → slice gated, demo bootable, ≥6 consented sessions per release, scorecard published**
9. Stale doc claims corrected: **8 → 0**, with the claims-gate live
10. Snapshot set: **30 layout-only → the live panel set, populated fixtures, no undeclared placeholder**

## Deferred to Wave 9 → **now planned**

**[Continuity Wave 9 — Plans 55–59, *Weight, Durability & the Shop Window*](Wave9_Continuity_Audit_INDEX.md)**
picked these up and added the last class of finding: **no retention policy exists anywhere** while
long-campaign play became real; **7 MB of genuine save history sits uncommitted on one disk**;
the working copy carries **1.34 GB** of tool weight with 122 design mockups inside `assets/`;
there is **no store-facing material at all** and `AI_DISCLOSURE.md` is still a placeholder template;
and the 22 finding classes from nine waves were each avoidable by a gate that didn't exist **or was
never run** — which is why Wave 9 closes with a retrospective instead of a tenth plan series.

Original candidates:

* **Store & launch craft** — wishlist/press surface, capsule/scroll art from 50's manifest, accessibility conformance statement, demo-freeze cadence, patch-note rhythm from 48.
* **The second holdfast** — outposts/colonies (parallel 160) and meta-campaign legacy (34C) as a *continuation* feature, now that generations (41C), consent (43C) and the calendar (38) exist.
* **Deep content waves under intake** — the merged `LINK` tickets from 53B step 6 as data PRs on 45A's ladder, each with a reachability number from 54C's scorecard.
* **Long-haul durability** — 400-year soak, save-format deprecation windows, migration corpus growth (39B/48A), and a real telemetry-informed difficulty review cadence.
* **A retrospective** — what eight waves of audits should teach the process itself: which finding classes were avoidable with the gates now in place, and how the plan layer would have looked if intake had existed first.
