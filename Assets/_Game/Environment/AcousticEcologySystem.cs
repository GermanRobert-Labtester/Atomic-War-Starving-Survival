using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Expansion VIII — Acoustic Ecology System. Sound is a physical resource
    /// and a hazard. Every room has an AcousticProfile. Concrete amplifies screams.
    /// Acoustic foam deadens sound, allowing secret meetings and hiding corpse burning.
    /// Item_AcousticDecoy draws raider UtilityAI to a specific location.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class AcousticEcologySystem
    {
        // ── Material constants ────────────────────────────────────────
        public const string Material_AcousticFoam = "acoustic_foam_panel";
        public const string Item_AcousticDecoy = "acoustic_decoy";
        public const string Item_MetronomeWindup = "metronome_windup";

        // ── Acoustic thresholds ───────────────────────────────────────
        public const float ConcreteReverbMultiplier = 1.5f;
        public const float FoamAbsorptionFraction = 0.85f;
        public const float SecretMeetingThreshold = 20f; // dB below this = secret safe
        public const float IncineratorDetectionThreshold = 40f;
        public const float DecoyRadiusMeters = 15f;
        public const float DecoyDurationMinutes = 5f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string, float> OnRoomAcousticChanged;
        public event Action<string> OnSecretMeetingSafe;
        public event Action<string> OnIncineratorDetected;
        public event Action<string> OnDecoyDeployed;
        public event Action<string> OnGossipAmplified;

        private readonly Dictionary<string, RoomAcousticProfile> _profiles = new Dictionary<string, RoomAcousticProfile>();
        private readonly List<AcousticDecoy> _activeDecoys = new List<AcousticDecoy>();

        public IReadOnlyDictionary<string, RoomAcousticProfile> Profiles => _profiles;
        public IReadOnlyList<AcousticDecoy> ActiveDecoys => _activeDecoys;

        // ── Room configuration ────────────────────────────────────────

        /// <summary>Set a room's acoustic profile.</summary>
        public void SetRoomProfile(string roomId, float baseNoise, bool hasFoam,
            bool isConcrete = true)
        {
            _profiles[roomId] = new RoomAcousticProfile
            {
                RoomId = roomId,
                BaseNoise = baseNoise,
                HasAcousticFoam = hasFoam,
                IsConcrete = isConcrete
            };
        }

        /// <summary>Install acoustic foam in a room.</summary>
        public bool InstallFoam(string roomId)
        {
            if (!_profiles.TryGetValue(roomId, out var profile)) return false;
            profile.HasAcousticFoam = true;
            profile.BaseNoise *= (1f - FoamAbsorptionFraction);
            OnRoomAcousticChanged?.Invoke(roomId, profile.BaseNoise);
            return true;
        }

        /// <summary>Get effective noise level in a room.</summary>
        public float GetEffectiveNoise(string roomId)
        {
            if (!_profiles.TryGetValue(roomId, out var profile)) return 0f;
            float noise = profile.BaseNoise;
            if (profile.IsConcrete && !profile.HasAcousticFoam)
                noise *= ConcreteReverbMultiplier;
            if (profile.HasAcousticFoam)
                noise *= (1f - FoamAbsorptionFraction);
            return noise;
        }

        // ── Secret meetings ───────────────────────────────────────────

        /// <summary>Check if a room is quiet enough for a secret meeting.</summary>
        public bool IsSecretMeetingSafe(string roomId)
        {
            float noise = GetEffectiveNoise(roomId);
            if (noise < SecretMeetingThreshold)
            {
                OnSecretMeetingSafe?.Invoke(roomId);
                return true;
            }
            return false;
        }

        // ── Incinerator detection ─────────────────────────────────────

        /// <summary>
        /// Check if the incinerator's noise is detectable from outside the room.
        /// Foam hides it.
        /// </summary>
        public bool IsIncineratorDetectable(string incineratorRoomId)
        {
            if (!_profiles.TryGetValue(incineratorRoomId, out var profile)) return false;
            if (profile.HasAcousticFoam) return false; // Foam hides it
            return profile.BaseNoise >= IncineratorDetectionThreshold;
        }

        /// <summary>
        /// Process incinerator noise detection. Triggers gossip if detected.
        /// </summary>
        public void ProcessIncineratorNoise(string incineratorRoomId, string adjacentRoomId)
        {
            if (IsIncineratorDetectable(incineratorRoomId))
            {
                OnIncineratorDetected?.Invoke(adjacentRoomId);
                OnGossipAmplified?.Invoke(adjacentRoomId);
            }
        }

        // ── Acoustic decoy ────────────────────────────────────────────

        /// <summary>
        /// Deploy an acoustic decoy (wind-up metronome in a tin can).
        /// Draws raider UtilityAI to a specific location during a siege.
        /// </summary>
        public bool DeployDecoy(string roomId, string deployerId)
        {
            _activeDecoys.Add(new AcousticDecoy
            {
                RoomId = roomId,
                DeployerId = deployerId,
                MinutesRemaining = DecoyDurationMinutes
            });
            OnDecoyDeployed?.Invoke(roomId);
            return true;
        }

        /// <summary>Check if a room has an active decoy.</summary>
        public bool HasActiveDecoy(string roomId)
        {
            for (int i = 0; i < _activeDecoys.Count; i++)
                if (_activeDecoys[i].RoomId == roomId && _activeDecoys[i].MinutesRemaining > 0f)
                    return true;
            return false;
        }

        /// <summary>Tick decoy timers.</summary>
        public void Tick(float gameMinutes)
        {
            for (int i = _activeDecoys.Count - 1; i >= 0; i--)
            {
                _activeDecoys[i].MinutesRemaining -= gameMinutes;
                if (_activeDecoys[i].MinutesRemaining <= 0f)
                    _activeDecoys.RemoveAt(i);
            }
        }

        // ── Gossip amplification ──────────────────────────────────────

        /// <summary>
        /// Get gossip amplification multiplier for a room based on acoustics.
        /// Concrete amplifies; foam dampens.
        /// </summary>
        public float GetGossipAmplification(string roomId)
        {
            if (!_profiles.TryGetValue(roomId, out var profile)) return 1f;
            if (profile.HasAcousticFoam) return 0.3f; // Foam dampens gossip
            if (profile.IsConcrete) return ConcreteReverbMultiplier;
            return 1f;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public AcousticEcologySave CaptureState()
        {
            var entries = new RoomAcousticSave[_profiles.Count];
            int i = 0;
            foreach (var kv in _profiles)
            {
                var p = kv.Value;
                entries[i++] = new RoomAcousticSave
                {
                    RoomId = p.RoomId,
                    BaseNoise = p.BaseNoise,
                    HasAcousticFoam = p.HasAcousticFoam,
                    IsConcrete = p.IsConcrete
                };
            }
            var decoys = new DecoySave[_activeDecoys.Count];
            for (int j = 0; j < _activeDecoys.Count; j++)
                decoys[j] = new DecoySave
                {
                    RoomId = _activeDecoys[j].RoomId,
                    DeployerId = _activeDecoys[j].DeployerId,
                    MinutesRemaining = _activeDecoys[j].MinutesRemaining
                };
            return new AcousticEcologySave { Profiles = entries, ActiveDecoys = decoys };
        }

        public void RestoreState(AcousticEcologySave save)
        {
            _profiles.Clear();
            _activeDecoys.Clear();
            if (save == null) return;
            if (save.Profiles != null)
                for (int i = 0; i < save.Profiles.Length; i++)
                    if (save.Profiles[i] != null)
                        _profiles[save.Profiles[i].RoomId] = new RoomAcousticProfile
                        {
                            RoomId = save.Profiles[i].RoomId,
                            BaseNoise = save.Profiles[i].BaseNoise,
                            HasAcousticFoam = save.Profiles[i].HasAcousticFoam,
                            IsConcrete = save.Profiles[i].IsConcrete
                        };
            if (save.ActiveDecoys != null)
                for (int i = 0; i < save.ActiveDecoys.Length; i++)
                    if (save.ActiveDecoys[i] != null)
                        _activeDecoys.Add(new AcousticDecoy
                        {
                            RoomId = save.ActiveDecoys[i].RoomId,
                            DeployerId = save.ActiveDecoys[i].DeployerId,
                            MinutesRemaining = save.ActiveDecoys[i].MinutesRemaining
                        });
        }
    }

    public class RoomAcousticProfile
    {
        public string RoomId;
        public float BaseNoise;
        public bool HasAcousticFoam;
        public bool IsConcrete;
    }

    public class AcousticDecoy
    {
        public string RoomId;
        public string DeployerId;
        public float MinutesRemaining;
    }

    [Serializable]
    public class AcousticEcologySave
    {
        public RoomAcousticSave[] Profiles;
        public DecoySave[] ActiveDecoys;
    }

    [Serializable]
    public class RoomAcousticSave
    {
        public string RoomId;
        public float BaseNoise;
        public bool HasAcousticFoam;
        public bool IsConcrete;
    }

    [Serializable]
    public class DecoySave
    {
        public string RoomId;
        public string DeployerId;
        public float MinutesRemaining;
    }
}
