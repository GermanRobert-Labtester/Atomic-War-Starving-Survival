# Continuity Wave 7 — Audit Index (Plans 45–49): *Content on Rails & the Measurement Layer*

**Snapshot audited:** `ccac926e` (branch `main`, 95 uncommitted paths, **0 git tags**) ·
**Date:** 2026-08-31
**Gates re-run:** `dotnet build Ashfall.csproj` 0/0 · `dotnet test` **5303 passed / 0 failed** ·
`--data-integrity-selftest` **PASS 138 catalogs / 5563 ids / 0 errors** · `triad-drift-gate` PASS ·
`warning-baseline-gate` PASS · Wave 3's three doc gates still red.

Prior waves: [W1 story](Wave1_Continuity_Audit_INDEX.md) · [W2 physics](Wave2_Continuity_Audit_INDEX.md)
· [W3 ship](Wave3_Continuity_Audit_INDEX.md) · [W4 world](Wave4_Continuity_Audit_INDEX.md) ·
[W5 interface](Wave5_Continuity_Audit_INDEX.md) · [W6 people](Wave6_Continuity_Audit_INDEX.md).

Waves 1–6 built the rails. Wave 7 makes the rails **load-bearing**: content must pass an acceptance
ladder, difficulty must be measured and decided, the accidental mod surface must become a written
contract, and a release must be an event with a tag, notes, and a rehearsed rollback path. Then — and
only then — the 452 dead definitions get poured on top.

---

## Wave 7 findings: the 10 highest-impact gaps

| # | Gap | Category | Severity | Why it matters | Smallest action | Deps | Timing |
|---|---|---|---|---|---|---|---|
| 1 | **29 catalogs / 452 authored definitions reach nobody** (atmosphere 152, medical 83, env-texts 36, audio logs 30, encounters 29, journal 28, memorials 27, echoes 23, arc events 15, choice stubs 10 …) while `EFFECT_PRODUCED` is **4 of 411** | content / testing | **critical** | The game's authored voice exists as files, not experience | 45A acceptance ladder, then 45B's sweep | 36A, 27C | **first** |
| 2 | **Root-array catalogs are counted as 0 definitions** — `cassette_sets`, `guilt_sources` (20), `confession_secrets` (8), `final_wishes` (8), `damaged_map_zones` (3), `*_survivor_fields` | technical architecture | **important** | Invisible to the utilisation gate *and* the `schema_version` presence rule ("bare-array root exempt"), so they can never fail or succeed | count them (one fix, two gates) | 45A | before 49 |
| 3 | **Exemptions never expire** — `ContentExemption.cs` has one `ExpiryCondition` (`exempt_echoes_future`) and no enforcement; 26 catalogs sit in `exempt_no_source_evidence` | production / content | **important** | "Deferred" becomes "forever", which is how 452 defs accumulated | expiry check in the gate: owner + rationale + date or fail | 45A | with 2 |
| 4 | **27 balance CSVs with no producer, no doc, no decisions** — `artifacts/balance/*.csv` (per-day needs/dose columns); `grep -rl "artifacts/balance" docs/ scripts/` → **0** | testing / production | **critical** | Every Waves-1–6 rebalance will be judged against numbers nobody can regenerate or attribute | check in the sweep harness + scenarios as data + `docs/balance/` | 46A | immediately |
| 5 | **No difficulty targets exist** — no `docs/balance*`; no stated survival-rate / time-to-first-death expectation anywhere, while difficulty itself only arrives with 34B | balance / design | **important** | Tuning is opinion; players inherit the opinion | write the targets, gate the drift | 34B, 46A | with 4 |
| 6 | **Zero player telemetry** — every "telemetry" hit in code is diegetic fiction (`Orbital`, `SumpFlooding`, `StatusPanel` cohort lines); the one action-instrumentation seed (`ObserveSigil("inventory.used")`, `Main.Inventory.cs:90`) is isolated | UX / production | **important** | The first hour — guidance, ration shock, dispatch confusion — is unobservable, and Waves 1–6 just rewired all of it | local, private, opt-in `PlaySessionRecorder` + synthetic players | 31C, 17B | during |
| 7 | **An accidental mod surface with no contract** — `ASHFALL_DATA` is resolver precedence #1, `schema_version` is on 411 files, `CatalogIntegrityValidator` can arbitrate packs, tags + overlays are extension-ready… and no document says what a pack may rely on | production / technical architecture | **important** | Every data-schema change is silently a mod break; a promise made in Discord can't be taken back | generated contract + fixture-pack CI + breaking-diff detection | 45A, 26A, 40B, 25C | before any public modding statement |
| 8 | **Release practice is absent** — **0 git tags**, `config/version="1.0.0"` with a `"unknown"` CLI fallback, no `CHANGELOG`, no versioning policy across game/data/save axes — and `AGENTS.md` still mandates `bit lane create / bit snap / bit export` for a git+Godot repo | production | **critical** | There is no known-good point to diff, roll back to, or promise compatibility from | version policy + generated changelog + first tag + `prepare-release.sh --check`; rewrite the stale VCS section | 29A, 39A | immediately (cheap) |
| 9 | **No save-compatibility window or hotfix rehearsal** — codecs migrate V1→V2→V3, but no policy states what a release must read; no fixture saves per historical version; no rollback runbook | technical architecture / production | **important** | The unforgivable bug is "your 200-hour campaign won't load"; it is currently untested by design | fixture saves + compatibility gate + rehearsed `HOTFIX.md` | 48A | before release |
| 10 | **More content is queued while content is dead** — the parallel waves (136–160) propose new entries into catalogs whose consumer status is exactly gap #1 | content / production | **important** | Authoring now inflates the bucket 45B is about to empty | sequence: rails (45A) → sweep (45B) → depth passes (49) → *then* expansion waves | 45A | gate the queue |

---

## Plans in this wave

| Plan | Title | Closes | DoD in one line |
|---|---|---|---|
| [45](Plan_45_Content_Acceptance_Pipeline.md) | The Content Acceptance Pipeline | 1, 2, 3, 10 | New dead content is a build failure; the 452 are wired, archived, or deleted with a reason. |
| [46](Plan_46_Playable_Metrics_Balance_Decisions_Player_Telemetry.md) | Playable Metrics | 4, 5, 6 | Every balance number is reproducible and attributed; a release candidate ships with a first-hour funnel report. |
| [47](Plan_47_Mod_Content_Pack_Contract.md) | The Mod & Content-Pack Contract | 7 | A pack can be installed, disabled, and validated — and breaking data changes fail CI. |
| [48](Plan_48_Release_Craft_Versioning_Changelog_Hotfix.md) | Release Craft | 8, 9 | One command cuts a release; a hotfix path is rehearsed before it's needed. |
| [49](Plan_49_Depth_Passes_Dead_Content_Onto_The_Rails.md) | Depth Passes | 1, 10 | 188 atmosphere + 83 medical + 85 memory + 64 encounter definitions become reachable, measured content. |

---

## Seven waves, one argument

| Wave | Question | Plans | Root finding |
|---|---|---|---|
| 1 — Story machine | Does choosing matter? | 15–19 | Ending hardcoded; choices unmakeable; 30 fake consoles |
| 2 — Bunker machine | Does doing matter? | 20–24 | Dose a literal; gear immortal; eating a no-op; power decorative |
| 3 — Ship it intact | Can we build/test/describe it? | 25–29 | 3 red gates; instructions citing a dead class; unbooted artifacts |
| 4 — World beyond the gate | Is anything else going on out there? | 30–34 | The war never ticked; 20/27 event kinds dropped; 6-node map |
| 5 — Human interface | Can a person operate this for 200 hours? | 35–39 | 74/147 seams unplugged; hunting yields vanish; no keyboard nav |
| 6 — The people in it | Is anybody *in* it? | 40–44 | Personality inferred not authored; eulogy engine unreferenced; affinity read by nobody |
| 7 — Rails & measurement | Will it stay fixed? | 45–49 | Content unaccepted, balance unattributed, releases untagged, players unmeasured |

**The complete sentence for the project:** *everything exists and nothing is connected — and nothing
was checking.* Thirty-five plans, 105 tasks, all reducible to six verbs:
**bind the port, emit the transition, deliver the goods, read one authority, gate the claim, tag the
release.**

**Highest-value tasks across all seven waves**, if only a handful ever run:
**19A · 22A · 24A · 29A · 31A · 34B.1 · 36A · 40A · 44A · 45A · 48A.**
Note what that list is: two content/loop fixes, one honesty fix, one legibility fix, one wiring fix,
one identity fix, one measurement fix, one release fix — and not a single new feature.

## Metrics to report at wave close

1. Catalogs with zero consumers (non-narrative): **29 / 452 defs → 0** wired-or-archived
2. `EFFECT_PRODUCED` catalogs: **4 → target published in 45A**, tracked per release
3. `exempt_no_source_evidence`: **26 / 429 defs → 0**, with expiry enforcement live on all exemptions
4. Reproducible balance corpus: **0 of 27 CSVs attributed → all regenerated from checked-in scenarios**
5. Difficulty targets documented: **none → `docs/balance/TARGETS.md`, drift-gated**
6. First-hour funnel report on a release candidate: **impossible → mandatory (synthetic players)**
7. Mod contract published + fixture packs in CI: **0 → generated contract, 5 fixture packs, breaking-diff detection**
8. Git tags / changelog: **0 / none → ≥1 tag, generated `CHANGELOG.md`, `prepare-release.sh --check`**
9. Save-compat coverage: **shape tests → fixture saves per supported version, checksum-stable**
10. Moral-choice quests reachable in play: **0 of 215 → measured share, reported**

## Deferred to Wave 8 → **now planned**

**[Continuity Wave 8 — Plans 50–54, *The Presented Game*](Wave8_Continuity_Audit_INDEX.md)** picked
these up — and found the aesthetic/backlog layer is in the same state the simulation was in Wave 1:
the asset gate checks **50 of 5,563 ids** and treats a fallback as a pass; **1,189 art files** are
referenced by nothing; there are **0 shaders, 0 tweens, 4 orphan scenes**, a 193-line map view with
zero references; surface ambience never starts in play and no ducking exists; **115 plan files** sit
in one folder with no status or premise check; and nothing in CI has ever been tested by a human
(the 7-day deterministic harness isn't even a gate).

Original candidates:

* **The parallel expansion waves, finally unblocked** — 136–160 as data PRs on 45A's ladder with 49's acceptance evidence (hunting/cooking, research unlocks, clothing, endings, companions, vehicles, outposts, espionage, black market, renovation).
* **Aesthetic completion** — the art/audio/lighting pass (`ashfall-design`, `ashfall-foundry`, 7C ambience state machine), now that panels are honest (16A), text is keyed (25), and inputs are known (37); a visual overhaul before the rails is how you produce 30 more fake consoles.
* **Ambition audit** — one document answering, per planned feature, whether the game still wants it; seven waves show the failure mode is breadth without connection, and the backlog (130+ 196 parallel plans) is now the risk.
* **Store certification** — accessibility conformance, controller certification, localization completion %, achievements/metrics parity, and the privacy stance from 46B written for a listing page.
* **The second holdfast** — only after 34C (legacy), 38C (deadlines), 41C (generations) and 43C (consent) exist: the outpost/colony idea becomes a continuation rather than a restart.
