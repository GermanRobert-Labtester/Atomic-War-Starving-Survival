# Starting Profile Balance Matrix

Metrics use the live item catalog values and the authored quantities in
`starting_supplies.json`.

- **Value**: sum of `tradeValue × amount`.
- **Weight**: sum of `weight × amount`; lower is a carrying advantage.
- **Food**: sum of `hungerRestore × amount`.
- **Water**: sum of `thirstRestore × amount`, including irradiated water at its
  ordinary risky value.
- **Medical**: sum of `healthEffect + radCleanse` for medical and anti-radiation
  items. Iodine is tracked as prevention, not treatment value.
- **Protection**: sum of catalog radiation protection across supplied gear.
- **Leverage**: nominal value of non-food, non-water, non-medical,
  non-protective supplies. This is a proxy for repair, treatment, devices, and
  greenhouse leverage, not a second economy authority.
- **Slots**: sum of `ceil(amount / stackMax)`.

| Profile | Value | Weight | Slots | Food | Water | Medical | Protection | Leverage |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| `origin_standard_holdfast` | 695 | 30.4 | 17 | 640 | 580 | 90 | 110 | 171 |
| `origin_waterworks_failure` | 591 | 24.3 | 16 | 480 | 340 | 60 | 30 | 267 |
| `origin_field_clinic` | 620 | 22.6 | 17 | 400 | 370 | 330 | 110 | 120 |
| `origin_machine_room` | 581 | 24.9 | 14 | 400 | 340 | 30 | 30 | 301 |
| `origin_greenhouse_remnant` | 569 | 41.3 | 18 | 320 | 370 | 30 | 30 | 257 |
| `origin_evacuated_checkpoint` | 552 | 23.8 | 16 | 320 | 340 | 60 | 110 | 224 |

## Balance reading

- Standard remains the broadest and strongest general-purpose opening.
- Waterworks trades food, water, and protection for treatment infrastructure
  and repair leverage.
- Field Clinic has the highest medical capacity but the weakest food/water
  reserve among the medical/protection-heavy profiles.
- Machine-Room has the highest repair/material leverage and the lowest slot
  burden, but lacks hazmat, Rad-Away, and desalination stock.
- Greenhouse has the highest weight and low immediate survival, trading that
  burden for seeds and growing materials.
- Evacuated Checkpoint has strong protection and measurement capacity but the
  weakest food reserve, with no special combat access.

The nominal values range from 79.4% to 89.2% of Standard. The survival and
leverage dimensions intentionally move in opposite directions. No alternate
profile is at least as strong as Standard across food, water, medical,
protection, and leverage; pairwise dominance is covered by the profile tests.
