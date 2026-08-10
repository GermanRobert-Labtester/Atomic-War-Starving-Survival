# ASHFALL (Working Title: Atomic War - Starving Survival)
## COMPREHENSIVE GAME DESIGN, CODEBASE ARCHITECTURE & MASTER REFERENCE MANUAL
> **Document Purpose:** This master reference document provides a complete, exhaustive, 360-degree blueprint of *ASHFALL*. It contains the full lore bible, system architecture, data models, mechanics, UI structure, current progress, and raw authored catalogs (321 items, 72 survivors, 16 recipes, 39 events, 15 echoes, 50 radio broadcasts, 5 locations). Use this text file directly in LLM chats (Qwen, Gemini, ChatGPT, Claude, Perplexity, Vibe Le Chat) for deep-dive brainstorming, feature expansion, narrative writing, and balance tuning.

---

## SYSTEM PROMPT & LLM INSTRUCTION HEADER
```
YOU ARE AN AI SYSTEM ARCHITECT AND LEAD GAME DESIGNER REVIEWING THE 'ASHFALL' SURVIVAL GAME MASTER DOCUMENT.
Context: ASHFALL is an original 2D atomic-war survival management game built in Unity 6 LTS (URP 2D, C#, UI Toolkit, ScriptableObjects + JSON, Utility AI).
Tone: Cold, exhausted, human, restrained. Inspired by This War of Mine, Darkest Dungeon, and Frostpunk. No magic, no fantasy, no real-world modern political references.
When generating ideas based on this document:
1. Respect all snake_case ID conventions and data schemas defined herein.
2. Align with the thin-MonoBehaviour / decoupled C# systems architecture.
3. Focus on systemic depth (radiation dosage, shelter degradation, medical pathologies, psychological decay, dynamic economy).
```

---

## CHAPTER 1: EXECUTIVE SUMMARY & GAME VISION

### 1.1 Core Pitch
*ASHFALL* is a hard-grit 2D atomic-war survival management simulation. The player assumes command of an underground fallout shelter (a former pre-war military sub-pen / civil defense bunker) in the upland market town of Tessarat following a catastrophic nuclear exchange between warring regional factions.
Unlike high-power post-apocalyptic power fantasies, *ASHFALL* is defined by scarcity, degradation, sickness, and moral erosion. Survivors are not action heroes; they are traumatized civilians, deserting soldiers, and exhausted specialists fighting against hunger, hypothermia, acute radiation sickness, air filtration failure, and structural collapses.

### 1.2 Aesthetic & Tone Guidelines
- **Visual Style:** 2D gritty graphic-novel aesthetic inspired by *This War of Mine* and *Darkest Dungeon*. Deep chiaroscuro light and shadow, desaturated palette dominated by cold ash blues, charcoal grays, rust oranges, and muted mud browns.
- **Tone:** Cold, exhausted, human, restrained. No heroic monologues or romanticized apocalypse. The tragedy is administrative, physical, and immediate.
- **Realism Guardrails:** Strict grounding in realistic post-nuclear physics and biology. Dosimeters click, iodine protects only the thyroid, blood types must match for transfusions, filters clog with ash, and cold is relentless. Zero fantasy, zero magic, zero aliens, zero sci-fi mutants with lasers.

### 1.3 High-Level Gameplay Loop
1. **Shelter Maintenance & Day Operations:** Manage power grids, air filtration units, water purification, heating fuel, room cleanliness, and structural integrity.
2. **Survivor Need Management:** Allocate rations, clean water, medical treatments, sleep shifts, and psychological support (or discipline) to keep survivors operational.
3. **Intel & Radio Interception:** Tune shortwave and VHF radio bands to gather plume reports, mortar warnings, trade convoy routes, and numbers station coordinates.
4. **Surface Scavenging Expeditions:** Dispatch survivors into irradiated zones (hospitals, gas stations, government bunkers, ruined suburbs) to loot vital components and medicine.
5. **Crisis & Narrative Event Resolution:** Face moral dilemmas, hatch raids, refugee arrivals, internal mutinies, mental breakdowns, and environmental disasters.
6. **Faction Relations & Victory Path Pursuit:** Build alliances or defend against factions (Central Garrison Remnants, Upland Militia, Cultists of the Glow, Warlords) to achieve one of 8 distinct endgame survival conditions.

---

## CHAPTER 2: NARRATIVE BIBLE & WORLD-BUILDING

### 2.1 Pre-War Context & The Fracture
For thirty years, a central government situated in the fertile river valley controlled trade routes, fuel pipelines, iodine stockpiles, and state broadcast networks. The upland provinces provided agricultural yields and raw materials, receiving subsidized grain and infrastructure in return.
When consecutive droughts dried the river system, mutual suspicion erupted into open conflict. The upland provincial assembly voted to withhold grain; the central garrison deployed armor and heavy artillery. The conflict escalated into ridgeline artillery duels, drone strikes on fuel depots, and grinding infantry attrition.

### 2.2 The Exchange & Timeline
- **Days -30 to 0 (Pre-War Escalation):** Propaganda broadcasts mask the collapsing diplomatic cables. Civil defense notices instruct citizens to test iodine pills and locate district fallout shelters.
- **Day 0 (The Exchange):** Detonations occur along strategic targets. An EMP blast disables unshielded electronics. High-altitude ash plumes enter the upper atmosphere.
- **Days 1 to 30 (Panic & Emergency Phase):** Chaos reigns on civilian radio bands. Heavy fallout rain settles over Tessarat. Counter-battery artillery fades into silence as military command structures collapse. Emergency ration centers run dry.
- **Day 30+ (Nuclear Winter & Silent Decay):** Automated emergency loops repeat corrupted instructions. Numbers stations broadcast single-use pad numbers. Temperatures plunge to -15°C to -30°C. Fallout storm fronts sweep the map. Survivors face chronic radiation and resource starvation.

### 2.3 Factions of the Wasteland
1. **Central Garrison Remnants:** Disciplined, heavily armed military survivors maintaining authoritarian protocol. They command tactical gear, fuel, and heavy weapons, but face desertion and logistics collapses.
2. **Upland Provincial Militia:** Local agrarian defense networks fighting for regional autonomy. They possess strong local knowledge, hunting gear, and hidden grain caches, but lack medical supplies.
3. **Cultists of the Glow:** Fanatical post-exchange cultists who view the nuclear fire as divine purging. They occupy high-rad zones and utilize psychological warfare.
4. **Scavenger Warlords:** Opportunistic raider gangs that exploit weak shelters, extort traders, and execute hatch raids.
5. **Safe Haven Communities / Rebuilders:** Fragile civilian collectives attempting to rebuild water filtration plants, libraries, and trade networks.

---

## CHAPTER 3: CODEBASE ARCHITECTURE & SYSTEM DESIGN

### 3.1 Stack & Frameworks
- **Engine:** Unity 6 LTS (2D URP Renderer with custom 2D lighting profiles).
- **Language:** C# (NET standard 2.1).
- **UI Framework:** UI Toolkit (UXML templates + USS stylesheets + C# UI View Controllers).
- **Architecture Pattern:** Decoupled Data-Driven Systems Architecture.
  - **Pure Logic Systems:** Plain C# classes handling math, simulation, and state modification.
  - **Thin MonoBehaviours:** View controllers handling input, rendering, and UI binding only.
  - **Event Bus:** Strong-typed Pub/Sub C# `EventBus` for all state mutations.
  - **ScriptableObject Catalogs:** Source-of-truth runtime catalogs populated by automated JSON importers.

### 3.2 Namespace & Directory Map
The codebase strictly follows the namespace scheme `AtomicWar._Game.<Module>`:

#### Module `AtomicWar._Game.AI` (47 files)
- `AIContext.cs`
- `ActionScorer.cs`
- `BeginChelationActionSO.cs`
- `BoilToolsActionSO.cs`
- `BuildWindTurbineActionSO.cs`
- `CaregiveActionSO.cs`
- `ChartMapActionSO.cs`
- `ClearRubbleActionSO.cs`
- `CompostWasteActionSO.cs`
- `CraftActionSO.cs`
- `DeconAndEnterActionSO.cs`
- `DecontaminateActionSO.cs`
- `DrinkActionSO.cs`
- `DrinkContaminatedWaterActionSO.cs`
- `EatActionSO.cs`
- `ExcavateActionSO.cs`
- `ExcavateEscapeHatchActionSO.cs`
- `GuardActionSO.cs`
- `HaulLootActionSO.cs`
- `HuntRatsActionSO.cs`
- `ListenToRadioActionSO.cs`
- `MentalBreakComfortActionSO.cs`
- `MercyKillActionSO.cs`
- `PanicActionSO.cs`
- `PedalGeneratorActionSO.cs`
- `PhantomActionSO.cs`
- `PurifyWaterActionSO.cs`
- `RefuelHeaterActionSO.cs`
- `RepairFilterActionSO.cs`
- `RestActionSO.cs`
- `ScavengeActionSO.cs`
- `SearchForChemsActionSO.cs`
- `SleepActionSO.cs`
- `SleepwalkActionSO.cs`
- `SuppressingFireActionSO.cs`
- `SurveyActionSO.cs`
- `TakeIodineActionSO.cs`
- `TalkDownActionSO.cs`
- `TeachSkillActionSO.cs`
- `TreatPatientActionSO.cs`
- `TunnelActionSO.cs`
- `UpgradeShieldingActionSO.cs`
- `UseAntiRadActionSO.cs`
- `WarmUpActionSO.cs`
- `HallucinationSystem.cs`
- `SurvivorAction.cs`
- `UtilityAI.cs`

#### Module `AtomicWar._Game.Core` (611 files)
- `Action_AdministerPlacebo.cs`
- `Action_BarricadeDoor.cs`
- `Action_BoilBatteries.cs`
- `Action_BroadcastPropaganda.cs`
- `Action_BurnCharcoal.cs`
- `Action_BuryTimeCapsule.cs`
- `Action_CallCaravan.cs`
- `Action_CoverTracks.cs`
- `Action_CrackMainframe.cs`
- `Action_Crawlspace.cs`
- `Action_Decrypt.cs`
- `Action_DemandTribute.cs`
- `Action_EstablishRoute.cs`
- `Action_Exile.cs`
- `Action_Fish.cs`
- `Action_HarvestOrgans.cs`
- `Action_InfectSelf.cs`
- `Action_IsotopeTrace.cs`
- `Action_Mercy.cs`
- `Action_MixCement.cs`
- `Action_MixChems.cs`
- `Action_Overwatch.cs`
- `Action_PhysicalTherapy.cs`
- `Action_PirateRadio.cs`
- `Action_PlaceBait.cs`
- `Action_Play.cs`
- `Action_PullTooth.cs`
- `Action_RigCorpse.cs`
- `Action_RoutePower.cs`
- `Action_Sabotage.cs`
- `Action_ScorchedEarth.cs`
- `Action_SealRoom.cs`
- `Action_SelfSurgery.cs`
- `Action_SilentTakedown.cs`
- `Action_SiphonGas.cs`
- `Action_SlaughterPet.cs`
- `Action_StabilizeDNA.cs`
- `Action_Stargazing.cs`
- `Action_TeachChild.cs`
- `Action_TellStories.cs`
- `Action_WorshipIdol.cs`
- `Affliction_AdrenalineCrash.cs`
- `Affliction_Amnesia.cs`
- `Affliction_Brainwashed.cs`
- `Affliction_BrittleBones.cs`
- `Affliction_BunkerFever.cs`
- `Affliction_CaveMadness.cs`
- `Affliction_FeralRegression.cs`
- `Affliction_ImaginaryFriend.cs`
- `Affliction_NerveDamage.cs`
- `Affliction_OldAge.cs`
- `Affliction_PhantomLimb.cs`
- `Affliction_RadHallucinations.cs`
- `Affliction_RadiationBlindness.cs`
- `Affliction_ScurvyDegeneration.cs`
- `Affliction_SporeLung.cs`
- `Affliction_Sterile.cs`
- `Affliction_SurvivorsGuilt.cs`
- `Affliction_TBI.cs`
- `Affliction_ThyroidCancer.cs`
- `Affliction_TrenchFoot.cs`
- `Affliction_ZoonoticFlu.cs`
- `AshDriftSystem.cs`
- `AudioEventBus.cs`
- `AudioEvent_Deafening.cs`
- `AudioEvent_Heartbeat.cs`
- `BicycleSystem.cs`
- `Biome_AshSwamp.cs`
- `Biome_GlassDesert.cs`
- `Biome_HighwayTunnel.cs`
- `Biome_SaltFlats.cs`
- `Biome_SkyscraperTops.cs`
- `Biome_Suburbs.cs`
- `BlackRainHazardSystem.cs`
- `BloodToxicitySystem.cs`
- `BunkerSocialDirector.cs`
- `BurnWardSystem.cs`
- `CampaignResult.cs`
- `ChemUseRouter.cs`
- `CognitiveDecaySystem.cs`
- `CombatStance_LastStand.cs`
- `Combat_BleedOut.cs`
- `Combat_Flanking.cs`
- `Combat_Suppression.cs`
- `CookingSystem.cs`
- `CorpseManagementSystem.cs`
- `Crisis_FeralFlora.cs`
- `Crisis_StructuralFailure.cs`
- `CultMoralDisgustSystem.cs`
- `DeadDropSystem.cs`
- `DebtCollectorSystem.cs`
- `DeserterSystem.cs`
- `DesertersStandSystem.cs`
- `DiagnosticsOverlay.cs`
- `DiseaseSystem_Expansion.cs`
- `Durability_Suppressor.cs`
- `Dynamic_Scapegoat.cs`
- `EMPEvent.cs`
- `EncounterSO.cs`
- `Encounter_Amalgamation.cs`
- `Encounter_Burrowers.cs`
- `Encounter_DeadLetterOffice.cs`
- `Encounter_FloodedMaze.cs`
- `Encounter_GlowingDead.cs`
- `Encounter_GlowingStag.cs`
- `Encounter_HitAndRun.cs`
- `Encounter_Leeches.cs`
- `Encounter_Mirelurker.cs`
- `Encounter_Pianist.cs`
- `Encounter_PressurePlate.cs`
- `Encounter_RiverPirates.cs`
- `Encounter_Roadblock.cs`
- `Encounter_RobotDog.cs`
- `Encounter_SleepingCamp.cs`
- `Encounter_TripwireMaze.cs`
- `Encounter_WarlordTank.cs`
- `Encounter_WeatherStation.cs`
- `EndgameConditionKind.cs`
- `EndgameEngine.cs`
- `EndgameState.cs`
- `Endgame_Ultimatum.cs`
- `EventBus.cs`
- `EventPoolBuilder.cs`
- `Event_Brawl.cs`
- `Event_ComingOfAge.cs`
- `Event_CultBlessing.cs`
- `Event_CultInitiation.cs`
- `Event_CultOfAI.cs`
- `Event_EMPCascade.cs`
- `Event_EuthanasiaPact.cs`
- `Event_FactionMerger.cs`
- `Event_FeralRescue.cs`
- `Event_FoundDiary.cs`
- `Event_GriefCascade.cs`
- `Event_HungerStrike.cs`
- `Event_Mudslide.cs`
- `Event_NodeCollapse.cs`
- `Event_NumbersStation.cs`
- `Event_ProjectSabotage.cs`
- `Event_RansomNote.cs`
- `Event_Schism.cs`
- `Event_SecretSociety.cs`
- `Event_SiblingFeud.cs`
- `Event_Sinkhole.cs`
- `Event_SpontaneousMurder.cs`
- `Event_TeenRebellion.cs`
- `Event_Triangulation.cs`
- `Event_VaultCollision.cs`
- `Event_WarlordSuccession.cs`
- `Event_WitchHunt.cs`
- `ExpeditionEncounterLog.cs`
- `ExpeditionState.cs`
- `ExpeditionSystem.Encounters.cs`
- `ExpeditionSystem.Flashpoint.cs`
- `ExpeditionSystem.Narrative.cs`
- `ExpeditionSystem.Ops.cs`
- `ExpeditionSystem.Start.cs`
- `ExpeditionSystem.Tick.cs`
- `ExpeditionSystem.Uxo.cs`
- `ExpeditionSystem.cs`
- `FactionRadioInterceptSystem.cs`
- `FactionRaidPlanSystem.cs`
- `FalloutStormHazardSystem.cs`
- `FlashpointChoreographer.Steps.cs`
- `FlashpointChoreographer.cs`
- `FlashpointChoreographerSave.cs`
- `FlashpointEvents.cs`
- `FlashpointInterceptSignal.cs`
- `FlashpointSequenceSO.cs`
- `FloodedNodeSystem.cs`
- `FuelDecaySystem.cs`
- `GameBootstrap.BatchSystems.cs`
- `GameBootstrap.Biomes.cs`
- `GameBootstrap.BunkerSocial.cs`
- `GameBootstrap.CoreFamilies.cs`
- `GameBootstrap.Diagnostics.cs`
- `GameBootstrap.Encounters.cs`
- `GameBootstrap.EventTrackers.cs`
- `GameBootstrap.Events.cs`
- `GameBootstrap.Handlers.Choices.cs`
- `GameBootstrap.Handlers.Parley.cs`
- `GameBootstrap.Handlers.cs`
- `GameBootstrap.Hatch.cs`
- `GameBootstrap.Hud.cs`
- `GameBootstrap.InitFoundation.Medical.cs`
- `GameBootstrap.InitFoundation.cs`
- `GameBootstrap.InitLate.Radio.cs`
- `GameBootstrap.InitLate.cs`
- `GameBootstrap.InitWorld.Diary.cs`
- `GameBootstrap.InitWorld.Narrative.cs`
- `GameBootstrap.InitWorld.cs`
- `GameBootstrap.InitializeSystems.cs`
- `GameBootstrap.InternalHorror.cs`
- `GameBootstrap.Lifecycle.cs`
- `GameBootstrap.MapAnomalies.cs`
- `GameBootstrap.MapHazards.cs`
- `GameBootstrap.Missions.Mental.cs`
- `GameBootstrap.Missions.Party.cs`
- `GameBootstrap.Missions.PartyHelpers.Expedition.cs`
- `GameBootstrap.Missions.PartyHelpers.cs`
- `GameBootstrap.Missions.Trade.cs`
- `GameBootstrap.Missions.cs`
- `GameBootstrap.ModLoader.cs`
- `GameBootstrap.RadiationExposure.cs`
- `GameBootstrap.Radio.cs`
- `GameBootstrap.Registry.cs`
- `GameBootstrap.ShelterLayout.cs`
- `GameBootstrap.ShelterModules.cs`
- `GameBootstrap.TickSystems.Ai.cs`
- `GameBootstrap.TickSystems.cs`
- `GameBootstrap.TwitchAPI.cs`
- `GameBootstrap.UiActions.Create.cs`
- `GameBootstrap.UiActions.Endgame.cs`
- `GameBootstrap.UiActions.Hud.cs`
- `GameBootstrap.UiActions.Radio.cs`
- `GameBootstrap.UiActions.Seed.cs`
- `GameBootstrap.UiActions.cs`
- `GameBootstrap.VictoryPaths.cs`
- `GameBootstrap.Weather.cs`
- `GameBootstrap.cs`
- `GameState.cs`
- `GhostStationSystem.Ops.cs`
- `GhostStationSystem.cs`
- `GraftRejectionSystem.cs`
- `HatchDilemmaPrompt.cs`
- `HatchEntrapmentSystem.cs`
- `HatchState.cs`
- `Hazard_CookOff.cs`
- `Hazard_ExplosiveCrafting.cs`
- `Hazard_FriendlyFire.cs`
- `Hazard_Methane.cs`
- `Hazard_MimicCrate.cs`
- `Hazard_SurgicalBotch.cs`
- `Hazard_WeaponBurst.cs`
- `HiddenStat_Unseen.cs`
- `HostageSystem.cs`
- `HouseToBunkerSystem.cs`
- `ISaveable.cs`
- `Item_AICoreData.cs`
- `Item_AmmoTypes.Catalog.cs`
- `Item_AmmoTypes.CombatLoot.cs`
- `Item_AmmoTypes.Ui.cs`
- `Item_AmmoTypes.cs`
- `Item_Ammonia.cs`
- `Item_Amphetamines.cs`
- `Item_AshGhillie.cs`
- `Item_AshGoat.cs`
- `Item_AutoDoc.cs`
- `Item_BioPlastic.cs`
- `Item_BloodBag.cs`
- `Item_BoneSaw.cs`
- `Item_Boots.cs`
- `Item_C4.cs`
- `Item_Caltrops.cs`
- `Item_CarrierBird.cs`
- `Item_ChildsDrawing.cs`
- `Item_Cigarettes.cs`
- `Item_ClimbingGear.cs`
- `Item_Decoy.cs`
- `Item_DogTags.cs`
- `Item_EMPGrenade.cs`
- `Item_EncryptedDrive.cs`
- `Item_EpiPen.cs`
- `Item_Exosuit.cs`
- `Item_FaradayPack.cs`
- `Item_ForeignBook.cs`
- `Item_GeigerCalibrator.cs`
- `Item_GlowingMushroom.cs`
- `Item_GoldBars.cs`
- `Item_Guitar.cs`
- `Item_Heirloom.cs`
- `Item_IBeam.cs`
- `Item_ImpureIodine.cs`
- `Item_JuggernautArmor.cs`
- `Item_KevlarVest.cs`
- `Item_Keycards.cs`
- `Item_Landmine.cs`
- `Item_LeadApron.cs`
- `Item_LiquidStitches.cs`
- `Item_LiveTrap.cs`
- `Item_Maggots.cs`
- `Item_MilGasMask.cs`
- `Item_MutantChicken.cs`
- `Item_MutantGland.cs`
- `Item_Nanites.cs`
- `Item_NightVision.cs`
- `Item_PackMule.cs`
- `Item_PasswordNote.cs`
- `Item_PhotoAlbum.cs`
- `Item_PotassiumIodide.cs`
- `Item_PresidentialSeal.cs`
- `Item_PrussianBlue.cs`
- `Item_RTGBattery.cs`
- `Item_SeedLedger.cs`
- `Item_ShockCollar.cs`
- `Item_Snowshoes.cs`
- `Item_SurgicalTubing.cs`
- `Item_TearGas.cs`
- `Item_TeddyBear.cs`
- `Item_Toys.cs`
- `Item_TrashHazmat.cs`
- `Item_UndeliveredMail.cs`
- `Item_VacuumTubes.cs`
- `Item_VinylCollection.cs`
- `Item_Vitamins.cs`
- `Item_WalkieTalkie.cs`
- `Item_WastelandSoap.cs`
- `Item_WaterTabs.cs`
- `Item_WeldingGoggles.cs`
- `Item_WorldCatalog.Expanded.cs`
- `Item_WorldCatalog.Loot.cs`
- `Item_WorldCatalog.cs`
- `Item_WristDosimeter.cs`
- `LaborCampSystem.cs`
- `LastWillSystem.cs`
- `LifeboatTransmissionSystem.cs`
- `LightningStrikesSystem.cs`
- `LocationQuestSystem.cs`
- `LocationScavengingSystem.cs`
- `LocationStateRuinSystem.cs`
- `Location_Arcade.cs`
- `Location_SlaveMarket.cs`
- `Location_StrandedYacht.cs`
- `LogRotationManager.cs`
- `MapAnomaly_AshDunes.cs`
- `MapAnomaly_BoilingLake.cs`
- `MapAnomaly_Cherenkov.cs`
- `MapAnomaly_DogDen.cs`
- `MapAnomaly_DontLook.cs`
- `MapAnomaly_DryCoral.cs`
- `MapAnomaly_FloodedSubway.cs`
- `MapAnomaly_GlassCrater.cs`
- `MapAnomaly_MassGrave.cs`
- `MapAnomaly_Mirage.cs`
- `MapAnomaly_PetrifiedForest.cs`
- `MapAnomaly_QuietZone.cs`
- `MapAnomaly_RustedTank.cs`
- `MapAnomaly_ServerFarm.cs`
- `MapAnomaly_Sinkhole.cs`
- `MapAnomaly_TangledDrop.cs`
- `MapAnomaly_TireFire.cs`
- `MapAnomaly_UXO_Nuke.cs`
- `MapHazard_AcidGeyser.cs`
- `MapHazard_Ashlanche.cs`
- `MapHazard_BiometricDoor.cs`
- `MapHazard_CraterWall.cs`
- `MapHazard_Crevice.cs`
- `MapHazard_FlammableGas.cs`
- `MapHazard_FrozenSurvivor.cs`
- `MapHazard_GasPockets.cs`
- `MapHazard_MagneticAnomaly.cs`
- `MapHazard_SinkholeCollapse.cs`
- `MapHazard_VenusTrap.cs`
- `Map_Aquifer.cs`
- `MobileCampSystem.cs`
- `Mode_IronMan.cs`
- `Module_RationLock.cs`
- `MoralChronicleBridge.cs`
- `MoralDilemmaSystem.cs`
- `MutatedEcosystemSystem.cs`
- `NPC_AddictsPassive.cs`
- `NPC_AggroScavengers.cs`
- `NPC_AggroTrader.cs`
- `NPC_Android.cs`
- `NPC_Bandits.cs`
- `NPC_BlackOps.cs`
- `NPC_Broker.cs`
- `NPC_Cannibals.cs`
- `NPC_ChemScientists.cs`
- `NPC_CityResidents.cs`
- `NPC_Collaborators.cs`
- `NPC_Conscripts.cs`
- `NPC_DesperateFamily.cs`
- `NPC_DrunksAggro.cs`
- `NPC_Homeless.cs`
- `NPC_LonePsychopath.cs`
- `NPC_Looters.cs`
- `NPC_Mercenaries.cs`
- `NPC_MilitaryPatrol.cs`
- `NPC_PassiveScavengers.cs`
- `NPC_PassiveTrader.cs`
- `NPC_PsychopathPair.cs`
- `NPC_RebelMilitia.cs`
- `NPC_RebelModerates.cs`
- `NPC_RebelSnipers.cs`
- `NPC_RebelZealots.cs`
- `NPC_Slavers.cs`
- `NPC_SpecOps.cs`
- `NPC_Survivalists.cs`
- `NPC_TaxCollector.cs`
- `NPC_Terrorists.cs`
- `NPC_TheNegotiator.cs`
- `NPC_TheOld.cs`
- `NPC_TheParents.cs`
- `NPC_TravelingCouple.cs`
- `NarrativeChainEngine.cs`
- `NarrativeEncounters.cs`
- `NeedleSterilizationSystem.cs`
- `NightScavengeSystem.cs`
- `Node_AutomatedArmory.cs`
- `Node_GhostShip.cs`
- `Node_MutantHive.cs`
- `Node_Orphanage.cs`
- `Node_PlayerBank.cs`
- `Node_Sector7G.cs`
- `Node_SporeHive.cs`
- `NutrientDripAutomation.cs`
- `PantryContaminationSystem.cs`
- `ParleyOfferPrompt.cs`
- `Pet_FeralCat.cs`
- `Pet_GuardDog.cs`
- `PheromoneMaskingSystem.cs`
- `PlayerInputHandler.cs`
- `Project_BioReactor.cs`
- `Project_DeepWell.cs`
- `Project_Elevator.cs`
- `Project_Minecart.cs`
- `Project_RadioArray.cs`
- `Project_SurfaceDome.cs`
- `PropagandaSystem.cs`
- `ProstheticCraftingSystem.cs`
- `RadioBroadcastSystem.cs`
- `RadioState.cs`
- `RadioTunerSystem.cs`
- `Role_Sheriff.cs`
- `SabotagedCacheSystem.cs`
- `SafeHavenEncounters.cs`
- `SaveMap.cs`
- `SaveSystem.Capture.Entities.cs`
- `SaveSystem.Capture.World.cs`
- `SaveSystem.Capture.cs`
- `SaveSystem.Dtos.cs`
- `SaveSystem.Entities.Survivors.cs`
- `SaveSystem.Entities.cs`
- `SaveSystem.IO.cs`
- `SaveSystem.Restore.Expeditions.cs`
- `SaveSystem.Restore.Medical.cs`
- `SaveSystem.Restore.World.cs`
- `SaveSystem.Restore.cs`
- `SaveSystem.Wiring.cs`
- `SaveSystem.cs`
- `SeismicVentsSystem.cs`
- `SevereFrostbiteSystem.cs`
- `ShelterEvent_CaravanAmbush.cs`
- `ShelterEvent_FalseCure.cs`
- `ShelterEvent_Ransom.cs`
- `ShelterEvent_Refugees.cs`
- `ShelterEvent_TheMirror.cs`
- `ShelterEvent_Tribute.cs`
- `ShelterModule_AcidTrap.cs`
- `ShelterModule_Autodoc.cs`
- `ShelterModule_Autopsy.cs`
- `ShelterModule_BatteryBank.cs`
- `ShelterModule_BioLatrine.cs`
- `ShelterModule_CCTV.cs`
- `ShelterModule_ChoreBoard.cs`
- `ShelterModule_Classroom.cs`
- `ShelterModule_Confessional.cs`
- `ShelterModule_Conveyor.cs`
- `ShelterModule_DaylightSensor.cs`
- `ShelterModule_DeadManSwitch.cs`
- `ShelterModule_DeconShower.cs`
- `ShelterModule_Dialysis.cs`
- `ShelterModule_DistressBeacon.cs`
- `ShelterModule_DronePad.cs`
- `ShelterModule_DroneStation.cs`
- `ShelterModule_Garage.cs`
- `ShelterModule_GunRack.cs`
- `ShelterModule_Hammock.cs`
- `ShelterModule_HandCrank.cs`
- `ShelterModule_HoloEmitter.cs`
- `ShelterModule_HotShower.cs`
- `ShelterModule_Incinerator.cs`
- `ShelterModule_InsectFarm.cs`
- `ShelterModule_Lathe.cs`
- `ShelterModule_MagmaTap.cs`
- `ShelterModule_Mortar.cs`
- `ShelterModule_MotionSensor.cs`
- `ShelterModule_PanicButton.cs`
- `ShelterModule_PanicRoom.cs`
- `ShelterModule_Pitfall.cs`
- `ShelterModule_PrintingPress.cs`
- `ShelterModule_PunchingBag.cs`
- `ShelterModule_RainBarrel.cs`
- `ShelterModule_RecordPlayer.cs`
- `ShelterModule_Reloader.cs`
- `ShelterModule_Sorter.cs`
- `ShelterModule_Sprinklers.cs`
- `ShelterModule_Thermostat.cs`
- `ShelterModule_Thumper.cs`
- `ShelterModule_TreadmillGen.cs`
- `ShelterModule_Turret.cs`
- `ShelterModule_VaultDoor.cs`
- `ShelterModule_WasteChute.cs`
- `ShelterModule_WoodStove.cs`
- `ShiftingHotspotSystem.cs`
- `Siege_Artillery.cs`
- `Siege_Biowarfare.cs`
- `Siege_Blockade.cs`
- `Siege_HostageShield.cs`
- `Siege_NightRaid.cs`
- `Siege_Sappers.cs`
- `Siege_SmokeOut.cs`
- `Siege_VehicleRam.cs`
- `SkirmishEncounter.cs`
- `Skirmish_Bandit_vs_Terror.cs`
- `Skirmish_Mil_vs_Rebel.cs`
- `Skirmish_Mil_vs_Terror.cs`
- `Skirmish_Rebel_vs_Bandit.cs`
- `Skirmish_Rebel_vs_Terror.cs`
- `SystemRegistry.cs`
- `SystemWiring.cs`
- `System_AdaptiveWarlords.cs`
- `System_BilgePumps.cs`
- `System_BloodTypes.cs`
- `System_CarrionBirds.cs`
- `System_EpilogueStats.cs`
- `System_Gossip.cs`
- `System_LegacyStart.cs`
- `System_LogicGates.cs`
- `System_ModLoader.cs`
- `System_Tolerance.cs`
- `System_TwitchAPI.cs`
- `TetanusAfflictionSystem.cs`
- `TimeSystem.cs`
- `ToothDecaySystem.cs`
- `TrackerSystem.cs`
- `Trader_PlagueConvoy.cs`
- `Trait_Anthropophobia.cs`
- `Trait_AshTongue.cs`
- `Trait_Clairvoyant.cs`
- `Trait_GenerationalTrauma.cs`
- `Trait_InheritedGenetics.cs`
- `Trait_Kleptomaniac.cs`
- `Trait_Mascot.cs`
- `Trait_Matriarch.cs`
- `Trait_PTSD.cs`
- `Trait_StuntedEmpathy.cs`
- `Trait_Superstitious.cs`
- `UIEvent_BlurredVision.cs`
- `UIEvent_CorruptionScare.cs`
- `UIEvent_FalseInventory.cs`
- `UIEvent_GhostRadio.cs`
- `UIEvent_Hacking.cs`
- `UIEvent_LowPower.cs`
- `UIEvent_MapRot.cs`
- `UIEvent_PhantomBlip.cs`
- `UI_ScenarioGen.cs`
- `UI_SpeedrunTimer.cs`
- `UxoHazardSystem.cs`
- `VehicleStrandingSystem.cs`
- `VehicleSystem.cs`
- `Vehicle_ArmoredTruck.cs`
- `Vehicle_Motorcycle.cs`
- `Vehicle_Rowboat.cs`
- `VictoryProjectManager.cs`
- `Victory_Airlift.cs`
- `Victory_Ascendancy.cs`
- `Victory_BuriedAlive.cs`
- `Victory_CannibalKing.cs`
- `Victory_Defection.cs`
- `Victory_Icebreaker.cs`
- `Victory_LoneSurvivor.cs`
- `Victory_MAD.cs`
- `Victory_Migration.cs`
- `Victory_TheBroadcast.cs`
- `Victory_TheCure.cs`
- `Victory_TheMartian.cs`
- `Victory_TrueEnding.cs`
- `Victory_UndergroundCity.cs`
- `Victory_Unifier.cs`
- `VisionLossSystem.cs`
- `VisitorDeck.cs`
- `VisitorRNGSystem.cs`
- `Visitor_AbandonedState.cs`
- `Visitor_ChurchHostile.cs`
- `Visitor_ChurchSanctuary.cs`
- `Visitor_ExplodedState.cs`
- `Visitor_FleeingHorde.cs`
- `Visitor_HospitalPatients.cs`
- `Visitor_HospitalStaff.cs`
- `Visitor_MilTrainingYard.cs`
- `Visitor_QuestFaction.cs`
- `Visitor_RebelTrainingYard.cs`
- `WaterEconomySystem.cs`
- `Weapon_Chainsaw.cs`
- `Weapon_Flamethrower.cs`
- `Weapon_HMG.cs`
- `Weapon_RPG.cs`
- `WeatherScapegoatSystem.cs`
- `Weather_AcidSnow.cs`
- `Weather_BioFog.cs`
- `Weather_BlackSnow.cs`
- `Weather_BloodRain.cs`
- `Weather_DeadWind.cs`
- `Weather_DeepFreeze.cs`
- `Weather_DustDevil.cs`
- `Weather_EMPStorm.cs`
- `Weather_FalseSpring.cs`
- `Weather_GlassStorm.cs`
- `Weather_OzoneHole.cs`
- `Weather_RadHail.cs`
- `Weather_SilentSpring.cs`
- `Weather_SolarFlare.cs`
- `Weather_StaticCharge.cs`
- `WorldEvent_Deforestation.cs`
- `WorldEvent_FinalWinter.cs`
- `WorldEvent_Fissure.cs`
- `WorldEvent_GreatFamine.cs`
- `WorldEvent_Megafauna.cs`
- `WorldPhaseSystem.cs`

#### Module `AtomicWar._Game.Crafting` (4 files)
- `CraftingStation.cs`
- `CraftingSystem.cs`
- `Recipe.cs`
- `WorkbenchSystem.cs`

#### Module `AtomicWar._Game.Data` (20 files)
- `DiaryFragmentSO.cs`
- `EncounterEventFactory.cs`
- `FixedLocationSO.cs`
- `GameEventCatalogSO.cs`
- `IntelNode.cs`
- `ItemCatalogSO.cs`
- `LocationCatalogSO.cs`
- `LocationDefinitionSO.cs`
- `LocationQuestNodeFactory.cs`
- `LocationStateModifier.cs`
- `LootTableSO.cs`
- `RadioBroadcastSO.cs`
- `RadioCatalogSO.cs`
- `RadioFrequencySO.cs`
- `RecipeCatalogSO.cs`
- `ShelterLayoutFactory.cs`
- `SurvivorArchetypeSO.cs`
- `SurvivorCatalogSO.cs`
- `TradeEconomy.cs`
- `WorldPhaseConfigSO.cs`

#### Module `AtomicWar._Game.Economy` (4 files)
- `BiologicalTradeItem.cs`
- `DynamicEconomySystem.cs`
- `FactionSO.cs`
- `TradeStance.cs`

#### Module `AtomicWar._Game.Editor` (14 files)
- `AssetManifestReport.cs`
- `BalanceReportWindow.cs`
- `BuildScript.cs`
- `CatalogGenerator.cs`
- `CreateDefaultBeliefProfiles.cs`
- `DiegeticHudDeploy.cs`
- `EventIdValidator.Check.cs`
- `EventIdValidator.Collect.cs`
- `EventIdValidator.cs`
- `GameplaySceneBuilder.cs`
- `GenerateFactionRadioVoLibrary.cs`
- `JsonDataImporter.cs`
- `MainMenuAssetGenerator.cs`
- `StartScreenDeploy.cs`

#### Module `AtomicWar._Game.Environment` (14 files)
- `DangerRing.cs`
- `FalloutMap.cs`
- `GeneratedMap.cs`
- `MapGenerator.cs`
- `MapNode.cs`
- `MapTile.cs`
- `PhotoperiodSystem.cs`
- `RadiationKnowledgeMap.cs`
- `RiverNodeSystem.cs`
- `SeasonProfile.cs`
- `TemperatureSystem.cs`
- `WeatherState.cs`
- `WeatherSystem.cs`
- `Weather_AlgaeBloom.cs`

#### Module `AtomicWar._Game.Events` (17 files)
- `EventContext.cs`
- `EventRunner.Apply.cs`
- `EventRunner.Factories.cs`
- `EventRunner.Journal.cs`
- `EventRunner.Selection.Choices.cs`
- `EventRunner.Selection.cs`
- `EventRunner.Tick.cs`
- `EventRunner.cs`
- `GameEvent.cs`
- `IntelReliability.cs`
- `JournalEntry.cs`
- `JournalSystem.cs`
- `JournalVoice.cs`
- `KnowledgeBase.cs`
- `MoralChronicleEntry.cs`
- `ScheduledEvent.cs`
- `SuspicionTracker.cs`

#### Module `AtomicWar._Game.Inventory` (7 files)
- `EquipSlot.cs`
- `EquipSlots.cs`
- `Inventory.cs`
- `ItemDefinition.cs`
- `ItemType.cs`
- `Item_TradeValues.cs`
- `ScrapValue.cs`

#### Module `AtomicWar._Game.Medical` (10 files)
- `ActiveAffliction.cs`
- `AddictionSystem.cs`
- `AfflictionPhase.cs`
- `AfflictionSO.cs`
- `AmputationSystem.cs`
- `BloodTransfusionSystem.cs`
- `DisabilitySO.cs`
- `MedicalSystem.cs`
- `ScurvySystem.cs`
- `TreatmentRecipeSO.cs`

#### Module `AtomicWar._Game.Radiation` (14 files)
- `AfflictionPipeline.cs`
- `Contamination.cs`
- `DecontaminationStation.cs`
- `DeviceState.cs`
- `Dosimeter.cs`
- `ExposureContext.cs`
- `GeigerCounter.cs`
- `InstrumentDevice.cs`
- `PrognosisPipeline.cs`
- `ProtectiveGear.cs`
- `RadZoneProfile.cs`
- `RadiationMutagenesisSystem.cs`
- `RadiationSystem.cs`
- `WornGear.cs`

#### Module `AtomicWar._Game.Settings` (1 files)
- `SettingsManager.cs`

#### Module `AtomicWar._Game.Shelter` (61 files)
- `AirFiltration.cs`
- `AirlockSystem.cs`
- `CartographySystem.cs`
- `CeilingCollapseSystem.cs`
- `CropLifecycleStage.cs`
- `CropSO.cs`
- `EscapeHatchSystem.cs`
- `ExcavationSystem.cs`
- `FreezePipeSystem.cs`
- `HatchDefenseSystem.BreachLoot.cs`
- `HatchDefenseSystem.HatchUpgrades.cs`
- `HatchDefenseSystem.RaidOutcomes.cs`
- `HatchDefenseSystem.RaidResolution.cs`
- `HatchDefenseSystem.RepelCosts.cs`
- `HatchDefenseSystem.cs`
- `HatchVisibilitySystem.cs`
- `HiddenStorageSystem.cs`
- `InternalLockSystem.cs`
- `JuryRigSystem.cs`
- `MaterialShieldingSystem.cs`
- `AirFiltrationModuleSO.cs`
- `BedModuleSO.cs`
- `CatchmentSurfaceModuleSO.cs`
- `ComfortStationModuleSO.cs`
- `DeconStationModuleSO.cs`
- `GrowLightModuleSO.cs`
- `HatchDefenseModuleSO.cs`
- `HeaterModuleSO.cs`
- `MedicalBedModuleSO.cs`
- `RadiationShieldingModuleSO.cs`
- `RadioModuleSO.cs`
- `StoveModuleSO.cs`
- `WaterPurifierModuleSO.cs`
- `WorkbenchModuleSO.cs`
- `NoiseSystem.cs`
- `PerimeterTrapSystem.cs`
- `PlanterBox.cs`
- `PowerConsumer.cs`
- `PowerNetwork.cs`
- `PowerSourceInstance.cs`
- `PowerSourceKind.cs`
- `PowerSourceSO.cs`
- `RoomFloodingSystem.cs`
- `Shelter.Ops.cs`
- `Shelter.cs`
- `ShelterAtmosphereSystem.cs`
- `ShelterLayout_SubPen.cs`
- `ShelterMapSO.cs`
- `ShelterModule.cs`
- `ShelterModuleInstance.cs`
- `ShelterRoom.cs`
- `ShelterUpgrader.cs`
- `Shielding.cs`
- `SleepQuality.cs`
- `StorageLayoutSO.cs`
- `StorageSlot.cs`
- `StructuralIntegritySystem.cs`
- `TunnelingSystem.cs`
- `VerminSystem.cs`
- `WasteSystem.cs`
- `WaterStorage.cs`

#### Module `AtomicWar._Game.Simulation` (4 files)
- `SimulationSystems.Core.cs`
- `SimulationSystems.Medical.cs`
- `SimulationSystems.Ops.cs`
- `SimulationSystems.cs`

#### Module `AtomicWar._Game.Survivors` (50 files)
- `BeliefProfileCatalogSO.cs`
- `BeliefProfileSO.cs`
- `BeliefSystem.cs`
- `BunkerSocialSystems.cs`
- `ChildDependentSystem.cs`
- `ChronicIllnessKind.cs`
- `ClothingDegradationSystem.cs`
- `CombatPerkSystem.cs`
- `DesperateChoiceKind.cs`
- `DisabilityId.cs`
- `EmpathSystem.cs`
- `ExpeditionPerkSystem.cs`
- `GriefKeepsakeSystem.cs`
- `InterpersonalAffinity.cs`
- `LightProfile.cs`
- `LightSystemHelper.cs`
- `MedicalPerkSystem.cs`
- `MentalBreakCatalogSO.cs`
- `MentalBreakSO.cs`
- `MentalBreakSystem.cs`
- `MentorshipSystem.cs`
- `MoralDilemmaEvent.cs`
- `Needs.cs`
- `NeedsProfile.cs`
- `NeedsSystem.PersonalQuests.cs`
- `NeedsSystem.cs`
- `PerkSO.cs`
- `PersonalQuestSystem.Ashes.cs`
- `PersonalQuestSystem.Chemistry.cs`
- `PersonalQuestSystem.CivilWar.cs`
- `PersonalQuestSystem.Rebuilders.cs`
- `PersonalQuestSystem.Titles.cs`
- `PersonalQuestSystem.cs`
- `PetState.cs`
- `PetSystem.cs`
- `PhantomIntruderSystem.cs`
- `PrognosisStage.cs`
- `QuestlineSO.cs`
- `RiskBiasTrait.cs`
- `ShelterPerkSystem.cs`
- `SkillAtrophySystem.cs`
- `SkillProgressionSystem.cs`
- `SocialPerkSystem.cs`
- `SpatialPsychologySystem.cs`
- `SurvivalPerkSystem.cs`
- `Survivor.cs`
- `SurvivorDiariesSystem.cs`
- `SurvivorNeedWrite.cs`
- `SurvivorStatus.cs`
- `WorldPhase.cs`

#### Module `AtomicWar._Game.UI` (33 files)
- `DiegeticHudController.cs`
- `DiegeticHudView.cs`
- `DosimeterHUD.cs`
- `EndgameSummaryUI.cs`
- `EnvironmentStatusHUD.cs`
- `EventModalUI.cs`
- `ExpeditionEncounterLogHUD.cs`
- `FactionRadioVoHook.cs`
- `FactionRadioVoLibrarySO.cs`
- `GeigerAudioHook.cs`
- `HUD.cs`
- `HatchDefenseHUD.cs`
- `HealthTrajectoryHUD.cs`
- `InternalHorrorHUD.cs`
- `InventoryStripUI.cs`
- `JournalBookUI.cs`
- `MainMenuController.Animation.cs`
- `MainMenuController.Dialogs.cs`
- `MainMenuController.Settings.cs`
- `MainMenuController.cs`
- `MainMenuModel.cs`
- `MapKnowledgeHUD.cs`
- `MapScreenUI.cs`
- `MoralChronicleUI.cs`
- `NeedsBar.cs`
- `PowerGridHUD.cs`
- `RadioInterceptHUD.cs`
- `RadioVoStubFactory.cs`
- `RoomAssignmentHUD.cs`
- `TradeScreenUI.cs`
- `TutorialOverlay.cs`
- `UtilityAIDebugHUD.cs`
- `WorkbenchUI.cs`

#### Module `AtomicWar._Game.Utilities` (11 files)
- `ExpeditionDifficulty.cs`
- `GameAssetKeys.cs`
- `GameAssetService.cs`
- `GameLog.cs`
- `GameObjectPool.cs`
- `GenericObjectPool.cs`
- `IGameAssetProvider.cs`
- `PendingGameLoad.cs`
- `SaveCollectionHelpers.cs`
- `SaveSlotPaths.cs`
- `SeededRandom.cs`

### 3.3 Data Pipeline & Import Tooling
1. **Authored Raw Data:** Located in `Assets/StreamingAssets/Data/*.json` (`items.json`, `recipes.json`, `survivors.json`, `events.json`, `echoes.json`, `radio.json`, `locations.json`).
2. **Editor Importers:** `JsonDataImporter.cs` parses raw JSON into temporary ScriptableObject instances.
3. **Catalog Generation:** `CatalogGenerator.cs` compiles individual SOs into master catalog assets (`ItemCatalogSO`, `RecipeCatalogSO`, `SurvivorCatalogSO`, `LocationCatalogSO`, `GameEventCatalogSO`, `RadioCatalogSO`).
4. **Runtime Access:** Gameplay systems query catalogs by unique `snake_case` IDs.

### 3.4 Persistence & Save System (`SaveSystem.cs`)
- System states are stored in lightweight, serializable C# structs/classes containing only primitive types (ints, floats, bools, strings, arrays).
- Supports multiple save slots with atomic JSON writes to prevent corruption.
- On game load, systems register with `SaveSystem` and re-hydrate their state sequentially without relying on Scene GameObject references.

---

## CHAPTER 4: DETAILED GAMEPLAY MECHANICS (SYSTEM DEEP DIVE)

### 4.1 Survivor & Needs Simulation
- **Core Needs (`Needs.cs`, `NeedsSystem.cs`):**
  - `Hunger` (0-100): Decays per hour. Triggers Weakened -> Malnourished -> Starving -> Death.
  - `Thirst` (0-100): Decays rapidly per hour. Triggers Dehydrated -> Severe Dehydration -> Death.
  - `Fatigue` (0-100): Accumulates during work/expeditions. Restored by sleeping in Beds/Bunks.
  - `Warmth` (0-100): Depends on room temperature and clothing. Hypothermia causes health loss.
  - `Morale` (0-100): Affected by deaths, hunger, room aesthetics, cleanliness. Low morale causes Mental Breaks.
  - `Health` (0-100): Overall physical condition. Depleted by trauma, radiation, sickness, starvation.
- **Psychological & Social Systems:**
  - `BeliefSystem.cs`: Faith, Pragmatism, Collectivism profiles altering survivor reactions to moral dilemmas.
  - `MentalBreakSystem.cs`: Triggers Apathy, Panic Attack, Violent Outburst, or Mutiny when Morale hits zero.
  - `BunkerSocialSystems.cs`: Manages Romance, Feuds, Mentorship, Imprisonment, Banishment, Pregnancy, and Black Market smuggling between shelter occupants.
  - `Perk Systems`: Combat, Expedition, Medical, Shelter, Social, and Survival perks earned through experience.

### 4.2 Radiation & Contamination Systems
- **`RadiationSystem.cs`:** Tracks ambient radiation dose (mSv/h) and total absorbed dose (mSv).
- **Pathology & Dosimetry:**
  - `Dosimeter.cs` & `GeigerCounter.cs`: Provide diegetic clicking sounds and visual rad readings.
  - `Iodine Pills`: Saturated thyroid prophylaxis (blocks rad absorption for 24h).
  - `Anti-Rad / Chelation`: Cleanses internal accumulated radiation dose.
  - `Hazmat Suits`: Degrade in durability while protecting against high fallout zones.
  - `RadiationMutagenesisSystem.cs`: Manages Acute Radiation Sickness (ARS) and long-term mutations/chronic illness.

### 4.3 Medical & Pathology Engine
- **`MedicalSystem.cs`:** Models active afflictions (`ActiveAffliction.cs`): Wound Bleeding, Trauma, Bacterial Infection, Food Poisoning, Frostbite, Acute Sickness.
- **Advanced Medical Features:**
  - `BloodTransfusionSystem.cs`: Full ABO blood group compatibility matrix (A, B, AB, O). Mis-matched transfusions trigger lethal hemolytic reactions.
  - `AddictionSystem.cs`: Tracks dependence on painkillers, stimulants, and alcohol.
  - `AmputationSystem.cs`: Last-resort surgical intervention for infected/gangrenous limbs.
  - `ScurvySystem.cs`: Nutritional deficiency tracking requiring Vitamin C / fresh produce.

### 4.4 Environment, Weather & Map Generation
- **`WeatherSystem.cs`:** Simulates 16 distinct post-nuclear weather conditions:
  - *Acid Snow, Bio Fog, Black Snow, Blood Rain, Dead Wind, Deep Freeze, Dust Devil, EMP Storm, False Spring, Glass Storm, Ozone Hole, Rad Hail, Silent Spring, Solar Flare, Static Charge, Algae Bloom.*
- **`TemperatureSystem.cs`:** Calculates outside temperature based on season and nuclear winter index, driving shelter heating requirements.
- **`FalloutMap.cs` & `GeneratedMap.cs`:** Dynamic 2D map node system with fog of war, river crossings (`RiverNodeSystem.cs`), and radiation knowledge uncertainty.

### 4.5 Shelter Infrastructure & Engineering
- **`Shelter.cs` Aggregate:** Manages modular rooms (Bunkhouse, Medical Bay, Hydroponics, Power Room, Decontamination, Workshop, Radio Room).
- **Infrastructure Subsystems:**
  - `PowerNetwork.cs`: Diesel Generators, Solar Panels, Wind Turbines, Manual Crank Generators. Tracks supply vs demand.
  - `AirFiltrationModuleSO.cs`: Air scrubbing filters that degrade over time. Clogged filters flood the shelter with fallout air.
  - `ShelterAtmosphereSystem.cs`: Tracks oxygen, CO2, toxin levels, and humidity.
  - `StructuralIntegritySystem.cs` & `MaterialShieldingSystem.cs`: Lead, concrete, and steel upgrades for radiation attenuation and raid defense.
  - `HatchDefenseSystem.cs`: Perimeter traps, lock levels, noise control, and hatch defense combat resolution.
  - `VerminSystem.cs` & `WasteSystem.cs`: Trash accumulation leads to rat infestations and disease vectors.

### 4.6 Crafting, Workbench & Economy
- **`CraftingSystem.cs`:** Validates ingredients, required station levels, crafting time, and tool wear.
- **`WorkbenchSystem.cs`:** Workbench upgrades, weapon maintenance, jury-rigging broken gear, and scrapping items into raw metals, electronic parts, and cloth.
- **`DynamicEconomySystem.cs`:** Dynamic trade valuation based on survivor demand, faction trust, and scarcity tiers (Basic, Medical, Tactical, Luxury).

### 4.7 Radio & Intel Interception System
- **`RadioTunerSystem.cs`:** Shortwave tuner searching frequencies:
  - *Civilian (88.5 FM), Military (102.1 FM), Emergency Automated Loops, Numbers Station (99.0 FM), Survivor Frequencies.*
- **Intel Types & Reliability:** Intercepts Plume Reports, Weather Forecasts, Mortar Warnings, Troop Movements, and Cache Coordinates. Intel degrades in confidence (1.0 -> 0.0) over 5 days.

### 4.8 Utility AI Engine (`AtomicWar._Game.AI`)
- Autonomous survivor behavior engine operating without runtime LLMs.
- **Scoring Formula:** `Score = BaseUtility * NeedMultiplier * TraitWeight * SafetyFactor`.
- Evaluates candidate actions (Sleep, Eat Rations, Purify Water, Repair Air Filter, Scavenge, Treat Patient, Rest by Heater) every tick and assigns the highest-scoring action to autonomous survivors.

### 4.9 Narrative Event Engine & Victory Paths
- **`EventRunner.cs`:** Weighted event picker evaluating daily preconditions, survivor traits, and shelter state.
- **`GameEvent.cs`:** Branching choices with immediate effects, skill checks, and delayed consequences (`ActiveDelayedConsequence`).
- **8 Victory Paths:**
  1. *MAD (Mutually Assured Destruction)*: Triggering automated retaliation networks.
  2. *Migration*: Escaping the fallout zone via long-range convoy.
  3. *The Broadcast*: Establishing a regional emergency radio network.
  4. *The Cure*: Synthesizing an anti-radiation genetic compound.
  5. *The Martian*: Achieving complete closed-loop shelter self-sufficiency.
  6. *True Ending*: Negotiating peace between remaining faction remnants.
  7. *Underground City*: Expanding the sub-pen into a permanent subterranean settlement.
  8. *Unifier*: Subjugating rival wasteland factions under one banner.

---

## CHAPTER 5: EXHAUSTIVE AUTHORED DATA CATALOGS

### 5.1 Item Catalog (321 Items)
Below is the complete dataset of all 321 authored items in `items.json`:

| ID | Name | Type | Stack Max | Weight (kg) | Rad Prot | Durability | Trade Value | Description |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| `dosimeter` | Dosimeter | Device | 1 | 0.5 | 0 | 0 | 30 | Personal dosimeter. Logs cumulative radiation dose for the UI. |
| `geiger_counter` | Geiger Counter | Device | 1 | 1.0 | 0 | 0 | 42.0 | Live rate meter. |
| `iodine_pills` | Iodine Pills | Iodine | 5 | 0.1 | 0 | 0 | 6 | Potassium iodide. Grants temporary radiation resistance while active. |
| `anti_rad` | Anti-Rad | AntiRad | 5 | 0.1 | 0 | 0 | 8 | Decorporation medication. Cleanses a chunk of current radiation dose. |
| `gas_mask` | Gas Mask | Protective | 1 | 1.5 | 30 | 100 | 40 | Filters airborne fallout from the lungs. Moderate protection, degrades with use. |
| `hazmat_suit` | Hazmat Suit | Protective | 1 | 5.0 | 80 | 100 | 40 | Full-body protective suit. High radiation protection, degrades with use. |
| `water_filter` | Water Filter | Filter | 5 | 0.5 | 0 | 0 | 20 | Replacement cartridge for purifying contaminated water. |
| `air_filter` | Air Filter | Filter | 5 | 1.0 | 0 | 0 | 20 | Replacement filter for the shelter air-filtration unit. |
| `clean_water` | Clean Water | Water | 10 | 0.5 | 0 | 0 | 15 | Safe drinking water. Restores thirst. |
| `irradiated_water` | Irradiated Water | IrradiatedWater | 10 | 0.5 | 0 | 0 | 2 | Quenches thirst, but contaminated. Drinking it adds a dose of radiation. |
| `canned_food` | Canned Food | Food | 10 | 0.5 | 0 | 0 | 12 | Preserved rations. Restores hunger and a little morale. |
| `fuel` | Fuel | Fuel | 20 | 2.0 | 0 | 0 | 14 | Combustible fuel for the heater and generator. |
| `cloth` | Cloth | Material | 20 | 0.2 | 0 | 0 | 1.2 | Salvaged fabric. Used to craft bandages and filters. |
| `scrap_metal` | Scrap Metal | Material | 20 | 0.5 | 0 | 0 | 1.2 | Salvaged metal parts. Used to craft filters and gear. |
| `bandage` | Bandage | Medical | 10 | 0.1 | 0 | 0 | 10 | Sterile dressing. Restores health when used. |
| `raw_meat` | Raw Meat | Food | 5 | 0.5 | 0 | 0 | 12 | Uncooked meat from mutated fauna. Slightly contaminated; cook before eating. |
| `cooked_meat` | Cooked Meat | Food | 5 | 0.5 | 0 | 0 | 12 | Cooked meat. Hearty meal that restores hunger and a little morale. |
| `dirty_water` | Dirty Water | IrradiatedWater | 10 | 0.5 | 0 | 0 | 2 | Contaminated water. Boil or filter it before drinking. |
| `medical_kit` | Medical Kit | Medical | 5 | 0.5 | 0 | 0 | 10 | Assembled medical supplies. Restores a large amount of health when used. |
| `battery` | Battery | Material | 10 | 0.2 | 0 | 0 | 5 | Cell pack for geiger counters and dosimeters. Without power, instruments go dark. |
| `calibration_kit` | Calibration Kit | Tool | 5 | 0.4 | 0 | 0 | 18 | Reference sources and tools to recalibrate a geiger or dosimeter. A mis-calibrated counter lies. |
| `tweezers` | Tweezers | Tool | 5 | 0.1 | 0 | 0 | 18 | Fine forceps for extracting shrapnel and dressing deep wounds. Needed for full gunshot care. |
| `splint` | Splint | Medical | 5 | 0.4 | 0 | 0 | 9.0 | Immobilises a limb. |
| `antibiotics` | Antibiotics | Medical | 10 | 0.05 | 0 | 0 | 10 | Broad-spectrum pills. Treat bacterial infections and dysentery before they become sepsis. |
| `jewelry` | Jewelry | Trade | 20 | 0.05 | 0 | 0 | 50 | Gold and stones from before. Pretty until the sky burns; then it buys nothing. |
| `currency` | Paper Currency | Trade | 100 | 0.01 | 0 | 0 | 20 | Pre-exchange notes. Still foldable. Not drinkable. |
| `mechanical_parts` | Mechanical Parts | Material | 50 | 0.15 | 0 | 0 | 3.0 | Gears, bolts, springs. |
| `electronic_scrap` | Electronic Scrap | Material | 50 | 0.1 | 0 | 0 | 6 | Boards, wire, and cracked vacuum tubes. Needed to repair instruments and the purifier. |
| `solar_cell` | Solar Cell | Material | 10 | 1.2 | 0 | 0 | 22.0 | A cracked photovoltaic panel pried off a weather station. It still charges, slowly, on the four hours a day the ash thins enough to let light through. |
| `chemicals` | Chemicals | Material | 30 | 0.25 | 0 | 0 | 5 | Solvents and reagents scavenged from labs and garages. |
| `handheld_radio` | Handheld Radio | Device | 1 | 0.8 | 0 | 0 | 22 | Portable receiver. Often dead. Still full of boards worth stripping. |
| `engine` | Engine | Tool | 1 | 25.0 | 0 | 100 | 80 | A scavenged vehicle engine. Repair it, feed it fuel and parts, and drive out of the ash. |
| `roots` | Roots | Food | 20 | 0.2 | 0.0 | 0 | 1 | Low-nutrition forage. Staves off starvation. |
| `berries` | Berries | Food | 20 | 0.15 | 0.0 | 0 | 1 | Low-nutrition forage. Staves off starvation. |
| `att_mil_suppressor` | Military Grade Suppressor | Tool | 1 | 0.4 | 0 | 100 | 55 | Threaded suppressor. Extremely rare loose loot. Usually already mounted on mil/rebel/specialist weapons. |
| `att_mil_laserdot` | Military Grade Laserdot Scope | Tool | 1 | 0.4 | 0 | 100 | 55 | IR-capable laser aiming module. Usually already mounted on mil/rebel/specialist weapons. |
| `att_mil_tactical_grip` | Military Grade Tactical Grip | Tool | 1 | 0.4 | 0 | 100 | 55 | Angled/vertical grip for recoil control. Usually already mounted on mil/rebel/specialist weapons. |
| `att_mil_long_range_scope` | Military Grade Long Range Scope | Tool | 1 | 0.4 | 0 | 100 | 55 | High-magnification optic. Usually already mounted on mil/rebel/specialist weapons. |
| `att_mil_holosight` | Military Grade Holosight | Tool | 1 | 0.4 | 0 | 100 | 55 | Holographic CQB optic. Usually already mounted on mil/rebel/specialist weapons. |
| `att_mil_double_scope_5x_10x` | Military Grade Double Scope 5×/10× | Tool | 1 | 0.4 | 0 | 100 | 60 | Flip dual-magnification scope. Usually already mounted on mil/rebel/specialist weapons. |
| `shell_casing` | Shell Casing | Material | 50 | 0.01 | 0 | 0 | 0.4 | Empty hull. Scrap or reload feedstock. |
| `bullet_casing` | Bullet Casing | Material | 80 | 0.005 | 0 | 0 | 0.4 | Empty brass. |
| `gunpowder` | Gunpowder | Material | 40 | 0.02 | 0 | 0 | 3.5 | Propellant grains. |
| `weapon_hmg` | Heavy Machine Gun | Weapon | 1 | 45.0 | 0 | 50 | 280.0 | Tripod-mounted .50 cal HMG. Provides massive hatch defense bonus when stock is on hand and the gun is mounted + oiled. Requires 2 operators to fire at full efficiency; jams without oil. Two crew + a stock pile of this weapon is the difference between a held hatch and a breached one. |
| `sulphur` | Sulphur | Material | 30 | 0.05 | 0 | 0 | 2 | Yellow powder for chemistry mixes. |
| `explosive_powder_nitroglycerin` | Explosive Powder (Nitroglycerin) | Material | 10 | 0.08 | 0 | 0 | 18 | Unstable high explosive. |
| `fertilizer` | Fertilizer | Material | 20 | 1.0 | 0 | 0 | 2 | Grow beds and compost product. |
| `salvaged_tech_trash` | Salvaged Tech-Trash | Material | 25 | 0.3 | 0 | 0 | 2 | Broken boards; scrape for electronics. |
| `ammo_deprecated_cal_9x19` | Deprecated Bullets (9x19) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_9x19 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_380acp` | Deprecated Bullets (380acp) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_380acp rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_762x25` | Deprecated Bullets (762x25) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_762x25 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_45acp` | Deprecated Bullets (45acp) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_45acp rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_9x21` | Deprecated Bullets (9x21) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_9x21 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_765x21` | Deprecated Bullets (765x21) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_765x21 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_12ga` | Deprecated Bullets (12ga) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_12ga rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_16ga` | Deprecated Bullets (16ga) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_16ga rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_556x45` | Deprecated Bullets (556x45) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_556x45 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_762x39` | Deprecated Bullets (762x39) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_762x39 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_545x39` | Deprecated Bullets (545x39) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_545x39 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_762x51` | Deprecated Bullets (762x51) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_762x51 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_300blk` | Deprecated Bullets (300blk) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_300blk rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_57x28` | Deprecated Bullets (57x28) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_57x28 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_46x30` | Deprecated Bullets (46x30) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_46x30 rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_762x54r` | Deprecated Bullets (762x54r) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_762x54r rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_338lapua` | Deprecated Bullets (338lapua) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_338lapua rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_408cheytac` | Deprecated Bullets (408cheytac) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_408cheytac rounds. Scrap only — not safe to fire. |
| `ammo_deprecated_cal_50bmg` | Deprecated Bullets (50bmg) | Material | 60 | 0.015 | 0 | 0 | 0.5 | Corroded cal_50bmg rounds. Scrap only — not safe to fire. |
| `body_armour_deprecated` | Deprecated Body Armour | Protective | 1 | 2.5 | 0 | 20 | 4 | Cracked plates. |
| `body_armour_military` | Military Grade Body Armour | Protective | 1 | 4 | 2 | 120 | 48 | Plate carrier with inserts. |
| `helmet_military` | Military Grade Helmet | Protective | 1 | 4 | 1 | 80 | 22 | Ballistic helmet. |
| `nv_goggles_military` | Military Grade NV Goggles | Protective | 1 | 4 | 0 | 60 | 55 | Night-vision goggles. |
| `helmet_heavy_military` | Heavy Military Grade Helmet | Protective | 1 | 4 | 2 | 140 | 35 | Heavy ballistic helmet. |
| `armour_heavy_military` | Heavy Military Grade Armour | Protective | 1 | 4 | 4 | 180 | 70 | Full heavy plate. |
| `helmet_heavy_deprecated` | Deprecated Heavy Helmet | Protective | 1 | 2.5 | 0 | 15 | 3 | Rusted heavy helmet. |
| `helmet_deprecated` | Deprecated Helmet | Protective | 1 | 2.5 | 0 | 10 | 2 | Split shell. |
| `body_armour_heavy_deprecated` | Deprecated Heavy Body Armour | Protective | 1 | 2.5 | 0 | 25 | 5 | Warped heavy plates. |
| `vegetable_carrot` | Vegetable — Carrot | Food | 20 | 0.15 | 0 | 0 | 1 | Vegetable — Carrot |
| `vegetable_potato` | Vegetable — Potato | Food | 20 | 0.25 | 0 | 0 | 1 | Vegetable — Potato |
| `vegetable_beetroot` | Vegetable — Beetroot | Food | 20 | 0.2 | 0 | 0 | 1 | Vegetable — Beetroot |
| `canned_meat` | Canned Meat | Food | 10 | 0.45 | 0 | 0 | 3 | Canned Meat |
| `preserved_crackers` | Preserved Crackers | Food | 25 | 0.2 | 0 | 0 | 1.5 | Preserved Crackers |
| `mre_military` | Military Grade MRE | Food | 8 | 0.6 | 0 | 0 | 12 | Military Grade MRE |
| `boiled_vegetable_soup` | Boiled Vegetable Soup | Food | 6 | 0.5 | 0 | 0 | 3 | Boiled Vegetable Soup |
| `hearty_meal_cooked` | Hearty Meal (Cooked) | Food | 4 | 0.7 | 0 | 0 | 6 | Hearty Meal (Cooked) |
| `fuel_1l` | Fuel 1L | Fuel | 10 | 1.0 | 0 | 0 | 4 | Fuel 1L |
| `fuel_0_5l_of_1l` | Fuel 0.5L / 1L | Fuel | 10 | 0.55 | 0 | 0 | 2 | Fuel 0.5L / 1L |
| `accelerant_full` | Accelerant (Full) | Fuel | 10 | 1.2 | 0 | 0 | 6 | Accelerant (Full) |
| `accelerant_half` | Accelerant (Half-Full) | Fuel | 10 | 0.7 | 0 | 0 | 3 | Accelerant (Half-Full) |
| `knife_improvised` | Improvised Knife | Tool | 1 | 0.4 | 0 | 40 | 6 | Improvised Knife |
| `knife_swiss_battle` | Swiss Battle Knife | Tool | 1 | 0.35 | 0 | 100 | 12 | Swiss Battle Knife |
| `bayonet_swiss_machete` | Swiss Bayonet Machete | Tool | 1 | 1.1 | 0 | 120 | 18 | Swiss Bayonet Machete |
| `hammer` | Hammer | Tool | 1 | 0.8 | 0 | 80 | 4 | Hammer |
| `screwdriver` | Screwdriver | Tool | 1 | 0.2 | 0 | 60 | 3 | Screwdriver |
| `multitool` | MultiTool | Tool | 1 | 0.3 | 0 | 90 | 10 | MultiTool |
| `shovel` | Shovel | Tool | 1 | 2.0 | 0 | 100 | 6 | Shovel |
| `grenade_military` | Military Grade Hand Grenade | Weapon | 4 | 0.4 | 0 | 1 | 95 | Military Grade Hand Grenade |
| `crowbar` | Crowbar | Tool | 1 | 2.5 | 0 | 120 | 14 | Crowbar |
| `wire_cutters` | Wire Cutters | Tool | 1 | 0.4 | 0 | 70 | 5 | Wire Cutters |
| `lockpick` | Lockpick | Tool | 1 | 0.05 | 0 | 30 | 15 | Lockpick |
| `metal_pipe` | Metal Pipe | Tool | 1 | 1.5 | 0 | 50 | 2 | Metal Pipe |
| `crowbar_broken` | Crowbar Broken | Material | 5 | 0.5 | 0 | 0 | 0.5 | Broken tool. Scrap only. |
| `wire_cutters_broken` | Wire Cutters Broken | Material | 5 | 0.5 | 0 | 0 | 0.5 | Broken tool. Scrap only. |
| `metal_pipe_broken` | Metal Pipe Broken | Material | 5 | 0.5 | 0 | 0 | 0.5 | Broken tool. Scrap only. |
| `shovel_broken` | Shovel Broken | Material | 5 | 0.5 | 0 | 0 | 0.5 | Broken tool. Scrap only. |
| `multitool_broken` | Multitool Broken | Material | 5 | 0.5 | 0 | 0 | 0.5 | Broken tool. Scrap only. |
| `knife_broken` | Knife Broken | Material | 5 | 0.5 | 0 | 0 | 0.5 | Broken tool. Scrap only. |
| `hammer_broken` | Hammer Broken | Material | 5 | 0.5 | 0 | 0 | 0.5 | Broken tool. Scrap only. |
| `screwdriver_broken` | Screwdriver Broken | Material | 5 | 0.5 | 0 | 0 | 0.5 | Broken tool. Scrap only. |
| `water_bottle_1l_full` | Water Bottle 1L (Full) | Water | 4 | 1.1 | 0 | 0 | 5 | Water Bottle 1L (Full) |
| `water_bottle_2l_full` | Water Bottle 2L (Full) | Water | 4 | 2.2 | 0 | 0 | 8 | Water Bottle 2L (Full) |
| `water_bottle_1l_of_2l` | Water Bottle 1L / 2L | Water | 4 | 1.6 | 0 | 0 | 3 | Water Bottle 1L / 2L |
| `water_bottle_0_5l_of_1l` | Water Bottle 0.5L / 1L | Water | 4 | 0.7 | 0 | 0 | 2 | Water Bottle 0.5L / 1L |
| `water_bottle_0_5l_of_2l` | Water Bottle 0.5L / 2L | Water | 4 | 1.1 | 0 | 0 | 2 | Water Bottle 0.5L / 2L |
| `water_bottle_1_5l_of_2l` | Water Bottle 1.5L / 2L | Water | 4 | 1.9 | 0 | 0 | 4 | Water Bottle 1.5L / 2L |
| `water_bottle_empty` | Empty Water Bottle | Material | 10 | 0.1 | 0 | 0 | 1 | Empty bottle. Refill or boil. |
| `ammo_545x39_jhp_ap` | 5.45×39mm JHP+AP | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_545x39_exi` | 5.45×39mm EXI | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_545x39_api` | 5.45×39mm API | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_300blk_jhp_ap` | .300 Blackout JHP+AP | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_300blk_exi` | .300 Blackout EXI | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_300blk_api` | .300 Blackout API | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_57x28_jhp_ap` | 5.7×28mm JHP+AP | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_57x28_exi` | 5.7×28mm EXI | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_57x28_api` | 5.7×28mm API | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_762x54r_jhp_ap` | 7.62×54R JHP+AP | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_762x54r_exi` | 7.62×54R EXI | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_762x54r_api` | 7.62×54R API | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_338lapua_jhp_ap` | .338 Lapua Magnum JHP+AP | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_338lapua_exi` | .338 Lapua Magnum EXI | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_338lapua_api` | .338 Lapua Magnum API | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_408cheytac_jhp_ap` | .408 CheyTac JHP+AP | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_408cheytac_exi` | .408 CheyTac EXI | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_408cheytac_api` | .408 CheyTac API | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_762x51_jhp_ap` | 7.62×51mm NATO JHP+AP | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_762x51_exi` | 7.62×51mm NATO EXI | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_50bmg_jhp_ap` | 12.7×99mm NATO JHP+AP | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `ammo_50bmg_exi` | 12.7×99mm NATO EXI | Weapon | 20 | 0.02 | 0 | 0 | 15 | Military dual-attribute load. Field-only. |
| `wood_block` | Wood Block | Material | 40 | 1.2 | 0.0 | 0.0 | 1.0 | Sawn timber block. Framing and fuel. |
| `sawdust_block` | Sawdust Block | Material | 30 | 0.4 | 0.0 | 0.0 | 0.35 | Compressed sawdust. Kindling and filler. |
| `book` | Book | Comfort | 10 | 0.4 | 0.0 | 0.0 | 3.0 | Bound pages. Morale, kindling, or knowledge. |
| `charcoal` | Charcoal | Material | 40 | 0.2 | 0.0 | 0.0 | 1.1 | Burned wood. Filters and slow heat. |
| `coal` | Coal | Fuel | 30 | 0.5 | 0.0 | 0.0 | 2.0 | Hard coal. Long burn for heaters. |
| `sugar` | Sugar | Material | 25 | 0.3 | 0.0 | 0.0 | 2.2 | White crystals. Cooking and bad morale swaps. |
| `scrap_wood` | Scrap Wood | Material | 40 | 0.8 | 0.0 | 0.0 | 0.6 | Broken boards. Crude repairs. |
| `plywood_sheet` | Plywood Sheet | Material | 15 | 2.5 | 0.0 | 0.0 | 2.5 | Thin sheet. Walls and shutters. |
| `bricks` | Bricks | Material | 20 | 2.0 | 0.0 | 0.0 | 1.8 | Fired clay. Shelter upgrades. |
| `cement_mix` | Cement Mix | Material | 10 | 5.0 | 0.0 | 0.0 | 4.0 | Dry cement. Needs 1L water for hatch/shelter work. |
| `plastic_material` | Plastic Material | Material | 40 | 0.15 | 0.0 | 0.0 | 0.9 | Salvaged plastic stock. |
| `tactical_scrap` | Tactical Scrap | Material | 20 | 0.4 | 0.0 | 0.0 | 6.0 | Mil-spec offcuts. Rare salvage. |
| `tungsten_bar` | Tungsten Bar | Material | 5 | 1.5 | 0.0 | 0.0 | 28.0 | Dense hard metal bar. |
| `titanium_bar` | Titanium Bar | Material | 5 | 0.9 | 0.0 | 0.0 | 32.0 | Light strong metal bar. |
| `nails` | Nails | Material | 80 | 0.02 | 0.0 | 0.0 | 0.3 | Loose nails. Framing. |
| `box_of_nails_10` | Box of Nails (10x) | Material | 20 | 0.25 | 0.0 | 0.0 | 2.5 | Ten nails boxed. |
| `box_of_nails_5` | Box of Nails (5x) | Material | 25 | 0.15 | 0.0 | 0.0 | 1.4 | Five nails boxed. |
| `duct_tape` | Duct Tape | Material | 15 | 0.15 | 0.0 | 0.0 | 3.5 | Grey tape. Fixes everything poorly. |
| `rope_2m_of_2m` | Rope 2M / 2M | Material | 5 | 0.6 | 0.0 | 0.0 | 5.0 | Two metres of rope. 0.4M per use. |
| `copper_wire_10m_of_10m` | Copper Wire 10M / 10M | Material | 5 | 0.5 | 0.0 | 0.0 | 6.0 | Ten metres copper. 1M per use. |
| `electrical_cable` | Electrical Cable | Material | 20 | 0.3 | 0.0 | 0.0 | 2.8 | Insulated cable length. |
| `rubber_hose` | Rubber Hose | Material | 15 | 0.5 | 0.0 | 0.0 | 2.2 | Flexible hose. Plumbing and siphons. |
| `fuse` | Fuse | Material | 30 | 0.02 | 0.0 | 0.0 | 1.0 | Single electrical fuse. |
| `fuse_assortment` | Fuse Assortment | Material | 10 | 0.15 | 0.0 | 0.0 | 4.5 | Mixed fuse set. |
| `circuit_board` | Circuit Board | Material | 15 | 0.1 | 0.0 | 0.0 | 8.0 | Salvaged board. Electronics craft. |
| `vacuum_tube` | Vacuum Tube | Material | 10 | 0.08 | 0.0 | 0.0 | 5.5 | Glass tube. Old radios and quirks. |
| `generator_parts` | Generator Parts | Material | 8 | 1.2 | 0.0 | 0.0 | 14.0 | Spare gen components. |
| `generator_alternator` | Generator Alternator | Material | 3 | 4.0 | 0.0 | 0.0 | 22.0 | Heavy alternator core. |
| `basic_tool_handle` | Basic Tool Handle | Material | 20 | 0.2 | 0.0 | 0.0 | 1.0 | Wooden handle blank. |
| `advanced_tool_handle` | Advanced Tool Handle | Material | 12 | 0.25 | 0.0 | 0.0 | 3.5 | Reinforced handle blank. |
| `multitool_base` | Multitool Base | Material | 10 | 0.2 | 0.0 | 0.0 | 4.0 | Frame for multitool craft. |
| `dry_yeast_powder` | Dry Yeast Powder | Material | 20 | 0.05 | 0.0 | 0.0 | 2.0 | Baking and brewing yeast. |
| `wheat_flour` | Wheat Flour | Material | 15 | 1.0 | 0.0 | 0.0 | 2.8 | Milled wheat. Bread and batter. |
| `oat_flour` | Oat Flour (High Quality) | Material | 12 | 1.0 | 0.0 | 0.0 | 3.5 | Fine oat flour. |
| `cooking_oil` | Cooking Oil | Material | 12 | 0.9 | 0.0 | 0.0 | 3.2 | Cooking fat. Stoves and recipes. |
| `salt` | Salt | Material | 25 | 0.3 | 0.0 | 0.0 | 1.8 | Preserving and seasoning. |
| `military_grade_sandstone` | Military Grade Sandstone | Material | 10 | 3.0 | 0.0 | 0.0 | 8.0 | Hard cut stone. Fortification. |
| `water_purification_tablet` | Water Purification Tablet | Medical | 40 | 0.01 | 0.0 | 0.0 | 1.5 | Single tablet. Treats a bottle. |
| `water_purification_tablets_40_of_40` | Water Purification Tablets 40/40 | Medical | 1 | 0.12 | 0.0 | 0.0 | 28.0 | Full bottle. 2 tablets per use. |
| `water_purification_tablets_20_of_40` | Water Purification Tablets 20/40 | Medical | 1 | 0.08 | 0.0 | 0.0 | 15.0 | Half-full bottle. 2 tablets per use. |
| `water_purification_tablets_0_of_40` | Water Purification Tablets 0/40 (Empty) | Material | 5 | 0.04 | 0.0 | 0.0 | 1.0 | Empty bottle. Refill elsewhere. |
| `flare_red` | Flare (Red) | Tool | 10 | 0.2 | 0.0 | 0.0 | 6.0 | Red signal flare. Night mark. |
| `flare_green` | Flare (Green) | Tool | 10 | 0.2 | 0.0 | 0.0 | 6.0 | Green signal flare. |
| `flare_yellow` | Flare (Yellow) | Tool | 10 | 0.2 | 0.0 | 0.0 | 6.0 | Yellow signal flare. |
| `smoke_grenade` | Smoke Grenade | Weapon | 6 | 0.35 | 0.0 | 0.0 | 55.0 | Screening smoke. Cover and signal. |
| `flashbang` | Flashbang | Weapon | 6 | 0.3 | 0.0 | 0.0 | 70.0 | Stun charge. Self-defense. |
| `workbench_basic` | Workbench (Basic) | Device | 1 | 10.0 | 0.0 | 100.0 | 28.0 | Entry craft station. |
| `workbench_intermediate` | Workbench (Intermediate) | Device | 1 | 12.0 | 0.0 | 100.0 | 45.0 | Expanded craft recipes. |
| `workbench_advanced` | Workbench (Advanced) | Device | 1 | 14.0 | 0.0 | 100.0 | 70.0 | Complex assemblies. |
| `workbench_professional` | Workbench (Professional) | Device | 1 | 16.0 | 0.0 | 100.0 | 110.0 | Top-tier civilian craft. |
| `workbench_upgrade_kit` | Workbench Upgrade Kit | Device | 1 | 12.0 | 0.0 | 100.0 | 40.0 | Upgrades a workbench one tier. |
| `research_table` | Research Table | Device | 1 | 12.0 | 0.0 | 100.0 | 48.0 | Blueprints and analysis. |
| `basic_cooking_stove` | Basic Cooking Stove | Device | 1 | 10.0 | 0.0 | 100.0 | 30.0 | Cooks rations. Needs fuel/matches. |
| `improvised_cooking_stove` | Improvised Cooking Stove | Device | 1 | 8.0 | 0.0 | 100.0 | 18.0 | Scrap stove. Smoky. |
| `advanced_cooking_stove` | Advanced Cooking Stove | Device | 1 | 14.0 | 0.0 | 100.0 | 65.0 | Efficient field kitchen. |
| `basic_heater` | Basic Heater | Device | 1 | 10.0 | 0.0 | 100.0 | 32.0 | Bunker warmth. Fuel hungry. |
| `improvised_heater` | Improvised Heater | Device | 1 | 8.0 | 0.0 | 100.0 | 16.0 | Scrap heater. Risk of smoke. |
| `advanced_heater` | Advanced Heater | Device | 1 | 14.0 | 0.0 | 100.0 | 68.0 | Controlled bunker heat. |
| `heater_lamp` | Heater Lamp | Device | 1 | 10.0 | 0.0 | 100.0 | 22.0 | Small radiant lamp. |
| `distiller` | Distiller | Device | 1 | 12.0 | 0.0 | 100.0 | 50.0 | Water/alcohol distillation. |
| `alcohol_distiller` | Alcohol Distiller | Device | 1 | 12.0 | 0.0 | 100.0 | 52.0 | Spirits still. Fuel and yeast. |
| `filter_item` | Filter Item | Device | 1 | 10.0 | 0.0 | 100.0 | 12.0 | Generic filter cartridge frame. |
| `basic_water_boiler` | Basic Water Boiler | Device | 1 | 10.0 | 0.0 | 100.0 | 26.0 | Boils water clean. |
| `improvised_water_boiler` | Improvised Water Boiler | Device | 1 | 8.0 | 0.0 | 100.0 | 14.0 | Tin boiler. Slow. |
| `advanced_water_boiler` | Advanced Water Boiler | Device | 1 | 14.0 | 0.0 | 100.0 | 55.0 | Fast safe boil. |
| `basic_herb_garden` | Basic Herb Garden | Device | 1 | 10.0 | 0.0 | 100.0 | 24.0 | Indoor herbs. |
| `improvised_herb_garden` | Improvised Herb Garden | Device | 1 | 8.0 | 0.0 | 100.0 | 12.0 | Tins and dirt. |
| `advanced_herb_garden` | Advanced Herb Garden | Device | 1 | 14.0 | 0.0 | 100.0 | 50.0 | Lush indoor herbs. |
| `herbal_farm_max_tier` | Herbal Farm (Max Tier) | Device | 1 | 16.0 | 0.0 | 100.0 | 95.0 | Full bunker herb farm. |
| `small_animal_trap` | Small Animal Trap | Device | 1 | 10.0 | 0.0 | 100.0 | 15.0 | Catches small game. |
| `medium_animal_trap` | Medium Animal Trap | Device | 1 | 12.0 | 0.0 | 100.0 | 28.0 | Larger game trap. |
| `basic_recycler` | Basic Recycler | Device | 1 | 10.0 | 0.0 | 100.0 | 30.0 | Breaks scrap to parts. |
| `improvised_recycler` | Improvised Recycler | Device | 1 | 8.0 | 0.0 | 100.0 | 16.0 | Crude scrap mill. |
| `advanced_recycle_bench` | Advanced Recycle Bench | Device | 1 | 14.0 | 0.0 | 100.0 | 72.0 | High-yield recycling. |
| `simple_tool_workshop` | Simple Tool Workshop | Device | 1 | 10.0 | 0.0 | 100.0 | 26.0 | Basic tool craft. |
| `basic_tool_workshop` | Basic Tool Workshop | Device | 1 | 10.0 | 0.0 | 100.0 | 32.0 | Standard tool craft. |
| `improvised_tool_workshop` | Improvised Tool Workshop | Device | 1 | 8.0 | 0.0 | 100.0 | 15.0 | Scrap tool bench. |
| `advanced_tool_workshop` | Advanced Tool Workshop | Device | 1 | 14.0 | 0.0 | 100.0 | 68.0 | Precision tools. |
| `basic_gunbench` | Basic Gunbench | Device | 1 | 12.0 | 0.0 | 100.0 | 55.0 | Simple firearm maintenance. |
| `improvised_gunbench` | Improvised Gunbench | Device | 1 | 10.0 | 0.0 | 100.0 | 35.0 | Crude weapon bench. |
| `tactical_weapons_bench` | Tactical Weapons Bench | Device | 1 | 14.0 | 0.0 | 100.0 | 90.0 | Military weapon work. |
| `advanced_tactical_weapon_bench` | Advanced Tactical Weapon Bench | Device | 1 | 16.0 | 0.0 | 100.0 | 125.0 | Spec-ops grade weapon bench. |
| `basic_refinement_bench` | Basic Refinement Bench | Device | 1 | 10.0 | 0.0 | 100.0 | 34.0 | Ore and part refine. |
| `improvised_refinement_bench` | Improvised Refinement Bench | Device | 1 | 8.0 | 0.0 | 100.0 | 17.0 | Scrap refine. |
| `tactical_refinement_bench` | Tactical Refinement Bench | Device | 1 | 14.0 | 0.0 | 100.0 | 80.0 | Mil-spec refine. |
| `advanced_tactical_refinement_workshop` | Advanced Tactical Refinement Workshop | Device | 1 | 16.0 | 0.0 | 100.0 | 120.0 | Top refine workshop. |
| `basic_tobacco_leaf` | Basic Tobacco Leaf | Material | 30 | 0.05 | 0.0 | 0.0 | 1.5 | Dried leaf. Harsh smoke. |
| `quality_tobacco_leaf` | Quality Tobacco Leaf | Material | 20 | 0.05 | 0.0 | 0.0 | 3.0 | Cured leaf. Smoother. |
| `basic_rollup_cigarette` | Basic Rollup Cigarette | Comfort | 40 | 0.01 | 0.0 | 0.0 | 1.2 | Hand-rolled smoke. |
| `quality_rollup_cigarette` | Quality Rollup Cigarette | Comfort | 30 | 0.01 | 0.0 | 0.0 | 2.2 | Better roll. |
| `herbal_cigarette` | Herbal Cigarette | Comfort | 30 | 0.01 | 0.0 | 0.0 | 1.5 | Garden herbs rolled. |
| `herbs` | Herbs | Material | 40 | 0.05 | 0.0 | 0.0 | 1.8 | Mixed garden herbs. |
| `menthol_leaf` | Menthol Leaf | Material | 25 | 0.04 | 0.0 | 0.0 | 2.0 | Cool leaf. |
| `menthol_cigarette` | Menthol Cigarette | Comfort | 30 | 0.01 | 0.0 | 0.0 | 2.0 | Menthol smoke. |
| `disposable_vape` | Disposable Vape | Comfort | 10 | 0.08 | 0.0 | 0.0 | 5.0 | Sealed vape stick. |
| `ejuice_10ml_10mg` | E-Juice 10ML 10mg Nicotine | Material | 15 | 0.05 | 0.0 | 0.0 | 4.0 | Low-nic e-liquid. |
| `ejuice_10ml_20mg` | E-Juice 10ML 20mg Nicotine | Material | 12 | 0.05 | 0.0 | 0.0 | 5.0 | Mid-nic e-liquid. |
| `ejuice_20ml_35mg` | E-Juice 20ML 35mg Nicotine | Material | 8 | 0.1 | 0.0 | 0.0 | 8.0 | High-nic e-liquid. |
| `nicotine_pouch` | Nicotine Pouch | Comfort | 40 | 0.01 | 0.0 | 0.0 | 1.8 | Oral pouch. |
| `quality_tobacco_nicotine_pouch` | Quality Tobacco Nicotine Pouch | Comfort | 30 | 0.01 | 0.0 | 0.0 | 3.0 | Premium pouch. |
| `coffee_arabica_bean` | Coffee Arabica Bean | Material | 25 | 0.05 | 0.0 | 0.0 | 2.5 | Better bean. |
| `coffee_robusta_bean` | Coffee Robusta Bean | Material | 25 | 0.05 | 0.0 | 0.0 | 1.8 | Harsh strong bean. |
| `instant_coffee` | Instant Coffee | Food | 30 | 0.02 | 0.0 | 0.0 | 2.0 | One serve powder. |
| `instant_coffee_10x_container` | Instant Coffee 10x Container | Food | 5 | 0.2 | 0.0 | 0.0 | 14.0 | Ten serves. |
| `coffee_creamer` | Coffee Creamer | Material | 20 | 0.1 | 0.0 | 0.0 | 1.5 | Powdered creamer. |
| `box_of_tea_20` | Box of Tea (20x) | Food | 5 | 0.15 | 0.0 | 0.0 | 8.0 | Twenty bags. |
| `ice_tea_0_5l_package` | Ice Tea 0.5L Package | Food | 10 | 0.55 | 0.0 | 0.0 | 3.5 | Sweet cold tea. |
| `herbal_tea` | Herbal Tea | Food | 15 | 0.2 | 0.0 | 0.0 | 3.0 | Brewed from garden herbs. |
| `package_rolled_oats_1kg_of_1kg` | Package of Rolled Oats 1KG/1KG | Food | 4 | 1.1 | 0.0 | 0.0 | 7.0 | Drains 0.1KG/use → 2 basic breakfast bowls (needs 1× water). |
| `dry_rice_1kg_of_1kg` | Dry Rice 1KG/1KG | Food | 4 | 1.1 | 0.0 | 0.0 | 6.5 | Drains 0.1KG/use; needs 1× water. |
| `dried_pasta_2kg_of_2kg` | Dried Pasta 2KG/2KG | Food | 3 | 2.1 | 0.0 | 0.0 | 9.0 | Drains 0.2KG/use. Alone: 1 quality dinner. +carrot+potato+2 water: 4 quality dinners. |
| `soy_and_rice_milk_1l_of_1l` | Soy and Rice Milk 1L/1L | Food | 6 | 1.05 | 0.0 | 0.0 | 6.0 | Drains 0.25L/use → high-quality oat breakfast ×2. |
| `emergency_civilian_ration_box_5` | Emergency Civilian Ration Box (5x) | Food | 4 | 2.0 | 0.0 | 0.0 | 18.0 | Sealed five-pack. |
| `emergency_civilian_ration_1` | Emergency Civilian Ration (1x) | Food | 15 | 0.4 | 0.0 | 0.0 | 4.0 | Single emergency meal. |
| `canned_fish` | Canned Fish | Food | 12 | 0.4 | 0.0 | 0.0 | 5.0 | Oily protein tin. |
| `canned_beans` | Canned Beans | Food | 15 | 0.4 | 0.0 | 0.0 | 3.5 | Beans in brine. |
| `jam_preserves` | Jam Preserves | Food | 10 | 0.4 | 0.0 | 0.0 | 4.0 | Sweet jar. Morale sugar. |
| `basic_breakfast_bowl` | Basic Breakfast Bowl | Food | 10 | 0.35 | 0.0 | 0.0 | 3.5 | Cooked oats bowl. |
| `quality_dinner_bowl` | Quality Dinner Bowl | Food | 8 | 0.5 | 0.0 | 0.0 | 6.0 | Cooked pasta dinner. |
| `high_quality_oat_breakfast` | High Quality Oat Breakfast | Food | 8 | 0.4 | 0.0 | 0.0 | 5.0 | Oats with milk. |
| `ceramic_water_filter` | Ceramic Water Filter | Filter | 5 | 0.6 | 0.0 | 80.0 | 22.0 | Reusable ceramic filter. |
| `can_opener` | Can Opener Tool | Tool | 5 | 0.15 | 0.0 | 100.0 | 8.0 | Opens tins cleanly. |
| `can_breaker` | Can Breaker | Tool | 8 | 0.3 | 0.0 | 20.0 | 3.0 | Brutal tin opener. 1 use tool-ish. |
| `insulated_flask` | Insulated Flask | Tool | 4 | 0.5 | 0.0 | 0.0 | 10.0 | Keeps liquid hot/cold. |
| `herbal_pills` | Herbal Pills | Medical | 20 | 0.02 | 0.0 | 0.0 | 4.0 | Mild herbal dose. |
| `herbal_bandage` | Herbal Bandage | Medical | 20 | 0.05 | 0.0 | 0.0 | 3.5 | Herb-treated wrap. |
| `bandage_roll` | Bandage Roll | Medical | 25 | 0.08 | 0.0 | 0.0 | 4.0 | Clean cloth roll. |
| `medkit` | Medkit | Medical | 4 | 1.2 | 0.0 | 0.0 | 35.0 | Field trauma kit. |
| `adhesive_bandages_box_6` | Adhesive Bandages Box (6x) | Medical | 10 | 0.05 | 0.0 | 0.0 | 5.0 | Takes 2 per use. |
| `antiseptic_1l_of_1l` | Antiseptic 1L/1L | Medical | 4 | 1.1 | 0.0 | 0.0 | 14.0 | Drains 0.1L/use or sterilises 2 bandages → 2 sterilised. |
| `sterilised_bandage` | Sterilised Bandage | Medical | 20 | 0.06 | 0.0 | 0.0 | 5.5 | Antiseptic-treated bandage. |
| `opioid_painkillers` | Opioid Painkillers | Medical | 15 | 0.02 | 0.0 | 0.0 | 28.0 | Strong pain relief. 1×. |
| `alcohol_wipes_box_10_of_10` | Alcohol Wipes Box 10/10 | Medical | 8 | 0.1 | 0.0 | 0.0 | 7.0 | 1 wipe per use. |
| `antibiotics_bottle_20` | Antibiotics Bottle (20x) | Medical | 5 | 0.08 | 0.0 | 0.0 | 40.0 | Uses 2 per treatment. |
| `epi_pen` | Epi-Pen | Medical | 4 | 0.1 | 0.0 | 0.0 | 45.0 | Emergency adrenaline. |
| `thermometer` | Thermometer | Tool | 8 | 0.05 | 0.0 | 0.0 | 6.0 | Checks temperature; craft component. |
| `medical_scissors` | Medical Scissors | Tool | 8 | 0.1 | 0.0 | 80.0 | 7.0 | Trauma shears. Crafting. |
| `iodine_pills_bottle_10_of_10` | Iodine Pills Bottle 10/10 | Iodine | 5 | 0.05 | 0.0 | 0.0 | 18.0 | Thyroid block. 1 per use. |
| `personal_dosimeter` | Personal Dosimeter | Device | 2 | 0.3 | 0.0 | 100.0 | 30.0 | Logs cumulative dose. |
| `respirator` | Respirator | Protective | 3 | 0.6 | 1.0 | 80.0 | 28.0 | Face seal. Needs filters. |
| `respirator_filter_box_5` | Respirator Filter Box (5x) | Filter | 6 | 0.4 | 0.0 | 0.0 | 20.0 | Five filters. |
| `respirator_filter` | Respirator Filter (1x) | Filter | 20 | 0.08 | 0.0 | 0.0 | 5.0 | Single filter cartridge. |
| `protective_goggles` | Protective Goggles | Protective | 5 | 0.15 | 0.0 | 60.0 | 8.0 | Eye seal. |
| `protective_rubber_gloves` | Protective Rubber Gloves | Protective | 8 | 0.1 | 0.0 | 40.0 | 6.0 | Chem gloves. |
| `decontamination_soap_5_of_5` | Decontamination Soap 5/5 | Medical | 8 | 0.2 | 0.0 | 0.0 | 12.0 | 5 washes. |
| `plastic_contamination_bag_box_5` | Plastic Contamination Bag Box (5x) | Material | 10 | 0.3 | 0.0 | 0.0 | 6.0 | Five hazmat bags. |
| `military_grade_shovel` | Military Grade Shovel | Tool | 2 | 1.5 | 0.0 | 140.0 | 22.0 | Entrenching tool. |
| `military_grade_hatchet` | Military Grade Hatchet | Weapon | 2 | 1.2 | 0.0 | 120.0 | 48.0 | Compact axe. |
| `firefighter_grade_fireaxe` | Firefighter Grade Fireaxe | Weapon | 1 | 2.8 | 0.0 | 140.0 | 55.0 | Heavy breach axe. |
| `pliers` | Pliers | Tool | 10 | 0.25 | 0.0 | 90.0 | 6.0 | Crafting grip tool. |
| `sewing_kit_10_of_10` | Sewing Kit 10/10 | Tool | 5 | 0.15 | 0.0 | 100.0 | 8.0 | 10 stitches/repairs. |
| `flashlight` | Flashlight | Tool | 5 | 0.3 | 0.0 | 80.0 | 9.0 | Battery lamp. |
| `military_grade_flashlight` | Military Grade Flashlight | Tool | 3 | 0.4 | 0.0 | 120.0 | 24.0 | Hardened lamp. |
| `matches` | Matches | Material | 40 | 0.02 | 0.0 | 0.0 | 1.5 | Stove and heater light. |
| `cigarette_lighter` | Cigarette Lighter | Tool | 15 | 0.05 | 0.0 | 0.0 | 3.5 | Reusable spark. |
| `car_battery` | Car Battery | Device | 2 | 12.0 | 0.0 | 0.0 | 25.0 | Heavy 12V cell. |
| `rechargeable_battery` | Rechargeable Battery | Material | 15 | 0.1 | 0.0 | 0.0 | 4.0 | Pack cell. |
| `aa_batteries_package_10` | AA Batteries Package (10x) | Material | 10 | 0.2 | 0.0 | 0.0 | 6.0 | Home upgrades craft. |
| `hand_crank_radio` | Hand-Crank Radio | Device | 2 | 0.8 | 0.0 | 100.0 | 20.0 | News without grid. |
| `small_solar_panel` | Small Solar Panel | Device | 2 | 2.5 | 0.0 | 0.0 | 35.0 | Trickle power. |
| `medium_solar_panel` | Medium Solar Panel | Device | 1 | 6.0 | 0.0 | 0.0 | 55.0 | Serious solar. |
| `generator` | Generator | Device | 1 | 25.0 | 0.0 | 100.0 | 80.0 | Fuel generator. |
| `kerosene_lantern` | Kerosene Lantern | Tool | 3 | 0.7 | 0.0 | 90.0 | 12.0 | Uses fuel or jetfuel. |
| `jetfuel_jerrycan_10l_of_10l` | Jetfuel Jerrycan 10L/10L | Fuel | 2 | 9.0 | 0.0 | 0.0 | 30.0 | Refills kerosene lantern. |
| `winter_coat` | Winter Coat | Protective | 2 | 2.5 | 0.0 | 80.0 | 18.0 | Heavy coat. Warmth. |
| `work_boots` | Work Boots | Protective | 2 | 1.5 | 0.0 | 100.0 | 12.0 | Hard soles. |
| `wool_blanket` | Wool Blanket | Comfort | 4 | 1.2 | 0.0 | 0.0 | 8.0 | Sleep warmth. |
| `improvised_rollup_bed` | Improvised Rollup Bed | Comfort | 2 | 2.0 | 0.0 | 0.0 | 5.0 | Scrap bedroll. |
| `woolbed` | Wool Bed | Comfort | 1 | 8.0 | 0.0 | 0.0 | 22.0 | Proper wool bed. |
| `advanced_heating_bed` | Advanced Heating Bed | Device | 1 | 12.0 | 0.0 | 0.0 | 60.0 | Heated bunk. |
| `wool_gloves` | Wool Gloves | Protective | 5 | 0.15 | 0.0 | 50.0 | 5.0 | Warm hands. |
| `family_photograph` | Family Photograph | Comfort | 5 | 0.02 | 0.0 | 0.0 | 2.0 | Paper memory. Morale. |
| `cassette_tape` | Cassette Tape | Comfort | 8 | 0.05 | 0.0 | 0.0 | 4.0 | Recorded voice. Rare comfort. |
| `sealed_government_document` | Sealed Government Document | Quest | 1 | 0.05 | 0.0 | 0.0 | 0.0 | Quest item. Do not open lightly. |
| `diamond` | Diamond | Trade | 5 | 0.01 | 0.0 | 0.0 | 45.0 | Hard stone. Below guns in barter. |
| `ruby` | Ruby | Trade | 5 | 0.01 | 0.0 | 0.0 | 38.0 | Red gem. |
| `sapphire` | Sapphire | Trade | 5 | 0.01 | 0.0 | 0.0 | 36.0 | Blue gem. |
| `amber` | Amber | Trade | 8 | 0.02 | 0.0 | 0.0 | 18.0 | Fossil resin. |
| `pistol_cz75_9x19` | CZ-75 9×19mm Pistol | Weapon | 1 | 1.1 | 0.0 | 120.0 | 105.0 | Service pistol. Chambers 9×19. |
| `pistol_beretta_92_9x19` | Beretta 92 9×19mm Pistol | Weapon | 1 | 1.15 | 0.0 | 120.0 | 108.0 | Full-size 9×19 service pistol. |
| `pistol_steyr_m9_9x19` | Steyr M9 9×19mm Pistol | Weapon | 1 | 0.95 | 0.0 | 120.0 | 102.0 | Polymer 9×19 sidearm. |
| `prewar_letter` | Pre-War Letter | Comfort | 5 | 0.05 | 0.0 | 0.0 | 1.0 | A sealed envelope, yellowed and brittle. The stamp is from a country that no longer exists. Someone wrote this to someone they loved, and it never arrived. |
| `charcoal_sketch` | Charcoal Sketch | Comfort | 3 | 0.02 | 0.0 | 0.0 | 0.5 | A childs drawing on the back of a ration card. Stick figures of a family. A sun with rays like bent spokes. A house with smoke coming out of the chimney. The paper is singed at the edges. |
| `snow_goggles` | Snow Goggles | Protective | 1 | 0.2 | 0.0 | 30.0 | 6.0 | Tinted lenses in a rubber frame. The strap is frayed but holds. Cuts the glare off the ash-fields and makes the horizon stop hurting. |

### 5.2 Crafting Recipes (16 Recipes)
Below is the complete dataset of all 16 crafting recipes in `recipes.json`:

- **`craft_bandage` (Craft Bandage):** Requires `2x cloth` at station `workbench` (0.5h) -> Produces `1x bandage`.
- **`purify_water` (Purify Water):** Requires `2x irradiated_water` at station `water_purifier` (0.5h) -> Produces `2x clean_water`.
- **`craft_anti_rad` (Craft Anti-Rad):** Requires `1x iodine_pills, 1x clean_water` at station `workbench` (1.0h) -> Produces `1x anti_rad`.
- **`craft_water_filter` (Craft Water Filter):** Requires `1x cloth, 1x scrap_metal` at station `workbench` (1.0h) -> Produces `1x water_filter`.
- **`craft_air_filter` (Craft Air Filter):** Requires `2x cloth, 1x scrap_metal` at station `workbench` (1.5h) -> Produces `1x air_filter`.
- **`cook_meat` (Cook Meat):** Requires `1x raw_meat, 1x fuel` at station `stove` (0.5h) -> Produces `1x cooked_meat`.
- **`boil_water` (Boil Water):** Requires `2x dirty_water, 1x fuel` at station `stove` (0.3h) -> Produces `1x clean_water`.
- **`craft_hazmat_patch` (Patch Hazmat Suit):** Requires `3x cloth, 2x scrap_metal, 1x water_filter` at station `workbench` (3.0h) -> Produces `1x hazmat_suit`.
- **`craft_gas_mask` (Craft Gas Mask):** Requires `2x cloth, 1x water_filter, 1x scrap_metal` at station `workbench` (2.0h) -> Produces `1x gas_mask`.
- **`refuel_heater` (Refuel Heater):** Requires `3x fuel` at station `heater` (0.1h) -> Produces `0x fuel`.
- **`craft_dosimeter` (Craft Dosimeter):** Requires `3x scrap_metal, 1x cloth` at station `workbench` (2.0h) -> Produces `1x dosimeter`.
- **`craft_medical_kit` (Craft Medical Kit):** Requires `3x bandage, 1x anti_rad, 2x iodine_pills` at station `workbench` (1.5h) -> Produces `1x medical_kit`.
- **`craft_geiger_counter` (Craft Geiger Counter):** Requires `5x scrap_metal, 1x battery, 1x cloth` at station `workbench` (3.0h) -> Produces `1x geiger_counter`.
- **`craft_calibration_kit` (Craft Calibration Kit):** Requires `2x scrap_metal, 1x cloth` at station `workbench` (1.5h) -> Produces `1x calibration_kit`.
- **`craft_battery` (Craft Battery):** Requires `2x scrap_metal, 1x dirty_water` at station `workbench` (1.0h) -> Produces `1x battery`.
- **`craft_engine` (Rebuild Engine):** Requires `30x mechanical_parts, 5x electronic_scrap, 10x scrap_metal` at station `workbench` (12.0h) -> Produces `1x engine`.

### 5.3 Survivor Archetypes (72 Survivors)
Below is the complete dataset of all 72 survivors in `survivors.json`:

| ID | Name | Profession | Base Health | Bio |
| --- | --- | --- | --- | --- |
| `elena_vasquez` | Elena Vasquez | Paramedic | 100 | Former field medic who ran triage in the first hours after the exchange. She doesn't talk about what she saw, but her hands never shake. |
| `marcus_olejnik` | Marcus Olejnik | Mechanical Engineer | 90 | Kept the municipal water plant running for three weeks after the grid fell. Came to the bunker when the last pipe froze. |
| `suki_tanaka` | Suki Tanaka | Farmer | 95 | Her family's greenhouse survived the initial blast. She knows which soil still grows and which poisons the roots. |
| `the_surgeon` | The Surgeon | Surgeon | 90 | Haunted by a patient lost on Day 1. Their hands still remember the cut they couldn't finish. |
| `the_pharmacist` | The Pharmacist | Pharmacist | 85 | Empty bottles line their pockets. The old CVS still has their logbook — if they dare go alone. |
| `the_vet` | The Vet | Veterinarian | 95 | They treated dogs before the war. Now the dogs glow, and something in them still wants to heal. |
| `the_therapist` | The Therapist | Therapist | 80 | They used to sit across from people and wait. The bunker still needs someone who can listen when the walls start talking. |
| `the_undertaker` | The Undertaker | Undertaker | 100 | They buried the first dead after the exchange. Closure is a trench and a shovel and the quiet that follows. |
| `the_veteran` | The Veteran | Soldier | 110 | Day 1 never ended for them. The old squad still calls on the radio — and something answers back. |
| `the_cop` | The Cop | Police Officer | 100 | They still wear the badge under the coat. The precinct lockbox is still there — if the streets allow it. |
| `the_bouncer` | The Bouncer | Bouncer | 120 | Door security before the war. Now the only door that matters is the hatch, and they still stand it alone. |
| `the_hunter` | The Hunter | Hunter | 95 | They tracked game before the blast. Something white still moves out there — and only a bow feels honest. |
| `the_prisoner` | The Prisoner | Convict | 90 | Left to die when the walls opened. The Warden's keys still hang on a ghoul's belt inside the Penitentiary. |
| `the_plumber` | The Plumber | Plumber | 95 | They know every main under the city. When the walls scream water, they still answer. |
| `the_electrician` | The Electrician | Electrician | 90 | The grid is a ghost. They walk its bones with rubber gloves and a prayer. |
| `the_architect` | The Architect | Architect | 85 | The firm is a tomb of paper and low air. The city's underbelly is still drawn somewhere. |
| `the_mechanic` | The Mechanic | Mechanic | 100 | Engines are honest. The highway pileup still has one heart left if they can drag it home. |
| `the_chemist` | The Chemist | Chemist | 85 | Chlorine does not negotiate. Someone has to put their body on the valve. |
| `the_botanist` | The Botanist | Botanist | 90 | Green is a religion now. Fourteen perfect days is all the soil asks. |
| `the_courier` | The Courier | Courier | 95 | Routes over roads. Five clean drops and the map still believes in them. |
| `the_burglar` | The Burglar | Burglar | 90 | Vaults remember hands. The ruined bank still has a lock that wants them. |
| `the_meteorologist` | The Meteorologist | Meteorologist | 85 | The sky still talks if you climb high enough into the storm. |
| `the_hazmat_tech` | The Hazmat Tech | Hazmat Technician | 100 | Ground Zero keeps a black box. The suit and the iodine have to be perfect. |
| `the_teacher` | The Teacher | Teacher | 85 | The school still has a list of names. Someone has to write them down. |
| `the_politician` | The Politician | Politician | 80 | Words still move people. The hatch is a podium if you broadcast hard enough. |
| `the_priest` | The Priest | Priest | 85 | Faith cracks under zero morale. Someone has to talk them down from the edge. |
| `the_reporter` | The Reporter | Reporter | 85 | Five military fragments still name who fired first. Truth is a kind of food. |
| `the_radio_host` | The Radio Host | Radio Host | 90 | Forty-eight hours of dead air would kill the allies. They keep talking through the storm. |
| `the_chef` | The Chef | Chef | 90 | One of every food left in the world. Then twenty-four hours for the last supper. |
| `the_athlete` | The Athlete | Athlete | 110 | Fifteen nodes out and back on foot before the clock hits forty-eight. |
| `the_firefighter` | The Firefighter | Firefighter | 105 | The generator room burns. No suit. Just hands and the will not to run. |
| `the_tailor` | The Tailor | Tailor | 85 | Ten ragged skins become something that can stop a bullet and a rad. |
| `the_watchmaker` | The Watchmaker | Watchmaker | 80 | The EMP killed an heirloom. Fifty scraps of electronics for one tick. |
| `the_historian` | The Historian | Historian | 80 | The museum is on fire. The Constitution is still inside. |
| `the_defector` | The Defector | Cult Defector | 100 | The Cult of the Glow remembers them. Only the leader's death ends the raid. |
| `the_addict` | The Addict | Addict | 85 | Fourteen clean days. Morphine in the cupboard. Hands stay empty. |
| `the_parent` | The Parent | Parent | 95 | The radio says the child is gone. If they live through the mourning, the bunker is family. |
| `the_fierce_mother` | The Fierce Mother | Mother | 95 | She will cancel her own meal if a child is hungry. The daycare still has the toy. |
| `the_exhausted_father` | The Exhausted Father | Father | 100 | He works until he collapses. Five tier-three modules before day fifty was the promise. |
| `the_naive_son` | The Naive Son | Child | 70 | He wanders the bunker playing. Adults feel hope just looking at him. |
| `the_hardened_daughter` | The Hardened Daughter | Child | 75 | She will not play. She will not be comforted. First blood at the hatch will change her. |
| `the_psychopath` | The Psychopath | Outsider | 100 | Death does not move them. They refuse other hands. Empty rooms make them sharp. |
| `the_serial_killer` | The Serial Killer | Neighbor | 100 | Kind. Charismatic. Something under the smile keeps score. |
| `the_liar` | The Liar | Storyteller | 90 | The UI says they are fine. The radio says the stash is real. Neither is true. |
| `the_hoarder` | The Hoarder | Collector | 90 | Double rations or the mood sours. Storage sheds weight into a private vault. |
| `the_general` | The General | Military | 100 | Day-1 command died with the first court-martial. Every remnant uniform is a death sentence. He still draws the map. |
| `the_saboteur` | The Saboteur | Rebel | 95 | Orders taste like ash. Traps open for them without a word. The checkpoint still stands. |
| `the_deserter` | The Deserter | Sniper | 90 | Ran once. Still flinches at hammers and generators. The hatch will ask if they can stay. |
| `the_quartermaster` | The Quartermaster | Logistics | 95 | Counts beans like lives. A messy shelf is a personal insult. One hundred of everything. |
| `the_child_soldier` | The Child Soldier | Child | 80 | Adult hands on a child's frame. Science and medicine will not stick. The rifle will not leave. |
| `the_empath` | The Empath | Counselor | 90 | Feels every room. Comfort before food. Three broken minds, and their own body pays. |
| `the_misanthrope` | The Misanthrope | Hermit | 95 | Shared air is an injury. Alone, they are fast. Fifteen days of no one. |
| `the_pollyanna` | The Pollyanna | Optimist | 90 | The meter never shows fear. Fallout rain is weather. Denial ends only when the body fails and lives. |
| `the_martyr` | The Martyr | Caregiver | 100 | Steps into the breach for strangers. Food leaves their tray without a word. Someone has to pay. |
| `the_arrogant_surgeon` | The Arrogant Surgeon | Surgeon | 100 | Gods do not mop floors. Patients leave smaller. One failed cut, ten days of the dark. |
| `the_relapsing_addict` | The Relapsing Addict | Addict | 90 | The hands shake for amphetamines. Below forty morale the locks mean nothing. Twenty-one clean days or the chemistry wins. |
| `the_insomniac` | The Insomniac | Night Watch | 95 | Sleep is a rumor. Beds return almost nothing. Five nights alone on the hatch, or the pacing never ends. |
| `the_hypochondriac` | The Hypochondriac | Patient | 85 | Every cough is a death sentence until the placebo. Real sepsis is the only diagnosis that sticks. |
| `the_pyromaniac` | The Pyromaniac | Arsonist | 95 | Heaters are churches. Low morale smells like smoke. Five fires put out, or the bunker burns. |
| `the_blind_preacher` | The Blind Preacher | Preacher | 90 | No expeditions. No guns. Only sound and three conversions from despair. The walls speak first. |
| `the_prepper` | The Prepper | Prepper | 100 | Paranoid before the sirens. Own MREs only. Survive the day the hatch is gone. |
| `the_outcast` | The Outcast | Mutant | 80 | Eight hundred millisieverts and counting. Meals alone. One thousand without dying is the only home left. |
| `the_feral_orphan` | The Feral Orphan | Child | 75 | No words. Raw meat. Sleeps on concrete. Thirty days under the Vet or the Fierce Mother. |
| `the_pacifist` | The Pacifist | Monk | 90 | No weapons. Always flees. A needless kill and the hunger strike begins. Level five, zero damage. |
| `the_widow` | The Widow | Botanist | 90 | Grief steals the work. Hydroponics before sleep. One Pre-War Rose, and the crying stops. |
| `the_ex_con` | The Ex-Con | Laborer | 110 | Stashes lock when the door opens. No orders from cops or generals. Drag one dying body home. |
| `the_sheriff` | The Sheriff | Lawman | 95 | Morale for the room. A failing heart. Guard duty when no one else stands. One raider boss. |
| `the_former_politician` | The Former Politician | Politician | 90 | Charisma maxed. Labor skill zero. Fourteen dirty days in a row, or the delegation never ends. |
| `the_tech_bro` | The Tech Bro | Engineer | 90 | The old world is coming back. The tablet wastes power. EMP, then a purifier from scrap. |
| `the_news_anchor` | The News Anchor | Anchor | 90 | Hygiene is a career. The journal never stops. One true broadcast from the tower. |
| `the_nomad` | The Nomad | Scavenger | 100 | Walls are a cage. Five days inside and the hatch opens alone. Build a bed worth staying for. |
| `the_exec` | The Exec | Executive | 95 | Modules work harder and die faster. In fire, inventory first. Ten thousand in trade value. |

### 5.4 Scavenging Locations (5 Locations)
Below is the complete dataset of all 5 locations in `locations.json`:

- **`abandoned_hospital` (Abandoned Hospital):** Danger Level 6 | Travel Time: 2.0h | Base Rads: 35 mSv/h. Description: *Collapsed wing of a regional hospital. Medical supplies may remain in sealed rooms.*
- **`rural_gas_station` (Rural Gas Station):** Danger Level 3 | Travel Time: 1.5h | Base Rads: 15 mSv/h. Description: *A stripped roadside station. Fuel drums and scrap metal scattered across the lot.*
- **`suburban_house` (Suburban House):** Danger Level 2 | Travel Time: 1.0h | Base Rads: 10 mSv/h. Description: *Intact house in a low-density neighborhood. Pantry, tools, maybe a working stove.*
- **`government_bunker` (Government Bunker):** Danger Level 8 | Travel Time: 4.0h | Base Rads: 60 mSv/h. Description: *Sealed military installation. High-value supplies behind heavy doors.*
- **`stranger_cache` (The Cartographer's Cache):** Danger Level 7 | Travel Time: 3.0h | Base Rads: 25 mSv/h. Description: *A sealed pre-war storage locker in a collapsed residential block. Coordinates from a stranger. Real or a trap — you won't know until you're there.*

### 5.5 Narrative Events (39 Events)
Below is a sample overview of authored game events in `events.json`:

- **`fallout_storm` - Fallout Storm** (Min Day: 1, Weight: 3.0): *The sky turns a sickly amber. Fine ash begins to settle on everything outside. The Geiger counter screams.*
- **`scavenger_arrival` - Scavenger at the Door** (Min Day: 3, Weight: 2.0): *A gaunt figure stands at the bunker entrance, hands raised. They carry a pack and a worn-out rifle. They ask to trade.*
- **`filter_failure` - Air Filter Failure** (Min Day: 5, Weight: 1.5): *A grinding noise from the ventilation shaft. The air filter has seized. Dust and faint radiation leak into the shelter.*
- **`radio_static` - Radio Transmission** (Min Day: 7, Weight: 1.0): *Through layers of static, a voice: coordinates, a supply cache, and a warning about what guards it.*
- **`water_contamination` - Water Contamination** (Min Day: 2, Weight: 2.0): *The stored water tastes metallic. A quick check with the dosimeter confirms it: the supply is hot.*
- **`stranger_sickness` - Sick Stranger** (Min Day: 4, Weight: 1.5): *A coughing figure stumbles into the airlock. Their skin is mottled with radiation burns. They collapse before speaking.*
- **`generator_sputter` - Generator Failure** (Min Day: 3, Weight: 2.0): *The lights flicker and die. The generator coughs once, then silence. Cold seeps in immediately.*
- **`rat_infestation` - Rats in the Stores** (Min Day: 6, Weight: 1.5): *Droppings in the food crate. Something has been gnawing at the ration packs. The scratching comes from the walls.*
- **`morale_crisis` - Shouting in the Dark** (Min Day: 8, Weight: 1.0): *Raised voices echo through the bunker. Two survivors are arguing over the last iodine pill. The tension is reaching a breaking point.*
- **`supply_drop_rumor` - Supply Drop Rumor** (Min Day: 10, Weight: 1.0): *A crackling radio message mentions a military supply drop three miles north. Could be real. Could be a trap.*
- **`hatch_breach` - Hatch Seal Breach** (Min Day: 4, Weight: 2.0): *A high-pitched whistle from the outer hatch. The rubber gasket has cracked. Fallout-laced air seeps in.*
- **`water_shortage` - Dry Pipes** (Min Day: 5, Weight: 2.5): *The water barrel is nearly empty. Rationing has begun, but thirst is already setting in. Desperation grows.*
- **`heater_malfunction` - Heater Overheat** (Min Day: 6, Weight: 1.5): *The heater unit glows red-hot. A acrid smell fills the shelter. Someone needs to shut it down before it catches fire.*
- **`dead_animal` - Dead Dog Outside** (Min Day: 3, Weight: 0.8): *A stray dog lies dead near the airlock, fur patchy, ribs showing. Fallout got it. A reminder of what waits outside.*
- **`cramped_quarters` - Cabin Fever** (Min Day: 12, Weight: 1.0): *The walls feel closer every day. Someone hasn't spoken in three days. Another paces endlessly. The bunker was not built for this many people.*
- **`found_diary` - Old Diary** (Min Day: 8, Weight: 0.5): *Behind a loose panel, a diary from the bunker's original occupant. The last entry is dated three weeks after the bombs. It does not end well.*
- **`ash_pileup` - Ash Accumulation** (Min Day: 7, Weight: 2.0): *Radioactive ash has piled up against the air intake. If it's not cleared, the filters will clog within hours.*
- **`trader_offers_medicine` - Medicine Trade** (Min Day: 10, Weight: 1.0): *A trader at the gate claims to have anti-rad medication. They want food in return. The deal feels too good.*
- **`power_cell_discovery` - Hidden Power Cell** (Min Day: 15, Weight: 0.7): *While clearing debris, a sealed power cell is found behind a wall panel. Still charged. Could power the radio for weeks.*
- **`night_terrors` - Night Terrors** (Min Day: 5, Weight: 1.0): *Someone wakes screaming. They dreamed of the flash, the heat, the silence after. No one sleeps well anymore.*
- **`filter_degradation` - Filter Wear** (Min Day: 10, Weight: 2.0): *The air filter's indicator has dropped into the yellow. Replacement is needed soon, or the air quality will suffer.*
- **`lucky_find` - Lucky Find** (Min Day: 3, Weight: 0.5): *A sealed crate in the corner, overlooked until now. Inside: canned food, a water filter, and a working flashlight.*
- **`radiation_leak` - Radiation Spike** (Min Day: 8, Weight: 2.0): *The dosimeter alarm triggers. Radiation levels inside the bunker have spiked. Something is leaking.*
- **`morale_boost` - Old Music** (Min Day: 14, Weight: 0.5): *Someone finds a working music player with a single battered disc. For a few minutes, the bunker feels almost human again.*
- **`structural_damage` - Wall Crack** (Min Day: 12, Weight: 1.5): *A hairline crack has appeared in the bunker wall. Water seeps through when it rains. If it widens, the shelter is compromised.*
- **`flashpoint_buildup_radio_dead_air` - Dead Air** (Min Day: 25, Weight: 1.0): *The civilian broadcast cut out mid-sentence. No carrier, no static. Just a thin repeating tone, then silence. We tried every frequency. Nothing.*
- **`flashpoint_buildup_trader_panic` - Trader in a Hurry** (Min Day: 26, Weight: 1.0): *The trader took the iodine without haggling. Didn't look at the food. Said they'd be back in two days, maybe less. Packed lighter than usual going out.*
- **`flashpoint_buildup_military_codes` - Numbers Station** (Min Day: 27, Weight: 1.0): *An automated military frequency cycling through cipher blocks. No operator, no preamble, just beeps at fixed intervals. Marcus sat and listened for an hour. Said it means nobody's left to talk.*
- **`flashpoint_buildup_news_anchor_panic` - Static, Then a Voice** (Min Day: 28, Weight: 1.0): *An international station cut in over the interference. The anchor's voice was shaking. 'Do not—' the signal collapsed. We never found out what not to do.*
- **`flashpoint_buildup_war_silence` - The Shelling Stopped** (Min Day: 29, Weight: 1.0): *Last night the artillery reached a fever pitch, shells falling close enough to feel in the floor. By morning, no gunfire at all. Just the wind against the hatch. Nobody slept through it.*
- **`silent_knock_part1` - A Knock in the Ash** (Min Day: 35, Weight: 1.5): *Three slow knocks on the outer hatch. Deliberate. Not debris, not wind. High fallout outside — the ash is coming down like grey snow. Whoever is out there cannot see, and neither can you.*
- **`silent_knock_part2a_wakes` - The Stranger Wakes** (Min Day: 37, Weight: 0.0): *He opens his eyes on Day 37. Radiation burns across his neck. He was a cartographer. Says he knows a pre-war cache — sealed, untouched. Grid coordinates. He writes them down on a strip of packaging.  He needs water. He's barely holding on.*
- **`silent_knock_part2b_scraping` - The Scraping** (Min Day: 38, Weight: 0.0): *On Day 38, it starts again — but this time not knocking. A long, slow scraping against the outer hatch seal. Something methodical. Then silence for six hours. Then again. It continues for two days, then stops.*
- **`silent_knock_part3_coordinates` - The Coordinates** (Min Day: 39, Weight: 0.0): *The address the stranger wrote. Grid reference for what he called a 'pristine pre-war cache'. Could be medicine, sealed food, batteries. Could be exactly what it says on the paper.  You have his word. And the memory of the water you gave him.*
- **`the_emissary` - The Emissary** (Min Day: 5, Weight: 1.5): *Someone from the scavenger camp stands at the hatch. Hands empty, voice dry. They want water — enough for three, they say. The canteen at their hip is dented and light.*
- **`emissary_return_favor` - The Favor** (Min Day: 1, Weight: 0.0): *Two days later, the same voice at the hatch — softer. A half-crate of canned goods sits on the threshold. Payment for the water, they say. No weapons in sight.*
- **`emissary_return_caught` - The Mechanic** (Min Day: 1, Weight: 0.0): *They came back with a thin man who smells of solder. He listens at the hatch for the purifier's hum. The lie is thin now. They wait.*
- **`emissary_return_grudge` - The Grudge** (Min Day: 1, Weight: 0.0): *Three days. Same hatch. Fewer words. They want water or they want a reason to stop asking politely.*
- **`emissary_return_raid_warning` - After the Hatch** (Min Day: 1, Weight: 0.0): *No knock. Just bootprints in the ash leading away from the hatch, then a radio burst on the scavenger band that cuts off mid-word. Someone will come back heavier.*

### 5.6 Echoes of the Past (15 Echoes)
Below is the dataset of survivor echoes in `echoes.json`:

- **`echo_answering_machine` - The Answering Machine**: *Under a collapsed awning, half-buried in ash, a solar-powered desk phone blinks a single red light. The voicemail counter reads 14. The battery is at 4%. Most of the messages are static, hang-ups, prayers. The last one is dated Day 1. A mother, voice steady, calling her daughter home from college.*
- **`echo_childs_coat` - The Child's Coat**: *Caught in a chain-link fence, a small red winter coat, zipped halfway. The hood is up. There's no one inside it, but the front is dark with the kind of blood that has been in the sun for a year. The stitching is intact. The label reads age 6.*
- **`echo_wedding_ring` - The Wedding Ring in the Glass**: *Two photographs fused to the glass of a picture frame by heat. A man and a woman on a beach. The wedding band is still inside the frame, rolled against the inner edge, scratched where the bride-to-be tried to pry it out. The inside is engraved: 04.12 — FOREVER, L.*
- **`echo_frozen_postman` - The Frozen Postman**: *A postal van, door open, engine dead. The driver is in the seat, hands still on the wheel, frosted glasses on. The mailbag is unzipped. Most of it is past-due bills and pharmacy ads. One envelope is unsealed, hand-addressed in pencil: To whoever finds this. There is no return address.*
- **`echo_music_box` - The Music Box**: *In a nursery that was never finished, a wooden ballerina turns on a brass key. The varnish has yellowed. The key is stiff but still works. The first phrase of Für Elise. Forty-seven seconds, then a tiny mechanical sigh. You stand there with the wind at your back.*
- **`echo_crayon_drawing` - The Crayon Drawing**: *A kindergarten wall, mostly collapsed. Where the plaster is still up, crayon suns, blue stick-figure families, a yellow dog. One drawing, bigger than the rest, is in careful letters: I love you Mommy. There is a brown hand-print in the corner with a name. T-HeM.*
- **`echo_family_dog_collar` - The Family Dog**: *A leather dog collar under a porch step. The buckle is still in the open position. The bone-shaped tag, brass, is engraved BUDDY. No dog. No body. The leather is the kind of leather that was rubbed soft by a hand.*
- **`echo_unsent_letter` - The Unsent Letter**: *On a kitchen table under an overturned coffee mug, a single page, lined, written in pencil. 'Dear Tom, I know we said things we can't take back. The world ended last week, in case you missed it. I'm sorry. I forgive you. I hope you got out.' No address. No stamp. The pencil is still on the page.*
- **`echo_anniversary_calendar` - The Anniversary Calendar**: *A wall calendar, pre-war, one month showing. One date is circled twice in red pen: TODAY. Above the calendar, a photo of a couple in their sixties, dressed for something. The kitchen smells faintly of coffee that hasn't been made in a year.*
- **`echo_newborn_bracelet` - The Newborn Bracelet**: *In a hospital corridor that never had patients, a sealed plastic bag pinned to a bulletin board. Inside, a printed hospital bracelet. BABY GIRL MORALES. Born 14 March. 7 lbs 4 oz. The cord had not been cut when the building lost power. The bag is dated eleven days before the first flash.*
- **`echo_birthday_cake` - The Birthday Cake**: *A chest freezer, lid blown open by a pipe burst. Inside, a sheet cake still on its cardboard base, eight unlit candles melted into pink frosting. The frosting is the wrong color now, but the piped words are legible: HAPPY 8TH BIRTHDAY EMMA.*
- **`echo_smoking_pipe` - The Smoking Pipe**: *A man's study. Pipe stand, three pipes. One is oak, the bowl carved with a name in small letters: JAMES. The ash from the last bowl is still in it, white-grey, undisturbed. A tumbler of something that is no longer anything sits beside a half-written letter to a son.*
- **`echo_reading_glasses` - The Reading Glasses**: *Apartment 3B. Bathroom cabinet still mirrored. A pair of bifocals, prescription scratched off the temple. They belonged to a man who read three books at a time — there are bookmarks in three different rooms, all in mid-sentence. The last book is face-down on a chair.*
- **`echo_library_card` - The Library Card**: *In a coat that has not moved in a year, a wallet. Inside, a library card stamped in red: DUE: 3 DAYS AFTER ARMAGEDDON. Two dollars, a stick of gum, and a school photo of a boy with a missing front tooth. The gum, somehow, is still good.*
- **`echo_post_it_fridge` - The Post-it on the Fridge**: *A yellow square on a fridge door that is now part of a wall. In blue pen, in a careful hand: Ran out of milk. Will be home by 6. Love you. — Dad. The fridge is open. The milk is gone. Six o'clock has come and gone a thousand times.*

### 5.7 Radio Broadcast Archive (50 Broadcasts from IntelBible)

# IntelBible.md — Radio Broadcast Archive

## Overview
This document contains 50 radio broadcast texts used by the RadioTunerSystem. Broadcasts are
categorized by game phase and frequency type. The player extracts these by tuning the radio,
and they serve as both narrative flavor and gameplay intel (Plume Reports, Weather Forecasts,
Mortar Warnings, etc).

**Tone:** Cold, exhausted, human, restrained. Show, don't preach. No magic, no fantasy, no
real countries/wars/people.

---

## Pre-War / Civil War Context

**The two sides.** The fracture was a slow one. For thirty years the central government in
the river valley had held the trade routes, the fuel pipelines, the iodine reserves, and the
broadcast towers. The upland provinces paid the taxes that kept the army and the ministries
running, and in return got subsidized grain and the occasional paved road. When the river
dried up for the second summer in a row, both sides decided the other one had been lying
about the reserves. By the time the first provincial assembly voted to keep its grain,
the central garrison was already on the road. The fighting has been conventional, mostly:
artillery exchanges along the ridgeline, drone strikes on fuel depots, an infantry push every
few weeks that gains two kilometers and then gives them back.

**Where the player is.** The bunker is in a small upland market town called (locals still
use the name) Tessarat, about forty kilometers from the contested ridgeline. The town's
official alignment is the central government, but the surrounding villages lean upland. The
shelling that has been audible on days 1-29 is the central garrison's counter-battery fire
against upland mortar positions in the hills to the east. The international news broadcasts
the player can sometimes pull in are from a neutral broadcast consortium that tries to cover
both sides without naming them; their panicked tone on day 28 reflects the diplomatic cables
leaking, not the battlefield. When the morning of day 30 arrives and the firing has stopped,
the silence means the central garrison's forward observers have either abandoned their posts
or stopped transmitting. It does not mean the war is over. It means the next order of business
is no longer artillery.

**What the buildup broadcasts are signaling.** The dead air on day 25 is the civilian
transmitter losing power. The trader panic on day 26 is a real phenomenon: people with
iodine and working water filters are paid for in food now, not money, because money has no
guarantee. The numbers station on day 27 is the military's automated contingency broadcast,
which fires when no human operator is on shift. The international anchor on day 28 is
broadcasting a partial evacuation order that was rescinded before it ended. The ceasefire
silence on day 29 is the worst of them, because it sounds like peace.

---

## Phase 1: Pre-War Propaganda (Days -30 to 0)

These broadcasts are nostalgic, state-sponsored, hopeful. They play on civilian frequencies
before the exchange. After Day 0, these frequencies go silent or become emergency broadcasts.

### Civilian Frequency (88.5 FM)
1. "Good morning, citizens. Today's weather: clear skies, high of 22°C. Remember to check
   your neighborhood radiation shelter assignment. Stay informed, stay safe."

2. "The Ministry of Civil Defense reminds all households: test your iodine pills monthly.
   Replace expired filters in your home ventilation units. Preparedness is patriotism."

3. "Community bulletin: The eastern district water purification station will undergo
   maintenance Tuesday. Bring your own containers. Boil all water before consumption."

4. "A message from the Bureau of Public Health: Annual thyroid screenings are now available
   at all district clinics. Early detection saves lives. Schedule yours today."

5. "This is the National Weather Service. A cold front is moving in from the north.
   Residents in exposed areas should secure outdoor equipment and check heating fuel reserves."

### Military Frequency (102.1 FM)
6. "Attention all units. Border surveillance reports increased activity in sector 7.
   Maintain elevated alert status. All leave canceled until further notice."

7. "Logistics update: Fuel convoys to northern outposts depart at 0600 tomorrow. Escort
   teams report to motor pool at 0500. Standard radiation protocols in effect."

8. "Signal intelligence briefing: Intercepted communications suggest enemy forces are
   repositioning artillery in the eastern corridor. All forward units take cover."

9. "Medical corps advisory: Radiation sickness cases at field hospital delta have doubled
   this week. Requesting additional medical supplies and personnel. Priority one."

10. "Training notice: All reservists in zones 4 through 9 report for refresher training
    next Monday. Bring your protective gear and dosimeters. No exceptions."

---

## Phase 2: Day 1-30 Panic (Emergency Broadcasts)

These broadcasts play during the first month after the exchange. They're chaotic, urgent,
sometimes contradictory. Military frequencies provide tactical warnings (Mortar Warning,
Troop Movement). Civilian frequencies broadcast emergency instructions.

### Civilian Emergency (88.5 FM)
11. "EMERGENCY ALERT. Fallout detected in your area. Seek shelter immediately. Seal all
    windows and doors. Do not go outside. Repeat: do not go outside."

12. "This is not a test. Multiple detonations confirmed. All citizens proceed to designated
    shelters. Bring supplies for 72 hours. If you hear sirens, you have ten minutes."

13. "Medical emergency broadcast. Radiation levels exceeding safe limits in districts 3, 5,
    and 8. Evacuation routes are marked. Follow the yellow lines. Do not use main roads."

14. "Water contamination confirmed in sector 4 reservoir. Do not drink tap water. Use
    bottled or boiled water only. Purification tablets distributed at shelter entrances."

15. "Attention survivors. Rescue teams are en route to your location. Stay in your shelters.
    Do not attempt to leave. Repeat: do not leave your shelters. Help is coming."

16. "Food distribution update. Rationing is now in effect. Report to your district center
    at 0800 tomorrow with your identification cards. One meal per person per day."

17. "Health advisory: Symptoms of radiation sickness include nausea, fatigue, and hair loss.
    If you experience these symptoms, report to the medical station immediately. Take iodine
    pills as directed."

18. "Security bulletin. Looters have been reported in the southern districts. All citizens
    are advised to remain indoors after dark. Curfew is in effect from 2000 to 0600."

19. "Weather update: A fallout storm is approaching from the west. Expected to arrive within
    six hours. Seal all shelters. Turn off ventilation systems. This is not a drill."

20. "Communication breakdown reported in eastern sectors. If you are in zones 6, 7, or 9,
    you may experience radio static. Alternate frequencies are 91.3 and 95.7."

### Military Tactical (102.1 FM)
21. "Warning: Artillery barrage detected. Impact zone: grid reference 47-82. All units in
    the area take cover immediately. Estimated duration: 20 minutes."

22. "Troop movement reported. Enemy convoy heading north on highway 12. Speed: 40 kph.
    Estimated arrival at checkpoint alpha: 1400 hours. All units prepare to engage."

23. "Air defense alert. Unidentified aircraft detected at bearing 270, range 50 kilometers.
    All units man anti-air positions. Prepare for possible strike."

24. "Reconnaissance report. Enemy forces establishing forward operating base in sector 9.
    Estimated strength: battalion. Requesting air support. Priority: high."

25. "Medical evacuation request. Field hospital gamma taking radiation casualties. Need
    transport to rear area. Urgent. Repeat: urgent."

26. "Supply convoy ambush reported on route delta. Three vehicles disabled. Requesting
    immediate reinforcement. Enemy strength: unknown. Proceed with caution."

27. "Chemical detection alert. Sensors indicate possible chemical agent in sector 3.
    All units don protective gear immediately. Await further instructions."

28. "Night patrol report. Contact with enemy reconnaissance team at 0200 hours. Two
    casualties. Enemy withdrew to the east. Pursuit not recommended due to radiation levels."

29. "Artillery adjustment. Battery 7 correcting fire mission. Danger close. All friendly
    units clear the target area. Repeat: clear the target area."

30. "Command update. Strategic situation deteriorating. All units prepare for possible
    withdrawal to secondary defensive line. Hold current positions until further notice."

---

## Phase 3: Post-Day 30 (Automated Loops & Numbers Stations)

After Day 30, military frequencies go silent. What remains are automated emergency broadcasts
(loops), numbers stations (encoded transmissions), and the occasional survivor broadcast.
These provide Plume Reports (radiation data) but with low confidence.

### Automated Emergency Loops (Emergency Frequency)
31. "This is an automated emergency broadcast. Radiation levels in this area exceed safe
    limits. Do not enter without protective equipment. Repeat: do not enter without
    protective equipment."

32. "Emergency water purification instructions. Boil all water for ten minutes. Add three
    drops of bleach per liter. Let stand for thirty minutes before consumption."

33. "This is an automated shelter locator signal. If you can hear this broadcast, you are
    within range of a designated fallout shelter. Proceed to the nearest marked entrance."

34. "Medical advisory: Iodine pills effective only if taken before radiation exposure.
    Do not take iodine if already exposed. Seek medical attention immediately."

35. "Emergency food storage protocols. Canned goods safe for consumption if seals are intact.
    Discard any cans showing bulging, rust, or damage. When in doubt, do not consume."

36. "This is an automated radiation monitoring station. Current readings: [DATA CORRUPTED].
    Next update in six hours. Stay tuned to this frequency for further information."

37. "Emergency shelter maintenance reminder. Check air filtration systems weekly. Replace
    filters when indicator shows red. Clean ventilation ducts monthly to prevent blockage."

38. "Automated weather warning. Nuclear winter conditions persist. Average temperature:
    minus 15°C. Ensure adequate heating fuel. Conserve energy. Ration food supplies."

39. "This is an automated survivor locator signal. If you are hearing this broadcast,
    transmit your location on frequency 99.0. Rescue teams will attempt contact."

40. "Emergency medical protocol. For radiation sickness: rest, hydration, iodine if available.
    Seek medical shelter immediately. Do not attempt to treat severe cases without training."

### Numbers Station (99.0 FM)
41. "Seven. Three. Nine. One. Five. Eight. Two. Four. Six. Zero. Seven. Three. Nine. One.
    Five. Eight. Two. Four. Six. Zero." [Repeats for 10 minutes]

42. "Alpha. Nine. Nine. Bravo. Seven. Three. Charlie. One. Five. Delta. Eight. Two. Echo.
    Four. Six. Foxtrot. Zero. Seven. Golf. Three. Hotel. Nine." [Encoded transmission]

43. "Niner. Niner. Niner. Break. Break. Break. This is Station Echo. Coordinates:
    Four-Seven. Eight-Two. Supply cache confirmed. Repeat: supply cache confirmed."

44. "One. One. Two. Three. Five. Eight. Thirteen. Twenty-one. Thirty-four. Fifty-five.
    Eighty-nine. One-forty-four." [Fibonacci sequence, possibly encoded coordinates]

45. "Mike. Romeo. Kilo. Seven. Seven. Three. Lima. November. Papa. Two. Two. Nine.
    Oscar. Quebec. Romeo. Five. Five. One." [NATO phonetic alphabet, encoded message]

### Survivor Broadcasts (Scattered Frequencies)
46. "Is anyone out there? This is... this is Maria. I'm in the old subway station on 5th
    street. I have water, but I need medicine. My daughter is sick. Please. If you can
    hear this..."

47. "Warning to all travelers. The highway north of the city is a death trap. Radiation
    levels off the charts. I lost two people there. Don't go north. Repeat: don't go north."

48. "This is the Westside Collective. We have established a safe zone in the old library
    building. We have food, water, medical supplies. All survivors welcome. Frequency 91.3
    for coordination."

49. "Day 47. Still alive. Found an unlooted pharmacy in sector 6. Lots of medical supplies.
    But the radiation... my dosimeter won't stop clicking. I don't know how much longer..."

50. "To whoever is broadcasting on 102.1: We know you're there. We've been listening.
    If you're military, we need help. If you're not... who's running this frequency?"

---

## Usage Notes

- **Intel Extraction:** When the player tunes to a frequency and successfully extracts intel,
  the RadioTunerSystem selects a random broadcast from that frequency's pool (see
  RadioFrequencySO.broadcasts).

- **Intel Types:**
  - Civilian frequencies (pre-war): Flavor/narrative only
  - Military frequencies (Day 1-30): MortarWarning, TroopMovement (gameplay intel)
  - Emergency frequencies (post-Day 30): PlumeReport (radiation data for map)
  - Numbers stations: Unknown/encoded (low-value intel, narrative flavor)

- **Confidence Levels:**
  - Pre-war broadcasts: 0.7-0.9 (reliable)
  - Day 1-30 emergency: 0.5-0.7 (moderate)
  - Post-Day 30 automated: 0.3-0.5 (low confidence, degraded intel)

- **Expiration:** Intel expires after 5 days. Plume Reports update the RadiationKnowledgeMap
  but with high uncertainty, reflecting the degraded post-war information environment.

---

## Tone Guidelines

These broadcasts should feel:
- **Human:** Real people in desperate situations, not dramatic monologues
- **Restrained:** No melodrama, no preaching, no moralizing
- **Specific:** Concrete details (grid references, times, quantities) ground the fiction
- **Fragmentary:** Post-war broadcasts are degraded, corrupted, incomplete
- **Cold:** The apocalypse is not romantic. It's exhausting, bureaucratic, and ugly

Avoid:
- Heroic speeches or inspirational messages
- Vague platitudes ("we will survive," "hope remains")
- Real country names, real wars, real people
- Fantasy or sci-fi elements (no mutants, no magic, no aliens)


---

## CHAPTER 6: USER INTERFACE & DIEGETIC HUD

### 6.1 UI Toolkit Architecture
- **Master Controller:** `HUD.cs` binds UI View Controllers to system events.
- **Widgets:**
  - `NeedsBar.cs`: Displays Survivor hunger, thirst, fatigue, warmth, morale, health.
  - `DosimeterHUD.cs`: Visual needle gauge and digital mSv counter.
  - `EventModalUI.cs`: Narrative decision window with choice buttons and skill odds.
  - `MapScreenUI.cs`: 2D node map navigation and expedition planner.
  - `TradeScreenUI.cs`: Dual-column barter interface calculating trade values.
  - `WorkbenchUI.cs`: Recipe selection, ingredient validation, crafting progress bar.
  - `JournalBookUI.cs` & `MoralChronicleUI.cs`: Story history, survivor logs, and moral records.
  - `PowerGridHUD.cs`: Shelter power generation vs consumption graph.
  - `RoomAssignmentHUD.cs`: Drag-and-drop survivor bed/workstation assignment.
  - `DiegeticHudController.cs` & `InternalHorrorHUD.cs`: Emergency overlays for shelter fires, corpse disposal, and hatch breaches.

### 6.2 Diegetic Audio Integration
- `GeigerAudioHook.cs`: Converts ambient rad dose into dynamic audio click frequency.
- `FactionRadioVoHook.cs`: Triggers static bursts, numbers station tones, and radio chatter.

---

## CHAPTER 7: CURRENT DEVELOPMENT PROGRESS & TEST STATUS

### 7.1 Implemented & Verified Systems
- [x] Core Session Lifecycle (`GameState`, `TimeSystem`, `EventBus`).
- [x] Persistence Layer (`SaveSystem` multi-slot JSON serialization).
- [x] Survivor Needs Decay & Psychological Thresholds.
- [x] Radiation Dosage, Iodine Prophylaxis, and Dosimeter Audio/Visuals.
- [x] Comprehensive Medical Pathologies (Blood Transfusions, Scurvy, Amputation).
- [x] Weather Engine (16 Weather Kinds & Temperature Simulation).
- [x] Shelter Subsystems (Power Grid, Air Scrubbers, Material Shielding, Hatch Defense).
- [x] Utility AI Autonomous Decision System.
- [x] Authoring Data Pipeline (JSON -> ScriptableObjects -> Runtime Catalogs).
- [x] Main Menu & In-Game UI Toolkit HUD Bindings.

### 7.2 Automated Test Suite Results
- **EditMode Suite (`AtomicWar.Tests.EditMode`):** GREEN (All pure C# logic tests passing).
- **PlayMode Suite (`AtomicWar.Tests.PlayMode`):** GREEN (Frame-by-frame Integration and `GameplaySceneSmokeTests` passing).

---

## CHAPTER 8: BRAINSTORMING PROMPTS FOR OTHER LLMS

Copy any of the prompts below into **Qwen**, **Gemini**, **ChatGPT**, **Claude**, **Perplexity**, or **Vibe Le Chat** alongside this file:

### Prompt A: New Scavenging Encounters & Location Expansion
```
Based on the ASHFALL master document above, design 5 new scavenging locations (following the LocationDefinitionSO schema) and 10 narrative event encounters. Maintain the cold, exhausted, non-fantasy post-nuclear tone. Output raw JSON format compatible with events.json and locations.json.
```

### Prompt B: Deepening Social & Faction Dynamics
```
Using the FactionSO, BunkerSocialSystems, and EventRunner architecture described in Chapter 3 & 4 of the ASHFALL document, propose a complete sub-system design for 'Faction Favors & Betrayals'. Detail the C# system class, state serialization, and 4 multi-stage questlines.
```

### Prompt C: Advanced Tactical Hatch Defense & Raid Mechanics
```
Review the HatchDefenseSystem and Shelter Infrastructure in Chapter 4. Design an expanded 2D tactical hatch defense mini-game concept that integrates perimeter traps, night vision gear, weapon maintenance, and survivor combat perks without breaking the thin MonoBehaviour / C# systems architecture.
```

### Prompt D: Balance & Scarcity Tuning
```
Examine the 321 items and 16 recipes in Chapter 5. Analyze the resource conversion loops (water purification, filter crafting, medical chelation). Identify potential exploit loops and provide a re-balanced JSON trade value and recipe ingredient matrix for Hardcore Survival Mode.
```