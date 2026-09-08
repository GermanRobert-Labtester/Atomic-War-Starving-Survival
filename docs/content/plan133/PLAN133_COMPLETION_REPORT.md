# Plan 133 Completion Report — Faction War Communiqués 18 → 40

> Data-only expansion. All 18 baseline entries preserved verbatim (verified by deep
> dict comparison against the pre-edit file AND by
> `FactionWarCommuniqueExpansionTests.Existing18Ids_PreservedVerbatim`).
> Note: the working tree's concurrent integration commit (bdd7da3b) swept the Plan 133
> files into the lane while the task was in its verification phase; the committed
> content is the final post-audit version (verified — 40 entries, all four §14 prose
> repairs present).

## Final counts per faction (§22)

| Faction | Baseline | Added | Final | Modes shipped across the faction's corpus (≥4 each — DoD) |
|---|---|---|---|---|
| faction_central_garrison | 6 | +7 | 13 | procedural denial; jurisdictional narrowing (span / fee / pumphouse); continuity-legitimacy (quota); concern-framing as paper trail (d530); concede-nothing ceasefire (d592); quiet walk-back clause (d592 review clause) |
| faction_rebuilders | 5 | +7 | 12 | manifests/counter-scale (d490); labor arithmetic (d507); practical witness record (d514); internal disagreement stated publicly (d527, d556); anger at euphemism (d568); benchmark/correction (d594); honest uncertainty (baseline d550/d582) |
| faction_ash_sign | 5 | +4 | 9 | doctrine grounded in practice (d523); honest uncertainty (d561, d577); observation-vs-interpretation discipline (d602); doctrinal fracture + public humility (baseline d583/d591) |
| faction_forward_roster | 2 | +4 | 6 | local legitimacy / origin (d575); posted-rate institutionalism (d576); rumor answered with signed records (d599); non-recognition response / anti-banditry self-definition (d608) |

**Allocation deviation from the §7 recommendation (G7/R6/A5/FR4 → G7/R7/A4/FR4)**,
justified by event-chain participation: the Rebuilders are the counterparty in nearly
every covered chain (they carry 7 of the new pairs' counter-voices), while the Ash
Sign's two shrine chains (d517/d578) were already triple-covered — the remaining
Ash-Sign-appropriate events (d522, d558, d578-pre-strike, d600) support exactly four
new entries without inventing involvement (risk #17/#12: duplicate perspectives,
implied supernatural knowledge).

## Event-chain coverage (§22)

- **14 chains have ≥2 competing faction perspectives** (target ≥10) — pinned by test.
- **5 chains have ≥3 perspectives** (target ≥5): d495, d517, d545, d578, d588 — pinned by test.
- Newly covered bands: cold_war (d488), open_conflict (d503, d509, d522, d524),
  offensive (d552, d558), culmination (d565, d600) — pinned by test.
- Deliberate non-coverage (documented, not gaps): d480/d485 (radio carries both
  voices; bulletin-board mystery preserved), d491 (Toll posts rate notices on radio,
  not communiqués — canon), d541 (branch-impossible content), d583/D-9 (clandestine;
  issues no public statements).

## Truth-mode distribution (authorNote classifications, §10)

31 of 40 entries carry authorNote (9 baseline + all 22 new). Distribution of the
corpus's hidden truth layer: accurate ×13; accurate-but-incomplete/misleading-framing
×6 (incl. baseline 4 "False" entries → false-with-evidence-unavailable ×4, false-and-cynical
×1, true-by-accident ×1); honest uncertainty ×3; doctrinal interpretation/humility
×4; correction-by-institution ×2 (d592's review clause, d583's retraction lineage).
No new truth enum added; no consumer reads authorNote (two mechanical gates pin this:
cross-surface text containment + src source-scan).

## Temporal histogram (§13)

| Band | Days | Communiqués |
|---|---|---|
| cold_war | 489–498 | 5 |
| open_conflict | 505–530 | 11 |
| the_offensive | 537–561 | 7 |
| culmination | 568–608 | 17 |

Validation performed: every day > 0; every statement on/after the earliest stage of
its chain (chronology test); later corrections follow earlier claims (d526 after
d522 fee, d592 walk-back after d549); no ceasefire statement precedes day 588 (test);
no Forward Roster statement precedes its first public statement day 573 (test).
Temporal spread pinned by `Temporal_Distribution_IsNotClusteredInOneBand`.

## Branch-sensitive entries rewritten/deferred (§9)

Deferred (documented in COMMUNIQUE_BRANCH_SAFETY_MATRIX.md): all d541 statements
(branch-unsafe); Garrison LN74 possession claim (branch-sensitive intercept routing
— test-pinned); any reference to all 16 `evt_p25_*` flag-gated chains
(`Communiques_NeverReferenceFlagGatedChains`); Rebuilders "door stays open" invitation
(would imply the player's d605 push). Rewritten during the repetition audit: 4 entries
(d592, d570, d527, d608 — phrase overlap repairs; see below). Every shipped entry is
class A or C, or D with an invariant outcome.

## Repetition / cross-surface audit results (§14)

- 5-gram scan vs radio/journal/dialogue: new entries carry **zero** content overlaps;
  three remaining shared 5-grams are documented-intentional (the canonical posted
  rate "two units standard goods or equivalent"; d599's deliberate reference to the
  radio broadcast it answers; a connective containing the location name).
- Repairs applied after the audit: d592 no longer duplicates radio d589's bulletin
  opener; d570's bureaucratic formula no longer templates d505; d527 no longer
  paraphrases baseline d538's "honor rule… three years without a single uniform";
  d608's origin echo softened from verbatim to callback.
- In-corpus repeats: none beyond documented signature uses ("Come through clean" ×3,
  bounded by test; "On the…" title skeleton capped at the baseline's 6 by test).
- Honest-uncertainty phrases bounded: "we do not know" family = 4 corpus-wide;
  "we are not calling" (Rebuilders negation habit) = 2 entries.

## authorNote render verification (§3.2)

1. Repo-wide consumer trace: `authorNote` is referenced only by the DTO field
   definition and the JSON files themselves — no test, host, UI, epilogue, or save
   consumer reads it.
2. Test gates added: `AuthorNote_NeverAppearsInPlayerFacingText` (normalized note
   text must not appear in any communiqué title/body, radio message, journal body,
   dialogue body, or location-override description) and
   `AuthorNote_NoPlayerFacingConsumers_Gate` (source-scans `src/` for any
   `authorNote` reference — fails if a future renderer appears unaudited).

## Save/reachability fixtures (§15/§16)

No seen/unlock state exists for communiqués (verified against
`FactionWarChainRunnerState` and the `year_of_ash` envelope) — §16's ten cases reduce
to: no save schema touched ⇒ old saves trivially valid; exact-day availability pinned
(`GetCommuniquesForFaction_DayBoundary_IsExact`); duplicate-id / unknown-chain /
unknown-faction / negative-day / empty-body / missing-authorNote loader policies
characterized and pinned with negative fixtures; content reload is a plain catalog
re-load (pinned by the existing `Loads_From_Missing_Directory_Without_Crash` and the
new loader fixtures). §15 finding (reported, not acted on per §23.13): **no
player-facing selector/renderer consumes the corpus today** — day-gated query API
exists (`GetCommuniquesForFaction`), but surfacing is a separate host-layer task;
Plan 133 stayed data-only.

## Modified files

| File | Change |
|---|---|
| `Assets/StreamingAssets/Data/faction_war_communiques.json` | 18 → 40 (198 pure insertions; 18 originals byte-identical; chronological order) |
| `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs` | NEW — 29 tests (20 baseline/contract + negative fixtures + 9 post-expansion pins) |
| `docs/content/plan133/` ×6 | baseline recon, baseline matrix, coverage matrix, branch-safety matrix, voice bible, this report |
| `docs/INDEX.md`, `docs/data/CATALOG_REGISTRY.md` | regenerated (gate-required sync) |

## Gate results (§18)

| Gate | Result |
|---|---|
| `dotnet build` (tests csproj) | **PASS** — 0 errors (pre-existing analyzer warnings only, all in concurrent streams' files) |
| `dotnet test` full suite | **PASS 10,198/10,198** (with Plan 133 content in place; run twice green during this task) |
| `dotnet build Ashfall.csproj` | **PASS** — 0 errors, 0 warnings |
| `--data-integrity-selftest` | **PASS** — 300 catalogs, 0 errors |
| `--content-utilization-selftest` | **PASS** — CI gate PASS, 0 orphaned; `faction_war_communiques.json` consumer mapping unchanged |
| `--bridge-selftest` | **PASS** — exit 0 |
| `python3 scripts/ci/run-gates.py --tier fast` | 46/47 green including all Plan-133-affected gates; **3 failing tests are owned by a concurrent in-flight stream** (`HostCliHelpContractTests` ×2 — uncommitted `src/Host/HostCli.cs` flags from the StartingSupplies stream; `JsonNamingMixPinTests` — `starting_supplies.json`, same stream). Zero of the failures touch communiqué paths; both gate-runner and full-suite runs passed for Plan 133's scope before that stream landed its edits (test count moved 10,198 → 10,205 with that stream's new files mid-verification). |
| Faction-war / real-campaign journey selftests | none exist as verbs (checked `HostCli`); canonical verbs above cover the plan's verification list |

## DoD checklist (§21)

- [x] 40 total communiqués, all original IDs preserved verbatim (test-pinned)
- [x] every new eventChain/faction reference resolves (strict-FK + known-faction tests)
- [x] chronology coherent (earliest-stage chronology + ceasefire + Roster-identity tests)
- [x] no statement depends on an unsupported branch gate (branch-safety matrix + `evt_p25_*` gate + intercept-possession gate)
- [x] author notes non-player-facing (two mechanical gates)
- [x] ≥4 rhetorical modes per major faction (documented above)
- [x] competing perspectives cover more of the war arc (14 multi-voice chains, 5 triplets)
- [x] no communiqué owns world state or event truth (bodies are claims; authorNote holds truth-layer only)
- [x] old saves valid (no save schema touched; no seen-state exists)
- [x] integrity, build, content-utilization gates pass; remaining fast-tier failures are concurrent-stream-owned and documented

## Quality targets (§8)

| Target | Result |
|---|---|
| ≥10 chains with two competing perspectives | **14** |
| ≥5 chains with three perspectives | **5** |
| ≥4 later statements revisiting/correcting an earlier claim | **4** (d526 fee self-revision; d592 review-clause walk-back; d602 continuation of the d583 retraction; d608 reframe of d607) |
| ≥4 honest-uncertainty statements | **4** (d561, d577, baseline d582, baseline d591) |
| ≥4 materially misleading without villain monologue | **6** (d489, d513, d530, d570, baseline d519, baseline d549) |
| ≥4 logistics/relief/records/infrastructure statements | **8** (d490, d505, d507, d568, d576, d599, d592 distribution clause, baseline d537) |
| ≥3 aftermath/ceasefire-legitimacy statements | **5** (d592, d594, d608, baseline d593, baseline d607) |
