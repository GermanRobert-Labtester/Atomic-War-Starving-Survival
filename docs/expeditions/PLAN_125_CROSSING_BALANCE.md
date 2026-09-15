# PLAN 125 — Amphibious Crossing Balance (Phase 11)

**Source:** `--plans-122-125-balance-soak` 54-cell route matrix (cargo 0.2/0.5/0.95 × current 0.2/0.4/0.6 × normal/storm × skill 0/50/100, fixed per-cell seeds, plan §11.4)
**Status:** ACCEPTED — valuable route flexibility with visible risk and cost.

## Observed matrix (kit Mk I: flotation 0.80, tolerance 0.55, flooded-rail class)

| Metric | Value |
|---|---|
| Matrix cells | 54 |
| Completed crossings | 15 |
| Refused on flotation margin | **36** |
| Refused on current | 0 (tolerance check handled pre-attempt via margin/risk gates) |
| Pontoon destroyed mid-crossing | 0 |
| Emergency recoveries | 3 |
| Pontoon condition loss | min 0, max 4287, avg 865 bp |

## Reading

- **The flotation margin is the dominant gate:** 36/54 cells refuse up front —
  cargo load ×0.6, kit mass 0.15, and vehicle damage each eat the 0.80
  flotation rating until the 1500 bp minimum fails. This is the intended
  loadout tradeoff (plan §7.6): you lighten cargo or you don't cross.
- **The remaining crossings are risky but feasible:** 15 completions across
  lighter-load cells; 3 emergency recoveries show the ingress/pump hazard path
  works under stress; pontoon condition loss spans 0–4287 bp (bounded, seeded).
- **No guaranteed crossings:** `amphibious_matrix_not_all_success` fails the
  design if every cell completes — a favorable-only matrix would be a hidden
  buff. `amphibious_matrix_some_succeed` keeps the capability valuable.
- **Condition loss is bounded:** worst observed 4287 bp (43%) in the harshest
  cell — expensive but survivable; the emergency-recovery branch returns crews
  to LandReady coherently (plan §7.8: no instant catastrophic loss without
  signposted risk).
- **Skill helps but never guarantees:** navigator skill reduces effective
  current risk and hazard rolls (bounded ≤ 2000 bp); no cell becomes risk-free.

## Gates

`amphibious_matrix_runs` (54 cells), `amphibious_matrix_not_all_success`,
`amphibious_matrix_some_succeed`, `amphibious_matrix_condition_loss_bounded` — ALL PASS.

## Follow-ups

- Mk II (tolerance 0.8, powered pump) unlocks `route_class_submerged_causeway`;
  its matrix extension is a natural Phase 12 follow-up once boats/naval routes
  are authored against deeper classes.
- Fuel-use accounting flows through `ExpeditionVehicleSystem` logistics
  recalculation (typed seam from Phase 7); the cargo-opportunity cost appears
  in the kit's `cargo_capacity_modifier_bp` (8200 ⇒ −18% capacity).
