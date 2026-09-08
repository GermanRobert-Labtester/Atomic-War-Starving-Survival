# Starting Cohort Balance Simulation

Plan 138 uses a deterministic first-30-day heuristic, not hidden gameplay
modifiers. Each profile is simulated with the existing needs semantics:
hunger/thirst/fatigue drift upward, warmth drifts downward without heat,
health is reduced only after existing critical thresholds, and radiation is
read from the authored initial condition.

The simulation records:

- first critical hunger/thirst day under the conservative ration schedule;
- first critical health day under the no-intervention stress schedule;
- total initial health, morale, hunger, thirst, and lifetime dose;
- medical, repair, food, expedition, and social role coverage from canonical
  professions;
- whether the profile has a deterministic unrecoverable state before day 30.

The simulator uses stable profile/member order and a fixed seed for any
scenario sampling. It does not mutate campaign state or consume UI RNG.
The test suite asserts repeatability, Standard parity, finite ranges, and that
no alternate dominates Standard across every tracked supported dimension.

The current repository has no implemented Plan 134 origin catalog, so no
6×6 cohort/origin matrix is claimed.
