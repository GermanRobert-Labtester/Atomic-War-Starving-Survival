# LETTER SHARE & CONSEQUENCE AUTHORITY MAP

## 1. Consequence Ownership Architecture
Plan 150 strictly separates document reading from simulation mutations. Most correspondence exists solely for environmental storytelling, memory preservation, and thematic depth.

```
Player Action / Inspection
        │
        ▼
┌──────────────────────────────────────┐
│  PersonalLetterCatalog / Projection  │ ── (Pure Read-Only Projection)
└──────────────────┬───────────────────┘
                   │
                   ▼
┌──────────────────────────────────────┐
│  JournalSystem (Knowledge Ledger)   │ ── Marks KnowledgeKey (e.g. narrative_discovered_*)
└──────────────────┬───────────────────┘
                   │
                   ▼ (Optional Typed Action Only)
┌──────────────────────────────────────┐
│  LetterDeliverySystem                │ ── Transitions state: Found -> Delivered / Withheld
└──────────────────┬───────────────────┘
                   │
                   ▼
┌──────────────────────────────────────┐
│  Authoritative Simulation Systems    │ ── (Bounded, typed consequence, e.g. MoraleSystem)
└──────────────────────────────────────┘
```

---

## 2. Authority Domain Rules

| Domain | Authority Owner | Letter Integration Rule | Prohibited Actions |
|---|---|---|---|
| **Discovery State** | `JournalSystem` / `KnowledgeBase` | `KnowledgeKeys.NarrativeDiscovered(letterId)` | Never write raw prose or full letters to save files. |
| **Delivery & Withholding** | `LetterDeliverySystem` | Transitions `LetterDeliveryState` (`Found`, `Addressed`, `Delivered`, `Withheld`). | Never allow repeatable delivery rewards; one-shot state transitions only. |
| **Survivor Morale** | `SurvivorNeedsSystem` / `LetterDeliverySystem` | Bounded morale adjustment (+6f on delivery, -3f on withholding) only when player explicitly commits an action. | Never grant morale points automatically simply for reading sad or hopeful text. |
| **Pantry & Food Stocks** | `PantrySystem` / `HoldfastInventory` | Authoritative stock counters. Letters complaining of low flour (`letter_03`, `letter_08`) do NOT deduct rations. | Never let narrative prose decrement inventory resources. |
| **Vital State & Death** | `StartingLevelSystem` / `SurvivorCatalog` | Real survivor health, radiation, and trauma meters. Mention of death in `letter_18` does NOT kill live characters. | Never execute `KillSurvivor` or alter life state from letter text. |
| **Faction Standing** | `FactionReputationSystem` | Faction standing matrices. | Never modify faction favor from finding letters. |
| **Map & Keys** | `WorldMapSystem` / `Inventory` | Real key items and locations. Mentions of keys in `letter_02` do NOT place physical keys in player inventory. | Never unlock locations or grant items without explicit gameplay primitives. |

---

## 3. Vertically Sliced Consequence Actions
For the five vertical slice letters:
1. `letter_01_to_mother`: Context & knowledge discovery only. No mechanical mutation.
2. `letter_04_to_lover_returning` & `letter_05_to_lover_gone`: Emotional resonance in bunks. Knowledge discovery only.
3. `letter_08_confession_theft`: Can be discovered in `room_storage_bay`. Unlocks optional dialogue context with Quartermaster Yelena; does not automatically trigger tribunal trial or confiscation.
4. `letter_07_last_letter`: Historical artifact in `room_airlock`. Preserved memory; no survivor state change.
5. `letter_17_to_the_engineer`: Unlocks technical context regarding generator cylinder knock. Cross-referenced in Journal with maintenance logs.
