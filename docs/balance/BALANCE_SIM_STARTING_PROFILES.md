# Starting Profile Balance Simulation

## Run contract

The Plan 134 balance pass is a deterministic catalog calculation rather than a
random campaign sweep. It uses the authoritative `items.json` and the six
profile rows from `starting_supplies.json`; no production data is modified.

The metrics and results are recorded in
`docs/content/plan134/STARTING_PROFILE_BALANCE_MATRIX.md`. The corresponding
Core tests recompute the matrix from the live catalog and reject profile
dominance or unknown item references.

## Result

The six-profile matrix is reproducible with the same catalog contents. The
alternate profiles intentionally lose immediate food/water capacity or
protection while gaining a narrow infrastructure, medical, greenhouse, or
survey advantage. No profile adds a progression-gated item or persistent
bonus.
