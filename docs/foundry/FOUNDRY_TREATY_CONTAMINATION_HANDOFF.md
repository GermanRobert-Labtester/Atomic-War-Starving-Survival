# Foundry Treaty Contamination & Water Security Handoff Contract

**Target Systems:** `Assets/Ashfall.Core/Radiation/`, `Assets/Ashfall.Core/WeatherSystem.cs`
**Host Hook:** `src/Foundry/SilentFoundryHostSession.cs`

---

## 1. Environmental & Contamination Narrative Principles

Treaty consequences interface with environmental contamination and water purity through resource scarcity and institutional maintenance:

1. **Autonomous Decision Rule 13 Compliance:**
   - The consequence schema contains no top-level `contamination_delta` or `rad_spike` primitives.
   - Contamination consequences operate strictly through supported market modifiers (`clean_water`, `water_filter`) and narrative reasons.
   - Environmental physics remain authoritative within `RadiationSystem` and `WeatherSystem`.

---

## 2. Water Security Treaties & Consequence Design

| Treaty ID | Outcome | Good Modifiers | Environmental Narrative in Reason |
|---|---|---|---|
| `treaty_deep_coast_aquifer_protection_treaty` | `met` | `clean_water` (`-0.35`) | Sediment screens cleaned, brine separator gaskets fitted, pump intake purity verified at Pump Station Nine. Regional water stress eases. |
| `treaty_deep_coast_aquifer_protection_treaty` | `violated` | `clean_water` (`+0.80`),<br>`water_filter` (`+0.50`) | Untreated bilge discharge contaminates coastal marsh intake tributaries. Brine breakthrough disables pump manifold; clean water becomes scarce and ceramic filters demand spikes. |
| `treaty_flotilla_saline_corridor_concordat` | `met` | `clean_water` (`-0.25`),<br>`fuel` (`-0.20`) | Tidal wash basins kept dredged and free of industrial sludge; desalination exchange stabilizes coastal potable water. |

---

## 3. Safety Guidelines

- **No Sabotage Recipes:** In accordance with the Project Safety Rule, consequence text and reasons must describe institutional failure, mechanical rupture, and neglect (fouled sediment screens, delayed filter gaskets, pump manifold failure) rather than actionable real-world biological, chemical, or radiological sabotage techniques.
