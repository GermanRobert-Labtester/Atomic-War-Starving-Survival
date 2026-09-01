using System;
using System.IO;
using Godot;
using Ashfall.Core.Localization;

namespace AtomicWar.GodotApp.Localization
{
    /// <summary>
    /// Godot host bridge for ASHFALL localization.
    /// Synchronizes the engine's TranslationServer with Core's LocalizationService.
    /// Provides convenient Tr(key) and TrFormat(key, args) helpers for UI panels.
    /// </summary>
    public static class AshfallLocalization
    {
        private static bool _initialized;

        public static string CurrentLocale => LocalizationService.Instance.CurrentLocale;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            // Load CSV catalog if present in assets/l10n/
            LoadCatalogFromRes("res://assets/l10n/strings.csv");

            // Wire locale change
            LocalizationService.Instance.OnLocaleChanged += locale =>
            {
                if (locale != "pseudo")
                {
                    TranslationServer.SetLocale(locale);
                }
                GD.Print($"[AshfallLocalization] Active locale switched to: {locale}");
            };
        }

        public static void SetLocale(string locale)
        {
            Initialize();
            LocalizationService.Instance.SetLocale(locale);
        }

        public static string Tr(string key, string? defaultText = null)
        {
            Initialize();
            return LocalizationService.Instance.Get(key, defaultText);
        }

        public static string TrFormat(string key, params object[] args)
        {
            Initialize();
            return LocalizationService.Instance.Format(key, args);
        }

        private static void LoadCatalogFromRes(string resPath)
        {
            try
            {
                if (ResourceLoader.Exists(resPath))
                {
                    using var file = Godot.FileAccess.Open(resPath, Godot.FileAccess.ModeFlags.Read);
                    if (file != null)
                    {
                        string content = file.GetAsText();
                        LocalizationService.Instance.LoadFromCsv(content);
                        GD.Print($"[AshfallLocalization] Loaded string catalog from {resPath} ({LocalizationService.Instance.RegisteredKeyCount} keys).");
                    }
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[AshfallLocalization] Failed to load catalog from {resPath}: {ex.Message}");
            }
        }
    }
}
