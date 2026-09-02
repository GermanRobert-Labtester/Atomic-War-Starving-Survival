using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Economy
{
    [Serializable]
    public sealed class BountyContractTemplateDef
    {
        public string template_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string contract_type { get; set; } = "Assassination";
        public string issuer_faction_id { get; set; } = string.Empty;
        public List<string> target_tags { get; set; } = new List<string>();
        public float min_payout { get; set; } = 100.0f;
        public float max_payout { get; set; } = 200.0f;
        public float posting_cost { get; set; } = 30.0f;
        public int duration_days { get; set; } = 8;
        public float standing_penalty { get; set; } = 15.0f;
        public string required_proof_item_id { get; set; } = "item_dog_tags_scavenged";
        public float rival_chance { get; set; } = 0.3f;
        public float betrayal_base_chance { get; set; } = 0.1f;
    }

    [Serializable]
    public sealed class BountyCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<BountyContractTemplateDef> templates { get; set; } = new List<BountyContractTemplateDef>();
    }

    public enum BountyContractStatus
    {
        Open,
        Accepted,
        Completed,
        Failed,
        Claimed
    }

    [Serializable]
    public sealed class BountyContract
    {
        public string contractId { get; set; } = string.Empty;
        public string templateId { get; set; } = string.Empty;
        public string issuerFactionId { get; set; } = string.Empty;
        public string targetId { get; set; } = string.Empty;
        public string targetFactionId { get; set; } = string.Empty;
        public string contractType { get; set; } = "Assassination";
        public float rewardAmount { get; set; } = 100.0f;
        public float postingCost { get; set; } = 30.0f;
        public int issuedDay { get; set; }
        public int expiryDay { get; set; }
        public BountyContractStatus status { get; set; } = BountyContractStatus.Open;
        public bool acceptedByPlayer { get; set; }
        public string rivalHunterId { get; set; } = string.Empty;
        public float rivalProgress { get; set; }
        public bool betrayed { get; set; }
        public string requiredProofItemId { get; set; } = "item_dog_tags_scavenged";
        public bool rewardClaimed { get; set; }
    }

    [Serializable]
    public sealed class TargetIntel
    {
        public string targetId { get; set; } = string.Empty;
        public string lastKnownZone { get; set; } = string.Empty;
        public float confidence { get; set; } = 0.5f; // 0..1
        public int lastUpdatedDay { get; set; }
    }

    [Serializable]
    public sealed class MercenaryState
    {
        public string systemId = MercenarySystem.SystemId;
        public List<BountyContract> contracts = new List<BountyContract>();
        public List<TargetIntel> intel = new List<TargetIntel>();
        public List<string> claimedContractIds = new List<string>();
        public int lastGenerationDay = -1;
    }

    public sealed class MercenarySystem
    {
        public const string SystemId = "mercenary_bounties";

        private MercenaryState _state = new MercenaryState();
        private readonly Dictionary<string, BountyContractTemplateDef> _templates = new Dictionary<string, BountyContractTemplateDef>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly ILog _log;
        private int _contractCounter;

        public MercenaryState State => _state;
        public IReadOnlyList<BountyContract> ActiveContracts => _state.contracts;

        public event Action<BountyContract>? OnBountyPosted;
        public event Action<BountyContract>? OnContractAccepted;
        public event Action<TargetIntel>? OnTargetIntelUpdated;
        public event Action<BountyContract>? OnBountyClaimed;
        public event Action<BountyContract>? OnMercenaryBetrayed;
        public event Action<BountyContract>? OnRivalClaimedTarget;

        public MercenarySystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            ILog? log = null,
            string dataPath = "")
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _log = log ?? NullLog.Instance;

            LoadCatalog(dataPath);
        }

        public void LoadCatalog(string dataPath)
        {
            string path = string.IsNullOrEmpty(dataPath)
                ? Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "bounty_board.json")
                : Path.Combine(dataPath, "bounty_board.json");

            if (!File.Exists(path))
            {
                RegisterTemplate(new BountyContractTemplateDef
                {
                    template_id = "bounty_template_raider_warlord",
                    display_name = "Eliminate Raider Warlord",
                    issuer_faction_id = "faction_holdfast_schedule",
                    min_payout = 120.0f,
                    max_payout = 250.0f,
                    posting_cost = 40.0f,
                    duration_days = 10,
                    required_proof_item_id = "item_warlord_trophy"
                });
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var container = JsonSerializer.Deserialize<BountyCatalogContainer>(json);
                if (container?.templates != null)
                {
                    foreach (var t in container.templates)
                        RegisterTemplate(t);
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[MercenarySystem] Failed to load catalog from {path}: {ex.Message}");
            }
        }

        public void RegisterTemplate(BountyContractTemplateDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.template_id)) return;
            _templates[def.template_id] = def;
        }

        public void GenerateBoard(int currentDay, IReadOnlyList<string> candidateTargetIds)
        {
            if (_state.lastGenerationDay == currentDay) return;
            _state.lastGenerationDay = currentDay;

            // Remove expired open contracts
            _state.contracts.RemoveAll(c => c.status == BountyContractStatus.Open && c.expiryDay <= currentDay);

            foreach (var kvp in _templates)
            {
                var tmpl = kvp.Value;
                if (candidateTargetIds == null || candidateTargetIds.Count == 0) continue;

                string target = candidateTargetIds[_rng.Next(0, candidateTargetIds.Count)];
                float payout = tmpl.min_payout + (float)_rng.NextDouble() * (tmpl.max_payout - tmpl.min_payout);

                var contract = new BountyContract
                {
                    contractId = $"bounty_{++_contractCounter}_{currentDay}",
                    templateId = tmpl.template_id,
                    issuerFactionId = tmpl.issuer_faction_id,
                    targetId = target,
                    targetFactionId = "faction_hostile_raiders",
                    contractType = tmpl.contract_type,
                    rewardAmount = (float)Math.Round(payout),
                    postingCost = tmpl.posting_cost,
                    issuedDay = currentDay,
                    expiryDay = currentDay + tmpl.duration_days,
                    status = BountyContractStatus.Open,
                    requiredProofItemId = tmpl.required_proof_item_id
                };

                _state.contracts.Add(contract);
            }
        }

        public ActionResult PostBounty(string templateId, string targetId, string targetFactionId, int currentDay)
        {
            if (!_templates.TryGetValue(templateId, out var tmpl))
                return ActionResult.Blocked("unknown_template", "mercenary.unknown_template");

            // Verify player can pay posting cost in scrap
            if (_inventory.CountById("scrap_metal") < (int)tmpl.posting_cost)
                return ActionResult.Blocked("insufficient_posting_funds", "mercenary.insufficient_posting_funds");

            _inventory.RemoveById("scrap_metal", (int)tmpl.posting_cost);

            var contract = new BountyContract
            {
                contractId = $"player_bounty_{++_contractCounter}_{currentDay}",
                templateId = templateId,
                issuerFactionId = "shelter_holdfast",
                targetId = targetId,
                targetFactionId = targetFactionId,
                contractType = tmpl.contract_type,
                rewardAmount = tmpl.max_payout,
                postingCost = tmpl.posting_cost,
                issuedDay = currentDay,
                expiryDay = currentDay + tmpl.duration_days,
                status = BountyContractStatus.Accepted,
                acceptedByPlayer = false,
                requiredProofItemId = tmpl.required_proof_item_id
            };

            // Assign NPC mercenary squad
            if (_rng.NextDouble() < tmpl.rival_chance)
            {
                contract.rivalHunterId = $"mercenary_squad_{_rng.Next(0, 100)}";
            }

            _state.contracts.Add(contract);
            OnBountyPosted?.Invoke(contract);
            return ActionResult.Success("mercenary.bounty_posted");
        }

        public ActionResult AcceptContract(string contractId, int currentDay)
        {
            var contract = _state.contracts.Find(c => c.contractId == contractId);
            if (contract == null) return ActionResult.Blocked("contract_not_found", "mercenary.contract_not_found");
            if (contract.status != BountyContractStatus.Open) return ActionResult.Blocked("not_open", "mercenary.not_open");
            if (contract.expiryDay <= currentDay) return ActionResult.Blocked("expired", "mercenary.expired");

            contract.status = BountyContractStatus.Accepted;
            contract.acceptedByPlayer = true;

            // Roll rival hunter competition
            _templates.TryGetValue(contract.templateId, out var tmpl);
            if (_rng.NextDouble() < (tmpl?.rival_chance ?? 0.3f))
            {
                contract.rivalHunterId = $"rival_hunter_{_rng.Next(0, 50)}";
                contract.rivalProgress = 0.1f;
            }

            // Reveal Target Intel
            var intel = new TargetIntel
            {
                targetId = contract.targetId,
                lastKnownZone = "loc_river_delta",
                confidence = 0.75f,
                lastUpdatedDay = currentDay
            };
            _state.intel.Add(intel);
            OnTargetIntelUpdated?.Invoke(intel);

            OnContractAccepted?.Invoke(contract);
            return ActionResult.Success("mercenary.contract_accepted");
        }

        public void TickDay(int currentDay)
        {
            for (int i = 0; i < _state.contracts.Count; i++)
            {
                var c = _state.contracts[i];
                if (c.status != BountyContractStatus.Accepted) continue;

                if (c.expiryDay <= currentDay)
                {
                    c.status = BountyContractStatus.Failed;
                    continue;
                }

                // Rival competition progress
                if (!string.IsNullOrEmpty(c.rivalHunterId) && c.acceptedByPlayer)
                {
                    c.rivalProgress += 0.2f + (float)_rng.NextDouble() * 0.15f;
                    if (c.rivalProgress >= 1.0f)
                    {
                        c.status = BountyContractStatus.Failed;
                        OnRivalClaimedTarget?.Invoke(c);
                        continue;
                    }
                }

                // Betrayal evaluation for player-posted bounties
                if (!c.acceptedByPlayer && !c.betrayed)
                {
                    _templates.TryGetValue(c.templateId, out var tmpl);
                    if (_rng.NextDouble() < (tmpl?.betrayal_base_chance ?? 0.1f))
                    {
                        c.betrayed = true;
                        c.status = BountyContractStatus.Failed;
                        OnMercenaryBetrayed?.Invoke(c);
                    }
                }
            }
        }

        public ActionResult ClaimReward(string contractId)
        {
            var contract = _state.contracts.Find(c => c.contractId == contractId);
            if (contract == null) return ActionResult.Blocked("contract_not_found", "mercenary.contract_not_found");
            if (contract.rewardClaimed || _state.claimedContractIds.Contains(contractId))
                return ActionResult.Blocked("already_claimed", "mercenary.already_claimed");
            if (contract.status != BountyContractStatus.Accepted && contract.status != BountyContractStatus.Completed)
                return ActionResult.Blocked("not_completable", "mercenary.not_completable");

            // Verify and consume proof item
            if (!string.IsNullOrEmpty(contract.requiredProofItemId))
            {
                if (_inventory.CountById(contract.requiredProofItemId) <= 0)
                    return ActionResult.Blocked("missing_proof", "mercenary.missing_proof");

                _inventory.RemoveById(contract.requiredProofItemId, 1);
            }

            // Award payout in scrap
            _inventory.AddById("scrap_metal", (int)contract.rewardAmount);

            contract.status = BountyContractStatus.Claimed;
            contract.rewardClaimed = true;
            _state.claimedContractIds.Add(contractId);

            OnBountyClaimed?.Invoke(contract);
            return ActionResult.Success("mercenary.bounty_claimed");
        }

        public void RestoreState(MercenaryState state)
        {
            if (state == null) return;
            _state = state;
            _contractCounter = _state.contracts.Count;
        }
    }
}
