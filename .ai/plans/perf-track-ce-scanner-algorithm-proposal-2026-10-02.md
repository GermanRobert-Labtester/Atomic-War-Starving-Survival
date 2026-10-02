# PERF PROGRAM — Track C/E proposal: the 3 CPU-bound sweep scanners

**Status:** `STATUS: AWAITING USER APPROVAL` (measured proposal; no code changed)
**Author:** perf program session, 2026-10-02
**Supersedes:** the earlier "cache the scanners" assumption (now falsified).

## Finding (measured, 2026-10-02)

The three remaining ≥60 s sweep scanners are **not** IO-bound and **not**
cacheable. They are **algorithm-bound** — a nested cross-product regex scan.

| Evidence | Value |
|---|---|
| Read all 4,196 `.cs` files (42.7 MB) + one regex pass | **1.87 s** (glob 0.66 + read 0.76 + regex 0.45) |
| `generate-architecture-map.py --check` actual | **~56 s** (user 56.0 / sys 0.7) |
| `generate-plan-integration-audit.py --check` actual | **~63 s** (user 62.9 / sys 0.4) |
| `generate-port-contract.py --check` actual | ~89 s (regex cross-product) |

Because raw IO+regex is 1.87 s, an input cache (Track B's prescription) cannot
recover the other ~54 s — the cost is in the scanners' logic.

### Confirmed hot loop — `generate-architecture-map.py` (~line 2414)

```python
for p in main_sources:                     # 306 Main*.cs
    content = read + strip comments/strings
    for setup_method in setup_methods:      # 314 distinct setup methods
        declaration = re.compile(...)                    # compiled 96,084 times
        source_without_declaration = declaration.sub("", content)   # 96,084 whole-file subs
        call_pattern = re.compile(rf"\b{re.escape(setup_method)}\s*\(")
        for match in call_pattern.finditer(source_without_declaration):  # 96,084 whole-file scans
            ...
```

- **96,084** `(file × method)` iterations; **~795 MB** of text re-scanned per run.
- Corrected earlier claim: `plan-integration`'s `load_graph()` import of
  `generate-architecture-map.py` is **0.02 s** (324 subsystems) — the "~56 s
  duplicated graph build" hypothesis is **falsified**; the duplication does not
  exist.

## Proposed fix (algorithm first — per the mission's rule)

**Single-pass inversion** in `generate-architecture-map.py`:

1. Build **one** regex matching any call to any setup method:
   `\b(name1|name2|…)\s*\(` (or `\b[A-Za-z_]\w*\s*\(` then filter to the set).
2. Strip declarations **once per file** (one `sub`), not once per method.
3. Scan each `Main*.cs` **once**, mapping matched names → setup methods.

This turns `files × methods` whole-file transforms into `files` transforms.

- Expected: **~56 s → ~2 s** (matches the 1.87 s raw-scan measurement).
- Same for the sibling scanners (their loops need the same inversion; confirm
  each hotspot before editing).
- **Correctness gate:** `generate-architecture-map.py --check` must stay PASS
  (byte-identical `ARCHITECTURE_TEST_MAP.md` + graph) — the sole authority.

## Rust?

**Not warranted yet.** The fix is algorithmic in the current language and should
recover the full cost; the mission's rule is algorithm before language. Rust is
reconsidered only if the inverted algorithm still exceeds ~10 s — it would then
be a strong fit (deterministic, engine-free, batch), absorbed into `tools/rstools`,
not a new implementation beside it.

## Approval requested

- Implement the inversion in `generate-architecture-map.py` (+ each scanner),
  verified byte-identical by the existing `--check` gates, under a fresh claim.
- Estimate: ~5 min/sweep reclaimed (56 + 63 + 89 s); no behavior change.