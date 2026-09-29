# Plan 1 — Development and export build boundary

STATUS: APPROVED BY USER

Editorial revision: 2026-09-28. Existing scope and approval retained.

## Outcome

Ship the gameplay host without host selftest code while keeping the development
harness callable. Treat assembly size and startup time as separate measurements.

## Contract and sequence

1. Keep `AshfallIncludeSelfTests=true` for development; default it to `false`
   for `ExportRelease`. Define `ASHFALL_SELFTEST` only when enabled.
2. Exclude standalone host test sources and guard embedded test members. Preserve
   normal startup, argument/environment handling, help and version reporting.
   Core diagnostic enum names are not evidence of leaked host test code.
3. Verify both configurations. Run development probes against packaged JSON;
   inspect the exported assembly for specific host test types and methods.
4. Compare DLLs from the same source snapshot, configuration and RID, changing
   only the selftest switch. Record startup separately under matched conditions.

## Ownership

`Ashfall.csproj`, `src/Host/HostCli.cs`, `src/Main.Application.cs`,
`src/Main.UiHandlers.cs`, `src/Host/AssetRegistry.cs`,
`scripts/ci/export-build.sh`, `export_presets.cfg`.

Coordinate moved CLI probes with the host plan and embedded `Main.GameFlow`
drivers with the save plan. The integrator serializes builds and acceptance.

## Acceptance and evidence limits

- Development and export builds compile; development selftests resolve.
- Exported host test symbols are absent; packaged data passes development probes;
  the exported game boots through normal startup.
- Report bytes and startup duration independently. Different snapshots or RIDs
  cannot establish the reduction caused by excluding tests.
- Frame/startup evidence belongs in [the performance records](../../docs/perf/).
  Existing measurements do not establish a frame or startup speedup.

Use focused verification through `bin/run-scoped-tests` where applicable.
No full-suite run or commit is authorized by this editorial revision.
