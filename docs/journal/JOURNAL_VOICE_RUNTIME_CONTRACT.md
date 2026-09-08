# Journal Voice Runtime Contract

## 1. Architectural Role and Overview

The **Journal Voice** system provides player-facing, diegetic narrative prose for survivor journal entries in ASHFALL. Rather than presenting generic system messages or dry log entries, occurrences in the shelter and across the wasteland are recorded from the subjective perspective of the authoring survivor.

The voice pipeline follows an engine-agnostic ports-and-adapters architecture residing in `Assets/Ashfall.Core/Journal/`:
- `Assets/Ashfall.Core/Journal/JournalVoice.cs`: Static façade exposing high-level composition methods (`ComposeBody`, `ComposeFullText`, `FormatTimestamp`).
- `Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs`: In-memory catalog model (`JournalVoiceProseCatalog`), entry DTO (`JournalVoiceProseEntry`), and deserialization service (`JournalVoiceProseCatalogLoader`).
- `Assets/Ashfall.Core/Journal/RiskBiasTrait.cs`: Defines survivor psychological biases (`Paranoid`, `Cautious`, `Realist`, `Reckless`, `Denialist`, `Fatalist`, `Empath`, `Sociopath`) and the `ISurvivorAuthor` interface.
- `Assets/Ashfall.Core/Journal/KnowledgeBase.cs`: Dedupes discoveries so each situation or codex topic is authored once.
- `Assets/Ashfall.Core/Journal/JournalSystem.cs`: Central journal controller managing the log ring buffer (cap 64), codex tabs, unread badges, and save/load persistence.

---

## 2. Data Authority and Schema

The authoritative catalog resides at:
`Assets/StreamingAssets/Data/journal_voice_prose.json`

### JSON Schema Structure
```json
{
  "schema_version": 1,
  "prose_variants": {
    "<situation_key>": {
      "default": "<neutral, factual narration>",
      "paranoid": "<hyper-vigilant, suspicious, conspiratorial>",
      "cautious": "<risk-averse, procedure-focused, measured>",
      "realist": "<pragmatic, operational, data/supply-centric>",
      "reckless": "<aggressive, dismissive of danger, action-oriented>",
      "denialist": "<minimizing, defensive, optimistic under stress>",
      "fatalist": "<resigned, accepting of decay and inevitable collapse>",
      "empath": "<optional expansion-specific perspective>",
      "sociopath": "<optional expansion-specific perspective>"
    }
  }
}
```

### Constraints and Semantics
1. **Root Object**: The file uses a top-level `"schema_version": 1` and `"prose_variants"` dictionary. The loader does not reject a missing version or unknown fields; data-integrity validation owns the catalog-level schema gate.
2. **Key Naming**: Current authored keys use lowercase `snake_case` (no spaces, hyphens, or uppercase characters).
3. **Runtime personality mapping**: `RiskBiasTrait` currently has eight enum values: `Paranoid`, `Cautious`, `Realist`, `Reckless`, `Denialist`, `Fatalist`, `Empath`, and `Sociopath`. The JSON names are the corresponding lowercase fields. `default` is also a stored prose bucket.
4. **Required coverage**: Existing catalog tests require `default` plus the six core biases for every canonical `KnowledgeKeys` entry. `empath` and `sociopath` are optional for older entries and are present on the existing expansion-06 and Plan 95 data entries.
5. **Distinctiveness**: Plan 95’s seven core variants are distinct within each new situation key. The pre-existing `high_co2` entry retains an older `default`/`cautious` duplicate; it is outside the Plan 95 data pass.
6. **Conciseness**: Plan 95 targets 1–2 sentences and approximately 15–45 words, but the loader does not enforce a word or sentence limit.
7. **Formatting**: Values are raw English strings. No localization-key lookup, placeholder substitution, Markdown, or rich-text processing exists in this pipeline. The current catalog contains no supported placeholders.

### Runtime-to-JSON personality table

| Runtime enum | JSON field |
|---|---|
| `RiskBiasTrait.Paranoid` | `paranoid` |
| `RiskBiasTrait.Cautious` | `cautious` |
| `RiskBiasTrait.Realist` | `realist` |
| `RiskBiasTrait.Reckless` | `reckless` |
| `RiskBiasTrait.Denialist` | `denialist` |
| `RiskBiasTrait.Fatalist` | `fatalist` |
| `RiskBiasTrait.Empath` | `empath` |
| `RiskBiasTrait.Sociopath` | `sociopath` |
| unknown enum value | `default` |

---

## 3. Resolution and Fallback Mechanics

When a journal entry is composed:
```csharp
string body = JournalVoice.ComposeBody(knowledgeKey, authorBias);
string full = JournalVoice.ComposeFullText(knowledgeKey, authorBias, day);
```

### Resolution Order
1. If the catalog is bound and contains `knowledgeKey`, `JournalVoice` calls `entry.GetProseForBias(bias)`.
2. A known non-empty field is returned as-is. Unknown enum values map to `entry.@default`.
3. A known key with an empty selected field does **not** fall through to `default`; `JournalVoiceProseCatalog.GetProse` returns the generic fallback instead.
4. An unbound catalog or unknown situation key returns `"Something changed. I wrote it down so I would not forget."`.
3. `ComposeFullText` formatting:
   - If `body` already begins with `"Day "`, it is returned unchanged.
   - Otherwise, prepends `"Day {day}. "`:
     ```csharp
     return $"Day {day}. {body}";
     ```

---

## 4. Situation-Key Producer Boundary

`JournalVoice` accepts an arbitrary caller-supplied `knowledgeKey`; it does not derive situation keys from needs, weather, radiation, expeditions, raids, disease, power, roster, death, or guilt state. The only direct production callers found in the current source use:

- the five baseline `KnowledgeKeys` values;
- dynamic codex keys;
- data-driven narrative/collectible journal-unlock targets;
- existing micro-location and expansion keys.

No current Core/host call or data `journalUnlockId` references the 12 Plan 95 situation keys. They are therefore authored candidates, not runtime-reachable production situations. Plan 95 adds no dispatch logic or threshold mapping.

## 5. Save/Restore and Mutation Guarantees

- `JournalSystem.CaptureState()` writes rendered `JournalEntry.Text` directly into the save payload (`JournalSave.Entries`).
- Once a journal entry is written to disk in a save slot, its text is **immutable**.
- Adding or modifying entries in `journal_voice_prose.json` will never alter the prose of past, already-recorded journal entries in existing player saves.
- New entries generated during ongoing gameplay will immediately resolve against the updated catalog.

---

## 6. Architectural Invariants

- **Core Invariant 1 (Zero Engine Coupling)**: Neither `JournalVoice`, `JournalVoiceProseCatalog`, nor `JournalSystem` reference `UnityEngine`, `Godot`, or `GodotSharp`.
- **Core Invariant 4 (Determinism)**: Lookup is a pure deterministic dictionary projection based on `(knowledgeKey, RiskBiasTrait)`.
- **Core Invariant 6 (Data Authority)**: The JSON file in `StreamingAssets/Data/` is authoritative. No hardcoded C# string mappings exist in the host layer.
