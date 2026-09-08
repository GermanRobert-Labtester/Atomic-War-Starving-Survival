# Faction War Event ↔ Communiqué Coverage (Plan 133 recon)

> Inventory of every chain in `faction_war_events.json` (38 chains) against communiqué
> coverage, with branch notes. "Auto" = chain fires by day/visit/resolution triggers;
> "Flagged" = requires a Plan 25 grievance flag (may never fire — excluded from
> communiqué references). Coverage counts include Plan 133 additions (final state).

## Main arc — auto-firing chains

### cold_war (480–495)

| Chain | Day | Factions | Branch notes | Communiqués (final) |
|---|---|---|---|---|
| evt_d480_grain_tally_dispute | 480 | G, R | Player choices affect player action only; dispute itself invariant | Radio-heavy (d481/d484); no communiqué (chosen gap: radio already carries both voices' short forms) |
| evt_d485_checkpoint_notice_war | 485 | G, R | "A Third Hand" ambiguity must stay ambiguous | None — mystery preserved by silence |
| evt_d488_manifest_holdup | 488 | G, R | Shortfall "on paper" invariant; calibration unverified | +G d489 (procedure), +R d490 (counter-scale) |
| evt_d491_toll_hike | 491 | warlords | Weighbridge Toll posts rate notices on radio, not communiqués (canon) | None (radio d493/d512 carry it) |
| evt_d495_the_clean_strike | 495 | G, R | Crater invariant | G d497, R d497, A d498 (existing triplet) |

### open_conflict (503–524)

| Chain | Day | Factions | Branch notes | Communiqués (final) |
|---|---|---|---|---|
| evt_d503_conscription_lists | 503 | G | **Denner's fate is branch-sensitive** — excluded | +G d505 (quota arithmetic), +R d507 (carrier arithmetic) |
| evt_d509_border_clash_span44 | 509 | G, R | Both patrols walk off (invariant); charges re-armed at s2 d512 (invariant) | +G d513 ("readiness"), +R d514 (span neutrality record) |
| evt_d517_almshouse_shelling | 515 | G, R | Shelling invariant | G d519, R d520, A d521 (existing triplet) |
| evt_d522_switchback_toll | 522 | G, A | Fee rescinded at s2 d525 (invariant) | +A d523 (pilgrim toll doctrine), +G d526 (rescinded-on-review) |
| evt_d524_market_price_spike | 524 | R | Guard detail forms at s2 d528 **regardless of stance choice** | +R d527 (divided vote), +G d530 (recorded concern — paper trail for Order 534) |

### the_offensive (533–558)

| Chain | Day | Factions | Branch notes | Communiqués (final) |
|---|---|---|---|---|
| evt_d533_garrison_offensive_grain_silo | 533 | G, R | Post stands at s2 d536 invariant; stock evacuation is player branch — excluded | G d537, R d538 (existing pair) |
| evt_d541_evacuation_window_plaza | 541 | G, R | **Three-way branch (warned/looted/silent)** — who knew, who warned is branch-sensitive; strike not preventable | **No communiqué** (any window/leak statement would be branch-unsafe; d545 pair/triplet already covers the aftermath) |
| evt_d545_ration_plaza_strike | 545 | G, R | Strike invariant; casualty detail branch-sensitive (no counts asserted) | G d549, R d550, A d552 (existing triplet) |
| evt_d552_rebuilders_fracture | 552 | R | Twelve walk out invariant (journal d555, comm d573 corroborate); which side the player backs is branch | +R d556 (on the twelve), +FR d575 (origin, after Roster identity exists) |
| evt_d558_ln74_signal_intercept | 558 | G | **Who receives the intercept is branch-sensitive**; the burst itself is public (radio d559) | +A d561 (dead-channel observation; no possession claim); G deliberately silent on the pad (would assert branch) |

### culmination (565–605)

| Chain | Day | Factions | Branch notes | Communiqués (final) |
|---|---|---|---|---|
| evt_d565_hydro_leverage_break | 565 | G, R | Deal stands + slope loses water at s2 d569 invariant; whether player exposes it is branch (public suspicion already canon via radio d566) | +G d570 (custodial coordination), +R d568 (which slopes, on whose ledger) |
| evt_d570_forward_roster_first_action | 570 | FR, G, R | Column slows invariant | FR d573 (existing), FR d593 (existing), +FR d576 (passage rules) |
| evt_d578_shrine_strike_anomaly | 576 | A | Anomaly invariant | G d581, R d582, A d583 (existing triplet), +A d577 (restless numbers — post-s1 d576, pre-strike observation) |
| evt_d583_d9_reassessment | 583 | black_ops | D/9 is clandestine — **issues no public statements (canon)** | None by design |
| evt_d588_ceasefire_by_exhaustion | 588 | G, R | Stand-down invariant | A d591 (existing), +G d592 (concede-nothing stand-down), +R d594 (post benchmark) |
| evt_d600_theory_surfaces | 600 | G, R, A | Theory circulates invariant; its truth is never asserted by anyone | +A d602 (measurement vs interpretation) |
| evt_d605_post_ceasefire_forward_roster | 605 | FR, G, R | Player's push (recognize/rejoin/silence) is branch — the fence holding is invariant | G d607 (existing), +FR d608 (non-recognition response) |

## Flagged Plan 25 chains — EXCLUDED from communiqué references (mechanical gate)

`evt_p25_marked_ruin` (200), `evt_p25_stopped_convoy` (220), `evt_p25_bitter_water`
(230), `evt_p25_empty_chair` (240), `evt_p25_cistern_toll_blockade` (250),
`evt_p25_prisoner_at_the_gate` (230), `evt_p25_refugees_from_the_line` (512),
`evt_p25_requisition` (515), `evt_p25_broken_route` (525),
`evt_p25_field_hospital_overflow` (536), `evt_p25_deserter_column` (548),
`evt_p25_retaliation` (555), `evt_p25_no_more_volunteers` (568),
`evt_p25_bread_before_bullets` (572), `evt_p25_quiet_faction` (578),
`evt_p25_refusal_at_dawn` (584).

These fire only while a campaign grievance flag is set; in a campaign where the flag
never got set, the events never happened — a static public statement referencing them
would be branch-impossible (§3.3/E-class). Gate: `Communiques_NeverReferenceFlagGatedChains`.

## Coverage arithmetic (final, 40 entries)

- Chains with ≥2 competing faction perspectives: **11**
  (d488, d503, d509, d522, d524, d533, d552, d565, d588, d605, plus existing d495/d517/d545/d578 triplets → 14 total)
- Chains with ≥3 perspectives: **5** (d495, d517, d545, d578, d588)
- Previously uncovered bands now covered: cold_war (d488), open_conflict
  (d503, d509, d522, d524), offensive (d552, d558), culmination (d565, d600).
- Deliberate non-coverage: d480/d485 (radio carries both voices; the bulletin-board
  mystery must stay unspoiled), d491 (Toll posts rate notices, not communiqués),
  d541 (branch-unsafe), d583 (D/9 clandestine).
