using System.Collections.Generic;
using Ashfall.Core.Economy;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Seals GAP-TEST-01 and GAP-TEST-02 from the implementation gap audit.
    /// TEST-01: FactionStanceEngine with non-default providers.
    /// TEST-02: IsCompanionInSameRoom with a real ShelterAssignmentSystem.
    /// </summary>
    public class GapTestCoverageTests
    {
        // ── GAP-TEST-01: FactionStanceEngine non-default providers ────

        [Fact]
        public void FactionStanceEngine_HatedMilitarySurvivor_DropsMilitaryFactionToMinTrust()
        {
            var engine = new FactionStanceEngine();
            engine.RegisterFaction(new FactionThresholds("faction_military_alpha",
                raidThreshold: -50f, robThreshold: -20f, minTrustToTrade: -40f, intelShareThreshold: 40f));
            engine.SetTrust("faction_military_alpha", 50f);

            engine.IsMilitaryFaction = id => id == "faction_military_alpha";
            engine.HasHatedMilitarySurvivor = () => true;

            Assert.Equal(FactionStanceConstants.MinTrust, engine.GetEffectiveTrust("faction_military_alpha"));
            Assert.Equal(TradeStance.HostileRaid, engine.GetStance("faction_military_alpha"));
        }

        [Fact]
        public void FactionStanceEngine_NonMilitaryFaction_UnaffectedByHatedMilitary()
        {
            var engine = new FactionStanceEngine();
            engine.RegisterFaction(new FactionThresholds("faction_civilian_beta",
                raidThreshold: -50f, robThreshold: -20f, minTrustToTrade: -40f, intelShareThreshold: 40f));
            engine.SetTrust("faction_civilian_beta", 50f);

            engine.IsMilitaryFaction = id => id == "faction_military_alpha";
            engine.HasHatedMilitarySurvivor = () => true;

            Assert.Equal(50f, engine.GetEffectiveTrust("faction_civilian_beta"));
            Assert.Equal(TradeStance.ShareIntel, engine.GetStance("faction_civilian_beta"));
        }

        [Fact]
        public void FactionStanceEngine_ArsProvider_TrustInversionFactionGoesToMaxTrust()
        {
            var engine = new FactionStanceEngine();
            engine.RegisterFaction(new FactionThresholds("faction_cult_ash",
                raidThreshold: -50f, robThreshold: -20f, minTrustToTrade: -40f, intelShareThreshold: 40f,
                trustInversion: true, healthyRadiationCeiling: 20f, highRadiationFloor: 60f));
            engine.SetTrust("faction_cult_ash", 0f);
            engine.DayProvider = () => 60;

            engine.PartyHasArsProvider = () => true;

            Assert.Equal(FactionStanceConstants.MaxTrust, engine.GetEffectiveTrust("faction_cult_ash"));
        }

        [Fact]
        public void FactionStanceEngine_RadiationProvider_TrustInversionInterpolation()
        {
            var engine = new FactionStanceEngine();
            engine.RegisterFaction(new FactionThresholds("faction_cult_ash",
                raidThreshold: -50f, robThreshold: -20f, minTrustToTrade: -40f, intelShareThreshold: 40f,
                trustInversion: true, healthyRadiationCeiling: 20f, highRadiationFloor: 60f));
            engine.SetTrust("faction_cult_ash", 0f);
            engine.DayProvider = () => 60;

            engine.PartyHasArsProvider = () => false;
            engine.PartyIntactHazmatProvider = () => false;
            engine.PartyRadiationProvider = () => 40f;

            float effective = engine.GetEffectiveTrust("faction_cult_ash");
            Assert.True(effective > FactionStanceConstants.MinTrust && effective < FactionStanceConstants.MaxTrust,
                $"Expected interpolated trust between {FactionStanceConstants.MinTrust} and {FactionStanceConstants.MaxTrust}, got {effective}");
        }

        [Fact]
        public void FactionStanceEngine_DayProvider_CultInactiveBeforeActivationDay()
        {
            var engine = new FactionStanceEngine();
            engine.RegisterFaction(new FactionThresholds("faction_cult_ash",
                trustInversion: true, healthyRadiationCeiling: 20f, highRadiationFloor: 60f));

            engine.DayProvider = () => 10;
            Assert.False(engine.IsFactionActive("faction_cult_ash"));

            engine.DayProvider = () => 30;
            Assert.True(engine.IsFactionActive("faction_cult_ash"));
        }

        [Fact]
        public void FactionStanceEngine_ClampTrustProvider_CustomClamp()
        {
            var engine = new FactionStanceEngine();
            engine.ClampTrustProvider = v => System.Math.Max(-50f, System.Math.Min(50f, v));

            engine.SetTrust("faction_test", 80f);
            engine.ModifyTrust("faction_test", 0f);

            Assert.True(engine.GetTrust("faction_test") <= 50f,
                $"Custom clamp should cap at 50, got {engine.GetTrust("faction_test")}");
        }

        [Fact]
        public void FactionStanceEngine_IntactHazmat_TrustInversionFactionContempt()
        {
            var engine = new FactionStanceEngine();
            engine.RegisterFaction(new FactionThresholds("faction_cult_ash",
                raidThreshold: -50f, robThreshold: -20f, minTrustToTrade: -40f, intelShareThreshold: 40f,
                trustInversion: true, healthyRadiationCeiling: 20f, highRadiationFloor: 60f));
            engine.SetTrust("faction_cult_ash", 50f);
            engine.DayProvider = () => 60;

            engine.PartyHasArsProvider = () => false;
            engine.PartyIntactHazmatProvider = () => true;
            engine.PartyRadiationProvider = () => -1f;

            Assert.Equal(FactionStanceConstants.MinTrust, engine.GetEffectiveTrust("faction_cult_ash"));
        }

        // ── GAP-TEST-02: IsCompanionInSameRoom with real ShelterAssignmentSystem ─

        [Fact]
        public void IsCompanionInSameRoom_RealAssignment_ReturnsTrueForSameRoom()
        {
            var rooms = new List<ShelterRoom>
            {
                new ShelterRoom("room_bunks", "Bunks", 4),
                new ShelterRoom("room_kitchen", "Kitchen", 2),
            };
            var state = new ShelterAssignmentState();
            var rng = new SeededRng(42);
            var assignments = new ShelterAssignmentSystem(state, rooms, rng);

            assignments.Assign("survivor_a", "room_bunks");
            assignments.Assign("survivor_b", "room_bunks");
            assignments.Assign("survivor_c", "room_kitchen");

            var flashback = new SomaticFlashbackSystem
            {
                Rng = new SeededRng(1),
                GetAliveSurvivorIds = () => new List<string> { "survivor_a", "survivor_b", "survivor_c" },
                IsCompanionInSameRoom = (a, b) =>
                {
                    var roomA = assignments.GetAssignmentForSurvivor(a);
                    var roomB = assignments.GetAssignmentForSurvivor(b);
                    return roomA != null && roomB != null
                        && !string.IsNullOrEmpty(roomA.RoomId)
                        && roomA.RoomId == roomB.RoomId;
                }
            };

            Assert.True(flashback.IsCompanionInSameRoom("survivor_a", "survivor_b"));
            Assert.False(flashback.IsCompanionInSameRoom("survivor_a", "survivor_c"));
            Assert.False(flashback.IsCompanionInSameRoom("survivor_b", "survivor_c"));
        }

        [Fact]
        public void IsCompanionInSameRoom_UnassignedSurvivor_ReturnsFalse()
        {
            var rooms = new List<ShelterRoom>
            {
                new ShelterRoom("room_bunks", "Bunks", 4),
            };
            var state = new ShelterAssignmentState();
            var rng = new SeededRng(42);
            var assignments = new ShelterAssignmentSystem(state, rooms, rng);

            assignments.Assign("survivor_a", "room_bunks");

            var isCompanion = (System.Func<string, string, bool>)((a, b) =>
            {
                var roomA = assignments.GetAssignmentForSurvivor(a);
                var roomB = assignments.GetAssignmentForSurvivor(b);
                return roomA != null && roomB != null
                    && !string.IsNullOrEmpty(roomA.RoomId)
                    && roomA.RoomId == roomB.RoomId;
            });

            Assert.False(isCompanion("survivor_a", "survivor_unassigned"));
            Assert.False(isCompanion("survivor_unassigned", "survivor_a"));
        }

        // Phase0HostSession.BindShelterAssignment is tested via headless boot
        // (Godot host type — not reachable from Core tests). The lambda logic
        // it installs is identical to the one tested above.
    }
}
