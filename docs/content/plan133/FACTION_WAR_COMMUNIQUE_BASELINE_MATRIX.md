# Faction War Communiqué Baseline Matrix — the existing 18 (Plan 133)

> One row per shipped entry (all preserved verbatim as compatibility anchors).
> Truth status quotes the `authorNote` classification where present. Consumers:
> as of baseline, **no player-facing renderer exists**; the only consumers are the
> catalog query API and tests (see PLAN133_BASELINE.md §5).

## Matrix

| # | ID | Chain | Day | Faction | Public claim (summary) | Hidden truth (authorNote) | Rhetorical mode | Contradicts | Branch-safe? |
|---|---|---|---|---|---|---|---|---|---|
| 1 | comm_d497_garrison_clean_strike | evt_d495 | 497 | Garrison | No Garrison fire mission logged; impact speculation = disinformation | (none) | Procedural denial + threat | R#2, A#3 | Yes — crater is invariant |
| 2 | comm_d497_rebuilders_clean_strike | evt_d495 | 497 | Rebuilders | We have no artillery; not accusing the Garrison; somebody did it | (none) | Honest agnosticism | G#1, A#3 | Yes |
| 3 | comm_d498_ash_sign_clean_strike | evt_d495 | 498 | Ash Sign | Where ash lands is a choice, a hand, not chance | (none) | Doctrinal pattern-reading | G#1, R#2 | Yes |
| 4 | comm_d519_garrison_almshouse | evt_d517 | 519 | Garrison | Impact attributed to unsecured insurgent ordnance | **False** — internal range logs show a Checkpoint Gamma overshoot, buried | Blame transfer | R#5, A#6 | Yes — shelling invariant |
| 5 | comm_d520_rebuilders_almshouse | evt_d517 | 520 | Rebuilders | Almshouse on the fall line from the new gun at Gamma — geometry | **True by accident** — no proof, only geometry | Counter-evidence (geometry) | G#4 | Yes |
| 6 | comm_d521_ash_sign_almshouse | evt_d517 | 521 | Ash Sign | A pilgrim carried our warning three days before; numbers moved first | **True and mundane** — real vigilance in doctrinal dress | Vigilance-as-doctrine | G#4 | Yes |
| 7 | comm_d537_garrison_exchange_checkpoint | evt_d533 | 537 | Garrison | Inspection post ensures "fair and continuous access"; cooperation "offered or otherwise" | (none) | Occupation framed as continuity | R#8 | Yes — post stands invariant |
| 8 | comm_d538_rebuilders_exchange_checkpoint | evt_d533 | 538 | Rebuilders | A squad at our own rope inspecting our goods is an occupation | (none) | Anger at euphemism | G#7 | Yes |
| 9 | comm_d549_garrison_ration_plaza | evt_d545 | 549 | Garrison | Impact caused by unauthorized Rebuilders supply movement | **False** — fabricated blame | Fabricated blame | R#10, A#11 | Yes — strike invariant |
| 10 | comm_d550_rebuilders_ration_plaza | evt_d545 | 550 | Rebuilders | No convoy of ours; a Garrison post stood two streets over until a month before | **Also incomplete** — the guess (missed post) is wrong; the plaza itself was the target | Counter-record + honest uncertainty | G#9 | Yes |
| 11 | comm_d552_ash_sign_ration_plaza | evt_d545 | 552 | Ash Sign | The ash also takes; no gentler reading; the numbers don't soften | Closest to true in spirit — precision read correctly, source unknown | Doctrinal fracture (public humility) | G#9 | Yes |
| 12 | comm_d573_forward_roster_checkpoint | evt_d570 | 573 | Forward Roster | Not Garrison, not raiders; twelve became thirty; toll is the price of standing | (none) | Self-definition / local legitimacy | (Garrison d607 implicit) | Yes |
| 13 | comm_d581_garrison_shrine_strike | evt_d578 | 581 | Garrison | Timing serves Ash Sign's narrative; no Garrison unit in range | **False and cynical** — insinuates the Ash Sign staged it | Cynical insinuation | R#14, A#15 | Yes — strike invariant |
| 14 | comm_d582_rebuilders_shrine_strike | evt_d578 | 582 | Rebuilders | No stake, no wish for harm, "for once we're not scoring the point" | **Honest agnosticism** | Restraint | G#13, A#15 | Yes |
| 15 | comm_d583_ash_sign_shrine_strike | evt_d578 | 583 | Ash Sign | We no longer tell you what the measuring means; claim withdrawn at cost | **True and central** — closest any voice comes to intuiting the sparing was never providence | Retraction / public humility | G#13, A#11 lineage | Yes |
| 16 | comm_d591_ash_sign_ceasefire_pause | evt_d588 | 591 | Ash Sign | The reading has calmed; we have not; still counting what doesn't add up | (none) | Honest uncertainty | — | Yes — ceasefire at s1 d588 |
| 17 | comm_d593_forward_roster_ceasefire_toll | evt_d570 | 593 | Forward Roster | Spur road checkpoint predates the pause and outlasts it; two units, same as always | (none) | Toll/accounting legitimacy | (Garrison stand-down framing) | Yes |
| 18 | comm_d607_garrison_forward_roster_recognition | evt_d605 | 607 | Garrison | Aware of the unlicensed toll post; not recognition, tolerance, or precedent | (none) | Concede-nothing recognition | FR#12,17 | Yes — deliberately worded, concedes nothing |

## Observations feeding Plan 133

1. **Strongest pattern**: same-event triplets with mutually incompatible public claims
   (d495, d517, d545, d578) + hidden truth separated from claim. This is the template.
2. **Coverage skew**: all covered chains are mid-war (495+) or culmination; the entire
   cold-war band (480–491) and open-conflict band (503–524) have **zero** communiqué
   coverage despite heavy radio coverage.
3. **Voice asymmetry** (must be preserved): Garrison = denial/classification/procedural
   blame; Rebuilders = material records/practical geometry; Ash Sign = pattern
   interpretation with admitted uncertainty; Forward Roster = defensive legitimacy.
4. **`authorNote` discipline**: notes carry truth-layer classification and privileged
   causal information (mystery-actor signature, hidden range logs) — never rendered.
5. **No statement depends on a branch outcome**; the two ceasefire-adjacent entries are
   day-gated after the events they reference.
