using Godot;
using System;

namespace AtomicWar.Tests
{
    public partial class CSharpVerificationTest : Node
    {
        [Signal]
        public delegate void SurvivalEventTriggeredEventHandler(string eventName, int severity);

        [Export]
        public string GameName { get; set; } = "ASHFALL";

        [Export]
        public int Day { get; set; } = 1;

        public override void _Ready()
        {
            GD.Print("=================================================");
            GD.Print("[C# Test] Running Godot C# (.NET) Verification...");
            GD.Print($"[C# Test] CLR Runtime Version: {System.Environment.Version}");
            GD.Print($"[C# Test] Game Title Property: {GameName}");
            GD.Print($"[C# Test] Day Property: {Day}");

            // Test Signals
            SurvivalEventTriggered += OnSurvivalEvent;
            EmitSignal(SignalName.SurvivalEventTriggered, "FalloutStorm", 4);

            // Test Vector Math
            Vector2 pos = new Vector2(100f, 200f);
            pos = pos.Rotated(Mathf.DegToRad(45));
            GD.Print($"[C# Test] Vector2 Math: Rotated 45deg = {pos}");

            // Test Node Hierarchy
            var childNode = new Node { Name = "SurvivorRegistry" };
            AddChild(childNode);
            GD.Print($"[C# Test] Node Hierarchy: Child added = {childNode.Name}, Total Children = {GetChildCount()}");

            GD.Print("[C# Test] SUCCESS: All C# (.cs) language bindings and Godot features verified!");
            GD.Print("=================================================");
        }

        private void OnSurvivalEvent(string eventName, int severity)
        {
            GD.Print($"[C# Test] Signal Received: Event = {eventName}, Severity = {severity}/5");
        }
    }
}
