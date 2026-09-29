// SPDX-License-Identifier: MIT
// Plan 210 — Survivor Personal Belongings & Effects host seam.
// The SurvivorSocialCoordinator remains the owning aggregate (captured inside
// the existing survivor_social save section). This partial adds the live host
// session, the authored catalog load, and the player-facing claim/gift/favorite
// surface so the Core authority is reachable from gameplay.
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

using Ashfall.Core.Exploration;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private PersonalBelongingsHostSession _personalBelongings = null!;
        private PersonalBelongingsPanel _personalBelongingsPanel = null!;

        public PersonalBelongingsHostSession PersonalBelongings => EnsurePersonalBelongings();

        public PersonalBelongingsHostSession EnsurePersonalBelongings()
        {
            if (_personalBelongings != null) return _personalBelongings;

            SetupSurvivorSocial();
            SetupInventory();
            SetupSurvivors();

            _personalBelongings = new PersonalBelongingsHostSession(_survivorSocial.Belongings);
            _personalBelongings.StateChanged += OnPersonalBelongingsChanged;
            _personalBelongings.SurvivorAlive = id => _survivors?.Needs.Get(id)?.IsAliveState == true;
            _personalBelongings.InventoryCount = id => _inventory?.Inventory?.CountById(id) ?? 0;
            _personalBelongings.ItemDisplayName = id => _inventory?.Catalog?.Get(id)?.displayName ?? id;
            _personalBelongings.CurrentDay = () => _simDay;
            _personalBelongings.SurvivorIdsProvider = () => _survivors?.RosterState
                .Where(s => s != null && s.IsAliveState)
                .Select(s => s.Id)
                .ToList() ?? new List<string>();
            _personalBelongings.SurvivorNameProvider = id => _survivors?.Roster?.FindDefinition(id)?.displayName ?? id;
            _personalBelongings.InventoryItemIdsProvider = () => _inventory?.Catalog == null
                ? new List<string>()
                : _inventory.Catalog.Ids.OrderBy(x => x, StringComparer.Ordinal).ToList();
            return _personalBelongings;
        }

        private void SetupPersonalBelongingsPanel()
        {
            if (_personalBelongingsPanel != null && _personalBelongingsPanel.IsInsideTree())
                return;

            EnsurePersonalBelongings();
            _personalBelongingsPanel = new PersonalBelongingsPanel();
            _personalBelongingsPanel.Bind(_personalBelongings);
            _personalBelongingsPanel.OnClose += () => _personalBelongingsPanel.Visible = false;
            _personalBelongingsPanel.Visible = false;
            AddChild(_personalBelongingsPanel);
        }

        public void ShowPersonalBelongingsPanel()
        {
            SetupPersonalBelongingsPanel();
            ShowPanelLifecycle(_personalBelongingsPanel);
            _personalBelongingsPanel.RefreshView();
        }

        private void ResetPersonalBelongings()
        {
            _personalBelongingsPanel?.Unbind();
            if (_personalBelongingsPanel != null && _personalBelongingsPanel.IsInsideTree())
                RemoveChild(_personalBelongingsPanel);
            _personalBelongingsPanel = null!;
            _personalBelongings = null!;
        }

        /// <summary>Returns one survivor's stable claim metadata.</summary>
        public IReadOnlyList<PersonalBelonging> GetPersonalBelongings(string survivorId)
        {
            SetupSurvivorSocial();
            return _survivorSocial?.Belongings.GetBelongingsForSurvivor(survivorId)
                ?? Array.Empty<PersonalBelonging>();
        }

        /// <summary>
        /// Claims an existing item definition as a survivor keepsake. Inventory
        /// remains the physical stack authority; this records only the stable
        /// survivor-to-item association and sentimental metadata.
        /// </summary>
        public bool ClaimPersonalBelonging(
            string survivorId,
            string itemId,
            BelongingCategory category = BelongingCategory.Keepsake,
            float sentimentalValue = 50f,
            string description = "")
        {
            return EnsurePersonalBelongings().Claim(survivorId, itemId, category, sentimentalValue, description);
        }

        /// <summary>Transfers claim metadata; physical inventory remains shared.</summary>
        public bool GiftPersonalBelonging(
            string fromSurvivorId,
            string toSurvivorId,
            string belongingId,
            string reason = "Gift")
        {
            return EnsurePersonalBelongings().Gift(fromSurvivorId, toSurvivorId, belongingId, reason);
        }

        public bool SetPersonalBelongingFavorite(
            string survivorId,
            string belongingId,
            bool isFavorite = true)
        {
            return EnsurePersonalBelongings().SetFavorite(survivorId, belongingId, isFavorite);
        }

        public bool ReportPersonalBelongingLoss(
            string survivorId,
            string belongingId,
            bool stolen = false)
        {
            return EnsurePersonalBelongings().ReportLoss(survivorId, belongingId, stolen);
        }

        /// <summary>
        /// Called exactly once by SurvivorFateSystem for each death. The
        /// deterministic roster order supplies a primary heir when one exists;
        /// no inventory item is duplicated or removed.
        /// </summary>
        private void HandlePersonalBelongingsInheritance(SurvivorFateEvent fate)
        {
            if (fate == null || string.IsNullOrEmpty(fate.survivorId)) return;
            SetupSurvivorSocial();
            SetupSurvivors();
            string heir = _survivors?.RosterState
                .Where(s => s != null && s.IsAliveState && !string.Equals(s.Id, fate.survivorId, StringComparison.Ordinal))
                .Select(s => s.Id)
                .FirstOrDefault() ?? string.Empty;
            if (string.IsNullOrEmpty(heir)) return;

            _survivorSocial!.Belongings.DistributeInheritanceOnDeath(
                fate.survivorId,
                heir,
                fate.day > 0 ? fate.day : _simDay);
        }

    }
}
