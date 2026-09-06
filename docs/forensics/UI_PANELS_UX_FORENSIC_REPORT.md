# 1. Target

Deep audit of missing, stubbed, copied/relabelled and over-shared Godot UI panels, plus the UX consequences of broken routes, false state, inert controls, layout and lifecycle defects. Requested by the project owner, including a complete finding register in AGENTS.md. Audited working tree: **2026-09-05**.

The standard is a genuinely domain-specific player workflow: **state → blocker → cost → consequence**, driven by its canonical Core owner. A new class name, different heading, color variant, typed field or screenshot is not a completed panel. Shared theme primitives are legitimate; copying one domain's body/data/actions into an unrelated domain is not.

Scope: full registry/configuration reconciliation and a 178-file panel-source inventory across src, deeper action/state tracing of all suspicious families, shared navigation/lifecycle/components, representative stored screenshots, and headless probes. This is not a claim that 178 screens received a fresh visual playthrough.

Artifacts: [full inventory](UI_PANELS_UX_INVENTORY.md), [accessibility detail](../ui/ACCESSIBILITY_REPORT.md), and the same UI-01–UI-23 finding register in [AGENTS.md](../../AGENTS.md).

# 2. Executive Finding

**The owner's concern is substantiated.** Several different systems have been given almost the same panel body without a genuine task-specific implementation. The clearest example is Research/Standing Record/Muster Atlas: all three show the same fixed faction/trust data and dossiers under different labels. Other families are effectively a label, a multiline readout, or a telemetry mockup with fake-success text.

The most important counts, without double-counting overlapping categories:

| Evidence set | Result |
|---|---:|
| Inventoried panel source files, including four UI-named expansion classes | 178 |
| Registered descriptors | 141 |
| Declared Live / shelved Prototype | 112 / 29 |
| Configured IDs not registered at all | 23, covering 22 classes |
| Live descriptor with no host action assignment | plans_110_113 |
| Near-identical hardcoded three-column prototype shells | 19 |
| Additional fake-success prototype consoles | 10 |
| Typed one-label stubs | 11 |
| Partial expansion readouts lacking management controls | 11 |
| Atlases with inert action bars | 6 |
| Wave 6 panels present but missing production Bind/player routing | 4 |
| Concurrent Plans 146–149 panel additions, still partial at inspection | 4 |

There are **23 confirmed finding groups** below. No save-corruption or deterministic-simulation failure is claimed merely because UI is missing. The most urgent defects are false domain data, wrong/inert commands, advertised dead routes, constructor exceptions and unreadable/unreachable navigation.

# 3. Evidence Summary

- Source/data authority took precedence over plans, closeout documents and generated coverage percentages.
- Reconciled every bootstrap descriptor against literal ConfigureActions calls and the expanded-ID loop; checked the only registration authority and OpenPlayerPanel rejection behavior.
- Inventoried public Bind signatures, always-true binding flags, empty refreshes, constructor shapes, events, direct Main references, and structural clone groups. Each suspicious family received source-level confirmation; regex shape alone is not a verdict.
- Traced canonical host setup, command handlers, StateChanged/LastEvent and save capture seams for missing surfaces. Searched exact names and domain equivalents before labeling gaps.
- Inspected stored Research, Standing Record and Muster default screenshots at 1280×800. They corroborate relabelled layouts and right-edge clipping. They are **historical/unbound fixtures**, not a fresh bound production rendering. Current bound-path source independently proves the fixed-data problems.
- Headless decon UI construction produced 39 managed exceptions/missing-node error entries in the relevant groups (18 missing-node + 18 null-reference + 3 disposed-object reports) before a PASS marker. These are diagnostic failures despite the smoke verdict. Shutdown leak messages are additional and are not included in that count.

Evidence: src/UI; src/Main.UiPanels.cs; src/Main.PlayerSurfaces.cs; src/Main.GameFlow.cs; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs; audit logs summarized in §12.

# 4. Architecture Placement

| Layer | Actual responsibility | Relevant seam |
|---|---|---|
| Assets/Ashfall.Core | State, rules, domain commands, catalogs and persistence DTOs; engine-agnostic | Existing domain systems; UI/PanelRegistry; save registry |
| Assets/StreamingAssets/Data | Authoritative authored definitions | Domain catalogs, not fixture values embedded in panels |
| src/Host and other host-domain directories | Setup/projection, command results, change notifications, capture/load adapters | Typed sessions, LastEvent, StateChanged |
| src/Main.* | Composition, route registration, binding, action adapters, tick and save orchestration | RegisterPlayerSurfaces; BuildUserInterface; Setup/Save methods |
| src/UI and other presentation directories | Godot layout, readable state, selection and input | Bind, RefreshView, OnActionRequested, Close |
| Tests and snapshots | Evidence of specific contracts only | xUnit, headless probes, scene contracts, stored render fixtures |

No Unity editor or tools were invoked. No gameplay, data, scenes, production UI or save formats were changed. Google Stitch was not invoked because this is an audit, not a request to generate or publish designs.

# 5. Current Implementation

The [inventory](UI_PANELS_UX_INVENTORY.md) enumerates every panel in scope and every descriptor. Do not infer maturity from naming:

- **STUB:** UI-05/UI-06 prototypes and UI-07 single-label panels.
- **PARTIAL / misleading:** atlas copies, fake action strips, partial expansion readouts and concurrent Plans 146–149 screens.
- **PORTED_NOT_WIRED / PARTIAL:** Wave 6; ChemicalLab; Electrostatic and Geothermal have some real binding/work but incomplete normal routing/events.
- **LIVE_GODOT but UX debt:** existing pooled consoles and many legitimate domain panels share working command owners; the report does not relabel these as stubs merely because they share AshfallDashboardShell.
- **Missing dedicated surface:** the four industrial domains behind plans_110_113; backend existence is separately established in Main.Plans110_113.

EXISTS ≠ COMPILES ≠ WIRED ≠ EXECUTES ≠ PLAYER-FACING ≠ VERIFIED.

# 6. Runtime Wiring

Expected path: player entry → descriptor → dependency setup/Bind → Open → selected domain target → command → Core state/result → host notification → refreshed feedback.

Observed breaks:

1. Twenty-three ConfigureActions calls target absent descriptors and silently fail (UI-09).
2. plans_110_113 resolves a Live descriptor but reaches MISSING ACTIONS (UI-10).
3. Wave 6 has constructed widgets and handlers without production Bind or registered entry (UI-11).
4. Specialist implementations are stranded or duplicate-constructed (UI-13).
5. New Plans 146–149 constructs/binds but has OPEN-only handlers and no domain event emissions (UI-14).
6. Manual visibility lists disagree with the route inventory (UI-16).

Prototype navigation is deliberately rejected. However, prototypes are eagerly added to the tree, so rejection does not prevent their _Ready failures. Merely promoting all shelved entries would expose unfinished UI.

Evidence: Main.PlayerSurfaces.cs; Main.GameFlow.cs:165–217/662–711; Main.UiPanels.cs; Main.PanelLifecycle.cs; PanelRegistry.ConfigureActions/Resolve/TryOpen.

# 7. Data Flow

The principal correctness issue is **presentation substituting invented values for owned state**. Research/Standing/Muster BuildData always creates fixed faction lists. Quest status totals are fixed; Maritime mixes catalog capacity and fabricated stages with live-sounding labels. Prototype telemetry and success text have no state source at all.

There are also domain-ID and action-payload losses: Map drops quadrant identity; Greenhouse drops water source/quantity; Decon substitutes a case ID for a survivor; survey/sample handlers omit required targets. These must be tested at command boundaries, not merely by confirming text appears.

Catalog presence is not a UI contract. Wave 6 catalogs and Plans110–113 systems already exist; no new JSON authority or parallel simulation is justified to fill their UI gaps. New Plans146–149 raw IDs/example recipe rows need real display projections before acceptance.

Evidence: UI-01–UI-04, UI-12, UI-14 and UI-20; Main.Plans78_81.cs; Main.Plans110_113.cs; Assets/StreamingAssets/Data.

# 8. State Ownership

Canonical state remains in Core and the owning host session. UI should own only selection, tabs, focus and presentation state. No new UI-local stock, progress or effect ledger should be introduced to make a mockup appear functional.

Known correct extension patterns include actual Ventilation binding in Electrostatic, SumpFlooding binding in SlurryDewateringSump, and host commands in the pooled consoles. These prove that similar outer frames or an object-typed Bind do not by themselves establish a stub.

Several Wave 6 handlers call session.System directly. This does not prove all state is unsaved, but it bypasses wrapper-specific LastEvent/StateChanged and command-result handling where those wrappers exist. Decon explicitly marks its dirty flag. Plans94–97's independent event selection can display another subsystem's earlier feedback instead of the latest user's result.

Evidence: src/UI/ElectrostaticScrubberPanel.cs:Bind; src/UI/SlurryDewateringSumpPanel.cs:Bind; src/Main.World.cs:462–520; src/Host/ChemicalReconHostSession.cs; src/UI/Plans94To97Panel.cs:FirstEvent.

# 9. Save/Load

This audit traced save **ownership/seams**, not a full save roundtrip of every screen:

| Family | Existing persistence seam | UI consequence |
|---|---|---|
| Plans110–113 | Main.Plans110_113 Setup/Save methods and campaign sections | Persisted systems can still lack a player workflow |
| Wave 6 | Main.Plans78_81 setup/capture plus owning host sessions | Bind/command/feedback must use those same owners |
| Electrostatic | Existing ventilation/shelter state | Do not create a second scrubber state/save authority |
| Geothermal Aquifer | SetupGeothermalAquifer, SaveGeothermalAquifer, GeothermalAquiferSaveStore | Wire the existing session/events, not a new panel-local model |
| Chemical synthesis | Main.ChemicalSynthesis and save orchestration | ChemicalLab should bind the current owner |
| New Plans146–149 | Main.Plans146_149 and new host/save files | Concurrent implementation; full regression acceptance is not established |

Fake-success prototypes mutate no real domain state; there is consequently no genuine result to persist. Bound atlas fixture data is reconstructed view data, not an alternative valid save. No save corruption was reproduced or asserted.

Evidence: src/Main.Plans110_113.cs; src/Main.Plans78_81.cs; src/Main.ShelterInfrastructure.cs:SaveGeothermalAquifer; src/Main.ChemicalSynthesis.cs; src/Main.Plans146_149.cs; src/Main.SaveOrchestrator.cs; Assets/Ashfall.Core/Save/SaveSectionRegistry.cs.

# 10. Determinism

No occurrences of System.Random, Guid.NewGuid, DateTime.Now or DateTime.UtcNow were found in the src/UI C# sweep. That is a narrow static result, not a deterministic replay certification.

Fixed index bucketing, first-item defaults and culture-sensitive parsing can still select the wrong player target/value without introducing randomness. Map's quadrant-index loss and Main.HandleGeothermalAction's float.Parse illustrate why stable IDs and invariant typed command arguments matter. Preserve seeded Core ownership; never implement measurement, hazard, brake or production simulation inside visual controls.

Evidence: src/UI/MapAtlasPanel.cs; src/UI/MaritimeAtlasPanel.cs; src/UI/Plans130To133Panel.cs; src/Main.World.cs:HandleGeothermalAction. No paired replay was added or run for this documentation-only audit.

# 11. UI/Player Feedback

The owner-facing failure is broader than visual repetition: a panel can look operational while showing unrelated state, offer inert commands, navigate nowhere or act on a different target than selected. Copying only the title/style creates false confidence.

[Accessibility detail](../ui/ACCESSIBILITY_REPORT.md) records runtime color contrast, 11px text, keyboard-inaccessible grid rows, undersized-at-scale target concerns, clipping and inconsistent Escape behavior. Stored atlas images corroborate the repeated bodies and clipped content. They do not prove every current layout at every viewport.

Domain-focused acceptance questions:

- Can the player identify the actual selected survivor, machine, location, recipe or job?
- Are state, blockers, resource cost and likely consequence distinct and truthful?
- Does activation cause the intended canonical state change and show the returned result?
- Can the task be completed with keyboard/controller as well as pointer, with visible focus and predictable back/close?
- Are empty/error/unavailable states explicit rather than replaced by examples?

# 12. Tests & Verification

The user noticed the background terminal had stalled. The earlier full xUnit run stopped producing output and its tool session was subsequently lost during interruptions. A process check found no remaining dotnet process; none was killed. Its final result is **UNKNOWN/INCOMPLETE**, not PASS. A later targeted attempt exposed sandbox socket permission denial; the approved retry outside the sandbox completed.

| Required / supplemental check | Observed result | What it proves / limits |
|---|---|---|
| dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj | Initial PASS; final recheck **FAIL: 8 errors, 5 warnings** | Concurrent Plans146–149 test additions no longer compile; earlier pass does not certify the changed tree |
| dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj | **INCOMPLETE / not verified** | Earlier full-run session lost; no full-suite pass available |
| dotnet build Ashfall.csproj | Final recheck **PASS, 0 warnings/0 errors** | Host compilation only |
| godot --headless --path . -- --data-integrity-selftest | **PASS** | 255 catalogs, 10783 authored IDs, 3834 reserved reuses, 0 errors/0 warnings in the earlier audit snapshot |
| godot --headless --path . -- --bridge-selftest | **PASS**, exit 0 | Stable shim-removal CI verb, not panel wiring |
| --scene-binding-selftest | **PASS, 22/22** | Only those scene contracts; not 178 panel workflows |
| --ui-accessibility-selftest | Prints **PASS, 5 gates**; teardown leaks/errors | Inadequate to establish actual contrast, complete focus navigation or all panels |
| --ui-layout-selftest | Prints **PASS**; teardown leaks/errors | Root-bound assertions at eight resolutions, not descendant fit; 16634 ObjectDB instances leaked |
| --decon-airlock-uitest | Prints **PASS** after constructor exceptions | **FAIL for UI health**: 18 missing Margin + 18 NullReferenceException + 3 ObjectDisposedException reports |
| bounded PanelRouteGateTests, --no-build | Approved retry **PASS, 19/19**, 229ms test duration | Previous available test assembly only; known scanner blind spots remain; latest source build is failing |

Latest test-build error examples: CoreSeededRng unresolved in new EbPvdCoatingEngineTests/MicrofluidicDiagnosticEngineTests; PowerSupplyContext.Throttled and EbPvdCoatingEngine.ResumeJob absent; incorrect delegate/context argument and RegisterRailSegment named argument in Plans146_149IntegrationTests. These are existing/concurrent work, not audit modifications. No production repair was attempted.

Headless commands isolated XDG_DATA_HOME and --log-file beneath /tmp/ashfall-ui-audit.KI94f7. Logs are temporary evidence, not durable repository dependencies. Selected durable observations:

```text
DATA_INTEGRITY_SELFTEST PASS — 0 errors, 0 warnings across 255 catalogs
[SCENE_BIND] PASS ... (22 scene contracts)
ObjectDisposedException: ExpeditionRadarPanel.RefreshDetail, line 370
ObjectDisposedException: FactionsNarrativePanel.RefreshDetail, line 325
ObjectDisposedException: SkillMatrixPanel.RefreshDetail, line 371
Node not found: "Margin" ... (18 occurrences)
NullReferenceException ... (18 corresponding occurrences)
DeconAirlockUiTest PASS
UI_LAYOUT_SELFTEST PASS
WARNING: 16634 ObjectDB instances were leaked at exit
```

Reproduction paths (headless only): dotnet build Ashfall.csproj; godot --headless --path . -- --decon-airlock-uitest; --ui-layout-selftest; --ui-accessibility-selftest. Review stderr/engine diagnostics as well as the process verdict. Do not regenerate golden baselines to hide defects.

# 13. Duplicates / Legacy / Forks

The structurally normalized clone groups are enumerated in the inventory: eight + ten equivalent structures, plus the Magnetic Drum near-copy. Their defect is absent domain behavior, not merely reuse of a panel frame. The three copied faction atlases are a stronger semantic mismatch: unchanged faction content in unrelated domains.

Other duplication is presentation debt: three omnibus consoles, atlas/canonical-panel pairs, overlapping journal entry labels, and alternate HUD/chronicle components. None justifies forking Core or reintroducing Unity. The active src/Bridge and Assets/_Game hosts are removed; no legacy-host implementation was treated as current functionality.

# 14. Existing Extension Seams

- Reuse PanelRegistry descriptors, explicit binding/open/close actions and availability rules; verify ConfigureActions success and every advertised entry.
- Use the actual host command/result/StateChanged seams. Preserve one LastEvent feedback source for the active task, not parallel success strings.
- Reuse AshfallUiHelpers, DashboardShell, StatusRail and DataGrid as **components**, repairing their accessibility contracts centrally; do not clone complete unrelated domain bodies.
- ChemicalLab, Electrostatic and Geothermal already contain useful specialist work. Wave 6 already has four substantial panel implementations and action events. Complete those seams after validating payload contracts.
- Use existing campaign save sections and canonical catalogs. A new panel does not require a new simulation or save owner.
- Google Stitch via Antigravity is required by project policy for later missing/stub screen/layout design; proposals must reconcile with actual theme tuples, available APIs and domain constraints. This audit did not request design generation.

# 15. Functional Equivalents

| Missing/duplicated presentation | Existing equivalent / nearby owner | Disposition |
|---|---|---|
| Research/Standing/Muster/Quest/Map/Maritime atlases | ResearchPanel, StandingRecordPanel, MusterPanel, QuestsPanel, MapPanel, MaritimePanel/DeepCoastPanel | Real domain counterparts; reuse owners, not unrelated bodies; avoid parallel fake data |
| Electrostatic scrubber | VentilationHostSession; existing ElectrostaticScrubberPanel | Already implemented in part; wiring/entry/lifecycle gap |
| Slurry dewatering | SumpFloodingHostSession; SlurryDewateringSumpPanel/SumpFloodingPanel | Real binding; excluded from prototype clone verdict |
| Chemical lab | ChemicalSynthesisHostSession; ChemicalLabPanel; PharmaLab is a different nearby workflow | Existing specialist surface stranded; not a reason to duplicate chemistry |
| Chronicle | Endgame owner; EpiloguePanel | Alternate presentation absent from Main; not proof that endings are missing |
| Weather history | OpenWeatherHistoryPanel and weather-history input action | F5 path exists without registry; not a missing feature merely from registry absence |
| Trauma bonding, Long Walk, vouch, radon, cupola, aquifer, memorial prototypes | TraumaBondSystem; Muster/LongWalkSystem; VouchAccessSystem; YearOfAshRadonSystem; CupolaFoundryEngine; GeothermalAquiferSystem; MemorialSystem | Partial semantic equivalents; verify exact contract before inventing a new system |
| Cartography, printing, fermentation prototypes | Existing world/cartography and narrative catalogs, printing/fermentation-related definitions | Presence is not proof of the proposed machine workflow; keep capability distinction |
| WeatherHardening, CounterIntelligence, ReconTelemetry | New/current host and cross-system integration paths | No direct typed panel consumers found; candidate UI exposure review, **not** confirmed separate-screen requirements from that fact alone |
| ShelterHudPanel / FactionRadioHudPanel | Existing dashboard/HUD and radio/economy composition | No direct Main reference is not enough to declare an entire domain missing; nested composition must be considered |

Evidence: named UI/host/Core classes located by exact-name and semantic re-search; src/Main.Application.cs:598; src/Main.UiHandlers.cs:49; src/Main.ShelterInfrastructure.cs; src/Main.FactionBranch.cs; src/Main.Expeditions.cs.

# 16. Confirmed Gaps

## UI-01 — HIGH — Research, Standing Record and Muster atlases are relabeled faction screens

Affected: ResearchAtlasPanel; StandingRecordAtlasPanel; MusterAtlasPanel.

All three populate the same five fixed faction/trust rows, coalition values and faction dossiers under different domain headings. Binding a host does not replace BuildData. Research still renders Faction/Current/ΔTrust instead of a research dependency/progress workflow; Standing Record and Muster reuse the same body and mostly fixed status metrics. Registered Live and host-configured is not proof of discoverable navigation.

Evidence: src/UI/ResearchAtlasPanel.cs:BuildData/BuildGrids/RefreshDetail (477/289/408); src/UI/StandingRecordAtlasPanel.cs:BuildData/RefreshStatusRail/RefreshDetail (433/232/364); src/UI/MusterAtlasPanel.cs:BuildData/RefreshStatusRail (433/232); src/Main.PlayerSurfaces.cs:485; snapshots/research_atlas_default.png; snapshots/standing_record_atlas_default.png; snapshots/muster_atlas_default.png.

Planning constraint / acceptance direction: Design domain-specific bodies against the existing Research, Standing Record and Muster owners. Share theme primitives, not faction datasets or unrelated workflow structure.

## UI-02 — HIGH — Six atlas action bars are inert text, not commands

Affected: MapAtlasPanel; MaritimeAtlasPanel; MusterAtlasPanel; QuestsAtlasPanel; ResearchAtlasPanel; StandingRecordAtlasPanel.

BuildActionFixtureRows feeds non-selectable AshfallDataGrid rows even on bound screens. Labels such as Dispatch Sortie, Plot Waypoint, Accept, Abandon, Inspect and Schedule have no corresponding button activation or command dispatch. A visually complete action strip therefore promises unavailable interaction.

Evidence: src/UI/MapAtlasPanel.cs:BuildActionRows/BuildActionFixtureRows (355/449); src/UI/MaritimeAtlasPanel.cs:BuildActionRows/BuildActionFixtureRows (319/423); src/UI/QuestsAtlasPanel.cs:BuildActionFixtureRows (372); src/UI/{ResearchAtlasPanel,StandingRecordAtlasPanel,MusterAtlasPanel}.cs:BuildActionFixtureRows.

Planning constraint / acceptance direction: Give each actual task a real command with selection, blocker, cost, consequence and authoritative result feedback. Do not make fake rows clickable without implementing the underlying contract.

## UI-03 — HIGH — Quest and maritime atlases substitute authored examples for current progress

Affected: QuestsAtlasPanel; MaritimeAtlasPanel.

Quests hardcodes 3 active/5 available/11 completed/4 locked/0 failed/1 abandoned on the bound path; a fixed five Holdfast keys are labeled Active independently of actual quest status. Maritime buckets catalog sites by index modulo four, reports stage Sealed and decision None, and sums catalog oxygen budgets rather than showing a current expedition's remaining oxygen. These are not reliable operational views.

Evidence: src/UI/QuestsAtlasPanel.cs:RefreshStatusRail/BuildQuestRows (154/185); src/UI/MaritimeAtlasPanel.cs:RefreshStatusRail/RoomRowsFor (255 onward).

Planning constraint / acceptance direction: Project quest progress and active dive-instance state. Distinguish catalog capacity/planning values from live remaining resources; render genuine empty/unknown states.

## UI-04 — HIGH — Map atlas selection loses quadrant identity

Affected: MapAtlasPanel.

Three quadrant grids emit local row indices into the same selection handler. ResolveVisibleRow and FindLocation walk the complete location list rather than the clicked quadrant's filtered list. East/South row zero can therefore show North/global row zero's detail; null-sector filtering further changes offsets.

Evidence: src/UI/MapAtlasPanel.cs:OnRowSelected wiring/TileRowsFor/ResolveVisibleRow/FindLocation (130 onward/285/338/395).

Planning constraint / acceptance direction: Carry a stable location ID from each rendered row to detail and actions; verify every quadrant, filtered row and empty state.

## UI-05 — HIGH — Nineteen three-column consoles are near-identical hardcoded shells

Affected: AquiferTreatyConcessionPanel; BasalRadonMigrationPanel; ClandestineInsurgencyPanel; CrossingSafeConductVouchPanel; CryogenicPermafrostCorePanel; FungalProteinFermenterPanel; HeavyMarineDieselGeneratorPanel; InductionCupolaFurnacePanel; IronCenotaphMemorialPanel; LongWalkExpeditionPanel; MagneticDrumArchivePanel; MechanicalProstheticsLathePanel; SonicRuptureDrillPanel; SubterraneanDebtLedgerPanel; SurfaceShrapnelAegisPanel; TraumaBondingCohortPanel; TroposphericRadioRelayPanel; UltrasonicDecontaminationAirlockPanel; VaultDoorBreachingPanel.

These assert IsBound=true, ignore Bind(object? session), render fixed telemetry and expose non-close buttons without Pressed handlers. Normalizing comments, strings, class names, numbers, whitespace and selected color-token names produces two exact structural groups of eight and ten; Magnetic Drum is the nineteenth near-copy with a changed margin accessor. All nineteen are shelved Prototype routes, not completed gameplay. Eighteen still look up Margin at the wrong tree depth and throw during eager construction.

Evidence: src/UI/AquiferTreatyConcessionPanel.cs:IsBound/Bind/RefreshView/BuildLayout/CreatePanelFrame (20/34/45/111/161); corresponding members in all named files; src/UI/MagneticDrumArchivePanel.cs; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:137; src/Main.UiPanels.cs:BuildUserInterface; headless decon-airlock-uitest log.

Planning constraint / acceptance direction: Keep them shelved until each has its own domain task design and real owner contract. Fix eager-construction health separately; recoloring, relabeling or assigning a host field does not complete a panel.

## UI-06 — MEDIUM — Ten additional prototypes simulate success without changing state

Affected: AnaerobicBiogasDigesterPanel; SubterraneanCartographyPanel; UndergroundPrintingPressPanel; SiliconIngotSlicingPanel; GeothermalSteamTurbinePanel; WarDogKennelPanel; IsotopeSeparatorPanel; PlasmaArcSmeltingPanel; BoreholeSeismographPanel; HeavyLogisticsAirlockPanel.

These roughly 126–130-line prototypes have unconditional IsBound, no session Bind, empty RefreshView/Unbind and hardcoded telemetry. Buttons only ShowFeedback, including success-sounding results, without a Core mutation. Biogas reports fixed 38.2°C/96.5% and feeding output with no inventory operation. Prototype gating limits direct player exposure but these are missing workflows, not implemented consoles.

Evidence: src/UI/AnaerobicBiogasDigesterPanel.cs; corresponding IsBound/RefreshView/ShowFeedback members in all ten files; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:126.

Planning constraint / acceptance direction: Resolve existing gameplay equivalents and owners before designing each unique workflow; do not promote simulated-success fixtures.

## UI-07 — HIGH — Eleven typed panels are still a single label with an empty refresh

Affected: AmputationTriagePanel; ArchaeologyExcavationPanel; CeremonyFestivalPanel; ChemWarfareDefensePanel; CommsArrayTransceiverPanel; FungiCultivationBedPanel; JusticeTribunalPanel; RailwayTerminalPanel; RoboticsWorkshopPanel; SurvivorDowntimePanel; WinterFreezePanel.

Each 33-line implementation stores a typed system but renders only AshfallDashboardShell plus one generic label and Close; RefreshView is empty. There is no entity selection, task state, actionable blocker, cost, consequence or domain command. RailwayTerminalPanel is not a completed bound-panel exemption despite the previous AGENTS claim.

Evidence: src/UI/RailwayTerminalPanel.cs:Bind/RefreshView/_Ready (13/19/21); same full-file structure in all eleven named files; src/Main.PlayerSurfaces.cs:45.

Planning constraint / acceptance direction: Treat all eleven as STUB/PARTIAL. Specify separate domain workflows and required Core projections before implementation.

## UI-08 — HIGH — Eleven expansion readouts lack their management workflows

Affected: AviationUI; ChemUI; LaborUI; PoliticsUI; PrisonerPanel; StealthReadoutPanel; MutationTreePanel; NurseryPanel; FalloutPlumePanel; DesperationCrisisPanel; MercenaryBountyBoardPanel.

Most use the same shell/status rail/single multiline Label and read actual system state when refreshed, but expose no domain commands beyond Close. A readout is not a dispatch, policy, allocation, training or care workflow. Mercenary's board is thinner still: active-count text plus a fixed no-targets message and neutral guild text. These are partial readouts, not evidence that all corresponding Core mechanics are absent.

Evidence: src/UI/{AviationUI,ChemUI,LaborUI,PoliticsUI,PrisonerPanel,StealthReadoutPanel,MutationTreePanel,NurseryPanel,FalloutPlumePanel,DesperationCrisisPanel}.cs:_Ready/RefreshView; src/UI/MercenaryBountyBoardPanel.cs:RefreshView; src/Main.PlayerSurfaces.cs:45/511.

Planning constraint / acceptance direction: Document the actual player decisions per domain and expose the existing command owners with selectable targets, blockers and costs; do not count a shared multiline label as a dedicated workflow.

## UI-09 — HIGH — Twenty-three configured navigation IDs were never registered

Affected: expansion_fallout_plume; desperation_crisis; mercenary_bounty_board; archaeology_excavation; amputation_surgery; railway_logistics; fungi_cultivation; justice_tribunal; chem_warfare_defense; comms_array_transceiver; ceremony_ritual; robotics_assembly; survivor_downtime; winter_freeze; aviation; narcotics; forced_labor; politics; prisoners; stealth; mutation_tree; nursery; fallout_detail.

ConfigureActions returns false for an unknown descriptor, and the host ignores that return value. These 23 IDs cover 22 classes because Fallout has two IDs. Nine have dashboard AddNavButton entries, so the dashboard advertises routes that OpenPlayerPanel rejects as unknown. Registering them alone would expose the incomplete bodies in UI-07/UI-08.

Evidence: Assets/Ashfall.Core/UI/PanelRegistry.cs:ConfigureActions/Resolve (204/227); Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs; src/Main.PlayerSurfaces.cs:45–114/511–554; src/UI/GameDashboardPanel.cs:435–448; src/Main.GameFlow.cs:165.

Planning constraint / acceptance direction: Reconcile descriptor, binding, availability, entry point, destination and close path as one contract. Gate unfinished workflows instead of silently adding Live descriptors.

## UI-10 — HIGH — A Live industrial route has no destination; four domains have no dedicated UI

Affected: plans_110_113; chlor-alkali synthesis; solar concentration; precision optics; ballistic shields.

The bootstrap registers plans_110_113 as Live, but no ConfigureActions destination or matching player panel exists. Main.Plans110_113 constructs, loads, ticks and captures four real host systems with no corresponding typed UI consumers. There is backend capability without an operational player surface.

Evidence: Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:169; src/Main.PlayerSurfaces.cs; src/Main.Plans110_113.cs; src/Host/{ChlorAlkaliHostSession,SolarConcentratorHostSession,PrecisionOpticsHostSession,BallisticShieldHostSession}.cs; src/Main.GameFlow.cs:217.

Planning constraint / acceptance direction: Plan four distinct task surfaces on the existing owners. Do not fix this by introducing another unrelated multi-system omnibus screen.

## UI-11 — HIGH — Wave 6 panels exist but are not bound or normally reachable

Affected: DeconAirlockPanel; GeodeticSurveyPanel; KineticStoragePanel; ChemicalReconPanel.

All four now have substantial UI source, construction and OnActionRequested subscriptions. No production Bind call to these panel instances or normal registered navigation route was found. They start hidden; OPEN handlers and tests can set visibility directly, which does not bind state or make a player entry point. The old 'files do not exist' statement is stale.

Evidence: src/Main.UiPanels.cs:396–440; src/Main.World.cs:462–520; src/Main.Plans78_81.cs; src/Main.UiTests.Wave6.cs:10–47; src/UI/{DeconAirlockPanel,GeodeticSurveyPanel,KineticStoragePanel,ChemicalReconPanel}.cs:Bind; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs.

Planning constraint / acceptance direction: Complete authoritative binding, dependency setup, route, discovery and lifecycle first; retain four domain-specific designs and validate actions before promotion.

## UI-12 — HIGH — Wave 6 action adapters contain wrong arguments and explicit no-ops

Affected: DeconAirlockPanel; GeodeticSurveyPanel; KineticStoragePanel; ChemicalReconPanel.

Decon emits a selected case ID but Main passes it to StartProtocolCycle's survivorId slot with empty gear and fixed contamination. Survey Observe supplies an empty target and fixed clear weather/skill; resolve does nothing. Flywheel EMERGENCY_BRAKE does nothing, while charge/discharge hardcode 1000 and 60. Chemical change_filter does nothing and sampling supplies an empty location. Several calls bypass host command wrappers and their LastEvent/StateChanged feedback; decon does explicitly mark its dirty flag, so not all persistence routing is absent.

Evidence: src/Main.World.cs:462–520; src/UI/DeconAirlockPanel.cs:365; Assets/Ashfall.Core/DecontaminationSystem.cs:StartProtocolCycle; Assets/Ashfall.Core/World/GeodeticSurveyEngine.cs:Observe; Assets/Ashfall.Core/Expeditions/ChemicalReconEngine.cs:CollectSample; src/Host/ChemicalReconHostSession.cs.

Planning constraint / acceptance direction: Pin typed command arguments and before/after state tests, especially emergency actions. A callable manual-brake Core command was not found; resolve that seam explicitly instead of inventing UI-owned brake behavior.

## UI-13 — HIGH — Existing specialist panels are stranded despite useful implementations

Affected: ElectrostaticScrubberPanel; GeothermalAquiferPanel; ChemicalLabPanel.

Electrostatic Scrubber already binds VentilationHostSession and invokes real stage controls; it is constructed twice across Main.UiPanels and SetupElectrostaticScrubberPanel, with no normal registered entry. Geothermal Aquifer now has a Setup-time Bind and real action buttons, but its OnActionRequested is not subscribed in construction and no normal route was found. ChemicalLabPanel has typed chemical-synthesis binding and operations but no Main construction/registration. File absence is the wrong diagnosis for all three.

Evidence: src/UI/ElectrostaticScrubberPanel.cs:Bind; src/Main.UiPanels.cs:BuildUserInterface; src/Main.ExpandedShelterSystems.cs:129–137; src/Main.ShelterInfrastructure.cs:SetupGeothermalAquifer (374–388); src/Main.UiPanels.cs:868–870; src/UI/GeothermalAquiferPanel.cs:105–120; src/UI/ChemicalLabPanel.cs; src/Main.ChemicalSynthesis.cs.

Planning constraint / acceptance direction: Reuse each existing specialist implementation's legitimate domain work, then finish its single construction/binding/route/event/lifecycle contract. PharmaLab does not automatically replace retort chemical synthesis.

## UI-14 — HIGH — New Plans 146–149 screens are partial readouts, not completed control workflows

Affected: EbPvdCoatingPanel; MicrofluidicDiagnosticPanel; MineFlailPanel; RailGrindingPanel.

These four files appeared/changed concurrently during the audit. At the final inspection they compile, are constructed and bound from BuildPlans146To149Panels, and have domain data projections. However, OnActionRequested is only declared, not emitted by domain controls; Main handlers implement OPEN only, and registry/player entry points are absent. Their shared sidebar/table/detail scaffold, hardcoded example values and raw machine IDs do not fulfill coating, diagnostics, clearing or grinding operations. This is an as-observed WIP finding, not an assertion about a future completed change.

Evidence: src/Main.UiPanels.cs:442; src/Main.Plans146_149.cs:140–224; src/UI/{EbPvdCoatingPanel,MicrofluidicDiagnosticPanel,MineFlailPanel,RailGrindingPanel}.cs; src/UI/EbPvdCoatingPanel.cs:RefreshView; Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs.

Planning constraint / acceptance direction: Require four distinct state→blocker→cost→consequence workflows, real commands and normal routes. Re-audit the concurrent work before accepting any closeout claim.

## UI-15 — MEDIUM — Three omnibus panels compress unrelated domains into one operations screen

Affected: Phase0Panel; Plans94To97Panel; Plans130To133Panel.

Phase0 explicitly groups ten systems under an internal phase label. Plans94To97 puts grain milling/storage, cryogenic separation and heliograph communications side by side; Plans130To133 does the same for powder metallurgy, NVIS communications, lyophilization and draisine rerailing. Real host-backed commands exist, so these are not all stubs, but the plan-number information architecture and compressed fixed layouts conflict with the owner's rejection of reusing one panel for unrelated tasks. Plans94's IsBound accepts any host while builders assume the others, and its first nonempty event can mask feedback from another subsystem.

Evidence: src/UI/Phase0Panel.cs:class summary/_Ready; src/UI/Plans94To97Panel.cs:IsBound/RefreshView/FirstEvent; src/UI/Plans130To133Panel.cs:_Ready/RefreshView.

Planning constraint / acceptance direction: Give the domains dedicated workflows or explicitly distinct navigable task views while preserving shared styling and existing command owners. No new gameplay systems merely to split presentation.

## UI-16 — HIGH — Overlay detection, dismissal and navigation maintain different incomplete lists

Affected: Global overlay lifecycle; WorkshopPanel; PharmaLabPanel; Phase0Panel; DeepCoastPanel; WeatherHistoryPanel; expanded shelter panels; Wave 6; specialist/new panels.

AnyOverlayPanelOpen covers far fewer controls than CloseAllOverlayPanels, and CloseAllOverlayPanels itself omits multiple constructed overlays. For example workshop, pharma, phase0, deep coast, weather history, water/kitchen expanded panels and newer panels are missing from the close list. OpenExpandedPanel does not repair this. Global Escape can treat an unlisted overlay as 'no overlay' and return to the menu when the panel does not consume the event; switching panels can leave an older overlay visible. This is source-proven list drift; every possible focus/stack combination was not replayed.

Evidence: src/Main.GameFlow.cs:AnyOverlayPanelOpen/_UnhandledInput (662/693); src/Main.PanelLifecycle.cs:CloseAllOverlayPanels (9–61); src/Main.PlayerSurfaces.cs:OpenExpandedPanel; src/Main.Application.cs:_UnhandledKeyInput.

Planning constraint / acceptance direction: Use one authoritative lifecycle contract for visibility, topmost focus, back/close and navigation. Verify each route with keyboard/controller cancel and multiple-overlay transitions.

## UI-17 — HIGH — Dashboard navigation exceeds the supported canvas before its extra controls

Affected: GameDashboardPanel navigation rail; atlas/fixed-width console layouts.

The dashboard navigation is a non-scrolling VBox with 40 navigation buttons at a 30-pixel minimum each: 1200 pixels before headings, separation, save/developer controls or header/footer. That already exceeds 1080 pixels. Stored 1280×800 Research/Standing/Muster atlas images also visibly cut off right-side grid content. Plans130To133 has a 1320-pixel minimum before fitting smaller targets. These are concrete overflow defects/risks, not evidence that root anchors solve responsive layout.

Evidence: src/UI/GameDashboardPanel.cs:BuildNavigationRail/AddNavButton (397–460/604); src/UI/Plans130To133Panel.cs:_Ready; snapshots/{research_atlas_default,standing_record_atlas_default,muster_atlas_default}.png.

Planning constraint / acceptance direction: Provide reachable scroll/reflow navigation and test descendant bounds, long labels, focus scrolling and supported scales. Preserve domain distinction while repairing sizing.

## UI-18 — HIGH — Three detail refreshes access freed labels during UI construction

Affected: ExpeditionRadarPanel; FactionsNarrativePanel; SkillMatrixPanel.

RefreshDetail empties the detail container through immediate child freeing and then accesses a cached label that belonged to it. A headless BuildUserInterface run logged ObjectDisposedException at ExpeditionRadar line 370, FactionsNarrative line 325 and SkillMatrix line 371. These failures occur before the decon UI smoke test announces PASS.

Evidence: src/UI/ExpeditionRadarPanel.cs:RefreshDetail (370); src/UI/FactionsNarrativePanel.cs:RefreshDetail (325); src/UI/SkillMatrixPanel.cs:RefreshDetail (371); src/UI/AshfallUiHelpers.cs:EmptyChildren; /tmp/ashfall-ui-audit.KI94f7/decon-airlock-uitest.log:11/40/69.

Planning constraint / acceptance direction: Repair ownership/lifetime of rebuilt detail children and assert fresh construction plus repeated selection/refresh is exception-free.

## UI-19 — MEDIUM — Production unbound/empty paths display plausible fixture data

Affected: GreenhousePanel; WeatherPanel; FactionMatrixPanel; FactionsNarrativePanel; SilentFoundryPanel; ExpeditionRadarPanel; DutyRosterPanel; SkillMatrixPanel; DoseLedgerPanel; SurvivalWorkstationPanel; MapAtlasPanel; MaritimeAtlasPanel; QuestsAtlasPanel.

Production Refresh/BuildRows branches include fixture builders on missing hosts or empty collections. A lost binding or genuinely empty campaign can therefore display credible invented rows rather than an explicit unavailable/empty state. This finding is about fallback behavior; it does not claim these panels all use fake data when correctly bound. UI-01/UI-03 document the separate bound-path violations.

Evidence: src/UI/GreenhousePanel.cs:304; src/UI/WeatherPanel.cs:121; src/UI/FactionMatrixPanel.cs:148; src/UI/FactionsNarrativePanel.cs:245; src/UI/SilentFoundryPanel.cs:280; src/UI/ExpeditionRadarPanel.cs:239/291; src/UI/DutyRosterPanel.cs:255; src/UI/SkillMatrixPanel.cs:225; src/UI/DoseLedgerPanel.cs:113; src/UI/SurvivalWorkstationPanel.cs:320; src/UI/MapAtlasPanel.cs:273; src/UI/MaritimeAtlasPanel.cs:281; src/UI/QuestsAtlasPanel.cs:BuildQuestRows.

Planning constraint / acceptance direction: Keep fixtures in explicit test/preview mode. Runtime empty, loading, unavailable and error states must be truthful and must not imply completed actions or inventory.

## UI-20 — HIGH — Greenhouse water choices all decode to clean 50-unit watering

Affected: GreenhousePanel; Main.HandleGreenhouseAction.

The panel emits water:25:clean, water:50:clean and water:50:tainted. Main splits only the first colon; '25:clean' and '50:tainted' are neither 'tainted' nor valid floats, so all three fall through to clean water with 50 units. The selected quantity/source is lost and the displayed stock gate can disagree with consumption. Seed selection, water choices, supply/readiness UI now exist; the older eight-gap specification must not be treated as current evidence. Amendment/maintenance/sterilization host API claims also need revalidation against the trimmed current host.

Evidence: src/UI/GreenhousePanel.cs:511–593; src/Main.World.cs:HandleGreenhouseAction (69–95); src/Host/GreenhouseHostSession.cs:Plant/Water; docs/ui/GREENHOUSE_UI_GAP_SPEC.md.

Planning constraint / acceptance direction: Pin quantity and source as a typed command contract with inventory-delta tests for all three choices, then reconcile the historical Stitch handoff with current APIs.

## UI-21 — HIGH — Coverage claims and smoke-test passes overstate player readiness

Affected: PlayerSurfaceManifest; PanelRouteGateTests; UiLayoutSelfTest; UiAccessibilitySelfTest; Wave 6/Plans 146–149 UI tests; snapshot manifest.

PlayerSurfaceManifest manufactures binding strings and marks Live routes ProductionRendered/reachable without observing execution. Route tests miss AddNavButton parameter literals and the inverse 'every configured ID is registered' check. Layout tests set and check root bounds, not descendants. Accessibility tests do not measure contrast or perform a full focus traversal. Wave 6 tests check construction/non-null, and new Plans146–149 tests primarily IsBound. The decon smoke log contains 18 missing-Margin errors, 18 NullReferenceExceptions and three ObjectDisposedExceptions before PASS; layout teardown reports 16634 leaked ObjectDB instances. Scene contracts and golden fixtures are limited evidence, not end-to-end gameplay.

Evidence: Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs:Generate; Ashfall.Core.Tests/UI/PanelRouteGateTests.cs; src/Host/HostCli.cs:RunUiLayoutSelfTest (2638 onward); src/Host/UiAccessibilitySelfTest.cs; src/Main.UiTests.Wave6.cs; src/Main.Plans146_149.cs:228 onward; docs/ui/snapshot_manifest.json; headless audit logs.

Planning constraint / acceptance direction: Require route→bind→visible→select→command→state delta→feedback→save/reload proof, fail on engine exceptions, and validate descendant bounds/focus. Never report generated 100% as observed UI completion.

## UI-22 — MEDIUM — UI documentation classifies files and contracts incorrectly

Affected: AGENTS.md missing/stub list and Greenhouse handoff; UI_PANEL_ARCHITECTURE_GUIDE; closeout/coverage claims.

The previous list says Electrostatic and Wave 6 files are missing and exempts RailwayTerminal as bound. Current source proves different states (UI-07/UI-11/UI-13). Bind(object?) alone is not a stub test: Electrostatic performs a real VentilationHostSession cast and subscriptions. The architecture guide's scene-contract examples and historical Greenhouse API descriptions are not a substitute for current source. Concurrent closeout files likewise cannot override missing command/navigation evidence.

Evidence: AGENTS.md:STITCH UI HANDOFF/Missing UI panels; docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md; src/UI/RailwayTerminalPanel.cs; src/UI/ElectrostaticScrubberPanel.cs:Bind; src/UI/GreenhousePanel.cs; docs/ui/GREENHOUSE_UI_GAP_SPEC.md.

Planning constraint / acceptance direction: Track EXISTS/COMPILES/WIRED/EXECUTES/PLAYER-FACING/VERIFIED separately. Update status with current source and action-level acceptance evidence, not filename creation or a screenshot.

## UI-23 — HIGH — Shared small-text tokens and controls undermine readability and keyboard access

Affected: Theme; AshfallDataGrid; AshfallSidebar; AshfallDashboardShell; GameDashboardPanel.

Runtime tuple colors give Dim/Ink contrast 3.45:1 and Dim/SurfaceCard 3.09:1, below the 4.5:1 body-text audit threshold; Dim/SelectedBg is 2.73:1. DataGrid headers and sidebar hints use Dim at 11px. Critical/SurfaceCard is 4.12:1. Several hex tokens disagree with the tuples actually rendered, so a mockup can use different colors from production. DataGrid row activation is mouse-GuiInput-only, without focusable keyboard selection. Close targets are 28px high and nav targets 30px: usability concerns at small scales, not a claimed universal target-size standards violation.

Evidence: Assets/Ashfall.Core/UI/Theme.cs:31–95/FontSizeLabel; src/UI/AshfallDataGrid.cs:BuildHeaderCell/BuildRow; src/UI/AshfallSidebar.cs:67/156; src/UI/AshfallDashboardShell.cs:AttachHeaderCloseButton (150); src/UI/GameDashboardPanel.cs:606; docs/ui/ACCESSIBILITY_REPORT.md.

Planning constraint / acceptance direction: Fix non-disabled readable text contrast and focusable row selection first; reconcile token representations, audit text scaling/long labels, and verify focus visibility/topmost behavior. Keep non-color warning labels.

# 17. Risks

**HIGH:** Wrong or invented domain state, actions with no effects/wrong arguments, dead advertised routes, eager-construction exceptions, disappearing/clipped navigation and inaccessible grid selection. These block reliable player operation even if individual Core tests pass.

**MEDIUM:** Shelved fake-success prototypes, omnibus information architecture, production fixtures on missing bindings, and stale documentation. Prototype gating reduces immediate exposure, but it does not make the workflows complete or avoid eager construction.

No CRITICAL save-corruption, determinism break or state-authority fork was reproduced in this UI audit. Creating independent UI-side simulations to fill gaps would introduce architectural risk and is explicitly excluded from acceptable follow-up work.

# 18. Constraints for Planning

1. **No copy-and-rename full panels for unrelated domains.** Shared visual components are permitted; the task body, selected entity, projections, blockers, costs and consequences must fit the domain. A color/title change does not close a finding.
2. **No blanket promotion of prototypes.** Define the owner/API and complete actions first; unavailable features must be honest and gated.
3. **No omnibus replacement for missing specialist workflows.** Split presentation while preserving canonical state and commands.
4. **Keep the audit separate from implementation.** The user requested analysis/reporting. These are findings and acceptance constraints, not approval to perform repairs or external design writes.
5. **Plan on current source, not historical closeouts.** Reconcile Greenhouse/Wave 6/new Plans146–149 changes before later design. An object Bind is not sufficient evidence either way.
6. **Acceptance requires player-path proof:** descriptor and entry, dependency setup, binding, visible state, stable selection, command argument correctness, state/inventory delta, feedback, keyboard/controller dismissal and save/reload where relevant.
7. **Accessibility is functional acceptance:** readable live colors/type, truthful labels, no color-only critical status, reachable navigation, keyboard grid selection, long-content and descendant-bound checks.
8. **Do not reset/overwrite concurrent work or update baselines to fit broken screens.** Only AGENTS and audit artifacts were edited here.
9. **Verification stays dotnet + godot --headless.** Report engine diagnostics and source-build failures; partial smoke passes do not certify completion.

Suggested order for a later approved plan: constructor/lifecycle health and dead routing → false data/wrong/inert commands → domain-specific missing workflows → accessibility/layout → stronger acceptance gates and documentation reconciliation. No implementation was performed.

# 19. Evidence Index

| Evidence | Supports |
|---|---|
| UI_PANELS_UX_INVENTORY.md | Complete 178-panel listing; all 141 descriptors; 23 unknown configurations; structural clone groups |
| src/UI/ResearchAtlasPanel.cs; StandingRecordAtlasPanel.cs; MusterAtlasPanel.cs | Unconditional copied BuildData and mismatched detail bodies |
| Six *AtlasPanel.cs files | Non-selectable action fixtures; false quest/maritime summaries; Map identity loss |
| PanelRegistryBootstrap.cs; PanelRegistry.cs; Main.PlayerSurfaces.cs; Main.GameFlow.cs | Declared maturity, ignored configuration failures, missing destinations and rejection |
| Main.UiPanels.cs; Main.PanelLifecycle.cs; Main.Application.cs | Construction, binding, nested lifecycle and input paths |
| Main.World.cs; Main.Plans78_81.cs; Wave 6 panels | Missing binds and incorrect/no-op adapters |
| Main.Plans110_113.cs; Main.Plans146_149.cs | Missing dedicated industrial surface and concurrent partial new screens |
| Main.ExpandedShelterSystems.cs; Main.ShelterInfrastructure.cs; Main.ChemicalSynthesis.cs | Existing specialist state and incomplete wiring |
| Assets/Ashfall.Core/UI/Theme.cs; DataGrid/Sidebar/Shell helpers | Runtime colors, tiny type, pointer-only grid selection, target sizes |
| Main.UiTests.Wave6.cs; HostCli.cs; UiAccessibilitySelfTest.cs; PlayerSurfaceManifest.cs; Ashfall.Core.Tests/UI/PanelRouteGateTests.cs | False confidence / limited test contracts |
| snapshots/research_atlas_default.png; standing_record_atlas_default.png; muster_atlas_default.png | Stored visual repetition/clipping evidence, not fresh bound-state proof |
| /tmp/ashfall-ui-audit.KI94f7/*.log | Earlier headless run diagnostics; representative results preserved in §12 |

Line numbers are inspection-time anchors and may drift with concurrent changes; member names are the primary durable locators. Braced file families in evidence indicate each explicitly named source file, not a single literal filename.

# 20. Confidence & Unknowns

**High confidence:** registry/configuration mismatch, unconditional fixed data, empty refreshes, absent event emissions, wrong payload decoding, explicit no-op branches, constructor exceptions, manual list drift, theme math and pointer-only row selection. These have direct source and/or runtime evidence.

**Qualified visual confidence:** three stored default screenshots were visually inspected. Current bound screens, all long localized strings, every viewport, focus stack and controller journey were not freshly replayed. Contrast ratios use actual opaque token tuples; translucent compositing depends on the underlying surface and requires rendered verification.

**Concurrent-work caveat:** the inventory grew from 174 to 178 while inspecting. Geothermal gained a Setup-time Bind; Plans146–149 gained construction/binding and compiled while their tests later failed. Findings reflect the final source checks described, not a frozen clean commit. Re-audit changes made after this timestamp.

**Test uncertainty:** the full xUnit session's result was lost, and final test-source compilation failed. The 19 passing route tests used an earlier built assembly. Earlier catalog/headless passes predate some concurrent additions. This report does not claim an all-green release gate.

The audit used ashfall-analyze for evidence/ownership classification and ashfall-ui-access for contrast, overflow and interaction analysis. Only audit documentation and the requested AGENTS.md record were changed; production defects remain open.
