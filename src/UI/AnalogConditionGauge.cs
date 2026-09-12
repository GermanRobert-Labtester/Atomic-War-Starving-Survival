// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class AnalogConditionGauge : Control
    {
        public float Value { get; set; } = 100f; // 0 to 100
        public float WarningThreshold { get; set; } = 20f;
        public string LabelText { get; set; } = "CONDITION";

        private Font? _font;

        public override void _Ready()
        {
            CustomMinimumSize = new Vector2(100, 100);
            _font = ThemeDB.FallbackFont;
        }

        public override void _Draw()
        {
            Vector2 center = new Vector2(Size.X / 2, Size.Y - 10);
            float radius = Math.Min(Size.X, Size.Y) - 20;

            float startAngle = (float)Math.PI;
            float endAngle = 0;

            // Draw background arc
            DrawArc(center, radius, startAngle, endAngle, 32, AshfallUiHelpers.ToColor(DesignTheme.LineSoft), 4, true);

            // Draw warning arc
            float warningEndAngle = startAngle + ((float)Math.PI * (WarningThreshold / 100f));
            DrawArc(center, radius, startAngle, warningEndAngle, 16, AshfallUiHelpers.ToColor(DesignTheme.Critical), 6, true);

            // Draw value arc
            float valueEndAngle = startAngle + ((float)Math.PI * (Math.Clamp(Value, 0, 100) / 100f));
            Color valueColor = Value <= WarningThreshold ? AshfallUiHelpers.ToColor(DesignTheme.Critical) : AshfallUiHelpers.ToColor(DesignTheme.Success);
            DrawArc(center, radius, startAngle, valueEndAngle, 32, valueColor, 4, true);

            // Draw needle
            float needleAngle = startAngle + ((float)Math.PI * (Math.Clamp(Value, 0, 100) / 100f));
            Vector2 needleEnd = center + new Vector2((float)Math.Cos(needleAngle), (float)Math.Sin(needleAngle)) * (radius - 5);
            DrawLine(center, needleEnd, AshfallUiHelpers.ToColor(DesignTheme.Warm), 3, true);
            DrawCircle(center, 4, AshfallUiHelpers.ToColor(DesignTheme.Surface));

            // Draw label
            if (_font != null)
            {
                string text = $"{LabelText}\n{Value:F0}%";
                Vector2 textSize = _font.GetStringSize(text, HorizontalAlignment.Center, -1, 14);
                DrawMultilineString(_font, center + new Vector2(-textSize.X / 2, -radius - 15), text, HorizontalAlignment.Center, -1, 14, -1, AshfallUiHelpers.ToColor(DesignTheme.Pale));
            }
        }
    }
}
