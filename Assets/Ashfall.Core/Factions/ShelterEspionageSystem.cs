// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Factions
{
    [Serializable]
    public sealed class SleeperAgentRecord
    {
        public string survivorId = string.Empty;
        public string factionId = string.Empty;
        public int loyaltyPermille = 500;
        public int suspicionPermille;
        public bool isIdentified;
        public bool isTurnedDoubleAgent;
        public int leaksCommittedCount;
    }

    [Serializable]
    public sealed class ActiveDeadDrop
    {
        public string dropId = string.Empty;
        public string templateId = string.Empty;
        public string targetLocationId = string.Empty;
        public int remainingDays;
        public int intelYield;
        public bool isIntercepted;
    }

    [Serializable]
    public sealed class SabotageIncident
    {
        public string incidentId = string.Empty;
        public string operationId = string.Empty;
        public string targetSubsystem = string.Empty;
        public int dayRecorded;
        public bool preventedByCounterIntel;
        public string summary = string.Empty;
    }

    [Serializable]
    public sealed class ShelterEspionageState
    {
        public string systemId = ShelterEspionageSystem.SystemId;
        public Dictionary<string, SleeperAgentRecord> sleeperAgents = new Dictionary<string, SleeperAgentRecord>(StringComparer.Ordinal);
        public List<ActiveDeadDrop> activeDeadDrops = new List<ActiveDeadDrop>();
        public List<SabotageIncident> recentIncidents = new List<SabotageIncident>();
        public int securityCounterIntelScore = 100;
        public int totalIntelPoints;
        public int nextDropCounter = 1;
        public int nextIncidentCounter = 1;
    }

    public sealed class ShelterEspionageSystem
    {
        public const string SystemId = "shelter_espionage";

        private readonly Dictionary<string, FactionOperationDefinition> _operations =
            new Dictionary<string, FactionOperationDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, DeadDropTemplateDefinition> _deadDrops =
            new Dictionary<string, DeadDropTemplateDefinition>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private ShelterEspionageState _state = new ShelterEspionageState();

        public ShelterEspionageSystem(FactionIntelligenceCatalog? catalog = null, ISeededRng? rng = null)
        {
            _rng = rng ?? new SeededRng(4242);
            if (catalog != null)
            {
                LoadCatalog(catalog);
            }
        }

        public void LoadCatalog(FactionIntelligenceCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            _operations.Clear();
            foreach (var op in catalog.operations)
            {
                if (!string.IsNullOrEmpty(op.id))
                    _operations[op.id] = op;
            }

            _deadDrops.Clear();
            foreach (var drop in catalog.dead_drop_templates)
            {
                if (!string.IsNullOrEmpty(drop.id))
                    _deadDrops[drop.id] = drop;
            }
        }

        public IReadOnlyDictionary<string, FactionOperationDefinition> Operations => _operations;
        public IReadOnlyDictionary<string, DeadDropTemplateDefinition> DeadDropTemplates => _deadDrops;

        public bool EnrollSleeperAgent(string survivorId, string factionId, int initialLoyalty = 600)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(factionId))
                return false;

            if (_state.sleeperAgents.ContainsKey(survivorId))
                return false;

            _state.sleeperAgents[survivorId] = new SleeperAgentRecord
            {
                survivorId = survivorId,
                factionId = factionId,
                loyaltyPermille = Math.Clamp(initialLoyalty, 0, 1000),
                suspicionPermille = 0,
                isIdentified = false,
                isTurnedDoubleAgent = false,
                leaksCommittedCount = 0
            };
            return true;
        }

        public bool IsSleeperAgent(string survivorId) =>
            !string.IsNullOrEmpty(survivorId) && _state.sleeperAgents.ContainsKey(survivorId);

        public SleeperAgentRecord? GetSleeperRecord(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            _state.sleeperAgents.TryGetValue(survivorId, out var rec);
            return rec;
        }

        public void AddSuspicion(string survivorId, int amount)
        {
            if (string.IsNullOrEmpty(survivorId) || amount <= 0) return;
            if (_state.sleeperAgents.TryGetValue(survivorId, out var rec))
            {
                rec.suspicionPermille = Math.Clamp(rec.suspicionPermille + amount, 0, 1000);
            }
        }

        public bool InvestigateSuspect(string survivorId, int investigatorSkill, out bool unmasked, out string report)
        {
            unmasked = false;
            report = string.Empty;

            if (!_state.sleeperAgents.TryGetValue(survivorId, out var rec))
            {
                report = "Survivor cleared of all covert suspicion. No hostile handler links found.";
                return true;
            }

            int checkRoll = _rng.Next(0, 1000);
            int threshold = 1000 - (rec.suspicionPermille / 2 + investigatorSkill * 5);

            if (checkRoll >= Math.Max(200, threshold))
            {
                unmasked = true;
                rec.isIdentified = true;
                report = $"Investigation conclusive: {survivorId} verified as active operative for {rec.factionId}.";
            }
            else
            {
                report = $"Investigation inconclusive for {survivorId}. Insufficient physical evidence collected.";
            }

            return true;
        }

        public bool AttemptTurnDoubleAgent(string survivorId, IPlayerInventoryPort? inventory, out string outcome)
        {
            outcome = string.Empty;
            if (!_state.sleeperAgents.TryGetValue(survivorId, out var rec))
            {
                outcome = "Target is not a covert operative.";
                return false;
            }

            if (!rec.isIdentified)
            {
                outcome = "Cannot confront an operative before unmasking their identity.";
                return false;
            }

            if (rec.isTurnedDoubleAgent)
            {
                outcome = "Operative has already sworn allegiance as a double agent.";
                return false;
            }

            // Turning double agent requires 5 scrap_metal or equivalent leverage
            if (inventory != null)
            {
                if (!inventory.HasSufficient("scrap_metal", 5))
                {
                    outcome = "Requires 5 scrap metal to provide security payoff / relocated family guarantee.";
                    return false;
                }
                if (!inventory.TryConsume("scrap_metal", 5))
                {
                    outcome = "Failed to consume security payoff resources.";
                    return false;
                }
            }

            rec.isTurnedDoubleAgent = true;
            rec.loyaltyPermille = Math.Clamp(1000 - rec.loyaltyPermille + 200, 400, 1000);
            _state.totalIntelPoints += 25;
            outcome = $"Operative successfully turned. {survivorId} now feeds disinformation to {rec.factionId}.";
            return true;
        }

        public void SetSecurityCounterIntelScore(int score)
        {
            _state.securityCounterIntelScore = Math.Clamp(score, 0, 1000);
        }

        public void TickDay(int currentDay, IPlayerInventoryPort? inventory = null)
        {
            // 1. Tick active dead drops
            for (int i = _state.activeDeadDrops.Count - 1; i >= 0; i--)
            {
                var drop = _state.activeDeadDrops[i];
                drop.remainingDays--;
                if (drop.remainingDays <= 0 && !drop.isIntercepted)
                {
                    _state.activeDeadDrops.RemoveAt(i);
                }
            }

            // 2. Sleeper activity
            foreach (var kvp in _state.sleeperAgents)
            {
                var agent = kvp.Value;
                if (agent.isTurnedDoubleAgent)
                {
                    // Turned double agents produce intel instead of leaks
                    _state.totalIntelPoints += 2;
                    continue;
                }

                // If not turned, agent conducts periodic leaks
                agent.suspicionPermille = Math.Clamp(agent.suspicionPermille + 5, 0, 1000);
                agent.leaksCommittedCount++;

                // Potential sabotage if high loyalty to faction and low shelter counter-intel
                if (agent.loyaltyPermille > 600 && _rng.Next(0, 100) < 15)
                {
                    bool prevented = _state.securityCounterIntelScore > 150 && _rng.Next(0, 1000) < _state.securityCounterIntelScore;
                    var incident = new SabotageIncident
                    {
                        incidentId = $"inc_{_state.nextIncidentCounter++}",
                        operationId = "fop_inventory_leak",
                        targetSubsystem = "storage",
                        dayRecorded = currentDay,
                        preventedByCounterIntel = prevented,
                        summary = prevented
                            ? $"Counter-intelligence intercepted covert siphon attempt by an unidentified infiltrator."
                            : $"Storage depot manifest tampered with; minor supplies reported missing."
                    };
                    _state.recentIncidents.Add(incident);

                    if (!prevented && inventory != null)
                    {
                        // Minor abstract leak
                        if (inventory.HasSufficient("scrap_metal", 2))
                            inventory.TryConsume("scrap_metal", 2);
                    }
                }
            }

            // 3. Spontaneous dead drop discovery if counter-intel is high
            if (_deadDrops.Count > 0 && _state.activeDeadDrops.Count < 3 && _state.securityCounterIntelScore > 120)
            {
                if (_rng.Next(0, 100) < 25)
                {
                    SpawnRandomDeadDrop();
                }
            }
        }

        public ActiveDeadDrop? SpawnRandomDeadDrop()
        {
            if (_deadDrops.Count == 0) return null;
            var list = new List<DeadDropTemplateDefinition>(_deadDrops.Values);
            var template = list[_rng.Next(0, list.Count)];

            var drop = new ActiveDeadDrop
            {
                dropId = $"drop_{_state.nextDropCounter++}_{template.id}",
                templateId = template.id,
                targetLocationId = template.target_location,
                remainingDays = template.expiry_days,
                intelYield = template.reward_intel_points,
                isIntercepted = false
            };
            _state.activeDeadDrops.Add(drop);
            return drop;
        }

        public bool InterceptDeadDrop(string dropId, out int intelPointsAwarded, out string report)
        {
            intelPointsAwarded = 0;
            report = string.Empty;

            var drop = _state.activeDeadDrops.Find(d => string.Equals(d.dropId, dropId, StringComparison.Ordinal));
            if (drop == null)
            {
                report = $"Dead drop '{dropId}' not found or already expired.";
                return false;
            }

            if (drop.isIntercepted)
            {
                report = "Dead drop already collected.";
                return false;
            }

            drop.isIntercepted = true;
            intelPointsAwarded = drop.intelYield;
            _state.totalIntelPoints += intelPointsAwarded;
            report = $"Dead drop successfully recovered at {drop.targetLocationId}. Secured {intelPointsAwarded} faction intelligence points.";
            _state.activeDeadDrops.Remove(drop);
            return true;
        }

        public ShelterEspionageState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<ShelterEspionageState>(json) ?? new ShelterEspionageState();
        }

        public void RestoreState(ShelterEspionageState? state)
        {
            if (state == null)
            {
                _state = new ShelterEspionageState();
                return;
            }
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(state);
            _state = s.Deserialize<ShelterEspionageState>(json) ?? new ShelterEspionageState();
        }
    }
}
