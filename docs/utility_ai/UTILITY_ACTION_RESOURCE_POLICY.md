# Utility Action Resource Policy

> **Resource Policy:** Guardrails preventing autonomous consumption of scarce or quest-critical items.

---

## 1. Protected Resource Tiers

| Tier | Items / Resources | Policy |
|---|---|---|
| **Tier 1: Quest / Unique** | Stamped letters, sealed canisters, keycards, rare relics | **Strictly Forbidden:** Utility actions can NEVER reserve or consume |
| **Tier 2: Rare Medicine** | Broad-spectrum antibiotics, rad-purge ampoules, surgical kits | **Strict Threshold:** Consumed only for acute lethal conditions, never for minor fatigue or slight bruises |
| **Tier 3: Industrial Stock** | Heavy motors, electronic components, precision solder | **Player Gated:** Only consumed if active repair / construction orders permit |
| **Tier 4: Bulk Consumables** | Raw produce, clean water, firewood, scrap metal, cloth | **Stock-Gated:** Consumed freely up to shelter quota, paused if reserve falls below emergency reserve buffer |

---

## 2. Failed Action Suppression

- If an action fails resource validation (e.g. `action_cook_food` attempts to run but no raw food exists), the subsystem rejects execution.
- The failure suppresses the action for that survivor for the current AI cycle, preventing infinite retry loops and log spam.
