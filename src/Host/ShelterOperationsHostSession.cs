// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Events;
using Ashfall.Core.Inventory;
using Ashfall.Core.Settlements;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Read/command adapter over the existing shelter, outpost, holiday,
    /// roster, and inventory authorities. This session owns no gameplay state
    /// and introduces no save section.
    /// </summary>
    public sealed class ShelterOperationsHostSession : HostSessionBase
    {
        private readonly IPlayerInventoryPort? _inventory;
        private readonly SurvivorsHostSession? _survivors;
        private readonly Func<int> _currentDay;
        private readonly Func<string, bool>? _garrisonFitnessCheck;

        public ShelterExpansionHostSession Construction { get; }
        public OutpostSettlementHostSession Outposts { get; }
        public SeasonalCelebrationHostSession Celebrations { get; }

        public ShelterOperationsHostSession(
            ShelterExpansionHostSession construction,
            OutpostSettlementHostSession outposts,
            SeasonalCelebrationHostSession celebrations,
            IPlayerInventoryPort? inventory,
            SurvivorsHostSession? survivors,
            Func<int> currentDay,
            Func<string, bool>? garrisonFitnessCheck = null)
        {
            Construction = construction ?? throw new ArgumentNullException(nameof(construction));
            Outposts = outposts ?? throw new ArgumentNullException(nameof(outposts));
            Celebrations = celebrations ?? throw new ArgumentNullException(nameof(celebrations));
            _inventory = inventory;
            _survivors = survivors;
            _currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
            _garrisonFitnessCheck = garrisonFitnessCheck;

            Construction.StateChanged += ForwardStateChanged;
            Outposts.StateChanged += ForwardStateChanged;
            Celebrations.StateChanged += ForwardStateChanged;
        }

        public int CurrentDay => Math.Max(1, _currentDay());
        public IReadOnlyList<BlueprintDef> Blueprints
            => new List<BlueprintDef>(Construction.System.Blueprints.Values);
        public IReadOnlyList<UpgradeDef> Upgrades
            => new List<UpgradeDef>(Construction.System.Upgrades.Values);
        public IReadOnlyList<ConstructionProjectDto> Projects => Construction.System.GetAllProjects();
        public IReadOnlyList<ExpansionRoomDto> Rooms => Construction.System.GetAllRooms();
        public IReadOnlyList<OutpostDef> OutpostDefinitions => Outposts.System.GetAllDefinitions();
        public IReadOnlyList<OutpostInstance> OutpostInstances => Outposts.System.GetAllInstances();
        public IReadOnlyList<HolidayDef> Holidays => new List<HolidayDef>(Celebrations.System.Holidays.Values);

        public IReadOnlyList<string> CrewCandidates
        {
            get
            {
                var ids = new List<string>();
                if (_survivors == null) return ids;
                foreach (var survivor in _survivors.RosterState)
                    if (survivor != null && survivor.IsAlive && survivor.Health > 0f
                        && !string.IsNullOrWhiteSpace(survivor.Id))
                        ids.Add(survivor.Id);
                ids.Sort(StringComparer.Ordinal);
                return ids;
            }
        }

        public ConstructionStartResult StartRoom(string blueprintId, int x, int y, int depth)
            => Construction.TryStartRoomConstruction(blueprintId, x, y, depth, CurrentDay, _inventory);

        public ConstructionStartResult StartRenovation(string roomId)
            => Construction.TryStartRenovation(roomId, CurrentDay, _inventory);

        public ConstructionStartResult StartUpgrade(string roomId, string upgradeId)
            => Construction.TryStartUpgrade(roomId, upgradeId, CurrentDay, _inventory);

        public ConstructionStartResult StartDepthExcavation()
            => Construction.TryStartDepthExcavation(CurrentDay, _inventory);

        public bool AssignCrew(string projectId, string survivorId)
            => Construction.AssignCrew(projectId, survivorId);

        public bool RelieveCrew(string projectId, string survivorId)
            => Construction.RelieveCrew(projectId, survivorId);

        public bool EstablishOutpost(string outpostId)
            => Outposts.TryEstablish(outpostId, _inventory);

        public bool AssignGarrison(string outpostId, string survivorId)
            => Outposts.AssignGarrison(outpostId, survivorId, _garrisonFitnessCheck);

        public bool RelieveGarrison(string outpostId, string survivorId)
            => Outposts.RelieveGarrison(outpostId, survivorId);

        public bool SupplyOutpost(string outpostId, string rationItemId, int rations)
            => Outposts.TrySupply(outpostId, rationItemId, rations, _inventory);

        public bool AbandonOutpost(string outpostId)
            => Outposts.Abandon(outpostId);

        public HolidayDef? HolidayForCurrentDay()
            => Celebrations.CheckHoliday(CurrentDay);

        public bool HoldHoliday(string holidayId, string scaleId, int participants,
            string foodItemId, string fuelItemId, ISeededRng? rng, out CelebrationRecord? record)
            => Celebrations.TryHoldCelebration(holidayId, scaleId, participants, CurrentDay,
                foodItemId, fuelItemId, _inventory, rng, out record);

        public bool SkipHoliday(string holidayId, out float moralePenalty)
            => Celebrations.TrySkipHoliday(holidayId, CurrentDay, out moralePenalty);

        public override void Dispose()
        {
            Construction.StateChanged -= ForwardStateChanged;
            Outposts.StateChanged -= ForwardStateChanged;
            Celebrations.StateChanged -= ForwardStateChanged;
            base.Dispose();
        }

        private void ForwardStateChanged() => RaiseStateChanged();
    }
}
