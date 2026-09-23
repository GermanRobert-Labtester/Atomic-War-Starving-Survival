# PLAN-ORPHAN-SEAL-01 — Appendix K: Public Member Signatures

**Generated:** 2026-09-21. The public surface a seal package must wire, listed
per orphan: public method signatures and public property names (up to 30
members per type; when a type has more, the omitted count is shown). Derived by
source scan, so overload sets and generic constraints appear verbatim.
**Use:** this is the API the host adapter binds to; if a needed member is not
here, the package's job is to add it to the owner, not to reach around it.

### `AccessibilitySettingsSystem`

```
void LoadCatalog(string json)
IReadOnlyList<AccessibilityProfileDef> GetAllProfiles()
AccessibilityProfileDef? GetProfile(string profileId)
bool ApplyProfile(string profileId)
void SetColorblindMode(string mode)
void SetFontScale(float scale)
void SetHighContrast(bool enabled)
void SetReducedMotion(bool enabled)
void SetVisualAudioAlerts(bool enabled)
void SetSubtitleSize(string size)
void SetCognitiveLoadReduction(bool enabled)
void SetAutoWalk(bool enabled)
void SetAimAssist(bool enabled)
AccessibilitySettingsState CaptureState()
void RestoreState(AccessibilitySettingsState? saved)
string profile_id
string display_name
string description
string colorblind_mode
float font_scale
bool high_contrast
bool reduced_motion
bool screen_reader_friendly
bool audio_descriptions
bool visual_audio_alerts
bool mono_audio
string subtitle_size
bool cognitive_load_reduction
bool auto_walk
bool aim_assist
… (17 more)
```

### `AudioAccessibilityCoordinator`

```
void LoadCatalog(string json)
bool TriggerCue(string cueId, double currentTimestampSeconds, out VisualAudioNotification? notification)
void ReleaseDucking()
bool ApplyPreset(string presetId)
AudioDiagnosticReadout GetDiagnosticReadout()
string CueId
string Category
string BusName
string VisualLabel
string Severity
float DuckLevelDb
float CoalesceWindowSeconds
string IconGlyph
string PresetId
string DisplayName
float MasterLimiterThresholdDb
float DialogueDuckingAttenuationDb
float HighFrequencyAttenuationDb
int SchemaVersion
List<AudioCueAccessibilityDef> Cues
List<AudioMixPresetDef> MixPresets
string CueId
string VisualLabel
string Severity
string IconGlyph
double TimestampSeconds
IReadOnlyList<string> ActiveBuses
float ActiveDuckingDb
string CurrentPresetId
VisualAudioNotification? LastNotification
… (4 more)
```

### `CassettePlaybackSystem`

```
void LoadCatalog(IEnumerable<CassetteSetDefinition> sets)
bool AcquirePart(string itemId)
ActionResult PlayPart(string itemId, int simDay = -1)
bool TryGetPart(string itemId, out CassettePartDefinition? partDef, out CassetteSetDefinition? setDef)
bool IsPartCollected(string itemId)
bool IsPartPlayed(string itemId)
bool IsSetComplete(string setId)
void GetSetProgress(string setId, out int collected, out int total)
IReadOnlyList<string> GetDiscoveredCacheLocations()
IReadOnlyList<string> GetCacheItems(string cacheLocation)
CassettePlaybackState CaptureState()
void RestoreState(CassettePlaybackState? saved)
```

### `BestiarySystem`

```
void LoadCatalog(string json, IJsonSerializer? serializer = null)
CreatureDiscoveryRecord RecordEncounter(string creatureId, int day, string locationId = "", string witnessId = "")
void RecordKill(string creatureId, int day, string locationId = "")
void RecordButcher(string creatureId, int day)
CreatureDiscoveryRecord? GetDiscovery(string creatureId)
IReadOnlyList<CreatureDiscoveryRecord> GetAllDiscoveries()
IReadOnlyList<CreatureSightingRecord> GetRecentSightings(int limit = 20)
float GetCompletionPercentage()
bool IsBasicStatsUnlocked(string creatureId)
bool IsBehaviorUnlocked(string creatureId)
bool IsCombatTacticsUnlocked(string creatureId)
BestiaryState CaptureState()
void RestoreState(BestiaryState? saved)
string CreatureId
int DiscoveredDay
int EncounterCount
int KillCount
int ButcherCount
string FirstLocationId
int LastEncounterDay
List<string> UnlockedNoteKeys
string SightingId
string CreatureId
int Day
string LocationId
string WitnessSurvivorId
string SightingType
int SchemaVersion
int NextSequence
List<CreatureDiscoveryRecord> Discoveries
… (1 more)
```

### `ChemicalPlumeDispersionEngine`

```
ChemicalPlumeState Clone()
static void AdvancePlumeDispersion(ChemicalPlumeState plume, WeatherDispersionVector weather)
static ShelterAirQualityResult EvaluateShelterAirInfiltration(int outdoorDensityPermille, PlumeToxicityTier toxicity, bool isFiltrationPowered, int shelterFilterConditionPermille)
static RespiratorProtectionResult EvaluateRespiratorProtection(int ambientDensityPermille, PlumeToxicityTier toxicity, int canisterConditionPermille)
string PlumeId
string AgentId
int SectorX
int SectorY
int DensityPermille
int RemainingLifespanTicks
PlumeToxicityTier ToxicityTier
int WindSpeedKph
int WindDirectionDeg
int PrecipitationIntensityPermille
AirQualityBand IndoorAirQuality
int IndoorContaminantDensityPermille
int FilterWearDeltaPermille
bool AirlockBreachWarning
bool Protected
int CanisterWearDeltaPermille
int EffectiveExposureDose
bool CanisterDepleted
```

### `CommitmentSystem`

```
void RegisterCommitment(CommitmentDefinition def)
bool RecordProgress(string commitmentId, int quantityAdded)
bool Settle(string commitmentId, int currentDay)
CommitmentReadModel? GetCommitment(string id, int currentDay)
IReadOnlyList<CommitmentReadModel> GetCommitments(int currentDay)
void CapturePreDaySnapshot(int day)
void RestorePreDaySnapshot(int day)
void TickDay(int day, List<DayStateChangeEvent> events)
CommitmentSaveState CaptureState()
void RestoreState(CommitmentSaveState? state)
```

### `InternalCommunicationSystem`

```
CommunicationCategory ParseCategory()
MessagePriority ParsePriority()
void LoadCatalog(string json)
void LoadCatalog(CommunicationCatalogData catalog)
CommunicationTemplateDefinition? GetTemplate(string templateId)
CommunicationMessage PostMessage(string authorId, CommunicationCategory category, string subject, string content, string? recipientId = null, MessagePriority priority = MessagePriority.Normal, int currentDay = 1, int durationDays = 7, CommunicationChannel channel = CommunicationChannel.BulletinBoard)
CommunicationMessage? PostFromTemplate(string templateId, string authorId, string? recipientId = null, int currentDay = 1, string? customDetails = null)
IntercomAnnouncement BroadcastIntercom(string authorId, string message, MessagePriority priority = MessagePriority.Normal, int currentDay = 1)
bool AcknowledgeIntercom(string announcementId, string survivorId)
bool MarkAsRead(string messageId, string readerId)
bool AcknowledgeMessage(string messageId, string acknowledgerId)
IReadOnlyList<CommunicationMessage> GetMessagesForRecipient(string survivorId)
IReadOnlyList<CommunicationMessage> GetPublicNotices()
BulletinBoard AddBulletinBoard(string name, string roomId, int capacity = 20, bool isLeadershipOnly = false)
void TickDay(int currentDay)
InternalCommunicationState CaptureState()
void RestoreState(InternalCommunicationState state)
string Id
string Title
string Category
string DefaultPriority
int DurationDays
string Description
int SchemaVersion
List<CommunicationTemplateDefinition> Templates
string MessageId
CommunicationCategory Category
CommunicationChannel Channel
string AuthorId
string RecipientId
… (23 more)
```

### `CommunicationsSystem`

```
AntennaDto Clone()
CommunicationsNetworkDto Clone()
InterceptedMessageDto Clone()
OutgoingBroadcastDto Clone()
void LoadCatalog(string json)
AntennaDto InstallAntenna(string antennaId, AntennaType type, string? name = null)
bool RepairAntenna(string antennaId, double amount)
void DegradeAntennas(double wearAmount)
double GetEffectiveReceptionRangeKm()
int GetEffectiveSensitivity()
InterceptedMessageDto? InterceptFactionSignal(string factionId, ISeededRng rng, int currentDay)
bool DecodeMessage(string messageId, int cryptanalysisSkill, ISeededRng rng)
OutgoingBroadcastDto? TransmitBroadcast(double frequencyMhz, string content, int encryptionLevel, int currentDay)
void JamFrequency(double frequencyMhz, int durationDays)
AntennaDto? GetAntenna(string antennaId)
CommunicationsNetworkDto? GetNetwork(string networkId)
InterceptedMessageDto? GetMessage(string messageId)
IReadOnlyList<AntennaDto> GetAllAntennas()
IReadOnlyList<InterceptedMessageDto> GetAllInterceptedMessages()
CommunicationsState CaptureState()
void RestoreState(CommunicationsState state)
string AntennaId
AntennaType Type
string Name
double RangeKm
int Sensitivity
int MaxChannels
int PowerDrawWatts
double Condition
bool IsActive
… (35 more)
```

### `ContentOrphanCertificationEngine`

```
static ContentCertificationReport Certify(IEnumerable<ContentCandidateRow> candidates, ISet<string> activeConsumerRegistry)
string ContentId
string CatalogSource
string CanonicalConsumer
bool IsPrerequisiteMet
bool IsCertificationClean
int TotalCandidatesEvaluated
int CertifiedActiveCount
int ExcludedDormantCount
int OrphanWarningCount
bool CanPromotePlan49
IReadOnlyList<string> OrphanIds
```

### `CookingSystem`

```
bool HasIngredients(IEnumerable<RecipeIngredient> ingredients)
bool TryConsumeIngredients(IEnumerable<RecipeIngredient> ingredients)
bool DeliverCookedFood(string itemId, int quantity, float radiationFraction, float nutritionValue)
void RegisterRecipe(CookingRecipe recipe)
bool TryGetRecipe(string recipeId, out CookingRecipe? recipe)
void LoadCatalog(string json)
static CookingSystem LoadFromDirectory(string dataDir, IFileIO fileIO, ISeededRng? rng = null)
ActionResult StartCooking(string recipeId, string cookId = "cook_survivor", string equipmentType = "improvised_stove", ICookingSource? source = null, float currentMinute = 0f)
int ProgressCooking(float deltaMinutes, ICookingSource? destination = null)
ActionResult CancelCooking(string operationId)
CookingState CaptureState()
void RestoreState(CookingState? state)
string itemId
int quantity
string id
string displayName
string description
string requiredEquipment
float cookTimeMinutes
float nutritionValue
float radiationRemoval
float shelfLifeDays
float moraleBonus
List<RecipeIngredient> inputItems
string outputItemId
int outputQuantity
string operationId
string recipeId
string assignedCookId
float startMinute
… (21 more)
```

### `CultureCreationSystem`

```
void LoadCatalog(string json)
IReadOnlyList<ArtFormDef> GetAllForms()
ArtFormDef? GetForm(string formId)
ArtworkRecord CreateArtwork(string creatorSurvivorId, string title, ArtMedium medium, ArtTheme theme, int day, float artistSkill = 50f, ISeededRng? rng = null)
bool DisplayArtwork(string artworkId, string locationId)
float GetShelterCultureMoraleBonus()
CultureCreationState CaptureState()
void RestoreState(CultureCreationState state)
string form_id
string form_name
string description
string medium
int required_skill
List<string> required_materials
int creation_time_days
float morale_boost
int cultural_value
string display_location_type
int schema_version
List<ArtFormDef> art_forms
string ArtworkId
string Title
string CreatorSurvivorId
ArtMedium Medium
ArtTheme Theme
int CreationDay
float QualityScore
float CulturalImpact
string DisplayLocation
bool IsMasterwork
… (5 more)
```

### `ShelterFestivalEngine`

```
FestivalPlanSaveState CaptureState()
FestivalPlan ScheduleFestival(FestivalType type, string title, int plannedDay, int durationDays = 1, IEnumerable<FestivalCommodityRequirement>? customCommodities = null)
bool TryCommenceFestival(string festivalId, int currentDay, Func<string, int, bool> tryConsumeCommodity)
void ProcessDailyTick(int currentDay)
bool CancelFestival(string festivalId)
ShelterFestivalSaveState CaptureState()
void RestoreState(ShelterFestivalSaveState? state)
string ItemId
int RequiredUnits
string FestivalId
FestivalType Type
string Title
int PlannedDay
int DurationDays
int DaysActive
bool IsActive
bool IsCompleted
int MoraleBoostPermille
int DespairReductionPermille
List<FestivalCommodityRequirement> RequiredCommodities
int schema_version
List<FestivalPlanSaveState> Festivals
string FestivalId
FestivalType Type
string Title
int PlannedDay
int DurationDays
int DaysActive
bool IsActive
bool IsCompleted
… (2 more)
```

### `ShelterMuseumSystem`

```
ArtifactType ParseArtifactType()
ExhibitionTheme ParseTheme()
void LoadCatalog(string json)
MuseumArtifactTemplate? GetTemplate(string templateId)
void AppointCurator(string survivorId, int currentDay)
MuseumArtifact DonateArtifact(string itemId, string name, ArtifactType type, string originStory, float significance, string donorId, int currentDay)
MuseumArtifact? DonateFromTemplate(string templateId, string donorId, int currentDay, string? customOrigin = null)
Exhibition CurateExhibition(string name, ExhibitionTheme theme, IEnumerable<string> artifactIds, int startDay, int durationDays, string description, float moraleBoost = 5f)
bool CloseExhibition(string exhibitionId, int currentDay)
float VisitMuseum(string visitorId, int currentDay)
void TickDay(int currentDay)
float GetHistoricalSignificanceScore()
IReadOnlyList<Exhibition> GetActiveExhibitions()
IReadOnlyList<MuseumArtifact> GetArtifactsOnDisplay()
IReadOnlyList<MuseumEvent> GetEvents()
MuseumEvent RecordEvent(string eventType, int day, string description, IEnumerable<string>? participants, string significance = "moderate")
ShelterMuseumState CaptureState()
void RestoreState(ShelterMuseumState state)
string ArtifactId
string ItemId
string ArtifactName
ArtifactType ArtifactType
string OriginStory
float HistoricalSignificance
float Condition
int DisplayedSince
string DonorId
bool IsOnDisplay
string ExhibitionId
string ExhibitionName
… (32 more)
```

### `PerimeterEarlyWarningEngine`

```
RadarContactSaveState CaptureState()
void SetMode(RadarOperationalMode mode)
void CalibrateSensors(int deltaPermille)
RadarContact? ProcessScanSweep(string sector, int actualDistanceMeters, bool isHostile, bool isStormOrDust, int currentTick, int seededRollPermille)
void ClearExpiredContacts(int currentTick, int maxAgeTicks = 100)
PerimeterEarlyWarningSaveState CaptureState()
void RestoreState(PerimeterEarlyWarningSaveState? state)
string ContactId
string Sector
int DistanceMeters
RadarTargetClassification Classification
int ConfidencePermille
int EstimatedArrivalMinutes
int DetectedTick
int schema_version
RadarOperationalMode Mode
int CalibrationPermille
List<RadarContactSaveState> Contacts
string ContactId
string Sector
int DistanceMeters
RadarTargetClassification Classification
int ConfidencePermille
int EstimatedArrivalMinutes
int DetectedTick
RadarOperationalMode Mode
int CalibrationPermille
```

### `DifficultySettingsSystem`

```
void LoadCatalog(string json)
bool SelectPreset(string presetId)
bool SetCustomScalar(string scalarName, float value)
void LockSettings()
DifficultyScalars GetEffectiveScalars()
DifficultySettingsState CaptureState()
void RestoreState(DifficultySettingsState? saved)
int SchemaVersion
string ActivePresetId
bool IsLocked
bool IsCustom
DifficultyScalars CustomScalars
```

### `FactionDiplomacySystem`

```
void LoadCatalog(string json)
IReadOnlyList<TreatyTemplateDef> GetAllTemplates()
TreatyTemplateDef? GetTemplate(string treatyTypeId)
DiplomaticRelationState GetOrCreateRelation(string factionId)
DiplomaticRelationState SetRelation(string factionId, int trust, string? relationLevel = null)
void AssignEnvoy(string factionId, string survivorId)
DiplomaticMissionRecord DispatchMission(string missionType, string targetFactionId, string envoyId, int day, int durationDays = 3, int envoySkill = 50)
void TickDay(int day)
bool ViolateTreaty(string treatyId, string reason, string violator = "player", int day = 1)
bool RenounceTreaty(string treatyId, int day = 1)
bool HasActiveTreaty(string factionId, string treatyTypeId)
IReadOnlyList<ActiveTreatyRecord> GetActiveTreaties()
IReadOnlyList<ActiveTreatyRecord> GetActiveTreatiesForFaction(string factionId)
IReadOnlyList<FactionTreatyViolationRecord> GetViolations()
IReadOnlyList<DiplomaticMissionRecord> GetMissions()
FactionDiplomacyState CaptureState()
void RestoreState(FactionDiplomacyState? state)
string term_id
string term_type
string description
float value
string condition
string treaty_type_id
string display_name
string category
int base_duration_days
int reputation_requirement
string relation_requirement
List<TreatyTermDef> terms
string description
… (43 more)
```

### `BlackMarketContrabandEngine`

```
static BlackMarketTradeQuote Rejected(BlackMarketActionType action, string rejectionReason)
static int CalculateFencingFeePermille(int fenceTrustPermille)
static BlackMarketTradeQuote QuoteBuy(string itemId, int quantity, int baseUnitValueChits, ContrabandClassification classification, int currentHeatPermille)
static BlackMarketTradeQuote QuoteSell(string itemId, int quantity, int baseUnitValueChits, ContrabandClassification classification, int currentHeatPermille)
static BlackMarketTradeQuote QuoteFence(string itemId, int quantity, int baseUnitValueChits, ContrabandClassification classification, int currentHeatPermille, int fenceTrustPermille)
static FundsResult ExecuteTransaction(FundsLedger ledger, BlackMarketTradeQuote quote, string counterpartyId, int day, ref int currentHeatPermille)
bool IsViable
BlackMarketActionType Action
int Quantity
int UnitPriceChits
int GrossValueChits
int FencingFeeChits
int NetFundsDelta
int GeneratedHeatPermille
int ProjectedHeatPermille
string ReasonKey
string RejectionReason
string Summary
```

### `BlackMarketHeatAttentionEngine`

```
SyndicateHeatRecordSaveState CaptureState()
SyndicateHeatRecord GetOrCreateRecord(string syndicateId, int heatThreshold = 100, string initialLocation = "underground_hideout")
BlackMarketAttentionBand EvaluateBand(int currentHeat, int threshold)
int AddHeat(string syndicateId, int delta, string reason, int currentDay)
void ProcessDailyTick(int currentDay, int campaignSeed = 0, int dailyCooling = DefaultDailyCooling)
bool TriggerRelocation(string syndicateId, int currentDay, int campaignSeed)
int GetRaidRiskPermille(string syndicateId)
bool CheckRaidTrigger(string syndicateId, int rollPermille)
BlackMarketHeatAttentionSaveState CaptureState()
void RestoreState(BlackMarketHeatAttentionSaveState? state)
string SyndicateId
int CurrentHeat
int HeatThreshold
BlackMarketAttentionBand Band
int LastCoolingDay
int TotalHeatAccumulated
bool IsRelocating
int RelocationDaysRemaining
int RelocationCount
string CurrentLocationKey
int schema_version
List<SyndicateHeatRecordSaveState> Syndicates
string SyndicateId
int CurrentHeat
int HeatThreshold
BlackMarketAttentionBand Band
int LastCoolingDay
int TotalHeatAccumulated
bool IsRelocating
int RelocationDaysRemaining
… (2 more)
```

### `ChitPurityAssayEngine`

```
static ChitAssayResult EvaluateAssay(int amount, PurityTier tier, int merchantPerceptionPermille, uint deterministicSeed, int transactionNonce = 1)
static ChitCertificationResult CertifyDilutedScrap(int dilutedAmount)
bool IsAccepted
bool IsCounterfeitDetected
int AcceptedAmount
int ConfiscatedAmount
int PenaltyTrustPermille
int GeneratedHeatPermille
string StatusNotice
bool Success
int CertifiedStandardAlloyChits
int CertificationFeeChits
string Summary
```

### `LoanSharkEnforcerEngine`

```
LoanDebtRecordSaveState CaptureState()
LoanDebtRecord IssueLoan(string creditorFactionId, string debtorId, int principalChits, int termDays, int dailyInterestPermille = DefaultDailyInterestPermille, int gracePeriodDays = DefaultGracePeriodDays, int currentDay = 1)
LoanDebtRecord? GetDebt(string debtId)
void ProcessDailyTick(int currentDay)
FundsResult RepayDebt(string debtId, int amountChits, int currentDay)
bool ForgiveDebt(string debtId, int currentDay, string reason = "creditor_forgiven")
bool IsTradeSanctioned(string creditorFactionId)
bool CheckEnforcerRaidTrigger(string debtId, int rollPermille)
LoanSharkEnforcerEngineSaveState CaptureState()
void RestoreState(LoanSharkEnforcerEngineSaveState? state)
string DebtId
string CreditorFactionId
string DebtorId
int PrincipalChits
int CurrentBalanceChits
int DailyInterestPermille
int IssuedDay
int DueDay
int GracePeriodDays
LoanEscalationStage Stage
int LastInterestAccrualDay
int TotalInterestAccrued
int TotalRepaid
bool BountyPlaced
string? AssociatedBountyId
int EnforcerRaidRiskPermille
int LastEscalationDay
int schema_version
List<LoanDebtRecordSaveState> Debts
string DebtId
… (16 more)
```

### `MigrationConsequenceEngine`

```
int GetMarketDemandMultiplierPermille(string regionId, string itemCategory)
int GetLaborPoolSizeMultiplierPermille(string regionId)
int GetTerritorialFrictionMultiplierPermille(string regionId)
string GetCaravanDemandPriority(string regionId)
bool TryApplyPhaseConsequence(int currentDay, string seasonPhase, string regionId)
MigrationConsequenceSaveState CaptureState()
void RestoreState(MigrationConsequenceSaveState? state)
int schema_version
HashSet<string> AppliedConsequenceKeys
```

### `RestockAllocationEngine`

```
static RestockAllocationResult Allocate(int capacity, IReadOnlyList<RestockCategory> categories)
string ItemId
int CurrentStock
int TargetPar
int AuthoredRestockOrder
string Name
int Weight
int ScarcityFloor
int AuthoredOrder
IReadOnlyList<RestockItemCandidate> Items
int TotalCapacityRequested
int TotalAllocated
IReadOnlyList<string> SortedItemIds
```

### `SeasonalHumanMigrationEngine`

```
int GetRegionPopulationWeight(string regionId)
bool TickDay(int currentDay, string currentSeasonPhase)
SeasonalMigrationSaveState CaptureState()
void RestoreState(SeasonalMigrationSaveState? state)
string Phase
string RegionId
int PopulationDelta
string FactionId
List<SeasonalMigrationEntry> Schedule
int schema_version
int DwellDays
List<FactionMigrationSchedule> Factions
int schema_version
string LastAppliedPhase
int LastTransitionDay
HashSet<string> AppliedTransitionKeys
string LastAppliedPhase
int LastTransitionDay
```

### `SurvivorBarterSystem`

```
void LoadCatalog(string json)
void LoadCatalog(BarterRulesCatalogData catalog)
BarterRuleDefinition? GetRule(string ruleId)
bool SetRule(string ruleId)
BarterOffer? CreateOffer(string offererId, string targetId, IEnumerable<string>? offeredItems = null, IEnumerable<string>? requestedItems = null, FavorType? offeredFavor = null, FavorType? requestedFavor = null, int currentDay = 1)
CompletedBarterTrade? AcceptOffer(string offerId, int currentDay = 1)
bool RejectOffer(string offerId)
bool FulfillFavor(string favorId, int currentDay = 1)
bool RaiseDispute(string tradeId, string complainantId)
TradeReputation GetReputation(string survivorA, string survivorB)
IReadOnlyList<BarterOffer> GetActiveOffers(string survivorId)
IReadOnlyList<FavorObligation> GetPendingFavors(string survivorId)
void TickDay(int currentDay)
SurvivorBarterSaveState CaptureState()
void RestoreState(SurvivorBarterSaveState state)
string Id
string Name
float BaseTrustGainPerTrade
float DisputeTrustPenalty
int MaxActiveOffersPerSurvivor
int OfferExpirationDays
int FavorDeadlineDays
string Description
int SchemaVersion
List<BarterRuleDefinition> Rules
string OfferId
string OffererId
string TargetSurvivorId
List<string> OfferedItemIds
List<string> RequestedItemIds
… (34 more)
```

### `TradeRouteMonopolyEngine`

```
static TradeRouteMonopolyValidationResult Valid()
static TradeRouteMonopolyValidationResult Invalid(string reason)
TradeRouteMonopolyValidationResult ValidateExclusiveGood(TradeRouteContract contract, IReadOnlyCollection<string>? forbiddenSuperiorGoods = null)
int GetCurrentMonopolyPremiumPermille(string routeId)
int CalculateExclusiveGoodUnitValue(int baseUnitValueChits, string routeId)
int CalculateCargoQuota(TradeRouteContract contract)
void RecordDelivery(string routeId, string exclusiveGoodId, int units, int currentDay)
void ProcessDailyRecovery(int currentDay)
TradeRouteMonopolySaveState CaptureState()
void RestoreState(TradeRouteMonopolySaveState? state)
bool IsValid
string FailureReason
string RouteId
string ExclusiveGoodId
int SaturationPermille
int TotalUnitsDelivered
int LastDeliveryDay
int LastRecoveryDay
int schema_version
List<RouteMarketSaturationState> RouteStates
```

### `TradeRouteRiskBindingEngine`

```
static TradeRouteRiskEvaluationResult EvaluateTransitRisk(TradeRouteContract contract, MapRoute route, int escortStrengthPermille, int regionalHostilityPermille)
bool IsTransitViable
int RaidProbabilityPermille
int RouteDisruptionRiskPermille
int ExpectedCargoAttritionPermille
int EscortMitigationPermille
bool TriggersDisruptionAlert
string RiskSummary
```

### `ApprenticeshipCurriculumEngine`

```
LearnerRecord Clone()
static CurriculumSessionResult AdvanceLiteracySession(LearnerRecord learner, int teacherSkillPermille, int manualAvailabilityPermille, int sessionSeed)
static bool EvaluateVocationalCertification(LearnerRecord learner, string tradeId)
static int CalculateManualTranscriptionYield(LiteracyLevel masterLiteracyLevel, int laborHours)
string SurvivorId
LiteracyLevel LiteracyLevel
int ComprehensionPermille
int FatigueSessionCount
int ComprehensionGained
bool AdvancedLiteracyTier
LiteracyLevel NewLiteracyLevel
int FatiguePenaltyPermille
```

### `SurvivorEducationSystem`

```
SurvivorEducationRecord Clone()
void LoadCatalog(string json)
EducationStageType ResolveStageForAge(int age)
SurvivorEducationRecord RegisterLearner(string survivorId, int initialAge)
bool UpdateAge(string survivorId, int newAge)
bool AssignTeacherAndSubject(string studentId, string teacherId, string subjectId, bool isParentChild = false)
EducationSessionResult ConductDailySession(string studentId, ISeededRng rng, bool hasSchoolroom = false, int teacherCompetencyPermille = 1000)
bool EvaluateGraduation(string studentId, int currentDay)
void AdjustShelterKnowledge(int delta)
SurvivorEducationRecord? GetRecord(string survivorId)
IReadOnlyList<SurvivorEducationRecord> GetAllLearners()
CurriculumSubjectDef? GetSubject(string subjectId)
EducationSystemState CaptureState()
void RestoreState(EducationSystemState state)
string StageId
string StageName
int MinAge
int MaxAge
int LearningCapacity
List<string> AvailableSubjects
string SubjectId
string SubjectName
string StageId
List<string> Prerequisites
string UnlockedSkillId
int ProficiencyRequired
double TeachingSpeedModifier
string Description
string SurvivorId
int Age
… (28 more)
```

### `EmergencyAlertSystem`

```
void LoadCatalog(string json)
IReadOnlyCollection<EmergencyAlertTypeDef> GetAllAlertTypes()
EmergencyAlertTypeDef? GetAlertType(string typeId)
ActiveEmergencyAlert RaiseAlert(string typeId, string sourceSystem, string affectedZone, int day, int hour)
bool AcknowledgeAlert(string alertId)
bool ResolveAlert(string alertId)
void TickHour()
EvacuationProtocolState ActivateProtocol(string protocolName, string assemblyZone, int assignedCount)
bool DeactivateProtocol(string protocolId)
ActiveEmergencyAlert? GetHighestPriorityAlert()
IReadOnlyList<ActiveEmergencyAlert> GetActiveAlerts()
IReadOnlyList<ActiveEmergencyAlert> GetAlertHistory()
EmergencyAlertState CaptureState()
void RestoreState(EmergencyAlertState? saved)
string type_id
string display_name
string severity
int base_priority
int response_window_hours
string recommended_protocol
string description
int schema_version
List<EmergencyAlertTypeDef> alert_types
string AlertId
string TypeId
string Severity
int DetectedDay
int DetectedHour
int RemainingHours
string SourceSystem
… (14 more)
```

### `InformantNetworkTradecraftEngine`

```
InformantRecord Clone()
static TradecraftOperationResult ExecuteTradecraftOperation(InformantRecord informant, long simTick, int worldSeed)
static bool DetectCompromisedAsset(InformantRecord informant, int shelterCounterIntelRatingPermille, long simTick, int worldSeed)
static InterrogationOutcome EvaluateInterrogation(InformantRecord captive, bool humaneProtocolsEnforced)
string InformantId
string TargetFactionId
InformantArchetype Archetype
TradecraftMethod ActiveMethod
int LoyaltyPermille
int SuspicionPermille
int IntelligenceYieldPermille
bool IsCompromised
bool IsDoubleAgent
bool Success
int IntelPointsDelivered
int SuspicionDeltaPermille
bool AssetCompromised
bool InterceptedByEnemy
bool ReliableIntelligenceObtained
int CredibilityScorePermille
int MoraleCostPermille
bool FabricatedIntelWarning
```

### `SeasonalCelebrationSystem`

```
void LoadCatalog(string json)
HolidayDef? CheckHolidayForDay(int day)
CelebrationRecord HoldCelebration(string holidayId, string scaleId = "small", int participantCount = 1, ISeededRng? rng = null)
float SkipHoliday(string holidayId)
float CommemorateAnniversary(string typeId, string entityName, int day)
CelebrationSaveState CaptureState()
void RestoreState(CelebrationSaveState? state)
string HolidayId
string Name
int TriggerDay
float BaseMoraleBoost
int FoodCost
int FuelCost
string Description
List<string> AllowedActivities
string TypeId
string Name
float BaseMoraleBoost
string Description
string ScaleId
float CostMultiplier
float MoraleMultiplier
bool IsMemorable
int SchemaVersion
List<HolidayDef> Holidays
List<AnniversaryTypeDef> AnniversaryTypes
List<CelebrationScaleDef> Scales
string CelebrationId
string HolidayId
int Day
… (12 more)
```

### `SubterraneanSubsidenceEngine`

```
static int GetStrataResiliencePermille(StrataType strata)
static int GetShoringMitigationPermille(int shoringLevel)
static SubsidenceEvaluationResult EvaluateSubsidence(ExcavationNodeProfile profile)
static SeismicTriggerOutcome TryTriggerInducedSeismicEvent(ExcavationNodeProfile profile, long simTick, int worldSeed)
static int CalculateDailyIntegrityDecayPermille(ExcavationNodeProfile profile, int waterLevelPermille)
string NodeId
DepthTier Tier
StrataType Strata
int VoidVolumeCubicMeters
int ShoringLevel
int StructuralIntegrityPermille
string NodeId
int SubsidenceRiskPermille
SubsidenceCategory Category
int SurfaceDistortionMm
int InducedSeismicRiskPermille
bool RequiresImmediateEvacuation
bool TremorTriggered
int TremorMagnitude
int IntegrityDamagePermille
int NewIntegrityPermille
bool CaveInOccurred
```

### `AerialReconWindowEngine`

```
static FlightWindowEvaluationResult Evaluate(int baseRangeKm, int airworthinessPermille, int windSpeedKmh, int visibilityPermille, int temperatureCelsius, int payloadWeightKg, int maxPayloadKg)
FlightWindowCondition Condition
bool IsLaunchPermitted
int WindShearRiskPermille
int VisibilityRiskPermille
int IcingRiskPermille
int TotalFlightRiskPermille
int EffectiveRangeKm
int AirdropDriftMeters
int AirworthinessWearPermille
string Advisory
```

### `ColonySystem`

```
void LoadCatalog(string json)
ColonyOutpost EstablishColony(string locationId, string name, ColonyType type, IEnumerable<string>? initialGarrison, float initialSupplies = 50f, int currentDay = 1)
ColonyBuilding? ConstructBuilding(string colonyId, string definitionId, int currentDay = 1)
SupplyLine EstablishSupplyLine(string originId, string destinationId, float cargoCapacity = 100f, float dailyFlow = 10f, int currentDay = 1)
bool SetSupplyLineStatus(string lineId, SupplyLineStatus status)
bool AssignSurvivorToColony(string colonyId, string survivorId)
float TransferSupplies(string colonyId, float amount)
void TickDay(int currentDay)
IReadOnlyList<ColonyOutpost> GetColonies()
ColonyOutpost? GetColony(string colonyId)
ColonyState CaptureState()
void RestoreState(ColonyState state)
string BuildingId
string DefinitionId
string Name
string Category
float Condition
int Capacity
float DefenseBonus
float MoraleBonus
int ConstructionDay
string TypeId
string DisplayName
string Description
float BaseDefense
int BaseCapacity
float RequiredSupplies
string BuildingId
string Name
string Category
… (29 more)
```

### `TerritoryControlSystem`

```
static TerritoryControlSystem FromJson(string territoryJson, string supplyLineJson)
FactionTerritoryDef? GetTerritory(string territoryId)
LocationTerritoryState? GetLocationState(string locationId)
SupplyLineState? GetSupplyLineState(string lineId)
IReadOnlyList<FactionTerritoryDef> GetAllTerritories()
IReadOnlyList<LocationTerritoryState> GetAllLocationStates()
IReadOnlyList<SupplyLineState> GetAllSupplyLines()
bool FortifyLocation(string locationId, int levelDelta = 1)
bool AssignGarrison(string locationId, int garrisonDelta)
bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0)
bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng)
bool RestoreSupplyLine(string supplyLineId)
void TickDay(int currentDay, ISeededRng? rng = null)
string Id
string Faction
string DisplayName
string Classification
string TerritoryScale
string PrimaryResourceInterest
List<string> ControlledNodes
List<string> ControlPoints
List<string> ContestedWith
int BaseControlStrength
double TradeTax
double TravelSafety
string ShiftTrigger
string Description
string Id
string OwningFactionId
string OriginLocationId
… (17 more)
```

### `OilseedPressingEngine`

```
static PressingYieldResult EvaluatePressing(int seedCount, PressToolGrade toolGrade = PressToolGrade.ManualScrewPress, OilseedPressingMode mode = OilseedPressingMode.CulinaryCookingOil)
static ConfitPreservationResult EvaluateConfitPreservation(string baseFoodItemId, int baseUnits, int availableOil, int availableSalt)
static int CalculateSustainableSeedRetention(int harvestCount, int plannedPlotsNextSeason = 4, int seedsPerPlot = 1)
static int EvaluateDietaryLipidContribution(bool hasPressedLipids, int baseCaloriesPercent)
int InputSeedCount
string PrimaryOutputItemId
int PrimaryOutputAmount
int ByproductMealAmount
int ExtractionEfficiencyPermille
int WasteLossPermille
string BaseFoodItemId
int BaseFoodUnitsConsumed
int OilUnitsConsumed
int SaltUnitsConsumed
string PreservedItemId
int PreservedUnitsProduced
int ShelfLifeMultiplierPermille
int SpoilageResistancePermille
```

### `SoilReclamationProfileEngine`

```
static SoilEvaluationResult Evaluate(int initialSalinityPermille, int initialRadionuclideLoadPermille, int initialOrganicMatterPermille, int initialPhTenths, SoilAmendment amendment, int amendmentQuantityUnits = 1)
SoilQualityTier QualityTier
int GerminationViabilityPermille
int CropMutationRiskPermille
int YieldMultiplierPermille
int NetSalinityPermille
int NetRadionuclideLoadPermille
int NetOrganicMatterPermille
int NetPhTenths
bool IsCultivable
```

### `SecondGenerationMilestoneEngine`

```
static MilestoneEvaluationResult EvaluateNextMilestone(ChildProfile profile, int currentDay)
static int CalculateSuccessionReadinessPermille(ChildProfile profile, bool parentDeceased, int parentKinshipPermille = 500)
bool Eligible
MilestoneKind Milestone
DevelopmentStage RequiredStage
float RequiredEducation
int AptitudeBonusPermille
string DossierNote
```

### `ShelterGovernanceEngine`

```
void LoadCatalog(string json, IJsonSerializer serializer)
void RegisterBlocDefinition(IdeologicalBlocDef def)
bool AssignSurvivorToBloc(string survivorId, string blocId)
string? GetSurvivorBloc(string survivorId)
PolicyConsentEvaluation EvaluatePolicyConsent(string scope, string optionId)
void ApplyPolicyEffects(string scope, string optionId)
void AdjustBlocGrievance(string blocId, int deltaBp)
DisputeCaseRecord OpenDispute(string initiatorId, string defendantId, GovernanceDisputeType type, int day)
bool ResolveDispute(string caseId, GovernanceDisputeResolution resolution, int day)
int CalculateStabilityRating()
void Tick(float gameHours)
ShelterGovernanceSaveState CaptureState()
void RestoreState(ShelterGovernanceSaveState? save)
```

### `ClothingWarmthSystem`

```
void RegisterProfile(ClothingItemProfile profile)
bool EquipClothing(string survivorId, string itemId, float condition = 1.0f)
bool UnequipClothing(string survivorId, string itemId)
IReadOnlyList<EquippedClothingInstance> GetEquipped(string survivorId)
void ApplyWetness(string survivorId, float delta)
void DryClothing(string survivorId, float hours)
void DegradeCondition(string survivorId, float wearHours)
float CalculateColdLossReduction(string survivorId)
ClothingWarmthSaveState CaptureState()
void RestoreState(ClothingWarmthSaveState? saved)
```

### `FoodTypeSystem`

```
void LoadCatalog(string json)
IReadOnlyCollection<FoodTypeDef> GetAllFoodTypes()
FoodTypeDef? GetFoodType(string typeId)
void SetStorageTemperature(float tempC)
TrackedFoodItem? AddFood(string foodTypeId, float initialFreshness = 100.0f, string preservation = "none", string storageLocation = "pantry", int day = 1)
TrackedFoodItem? GetFoodItem(string itemId)
float GetTemperatureMultiplier(float tempC, float sensitivity)
float GetPreservationMultiplier(string preservation, string foodTypeId)
void TickDay(int day)
string CheckFoodSafety(string itemId)
int GetFreshFoodCount()
int GetSpoiledFoodCount()
FoodTypeSystemState CaptureState()
void RestoreState(FoodTypeSystemState? saved)
string type_id
string display_name
string category
float base_spoilage_days
float temperature_sensitivity
List<string> optimal_preservations
string description
int schema_version
List<FoodTypeDef> food_types
string ItemId
string FoodTypeId
float FreshnessPercent
string PreservationMethod
string StorageLocation
int DayAdded
bool IsSpoiled
… (4 more)
```

### `CampaignLegacySystem`

```
void RegisterTrait(LegacyTrait trait)
bool TryGetTrait(string traitId, out LegacyTrait? trait)
void LoadCatalog(string json)
static CampaignLegacySystem LoadFromDirectory(string dataDir, IFileIO fileIO, ISeededRng? rng = null)
void ArchiveCampaign(CampaignLegacy legacy, ISeededRng? rng = null)
StartingCampaignContext PrepareNewGameContext()
CampaignLegacyState CaptureState()
void RestoreState(CampaignLegacyState? state)
string campaignId
string endingId
int daysSurvived
int survivorCount
int deathsRecorded
List<string> shelterImprovements
List<string> legacyTraits
List<string> campaignFlags
int completionDay
string id
string name
string description
string source
string effect_type
float magnitude
string evolution_target_id
int generation
bool inherited
int schema_version
List<CampaignLegacy> completedCampaigns
List<LegacyTrait> activeLegacyTraits
List<string> inheritedImprovements
… (9 more)
```

### `MaritimeExplorationSystem`

```
void LoadCatalog(string json)
IReadOnlyList<MaritimeZoneDef> GetAllZoneDefs()
MaritimeZoneDef? GetZoneDef(string zoneId)
bool DiscoverZone(string zoneId)
bool IsZoneDiscovered(string zoneId)
DiveSiteRecord RegisterDiveSite(string siteId, string siteName, string zoneId, DiveSiteType type, float depthMeters, float hazardLevel, int maxExplorations = 3, IEnumerable<string>? lootItemIds = null)
bool DiscoverSite(string siteId, int day = 1)
DiveSiteRecord? GetSite(string siteId)
DivingEquipmentRecord RegisterEquipment(string equipmentId, string equipmentType, float depthRating = 50.0f, float protectionRating = 50.0f)
DivingEquipmentRecord? GetEquipment(string equipmentId)
bool ValidateExpedition(string siteId, IReadOnlyList<string> diverIds, IReadOnlyList<string> equipmentIds, out string validationMessage)
MaritimeExplorationState CaptureState()
void RestoreState(MaritimeExplorationState state)
string zone_id
string name
string zone_type
float water_temp_celsius
float radiation_level
float current_strength
float visibility
List<string> dive_sites
string required_equipment_type
float min_depth_meters
float max_depth_meters
string description
int schema_version
List<MaritimeZoneDef> zones
string SiteId
string SiteName
string ZoneId
… (40 more)
```

### `ClinicalWardTriageEngine`

```
ClinicalWardState Clone()
static TriageEvaluationResult EvaluatePatientTriage(int traumaSeverityPermille, int vitalStabilityPermille, bool isContagious, ClinicalWardState ward)
static SurgicalReadinessResult EvaluateSurgicalPreparation(ClinicalWardState ward, int procedureComplexityPermille, int patientConditionPermille, int surgerySeed)
static int ComputeBedTurnoverCapacity(ClinicalWardState ward, int averageLengthOfStayDays)
static int CalculateNosocomialInfectionRisk(WardCleanlinessGrade cleanliness, int wardOccupancyPermille, int sterileSupplyPermille)
string WardId
int TotalBeds
int OccupiedBeds
int SterileSupplyStockPermille
int StaffingReadinessPermille
WardCleanlinessGrade Cleanliness
int IsolationBedsTotal
int IsolationBedsOccupied
TriagePriorityTier AssignedPriority
int EstimatedUrgencyMinutes
bool BedAvailable
bool RequiresIsolation
string RecommendationNotice
bool IsApprovedForSurgery
int ShockRiskPermille
int InfectionRiskPermille
int SterileSuppliesConsumedPermille
string BottleneckReason
```

### `DependencyTaperWithdrawalEngine`

```
TaperProgramState Clone()
static int ComputeRecommendedStepDown(int dependencyPermille, bool isMedicallySupervised, int substituteMedicineAvailablePermille)
static TaperDayResult AdvanceTaperDay(TaperProgramState program, bool peerSupportRunToday)
static DependencySeverityTier ClassifyDependencySeverity(int dependencyPermille)
static CarePolicyPosture RecommendCarePolicy(int criticalCaseCount, int totalShelterPopulation, CarePolicyPosture currentPosture)
string SurvivorId
int DependencyPermille
int DailyStepDownPermille
int CurrentSubstituteDosePermille
int PeerSupportSessionsCompleted
bool IsMedicallySupervised
int TaperDaysElapsed
int NewSubstituteDosePermille
WithdrawalSymptomBand Symptoms
bool TaperComplete
bool RequiresMedicalEscalation
int ProductivityPenaltyPermille
```

### `PalliativeCareDignityEngine`

```
PalliativePatientRecord Clone()
static DailyPalliativeOutcome AdvanceDailyCare(PalliativePatientRecord patient, int medicineAvailabilityPermille, int caregiverSkillPermille)
static void EvaluateGriefStageProgression(PalliativePatientRecord patient, long simTick, int worldSeed)
static MemorialLegacyEcho CalculateMemorialEcho(PalliativePatientRecord deceased)
string SurvivorId
int DaysRemainingPrognosis
int PainLevelPermille
int LucidityPermille
int DignityIndexPermille
PalliativeCareProtocol ActiveProtocol
string FinalWishQuestId
bool FinalWishFulfilled
GriefStage CurrentGriefStage
int DaysInCurrentGriefStage
int PainDeltaPermille
int LucidityDeltaPermille
int DignityDeltaPermille
bool PrognosisExpired
string SurvivorId
int MoraleDelta
string MemorialJournalKey
bool DiedInDignity
```

### `ProstheticConditionWearEngine`

```
static ProstheticWearEvaluationResult EvaluateDailyWear(int currentConditionPermille, ProstheticComplexityClass complexity, int laborIntensityPermille, int maintenanceQualityPermille)
int NetConditionPermille
int WearDeltaPermille
int BiomechanicalEfficiencyPermille
int FailureRiskPermille
bool RequiresImmediateMaintenance
string MaintenanceStatus
```

### `RehabilitationProgressionEngine`

```
static RehabRecord StartRehabilitation(string prostheticTypeKey)
static RehabRecord AdvanceDaily(RehabRecord? current, float resilienceMultiplier = 1.0f, int days = 1, int fittingDurationDays = DefaultFittingDurationDays, int adaptationDurationDays = DefaultAdaptationDurationDays)
static float GetQualityFactor(RehabRecord? rehab)
```

### `SurgicalGraftRejectionEngine`

```
SurgicalGraftRecordSaveState CaptureState()
static int GetBaseBiocompatibilityPermille(GraftBiocompatibilityTier tier)
SurgicalGraftRecord PerformGraft(string survivorId, string limbKey, string donorSourceId, GraftBiocompatibilityTier tier, int currentDay)
SurgicalGraftRecord? GetGraft(string graftId)
IReadOnlyList<SurgicalGraftRecord> GetGraftsForSurvivor(string survivorId)
bool AdministerImmunosuppressant(string graftId, int dosePermille, int currentDay)
void ProcessDailyTick(int currentDay, Func<string, int, int>? seededRngRoll = null)
SurgicalGraftRejectionEngineSaveState CaptureState()
void RestoreState(SurgicalGraftRejectionEngineSaveState? state)
string GraftId
string SurvivorId
string LimbKey
string DonorSourceId
GraftBiocompatibilityTier Tier
int GraftDay
GraftStatus Status
int IntegrationProgressPermille
int RejectionRiskPermille
int ImmunosuppressantLevelPermille
int DaysWithoutImmunosuppressant
int LastUpdateDay
int schema_version
List<SurgicalGraftRecordSaveState> Grafts
string GraftId
string SurvivorId
string LimbKey
string DonorSourceId
GraftBiocompatibilityTier Tier
int GraftDay
GraftStatus Status
… (5 more)
```

### `ModSupportSystem`

```
void LoadSpecification(string json)
ModContractValidationResult ValidateManifest(ModManifest? manifest)
ModRegistration RegisterMod(ModManifest manifest, string currentGameVersion = "1.0.0", int currentContractVersion = 1)
bool SetModEnabled(string modId, bool enabled)
void ReevaluateDependencies()
IReadOnlyList<string> ResolveLoadOrder()
IReadOnlyList<ModConflict> DetectConflicts()
ModSaveState CaptureState()
void RestoreState(ModSaveState? state)
string PolicyId
string Description
int SchemaVersion
string SchemaName
int CurrentModContractVersion
string SupportedGameVersions
List<string> RequiredManifestFields
List<string> OptionalManifestFields
List<string> AllowedCatalogs
List<ModConflictPolicyDef> ConflictPolicies
string ModId
string DisplayName
string Version
int LoadOrder
bool AllowOverrides
ModStatus Status
string StatusReason
List<string> Catalogs
List<string> Dependencies
ModManifest? Manifest
string ModIdA
… (9 more)
```

### `LetterDeliverySystem`

```
LetterDeliveryRecord? GetRecord(string letterId)
LetterDeliveryRecord DiscoverLetter(string letterId, int day, string recipientSurvivorId = "")
bool AddressLetter(string letterId, string recipientSurvivorId, int day)
bool DeliverLetter(string letterId, int day, string notes = "", float customMoraleDelta = 6.0f)
bool WithholdLetter(string letterId, int day, string notes = "")
bool MarkUnanswered(string letterId, int day, string notes = "")
LetterDeliverySystemState CaptureState()
void RestoreState(LetterDeliverySystemState? state)
```

### `NpcMemorySystem`

```
void LoadDialogueCatalog(string json)
IReadOnlyList<NpcMemoryDialogueDef> GetAllDialogueTemplates()
IReadOnlyList<NpcMemoryDialogueDef> GetDialogueTemplatesForTone(NpcDialogueTone tone)
NpcRelationship GetOrCreate(string npcId)
NpcRelationship? Get(string npcId)
NpcMemoryEntry RecordAction(string npcId, NpcMemoryActionType action, int day, string targetId = "", float intensity = 50f, params string[] tags)
void TickDailyDecay(int currentDay, float decayRatePerDay = 1.0f)
bool Forgive(string npcId, string reason, float restitutionAmount = 0f)
float GetTradePriceMultiplier(string npcId)
bool IsTradeRefused(string npcId)
NpcDialogueTone GetDialogueTone(string npcId)
NpcMemorySaveState CaptureState()
void RestoreState(NpcMemorySaveState? state)
string NpcId
NpcMemoryActionType Action
string TargetId
int Day
float Intensity
bool Forgiven
List<string> Tags
string NpcId
float PersonalTrust
float GrudgeLevel
float FavorOwed
int LastInteractionDay
List<NpcMemoryEntry> Memories
string tone
string template_id
string text
int schema_version
… (5 more)
```

### `SurvivorLetterDeliverySystem`

```
SurvivorLetterRecordState GetOrCreateRecord(string letterId)
string GetState(string letterId)
IReadOnlyList<SurvivorLetterRecordState> GetAllRecords()
IReadOnlyList<SurvivorLetterRecordState> GetRecordsByState(string deliveryState)
bool MarkFound(string letterId, int day)
bool TryAddressToSurvivor(string letterId, IEnumerable<DwellerAddressCandidate> livingDwellers)
bool AssignRecipientExplicit(string letterId, string survivorId)
bool Deliver(string letterId, int day, Action<string, float>? applyMorale = null, float moraleBonus = DefaultDeliveryMoraleBonus)
bool Withhold(string letterId, int day, Action<string, float>? applyMorale = null, float moralePenalty = DefaultWithholdMoralePenalty)
bool MarkUnanswered(string letterId, int day)
SurvivorLetterDeliverySaveState CaptureState()
void RestoreState(SurvivorLetterDeliverySaveState? save)
string SurvivorId
string Name
string Role
bool IsAlive
```

### `SleepAcousticRestEngine`

```
SleepingQuarterState Clone()
static int CalculateAcousticAttenuation(int sourceDecibels, int wallSoundproofingPermille, int doorSoundproofingPermille)
static int CalculateCrowdingDensity(int occupants, int roomAreaSqMetres)
static SleepQualityResult EvaluateSleepQuality(SleepingQuarterState quarter, bool isQuietHours, QuietHoursCompliance compliance, int hoursSlept)
static int ComputeNetFatigueRecovery(int baseFatigueRecovery, int fatigueMultiplierPermille)
string RoomId
int RoomAreaSquareMetres
int AssignedOccupants
int WallSoundproofingPermille
int DoorSoundproofingPermille
int AmbientNoiseLevelDecibels
int DarknessQualityPermille
bool HasSensoryReliefKit
int SleepQualityIndexPermille
SleepEnvironmentBand EnvironmentBand
int NetDecibelsAtBunk
int CrowdingPenaltyPermille
int FatigueRestorationMultiplierPermille
int MoraleRestorationBonus
```

### `CommonTableRationingEngine`

```
static int EvaluateLipidAbsorptionModifier(bool hasLipids)
static NutritionEvaluationResult Evaluate(IEnumerable<FoodCategory> consumedCategories, RationLevel policy, int cookSkillPermille, int populationCount, int consecutiveLeanDays = 0, bool hasRationInequality = false)
DiversityTier DiversityTier
RationLevel RationLevel
int UniqueCategoryCount
int BaseCaloriesPercent
int NetCalorieIntakePercent
int DeficiencyRiskPermille
int MoraleDeltaPermille
int GrievanceProbabilityPermille
int CookWasteReductionPermille
int RequiredFoodUnits
```

### `PrecisionGlassworksOpticsEngine`

```
GlassBatchState Clone()
static void AdvanceAnnealingStage(GlassBatchState batch, int kilnTemperaturePermille)
static LensGrindingResult GrindCorrectionLens(GlassPurityTier glassPurity, VisionCorrectionBand targetBand, int grinderSkillPermille, int abrasiveGritAvailablePermille, int grindSeed)
static TheodoliteCalibrationResult CalibrateTheodolite(GlassPurityTier lensPurity, int calibratorSkillPermille)
static GlassPurityTier MinimumPurityForVision(VisionCorrectionBand band)
string BatchId
int SilicaPurityPermille
int AnnealingStage
int ThermalShockRiskPermille
bool IsCracked
GlassPurityTier ResultingTier
int OpticalPrecisionPermille
bool MeetsPrescriptionTolerance
int GritConsumedPermille
bool LensCracked
int CalibrationAccuracyPermille
int SurveyRangeMetres
bool IsFieldReady
```

### `ConfessionSecretSystem`

```
bool IsDiscovered(string secretId)
bool IsResolved(string secretId)
SecretChoiceRecord? GetChoice(string secretId)
bool DiscoverSecret(string secretId, int currentDay, string sourceId = "")
bool ExposeSecret(string secretId, int currentDay, NeedsSystem? needs = null, GuiltInsomniaSystem? guilt = null, Action<string, float>? onFactionStandingChanged = null)
bool BlackmailSecret(string secretId, int currentDay, MoralBranchingSystem? moral = null)
bool KeepSecret(string secretId, int currentDay, SurvivorRelationsSystem? relations = null, string confidantSurvivorId = "")
bool ResolveInterpersonal(string secretId, int currentDay, bool forgive, string confessorId, string listenerId, SurvivorRelationsSystem? relations = null, NeedsSystem? needs = null)
ConfessionSecretState CaptureState()
void RestoreState(ConfessionSecretState state)
```

### `PublicBroadsheetPressEngine`

```
TypeTrayState Clone()
static PrintRunResult ExecutePrintRun(TypeTrayState tray, PublicationKind kind, int targetCopies, int compositorSkillPermille, int shelterPopulation)
static int CalculateRumorDebunkingCorrection(int rumorStrengthPermille, int pamphletAudienceReachPermille, int evidenceQualityPermille)
static void RestoreTypeTray(TypeTrayState tray, int freshTypePiecesAdded)
int TypePiecesAvailable
int TypeWearPermille
int InkReservoirPermille
int PaperStockPermille
int CopiesPrinted
int AudienceReachPermille
int MoraleStabilizationPermille
int InkConsumedPermille
int PaperConsumedPermille
int TypeWearIncurredPermille
bool BlockedByShortage
string ShortageReason
```

### `PsychologicalProfileSystem`

```
void LoadCatalog(string json)
SurvivorPsychologicalProfile EnsureProfile(string survivorId)
SurvivorPsychologicalProfile? GetProfile(string survivorId)
bool RecordTraumaEvent(string survivorId, string traumaTag, float traumaSeverity, int currentDay, ISeededRng rng)
PhobiaTriggerResult EvaluatePhobiaExposure(string survivorId, string triggerCondition)
bool TeachCopingMechanism(string survivorId, string mechanismId, string source, int currentDay)
bool ConductTherapySession(string survivorId, string phobiaId, float therapistSkill)
float GetProfileResilienceScore(string survivorId)
IReadOnlyList<PhobiaDef> GetAllPhobias()
IReadOnlyList<CopingMechanismDef> GetAllCopingMechanisms()
PsychologyState CaptureState()
void RestoreState(PsychologyState state)
string effect
float value
string phobia_id
string phobia_name
string description
string trigger_condition
List<string> developed_from_traumas
int severity_threshold
List<PhobiaEffectDef> effects
string mechanism_id
string mechanism_name
int effectiveness
List<string> side_effects
string learned_from
int schema_version
List<PhobiaDef> phobia_definitions
List<CopingMechanismDef> coping_mechanisms
string phobia_id
… (23 more)
```

### `RadioPropagationEngine`

```
static float GetWeatherAttenuation(WeatherKind weather)
static float GetWeatherAttenuation(string weatherCondition)
static float GetTerrainAttenuation(string terrainClass)
static float GetDistanceAttenuation(int distanceTicks)
static RadioPropagationResult EvaluatePropagation(DistressSignalDefinition signal, RadioPropagationContext context, float frequencyOffsetMhz = 0f)
static void UpdateMonotonicClarity(ActiveDistressSignal activeSignal, float observedClarity)
int Day
string WeatherCondition
WeatherKind Weather
int DistanceTicks
string TerrainClass
float AmbientNoise
float OperatorSkill
float CarrierStrength
float AttenuationFactor
float NoiseFloor
float EffectiveVu
bool IsLocked
float Clarity
int AudibleFragmentIndex
string Explanation
```

### `SessionDurabilityManager`

```
void SetActiveSlot(string slotId)
SaveSlotDefinition? GetSlot(string slotId)
bool RegisterOrUpdateSlot(string slotId, string displayName, int campaignDay, int survivorCount, string checksum, string profileId = "default")
bool CreateBackup(string slotId, string backupChecksum)
bool RecordInterruptedWrite(string slotId, string reason)
bool TryRecoverBackup(string slotId, out string recoveredChecksum)
bool ValidateSlotChecksum(string slotId, string candidateChecksum)
void RecordDayAdvance(int day, float durationMs, long stateBytes)
SoakStabilityReport EvaluateSoakStability(float maxAllowedP95Ms = 2500f, float maxAllowedSlopeBytesPerDay = 50000f)
SessionDurabilityState CaptureState()
void RestoreState(SessionDurabilityState state)
string SlotId
string ProfileId
string DisplayName
int CampaignDay
int SurvivorCount
string LastSavedUtc
string Checksum
bool IsCorrupt
bool HasBackup
string BackupChecksum
int Day
float AdvanceDurationMs
long TrackedStateBytes
int SchemaVersion
string ActiveSlotId
List<SaveSlotDefinition> Slots
List<SessionSoakSample> SoakSamples
List<string> CorruptedSlotIds
int MaxSlots
… (7 more)
```

### `OutpostSettlementSystem`

```
static OutpostSettlementSystem FromJson(string json)
OutpostDef? GetDefinition(string outpostId)
OutpostInstance? GetInstance(string outpostId)
IReadOnlyList<OutpostDef> GetAllDefinitions()
IReadOnlyList<OutpostInstance> GetAllInstances()
bool EstablishOutpost(string outpostId, Func<string, int, bool>? costConsumer = null)
bool AssignGarrison(string outpostId, string survivorId, Func<string, bool>? fitnessCheck = null)
bool RelieveGarrison(string outpostId, string survivorId)
bool SupplyOutpost(string outpostId, int rationsDelivered)
void TickDay(Func<string, int, int>? centralRationSupplyProvider = null)
bool SimulateRisk(string outpostId, int dangerRating, ISeededRng rng)
bool AbandonOutpost(string outpostId)
string Id
string Name
string GraphNodeId
int MaxGarrisonBunks
int DailySupplyDemand
int DefenseRating
int RadioRelayRange
string OutpostId
string GraphNodeId
bool IsEstablished
int ConditionPermille
List<string> GarrisonSurvivorIds
int DaysSinceSupply
bool IsStarving
bool IsOverrun
int RationReserve
Action<string>? OnOutpostOverrunSeam
Action<string>? OnOutpostStarvingSeam
```

### `ChemicalReagentSynthesisEngine`

```
SynthesisReactorState Clone()
static SynthesisReactionStepResult EvaluateReactionStep(SynthesisReactorState state, int targetBatchKg, int reactantRatioPermille, int operatorSkillPermille)
static MassBalanceResult CalculateMassBalance(int precursorInputKg, int stoichiometricRatioPermille, int conversionRatePermille)
static ReagentPurityGrade DeterminePurityGrade(int feedstockPurityPermille, int catalystActivityPermille, int processStabilityPermille)
static int CalculateWasteNeutralizationDemand(int acidicWasteLitres, int acidConcentrationPermille)
string ReactorId
int OperatingTemperaturePermille
int OperatingPressurePermille
int CatalystActivityPermille
int FeedstockPurityPermille
int CoolingCapacityPermille
SynthesisReactorHazardState HazardState
int OutputBatchKg
ReagentPurityGrade PurityGrade
SynthesisReactorHazardState HazardState
int CatalystDegradationPermille
int NeutralizingWasteVolumeLitres
bool IsReactionAborted
int TheoreticalYieldKg
int ActualYieldKg
int ConversionRatePermille
int UnreactedPrecursorKg
```

### `CupolaFoundryEngine`

```
CupolaFurnaceState Clone()
bool TryStartFoundryBatch(string chargeId, string moldId, string workerId = "")
void TickDay(int currentDay)
CupolaCastResult? TryTapMold()
bool AbortBatch(string reason)
bool TryServiceCupola(bool includeDescale, string workerId = "")
CupolaFoundrySave CaptureState()
void RestoreState(CupolaFoundrySave? save)
```

### `DisasterResponseSystem`

```
DisasterEventDto Clone()
EmergencyProtocolDto Clone()
void LoadCatalog(string json)
DisasterEventDto TriggerDisaster(DisasterType type, DisasterSeverity severity, List<string> affectedRooms, int currentDay)
bool ActivateProtocol(EmergencyProtocolType type)
bool DeactivateProtocol(EmergencyProtocolType type)
bool IsProtocolActive(EmergencyProtocolType type)
bool TickDisaster(string disasterId, double baseMitigationLabor, int currentDay, ISeededRng rng)
double CalculateRoomDamage(DisasterEventDto disaster, string roomId)
void AdjustResilience(double delta)
DisasterEventDto? GetDisaster(string disasterId)
IReadOnlyList<DisasterEventDto> GetAllDisasters()
DisasterResponseState CaptureState()
void RestoreState(DisasterResponseState state)
string DisasterId
DisasterType Type
DisasterSeverity Severity
string Name
List<string> AffectedRoomIds
int StartedDay
int DurationDays
double MitigationInvested
double MitigationRequired
double DamageAccumulated
DisasterStatus Status
int ResolvedDay
string ProtocolId
EmergencyProtocolType Type
string Name
bool IsActive
… (14 more)
```

### `EmergencyMusterReadinessEngine`

```
static MusterReadinessEvaluationResult Evaluate(int populationCount, int activeWardenCount, int daysSinceLastDrill, EmergencyDrillType lastDrillType, int routeClearancePermille, int refugeChamberIntegrityPermille, int recentDrillCountIn14Days = 0)
int CompositeReadinessScorePermille
int EstimatedEvacuationMinutes
int MissingSurvivorRiskPermille
int CascadeInterventionMarginMinutes
int ComplianceFatiguePermille
bool IsReadinessCertified
string ReadinessAdvisory
```

### `KilnFiringEngine`

```
KilnBatchState Clone()
static void AdvanceFiringStage(KilnBatchState batch, int kilnTemperaturePermille, int fuelAvailablePermille)
static LimeCalcinationResult CalcinateLimestone(int limestoneKg, int kilnTemperaturePermille, int soakHours, int fuelAvailablePermille)
static int CalculateRefractoryLiningWear(KilnLoadKind loadKind, int kilnTemperaturePermille)
string BatchId
KilnLoadKind LoadKind
int RawMaterialQualityPermille
int FiringStage
int HeatWorkPermille
bool IsWaster
DrawGrade ResultingGrade
int QuicklimeYieldKg
int ResidualCarbonatePermille
bool IsFullyCalcined
int FuelConsumedPermille
```

### `MechanicalPowerDrivelineEngine`

```
DrivelineBranchState Clone()
static int CalculatePrimeMoverOutput(PrimeMoverType moverType, int resourceAvailabilityPermille, int operatorSkillPermille)
static PowerTransmissionResult TransmitMechanicalPower(DrivelineBranchState branch, int inputPowerWatts, int operatingHours)
static MachiningToleranceResult EvaluateMachiningTolerance(DrivelineBranchState branch, int requiredToleranceMicrons)
static void ApplyLubricationMaintenance(DrivelineBranchState branch, int freshLubricantVolumeMl, int alignmentAdjustmentPermille)
string BranchId
DrivelineCouplingKind CouplingKind
int ShaftLengthMetres
int LubricationQualityPermille
int BearingWearPermille
int AlignmentPrecisionPermille
int MachineToolRunoutMicrons
int OperatingHoursAccumulated
int InputTorqueWatts
int DeliveredTorqueWatts
int TransmissionEfficiencyPermille
int FrictionLossWatts
int LubricantDegradationPermille
int BearingWearIncurredPermille
bool MeetsTolerance
int RunoutDriftMicrons
int EffectiveToolPrecisionPermille
string BottleneckReason
```

### `PowerLoadSheddingEngine`

```
static LoadSheddingEvaluationResult Evaluate(int availableGenerationKw, IEnumerable<SubgridLoadDemand> demands, int gridWearPermille = 0)
string ConsumerId
LoadPriorityTier Priority
int DemandKw
int AvailableGenerationKw
int TotalDemandKw
int ServedLoadKw
int ShedLoadKw
int BrownoutRiskPermille
int CascadingTripRiskPermille
int EnergyPovertyMoralePenaltyPermille
bool IsBlackout
IReadOnlyList<string> ShedConsumers
```

### `ShelterExpansionSystem`

```
ExpansionRoomDto Clone()
ConstructionProjectDto Clone()
void LoadCatalog(string json)
bool IsCellOccupied(int gridX, int gridY, int depthLevel)
ConstructionProjectDto? StartRoomConstruction(string blueprintId, int gridX, int gridY, int depthLevel, int currentDay)
ConstructionProjectDto? StartRenovation(string targetRoomId, int currentDay)
ConstructionProjectDto? StartUpgrade(string targetRoomId, string upgradeId, int currentDay)
ConstructionProjectDto StartDepthExcavation(int currentDay)
bool ProgressProject(string projectId, double dailyLabor, int currentDay)
void DegradeRoomCondition(string roomId, double wearAmount)
void AdjustStability(double delta)
ExpansionRoomDto? GetRoom(string roomId)
IReadOnlyList<ExpansionRoomDto> GetAllRooms()
ConstructionProjectDto? GetProject(string projectId)
IReadOnlyList<ConstructionProjectDto> GetAllProjects()
ShelterExpansionState CaptureState()
void RestoreState(ShelterExpansionState state)
string BlueprintId
string Name
string RoomTypeId
string Description
double BaseLaborDays
int MinDepthLevel
int MaxDepthLevel
double StabilityCost
string UpgradeId
string Name
string Description
double LaborDays
double StabilityRestored
… (38 more)
```

### `ShelterIdentitySystem`

```
void LoadCatalog(string json, IJsonSerializer serializer)
void RegisterOrigin(ShelterOriginDef def)
ActionResult SetShelterName(string name)
ActionResult SetMotto(string motto)
ActionResult SetEmblem(string symbol, string color)
ActionResult SelectOrigin(string originId, int day = 1, string founderSurvivorId = "")
ShelterOriginDef? GetSelectedOrigin()
void RecordFactionReputation(string factionId, int delta)
int GetFactionReputation(string factionId)
void RecordCommunityAction(string actionType, int magnitude = 1)
void AdjustInfamy(int delta)
List<string> GetKnownForTags()
string FormatText(string template)
ShelterIdentityState CaptureState()
void RestoreState(ShelterIdentityState? saved)
```

### `ShelterMaintenanceSystem`

```
void LoadCatalog(string json)
IReadOnlyCollection<ShelterComponentDef> GetAllDefinitions()
ShelterComponentDef? GetDefinition(string componentId)
ShelterComponentState? GetComponent(string componentId)
void TickDay(int currentDay, float weatherStressMult = 1.0f, float radiationStressMult = 1.0f)
bool PerformMaintenance(string componentId, string actionType, float skillLevel, int day)
float GetAverageIntegrity()
IReadOnlyList<ShelterComponentState> GetFailedComponents()
IReadOnlyList<ShelterComponentState> GetWarningComponents()
ShelterMaintenanceState CaptureState()
void RestoreState(ShelterMaintenanceState? saved)
string component_id
string display_name
string component_type
float max_condition
float base_degradation_rate
float warning_threshold
float failure_threshold
List<string> repair_cost_items
int repair_time_hours
string description
int schema_version
List<ShelterComponentDef> components
string ComponentId
float Condition
int LastMaintainedDay
bool IsOperational
bool HasWarning
string ActionId
string ComponentId
… (8 more)
```

### `TrophySystem`

```
void ClearCatalog()
void RegisterTrophy(TrophyDefinition trophy)
bool LoadCatalogFromJson(string json)
TrophyDefinition? GetTrophy(string trophyId)
TrophyDefinition? GetTrophyForSpecies(string speciesId)
TrophyDefinition? GetTrophyByItemId(string itemId)
string GetTrophyRecipeForSpecies(string speciesId)
float GetTrophyMoraleModifier(string itemId)
bool IsAwarded(string trophyId)
TrophyAwardRecord? RecordQuarryPreserved(string speciesId, int day = 1)
TrophySaveState CaptureState()
void RestoreState(TrophySaveState? saved)
string Id
string SpeciesId
string TrophyItemId
string RecipeId
string DisplayName
string Description
float LocalizedMoraleDelta
string RecommendedRoomId
string ConditionKind
int SchemaVersion
string CollectionId
List<TrophyDefinition> Trophies
string TrophyId
string SpeciesId
string RecipeId
string ItemId
string DisplayName
float LocalizedMoraleDelta
… (2 more)
```

### `SpiritualRitualCalendarEngine`

```
static HolyDayObservance? GetScheduledObservance(int campaignDay, string movementId)
static RitualEffectResult EvaluateRitualObservance(SpiritualRitualDefinition ritual, int daysSinceLastPerformed, int shelterMoralePermille = 500)
static int CalculateIdeologicalFrictionMitigation(string movementA, string movementB, bool sharedRitualObserved)
string HolyDayId
string MovementId
string Title
int DayOfYear
int MoraleBonusPermille
int FrictionReductionPermille
bool IsAllowed
int MoraleDeltaPermille
int FrictionReductionPermille
int CooldownRemainingDays
string Reason
```

### `AgingSystem`

```
void LoadCatalog(string json)
IReadOnlyList<LifeStageDef> GetAllLifeStages()
IReadOnlyList<AgingMilestoneDef> GetAllMilestones()
SurvivorAgingRecord RegisterSurvivor(string survivorId, int baseAgeYears, int joinedDay)
SurvivorAgeProfile EvaluateSurvivor(string survivorId, int currentDay)
bool IsRetired(string survivorId)
bool RetireSurvivor(string survivorId, int currentDay)
void TickDay(int currentDay, IEnumerable<string>? livingSurvivorIds = null)
bool HasLivingElderMentor(int currentDay, IEnumerable<string> livingSurvivorIds)
AgingState CaptureState()
void RestoreState(AgingState? saved)
string stage_id
string stage_enum
string display_name
int min_age
int max_age
float physical_labor_multiplier
float fatigue_accumulation_multiplier
float mentorship_xp_bonus
bool can_retire
string description
int age
string label
int morale_bonus
int schema_version
int days_per_year
int min_retirement_age_years
List<LifeStageDef> life_stages
List<AgingMilestoneDef> milestones
string SurvivorId
… (11 more)
```

### `AntenatalMaternalHealthEngine`

```
MaternalPregnancyState Clone()
static GestationTrimester ResolveTrimester(int gestationDays, bool isPostpartum = false)
static TrimesterProgressionResult AdvancePregnancyDay(MaternalPregnancyState state, int nutritionIntakePermille, int restHoursProvided)
static BirthResolutionResult ResolveBirthDelivery(MaternalPregnancyState state, int birthSeed)
static int ComputePostpartumRecoveryRate(int daysPostpartum, int careQualityPermille, int nutritionPermille)
string MotherSurvivorId
int GestationDays
int MaternalNutritionReservePermille
int MaternalFatiguePermille
int ShelterSanitationQualityPermille
int MedicalSupervisionQualityPermille
int AccumulatedStressPermille
bool IsPostpartum
GestationTrimester Trimester
MaternalHealthStatus HealthStatus
int DailyCaloricDemandMultiplierPermille
int ComplicationRiskPermille
bool IsLaborReady
BirthOutcomeClassification Outcome
int NeonatalVigorPermille
int MaternalExhaustionPermille
int PostpartumRecoveryDaysNeeded
```

### `BackstorySystem`

```
void LoadCatalog(string json)
IReadOnlyCollection<OccupationDef> GetAllOccupations()
IReadOnlyCollection<LifeExperienceDef> GetAllExperiences()
IReadOnlyCollection<BackstoryTemplateDef> GetAllTemplates()
OccupationDef? GetOccupation(string id)
LifeExperienceDef? GetExperience(string id)
BackstoryTemplateDef? GetTemplate(string id)
SurvivorBackstory AssignFromTemplate(string survivorId, string templateId, int day)
SurvivorBackstory AssignCustom(string survivorId, string occupationId, IEnumerable<string> experienceIds, string preWarLife, string definingMoment, string reasonForSurvival, int day)
SurvivorBackstory? GetBackstory(string survivorId)
BackstoryProjection ProjectEffects(string survivorId)
bool RevealSecret(string survivorId, string secretId)
BackstoryState CaptureState()
void RestoreState(BackstoryState? saved)
string skill_id
int bonus
string skill_id
int penalty
string occupation_id
string label
List<SkillBonusDef> skill_bonuses
List<SkillPenaltyDef> skill_penalties
List<string> starting_traits
List<string> starting_item_ids
string flavor
string experience_id
string label
string category
string rarity
SkillBonusDef? skill_bonus
… (31 more)
```

### `HobbySystem`

```
void LoadCatalog(string json)
bool CanConductSession(string hobbyId, IEnumerable<string>? availableFacilities)
static HobbyMastery ResolveMastery(float proficiency)
SurvivorHobbyProgress GetOrCreateProgress(string survivorId, string hobbyId)
HobbySessionResult ConductSession(string survivorId, string hobbyId, int currentDay, IEnumerable<string>? coParticipantIds = null, ISeededRng? rng = null)
float GetSharedHobbyAffinityBonus(string survivorA, string survivorB)
IReadOnlyList<SurvivorHobbyProgress> GetSurvivorHobbies(string survivorId)
HobbySystemState CaptureState()
void RestoreState(HobbySystemState state)
string HobbyId
string Name
HobbyCategory Category
string RequiredFacility
float BaseMoraleBonus
string Description
string SurvivorId
string HobbyId
float Proficiency
int SessionsCompleted
int LastSessionDay
HobbyMastery Mastery
string SurvivorId
string HobbyId
int Day
float MoraleGained
float ProficiencyGained
HobbyMastery NewMastery
List<string> CoParticipants
int SchemaVersion
int NextSequence
… (3 more)
```

### `RecruitmentSystem`

```
void LoadCatalog(string json)
IReadOnlyList<RecruitmentCampaignDef> GetAllCampaignDefs()
IReadOnlyList<RecruitmentCandidateDef> GetAllCandidateDefs()
RecruitmentCampaignDef? GetCampaignDef(string id)
RecruitmentCandidateDef? GetCandidateDef(string id)
RecruitmentCandidateRecord DiscoverCandidate(string candidateTemplateId, string locationId, int day, string currentFaction = "")
void TickDay(int day)
IReadOnlyList<RecruitmentCampaignRecord> GetActiveCampaigns()
IReadOnlyList<RecruitmentCandidateRecord> GetKnownCandidates()
IReadOnlyList<DefectionOfferRecord> GetDefectionOffers()
RecruitmentState CaptureState()
void RestoreState(RecruitmentState? state)
string campaign_type_id
string display_name
string category
int base_duration_days
int base_success_chance
int cost_food
int cost_water
int cost_currency
string description
string candidate_template_id
string candidate_type
string display_name
int base_willingness
string primary_skill
string personality_trait
string description
int schema_version
List<RecruitmentCampaignDef> campaign_templates
… (38 more)
```

### `SurvivorAgingProgressionEngine`

```
static int EvaluateAgeYears(int joinedDay, int currentDay, int baseAgeYears = DefaultRecruitmentAgeYears, int daysPerYear = DefaultDaysPerYear)
static SurvivorLifeStage EvaluateStage(int ageYears)
static SurvivorAgeProfile CalculateProfile(string survivorId, int joinedDay, int currentDay, int baseAgeYears = DefaultRecruitmentAgeYears, bool isRetired = false, int daysPerYear = DefaultDaysPerYear)
static float CalculateApprenticeLearningMultiplier(bool hasElderMentorPresent)
static bool HasLivingElderInShelter(IEnumerable<SurvivorAgeProfile>? partyProfiles)
string SurvivorId
int EffectiveAgeYears
int CampaignTenureDays
SurvivorLifeStage Stage
float PhysicalLaborMultiplier
float FatigueAccumulationMultiplier
float MentorshipXpBonus
bool IsRetirementEligible
bool IsRetired
string Description
```

### `SurvivorAutonomySystem`

```
void LoadCatalog(string jsonContent)
void LoadCatalog(IFileIO fileIO, string path)
bool IsOnCooldown(string survivorId, int currentDay)
AutonomyAction? EvaluateDailyAutonomy(string actorId, int currentDay, float morale, float fatigue, string[]? traits = null, string? targetId = null, int pairAffinity = 0)
bool OverrideRefusal(string actionId, bool enforceWork)
void AssignGoal(string survivorId, string goalId, string title, int targetProgress = 3)
bool AdvanceGoal(string survivorId, int amount = 1)
SurvivorGoalProgress? GetGoal(string survivorId)
SurvivorAutonomySaveState CaptureState()
void RestoreState(SurvivorAutonomySaveState? state)
string id
string action_type
string trigger_kind
string title
string description_template
float base_probability
int affinity_delta
float morale_delta
string required_trait
float min_morale
float max_morale
float min_fatigue
int schema_version
List<AutonomyActionTemplate> actions
string actionId
string templateId
string actorId
string targetId
AutonomyActionType actionType
AutonomyTriggerKind triggerKind
… (18 more)
```

### `SurvivorRoleSystem`

```
void LoadCatalog(string json)
IReadOnlyCollection<SurvivorRoleDef> GetAllRoleDefs()
SurvivorRoleDef? GetRoleDef(string roleId)
bool CanAssignRole(string survivorId, string roleId, Dictionary<string, int>? survivorSkills, out string failureReason)
SurvivorRoleAssignment? AssignRole(string survivorId, string roleId, Dictionary<string, int>? skills = null, int day = 1, bool force = false)
bool UnassignRole(string survivorId)
SurvivorRoleAssignment? GetRoleAssignment(string survivorId)
bool AddRoleXp(string survivorId, int xp)
float GetRoleBonus(string survivorId, string bonusType)
bool TriggerAutoAction(string survivorId, string actionType)
SurvivorRoleState CaptureState()
void RestoreState(SurvivorRoleState? saved)
string role_id
string display_name
string category
string primary_bonus_type
float primary_bonus_value
string secondary_bonus_type
float secondary_bonus_value
string auto_action_type
string description
int schema_version
List<SurvivorRoleDef> roles
string SurvivorId
string RoleId
int Level
int ExperiencePoints
int AssignedDay
int TotalAutoActionsExecuted
int SchemaVersion
… (3 more)
```

### `SurvivorRoutineSystem`

```
void LoadCatalog(string json)
IReadOnlyCollection<RoutineTemplateDef> GetAllTemplates()
RoutineTemplateDef? GetTemplate(string templateId)
void SetEnforcementLevel(string level)
SurvivorRoutineRecord AssignRoutine(string survivorId, string templateId)
SurvivorRoutineRecord? GetRoutine(string survivorId)
void SetPreference(string survivorId, string chronotype, string workShift = "morning", string social = "balanced")
RoutinePreferenceRecord? GetPreference(string survivorId)
string GetActivityAtHour(string survivorId, int hour)
RoutineSatisfactionRecord EvaluateDailySatisfaction(string survivorId, int day, int hoursWorked, int hoursSlept, int mealsHad, int socialHours)
IReadOnlyList<RoutineConflictRecord> DetectConflicts(int day, Dictionary<string, string>? roomAssignments = null, Dictionary<string, string>? workspaceAssignments = null)
bool ResolveConflict(string conflictId)
IReadOnlyList<RoutineConflictRecord> GetActiveConflicts()
SurvivorRoutineState CaptureState()
void RestoreState(SurvivorRoutineState? saved)
string block_id
int start_hour
int end_hour
string activity_type
string flexibility
string template_id
string display_name
int wake_hour
int sleep_hour
List<int> meal_hours
int work_start_hour
int work_end_hour
string preferred_chronotype
string description
List<TimeBlockDef> default_blocks
… (33 more)
```

### `PlayableMetricsAggregationEngine`

```
static AggregatedMetricsResult Evaluate(SessionMetricInputs inputs)
int DaysSurvived
int PeakPopulation
int CasualtiesCount
int TotalScavengeSorties
int TotalResourcesHarvested
int TotalWaterPurifiedLiters
int CrisesResolved
int CrisesFailed
int DifficultyScalarPermille
SessionReadinessGrade Grade
int HardshipIndexPermille
int EfficiencyRatingPermille
int SurvivalStabilityScorePermille
bool HasCriticalFailure
string SummaryDescription
```

### `GarmentLayeringThermalEngine`

```
WornGarmentState Clone()
static ThermalLayeringResult EvaluateThermalInsulation(IReadOnlyList<WornGarmentState> garments, int shelterThermalComfortPermille)
static int CalculateLaundryHygieneRestoration(int garmentDirtPermille, int washQualityPermille)
static void AdvanceDailyGarmentWear(WornGarmentState garment, ActivityLevel activity)
string GarmentId
GarmentLayer Layer
int InsulationPermille
int DurabilityPermille
int DirtPermille
bool IsWaterproof
int EffectiveWarmthPermille
bool HasWaterproofShell
bool IsFullyLayered
int HygienePenaltyPermille
```

### `VisitorIntegrationSystem`

```
VisitorType ParseType()
HousingType ParseHousing()
void LoadCatalog(string json)
void LoadCatalog(VisitorCatalogData catalog)
VisitorTemplateDefinition? GetTemplate(string templateId)
VisitorRecord AdmitVisitor(string name, VisitorType type, string admittedBy, int currentDay = 1, string? notes = null, float dailyFood = 1.0f, float dailyWater = 1.5f, int plannedDurationDays = -1)
VisitorRecord? AdmitFromTemplate(string templateId, string name, string admittedBy, int currentDay = 1)
bool AssignHousing(string visitorId, string roomId, HousingType housing, int currentDay = 1)
IntegrationTask? AssignIntegrationTask(string visitorId, string taskType, string assignedTo, int currentDay = 1, int durationDays = 3)
bool CompleteIntegrationTask(string taskId, int currentDay = 1)
bool SetMonitoringLevel(string visitorId, MonitoringLevel level)
bool RecruitVisitor(string visitorId, int currentDay = 1)
VisitorDepartureRecord? DepartVisitor(string visitorId, DepartureType type, string reason, int currentDay = 1, string finalStanding = "good_standing")
void TickDay(int currentDay)
IReadOnlyList<VisitorRecord> GetActiveVisitors()
VisitorRecord? GetVisitor(string visitorId)
VisitorIntegrationSaveState CaptureState()
void RestoreState(VisitorIntegrationSaveState state)
string Id
string Name
string Type
int BaseIntegrationDays
string DefaultHousing
float DailyFoodConsumption
float DailyWaterConsumption
string Description
int SchemaVersion
List<VisitorTemplateDefinition> Templates
string VisitorId
string Name
… (32 more)
```

### `SurvivorVoiceSystem`

```
void LoadCatalog(string json)
void LoadCatalog(VoiceLineCatalogData data)
void RegisterLine(VoiceLineDefinition line)
bool TrySelectVoiceLine(SurvivorSpeechContext context, string trigger, int currentDay, ISeededRng? rng, out VoiceLinePayload payload)
SurvivorVoiceState CaptureState()
void RestoreState(SurvivorVoiceState state)
string Id
string Speaker
string Trigger
string Register
string TextKey
string TextEnglish
float MinMorale
float MaxMorale
float MinFatigue
float MaxFatigue
float Weight
int CooldownDays
int SchemaVersion
List<VoiceLineDefinition> Lines
string LineId
string SpeakerSurvivorId
string Profession
string Trigger
string TextKey
string TextEnglish
string Register
int UtteredDay
string SurvivorId
string Profession
… (6 more)
```

### `VoiceLineDispatchCoordinator`

```
static VoiceDispatchResult Success(VoicePlaybackPayload payload)
static VoiceDispatchResult Rejected(string reason)
bool IsPlaying(int currentTick)
VoiceDispatchResult TryDispatch(VoiceLineSelectionResult selection, VoiceLinePriority priority, int currentTick, int durationTicks = 5)
VoiceDispatchResult TrySelectAndDispatch(SurvivorVoiceContext survivor, string eventKind, IEnumerable<VoiceLineCandidate> candidates, VoiceLinePriority priority, int currentTick, int durationTicks = 5, int deterministicSeed = 0)
void StopActivePlayback(int currentTick)
VoiceDispatchCoordinatorSaveState CaptureState()
void RestoreState(VoiceDispatchCoordinatorSaveState? state)
string SpeakerSurvivorId
string LineId
string AudioCueKey
string TextKey
VoiceLinePriority Priority
int DispatchTick
int DurationTicks
bool PreemptedPrevious
bool Dispatched
VoicePlaybackPayload? Payload
string RejectionReason
int schema_version
int GlobalLastSpokenTick
string? ActiveSpeakerId
string? ActiveLineId
VoiceLinePriority ActivePriority
int ActiveLineEndTick
int PerCharacterCooldownTicks
int GlobalCooldownTicks
```

### `VoiceLineSelectionEngine`

```
bool HasTag(string tag)
static VoiceLineSelectionResult SelectLine(SurvivorVoiceContext survivor, string eventKind, IEnumerable<VoiceLineCandidate> candidates, int deterministicSeed = 0)
string SurvivorId
string VoiceProfileId
int MoralePermille
int StressPermille
IReadOnlyList<string> TraumaTags
bool IsIncapacitated
string LineId
string VoiceKey
string TextKey
string AudioCueKey
string EventKind
int MinStressPermille
int MaxStressPermille
int MinMoralePermille
int MaxMoralePermille
string RequiredTraumaTag
int PriorityWeight
bool HasLine
string LineId
string VoiceKey
string TextKey
string AudioCueKey
string SpeakerSurvivorId
```

### `WaterQualityProfileEngine`

```
static WaterContaminantProfile EvaluateSourceContamination(WaterSourcePurityTier tier, int floodContaminationPermille = 0)
static WaterTreatmentYield CalculateTreatmentYield(WaterSourcePurityTier sourceTier, TreatmentMode mode, int filterIntegrityPermille = 1000)
static int CalculateHealthRiskPermille(WaterContaminantProfile profile)
int ParticulatePpm
int HeavyMetalsPpm
int RadIsotopesBqL
int BioPathogensCfu
int YieldFractionPermille
int FilterWearPermille
WaterSourcePurityTier ResultingPurityTier
int PathogenRiskPermille
int HeavyMetalRiskPermille
```

### `WaterSourceSystem`

```
void LoadCatalog(string json)
IReadOnlyCollection<WaterSourceDef> GetAllSourceDefs()
IReadOnlyList<WaterConnectionDef> GetAllConnections()
WaterSourceDef? GetSourceDef(string sourceId)
WaterSourceState? GetSource(string sourceId)
WaterSourceState? GetActiveSource()
bool DiscoverSource(string sourceId, int day)
bool SetActiveSource(string sourceId)
void TickDay(int day, float rainMultiplier = 1.0f, float environmentalContamination = 0.0f)
WaterTestResult ConductWaterTest(string sourceId, int day, string survivorId, float kitAccuracy = 0.95f)
bool PerformMaintenance(string sourceId, int day)
bool UpgradeInfrastructure(string sourceId, string newLevel)
float CalculateAvailableWater()
IReadOnlyList<WaterTestResult> GetTestHistory(string? sourceId = null)
WaterSourceSystemState CaptureState()
void RestoreState(WaterSourceSystemState? saved)
string source_id
string display_name
string source_type
string location_id
float flow_rate_l_day
float base_contamination
string initial_infrastructure_level
string description
string connection_id
string source_a
string source_b
string connection_type
float transfer_rate
int schema_version
… (24 more)
```

### `NuclearWinterProgressionSystem`

```
void LoadCatalog(string json)
WinterPhaseDef GetPhaseForDay(int day)
SeasonalCycleDef GetSeasonForDay(int day)
ClimateState EvaluateClimate(int day, ISeededRng? rng = null)
ClimateState AdvanceDay(int day, ISeededRng? rng = null)
float CalculateHeatingDemand(float baseDemand, int day)
float CalculateExpeditionRisk(float baseRisk, int day)
float CalculateCropYieldMultiplier(int day, bool hasGreenhouse)
NuclearWinterSaveState CaptureState()
void RestoreState(NuclearWinterSaveState? state)
string PhaseId
string DisplayName
int StartDay
int EndDay
float SeverityModifier
float TemperatureBaseCelsius
float StormFrequencyModifier
float RadiationModifier
float DaylightPenaltyHours
string Description
string SeasonId
string DisplayName
int DurationDays
float TemperatureOffsetCelsius
float StormChanceMultiplier
float BaseDaylightHours
float MoraleDailyImpact
int Day
string PhaseId
string PhaseDisplayName
… (20 more)
```

### `WeatherCascadeSystem`

```
void BindWeatherSource(IWeatherCascadeSource source)
WeatherEvent TriggerCascade(WeatherKind kind, float severity, int day, IReadOnlyList<string>? regions = null)
void TickDay(int newDay)
WeatherCascadeState CaptureState()
void RestoreState(WeatherCascadeState? state)
int CurrentDay
int FortificationLevel
```

### `CascadeTargetSystem`

```
void LoadCatalog(string json)
static WeatherGameplayCascadeEngine LoadFromDirectory(string dataDir, IFileIO fileIO)
WeatherEvent EvaluateWeatherCascade(WeatherKind kind, float severity, int currentDay, IReadOnlyList<string>? regions = null, int fortificationLevel = 0, ISeededRng? rng = null)
void TickDay(int currentDay)
WeatherCascadeState CaptureState()
void RestoreState(WeatherCascadeState? state)
string id
string weather_kind
string target_system
string effect_type
float magnitude
int duration_days
string description
string effectId
CascadeTargetSystem targetSystem
CascadeEffectType effectType
float magnitude
int durationDays
string description
string id
WeatherKind weatherKind
float severity
int startDay
int durationDays
List<string> affectedRegions
List<WeatherEffect> effects
int schema_version
List<WeatherEvent> activeEvents
List<WeatherEvent> eventHistory
List<WeatherEffect> activeEffects
```

### `ModalTravelDispatchEngine`

```
static ModalTravelDispatchResult Refused(TravelModality modality, int distanceKm, string refusalReason)
static ModalTravelDispatchResult EvaluateDispatch(MapRoute? route, TravelModality modality, int vehicleConditionPermille = 1000, int fuelAvailableUnits = 0, int weatherWindowPermille = 1000)
bool CanDispatch
TravelModality Modality
int DistanceKm
int EstimatedDurationHours
int FuelRequiredUnits
int WeatherHazardRiskPermille
int TerrainAttritionRiskPermille
string RefusalReason
string Summary
```

### `NightWatchPatrolReadinessEngine`

```
static PatrolCoverageResult EvaluatePatrolCoverage(int assignedPatrollerCount, int sectorPerimeterMetres, int averageFatiguePermille, int nightVisionEquipmentPermille)
static int AdvanceWatchFatigue(int currentFatiguePermille, int shiftHours, int restQualityPermille)
static WatchFatigueTier ClassifyFatigue(int fatiguePermille)
static GateProtocolReadinessResult EvaluateGateProtocol(int gateStaffPermille, int drillRecencyPermille, int mechanicalConditionPermille, bool alarmSystemOnline)
static int ComputeWatchReadinessScore(int patrolCoveragePermille, int averageFatiguePermille, int drillRecencyPermille)
PatrolCoverageGrade CoverageGrade
int DetectionProbabilityPermille
int UncoveredGapHours
bool MeetsReadinessThreshold
int ReadinessPermille
bool IsGateReady
int EstimatedSealTimeMinutes
string PrimaryBottleneck
```

### `StormForecastReadinessEngine`

```
static StormForecastResult EvaluateForecastReliability(int observationSkillPermille, int leadTimeHours, StormSeverityClass stormSeverity, int forecastSeed)
static SeasonalReadinessResult AssessSeasonalReadiness(int sealedAirlockPermille, int filterStockPermille, int medicalReadinessPermille, int drillRecencyPermille, StormSeverityClass targetSeverity)
static int CalculateObservationPostDecay(int instrumentQualityPermille)
int ConfidencePermille
ForecastConfidenceTier ConfidenceTier
StormSeverityClass PredictedSeverity
int LeadTimeHours
bool IssueWarning
SeasonalReadinessBand ReadinessBand
int ReadinessPermille
bool CanAbsorbBlackRain
string PrimaryGap
```

### `WeatherForecastReliabilityEngine`

```
static ForecastReliabilityResult EvaluateReliability(int distanceKmFromStation, int leadTimeDays, int atmosphericInterferencePermille, int stationCalibrationPermille = 800)
ForecastConfidenceGrade Grade
int ReliabilityScorePermille
int LeadTimeDays
bool IsReliableForDispatch
string ReliabilitySummary
```

### `WildlifeHarvestQuotaEngine`

```
static HarvestQuotaResult EvaluateHarvestQuota(int currentPopulationPermille, int seasonalReproductionPermille, int requestedHarvestUnits)
static PredatorConflictPosture EvaluatePredatorConflict(int predatorPopulationPermille, int proximityMetres, int shelterNoisePermille)
static TamingReadinessResult EvaluateTamingReadiness(int animalHungerPermille, int trustExposurePermille, int speciesTamabilityPermille, int tamingSeed)
int MaxSafeHarvestUnits
SpeciesPopulationBand PostHarvestBand
bool IsWithinQuota
int OverhuntCollapseRiskPermille
int ReadinessPermille
bool IsTameable
int EstimatedSessionsNeeded
```

