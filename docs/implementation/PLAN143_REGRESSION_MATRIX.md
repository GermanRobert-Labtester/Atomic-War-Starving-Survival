# Plan 143 regression matrix

| Area | Required proof |
|---|---|
| Catalog | 15 events parse; 18 choices and 17 effects are typed; duplicate IDs, invalid weights, unknown effects, and malformed refs fail closed |
| Graph | Stage 2 cannot precede stage 1; branches are exclusive; stage 3 acknowledges once; survivor absence/death/away blocks selection |
| RNG | Same campaign seed and day produce the same ordinal-sorted candidate and pending event; catalog order does not alter it |
| Consequences | Valid morale, faction standing, knowledge, and expedition-offer adapters mutate exactly once; invalid preflight mutates nothing |
| Persistence | Pending choice survives save/load; completed/branch state survives; restore never replays downstream effects; old saves remain valid |
| UI | Normal narrative route presents the pending choice; choices bind to typed event IDs; rebind and acknowledgement do not mutate state |
| Content utilization | Loader, registration, queries, selection, and consumption evidence are emitted from the real arc system |
| Repository gates | Data integrity, full Core tests, Godot host build, fast CI gates, and available narrative/faction/expedition/save selftests pass |
