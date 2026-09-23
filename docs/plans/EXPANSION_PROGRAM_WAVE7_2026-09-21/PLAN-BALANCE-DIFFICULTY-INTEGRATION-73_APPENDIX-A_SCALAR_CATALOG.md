# PLAN-BALANCE-DIFFICULTY-INTEGRATION-73 — Appendix A: Difficulty Scalar Catalog

**Generated:** 2026-09-21 from `Assets/StreamingAssets/Data/difficulty_presets.json`
(4 presets · default `difficulty_standard` ·
8 distinct scalars).
**Use:** BD-73B — the scalar census: every scalar below needs a live consumer
in host code; a hardcoded fallback is retired when its consumer binds.
Monotonicity (BD-73D2) is validated across these preset columns; the scalar
matrix makes the check mechanical (each scalar must be non-decreasing in
difficulty order).

## Preset overview

| Preset | Display | Scalars | Starting bonus items |
|---|---|---:|---|
| `difficulty_sparing` | SPARING | 8 | `canned_food`, `iodine_pills` |
| `difficulty_standard` | STANDARD | 8 | — |
| `difficulty_austere` | AUSTERE | 8 | — |
| `difficulty_dirge` | DIRGE | 8 | — |

## Scalar matrix (per preset)

| Scalar | SPARING | STANDARD | AUSTERE | DIRGE |
|---|---:|---:|---:|---:|
| `crisis_deadline_mult` | 1.25 | 1.0 | 0.8 | 0.65 |
| `disease_onset_mult` | 0.8 | 1.0 | 1.25 | 1.5 |
| `equipment_decay_mult` | 0.8 | 1.0 | 1.25 | 1.5 |
| `hostile_encounter_mult` | 0.75 | 1.0 | 1.35 | 1.75 |
| `hunger_rate_mult` | 0.75 | 1.0 | 1.35 | 1.75 |
| `market_price_mult` | 0.9 | 1.0 | 1.15 | 1.3 |
| `radiation_gain_mult` | 0.75 | 1.0 | 1.3 | 1.6 |
| `thirst_rate_mult` | 0.75 | 1.0 | 1.35 | 1.75 |

**`difficulty_sparing`** — 8 scalars

| Scalar | Value |
|---|---:|
| `crisis_deadline_mult` | 1.25 |
| `disease_onset_mult` | 0.8 |
| `equipment_decay_mult` | 0.8 |
| `hostile_encounter_mult` | 0.75 |
| `hunger_rate_mult` | 0.75 |
| `market_price_mult` | 0.9 |
| `radiation_gain_mult` | 0.75 |
| `thirst_rate_mult` | 0.75 |

**`difficulty_standard`** — 8 scalars

| Scalar | Value |
|---|---:|
| `crisis_deadline_mult` | 1.0 |
| `disease_onset_mult` | 1.0 |
| `equipment_decay_mult` | 1.0 |
| `hostile_encounter_mult` | 1.0 |
| `hunger_rate_mult` | 1.0 |
| `market_price_mult` | 1.0 |
| `radiation_gain_mult` | 1.0 |
| `thirst_rate_mult` | 1.0 |

**`difficulty_austere`** — 8 scalars

| Scalar | Value |
|---|---:|
| `crisis_deadline_mult` | 0.8 |
| `disease_onset_mult` | 1.25 |
| `equipment_decay_mult` | 1.25 |
| `hostile_encounter_mult` | 1.35 |
| `hunger_rate_mult` | 1.35 |
| `market_price_mult` | 1.15 |
| `radiation_gain_mult` | 1.3 |
| `thirst_rate_mult` | 1.35 |

**`difficulty_dirge`** — 8 scalars

| Scalar | Value |
|---|---:|
| `crisis_deadline_mult` | 0.65 |
| `disease_onset_mult` | 1.5 |
| `equipment_decay_mult` | 1.5 |
| `hostile_encounter_mult` | 1.75 |
| `hunger_rate_mult` | 1.75 |
| `market_price_mult` | 1.3 |
| `radiation_gain_mult` | 1.6 |
| `thirst_rate_mult` | 1.75 |


## Distinct scalars

`crisis_deadline_mult`, `disease_onset_mult`, `equipment_decay_mult`, `hostile_encounter_mult`, `hunger_rate_mult`, `market_price_mult`, `radiation_gain_mult`, `thirst_rate_mult`
