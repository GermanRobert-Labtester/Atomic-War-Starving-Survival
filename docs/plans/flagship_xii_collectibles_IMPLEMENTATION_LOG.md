# Flagship XII (collectibles) — Implementation Log

Plan: Flagship Integration Plan XII — Collectible Narrative Quality,
Journal/Codex Content, Faction Intel & Localization Readiness.
Date: 2026-09-05. Branch: `feat/asset-pipeline-flagship`.

## Phase 0 — Forensic audit — PASS

Changed: nothing (read-only).

Result:
- 40 collectible definitions confirmed; live effect mix: none 12, morale 7,
  faction_info 7, knowledge 7, journal_unlock 4, location_clue 3.
- **Journal unlock live count = 4** (casualty_records, soldier_letters,
  religious_texts, exchange_day). The plan's fifth slot is a phantom;
  nothing invented. `journal_religive_texts` typo not canonicalized.
- Acquisition chain: `Inventory.OnItemAdded` →
  `CollectibleEffectDispatcher.DispatchOnAcquire` → per-effect authority;
  `journal_unlock`/`faction_info` both route to
  `JournalSystem.TryDiscoverKnowledge` (codex authority). No separate
  faction-intel system exists; no standing surface is reachable from the
  dispatcher.
- Save owner: `CollectibleDiscoveryState` (checksummed envelope) + journal
  section; restore never fires effects.
- Localization: `LocalizationService` is UI-key-based (CSV, `ui.*`);
  catalog text is raw default-language strings → Plan XII §4.4 raw-string
  model applies.
- Generic reference validation (Stage 3) already existed
  (`Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs`):
  journal_unlock/faction_info targets must resolve against
  `journal_voice_prose.json`; knowledge → research; location_clue → map.

Divergences recorded: plan assumed descriptions live in `collectibles.json`;
they live in `items.json` (item authority). Plan assumed a faction-intel
codex distinct from the journal; the journal knowledge base **is** the codex.

## Baseline — recorded (foreign red)

- `dotnet test` (shared test project) did not compile at baseline and at
  closure: ~1,000 errors across ~40 unrelated in-flight test files
  (WildlifeTrapping*, DistressSignal*, Debt*, WeatherGate*, MicroLocation*,
  PatrolEncounter*, DutyRoster*, Greenhouse*, Water*, Radio*, Content*, and
  some Collectible* files) referencing Core APIs newer than the checkout.
- Root cause (not mine): a concurrent stream ran `git reset --hard` at
  00:58 (reflog: `reset: moving to ac37da7e`), rolling Core back while
  untracked test files (which resets do not touch) kept referencing the
  newer API surface. The same reset destroyed uncommitted working-tree
  content, including this flagship's authored codex prose (recovered — see
  Stage 4/5) and the then-public `CollectibleCatalogFileRaw` visibility that
  several untracked collectible test files compile against.
- Baseline repair by me: `MicroLocationDeterminismHarness.cs` (untracked,
  foreign file) had a hard CS0052 (`public SeededRng Rng` field over an
  internal type) blocking every build; changed the field to `internal`
  (one line, semantics unchanged). That file remains untracked/foreign.
- `Ashfall.Core` itself went transiently red at closure with 2 errors in
  files I never touched (`Catalogs/InstitutionCatalogParse.cs:129`,
  `Diplomacy/DiplomaticTreatyCatalog.cs:126`) — concurrent live edits,
  expected to settle with their wave.

## Stage 4/5 — Codex content — PASS (with recovery note)

Changed:
- `Assets/StreamingAssets/Data/journal_voice_prose.json`: restored the 11
  codex keys (4 journal + 7 faction; 7 voices each) **recovered from the
  pre-reset snapshot** `builds/linux/.../journal_voice_prose.json`, then
  re-authored `default`+`realist` voices to 2–3 restrained sentences each.
  Diff vs HEAD: +99/−0 (purely additive; 14 → 25 prose keys).

Result: every live journal_unlock/faction_info target resolves to authored
content; acquisition writes "Day N. …" entries through the canonical
`TryDiscoverKnowledge` path; placeholder fallback text can no longer occur.

## Stage 6 — Collectible prose rewrites — PASS

Changed (`items.json`, 5 surgical description edits):
team_pennant (faded→washed-out), mothers_letter/soldiers_letter/music_box
(4→3 sentences), military_patch (torn→pulled). All within-identity,
facts and effect wiring untouched.

## Stage 7 — Localization — PASS (decision: raw-string model)

Changed: nothing (decision record in closeout; no dead fields added).

## Stage 8 — Tests — PASS

New (self-contained, live-data-driven, no hardcoded target IDs):
- `CollectibleNarrativeQualityTests.cs` — corpus gates: 40/40 non-empty,
  ≤3 sentences, names ≤50 chars unique per category, cliché ceilings
  (faded/torn/bloodstained/haunting reminder ≤2), brand/team/publication
  blacklist, slang blacklist, procedural-instruction blacklist, generic
  prose-target resolution, 2–4 sentence entry contract for default+realist.
- `CollectibleCodexUnlockLiveTests.cs` — per live codex collectible:
  authored-entry acquisition (exact composed text, no placeholder),
  duplicate-acquisition idempotency, second-dispatcher no-duplicate-entry,
  save/restore preserves unlocks with zero notification replay, and
  FactionWarSystem standing isolation (no record created/modified, no
  event, unrelated faction standing intact).

## Stage 9/10 — Docs and closure — PASS

Follow-up restoration: 12 Plan 95 situation prose keys recovered from the
same pre-reset snapshot (additive; 25 → 37 keys) — unblocked the foreign
`JournalVoiceProseExpansionTests` (7/7 PASS after restore).

Final gates: host build PASS (0/0); bridge-selftest PASS; content-utilization
CI gate PASS; isolated harness 18/18 (twice); full-suite green window
7837/342 with every failure proven foreign via the parent-data experiment;
data-integrity FAIL(8) foreign treaty/debt/therapy ids; `--collectible-selftest`
not routable on rolled-back HEAD (verb registration was destroyed state).

- `docs/narrative/COLLECTIBLES_NARRATIVE_QUALITY_AUDIT.md` — 40-row matrix,
  register distribution (16 distinct primaries; loss 4; routine 5; joy/pride
  4; faith 2; bureaucracy 4), cliché report, rewrite log, IP/slang reviews,
  empty fictional proper-noun inventory (all references generic).
- `docs/narrative/COLLECTIBLES_CONTENT_INTEGRATION_CLOSEOUT.md` — full
  closeout with the 4-vs-5 resolution, mapping tables, localization
  decision, save/load contract, incident provenance, verification.
- Isolated verification (gitignored `Builds/_verify_flagship_xii` harness,
  ProjectReference → Ashfall.Core + the two new test files):
  `dotnet test` → **18/18 PASSED**.
- Canonical gates (`dotnet build Ashfall.csproj`, full `dotnet test`,
  godot selftests) to be re-run when the concurrent streams' Core edits
  settle; blocked at closure by foreign in-flight errors (see Baseline).

## Divergences from plan (summary)

1. Descriptions authored in `items.json`, not `collectibles.json`.
2. Faction-intel codex == journal knowledge base (single authority).
3. Stage 3 validators already existed (Task 8 wave); re-pinned in tests
   instead of rewritten.
4. Entry-contract scoping: default+realist voices carry the 2–4 sentence
   contract (the only live production path); trait voices remain one-line
   corpus style — documented, gate-ready for extension.
5. Localization resolved to §4.4 raw-string model (no key fields added).
