# Plan 141 — Save Compatibility & Migration Policy

## 1. Save Boundary & Persistence Policy

A fundamental architectural rule of ASHFALL is that **static definition data is never duplicated into campaign save files**.

- **No Save DTO Modifications:** Plan 141 introduces zero new fields to `MedicalSaveStore`, `SurvivorsSaveStore`, `DiseaseSaveStore`, or any campaign save envelope.
- **Save Checksum Preservation:** Because no save structures or serializable DTOs are altered, `SaveChecksum` calculations remain 100% stable. Pre-existing campaign saves retain identical hashes.
- **Runtime Derivation:** All clinical prose, symptom lines, and casebook entries are resolved at runtime from static catalog JSON based on live condition identifiers.

---

## 2. Backward & Forward Compatibility Scenarios

| Scenario | Campaign Save State | Plan 141 Behavior | Integrity Contract |
|---|---|---|---|
| **Old save created prior to Plan 141** | Contains active survivors with radiation dose, respiratory degradation, or health deficits. | Panels successfully project clinical diagnosis and symptom context immediately upon load. | Clean pass; zero migration required. |
| **New save loaded in stripped environment (prose files missing)** | Contains standard medical state. Catalog file `medical_texts.json` is missing or unreadable. | Catalog loader logs non-fatal diagnostic. UI gracefully omits prose; mechanical treatments and vitals function identically. | Invariant 4.1 preserved; zero crash. |
| **Corrupted / malformed prose file** | JSON syntax error in `medical_texts.json`. | Loader catches error, reports diagnostic, falls back to empty catalog. | No game failure; medical simulation continues. |
| **Old save with unknown condition ID** | Legacy or modded condition ID not in `medical_texts.json`. | Resolver returns `null`; panels render mechanical stats without descriptive prose. | Safe fallback; no null reference. |

---

## 3. Invariant Verification

1. **Round-Trip Parity:** Capturing and restoring game state via `SaveStoreHub` produces bit-for-bit identical state before and after Plan 141.
2. **Determinism:** Seeded simulation runs produce identical outcomes whether `MedicalTextCatalog` is queried or not.
