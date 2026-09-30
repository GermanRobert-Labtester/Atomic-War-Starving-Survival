# Plan 1 — Development and export build boundary

STATUS: APPROVED BY USER

Editorial revision: 2026-09-28. Existing scope and approval retained.

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no contract item and no acceptance
> criterion. **Acceptance and evidence limits** below remain binding and are the authority on what
> these measurements may and may not claim.

---

## 0. Framing — The Boundary

> *"A release is not what you built. It is what you agreed to let out of the building."*

Every game ships two binaries that share a name. One carries the development harness, the probes,
the selftests and the arguments that prove the thing works. The other carries none of it, and is
the only one a player will ever hold.

Plan 1 draws that line and then does something rarer than drawing it: it refuses to claim a
benefit it cannot isolate. **Different snapshots or RIDs cannot establish the reduction caused by
excluding tests** — so bytes and startup duration are reported independently, and a frame or
startup speedup is explicitly *not* established. That sentence is the most professional line in
this plan.

**Tone & register.** Workshop-plain, boundary-conscious. The vocabulary is the build: *assembly,
switch, RID, snapshot, symbol, probe, guard*. Prose should read like a machinist's note about what
was left in the crate and why.

**The interesting boundary.** `ASHFALL_SELFTEST` is defined only when enabled — which means the
build either *has a self* or does not. And the plan's sharpest observation: **"Core diagnostic enum
names are not evidence of leaked host test code."** A name is not a body. The plan knows the
difference and insists on it.

**The second layer.** A release is an act of withholding, and this plan is the etiquette of
withholding done properly: it removes the harness, keeps the ability to prove things, and then
declines to claim the one benefit everyone expects it to claim. Boundaries drawn this honestly
tend to hold.

**Texture (second prose pass — commentary only).**

- "The crate left the building with the probes removed and the ability to prove things intact." That is the whole trade.
- `ASHFALL_SELFTEST` is defined only when enabled — the build either *has a self* or does not. No third state, no half-light.
- "A name is not a body" is said twice in this file, in writing. Repetition is how a plan teaches its readers not to argue.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, contract item, acceptance criterion
or register row changes. The register below is unchanged.)*

- Two binaries share a name, and only one of them will be held. The boundary is drawn by
  subtraction — what the crate must *not* contain — which is how every honest release is defined.
- The refusal to claim an isolated benefit is the professional line of the file: bytes and startup
  are reported apart, and the speedup is *not established*. An unproven claim is a debt with
  interest.
- `ASHFALL_SELFTEST` means the build either has a self or does not — and a name is not a body:
  diagnostic enum names in Core prove nothing about leaked host code.

> "A release is what you agreed to let out of the building. The agreement is the artefact."

---

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

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's own **evidence limits**. They are not defects; they are the honest edge of
what a build-boundary change can prove. Any later pass that resolves one must re-measure.

| # | Open item | What is actually known | Who may resolve it (later, re-measured) |
|---|---|---|---|
| PB-OM-1 | Did excluding selftests make the game faster? | **Not established.** Bytes and startup duration are reported independently by design. | A matched-snapshot capture changing only the switch. |
| PB-OM-2 | How much smaller is a release *because of* the exclusion? | A byte delta is recordable; attribution across different snapshots/RIDs is explicitly refused. | Same source snapshot, same configuration, same RID. |
| PB-OM-3 | Are there other leaks of development code into release? | The plan inspects for **specific** host test types and methods. Absence of those is not absence of all. | A symbol-level audit with a stated inventory. |
| PB-OM-4 | What are `Core diagnostic enum` names for? | Explicitly ruled **not** evidence of leaked test code. Their purpose is not restated here. | The Core owner, if diagnostics are ever documented. |
| PB-OM-5 | Does the exported game behave identically? | It **boots through normal startup** and passes packaged-data probes. Behavioural parity is not asserted. | A paired play-path comparison. |
