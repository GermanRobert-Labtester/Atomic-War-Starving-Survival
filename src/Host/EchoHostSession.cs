// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Flags;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin host adapter for the Core EchoSystem. It owns catalog loading and
    /// persistence only; effects are applied by Main through existing owners.
    /// </summary>
    public sealed class EchoHostSession : HostSessionBase
    {
        public EchoSystem Engine { get; }
        public EchoDefinition? PendingEcho => Engine.PendingEcho;
        public string LastEvent { get; private set; } = string.Empty;

        public EchoHostSession(EchoSystem engine)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Engine.OnEchoSurfaced += echo =>
            {
                LastEvent = $"Echo surfaced: {echo.Title}.";
                RaiseStateChanged();
            };
            Engine.OnEchoResolved += result =>
            {
                LastEvent = $"Echo resolved: {result.EchoId} / {result.ChoiceId}.";
                RaiseStateChanged();
            };
            Engine.OnDelayedConsequenceDue += result =>
            {
                LastEvent = $"Echo consequence due: {result.EchoId} / {result.ChoiceId}.";
                RaiseStateChanged();
            };
            Engine.OnStateChanged += _ => RaiseStateChanged();
        }

        public static EchoHostSession Create(string dataDir)
        {
            var engine = new EchoSystem();
            var session = new EchoHostSession(engine);
            if (!string.IsNullOrWhiteSpace(dataDir))
            {
                var load = EchoCatalogLoader.LoadDetailed(
                    dataDir,
                    new FileSystemIO(),
                    new SystemTextJsonSerializer());
                if (load.IsSuccess)
                    engine.RegisterRange(load.Echoes);
                else if (load.Errors.Count > 0)
                    session.LastEvent = "Echo catalog disabled: " + load.Errors[0];
            }

            var saved = EchoSaveStore.TryLoad();
            if (saved != null)
            {
                engine.RestoreState(saved);
                session.LastEvent = "Echo history restored from save.";
            }
            return session;
        }

        public void ConfigureFlags(IFlagLedger? flags)
        {
            Engine.HasWorldFlag = flags == null
                ? (_ => false)
                : flag => flags.IsSet(flag);
        }

        public EchoDefinition? SelectForDay(int day, ISeededRng rng)
            => Engine.SelectForDay(day, rng);

        public EchoResolutionResult Resolve(string echoId, string choiceId, int day)
            => Engine.Resolve(echoId, choiceId, day);

        public System.Collections.Generic.IReadOnlyList<EchoDelayedConsequenceResult> TickDay(int day)
            => Engine.TickDay(day);

        public EchoState CaptureSave() => Engine.CaptureState();

        public void RestoreSave(EchoState state) => Engine.RestoreState(state);
    }
}
