# ASHFALL — Phase 16 Production-Art Priority

**Date:** this phase (turn).
**Source data:** `docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json` (478 actionable rows).
**Scoring model:** `visibility × importance × runtime_readiness × semantic_uniqueness × family_completion ÷ generation_complexity`.

| Band | Count | Meaning |
|---|---|---|
| **P0** | 0 | No P0 synthetically produced. AssetRegistry runtime gates zero-fallback — production pres is intact. |
| **P1** | 163 | Highest visibility / runtime surface. Generated first. |
| **P2** | 199 | High-value content completion. |
| **P3** | 110 | Secondary / expansion. |
| **P4** | 6   | Future / low-frequency. |

`P0` is intentionally **not populated** — the production runtime still has zero fallback activations (verified by `--asset-registry-selftest`: 48/48 PASS, 0 missing, 0 failed-to-load). There is no fabricated urgency.

---

## Phase 16 status

- Quarantine confirmed: 21 evidence-strong deprecated ammo files moved to `assets/_quarantine_legacy/`.
- Image generation: **BLOCKED_EXTERNAL_AUTH** — api-key rejected by platform (`API key status is not active`).
- AssetRegistry selftest: 48/48 PASS (0 missing, 0 load-failed).
- Data integrity selftest: 0 errors / 0 warnings across 94 catalogs.
- Bridge selftest: 41/41 PASS.
- Core test suite: 1985 / 1985 PASS (up from 1973 → +12 new tests since Phase 14).
- Godot build: 0 errors / 0 warnings.
- `--ui-snapshot-uitest`: blocked (headless dummy renderer can't `Texture2D.GetImage()`) — pre-existing limitation.

---

## P1 — Required immediately for active gameplay

The 163 P1 rows split across:

| Family | P1 count | Notes |
|---|---|---|
| Inventory-Item (Medical) | 7 | Trauma, radiation, food are frequent HUD refreshes. |
| Inventory-Item (Ammunition) | 1 | Caliber-specific cartridges enter the salvage panel. |
| Inventory-Item (Weapons) | 0 | Done elsewhere. |
| Inventory-Item (Crafting-Material) | 9 | Survival workstation recipe panel. |
| Inventory-Item (Food-Water) | 26 | Needs HUD level icons / Wellbeing strip. |
| Inventory-Item (Equipment) | 3 | Equipped gear panel. |
| Inventory-Item (Other) | 17 | Documents, keys, lore items. |
| Location-Art | 65 | Location panel hero shots. |
| NPC-Portrait | 35 | NPC party roster. |
| Survivor-Portrait | 2 | Family adult + child. |
| Faction-Art | 0 | None in P1. |

The Inventory-Item P1 entries are the missing tails after Phase 11 resolved the canonical-survivor roster.

---

## P2 — High-value content completion

199 rows across Inventory-Item (174 Other misc.), Location-Art (174), and one Faction-Art. Locations are routed through the **Expedition** and **Holdfast** panels at navigation moments; these become natural Phase 16+ targets.

---

## P3 — Secondary / expansion

110 rows of low-engagement-rate expansion content (`year_of_ash_locations`, `holdfast_locations`, expansion-portraits).

---

## P4 — Future / low-frequency

6 rows are reference-only or pending decommissioning. Documented but not generated unless the corresponding gameplay feature activates.

---

## Recommended first batch (24–36 assets)

When image generation is available, the first batch should be **demonstrative**: pick small, well-clustered families whose style anchors are most easily satisfied.

### Phase 17 — Runtime-surfaced priority

The Phase 17 runtime-context trace (`docs/visual/RUNTIME_CONTEXT_TRACE.md`, phase 17 section) cross-references the AssetRegistrySelfTest's top-N sliding window against the actionable manifest. The result: **only 39 of the 478 actionable rows are surfaced by the top-N runtime probe today**.

| Category | Catalog total | Top-N in manifest | Surfaced actionable |
|---|---|---|---|
| items | 499 | 0 | 0 (top-N items all have art) |
| survivors | 102 | 0 | 0 (top-N survivor IDs are not in the manifest) |
| locations | 105 | 3 | 3 |
| characters | 36 | 36 | 36 |
| **TOTAL** | | | **39** |

The 36 character rows are the *only* family where the top-N runtime probe overlaps with the actionable manifest, because the canonical survivor roster (Phase 11) already covered the survivor IDs. The 3 location rows are:

- `loc_grange_hall` (P1)
- `loc_apiary_rows` (P1)
- `loc_seed_library_anex` (P1)

### Recommended first batch (39 assets — runtime-surfaced only)

| Rank | ID | Family | Reason |
|---|---|---|---|
| 1 | `npc_bram_ostrowski` | NPC-Portrait | P1, top-N surfaced |
| 2 | `npc_sergeant_pell` | NPC-Portrait | P1, top-N surfaced |
| 3 | `npc_doctor_ianov` | NPC-Portrait | P1, top-N surfaced |
| 4 | `npc_wren` | NPC-Portrait | P1, top-N surfaced |
| 5 | `npc_kestrel` | NPC-Portrait | P2, top-N surfaced |
| 6 | `npc_nomi_fisk` | NPC-Portrait | P1, top-N surfaced |
| 7 | `npc_ivor_lasko` | NPC-Portrait | P1, top-N surfaced |
| 8 | `npc_the_cartwright_sisters` | NPC-Portrait | P1, top-N surfaced |
| 9 | `npc_edor_vale` | NPC-Portrait | P1, top-N surfaced |
| 10 | `npc_yara_holm` | NPC-Portrait | P1, top-N surfaced |
| 11 | `npc_leva_quist` | NPC-Portrait | P1, top-N surfaced |
| 12 | `npc_cael_ormund` | NPC-Portrait | P1, top-N surfaced |
| 13 | `npc_halden_mire` | NPC-Portrait | P1, top-N surfaced |
| 14 | `npc_cluster_teacher` | NPC-Portrait | P1, top-N surfaced |
| 15 | `npc_osran_kell` | NPC-Portrait | P1, top-N surfaced |
| 16 | `npc_mattis_cray` | NPC-Portrait | P1, top-N surfaced |
| 17 | `npc_wyn_sabler` | NPC-Portrait | P1, top-N surfaced |
| 18 | `npc_dessa_vane` | NPC-Portrait | P1, top-N surfaced |
| 19 | `npc_perrin_ashby` | NPC-Portrait | P1, top-N surfaced |
| 20 | `npc_ivo_fenn` | NPC-Portrait | P1, top-N surfaced |
| 21 | `npc_kess_adler` | NPC-Portrait | P1, top-N surfaced |
| 22 | `npc_ansel_duth` | NPC-Portrait | P1, top-N surfaced |
| 23 | `npc_tamsin_rook` | NPC-Portrait | P1, top-N surfaced |
| 24 | `npc_len_quill` | NPC-Portrait | P1, top-N surfaced |
| 25 | `npc_hadi_morrow` | NPC-Portrait | P1, top-N surfaced |
| 26 | `npc_nila_brant` | NPC-Portrait | P1, top-N surfaced |
| 27 | `npc_maren_holt` | NPC-Portrait | P1, top-N surfaced |
| 28 | `npc_ira_vell` | NPC-Portrait | P1, top-N surfaced |
| 29 | `npc_benno_kade` | NPC-Portrait | P1, top-N surfaced |
| 30 | `npc_quil_esser` | NPC-Portrait | P1, top-N surfaced |
| 31 | `npc_osric_tann` | NPC-Portrait | P1, top-N surfaced |
| 32 | `npc_dara_mewn` | NPC-Portrait | P1, top-N surfaced |
| 33 | `npc_dr_irina_vel` | NPC-Portrait | P1, top-N surfaced |
| 34 | `npc_wyn_omah` | NPC-Portrait | P1, top-N surfaced |
| 35 | `npc_piet_abar` | NPC-Portrait | P1, top-N surfaced |
| 36 | `npc_saria_voss` | NPC-Portrait | P1, top-N surfaced |
| 37 | `loc_grange_hall` | Location-Art | P1, top-N surfaced |
| 38 | `loc_apiary_rows` | Location-Art | P1, top-N surfaced |
| 39 | `loc_seed_library_annex` | Location-Art | P1, top-N surfaced |

This batch is the **only** family-cohesive cluster that maps to runtime-visible content today. The remaining 437 actionable rows are deep-tail content that reaches the runtime only after deep gameplay progression — they will fall into Batch 2/3 once the pipeline is proven stable on this batch.

### Why the inventory rows are not in Batch 1

Phase 11 already resolved the canonical inventory list (the 499 items.json IDs), so the top-N items at runtime all have art. This is **a Phase 11 success**, not a Phase 17 failure. The narrative items added by the Year of Ash / Holdfast / Crossing expansions are the 233 actionable Inventory-Item rows — they appear in-game only when the player has progressed into the relevant expansion; they are not "first-paint" content.

### Why the survivor rows are not in Batch 1

Same reason: the survivor roster is canonical (Phase 11). The two P1 survivor rows (`survivor_family_adult`, `survivor_family_child`) are intentionally not in the top-N because they are template placeholders for narrative events, not the named-survivor runtime roster.

---

## Risk model

1. The chosen model returns a *variant* of an existing asset family but with subtly wrong palette.
2. The model adds visible text / watermarks / AI signs in 5-10% of outputs.
3. The first batch's anchor assets have already-perceptually-distinct sub-families that could confuse the prompt-engine.

Mitigation:
- Family-cohesive first batch (above) reduces prompt-engine confusion.
- QA per batch before promotion.
- Per-family halt if >20% failures.

---

## Wait state

Image generation is **BLOCKED_EXTERNAL_AUTH** in this environment. The first batch is ready and validated against the AssetRegistry; it will fire automatically when an authenticated `arkcli +gen` is wired.

If the executor cannot wire auth within a reasonable window, the next-best path is to **manually** produce a piloted 6-asset batch using the existing accepted asset family as a *template*, saving under the canonical target filename, and rerun the wiring trace + gallery afterwards.
