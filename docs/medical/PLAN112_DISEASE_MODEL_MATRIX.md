# Plan 112 disease model matrix

The four additions use only fields consumed by `DiseaseDefinition` and
`DiseaseSystem`. They are communicable disease entries, not radiation,
toxicity, deficiency, frostbite, tetanus, or rabies placeholders.

| ID | Clinical profile | Vector | Lethality | Incubation / illness | Infectivity / interval / candidates | Immunity |
|---|---|---|---:|---:|---:|---:|
| `disease_dysentery` | acute fecal-oral dehydration | water | .25 | 1 / 6 | .45 / 1 / 4 | 21d / .95 |
| `disease_meningococcal_fever` | fast invasive respiratory fever | air | .60 | 2 / 5 | .30 / 2 / 2 | 30d / 1.00 |
| `disease_bloodborne_hepatitis` | slow bloodborne hepatic illness | blood | .35 | 10 / 18 | .12 / 7 / 1 | 30d / .90 |
| `disease_spore_wound_dermatitis` | spore exposure through damaged skin | spore | .22 | 3 / 8 | .22 / 3 / 2 | 14d / .85 |

## Phase rules

Every addition carries authored incubation, prodromal, symptomatic, and severe
phases. Hepatitis also carries a critical phase. Incubation does not shed;
phase contagiousness multiplies the base infectivity. These values are
clinical progression data, not host-side calculations.

## Runtime reachability

All four are registered by the existing `BindCatalog` path and can be seeded
by the existing `DiseaseHostSession.Infect` and `DiseaseSystem.TryExpose`
contracts. Their spread, quarantine, treatment, lethality, immunity, and save
behavior therefore use existing code without new adapters.
