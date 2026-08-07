using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Structural guard against the stub-save bug class.
    ///
    /// Fifteen systems shipped with <c>CaptureState()</c> returning a DTO that held
    /// nothing but its own <c>systemId</c> and <c>RestoreState()</c> written as
    /// <c>{ _ = saved; }</c>. They were registered with SaveSystem and appeared in
    /// the save file, so nothing looked wrong — but every tetanus case, frostbite,
    /// cataract, blocked route, and stranded vehicle silently reset on load.
    ///
    /// The tell is mechanical: a system that owns an id-keyed state dictionary but
    /// whose save DTO carries no payload cannot possibly persist that dictionary.
    /// This test reflects over the shipped assembly and fails on that shape, so the
    /// next system written from the same template is caught at test time rather than
    /// by a player losing a campaign's worth of afflictions.
    /// </summary>
    [TestFixture]
    public class SaveStateCompletenessTests
    {
        /// <summary>
        /// Types whose dictionaries are derived caches, static catalogs, or immutable
        /// config templates rather than mutable run state, so an empty save DTO is
        /// genuinely correct. Add here only after checking the field is either never
        /// mutated during play or explicitly rebuilt on restore — and say which.
        /// </summary>
        private static readonly HashSet<string> ExemptFromMapPersistence = new HashSet<string>
        {
            // _lastHealth is a per-frame delta cache, not run state; RestoreState
            // clears it so it rebuilds from the restored survivors on the next tick.
            "BunkerSocialDirector",
        };

        /// <summary>
        /// Types whose <c>RestoreState</c> is intentionally a no-op, with the reason
        /// stated at the call site.
        /// </summary>
        private static readonly HashSet<string> ExemptFromRestore = new HashSet<string>
        {
            // CaptureState returns the readonly tuning config itself; restoring it
            // would swap live constants for whatever an old save happened to hold.
            "Action_Fish",
            // DTO carries only a Version stamp reserved for future accumulators.
            "PantryContaminationSystem",
        };

        /// <summary>
        /// Every type across all gameplay assemblies, not just Core. Systems live in
        /// Environment, Medical, Shelter, Simulation, and others, and a guard that
        /// only saw Core would leave most of the codebase unchecked.
        /// </summary>
        private static IEnumerable<Type> GameAssemblyTypes()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetName().Name?.StartsWith("AtomicWar", StringComparison.Ordinal) != true)
                    continue;

                Type[] types;
                try { types = asm.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { types = ex.Types; }

                foreach (Type t in types)
                    if (t != null) yield return t;
            }
        }

        private static bool IsStateMap(Type t) =>
            t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);

        /// <summary>A DTO that only carries its own id persists nothing.</summary>
        private static bool CarriesNoPayload(Type dto) =>
            dto.GetFields(BindingFlags.Public | BindingFlags.Instance)
               .All(f => f.Name == "systemId");

        [Test]
        public void EverySystemOwningAStateMap_PersistsIt()
        {
            var offenders = new List<string>();

            foreach (Type type in GameAssemblyTypes())
            {
                if (!type.IsClass || type.IsAbstract || ExemptFromMapPersistence.Contains(type.Name)) continue;

                MethodInfo capture = type.GetMethod("CaptureState",
                    BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (capture == null || capture.ReturnType == typeof(void)) continue;
                // A declared return of `object` picks its DTO at runtime; nothing can
                // be concluded from the signature alone.
                if (capture.ReturnType == typeof(object)) continue;

                var maps = type
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => IsStateMap(f.FieldType))
                    .Select(f => f.Name)
                    .ToList();

                if (maps.Count > 0 && CarriesNoPayload(capture.ReturnType))
                {
                    offenders.Add(
                        $"{type.Name}: owns {string.Join(", ", maps)} but " +
                        $"{capture.ReturnType.Name} carries only systemId — that state is dropped on load.");
                }
            }

            Assert.IsEmpty(offenders,
                "Systems whose save DTO cannot hold their own state:\n  " +
                string.Join("\n  ", offenders) +
                "\n\nUse SaveMap.Capture/SaveMap.Restore, or add the type to Exempt " +
                "if the dictionary is a read-only catalog.");
        }

        [Test]
        public void RestoreState_IsNeverAPureStub()
        {
            // A no-op RestoreState paired with a payload-carrying DTO means the save
            // file grew a section that is written and then ignored — strictly worse
            // than not saving it, because it looks correct in the file.
            var offenders = new List<string>();

            foreach (Type type in GameAssemblyTypes())
            {
                if (!type.IsClass || type.IsAbstract || ExemptFromRestore.Contains(type.Name)) continue;

                MethodInfo capture = type.GetMethod("CaptureState",
                    BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (capture == null || capture.ReturnType == typeof(object)) continue;
                if (CarriesNoPayload(capture.ReturnType)) continue;

                // Match on the DTO type, not an exact signature: several systems need
                // extra context to rebuild (Inventory takes an item lookup, survivor
                // systems take the roster) and GeneratedMap names its restore
                // RestoreRevealState. All are legitimate restores.
                MethodInfo restore = type
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m =>
                        m.Name.StartsWith("Restore", StringComparison.Ordinal) &&
                        m.GetParameters().FirstOrDefault()?.ParameterType == capture.ReturnType);

                if (restore == null)
                {
                    offenders.Add($"{type.Name}: captures {capture.ReturnType.Name} but nothing consumes it on load.");
                    continue;
                }

                // An empty body compiles to ~1-2 IL bytes (nop/ret). A real restore is longer.
                MethodBody body = restore.GetMethodBody();
                if (body != null && body.GetILAsByteArray()?.Length <= 2)
                    offenders.Add($"{type.Name}.RestoreState is empty while {capture.ReturnType.Name} carries state.");
            }

            Assert.IsEmpty(offenders, "Write-only save sections:\n  " + string.Join("\n  ", offenders));
        }

        // -----------------------------------------------------------------
        // SaveMap behaviour
        // -----------------------------------------------------------------

        private sealed class Entry { public string id; public int value; }

        [Test]
        public void SaveMap_RoundTripsAMapThroughAList()
        {
            var source = new Dictionary<string, Entry>
            {
                ["a"] = new Entry { id = "a", value = 1 },
                ["b"] = new Entry { id = "b", value = 2 },
            };

            var target = new Dictionary<string, Entry>();
            SaveMap.Restore(target, SaveMap.Capture(source), e => e.id);

            Assert.AreEqual(2, target.Count);
            Assert.AreEqual(2, target["b"].value);
        }

        [Test]
        public void SaveMap_Restore_ReplacesLiveStateRatherThanMergingIntoIt()
        {
            // Loading a save must not leave an affliction behind that the save says
            // was cured.
            var live = new Dictionary<string, Entry> { ["stale"] = new Entry { id = "stale" } };

            SaveMap.Restore(live, new List<Entry> { new Entry { id = "fresh" } }, e => e.id);

            Assert.IsFalse(live.ContainsKey("stale"), "Pre-load state must not survive a load.");
            Assert.IsTrue(live.ContainsKey("fresh"));
        }

        [Test]
        public void SaveMap_Restore_WithNullList_ClearsRatherThanKeepingStaleEntries()
        {
            var live = new Dictionary<string, Entry> { ["stale"] = new Entry { id = "stale" } };

            SaveMap.Restore(live, null, e => e.id);

            Assert.IsEmpty(live, "An absent save section means 'nothing here', not 'keep what you had'.");
        }

        [Test]
        public void SaveMap_SkipsNullAndUnkeyedEntries()
        {
            var live = new Dictionary<string, Entry>();

            SaveMap.Restore(live, new List<Entry>
            {
                null,
                new Entry { id = null },
                new Entry { id = "" },
                new Entry { id = "real" },
            }, e => e.id);

            Assert.AreEqual(1, live.Count, "Only the keyed entry can ever be looked up again.");
            Assert.IsTrue(live.ContainsKey("real"));
        }

        [Test]
        public void SaveMap_Capture_OfNullOrEmpty_YieldsAnEmptyList()
        {
            Assert.IsEmpty(SaveMap.Capture<Entry>(null));
            Assert.IsEmpty(SaveMap.Capture(new Dictionary<string, Entry>()));
        }
    }
}
