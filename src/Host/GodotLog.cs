using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    public sealed class GodotLog : ILog
    {
        public void Info(string message) => GD.Print(message);
        public void Warn(string message) => GD.PushWarning(message);
        public void Error(string message) => GD.PrintErr(message);
    }
}
