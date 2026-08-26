using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core;
using Ashfall.Core.UtilityAI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Utility AI core: loads the action
    /// catalog, evaluates + selects actions for a demo survivor, and drives
    /// the crossing companion actions. No rules here — hosts only wire.
    /// </summary>
    public sealed class UtilityAiHostSession
    : HostSessionBase{
        public UtilityAiSystem Engine { get; }
        public List<UtilityActionDef> Actions { get; } = new List<UtilityActionDef>();

        public string LastEvent { get; private set; } = string.Empty;
        public UtilityAiHostSession(UtilityAiSystem engine = null!)
        {
            Engine = engine ?? new UtilityAiSystem();
            Engine.OnActionSelected += (sv, actionId, score) =>
            {
                LastEvent = $"{sv} selects {actionId} (score {score:0.000})";
                RaiseStateChanged();
            };
        }

        public static UtilityAiHostSession Create(string dataDir)
        {
            var session = new UtilityAiHostSession();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                session.Actions.AddRange(UtilityActionCatalogLoader.Load(dataDir, fileIO, serializer));
            }
            return session;
        }

        // ── Demo actions ─────────────────────────────────────────────

        public string EvaluateDemo(string survivorId, float fatigue, float skill,
            params string[] traits)
        {
            var ctx = new AIActionContext
            {
                SurvivorId = survivorId,
                IsAlive = true,
                Fatigue = fatigue,
                CraftingSkill = skill
            };
            foreach (var t in traits) ctx.Traits.Add(t);

            var picked = Engine.SelectAction(ctx, Actions, new SeededRng(2026));
            return picked != null
                ? $"{survivorId} selects {picked.displayName} ({picked.id})."
                : $"{survivorId} selects nothing (all actions gated or vetoed).";
        }

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"Utility AI: {Actions.Count} actions in catalog.\n");
            for (int i = 0; i < Actions.Count; i++)
                sb.Append($"  {Actions[i].id} — base {Actions[i].baseScore:0.00}, " +
                          $"gate {Actions[i].fatigueGate:0}, tags [{string.Join(",", Actions[i].tags)}]\n");
            return sb.ToString().TrimEnd();
        }
    }
}
