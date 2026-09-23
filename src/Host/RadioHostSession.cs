// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin host read model for the faction radio view. The corpus and
    /// deterministic intercept selection stay in Ashfall.Core; this class only
    /// owns the current day, receiver history, and host notifications.
    /// </summary>
    public sealed class RadioHostSession
    : HostSessionBase{
        public const int DemoSeed = 2026;
        private const string CorpusFileName = "faction_radio_corpus.json";
        private readonly List<RadioIntercept> _history = new();
        private readonly HashSet<string> _playedBroadcastKeys = new();

        /// <summary>
        /// Static mapping retained for legacy event names, now resolving to stable
        /// catalog IDs rather than direct resource file names.
        /// </summary>
        private static readonly Dictionary<string, string> s_voiceOverMap = new()
        {
            { "vo_kind_parley", AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioVoKindParley },
            { "vo_kind_hatch", AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioVoKindHatch },
            { "vo_ch3_ash_road", AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioVoCh3AshRoad },
            { "vo_ch7_milband", AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioVoCh7Milband },
            { "vo_ch11_stockpile", AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioVoCh11Stockpile },
        };

        /// <summary>
        /// Fired when a new (non-duplicate) broadcast is intercepted.
        /// Carries the intercept and the resolved voice-over cue ID (null if none).
        /// </summary>
        public event Action<RadioIntercept, string?>? BroadcastIntercepted;

        public FactionRadioEngine Engine { get; }
        public SignalTriangulationSystem Triangulation { get; }
        public RadioBroadcastCatalog BroadcastCatalog { get; }
        public RadioStationCatalog Stations { get; }
        public RadioScheduleCoordinator ScheduleCoordinator { get; }
        public RadioDistressSystem DistressSystem { get; }
        public RadioRecordingSystem RecordingSystem { get; }
        public RadioSignalLog SignalLog { get; }
        public DistressRescueMissionManager RescueMissions { get; }

        /// <summary>Tasks 9–12 Wave 2 — radio-owned signal-trust ledger. Owned
        /// by the session, bound to the default mission manager, persisted in
        /// the radio save section (V5).</summary>
        public SignalTrustLedger SignalTrust { get; }

        /// <summary>Tasks 9–12 Wave 3 — radio-owned follow-up scheduler. Owned
        /// by the session, bound to the mission lifecycle events, persisted in
        /// the radio save section (V6).</summary>
        public DistressFollowUpScheduler FollowUps { get; }
        public ISeededRng Rng { get; }
        public IReadOnlyList<RadioIntercept> History => _history;
        public int Day { get; private set; }
        public float CurrentFrequency { get; private set; }
        public RadioIntercept? LastIntercept { get; private set; }
        public ScheduledBroadcastResult? LastScheduledBroadcast { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;
        public Func<string>? WeatherConditionProvider { get; set; }
        public Func<WeatherKind>? WeatherKindProvider { get; set; }

        public RadioReceiverBand CurrentBand => RadioReceiverPlan.GetBandForFrequencyMhz(CurrentFrequency);

        public void SetBand(string bandId)
        {
            int idx = RadioReceiverPlan.GetBandIndex(bandId);
            var band = RadioReceiverPlan.GetBandByIndex(idx);
            Listen(band.MinMhz);
        }

        public void CycleBand()
        {
            var next = RadioReceiverPlan.NextBand(CurrentBand);
            Listen(next.MinMhz);
        }

        public RadioHostSession(
            FactionRadioEngine engine,
            ISeededRng? rng = null,
            int day = 1,
            SignalTriangulationSystem? triangulation = null,
            RadioBroadcastCatalog? broadcastCatalog = null,
            RadioStationCatalog? stationCatalog = null,
            RadioDistressSystem? distressSystem = null,
            RadioRecordingSystem? recordingSystem = null,
            RadioSignalLog? signalLog = null,
            DistressRescueMissionManager? rescueMissions = null)
        {
            Engine = engine ?? new FactionRadioEngine();
            Triangulation = triangulation ?? new SignalTriangulationSystem();
            Rng = rng ?? new SeededRng(DemoSeed);
            Day = Math.Max(1, day);

            BroadcastCatalog = broadcastCatalog ?? new RadioBroadcastCatalog();
            Stations = stationCatalog ?? new RadioStationCatalog();
            ScheduleCoordinator = new RadioScheduleCoordinator(BroadcastCatalog, Stations);
            DistressSystem = distressSystem ?? new RadioDistressSystem();
            RecordingSystem = recordingSystem ?? new RadioRecordingSystem();
            SignalLog = signalLog ?? new RadioSignalLog();
            // Tasks 9–12 Wave 2 — the session owns the signal-trust ledger and
            // binds it to the default mission manager so trust events flow from
            // the exactly-once lifecycle transitions. Externally supplied
            // managers keep their own wiring.
            SignalTrust = new SignalTrustLedger();
            RescueMissions = rescueMissions ?? new DistressRescueMissionManager(null, DistressSystem, SignalTrust);
            // Tasks 9–12 Wave 3 — the session owns the follow-up scheduler,
            // binds it to the mission manager's exactly-once lifecycle events,
            // and ticks it beside the mission daily tick.
            FollowUps = new DistressFollowUpScheduler(DistressSystem, RescueMissions);
            FollowUps.BindToMissionEvents();
            FollowUps.OnFollowUpFired += (parentId, followUp, day) =>
            {
                LastEvent = $"Follow-up transmission from {parentId}: {followUp.Text}";
                // Tasks 9–12 Wave 4 — follow-up transmissions resolve their own
                // audio cue exactly like other radio content. Follow-ups fire
                // exactly once by scheduler construction, so no dedupe is needed.
                if (!string.IsNullOrWhiteSpace(followUp.AudioCue))
                    AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(followUp.AudioCue);
                RaiseStateChanged();
            };

            CurrentFrequency = FirstFrequency();
            Triangulation.OnStateChanged += _ => RaiseStateChanged();
            Triangulation.OnLocationRevealed += id => { LastEvent = $"Location discovered: {id}"; RaiseStateChanged(); };
            DistressSystem.OnSignalIntercepted += (def, state) => { LastEvent = $"Distress intercepted: {def.SourceName}"; RaiseStateChanged(); };
            DistressSystem.OnSignalExpired += (def, state) => { LastEvent = $"Distress expired: {def.SourceName}"; RaiseStateChanged(); };
            RescueMissions.OnStageChanged += (m, stage) =>
            {
                LastEvent = $"Distress mission {m.QuestId}: {stage}";
                // Claim once on successful terminal stages so OnRewardsGranted
                // can reach inventory / reputation consumers in Main.
                // TerminalFailed also claims: eligibility lives in Core — only a
                // real dead-arrival salvage (sender dead + arrival resolved +
                // survival model) grants anything, always with rep 0.
                if (stage == DistressRescueMissionStage.TerminalRescued
                    || stage == DistressRescueMissionStage.TerminalSurvived
                    || stage == DistressRescueMissionStage.TerminalFailed)
                {
                    var (items, rep) = RescueMissions.ClaimIdempotentRewards(m.QuestId);
                    if (items.Count > 0 || rep != 0)
                        LastEvent = $"Distress rewards claimed for {m.QuestId}: {items.Count} items, rep {rep}.";
                }
                RaiseStateChanged();
            };
        }

        public static RadioHostSession Create(string dataDir, int day = 1, ICampaignRngManager? campaignRng = null)
        {
            string actualDataDir = dataDir ?? string.Empty;
            string path = Path.Combine(actualDataDir, CorpusFileName);
            if (!File.Exists(path))
            {
                actualDataDir = CatalogPath.ResolveDataDir();
                path = Path.Combine(actualDataDir, CorpusFileName);
            }

            string json = File.Exists(path) ? File.ReadAllText(path) : string.Empty;
            var broadcastCatalog = new RadioBroadcastCatalog();
            broadcastCatalog.LoadFromDataDirectory(actualDataDir, new Ashfall.Core.FileSystemIO(), new Ashfall.Core.SystemTextJsonSerializer());

            var distressSystem = new RadioDistressSystem();
            // Built-ins are compatibility fallbacks for sparse fixtures. Load
            // the expansion layer first so it can replace those fallbacks, then
            // load the primary Plan 50 authority last so duplicate IDs in the
            // optional layer cannot override the canonical primary definitions.
            string distressExpPath = Path.Combine(actualDataDir, "radio_distress_signals_expansion.json");
            if (File.Exists(distressExpPath))
            {
                distressSystem.LoadFromJson(File.ReadAllText(distressExpPath));
            }
            string distressPath = Path.Combine(actualDataDir, "radio_distress_signals.json");
            if (File.Exists(distressPath))
            {
                distressSystem.LoadFromJson(File.ReadAllText(distressPath));
            }

            var stationCatalog = new RadioStationCatalog();
            stationCatalog.LoadFromDataDirectory(actualDataDir);

            var triangulation = new SignalTriangulationSystem();
            try
            {
                var io = new Ashfall.Core.FileSystemIO();
                var ser = new Ashfall.Core.SystemTextJsonSerializer();
                var dfDto = DirectionFindingCatalogLoader.Load(actualDataDir, io, ser);
                DirectionFindingCatalogLoader.Validate(dfDto);
                triangulation.LoadCatalog(dfDto);
            }
            catch (Exception ex) /* optional: DF catalog absent in sparse fixtures */
            {
                CatalogDiagnostics.Warn(actualDataDir, "direction_finding_catalog", ex);
            }

            var session = new RadioHostSession(
                FactionRadioEngine.LoadFromJson(json),
                campaignRng != null
                    ? campaignRng.Fork(CampaignStreamIds.Radio, day)
                    : new SeededRng(DemoSeed),
                day,
                triangulation,
                broadcastCatalog,
                stationCatalog,
                distressSystem);

            session.Listen();
            // Persistence: a radio save (checksummed, user://) wins over fresh
            // state — history, played-broadcast dedup keys, and tuned frequency
            // all survive a reload. No save = fresh receiver (legacy fallback).
            var save = RadioSaveStore.TryLoad();
            if (save != null)
            {
                session.RestoreSave(save);
                session.LastEvent = "Radio state restored from save.";
            }
            return session;
        }

        public void SetDay(int day)
        {
            Day = Math.Max(1, day);
            DistressSystem.TickDaily(Day);
            // Tasks 9–12 Wave 3 — the scheduler must know the day BEFORE the
            // mission tick so event-time schedules compute due = day + delay.
            FollowUps.SetDay(Day);
            RescueMissions.TickDaily(Day);
            // After the mission tick so an expiry that schedules a 0-delay
            // follow-up this day can fire deterministically.
            FollowUps.TickDaily(Day);
        }

        public string Listen(float? frequencyMhz = null)
        {
            if (frequencyMhz.HasValue)
                CurrentFrequency = frequencyMhz.Value;

            // Plan 24: Unified scheduling resolution
            LastScheduledBroadcast = ScheduleCoordinator.Resolve(CurrentFrequency, Day, Rng);
            if (LastScheduledBroadcast != null && LastScheduledBroadcast.HasTransmission && !LastScheduledBroadcast.IsSilence)
            {
                SignalLog.LogIntercept(LastScheduledBroadcast, Day);
            }

            // Check for distress signal intercept
            var distress = DistressSystem.FindSignalAtFrequency(CurrentFrequency);
            if (distress != null)
            {
                DistressSystem.Intercept(distress.FrequencyId, Day);
                RescueMissions.RecordSignalHeard(distress.FrequencyId, Day);
            }
            var intercept = Engine.GetBroadcastAtFrequency(CurrentFrequency, Day, Rng);
            LastIntercept = intercept;
            _history.Add(intercept);
            if (_history.Count > 32)
                _history.RemoveAt(0);

            LastEvent = string.IsNullOrWhiteSpace(intercept.FactionId)
                ? (LastScheduledBroadcast != null && LastScheduledBroadcast.HasTransmission && !LastScheduledBroadcast.IsSilence
                    ? $"Intercepted {LastScheduledBroadcast.StationName} at {CurrentFrequency:0.00} MHz."
                    : $"Dead air at {CurrentFrequency:0.00} MHz.")
                : $"Intercepted {intercept.Callsign} at {CurrentFrequency:0.00} MHz.";

            // Audio: a tuner motion always produces static; a received station
            // adds a short lock confirmation before any authored voice clip.
            var audio = AtomicWar.GodotApp.Audio.AudioManager.Instance;
            audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioTune);
            audio?.PlayRadioStatic();
            if (!string.IsNullOrWhiteSpace(intercept.FactionId) || (LastScheduledBroadcast != null && LastScheduledBroadcast.HasTransmission && !LastScheduledBroadcast.IsSilence))
                audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioSignalLock);

            // Tasks 9–12 Wave 4 — distress-signal audio projection. Plays only
            // when the signal was LEGITIMATELY detected through the tuner flow
            // (the Intercept transition above — never on catalog load, internal
            // eligibility, or a save containing an undiscovered signal). Cue is
            // derived from the authoritative stage (stage override → signal
            // default → text-only fallback); the dedupe key rides the existing
            // persisted playedBroadcastKeys ledger, so an already-heard cue does
            // not replay after a reload and a stage change to a new cue plays.
            if (distress != null)
            {
                string distressCue = Ashfall.Core.Radio.DistressAudioCueResolver.ResolveForDay(distress, Day);
                if (!string.IsNullOrEmpty(distressCue))
                {
                    string distressKey = $"distress:{distress.FrequencyId}:{distressCue}";
                    if (_playedBroadcastKeys.Add(distressKey))
                        audio?.PlayCue(distressCue); // missing cue → logged once, text continues
                }
            }

            // Audio: voice-over only for new (non-duplicate) broadcasts with a mapped clip
            string? voiceOverClip = ResolveVoiceOver(intercept);
            if (string.IsNullOrEmpty(voiceOverClip) && LastScheduledBroadcast != null && !string.IsNullOrEmpty(LastScheduledBroadcast.AudioCue))
            {
                voiceOverClip = LastScheduledBroadcast.AudioCue;
            }
            string broadcastKey = MakeBroadcastKey(intercept);
            if (voiceOverClip != null && _playedBroadcastKeys.Add(broadcastKey))
            {
                audio?.PlayVoiceOverCue(voiceOverClip);
            }

            BroadcastIntercepted?.Invoke(intercept, voiceOverClip);
            RaiseStateChanged();
            return LastEvent;
        }

        public string BroadcastBeacon(string customMessage = "Holdfast shelter holding. Awaiting courier contact.")
        {
            var beacon = new RadioIntercept(
                "faction_holdfast",
                "HOLDFAST BASE",
                CurrentFrequency,
                RadioEventKind.ParleyResolution,
                customMessage,
                5,
                Day);
            _history.Add(beacon);
            if (_history.Count > 32)
                _history.RemoveAt(0);

            // The broadcast is the latest intercept: mirror Listen() so the
            // receiver's LastIntercept reflects the beacon just sent. This keeps
            // the shelter-operations gate green and makes the UI's "latest
            // intercept" readout show the outgoing beacon, not stale dead air.
            LastIntercept = beacon;

            LastEvent = $"Emergency beacon broadcast on {CurrentFrequency:0.00} MHz.";
            string cueId = AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioVoKindParley;
            AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioMorse);
            AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayVoiceOverCue(cueId);
            BroadcastIntercepted?.Invoke(beacon, cueId);
            RaiseStateChanged();
            return LastEvent;
        }

        public void TuneDelta(float deltaMhz)
        {
            var band = RadioReceiverPlan.GetBandForFrequencyMhz(CurrentFrequency);
            float target = (float)Math.Round(band.ClampMhz(CurrentFrequency + deltaMhz), 2);
            Listen(target);
        }

        /// <summary>
        /// Record a direction-finding observation for the currently intercepted signal.
        /// </summary>
        public bool RecordBearingObservation(float bearingDegrees)
        {
            string sigId = LastIntercept.HasValue && !string.IsNullOrEmpty(LastIntercept.Value.FactionId)
                ? LastIntercept.Value.FactionId
                : $"freq_{CurrentFrequency:000.0}";

            string weather = WeatherConditionProvider?.Invoke()
                ?? WeatherKindProvider?.Invoke().ToString()
                ?? "Clear";

            var obs = new RadioObservation
            {
                signalId = sigId,
                stationId = "station_holdfast",
                day = Day,
                hour = 12f,
                bearingDegrees = (float)Math.Round(Math.Clamp(bearingDegrees, 0f, 359f), 1),
                errorDegrees = 1.5f,
                signalStrength = (LastIntercept.HasValue ? LastIntercept.Value.SignalStrength : 2) / 5.0f,
                noiseLevel = 0.05f,
                frequencyMhz = CurrentFrequency,
                weatherCondition = weather,
                operatorSkill = 0.9f
            };

            bool ok = Triangulation.RecordObservation(obs);
            if (ok)
            {
                LastEvent = $"Recorded DF bearing {obs.bearingDegrees:000}° on {CurrentFrequency:0.00} MHz ({Triangulation.GetObservationCount(sigId)} obs).";
                AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioMorse);
                RaiseStateChanged();
            }
            return ok;
        }

        /// <summary>
        /// Process collected observations to triangulate signal emitter and reveal wasteland coordinates.
        /// </summary>
        public TriangulationCandidate? TriangulateCurrentSignal()
        {
            string sigId = LastIntercept.HasValue && !string.IsNullOrEmpty(LastIntercept.Value.FactionId)
                ? LastIntercept.Value.FactionId
                : $"freq_{CurrentFrequency:000.0}";

            var candidate = Triangulation.Triangulate(sigId, Rng);
            if (candidate != null)
            {
                RescueMissions.RecordSignalIdentified(sigId);
                bool discovered = Triangulation.IsLocationDiscovered(candidate.locationId);
                LastEvent = discovered
                    ? $"Triangulation confirmed: {candidate.displayName} at ({candidate.estimatedX:F1}, {candidate.estimatedY:F1}) [Conf: {candidate.confidence:P0}]."
                    : $"Triangulation progress on {sigId}: {candidate.confidence:P0} confidence ({Triangulation.GetObservationCount(sigId)} obs).";
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Insufficient bearing observations to triangulate {sigId} (need ≥2).";
                RaiseStateChanged();
            }
            return candidate;
        }

        /// <summary>
        /// Plan 212 follow-up — market rumor bridge. Real market state (a
        /// shock the canonical market applied or expired) relayed as a short
        /// band item so trade news reaches the receiver the way other world
        /// facts do. No invented prices; the text comes from Core's
        /// <see cref="Ashfall.Core.Economy.EconomyMarketRumorRules"/>.
        /// </summary>
        public string RecordMarketRumor(string message, int day)
        {
            if (string.IsNullOrWhiteSpace(message)) return LastEvent;
            var rumor = new RadioIntercept(
                "faction_holdfast",
                "MARKET WATCH",
                CurrentFrequency,
                RadioEventKind.MarketRumor,
                message,
                4,
                day > 0 ? day : Day);
            _history.Add(rumor);
            if (_history.Count > 32)
                _history.RemoveAt(0);
            LastEvent = $"Market rumor on the band: {message}";
            BroadcastIntercepted?.Invoke(rumor, null);
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>
        /// Player-visible warlord radio warning (doctrine shift / hostile action).
        /// Adds a RaidWarning intercept under the canonical warlords_sector_4
        /// identity so the radio history carries the consequence. Presentation
        /// only — the warlord AI emits the intent through Core events; this is
        /// the thin adapter that surfaces it.
        /// </summary>
        public string InterceptWarlordWarning(string message, int day)
        {
            if (string.IsNullOrWhiteSpace(message)) return "";
            var warning = new RadioIntercept(
                "warlords_sector_4",
                "TOLL HOUSE RELAY",
                94.2f,
                RadioEventKind.RaidWarning,
                message,
                3,
                day > 0 ? day : Day);
            _history.Add(warning);
            if (_history.Count > 32)
                _history.RemoveAt(0);
            LastEvent = "Warlord radio warning intercepted on 94.2 MHz.";
            BroadcastIntercepted?.Invoke(warning, null);
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>
        /// Cultural broadcast bridge: rare vinyl → shortwave. Deterministic, host-wired.
        /// VinylMoraleSystem fires OnCulturalBroadcast for rare records; Main wires it here.
        /// Power load (150W) is checked by the host before calling — this method only records the signal.
        /// </summary>
        public string RecordCulturalBroadcast(string recordId, string genre, string displayName, int day, float signalStrength)
        {
            if (string.IsNullOrWhiteSpace(recordId)) return string.Empty;
            string msg = $"Cultural broadcast: '{displayName}' ({genre}) — pre-war vinyl on shortwave. Wanderers may hear this.";
            var broadcast = new RadioIntercept(
                "faction_holdfast",
                "HOLDFAST CULTURAL RELAY",
                98.6f,
                RadioEventKind.CulturalBroadcast,
                msg,
                Math.Clamp((int)(signalStrength * 9f), 1, 9),
                day > 0 ? day : Day);
            _history.Add(broadcast);
            if (_history.Count > 32)
                _history.RemoveAt(0);
            LastEvent = $"Cultural broadcast on 98.6 MHz: {recordId} ({genre})";
            BroadcastIntercepted?.Invoke(broadcast, null);
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>
        /// Resolve a voice-over cue ID for this intercept.
        /// Returns null if no clip is mapped for this faction/event combination.
        /// </summary>
        private static string? ResolveVoiceOver(RadioIntercept intercept)
        {
            if (string.IsNullOrWhiteSpace(intercept.FactionId))
                return null;

            // Event-kind-based mapping for vo_kind_* clips
            if (intercept.Kind == RadioEventKind.ParleyResolution &&
                s_voiceOverMap.ContainsKey("vo_kind_parley"))
                return s_voiceOverMap["vo_kind_parley"];

            // Fallback: message-text matching (backward compatible)
            foreach (var kvp in s_voiceOverMap)
            {
                if (intercept.Message != null &&
                    intercept.Message.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            }
            return null;
        }

        /// <summary>
        /// Stable dedup key: day + frequency + message hash.
        /// Prevents replay on UI refresh/reopen while allowing same-frequency different-day broadcasts.
        /// Uses the deterministic Core StableHash — string.GetHashCode() is randomized
        /// per process and would make the key unstable across runs.
        /// </summary>
        private static string MakeBroadcastKey(RadioIntercept intercept)
        {
            return $"{intercept.Day}:{intercept.FrequencyMhz:F2}:{StableHash.Of(intercept.Message)}";
        }

        /// <summary>True when this broadcast's dedup key was already played (voice-over already fired).</summary>
        public bool HasPlayed(RadioIntercept intercept)
        {
            return _playedBroadcastKeys.Contains(MakeBroadcastKey(intercept));
        }

        // ── Persistence ─────────────────────────────────────────────────────

        /// <summary>
        /// Snapshot every authoritative mutable value: ordered intercept history,
        /// played-broadcast dedup keys, tuned frequency, sim day, discovered stations,
        /// custom presets, distress signals, signal log, recorded cassettes, and station overrides.
        /// </summary>
        public RadioSaveState CaptureSave()
        {
            var state = new RadioSaveState
            {
                day = Day,
                currentFrequency = CurrentFrequency,
                history = new List<RadioInterceptEntry>(_history.Count)
            };
            for (int i = 0; i < _history.Count; i++)
            {
                var h = _history[i];
                state.history.Add(new RadioInterceptEntry
                {
                    factionId = h.FactionId,
                    callsign = h.Callsign,
                    frequencyMhz = h.FrequencyMhz,
                    kind = (int)h.Kind,
                    message = h.Message,
                    signalStrength = h.SignalStrength,
                    day = h.Day
                });
            }
            state.playedBroadcastKeys = new List<string>(_playedBroadcastKeys);
            state.playedBroadcastKeys.Sort(StringComparer.Ordinal);

            // Plan 24 V2 extensions
            state.discoveredStationIds = SignalLog.CaptureDiscoveredStations();
            state.customPresets = SignalLog.CapturePresets();
            state.distressSignals = DistressSystem.CaptureState();
            state.signalLog = SignalLog.CaptureEntries();
            state.recordedCassettes = RecordingSystem.CaptureState();

            var overrides = Stations.ExportOverrides();
            state.stationOverrides = new List<StationStateOverrideEntry>(overrides.Count);
            foreach (var kvp in overrides)
            {
                state.stationOverrides.Add(new StationStateOverrideEntry
                {
                    stationId = kvp.Key,
                    state = (int)kvp.Value
                });
            }
            state.stationOverrides.Sort((a, b) => string.Compare(a.stationId, b.stationId, StringComparison.Ordinal));

            // Plan B88 — persist continuous DF triangulation (observations/candidates/baselines).
            state.triangulation = Triangulation.CaptureState();

            // Rescue-signal runtime (V4) — mission stages, deadlines, expedition
            // association, receipts, sender survival, ignore consequence, and
            // authenticity assessments survive a reload.
            state.rescueMissions = RescueMissions.CaptureState();

            // Tasks 9–12 Wave 2 (V5) — signal-trust ledger survives a reload.
            state.signalTrust = SignalTrust.CaptureState();

            // Tasks 9–12 Wave 3 (V6) — pending/fired follow-ups survive a reload.
            state.signalFollowUps = FollowUps.CaptureState();

            return state;
        }

        /// <summary>
        /// Rebuild receiver state from a snapshot. Overwrites history, dedup keys,
        /// frequency, day, discovered stations, distress states, and recorded cassettes.
        /// </summary>
        public void RestoreSave(RadioSaveState state)
        {
            _history.Clear();
            _playedBroadcastKeys.Clear();
            if (state == null) return;

            Day = Math.Max(1, state.day);
            CurrentFrequency = state.currentFrequency > 0f ? state.currentFrequency : FirstFrequency();

            if (state.history != null)
                for (int i = 0; i < state.history.Count; i++)
                {
                    var e = state.history[i];
                    if (e == null) continue;
                    _history.Add(new RadioIntercept(
                        e.factionId, e.callsign, e.frequencyMhz,
                        (RadioEventKind)e.kind, e.message, e.signalStrength, Math.Max(1, e.day)));
                }

            if (state.playedBroadcastKeys != null)
                for (int i = 0; i < state.playedBroadcastKeys.Count; i++)
                    if (!string.IsNullOrEmpty(state.playedBroadcastKeys[i]))
                        _playedBroadcastKeys.Add(state.playedBroadcastKeys[i]);

            // Plan 24 V2 restorations
            DistressSystem.RestoreState(state.distressSignals);
            SignalLog.RestoreState(state.signalLog, state.discoveredStationIds, state.customPresets);
            RecordingSystem.RestoreState(state.recordedCassettes);

            if (state.stationOverrides != null)
            {
                var map = new Dictionary<string, RadioStationState>(StringComparer.OrdinalIgnoreCase);
                foreach (var ov in state.stationOverrides)
                {
                    if (ov != null && !string.IsNullOrEmpty(ov.stationId))
                        map[ov.stationId] = (RadioStationState)ov.state;
                }
                Stations.ImportOverrides(map);
            }

            // Plan B88 — restore continuous DF triangulation nest (empty on pre-V3 saves).
            if (state.triangulation != null)
                Triangulation.RestoreState(state.triangulation);

            // Rescue-signal runtime (V4) — mission state restored additively;
            // pre-V4 saves leave missions at their neutral authored defaults.
            if (state.rescueMissions != null)
                RescueMissions.RestoreState(state.rescueMissions);

            // Tasks 9–12 Wave 2 (V5) — trust ledger restored additively;
            // pre-V5 saves leave trust at its neutral default (score 50).
            SignalTrust.RestoreState(state.signalTrust);

            // Tasks 9–12 Wave 3 (V6) — follow-up scheduler restored additively;
            // pre-V6 saves leave it empty. Restore fires no events, so a
            // restored pending follow-up fires exactly once on its due tick.
            FollowUps.RestoreState(state.signalFollowUps);
            FollowUps.SetDay(Day);

            LastEvent = "Radio state restored.";
        }

        // ── Triangulation demo actions ────────────────────────────────

        /// <summary>
        /// Active observation station identifier (defaults to "station_alpha").
        /// Provides an authored station-selection seam for multi-station setups (D19b).
        /// </summary>
        public string ActiveStationId { get; set; } = "station_alpha";

        /// <summary>Sets the active observation station identifier (D19b).</summary>
        public void SetActiveStation(string stationId)
        {
            if (!string.IsNullOrWhiteSpace(stationId))
            {
                ActiveStationId = stationId.Trim();
            }
        }

        /// <summary>Record a directional observation of a signal.</summary>
        public string RecordObservation(string signalId, float bearing, float signalStrength = 0.7f, float noise = 0.2f, string? stationId = null)
        {
            string resolvedStation = !string.IsNullOrWhiteSpace(stationId) && stationId != "station_alpha"
                ? stationId!
                : ActiveStationId;

            var obs = new RadioObservation
            {
                signalId = signalId,
                stationId = resolvedStation,
                day = Day,
                hour = 12f,
                bearingDegrees = bearing,
                errorDegrees = 5f + noise * 10f,
                signalStrength = signalStrength,
                noiseLevel = noise,
                frequencyMhz = CurrentFrequency,
                weatherCondition = "Clear",
                operatorSkill = 0.6f
            };
            bool ok = Triangulation.RecordObservation(obs);
            return ok
                ? $"Observation recorded: {signalId} at bearing {bearing:F0}° (strength {signalStrength:F2}, noise {noise:F2})."
                : "Invalid observation.";
        }

        public string RecordObservationDemo(string signalId, float bearing, float signalStrength = 0.7f, float noise = 0.2f)
            => RecordObservation(signalId, bearing, signalStrength, noise);

        /// <summary>Attempt to triangulate a signal.</summary>
        public string TriangulateSignal(string signalId)
        {
            var candidate = Triangulation.Triangulate(signalId, Rng);
            if (candidate == null)
                return $"Not enough observations for {signalId}. Need at least {SignalTriangulationSystem.MinObservationsForHypothesis}.";
            bool discovered = Triangulation.IsLocationDiscovered(candidate.locationId);
            return discovered
                ? $"Triangulation complete! Location {candidate.locationId} discovered (confidence {candidate.confidence:F2}, uncertainty ±{candidate.uncertaintyRadiusKm:F0} km)."
                : $"Hypothesis: {candidate.locationId} (confidence {candidate.confidence:F2}, uncertainty ±{candidate.uncertaintyRadiusKm:F0} km, {candidate.observationCount} observations).";
        }

        public string TriangulateDemo(string signalId) => TriangulateSignal(signalId);

        /// <summary>Get triangulation status for a signal.</summary>
        public string TriangulationStatusLine(string signalId)
        {
            int obsCount = Triangulation.GetObservationCount(signalId);
            var candidate = Triangulation.GetCandidate(signalId);
            if (candidate == null)
                return $"Signal {signalId}: {obsCount} observation(s). No hypothesis yet.";
            bool discovered = Triangulation.IsLocationDiscovered(candidate.locationId);
            return $"Signal {signalId}: {obsCount} obs, confidence {candidate.confidence:F2}, " +
                   $"uncertainty ±{candidate.uncertaintyRadiusKm:F0} km" +
                   (discovered ? " [DISCOVERED]" : " [pending]");
        }

        public string StatusLine()
        {
            string carrier = LastIntercept.HasValue
                ? (string.IsNullOrWhiteSpace(LastIntercept.Value.FactionId)
                    ? "dead air"
                    : LastIntercept.Value.Callsign)
                : "no carrier sampled";
            return $"Radio: {Engine.FactionCount} channels · {CurrentFrequency:0.00} MHz · {carrier} · " +
                   $"{_history.Count} intercepts logged (day {Day}).";
        }

        public RadioProgramSlot? GetCurrentSlot(string stationId, int? hour = null)
        {
            int h = hour ?? 12;
            return Stations.GetCurrentSlot(stationId, Day, h);
        }

        public RadioProgramSlot? GetNextSlot(string stationId, int? hour = null)
        {
            int h = hour ?? 12;
            return Stations.GetNextSlot(stationId, Day, h);
        }

        public RadioSignalStrength GetSignalStrength(string stationId, RadioReceptionFactors? factors = null)
        {
            return Stations.ComputeSignalStrength(stationId, factors);
        }

        public RadioStationDefinition? GetStationAtCurrentFrequency()
        {
            return Stations.FindStationAtFrequency(CurrentFrequency, 0.2f);
        }

        private float FirstFrequency()
        {
            var factions = Engine.GetAllFactions();
            return factions.Count > 0 ? Engine.GetFactionFrequency(factions[0]) : 88.4f;
        }
    }
}
