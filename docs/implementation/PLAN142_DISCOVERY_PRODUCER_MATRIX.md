# Plan 142 Discovery and Producer Matrix

## Activation policy

The corpus catalog is loaded during journal setup, but records are not
automatically inserted. This prevents a fresh campaign from receiving 189
entries, avoids consuming the 64-entry visible cap, and preserves discovery
semantics.

| Producer class | Existing journal call | Plan 142 behavior |
|---|---|---|
| authored-key producer | raw-entry/discovery call uses a catalog knowledge key | adapter supplies authored body, author, stable ID, and source time |
| wasteland map discovery | `WastelandMapSystem.OnNodeDiscovered` → `JournalSystem.TryAddAuthoredEntry` | six location-keyed canonical batch records can land when their real map node is discovered |
| generated gameplay producer | raw-entry call uses a key absent from the corpus | existing text, author, timestamp, sequence ID remain unchanged |
| codex-only evidence | `Unlock*` / `AddKnowledgeEvidence` | unlock behavior remains unchanged; no authored body is injected |
| faction-war content | faction-war catalog/runner | remains on faction-war path; no generic ingestion |
| generated voice | `JournalVoice` | remains trait-driven; never substitutes for authored body |
| ambient record with no producer | no matching current call site | loaded and validated, but deferred until a real owning producer exists |

No row receives an invented item, flag, quest consequence, or world mutation.
Journal insertion remains the only effect.

## No boot flood

`SetupJournal` restores an existing save or runs the existing demo seed. It
does not activate the entire corpus. Catalog loading is read-only and does
not emit `OnEntryAdded`, `OnNotificationPing`, or codex events.
