using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

using AtomicWar._Game.Encounters;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompts #901/#903/#904 — the three narrative encounters reaching gameplay.
    ///
    /// NarrativeEncounters registered their EncounterSOs from the start, so the
    /// encounters really did appear on expeditions. But the SOs carried no choices
    /// and nothing constructed the Encounter_* classes holding their outcomes, so
    /// resolving one printed its description and did nothing: no loot, no morale,
    /// no faction standing, nothing persisted. These pin the dispatch.
    /// </summary>
    [TestFixture]
    public class NarrativeEncounterDispatchTests
    {
        private List<Object> _destroy;

        [SetUp]
        public void SetUp() => _destroy = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _destroy.Count; i++)
            {
                if (_destroy[i] != null) Object.DestroyImmediate(_destroy[i]);
            }
            _destroy = null;
        }

        // ── The SOs themselves ──────────────────────────────────────────

        [Test]
        public void EveryNarrativeEncounter_OffersChoices()
        {
            // Empty choices are why these were inert: PickBeliefWeightedChoice
            // returns null for an empty list and ApplyEncounterChoice no-ops on null.
            foreach (var enc in CreateAll())
            {
                Assert.IsNotNull(enc.choices, $"{enc.id} must offer choices");
                Assert.That(enc.choices.Count, Is.GreaterThan(0), $"{enc.id} must offer choices");
                for (int i = 0; i < enc.choices.Count; i++)
                {
                    Assert.IsFalse(string.IsNullOrEmpty(enc.choices[i].ChoiceId),
                        $"{enc.id} choice {i} needs an id for the dispatcher to match on");
                }
            }
        }

        [Test]
        public void EveryChoiceId_IsHandledByTheDispatcher()
        {
            // A choice whose id the dispatcher does not recognise falls through the
            // switch silently — the exact failure mode this whole fixture exists for.
            foreach (var enc in CreateAll())
            {
                for (int i = 0; i < enc.choices.Count; i++)
                {
                    var h = NewHarness();
                    h.Resolve(enc.id, enc.choices[i].ChoiceId);
                    Assert.IsTrue(h.AnythingHappened(),
                        $"{enc.id}/{enc.choices[i].ChoiceId} resolved to no effect at all");
                }
            }
        }

        // ── Prompt #901 — Dead Letter Office ────────────────────────────

        [Test]
        public void ReadingTheLetters_CostsMorale_AndLeavesOneKeepsake()
        {
            var h = NewHarness();
            float before = h.Survivor.Needs.Morale;

            h.Resolve(NarrativeEncounters.DeadLetterOfficeId, NarrativeEncounters.ChoiceReadLetters);

            Assert.That(h.Survivor.Needs.Morale, Is.LessThan(before));
            Assert.That(h.LootIds(), Contains.Item(Encounter_DeadLetterOffice.LetterItemId));
        }

        [Test]
        public void ReadingTheLetters_Twice_CostsNothingTheSecondTime()
        {
            var h = NewHarness();
            h.Resolve(NarrativeEncounters.DeadLetterOfficeId, NarrativeEncounters.ChoiceReadLetters);
            float afterFirst = h.Survivor.Needs.Morale;

            h.Resolve(NarrativeEncounters.DeadLetterOfficeId, NarrativeEncounters.ChoiceReadLetters);

            Assert.AreEqual(afterFirst, h.Survivor.Needs.Morale, 0.001f,
                "an emptied van has no grief left to hand out");
        }

        [Test]
        public void DeliveringTheLetter_BuysStandingWithTheScavengerCamp()
        {
            var h = NewHarness();

            h.Resolve(NarrativeEncounters.DeadLetterOfficeId, NarrativeEncounters.ChoiceDeliverLetter);

            Assert.AreEqual(1, h.TrustCalls.Count);
            Assert.AreEqual(Encounter_DeadLetterOffice.TargetFactionId, h.TrustCalls[0].Key);
            Assert.AreEqual(Encounter_DeadLetterOffice.DeliveryTrustBoost, h.TrustCalls[0].Value, 0.001f);

            // The letter is gone; a second delivery must not pay again.
            h.Resolve(NarrativeEncounters.DeadLetterOfficeId, NarrativeEncounters.ChoiceDeliverLetter);
            Assert.AreEqual(1, h.TrustCalls.Count);
        }

        [Test]
        public void TakingTheSupplies_FillsThePackWithRations()
        {
            var h = NewHarness();

            h.Resolve(NarrativeEncounters.DeadLetterOfficeId, NarrativeEncounters.ChoiceTakeSupplies);

            Assert.That(h.LootIds(), Contains.Item(Encounter_DeadLetterOffice.SupplyItemId));
        }

        [Test]
        public void BurningTheVan_HitsTheWholeParty_Once()
        {
            var h = NewHarness(bystanders: 2);
            float[] before = h.PartyMorale();

            h.Resolve(NarrativeEncounters.DeadLetterOfficeId, NarrativeEncounters.ChoiceBurnVan);

            float[] afterBurn = h.PartyMorale();
            for (int i = 0; i < before.Length; i++)
            {
                Assert.That(afterBurn[i], Is.LessThan(before[i]),
                    "everyone in the bunker watches the smoke, not just the arsonist");
            }

            // A burnt van cannot be burnt again.
            h.Resolve(NarrativeEncounters.DeadLetterOfficeId, NarrativeEncounters.ChoiceBurnVan);
            Assert.AreEqual(afterBurn, h.PartyMorale());
        }

        // ── Prompt #903 — Weather Station ───────────────────────────────

        [Test]
        public void ExtractingTheData_RaisesTheForecastFlag()
        {
            var h = NewHarness();

            h.Resolve(NarrativeEncounters.WeatherStationId, NarrativeEncounters.ChoiceExtractData);

            Assert.IsTrue(h.Flags.TryGetValue(Encounter_WeatherStation.ForecastBoostFlag, out bool set) && set);
            Assert.AreEqual(h.WeatherStation.State.forecastAccuracyBonusDays,
                h.WeatherStation.GetForecastBonusDays());
        }

        [Test]
        public void StrippingTheStation_YieldsScrap_AndOnlyOnce()
        {
            var h = NewHarness();

            h.Resolve(NarrativeEncounters.WeatherStationId, NarrativeEncounters.ChoiceScavengeElectronics);
            int first = CountOf(h.LootIds(), Encounter_WeatherStation.ElectronicScrapItemId);
            Assert.That(first, Is.GreaterThan(0));

            h.Resolve(NarrativeEncounters.WeatherStationId, NarrativeEncounters.ChoiceScavengeElectronics);
            Assert.AreEqual(first, CountOf(h.LootIds(), Encounter_WeatherStation.ElectronicScrapItemId));
        }

        [Test]
        public void LeavingItRunning_IsASmallKindnessToYourself()
        {
            var h = NewHarness();
            float before = h.Survivor.Needs.Morale;

            h.Resolve(NarrativeEncounters.WeatherStationId, NarrativeEncounters.ChoiceLeaveRunning);

            Assert.That(h.Survivor.Needs.Morale, Is.GreaterThan(before));
        }

        // ── Prompt #904 — The Pianist ───────────────────────────────────

        [Test]
        public void Listening_LiftsMorale_AndDiminishesWithRepeatVisits()
        {
            var h = NewHarness();

            float before = h.Survivor.Needs.Morale;
            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceListen);
            float firstGain = h.Survivor.Needs.Morale - before;

            before = h.Survivor.Needs.Morale;
            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceListen);
            float secondGain = h.Survivor.Needs.Morale - before;

            Assert.That(firstGain, Is.GreaterThan(0f));
            Assert.That(secondGain, Is.LessThan(firstGain));
        }

        [Test]
        public void SharingFood_SpendsARation_AndOnlyWhenThereIsOne()
        {
            var h = NewHarness();
            h.Larder[Harness.PianistFoodId] = 1;
            float before = h.Survivor.Needs.Morale;

            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceShareFood);

            Assert.AreEqual(0, h.Larder[Harness.PianistFoodId], "the tin has to actually leave the bunker");
            Assert.That(h.Survivor.Needs.Morale, Is.GreaterThan(before));

            // Empty larder: the offer cannot be made, and morale must not move.
            float afterShare = h.Survivor.Needs.Morale;
            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceShareFood);
            Assert.AreEqual(afterShare, h.Survivor.Needs.Morale, 0.001f);
        }

        [Test]
        public void SharingFood_WithNothingToShare_DoesNotFeedHim()
        {
            var h = NewHarness();
            h.Larder.Clear();

            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceShareFood);

            Assert.IsFalse(h.Pianist.State.hasReceivedFood,
                "an empty pack must not count as a meal he remembers");
        }

        [Test]
        public void TellingHimAboutTheBunker_SpreadsTheWordOnce()
        {
            var h = NewHarness();

            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceTellAboutBunker);

            Assert.IsTrue(h.Flags.TryGetValue(Encounter_Pianist.ToldAboutBunkerFlag, out bool set) && set);
            Assert.IsTrue(h.Pianist.State.hasBeenToldAboutBunker);
        }

        [Test]
        public void CuttingTheWire_PaysOnce_AndEndsTheMusic()
        {
            var h = NewHarness();
            float before = h.Survivor.Needs.Morale;

            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceDestroyPiano);
            int wire = CountOf(h.LootIds(), Encounter_Pianist.WireCutterItemId);
            float afterCut = h.Survivor.Needs.Morale;

            Assert.AreEqual(Encounter_Pianist.WireCutterYield, wire);
            Assert.That(afterCut, Is.LessThan(before));

            // "Destroys the encounter permanently" was documented but never recorded,
            // so a second visit used to hand out the wire and the guilt all over again.
            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceDestroyPiano);
            Assert.AreEqual(wire, CountOf(h.LootIds(), Encounter_Pianist.WireCutterItemId));
            Assert.AreEqual(afterCut, h.Survivor.Needs.Morale, 0.001f);
        }

        [Test]
        public void ListeningToAStrippedPiano_GivesNoComfort()
        {
            var h = NewHarness();
            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceDestroyPiano);
            float before = h.Survivor.Needs.Morale;

            h.Resolve(NarrativeEncounters.PianistId, NarrativeEncounters.ChoiceListen);

            Assert.AreEqual(before, h.Survivor.Needs.Morale, 0.001f);
        }

        // ── Unbound hosts ───────────────────────────────────────────────

        [Test]
        public void AnUnboundHost_ResolvesTheseEncountersHarmlessly()
        {
            // The binds are optional — tests and partial hosts build the expedition
            // system alone — so an unbound dispatch must not throw.
            var h = NewHarness(bindEncounters: false, bindWriters: false);
            foreach (var enc in CreateAll())
            {
                for (int i = 0; i < enc.choices.Count; i++)
                {
                    string id = enc.id, choice = enc.choices[i].ChoiceId;
                    Assert.DoesNotThrow(() => h.Resolve(id, choice));
                }
            }
        }

        [Test]
        public void BoundEncounters_ResolveEvenWithoutTrustOrFlagWriters()
        {
            // Faction trust and world flags come from the host; a headless host
            // still has to get the loot and morale half of the outcome.
            var h = NewHarness(bindWriters: false);

            Assert.DoesNotThrow(() =>
                h.Resolve(NarrativeEncounters.WeatherStationId, NarrativeEncounters.ChoiceExtractData));
            Assert.IsTrue(h.WeatherStation.State.isDataExtracted);

            Assert.DoesNotThrow(() =>
                h.Resolve(NarrativeEncounters.DeadLetterOfficeId, NarrativeEncounters.ChoiceDeliverLetter));
            Assert.IsTrue(h.DeadLetterOffice.State.letterDelivered);
        }

        // ── Downstream of the choices ───────────────────────────────────

        [Test]
        public void ExtractedWeatherData_ShortensButEnablesTheForecast()
        {
            // The extraction used to end at a world flag: GetForecastBonusDays had
            // no callers, so five days of readings changed nothing anyone could see.
            var station = new Encounter_WeatherStation();
            var weather = new WeatherSystem();
            weather.BindStationForecast(station.GetForecastBonusDays);

            Assert.IsEmpty(weather.GetPerfectForecast(),
                "no logger copied, no forecast");

            station.ExtractData();
            var forecast = weather.GetPerfectForecast();

            Assert.AreEqual(station.State.forecastAccuracyBonusDays, forecast.Length,
                "the logger holds five days of readings, not the Stormcaller's ten");
        }

        [Test]
        public void WordOfTheBunker_BringsExactlyOnePerson()
        {
            // RollRefugeeArrival had no caller at all, so "someone else might come"
            // was a flag and nothing more. It is also one-shot: a 2%/day roll left
            // running for a long campaign would otherwise repopulate the bunker.
            var pianist = new Encounter_Pianist();
            var rng = new System.Random(1234);

            for (int day = 0; day < 500; day++)
                Assert.IsFalse(pianist.RollRefugeeArrival(rng),
                    "nobody comes if he was never told");

            pianist.TellAboutBunker();

            int arrivals = 0;
            for (int day = 0; day < 500; day++)
                if (pianist.RollRefugeeArrival(rng)) arrivals++;

            Assert.AreEqual(1, arrivals);
            Assert.IsTrue(pianist.State.refugeeArrived);
        }

        // ── Shipped data ────────────────────────────────────────────────

        [Test]
        public void ItemsJson_CoversEveryNarrativePayout()
        {
            // The host resolves these through the items.json-backed catalog. An id
            // that is not in there resolves to null and the choice pays out nothing
            // — silently, because a missing definition is skipped, not reported.
            // solar_cell was exactly that: promised by the encounter, defined nowhere.
            string path = Path.Combine(Application.streamingAssetsPath, "Data", "items.json");
            Assert.IsTrue(File.Exists(path), $"items.json not found at {path}");
            string json = File.ReadAllText(path);

            foreach (string itemId in new[]
            {
                Encounter_DeadLetterOffice.SupplyItemId,
                Encounter_DeadLetterOffice.LetterItemId,
                Encounter_WeatherStation.SolarCellItemId,
                Encounter_WeatherStation.ElectronicScrapItemId,
                Encounter_Pianist.WireCutterItemId
            })
            {
                StringAssert.Contains($"\"id\": \"{itemId}\"", json,
                    $"'{itemId}' is paid out by a narrative encounter but is not in items.json");
            }
        }

        // ── Harness ─────────────────────────────────────────────────────

        private List<EncounterSO> CreateAll()
        {
            var list = new List<EncounterSO>
            {
                NarrativeEncounters.CreateDeadLetterOffice(),
                NarrativeEncounters.CreateWeatherStation(),
                NarrativeEncounters.CreatePianist()
            };
            for (int i = 0; i < list.Count; i++) _destroy.Add(list[i]);
            return list;
        }

        private static int CountOf(List<string> ids, string itemId)
        {
            int n = 0;
            for (int i = 0; i < ids.Count; i++)
                if (ids[i] == itemId) n++;
            return n;
        }

        private Harness NewHarness(int bystanders = 0, bool bindEncounters = true, bool bindWriters = true)
        {
            var h = new Harness(_destroy, bystanders, bindEncounters, bindWriters);
            return h;
        }

        /// <summary>
        /// Holds one expedition state and feeds chosen encounter/choice pairs
        /// straight through the dispatch seam, skipping the pool roll and the
        /// belief-weighted choice pick.
        /// </summary>
        private sealed class Harness
        {
            /// <summary>The ration Matej is fed with — same id the van's packs hold.</summary>
            public const string PianistFoodId = Encounter_DeadLetterOffice.SupplyItemId;

            /// <summary>Item ids the narrative outcomes pay out in.</summary>
            private static readonly string[] PayoutItemIds =
            {
                Encounter_DeadLetterOffice.SupplyItemId,
                Encounter_DeadLetterOffice.LetterItemId,
                Encounter_WeatherStation.SolarCellItemId,
                Encounter_WeatherStation.ElectronicScrapItemId,
                Encounter_Pianist.WireCutterItemId
            };

            /// <summary>
            /// Stand in for the items.json-backed catalog the host injects. Every id
            /// here must exist in Assets/StreamingAssets/Data/items.json — the
            /// ItemsJsonCoversNarrativePayouts test is what keeps the two in step.
            /// </summary>
            private static ItemCatalogSO BuildCatalog(List<Object> destroy)
            {
                var catalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
                destroy.Add(catalog);
                catalog.items = new List<ItemDefinition>();
                for (int i = 0; i < PayoutItemIds.Length; i++)
                {
                    var def = ScriptableObject.CreateInstance<ItemDefinition>();
                    def.id = PayoutItemIds[i];
                    def.displayName = PayoutItemIds[i];
                    def.stackMax = 10;
                    def.weight = 0.1f;
                    destroy.Add(def);
                    catalog.items.Add(def);
                }
                return catalog;
            }

            public readonly Survivor Survivor;
            public readonly List<Survivor> Party = new List<Survivor>();
            public readonly Dictionary<string, int> Larder = new Dictionary<string, int>();
            public readonly Dictionary<string, bool> Flags = new Dictionary<string, bool>();
            public readonly List<KeyValuePair<string, float>> TrustCalls =
                new List<KeyValuePair<string, float>>();

            public readonly Encounter_DeadLetterOffice DeadLetterOffice = new Encounter_DeadLetterOffice();
            public readonly Encounter_WeatherStation WeatherStation = new Encounter_WeatherStation();
            public readonly Encounter_Pianist Pianist = new Encounter_Pianist();

            private readonly ExpeditionSystem _sys;
            private readonly ExpeditionState _exp;
            private readonly float _startingMorale;
            private readonly Dictionary<string, EncounterSO> _encounters =
                new Dictionary<string, EncounterSO>();

            public Harness(List<Object> destroy, int bystanders, bool bindEncounters, bool bindWriters)
            {
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                destroy.Add(profile);
                var needs = new NeedsSystem(profile);
                var rad = new RadiationSystem(needs);
                var inv = new Inventory { Capacity = 60, MaxWeight = 400f };

                Survivor = new Survivor { Id = "sv_courier", DisplayName = "Courier", State = SurvivorState.Idle };
                needs.Register(Survivor);
                rad.Register(Survivor);
                Party.Add(Survivor);
                for (int i = 0; i < bystanders; i++)
                {
                    var b = new Survivor { Id = "sv_home" + i, DisplayName = "Bystander " + i };
                    needs.Register(b);
                    Party.Add(b);
                }
                // Mid-range, so both a gain and a loss are visible: a survivor
                // pinned at full morale would hide every positive outcome.
                for (int i = 0; i < Party.Count; i++) Party[i].Needs.Morale = 50f;

                Larder[PianistFoodId] = 3;

                _sys = new ExpeditionSystem(rad, inv, BuildCatalog(destroy), new ExpeditionSystem.Config
                {
                    Seed = 7,
                    CreateDefaultEncounters = false
                });
                _sys.SetNeedsSystem(needs);
                _sys.SetItemHandlers(
                    id => Larder.TryGetValue(id, out int n) ? n : 0,
                    (id, count) =>
                    {
                        if (!Larder.TryGetValue(id, out int have) || have < count) return false;
                        Larder[id] = have - count;
                        return true;
                    });
                // The only seam that supplies the full roster (burning the van is
                // party-wide) also happens to be the combat-perk bind.
                _sys.BindCombatPerks(null, null, null, () => Party);

                if (bindEncounters)
                {
                    _sys.BindDeadLetterOffice(DeadLetterOffice);
                    _sys.BindWeatherStation(WeatherStation);
                    _sys.BindPianist(Pianist);
                }
                if (bindWriters)
                {
                    _sys.BindFactionTrustWriter((f, d) => TrustCalls.Add(new KeyValuePair<string, float>(f, d)));
                    _sys.BindWorldFlagWriter((f, v) => Flags[f] = v);
                }

                foreach (var enc in new[]
                {
                    NarrativeEncounters.CreateDeadLetterOffice(),
                    NarrativeEncounters.CreateWeatherStation(),
                    NarrativeEncounters.CreatePianist()
                })
                {
                    destroy.Add(enc);
                    _encounters[enc.id] = enc;
                    _sys.AddEncounter(enc);
                }

                _exp = new ExpeditionState
                {
                    Survivor = Survivor,
                    TargetLocationName = "Ring Road",
                    // Generous, so a full pack never masks a missing payout.
                    CarryingCapacity = 400f
                };
                _startingMorale = Survivor.Needs.Morale;
            }

            public void Resolve(string encounterId, string choiceId)
            {
                var enc = _encounters[encounterId];
                EventChoice chosen = null;
                for (int i = 0; i < enc.choices.Count; i++)
                {
                    if (enc.choices[i].ChoiceId == choiceId) { chosen = enc.choices[i]; break; }
                }
                Assert.IsNotNull(chosen, $"{encounterId} has no choice '{choiceId}'");
                _sys.ForceNarrativeChoiceForTests(_exp, enc, chosen);
            }

            public List<string> LootIds() =>
                new List<string>(_exp.CollectedLootItemIds);

            public float[] PartyMorale()
            {
                var m = new float[Party.Count];
                for (int i = 0; i < Party.Count; i++) m[i] = Party[i].Needs.Morale;
                return m;
            }

            /// <summary>Did the choice change anything observable at all?</summary>
            public bool AnythingHappened()
            {
                return LootIds().Count > 0
                    || TrustCalls.Count > 0
                    || Flags.Count > 0
                    || !Mathf.Approximately(Survivor.Needs.Morale, _startingMorale)
                    || DeadLetterOffice.State.isLooted
                    || DeadLetterOffice.State.lettersRead
                    || DeadLetterOffice.State.letterDelivered
                    || WeatherStation.State.isDataExtracted
                    || WeatherStation.State.isSolarPanelTaken
                    || WeatherStation.State.isElectronicsScavenged
                    || Pianist.State.timesVisited > 0
                    || Pianist.State.hasReceivedFood
                    || Pianist.State.hasBeenToldAboutBunker
                    || Pianist.State.pianoDestroyed;
            }
        }
    }
}
