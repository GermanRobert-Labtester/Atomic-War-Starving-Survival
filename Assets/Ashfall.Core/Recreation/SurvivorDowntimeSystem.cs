using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Recreation
{
    [Serializable]
    public sealed class HobbyDef
    {
        public string hobby_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int duration_hours { get; set; } = 2;
        public float base_stress_relief { get; set; } = 15f;
        public int morale_effect { get; set; } = 5;
        public List<string> required_item_ids { get; set; } = new List<string>();
        public List<string> optional_item_ids { get; set; } = new List<string>();
        public List<string> required_room_tags { get; set; } = new List<string>();
        public List<string> compatible_trait_tags { get; set; } = new List<string>();
        public List<string> incompatible_trait_tags { get; set; } = new List<string>();
        public int social_min { get; set; } = 1;
        public int social_max { get; set; } = 4;
        public float brawl_risk { get; set; } = 0.02f;
        public string output_item_id { get; set; } = string.Empty;
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class RecreationCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<HobbyDef> hobbies { get; set; } = new List<HobbyDef>();
    }

    [Serializable]
    public sealed class ActiveHobbySession
    {
        public string sessionId = string.Empty;
        public string hobbyId = string.Empty;
        public string roomId = string.Empty;
        public List<string> participantIds = new List<string>();
        public int startedDay;
        public bool isFinished;
    }

    [Serializable]
    public sealed class SurvivorHobbyProfile
    {
        public string survivorId = string.Empty;
        public string favoriteHobbyId = string.Empty;
        public int skillLevel = 1;
        public float skillXp = 0f;
        public int totalSessionsCompleted = 0;
        public float stressRelievedTotal = 0f;
    }

    [Serializable]
    public sealed class RecreationState
    {
        public int schemaVersion = 1;
        public List<ActiveHobbySession> activeSessions = new List<ActiveHobbySession>();
        public List<SurvivorHobbyProfile> profiles = new List<SurvivorHobbyProfile>();
        public List<string> sessionHistory = new List<string>();
    }

    public sealed class SurvivorDowntimeSystem
    {
        public const string SystemId = "recreation";
        private RecreationState _state = new RecreationState();
        private readonly Dictionary<string, HobbyDef> _catalog = new Dictionary<string, HobbyDef>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly NeedsSystem _needs;
        private readonly ShelterSocialDynamicsSystem? _social;
        private readonly ILog _log;
        private int _currentDay;
        private int _sessionCounter;

        public RecreationState State => _state;
        public event Action<ActiveHobbySession>? OnHobbyStarted;
        public event Action<ActiveHobbySession, float>? OnHobbyCompleted;
        public event Action<ActiveHobbySession, string, string>? OnHobbyBrawl;

        public SurvivorDowntimeSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            NeedsSystem needs,
            ShelterSocialDynamicsSystem? social = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _needs = needs ?? throw new ArgumentNullException(nameof(needs));
            _social = social;
            _log = log ?? NullLog.Instance;
            RegisterDefaultHobbies();
        }

        private void RegisterDefaultHobbies()
        {
            RegisterHobby(new HobbyDef
            {
                hobby_id = "hobby_whittling",
                display_name = "Wood Whittling & Carving",
                duration_hours = 2,
                base_stress_relief = 15f,
                morale_effect = 5,
                required_item_ids = new List<string> { "scrap_wood" },
                social_min = 1,
                social_max = 1,
                output_item_id = "item_carved_figurine"
            });
            RegisterHobby(new HobbyDef
            {
                hobby_id = "hobby_guitar",
                display_name = "Acoustic Guitar Strumming",
                duration_hours = 2,
                base_stress_relief = 24f,
                morale_effect = 10,
                required_item_ids = new List<string> { "item_acoustic_guitar" },
                social_min = 1,
                social_max = 4,
                brawl_risk = 0.02f
            });
            RegisterHobby(new HobbyDef
            {
                hobby_id = "hobby_card_games",
                display_name = "Tabletop Card Games & Poker",
                duration_hours = 2,
                base_stress_relief = 20f,
                morale_effect = 8,
                required_item_ids = new List<string> { "item_playing_cards" },
                social_min = 2,
                social_max = 4,
                brawl_risk = 0.05f
            });
            RegisterHobby(new HobbyDef
            {
                hobby_id = "hobby_storytelling",
                display_name = "Campfire Old-World Storytelling",
                duration_hours = 2,
                base_stress_relief = 22f,
                morale_effect = 9,
                social_min = 2,
                social_max = 6,
                brawl_risk = 0.01f
            });
        }

        public void RegisterHobby(HobbyDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.hobby_id))
                _catalog[def.hobby_id] = def;
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;
            try
            {
                var serializer = new SystemTextJsonSerializer();
                var catalog = serializer.Deserialize<RecreationCatalog>(jsonContent);
                if (catalog?.hobbies != null)
                {
                    foreach (var h in catalog.hobbies)
                        RegisterHobby(h);
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[SurvivorDowntime] Failed to load recreation catalog: {ex.Message}");
            }
        }

        public HobbyDef? GetHobby(string hobbyId)
        {
            if (string.IsNullOrEmpty(hobbyId)) return null;
            _catalog.TryGetValue(hobbyId, out var def);
            return def;
        }

        public SurvivorHobbyProfile GetOrCreateProfile(string survivorId)
        {
            var profile = _state.profiles.Find(p => p.survivorId == survivorId);
            if (profile == null)
            {
                profile = new SurvivorHobbyProfile { survivorId = survivorId };
                _state.profiles.Add(profile);
            }
            return profile;
        }

        public ActionResult StartSession(string hobbyId, string roomId, List<string> participantIds)
        {
            var hobby = GetHobby(hobbyId);
            if (hobby == null)
                return ActionResult.Failed("unknown_hobby", "recreation.unknown_hobby");

            if (participantIds == null || participantIds.Count < hobby.social_min)
                return ActionResult.Blocked("too_few_participants", "recreation.too_few_participants");

            if (participantIds.Count > hobby.social_max)
                return ActionResult.Blocked("too_many_participants", "recreation.too_many_participants");

            if (hobby.required_item_ids != null && hobby.required_item_ids.Count > 0)
            {
                foreach (var reqItem in hobby.required_item_ids)
                {
                    if (_inventory.CountById(reqItem) < 1)
                        return ActionResult.Blocked("missing_hobby_item", "recreation.missing_hobby_item");
                }
            }

            var session = new ActiveHobbySession
            {
                sessionId = $"session_{_currentDay}_{hobbyId}_{++_sessionCounter}",
                hobbyId = hobbyId,
                roomId = roomId,
                participantIds = new List<string>(participantIds),
                startedDay = _currentDay,
                isFinished = false
            };

            _state.activeSessions.Add(session);
            OnHobbyStarted?.Invoke(session);
            return ActionResult.Success("recreation.session_started",
                new Dictionary<string, double> { { "participants", participantIds.Count } });
        }

        public ActionResult CompleteSession(string sessionId)
        {
            var session = _state.activeSessions.Find(s => s.sessionId == sessionId && !s.isFinished);
            if (session == null)
                return ActionResult.Failed("unknown_session", "recreation.unknown_session");

            var hobby = GetHobby(session.hobbyId);
            if (hobby == null) return ActionResult.Failed("unknown_hobby", "recreation.unknown_hobby");

            float totalStressRelieved = 0f;

            foreach (var sid in session.participantIds)
            {
                var prof = GetOrCreateProfile(sid);
                float skillMultiplier = 1.0f + (0.10f * (prof.skillLevel - 1));
                float stressRelief = hobby.base_stress_relief * skillMultiplier;

                prof.stressRelievedTotal += stressRelief;
                prof.totalSessionsCompleted++;
                prof.skillXp += 15f;
                if (prof.skillXp >= 50f && prof.skillLevel < 5)
                {
                    prof.skillLevel++;
                    prof.skillXp = 0f;
                }

                _needs.Modify(sid, NeedKind.Morale, hobby.morale_effect);
                totalStressRelieved += stressRelief;
            }

            if (session.participantIds.Count > 1)
            {
                if (_rng.NextDouble() < hobby.brawl_risk)
                {
                    string p1 = session.participantIds[0];
                    string p2 = session.participantIds[1];
                    _needs.Modify(p1, NeedKind.Morale, -5);
                    _needs.Modify(p2, NeedKind.Morale, -5);
                    OnHobbyBrawl?.Invoke(session, p1, p2);
                }
            }

            if (!string.IsNullOrEmpty(hobby.output_item_id))
            {
                _inventory.AddById(hobby.output_item_id, 1);
            }

            session.isFinished = true;
            _state.sessionHistory.Add(session.sessionId);
            _state.activeSessions.Remove(session);

            OnHobbyCompleted?.Invoke(session, totalStressRelieved);
            return ActionResult.Success("recreation.session_completed",
                new Dictionary<string, double> { { "stressRelieved", totalStressRelieved } });
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            var pending = new List<ActiveHobbySession>(_state.activeSessions);
            foreach (var s in pending)
            {
                CompleteSession(s.sessionId);
            }
        }

        public RecreationState CaptureState() => CloneState(_state);

        public void RestoreState(RecreationState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static RecreationState CloneState(RecreationState src)
        {
            if (src == null) return new RecreationState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<RecreationState>(json) ?? new RecreationState();
        }
    }
}
