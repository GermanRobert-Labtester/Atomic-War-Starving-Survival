# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# HostCli Self-Test Null-Hazard Repair + Silent-Bug Sweep — pkg 18 (2026-09-30)

STATUS: APPROVED BY USER
(Batch mandate: "Search for bugs, errors, warnings, silent bugs, missing
tool calls etc and diagnose plus repair" — 2026-09-30 session.)

## Diagnosis

Full rebuild warning inventory (host 14 unique, Core 0, tests 0):

- **7 × CS0162 unreachable** (ChronicCondition, Genealogy,
  PharmaceuticalTablet, ShelterMuseum, SurgicalGraft, SurvivorRoles ×2):
  constant-folded save-store contract-name checks — the compiler proves
  the `[FAIL]` else-branch dead because the SectionName/FileName fields
  are const. Benign; the dead branch is the intentional failure path.
  Left as-is, documented here.
- **7 × CS8602** (Barter 67, CaravanItemValue 57, CombatDoctrine 70,
  EconomyFamily 43, LoanShark 20, PlayerSurfaceManifest 36,
  VoluntaryRegister 49): all one defect class — self-test verbs that
  `Check(x != null)` (which only prints FAIL and continues) then
  dereference `x` anyway. On premise failure the probe dies mid-run
  inside the catch-all with the misleading "[FAIL] Unexpected probe
  exception" instead of per-check FAILs. 11 deref sites in total once
  the later repeated occurrences were included.

## Repairs (src/Host/HostCli.*.cs only)

- LoanShark: `debt == null ? null : …` for GetDebt/RepayDebt lookups;
  `debt != null &&` on ForgiveDebt.
- VoluntaryRegister: `state1?.entries.Count == 1`.
- PlayerSurfaceManifest: early-abort guard after Check 1 (manifest null →
  honest FAIL + return 1); guards on checks 2–5; `manifest!` on check 6.
- CaravanItemValue: early-abort guard after Check 1 (catalog missing →
  FAIL + return 1); check-4 empty-`real` guard before the `real!` uses;
  `var catalog = itemCatalog!` for lambda use (lambda closures see
  declared nullability, not flow state).
- CombatDoctrine: `liveCapability`/`?.` for the nullable
  DoctrineCapability derefs (checks 6 and 8).
- EconomyFamily: `buy != null &&` on check 5.
- Barter: `offer == null ? null : AcceptOffer(offer.OfferId, 3)`.

## Silent-bug / missing-wiring sweep (no changes needed)

- Swallowed exceptions: zero empty `catch { }` sites under `src/` or
  `Assets/Ashfall.Core/` (CatalogDiagnostics documents the historical fix).
- Event wiring: `OnDeepLinkRequested` → subscribed (Main.Campaign.cs:117);
  QuestDetailPanel `OnMoralChoiceSelected` is a documented standalone
  fallback (Main resolves via Bind only, no double-resolve);
  CaravanBarterLedgerPanel `OnSetActiveFaction` deliberately unwired
  (comment: would corrupt stance rail); GreenhousePanel `OnPlotSelected`,
  SilentFoundryPanel `OnProductSelected`, DutyRosterPanel
  `OnRoleSelected` are dormant notifications — selection behavior runs
  internally (`_selectedIndex` + `RefreshDetail()`) and other events are
  bound. No player-visible path drops input.
- PatrolEncounterIntegrity.cs(48) CS8602 left untouched — foreign
  uncommitted WIP (not this stream's file).

## Verification

1. `dotnet build Ashfall.csproj --no-incremental` — 0 errors; warnings
   reduced to the 7 benign CS0162 + 1 foreign-file CS8602.
2. All seven touched verbs run headless and PASS with no probe
   exceptions: barter 7/7, caravan-item-value 9/9, combat-doctrine 10/10,
   economy-family 9/9, loan-shark 7/7, player-surface-manifest 9/9,
   voluntary-register 11/11.
3. Headless boot: 0 script errors.
4. Pathspec commit (7 HostCli files + plan).
