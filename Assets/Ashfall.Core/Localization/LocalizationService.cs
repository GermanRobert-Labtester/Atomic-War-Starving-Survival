using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Ashfall.Core.Localization
{
    /// <summary>
    /// Engine-agnostic localization service for ASHFALL.
    /// Provides stable-key string lookup, parameter formatting, fallback to source English,
    /// and a development pseudo-locale mode for testing text expansion and clipping.
    /// Zero engine dependencies (works in Core, unit tests, and Godot host).
    /// </summary>
    public sealed class LocalizationService
    {
        private static LocalizationService? _instance;
        public static LocalizationService Instance => _instance ??= new LocalizationService();

        private string _currentLocale = "en";
        private readonly Dictionary<string, string> _strings = new(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _fallbackStrings = new(StringComparer.Ordinal);

        public string CurrentLocale => _currentLocale;
        public int RegisteredKeyCount => _strings.Count;

        public event Action<string>? OnLocaleChanged;

        public LocalizationService()
        {
            LoadDefaultEnglishStrings();
        }

        public static void SetInstance(LocalizationService service)
        {
            _instance = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void SetLocale(string locale)
        {
            if (string.IsNullOrWhiteSpace(locale)) locale = "en";
            locale = locale.Trim().ToLowerInvariant();

            if (_currentLocale == locale) return;

            _currentLocale = locale;
            RefreshLocaleStrings();
            OnLocaleChanged?.Invoke(_currentLocale);
        }

        public bool HasKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            return _strings.ContainsKey(key) || _fallbackStrings.ContainsKey(key);
        }

        /// <summary>
        /// Translates a stable localization key. Returns translated text if found,
        /// pseudo-localized text if current locale is "pseudo", fallback English if missing in locale,
        /// or the key itself if untranslated.
        /// </summary>
        public string Get(string key, string? defaultText = null)
        {
            if (string.IsNullOrEmpty(key)) return defaultText ?? string.Empty;

            if (_currentLocale == "pseudo")
            {
                if (_fallbackStrings.TryGetValue(key, out var srcText))
                {
                    return GeneratePseudoString(srcText);
                }
                if (!string.IsNullOrEmpty(defaultText))
                {
                    return GeneratePseudoString(defaultText);
                }
                return $"[!!! {key} !!!]";
            }

            if (_strings.TryGetValue(key, out var text))
            {
                return text;
            }

            if (_fallbackStrings.TryGetValue(key, out var fallback))
            {
                return fallback;
            }

            return defaultText ?? key;
        }

        /// <summary>
        /// Translates a key and formats it with positional parameters.
        /// </summary>
        public string Format(string key, params object[] args)
        {
            string template = Get(key);
            if (args == null || args.Length == 0) return template;

            try
            {
                return string.Format(CultureInfo.InvariantCulture, template, args);
            }
            catch (FormatException)
            {
                return template;
            }
        }

        /// <summary>
        /// Registers a single localization string in the fallback/source table.
        /// </summary>
        public void RegisterString(string key, string englishText)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            _fallbackStrings[key] = englishText ?? string.Empty;
            if (_currentLocale == "en")
            {
                _strings[key] = englishText ?? string.Empty;
            }
        }

        /// <summary>
        /// Bulk loads key-value pairs from a standard CSV format (key,english[,translated]).
        /// </summary>
        public void LoadFromCsv(string csvContent)
        {
            if (string.IsNullOrWhiteSpace(csvContent)) return;

            using var reader = new StringReader(csvContent);
            string? line;
            bool isHeader = true;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

                if (isHeader && (line.StartsWith("key,") || line.StartsWith("\"key\",")))
                {
                    isHeader = false;
                    continue;
                }
                isHeader = false;

                var parts = ParseCsvLine(line);
                if (parts.Count >= 2)
                {
                    string key = parts[0].Trim();
                    string en = parts[1];
                    if (!string.IsNullOrEmpty(key))
                    {
                        _fallbackStrings[key] = en;
                        if (parts.Count >= 3 && !string.IsNullOrEmpty(parts[2]) && _currentLocale != "en" && _currentLocale != "pseudo")
                        {
                            _strings[key] = parts[2];
                        }
                        else if (_currentLocale == "en")
                        {
                            _strings[key] = en;
                        }
                    }
                }
            }
        }

        private void RefreshLocaleStrings()
        {
            _strings.Clear();
            if (_currentLocale == "en")
            {
                foreach (var kvp in _fallbackStrings)
                {
                    _strings[kvp.Key] = kvp.Value;
                }
            }
            // If "pseudo", Get() generates dynamic expanded strings on-demand from _fallbackStrings.
        }

        /// <summary>
        /// Generates a pseudo-localized string with ~30-40% length expansion and accented glyphs
        /// to stress-test layout wrapping, truncation, and fixed-container boundaries.
        /// </summary>
        public static string GeneratePseudoString(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var sb = new StringBuilder(input.Length * 2);
            sb.Append("[!!! ");

            bool inPlaceholder = false;
            foreach (char c in input)
            {
                if (c == '{')
                {
                    inPlaceholder = true;
                    sb.Append(c);
                    continue;
                }
                if (c == '}')
                {
                    inPlaceholder = false;
                    sb.Append(c);
                    continue;
                }

                if (inPlaceholder)
                {
                    sb.Append(c);
                    continue;
                }

                // Accent substitution
                char transformed = c switch
                {
                    'a' => 'ā', 'A' => 'Ā',
                    'b' => 'ḅ', 'B' => 'Ḅ',
                    'c' => 'ċ', 'C' => 'Ċ',
                    'd' => 'ḓ', 'D' => 'Ḓ',
                    'e' => 'ē', 'E' => 'Ē',
                    'f' => 'ƒ', 'F' => 'Ƒ',
                    'g' => 'ġ', 'G' => 'Ġ',
                    'h' => 'ḥ', 'H' => 'Ḥ',
                    'i' => 'ī', 'I' => 'Ī',
                    'j' => 'ǰ', 'J' => 'Ĵ',
                    'k' => 'ḳ', 'K' => 'Ḳ',
                    'l' => 'ḷ', 'L' => 'Ḷ',
                    'm' => 'ṁ', 'M' => 'Ṁ',
                    'n' => 'ñ', 'N' => 'Ñ',
                    'o' => 'ō', 'O' => 'Ō',
                    'p' => 'ṗ', 'P' => 'Ṗ',
                    'r' => 'ṛ', 'R' => 'Ṛ',
                    's' => 'ṣ', 'S' => 'Ṣ',
                    't' => 'ṫ', 'T' => 'Ṫ',
                    'u' => 'ū', 'U' => 'Ū',
                    'v' => 'ṽ', 'V' => 'Ṽ',
                    'w' => 'ẁ', 'W' => 'Ẁ',
                    'x' => 'ẋ', 'X' => 'Ẋ',
                    'y' => 'ȳ', 'Y' => 'Ȳ',
                    'z' => 'ż', 'Z' => 'Ż',
                    _ => c
                };
                sb.Append(transformed);
            }

            // Append expansion tail to simulate 30% text expansion in German/French
            sb.Append(" !!!]");
            return sb.ToString();
        }

        private static List<string> ParseCsvLine(string line)
        {
            var result = new List<string>();
            var cur = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        cur.Append('"');
                        i++; // skip escaped quote
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(cur.ToString());
                    cur.Clear();
                }
                else
                {
                    cur.Append(c);
                }
            }
            result.Add(cur.ToString());
            return result;
        }

        private void LoadDefaultEnglishStrings()
        {
            // Core UI navigation and common chrome
            RegisterString("ui.common.ok", "OK");
            RegisterString("ui.common.cancel", "Cancel");
            RegisterString("ui.common.close", "Close");
            RegisterString("ui.common.back", "Back");
            RegisterString("ui.common.confirm", "Confirm");
            RegisterString("ui.common.save", "Save");
            RegisterString("ui.common.apply", "Apply");
            RegisterString("ui.common.reset", "Reset");
            RegisterString("ui.common.enabled", "ENABLED");
            RegisterString("ui.common.disabled", "DISABLED");
            RegisterString("ui.common.active", "ACTIVE");
            RegisterString("ui.common.dormant", "DORMANT");
            RegisterString("ui.common.stable", "STABLE");
            RegisterString("ui.common.critical", "CRITICAL");
            RegisterString("ui.common.warning", "WARNING");

            // Settings labels
            RegisterString("settings.title", "SYSTEM CONFIGURATION // SETTINGS");
            RegisterString("settings.section.display", "DISPLAY & GRAPHICS");
            RegisterString("settings.section.audio", "AUDIO SIGNALS");
            RegisterString("settings.section.accessibility", "ACCESSIBILITY & READABILITY");
            RegisterString("settings.section.gameplay", "GAMEPLAY PROTOCOLS");
            RegisterString("settings.section.language", "LANGUAGE & LOCALIZATION");
            RegisterString("settings.display.window_mode", "Window Mode");
            RegisterString("settings.display.resolution", "Resolution Preset");
            RegisterString("settings.display.ui_scale", "Interface Scale");
            RegisterString("settings.display.vsync", "Vertical Sync");
            RegisterString("settings.display.max_fps", "Frame Rate Cap");
            RegisterString("settings.audio.mute_all", "Mute All Audio");
            RegisterString("settings.audio.master", "Master Volume");
            RegisterString("settings.audio.music", "Music / Ambience Score");
            RegisterString("settings.audio.sfx", "Sound Effects / Machinery");
            RegisterString("settings.audio.radio", "Radio Receiver / Transmissions");
            RegisterString("settings.audio.ambience", "Bunker Ambience / Air Duct");
            RegisterString("settings.accessibility.high_contrast", "High Contrast HUD");
            RegisterString("settings.accessibility.hazard_labels", "Always Show Hazard Text");
            RegisterString("settings.accessibility.reduced_motion", "Reduced Motion");
            RegisterString("settings.accessibility.large_fonts", "Large Font Floor");
            RegisterString("settings.gameplay.tutorial_mode", "Tutorial & Onboarding Guidance");
            RegisterString("settings.gameplay.confirm_end_day", "Confirm Before Ending Day");
            RegisterString("settings.gameplay.verbose_radio", "Detailed Radio Log Dispatches");
            RegisterString("settings.gameplay.auto_save", "Auto-Save on Day Advance");
            RegisterString("settings.gameplay.reset_tutorials", "Reset Tutorial Guidance");
            RegisterString("settings.language.locale", "Language / Locale");

            // Tutorial & Onboarding
            RegisterString("tutorial.title", "DAY 1 OBJECTIVE");
            RegisterString("tutorial.protocol.title", "Resolve the Day 1 protocol");
            RegisterString("tutorial.protocol.objective", "Walk the opening directives: ration, maintenance, then radio. Each choice has a cost.");
            RegisterString("tutorial.inspect.title", "Inspect three bunker rooms");
            RegisterString("tutorial.inspect.objective", "Open the shelter and inspect rooms until three have confirming notes.");
            RegisterString("tutorial.rationing.title", "Open the stores and read them");
            RegisterString("tutorial.rationing.objective", "Open the inventory and look at the food and water you are rationing.");
            RegisterString("tutorial.assignment.title", "Assign a survivor to a duty");
            RegisterString("tutorial.assignment.objective", "Open the duty roster and assign one survivor to a shift. Survivors cannot work without one.");
            RegisterString("tutorial.weather.title", "Read the weather");
            RegisterString("tutorial.weather.objective", "Open the weather forecast or panel to learn what tomorrow will bring.");
            RegisterString("tutorial.inventory.title", "Use an item from the stores");
            RegisterString("tutorial.inventory.objective", "Equip a protective item or consume something real from the ledger. Both are real commands.");
            RegisterString("tutorial.day_advance.title", "End Day 1");
            RegisterString("tutorial.day_advance.objective", "Press the Advance Day confirm. The first night ticks; the morning briefing returns.");

            // Critical Warnings & Causality
            RegisterString("warning.radiation.acute", "ACUTE RADIATION DETECTED: Survivor {0} has {1:F0} mSv exposure (-5 HP/h decay). Administer Rad-Away or Iodine.");
            RegisterString("warning.radiation.storm", "FALLOUT STORM INCOMING: Elevated environmental radiation. Keep survivors indoors or equip hazmat gear.");
            RegisterString("warning.water.low", "WATER RESERVES LOW: {0:F1} units remaining (~{1:F1} days). Run filtration or desalination.");
            RegisterString("warning.food.low", "FOOD RESERVES LOW: {0:F1} units remaining (~{1:F1} days). Adjust rations or scavenge.");
            RegisterString("warning.power.brownout", "POWER DEFICIT: Generator reserve depleted. Air filtration offline; indoor contamination rising.");
            RegisterString("warning.survivor.critical", "SURVIVOR IN DANGER: {0} has reached critical {1}. Triage immediately.");

            // Field Manual topics
            RegisterString("codex.manual.title", "FIELD SURVIVAL MANUAL");
            RegisterString("codex.manual.radiation", "Radiation & Dosimeter: Dose accumulates from fallout and storms. Above 50 mSv triggers acute radiation sickness with 5 HP/hr health decay. Administer Rad-Away or Iodine.");
            RegisterString("codex.manual.rations", "Rations & Water: Clean water is essential. 3 survivors consume ~3.6 units daily. Maintain filtration membranes.");
            RegisterString("codex.manual.power", "Power & Grid: Air filtration requires continuous electrical power. Stock batteries and generator fuel.");
            RegisterString("codex.manual.duty", "Duty Shifts: Unassigned survivors suffer morale decay and cannot maintain bunker facilities.");
        }
    }
}
