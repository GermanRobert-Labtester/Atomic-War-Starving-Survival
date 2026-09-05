# Micro-Location Hazard Risk/Reward — Zoonotic Flu vs Dead-Livestock Salvage (F17)

**Scope:** Closes the F17 open item — expected-cost analysis of the `micro_dead_livestock / scavenge_livestock` contamination consequence against its authored reward.
**Data authority:** `Assets/StreamingAssets/Data/disease_catalog.json`, `items.json`, `micro_locations.json` (all numbers below are read from these files, not invented).
**Simulation authority:** `Ashfall.Core.Disease.DiseaseSystem` (seeded, deterministic — same seed ⇒ same outcome).
**Date:** 2026-09-05

---

## 1. The exchange, as authored

| Side | Component | Authored value |
|---|---|---|
| **Reward** | `cloth` ×2 | 2.4 trade value (1.2 each), 0.2 kg total |
| **Reward** | morale | −2 |
| **Reward** | guilt | +1 |
| **Cost** | world flag `micro_contamination_exposure` | certain on first scavenge — routes to `DiseaseSystem.Infect(survivor, disease_zoonotic_flu, day)` exactly once (F17 contract) |

`disease_zoonotic_flu` authored parameters: lethality **0.18**, incubation **1 day**, illness **5 days**, infectivity **0.55**, spread interval **1 day**, spread radius **4**, vector **air**, countermeasure **`gas_mask`** (trade value 40).

## 2. Expected cost of one scavenge (deterministic mechanics)

The F17 hook applies `Infect` directly — no probability roll at the exposure layer. What the dice decide is the *outcome*, inside the disease authority:

1. **Infection: certain.** The scavenger is infected the day they scavenge.
2. **Course:** 1 incubation day + 5 illness days; after the 5th sick day a single seeded roll decides death (`lethality 0.18`, reduced by applied treatment) or recovery (`DiseaseSystem.ResolveOutcomes`).
3. **Death risk: 18%** per scavenge event, before treatment or lethality modifiers.
4. **Secondary spread:** while contagious and unquarantined, nightly spread rolls at infectivity 0.55 to shelter candidates within radius 4. A single case cannot declare an outbreak (threshold: 3 simultaneous infections), but two more infections from any source tips it — and an outbreak traces back to whatever seeded the first case.
5. **Mitigation:** quarantine (isolation ward) stops the spread; the authored countermeasure item (`gas_mask`) arms the shelter's air protocol, blocking vector spread — it does **not** undo an infection already applied. Treatment lowers the lethality roll.

**Framed economically:** 2.4 trade value buys a certain 6-day illness for one survivor, an 18% death roll, and a contagious body in the shelter unless quarantined. Against the expedition's other risk choices this is deliberately **risk-dominated** — a desperation trap with full player information (the site description flags the Geiger stutter; the `avoid_livestock` choice is free and always available).

## 3. Comparison within the site (all deterministic, from `micro_locations.json`)

| Choice | Gain | Cost | Depletes |
|---|---|---|---|
| `scavenge_livestock` | cloth ×2 | guaranteed zoonotic-flu exposure (above) | yes |
| `inspect_livestock_tags` | journal knowledge (`micro_dead_livestock_tags` + authored prose) | none | no |
| `avoid_livestock` | nothing | none | no |

The site is a textbook temptation structure: the only loot choice is the only lethal one. This matches the project tone rules (restrained consequences for greed) and requires no change.

## 4. Recommendation

**Keep as authored.** The exposure is certain rather than rolled, which is the clearer, more honest design (plan §10.9's preference for fixed deterministic consequences) and the F17 contract surfaces it to the player via the feedback strip ("Exposure: … Watch for fever."). Two data-driven dials exist if the owner later wants the trap softer — both single-file edits, no code:

- reduce `disease_zoonotic_flu.lethality` (0.18 → e.g. 0.10) in `disease_catalog.json`;
- or add `lethality_reduction` pressure via the existing treatment/countermeasure economy (gas_mask is already the authored countermeasure).

Do **not** soften it in the micro-location layer: the flag→infection contract must stay a pure pass-through into the disease authority (no hidden risk modifiers at the discovery layer).

## 5. Deferred extensions (unchanged from F17)

Survivor skill-based hazard mitigation, equipment-gated scavenge safety (gas_mask auto-protection roll), post-scavenge decontamination station hook, and weather-sensitive exposure severity — all belong to the disease/exposure authorities, not the discovery layer (see `docs/discovery/MICRO_LOCATION_HAZARDS.md`).
