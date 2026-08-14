using System;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Centralized design token repository for the ASHFALL UI system.
    /// Source of truth derived from the ASHFALL Design System &amp; Figma variables.
    /// Grim 2D survival aesthetic: muted desaturated palette, rust accents, diegetic textures.
    /// </summary>
    public static class Theme
    {
        public static class Colors
        {
            // Backgrounds
            public static readonly Color BackgroundPrimary = new Color(0.102f, 0.102f, 0.102f, 1.0f);   // #1A1A1A
            public static readonly Color BackgroundPanel   = new Color(0.173f, 0.173f, 0.173f, 1.0f);   // #2C2C2C
            public static readonly Color BackgroundModal   = new Color(0.078f, 0.078f, 0.078f, 0.97f);   // rgba(20,20,20,0.97)

            // Borders
            public static readonly Color BorderDefault = new Color(1.0f, 0.757f, 0.027f, 0.15f); // rgba(255,193,7,0.15)
            public static readonly Color BorderActive  = new Color(1.0f, 0.757f, 0.027f, 0.50f); // rgba(255,193,7,0.50)
            public static readonly Color BorderDanger  = new Color(0.957f, 0.263f, 0.212f, 0.50f); // rgba(244,67,54,0.50)

            // Typography & Indicators
            public static readonly Color TextPrimary   = new Color(0.878f, 0.878f, 0.878f, 1.0f);   // #E0E0E0
            public static readonly Color TextSecondary = new Color(0.620f, 0.620f, 0.620f, 1.0f);   // #9E9E9E
            public static readonly Color TextAccent    = new Color(1.0f, 0.757f, 0.027f, 1.0f);     // #FFC107 (Amber)
            public static readonly Color TextDanger    = new Color(0.957f, 0.263f, 0.212f, 1.0f);   // #F44336
            public static readonly Color TextSuccess   = new Color(0.298f, 0.686f, 0.314f, 1.0f);   // #4CAF50
            public static readonly Color RadiationGlow = new Color(0.0f, 0.737f, 0.831f, 1.0f);     // #00BCD4 (Cyan-green)

            // Progress & Meters
            public static readonly Color ProgressFill  = new Color(1.0f, 0.757f, 0.027f, 1.0f);     // #FFC107
            public static readonly Color ProgressBg    = new Color(1.0f, 0.757f, 0.027f, 0.10f);    // rgba(255,193,7,0.10)

            // Hex String Constants (for UXML / rich text / debug)
            public const string HexBackgroundPrimary = "#1A1A1A";
            public const string HexBackgroundPanel   = "#2C2C2C";
            public const string HexTextPrimary       = "#E0E0E0";
            public const string HexTextSecondary     = "#9E9E9E";
            public const string HexTextAccent        = "#FFC107";
            public const string HexTextDanger        = "#F44336";
            public const string HexTextSuccess       = "#4CAF50";
            public const string HexRadiationGlow     = "#00BCD4";
        }

        public static class Typography
        {
            public const int H1 = 28;
            public const int H2 = 22;
            public const int H3 = 18;
            public const int Body = 14;
            public const int Mono = 12;
            public const int Small = 11;
            public const int Label = 10;

            public const float H1LetterSpacing = 1.2f;
            public const float H2LetterSpacing = 1.0f;
            public const float H3LetterSpacing = 0.8f;
            public const float BodyLetterSpacing = 0.5f;
            public const float MonoLetterSpacing = 1.5f;
            public const float SmallLetterSpacing = 0.3f;
            public const float LabelLetterSpacing = 0.8f;
        }

        public static class Spacing
        {
            public const float Xs = 4f;
            public const float Sm = 8f;
            public const float Md = 12f;
            public const float Lg = 16f;
            public const float Xl = 24f;
        }

        public static class Radius
        {
            public const float Sm = 2f;
            public const float Md = 4f;
            public const float Lg = 8f;
        }

        public static class AssetPaths
        {
            public const string PanelBackground9Slice = "Assets/UI/Textures/panel_bg_9slice.png";
            public const string HeaderBar9Slice       = "Assets/UI/Textures/header_bar_9slice.png";

            public const string BioBloodIcon  = "Assets/UI/Icons/icon_bio_blood.png";
            public const string BioMarrowIcon = "Assets/UI/Icons/icon_bio_marrow.png";
            public const string BioPlasmaIcon = "Assets/UI/Icons/icon_bio_plasma.png";
            public const string BioOrganIcon  = "Assets/UI/Icons/icon_bio_organ.png";

            public const string BadgeCritical = "Assets/UI/Icons/badge_scarcity_critical.png";
            public const string BadgeHigh     = "Assets/UI/Icons/badge_scarcity_high.png";
            public const string BadgeModerate = "Assets/UI/Icons/badge_scarcity_moderate.png";
            public const string BadgeLow      = "Assets/UI/Icons/badge_scarcity_low.png";

            public const string ShockPlumeIcon  = "Assets/UI/Icons/icon_shock_plume.png";
            public const string ShockConvoyIcon = "Assets/UI/Icons/icon_shock_convoy.png";
            public const string ShockWarIcon    = "Assets/UI/Icons/icon_shock_war.png";
            public const string ShockWinterIcon = "Assets/UI/Icons/icon_shock_winter.png";
        }
    }
}
