using System.Collections.Generic;
using AtomicWar._Game.UI.MainMenu;
using AtomicWar._Game.Utilities;
using NUnit.Framework;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// MainMenuModel is pure data, so these tests guard the things a careless
    /// edit actually breaks: the row order the controller indexes into, the
    /// Continue detail line the player reads to decide what they are resuming,
    /// and empty dialog copy that would ship a blank panel.
    /// </summary>
    [TestFixture]
    public class MainMenuModelTests
    {
        [Test]
        public void Entries_AreInTheOrderTheControllerAssumes()
        {
            var ids = new List<MainMenuModel.EntryId>();
            foreach (MainMenuModel.Entry entry in MainMenuModel.Entries) ids.Add(entry.Id);

            Assert.That(ids, Is.EqualTo(new[]
            {
                MainMenuModel.EntryId.Continue,
                MainMenuModel.EntryId.NewExpedition,
                MainMenuModel.EntryId.Settings,
                MainMenuModel.EntryId.Credits,
                MainMenuModel.EntryId.Exit,
            }));
        }

        [Test]
        public void Entries_HaveNoDuplicateIds()
        {
            var seen = new HashSet<MainMenuModel.EntryId>();
            foreach (MainMenuModel.Entry entry in MainMenuModel.Entries)
            {
                Assert.That(seen.Add(entry.Id), Is.True, $"Duplicate entry id: {entry.Id}");
            }
        }

        [Test]
        public void Entries_AllHaveLabelAndDetail()
        {
            foreach (MainMenuModel.Entry entry in MainMenuModel.Entries)
            {
                Assert.That(entry.Label, Is.Not.Null.And.Not.Empty, $"{entry.Id} label");
                Assert.That(entry.Detail, Is.Not.Null.And.Not.Empty, $"{entry.Id} detail");
            }
        }

        [Test]
        public void ContinueDetail_NoSlot_ReadsAsNoActiveLog()
        {
            Assert.That(MainMenuModel.ContinueDetail(null),
                Is.EqualTo(MainMenuModel.ContinueDetailNoSave));
            Assert.That(MainMenuModel.ContinueDetail(string.Empty),
                Is.EqualTo(MainMenuModel.ContinueDetailNoSave));
        }

        [Test]
        public void ContinueDetail_WithSlot_NamesTheSlotInUpperCase()
        {
            Assert.That(MainMenuModel.ContinueDetail("autosave"), Does.Contain("AUTOSAVE"));
            Assert.That(MainMenuModel.ContinueDetail("quicksave"), Does.Contain("QUICKSAVE"));
        }

        [Test]
        public void IndexLabel_IsTwoDigitsFromOne()
        {
            Assert.That(MainMenuModel.IndexLabel(0), Is.EqualTo("01"));
            Assert.That(MainMenuModel.IndexLabel(4), Is.EqualTo("05"));
        }

        [Test]
        public void DialogCopy_IsCompleteForEveryDialog()
        {
            var dialogs = new Dictionary<string, MainMenuModel.DialogCopy>
            {
                { "new expedition", MainMenuModel.NewExpeditionDialog },
                { "settings", MainMenuModel.SettingsDialog },
                { "credits", MainMenuModel.CreditsDialog },
                { "quit", MainMenuModel.QuitDialog },
            };

            foreach (KeyValuePair<string, MainMenuModel.DialogCopy> pair in dialogs)
            {
                MainMenuModel.DialogCopy copy = pair.Value;
                Assert.That(copy.Eyebrow, Is.Not.Null.And.Not.Empty, $"{pair.Key} eyebrow");
                Assert.That(copy.Title, Is.Not.Null.And.Not.Empty, $"{pair.Key} title");
                Assert.That(copy.Body, Is.Not.Null.And.Not.Empty, $"{pair.Key} body");
                Assert.That(copy.Confirm, Is.Not.Null.And.Not.Empty, $"{pair.Key} confirm");
                Assert.That(copy.Back, Is.Not.Null.And.Not.Empty, $"{pair.Key} back");
            }
        }

        /// <summary>
        /// The prototype told the player their progress would be retained. In
        /// this codebase a new game's first autosave overwrites the same slot,
        /// so the copy must warn instead. This is the one string whose wording
        /// is a correctness matter rather than a taste matter.
        /// </summary>
        [Test]
        public void NewExpeditionDialog_WarnsThatProgressIsOverwritten()
        {
            Assert.That(MainMenuModel.NewExpeditionDialog.Body, Does.Contain("overwrites"));
            Assert.That(MainMenuModel.NewExpeditionDialog.Body, Does.Not.Contain("retained"));
        }

        [Test]
        public void CreditsDialog_NamesTheAuthor()
        {
            Assert.That(MainMenuModel.CreditsDialog.Body, Does.Contain(MainMenuModel.AuthorName));
        }

        [Test]
        public void DifficultyLabel_MapsBothValues()
        {
            Assert.That(MainMenuModel.DifficultyLabel(ExpeditionDifficulty.Operative),
                Is.EqualTo(MainMenuModel.DifficultyOperativeLabel));
            Assert.That(MainMenuModel.DifficultyLabel(ExpeditionDifficulty.Veteran),
                Is.EqualTo(MainMenuModel.DifficultyVeteranLabel));
        }
    }
}
