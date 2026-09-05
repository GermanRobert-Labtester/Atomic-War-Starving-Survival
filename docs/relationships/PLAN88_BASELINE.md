# Plan 88 — Baseline Discovery & Forensic Reconciliation

> **Subsystem:** Confession & Secret System (`Assets/Ashfall.Core/Phantoms/`)
> **Data Authority:** `Assets/StreamingAssets/Data/confession_secrets.json`
> **Consumer Systems:** `ConfessionSecretCatalog`, `ConfessionSecretSystem`, `SurvivorRelationsSystem`, `GuiltInsomniaSystem`, `MoralBranchingSystem`

---

## 1. Executive Summary

Plan 88 requested expanding `confession_secrets.json` from a supposed 8-entry baseline to 20 confession secrets covering 12 specific survivor archetypes (`the_engineer`, `the_nurse`, `the_cook`, `the_farmer`, `the_priest`, `the_journalist`, `the_pilot`, `the_scientist`, `the_carpenter`, `the_child`, `the_old_man`, `the_hunter`).

Forensic analysis of the live repository revealed an evolved reality:
1. **Catalog State Prior to Plan 88:** `confession_secrets.json` had 26 entries authored across three categories:
   - **8 Original Personal Secrets:** `the_surgeon`, `the_soldier`, `the_pharmacist`, `the_mother`, `the_mechanic`, `the_teacher`, `the_refugee`, `the_electrician` (entries 1–8).
   - **8 Expansion Personal Secrets:** `the_cook`, `the_engineer`, `the_farmer`, `the_priest`, `the_journalist`, `the_pilot`, `the_scientist`, `the_hunter` (entries 9–16).
   - **6 Faction Institutional Secrets:** `faction_independent`, `faction_military`, `faction_rebel`, `faction_iron_clique`, `faction_meridian`, `faction_order` (entries 17–22).
   - **4 Bunker Internal Secrets:** `the_quartermaster`, `the_overseer`, `the_pharmacist` (morphine cache), `the_guard` (entries 23–26).
2. **Active Test Assertion:** `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:42` pinned `catalog.AllSecrets.Count >= 26` and explicitly asserted faction and personal secrets. Shrinking the file to 20 total records would have broken existing tests.
3. **Archetype Delta:** Of the 12 requested archetypes in Plan 88, exactly 8 were already authored in entries 9–16. Exactly 4 remained unauthored:
   - `the_nurse` (Task 88N)
   - `the_carpenter` (Task 88U)
   - `the_child` (Task 88V)
   - `the_old_man` (Task 88W)
4. **Target Reconciliation:** Authoring the 4 missing archetypes brings the personal survivor confession count from 16 to **exactly 20 personal survivor confession secrets** (8 baseline + 12 requested). Total records in `confession_secrets.json` becomes 30 (20 personal + 6 faction + 4 bunker), maintaining 100% backward compatibility and fulfilling every requested archetype and story prompt.

---

## 2. Consuming Systems Architecture

```
                                 [confession_secrets.json]
                                             │
                                             ▼
                                 ConfessionSecretCatalog
                                             │
                                             ▼
                                 ConfessionSecretSystem
                     ┌───────────────────────┼────────────────────────┐
                     │                       │                        │
                     ▼                       ▼                        ▼
           ResolveInterpersonal()      ExposeSecret()           BlackmailSecret()
          (Forgive vs Grudge)     (Faction Standing / Guilt)  (Moral Hardening / Loot)
                     │                       │                        │
                     ▼                       ▼                        ▼
           SurvivorRelationsSystem        FlagLedger             MoralBranchingSystem
             (Affinity & Trust)      GuiltInsomniaSystem         (Numbed Resilience)
                     │
                     ▼
                NeedsSystem
                 (Morale)
```

### Runtime Contracts
- **Interpersonal Resolution:** `ConfessionSecretSystem.ResolveInterpersonal(secretId, currentDay, forgive, confessorId, listenerId, relations, needs)`
  - If `forgive == true`: modifies affinity by `forgiveness_affinity`, awards trust (+15f), updates confessor/listener morale via `forgiveness_morale`.
  - If `forgive == false`: modifies affinity by `grudge_affinity`, penalizes morale by `grudge_morale`.
- **Moral Leverage Paths:**
  - `ExposeSecret()`: increases/decreases faction standing via callback, adds guilt source to `GuiltInsomniaSystem`.
  - `BlackmailSecret()`: increases moral hardening (`NumbedResilienceLevel`) via `MoralBranchingSystem`.
  - `KeepSecret()`: grants substantial trust to the confidant via `SurvivorRelationsSystem`.
- **Idempotence:** Discovered and resolved state is tracked in `ConfessionSecretState` (`discoveredSecretIds`, `resolvedSecretIds`, `leverageChoices`) and preserved across saves.

---

## 3. Archetype Resolution Audit

| Archetype Concept | Canonical ID | Presence in `survivors.json` | Presence in `final_wishes.json` | Discovery Source Item |
|---|---|---|---|---|
| Surgeon | `the_surgeon` | Yes (`the_surgeon`, line 26) | Yes (line 5) | `silver_scalpel` |
| Soldier | `the_soldier` | Yes (`the_veteran`, line 74) | Yes (line 30) | `dog_tags` |
| Pharmacist | `the_pharmacist` | Yes (`the_pharmacist`, line 38) | No | `morphine` |
| Mother | `the_mother` | Yes (`the_fierce_mother`, line 335) | Yes (line 83) | `childs_mitten` |
| Mechanic | `the_mechanic` | Yes (`the_mechanic`, line 146) | Yes (line 106) | `mechanic_gloves` |
| Teacher | `the_teacher` | Yes (`the_teacher`, line 209) | Yes (line 126) | `pocket_notebook` |
| Refugee | `the_refugee` | Yes (`the_parent` / refugee slice) | Yes (line 149) | `undelivered_mail` |
| Electrician | `the_electrician` | Yes (`the_electrician`, line 128) | Yes (line 164) | `engineers_slide_rule` |
| Cook | `the_cook` | Yes (`the_chef`, line 254; `reese_flores`) | Yes (line 483) | `recipe_tin` |
| Engineer | `the_engineer` | Yes (`the_tech_bro`, line 714; `aris_thorne`) | No | `engineers_slide_rule` |
| Farmer | `the_farmer` | Yes (`taylor_morgan`, line 1072) | Yes (line 363) | `family_heirloom_seeds` |
| Priest | `the_priest` | Yes (`the_priest`, line 227) | Yes (`the_preacher`, line 304) | `pocket_notebook` |
| Journalist | `the_journalist` | Yes (`the_reporter`, line 236; `the_news_anchor`) | Yes (line 326) | `undelivered_mail` |
| Pilot | `the_pilot` | Yes (`survivor_drone_op`, line 886) | No | `tarnished_medal` |
| Scientist | `the_scientist` | Yes (`the_chemist`, line 155; `the_botanist`) | No | `dosimeter` |
| Hunter | `the_hunter` | Yes (`the_hunter`, line 101; `blake_sullivan`) | Yes (line 222) | `engraved_lighter` |
| Nurse | `the_nurse` | Yes (`ariana_cruz`, line 1143) | Yes (line 60) | `nurse_fob_watch` |
| Carpenter | `the_carpenter` | Yes (`elliot_bennett`, line 1206) | Yes (line 244) | `box_of_nails_10` |
| Child | `the_child` | Yes (`the_child_soldier`, line 485; `the_feral_orphan`) | Yes (`teach_the_children`) | `childs_drawing` |
| Old Man / Elder | `the_old_man` | Yes (`the_sheriff`, line 690; `the_veteran`) | Yes (`the_neighbor`, line 404) | `tarnished_pocket_watch` |

All 20 archetypes cleanly map to established survivor archetypes and canonical data items without creating phantom dependencies.
