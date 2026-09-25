# Plan A1: Whole-Game P0 — Host Build-Green + CLI Action Parity Gate

STATUS: APPROVED BY USER

## 1. Goal & Outcome

**Goal (4 bounded outcomes):**
1. Establish the true build state of the current worktree for both compile targets (host + tests).
2. Remove the one real CLI catalog divergence found by measurement:
   the host calls the Plan-173 probe `RadioProductionSelfTest` while the declarative
   authority (`Assets/Ashfall.Core/HostCliRegistry.cs` + generated
   `docs/ci/SELFTEST_MANIFEST.json`) calls it `RadioProgramProductionSelfTest`.
3. Add one durable static parity gate so the ERR-01 class cannot recur silently:
   a dispatched probe/fixture that is dropped from (or never added to) the CLI
   catalog. Measured gap today: **26 dispatched host probes are absent from
   `SELFTEST_MANIFEST.json`** (they therefore never run in manifest-driven shard
   smoke runs, carry no time budget, and are not listed by `--list-selftests`).
4. Record the remaining divergence honestly as decision-backed debt (in `KNOWN_DEBT.md`)
   with exact counts and a promotion condition.

**Non-goals (explicit):**
- No gameplay, data, save, UI, panel, or tuning change.
- **No collapse of the two `HostCliAction` enums.** Merging the 240-member host enum
  with the 216-member Core catalog is a foreman-signed refactor (30 divergent members,
  descriptor authoring, generator updates). It is recorded as ACCEPTED debt instead.
- No touching of live claimed paths: `src/Main.PlayerSurfaces.cs`,
  `src/UI/GameDashboardPanel.cs`, `src/Main.PanelLifecycle.cs`,
  `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`, `src/Main.UiPanels.cs`,
  `src/Main.CampaignOwners.cs`, `docs/roadmap/**`, `scripts/ci/generate-plan-register.py`.
- No documentation-corpus cleanup (that is package B1).

## 2. Claimed Paths & Affected Files

**New:**
- `.ai/plans/wholegame-p0-build-green.md` (this file)
- `Ashfall.Core.Tests/HostCliActionParityContractTests.cs` (sibling of the existing
  `Ashfall.Core.Tests/HostCliHelpContractTests.cs`, same static-gate style)

**Edited (all unclaimed at claim time):**
- `src/Host/HostCli.cs` — 1 enum member rename + 1 parse return
- `src/Main.Application.cs` — 1 case label rename
- `KNOWN_DEBT.md` — 1 ACCEPTED row
- `INTEGRATION_PLANS.md` — package entry
- `.ai/state.md` — task state
- `WORKTREE_OWNERSHIP.md` — this claim row

**Read-only authorities used:** `docs/ci/SELFTEST_MANIFEST.json`,
`Assets/Ashfall.Core/HostCliRegistry.cs`, `scripts/ci/generate-selftest-manifest.py`,
`scripts/ci/generate-architecture-map.py`.

## 3. Pre-flight Evidence (measured 2026-09-25, this checkout)

- `dotnet build Ashfall.csproj` → **Build succeeded, 0 Warning(s), 0 Error(s)**.
  The red build reported in `docs/health/CODEHEALTH_SWEEP_2026-09-25.md` (ERR-01)
  and in the PFGL claim log is **stale**: it predates the 06:21/08:51/08:52 edits.
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore` → **0/0**.
- All 17 previously-dead probe flags (`--shelter-archive-selftest` … `--working-animals-selftest`)
  are parse-reachable again (2 occurrences each: parse + help).
- Host enum members 240 · Core enum members 216 · host-only 27 · core-only 3
  (`LogDirConfig`, `UserDataDirConfig`, `RadioProgramProductionSelfTest`).
- Dispatch coverage: 239/240 host members are referenced in `src/Main.Application.cs`
  (`Interactive` is the no-arg default, handled by fallthrough).
- Manifest coverage: 208 cataloged tests; **26 dispatched host `*SelfTest/*UiTest`
  actions are not cataloged**; every cataloged flag IS parsed (0 missing).
- Existing gate coverage: `HostCliHelpContractTests` proves Parse ↔ PrintHelp only.
  Nothing gates manifest ↔ host enum ↔ dispatch. `generate-selftest-manifest.py --check`
  compares the file against the Core registry only.
- Generated gates green at start: `generate-selftest-manifest.py --check` OK (208 tests);
  `generate-architecture-map.py --check` OK (267 subsystems).

## 4. Implementation Steps

1. Record the claim row in `WORKTREE_OWNERSHIP.md`.
2. Rename the host action to the authority name:
   `RadioProductionSelfTest` → `RadioProgramProductionSelfTest` in `src/Host/HostCli.cs`
   (enum + `Parse` return) and `src/Main.Application.cs` (case label). 3 references total.
3. Add `Ashfall.Core.Tests/HostCliActionParityContractTests.cs` with four tests:
   - `EveryManifestAction_ExistsInHostEnum_AndIsDispatched`
   - `EveryManifestFlag_IsParsedByHostCli`
   - `EveryHostEnumMember_IsDispatched` (`Interactive` documented as the no-arg default)
   - `UnmanifestedHostSelfTests_MatchDocumentedBaseline` — pins the exact 26-name
     baseline so the set can only shrink deliberately; each name carries the
     `DEBT-HOSTCLI-PROBE-MANIFEST-GAP` pointer.
4. Add the ACCEPTED debt row (26 probes + enum-catalog divergence + promotion condition).
5. Add the `INTEGRATION_PLANS.md` package entry and update `.ai/state.md`.

## 5. Verification & Termination Criteria

- [ ] `dotnet build Ashfall.csproj --nologo` → 0 warnings / 0 errors
- [ ] `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo` → 0/0
- [ ] `bash scripts/run_test.sh Ashfall.Core.Tests/HostCliActionParityContractTests.cs` → 4/4 PASS
- [ ] `bash scripts/run_test.sh Ashfall.Core.Tests/HostCliHelpContractTests.cs` → 2/2 PASS (no regression)
- [ ] `python3 scripts/ci/generate-selftest-manifest.py --check` → OK
- [ ] `python3 scripts/ci/generate-architecture-map.py --check` → OK
- [ ] `git diff --check` clean on every file this package touches
- [ ] No file outside section 2 is modified

## 6. Known Limitations (recorded, not hidden)

- The gate pins a 26-name baseline rather than forcing 26 catalog entries; authoring
  those Core descriptors belongs to the signed enum-merge package.
- The duplicate-enum divergence (30 members) remains until that package lands.
- No interactive/headless runtime probe is run here; this package is static-gate +
  compile verification only (runtime probes stay with their owning packages).
