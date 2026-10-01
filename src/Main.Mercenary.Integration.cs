// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Farming;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private MercenarySystem? _mercenary;

        // ── Plan 188: Bounties & Mercenary Contracts ──────────────────────

        public MercenarySystem EnsureMercenary()
        {
            if (_mercenary != null) return _mercenary;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("mercenary") : new SeededRng(188);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _mercenary = new MercenarySystem(rng, inv, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("bounty_board.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<BountyCatalogContainer>(json);
                        if (container?.templates != null)
                        {
                            foreach (var t in container.templates)
                                _mercenary.RegisterTemplate(t);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Mercenary] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = MercenarySaveStore.TryLoad();
            if (saved != null)
            {
                _mercenary.RestoreState(saved);
            }

            _mercenary.OnBountyClaimed += (contract) =>
            {
                _journal?.TryAddRawEntry("bounty_claimed", $"Bounty contract {contract.contractId} fulfilled. Reward: {contract.rewardAmount} scrap.", null!, _simDay);
            };

            _mercenary.OnMercenaryBetrayed += (contract) =>
            {
                _journal?.TryAddRawEntry("mercenary_betrayal", $"Hired mercenaries betrayed contract {contract.contractId} on target {contract.targetId}!", null!, _simDay);
            };

            // Canonical NPC identity catalog supplies the deterministic
            // candidate target pool for board generation.
            _mercenaryCandidateTargets = LoadNpcCandidateTargets();

            return _mercenary;
        }

        private System.Collections.Generic.List<string>? _mercenaryCandidateTargets;

        private System.Collections.Generic.List<string> LoadNpcCandidateTargets()
        {
            var list = new System.Collections.Generic.List<string>();
            try
            {
                string path = System.IO.Path.Combine(_dataDir, "characters.json");
                if (System.IO.File.Exists(path))
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(System.IO.File.ReadAllText(path));
                    if (doc.RootElement.TryGetProperty("characters", out var arr) && arr.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        foreach (var el in arr.EnumerateArray())
                        {
                            if (el.TryGetProperty("id", out var idEl) && idEl.GetString() is { } id
                                && id.StartsWith("npc_", StringComparison.Ordinal))
                                list.Add(id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Main.Mercenary] Failed to load candidate targets: {ex.Message}");
            }
            return list;
        }

        private void SetupMercenary()
        {
            EnsureMercenary();
        }

        private void SaveMercenary()
        {
            if (_mercenary != null)
            {
                CaptureSection("mercenary_bounties", MercenarySaveStore.TryCapturePersisted(_mercenary.CaptureState()));
            }
        }
        private void CloseMercenaryBountyBoardPanel() { ClosePanelAnimated(_mercenaryBountyBoardPanel); }

        // ── Plan 188: mercenary bounty board commands ──────────────────────

        private void HandleMercenaryAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseMercenaryBountyBoardPanel(); return; }
            if (_mercenaryBountyBoardPanel == null || _mercenary == null) return;

            Ashfall.Core.ActionResult? res = action switch
            {
                // Claim requires the authored proof item — the canonical
                // inventory authority holds it; Core verifies and pays once.
                "claim" => (Ashfall.Core.ActionResult?)_mercenary.ClaimReward(param),
                "accept" => (Ashfall.Core.ActionResult?)_mercenary.AcceptContract(param, _simDay),
                _ => null
            };

            if (res != null)
                _mercenaryBountyBoardPanel.ShowFeedback(
                    res.Value.IsSuccess
                        ? (action == "claim"
                            ? "Payout collected at the board. The ledger closes."
                            : "Contract accepted. Proof of the deed is what gets paid — nothing else.")
                        : "The board refused it — check proof, expiry and standing.",
                    !res.Value.IsSuccess);
            _mercenaryBountyBoardPanel.RefreshView();
        }

    }
}
