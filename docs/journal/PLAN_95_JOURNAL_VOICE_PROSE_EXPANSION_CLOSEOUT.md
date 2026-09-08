# Plan 95 — Journal Voice Prose Expansion Closeout

## Status

**COMPLETE — prose expansion with deferred producer-dependent keys.**

The authored corpus is present, but the current runtime does not request any
of the 12 Plan 95 situation keys. This closeout deliberately does not claim
player reachability and does not add Core dispatch logic.

Required wording:

```text
Journal voice prose expansion authored.
Production-added key count: 0 in this implementation pass.
Deferred producer-dependent keys: 12.
No Core journal dispatch logic was added under Plan 95.
```

The 12 keys were already present in the working catalog when this audit
started. They remain documented and tested as authored candidates. A later
integration task must activate them through an existing producer or remove
them from production data until such a producer exists.

## 1. Runtime contract

- Data authority: `Assets/StreamingAssets/Data/journal_voice_prose.json`.
- Root: `schema_version` plus `prose_variants`.
- Input: caller-supplied `knowledgeKey` and `RiskBiasTrait`.
- Core voice buckets: `default`, `paranoid`, `cautious`, `realist`,
  `reckless`, `denialist`, `fatalist`.
- Additional live enum fields: `empath` and `sociopath`; optional on older
  entries and present in the recovered Plan 95 data.
- Values are raw English strings. There is no localization, placeholder,
  Markdown, or rich-text substitution.
- Unknown key/unbound catalog and empty selected variant use the generic
  fallback. Unknown enum values select `default`.
- `JournalSystem` persists rendered `JournalEntry.Text`, so later catalog
  changes do not rewrite existing saved entries.

See `JOURNAL_VOICE_RUNTIME_CONTRACT.md` for the source-level contract.

## 2. Catalog and authored content

| Measure | Result |
|---|---:|
| Current catalog keys | 39 |
| Existing keys before Plan 95 snapshot | 25 |
| Plan 95 situation candidates | 12 |
| Plan 95 core variants | 84 |
| Runtime-reachable Plan 95 keys | 0 |
| Deferred producer-dependent keys | 12 |
| Core/C# dispatch changes | 0 |
| Save schema changes | 0 |

The detailed 84-line prose matrix is in
`PLAN_95_JOURNAL_VOICE_KEY_MATRIX.md`.

## 3. Producer evidence

All 12 candidates are **DEFERRED**. The audit searched exact key literals,
`TryDiscover`/`TryDiscoverKnowledge` callers, data `journalUnlockId` values,
collectible journal targets, and the existing `KnowledgeKeys` list.

Feedback IDs such as `low_food`, `disease_outbreak`, and `power_failure` do
not dispatch journal entries. Plan names and future-system documentation do
not count as producers.

See `PLAN_95_JOURNAL_VOICE_PRODUCER_MATRIX.md` for the complete key-by-key
classification and activation contract.

## 4. Voice-quality review

- The 84 core variants are non-empty and snake_case-keyed.
- Each Plan 95 key has seven non-empty core voice fields.
- Within-key exact duplicates were not found in the Plan 95 corpus.
- Paranoid lines vary suspicion through records, timing, motives, and missing
  information rather than repeating a conspiracy claim.
- Cautious lines vary mitigation, staging, reserve protection, and review.
- Realist lines remain practical without making every line a numeric report.
- Reckless lines favor momentum without requiring irrational self-destruction.
- Denialist lines minimize or reframe without contradicting unavoidable facts.
- Fatalist lines vary resignation and inevitability rather than repeating a
  single doom phrase.
- The prose does not add unsupported names, causes, culprits, or placeholders.

The pre-existing `high_co2` default/cautious duplicate is outside the Plan 95
candidate set and remains unchanged.

## 5. Persistence and UI review

Journal entries store rendered text, author, key, and timestamp in
`JournalSave`. Catalog updates affect future entries only. The journal UI
renders plain text and already uses wrapping labels; Plan 95 adds no special
formatting or line-break requirements. The prose stays within the existing
short journal-entry style, with a 1–2 sentence target.

## 6. Verification

| Check | Result |
|---|---|
| JSON parse and Plan 95 coverage audit | PASS, 12 keys and 84 core variants |
| Producer reachability audit | PASS, 12 correctly deferred |
| Scoped JSON/diff validation | PASS |
| Data-integrity selftest | PASS, 0 errors across 298 catalogs |
| Content-utilization selftest | PASS, CI gate; existing unrelated catalog warnings remain |
| Focused journal voice tests | PASS, 29 tests |
| Full `Ashfall.Core.Tests` | PASS, 9,784 tests |
| `dotnet build Ashfall.csproj` | PASS, 0 warnings, 0 errors |
| `--journal-selftest` | PASS, 23/23 |
| `--journal-save-selftest` | PASS |

This report must not be read as evidence that the 12 candidate situations
currently fire in gameplay. The repository-wide `git diff --check` remains
blocked by an unrelated pre-existing blank line in
`docs/narrative/PLAN_89_MUSTER_EPILOGUES_EXPANSION_CLOSEOUT.md`; the scoped
Plan 95 check passes.
5. **Scene Lint**: