# Plan 79 — Autopsy Coverage Matrix

## Stale-premise reconciliation

The plan brief stated a baseline of **3 procedures**. Repository truth at implementation time: **9 procedures** — concurrent work had already authored the plan's P4–P8 slate (blunt, ballistic, respiratory, hypothermia, spore isolation, neurotoxin assay). Plan 79 therefore executed as a **gap-closure expansion (9 → 12)**.

## Final 12-procedure catalog

| # | Procedure ID | Display name | Diagnostic domain | Air | Path | Hours | Research unlock |
|---:|---|---|---|---:|---:|---:|---|
| 1 | `procedure_rad_pathology` | Radiation Pathology | radiation (existing) | 0.15 | 0.05 | 4 | `knowledge_radiation_basics` |
| 2 | `procedure_toxicology` | Toxicology Screen | poison/toxin (existing) | 0.10 | 0.08 | 3 | `knowledge_pathogen_containment` |
| 3 | `procedure_containment_autopsy` | Containment Autopsy | high-containment/unknown pathogen (existing) | 0.30 | 0.20 | 6 | `knowledge_pathogen_containment` |
| 4 | `procedure_blunt_trauma` | Blunt Force & Crush Forensics | blunt/crush (existing) | 0.05 | 0.02 | 3 | `knowledge_field_trauma_surgery` |
| 5 | `procedure_ballistic_forensics` | Ballistic & Shrapnel Extraction Forensics | penetrating/fragment (existing) | 0.05 | 0.03 | 4 | `knowledge_field_trauma_surgery` |
| 6 | `procedure_respiratory_contamination` | Pulmonary Asbestos & Rad-Dust Screen | inhaled particulates (existing) | 0.25 | 0.05 | 4 | `knowledge_radiation_basics` |
| 7 | `procedure_hypothermia_pathology` | Severe Hypothermia & Frostbite Pathology | cold exposure (existing) | 0.02 | 0.02 | 3 | `knowledge_field_trauma_surgery` |
| 8 | `procedure_spore_infection_isolation` | Fungal Spore & Bio-Contaminant Isolation | fungal/bio-contaminant (existing) | 0.35 | 0.25 | 5 | `knowledge_pharmacology_synthesis` |
| 9 | `procedure_poison_biochemical_assay` | Neurotoxin & Heavy Metal Assay | deep toxicology (existing) | 0.15 | 0.10 | 5 | `knowledge_pharmacology_synthesis` |
| 10 | `procedure_deprivation_pathology` | **Deprivation & Wasting Pathology** | starvation/dehydration (**new**) | 0.02 | 0.02 | 3 | `knowledge_food_preservation` |
| 11 | `procedure_blast_injury` | **Blast Overpressure Forensics** | primary blast injury (**new**) | 0.08 | 0.04 | 5 | `knowledge_fortified_chokepoints` |
| 12 | `procedure_forensic_unknown` | **Full Forensic Examination** | uncertain/mixed cause (**new**) | 0.18 | 0.12 | 7 | `knowledge_pathogen_containment` |

## Gap analysis vs. the plan's working slate

| Plan procedure | Status in 9-procedure baseline | Plan 79 action |
|---|---|---|
| Radiation | covered (#1) | preserve |
| Toxicology | covered (#2, deepened by #9) | preserve |
| Containment/disease | covered (#3 + #8) | preserve |
| Penetrating/ballistic | covered (#5) | preserve |
| Blunt/crush | covered (#4) | preserve |
| Respiratory/CO | partially (#6 — particulates) | preserve |
| Cold | covered (#7) | preserve |
| **Deprivation (starvation/dehydration)** | **missing** | **added #10** |
| **Blast overpressure** | **missing** (#5 covers fragments, not primary blast injury) | **added #11** |
| **Forensic/uncertain cause** | **missing** (toxicology is toxin-focused, not broad differential) | **added #12** |
| Chemical exposure | covered by #9/#6 | skipped (duplicate) |
| Suspicious death | covered by #2/#9 + new #12 | folded into #12 |

## Distinctness audit (§79L.10)

- `procedure_blast_injury` vs `procedure_ballistic_forensics`: blast lung/overpressure hemorrhage/concussion vs. bullet trajectory/fragment extraction — distinct diagnostic questions.
- `procedure_forensic_unknown` vs `procedure_toxicology`: broad multi-cause differential (concealed trauma, mixed cause) vs. targeted toxin screen. The forensic exam costs the most time (7h) and has moderate risk, but yields fewer specialized findings per cause than any specialist — specialist value preserved (§79H.5).
- `procedure_deprivation_pathology`: unique lowest-risk tier alongside hypothermia; distinct deprivation findings (`starvation_wasting`, `severe_dehydration`, `immune_collapse`).
- All 12 finding sets are unique (verified programmatically).

## Risk/time ladder (final)

Risk tiers: deprivation/hypothermia (0.02) < blunt/ballistic (0.05) < blast (0.08) < toxicology (0.10) < rad/poison-assay (0.15) < forensic (0.18) < containment (0.30) < spore isolation (0.35). Time: 3h focused → 5h detailed → 6–7h containment/broad. Higher risk correlates with clinically relevant research (containment/pathogen), not arbitrary reward scaling.
