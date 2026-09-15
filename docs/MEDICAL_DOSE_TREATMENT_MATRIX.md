# Medical dose-treatment matrix

| Treatment | Item | Mechanical scope | Research | Schedule | Authority |
|---|---|---|---|---|---|
| Potassium iodide | `iodine_pills` | invokes survivor radiation iodine protection; does not erase dose | existing radiation treatment availability | immediate | `RadiationSicknessAfflictionHandler` → `SurvivorsHostSession` |
| Anti-rad / chelation | `rad_away` | removes the existing acute/current radiation dose through `RadiationSystem`; does not write lifetime exposure or the separate dose ledger | `knowledge_chelation_therapy` when the shared capability provider is bound | immediate | `RadiationSicknessAfflictionHandler` → `RadiationSystem` |
| Oxygen support | `item_oxygen_supply` | applies the existing respiratory relief handler | none | 24 game hours | `RespiratoryAfflictionHandler` → `RespiratoryDegenerationSystem` |

The anti-rad path is intentionally not presented as generic erasure of
absorbed/lifetime dose: the host callback calls `AdministerAntiRad`, whose
scope is the current bounded acute dose field. The treatment item is consumed
by the pipeline before the handler applies the effect; a bound production
research query gates the chelation treatment without duplicating research state
in medical saves.

The existing dose ledger and radiation progression remain separate authorities.
No new cumulative-dose reset or parallel pharmacology state was introduced.
