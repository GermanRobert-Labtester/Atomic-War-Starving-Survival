# Plan 118 — Synthetic lubricant balance

The first catalog is intentionally conservative: two feedstock units produce
bounded split outputs after two process ticks. Catalyst condition decays on
active ticks and off-band thermal/pressure inputs reduce yield/quality. A
consumer receives a wear benefit only after it is registered and serviced;
there is no global wear multiplier.

Current authored profile: `ft_reactor_mk1`; products are a synthetic lubricant,
wax/byproduct and light fraction. The 60-day proof uses the real catalog and
records fuel inventory, catalyst condition, active batches, output buffers and
seeded divergence in `artifacts/advanced-industrial-recon-60d.{json,md}`.

No balance tuning was applied in this slice. A future tuning pass must use the
catalog fields and a long-horizon artifact, not tick-code constants.
