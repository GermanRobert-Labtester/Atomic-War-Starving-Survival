# Plan 119 — Sensor characterization

The detector uses a bounded confidence model composed from fault intensity,
range attenuation, visibility/ash, environment activity, sensor condition,
calibration, operator skill and seeded noise. Confidence and signal are both
clamped to `[0,1]`; weak or distant signals may yield no observation.

The focused suite covers clear strong faults, range/visibility comparison,
atomic battery use, non-mutation of the supplied power fault, deterministic
observation capture/restore, and seeded behavior. The combined 60-day artifact
records latest confidence so a different fixed seed has an observable result
without requiring every field to diverge.

No zero-false-positive or exact-component claim is made. Weather modifiers
affect observation quality only; owning the detector does not improve grid
reliability.
