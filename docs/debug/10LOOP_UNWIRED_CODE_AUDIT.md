# ASHFALL 10-Loop Bug Audit — Unwired & Dead Code (whole repo)

## 1. Audit Target

Repository-wide scan for **unwired code** (implemented systems whose feeder/consumer chain is broken),
**dead code** (loaders/systems with zero production reach), and **misswiring** (connected to the wrong
authority). Follow-up action requested by the requester: wire confirmed gaps correctly.

## 2. Scope & Method

- Full-tree pattern scans (2026-09-02 working tree, HEAD `7738facc`): `RegisterDefaults` definitions
  and callers; `LoadAndRegister`/`Load` declarations vs production call sites; Core `event` declarations
  vs any `+=` subscriber (Core+src+tests); `SaveSectionRegistry` ↔ `CaptureSection`/SaveMethod symmetry;
  content-utilization selftest orphan report; targeted file reads for every candidate.
- Every candidate re-verified with raw `grep -rln` across Core+src+Tests before confirmation. Two
  scan-method defects were caught and corrected mid-audit (see §11) — the raw-grep pass is authoritative.

## 3. Baseline Verification

- `dotnet build` tests + host: 0 errors. Suite green at time of audit (post-`7738facc`).
- `--content-utilization-selftest`: 458 catalogs — 118 gameplay-consumed, 279 codex-only, **0 orphaned**, 36 unresolved (mostly new concurrent-stream files), 25 exempted.
- `--data-integrity-selftest`: PASS, 0 findings (172 catalogs) at audit start.

## 4. Loop Completion Matrix

| Loop | Lens | Candidates examined | Confirmed | Rejected |
|---|---|---|---|---|
| 1 | Structural/static | RegisterDefaults, fake-fallback returns, dead methods | 3 | 1 |
| 2 | Call-graph reachability | 12 LoadAndRegister loaders, host-session construction graph | 7 | 5 (regex false positives) |
| 3 | State transitions | CaptureState unknown-ID handling (prior Plan 34 work re-verified) | 0 | — |
| 4 | Save/load | 69 registry sections vs 68 literal + 1 variable-keyed captures; SaveMethod existence | 0 (clean) | 1 (onboarding) |
| 5 | Determinism | RNG/dict-order spot checks on scanned systems | 0 new (Invariant-4 already gated by suite) | — |
| 6 | Data/catalog | loader↔JSON↔consumer triads for every dead candidate | 6 | 2 |
| 7 | Events/lifecycle | ~95 raised-but-unsubscribed Core events | 0 defects (architectural note) | ~95 |
| 8 | UI/player-facing | host-session refresh machinery vs system events (6 sampled) | 0 | 6 |
| 9 | Test adversarial | test-only loaders, tests proving dead code works | 1 (fake-count test gap → added honesty test) | — |
| 10 | Cross-system synthesis | full chains: craft→specialty→mastery→trade; catalog→loader→host→UI | 2 (BUG-01 chain, BUG-02 authority) | — |

## 5. Executive Findings

The dominant defect class is **"loader landed, feeder call never wired"**: catalogs + loaders + tests
exist and pass, but no production code ever invokes the loader — so the (often fully wired) downstream
system runs on nothing. This shipped three times: regional-treaty (fixed by 25G.7), research knowledge
(fixed by Plan 34), and trade specialties (fixed by this audit). A permanent source-scan gate
(`LoaderWiringGateTests`) now fails CI for any loader without a production call site or a dispositioned
allowlist entry.

## 6. Critical Findings

None. (No save corruption, determinism, or crash-class defects found in this audit's scope.)

## 7. High Findings

### BUG-01 — Trade-specialty loop: fully wired system, never fed (FIXED this audit)
**Severity:** High · **Confidence:** CONFIRMED · **Category:** INTEGRATION + DATA · **Active Runtime:** YES (before fix)
**Trigger:** any craft in the live game.
**Expected:** crafting matching items progresses a survivor's pre-war trade specialty (milestones → mastery → skill/morale/narrative hooks).
**Actual:** `TradeSpecialtySystem` was fully wired in `Phase0HostSession` (construction, `CraftItem`, events subscribed, save via `tradeSpecialty` field, UI status strings) — but `TradeSpecialtyCatalogLoader.LoadAndRegister` had **zero production callers** and `RegisterProfessionPatterns` is only invoked inside that loader. The system ran patternless; `OnItemCrafted` always returned at the `ProfessionItemCategories` gate; mastery was unreachable. The loader also masked the gap with a fake fallback: `return 4; // default hardcoded count` on missing data.
**Root cause:** loader landed with tests (16 professions proven) but the feeder call was never wired into the host; the fake count made the absence invisible even to callers who checked counts.
**Evidence:** grep — `TradeSpecialtyCatalogLoader` referenced only by its own file + 1 test; `RegisterProfessionPatterns` only at `TradeSpecialtyCatalogLoader.cs:113`; `trade_specialties.json` (16 professions) present and load-tested.
**Fix applied (this audit):** loader now returns honest `items.Count`; `Phase0HostSession.LoadTradeSpecialties(dataDir)` added (mirrors `LoadPhantomRules`); called from `Main.SetupPhase0`; honesty test `LoadAndRegister_MissingCatalog_ReturnsZero_NeverFakeDefault` added. 18/18 specialty tests green.
**Remaining gap (documented, needs design):** production craft **attribution** — `CraftingSystem` crafts carry no survivor/profession, so nothing in production calls `Phase0HostSession.CraftItem`; the only caller is a debug button (`OnPhase0CraftClicked` → hardcoded `"elena_vasquez"/"machinist"`). Correct producer wiring needs workbench→survivor assignment design (duty-roster consult). The debug button now at least demonstrates a live loop.
**Save impact:** none (specialty state already persisted inside the `phase0` section). **Determinism impact:** none.

### BUG-02 — Radio stations: last gameplay-catalog `RegisterDefaults` dual authority (REPORTED, not fixed)
**Severity:** High (authority fork risk) · **Confidence:** CONFIRMED · **Category:** INTEGRATION
**Actual:** `RadioStationCatalog` ctor calls `RegisterDefaults()` — 6 canonical stations defined in Core while `radio.json` (237 lines changed by the concurrent stream on 2026-09-01) is the data authority for broadcast content.
**Why not fixed here:** `src/Host/RadioHostSession.cs` + radio JSON were modified by an active concurrent stream the same day (commit `7738facc`, "Plan 50 — radio distress signals"); touching the seam risks clobbering in-flight work. Scheduled as **batch2 Task B1** with the full Plan-34 method (parity fixture → JSON authority → delete defaults → gate).
**Note:** the catalog's station *state overrides* already save/restore via the `radio` section — only the base definitions are hardcoded.

## 8. Medium Findings

### FINDING-03 — Four fully-dead loader+system pairs (missing features, wired plans specified)
**Confidence:** CONFIRMED (zero references outside their own files, zero tests)
| Loader | System | Data file | Suggested wiring venue |
|---|---|---|---|
| `AtmosphereCatalogLoader` | `AtmosphereTextSystem` (254 ln, read-only `GetTextForLocation*`) | `environmental_atmosphere_expansion.json` | flavor text venue decision (briefing collector / expedition journal line) |
| `EnvironmentalTextCatalogLoader` | `EnvironmentalTextSystem` (174 ln, same shape) | `environmental_texts_expansion_05.json` | same venue decision as above |
| `DebtTemplateCatalogLoader` | `DebtTemplateCatalog` | `ledger_debt_templates.json` | economy holdfast-trade debt sessions |
| `HoldfastNpcCatalogLoader` | `HoldfastNpcCatalog` | holdfast NPC JSON | holdfast quest loops (B11) |
These are **missing features, not defects**: each needs a product decision on where its content surfaces before wiring. All four are allowlisted with dispositions in `LoaderWiringGateTests` so they stay visible instead of rotting silently.

## 9. Low Findings

### FINDING-04 — Designed-dormant content correctly unwired (no action)
`SkyLayerArmorSystem` = "Expansion 11 — THE ORBITAL HARROW" (overhead armor, rad attenuation);
`SpiritualMeaningCoordinator` = "Plan 30". Both with loaders, zero production reach — **by design**
(extensions not yet activated). Allowlisted with dispositions.

### FINDING-05 — Concurrent stream's collectibles catalog unwired (observation)
`CollectibleCatalog` + `CollectibleCatalogLoader` + 40 items + `collectibles.json` + 230-line test
committed in `7738facc` with zero production wiring. Presumed in-flight work by that stream; **do not
touch from this stream** (allowlisted with that note).

## 10. Suspected / Needs Reproduction

- 36 "Unresolved" classifications in the content-utilization report — spot checks suggest new
  concurrent-stream catalogs the scanner hasn't been taught; re-run the report after the stream lands.
- `Main.Phase0.cs` debug-button handlers (`OnPhase0CraftClicked` etc.) write to `_statusLabel` —
  legacy Phase-0 demo surface; confirm it is dev-only UI or retire it (cosmetic; not audited further).

## 11. Rejected False Positives (with reasons — useful for future audits)

1. **Onboarding save loss** — `SaveOnboarding()` calls `CaptureSection(section, …)` with a *variable* key; fully wired incl. `SaveAll` + `FlushOnboardingIfDirty`. My literal-key scan missed it. Clean.
2. **~95 raised-but-never-subscribed Core events** — sampled six host sessions (DoseLedger, Waystation, Maritime, Journal, …): each runs its own `RaiseStateChanged`/dirty machinery, so the events are redundant signal, not stale UI. Architectural consistency question for the event-surface audit (batch1 Task B12), not per-event bugs.
3. **First loader-scan results** — my regex attributed `LoadAndRegister` to the first `class` in each file (the wire DTO), producing 11 bogus "dead" loaders; corrected by backward class resolution.
4. **Path-filter bug** — `'/src/' in path` never matches `src/...` paths, silently emptying the production-reference set in two scans; corrected with raw `grep -rln` re-verification. **Method lesson recorded: never trust a hand-rolled path filter over a plain repo-wide grep.**

## 12. Root-Cause Clusters

- **Cluster A — unfed loaders (3 historical occurrences, now gated):** catalog+loader+tests land; host call never does. Guard: `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs`.
- **Cluster B — silent fallbacks masking A:** fake counts / `RegisterDefaults()` fallbacks make unfed loaders *look* healthy. Guard: Plan-34 loader discipline (honest 0 + diagnostics); the specialty fake-count was the last known instance (sweep found no others: `grep "return [0-9]+; //"` over loaders = clean).

## 13. Cross-System Failure Chains

- **craft → `CraftingSystem.OnCraftCompleted` → (nothing)** and **debug button → `CraftItem` → patternless system**: BUG-01's chain; the attribution design (workbench assignment) closes it end-to-end.
- **`radio.json` content authority ↔ hardcoded station base**: BUG-02's fork; resolves when B1 lands.

## 14. Test Coverage Gaps

- No test previously asserted loaders are *reached* from production (now: `LoaderWiringGateTests`).
- No test previously asserted loader count honesty (now: specialty missing-catalog test; generalize if other fake-counts appear — sweep found none).

## 15. Migration/Legacy Risks

- `Assets/_Game/` is deleted; no legacy-tree findings. Unity `.meta` siblings in `StreamingAssets/Data` are inert.

## 16. Save/Determinism Findings

- Save symmetry audit: **clean** — 69 registry sections, 68 literal + 1 variable-keyed captures, zero orphan captures, zero missing SaveMethod implementations, zero captured-but-unregistered keys.
- No determinism findings in scanned systems (Invariant-4 already suite-gated).

## 17. Recommended Investigation Order

1. ~~BUG-01 fix~~ (done this audit) → design craft **attribution** (workbench assignment) to close the specialty chain end-to-end.
2. **BUG-02** via batch2 Task B1 (radio JSON authority) — after the concurrent stream's radio work settles.
3. FINDING-03 flavor/debt/holdfast wirings — each needs its venue decision first (batch2 B16 briefing collectors are the natural host for atmosphere/environmental text).
4. FINDING-05 — let the collectibles stream land its own wiring; re-run the loader gate after (it will force the disposition to resolve).
5. Optionally extend the gate's idea to `Load(`-style loaders once (deliberately not done now: high false-positive risk via wrapper methods).

## 18. Evidence Index

- Scans: this session's greps + python passes (RegisterDefaults, LoadAndRegister graph, events, save symmetry, fake-fallback).
- Key files: `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`, `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs`, `src/Host/Phase0HostSession.cs`, `src/Main.Phase0.cs`, `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`, `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs` (new).
- Baseline commit at audit start: `7738facc`.

## 19. Audit Confidence

High for findings 01–05 (raw-grep verified, read+confirmed at call sites). Medium for the "36 unresolved" note (not itemized). The repo is large; this audit covered the systematic unwired-code classes, not every subsystem's internal logic.

## 20. Audit Completion Statement

All 10 loops executed; candidates falsified where evidence contradicted them; two scan-method defects in the audit tooling itself caught and corrected; fixes limited to the confirmed defect (BUG-01) plus the permanent gate; no unrelated code touched; baseline commit `7738facc` recorded.
