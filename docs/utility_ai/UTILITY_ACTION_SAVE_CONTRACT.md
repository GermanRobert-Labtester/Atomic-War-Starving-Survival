# Utility Action Save Contract

> **Persistence Contract:** State boundaries and backward compatibility for Utility AI actions.

---

## 1. Stateless AI Core

- As documented in `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` (Audit A6):
  `Stateless: no save state exists; contexts are per-call host data.`
- `UtilityAiSystem` does NOT maintain a separate on-disk save file or persistent state envelope.
- Selected action IDs are consumed immediately by survivor task executives or host sessions (`AIActionContext` is passed per-call).

---

## 2. Backward Compatibility with Existing Saves

- Saves storing survivor state (such as `SurvivorsSaveStore`) record current task/activity strings.
- Expanding `utility_actions.json` from 6 to 20 actions introduces zero breaking changes to existing saves.
- Old saves continue to load cleanly, and newly loaded games immediately have access to all 20 actions for selection.
