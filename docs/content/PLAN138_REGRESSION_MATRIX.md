# Plan 138 Regression Matrix

| Area | Required proof | Status |
|---|---|---|
| legacy loader | flat `starting_survivors.json` remains exact Standard | PASS, parity test |
| cohort loader | six profiles, stable order, default selection | PASS, 11 focused tests |
| candidate validation | canonical IDs, no active questline, ranges, duplicates | PASS, negative fixtures |
| fresh slot | second New Game selects a distinct slot and preserves first | PASS, lifecycle self-test |
| fresh initialization | defaults apply once under `FreshInitialize` | PASS, lifecycle self-test |
| restore | one, many, and valid zero-member saves never reseed | PASS, lifecycle self-test |
| failed restore | corrupt/missing save does not become New Game | PASS, lifecycle self-test |
| save/reload | actual roster beats current catalog defaults | PASS, campaign A restore |
| UI | default fast start, alternate selection, cancel has no mutation | PASS, lifecycle UI probe |
| deterministic simulation | repeated 30-day scenarios match | PASS, 11 focused tests |
| Plan 134 boundary | cohort has no supply/loadout authority | PASS by source audit |
| full regression | Core, host, Godot, data, journey, utilization, fast gates | Core/data/bridge gates PASS; current host recheck blocked by unrelated Plan 140/UI worktree compile errors; prior lifecycle/host gates passed |
