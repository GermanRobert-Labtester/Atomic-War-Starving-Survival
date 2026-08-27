using System;
using System.Collections.Generic;

namespace Ashfall.Core.UI
{
    /// <summary>
    /// Typed descriptor for a single player-navigable surface.
    /// </summary>
    public sealed class PanelDescriptor
    {
        /// <summary>Canonical snake_case id matched by OpenPlayerPanel's switch / registry router.</summary>
        public string Id { get; }

        /// <summary>Human-readable name for diagnostics and documentation.</summary>
        public string DisplayName { get; }

        /// <summary>
        /// Which surface group owns this panel.
        /// Dashboard = accessible from the in-game HUD while playing.
        /// MainMenu  = accessible from the pre-game main menu.
        /// Expanded  = routed through OpenExpandedPanel (shelter sub-systems).
        /// Secondary = detail/utility panels opened from other panels.
        /// </summary>
        public PanelGroup Group { get; }

        /// <summary>
        /// IDs of host setup methods that must be called before Bind (informational and lifecycle-driven).
        /// E.g. new[] { "survivors", "inventory" }
        /// </summary>
        public string[] SetupDependencies { get; }

        /// <summary>True when this panel may be opened even while the game is not in Playing state.</summary>
        public bool AvailableInMenu { get; }

        /// <summary>
        /// Optional predicate determining if the panel is currently available.
        /// </summary>
        public Func<bool>? AvailabilityRule { get; set; }

        /// <summary>
        /// Optional delegate to bind host sessions/models to the panel before opening.
        /// </summary>
        public Action? BindAction { get; set; }

        /// <summary>
        /// Optional delegate to display / open the panel surface.
        /// </summary>
        public Action? OpenAction { get; set; }

        /// <summary>
        /// Optional delegate to hide / close the panel surface.
        /// </summary>
        public Action? CloseAction { get; set; }

        public PanelDescriptor(
            string id,
            string displayName,
            PanelGroup group,
            string[]? setupDependencies = null,
            bool availableInMenu = false,
            Func<bool>? availabilityRule = null,
            Action? bindAction = null,
            Action? openAction = null,
            Action? closeAction = null)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Panel id must not be empty.", nameof(id));
            Id = id;
            DisplayName = displayName;
            Group = group;
            SetupDependencies = setupDependencies ?? Array.Empty<string>();
            AvailableInMenu = availableInMenu;
            AvailabilityRule = availabilityRule;
            BindAction = bindAction;
            OpenAction = openAction;
            CloseAction = closeAction;
        }

        /// <summary>
        /// Evaluates availability given whether the UI is currently in the main menu.
        /// </summary>
        public bool IsAvailable(bool isMenu = false)
        {
            if (isMenu && !AvailableInMenu)
                return false;
            return AvailabilityRule == null || AvailabilityRule();
        }

        /// <summary>Invokes the registered bind action if present.</summary>
        public void Bind() => BindAction?.Invoke();

        /// <summary>Invokes the registered open action if present.</summary>
        public void Open() => OpenAction?.Invoke();

        /// <summary>Invokes the registered close action if present.</summary>
        public void Close() => CloseAction?.Invoke();

        public override string ToString() => $"[Panel:{Id}({Group})]";
    }

    public enum PanelGroup
    {
        /// <summary>Core gameplay panels on the main dashboard.</summary>
        Dashboard,
        /// <summary>Pre-game main-menu surfaces.</summary>
        MainMenu,
        /// <summary>Shelter sub-system panels routed through OpenExpandedPanel.</summary>
        Expanded,
        /// <summary>Detail / utility panels opened from other panels or the HUD.</summary>
        Secondary
    }

    /// <summary>
    /// Engine-agnostic registry of every player-navigable panel in ASHFALL.
    /// Populated by the host's PanelRegistryBootstrap and consulted by
    /// PanelRouteGateTests to ensure every emitted id has a registered destination.
    /// Zero engine dependencies — Invariant 1.
    /// </summary>
    public static class PanelRegistry
    {
        private static readonly Dictionary<string, PanelDescriptor> s_descriptors =
            new(StringComparer.Ordinal);

        private static readonly List<string> s_registrationOrder = new();

        /// <summary>Register a descriptor. Idempotent — second registration for the same id is a no-op.</summary>
        public static void Register(PanelDescriptor descriptor)
        {
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));
            if (s_descriptors.ContainsKey(descriptor.Id)) return;
            s_descriptors[descriptor.Id] = descriptor;
            s_registrationOrder.Add(descriptor.Id);
        }

        /// <summary>Returns the descriptor for <paramref name="panelId"/>, or null if unknown.</summary>
        public static PanelDescriptor? Get(string panelId)
        {
            if (string.IsNullOrEmpty(panelId)) return null;
            s_descriptors.TryGetValue(panelId, out var d);
            return d;
        }

        /// <summary>Returns true when <paramref name="panelId"/> has a registered descriptor.</summary>
        public static bool IsRegistered(string panelId) => Get(panelId) != null;

        /// <summary>All registered ids in insertion order.</summary>
        public static IReadOnlyList<string> AllIds => s_registrationOrder;

        /// <summary>Count of registered panels.</summary>
        public static int Count => s_descriptors.Count;

        /// <summary>Clear all registrations (used in tests only).</summary>
        internal static void ClearForTest()
        {
            s_descriptors.Clear();
            s_registrationOrder.Clear();
        }

        /// <summary>
        /// Configure or override host action delegates on a registered descriptor.
        /// </summary>
        public static bool ConfigureActions(
            string panelId,
            Action? bindAction = null,
            Action? openAction = null,
            Action? closeAction = null,
            Func<bool>? availabilityRule = null)
        {
            var d = Get(panelId);
            if (d == null) return false;
            if (bindAction != null) d.BindAction = bindAction;
            if (openAction != null) d.OpenAction = openAction;
            if (closeAction != null) d.CloseAction = closeAction;
            if (availabilityRule != null) d.AvailabilityRule = availabilityRule;
            return true;
        }

        /// <summary>
        /// Resolve a route: returns the descriptor when found, or null when not.
        /// Use this in the host's OpenPlayerPanel to emit diagnostics on unknown routes.
        /// </summary>
        public static PanelDescriptor? Resolve(string panelId, Action<string>? onUnknown = null)
        {
            var d = Get(panelId);
            if (d == null)
            {
                string msg = $"[PanelRegistry] UNKNOWN ROUTE: '{panelId}' has no registered descriptor — this is a dead navigation target.";
                onUnknown?.Invoke(msg);
            }
            return d;
        }

        /// <summary>
        /// Attempts to navigate to and open a panel through the typed registry.
        /// Verifies registration and availability, invokes bind + open, and reports diagnostics on failure.
        /// </summary>
        public static bool TryOpen(string panelId, Action<string>? onDiagnostic = null, bool isMenu = false)
        {
            var d = Resolve(panelId, onDiagnostic);
            if (d == null) return false;

            if (isMenu && !d.AvailableInMenu)
            {
                string msg = $"[PanelRegistry] BLOCKED ROUTE: '{panelId}' is not available in main menu.";
                onDiagnostic?.Invoke(msg);
                return false;
            }

            if (d.AvailabilityRule != null && !d.AvailabilityRule())
            {
                string msg = $"[PanelRegistry] BLOCKED ROUTE: '{panelId}' availability condition not met.";
                onDiagnostic?.Invoke(msg);
                return false;
            }

            d.BindAction?.Invoke();
            d.OpenAction?.Invoke();
            return true;
        }

        /// <summary>
        /// Attempts to close a panel through its registered close action.
        /// </summary>
        public static bool TryClose(string panelId)
        {
            var d = Get(panelId);
            if (d == null) return false;
            d.CloseAction?.Invoke();
            return true;
        }
    }
}
