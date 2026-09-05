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
      "empath": "<optional interpersonal/compassionate perspective>",
      "sociopath": "<optional utilitarian/clinical extraction perspective>"
    }
  }
}
```

### Constraints and Semantics
1. **Root Object**: Requires top-level `"schema_version": 1` and `"prose_variants"` dictionary.
2. **Key Naming**: Strict lowercase `snake_case` (no spaces, no hyphens, no uppercase characters).
3. **Core Biases**: Every situation key must define `default` plus the six core personality biases: `paranoid`, `cautious`, `realist`, `reckless`, `denialist`, `fatalist`.
4. **Distinctiveness**: Within any single situation key, all variants must be distinct strings reflecting their unique cognitive worldview.
5. **Conciseness**: Target length is 1–2 sentences (15–45 words) to avoid UI text truncation and wrap cleanly in Godot's word-smart labels.

---

## 3. Resolution and Fallback Mechanics

When a journal entry is composed:
```csharp
string body = JournalVoice.ComposeBody(knowledgeKey, authorBias);
string full = JournalVoice.ComposeFullText(knowledgeKey, authorBias, day);
```

### Resolution Order
1. If the catalog is bound and contains `knowledgeKey`:
   - Evaluates `entry.GetProseForBias(bias)`.
   - If the trait string is populated and non-empty, returns it.
   - If the bias trait is an unknown or unmapped enum value, returns `entry.@default`.
2. Fallback:
   - If the catalog is unbound, or the key is not present in `prose_variants`:
   - Returns `"Something changed. I wrote it down so I would not forget."`.
3. `ComposeFullText` formatting:
   - If `body` already begins with `"Day "`, it is returned unchanged.
   - Otherwise, prepends `"Day {day}. "`:
     ```csharp
     return $"Day {day}. {body}";
     ```

---

## 4. Save/Restore and Mutation Guarantees

- `JournalSystem.CaptureState()` writes rendered `JournalEntry.Text` directly into the save payload (`JournalSave.Entries`).
- Once a journal entry is written to disk in a save slot, its text is **immutable**.
- Adding or modifying entries in `journal_voice_prose.json` will never alter the prose of past, already-recorded journal entries in existing player saves.
- New entries generated during ongoing gameplay will immediately resolve against the updated catalog.

---

## 5. Architectural Invariants

- **Core Invariant 1 (Zero Engine Coupling)**: Neither `JournalVoice`, `JournalVoiceProseCatalog`, nor `JournalSystem` reference `UnityEngine`, `Godot`, or `GodotSharp`.
- **Core Invariant 4 (Determinism)**: Lookup is a pure deterministic dictionary projection based on `(knowledgeKey, RiskBiasTrait)`.
- **Core Invariant 6 (Data Authority)**: The JSON file in `StreamingAssets/Data/` is authoritative. No hardcoded C# string mappings exist in the host layer.
