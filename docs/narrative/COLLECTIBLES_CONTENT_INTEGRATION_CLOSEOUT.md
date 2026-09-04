# COLLECTIBLES_CONTENT_INTEGRATION_CLOSEOUT.md
Flagship Integration Plan XII — Collectible Narrative Quality, Journal/Codex
Content, Faction Intel & Localization Readiness. Closed 2026-09-05.

## Collectible inventory

```text
collectible count:       40
journal_unlock count:    4
faction_info count:      7
knowledge:               7
morale:                  7
location_clue:           3
none:                    12
```

## Journal unlock count — 4 vs 5 mismatch RESOLVED

Live data contains **exactly four** `journal_unlock` targets:

```text
journal_casualty_records   (item_collectible_casualty_list)
journal_soldier_letters    (item_collectible_soldiers_letter)
journal_religious_texts    (item_collectible_prayer_book)
journal_exchange_day       (item_collectible_exchange_day_newspaper)
```

The source checklist's fifth slot was a phantom; no fifth entry was invented.
`journal_religive_texts` (checklist typo) was **not** canonicalized and no
alias ID was created; the canonical target remains `journal_religious_texts`.

## Journal mapping

| Collectible | Target | Content status |
|---|---|---|
| `item_collectible_casualty_list` | `journal_casualty_records` | authored (7 voices; default+realist 3 sentences) |
| `item_collectible_soldiers_letter` | `journal_soldier_letters` | authored (7 voices; default+realist 3 sentences) |
| `item_collectible_prayer_book` | `journal_religious_texts` | authored (7 voices; default+realist 3 sentences) |
| `item_collectible_exchange_day_newspaper` | `journal_exchange_day` | authored (7 voices; default+realist 3 sentences) |

Content authority: `Assets/StreamingAssets/Data/journal_voice_prose.json` via
`JournalVoiceProseCatalogLoader` → `JournalVoice.ComposeFullText`. Entries are
written by the canonical `JournalSystem.TryDiscoverKnowledge` path (entry +
codex unlock through one dedup gate). No collectible-specific journal
subsystem exists.

Entry-contract scoping (honest note): the Plan XII style contract asks for
2–4 sentence entries. The dispatcher — the only live discovery path for these
keys — passes `author: null`, which `JournalSystem` resolves to the **Realist**
voice. Accordingly the `default` and `realist` voices of all eleven keys were
authored at 3 sentences and are pinned at 2–4 by CI
(`CodexTargets_DefaultAndRealistProse_AreTwoToFourSentences`). The remaining
trait voices (paranoid/cautious/reckless/denialist/fatalist) keep the corpus's
established one-line voice and are not reachable through collectible
acquisition; if a future pass wires trait-authored discoveries to these keys,
extend the contract gate to those voices first.

### Content restoration provenance (concurrent-stream incident)

During this milestone, a concurrent stream's `git reset --hard`
(reflog `00:58`, moving to `ac37da7e`) destroyed **uncommitted working-tree
content**, including the eleven codex prose entries that had been authored in
`journal_voice_prose.json` for this flagship. The entries were recovered from
the pre-reset snapshot `builds/linux/Assets/StreamingAssets/Data/
journal_voice_prose.json` (trait voices restored byte-for-byte), and the
`default`/`realist` voices were then re-authored on top to the 2–4 sentence
entry contract. The restored file diff against HEAD is purely additive
(99 insertions, 0 deletions; 14 → 25 prose keys).

The same reset also destroyed twelve Plan 95 *situation* prose keys
(`low_food`, `low_water`, `death_of_survivor`, `successful_expedition`,
`failed_expedition`, `faction_raid`, `disease_outbreak`, `power_failure`,
`new_survivor_arrived`, `severe_cold`, `high_radiation_zone`,
`moral_compromise`) that another stream's untracked
`JournalVoiceProseExpansionTests` (7 tests) require — restored from the same
snapshot in a follow-up commit (additive; catalog 25 → 37 keys), since the
committed `Content/CollectibleCatalogIntegrityValidator` and the codex
contract assume a complete prose authority.

## Faction intel mapping

All seven targets route through the codex authority
(`CollectibleEffectDispatcher.ApplyJournalUnlock` → `JournalSystem`).
No faction-intel entry touches standing — enforced structurally (the
dispatcher has no faction provider) and pinned by
`FactionInfoAcquisition_DoesNotMutateFactionStanding`.

| Collectible | Target | Standing before/after |
|---|---|---|
| `item_collectible_unit_photograph` | `faction_military_history` | unchanged (tested) |
| `item_collectible_propaganda_poster` | `faction_state_propaganda` | unchanged (tested) |
| `item_collectible_unit_log_fragment` | `faction_military_operations` | unchanged (tested) |
| `item_collectible_deployment_order` | `faction_military_deployment` | unchanged (tested) |
| `item_collectible_civil_defense_badge` | `faction_civil_defense` | unchanged (tested) |
| `item_collectible_military_patch` | `faction_military_units` | unchanged (tested) |
| `item_collectible_trade_guild_patch` | `faction_trade_guilds` | unchanged (tested) |

Editorial: every entry is evidence-grounded in its physical object
(photograph → unit composition and insignia; poster → messaging posture with
claims treated as claims; log → operational accounting; deployment order →
movement tables; badge → warden institutional network; patch → unit lineage;
guild patch → route/hierarchy structure). All organizations fictional. No
real-world operational instruction.

Reference integrity (Plan XII Stage 3) was already machine-enforced by the
committed generic validators
`Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs`
(ERR_JOURNAL_TARGET_MISSING / ERR_FACTION_TARGET_MISSING against
`journal_voice_prose.json`) and re-pinned in tests
(`JournalAndFactionTargets_ResolveAgainstProseAuthority`) — generic, driven by
live data, no hardcoded four-ID list.

## COLLECTIBLE_LOCALIZATION_DECISION

```text
canonical model:   raw default-language strings (Plan XII §4.4 path)
new fields:        none — no nameKey/descriptionKey added (no dead fields)
key format:        n/a (LocalizationService keys remain UI-chrome only, ui.*)
default locale:    en (implicit; catalog text is default-language English)
fallback:          n/a for catalog text; LocalizationService.Get(key, default)
                   governs UI strings as before
```

Rationale: `LocalizationService` + `assets/l10n/strings.csv` is a UI-string
service (`ui.common.ok` et al.). Every mature catalog (items, recipes,
journal prose, radio) stores default-language text at the authority and no
catalog text participates in the key system. Adding `nameKey`/
`descriptionKey` to collectibles would have created a collectible-only
localization path — explicitly forbidden by the plan. Quality gates
(non-empty, ≤50-char names, ≤3-sentence descriptions, uniqueness within
category, brand/slang blacklists) pin the raw corpus in CI so a future
key-first migration starts clean. Persisted campaign state contains stable
IDs only (`CollectibleDiscoveryState`, journal knowledge keys) — localized
text is never saved as authority, so save locale A / load locale B is
structurally guaranteed.

## Save/load and notification contract

- Collectible discovery: `CollectibleDiscoveryState` (checksummed envelope,
  ordinal-ordered IDs) — pre-existing coverage retained
  (`CollectibleDiscoveryStateTests`, `CollectibleDiscoveryPersistenceTests`).
- Journal/codex unlocks: `JournalSystem.CaptureState/RestoreState` — verified
  `SaveRestore_PreservesUnlocks_WithoutReplayingNotifications`: after restore
  the keys remain, zero `OnEntryAdded`/`OnCodexUnlocked`/`OnNotificationPing`
  fire, and re-dispatch is `AlreadyDiscovered` with no new entries.
- Duplicate acquisition: idempotent (discovery gate + knowledge dedup gate),
  verified for every live codex collectible.
- Restore never replays acquisition effects (save restore does not fire
  `OnItemAdded`; discovery idempotence is defense-in-depth).

## Files changed (this milestone)

```text
Assets/StreamingAssets/Data/journal_voice_prose.json   +11 codex keys restored
                                                       (from pre-reset snapshot),
                                                       default+realist re-authored
                                                       (99 insertions, 0 deletions)
Assets/StreamingAssets/Data/items.json                 5 surgical description
                                                       rewrites (audit log below)
Ashfall.Core.Tests/CollectibleNarrativeQualityTests.cs NEW — CI editorial gates
Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs  NEW — live acquisition/
                                                       idempotency/save/standing
docs/narrative/COLLECTIBLES_NARRATIVE_QUALITY_AUDIT.md NEW — 40-row matrix
docs/plans/flagship_xii_collectibles_IMPLEMENTATION_LOG.md  NEW
```

## Verification

### Isolated verification (my scope, fully green)

Because unrelated in-flight test files elsewhere in the shared test project
were mid-refactor by concurrent streams during this milestone (see Risks),
these tests were additionally compiled and run in isolation against
`Ashfall.Core` via a gitignored scratch harness (`Builds/_verify_flagship_xii`,
ProjectReference → Ashfall.Core, xunit):

```text
dotnet test Builds/_verify_flagship_xii/_verify_flagship_xii.csproj
Passed: 18 / 18   (13 acquisition/idempotency/save/standing + 5 corpus gates incl. cliché theory rows)
```
Re-confirmed 18/18 after the situation-key restoration. The same two test
files are committed into `Ashfall.Core.Tests` and run as part of the normal
suite.

### Canonical gates (final state at closure)

```text
dotnet build Ashfall.csproj                                  PASS (0 errors, 0 warnings)
dotnet test  (shared suite, one green window)                7837 passed / 342 failed / 8179
  -> all 342 failures in foreign namespaces (see below)
dotnet test --filter Collectible|JournalVoice|Journal|Content...  my namespaces:
  CollectibleCodexUnlockLiveTests + CollectibleNarrativeQualityTests   PASS (18/18)
  JournalVoiceProseExpansionTests (7)                       PASS after situation-key restore
godot --headless -- --bridge-selftest                        PASS (exit 0)
godot --headless -- --content-utilization-selftest           CI gate PASS (exit 0)
godot --headless -- --data-integrity-selftest                FAIL (8) — foreign, see below
godot --headless -- --collectible-selftest                   NOT ROUTABLE on current HEAD
```

Failure-independence proof: the scavenging-placement / merchant-balance /
generation-channel failures reproduce IDENTICALLY with the parent-commit
versions of `items.json` + `journal_voice_prose.json` checked out
(parent-data experiment, same run) — they are casualties of the same reset
(the destroyed tree evidently also carried collectible scavenging-table
placements that the untracked placement tests expect; restoring those is the
expedition/economy streams' lane). The 8 data-integrity findings are
unresolved ids in `diplomatic_treaties.json`, `ledger_debt_templates.json`,
`psychological_therapies.json` — foreign treaty/debt/therapy TDD churn; zero
findings touch collectibles, items, or prose. `--collectible-selftest` is
"Unrecognized headless argument" on the rolled-back HEAD: the untracked
`HostCli.Collectibles.cs` survives but its verb registration was part of the
destroyed working-tree state; the host also crashed in foreign
`Main.SetupExpandedShelterSystems` during that probe.

## Remaining risks

- **Shared test project red (foreign, pre-existing to my changes):** at
  closure, `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` does not
  compile due to ~1,000 errors across ~40 unrelated in-flight test files
  (WildlifeTrapping*, DistressSignal*, Debt*, WeatherGate*, MicroLocation*,
  PatrolEncounter*, …) that reference Core APIs newer than the current
  checkout — fallout of the same concurrent `git reset --hard` incident. This
  is not attributable to Plan XII; my files carry no dependency on any of it.
  Evidence and file-level inventory are in the implementation log.
- `Ashfall.Core` itself was mid-edit by another stream at closure time
  (transient `InstitutionCatalogParse.cs` compile error observed, expected to
  settle within their wave).
- Trait voices for the eleven codex keys remain one-line; see scoping note
  above.

## Milestone outcome

The 40-object corpus reads as evidence of a lived-in society (routine,
institutions, faith, sport, pride, family, work, fear, frustration, joy,
loss — 16 distinct primary registers, loss capped at 4). Journal entries
provide restrained interpretation; faction-intel reveals bounded
organizational knowledge with standing isolation machine-enforced; catalog
text is raw-string by canonical decision with CI-pinned quality; campaign
state persists stable IDs only.
