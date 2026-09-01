# Research Failure Policy (Plan 34 §1.10, §34D.9)

The JSON catalog is the **sole authored research authority**. There is no hardcoded fallback.
Contract: a broken catalog fails loudly at the earliest gate, never silently at the player.

## Diagnostic matrix

| Condition | Detected by | Behavior |
|---|---|---|
| `research_knowledge.json` missing | `ResearchKnowledgeCatalogLoader.Load` (returns empty) | host warns: `ResearchHostSession.LoadCatalog` / `EnsureSharedResearch` emit an error/warning; `--research-catalog-selftest` FAILS; unit test `LoadAndRegister_MissingCatalog_RegistersNothing_NoFallback` pins zero-registration |
| malformed JSON | loader `catch` → `CatalogDiagnostics.Warn` | empty catalog + host warning + selftest FAIL |
| `schema_version` unsupported | loader container contract (v1) | parse failure path → diagnostics as above |
| duplicate node ID | `ValidateDag` | `--research-catalog-selftest` FAIL with node id |
| unknown category | not gated (UI metadata only — §34C.3: subjective quality is not a Tier-1 startup failure) | renders as-is |
| missing prerequisite | `ValidateDag` ("unresolved prerequisite") | selftest FAIL; unit tests pin |
| prerequisite cycle | `ValidateDag` 3-color DFS ("Cycle detected") | selftest FAIL; unit tests pin direct cycles |
| invalid cost (`days_to_complete ≤ 0`) | loader test `Load_AllNodesHaveValidIdAndDisplayName` (`daysToComplete > 0`) | unit-test gate |
| unknown `breakthrough_item` | `--research-catalog-selftest` cross-ref walk + `ResearchSaveIntegrationTests.Catalog_BreakthroughItemsResolve…` | FAIL naming node + item |
| relic `research_unlock_id` not in catalog | `--research-catalog-selftest` | FAIL; at runtime the workshop logs a warning and grants no fabricated node (fabrication path deleted, Plan 34) |
| manual/autopsy knowledge grant not in catalog | `--research-catalog-selftest` | FAIL naming source file |
| zero-node catalog | `--research-catalog-selftest` + host load warning | FAIL / explicit diagnostic — never an empty-but-quiet tree |
| catalog < 40 nodes | `--research-catalog-selftest` regression floor | FAIL (guards against content loss) |

## Failure semantics at runtime

- `StartResearch` on a node whose prerequisite is missing → `false` + engine warning log.
- `CompleteResearch` on an unregistered ID → `false` (state untouched).
- Save referencing an unknown active node → progress accumulates harmlessly; resumes when the
  node exists again. Unknown IDs are never dropped on capture.

## Verification commands

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj          # includes research parity/integration suites
godot --headless --path . -- --research-catalog-selftest          # catalog gate (Plan 34)
godot --headless --path . -- --data-integrity-selftest            # generic cross-catalog tiers (knowledge_/item_ refs)
```
