# ASHFALL fifteen-asset generation batch — 2026-09-30

> **STATUS: APPROVED BY USER — GENERATION COMPLETE; STAGED, NOT RUNTIME INTEGRATED**

User authorization: “Please start generating a batch of 15 game assets!”

Outcome: generate 12 surface backdrop candidates (three scene families × day/dawn/dusk/night) and three coherent survivor sprite-sheet candidates (base/rust/olive). Existing Surface and Characters manifests explicitly mark these files PLACEHOLDER; BackdropArt and SurvivorActorView consume their contracts. Surface bake script is existing foreign work and stays read-only.

Non-goals: runtime integration, overwriting existing art, gameplay/data/source changes, commits, and full-suite tests. Candidates are staged for visual and frame-layout review; generation does not certify import, wiring or animation.

Owned files: artifacts/asset-generation/batch15-2026-09-30/{wasteland_sky_day1_7,wasteland_sky_dawn,wasteland_sky_dusk,wasteland_sky_night,surface_hatch_approach_day1_7,surface_hatch_approach_dawn,surface_hatch_approach_dusk,surface_hatch_approach_night,expedition_departure_day1_7,expedition_departure_dawn,expedition_departure_dusk,expedition_departure_night,char_base_sheet,char_base_sheet_rust,char_base_sheet_olive}.png; prompts.json, REPORT.md, preview.png and sprite-preview.png in that directory; this plan; bounded additive claim and .ai/state.md entry. Temporary per-frame crops live only under /tmp/ashfall-batch15-frames.

Acceptance: 15 separate built-in image-generation calls; every successful output saved inside workspace; correct dimensions/transparency checked; contact-sheet visual review; exact failures/QA limits recorded; preserve unrelated dirty work. Tool failure is recorded without silent API fallback.

Verification: bin/ashfall-dev validate-json (714/714 PASS pre-generation); ImageMagick identify and contact-sheet review of generated files. No code tests required for staged assets.

Outcome: all 15 PNG candidates saved and size/alpha verified. Three additional sprite correction calls improved idle and passing poses; local alpha-aware repacking aligned the 64×96 cells. Previews and provenance are saved alongside REPORT.md. Sprite gait and alpha-edge cleanup remain review work; no production integration was attempted. Generation acceptance is complete; this staging plan is intentionally not marked FULLY INTEGRATED.
