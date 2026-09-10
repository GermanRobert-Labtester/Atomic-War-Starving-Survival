# PLAN 150 SAVE COMPATIBILITY

## 1. Save System Guarantees
Plan 150 activates `letters_expansion.json` without introducing new save stores or altering save file schemas.

### Key Invariants
1. **Zero New Save Stores**: No new save file (e.g. `LetterSaveStore`) is created.
2. **Knowledge Persistence**: Discovered letters are recorded as string keys in the existing `JournalSystem` knowledge ledger:
   `KnowledgeKeys.NarrativeDiscovered(letterId)`
   This ledger is already versioned, checksummed, and tested in `JournalSaveStore`.
3. **No Save Bloat**: Full letter prose is never saved to the player's disk. Only lightweight ID keys are stored.
4. **No Retroactive Mutations**: Loading an existing save file into a Plan 150 build does not retroactively alter survivor traits, inventory, or relationship meters.
5. **Rollback Safe**: If a campaign save made under Plan 150 is loaded into an earlier version of the binary, unrecognized knowledge keys are safely ignored without schema errors.

---

## 2. Checksum & Determinism
- `SaveChecksum` hashes are completely unaffected because no fields were added to any serializable save DTOs.
- `LetterDeliverySystem` state capture and restore mechanisms continue to pass their pinned unit tests (`LetterDeliverySystemTests`).
