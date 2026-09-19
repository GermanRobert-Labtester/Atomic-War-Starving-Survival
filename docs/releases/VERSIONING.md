# ASHFALL — Versioning & Compatibility Policy

> **Authoritative Specification**: Defines the three-axis versioning architecture, semver mappings,
> save support windows, and version equality rules for ASHFALL.

---

## 1. The Three Version Axes

ASHFALL maintains three independent, cooperating version axes:

| Axis | Representation | Primary Authority | Synchronization | "Compatible" Definition |
|---|---|---|---|---|
| **Game** | Semver `MAJOR.MINOR.PATCH` | `project.godot` (`application/config/version`) | Mirrored to `Directory.Build.props` (`<VersionPrefix>`) and `export_presets.cfg` (`application/file_version`, `application/product_version`) via `scripts/release/set_version.py` | A save written by game version X loads on version Y per save-axis rules. |
| **Data** | Integer `schema_version` | Individual catalog JSON headers in `Assets/StreamingAssets/Data/*.json` | Inventoried live by `VersionReport.ScanDataSchemas` | A build accepts catalogs whose `schema_version` is less than or equal to the maximum supported by its loaders. Catalogs newer than the build are rejected with a file-specific error. |
| **Save** | Integer versions per store + `manifestVersion` | Codec constants (`CurrentSaveVersion`, `MigrationFromVersion`), `CampaignEnvelopeBuilder.CurrentEnvelopeVersion` | Validated by `SaveSupportWindowTests` and `VersionReport.SaveSchemaVersions` | Store loads if on-disk version is between `MigrationFromVersion` and `CurrentSaveVersion`. Aggregate save loads if `manifestVersion` is 1 (migrates in-memory) or 2 (current). Future versions are refused, never truncated. |

---

## 2. Project Semver Mapping

In ASHFALL, a breaking change is a lost or corrupted campaign. The semver mapping reflects this reality:

- **MAJOR (`X.0.0`)**:
  - Any change that renders previously loadable saves unreadable (e.g. bumping envelope `manifestVersion` without an automatic in-place migration path).
  - Dropping support for an established codec migration floor.
  - A data-schema change that rejects catalogs accepted by prior builds (`MOD_CONTRACT` breaking change).
  - Removal of published CLI commands or primary player routes.

- **MINOR (`X.Y.0`)**:
  - New content, game systems, and player reachability.
  - Additive save-schema evolution (bumping a codec's `CurrentSaveVersion` **with** deterministic migration from every previously supported version).
  - Introduction of a new registered save section.
  - Additive data-schema evolution (bumping `schema_version` with backward-compatible defaults).

- **PATCH (`X.Y.Z`)**:
  - Pure bug fixes, performance improvements, visual tuning, and documentation.
  - **Zero save-schema constant or DTO shape changes.**
  - **Zero data-schema version changes.**
  - **Zero public contract breakage.**
  - This is the hotfix class governed by `docs/releases/HOTFIX.md`.

---

## 3. Version Authority & Synchronization

1. **Single Runtime Source**: `project.godot` (`application/config/version`) is the runtime authority read by Godot and baked into exported PCK binaries.
2. **Build Mirroring**: `Directory.Build.props` (`<VersionPrefix>`) and `export_presets.cfg` (Windows desktop `file_version` and `product_version`) must match `project.godot` byte-for-byte.
3. **Automated Synchronization**: Versions are updated exclusively via `scripts/release/set_version.py` (called by `prepare-release.sh` and `hotfix.sh`). Manual edits to one file without the others violate the `version_gate` CI gate.
4. **Never Unknown**: `HostCli.PrintVersion` strictly validates the version string using `ReleaseVersion.TryParse`. Any empty, missing, or non-semver version renders explicitly as:
   ```
   game         : INVALID (config/version missing or not semver)
   ```
   An invalid version fails CI and blocks release exports.

---

## 4. Save Support Window Policy

A release guarantees:
1. **Proven Fixture Compatibility**: Every release must successfully load and round-trip every committed fixture in `artifacts/golden_saves/` and `artifacts/golden_saves/historical/`.
2. **Deterministic Envelope Migration**:
   - `manifestVersion: 1` loads and automatically migrates to `manifestVersion: 2` in memory upon load. It is rewritten to current format on the next save.
   - `manifestVersion: 2` loads directly as current.
   - Any other `manifestVersion` is refused with an explicit error and quarantined without truncation.
3. **Lossless Codec Migration**:
   - Every versioned save store migrates deterministically from its documented `MigrationFromVersion` to `CurrentSaveVersion`.
   - Checksum-envelope sections verify SHA-256 integrity and quarantine corrupt files to `.corrupt-*` without destroying existing data.
4. **Window Modification**:
   - The save support window may only be narrowed during a **MAJOR** release.
   - Any retirement of a supported version must be explicitly documented in `CHANGELOG.md` under `### Save & data compatibility` and accompanied by the removal of the corresponding historical fixture.
