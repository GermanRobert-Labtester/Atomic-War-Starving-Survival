using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.NpcArcs
{
    /// <summary>
    /// ASHFALL — Plan 52 recurring-NPC arc data contract.
    ///
    /// Arc metadata is authored per NPC in <c>npc_arcs.json</c>; the identity
    /// itself stays in <c>characters.json</c>. The catalog is declarative
    /// only: every condition below references an EXISTING campaign authority
    /// (expansion-quest progress, survivor roster, campaign day). No arc
    /// runtime state exists — the current state is always derived, so a save
    /// that round-trips quests + roster resolves identical arc states.
    /// </summary>

    /// <summary>A "this quest was resolved with this choice" condition.</summary>
    [Serializable]
    public class NpcArcChoiceCondition
    {
        public string quest_id = string.Empty;
        public string choice_id = string.Empty;
    }

    /// <summary>
    /// One authored state of a recurring NPC's arc. States are static
    /// descriptors; the resolver picks the winning state per campaign day.
    /// </summary>
    [Serializable]
    public class NpcArcStateDefinition
    {
        public string id = string.Empty;

        /// <summary>initial | evolved | late | terminal (authored label; the
        /// resolver treats every state uniformly through precedence).</summary>
        public string kind = "initial";

        /// <summary>Short role label for the state ("Waystation Trader").</summary>
        public string role = string.Empty;

        /// <summary>Where this version of the NPC can be found. Empty = unknown/unchanged.</summary>
        public string location_id = string.Empty;

        /// <summary>One-to-three sentence state-of-the-person summary.</summary>
        public string summary = string.Empty;

        /// <summary>Campaign-day window. 0 = unbounded on that side.</summary>
        public int min_day = 0;
        public int max_day = 0;

        /// <summary>
        /// Higher wins. Documented conventions: 10 initial/day-fallback,
        /// 20-40 evolved/late branches, 60 recruited, 90 authored terminal
        /// branches (death/disappearance quests), 100 roster death.
        /// Ties resolve in authored order (stable, deterministic).
        /// </summary>
        public int precedence = 10;

        /// <summary>Terminal states override every later day-based state.</summary>
        public bool terminal = false;

        /// <summary>Quests that must be completed (expansion-quest authority).</summary>
        public List<string> requires_completed = new List<string>();

        /// <summary>Quests that must NOT be completed.</summary>
        public List<string> excludes_completed = new List<string>();

        /// <summary>Quests that must have been resolved with the given choice.</summary>
        public List<NpcArcChoiceCondition> requires_choice = new List<NpcArcChoiceCondition>();

        /// <summary>Matches only while the NPC is a living shelter resident
        /// (survivor roster definition id == npc id).</summary>
        public bool when_recruited = false;

        /// <summary>Matches only while the roster records this NPC as dead.</summary>
        public bool when_dead = false;

        /// <summary>Arc quest that presents this state (optional; display/integration hint).</summary>
        public string quest_id = string.Empty;
    }

    /// <summary>One recurring NPC's authored arc.</summary>
    [Serializable]
    public class NpcArcDefinition
    {
        public string npc_id = string.Empty;
        public string display_name = string.Empty;

        /// <summary>Flagship arcs carry the full initial/evolved/late grammar.</summary>
        public bool flagship = false;

        /// <summary>True when a recruitment branch exists via the survivor roster.</summary>
        public bool recruitable = false;

        public List<NpcArcStateDefinition> states = new List<NpcArcStateDefinition>();
    }

    [Serializable]
    public class NpcArcCatalogRoot
    {
        public int schema_version = 1;
        public List<NpcArcDefinition> arcs = new List<NpcArcDefinition>();
    }

    /// <summary>Read-only in-memory view over npc_arcs.json.</summary>
    public sealed class NpcArcCatalog
    {
        public const string FileName = "npc_arcs.json";

        private readonly List<NpcArcDefinition> _arcs = new List<NpcArcDefinition>();
        private readonly Dictionary<string, NpcArcDefinition> _byNpcId =
            new Dictionary<string, NpcArcDefinition>(StringComparer.Ordinal);

        public IReadOnlyList<NpcArcDefinition> Arcs => _arcs;

        public NpcArcDefinition? Find(string npcId)
        {
            if (string.IsNullOrEmpty(npcId)) return null;
            return _byNpcId.TryGetValue(npcId, out var arc) ? arc : null;
        }

        public void Register(NpcArcDefinition arc)
        {
            if (arc == null || string.IsNullOrEmpty(arc.npc_id)) return;
            if (_byNpcId.ContainsKey(arc.npc_id)) return;
            _arcs.Add(arc);
            _byNpcId[arc.npc_id] = arc;
        }

        /// <summary>Load npc_arcs.json. Missing file yields an empty catalog
        /// (matching the other catalog loaders' silent-empty optional behavior).</summary>
        public static NpcArcCatalog Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();

            var catalog = new NpcArcCatalog();
            string path = System.IO.Path.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return catalog;

            string json = fileIO.ReadAllText(path);
            if (string.IsNullOrEmpty(json)) return catalog;

            try
            {
                var root = serializer.Deserialize<NpcArcCatalogRoot>(json);
                if (root?.arcs == null) return catalog;
                foreach (var arc in root.arcs)
                    catalog.Register(arc);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "npc_arcs", ex);
            }
            return catalog;
        }
    }
}
