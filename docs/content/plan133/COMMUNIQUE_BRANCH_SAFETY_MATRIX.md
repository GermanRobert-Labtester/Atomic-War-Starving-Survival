# Communiqué Branch Safety Matrix (Plan 133)

> §9 classification of every communiqué's factual content:
> **A** invariant across all branches · **B** true only in one branch · **C**
> interpretation/opinion independent of branch · **D** prediction/claim before
> resolution (allowed only when the outcome is itself invariant) · **E** impossible
> under some outcomes.
>
> **Policy applied**: only A / C / D-invariant content is shipped. No B or E content
> exists in the corpus. Branch gating is never simulated via prose in `title` or
> `authorNote` (invariant 3.3); mechanical gates live in
> `FactionWarCommuniqueExpansionTests`.

## Baseline 18

| ID | Class | Notes |
|---|---|---|
| comm_d497_garrison_clean_strike | A/C | crater invariant; attribution opinion |
| comm_d497_rebuilders_clean_strike | A/C | no-artillery fact invariant; agnosticism opinion |
| comm_d498_ash_sign_clean_strike | C | pure doctrinal interpretation |
| comm_d519_garrison_almshouse | A/C | shelling invariant; "insurgent ordnance" is false opinion (authorNote) |
| comm_d520_rebuilders_almshouse | A/C | geometry of the emplacement invariant |
| comm_d521_ash_sign_almshouse | A/C | warning delivered before strike (s1 d515) invariant |
| comm_d537_garrison_exchange_checkpoint | A/C | post stands invariant (s2 d536 fires regardless) |
| comm_d538_rebuilders_exchange_checkpoint | A/C | ditto |
| comm_d549_garrison_ration_plaza | A/C | strike invariant; "convoy" claim is false opinion (authorNote) |
| comm_d550_rebuilders_ration_plaza | A/C | no-convoy record invariant; guess marked incomplete |
| comm_d552_ash_sign_ration_plaza | C | doctrinal reading only |
| comm_d573_forward_roster_checkpoint | A/C | Roster origin invariant (journal d555 corroborates) |
| comm_d581_garrison_shrine_strike | A/C | shrine strike invariant; insinuation false (authorNote) |
| comm_d582_rebuilders_shrine_strike | A/C | honest agnosticism |
| comm_d583_ash_sign_shrine_strike | A/C | retraction of doctrine, not event claims |
| comm_d591_ash_sign_ceasefire_pause | A/C | ceasefire + calmed readings invariant |
| comm_d593_forward_roster_ceasefire_toll | A/C | checkpoint + rate invariant |
| comm_d607_garrison_forward_roster_recognition | A/C | carefully worded to concede nothing about the player's d605 push |

## New 22

| ID | Class | Branch-sensitive facts explicitly excluded |
|---|---|---|
| comm_d489_garrison_manifest_inspection | A/C | calibration claim (Garrison's own) is opinion; holdup + discrepancy invariant |
| comm_d490_rebuilders_two_scales | A/C | Exchange pre-weigh invariant; thief accusation withheld |
| comm_d505_garrison_labor_quota | A/C | **Denner's fate (exemption/disappearance) excluded** — quota notice invariant |
| comm_d507_rebuilders_quota_arithmetic | A/C | roster's own notice count — self-description, invariant |
| comm_d513_garrison_span_readiness | A/C | re-arm invariant (s2 d512 fires regardless of standoff outcome) |
| comm_d514_rebuilders_span_record | A/C | both-patrols-walked-off invariant; three years of crossing invariant |
| comm_d523_ash_sign_pilgrim_toll | A/C | toll imposed invariant |
| comm_d526_garrison_waystation_fee | A/C | fee rescinded invariant (s2 d525 fires regardless) |
| comm_d527_rebuilders_guard_detail | A/C | detail forms invariant (s2 d528 fires regardless of stance choice); vote closeness corroborated by "against her own instinct" |
| comm_d530_garrison_recorded_concern | A/C | detail exists invariant; memo motive is hidden (authorNote) |
| comm_d556_rebuilders_fracture | A/C | twelve walk out invariant (journal d555 + comm d573 corroborate); **which side the player backs excluded**; Roster camp location stated as "we understand" (hearsay, not assertion) |
| comm_d561_ash_sign_dead_channel | A/C | **intercept/pad possession excluded** (branch: Garrison/Rebuilders/player); burst publicity invariant (radio d559); gated by test `NoCommunique_AssertsPossessionOfTheBranchSensitiveIntercept` |
| comm_d568_rebuilders_pumphouse_questions | A/C | questions only — **asserts no arrangement facts**; asks because branch-independent stonewalling |
| comm_d570_garrison_pumphouse_coordination | A/D-invariant | upper-terrace reallocation is the s2 d569 outcome, which fires regardless of the player's expose/silence/broker choice; "seasonal" framing is the hidden euphemism (authorNote) |
| comm_d575_forward_roster_origin | A/C | origin facts invariant |
| comm_d576_forward_roster_passage_rules | A/C | posted rules are the Roster's own declared policy (self-description) |
| comm_d577_ash_sign_restless_numbers | A/D-invariant | comments on s1 d576 (already fired); advises hedged, promises only the count; the shrine does NOT predict its own strike (authorNote: misread direction) |
| comm_d592_garrison_standdown | A/C | stand-down invariant; "review of earlier assessments" commits to nothing |
| comm_d594_rebuilders_ceasefire_benchmark | A/C | checkpoint at the rope still standing invariant (no chain stage removes it) |
| comm_d599_forward_roster_crates | A/C | crates were the Roster's own (self-description; answers radio d585's public rumor) |
| comm_d602_ash_sign_the_question | A/C | theory circulates invariant (chain fires after ceasefire resolution); **the theory is never asserted as true** |
| comm_d608_forward_roster_non_recognition | A/C | fence holding invariant; **the player's recognition/rejoin push at d605 excluded** — response concedes nothing about it |

## Deferred / rejected candidates (B/E)

- **evt_d541 evacuation window** — any statement about who knew, who leaked, whether
  the queue was warned, or whether the stores moved is branch-sensitive (warned /
  looted / silent). No communiqué authored; chain left uncovered by design.
- **evt_d583 D/9 reassessment** — clandestine detachment; no public statement (canon).
- **evt_p25_* chains (16)** — flag-gated; may never have fired. Mechanically excluded
  by `Communiques_NeverReferenceFlagGatedChains`. No candidate was drafted.
- **Garrison LN74 possession claim** — would assert the branch-sensitive intercept
  routing. Rejected; Ash Sign's dead-channel statement observes only the public burst.
- **Rebuilders "door stays open" invitation to the Roster** — would imply the player's
  fold-back push at d605. Rejected in favor of d594's checkpoint benchmark.
