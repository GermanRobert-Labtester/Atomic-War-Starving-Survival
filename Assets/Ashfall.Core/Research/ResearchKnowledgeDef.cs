using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Individual research knowledge node. Engine-agnostic POCO — no
    /// UnityEngine / Godot / JsonUtility references. Mirrors
    /// <see cref="Survivors.SkillDef"/> shape.
    /// </summary>
    public sealed class ResearchKnowledgeDef
    {
        /// <summary>snake_case knowledge id (e.g. "knowledge_water_basics").</summary>
        public string id;

        /// <summary>Player-facing label.</summary>
        public string displayName;

        /// <summary>Discipline category tag.</summary>
        public string category;

        /// <summary>One-sentence flavour description.</summary>
        public string description;

        /// <summary>Knowledge ids that must be unlocked before this node can be researched.</summary>
        public string[] prerequisites;

        /// <summary>item_id awarded when the node completes.</summary>
        public string breakthroughItem;

        /// <summary>Days needed in the research queue.</summary>
        public int daysToComplete;

        /// <summary>True when this node has been unlocked (discovered in the tree).</summary>
        public bool isUnlocked;

        /// <summary>True when this node has been completed.</summary>
        public bool isCompleted;

        public ResearchKnowledgeDef() { }

        public ResearchKnowledgeDef(
            string id, string displayName, string category,
            string description, int daysToComplete,
            string[] prerequisites = null,
            string breakthroughItem = null)
        {
            this.id = id;
            this.displayName = displayName;
            this.category = category;
            this.description = description;
            this.daysToComplete = daysToComplete;
            this.prerequisites = prerequisites ?? System.Array.Empty<string>();
            this.breakthroughItem = breakthroughItem;
            this.isUnlocked = false;
            this.isCompleted = false;
        }
    }
}
