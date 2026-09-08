# Plan 137 — Forensic Baseline & Catalog Inventory

## 1. Executive Summary
Plan 137 activates the authored survivor enrichment layer across ASHFALL so that survivors possess deep, persistent narrative identities (beliefs, pre-war professions, personal keepsakes, phantom backgrounds, and philosophical stances) without modifying gameplay stats or compromising determinism and save invariants.

This document inventories the baseline state of all source catalogs, schemas, data references, and initial consumer seams prior to and during activation.

---

## 2. Catalog & Data Authority Inventory

### 2.1 Primary Catalogs
| Catalog File | Path | Total Records | Schema Version | Role |
|---|---|---|---|---|
| `expansion_survivor_fields.json` | `Assets/StreamingAssets/Data/expansion_survivor_fields.json` | 72 | 1 | Baseline survivor enrichment (beliefs, professions, keepsakes, phantoms) |
| `expansion_item_tags.json` | `Assets/StreamingAssets/Data/expansion_item_tags.json` | 115 | 1 | Narrative item tagging (keepsake candidates, phantom triggers, restorable photos) |
| `deep_lore_survivor_fields.json` | `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` | 4 | 1 | Deep lore specialist overlay (philosophical stance, unique profiles) |
| `antigravity_survivor_fields.json` | `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` | 11 | 1 | Specialist archetype overlay (stance, manifesto law codes) |
| `survivors.json` | `Assets/StreamingAssets/Data/survivors.json` | 76 | 1 | Authoritative survivor roster definitions |

### 2.2 Forensic ID Reconciliation
- **Baseline Coverage:** 72 records in `expansion_survivor_fields.json`.
  - **Resolution Rate:** 72/72 (100%) resolve directly to canonical IDs in `survivors.json`.
  - **Unresolved / Orphan IDs:** 0.
- **Deep Lore Coverage:** 4 records (`aris_thorne`, `maya_lin`, `victor_vance`, `elena_rostov`).
  - **Resolution Rate:** 4/4 (100%) resolve to `survivors.json`.
  - **Overlap with Baseline:** 0 records (pure disjoint partition completing the 76 survivors in `survivors.json`).
- **Archetype / Specialist Overlay:** 11 records in `antigravity_survivor_fields.json`.
  - Maps specialized archetypes (`the_veteran`, `the_surgeon`, etc.) supplying `stance` and `manifesto_law_code`.
  - Safely merged into matching survivor IDs without overwriting core baseline fields.

---

## 3. Dimensional Field Analysis

### 3.1 Belief Profiles (7 Distinct Profiles)
The enrichment layer defines 7 distinct belief profiles reflecting human psychology in an irradiated post-nuclear world:
1. `atheist_rationalist` — Rejection of superstition; reliance on physical evidence and empirical truth.
2. `collectivist_solidarity` — Prioritization of commune survival, mutual aid, and egalitarian burden-sharing.
3. `military_discipline` — Adherence to chain of command, operational hierarchy, and procedural order.
4. `pacifist` — Absolute aversion to violence, execution, and unnecessary bloodshed.
5. `pragmatic_individualism` — Focus on personal competence, self-reliance, and transactional alliances.
6. `religious_faith` — Traditional theological prayer, providence, and divine penitence amidst the ash.
7. `superstitious_traditional` — Omens, rituals, taboos, and folk beliefs regarding fallout and the dead.

### 3.2 Pre-War Professions
- Explicitly defined in `expansion_survivor_fields.json` (4 distinct categories across 21 survivors):
  - `nurse` (7 survivors)
  - `machinist` (6 survivors)
  - `electrician` (4 survivors)
  - `teacher` (4 survivors)
- Unassigned / Deferred: 51 survivors have empty `pre_war_profession_id` in the expansion file and cleanly defer to the canonical `def.profession` field in `survivors.json`.

### 3.3 Phantom Backgrounds (9 Distinct Keys)
Used by `PhantomMemoryHostSession` to contextualize memory triggers, trauma flashpoints, and haunting manifestations:
1. `child_refugee`
2. `driver`
3. `electrician`
4. `former_soldier`
5. `generic`
6. `machinist`
7. `miner`
8. `nurse`
9. `teacher`

### 3.4 Personal Keepsakes
- **Total Unique Keepsakes Referenced:** 72 personal keepsake identifiers.
- **Physical In-Game Items:** 8 keepsakes correspond to standard tangible item definitions registered in item catalogs (`worn_stethoscope`, `tarnished_pocket_watch`, `family_heirloom_seeds`, `silver_scalpel`, `childs_drawing`, `teddy_bear`, `wedding_ring`, `pipe_wrench`).
- **Narrative Personal Keepsakes:** 64 keepsakes represent narrative heirlooms, tokens, letters, and mementos preserved across survivor journals and memory systems.

### 3.5 Item Tags (`expansion_item_tags.json`)
- **Total Tagged Items:** 115 items.
- **Total Unique Tag Tokens:** 33 distinct narrative markers.
- **High-Frequency Tags:**
  - `personal_keepsake_candidate` (51 items)
  - `pre_war_artifact` (34 items)
  - `phantom_memory_trigger` (28 items)
  - `restorable_photograph` (12 items)
  - `heirloom` (10 items)

---

## 4. Architectural Invariants Preserved
1. **Zero Engine Coupling in Core:** All enrichment domain models (`ExpansionEnrichmentCatalog`, `SurvivorEnrichmentService`, `SurvivorEnrichmentView`, `ItemInspectionModel`) reside in `Assets/Ashfall.Core/` with zero references to Godot or Unity.
2. **Stat Immutability:** Pre-war professions and belief profiles provide contextual narrative depth, dialog flavor, and ideological friction. They do not inject hidden combat multipliers, HP buffs, or sanity modifiers.
3. **Keepsake Association vs. Possession:** Keepsake metadata defines emotional connection, never automatic item grants. Inventory presence is evaluated dynamically at runtime.
4. **Deterministic Precedence:** Baseline data is authoritative; specialist overlays contribute non-conflicting supplemental attributes via deterministic field merging.
