using System;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Base class for all Godot host sessions. Inherits standardized state-change,
    /// dirty tracking, monotonically increasing state versioning, presentation
    /// refresh separation, and save flush semantics from <see cref="StatefulSessionBase"/>.
    /// </summary>
    public class HostSessionBase : StatefulSessionBase
    {
    }
}
