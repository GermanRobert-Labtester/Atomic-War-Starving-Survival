# ASHFALL 10-Loop Bug Audit

## 1. Audit Target

Player-facing Godot UI: panel inventory, route reachability, live-system binding, flagship-screen fidelity, and interaction lifecycle.

## 2. Scope

Read-only review of `src/UI/`, `src/Main*.cs`, `Assets/Ashfall.Core/UI/`, related Core systems, player-surface tests, and the existing 1280x800 snapshot suite. No production code was changed.

## 3. Baseline Verification

Baseline commit: `ccac926e986bce74295a255bc7f225ac52f01a02`.

The worktree already contained unrelated audio and documentation changes; they were not modified by this audit.

## 4. Loop Completion Matrix

| Loop | Focus | Candidates | Confirmed | Rejected |
|---|---|---:|---:|---:|
| 1 | Panel and route inventory | 4 | 2 | 2 |
| 2 | Core-to-UI interaction paths | 5 | 2 | 1 |
| 3 | Flagship console data authority | 33 | 30 | 3 |
| 4 | Action binding and persistence | 4 | 2 | 2 |
| 5 | Navigation and discoverability | 3 | 1 | 2 |
| 6 | Event subscription lifecycle | 5 | 5 | 0 |
| 7 | Snapshot/style conformance | 3 | 0 | 3 |
| 8 | Existing tests and gates | 4 | 3 | 1 |
| 9 | Save/determinism boundary review | 3 | 1 | 2 |
| 10 | Cross-system player-journey trace | 4 | 3 | 1 |

## 5. Executive Findings

The player UI is broad rather than sparse: 140 panel classes, 134 registered navigable panel IDs, and 29 visual regression targets already exist. The need is not a new general dashboard, map, inventory, garage, weather, or expedition panel.

The actionable design backlog is:

1. **One new flagship interaction panel:** a data-backed Moral Choice decision surface.
2. **One existing-surface UX upgrade:** a reachable, reopenable onboarding/guidance overlay.
3. **Thirty existing registered flagship consoles:** convert to real Core/host-backed experiences or remove from player routing; they must not be counted as shipped UI.
4. **Four live panels / five subscription paths:** correct event lifecycle before treating their views as stable.

The established visual language is worth preserving: 1280x800 dark graphite console, fine brass frames and grid rails, condensed uppercase amber headings, monospaced telemetry, and sparse cyan/red status accents. It has no need for generic cards, rounded SaaS controls, glass effects, or decorative generated imagery.

## 6. Critical Findings

None found in this review.

## 7. High Findings

### BUG-UI-001 — Moral choices cannot be resolved by a player

- **Status:** CONFIRMED
- **Active path:** Yes
- **Evidence:** `src/Main.MoralChoice.cs` loads catalog definitions, constructs and persists `MoralChoiceSystem`, and exposes `TryResolveMoralChoice` at line 91. The method has no call site under `src/`. `FactionsPanel` presents only a read-only “weight of choices” progression view.
- **Player impact:** Moral narrative choices can be authored, saved, and evaluated in Core but no Godot interaction can commit a selection.
- **Design implication:** One flagship Moral Choice panel/modal is required. It must present contextual stakes and outcomes without exposing the intentionally hidden numeric morality score.

### BUG-UI-002 — Thirty player-routable flagship consoles have no authoritative data contract

- **Status:** CONFIRMED
- **Active path:** Yes
- **Evidence:** All 30 are registered in `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`, instantiated in `src/Main.UiPanels.cs:703–819`, and made openable in `src/Main.PlayerSurfaces.cs:402–518`. Their 5,186 source lines contain hard-coded telemetry, feedback-only buttons, empty refreshes, or no action bindings. Twenty have no `Bind` method; searches found no corresponding Core system or host session.
- **Representative evidence:** `AnaerobicBiogasDigesterPanel` reports a true `IsBound` value while rendering literal temperature/pH values and feedback strings. `CryogenicPermafrostCorePanel` renders literal depth/temperature text; its four action buttons have no operational handlers.
- **Affected surfaces:** `AnaerobicBiogasDigester`, `SubterraneanCartography`, `UndergroundPrintingPress`, `SiliconIngotSlicing`, `GeothermalSteamTurbine`, `WarDogKennel`, `IsotopeSeparator`, `PlasmaArcSmelting`, `BoreholeSeismograph`, `HeavyLogisticsAirlock`, `CryogenicPermafrostCore`, `BasalRadonMigration`, `TraumaBondingCohort`, `ClandestineInsurgency`, `SubterraneanDebtLedger`, `SurfaceShrapnelAegis`, `LongWalkExpedition`, `SonicRuptureDrill`, `VaultDoorBreaching`, `IronCenotaphMemorial`, `AquiferTreatyConcession`, `CrossingSafeConductVouch`, `MechanicalProstheticsLathe`, `FungalProteinFermenter`, `UltrasonicDecontaminationAirlock`, `TroposphericRadioRelay`, `InductionCupolaFurnace`, `HeavyMarineDieselGenerator`, `SlurryDewateringSump`, and `MagneticDrumArchive`.
- **Player impact:** The navigation surface promises live game systems that do not exist. Additional console designs would multiply this false affordance.

## 8. Medium Findings

### BUG-UI-003 — Four panels leak or duplicate event subscriptions on rebind

- **Status:** HIGH-CONFIDENCE STATIC FINDING
- **Active path:** Yes when a panel is rebound
- **Evidence:** `WeatherHistoryPanel`, `GeigerCalibrationPanel`, `FireIncidentPanel`, and `TriangulationPanel` unsubscribe a newly created lambda rather than the delegate subscribed earlier. `TriangulationPanel` additionally leaves `OnLocationRevealed` subscribed.
- **Player impact:** Reopened/rebound panels can refresh more than once per event or retain stale handlers.

### BUG-UI-004 — The onboarding panel cannot be opened after startup

- **Status:** CONFIRMED
- **Active path:** Yes
- **Evidence:** `OnboardingHintPanel._Ready()` sets `Visible = false`; `SetupOnboarding()` constructs it and persists its state, but no player route, HUD affordance, or `Visible = true` call opens it. Its only broad interaction is being hidden by `CloseAllOverlayPanels`.
- **Player impact:** The system described as an always-available reference is unreachable.
- **Design implication:** Rework the existing overlay as a compact, reopenable `GUIDANCE // F1` experience rather than adding a duplicate tutorial screen.

### BUG-UI-005 — Several routed panels receive fresh/disconnected state instead of the campaign authority

- **Status:** CONFIRMED
- **Active path:** Yes
- **Evidence:** `Main.PlayerSurfaces.cs` opens `FireIncidentPanel` with `new ShelterFireHazardSystem()` and a literal incident id; it supplies new `FactionStanceEngine` instances to Faction Matrix/Narrative surfaces and a new `SkillProgressionSystem` to Skill Matrix. These are not campaign-owned live systems.
- **Player impact:** These panels can show empty/default/fabricated state despite appearing navigable.

## 9. Low Findings

None recorded.

## 10. Suspected Findings

The fire route's fresh system is initialized with a `String.GetHashCode()`-derived seed inside the panel path. Because the route currently carries no live incident, this is a latent determinism concern rather than a confirmed player-visible failure.

## 11. Rejected Candidates

- **Missing expedition vehicle/garage panel:** rejected. `ExpeditionPanel` already exposes vehicle selection, refuelling, estimates, cargo, and weapon-condition logistics.
- **Missing general dashboard/map/inventory/weather screens:** rejected. These already exist and are part of the snapshot suite.
- **Twenty-five unconfigured routes from initial static scan:** rejected. `Main.PlayerSurfaces.cs` configures expanded surfaces in a dynamic loop; 109 explicit calls plus that loop cover the 134 registered routes.
- **Style inconsistency across the sampled flagship screens:** rejected. Silent Foundry, Expedition Radar, Greenhouse, and Map Atlas consistently use the project console style.

## 12. Root-Cause Clusters

1. **Visual component mistaken for a gameplay capability:** thirty static console classes are registered before Core/host authority exists.
2. **Metadata gates overstate runtime health:** route and panel coverage gates establish descriptor/route presence, not that an action delegates into a live session.
3. **Anonymous event handler identity:** fresh lambdas prevent correct unsubscription.
4. **Reachability omitted from lifecycle design:** a persisted UI state is created but has no player-facing entry point.

## 13. Cross-System Chains

- Moral catalog JSON → `MoralChoiceSystem` → campaign save section → no host/UI choice resolver.
- Onboarding Core state → `OnboardingHintPanel` construction → hidden overlay → no navigation/HUD route.
- Static console class → registry → player route → hard-coded feedback/no Core authority.
- Campaign surface → rebinding → mismatched lambda unsubscribe → duplicated refresh/event propagation.

## 14. Tests and Gate Gaps

- `PanelRouteGateTests` proves emitted routes are registered; it does not prove all descriptors dispatch a live action.
- `PlayerSurfaceCoverageGateTests` treats setup metadata as binding coverage; it does not verify a real `Bind` into campaign state.
- No Godot player-journey test resolves a moral choice through the host/UI.
- No route/visibility test proves onboarding can be opened and reopened.
- `PanelBindLifecycleSelfTest` does not cover the four identified subscription paths.
- No gate rejects a routed panel that renders only literal telemetry or feedback-only actions.

## 15. Save, Load, and Determinism Review

Moral choice state is persisted through the campaign envelope, but it is operationally inert because no player action reaches `TryResolveMoralChoice`. The static consoles add no trustworthy persisted state. The fresh fire-system route is a separate ownership problem and should not become a second authority.

## 16. Security, Content, and Accessibility Review

No credential or external-data issue was observed. The existing console language has strong status color hierarchy, but generated designs must preserve text alternatives to color and readable high-contrast states for warnings, disabled choices, and irreversible actions.

## 17. Suggested Investigation Order

1. Establish the Moral Choice host/UI contract and design its decision surface.
2. Quarantine or classify the 30 unbacked consoles before adding new menu entries.
3. Trace ownership for Fire Incident, Faction Matrix/Narrative, and Skill Matrix.
4. Validate repeated open/rebind behavior for the four event-lifecycle panels.
5. Add a visible onboarding guidance entry and test its reopen path.

## 18. Evidence Index

- `src/UI/` — 140 panel classes; 29 existing snapshot targets.
- `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` — 134 registered navigable IDs.
- `src/Main.UiPanels.cs:703–819` — 30 console constructions.
- `src/Main.PlayerSurfaces.cs:402–518` — routed console actions; `:303`, `:342`, `:347`, `:352` — fresh/disconnected bindings.
- `src/Main.MoralChoice.cs:91` — uncalled moral-choice resolver.
- `src/UI/FactionsPanel.cs:315` — read-only moral/progression display.
- `src/UI/OnboardingHintPanel.cs` and `src/Main.Onboarding.cs` — hidden, unrouteable guidance overlay.
- `src/UI/WeatherHistoryPanel.cs`, `GeigerCalibrationPanel.cs`, `FireIncidentPanel.cs`, `TriangulationPanel.cs` — subscription identity defects.
- `src/UI/SnapshotHarness.cs` and `snapshots/*.png` — established flagship visual contract.

## 19. Audit Confidence

High. Counts were derived from source and registry inspection; the major gaps have direct source-level evidence. Event-lifecycle impact is high-confidence static analysis and should be exercised in a focused repeated-bind runtime test.

## 20. Audit Completion Statement

All ten required review loops were completed. This audit is read-only except for this report. No production code, data catalog, scene, or asset was modified.
