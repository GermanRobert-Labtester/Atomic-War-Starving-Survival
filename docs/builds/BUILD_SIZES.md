# ASHFALL — Build Sizes (regression table)

All rows measured from the same checkout on 2026-09-25 (Linux/X11 preset, Godot
4.7.1.stable.mono). Update this table whenever a release export is produced and compare
against the previous row; investigate any growth above ~5% that is not explained by an
intentional content wave.

| Date | Platform | Binary (B) | PCK (B) | Loose data (MB) | Boot wall (s) | Notes |
|---|---|---|---|---|---|---|
| 2026-09-25 | Linux/X11 | 73,627,864 | 272,116,284 | 26 | 3.51 | Alpha packaging pass; 425/425 data catalogs stored in PCK; loose-data deployment verified PASS |
| 2026-09-25 | Linux/X11 | 73,627,864 | 287,571,036 | 26 | — | `scripts/tools/ashfall-package-verify.sh` PASS: isolated PCK-only verification 425 catalogs / 0 errors. PCK +15.5 MB vs previous row = faction emblem wave (98 emblems into `assets/ui/Icons/`) + portrait wave in progress; Windows export skipped (release template not installed) |
| 2026-09-25 | Linux/X11 | 73,627,864 | 294,172,544 | 26 | 3.51 | Two-platform package verify PASS; zip `dist/ashfall-alpha-linux-x86_64-20260925.zip` 338.4 MB + `.sha256` |
| 2026-09-25 | Windows Desktop | 109,359,104 | 294,529,552 | — | — | Windows templates installed; export + **wine smoke PASS** (425 catalogs, 0 errors); zip `dist/ashfall-alpha-windows-x86_64-20260925.zip` 349.0 MB + `.sha256` |
| 2026-09-26 | Linux/X11 | 73,627,864 | **128,358,532** | 26 | — | **Download-budget pass (task 4):** PCK 294.2 MB → 128.4 MB. (a) `assets/ui/reference/*` dev reference excluded from all presets; (b) faction icons `compress/mode=2` + `size_limit=256` (referenced ctex 15.2 → 8.1 MB, 68/68 emblems resolve); (c) `assets/art/*.jpg` (1,853 imports) switched to lossy (`compress/mode=1`) at full dimensions after `size_limit=256` was found to resize item art and drift `market_default`; (d) trim policy is now a script — `scripts/tools/trim_texture_imports.py` (idempotent, `--dry-run`, wired into `art_wave_closeout.sh` before the import step). Snapshots rebaselined 32/32 (`snapshot_rebaseline.sh --apply`) for compression-only drift; layout drift zero. |

Reference points:

* Items in the PCK grow with art waves (faction emblems 98/98, portraits in progress) and
  data catalogs (427 as of the F4 hotspot policy).
* `--asset-registry-selftest` coverage is the canary for missing art, not size.
* Re-run the export-smoke recipe in `docs/builds/EXPORT_REPORT.md` §5 after every row.
