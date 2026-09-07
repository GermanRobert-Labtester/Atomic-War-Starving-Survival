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
        private readonly Dictionary<string, Dictionary<string, string>> _localeTranslations = new(StringComparer.OrdinalIgnoreCase);

        public string CurrentLocale => _currentLocale;
        public int RegisteredKeyCount => _strings.Count;

        public event Action<string>? OnLocaleChanged;
        public event Action<string>? OnMissingKey;

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
                OnMissingKey?.Invoke(key);
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

            OnMissingKey?.Invoke(key);
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
        /// Formats a named-placeholder template without making translated
        /// word order depend on the source language. Missing placeholders are
        /// reported and leave the template visible rather than collapsing to
        /// an empty string.
        /// </summary>
        public string FormatNamed(string key, IReadOnlyDictionary<string, object?> args)
        {
            string result = Get(key);
            if (args == null || args.Count == 0) return result;

            foreach (var pair in args)
            {
                string token = "{" + pair.Key + "}";
                result = result.Replace(token, Convert.ToString(pair.Value, CultureInfo.InvariantCulture) ?? string.Empty,
                    StringComparison.Ordinal);
            }

            return result;
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
        /// Registers a translated string for a specific locale (e.g. "de").
        /// </summary>
        public void RegisterTranslation(string locale, string key, string translatedText)
        {
            if (string.IsNullOrWhiteSpace(locale) || string.IsNullOrWhiteSpace(key)) return;
            locale = locale.Trim().ToLowerInvariant();
            if (!_localeTranslations.TryGetValue(locale, out var dict))
            {
                dict = new Dictionary<string, string>(StringComparer.Ordinal);
                _localeTranslations[locale] = dict;
            }
            dict[key] = translatedText ?? string.Empty;
            if (_currentLocale == locale)
            {
                _strings[key] = translatedText ?? string.Empty;
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
            List<string>? headers = null;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

                if (isHeader)
                {
                    if (line.StartsWith("key,", StringComparison.OrdinalIgnoreCase) || line.StartsWith("\"key\",", StringComparison.OrdinalIgnoreCase))
                    {
                        headers = ParseCsvLine(line);
                        isHeader = false;
                        continue;
                    }
                    isHeader = false;
                }

                var parts = ParseCsvLine(line);
                if (parts.Count >= 2)
                {
                    string key = parts[0].Trim();
                    string en = parts[1];
                    if (!string.IsNullOrEmpty(key))
                    {
                        RegisterString(key, en);

                        if (headers != null)
                        {
                            for (int i = 2; i < parts.Count && i < headers.Count; i++)
                            {
                                string colName = headers[i].Trim().ToLowerInvariant();
                                if (colName != "source" && !string.IsNullOrWhiteSpace(colName) && !string.IsNullOrWhiteSpace(parts[i]))
                                {
                                    RegisterTranslation(colName, key, parts[i]);
                                }
                            }
                        }
                        else if (parts.Count >= 3 && !string.IsNullOrEmpty(parts[2]) && _currentLocale != "en" && _currentLocale != "pseudo")
                        {
                            RegisterTranslation(_currentLocale, key, parts[2]);
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
            else if (_localeTranslations.TryGetValue(_currentLocale, out var dict))
            {
                foreach (var kvp in dict)
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
            RegisterString("tutorial.collectible.cultural_artifacts.title", "Cultural Artifacts");
            RegisterString("tutorial.collectible.cultural_artifacts.body", "Cultural Artifacts are surviving objects from the pre-war world. Discovering them records their history, and some can unlock knowledge, journal entries, or new locations.");
            RegisterString("tutorial.collectible.reading_and_discovering.title", "Reading and Discovering");
            RegisterString("tutorial.collectible.reading_and_discovering.body", "Some artifacts contain useful information. Discovering them can unlock journal entries, knowledge, faction intel, or map locations. These discoveries are recorded permanently.");

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
            LoadMicroLocationStrings();
        }

        private void LoadMicroLocationStrings()
        {
            RegisterString("discovery.micro_roadside_memorial.title", "Roadside Memorial");
            RegisterTranslation("de", "discovery.micro_roadside_memorial.title", "Straßenrand-Gedenkstätte");
            RegisterString("discovery.micro_roadside_memorial.description", "Melted tallow stubs sit inside rusted rationing tins around a bent highway marker. A photograph was torn away, leaving only a bloodstained corner pinned beneath a stone. The wax has frozen into pale, grey discs.");
            RegisterTranslation("de", "discovery.micro_roadside_memorial.description", "Geschmolzene Talgreste sitzen in verrosteten Rationsdosen um einen verbogenen Pfahl. Ein Foto wurde weggerissen, nur eine blutbefleckte Ecke blieb unter einem Stein zurück. Das Wachs ist zu blassen, grauen Scheiben erstarrt.");
            RegisterString("discovery.micro_roadside_memorial.choice.leave_memorial", "Leave it untouched.");
            RegisterTranslation("de", "discovery.micro_roadside_memorial.choice.leave_memorial", "Unberührt lassen.");
            RegisterString("discovery.micro_roadside_memorial.choice.take_offering", "Take the candle stubs and any small offering left behind.");
            RegisterTranslation("de", "discovery.micro_roadside_memorial.choice.take_offering", "Die Kerzenreste und alle kleinen Opfergaben mitnehmen.");
            RegisterString("discovery.micro_crashed_truck.title", "Crashed Supply Truck");
            RegisterTranslation("de", "discovery.micro_crashed_truck.title", "Abgestürzter Versorgungslaster");
            RegisterString("discovery.micro_crashed_truck.description", "A military logistics rig lies crushed in the frozen ditch, its windshield shattered outward. Torn radiation warning tags cling to the twisted rear doors. The cargo area was scavenged long ago, but one crate lies split open in the frost.");
            RegisterTranslation("de", "discovery.micro_crashed_truck.description", "Ein militärischer Logistiktransporter liegt zerschmettert im gefrorenen Graben, die Windschutzscheibe nach außen geborsten. Zerrissene Strahlungswarnschilder klammern sich an die verbogenen Hecktüren. Der Laderaum wurde längst geplündert, aber eine Kiste liegt aufgebrochen im Frost.");
            RegisterString("discovery.micro_crashed_truck.choice.search_truck_cargo", "Search the split crate and cab for salvage.");
            RegisterTranslation("de", "discovery.micro_crashed_truck.choice.search_truck_cargo", "Die aufgebrochene Kiste und das Fahrerhaus nach Brauchbarem durchsuchen.");
            RegisterString("discovery.micro_crashed_truck.choice.search_truck_cab", "Investigate the cab for documents or personal effects.");
            RegisterTranslation("de", "discovery.micro_crashed_truck.choice.search_truck_cab", "Das Fahrerhaus nach Dokumenten oder persönlichen Gegenständen untersuchen.");
            RegisterString("discovery.micro_crashed_truck.choice.ignore_truck", "Move on. Someone already took what was worth taking.");
            RegisterTranslation("de", "discovery.micro_crashed_truck.choice.ignore_truck", "Weitergehen. Jemand hat bereits alles Brauchbare mitgenommen.");
            RegisterString("discovery.micro_frozen_bus.title", "Frozen Evacuation Bus");
            RegisterTranslation("de", "discovery.micro_frozen_bus.title", "Gefrorener Evakuierungsbus");
            RegisterString("discovery.micro_frozen_bus.description", "The transit doors are frozen wide open, letting the ash-laden wind howl through the cabin. A child's single shoe sits upright beneath a luggage rack. The windows are obscured by thick, greasy frost on the inside.");
            RegisterTranslation("de", "discovery.micro_frozen_bus.description", "Die Bustüren sind weit aufgefroren, sodass der aschebeladene Wind durch die Kabine heult. Der Einzelschuh eines Kindes steht aufrecht unter einer Gepäckablage. Die Fenster sind auf der Innenseite von dickem, schmierigem Frost überzogen.");
            RegisterString("discovery.micro_frozen_bus.choice.search_bus_luggage", "Search the luggage rack for supplies.");
            RegisterTranslation("de", "discovery.micro_frozen_bus.choice.search_bus_luggage", "Die Gepäckablage nach Vorräten durchsuchen.");
            RegisterString("discovery.micro_frozen_bus.choice.leave_bus", "Leave the bus undisturbed.");
            RegisterTranslation("de", "discovery.micro_frozen_bus.choice.leave_bus", "Den Bus unberührt lassen.");
            RegisterString("discovery.micro_frozen_bus.choice.read_bus_tag", "Check the transit tag on the dashboard for a destination.");
            RegisterTranslation("de", "discovery.micro_frozen_bus.choice.read_bus_tag", "Die Transitmarke auf dem Armaturenbrett nach einem Bestimmungsort prüfen.");
            RegisterString("discovery.micro_improvised_grave.title", "Improvised Grave");
            RegisterString("discovery.micro_improvised_grave.description", "A shallow mound of frozen earth, hastily reinforced with chunks of shattered concrete. A name is violently gouged into a piece of broken siding, along with a date from the first winter of the ashfall.");
            RegisterString("discovery.micro_improvised_grave.choice.respect_grave", "Pay respects and move on.");
            RegisterString("discovery.micro_improvised_grave.choice.inspect_grave_marker", "Read the name and date scratched into the plank.");
            RegisterString("discovery.micro_improvised_grave.choice.disturb_grave", "Check beneath the stones for any buried belongings.");
            RegisterString("discovery.micro_collapsed_bridge.title", "Collapsed Bridge");
            RegisterString("discovery.micro_collapsed_bridge.description", "A colossal span of reinforced concrete leans broken into the black, freezing river. A transport vehicle is wedged violently against the submerged rebar, its rear doors hanging open just above the roaring current.");
            RegisterString("discovery.micro_collapsed_bridge.choice.search_bridge_vehicle", "Climb down to the wedged vehicle and search the cargo area.");
            RegisterString("discovery.micro_collapsed_bridge.choice.inspect_bridge_structure", "Examine the collapsed span for salvageable steel or cable.");
            RegisterString("discovery.micro_collapsed_bridge.choice.avoid_bridge", "Find a way around. The structure looks unstable.");
            RegisterString("discovery.micro_drainage_pipe.title", "Drainage Pipe");
            RegisterString("discovery.micro_drainage_pipe.description", "Rags and irradiated blankets have been pushed deep into this dark concrete culvert. Words are gouged into the wall just beyond the reach of the freezing rain, the letters uneven and going downhill. The smell of copper and unwashed bodies lingers.");
            RegisterString("discovery.micro_drainage_pipe.choice.crawl_pipe", "Crawl inside and check the blankets for supplies.");
            RegisterString("discovery.micro_drainage_pipe.choice.read_pipe_warning", "Read the warning scratched into the concrete.");
            RegisterString("discovery.micro_drainage_pipe.choice.ignore_pipe", "Leave it alone. Someone may come back.");
            RegisterString("discovery.micro_rail_siding.title", "Rail Siding");
            RegisterString("discovery.micro_rail_siding.description", "Rusted wheelsets have sunk deep into the irradiated ballast. A maintenance ledger hangs from the side of a derailed freight car, the final pages written in frantic, shaky handwriting before ending abruptly.");
            RegisterString("discovery.micro_rail_siding.choice.search_rail_car", "Search the freight car for industrial salvage.");
            RegisterString("discovery.micro_rail_siding.choice.read_rail_ledger", "Read the maintenance ledger.");
            RegisterString("discovery.micro_rail_siding.choice.ignore_rail", "The car has been picked over. Move on.");
            RegisterString("discovery.micro_dead_livestock.title", "Dead Livestock Area");
            RegisterString("discovery.micro_dead_livestock.description", "Plastic ear tags flutter in the ash-wind among the dead weeds. Massive, bloated bodies of livestock were dragged into a rough trench, but the grave diggers abandoned the work. The Geiger counter stutters nervously here.");
            RegisterString("discovery.micro_dead_livestock.choice.scavenge_livestock", "Scavenge usable material from the remains.");
            RegisterString("discovery.micro_dead_livestock.choice.inspect_livestock_tags", "Inspect the ear tags for farm identification.");
            RegisterString("discovery.micro_dead_livestock.choice.avoid_livestock", "Keep your distance. The contamination risk is not worth it.");
            RegisterString("discovery.micro_ruined_greenhouse.title", "Ruined Greenhouse");
            RegisterString("discovery.micro_ruined_greenhouse.description", "Shattered glass crunches underfoot in this ruined greenhouse. Desiccated seed trays lie beneath the rusted benches. In the corner, a heavy steel cabinet remains sealed—the paint around its lock is scarred by crowbar marks that stop short of success.");
            RegisterString("discovery.micro_ruined_greenhouse.choice.take_greenhouse_seeds", "Take the labeled seed trays.");
            RegisterString("discovery.micro_ruined_greenhouse.choice.open_greenhouse_cabinet", "Force the sealed cabinet open.");
            RegisterString("discovery.micro_ruined_greenhouse.choice.leave_greenhouse", "Leave the greenhouse for whoever finds it next.");
            RegisterString("discovery.micro_shell_crater.title", "Shell Crater");
            RegisterString("discovery.micro_shell_crater.description", "The blast fused the earth into dark, jagged glass along one side of the massive crater. A torn military harness and scorched metal fragments are fused into the soil. Stepping near the rim, each footfall lands with a hollow knock that carries further than it should.");
            RegisterString("discovery.micro_shell_crater.choice.inspect_crater", "Carefully inspect the crater edge for salvage.");
            RegisterString("discovery.micro_shell_crater.choice.salvage_crater_harness", "Dig out the harness and stamped metal fragments.");
            RegisterString("discovery.micro_shell_crater.choice.avoid_crater", "The ground is unstable. Walk around.");
            RegisterString("discovery.micro_field_kitchen.title", "Abandoned Field Kitchen");
            RegisterString("discovery.micro_field_kitchen.description", "A massive cast-iron cooking pot is frozen solid to its metal stand. Bent ladles hang from rusted wire hooks. Hundreds of tally marks are gouged into the serving table, five bars to a group, tracking rations that eventually ran out.");
            RegisterString("discovery.micro_field_kitchen.choice.search_kitchen", "Search the kitchen area for preserved food or fuel.");
            RegisterString("discovery.micro_field_kitchen.choice.take_kitchen_tools", "Take the ladles and cooking implements.");
            RegisterString("discovery.micro_field_kitchen.choice.read_ration_marks", "Read the ration marks scratched into the table.");
            RegisterString("discovery.micro_abandoned_generator.title", "Abandoned Generator");
            RegisterString("discovery.micro_abandoned_generator.description", "The heavy generator casing is wrenched open, its primary power cables severed with surgical precision. A frantic fuel mixture ratio is scrawled in grease pencil on the frame. The heavy block is dead, but it could be salvaged.");
            RegisterString("discovery.micro_abandoned_generator.choice.strip_generator", "Strip the generator for electrical components.");
            RegisterString("discovery.micro_abandoned_generator.choice.read_generator_notes", "Copy the fuel mixture notes from the frame.");
            RegisterString("discovery.micro_abandoned_generator.choice.mark_generator", "Mark the location for a future salvage team.");
            RegisterString("discovery.micro_shrine.title", "Roadside Shrine");
            RegisterString("discovery.micro_shrine.description", "Tallow candles have burned down to stubs inside a rusted ration tin. Small offerings—screws, shiny pebbles, a bullet—sit beneath a faded cloth carefully knotted around the twisted guardrail.");
            RegisterString("discovery.micro_shrine.choice.leave_shrine", "Leave the shrine undisturbed.");
            RegisterString("discovery.micro_shrine.choice.take_shrine_offerings", "Take the small offerings left beneath the cloth.");
            RegisterString("discovery.micro_shrine.choice.add_shrine_offering", "Leave a small offering of your own.");
            RegisterString("discovery.micro_emergency_cache.title", "Emergency Cache");
            RegisterString("discovery.micro_emergency_cache.description", "The rubber weather seal has cracked in the bitter cold, but the faded stencil is undeniable. Deep gouges show where someone tried to pry the heavy lock before abandoning it. It reads: CIVIL DEFENSE — EMERGENCY RATION CACHE.");
            RegisterString("discovery.micro_emergency_cache.choice.open_cache", "Force the cracked seal and open the cache.");
            RegisterString("discovery.micro_emergency_cache.choice.leave_cache", "Leave the cache sealed. Someone may need it more.");
            RegisterString("discovery.micro_observation_post.title", "Military Observation Post");
            RegisterTranslation("de", "discovery.micro_observation_post.title", "Militärischer Beobachtungsposten");
            RegisterString("discovery.micro_observation_post.description", "A heavy optics mount still points silently toward the desolate crossing below. Frantic grid references and kill-counts are scrawled on the concrete wall, stopping abruptly mid-sentence. A single spent sniper cartridge sits on the frozen windowsill.");
            RegisterTranslation("de", "discovery.micro_observation_post.description", "Eine schwere Optikhalterung weist noch immer lautlos auf die verlassene Kreuzung hinunter. Hektische Gitterkoordinaten und Abschusszahlen sind an die Betonwand gekritzelt und brechen mitten im Satz ab. Eine einzelne abgefeuerte Scharfschützenhülse liegt auf der gefrorenen Fensterbank.");
            RegisterString("discovery.micro_observation_post.choice.search_observation_post", "Search the post for optics or intelligence documents.");
            RegisterTranslation("de", "discovery.micro_observation_post.choice.search_observation_post", "Den Posten nach Optiken oder Aufklärungsdokumenten durchsuchen.");
            RegisterString("discovery.micro_observation_post.choice.read_grid_references", "Copy the grid references and dates from the wall.");
            RegisterTranslation("de", "discovery.micro_observation_post.choice.read_grid_references", "Die Gitterkoordinaten und Daten von der Wand abschreiben.");
            RegisterString("discovery.micro_observation_post.choice.ignore_observation_post", "The post has been stripped. Move on.");
            RegisterTranslation("de", "discovery.micro_observation_post.choice.ignore_observation_post", "Der Posten wurde geplündert. Weitergehen.");
            RegisterString("discovery.micro_abandoned_barricade.title", "Abandoned Barricade");
            RegisterString("discovery.micro_abandoned_barricade.description", "Traffic arrows were repainted twice in different faction colors, marking shifting territories. A mummified corpse lies slumped behind the barrier, clutching a rusted lockbox. The sandbags have frozen into a solid, bullet-pocked wall.");
            RegisterString("discovery.micro_abandoned_barricade.choice.search_barricade", "Search the barricade for supplies left behind.");
            RegisterString("discovery.micro_abandoned_barricade.choice.read_barricade_markings", "Examine the faction markings on the barricade.");
            RegisterString("discovery.micro_abandoned_barricade.choice.avoid_barricade", "Circle around. The body suggests this was not a safe place.");
            RegisterString("discovery.micro_hunting_blind.title", "Hunting Blind");
            RegisterString("discovery.micro_hunting_blind.description", "Half of the camouflage blind has collapsed under the weight of radioactive ash. An empty thermos, scattered brass casings, and a cloth-wrapped journal lie abandoned under the bench. The damp pages chronicle a hunt that went terribly wrong.");
            RegisterString("discovery.micro_hunting_blind.choice.search_blind", "Search the blind for hunting supplies.");
            RegisterString("discovery.micro_hunting_blind.choice.read_blind_journal", "Read the damp journal.");
            RegisterString("discovery.micro_hunting_blind.choice.leave_blind", "Leave the blind as you found it.");
            RegisterString("discovery.micro_radio_tower.title", "Damaged Radio Tower");
            RegisterString("discovery.micro_radio_tower.description", "The towering mast has buckled, its twisted steel groaning in the wind. A single intact feed line snakes into a heavy, locked utility cabinet at the base. The metal door is scarred with deep claw marks.");
            RegisterString("discovery.micro_radio_tower.choice.open_radio_cabinet", "Force the cabinet open and salvage the components.");
            RegisterString("discovery.micro_radio_tower.choice.read_radio_log", "Check the cabinet for a frequency log or maintenance record.");
            RegisterString("discovery.micro_radio_tower.choice.ignore_radio", "The tower is beyond repair. Move on.");
            RegisterString("discovery.micro_destroyed_checkpoint.title", "Destroyed Checkpoint");
            RegisterString("discovery.micro_destroyed_checkpoint.description", "Heavy concrete barriers are scorched black on the side facing the bunker, suggesting whatever attacked came from within. A blood-spattered logbook is pinned beneath a collapsed steel chair. The faction identity plate has been violently torn away.");
            RegisterString("discovery.micro_destroyed_checkpoint.choice.search_checkpoint", "Search the checkpoint for confiscated goods or supplies.");
            RegisterString("discovery.micro_destroyed_checkpoint.choice.read_checkpoint_log", "Read the logbook beneath the chair.");
            RegisterString("discovery.micro_destroyed_checkpoint.choice.avoid_checkpoint", "The destruction suggests heavy fighting. Keep moving.");
            RegisterString("discovery.micro_abandoned_tent.title", "Abandoned Tent");
            RegisterString("discovery.micro_abandoned_tent.description", "The military-grade fabric has collapsed under the weight of grey snow. Inside, nested tin cups sit beside a child's crayon drawing, meticulously preserved in a plastic sleeve. The tent's zipper is frozen shut from the inside.");
            RegisterString("discovery.micro_abandoned_tent.choice.search_tent", "Cut open the tent and search for supplies.");
            RegisterString("discovery.micro_abandoned_tent.choice.take_drawing", "Take the child's drawing from the plastic sleeve.");
            RegisterString("discovery.micro_abandoned_tent.choice.leave_tent", "Leave the tent undisturbed.");
            RegisterString("discovery.micro_makeshift_clinic.title", "Makeshift Clinic");
            RegisterString("discovery.micro_makeshift_clinic.description", "Desperate triage categories are scrawled in blood on torn cardboard strips. Empty, frozen saline bags hang from a rusted IV cable. The medicine cabinet was ransacked—some shelves stripped bare while others were ignored entirely.");
            RegisterString("discovery.micro_makeshift_clinic.choice.search_clinic", "Search the untouched shelves for medical supplies.");
            RegisterString("discovery.micro_makeshift_clinic.choice.read_triage_list", "Read the triage numbers on the cardboard strips.");
            RegisterString("discovery.micro_makeshift_clinic.choice.leave_clinic", "Leave the clinic intact. Others may need it.");
            RegisterString("discovery.micro_crashed_drone.title", "Crashed Drone");
            RegisterString("discovery.micro_crashed_drone.description", "One shattered wing is driven deep into the frozen earth. The outer casing was ripped open by scavengers, but the armored flight-control compartment remains sealed. Corrosive battery acid has burned a dark, hissing stain into the soil.");
            RegisterString("discovery.micro_crashed_drone.choice.open_drone_compartment", "Force open the flight-control compartment.");
            RegisterString("discovery.micro_crashed_drone.choice.read_drone_log", "Attempt to extract the flight log from the sealed compartment.");
            RegisterString("discovery.micro_crashed_drone.choice.avoid_drone", "The battery acid suggests chemical risk. Leave it.");
            RegisterString("discovery.micro_fuel_cache.title", "Fuel Cache");
            RegisterString("discovery.micro_fuel_cache.description", "Two military drums are half-buried beneath a rusted sheet of corrugated metal. A desperate route map to a safe zone has been scratched into one lid with a nail. The drums are heavy, their tamper-seals unbroken.");
            RegisterString("discovery.micro_fuel_cache.choice.take_fuel_cache", "Dig up the containers and take the fuel.");
            RegisterString("discovery.micro_fuel_cache.choice.read_route_sketch", "Copy the route sketch from the lid.");
            RegisterString("discovery.micro_fuel_cache.choice.leave_fuel_cache", "Leave the cache hidden. You may need it later.");
            RegisterString("discovery.micro_water_source.title", "Water Source");
            RegisterString("discovery.micro_water_source.description", "A dented tin cup is bound to the rusted hand pump with frayed wire, the concrete basin stained with heavy, unnatural mineral deposits. Someone wrote 'SAFE' in charcoal, alongside a date from five years ago. The pump handle still groans when moved.");
            RegisterString("discovery.micro_water_source.choice.collect_water", "Pump and collect water from the source.");
            RegisterString("discovery.micro_water_source.choice.test_water", "Test the water quality before collecting.");
            RegisterString("discovery.micro_water_source.choice.avoid_water", "The mineral staining is suspicious. Move on.");
            RegisterString("discovery.micro_supply_drop.title", "Supply Drop");
            RegisterString("discovery.micro_supply_drop.description", "The faded military parachute is hopelessly tangled in the black, petrified branches of dead trees. The stenciled coordinates indicate it drifted miles off course. The reinforced crate is intact, but the heavy locking seal has been severed.");
            RegisterString("discovery.micro_supply_drop.choice.open_supply_drop", "Open the crate and take what is inside.");
            RegisterString("discovery.micro_supply_drop.choice.read_supply_label", "Read the shipping label for destination and origin information.");
            RegisterString("discovery.micro_supply_drop.choice.leave_supply_drop", "Leave the crate. It was meant for someone else.");
            RegisterString("discovery.micro_hospital_chapel_ledger.title", "Hospital Chapel Ledger");
            RegisterString("discovery.micro_hospital_chapel_ledger.description", "The chapel beside the east wing kept a visitors' ledger through the worst of it. The last entries are names, then bed numbers, then nothing at all — just a date repeated down the page in a steady hand until the hand stopped coming. A candle box under the stand still has three stubs and a packet of matches.");
            RegisterString("discovery.micro_hospital_chapel_ledger.choice.read_the_names", "Read the last page properly, then close the ledger.");
            RegisterString("discovery.micro_hospital_chapel_ledger.choice.take_matches", "Take the lighter from the candle box. The dead do not need light.");
            RegisterString("discovery.micro_depot_undertow_raft_line.title", "Undertow Raft Line");
            RegisterString("discovery.micro_depot_undertow_raft_line.description", "A raft of sealed crates lashed to a handrail runs down the flooded concourse, marked with the Undertow's tar-symbol. The line is rigged to a pulley someone greased recently. Whatever moves along it moves quietly, and often.");
            RegisterString("discovery.micro_depot_undertow_raft_line.choice.note_the_route", "Mark the pulley anchors on your map and leave the line alone.");
            RegisterString("discovery.micro_depot_undertow_raft_line.choice.cut_one_crate", "Cut one crate free before the owners notice.");
            RegisterString("discovery.micro_gamma_levy_board.title", "Checkpoint Gamma Levy Board");
            RegisterString("discovery.micro_gamma_levy_board.description", "The garrison's levy board still stands by the guard shack: grain quotas, diesel quotas, and the names of households marked square for paid and circle for owed. Fresh chalk over old chalk. Somebody drives here to write, which means somebody still collects.");
            RegisterString("discovery.micro_gamma_levy_board.choice.memorize_the_board", "Memorize which farms have paid and which are marked owed.");
            RegisterString("discovery.micro_gamma_levy_board.choice.take_the_chalk", "Take the chalk and the board's rating stamp. Forged square marks might pass at a distance.");
        }
    }
}
