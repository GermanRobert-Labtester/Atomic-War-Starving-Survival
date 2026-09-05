# Plan 100 — Moral Choice Faction Reactions Baseline & Forensics

## 1. Executive Summary

Plan 100 expands the world-response layer for ASHFALL's invisible moral choice system (`MoralChoiceSystem.cs`). Rather than surfacing a numeric morality bar to the player, the wasteland responds socially through NPC faction dialogues, outpost notices, and journal entries.

This document records the baseline state, architectural discoveries, and trigger classifications established during Phase 0 reconnaissance.

---

## 2. Catalog & Schema Authority

- **File Path**: `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json`
- **Schema Version**: `1`
- **Catalog Loader**: `Ashfall.Core.MoralChoice.MoralChoiceFactionReactionsCatalogLoader`
- **Data Wire Model**: `Ashfall.Core.MoralChoice.MoralChoiceFactionReactionsData`
- **Container Structure**:
  - `schema_version`: integer (`1`)
  - `description`: top-level summary
  - `threshold_reactions`: Dictionary mapping string `event_id` to `MoralThresholdReactionRecord`
- **Record Structure**:
  - `event_description`: summary of firing trigger
  - `peacekeeper_dialogue`: list of dialogue blocks
  - `raider_dialogue`: list of dialogue blocks
  - `knowledge_keeper_dialogue`: list of dialogue blocks
  - `civilian_dialogue`: optional list of dialogue blocks
  - `journal_entry`: player-voice journal reflection

---

## 3. Trigger Classification (Phase 0 Audit)

The roadmap originally hypothesized milestones (`first_mercy`, `first_betrayal`) and history counts (`neutral_drift`) as threshold reactions. Forensic examination of `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` revealed the actual runtime trigger architecture:

- **Classification**: **Trigger Case T1 — Band Crossings & Overflow Settlement**
- **Mechanism**:
  1. During daily quest resolution, `moralDelta` accumulates into `moralScore` (clamped between `-200` and `+200`).
  2. Score overflow flags (`LegendPositiveFlag`, `LegendNegativeFlag`) are stored in `pendingLegendFlags`.
  3. During overnight settlement (`MoralChoiceSystem.Reconcile(int day)`):
     - Pending legend flags trigger `moral_event_legend_positive` or `moral_event_legend_negative`.
     - Discrete moral bands are checked against `bandAtLastReconcile`.
     - Each traversed boundary fires its one-time threshold event via `FireBandEvents`.
     - Fired events are committed to `state.firedThresholdEvents` and will never refire.

---

## 4. Canonical Six-Event Roster

| Event ID | Firing Trigger | Band / Condition | Primary Narrative Theme |
|---|---|---|---|
| `moral_event_bounty_issued` | `MoralChoiceSystem.Reconcile` | `MoralPathBand.VeryEvil` (Score <= -100) | Law enforcement bounty posted; shoot-on-sight warnings. |
| `moral_event_contract_taken` | `MoralChoiceSystem.Reconcile` | `MoralPathBand.Positive` (Score 50..99) | Formal Peacekeeper partnership and trade escort. |
| `moral_event_contract_raised` | `MoralChoiceSystem.Reconcile` | `MoralPathBand.VeryPositive` (Score >= 100) | Full regional logistics support; recognized leader. |
| `moral_event_patrol_defense` | `MoralChoiceSystem.Reconcile` | `MoralPathBand.VeryPositive` (Score >= 100) | Perimeter sentries stationed outside player shelter. |
| `moral_event_legend_positive` | `MoralChoiceSystem.Reconcile` | Score Overflow > +200 | Wasteland mythologizes player; children named in honor. |
| `moral_event_legend_negative` | `MoralChoiceSystem.Reconcile` | Score Overflow < -200 | Player treated as existential monster / bogeyman. |

---

## 5. Faction Dialogue Coverage & Remediation

Prior to Plan 100, only the first three events contained full three-faction dialogue. The expansion completed all missing faction surfaces:
1. `moral_event_patrol_defense`: Authored `raider_dialogue` (ridge scout assessing the perimeter) and `knowledge_keeper_dialogue` (surveyor logging the permanent node).
2. `moral_event_legend_positive`: Authored `raider_dialogue` (caravan broker noting untouchable supply lines) and `knowledge_keeper_dialogue` (Chief Chronicler opening a dedicated historical volume).
3. `moral_event_legend_negative`: Authored `knowledge_keeper_dialogue` (Chief Chronicler entering the atrocities into the Red Cartulary and barring archive vaults).
