# Plan 120 — Component consumer matrix

| Component | Output | Explicit projection | Intended consumer |
|---|---|---|---|
| `composite_sensor_housing` | `item_faraday_mesh` | mass `0.80`, durability `1.10`, structural-or-better certification | future UV detector housing adapter |
| `composite_gpr_cart_frame` | `item_geophone_probe` | mass `0.75`, durability `1.15`, field-or-better certification | future GPR cart adapter |

The engine only returns these factors after a completed non-reject batch. It
does not rewrite vehicle mass, range, sensor physics or expedition capacity.
Those changes, when wired, must be recalculated by the owning consumer
authority.
