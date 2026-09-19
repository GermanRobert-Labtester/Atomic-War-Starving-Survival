// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Propaganda
{
    public enum PropagandaMedium
    {
        Leaflet = 0,
        RadioBroadcast = 1,
        WallPosting = 2,
        RumorCampaign = 3,
        BroadcastIntercept = 4
    }

    public enum MessageTruthfulness
    {
        Truth = 0,
        HalfTruth = 1,
        Lie = 2,
        Exaggeration = 3
    }

    public enum PropagandaTheme
    {
        Hope = 0,
        Fear = 1,
        Unity = 2,
        Division = 3,
        Triumph = 4,
        Sacrifice = 5
    }

    public enum PropagandaObjective
    {
        UndermineFaction = 0,
        BoostMorale = 1,
        RecruitDefectors = 2,
        ProtectIdentity = 3,
        DestabilizeRegion = 4
    }

    public enum CampaignStatus
    {
        Planned = 0,
        Active = 1,
        Completed = 2,
        Compromised = 3
    }

    [Serializable]
    public sealed class PropagandaMessage
    {
        public string MessageId { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public PropagandaMedium Medium { get; set; } = PropagandaMedium.Leaflet;
        public MessageTruthfulness Truthfulness { get; set; } = MessageTruthfulness.Truth;
        public PropagandaTheme Theme { get; set; } = PropagandaTheme.Hope;
        public string TargetFactionId { get; set; } = string.Empty;
        public string TargetAudience { get; set; } = "Civilians";
        public string Content { get; set; } = string.Empty;
        public float Quality { get; set; } = 50f;
        public int CreationDay { get; set; } = 1;
    }

    [Serializable]
    public sealed class PropagandaCampaign
    {
        public string CampaignId { get; set; } = string.Empty;
        public string CampaignName { get; set; } = string.Empty;
        public string TargetFactionId { get; set; } = string.Empty;
        public PropagandaObjective Objective { get; set; } = PropagandaObjective.UndermineFaction;
        public List<string> MessageIds { get; set; } = new List<string>();
        public int StartDay { get; set; } = 1;
        public int DurationDays { get; set; } = 5;
        public CampaignStatus Status { get; set; } = CampaignStatus.Planned;
        public float AccumulatedEffectiveness { get; set; } = 0f;
        public bool WasDetected { get; set; } = false;
    }

    [Serializable]
    public sealed class PropagandaState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public float ShelterCredibility { get; set; } = 75f; // 0 to 100
        public List<PropagandaMessage> Messages { get; set; } = new List<PropagandaMessage>();
        public List<PropagandaCampaign> Campaigns { get; set; } = new List<PropagandaCampaign>();
        public Dictionary<string, float> FactionMoraleImpacts { get; set; } = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Plan 168 — Propaganda & Morale Warfare System.
    /// Governs creation of propaganda messages across mediums (leaflets, broadcasts, wall postings, rumors, intercepts),
    /// truthfulness and themes, coordination into multi-day campaigns, detection risks, and impact on faction morale.
    /// </summary>
    public sealed class PropagandaSystem
    {
        private readonly PropagandaState _state;

        public event Action<PropagandaMessage>? OnMessageCreated;
        public event Action<PropagandaCampaign>? OnCampaignStarted;
        public event Action<PropagandaCampaign, float>? OnCampaignProgressed;
        public event Action<PropagandaCampaign>? OnCampaignCompleted;
        public event Action<PropagandaCampaign>? OnCampaignCompromised;
        public event Action? OnStateChanged;

        public float ShelterCredibility => _state.ShelterCredibility;
        public int MessageCount => _state.Messages.Count;
        public int ActiveCampaignCount => _state.Campaigns.Count(c => c.Status == CampaignStatus.Active);
        public IReadOnlyList<PropagandaMessage> Messages => _state.Messages;
        public IReadOnlyList<PropagandaCampaign> Campaigns => _state.Campaigns;
        public PropagandaState State => _state;

        public PropagandaSystem(PropagandaState? state = null)
        {
            _state = state ?? new PropagandaState();
        }

        public PropagandaMessage CreateMessage(
            string authorId,
            PropagandaMedium medium,
            MessageTruthfulness truthfulness,
            PropagandaTheme theme,
            string targetFactionId,
            string content,
            float authorSkill = 50f,
            string targetAudience = "Civilians",
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(authorId)) throw new ArgumentNullException(nameof(authorId));
            if (string.IsNullOrWhiteSpace(targetFactionId)) throw new ArgumentNullException(nameof(targetFactionId));

            float baseQuality = Math.Clamp(authorSkill, 10f, 100f);

            // Truthfulness modifier on quality vs risk:
            // Lies are flashy (+10 quality) but risky; truth is grounded
            float quality = truthfulness switch
            {
                MessageTruthfulness.Lie => Math.Min(100f, baseQuality + 10f),
                MessageTruthfulness.Exaggeration => Math.Min(100f, baseQuality + 5f),
                MessageTruthfulness.HalfTruth => baseQuality,
                _ => Math.Max(10f, baseQuality - 5f) // Truth requires nuance
            };

            var message = new PropagandaMessage
            {
                MessageId = $"prop_msg_{_state.NextSequence++}",
                AuthorId = authorId.Trim(),
                Medium = medium,
                Truthfulness = truthfulness,
                Theme = theme,
                TargetFactionId = targetFactionId.Trim(),
                TargetAudience = targetAudience ?? "Civilians",
                Content = content ?? string.Empty,
                Quality = quality,
                CreationDay = Math.Max(1, currentDay)
            };

            _state.Messages.Add(message);
            OnMessageCreated?.Invoke(message);
            OnStateChanged?.Invoke();
            return message;
        }

        public PropagandaCampaign LaunchCampaign(
            string campaignName,
            string targetFactionId,
            PropagandaObjective objective,
            IEnumerable<string> messageIds,
            int durationDays = 5,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(campaignName)) throw new ArgumentNullException(nameof(campaignName));
            if (string.IsNullOrWhiteSpace(targetFactionId)) throw new ArgumentNullException(nameof(targetFactionId));

            var campaign = new PropagandaCampaign
            {
                CampaignId = $"prop_cmp_{_state.NextSequence++}",
                CampaignName = campaignName.Trim(),
                TargetFactionId = targetFactionId.Trim(),
                Objective = objective,
                MessageIds = messageIds?.ToList() ?? new List<string>(),
                StartDay = Math.Max(1, currentDay),
                DurationDays = Math.Max(1, durationDays),
                Status = CampaignStatus.Active,
                AccumulatedEffectiveness = 0f,
                WasDetected = false
            };

            _state.Campaigns.Add(campaign);
            OnCampaignStarted?.Invoke(campaign);
            OnStateChanged?.Invoke();
            return campaign;
        }

        public void TickDay(int currentDay, float randomRoll = 0.5f)
        {
            foreach (var cmp in _state.Campaigns)
            {
                if (cmp.Status != CampaignStatus.Active) continue;

                int daysElapsed = currentDay - cmp.StartDay;
                if (daysElapsed < 0) continue;

                // Resolve day progress
                float dailyEffectiveness = CalculateDailyEffectiveness(cmp);
                cmp.AccumulatedEffectiveness = Math.Clamp(cmp.AccumulatedEffectiveness + dailyEffectiveness, 0f, 100f);

                OnCampaignProgressed?.Invoke(cmp, dailyEffectiveness);

                // Check detection risk based on mediums & truthfulness of campaign messages
                float detectionRisk = CalculateDetectionRisk(cmp);
                if (randomRoll < detectionRisk && !cmp.WasDetected)
                {
                    cmp.WasDetected = true;
                    cmp.Status = CampaignStatus.Compromised;

                    // Penalty on shelter credibility if lies were exposed
                    bool usedLies = cmp.MessageIds
                        .Select(id => _state.Messages.FirstOrDefault(m => m.MessageId == id))
                        .Any(m => m != null && (m.Truthfulness == MessageTruthfulness.Lie || m.Truthfulness == MessageTruthfulness.Exaggeration));

                    float penalty = usedLies ? 20f : 5f;
                    _state.ShelterCredibility = Math.Clamp(_state.ShelterCredibility - penalty, 0f, 100f);

                    OnCampaignCompromised?.Invoke(cmp);
                    continue;
                }

                // If duration reached and not compromised, complete it
                if (daysElapsed >= cmp.DurationDays)
                {
                    cmp.Status = CampaignStatus.Completed;
                    ApplyCampaignOutcome(cmp);
                    OnCampaignCompleted?.Invoke(cmp);
                }
            }
            OnStateChanged?.Invoke();
        }

        private float CalculateDailyEffectiveness(PropagandaCampaign campaign)
        {
            var messages = campaign.MessageIds
                .Select(id => _state.Messages.FirstOrDefault(m => m.MessageId == id))
                .Where(m => m != null)
                .ToList();

            if (messages.Count == 0) return 5f;

            float avgQuality = messages.Average(m => m!.Quality);
            float mediumMod = messages.Average(m => m!.Medium switch
            {
                PropagandaMedium.RadioBroadcast => 1.5f,
                PropagandaMedium.BroadcastIntercept => 1.8f,
                PropagandaMedium.Leaflet => 1.0f,
                PropagandaMedium.RumorCampaign => 0.9f,
                PropagandaMedium.WallPosting => 0.7f,
                _ => 1.0f
            });

            // Shelter credibility scales impact
            float credibilityMod = _state.ShelterCredibility / 100f;
            return (avgQuality / 10f) * mediumMod * (0.5f + (0.5f * credibilityMod));
        }

        private float CalculateDetectionRisk(PropagandaCampaign campaign)
        {
            var messages = campaign.MessageIds
                .Select(id => _state.Messages.FirstOrDefault(m => m.MessageId == id))
                .Where(m => m != null)
                .ToList();

            if (messages.Count == 0) return 0.05f;

            return messages.Max(m => m!.Medium switch
            {
                PropagandaMedium.WallPosting => 0.35f,
                PropagandaMedium.BroadcastIntercept => 0.30f,
                PropagandaMedium.Leaflet => 0.20f,
                PropagandaMedium.RadioBroadcast => 0.15f,
                PropagandaMedium.RumorCampaign => 0.10f,
                _ => 0.15f
            });
        }

        private void ApplyCampaignOutcome(PropagandaCampaign campaign)
        {
            float impact = campaign.AccumulatedEffectiveness;
            // Morale drop or boost depending on objective
            float moraleDelta = campaign.Objective switch
            {
                PropagandaObjective.UndermineFaction => -impact * 0.5f,
                PropagandaObjective.DestabilizeRegion => -impact * 0.4f,
                PropagandaObjective.BoostMorale => impact * 0.5f,
                PropagandaObjective.RecruitDefectors => -impact * 0.3f,
                _ => 0f
            };

            if (!_state.FactionMoraleImpacts.ContainsKey(campaign.TargetFactionId))
            {
                _state.FactionMoraleImpacts[campaign.TargetFactionId] = 0f;
            }

            _state.FactionMoraleImpacts[campaign.TargetFactionId] += moraleDelta;
        }

        public float GetFactionMoraleImpact(string factionId)
        {
            if (string.IsNullOrWhiteSpace(factionId)) return 0f;
            return _state.FactionMoraleImpacts.TryGetValue(factionId.Trim(), out var impact) ? impact : 0f;
        }

        public PropagandaState CaptureState()
        {
            var state = new PropagandaState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                ShelterCredibility = _state.ShelterCredibility,
                Messages = new List<PropagandaMessage>(_state.Messages.Count),
                Campaigns = new List<PropagandaCampaign>(_state.Campaigns.Count),
                FactionMoraleImpacts = new Dictionary<string, float>(_state.FactionMoraleImpacts, StringComparer.OrdinalIgnoreCase)
            };

            foreach (var m in _state.Messages)
            {
                state.Messages.Add(new PropagandaMessage
                {
                    MessageId = m.MessageId,
                    AuthorId = m.AuthorId,
                    Medium = m.Medium,
                    Truthfulness = m.Truthfulness,
                    Theme = m.Theme,
                    TargetFactionId = m.TargetFactionId,
                    TargetAudience = m.TargetAudience,
                    Content = m.Content,
                    Quality = m.Quality,
                    CreationDay = m.CreationDay
                });
            }

            foreach (var c in _state.Campaigns)
            {
                state.Campaigns.Add(new PropagandaCampaign
                {
                    CampaignId = c.CampaignId,
                    CampaignName = c.CampaignName,
                    TargetFactionId = c.TargetFactionId,
                    Objective = c.Objective,
                    MessageIds = new List<string>(c.MessageIds),
                    StartDay = c.StartDay,
                    DurationDays = c.DurationDays,
                    Status = c.Status,
                    AccumulatedEffectiveness = c.AccumulatedEffectiveness,
                    WasDetected = c.WasDetected
                });
            }

            return state;
        }

        public void RestoreState(PropagandaState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.ShelterCredibility = state.ShelterCredibility;

            _state.Messages.Clear();
            if (state.Messages != null)
            {
                foreach (var m in state.Messages)
                {
                    _state.Messages.Add(new PropagandaMessage
                    {
                        MessageId = m.MessageId,
                        AuthorId = m.AuthorId,
                        Medium = m.Medium,
                        Truthfulness = m.Truthfulness,
                        Theme = m.Theme,
                        TargetFactionId = m.TargetFactionId,
                        TargetAudience = m.TargetAudience,
                        Content = m.Content,
                        Quality = m.Quality,
                        CreationDay = m.CreationDay
                    });
                }
            }

            _state.Campaigns.Clear();
            if (state.Campaigns != null)
            {
                foreach (var c in state.Campaigns)
                {
                    _state.Campaigns.Add(new PropagandaCampaign
                    {
                        CampaignId = c.CampaignId,
                        CampaignName = c.CampaignName,
                        TargetFactionId = c.TargetFactionId,
                        Objective = c.Objective,
                        MessageIds = new List<string>(c.MessageIds ?? Enumerable.Empty<string>()),
                        StartDay = c.StartDay,
                        DurationDays = c.DurationDays,
                        Status = c.Status,
                        AccumulatedEffectiveness = c.AccumulatedEffectiveness,
                        WasDetected = c.WasDetected
                    });
                }
            }

            _state.FactionMoraleImpacts.Clear();
            if (state.FactionMoraleImpacts != null)
            {
                foreach (var kvp in state.FactionMoraleImpacts)
                {
                    _state.FactionMoraleImpacts[kvp.Key] = kvp.Value;
                }
            }
            OnStateChanged?.Invoke();
        }
    }
}
