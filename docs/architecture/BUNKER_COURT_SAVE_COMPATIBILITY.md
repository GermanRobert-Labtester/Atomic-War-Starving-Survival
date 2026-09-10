# Bunker Court Save Compatibility & Persistence Contract

**Document ID:** ARCH-BUNKER-COURT-SAVE-COMPATIBILITY
**Status:** Approved Architectural Specification
**Project:** ASHFALL (Godot 4.7+ .NET 8 Host / C# Core)
**Date:** 2026-09-09

---

## 1. Zero Save Bloat Invariant

A central tenet of Plan 146 is **zero save bloat**. The runtime activation of all 24 bunker tribunal court records introduces:
- **0** new save files.
- **0** new save sections or envelopes in `SaveStoreHub` or `SaveSectionRegistry`.
- **0** schema version increments.
- **0** byte overhead for uninspected or undiscovered records.

---

## 2. Persistence Delegation to Journal Knowledge Base

Discovery of court records leverages ASHFALL's existing, battle-tested `JournalSaveStore` via `KnowledgeBase`:

```
Discovery ID: disc_court_{slug}
Knowledge Key: narrative_disc_disc_court_{slug}
```

When a player discovers a case through a bunker terminal, `JournalSystem.UnlockNarrativeDiscovered(discoveryId)` is invoked, which records the string key into the campaign's `_knowledge` set.

### Invariant Checks:
1. **Existing Save Compatibility:** Any existing save file (prior to Plan 146) simply lacks the `narrative_disc_disc_court_*` keys. Upon loading in the updated build, those cases remain "undiscovered" in the codex until found by the player. No deserialization errors or schema warnings are triggered.
2. **Forward Save Compatibility:** Saves made with Plan 146 will simply contain the string keys in `journal_save.json`. If loaded by an older build, unknown keys are preserved in the knowledge list without error.
3. **No Retroactive State Changes:** Loading a save with discovered court cases executes **zero** penalty evaluations, inventory deductions, or morale penalties.
