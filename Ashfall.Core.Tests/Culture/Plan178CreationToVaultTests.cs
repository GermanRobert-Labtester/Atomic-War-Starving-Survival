// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Culture;
using Xunit;

namespace Ashfall.Core.Tests.Plan178Culture
{
    /// <summary>
    /// DEBT-178-CREATION-TO-VAULT — the creation command routes a real campaign
    /// milestone (memorial death) into the culture vault's existing chronicle
    /// append, and the stable milestone keys cannot collide across survivors.
    /// The vault owns append + dedup; no new archive save or art ledger exists.
    /// </summary>
    public sealed class Plan178CreationToVaultTests
    {
        private static CulturalArchiveVaultSystem CreateVault()
            => new CulturalArchiveVaultSystem(new Ashfall.Core.Inventory.Inventory());

        [Fact]
        public void Memorial_keys_are_stable_and_survivor_scoped()
        {
            Assert.Equal("chronicle_memorial_sv_alpha", ArchiveChronicleMilestones.MemorialKey("sv_alpha"));
            Assert.NotEqual(
                ArchiveChronicleMilestones.MemorialKey("sv_alpha"),
                ArchiveChronicleMilestones.MemorialKey("sv_bravo"));
            Assert.Equal("memorial", ArchiveChronicleMilestones.Memorial);
            Assert.Equal("expedition_return", ArchiveChronicleMilestones.ExpeditionReturn);
        }

        [Fact]
        public void Recording_a_memorial_appends_one_chronicle_entry()
        {
            var vault = CreateVault();
            ArchiveChronicleEntry? seen = null;
            vault.OnChronicleEntryAdded += e => seen = e;

            var result = vault.TryRecordChronicleEntry(
                ArchiveChronicleMilestones.Memorial,
                campaignDay: 33,
                ArchiveChronicleMilestones.MemorialKey("sv_alpha"),
                new[] { "sv_alpha" });

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.NotNull(seen);
            Assert.Equal(ArchiveChronicleMilestones.Memorial, seen!.event_type);
            Assert.Equal("chronicle_memorial_sv_alpha", seen.summary_key);
            Assert.Equal(33, seen.campaign_day);
            Assert.Single(vault.Chronicle);
        }

        [Fact]
        public void Two_survivors_memorialized_on_the_same_day_both_enter()
        {
            // Distinct survivor-scoped keys must not collide in the vault's
            // (event_type, day, summary_key) dedup.
            var vault = CreateVault();
            vault.TryRecordChronicleEntry(ArchiveChronicleMilestones.Memorial, 40,
                ArchiveChronicleMilestones.MemorialKey("sv_alpha"), new[] { "sv_alpha" });
            vault.TryRecordChronicleEntry(ArchiveChronicleMilestones.Memorial, 40,
                ArchiveChronicleMilestones.MemorialKey("sv_bravo"), new[] { "sv_bravo" });

            Assert.Equal(2, vault.Chronicle.Count);
        }

        [Fact]
        public void Same_survivor_is_not_chronicled_twice()
        {
            var vault = CreateVault();
            string key = ArchiveChronicleMilestones.MemorialKey("sv_alpha");
            Assert.Equal(ActionResult.StatusKind.Success,
                vault.TryRecordChronicleEntry(ArchiveChronicleMilestones.Memorial, 50, key, new[] { "sv_alpha" }).Status);
            Assert.Equal(ActionResult.StatusKind.Blocked,
                vault.TryRecordChronicleEntry(ArchiveChronicleMilestones.Memorial, 50, key, new[] { "sv_alpha" }).Status);
            Assert.Single(vault.Chronicle);
        }

        [Fact]
        public void Chronicle_entries_survive_a_save_round_trip()
        {
            var vault = CreateVault();
            vault.TryRecordChronicleEntry(ArchiveChronicleMilestones.Memorial, 12,
                ArchiveChronicleMilestones.MemorialKey("sv_alpha"), new[] { "sv_alpha" });
            var saved = vault.CaptureState();

            var restored = CreateVault();
            restored.RestoreState(saved);

            Assert.Single(restored.Chronicle);
            Assert.Equal("chronicle_memorial_sv_alpha", restored.Chronicle[0].summary_key);
        }
    }
}
