using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Regression net for the equipSlot dual-path bug.
    ///
    /// The JSON import gate and the runtime world-catalog loader each had their own
    /// equipSlot parser. The runtime one accepted "Torso"; the import gate did not.
    /// Data written as "Torso" loaded fine in play but hard-failed the build's data
    /// validation gate, and any spelling neither parser knew silently became
    /// EquipSlot.None — an item that just quietly refused to be equipped.
    ///
    /// These tests pin the contract: one shared parser, and shipped data uses
    /// canonical enum names only.
    /// </summary>
    [TestFixture]
    public class EquipSlotParsingTests
    {
        [Test]
        public void TryParse_AcceptsEveryCanonicalName_CaseInsensitively()
        {
            foreach (EquipSlot expected in Enum.GetValues(typeof(EquipSlot)))
            {
                string name = expected.ToString();

                Assert.IsTrue(EquipSlots.TryParse(name, out var exact),
                    $"canonical name '{name}' must parse");
                Assert.AreEqual(expected, exact);

                Assert.IsTrue(EquipSlots.TryParse(name.ToLowerInvariant(), out var lower),
                    $"lowercase '{name}' must parse");
                Assert.AreEqual(expected, lower);
            }
        }

        [Test]
        public void TryParse_MapsLegacyAliasesOntoBody()
        {
            foreach (string alias in new[] { "Torso", "torso", " chest ", "CHEST" })
            {
                Assert.IsTrue(EquipSlots.TryParse(alias, out var slot),
                    $"legacy alias '{alias}' must still parse so old saves keep working");
                Assert.AreEqual(EquipSlot.Body, slot, $"'{alias}' should resolve to Body");
            }
        }

        [Test]
        public void TryParse_TreatsEmptyAsNone_ButReportsUnknownSpellings()
        {
            Assert.IsTrue(EquipSlots.TryParse(null, out var fromNull));
            Assert.AreEqual(EquipSlot.None, fromNull);

            Assert.IsTrue(EquipSlots.TryParse("   ", out var fromBlank));
            Assert.AreEqual(EquipSlot.None, fromBlank);

            // The point of the bool: a typo must be reportable, not silently dropped.
            Assert.IsFalse(EquipSlots.TryParse("Torsoo", out var fromTypo),
                "an unrecognised spelling must return false so callers can report it");
            Assert.AreEqual(EquipSlot.None, fromTypo);
        }

        [Test]
        public void IsCanonicalName_RejectsLegacyAliases_SoShippedDataStaysClean()
        {
            Assert.IsTrue(EquipSlots.IsCanonicalName("Body"));
            Assert.IsTrue(EquipSlots.IsCanonicalName(""), "empty means 'no slot' and is valid");

            Assert.IsFalse(EquipSlots.IsCanonicalName("Torso"),
                "aliases parse at runtime but must not be allowed in shipped data");
            Assert.AreEqual("Body", EquipSlots.CanonicalNameForAlias("Torso"),
                "the validator needs to name the replacement to be actionable");
            Assert.IsNull(EquipSlots.CanonicalNameForAlias("Torsoo"));
        }

        [Test]
        public void ShippedItemData_UsesOnlyCanonicalEquipSlotNames()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Data", "items.json");
            Assert.IsTrue(File.Exists(path), $"items.json not found at {path}");

            // Deliberately a raw text scan rather than a typed load: this asserts on
            // what is actually committed to disk, which is what the build gate reads.
            string json = File.ReadAllText(path);
            var offenders = new List<string>();
            foreach (System.Text.RegularExpressions.Match m in
                     System.Text.RegularExpressions.Regex.Matches(json, "\"equipSlot\"\\s*:\\s*\"([^\"]*)\""))
            {
                string raw = m.Groups[1].Value;
                if (!EquipSlots.IsCanonicalName(raw))
                    offenders.Add(raw);
            }

            Assert.IsEmpty(offenders,
                "items.json must use canonical EquipSlot names; found: " + string.Join(", ", offenders));
        }
    }
}
