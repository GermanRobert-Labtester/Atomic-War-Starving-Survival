# Wasteland Grave Epitaphs — Selection Contract

**Core Determinism Pillar (Invariant 4):** Deterministic execution across all hosts and seeds.
**Authority:** Seeded PRNG (`ISeededRng` / `CoreSeededRng`).

---

## 1. Selection Architecture

When an environmental grave or memorial entry selects an epitaph:

```text
cause_of_death (string) + deterministic seed (ulong / int)
                    ↓
Filter candidate list by matching cause:
    candidates = catalog.Epitaphs.Where(e => e.Cause == cause).ToList()
                    ↓
If candidates is empty, fallback to "unknown" or "unspecified":
    candidates = catalog.Epitaphs.Where(e => e.Cause == "unknown" || e.Cause == "unspecified").ToList()
                    ↓
Seeded modulo index selection:
    selectedIndex = rng.Next(0, candidates.Count)
                    ↓
Selected Epitaph String
```

---

## 2. Invariants

1. **Deterministic Stability:** For any fixed pair of `(cause, seed)`, the selected epitaph string is guaranteed to be identical across runs, hosts, and platforms.
2. **List Ordering Preservation:** The catalog order of existing records is strictly preserved. New records are appended sequentially.
3. **No Unreachable Candidates:** For every supported cause, every candidate in its candidate pool has a non-zero probability of selection across uniform integer rolls `Next(0, count)`.
4. **Fallback Safety:** An unrecognized cause always resolves to a valid candidate from `unknown` or `unspecified`, guaranteeing that a blank or null string is never produced.
