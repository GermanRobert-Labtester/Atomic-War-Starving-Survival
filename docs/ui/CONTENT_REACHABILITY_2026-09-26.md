# Content Reachability Sweep — 2026-09-26

Registry-loaded is not player-reachable. This sweep checks whether each
loaded art id is referenced by any data catalog or host/UI source file
(registry file itself excluded). Orphans are candidates for wiring or retirement.

| Category | Loaded | Referenced (data/code) | Orphan |
|---|---|---|---|
| faction | 98 | 98 | 0 |
| portrait | 267 | 267 | 0 |
| location | 385 | 385 | 0 |
| item | 996 | 996 | 0 |
## Method and limits

- Haystack: 3,002 files / ~36 MB of text — every JSON under
  `Assets/StreamingAssets/Data/` plus every `.cs` under `src/` and
  `Assets/Ashfall.Core/`.
- A hit means the id string appears in a data catalog or a host/UI source file.
  This is a **premise** check: it proves nothing is stranded by name, not that
  every id reaches a rendered surface.
- The stronger runtime gate is `--asset-registry-selftest` (resolution) plus the
  panel render audits; the combination is what this sweep supports.

## Verdict

**No orphan art ids.** All 1,746 loaded ids (faction 98, portrait 267,
location 385, item 996) are referenced from the data or source tree, so the art
wave is not generating (or retaining) unreferenced weight. No wiring or
retirement work is required; re-run this sweep after the item/location waves
finish.
