# Continuity Wave 6 — Audit Index (Plans 40–44): *The People In It*

**Snapshot audited:** `ccac926e` (branch `main`, 95 uncommitted paths) · **Date:** 2026-08-31
**Gates re-run:** `dotnet build Ashfall.csproj` 0/0 · `dotnet test` **5303 passed / 0 failed** ·
`--data-integrity-selftest` **PASS 138 catalogs / 5563 ids / 0 errors** · `triad-drift-gate` PASS ·
`warning-baseline-gate` PASS · Wave 3's three doc gates still red.

Prior waves: [W1 story](Wave1_Continuity_Audit_INDEX.md) · [W2 physics](Wave2_Continuity_Audit_INDEX.md)
· [W3 ship](Wave3_Continuity_Audit_INDEX.md) · [W4 world](Wave4_Continuity_Audit_INDEX.md) ·
[W5 interface](Wave5_Continuity_Audit_INDEX.md).

Waves 1–5 made the game *connected*. Wave 6 asks the last question a survival-management game has
to answer: **are the people in it anybody?**

---

## The headline numbers this wave

| Measurement | Value |
|---|---|
| Survivor identity fields authored in the enrichment layer, with **zero consumers** | **72** belief profiles / keepsakes / phantom backgrounds / professions (`expansion_survivor_fields.json`) + 11 + 4 + **67** item tags |
| The Core catalog class written to query them (`ExpansionEnrichmentCatalog`) | referenced by **nothing** (`src/`, Core, tests: 0) |
| What the host does instead | `InferBeliefProfile(def)` — a "best-effort mapping" heuristic (`src/Main.SurvivorSocial.cs:36,43,60`) that **invents** belief from traits |
| Core classes for the game's most emotional beats, with **no reference anywhere** | `Journal/ProceduralEulogyEngine.cs` (103 lines) — 0 refs in `src/`, Core, or tests |
| …and with test-only refs | `Narrative/DwellerHeirloomCatalog.cs` (30 keepsakes/heirlooms) · `ApplyGrief` (Wave 5) · `DwellerHeirloomCatalogTests` |
| `LeadershipSystem` — designation, stress, break risk, deaths witnessed, crisis | `DesignateLeader` and `OnCrisisEvent`: **0 non-test callers**; the only "Leader" strings in `src/` are faction labels, one of which is hardcoded: `TradeScreenGodotPanel.cs:164 → Text = "Leader: Varek (gen 1)"` |
| Affinity: computed by 3 systems, displayed in 1 panel, **read by 0 consumers** | `IdeologicalFrictionSystem.cs:30–31` daily drift, `TraumaBondSystem.cs:46 BondAffinityBonus = 15f`, coordinator→`SurvivorRelationsPanel` — and no duty, party, care, or production reader anywhere |
| Children growing up | `CohortSystem.TryMaturation(childId, day)`: **0 non-test `src/` callers** |
| Dead narrative catalogs this wave revives | `wall_carving_templates.json` (3), `confession_secrets.json` (8, `exempt_no_source_evidence`), `echoes.json` (23, `exempt_echoes_future`), `phantom_background` inputs |

**The shape of it:** the inner-life layer is not missing — it is *five islands with a panel each*.
Identity (authored, unread), memory (built, unwired), voice (nothing at all), consent (modelled,
invisible), and outcomes (no consumer). Each island is individually healthy; none of them meet. That
is the same diagnosis as Waves 1–5, now applied to the part of the game players actually remember.

---

## Wave 6 findings: the 10 highest-impact inner-life gaps

| # | Gap | Category | Severity | Why it matters to the player | Smallest action | Deps | Timing |
|---|---|---|---|---|---|---|---|
| 1 | **Personality is inferred, not authored** — `InferBeliefProfile` guesses beliefs while `expansion_survivor_fields.json` (72) + `ExpansionEnrichmentCatalog` go unread | content / system connection | **critical** | The friction, bonds and grievances the game simulates run on invented data, so people feel interchangeable | load the catalog, call `RegisterBelief` from data, delete the heuristic | 36A | **first** |
| 2 | **`ProceduralEulogyEngine` is referenced by nothing** (0 refs anywhere) | content / UX | **critical** | When someone dies, the one system written to say something about them never runs | instantiate it in the death pipeline | 40A, 24C | with 1 |
| 3 | **`ApplyGrief` / heirlooms are test-only** — `IGriefSink`/`DeathQuality` landed at `b48b4494`; `DwellerHeirloomCatalog` has no host consumer | system connection | **critical** | Grief and keepsakes are unit-proven and gameplay-absent; nothing is inherited or missed | bind grief to relations/morale; distribute keepsakes on death | 41A | with 2 |
| 4 | **Survivors never speak** — no line/voice/bark system for people (the only bark mechanism is for roster *marks*, `MoraleMarkSystem.cs:90`), while 118 radio broadcasts give voice to nobody present | UX / content | **important** | The cast is silent exactly when the game asks the player to care about them | a keyed, data-driven line bank + delivery into existing surfaces | 40A, 31, 25A | during |
| 5 | **Leadership is invisible and unchoosable** — `DesignateLeader`/`OnCrisisEvent` never called; stress/break-risk unobserved | core loop / progression | **critical** | A whole authority dimension of the shelter exists on paper | wire designation into the roster surface + crisis events + the existing morale channel | 40A, 24B | during |
| 6 | **Affinity changes nothing** — no consumer outside the systems that write it | system connection / balance | **critical** | Two people who hate each other and two who would die for each other are mechanically identical | one `Relations.EffectOf(a,b)` query consumed by duty, party, care, training | 44A, 24A | during |
| 7 | **Policy is two toggles** — curfew + emergency override (real, but display-adjacent effects only) and ration triage; no shared mechanism, no grievance feedback loop | progression | **important** | Governing the shelter isn't a game layer, though every ingredient exists | generalise the ration-triage pattern into an authored policy catalogue | 43A, 24B | after 5 |
| 8 | **Consent has no ladder** — no refusal, desertion, challenge, or arbitration path from accumulated grievance (grep `mutiny\|election\|vote\|council` → only unrelated "selection" hits) | progression / content | **important** | The crew cannot say no, so their suffering has no consequence — the genre's central tension | an authored escalation ladder with off-ramps, *gated on* 43A/43B existing | 43B | **may defer legitimately** |
| 9 | **Children never grow up** — `TryMaturation` uncalled; succession chapters are cosmetic strings (W4 #5) | progression | **important** | A multi-year game has no generational turnover despite `BookChild`, age bands, and a succession engine | call maturation from the calendar; age classes into rations/duty/dose | 38A, 22B, 24A | during |
| 10 | **Identity duplication and string-shaped coupling** — `profession` exists in `survivors.json` *and* the enrichment file; keepsakes are recognised by parsing `item_personal_keepsake_*` (`ShelterDecorSystem.cs:231–262`); `CraftingSystem.cs:336–337` hardcodes item id lists that `expansion_item_tags.json` (67) was written to replace | technical architecture | **important** | Two sources of a fact means one of them is a lie waiting to happen | resolve the fork, load tags, delete the id lists, add integrity tiers | 40A/40B | with 1 |

---

## Plans in this wave

| Plan | Title | Closes | DoD in one line |
|---|---|---|---|
| [40](Plan_40_Authored_Personality_Not_Inferred.md) | Authored Personality, Not Inferred | 1, 10 | Belief/keepsake/profession/tags come from data; the heuristic is deleted; behaviour keys off tags. |
| [41](Plan_41_Memory_That_Acts_Heirlooms_Eulogies_Generations.md) | Memory That Acts | 2, 3, 9 | A death changes the shelter six ways; places keep receipts; children mature. |
| [42](Plan_42_A_Voice_For_Each_Of_Them.md) | A Voice for Each of Them | 4 | Survivors speak in their own register, and it's in the journal afterwards. |
| [43](Plan_43_Governing_Together_Leadership_Policy_Consent.md) | Governing Together | 5, 7, 8 | The player appoints a leader, adopts policies with audible objections, and the crew can refuse — or the gate document says why it waits. |
| [44](Plan_44_Relations_That_Change_Outcomes.md) | Relations That Change Outcomes | 6 (+ wave closure) | Pair state changes shift tables and party lists, with a traceable reason for every change. |

---

## Six waves, one argument

| Wave | Question | Plans | Root finding |
|---|---|---|---|
| 1 — Story machine | Does choosing matter? | 15–19 | Ending hardcoded; choices unmakeable; 30 fake consoles |
| 2 — Bunker machine | Does doing matter? | 20–24 | Dose a literal; gear immortal; eating a no-op; power decorative |
| 3 — Ship it intact | Can we build/test/describe it? | 25–29 | 3 red gates; instructions citing a dead class; unbooted artifacts |
| 4 — World beyond the gate | Is anything else going on out there? | 30–34 | The war never ticked; 20/27 event kinds dropped; 6-node map |
| 5 — Human interface | Can a person operate this for 200 hours? | 35–39 | 74/147 seams unplugged; hunting yields vanish; no keyboard nav |
| 6 — The people in it | Is anybody *in* it? | 40–44 | Personality inferred not authored; eulogy engine referenced by nothing; affinity read by no one |

**The one sentence that covers thirty plans:** *the systems exist and the seams don't.*
Thirty waves of the same bug wearing different clothes — a hardcoded ending, a null callback, an
uncalled `SimulateDailyFriction`, a dropped event kind, an unloaded belief catalog, an `Infer…`
heuristic standing where data was authored. The permanent cure is Wave 5's Plan 36 (port contract)
plus Wave 3's Plan 29 (claims must cite evidence) plus this wave's Plan 44C (prove the loop with one
seeded run).

**Highest-value tasks across all six waves**, if only a handful are ever executed:
**19A · 22A · 24A · 29A · 31A · 34B.1 · 36A · 40A · 44A.**

## Metrics to report at wave close

1. Survivor fields authored-but-unconsumed: **72 → 0**; `Infer*` derivation functions in `src/`: **1 → 0**
2. Core classes with zero references anywhere: `ProceduralEulogyEngine` **+ the 34 `TEST_ONLY` seams from Wave 5's table → 0**
3. `DesignateLeader` / `OnCrisisEvent` / `TryMaturation` / `ApplyGrief` / `ServeMeal` / `SetHunterSkill` / `ConsumeRation` non-test callers: **0 → ≥1 each**
4. Dead narrative catalogs revived: `wall_carving_templates` (3), `confession_secrets` (8), `echoes` (23) → consumed and de-exempted
5. Hardcoded item-id lists replaced by tags: `CraftingSystem.cs:336–337`, `HoldfastTerminalPanel.cs:212`, `ShelterDecorSystem.cs:231–262`, `SilentFoundryHostSession.cs:257` → **0**
6. Affinity consumers: **0 → ≥4** (duty, expedition, caregiving, apprenticeship), each behind one query API
7. Inner-life loop arrows asserted by one seeded journey test: **0 → 9**
8. Registry/atlas classifications retired with evidence: *"Orphan State (Underconnected)"*, *"Hidden State (Weak Feedback)"*, Leadership/Trauma-Bond/Caregiving recommendation rows

## Deferred to Wave 7 → **now planned**

**[Continuity Wave 7 — Plans 45–49, *Content on Rails & the Measurement Layer*](Wave7_Continuity_Audit_INDEX.md)** picked these up: the content-acceptance ladder (29 catalogs / 452 definitions still reach nobody), reproducible balance sweeps (27 unreferenced CSVs), local player telemetry, the mod/content-pack contract, and release craft (**0 git tags**, no changelog, no versioning policy). Read it before queuing more content waves.

Original candidates:

* **Depth of the same verbs** — the parallel content waves (136, 141, 142, 145, 151–158, 160) on the rails Waves 4–6 built: routes, intel channels, tags, delivery contracts, identity, voice banks, policies. Each is now a data PR rather than a new subsystem.
* **Difficulty & first-hour at scale** — 20+ seeded funnels using 31's event stream, 38's calendar and 42's voice to place the retention cliff; the tutorial-review skill becomes a report, not a document.
* **Mod/creator surface** — tags (40B) + overlays (25C) + content-utilisation runtime evidence (27C) are exactly a mod contract; write it once, gate it.
* **Release craft** — changelog/version discipline, patch lanes, save-compatible hotfix path (`ashfall-hotfix-rollback`, `ashfall-release-captain`), and store-readiness (localization completion, accessibility conformance, controller certification).
* **Ambition audit** — a pass that asks, per plan, whether the feature is still wanted: five waves of findings suggest the project's failure mode is *breadth without connection*, not missing features.
