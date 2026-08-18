# ASHFALL — Phase 16 Production-Art Generation Ledger

**Date:** this phase.
**Source data:** `docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json` (478 actionable rows).

## Status counts at ledger-start

| Status | Count |
|---|---|
| `PENDING` | 478 |
| `SKIP_REFERENCE_ONLY` | 136 |
| `TOTAL` | 614 |

## Phase 16 promotion summary

| Status | Count |
|---|---|
| `PROMOTED` | **0** |
| `TECHNICAL_QA_PASS` | 0 |
| `SEMANTIC_QA_PASS` | 0 |
| `STYLE_QA_PASS` | 0 |
| `DUPLICATE_QA_PASS` | 0 |
| `RUNTIME_SIZE_PASS` | 0 |
| `REGISTRY_RESOLVED` | 0 |
| `GALLERY_VERIFIED` | 0 |
| `RUNTIME_CONTEXT_VERIFIED` | 0 |
| `REJECTED` | 0 |
| `REGENERATED` | 0 |
| `SKIP_EXISTING` | 0 |

## Phase 16 generation status

**Image generation: BLOCKED_EXTERNAL_AUTH** — `arkcli +gen` returned
`API key status is not active` for every model identity tested (seedream-4-0-250828, seedream-3-0-t2i-250415, nano-banana-pro, doubao-1-5-vision-pro-32k-250115, gemini-2.5-flash-image-preview).

The structural pipeline is **all-green** — pipeline, queue, prompt templates, family reference packs, QA harness, list of staging paths, wiring trace, gallery regeneration, runtime-context trace, and reporting are all exercised this phase.

When image generation becomes available, the priority-ordered manifest will drive the first batch in this order:

1. **Batch 1 — Family-cohesive demo (24–36 assets)** — see `PRODUCTION_ART_PRIORITY.md`.
2. The composer emits a per-content_id prompt JSON in `docs/visual/generated_prompts/`.
3. Each candidate is staged to `assets/_staging_generated/<family>/<id>.{jpg|png}`.
4. QA gating (`tools/production_qa.py`) → technical → semantic → style → duplicate → runtime-size.
5. Promotion via `tools/production_promote.py --id <content_id>` (only QA-PASS).
6. Wiring re-trace (`tools/visual_wiring_postfix.py`) confirms zero MISSING for the batch.
7. Contact-sheet regenerates (`tools/production_gallery_render.py`).
8. Runtime-context trace confirms the asset renders in the right panel.

## Per-content_id record (excerpt, top 30 P1)

| content_id | family | band | generation_status | qa_status | promotion_status | rejection_reason |
|---|---|---|---|---|---|---|
| `survivor_family_adult` | Survivor-Portrait | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `survivor_family_child` | Survivor-Portrait | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_grain_silo` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_ration_queue_plaza` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `npc_cluster_teacher` | NPC-Portrait | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `iodine_tablets` | Inventory-Item Medical | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `clean_water_jug` | Inventory-Item Food-Water | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `dried_rations` | Inventory-Item Food-Water | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `grain_exchange_scale_weight` | Inventory-Item Food-Water | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `military_rations` | Inventory-Item Food-Water | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `mira_chalk_ration_token` | Inventory-Item Food-Water | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `ration_plaza_paint_stick` | Inventory-Item Food-Water | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `spirits` | Inventory-Item Food-Water | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `water_purification_tablets` | Inventory-Item Food-Water | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `water_sample_contaminated` | Inventory-Item Food-Water | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `faction_the_compact` | Faction-Art | P2 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `faction_the_cutters` | Faction-Art | P2 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `faction_the_fleet` | Faction-Art | P2 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `faction_the_office` | Faction-Art | P2 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `faction_the_overlay` | Faction-Art | P2 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `faction_the_scale` | Faction-Art | P2 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `faction_the_underwrite` | Faction-Art | P2 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_alloc_12b` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_apiary_rows` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_ash_sign_shrine` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_avalanche_gallery` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_bathymetric_boat` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_bridge_seven` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_bus_reversal_loop` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |
| `loc_cider_press` | Location-Art | P1 | PENDING | NOT_STARTED | — | BLOCKED_EXTERNAL_AUTH |

(All 478 actionable rows share the same `BLOCKED_EXTERNAL_AUTH` status.)

## Re-run instructions (Phase 17 prep)

```bash
# (1) Verify auth is renewed
arkcli auth status --api-key "$BYTEPLUS_API_KEY"

# (2) Confirm 1-call test
arkcli +gen "small weathered glass jar" --modality image --model seedream-4-0-250828 --ratio 1:1 --size 1024 --output-format jpeg --no-open --save-to /tmp/probe/

# (3) Compose Batch 1 prompts (already done)
python3 tools/production_prompt_composer.py

# (4) Run a small batch (≤ 6 assets) end-to-end and verify the gate
python3 tools/production_qa.py
python3 tools/production_promote.py --id <content_id>

# (5) Re-trace wiring
python3 tools/visual_wiring_postfix.py

# (6) Regenerate gallery contact sheets
python3 tools/production_gallery_render.py

# (7) Update this ledger
python3 tools/production_ledger.py
```
