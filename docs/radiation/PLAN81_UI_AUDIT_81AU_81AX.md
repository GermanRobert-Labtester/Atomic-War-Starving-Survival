# Plan 81 — UI-Adjacent Re-Audit (Tasks 81AU–81AX)

> **Scope:** Verify the dose-ledger UI claims made in
> `PLAN_81_DOSE_LOCATIONS_EXPANSION_CLOSEOUT.md` against current source and a
> headless `--dose-uitest` run, now that the 14-location catalog is restored.
> **Audit only — no UI code changed.**

---

## 1. Surfaces examined

| Surface | Route / host | Locations rendered? |
|---|---|---|
| `src/UI/DoseLedgerPanel.cs` | registered `dose_ledger` (PanelRegistryBootstrap.cs:120), bound to `DoseLedgerHostSession` | No — per-survivor ledger; locations appear only as `reading.source` provenance strings |
| `src/Dose/DoseRegisterSurface.cs` | Phase0 panel right column (`src/Main.Phase0.cs:276`), Content tab | Yes — all 14 (displayName + raw id + description) |
| `src/UI/RadiationHistoryPanel.cs` | radiation history readout | Provenance only (`reading.source` at line 79) |
| `src/UI/RadiationDetailPanel.cs`, `GeigerCalibrationPanel.cs` | — | No location/sector/rate content |

---

## 2. Task-by-task verdicts

### 81AU — Dose Ledger UI Coverage (sector/location names) — **PARTIAL**

- `DoseRegisterSurface.RenderContent()` iterates `_session.Content.locations`
  generically: all 14 entries render without crash or missing labels, including
  the 9 new sector locations.
- **Defect (mislabel):** the section header is `"Rooms — standing places:"`.
  That was accurate for the 5 bunker rooms; it is now factually wrong for
  surface aprons, expedition perimeters, travel corridors and a faction
  checkpoint. The player is told that the Irradiated Forest Edge is a "room".
- **Defect (raw IDs):** both `DoseLedgerPanel.RefreshDetail()` and
  `RadiationHistoryPanel.cs:79` render the reading source as the **raw
  location ID** (`loc_military_depot_perimeter`), violating the house rule
  "never render raw item IDs". Attribution exists (81AG provenance is
  mechanically correct) but is not player-friendly.

### 81AV — Sector Filter / Grouping — **NO CRASH; SECTOR DIMENSION ABSENT**

- No sector grouping, filtering or labeling exists in any dose surface.
  A `grep -rn "\.sector" src/` finds zero dose-location consumers (only the
  unrelated evolving-world seed system).
- Nothing breaks with 5 sectors: `RenderContent` is schema-agnostic and the
  headless `--dose-uitest` passes with the 14-entry catalog loaded.
- The UI-16/UI-21 caveat applies: the only layout surface (Content tab) is a
  single autowrapped `Label` inside a 240 px-min `TabContainer` holding 14 ×
  (name + id + 2–3 sentence description) ≈ 70 lines. Bounds/truncation were
  not verifiable headlessly; treat as an overflow risk, not a confirmed defect.

### 81AW — Risk-Level Display — **FAIL (absent, not merely unstyled)**

- `DoseLocationDef.riskLevel` has **zero consumers outside tests**. The 0–8
  scale is authored, validated, documented in the risk matrix — and rendered
  nowhere. The acceptance criterion "highest/lowest values are legible; risk
  is not communicated by color alone" is vacuously satisfied because risk is
  not communicated at all.

### 81AX — Radiation Unit Display — **PASS on truthfulness; FAIL on visibility**

- **Truthful:** no surface displays µSv/h at all — `radiationUsv` has **zero
  consumers in `src/`** (`grep -rn "radiationUsv" src/` → empty). Every dose
  label (`mSv`) is truthful for the values actually shown
  (cumulative/booked/nominal are mSv in `DoseLedgerSystem`). No false `/h`
  claim exists. The plan's critical rule ("do not show `/h` if runtime value
  is not hourly") is satisfied trivially.
- **Real defect (small-dose invisibility):** both
  `DoseLedgerPanel` (`nominalMsv:0.0/bookedMsv:0.0`) and
  `RadiationHistoryPanel` (`bookedMsv:0.0`) format readings with `:0.0`.
  The newly authored low-rate geography produces readings far below the
  format's resolution:
  - 1 h at shelter exterior approach (0.85 µSv/h) = 0.00085 mSv → renders
    as `0.0/0.0 mSv`
  - 2 h at contaminated water access (3.5 µSv/h) = 0.007 mSv → `0.0/0.0 mSv`
  The accumulated dose for exactly the locations Plan 81 added is displayed
  as zero. Bunker readings (0.02 µSv/h) were always sub-resolution; the
  expansion makes the problem player-visible across routine surface work.

### Headless verification run

```
godot --headless --path . -- --dose-uitest
→ SELFTEST PASS: dose_uitest (exit 0)
  assertions: surface/npcs/book/diagnose/palliative/cohort/volunteer/rendered — all True
  NOT asserted: sector rendering, risk rendering, unit labels, content-tab
  bounds with 14 locations
  teardown: 219 ShapedTextData + 12 texture RID leaks (documented UI-21 pattern)
```

---

## 3. Closeout claim corrections

`PLAN_81_DOSE_LOCATIONS_EXPANSION_CLOSEOUT.md` §UI/accessibility claimed:

| Claim | Source evidence | Verdict |
|---|---|---|
| "sector display: rendered in Dose Ledger UI" | no `.sector` consumer in src/ | **UNSUPPORTED** |
| "risk display: numeric 0–8 scale supported without color-only reliance" | `riskLevel` has no UI consumer | **UNSUPPORTED** |
| "radiation unit: truthful uSv/h display" | no `radiationUsv` consumer in src/ | **UNSUPPORTED** (mSv labels are truthful, but no uSv/h display exists) |
| "description: concise 1–3 sentences readable without truncation" | plausible for `RenderContent`; bounds unverified | **UNVERIFIED** |

This is the UI-21/UI-22 pattern: generated/selftest "PASS" converted into a
player-facing completion claim. The claims above describe the **data model's**
capability, not an implemented UI.

---

## 4. Disposition (follow-up work, not Plan 81 scope)

Plan 81's architecture rule kept it data-first; these are the resulting UI
gaps, in priority order:

1. **Fix the small-dose format** — display µSv precision (e.g. `0.0009 mSv`
   or `850 µSv`) in `DoseLedgerPanel.RefreshDetail` and
   `RadiationHistoryPanel`. Smallest safe change; restores visibility of
   low-rate geography.
2. **Replace the raw source ID** with `DoseContentCatalog` displayName lookup
   (fallback to ID) in both provenance renderers.
3. **Fix the "Rooms — standing places:" header** or group the Content tab by
   sector when the panel is next touched (it lives inside the Phase0 omnibus,
   already flagged UI-15).
4. Render `riskLevel` and the location baseline rate where dose locations are
   listed — only together with a real route (81AU/81AW acceptance), not as a
   label-only change.
5. Extend `--dose-uitest` to assert sector/rate rendering and content-tab
   bounds before any of the above may be claimed complete (UI-21 rule:
   route→bind→visible→select→command→feedback, fail on engine exceptions).

No dose/radiation Core logic is affected: `DoseLedgerSystem`,
`DoseContentCatalog` and the accumulation contract are correct as shipped.
