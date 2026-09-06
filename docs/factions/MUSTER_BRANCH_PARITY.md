# MUSTER FACTION BRANCH PARITY REPORT
## Alignment, Recruitment Profiles, and Doctrine Harmonization

**Document Version:** 1.0.0
**Domain:** Faction Warfare / The Weight of Choices
**Catalogs Covered:** `military_faction_branch.json`, `rebel_faction_branch.json`, `independent_faction_branch.json`

---

## 1. Catalog Schema Parity

All three faction branch catalogs share an identical structural contract:
- `id` (canonical snake_case string)
- `display_name` (localized title)
- `ponr_flag` (unique point-of-no-return progression flag)
- `ponr_trigger` (narrative event boundary)
- `entry_band_min` / `entry_band_max` (moral path constraints)
- `endings` (nested array of morality-gated conclusion keys)

### 1.1 Branch Distribution Summary

| Faction Catalog | Total Branches | Morality Range Covered | PRPF / External Requirements |
|---|---|---|---|
| **Military** (`military_faction_branch.json`) | 8 Branches | `very_evil` to `very_positive` | Unaligned / Standard Military Command |
| **Rebel** (`rebel_faction_branch.json`) | 8 Branches | `very_evil` to `very_positive` | Unaligned / Insurgency Underground |
| **Independent** (`independent_faction_branch.json`) | 8 Branches | `very_evil` to `very_positive` | PRPF Standing & Dual Hostility Checks |

---

## 2. Warfare & Rally Parity Profile

When mobilizing a Muster force under each faction branch, the following recruitment, supply, and tactical profiles apply:

| Branch Archetype | Manpower Need | Munitions Need | Rations Need | Preferred Tactical Doctrine | Escalation Impact | Recovery Time |
|---|---|---|---|---|---|---|
| **Military (Loyalists)** | High (4–6) | High (5.56mm / Heavy) | Medium (Canned Food) | `warlord_doctrine_procedure` | +15 Tension | 3 Days |
| **Military (Defectors)** | Medium (3–4) | Medium (Mixed) | Low (Scavenged) | `warlord_doctrine_consolidation` | +10 Tension | 2 Days |
| **Rebel (Insurgents)** | Low (2–3) | Medium (Guerilla) | High (Water/Meds) | `warlord_doctrine_annexation` | +25 Tension | 1 Day |
| **Rebel (Vanguard)** | Medium (3–5) | High (IEDs / Heavy) | Medium (Rations) | `warlord_doctrine_toll` | +20 Tension | 2 Days |
| **Independent (Mercenary)**| Flexible (2–6) | Paid (Trade Chits) | Paid (Barter Goods) | `warlord_doctrine_traffic` | +5 Tension | 1 Day |
| **Independent (Sanctuary)**| Defensive (2–4)| Low (Sidearms) | High (Herbal/Clinic) | `warlord_doctrine_withdrawal` | -10 Tension | 4 Days |

---

## 3. Exclusion and Commitment Invariants

1. **Strict Mutual Exclusivity:** Committing to any branch in one faction permanently locks out all branches in competing factions.
2. **Zero Ghost Commitment:** A branch cannot be mustered unless the player has passed the moral band requirement and committed to the branch in `FactionBranchCoordinator`.
3. **No String Switching:** Code references branch kinds via the strongly-typed `FactionBranchKind` enum, never through string inspection.
