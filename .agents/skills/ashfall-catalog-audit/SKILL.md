---
name: ashfall-catalog-audit
description: Performs a deep read-only audit of ASHFALL JSON authority, cross-file references, schema versions, naming, ranges, duplicates, and orphan candidates beyond the standard catalog selftest. Use when data changes or catalog integrity needs forensic review.
---

# ASHFALL Catalog Deep Auditor

## Boundary

`CatalogIntegrityValidator` and `--data-integrity-selftest` are the baseline.
This skill investigates gaps they do not claim to cover; it does not replace
them or create a second data authority.

## Workflow

1. Inventory JSON under `Assets/StreamingAssets/Data/` and identify the
   authoritative file for each definition family.
2. Run the baseline data-integrity self-test and record its result.
3. Parse current JSON and inspect cross-file references, including item,
   recipe, location, faction, survivor, quest, event, flag, and expansion
   references where those keys actually exist.
4. Check `schema_version`, consistent property naming within each contract,
   duplicate IDs, range ordering, null/empty semantics, and suspicious orphan
   definitions. Treat heuristic orphan results as candidates until reachability
   is confirmed in code or narrative data.
5. Compare against the current loader/DTO contract. Do not assume snake_case
   is safe if a loader currently expects another shape; report the migration
   seam.
6. Classify findings as validator-covered, new confirmed, heuristic, or
   documentation-only. Propose mechanical fixes separately from gameplay or
   narrative decisions.

## Rules

- Read-only by default; never mass-rewrite JSON from this skill.
- Never invent IDs or silently normalize data.
- Hand approved schema changes to `ashfall-data-schema`, new definitions to
  `ashfall-data-add`, and code/loader changes to `ashfall-implement`.
- Keep fictional-world content rules in force and cite the existing compliance
  test when relevant.
- Use `godot --headless --path . -- --data-integrity-selftest` and dotnet tests
  as applicable; no Unity tooling.

## Output

Return file counts, baseline result, reference matrix, schema/naming findings,
orphan candidates with confidence, and a minimal remediation backlog. If a
report is requested, use `docs/data/CATALOG_DEEP_AUDIT.md` without overwriting
an existing owner’s report.

## Quality gate

- Every confirmed broken reference has source and target file evidence.
- Heuristics are not presented as failures.
- Existing validator coverage and newly discovered gaps are clearly separated.
