# Plan 74 — Chapter Pacing Matrix

Day targets are **pacing documentation only**. The progression schema has no trigger fields (see runtime contract); the authoritative calendar is `weather_seasons.json`.

## Authoritative season calendar (weather_seasons.json, profile `default_winter`)

| Season window | Start day | Character |
|---|---:|---|
| Ash Fall | 0 | ashfall 2.2, fallout storms 1.0 |
| The Deep Freeze | 60 | blizzard 2.5 — winter onset |
| The Thaw | 120 | rain 2.2 — mobility returns |
| The Black Bloom | 180 | ashfall 1.6, black rain 0.9 — ash returns |
| High Cold | 240 | blizzard 2.6 — second deep winter |
| The Turning | 300 | clearest window — relative calm |

## Campaign duration authority

The epilogue (`CampaignEpilogueEngine`) takes `FinalDay` from the live runtime (`Main.GameFlow.cs`: HoldfastRuntime day / sim day). No fixed campaign-length cap exists in the runtime; campaigns run as long as the shelter survives. Day 300+ is reachable, so a Turning-era final chapter is not unreachable.

## Chapter pacing vs. real calendar

| Chapter | Title | Proposed timing | Actual valid anchor | Seasonal relation |
|---:|---|---|---|---|
| 1 | The Exchange | day 0 | campaign start | Ash Fall |
| 2 | Ashfall | days 1–14 | fallout period | Ash Fall |
| 3 | The Bunker | days ~7–30 | shelter establishment | Ash Fall |
| 4 | First Contact | days ~20–40 | first expeditions/traders | Ash Fall |
| 5 | The Long Winter | ~day 60 | **Deep Freeze onset (day 60)** | The Deep Freeze |
| 6 | The Consolidation | ~days 35–60 | late Ash Fall, before Deep Freeze | Ash Fall → Deep Freeze |
| 7 | The Long Dark | ~days 60–120 | interior of Deep Freeze | The Deep Freeze |
| 8 | The Thaw | ~day 120 | **Thaw onset (day 120)** | The Thaw |
| 9 | The Schism | ~days 120–160 | mid-Thaw political maturity | The Thaw |
| 10 | The Black Market | ~days 150–180 | late Thaw / scarcity under renewal | Thaw → Black Bloom |
| 11 | The Reckoning | ~days 180–210 | ash returns; debts resurface | The Black Bloom |
| 12 | The Rebuilding | ~days 200–240 | infrastructure recovery capacity | The Black Bloom |
| 13 | The Second Winter | ~day 240 | **High Cold onset (day 240)** | High Cold |
| 14 | The Muster | ~days 240–300 | after second winter, before calm | High Cold → The Turning |
| 15 | The Inheritance | ~day 300+ | **The Turning (day 300)** / endgame | The Turning |

## Notes

- Because chapters are display-only, "timing" here is thematic alignment, not a runtime trigger. Descriptions deliberately avoid hard day claims.
- Season titles were checked against the authoritative windows: "The Thaw" chapter name matches the season window `window_thaw` exactly; "The Long Winter" (existing) matches the profile display name "The Year of Ash and Ice" winter framing; "The Second Winter" aligns with High Cold.
- No chapter declares weather the season system does not support (no "spring bloom" language; the thaw chapter mentions water/rain, which is what `window_thaw` actually weights).