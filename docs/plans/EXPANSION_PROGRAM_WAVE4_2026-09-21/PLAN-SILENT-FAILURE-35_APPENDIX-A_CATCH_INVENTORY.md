# PLAN-SILENT-FAILURE-35 — Appendix A: Catch Site Inventory

**Generated:** 2026-09-21 across `Assets/Ashfall.Core/**` + `src/**`
(692 catch sites).
**Classification:** `SWALLOW_EMPTY` (empty body) · `LOG_AND_CONTINUE` (logs
and continues) · `PROPAGATE` (throw/return) · `OTHER` (needs review).
**Use:** SF-35A/35B — every `SWALLOW_EMPTY` and `OTHER` row needs a typed
result, an inventory entry with a reason, or a conversion; `LOG_AND_CONTINUE`
rows need a typed error at the boundary.

## Inventory

| File:line | Catch type | Class | Body (first 80 chars) |
|---|---|---|---|
| `Assets/Ashfall.Core/ApprenticeshipSystem.cs:159` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[Apprentice] Failed to load mentorship catalog: {ex.Message` |
| `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs:146` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[ArchaeologySystem] Failed to load catalog from {path` |
| `Assets/Ashfall.Core/Audio/AudioSettingsCodec.cs:42` | bare | OTHER | `// Fall through to resilient element-by-element parsing` |
| `Assets/Ashfall.Core/Audio/AudioSettingsCodec.cs:95` | Exception | PROPAGATE | `return (new AudioSettingsData(), $"[AudioSettingsCodec] Malformed JSON syntax ({` |
| `Assets/Ashfall.Core/Audio/CassetteSetCatalogLoader.cs:65` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "CassetteSetDefinition list", ex_CATDIAG);
       ` |
| `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs:235` | Exception | OTHER | `anyFailure = true;
                        if (failClosed)
                     ` |
| `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs:268` | Exception | OTHER | `anyFailure = true;
                        report = new DayOwnerReport(reg.Owner` |
| `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs:295` | Exception | OTHER | `if (failClosed)
                        {
                            _pendingRe` |
| `Assets/Ashfall.Core/Campaign/CampaignDaySave.cs:59` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "CampaignDaySave: malfo` |
| `Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs:132` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "DailyBriefingSave: mal` |
| `Assets/Ashfall.Core/CatalogFileSystem.cs:33` | Exception | OTHER | `// Enumeration is an optional adapter seam, but an I/O failure
                /` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:618` | Exception | PROPAGATE | `report.Error("cannot enumerate data directory: " + e.Message);
                r` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:701` | Exception | OTHER | `report.Error("patrol encounter validator error: " + ex.Message);` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:748` | Exception | OTHER | `report.Error("weather route gate validator error: " + ex.Message);` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:767` | Exception | OTHER | `report.Error("shelter shielding validator error: " + ex.Message);` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:787` | Exception | OTHER | `report.Error("weather effects validator error: " + ex.Message);` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:889` | Exception | OTHER | `report.Error($"{entryPath` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:915` | Exception | OTHER | `report.Error("narrative discovery manifest validator error: " + ex.Message);` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:982` | Exception | OTHER | `report.Error(Difficulty.DifficultyPresetCatalogLoader.FileName + ": " + ex.Messa` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:1047` | Exception | OTHER | `// Resilient probe: the generic catalog pass owns the detailed items.json
      ` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:1160` | JsonException | OTHER | `report.Error(fileName + ": malformed JSON: " + ex.Message);` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:1164` | Exception | OTHER | `report.Error(fileName + ": validator error: " + ex.Message);` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:1423` | Exception | OTHER | `report.Error("duty_roles.json: validator error: " + ex.Message);` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:1551` | JsonException | PROPAGATE | `report.Error("catalog '" + path + "': malformed JSON (" + ex.GetType().Name + ")` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:1556` | IOException | PROPAGATE | `report.Error("catalog '" + path + "': I/O failure (" + ex.GetType().Name + "): "` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:1561` | Exception | PROPAGATE | `report.Error("catalog '" + path + "': catalog read failure (" + ex.GetType().Nam` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:1910` | Exception | PROPAGATE | `report.Error("Plan 144 duplicate-quest gate could not read '" + fullPath
       ` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:2466` | Exception | OTHER | `report.Error($"{fileName` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:2542` | Exception | OTHER | `report.Error("economy geography validator could not enumerate economy_goods.json` |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:3134` | Exception | OTHER | `report.Error("wildlife trapping catalog validator error: " + ex.Message);` |
| `Assets/Ashfall.Core/Codex/CodexEntryCatalog.cs:127` | Exception | PROPAGATE | `Ashfall.Core.IO.CatalogDiagnostics.Warn(
                    pathForDiagnostics ` |
| `Assets/Ashfall.Core/CohortTuning.cs:31` | bare | PROPAGATE | `return Default;` |
| `Assets/Ashfall.Core/CollectibleCatalog.cs:58` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "CollectibleCatalogFileRaw", ex);
                ` |
| `Assets/Ashfall.Core/Combat/BallisticShieldEngine.cs:95` | Exception | LOG_AND_CONTINUE | `log?.Error($"[BallisticShield] failed loading catalog: {ex.Message` |
| `Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs:565` | Exception | PROPAGATE | `throw new InvalidOperationException($"Failed to load {FileName` |
| `Assets/Ashfall.Core/Combat/CombatCatalog.cs:635` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "FactionIdAuthority", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Commitments/CommitmentCatalogLoader.cs:43` | Exception | OTHER | `result.Errors.Add($"CommitmentCatalogLoader: Failed to read {FileName` |
| `Assets/Ashfall.Core/Commitments/CommitmentCatalogLoader.cs:54` | Exception | OTHER | `result.Errors.Add($"CommitmentCatalogLoader: JSON deserialize failure in {FileNa` |
| `Assets/Ashfall.Core/Communications/CommunicationsSystem.cs:210` | bare | OTHER | `LoadEmbeddedDefaults();` |
| `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs:95` | Exception | LOG_AND_CONTINUE | `log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse collectibles.j` |
| `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs:129` | Exception | LOG_AND_CONTINUE | `log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse items.json: {e` |
| `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs:156` | Exception | LOG_AND_CONTINUE | `log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse research_knowl` |
| `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs:178` | Exception | LOG_AND_CONTINUE | `log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse wasteland_map_` |
| `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs:200` | Exception | LOG_AND_CONTINUE | `log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse journal_voice_` |
| `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs:245` | Exception | LOG_AND_CONTINUE | `log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse scavenging_tab` |
| `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs:267` | bare | OTHER | `/* best-effort: probe-only text scan, tolerate unreadable files */` |
| `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1787` | Exception | LOG_AND_CONTINUE | `_log?.Warn($"[CountDefinitions] Failed to read {cat.Path` |
| `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1800` | Exception | LOG_AND_CONTINUE | `_log?.Warn($"[CountDefinitions] Invalid root shape in {cat.Path` |
| `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1989` | bare | OTHER | `/* cleanup: skip unreadable core files */` |
| `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1994` | bare | OTHER | `/* cleanup: skip unreadable src files */` |
| `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1998` | Exception | LOG_AND_CONTINUE | `_log?.Warn($"[VerifyConsumers] Failed to read source: {ex.Message` |
| `Assets/Ashfall.Core/Cooking/CookingSystem.cs:184` | bare | OTHER | `// Catalog parse fallback` |
| `Assets/Ashfall.Core/Crafting/PharmaRecipeCatalogLoader.cs:97` | Exception | OTHER | `CatalogDiagnostics.Warn("PharmaRecipeCatalogLoader", FileName, ex);
            ` |
| `Assets/Ashfall.Core/Crafting/RecipeCatalogLoader.cs:169` | Exception | OTHER | `CatalogDiagnostics.Warn("RecipeCatalogLoader", FileName, ex);
                re` |
| `Assets/Ashfall.Core/Crafting/RelicCatalogLoader.cs:99` | Exception | OTHER | `CatalogDiagnostics.Warn("RelicCatalogLoader", FileName, ex);
                res` |
| `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs:482` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "CrossingQuestDef list", ex_CATDIAG);
            ` |
| `Assets/Ashfall.Core/CrossingCatalog.cs:219` | Exception | LOG_AND_CONTINUE | `_log.Error("Crossing encounters parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/CrossingCatalog.cs:256` | Exception | LOG_AND_CONTINUE | `_log.Error("Crossing " + label + " parse failed: " + e.Message);
               ` |
| `Assets/Ashfall.Core/CryogenicAirSeparationSystem.cs:267` | Exception | LOG_AND_CONTINUE | `(log ?? NullLog.Instance).Warn($"[CryogenicAirSeparationCatalog] failed to load ` |
| `Assets/Ashfall.Core/Culture/DocumentationSystem.cs:194` | bare | OTHER | `// Fallback` |
| `Assets/Ashfall.Core/Culture/ShelterMuseumSystem.cs:188` | bare | OTHER | `// Fallback` |
| `Assets/Ashfall.Core/DebtTemplateCatalog.cs:108` | Exception | PROPAGATE | `catalog.Errors.Add(DebtTemplateCatalog.FileName + " parse failed: " + e.Message)` |
| `Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs:361` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "item catalog probe", ex_CATDIAG);
               ` |
| `Assets/Ashfall.Core/Defense/PerimeterDefenseCatalog.cs:144` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "PerimeterDefensesContainer", ex);
               ` |
| `Assets/Ashfall.Core/Difficulty/DifficultyPresetCatalog.cs:251` | JsonException | PROPAGATE | `throw new InvalidOperationException("difficulty catalog is malformed: " + ex.Mes` |
| `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs:467` | Exception | PROPAGATE | `catalog.Errors.Add("disease_catalog.json parse failed: " + e.Message);
         ` |
| `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs:467` | Exception | LOG_AND_CONTINUE | `System.Console.Error.WriteLine("[DiseaseHeadlessDemo] items.json read failed: " ` |
| `Assets/Ashfall.Core/Disease/PathogenStrainSave.cs:73` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("<decode>", "PathogenStrainSaveState", ex_CATDIAG);
    ` |
| `Assets/Ashfall.Core/DoseContentCatalog.cs:128` | Exception | OTHER | `CatalogDiagnostics.Warn(locPath, "DoseLocationsRoot", ex_CATDIAG);` |
| `Assets/Ashfall.Core/DoseContentCatalog.cs:146` | Exception | OTHER | `CatalogDiagnostics.Warn(itemPath, "DoseItemsRoot", ex_CATDIAG);` |
| `Assets/Ashfall.Core/DoseContentCatalog.cs:168` | Exception | OTHER | `CatalogDiagnostics.Warn(questPath, "DoseQuestDef list", ex_CATDIAG);` |
| `Assets/Ashfall.Core/DoseLedgerSave.cs:101` | Exception | PROPAGATE | `throw new InvalidOperationException("DoseLedgerSave: malformed save payload: " +` |
| `Assets/Ashfall.Core/DoseRegistersCatalog.cs:86` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "DoseRegistersCatalog", ex_CATDIAG);
             ` |
| `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs:220` | Exception | LOG_AND_CONTINUE | `_log.Error("Duty Roster " + label + " parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/DutyRoster/DutyRosterSave.cs:158` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "DutyRosterSave: malfor` |
| `Assets/Ashfall.Core/DutyRoster/DutyRosterSave.cs:169` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "DutyRosterSave: malfor` |
| `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs:637` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Ecology/EcologicalInfestationCatalog.cs:31` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "EcologicalInfestationFileRaw", ex);
             ` |
| `Assets/Ashfall.Core/Economy/BlackMarketInventoryCatalog.cs:132` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs:481` | bare | PROPAGATE | `line.quantity = previousQuantity;
                throw;` |
| `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs:526` | bare | PROPAGATE | `RollbackSellLine(syndicateId, line, createdLine, previousQuantity);
            ` |
| `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs:613` | bare | PROPAGATE | `_state.debts.Remove(debt);
                ledger.trust = previousTrust;
       ` |
| `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs:684` | bare | PROPAGATE | `debt.repaidUnits = previousRepaid;
                debt.status = previousStatus;` |
| `Assets/Ashfall.Core/Economy/CaravanCatalogLoader.cs:63` | bare | OTHER | `// Fallback to compiled defaults` |
| `Assets/Ashfall.Core/Economy/CaravanTradeRouteCatalog.cs:60` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "CaravanTradeRoutesContainer", ex);
              ` |
| `Assets/Ashfall.Core/Economy/CommodityBaselineCatalog.cs:109` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Economy/GoodsCatalog.cs:180` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Economy/HardcoreEconomyTuningLoader.cs:41` | JsonException | PROPAGATE | `return HardcoreEconomyTuningLoadResult.Failure(new[] { $"JSON parse error: {ex.M` |
| `Assets/Ashfall.Core/Economy/MercenarySystem.cs:153` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[MercenarySystem] Failed to load catalog from {path` |
| `Assets/Ashfall.Core/Economy/RegionalPriceAtlas.cs:238` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs:187` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[ShelterBarter] Failed to parse caravan catalog: {ex.Message` |
| `Assets/Ashfall.Core/Economy/TradeEmbargoSystem.cs:602` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Economy/TradeTextCatalog.cs:185` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(FileName, "trade_texts root object", ex);
              ` |
| `Assets/Ashfall.Core/Economy/TradeTextCatalog.cs:202` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(FileName, "trade_texts root object", ex);
              ` |
| `Assets/Ashfall.Core/Education/SurvivorEducationSystem.cs:255` | bare | OTHER | `LoadEmbeddedDefaults();` |
| `Assets/Ashfall.Core/Endgame/CampaignCompletionHistory.cs:388` | Exception | PROPAGATE | `error = "Invalid completion-history JSON (" + ex.Message + ").";
               ` |
| `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs:66` | Exception | PROPAGATE | `Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "EpilogueChronicleCatalogData", ex` |
| `Assets/Ashfall.Core/EquipmentConditionSystem.cs:262` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[EquipmentCondition] Failed to load degradation catalog: {ex.Message` |
| `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs:78` | bare | OTHER | `// Fallback to built-in defaults if JSON parse fails` |
| `Assets/Ashfall.Core/ExpansionEnrichmentCatalog.cs:324` | Exception | LOG_AND_CONTINUE | `_log.Warn("Parse failed " + path + ": " + ex.Message);` |
| `Assets/Ashfall.Core/ExpansionEnrichmentCatalog.cs:343` | Exception | LOG_AND_CONTINUE | `_log.Warn("Parse failed " + path + ": " + ex.Message);` |
| `Assets/Ashfall.Core/ExpansionHubSave.cs:391` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "ExpansionHubSave: malf` |
| `Assets/Ashfall.Core/ExpansionHubSave.cs:402` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "ExpansionHubSave: malf` |
| `Assets/Ashfall.Core/ExpansionQuestSave.cs:50` | Exception | PROPAGATE | `throw new InvalidOperationException("ExpansionQuestSave: malformed save payload:` |
| `Assets/Ashfall.Core/ExpansionQuestSystem.cs:373` | Exception | OTHER | `CatalogDiagnostics.Warn(file, "unknown", ex);` |
| `Assets/Ashfall.Core/Expeditions/AviationSystem.cs:145` | bare | OTHER | `// Graceful fallback` |
| `Assets/Ashfall.Core/Expeditions/ColonySystem.cs:185` | bare | OTHER | `// Catalog parsing fallback` |
| `Assets/Ashfall.Core/Expeditions/DraisineRerailingSystem.cs:82` | Exception | LOG_AND_CONTINUE | `log?.Error($"[DraisineRecovery] failed loading catalog: {ex.Message` |
| `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs:59` | bare | PROPAGATE | `// A corrupt catalog must never block the game — empty garage.
                r` |
| `Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs:123` | Exception | OTHER | `CatalogDiagnostics.Warn("ExpeditionCatalogLoader", path, ex);` |
| `Assets/Ashfall.Core/Expeditions/ExpeditionLootValidator.cs:90` | bare | OTHER | `// best-effort` |
| `Assets/Ashfall.Core/Expeditions/ExpeditionNavalSystem.cs:135` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[NavalSystem] Failed to load naval vessels catalog: {ex.Message` |
| `Assets/Ashfall.Core/Expeditions/RadarEcmCatalog.cs:92` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "radar_ecm_catalog", e); return null;` |
| `Assets/Ashfall.Core/Expeditions/ReconTelemetryCatalogLoader.cs:27` | Exception | PROPAGATE | `throw new InvalidOperationException($"Failed to load {CatalogFileName` |
| `Assets/Ashfall.Core/Expeditions/VehicleArmorGradeCatalog.cs:117` | Exception | PROPAGATE | `result.Errors.Add("vehicle_armor_grades.json: malformed JSON: " + ex.Message);
 ` |
| `Assets/Ashfall.Core/Expeditions/VerticalAscentCatalog.cs:98` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "climbing_winch_catalog", ex);
                ret` |
| `Assets/Ashfall.Core/Exploration/CartographySystem.cs:171` | bare | OTHER | `// Catalog parse fallback` |
| `Assets/Ashfall.Core/Factions/EspionageSystem.cs:109` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "EspionageMissionCatalogLoader", ex);` |
| `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:358` | Exception | LOG_AND_CONTINUE | `_log.Error($"FactionBranchCoordinator commit error: {ex.Message` |
| `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:391` | Exception | PROPAGATE | `return ActionResult.Failed("ponr_error", ex.Message);` |
| `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:415` | Exception | PROPAGATE | `return ActionResult.Failed("ending_error", ex.Message);` |
| `Assets/Ashfall.Core/Factions/FactionDisplayNameCatalog.cs:50` | bare | OTHER | `// Silent fallback — humanization will be used instead` |
| `Assets/Ashfall.Core/Factions/ForcedLaborSystem.cs:118` | bare | OTHER | `// Graceful fallback` |
| `Assets/Ashfall.Core/Factions/InfiltratorCatalogLoader.cs:27` | Exception | PROPAGATE | `throw new InvalidOperationException($"Failed to load {CatalogFileName` |
| `Assets/Ashfall.Core/Farming/CropStrainCatalog.cs:149` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "crop strain catalog", ex);
                return` |
| `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs:37` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "<root>", ex);` |
| `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs:52` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "<root>", ex);` |
| `Assets/Ashfall.Core/Foundry/GlassworksCatalog.cs:157` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "GlassworksRecipesFile", ex);
                cata` |
| `Assets/Ashfall.Core/Foundry/MaterialProfileCatalog.cs:121` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Foundry/MetallurgyHeavyCatalog.cs:169` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "MetallurgyRecipesFile", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Foundry/PowderMetallurgySystem.cs:126` | Exception | LOG_AND_CONTINUE | `log?.Error($"[PowderMetallurgy] failed loading catalog: {ex.Message` |
| `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs:156` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "RegionalTreatiesFile", ex_CATDIAG);
             ` |
| `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs:179` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "FoundryProductionFile", ex_CATDIAG);
            ` |
| `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs:201` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "FoundryFactionEntry", ex_CATDIAG);
              ` |
| `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs:141` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "FoundryTreatyConsequenceFile", ex_CATDIAG);
     ` |
| `Assets/Ashfall.Core/GenerationalLineageExtension.cs:104` | bare | OTHER | `// Fallback` |
| `Assets/Ashfall.Core/Governance/ShelterGovernanceEngine.cs:156` | bare | OTHER | `// Fallback to defaults if catalog parse fails
                RegisterDefaultBl` |
| `Assets/Ashfall.Core/GrainProcessingSystem.cs:360` | Exception | LOG_AND_CONTINUE | `(log ?? NullLog.Instance).Warn($"[GrainProcessingCatalog] failed to load {path` |
| `Assets/Ashfall.Core/HeliographSystem.cs:263` | Exception | LOG_AND_CONTINUE | `(log ?? NullLog.Instance).Warn($"[HeliographCatalog] failed to load {path` |
| `Assets/Ashfall.Core/HoldfastCatalog.cs:211` | Exception | LOG_AND_CONTINUE | `_log.Error("Holdfast locations parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/HoldfastCatalog.cs:235` | Exception | LOG_AND_CONTINUE | `_log.Error("Holdfast " + label + " parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/HoldfastCatalog.cs:271` | Exception | LOG_AND_CONTINUE | `_log.Error("Holdfast " + label + " parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/HoldfastCatalog.cs:307` | Exception | LOG_AND_CONTINUE | `_log.Error("Holdfast " + label + " parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/HoldfastSave.cs:246` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "HoldfastSave: malforme` |
| `Assets/Ashfall.Core/HoldfastTradeSession.cs:482` | bare | PROPAGATE | `_value = previousValue;
                throw;` |
| `Assets/Ashfall.Core/HoldfastTradeSession.cs:656` | Exception | OTHER | `Inventory.RemoveItem(canonical, quantity);
                _value = prevValue;
 ` |
| `Assets/Ashfall.Core/HostDefaults.cs:43` | bare | PROPAGATE | `return new string[0];` |
| `Assets/Ashfall.Core/IO/CatalogBootValidator.cs:134` | Exception | OTHER | `CatalogDiagnostics.Warn(entry.DisplayName, entry.FileName, ex);
                ` |
| `Assets/Ashfall.Core/IO/CatalogBootValidator.cs:171` | Exception | OTHER | `CatalogDiagnostics.Warn(entry.DisplayName, entry.FileName, ex);
                ` |
| `Assets/Ashfall.Core/IO/CatalogDiagnostics.cs:6` | bare | SWALLOW_EMPTY | `` |
| `Assets/Ashfall.Core/IO/CatalogDiagnostics.cs:62` | bare | PROPAGATE | `// Last-resort swallow: if the sink itself throws, we
                // preserv` |
| `Assets/Ashfall.Core/IO/CatalogKeyNormalizer.cs:71` | JsonException | LOG_AND_CONTINUE | `// Malformed input is passed through so the typed loader surfaces
              ` |
| `Assets/Ashfall.Core/IO/CatalogLoadResult.cs:286` | Exception | OTHER | `CatalogDiagnostics.Warn(schema, filePath, ex);
                var severity = cl` |
| `Assets/Ashfall.Core/IO/CatalogLoadResult.cs:338` | Exception | OTHER | `CatalogDiagnostics.Warn(schema, filePath, ex);
                var severity = cl` |
| `Assets/Ashfall.Core/Inventory/Inventory.cs:1167` | bare | PROPAGATE | `Add(item, 1);
                throw;` |
| `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs:250` | Exception | OTHER | `CatalogDiagnostics.Warn("ItemCatalogLoader", "<root>", ex);
                resu` |
| `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs:326` | Exception | OTHER | `CatalogDiagnostics.Warn("ItemCatalogLoader", StartingSuppliesFileName, ex);
    ` |
| `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs:584` | Exception | OTHER | `CatalogDiagnostics.Warn("ItemCatalogLoader", path, ex);` |
| `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs:639` | Exception | OTHER | `CatalogDiagnostics.Warn("ItemCatalogLoader", "LoadItemsFile", ex);
             ` |
| `Assets/Ashfall.Core/Inventory/ItemDescriptionCatalogLoader.cs:94` | Exception | OTHER | `CatalogDiagnostics.Warn(fullPath, "ItemDescriptionTextsJsonRoot", ex);
         ` |
| `Assets/Ashfall.Core/Journal/JournalCorpus.cs:229` | Exception | PROPAGATE | `throw new JournalCorpusFormatException(
                    $"Could not parse au` |
| `Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:118` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(ProseFile, "<root>", ex);
                return Empty;` |
| `Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs:110` | bare | OTHER | `// Catalog parse fallback` |
| `Assets/Ashfall.Core/Maritime/DeepLoreLocationCatalogLoader.cs:74` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "DeepLoreLocationContainer", ex_CATDIAG);
        ` |
| `Assets/Ashfall.Core/Maritime/DiveSiteCatalog.cs:117` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "DiveSiteContainer", ex_CATDIAG);
                ` |
| `Assets/Ashfall.Core/Medical/BionicsSystem.cs:701` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Medical/LyophilizationSystem.cs:95` | Exception | LOG_AND_CONTINUE | `log?.Error($"[Lyophilization] failed loading catalog: {ex.Message` |
| `Assets/Ashfall.Core/Medical/MedicalTextCatalog.cs:111` | Exception | PROPAGATE | `// Non-fatal catalog loading failure: returns empty catalog
                Ashf` |
| `Assets/Ashfall.Core/Medical/MedicalWardSave.cs:57` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "MedicalWardSave: malfo` |
| `Assets/Ashfall.Core/Medical/MutationSystem.cs:245` | Exception | LOG_AND_CONTINUE | `_log.Warn($"Failed to deserialize MutationCatalog: {ex.Message` |
| `Assets/Ashfall.Core/Medical/NarcoticsSystem.cs:124` | bare | OTHER | `// Graceful fallback` |
| `Assets/Ashfall.Core/Medical/SurgicalProcedureCatalog.cs:57` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "SurgicalProceduresContainer", ex);
              ` |
| `Assets/Ashfall.Core/Memorial/GraveEpitaphCatalog.cs:61` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "WastelandGraveEpitaphEntry list", ex_CATDIAG);
  ` |
| `Assets/Ashfall.Core/Mods/JsonModLayering.cs:203` | Exception | OTHER | `rejected.Add(directoryName);
                    diagnostics.Add(new ModDiagnost` |
| `Assets/Ashfall.Core/Mods/JsonModLayering.cs:493` | Exception | PROPAGATE | `error = "invalid JSON: " + ex.Message;
                rejectionCode = ModReject` |
| `Assets/Ashfall.Core/Mods/JsonModLayering.cs:605` | bare | PROPAGATE | `return null;` |
| `Assets/Ashfall.Core/Narrative/BunkerCourtCatalog.cs:132` | Exception | OTHER | `CatalogDiagnostics.Warn(canonicalPath, "BunkerCourtCatalog", ex);` |
| `Assets/Ashfall.Core/Narrative/BunkerGraffitiCatalog.cs:116` | Exception | OTHER | `CatalogDiagnostics.Warn(canonicalPath, "BunkerGraffitiCatalog", ex);` |
| `Assets/Ashfall.Core/Narrative/BunkerGraffitiCatalog.cs:129` | Exception | OTHER | `CatalogDiagnostics.Warn(expansionPath, "GraffitiExpansionFile", ex);` |
| `Assets/Ashfall.Core/Narrative/BunkerMaintenanceCatalog.cs:90` | Exception | OTHER | `CatalogDiagnostics.Warn(canonicalPath, "BunkerMaintenanceCatalog", ex);` |
| `Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs:197` | Exception | OTHER | `result.Errors.Add($"bureaucratic document catalog parse failed: {ex.Message` |
| `Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs:334` | Exception | OTHER | `result.Errors.Add($"bureaucratic document runtime map parse failed: {ex.Message` |
| `Assets/Ashfall.Core/Narrative/Continuity/NarrativeContinuityEngine.cs:106` | JsonException | OTHER | `report.Findings.Add(new NarrativeFinding
                    {
                 ` |
| `Assets/Ashfall.Core/Narrative/ContrabandCatalogValidator.cs:103` | JsonException | PROPAGATE | `// NaN/Infinity literals and other malformed numbers are rejected at parse level` |
| `Assets/Ashfall.Core/Narrative/DwellerMedicalCatalog.cs:77` | Exception | OTHER | `Ashfall.Core.IO.CatalogDiagnostics.Warn(canonicalPath, "DwellerMedicalCasebookFi` |
| `Assets/Ashfall.Core/Narrative/DwellerMedicalCatalog.cs:90` | Exception | OTHER | `Ashfall.Core.IO.CatalogDiagnostics.Warn(expansionPath, "MedicalDocumentsExpansio` |
| `Assets/Ashfall.Core/Narrative/EchoCatalog.cs:172` | Exception | PROPAGATE | `result.Errors.Add("catalog read failed: " + ex.Message);
                return ` |
| `Assets/Ashfall.Core/Narrative/EchoCatalog.cs:183` | Exception | PROPAGATE | `result.Errors.Add("catalog JSON parse failed: " + ex.Message);
                r` |
| `Assets/Ashfall.Core/Narrative/HoldfastNpcCatalog.cs:107` | Exception | OTHER | `Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "HoldfastNpcCatalogRoot", ex);` |
| `Assets/Ashfall.Core/Narrative/MicroLocationEncounterLoader.cs:49` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "MicroLocation EncounterDefinition list", ex_CATDI` |
| `Assets/Ashfall.Core/Narrative/NarrativeArcEventSystem.cs:417` | Exception | PROPAGATE | `result.Errors.Add("catalog read failed: " + ex.Message);
                return ` |
| `Assets/Ashfall.Core/Narrative/NarrativeArcEventSystem.cs:428` | Exception | PROPAGATE | `result.Errors.Add("catalog JSON parse failed: " + ex.Message);
                r` |
| `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs:551` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "EncounterDefinition list", ex_CATDIAG);
         ` |
| `Assets/Ashfall.Core/Narrative/NarrativeProgressionCatalogLoader.cs:51` | bare | PROPAGATE | `return new List<NarrativeProgressionEntry>();` |
| `Assets/Ashfall.Core/Narrative/PersonalLetterCatalog.cs:85` | Exception | OTHER | `CatalogDiagnostics.Warn(canonicalPath, "PersonalLetterCatalog", ex);` |
| `Assets/Ashfall.Core/Narrative/PersonalLetterCatalog.cs:98` | Exception | OTHER | `CatalogDiagnostics.Warn(batch2Path, "PersonalLetterCatalog", ex);` |
| `Assets/Ashfall.Core/Narrative/PoliticsSystem.cs:127` | bare | OTHER | `// Graceful fallback` |
| `Assets/Ashfall.Core/Narrative/ProceduralNarrativeSystem.cs:62` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "QuestTemplateCatalogLoader", ex);` |
| `Assets/Ashfall.Core/NpcArcs/NpcArcCatalog.cs:153` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "npc_arcs", ex);` |
| `Assets/Ashfall.Core/Ports.cs:31` | bare | OTHER | `/* cleanup: best-effort directory creation */` |
| `Assets/Ashfall.Core/Ports.cs:37` | bare | OTHER | `/* cleanup: best-effort file deletion */` |
| `Assets/Ashfall.Core/Ports.cs:52` | bare | PROPAGATE | `return new string[0];` |
| `Assets/Ashfall.Core/Ports.cs:66` | bare | PROPAGATE | `return new string[0];` |
| `Assets/Ashfall.Core/Ports.cs:80` | bare | OTHER | `/* cleanup: best-effort directory deletion */` |
| `Assets/Ashfall.Core/QuestlineMasterCatalog.cs:147` | Exception | LOG_AND_CONTINUE | `_log.Warn("Questline master parse failed: " + ex.Message);` |
| `Assets/Ashfall.Core/Radiation/ExposureEnvironment.cs:329` | bare | OTHER | `// Non-fatal parse failure in optional candidate file` |
| `Assets/Ashfall.Core/Radio/AcousticDirectionFindingCatalog.cs:126` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "acoustic_triangulation_catalog", ex);
           ` |
| `Assets/Ashfall.Core/Radio/NvisCommunicationsSystem.cs:62` | Exception | LOG_AND_CONTINUE | `log?.Error($"[NVIS] failed loading catalog: {ex.Message` |
| `Assets/Ashfall.Core/Radio/PsyOpsSave.cs:102` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("<decode>", "PsyOpsSaveState", ex_CATDIAG);
            ` |
| `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs:227` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("faction_radio_corpus.json", "FactionRadioCorpusLoader",` |
| `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs:301` | Exception | OTHER | `CatalogDiagnostics.Warn("radio.json", "BaseRadioLoader", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs:353` | Exception | OTHER | `CatalogDiagnostics.Warn("year_of_ash_radio.json", "YearOfAshRadioLoader", ex_CAT` |
| `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs:398` | Exception | OTHER | `CatalogDiagnostics.Warn("verdict_radio.json", "VerdictRadioLoader", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs:441` | Exception | OTHER | `CatalogDiagnostics.Warn("faction_war_radio.json", "FactionWarRadioLoader", ex_CA` |
| `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs:633` | Exception | OTHER | `CatalogDiagnostics.Warn("<json>", "RadioDistressSystem", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Radio/RadioSave.cs:277` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("<decode>", "RadioSaveState", ex_CATDIAG);
             ` |
| `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs:50` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("<json>", "RadioStationCatalog", ex_CATDIAG);
          ` |
| `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs:66` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "RadioStationCatalog", ex_CATDIAG);
              ` |
| `Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs:174` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[SurvivorDowntime] Failed to load recreation catalog: {ex.Message` |
| `Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs:96` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(DefaultFileName, "ResearchKnowledgeCatalogLoader", ex_CA` |
| `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs:124` | bare | OTHER | `// Catalog parse fallback` |
| `Assets/Ashfall.Core/Research/TechSalvageCatalog.cs:106` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "TechSalvageCatalogLoader", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs:70` | Exception | LOG_AND_CONTINUE | `log?.Error($"[{tag` |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs:115` | Exception | LOG_AND_CONTINUE | `log?.Warn($"[{tag` |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs:147` | Exception | LOG_AND_CONTINUE | `log?.Error($"[{tag` |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs:159` | Exception | LOG_AND_CONTINUE | `log?.Warn($"[{tag` |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs:200` | bare | OTHER | `// Not in standard generic envelope format, try legacy fallback` |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs:240` | bare | OTHER | `// Deserialization failure` |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs:247` | Exception | LOG_AND_CONTINUE | `log?.Error($"[{tag` |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs:313` | Exception | PROPAGATE | `return (false, null, ex.Message);` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:132` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: failed to enumerate profile directory '{profileDir` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:177` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: failed to enumerate saves directory '{savesDir` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:244` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: failed to load authoritative manifest for slot '{sl` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:273` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: failed to load manifest for slot '{slotId` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:513` | Exception | LOG_AND_CONTINUE | `_log.Error($"SaveSlotService: failed to write aggregate envelope for slot '{slot` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:552` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: failed to read aggregate for slot '{slotId` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:573` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: JSON deserialize failed for slot '{slotId` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:597` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: aggregate validation threw for slot '{slotId` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:659` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: legacy aggregate migration failed for slot '{slotId` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:741` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: legacy inspection compatibility path failed for slo` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:861` | Exception | LOG_AND_CONTINUE | `_log.Error($"SaveSlotService: failed to reset slot '{slotId` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:890` | Exception | LOG_AND_CONTINUE | `_log.Error($"SaveSlotService: failed to delete slot '{slotId` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:958` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: failed to read aggregate for terminal peek on slot ` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:972` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: JSON deserialize failed for terminal peek on slot '` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:986` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: validation threw during terminal peek for slot '{sl` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:1016` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: migration failed during terminal peek for slot '{sl` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:1058` | Exception | LOG_AND_CONTINUE | `_log.Warn($"SaveSlotService: terminal manifest projection failed for slot '{slot` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:1167` | Exception | OTHER | `error = $"Legacy import failed: {ex.Message` |
| `Assets/Ashfall.Core/Save/SaveSlotService.cs:1192` | Exception | LOG_AND_CONTINUE | `_log.Error($"SaveSlotService: failed to quarantine corrupt save for slot '{slotI` |
| `Assets/Ashfall.Core/Save/SaveStore.cs:160` | Exception | LOG_AND_CONTINUE | `_log.Error($"[{_logTag` |
| `Assets/Ashfall.Core/Save/SaveStore.cs:193` | Exception | LOG_AND_CONTINUE | `_log.Error($"[{_logTag` |
| `Assets/Ashfall.Core/Save/SaveStore.cs:214` | Exception | LOG_AND_CONTINUE | `_log.Error($"[{_logTag` |
| `Assets/Ashfall.Core/Save/SaveStore.cs:232` | Exception | LOG_AND_CONTINUE | `_log.Error($"[{_logTag` |
| `Assets/Ashfall.Core/Save/SaveStore.cs:252` | Exception | LOG_AND_CONTINUE | `_log.Error($"[{_logTag` |
| `Assets/Ashfall.Core/Save/SaveStore.cs:270` | Exception | LOG_AND_CONTINUE | `_log.Error($"[{_logTag` |
| `Assets/Ashfall.Core/Save/SaveStore.cs:295` | Exception | LOG_AND_CONTINUE | `_log.Error($"[{_logTag` |
| `Assets/Ashfall.Core/Settings/UserSettingsCodec.cs:43` | Exception | PROPAGATE | `return (new UserSettingsData(), $"[UserSettingsCodec] Invalid settings JSON ({ex` |
| `Assets/Ashfall.Core/Shelter/AeroponicsSystem.cs:521` | Exception | PROPAGATE | `throw new InvalidOperationException($"Failed to load {FileName` |
| `Assets/Ashfall.Core/Shelter/CascadeRuleCatalog.cs:64` | Exception | OTHER | `// Loader diagnostic context (catch-policy gate): a cascade
                // c` |
| `Assets/Ashfall.Core/Shelter/CellulosicBiofuelCatalog.cs:104` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "cellulosic_ethanol_catalog", e); return null;` |
| `Assets/Ashfall.Core/Shelter/ChlorAlkaliSynthesisEngine.cs:100` | Exception | LOG_AND_CONTINUE | `log?.Error($"[ChlorAlkali] failed loading catalog: {ex.Message` |
| `Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs:105` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "CryoCultivarsFile", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Shelter/CupolaFoundryCatalog.cs:156` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "cupola_foundry_catalog", ex);
                ret` |
| `Assets/Ashfall.Core/Shelter/DisasterResponseSystem.cs:187` | bare | OTHER | `LoadEmbeddedDefaults();` |
| `Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs:120` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(FileName, "FluidInfrastructureCatalogLoader", ex);
     ` |
| `Assets/Ashfall.Core/Shelter/FogHarvestingCatalog.cs:80` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "fog_harvesting_catalog", e); return null;` |
| `Assets/Ashfall.Core/Shelter/GeothermalCatalogLoader.cs:27` | Exception | PROPAGATE | `throw new InvalidOperationException($"Failed to load {CatalogFileName` |
| `Assets/Ashfall.Core/Shelter/GeothermalOrcSystem.cs:549` | Exception | PROPAGATE | `throw new InvalidOperationException($"Failed to load {FileName` |
| `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs:312` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "ShelterMachineTellCatalog", ex);` |
| `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs:61` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "OrbitalHarrowCatalogFile", ex);` |
| `Assets/Ashfall.Core/Shelter/PneumaticDispatchSystem.cs:621` | Exception | PROPAGATE | `throw new InvalidOperationException($"Failed to load {FileName` |
| `Assets/Ashfall.Core/Shelter/PowerGridSave.cs:60` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "PowerGridSave: malform` |
| `Assets/Ashfall.Core/Shelter/PowerSubgridCatalog.cs:54` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "PowerSubgridNodesContainer", ex);
               ` |
| `Assets/Ashfall.Core/Shelter/PrecisionBroachingCatalog.cs:77` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "precision_broaching_catalog", e); return null;` |
| `Assets/Ashfall.Core/Shelter/PrecisionOpticsEngine.cs:89` | Exception | LOG_AND_CONTINUE | `log?.Error($"[PrecisionOptics] failed loading catalog: {ex.Message` |
| `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs:163` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "ShelterRoomIdentityCatalog", ex);` |
| `Assets/Ashfall.Core/Shelter/SanitationFacilityCatalog.cs:116` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs:130` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[SeismicDynamics] Failed to parse fault catalog: {ex.Message` |
| `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs:125` | bare | OTHER | `// Catalog parse fallback` |
| `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs:57` | Exception | PROPAGATE | `throw new InvalidOperationException(
                    "ShelterAssignmentSave:` |
| `Assets/Ashfall.Core/Shelter/ShelterExpansionSystem.cs:298` | bare | OTHER | `LoadEmbeddedDefaults();` |
| `Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs:101` | bare | OTHER | `RegisterDefaultOrigins();` |
| `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs:77` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "FileRead", ex);
                error = $"{Fi` |
| `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs:89` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "ShelterPowerGridCatalogDef", ex);
           ` |
| `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs:90` | Exception | OTHER | `CatalogDiagnostics.Warn(fullPath, "ShelterRoomCatalogContainer", ex);` |
| `Assets/Ashfall.Core/Shelter/ShelterShieldingModel.cs:79` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("ShelterShieldingCatalog", path, ex);
                ca` |
| `Assets/Ashfall.Core/Shelter/SkyLayerArmorCatalog.cs:70` | Exception | OTHER | `CatalogDiagnostics.Warn(fullPath, "SkyLayerArmorCatalogContainer", ex);` |
| `Assets/Ashfall.Core/Shelter/SolarConcentratorEngine.cs:80` | Exception | LOG_AND_CONTINUE | `log?.Error($"[SolarConcentrator] failed loading catalog: {ex.Message` |
| `Assets/Ashfall.Core/Shelter/TrophySystem.cs:160` | bare | PROPAGATE | `return false;` |
| `Assets/Ashfall.Core/ShelterThermalSystem.cs:254` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[ShelterThermal] Failed to load insulation catalog: {ex.Message` |
| `Assets/Ashfall.Core/ShelterThermalSystem.cs:346` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[ShelterThermal] Failed to load thermal gear: {ex.Message` |
| `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs:160` | Exception | LOG_AND_CONTINUE | `_log.Error("Standing Record layouts parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs:159` | Exception | LOG_AND_CONTINUE | `_log.Error("Standing Record memory parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs:125` | Exception | LOG_AND_CONTINUE | `_log.Error("Standing Record quests parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs:151` | Exception | LOG_AND_CONTINUE | `_log.Error("Standing Record factions parse failed: " + e.Message);` |
| `Assets/Ashfall.Core/Subterranean/SubterraneanSave.cs:78` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("<decode>", "SubterraneanSaveState", ex_CATDIAG);
      ` |
| `Assets/Ashfall.Core/Survivors/DesperationSystem.cs:136` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[DesperationSystem] Failed to load catalog from {path` |
| `Assets/Ashfall.Core/Survivors/FinalWishCatalogLoader.cs:40` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "<root>", ex);` |
| `Assets/Ashfall.Core/Survivors/FinalWishCatalogLoader.cs:54` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "<root>", ex);` |
| `Assets/Ashfall.Core/Survivors/FitnessForDutyModel.cs:749` | Exception | PROPAGATE | `result.Errors.Add(path + ": invalid JSON: " + ex.Message);
                retur` |
| `Assets/Ashfall.Core/Survivors/HobbySystem.cs:153` | bare | OTHER | `EnsureDefaultHobbies();` |
| `Assets/Ashfall.Core/Survivors/MoraleContagionSave.cs:102` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("<decode>", "MoraleContagionSaveState", ex_CATDIAG);
   ` |
| `Assets/Ashfall.Core/Survivors/NeedsPerformanceBridge.cs:173` | bare | OTHER | `// Fall back to defaults on corrupt payload
                s_config = NeedsPerf` |
| `Assets/Ashfall.Core/Survivors/SkillCatalogLoader.cs:104` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(DefaultFileName, "SkillCatalogLoader", ex_CATDIAG);
    ` |
| `Assets/Ashfall.Core/Survivors/StartingCohortCatalog.cs:184` | Exception | OTHER | `result.Errors.Add($"Could not parse {FileName` |
| `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs:287` | Exception | OTHER | `CatalogDiagnostics.Warn("SurvivorCatalog", path, ex);
                result.Add` |
| `Assets/Ashfall.Core/Survivors/SurvivorStartingStateLoader.cs:96` | Exception | OTHER | `CatalogDiagnostics.Warn("SurvivorStartingStateLoader", FileName, ex);
          ` |
| `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs:90` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(DefaultFileName, "TradeSpecialtyCatalogLoader", ex_CATDI` |
| `Assets/Ashfall.Core/Survivors/ZealotrySystem.cs:642` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/Thirdonary/ThirdonaryCatalogLoader.cs:39` | Exception | OTHER | `CatalogDiagnostics.Warn(FileName, "unknown", ex);` |
| `Assets/Ashfall.Core/Thirdonary/ThirdonarySave.cs:37` | Exception | PROPAGATE | `throw new InvalidOperationException("ThirdonarySave: malformed save payload: " +` |
| `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs:115` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "UtilityActionDef list", ex_CATDIAG);
            ` |
| `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:53` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "VerdictLocationEntry list", ex_CATDIAG);
        ` |
| `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:119` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "VerdictItemEntry list", ex_CATDIAG);
            ` |
| `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:160` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "VerdictRadioContainer", ex_CATDIAG);
            ` |
| `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:199` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "VerdictDataContainer", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:222` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "VerdictDataContainer.world_history_ladder", ex_CA` |
| `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs:148` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "VerdictNpcEntry list", ex_CATDIAG);
             ` |
| `Assets/Ashfall.Core/Verdict/VerdictQuestCatalogLoader.cs:56` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "Verdict quest catalog", ex_CATDIAG);
            ` |
| `Assets/Ashfall.Core/Verdict/VerdictSave.cs:167` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("<decode>", "VerdictSave", ex_CATDIAG);
                ` |
| `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs:362` | Exception | OTHER | `CatalogDiagnostics.Warn(factionPath, "FactionTributeProbe", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs:425` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "WarlordCatalogWrapperProbe", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs:446` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "WarlordFactionProbe list", ex_CATDIAG);` |
| `Assets/Ashfall.Core/Waystation/WaystationCatalogLoader.cs:59` | bare | OTHER | `// Fallback to compiled defaults on read error` |
| `Assets/Ashfall.Core/Weather/WeatherGameplayCascadeEngine.cs:116` | bare | OTHER | `// Fallback: templates empty` |
| `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs:315` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "WildlifeTrappingCatalogFileRaw", ex);
           ` |
| `Assets/Ashfall.Core/World/AnomalyHazardCatalog.cs:116` | Exception | PROPAGATE | `result.Errors.Add("catalog malformed JSON: " + e.Message);
                retur` |
| `Assets/Ashfall.Core/World/DamagedMapCatalog.cs:264` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("DamagedMapCatalog", path, ex);
                return (` |
| `Assets/Ashfall.Core/World/FalloutSystem.cs:123` | Exception | LOG_AND_CONTINUE | `_log.Warn($"[FalloutSystem] Failed to load catalog from {path` |
| `Assets/Ashfall.Core/World/RouteRegionTopology.cs:104` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "TopologyEnvelope", ex);` |
| `Assets/Ashfall.Core/World/SeasonalEventCatalog.cs:58` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "SeasonalEventCatalogFile", ex);` |
| `Assets/Ashfall.Core/World/WeatherEffectsCatalog.cs:191` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("WeatherEffectsCatalog", path, ex);
                retu` |
| `Assets/Ashfall.Core/World/WeatherGateCatalogLoader.cs:35` | Exception | OTHER | `catalog.Errors.Add($"weather_route_gates.json: malformed JSON ({ex.GetType().Nam` |
| `Assets/Ashfall.Core/World/WeatherHardeningCatalogLoader.cs:27` | Exception | PROPAGATE | `throw new InvalidOperationException($"Failed to load {CatalogFileName` |
| `Assets/Ashfall.Core/World/WeatherRouteGateCatalog.cs:180` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("WeatherRouteGateCatalog", path, ex);
                re` |
| `Assets/Ashfall.Core/World/WeatherSystem.cs:525` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "SeasonProfileDef", ex_CATDIAG);
                r` |
| `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs:84` | bare | OTHER | `// Fallback to defaults` |
| `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs:46` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "YearOfAshQuestContainer (dynamic questlines)", ex` |
| `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:378` | Exception | LOG_AND_CONTINUE | `_log.Warn("Parse failed " + path + ": " + ex.Message);` |
| `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:392` | Exception | LOG_AND_CONTINUE | `_log.Warn("Parse failed " + path + ": " + ex.Message);` |
| `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:406` | Exception | LOG_AND_CONTINUE | `_log.Warn("Parse failed " + path + ": " + ex.Message);` |
| `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:420` | Exception | LOG_AND_CONTINUE | `_log.Warn("Parse failed " + path + ": " + ex.Message);` |
| `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:434` | Exception | LOG_AND_CONTINUE | `_log.Warn("Parse failed " + path + ": " + ex.Message);` |
| `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:448` | Exception | LOG_AND_CONTINUE | `_log.Warn("Parse failed " + path + ": " + ex.Message);` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:156` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "YearOfAshQuestContainer", ex_CATDIAG);` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:165` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "QuestlineDefinition list", ex_CATDIAG);
         ` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:191` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "YearOfAshItemContainer", ex_CATDIAG);` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:200` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "YearOfAshItemEntry list", ex_CATDIAG);
          ` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:226` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "YearOfAshEventContainer", ex_CATDIAG);` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:235` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "YearOfAshEventEntry list", ex_CATDIAG);
         ` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:302` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "RawQuestContainer", ex_CATDIAG);` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:313` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "YearOfAshQuestContainer", ex_CATDIAG);` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:322` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "QuestlineDefinition list", ex_CATDIAG);
         ` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:348` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "YearOfAshLocationContainer", ex_CATDIAG);` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:357` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "YearOfAshLocationEntry list", ex_CATDIAG);
      ` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:383` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "YearOfAshRadioContainer", ex_CATDIAG);` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:392` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "YearOfAshRadioEntry list", ex_CATDIAG);
         ` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:418` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "YearOfAshSurvivorContainer", ex_CATDIAG);` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:427` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "YearOfAshSurvivorEntry list", ex_CATDIAG);
      ` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshStormCatalog.cs:52` | Exception | PROPAGATE | `CatalogDiagnostics.Warn(path, "StormWindowEntry list", ex_CATDIAG);
            ` |
| `src/Audio/AudioCueCatalog.cs:415` | bare | PROPAGATE | `/* CatalogPath may throw if segment is invalid; fall through */` |
| `src/Audio/AudioCueCatalog.cs:496` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[AudioCueCatalog] Failed to load audio cues from {jsonPath` |
| `src/Audio/AudioManager.cs:156` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[AudioManager] SetupDspEffects warning: {ex.Message` |
| `src/Audio/AudioManager.cs:688` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[AudioManager] Direct stream load failed for {resPath` |
| `src/Audio/AudioManager.cs:764` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[AudioManager] WAV container parsing failed for {osPath` |
| `src/Audio/AudioSelfTest.cs:237` | Exception | LOG_AND_CONTINUE | `distressCatalogsParsed = false;
                    GD.PrintErr($"  [FAIL] Distr` |
| `src/Audio/AudioSelfTest.cs:469` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[AudioSelfTest] Recovery test exception: {ex.Message` |
| `src/Audio/AudioSelfTest.cs:798` | Exception | LOG_AND_CONTINUE | `disposeThrew = true;
                GD.PrintErr($"  [WARN] ShelterOperationsAud` |
| `src/Audio/AudioSettings.cs:176` | Exception | OTHER | `_lastDiagnosticMessage = $"[AudioSettings] Failed to read audio settings from '{` |
| `src/Audio/AudioSettings.cs:201` | Exception | OTHER | `_lastDiagnosticMessage = $"[AudioSettings] Save failed to '{path` |
| `src/Foundry/SilentFoundryHostSession.cs:222` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[SilentFoundry] journal template load failed: " + e.Message);` |
| `src/Foundry/SilentFoundryHostSession.cs:473` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[SilentFoundry] foundry_items.json load failed: " + e.Message);` |
| `src/Host/AssetCoverageScanner.cs:588` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[AssetRegistrySelfTest] Failed to extract IDs from {path` |
| `src/Host/AssetRegistry.cs:1228` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[AssetRegistrySelfTest] Failed to extract IDs from {path` |
| `src/Host/ChemicalDependencyHostSession.cs:67` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[ChemicalDependency] save failed: " + e.Message);` |
| `src/Host/ChemicalDependencyHostSession.cs:81` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[ChemicalDependency] restore failed: " + e.Message);` |
| `src/Host/ChemicalDependencySaveSelfTest.cs:26` | Exception | PROPAGATE | `return $"[FAIL] {ex.GetType().Name` |
| `src/Host/CombatHostSession.cs:277` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Combat] combat_catalog.json load failed, falling back to defaults` |
| `src/Host/CombatHostSession.cs:296` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Combat] breaching_equipment_catalog.json load failed: {ex.Message` |
| `src/Host/CompletionHistorySelfTest.cs:79` | Exception | OTHER | `Check(false, "selftest threw " + ex);` |
| `src/Host/CompletionHistorySelfTest.cs:89` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[B3 COMPLETION HISTORY] Failed to clean test file: " + cleanupEx.Me` |
| `src/Host/CompletionHistoryStore.cs:56` | Exception | LOG_AND_CONTINUE | `string diagnostic = "[CompletionHistoryStore] Failed to read completion history ` |
| `src/Host/CompletionHistoryStore.cs:108` | Exception | LOG_AND_CONTINUE | `DiagnosticMessage = "[CompletionHistoryStore] Failed to persist completion histo` |
| `src/Host/CompletionHistoryStore.cs:116` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[CompletionHistoryStore] Failed to clean temporary history file: " ` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:89` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] Error during collection: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:131` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] echoes: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:176` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] journal corpus: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:249` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] bureaucratic documents: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:319` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] fringe cults: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:382` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] paper/printing corpus: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:436` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] bone/horn corpus: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:461` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] items.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:480` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] survivors.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:521` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr(
                    $"[RuntimeEvidence] {StartingCohortCatalo` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:561` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] narrative_encounters.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:609` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] {NarrativeArcEventCatalogLoader.FileName` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:667` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] questline_master.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:689` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] expeditions.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:706` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] radio.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:724` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] economy_goods.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:796` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr(
                    $"[RuntimeEvidence] {TradeTextCatalogLoad` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:817` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] wasteland_map_v1.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:832` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] weather_seasons.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:847` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] events.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:862` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] recipes.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:887` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] tech_salvage.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:908` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] espionage_missions.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:924` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] fluid_infrastructure.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:945` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] quest_templates.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:960` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] faction_lore.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:975` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] world_history.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:990` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] combat_catalog.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1006` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] disease_catalog.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1021` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] vehicles.json: {ex.Message` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1038` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] {file` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1056` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] {file` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1074` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] {file` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1092` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] {file` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1110` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] {file` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1128` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] {file` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1146` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] {file` |
| `src/Host/ContentUtilizationRuntimeCollector.cs:1181` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[RuntimeEvidence] {file` |
| `src/Host/ContentUtilizationSelfTest.cs:175` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"Content utilization self-test failed: {ex.Message` |
| `src/Host/ContentUtilizationSelfTest.cs:234` | bare | PROPAGATE | `return string.Empty;` |
| `src/Host/ContrabandStashSelfTest.cs:188` | Exception | PROPAGATE | `return $"[FAIL] {ex.GetType().Name` |
| `src/Host/CounterIntelligenceHostSession.cs:53` | Exception | OTHER | `GD.PushWarning($"[Ashfall Godot] CounterIntelligence catalog load failed: {ex.Me` |
| `src/Host/EquipmentConditionHostSession.cs:90` | Exception | OTHER | `GD.PushWarning($"[Ashfall Godot] Equipment degradation catalog load failed: {ex.` |
| `src/Host/GeothermalAquiferHostSession.cs:58` | Exception | OTHER | `GD.PushWarning($"[Ashfall Godot] Geothermal catalog load failed: {ex.Message` |
| `src/Host/GodotFileIO.cs:154` | bare | PROPAGATE | `return Array.Empty<string>();` |
| `src/Host/GodotFileIO.cs:172` | bare | OTHER | `/* cleanup: best-effort directory deletion */` |
| `src/Host/GodotLog.cs:33` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[GodotLog] Failed to initialize log directory '{logDir` |
| `src/Host/GodotLog.cs:53` | bare | OTHER | `// Fallback: file write failure must never crash the host process` |
| `src/Host/GreenhouseHostSession.cs:435` | bare | OTHER | `// Fall back to legacy bare state decode` |
| `src/Host/HiddenAgendaSelfTest.cs:127` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[HiddenAgendaSelfTest] Unexpected exception: {ex` |
| `src/Host/HoldfastFlavorCatalog.cs:86` | Exception | LOG_AND_CONTINUE | `log.Error("[Flavor] Failed to load " + FileName + ": " + e.Message);` |
| `src/Host/HoldfastRuntimeSession.cs:192` | Exception | OTHER | `// Rollback on any failure
                World.RestoreSave(worldSnapshot);
   ` |
| `src/Host/HoldfastRuntimeSession.cs:749` | Exception | PROPAGATE | `LastPersistenceMessage = "Fresh start failed: " + e.Message;
                ret` |
| `src/Host/HoldfastTradeSaveStore.cs:107` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[HoldfastTrade] load failed: " + e.Message);
                return` |
| `src/Host/HoldfastTradeSaveStore.cs:145` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[HoldfastTrade] Deserialization failure: {ex.Message` |
| `src/Host/HoldfastTradeSaveStoreSelfTest.cs:60` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Cleanup] Best-effort quarantine file delete failed: " + ex.Message` |
| `src/Host/HoldfastTradeSaveStoreSelfTest.cs:168` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[HoldfastTradeSaveStoreSelfTest] error: " + ex);
                Ch` |
| `src/Host/HoldfastTradeSaveStoreSelfTest.cs:175` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Cleanup] Best-effort temp file delete failed: " + ex.Message);` |
| `src/Host/HoldfastTradeSaveStoreSelfTest.cs:176` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Cleanup] Best-effort backup file delete failed: " + ex.Message);` |
| `src/Host/HoldfastTradeSaveStoreSelfTest.cs:182` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Cleanup] Best-effort wildcard file delete failed: " + ex.Message);` |
| `src/Host/HostCli.AdvancedIndustrialRecon.cs:42` | Exception | OTHER | `Check(checks, false, "118 selftest exception", ex.Message);` |
| `src/Host/HostCli.AdvancedIndustrialRecon.cs:65` | Exception | OTHER | `Check(checks, false, "119 selftest exception", ex.Message);` |
| `src/Host/HostCli.AdvancedIndustrialRecon.cs:87` | Exception | OTHER | `Check(checks, false, "120 selftest exception", ex.Message);` |
| `src/Host/HostCli.AdvancedIndustrialRecon.cs:109` | Exception | OTHER | `Check(checks, false, "121 selftest exception", ex.Message);` |
| `src/Host/HostCli.AdvancedIndustrialRecon.cs:148` | Exception | OTHER | `Check(checks, false, "advanced integrated selftest exception", ex.Message);` |
| `src/Host/HostCli.Difficulty.cs:225` | Exception | LOG_AND_CONTINUE | `failed++;
                GD.PrintErr("[DIFFICULTY] unhandled self-test exceptio` |
| `src/Host/HostCli.ExpeditionPlaytest.cs:108` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"EXPEDITION_PLAYTEST_SELFTEST FAIL — {ex` |
| `src/Host/HostCli.ExportParity.cs:65` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[PARITY] cannot enumerate source data dir '{dataDirectory` |
| `src/Host/HostCli.ExportParity.cs:150` | Exception | LOG_AND_CONTINUE | `GD.Print("[PARITY] executable-path probe skipped: " + ex.Message);` |
| `src/Host/HostCli.ExportParity.cs:228` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[PARITY] JSON parse error in {path` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs:85` | Exception | OTHER | `Check(false, "exception: " + ex.Message);` |
| `src/Host/HostCli.Mods.cs:66` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr("[ModSelfTest] FAIL — " + ex.Message);
                return ` |
| `src/Host/HostCli.Onboarding.cs:57` | Exception | OTHER | `Check(false, $"inventory seed bootstrap: {ex.Message` |
| `src/Host/HostCli.Onboarding.cs:74` | Exception | OTHER | `Check(false, $"duty roster bootstrap: {ex.Message` |
| `src/Host/HostCli.PanelTests.cs:139` | Exception | OTHER | `Check(false, "selftest threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:284` | Exception | OTHER | `Check(false, "selftest threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:384` | Exception | OTHER | `Check(false, "selftest threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:1044` | Exception | OTHER | `report.FailedCount++;
                report.Passed = false;
                rep` |
| `src/Host/HostCli.PanelTests.cs:1121` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[OralLore] selftest exception: {ex.GetType().Name` |
| `src/Host/HostCli.PanelTests.cs:1173` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] gear-bridge probe threw: " + e.Message);
                pass =` |
| `src/Host/HostCli.PanelTests.cs:1213` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] save/load round-trip probe threw: " + e.Message);
             ` |
| `src/Host/HostCli.PanelTests.cs:1284` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] D1 stale-restore probe threw: " + e.Message);
                p` |
| `src/Host/HostCli.PanelTests.cs:1419` | InvalidOperationException | OTHER | `mismatchRejected = ex.Message.Contains("identity", StringComparison.OrdinalIgnor` |
| `src/Host/HostCli.PanelTests.cs:1425` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] H10 persistence contract probe threw: " + e.Message);
         ` |
| `src/Host/HostCli.PanelTests.cs:1498` | Exception | OTHER | `Check(false, "WorldHostSession sky armor probe exception: " + ex.Message);` |
| `src/Host/HostCli.PanelTests.cs:1550` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] save-integrity probe threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:1584` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] legacy-save probe threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:1622` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] corrupt-save probe threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:1722` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] reload-continuity probe threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:1767` | Exception | OTHER | `report.FailedCount++;
                report.Passed = false;
                rep` |
| `src/Host/HostCli.PanelTests.cs:1812` | Exception | OTHER | `report.FailedCount++;
                report.Passed = false;
                rep` |
| `src/Host/HostCli.PanelTests.cs:1908` | Exception | OTHER | `Check(false, "selftest threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:2043` | Exception | OTHER | `Check(false, "black flotilla selftest threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:2131` | Exception | OTHER | `Check(false, "radio selftest threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:2259` | Exception | OTHER | `Check(false, "selftest threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:2492` | Exception | OTHER | `Check(false, "standalone systems selftest threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:2835` | Exception | OTHER | `Check(false, "phase0 selftest threw: " + e.Message);` |
| `src/Host/HostCli.PanelTests.cs:2991` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Day1PlayableSelfTest] Exception thrown: {ex.Message` |
| `src/Host/HostCli.PanelTests.cs:3175` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Day1ToDay2MilestoneSelfTest] §21 section exception: {ex.Message` |
| `src/Host/HostCli.PanelTests.cs:3295` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"  [FAIL] Exception at {w` |
| `src/Host/HostCli.PanelTests.cs:3442` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[SettingsSelfTest] Exception: {ex.Message` |
| `src/Host/HostCli.PanelTests.cs:3600` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[PlayableShellSelfTest] Exception: {ex.Message` |
| `src/Host/HostCli.PanelTests.cs:3752` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[ShelterHazardLoopSelfTest] Exception: {ex.Message` |
| `src/Host/HostCli.PanelTests.cs:4112` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[ShelterOperationsSelfTest] Exception: {ex.Message` |
| `src/Host/HostCli.Plans122to125.cs:83` | Exception | OTHER | `Check("sofc_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans122to125.cs:128` | Exception | OTHER | `Check("cvd_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans122to125.cs:157` | Exception | OTHER | `Check("sra_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans122to125.cs:207` | Exception | OTHER | `Check("amb_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans122to125.cs:292` | Exception | OTHER | `Check("save_roundtrips_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans122to125.cs:679` | Exception | OTHER | `Check("harness_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans122to125.cs:782` | Exception | OTHER | `Check("sofc_soak_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans122to125.cs:839` | Exception | OTHER | `Check("acoustic_soak_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans122to125.cs:907` | Exception | OTHER | `Check("diamond_soak_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans122to125.cs:986` | Exception | OTHER | `Check("amphibious_soak_no_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans139_141.cs:60` | Exception | OTHER | `Check("insar_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans139_141.cs:98` | Exception | OTHER | `Check("extrusion_exception", false, ex.Message);` |
| `src/Host/HostCli.Plans139_141.cs:127` | Exception | OTHER | `Check("runflat_exception", false, ex.Message);` |
| `src/Host/HostCli.PlansB86_B89.cs:53` | Exception | OTHER | `Check("catalog_valid", false, ex.Message);` |
| `src/Host/HostCli.PlansB86_B89.cs:173` | Exception | OTHER | `Check("catalog_valid", false, ex.Message);` |
| `src/Host/HostCli.PlansB86_B89.cs:352` | Exception | OTHER | `Check("catalog_valid", false, ex.Message);` |
| `src/Host/HostCli.PlansB86_B89.cs:496` | Exception | OTHER | `Check("catalog_valid", false, ex.Message);` |
| `src/Host/HostCli.SelfTests.cs:61` | Exception | LOG_AND_CONTINUE | `string message = "[COLLECTIBLE] collectible integrity validation crashed: " + ex` |
| `src/Host/HostCli.SelfTests.cs:77` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[DATA] Failed to enumerate catalog files: " + ex.Message);
        ` |
| `src/Host/HostCli.SelfTests.cs:326` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] " + id + " delegate threw: " + e.Message);
                    ` |
| `src/Host/HostCli.SelfTests.cs:485` | Exception | OTHER | `Check(false, "warlord host playthrough threw: " + e.Message);` |
| `src/Host/HostCli.SelfTests.cs:563` | Exception | OTHER | `Check(false, "warlord ui selftest threw: " + e.Message);` |
| `src/Host/HostCli.SelfTests.cs:665` | Exception | OTHER | `Check(false, "host playthrough threw: " + e.Message);` |
| `src/Host/HostCli.SelfTests.cs:903` | Exception | LOG_AND_CONTINUE | `GD.Print("[FAIL] verdict selftest threw: " + e);
                failures++;` |
| `src/Host/HostCli.SelfTests.cs:1060` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[PREFLIGHT] Failed to enumerate catalog files: " + e.Message);
    ` |
| `src/Host/HostCli.SelfTests.cs:1113` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[READ_ERROR] (" + classification + ") " + filePath + ": " + e.Messa` |
| `src/Host/HostCli.SelfTests.cs:1135` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[MALFORMED] (" + classification + ") " + filePath + ": " + e.Messag` |
| `src/Host/HostCli.SelfTests.cs:1221` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[CampaignFuzz] selftest error: {ex` |
| `src/Host/HostCli.SkyDefense.cs:119` | Exception | OTHER | `Check("exception", false, ex.Message);` |
| `src/Host/HostCli.StartingSupplies.cs:107` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] starting supplies selftest exception: {ex` |
| `src/Host/HostCli.Summary.cs:22` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Cleanup] Best-effort temp file delete failed: " + ex.Message);` |
| `src/Host/HostCli.Summary.cs:39` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Cleanup] Best-effort temp directory delete failed: " + ex.Message)` |
| `src/Host/HostCli.VehicleGarage.cs:207` | Exception | OTHER | `Check("exception", false, ex.Message);` |
| `src/Host/HostCli.WorldPlaytest.cs:134` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"WORLD_PLAYTEST_SELFTEST FAIL — {ex.GetType().Name` |
| `src/Host/HostCli.WorldPlaytest.cs:984` | bare | PROPAGATE | `return "unknown";` |
| `src/Host/HostSessionContracts.cs:162` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[HOST_WIRING] Exception obtaining report from {reporter.GetType().` |
| `src/Host/InventorySaveSelfTest.cs:15` | bare | OTHER | `/* cleanup: best-effort removal of stale test save */` |
| `src/Host/InventorySaveSelfTest.cs:36` | Exception | PROPAGATE | `return $"[FAIL] {ex.GetType().Name` |
| `src/Host/JournalHostSession.cs:43` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Journal] save failed: " + e.Message);` |
| `src/Host/JournalHostSession.cs:57` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Journal] restore failed: " + e.Message);` |
| `src/Host/JournalSaveSelfTest.cs:26` | Exception | PROPAGATE | `return $"[FAIL] {ex.GetType().Name` |
| `src/Host/LoaderWiringSelfTest.cs:72` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] Failed to parse loader_wiring_policy.json: {ex.Message` |
| `src/Host/MedicalWardHostSession.cs:61` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[MedicalWard] save failed: " + e.Message);` |
| `src/Host/MedicalWardHostSession.cs:77` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[MedicalWard] restore failed: " + e.Message);` |
| `src/Host/MedicalWardSaveSelfTest.cs:24` | Exception | PROPAGATE | `return $"[FAIL] {ex.GetType().Name` |
| `src/Host/ModRuntime.cs:84` | Exception | LOG_AND_CONTINUE | `string message = "[Mods] Staging failed; base catalogs remain active: " + ex.Mes` |
| `src/Host/NarrativeContinuitySelfTest.cs:62` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"Narrative continuity self-test failed: {ex.Message` |
| `src/Host/PanelBindLifecycleSelfTest.cs:190` | bare | OTHER | `/* cleanup: best-effort temp directory delete */` |
| `src/Host/PanelBindLifecycleSelfTest.cs:1364` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] PanelBindLifecycleSelfTest exception: {ex.GetType().Name` |
| `src/Host/PerformanceSelfTest.cs:135` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"RUNTIME_SCALE_SELFTEST REPORT WRITE FAIL: {ex.Message` |
| `src/Host/PersonalQuestSelfTest.cs:140` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[PersonalQuestSelfTest] Unhandled exception: " + ex);
             ` |
| `src/Host/PhantomMemoryHostSession.cs:351` | bare | OTHER | `// Fallback in case of bare array JSON
                    entries = json.Deseri` |
| `src/Host/PhantomMemoryHostSession.cs:390` | Exception | OTHER | `string msg = $"[PhantomMemory] Failed to load rules: {ex.Message` |
| `src/Host/Phase0HostSession.cs:574` | bare | OTHER | `// Fallback in case of bare array JSON
                    entries = json.Deseri` |
| `src/Host/Phase0HostSession.cs:601` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Phase0] Failed to load phantom rules: {ex.Message` |
| `src/Host/Phase0HostSession.cs:624` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Phase0] Failed to load final-wish catalog: {ex.Message` |
| `src/Host/PortContractSelfTest.cs:74` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] Failed to parse port_contract_policy.json: {ex.Message` |
| `src/Host/PropagandaSelfTest.cs:162` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[PropagandaSelfTest] Unhandled exception: " + ex);
                ` |
| `src/Host/RadioCatalogSelfTest.cs:45` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] Exception during RadioStationCatalogLoader execution: {ex.M` |
| `src/Host/ReconTelemetryHostSession.cs:63` | Exception | OTHER | `GD.PushWarning($"[Ashfall Godot] ReconTelemetry catalog load failed: {ex.Message` |
| `src/Host/RelationshipDecaySelfTest.cs:88` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[RelationshipDecaySelfTest] Unhandled exception: " + ex);
         ` |
| `src/Host/RumorNetworkSelfTest.cs:139` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[RumorNetworkSelfTest] Unhandled exception: " + ex);
              ` |
| `src/Host/SaveLoadHostSession.cs:370` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[SaveLoad] Manifest update failed for slot '{_activeSlotId` |
| `src/Host/SaveLoadHostSession.cs:445` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[SaveLoad] Skipping corrupt legacy section '{pair.Value` |
| `src/Host/SaveLoadHostSession.cs:675` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[SaveLoad] Envelope save failed, previous envelope preserved: {ex.` |
| `src/Host/SaveLoadHostSession.cs:942` | Exception | OTHER | `errors.Add($"Derived section projection transaction failed: {ex.Message` |
| `src/Host/SaveLoadHostSession.cs:960` | Exception | OTHER | `errors.Add($"Rollback of '{original.Key` |
| `src/Host/SaveLoadHostSession.cs:971` | Exception | OTHER | `errors.Add($"In-progress marker cleanup failed: {markerEx.Message` |
| `src/Host/SaveLoadHostSession.cs:982` | Exception | LOG_AND_CONTINUE | `if (committed)
                    GD.PrintErr($"[SaveLoad] Projection staging c` |
| `src/Host/SaveLoadHostSession.cs:993` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[SaveLoad] In-progress marker cleanup failed for slot '{slotId` |
| `src/Host/SaveLoadHostSession.cs:1005` | Exception | OTHER | `// campaign.json has already committed and remains authoritative;
            //` |
| `src/Host/SaveLoadUiFailureSelfTest.cs:483` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] SaveLoadUiFailureSelfTest exception: {ex.GetType().Name` |
| `src/Host/SaveLoadUiFailureSelfTest.cs:495` | bare | OTHER | `// Ignore temp cleanup error` |
| `src/Host/SaveSlotRoot.cs:45` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[SaveSlotRoot] Failed to create user directory '{userDir` |
| `src/Host/SaveStoreChecksumSelfTest.cs:101` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] Unexpected exception: {ex.Message` |
| `src/Host/SceneBindingSelfTest.cs:321` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[SCENE_BIND] FAIL {sc.ResPath` |
| `src/Host/SevenDayDeterministicSmokeTest.cs:89` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] Gate 1: Exception during baseline run: {ex.Message` |
| `src/Host/SevenDayDeterministicSmokeTest.cs:186` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] Gate 7: Exception during save/reload run: {ex.Message` |
| `src/Host/SevenDayDeterministicSmokeTest.cs:242` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] Unexpected exception in smoke run: {ex.Message` |
| `src/Host/SevenDayDeterministicSmokeTest.cs:250` | bare | OTHER | `/* cleanup: best-effort temp directory delete */` |
| `src/Host/ShelterAssignmentHostSession.cs:122` | Exception | PROPAGATE | `s_log.Error("[ShelterAssignmentSaveStore] capture failed: " + e.Message);
      ` |
| `src/Host/ShelterAssignmentHostSession.cs:137` | Exception | PROPAGATE | `s_log.Error("[ShelterAssignmentSaveStore] restore failed: " + e.Message);
      ` |
| `src/Host/ShelterAssignmentHostSession.cs:158` | Exception | PROPAGATE | `s_log.Error("[ShelterAssignmentSaveStore] save failed: " + e.Message);
         ` |
| `src/Host/ShelterAssignmentHostSession.cs:172` | Exception | PROPAGATE | `s_log.Error("[ShelterAssignmentSaveStore] load failed: " + e.Message);
         ` |
| `src/Host/ShelterAtmosphereSelfTest.cs:108` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[ShelterAtmosphereSelfTest] Exception: {ex.Message` |
| `src/Host/ShelterReputationSelfTest.cs:126` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[ShelterReputationSelfTest] Unexpected exception: {ex` |
| `src/Host/ShelterSecuritySelfTest.cs:128` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[ShelterSecuritySelfTest] Unhandled exception: " + ex);
           ` |
| `src/Host/StandingRecordHostSession.cs:52` | Exception | OTHER | `hostLog.Error("[StandingRecord] load failed: " + ex.Message);` |
| `src/Host/StartingLevelHostSession.cs:178` | bare | OTHER | `// Fall back to legacy bare state decode` |
| `src/Host/SurvivorDeathLegacySelfTest.cs:115` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[SurvivorDeathLegacySelfTest] Unhandled exception: " + ex);
       ` |
| `src/Host/TimeCapsuleSelfTest.cs:121` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[TimeCapsuleSelfTest] Unhandled exception: " + ex);
               ` |
| `src/Host/UiAccessibilitySelfTest.cs:197` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] UI Accessibility Self-Test encountered exception: {ex` |
| `src/Host/UiAccessibilitySelfTest.cs:236` | bare | OTHER | `// Fallback for environments where PackedScene loader is mock-only` |
| `src/Host/WeatherHardeningHostSession.cs:58` | Exception | OTHER | `GD.PushWarning($"[Ashfall Godot] WeatherHardening catalog load failed: {ex.Messa` |
| `src/Host/WeatherHostSession.cs:68` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Weather] restore failed: " + e.Message);` |
| `src/Host/WeatherSaveSelfTest.cs:26` | Exception | PROPAGATE | `return $"[FAIL] {ex.GetType().Name` |
| `src/Journal/JournalCatalogData.cs:168` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[JournalCatalog] Failed to load verdict ladder overlays: {ex.Messa` |
| `src/Journal/JournalCatalogData.cs:241` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[JournalCatalog] Failed to load shelter room identities: {ex.Messa` |
| `src/Journal/JournalCatalogData.cs:256` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[JournalCatalog] Failed to load catalog list from '{path` |
| `src/Journal/JournalSelfTest.cs:184` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[JournalSelfTest] temp cleanup failed for {tmpPath` |
| `src/Localization/AshfallLocalization.cs:81` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[AshfallLocalization] Failed to load catalog from {resPath` |
| `src/Main.Anomaly.cs:49` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Anomaly] Failed to parse {catalogPath` |
| `src/Main.Bionics.cs:51` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Bionics] Failed to parse {catalogPath` |
| `src/Main.Campaign.cs:104` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] Calendar reconciliation skipped: " + ex.Message)` |
| `src/Main.Campaign.cs:158` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] DailyBriefing load failed: " + e.Message);
     ` |
| `src/Main.Campaign.cs:173` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] DailyBriefing save failed: " + e.Message);` |
| `src/Main.Campaign.cs:190` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] CampaignDay save failed: " + e.Message);` |
| `src/Main.Campaign.cs:286` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] Memorial load failed: " + e.Message);` |
| `src/Main.Campaign.cs:305` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] Memorial save failed: " + e.Message);` |
| `src/Main.Companion.cs:61` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Companion] Failed to parse {catalogPath` |
| `src/Main.DayRecord.cs:29` | Exception | OTHER | `GD.PushWarning($"[DayRecord] append failed: {e.Message` |
| `src/Main.Difficulty.cs:83` | Exception | PROPAGATE | `error = ex.Message;
                return false;` |
| `src/Main.Difficulty.cs:105` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Ashfall Godot] Difficulty preset failed, using standard scalars: "` |
| `src/Main.Difficulty.cs:135` | Exception | PROPAGATE | `error = ex.Message;
                return false;` |
| `src/Main.Difficulty.cs:180` | Exception | PROPAGATE | `return ex.Message;` |
| `src/Main.Difficulty.cs:213` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Ashfall Godot] Difficulty bonus grant refused: " + ex.Message);
  ` |
| `src/Main.Economy.cs:204` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] SilentFoundry save failed: " + e.Message);` |
| `src/Main.ExpandedShelterSystems.cs:271` | Exception | LOG_AND_CONTINUE | `// Memorial integration is optional; log warning without blocking autopsy flow.
` |
| `src/Main.Expeditions.cs:494` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] WastelandMap save failed: " + e.Message);` |
| `src/Main.Expeditions.cs:508` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] EncounterChoice save failed: " + e.Message);` |
| `src/Main.Expeditions.cs:522` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] TravelEncounters save failed: " + e.Message);` |
| `src/Main.Expeditions.cs:550` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] TravelEncounters setup failed: " + e.Message);` |
| `src/Main.Expeditions.cs:571` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] EncounterChoice load failed: " + e.Message);` |
| `src/Main.GameFlow.cs:177` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[Ashfall Godot] New Game aborted: " + ex.Message);
                ` |
| `src/Main.Medical.cs:372` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] MedicalWard save failed: " + e.Message);
       ` |
| `src/Main.Medical.cs:391` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] MedicalWard load failed: " + e.Message);` |
| `src/Main.Medical.cs:404` | Exception | OTHER | `GD.PushWarning("[Ashfall Godot] Disease save failed: " + e.Message);` |
| `src/Main.MoralChoice.cs:88` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Ashfall Godot] Moral choice restore rejected: {e.Message` |
| `src/Main.Onboarding.cs:55` | InvalidOperationException | OTHER | `GD.PushWarning($"[Onboarding] Save load rejected: {ex.Message` |
| `src/Main.Plans126_129.cs:48` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.BioFermentation] Failed to parse {catalogPath` |
| `src/Main.Plans146_149.cs:230` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Plans146] ebpvd_coating_catalog.json load failed; engine seeds in` |
| `src/Main.Plans146_149.cs:255` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Plans148] microfluidic_diagnostic_catalog.json load failed; engin` |
| `src/Main.Plans146_149.cs:273` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Plans147] mine_flail_catalog.json load failed; engine seeds in fo` |
| `src/Main.Plans146_149.cs:291` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Plans149] rail_grinding_catalog.json load failed; engine seeds in` |
| `src/Main.Plans147.cs:63` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Contraband] failed to parse dependency catalog: {ex.Message` |
| `src/Main.Plans178_181.cs:52` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Generational] Failed to parse {catalogPath` |
| `src/Main.Plans178_181.cs:138` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Prisoners] Failed to parse {catalogPath` |
| `src/Main.Plans178_181.cs:209` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Mutations] Failed to parse {catalogPath` |
| `src/Main.Plans178_181.cs:270` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Stealth] Failed to parse {catalogPath` |
| `src/Main.Plans186_189.cs:48` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Fallout] Failed to parse {catalogPath` |
| `src/Main.Plans186_189.cs:115` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Desperation] Failed to parse {catalogPath` |
| `src/Main.Plans186_189.cs:175` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Mercenary] Failed to parse {catalogPath` |
| `src/Main.Plans186_189.cs:227` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Mercenary] Failed to load candidate targets: {ex.Message` |
| `src/Main.Plans186_189.cs:274` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Archaeology] Failed to parse {catalogPath` |
| `src/Main.Plans190_193.cs:54` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Amputation] Failed to parse {catalogPath` |
| `src/Main.Plans190_193.cs:128` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Railway] Failed to parse {catalogPath` |
| `src/Main.Plans190_193.cs:148` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Railway] Failed to parse {logisticsPath` |
| `src/Main.Plans190_193.cs:217` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Fungi] Failed to parse {catalogPath` |
| `src/Main.Plans190_193.cs:298` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Justice] Failed to parse {catalogPath` |
| `src/Main.Plans202_205.cs:45` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Pyrolysis] Failed to parse {catalogPath` |
| `src/Main.Plans202_205.cs:140` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Airdrop] Failed to parse {catalogPath` |
| `src/Main.Plans50_53.cs:54` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Ashfall Godot] Failed to load vehicle_modifications.json: {ex.Mes` |
| `src/Main.Plans50_53.cs:72` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Ashfall Godot] Failed to load {VehicleArmorGradeCatalogLoader.Fil` |
| `src/Main.Plans50_53.cs:124` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Ashfall Godot] Failed to load faction_intelligence.json: {ex.Mess` |
| `src/Main.Plans50_53.cs:170` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Ashfall Godot] Failed to load psychological_trauma.json: {ex.Mess` |
| `src/Main.Plans50_53.cs:216` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Ashfall Godot] Failed to load shelter_audio_cues.json: {ex.Messag` |
| `src/Main.ShelterInfrastructure.cs:286` | bare | PROPAGATE | `return null;` |
| `src/Main.ShelterInfrastructure.cs:342` | bare | PROPAGATE | `return string.Empty;` |
| `src/Main.UiHandlers.cs:311` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[QuitUiTestAfterFrame] {ex.GetType().Name` |
| `src/Main.UiHandlers.cs:315` | bare | OTHER | `/* last-resort: process may already be tearing down */` |
| `src/Main.UiTests.CompositionRoot.cs:281` | bare | OTHER | `dict[field.Name] = null;` |
| `src/Main.UiTests.RealCampaignJourney.cs:509` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FAIL] RealCampaignJourneySelfTest exception: {ex.GetType().Name` |
| `src/Main.UiTests.RealCampaignJourney.cs:522` | bare | OTHER | `// Temp cleanup is best-effort; never let it mask the test result.` |
| `src/Main.UiTests.StartingCohortLifecycle.cs:205` | Exception | LOG_AND_CONTINUE | `GD.PrintErr(
                    $"[FAIL] StartingCohortLifecycleSelfTest except` |
| `src/Main.UiTests.StartingCohortLifecycle.cs:219` | bare | OTHER | `// Temp cleanup is best-effort; never mask the test result.` |
| `src/Main.UiTests.StartingCohortLifecycle.cs:251` | Exception | LOG_AND_CONTINUE | `GD.PrintErr("[StartingCohortLifecycle] campaign header probe failed: " + ex.Mess` |
| `src/Main.WorldPlaytest.cs:89` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[WorldPlaytest] production owner probe failed: {ex` |
| `src/Main.WorldPlaytest.cs:108` | bare | OTHER | `// Disposable test scratch cleanup is best effort.` |
| `src/Main.Zealotry.cs:49` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Main.Zealotry] Failed to parse {catalogPath` |
| `src/Radio/FactionRadioSelfTest.cs:82` | Exception | LOG_AND_CONTINUE | `GD.Print($"  [FAIL] Exception during test: {ex.Message` |
| `src/Settings/UserSettings.cs:57` | Exception | OTHER | `_lastDiagnosticMessage = $"[UserSettingsStore] Failed to read settings from '{pa` |
| `src/Settings/UserSettings.cs:94` | Exception | OTHER | `_lastDiagnosticMessage = $"[UserSettingsStore] Failed to save settings to '{path` |
| `src/Settings/UserSettings.cs:98` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[UserSettings] Failed to clean temp file: {cleanupEx.Message` |
| `src/Settings/UserSettings.cs:173` | Exception | LOG_AND_CONTINUE | `GD.Print($"[UserSettingsStore] Display apply notice (headless/unsupported): {ex.` |
| `src/Settings/UserSettings.cs:186` | Exception | LOG_AND_CONTINUE | `GD.Print($"[UserSettingsStore] Localization apply notice: {ex.Message` |
| `src/Settings/UserSettings.cs:219` | Exception | LOG_AND_CONTINUE | `GD.Print($"[UserSettingsStore] Accessibility apply notice: {ex.Message` |
| `src/Settings/UserSettings.cs:229` | Exception | LOG_AND_CONTINUE | `GD.Print($"[UserSettingsStore] Keybinding apply notice: {ex.Message` |
| `src/UI/AfflictionsPanel.cs:64` | bare | PROPAGATE | `return null;` |
| `src/UI/AshfallUiHelpers.cs:60` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[AshfallUiHelpers] Failed to load font '{path` |
| `src/UI/AshfallUiHelpers.cs:765` | Exception | OTHER | `CatalogDiagnostics.Warn(path, "Texture load (ResourceLoader)", ex_CATDIAG);
    ` |
| `src/UI/AshfallUiHelpers.cs:783` | Exception | OTHER | `CatalogDiagnostics.Warn(alt, "Texture load (case-normalized)", ex_CATDIAG);
    ` |
| `src/UI/AshfallUiHelpers.cs:804` | Exception | OTHER | `CatalogDiagnostics.Warn(osPath, "Texture load (filesystem)", ex_CATDIAG);
      ` |
| `src/UI/AshfallUiHelpers.cs:826` | Exception | OTHER | `CatalogDiagnostics.Warn(altOsPath, "Texture load (alt filesystem)", ex_CATDIAG);` |
| `src/UI/BackdropArt.cs:57` | Exception | OTHER | `GD.PushWarning($"[BackdropArt] backdrop load failed: {resPath` |
| `src/UI/CaravanBarterLedgerPanel.cs:151` | Exception | PROPAGATE | `CatalogDiagnostics.Warn("<reflection>", "ConsecutiveRepels property", ex_CATDIAG` |
| `src/UI/ExpeditionPanel.cs:163` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[ExpeditionPanel] Failed to load faction lore display names: {ex.M` |
| `src/UI/FactionCommuniqueBoardPanel.cs:216` | Exception | OTHER | `CatalogDiagnostics.Warn(osPath, "faction_lore.json", ex);` |
| `src/UI/FactionMatrixPanel.cs:98` | Exception | OTHER | `CatalogDiagnostics.Warn(osPath, "faction_lore.json", ex_CATDIAG);
            //` |
| `src/UI/FactionMatrixPanel.cs:190` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[FactionMatrix] IsFactionActive probe failed: {ex.Message` |
| `src/UI/FactionsNarrativePanel.cs:98` | Exception | OTHER | `CatalogDiagnostics.Warn(osPath, "faction_lore.json", ex_CATDIAG);
            //` |
| `src/UI/JournalPanel.cs:429` | Exception | OTHER | `CatalogDiagnostics.Warn("<scroll>", "ScrollToChild", ex);` |
| `src/UI/MainMenuPanel.cs:89` | Exception | OTHER | `CatalogDiagnostics.Warn("<ui>", "UiBackgroundCarousel", ex_CATDIAG);
           ` |
| `src/UI/MedicalPanel.cs:50` | bare | PROPAGATE | `return null;` |
| `src/UI/MedicalPanel.cs:895` | Exception | OTHER | `CatalogDiagnostics.Warn("<scroll>", "ScrollToChild", ex_CATDIAG);
              ` |
| `src/UI/SceneBindingHeadlessProbe.cs:96` | Exception | LOG_AND_CONTINUE | `Godot.GD.PrintErr($"[SCENE_BIND] FAIL {reg.ResPath` |
| `src/UI/ShelterPanel.cs:383` | Exception | OTHER | `CatalogDiagnostics.Warn("<scroll>", "ScrollToChild", ex_CATDIAG);
              ` |
| `src/UI/SnapshotOrchestrator.cs:132` | Exception | OTHER | `Fail($"mount-exception: {e.Message` |
| `src/UI/SnapshotOrchestrator.cs:215` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[SnapshotOrchestrator] Open call failed: {ex.Message` |
| `src/UI/SnapshotOrchestrator.cs:265` | Exception | OTHER | `Fail($"read-exception: {e.Message` |
| `src/UI/VerdictDashboardPanel.cs:131` | Exception | OTHER | `CatalogDiagnostics.Warn("<ui>", "VerdictDashboard refresh", ex_CATDIAG);
       ` |
| `src/World/HoldfastInteriorView.cs:178` | Exception | OTHER | `GD.PushWarning($"[HoldfastInteriorView] shelter art load failed: {resPath` |
| `src/World/WastelandMapView.cs:96` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Ashfall Godot][World] Error initializing: {ex.Message` |
| `src/World/WastelandMapView.cs:193` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Ashfall Godot][World] Failed to create marker for {node.Id` |
| `src/World/WastelandMapView.cs:226` | Exception | LOG_AND_CONTINUE | `GD.PrintErr($"[Ashfall Godot][World] Failed to create trap marker {markerState.M` |

## Class counts

LOG_AND_CONTINUE: 285, OTHER: 247, PROPAGATE: 159, SWALLOW_EMPTY: 1
