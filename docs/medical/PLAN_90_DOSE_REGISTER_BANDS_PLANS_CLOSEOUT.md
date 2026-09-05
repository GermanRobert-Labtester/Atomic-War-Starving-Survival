# PLAN 90 — DOSE REGISTER BANDS & PLANS EXPANSION — CLOSEOUT

> **SUPERSEDED (Plan 90B):** the blocker below was subsequently resolved and the
> 12-band / 8-plan expansion shipped. See
> `docs/medical/PLAN_90B_DOSE_REGISTER_UNBLOCK_CLOSEOUT.md` for the final
> thresholds, plans, save migration, and verification results. This document is
> retained as the historical audit record.

**Final status: ❌ BLOCKED — Core band-selection ladder is hardcoded to four bands.**

Plan 90's own BLOCKED criteria are met in full:

> "Use if loader enforces exact counts; band selection assumes four fixed IDs;
> thresholds are hardcoded in Core."

All three conditions verified by evidence (§2). Per the plan's hard constraint —
a **pure data pass** with **no Core code changes** and **existing dose-register
tests passing** — the only correct action was to stop before touching
`dose_registers.json`. **The data file was not modified.** A 12-band JSON would
break two count/index-pinning tests and ship eight bands the runtime can never
select (dead data the plan's quality bar explicitly forbids).

---

## 1. What was delivered (all four artifacts)

| Artifact | Content |
|---|---|
| `docs/medical/DOSE_REGISTERS_RUNTIME_CONTRACT.md` | Task 90A full runtime contract — lookup algorithm, ID plumbing, save model, plan execution, UI consumers, test pins |
| `docs/medical/PLAN_90_DOSE_REGISTER_BASELINE_MATRIX.md` | Task 90B — all 4 bands + 3 plans with every external reference mapped |
| `docs/medical/DOSE_REGISTER_PLAN_COST_INVENTORY.md` | Task 90H — cost grammar, verified item IDs, feasibility verdicts, ready-to-apply 8-plan slate |
| this file | closeout + unblock requirements |

## 2. The blocker, precisely

The 4-band ladder is **compiled**, not merely data:

1. **Thresholds hardcoded** — `DoseLedgerSystem.AmberMsv=100f / RedMsv=300f / BlackMsv=600f` (`DoseLedgerSystem.cs:60-62`); `BandFor()` (`:173-178`) is a pure function of these constants. The catalog's `threshold_msv` values are **never read** for selection.
2. **Four fixed IDs** — `DoseRegistersCatalogLoader.BandIdFor(int)` is a 4-case switch with `default → "band_green"` (`DoseRegistersCatalog.cs:103-112`); band ints ≥ 4 would silently label as Green. `GetAdministrativeBand` string-compares the same four IDs (`DoseLedgerSystem.cs:213-216`).
3. **Enum width** — `DoseBandResult` (`Green=0…Black=3`) is the public return of `BookReading`, consumed by tests and `Plan81` tests.
4. **Cross-system int ladder** — `SickBand.band` (int, saved) and `DiseaseTriage.SickBandFor` map illness stages onto 0–3.
5. **Test pins** — `Load_FindsFourBandsThreePlansThreeGuesses` (exact counts 4/3/3) and `Load_BandThresholdsBind` (**index** assertions `bands[1]=100, bands[2]=300, bands[3]=600`) both fail on any strictly-increasing 12-band insert.

## 3. Definition-of-Done disposition (plan §6)

| DoD item | Result |
|---|---|
| 1–13 (runtime/band/plan/UI/save audits) | **DONE** — see contract doc |
| 14–18 (12 bands shipped, ordering, no duplicate 0) | **NOT DONE — blocked** |
| 19–22 (8 plans shipped, costs resolve) | **NOT DONE — blocked** (slate designed in cost inventory) |
| 23–26 (KI/chelation/isolation/transfer guardrails) | **DONE by rejection** — all four conditional plans rejected with evidence; no false medicine shipped |
| 27–31 (dispositions, guesses, calibration, actions, NPCs) | **DONE as audit** — existing content already guardrail-clean; no "four bands" text in data |
| 32–36 (integrity, tests, build) | **PASS — repo untouched, all green** (§6) |
| 37–38 (no Core/save-schema changes) | **PASS** — zero code/data changes |
| 39 (closeout records substitutions) | **DONE** — this file |

## 4. The design, preserved for the follow-on plan

Nothing from the plan's design work is lost. Ready to apply once unblocked:

- **12-band ladder** with anchors preserved: `0 / 25 / 50 / 100 / 150 / 200 / 300 / 400 / 500 / 600 / 800 / 1000` mSv — bands `band_pale, band_yellow, band_orange, band_rose, band_crimson, band_violet, band_indigo, band_void` (rename `band_void` → `band_slate` recommended for tone).
- **8-plan slate** with verified item costs: observation / bed rest / fluids (`clean_water`) / supportive care (`medical_kit`) / pain control (`painkillers`) — no KI, no chelation, no decon, no isolation, no transfer (all rejected with evidence in the cost inventory).
- **Tone:** all proposed dispositions are institutional (duty restriction, review, surveillance) — no cumulative-mSv prognosis claims.

## 5. Unblock requirements (minimal Core change list for the follow-on plan)

This is **not** Plan 90 scope; it is the smallest change-set that makes the data honest:

1. `DoseLedgerSystem` — replace the 3 constants + `BandFor` chain with a catalog-driven threshold ladder (inject `DoseRegistersCatalog` or a threshold array); keep 100/300/600 as defaults so save semantics hold.
2. `DoseRegistersCatalogLoader` — replace `BandIdFor` switch with index-safe list lookup (clamp to top band, not Green, for out-of-range ints).
3. `DoseBandResult` — widen (add 8 members) or replace with `int` + `NoEntry` sentinel; update `BookReading` consumers.
4. `DiseaseTriage.SickBandFor` — document/verify stage mapping still lands on sensible rungs of the finer ladder (or keep 0/100/300/600-equivalent rungs explicitly).
5. Save compat: `SickBand.band` ints 0–3 keep meaning; new ints 4–11 are additive; `administrativeClassificationOverride` strings gain 8 new accepted values — old saves load unchanged.
6. Update the two count/index-pinning catalog tests to the new contract (this is a deliberate contract change, not test-breakage).
7. Then apply the JSON: 12 bands + 8 plans per §4, run the plan's full boundary matrix (0…>1000).

## 6. Verification (run at closeout — repository untouched)

| Check | Result |
|---|---|
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS** — full suite green, including `DoseRegistersCatalogTests` (4/3/3 pins intact) |
| `dotnet build Ashfall.csproj` | **PASS** — 0 errors |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS** — 0 errors |
| `--content-utilization-selftest` (where available) | n/a — no new content shipped |
| New C# tests | **none added** (per hard constraint) |
| Core code changes | **none** |
| Save-schema changes | **none** |

## 7. Lessons for the follow-on

- The register's honesty is its strength: plans are inscriptions, not cures. Keep it that way — the rejected plans (KI/chelation/decon/isolation/transfer) would have broken medical credibility faster than missing granularity ever would.
- The catalog's `calibration` and `registers` JSON blocks are currently dead (no DTO fields). The follow-on should either deserialize them or drop them from the file.
- `plan_morphine_tray`'s `cost: "morphine"` is a pre-existing unresolved reference; the closest real item is `painkillers`. Correct it in the same follow-up that expands plans.
