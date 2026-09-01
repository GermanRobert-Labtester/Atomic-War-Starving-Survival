using System;
using Xunit;
using Ashfall.Core.Localization;
using Ashfall.Core.Settings;

namespace Ashfall.Core.Tests.Localization
{
    public class LocalizationServiceTests
    {
        [Fact]
        public void Get_ReturnsRegisteredEnglishString()
        {
            var loc = new LocalizationService();
            loc.RegisterString("test.hello", "Hello Survivor");

            string result = loc.Get("test.hello");
            Assert.Equal("Hello Survivor", result);
        }

        [Fact]
        public void Get_MissingKey_ReturnsFallbackOrKey()
        {
            var loc = new LocalizationService();

            string resultWithDefault = loc.Get("non.existent.key", "Default Text");
            Assert.Equal("Default Text", resultWithDefault);

            string resultWithoutDefault = loc.Get("non.existent.key");
            Assert.Equal("non.existent.key", resultWithoutDefault);
        }

        [Fact]
        public void Format_SubstitutesPositionalParameters()
        {
            var loc = new LocalizationService();
            loc.RegisterString("test.dose", "Dose: {0:F1} mSv for survivor {1}");

            string result = loc.Format("test.dose", 38.54, "Mikhail");
            Assert.Equal("Dose: 38.5 mSv for survivor Mikhail", result);
        }

        [Fact]
        public void LoadFromCsv_ParsesKeysAndTranslations()
        {
            var loc = new LocalizationService();
            string csv = "key,en\n" +
                         "ui.menu.start,\"Start Expedition\"\n" +
                         "ui.menu.quit,\"Abandon Shelter\"\n";

            loc.LoadFromCsv(csv);

            Assert.True(loc.HasKey("ui.menu.start"));
            Assert.Equal("Start Expedition", loc.Get("ui.menu.start"));
            Assert.Equal("Abandon Shelter", loc.Get("ui.menu.quit"));
        }

        [Fact]
        public void PseudoLocalization_ExpandsStringAndSubstitutesAccents()
        {
            var loc = new LocalizationService();
            loc.RegisterString("test.water", "Water reserve low");
            loc.SetLocale("pseudo");

            string pseudo = loc.Get("test.water");

            // Verify pseudo format
            Assert.StartsWith("[!!! ", pseudo);
            Assert.EndsWith(" !!!]", pseudo);
            Assert.True(pseudo.Length > "Water reserve low".Length);
        }

        [Fact]
        public void PseudoLocalization_PreservesFormatPlaceholders()
        {
            string src = "Survivor {0} took {1} dose";
            string pseudo = LocalizationService.GeneratePseudoString(src);

            Assert.Contains("{0}", pseudo);
            Assert.Contains("{1}", pseudo);
        }

        [Fact]
        public void UserSettingsCodec_SanitizesLocaleAndTutorialMode()
        {
            var data = new UserSettingsData
            {
                Locale = "INVALID_LOCALE",
                TutorialMode = 99
            };

            var sanitized = UserSettingsCodec.Sanitize(data, out string? msg);

            Assert.Equal("en", sanitized.Locale);
            Assert.Equal(0, sanitized.TutorialMode);
            Assert.NotNull(msg);
        }

        [Fact]
        public void UserSettingsCodec_PreservesValidPseudoLocaleAndTutorialMode()
        {
            var data = new UserSettingsData
            {
                Locale = "pseudo",
                TutorialMode = 1
            };

            var sanitized = UserSettingsCodec.Sanitize(data, out _);

            Assert.Equal("pseudo", sanitized.Locale);
            Assert.Equal(1, sanitized.TutorialMode);
        }
    }
}
