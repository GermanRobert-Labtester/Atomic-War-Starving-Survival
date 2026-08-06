using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Shared base class for host-integration tests that exercise
    /// PersonalQuestSystem wiring with SkillProgressionSystem + NeedsSystem.
    /// Eliminates the duplicated SetUp/TearDown/MakeItem/MakeArchetype
    /// boilerplate that was copy-pasted across 5 test files (audit smell fix).
    /// </summary>
    public abstract class PersonalQuestHostTestBase
    {
        protected const float Eps = 0.02f;

        protected SkillProgressionSystem _progression;
        protected PersonalQuestSystem _quests;
        protected List<Survivor> _survivors;
        protected NeedsProfile _profile;
        protected NeedsSystem _needs;
        protected readonly List<Object> _toDestroy = new List<Object>();

        [SetUp]
        public virtual void SetUp()
        {
            _progression = new SkillProgressionSystem();
            _progression.RegisterDefaultPerks();
            _quests = new PersonalQuestSystem();
            _quests.Bind(_progression);
            _progression.BindPersonalQuests(_quests);
            _survivors = new List<Survivor>();

            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            Track(_profile);
            _profile.hungerPerHour = 0f;
            _profile.thirstPerHour = 0f;
            _profile.fatiguePerHour = 0f;
            _profile.warmthLossPerHourInCold = 0f;
            _profile.hungerCritical = 100f;
            _profile.thirstCritical = 100f;
            _profile.warmthCritical = 0f;
            _profile.moraleLossPerHourWhileCritical = 0f;
            _needs = new NeedsSystem(_profile);
            _needs.BindPersonalQuests(_quests, () => _survivors);
        }

        [TearDown]
        public virtual void TearDown()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy.Clear();
        }

        protected T Track<T>(T obj) where T : Object
        {
            _toDestroy.Add(obj);
            return obj;
        }

        protected Survivor MakeArchetype(string archetypeId, string runtimeId = null)
        {
            var sv = PersonalQuestSystem.MakeArchetypeSurvivor(archetypeId, runtimeId);
            Assert.IsNotNull(sv, "archetype " + archetypeId);
            _quests.AssignProfile(sv, PersonalQuestSystem.ProfileForArchetype(archetypeId));
            _survivors.Add(sv);
            _needs.Register(sv);
            return sv;
        }

        protected static ItemDefinition MakeItem(
            string id,
            ItemType type,
            float tradeValue = 0f,
            float hungerRestore = 0f)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.tradeValue = tradeValue;
            item.hungerRestore = hungerRestore;
            item.stackMax = 99;
            item.weight = 0.1f;
            return item;
        }
    }
}
