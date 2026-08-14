using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MaterialShieldingSystemTests
    {
        [Fact]
        public void NoCeilings_AttenuationZero_BleedIsFull()
        {
            var sys = new MaterialShieldingSystem();
            Assert.Equal(0f, sys.GetWeakestCeilingAttenuation());
            Assert.Equal(100f, sys.GetRadiationBleed(100f), 3);
        }

        [Fact]
        public void UpgradeCeiling_RaisesAttenuation()
        {
            var sys = new MaterialShieldingSystem();
            sys.UpgradeCeiling("room_a", MaterialShieldingSystem.WallMaterial.Concrete);
            Assert.Equal(0.8f, sys.GetCeilingAttenuation("room_a"), 3);
            Assert.Equal(0.8f, sys.GetWeakestCeilingAttenuation(), 3);
            // 100 ambient, 80% blocked → 20 bleeds in.
            Assert.Equal(20f, sys.GetRadiationBleed(100f), 3);
        }

        [Fact]
        public void WeakestCeiling_GovernsBleed()
        {
            var sys = new MaterialShieldingSystem();
            sys.UpgradeCeiling("room_a", MaterialShieldingSystem.WallMaterial.Lead);   // 0.99
            sys.UpgradeCeiling("room_b", MaterialShieldingSystem.WallMaterial.Wood);   // 0.1
            // The wood roof is the weak point: 100 → 90 bleeds in.
            Assert.Equal(90f, sys.GetRadiationBleed(100f), 3);
        }

        [Fact]
        public void Upgrade_ReportsEvent()
        {
            var sys = new MaterialShieldingSystem();
            string room = null;
            MaterialShieldingSystem.WallMaterial mat = MaterialShieldingSystem.WallMaterial.None;
            sys.OnCeilingUpgraded += (r, m) => { room = r; mat = m; };
            sys.UpgradeCeiling("room_a", MaterialShieldingSystem.WallMaterial.Dirt);
            Assert.Equal("room_a", room);
            Assert.Equal(MaterialShieldingSystem.WallMaterial.Dirt, mat);
        }

        [Fact]
        public void SaveRoundtrip_PreservesMaterials()
        {
            var sys = new MaterialShieldingSystem();
            sys.UpgradeCeiling("room_a", MaterialShieldingSystem.WallMaterial.Concrete);
            sys.UpgradeCeiling("room_b", MaterialShieldingSystem.WallMaterial.Lead);

            var state = sys.CaptureState();
            var restored = new MaterialShieldingSystem();
            restored.RestoreState(state);

            Assert.Equal(MaterialShieldingSystem.WallMaterial.Concrete, restored.GetCeilingMaterial("room_a"));
            Assert.Equal(MaterialShieldingSystem.WallMaterial.Lead, restored.GetCeilingMaterial("room_b"));
            Assert.Equal(0.8f, restored.GetWeakestCeilingAttenuation(), 3);
        }

        [Fact]
        public void Restore_OutOfRangeMaterial_Clamped()
        {
            var sys = new MaterialShieldingSystem();
            sys.RestoreState(new MaterialShieldingSave
            {
                RoomIds = new[] { "room_a" },
                Materials = new[] { 99 }
            });
            Assert.Equal(MaterialShieldingSystem.WallMaterial.Lead, sys.GetCeilingMaterial("room_a"));
        }
    }
}
