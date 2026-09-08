# Plan 112 countermeasure matrix

`countermeasure_item_id` identifies the catalog item associated with the
vector protocol. It does not replace an entry in `treatments[]`.

| Vector | Catalog countermeasure | Active protocol | New diseases |
|---|---|---|---|
| water | `clean_water` | purify water stores | `disease_dysentery` |
| air | `gas_mask` | seal ventilators | `disease_meningococcal_fever` |
| blood | `antibiotics` | sterilise surgical tools | `disease_bloodborne_hepatitis` |
| spore | `hazmat_suit` | engage air filtration | `disease_spore_wound_dermatitis` |

## Patient treatment

| Disease | Authorised treatment | Role | Window | Lethality reduction |
|---|---|---|---:|---:|
| `disease_dysentery` | `antibiotics` | curative | day 3 | .20 |
|  | `clean_water` | supportive | open | .05 |
| `disease_meningococcal_fever` | `antibiotics` | curative | day 2 | .30 |
|  | `medical_kit` | supportive | open | .10 |
| `disease_bloodborne_hepatitis` | `medical_kit` | supportive | open | .10 |
|  | `herbal_tea` | symptomatic | open | .00 |
| `disease_spore_wound_dermatitis` | `antiseptic_1l_of_1l` | suppressive | day 3 | .10 |
|  | `medical_kit` | supportive | open | .10 |

All referenced item IDs resolve in the shared `items.json` authority. No
unsupported `warm_clothing`, `soap`, `vitamin_c_source`, `item_water_filter`,
or `surgical_kit` references were introduced.
