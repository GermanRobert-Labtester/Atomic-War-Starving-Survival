using System;
using System.IO;
using System.Text.Json;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Feedback;
using Ashfall.Core.IO;

namespace Ashfall.Core.Tests.UI
{
    public class FeedbackMessageTests
    {
        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void ExportAndVerify_All200Templates_LoadFromJson_AndMatchCatalog()
        {
            var defaultContainer = FeedbackMessageCatalogLoader.CreateDefaultContainer();
            Assert.Equal(1, defaultContainer.schema_version);
            Assert.Equal(200, defaultContainer.messages.Count);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };
            string jsonString = JsonSerializer.Serialize(defaultContainer, options);

            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            string targetPath = fileIO.Combine(dataDir, FeedbackMessageCatalogLoader.FileName);

            // Write canonical feedback_messages.json
            fileIO.WriteAllText(targetPath, jsonString);

            // Load back through core loader
            var serializer = new SystemTextJsonSerializer();
            var catalog = FeedbackMessageCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);

            Assert.Equal(200, catalog.AllTemplates.Count);

            foreach (var template in defaultContainer.messages)
            {
                Assert.True(catalog.TryGetTemplate(template.category, template.key, out var loaded));
                Assert.NotNull(loaded);
                Assert.Equal(template.key, loaded.key);
                Assert.Equal(template.category, loaded.category);
                Assert.Equal(template.severity, loaded.severity);
                Assert.Equal(template.template, loaded.template);
                Assert.Equal(template.parameter_count, loaded.parameter_count);
                Assert.Equal(template.display_duration_seconds, loaded.display_duration_seconds);
            }
        }

        [Fact]
        public void CategoryScoped_DuplicateKeys_PreserveCategorySpecificTemplates()
        {
            var catalog = FeedbackMessageCatalogLoader.CreateDefaultContainer();
            var c = new FeedbackMessageCatalog(catalog.messages);

            // relationship_improved in success vs relationship
            string successMsg = c.FormatCategory("success", "relationship_improved", "Elena");
            string relMsg = c.FormatCategory("relationship", "relationship_improved", "Elena", 75);

            Assert.Equal("Your relationship with Elena has improved.", successMsg);
            Assert.Equal("Your relationship with Elena has improved to 75/100.", relMsg);

            // storm_approaching in warning vs world_state
            string warnMsg = c.FormatCategory("warning", "storm_approaching", 4);
            string worldMsg = c.FormatCategory("world_state", "storm_approaching", 4);

            Assert.Equal("A storm is approaching in 4 hours. Prepare the bunker.", warnMsg);
            Assert.Equal("The weather forecast predicts a radiation storm in 4 hours.", worldMsg);
        }

        [Fact]
        public void MissingKey_ReturnsDefaultCategoryFallback()
        {
            var catalog = new FeedbackMessageCatalog();

            // Category defaults
            Assert.Equal("Success! Operation completed.", catalog.FormatCategory("success", "unknown_key"));
            Assert.Equal("Operation failed. Check your inputs.", catalog.FormatCategory("failure", "unknown_key"));
            Assert.Equal("Warning: Proceed with caution.", catalog.FormatCategory("warning", "unknown_key"));
            Assert.Equal("Error: Something went wrong.", catalog.FormatCategory("error", "unknown_key"));
            Assert.Equal("Are you sure you want to proceed?", catalog.FormatCategory("confirmation", "unknown_key"));
            Assert.Equal("ALERT: Important update available!", catalog.FormatCategory("alert", "unknown_key"));

            // Generic Format with missing key
            string formatted = catalog.Format("totally_unknown_key", "arg1", 42);
            Assert.NotNull(formatted);
            Assert.Contains("arg1", formatted);
        }

        [Fact]
        public void ParameterSubstitution_ExactArgs_FormatsCorrectly()
        {
            var catalog = new FeedbackMessageCatalog();
            catalog.RegisterTemplate(new FeedbackMessageTemplate
            {
                key = "quest_completed",
                category = "success",
                severity = "success",
                template = "Quest completed! You've earned {0} reputation and {1} resources.",
                parameter_count = 2
            });

            string formatted = catalog.Format("quest_completed", 50, 100);
            Assert.Equal("Quest completed! You've earned 50 reputation and 100 resources.", formatted);
        }

        [Fact]
        public void ParameterSubstitution_MissingOrMismatchedArgs_GracefulFallbackWithoutException()
        {
            var catalog = new FeedbackMessageCatalog();
            catalog.RegisterTemplate(new FeedbackMessageTemplate
            {
                key = "expedition_progress",
                category = "progress",
                severity = "info",
                template = "Expedition progress: {0} days elapsed. {1} days remaining. Distance: {2} km.",
                parameter_count = 3
            });

            // Passing only 1 argument to a 3-argument template (would throw FormatException in string.Format)
            string partial1 = catalog.Format("expedition_progress", 5);
            Assert.NotNull(partial1);
            Assert.Contains("5 days elapsed", partial1);

            // Passing 0 arguments to a 3-argument template
            string partial0 = catalog.Format("expedition_progress");
            Assert.NotNull(partial0);

            // Passing null arguments
            string partialNull = catalog.Format("expedition_progress", null);
            Assert.NotNull(partialNull);

            // Passing null template directly to SafeFormat
            Assert.Equal(string.Empty, FeedbackMessageCatalog.SafeFormat(null));
        }

        [Fact]
        public void SeverityAndDuration_CalculatedCorrectly()
        {
            var catalog = new FeedbackMessageCatalog();
            catalog.RegisterTemplate(new FeedbackMessageTemplate
            {
                key = "storm_alert",
                category = "alert",
                severity = "critical",
                template = "ALERT: Radiation storm approaching in {0} hours!",
                display_duration_seconds = 5.0f
            });

            Assert.Equal(FeedbackSeverity.Critical, catalog.GetSeverity("storm_alert"));
            Assert.Equal(5.0f, catalog.GetDisplayDuration("storm_alert"));

            // Missing key defaults
            Assert.Equal(FeedbackSeverity.Info, catalog.GetSeverity("unknown_key"));
            Assert.Equal(3.0f, catalog.GetDisplayDuration("unknown_key"));
        }
    }
}
