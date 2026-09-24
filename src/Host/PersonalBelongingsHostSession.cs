// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : PersonalBelongingsHostSession
// Core System  : Ashfall.Core.Survivors.PersonalBelongingsSystem
// Host Caller  : Main.Plans163_210 / Main.PersonalBelongings
// Purpose      : Plan 210 — survivor keepsake claims, sentimental bonding,
//                favorites, reciprocal gifts, loss reporting, and inheritance.
//                Physical item stacks and storage capacity remain owned by the
//                canonical Inventory; this session owns claim metadata only.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public sealed class PersonalBelongingsHostSession
    {
        private readonly PersonalBelongingsSystem _system;

        public PersonalBelongingsSystem System => _system;

        public event Action? StateChanged;

        /// <summary>True when the survivor is a living roster member.</summary>
        public Func<string, bool>? SurvivorAlive { get; set; }

        /// <summary>Physical stack count of an item definition in the shared inventory.</summary>
        public Func<string, int>? InventoryCount { get; set; }

        /// <summary>Display name for an inventory item definition.</summary>
        public Func<string, string>? ItemDisplayName { get; set; }

        /// <summary>Current campaign day.</summary>
        public Func<int>? CurrentDay { get; set; }

        /// <summary>Living roster ids, in stable roster order, for UI selection.</summary>
        public Func<IReadOnlyList<string>>? SurvivorIdsProvider { get; set; }

        /// <summary>Display name for a survivor id.</summary>
        public Func<string, string>? SurvivorNameProvider { get; set; }

        /// <summary>Inventory item-definition ids available to claim, ordinal-sorted.</summary>
        public Func<IReadOnlyList<string>>? InventoryItemIdsProvider { get; set; }

        public PersonalBelongingsHostSession(PersonalBelongingsSystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));
            _system.OnBelongingAcquired += _ => StateChanged?.Invoke();
            _system.OnBelongingGifted += _ => StateChanged?.Invoke();
            _system.OnBelongingInherited += (_, _) => StateChanged?.Invoke();
            _system.OnBelongingLost += _ => StateChanged?.Invoke();
            _system.OnFavoriteToggled += (_, _) => StateChanged?.Invoke();
        }

        public IReadOnlyList<PersonalBelonging> GetBelongingsFor(string survivorId)
        {
            return _system.GetBelongingsForSurvivor(survivorId);
        }

        public IReadOnlyCollection<PersonalBelongingTemplate> Templates => _system.Templates;

        /// <summary>
        /// Claims an existing shared-inventory item definition as a survivor's
        /// keepsake. Inventory remains the physical authority; a claim is
        /// rejected when the item is missing or already claimed.
        /// </summary>
        public bool Claim(
            string survivorId,
            string itemId,
            BelongingCategory category = BelongingCategory.Keepsake,
            float sentimentalValue = 50f,
            string description = "")
        {
            if (string.IsNullOrWhiteSpace(survivorId) || string.IsNullOrWhiteSpace(itemId)) return false;
            if (SurvivorAlive != null && !SurvivorAlive(survivorId)) return false;
            if (InventoryCount != null && InventoryCount(itemId) <= 0) return false;
            if (_system.HasClaimForItem(itemId)) return false;

            string name = ItemDisplayName?.Invoke(itemId) ?? itemId;
            int day = CurrentDay?.Invoke() ?? 1;
            var belonging = _system.RegisterBelonging(
                survivorId,
                itemId,
                name,
                category,
                sentimentalValue,
                condition: 100f,
                acquiredDay: day,
                acquiredFrom: "holdfast_inventory",
                description: description);
            return belonging != null;
        }

        /// <summary>
        /// Grants an authored keepsake template directly to a survivor. Used for
        /// narrative/expedition rewards where the item is a personal possession,
        /// not a shared inventory stack.
        /// </summary>
        public bool ClaimTemplate(string survivorId, string templateId, string acquiredFrom = "story")
        {
            if (string.IsNullOrWhiteSpace(survivorId) || string.IsNullOrWhiteSpace(templateId)) return false;
            if (SurvivorAlive != null && !SurvivorAlive(survivorId)) return false;
            int day = CurrentDay?.Invoke() ?? 1;
            return _system.RegisterFromTemplate(survivorId, templateId, day, acquiredFrom) != null;
        }

        public bool Gift(string fromSurvivorId, string toSurvivorId, string belongingId, string reason = "Gift")
        {
            if (SurvivorAlive != null && (!SurvivorAlive(fromSurvivorId) || !SurvivorAlive(toSurvivorId)))
                return false;
            int day = CurrentDay?.Invoke() ?? 1;
            return _system.GiftBelonging(fromSurvivorId, toSurvivorId, belongingId, day, reason) != null;
        }

        public bool SetFavorite(string survivorId, string belongingId, bool isFavorite = true)
        {
            if (SurvivorAlive != null && !SurvivorAlive(survivorId)) return false;
            return _system.SetFavorite(survivorId, belongingId, isFavorite);
        }

        public bool ReportLoss(string survivorId, string belongingId, bool stolen = false)
        {
            if (SurvivorAlive != null && !SurvivorAlive(survivorId)) return false;
            int day = CurrentDay?.Invoke() ?? 1;
            return _system.ReportTheftOrLoss(survivorId, belongingId, day, stolen);
        }

        public PersonalBelongingsState CaptureState() => _system.CaptureState();

        public void RestoreState(PersonalBelongingsState state)
        {
            _system.RestoreState(state);
            StateChanged?.Invoke();
        }
    }
}
