// Engine-agnostic design tokens for the Ashfall diegetic UI.
// Mirrors the USS :root variables in DiegeticHud.uss and MainMenu.uss.
// Both Unity and Godot hosts reference these constants for programmatic
// UI construction — zero UnityEngine / Godot references.

namespace Ashfall.Core.UI
{
    /// <summary>
    /// Standardized severity vocabulary for UI alerts, telemetry readouts, and badge styling.
    /// </summary>
    public enum SeverityLevel
    {
        Normal,
        Attention,
        Dangerous,
        Critical,
        Unavailable
    }

    /// <summary>
    /// Ashfall design tokens — colors, spacing, typography sizes.
    /// Extracted from the Figma Make prototype and ported to USS.
    /// All values are authoritative; USS :root blocks must match.
    /// </summary>
    public static class Theme
    {
        // ── Colors (hex RGBA) ───────────────────────────────────────────
        // Base palette — matches DiegeticHud.uss :root.

        /// <summary>Near-black background. #090b0c.</summary>
        public const string InkHex = "#090B0C";
        public static readonly (float r, float g, float b, float a) Ink = (0.035f, 0.043f, 0.047f, 1f);

        /// <summary>Semi-transparent panel background. rgba(9,11,12,0.86).</summary>
        public static readonly (float r, float g, float b, float a) InkPanel = (0.035f, 0.043f, 0.047f, 0.86f);

        /// <summary>Default border — warm tan at 27% opacity.</summary>
        public static readonly (float r, float g, float b, float a) Line = (0.851f, 0.769f, 0.596f, 0.27f);

        /// <summary>Soft border — warm tan at 14% opacity.</summary>
        public static readonly (float r, float g, float b, float a) LineSoft = (0.851f, 0.769f, 0.596f, 0.14f);

        /// <summary>Primary accent — warm amber. #d3aa62.</summary>
        public const string WarmHex = "#D3AA62";
        public static readonly (float r, float g, float b, float a) Warm = (0.827f, 0.667f, 0.384f, 1f);

        /// <summary>Highlight / emphasis — hot amber. #f4c875.</summary>
        public const string HotHex = "#F4C875";
        public static readonly (float r, float g, float b, float a) Hot = (0.957f, 0.784f, 0.459f, 1f);

        /// <summary>Primary text — pale bone. #e6e0d2.</summary>
        public const string PaleHex = "#C7DCD0"; // Flagship Phosphor Green
        public static readonly (float r, float g, float b, float a) Pale = (0.902f, 0.878f, 0.824f, 1f);

        /// <summary>Secondary text — muted grey. #938f84.</summary>
        public const string MutedHex = "#938F84";
        public static readonly (float r, float g, float b, float a) Muted = (0.576f, 0.561f, 0.518f, 1f);

        /// <summary>Tertiary / disabled text — dim grey. #66675f.</summary>
        public const string DimHex = "#66675F";
        public static readonly (float r, float g, float b, float a) Dim = (0.400f, 0.404f, 0.373f, 1f);

        /// <summary>Military exclusive — copper rust. #c4785a.</summary>
        public const string ExclusiveHex = "#C4785A";
        public static readonly (float r, float g, float b, float a) Exclusive = (0.769f, 0.471f, 0.353f, 1f);

        /// <summary>Danger / critical — restrained red. #e63333.</summary>
        public const string CriticalHex = "#E63333";
        public static readonly (float r, float g, float b, float a) Critical = (0.902f, 0.200f, 0.200f, 1f);

        // ── Semantic Design Tokens ───────────────────────────────────────
        public const string SurfaceHex = "#050709"; // Deep Black
        public static readonly (float r, float g, float b, float a) Surface = (0.055f, 0.067f, 0.078f, 1f);

        public const string SurfaceCardHex = "#090B0D"; // Deep Black Card
        public static readonly (float r, float g, float b, float a) SurfaceCard = (0.078f, 0.098f, 0.118f, 1f);

        public static readonly (float r, float g, float b, float a) BackdropOverlay = (0.031f, 0.039f, 0.047f, 0.94f);

        public const string SelectedBgHex = "#282319";
        public static readonly (float r, float g, float b, float a) SelectedBg = (0.157f, 0.137f, 0.098f, 1f);

        public const string HoverBgHex = "#1D2228";
        public static readonly (float r, float g, float b, float a) HoverBg = (0.114f, 0.133f, 0.157f, 1f);

        public const string SuccessHex = "#5CD670";
        public static readonly (float r, float g, float b, float a) Success = (0.361f, 0.839f, 0.439f, 1f);

        public const string WarningHex = "#FF6B35"; // Flagship Hazard Orange
        public static readonly (float r, float g, float b, float a) Warning = (0.788f, 0.482f, 0.227f, 1f);

        public const string RadiationHex = "#D9A026";
        public static readonly (float r, float g, float b, float a) Radiation = (0.851f, 0.627f, 0.149f, 1f);

        public const string RadiationAcuteHex = "#E65C2B";
        public static readonly (float r, float g, float b, float a) RadiationAcute = (0.902f, 0.361f, 0.169f, 1f);

        public const string InfoHex = "#6EA3A8";
        public static readonly (float r, float g, float b, float a) Info = (0.431f, 0.639f, 0.659f, 1f);

        // ── Expansion IV tokens ─────────────────────────────────────────

        /// <summary>Structural entropy — corroded amber. #c97b3a.</summary>
        public const string EntropyHex = "#C97B3A";
        public static readonly (float r, float g, float b, float a) Entropy = (0.788f, 0.482f, 0.227f, 1f);

        /// <summary>Lethe protocol — faded cyan-grey. #6ea3a8.</summary>
        public const string LetheHex = "#6EA3A8";
        public static readonly (float r, float g, float b, float a) Lethe = (0.431f, 0.639f, 0.659f, 1f);

        /// <summary>Ozone scourge — bleached white. #dde8e8.</summary>
        public const string OzoneHex = "#DDE8E8";
        public static readonly (float r, float g, float b, float a) Ozone = (0.867f, 0.910f, 0.910f, 1f);

        /// <summary>Memory flash — near-black with warm cast.</summary>
        public static readonly (float r, float g, float b, float a) Ghost = (0.035f, 0.043f, 0.047f, 0.92f);

        /// <summary>Entropy glow overlay.</summary>
        public static readonly (float r, float g, float b, float a) EntropyGlow = (0.788f, 0.482f, 0.227f, 0.18f);

        /// <summary>Lethe amber — sight-gauge fill.</summary>
        public const string LetheAmberHex = "#D4A35A";
        public static readonly (float r, float g, float b, float a) LetheAmber = (0.831f, 0.639f, 0.353f, 1f);

        /// <summary>Lethe redline.</summary>
        public const string LetheRedHex = "#D94040";
        public static readonly (float r, float g, float b, float a) LetheRed = (0.851f, 0.251f, 0.251f, 1f);

        // ── Spacing (px) ────────────────────────────────────────────────

        public const int HudEdge = 24;
        public const int HudPanelPadding = 12;

        public const int SpacingXs = 4;
        public const int SpacingSm = 8;
        public const int SpacingMd = 12;
        public const int SpacingLg = 16;
        public const int SpacingXl = 24;

        // ── Corner radius (px) ──────────────────────────────────────────

        public const int RadiusSm = 0; // Brutalist Sharp
        public const int RadiusMd = 0; // Brutalist Sharp
        public const int RadiusLg = 0; // Brutalist Sharp

        // ── Typography sizes (px) ───────────────────────────────────────

        public const int FontSizeH1 = 30;
        public const int FontSizeH2 = 24;
        public const int FontSizeH3 = 19;
        public const int FontSizeBody = 15;
        public const int FontSizeSmall = 12;
        public const int FontSizeMono = 13;
        public const int FontSizeLabel = 11;

        // ── Diegetic HUD typography (matches USS) ───────────────────────

        public const int DiegeticTitleSize = 13;
        public const int DiegeticStatusSize = 12;
        public const int DiegeticBodySize = 12;
        public const int DiegeticHintSize = 11;

        // ── Panel sizing (px) ───────────────────────────────────────────

        public const int PanelMaxWidth = 420;
        public const int PanelMinWidthNarrow = 260;
        public const int PanelMinWidthStandard = 340;
        public const int PanelMinWidthWide = 400;
        public const int PanelMaxWidthWide = 520;

        // ── Trade screen sizing (px) ────────────────────────────────────

        public const int TradePanelMinWidth = 560;
        public const int TradePanelMaxWidth = 720;
        public const int TradePanelMaxHeight = 600;
        public const int TradeColumnMinWidth = 240;

        // ── Economy HUD sizing (px) ─────────────────────────────────────

        public const int EconomyStripWidth = 220;
        public const int EconomyStripHeight = 32;
        public const int EconomyPanelMinWidth = 380;
        public const int EconomyPanelMaxWidth = 500;
        public const int EconomyPanelMaxHeight = 480;
    }
}
