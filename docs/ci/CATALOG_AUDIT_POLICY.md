# Catalog Audit Policy (`docs/ci/catalog_audit_policy.json`)

Read-only reference for the `catalog_audit` CI gate
(`ashfall-dev audit-catalogs`, `tools/gotools/pkg/catalogaudit`). The runtime
cross-reference authority remains `Ashfall.Core.CatalogIntegrityValidator`
(`data_integrity`); this policy drives a fast static pre-flight only.

## Fields

| Field | Meaning |
|---|---|
| `schema_version` | Policy format version (string). Must match the baseline's `schema_version`. |
| `description` | Human summary of what the policy governs. |
| `data_dir` | Catalog directory, relative to the repo root. |
| `id_naming_regex` | Canonical id regex (D03). |
| `unit_suffix_advisory_regex` | Non-failing advisory pattern for redundant unit suffixes (e.g. `*_10m_of_10m`). |
| `default_domain` | Domain assigned to a catalog that matches no `id_domains` glob. |
| `id_domains` | `{domain, globs[]}`. A file's **assigned** domain (first match) drives duplicate detection; reference targets use the union of all files matching a domain's globs. |
| `duplicate_id_allowlist` | Ids intentionally defined in 2+ id-domains (sorted, unique). Emitted as policy-acknowledged, never baselined. |
| `advisory_allowlist` | Advisory keys that `--fail-on-advisory` tolerates (sorted, unique). |
| `mirror_unresolved_max` | Ceiling on mirror ids without a primary author (`0` disables the ratchet). |
| `reference_rules` | `{source_glob, field, container?, target_domain, note?, allowlist?, allowlist_reasons?}`. |
| `reference_rules_excluded` | `{source_glob, field, reason}` for reference-shaped fields deliberately not enforced. |
| `schema_version_default` | Expected `schema_version` for catalogs not named in the map below. |
| `schema_version_expectations` | `{catalog: expected_int}` for non-default catalogs. |

## Reference rules

- `field` may be a scalar or an array of strings.
- `container` scopes the check to a named root array (e.g. `ammo`); a missing
  container fails closed.
- `allowlist` grandfathers values that are deliberately outside the target
  domain; `allowlist_reasons` documents each one. A stale allowlist entry is
  reported (and fails `--strict-stale`).
- A rule whose `source_glob` matches no catalog, or whose `target_domain` has no
  ids, fails closed.

## The two `lootCategories` contracts

Expedition destinations (`expeditions.json`) use **literal item ids**.
`locations_expansion3.json` uses **abstract category tags** and is listed in
`reference_rules_excluded`. Never add it to `reference_rules` without authoring
the item-id contract.

## Commands

```bash
tools/rstools/target/release/ashfall-dev audit-catalogs --root . --check --strict-stale
tools/rstools/target/release/ashfall-dev audit-catalogs --root . --list-checks
tools/rstools/target/release/ashfall-dev audit-catalogs --root . --dump-ids canonical_item
tools/rstools/target/release/ashfall-dev audit-catalogs --root . --update-baseline   # after review
```
