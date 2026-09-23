# PLAN-ORPHAN-SEAL-01 — Appendix AM: Generator Inventory

**Generated:** 2026-09-21. The appendix generators are now **versioned in the
repository** at `tools/generators/` (14 scripts), so appendix
regeneration is reproducible instead of living in a shell session. Each script
is read-only over production sources (scan + emit markdown) and writes only
inside this programme directory.
**Use:** to re-run an appendix, execute the matching script from the repository
root; to verify an appendix is current, re-run and diff. Appendix AJ lists the
trigger events.

| Generator | Produces |
|---|---|
| `gen_plan1_def.py` | Appendices D, E, F |
| `gen_plan1_ghi.py` | Appendices G, H, I |
| `gen_plan1_jkl.py` | Appendices J, K, L |
| `gen_plan1_mno.py` | Appendices M, N, O |
| `gen_p_fixed.py` | Appendix P (corrected closure-based version) |
| `gen_plan1_pqr.py` | Appendix Q, R |
| `gen_plan1_stu.py` | Appendices S, T, U |
| `gen_plan1_vwx.py` | Appendix V (with `fix_vwx.py` corrections applied: V, W, X) |
| `fix_vwx.py` | Corrected regeneration of V, W, X (recursive data scan; field-only statics) |
| `gen_plan1_yzaa.py` | Appendix AA + first draft of Y/Z |
| `fix_yz.py` | Corrected regeneration of Y and Z (snake-balanced batches; shared shapes) |
| `gen_plan1_abacad.py` | Appendices AB, AD (+ first draft AC) |
| `gen_plan1_aeag.py` | Appendices AE, AF, AG |
| `gen_plan1_ahaj.py` | Appendices AH, AI, AJ |
| `gen_scaffolds.py` | Scaffolding appendices for ten session plans (62, 64, 67, 68, 70, 72, 75, 77, 78, 79) |
| `gen_scaffolds2.py` | Paired scaffolds for ten more plans (81, 82, 83, 84, 86, 87, 89, 90, 91, 92), each adopting a named session plan as its scaffolding authority |
| `gen_scaffolds3.py` | Paired scaffolds, batch 3 (88, 93, 94, 95, 96, 97, 98, 99, 100, 131) with bespoke data sections for l10n, deprecation, release tooling, and programme self-counts |
| `gen_scaffolds4.py` | Paired scaffolds, batch 4 (132, 133, 134, 135, 136, 137, 141, 142, 143, 145) — Wave 11 subsystems each paired with a session authority |
| `gen_scaffolds5.py` | Paired scaffolds, batch 5 (138, 139, 140, 144, 146, 147, 148, 149, 150, 151) — Wave 11/12 subsystems each paired with a session authority |
| `gen_scaffolds6.py` | Paired scaffolds, batch 6 (152–161) — Wave 12 frontier plans plus Espionage 161, each paired with a session authority |
| `gen_scaffolds7.py` | Paired scaffolds, batch 7 (162–171) — Wave 13 content plans, each paired with a session authority |
| `gen_scaffolds8.py` | Paired scaffolds, batch 8 (172–175, 178, 180, 181, 183, 186, 189) — Wave 13 remnants + Wave 14 systems |
| `gen_scaffolds9.py` | Paired scaffolds, batch 9 (191, 192, 193, 194, 195, 196, 199, 201, 203, 205) — Wave 15 systems |
| `gen_expand_plans.py` | Total content expansion for ten Wave 19 family plans (273, 278, 270, 275, 279, 265, 267, 266, 277, 276): census, data/state surface, verification, rollout, acceptance matrix |
| `gen_expand_plans_driftaware.py` | Drift-aware family census variant (batch 2: 261, 262, 263, 264, 268, 269, 271, 272, 274, 280) — annotates every file `still unmentioned` / `since-mentioned` |
| `gen_expand_plans_batch3.py` | Domain-aware content expansion (batch 3: 255, 248, 246, 247, 250, 239, 257, 258, 222, 229) — premise-marked census, data/state surface, verification, rollout, acceptance |
| `gen_expand_plans_batch4.py` | Domain-aware content expansion (batch 4: 241, 253, 243, 251, 245, 235, 254, 231, 230, 217) |
| `gen_expand_plans_batch5.py` | Domain-aware content expansion (batch 5: 256, 252, 232, 259, 226, 242, 227, 236, 179, 225) |
| `gen_expand_plans_batch6.py` | Domain-aware content expansion (batch 6: 206, 249, 214, 244, 240, 190, 228, 184, 187, 223) |
| `gen_expand_plans_batch7.py` | Domain-aware content expansion (batch 7: 233, 221, 224, 219, 215, 207, 211, 220, 237, 200) |
| `gen_expand_plans_batch8.py` | Domain-aware content expansion (batch 8: 213, 209, 234, 197, 188, 182, 202, 212, 216; Plan 56's build-hygiene census is bespoke) |
| `gen_expand_plans_batch9.py` | Domain-aware content expansion (batch 9: 260, 204, 177, 198, 185, 238, 210, 129, 127, 157) |
| `gen_expand_plans_batch10.py` | Domain-aware content expansion (batch 10: 218, 58, 128, 176, 54, 196, 55, 201, 194, 126) |
| `gen_expand_plans_batch11.py` | Domain-aware content expansion (batch 11: 208, 191, 180, 114, 117, 203, 193, 181, 143, 122) |
| `gen_expand_plans_batch12.py` | Domain-aware content expansion (batch 12: 120, 166, 53, 123, 152, 112, 199, 205, 171, 102) |
| `gen_expand_plans_batch13.py` | Domain-aware content expansion (batch 13, eight source plans: 111, 125, 175, 107, 186, 183, 151, 153; Plans 115 and 72 use bespoke docs/threading censuses) |
| `gen_expand_plans_batch14.py` | Domain-aware content expansion (batch 14: 189, 158, 156, 106, 103, 192, 160, 174, 164, 77; Plan 106 carries a bespoke input-surface census) |
| `gen_expand_plans_batch15.py` | Domain-aware content expansion (batch 15, eight source plans: 121, 178, 167, 150, 172, 154, 155, 169; Plans 71 and 52 use bespoke host/l10n censuses) |
| `gen_expand_plans_batch16.py` | Domain-aware content expansion (batch 16, nine source plans: 108, 165, 113, 142, 133, 195, 134, 118, 104; Plan 60 uses a bespoke asset/license census) |
| `gen_expand_plans_batch17.py` | Domain-aware content expansion (batch 17: 144, 148, 105, 168, 57, 51, 162, 138, 145, 141) |
| `gen_expand_plans_batch18.py` | Domain-aware content expansion (batch 18: 132, 76, 75, 110, 109, 149, 173, 170, 139, 163) |
| `gen_expand_plans_batch19.py` | Domain-aware content expansion (batch 19, eight source plans: 161, 91, 124, 131, 135, 101, 147, 130; Plans 74 and 100 use bespoke QA/programme censuses) |
| `gen_expand_plans_batch20.py` | Domain-aware content expansion (batch 20, seven source plans: 119, 116, 159, 66, 136, 64, 63; Plans 59, 73, 99 use bespoke agent/balance/hotfix censuses) |
| `gen_expand_plans_batch21.py` | Domain-aware content expansion (batch 21: 137, 67, 69, 92, 89, 140, 90, 146, 93, 65) |
| `gen_expand_plans_batch22.py` | Domain-aware content expansion (batch 22, nine source plans: 82, 88, 98, 68, 97, 70, 79, 96, 95; Plan 86 uses a bespoke CLI-surface census) |
| `gen_expand_plans_batch23.py` | Domain-aware content expansion (batch 23, nine source plans: 81, 85, 78, 80, 61, 84, 87, 83, 62; Plan 94 uses a bespoke deprecation census) |
| `gen_expand_plans_batch24.py` | Domain-aware content expansion (batch 24, seven source plans: 33, 42, 41, 32, 50, 34, 27; Plans 23, 22, 24 use bespoke selftest/data/debt censuses) |
| `gen_expand_plans_batch25.py` | Domain-aware content expansion (batch 25, eight source plans: 43, 36, 37, 48, 44, 47, 49, 46; Plans 35 and 31 use bespoke failure/IO censuses) |
| `gen_expand_plans_batch26.py` | Domain-aware content expansion (batch 26, eight source plans: 45, 38, 39, 29, 30, 28, 26, 40; Plans 21 and 25 use bespoke event/input censuses) |
| `gen_expand_plans_batch27.py` | Domain-aware content expansion (batch 27, three source plans: 16, 18, 15; Plans 17, 19, 11, 13, 14, 12, 20 use bespoke test/asset/registry/determinism/data/save/release censuses) |
| `gen_expand_plans_batch28.py` | Final expansions (Plans 01–06, bespoke kit/decision/culture/body/launch censuses) plus tier-2 reference-graph sections for Plans 30, 146, 62, 48 |
| `gen_cross_plan_coupling.py` | Cross-plan coupling sections (batch 1: 28, 42, 44, 83, 87, 93, 61, 78, 80, 27) — incoming plan edges + package→file touch maps |

**Known correction lineage:** AC was corrected by a separate pass (signature
extraction); the corrected logic is embedded in `gen_plan1_abacad.py`'s
successor run recorded in EVIDENCE.md. A future maintainer re-runs the
generator and diffs; a mismatch is a finding, not an error.

