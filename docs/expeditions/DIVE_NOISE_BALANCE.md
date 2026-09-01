# Deep-Coast Dive Noise Balance & Acoustic Model

**Document:** `docs/expeditions/DIVE_NOISE_BALANCE.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/dive_sites.json`
**Runtime System:** [`Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`](../../Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs)

---

## 1. Acoustic Model & Noise Thresholds

The dive simulation evaluates acoustic generation per search action against the site's base noise floor and ambient hydrophone detection thresholds:

- **Quiet Mooring / Low Noise (`base_noise_floor <= 0.45`):**
  - Sites: `site_exp09_barge_flotilla` (0.40), `site_exp09_submerged_siphon` (0.40), `site_exp09_flooded_metro` (0.45).
  - Diver can take 6–8 methodical search actions before exceeding detection limits.
- **Moderate Harbor Currents (`0.50 <= base_noise_floor <= 0.60`):**
  - Sites: `site_exp09_ss_sovereign` (0.50), `site_exp09_flooded_field_hospital` (0.50), `site_exp09_submerged_convoy` (0.55), `site_exp09_ferry_terminal` (0.60).
  - Diver can take 4–5 search actions; forced cutting or forceful prying risks crossing the detection threshold.
- **High Acoustic Exposure (`base_noise_floor >= 0.65`):**
  - Sites: `site_exp09_drowned_fuel_depot` (0.65), `site_exp09_wrecked_patrol_craft` (0.65), `site_exp09_naval_patrol` (0.70), `site_exp09_offshore_relay` (0.70), `site_exp09_sunken_submarine` (0.80).
  - Extreme acoustic hazard; 2–3 actions maximum before triggering patrol alarms or structural collapse.
