// SPDX-License-Identifier: MIT
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

            Assert.Equal("Standing with Elena improved.", successMsg);
            Assert.Equal("Standing with Elena is 75/100.", relMsg);

            // storm_approaching in warning vs world_state
            string warnMsg = c.FormatCategory("warning", "storm_approaching", 4);
            string worldMsg = c.FormatCategory("world_state", "storm_approaching", 4);

            Assert.Equal("Storm in 4 hours. Seal the intake.", warnMsg);
            Assert.Equal("Fallout storm due in 4 hours.", worldMsg);
        }

        [Fact]
        public void MissingKey_ReturnsDefaultCategoryFallback()
        {
            var catalog = new FeedbackMessageCatalog();

            // Category defaults
            Assert.Equal("Done.", catalog.FormatCategory("success", "unknown_key"));
            Assert.Equal("That didn't take. Check the inputs.", catalog.FormatCategory("failure", "unknown_key"));
            Assert.Equal("Caution.", catalog.FormatCategory("warning", "unknown_key"));
            Assert.Equal("Something went wrong.", catalog.FormatCategory("error", "unknown_key"));
            Assert.Equal("Continue?", catalog.FormatCategory("confirmation", "unknown_key"));
            Assert.Equal("ALERT: Something needs attention.", catalog.FormatCategory("alert", "unknown_key"));

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
                template = "Task closed. Standing {0}. Stores {1}.",
                parameter_count = 2
            });

            string formatted = catalog.Format("quest_completed", 50, 100);
            Assert.Equal("Task closed. Standing 50. Stores 100.", formatted);
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
                template = "Expedition: {0} days out. {1} days left. Distance: {2} km.",
                parameter_count = 3
            });

            // Passing only 1 argument to a 3-argument template (would throw FormatException in string.Format)
            string partial1 = catalog.Format("expedition_progress", 5);
            Assert.NotNull(partial1);
            Assert.Contains("5 days out", partial1);

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
                template = "ALERT: Fallout storm in {0} hours.",
                display_duration_seconds = 5.0f
            });

            Assert.Equal(FeedbackSeverity.Critical, catalog.GetSeverity("storm_alert"));
            Assert.Equal(5.0f, catalog.GetDisplayDuration("storm_alert"));

            // Missing key defaults
            Assert.Equal(FeedbackSeverity.Info, catalog.GetSeverity("unknown_key"));
            Assert.Equal(3.0f, catalog.GetDisplayDuration("unknown_key"));
        }

        [Fact]
        public void DisplayDuration_ClampedToPresentationBounds()
        {
            var catalog = new FeedbackMessageCatalog();
            catalog.RegisterTemplate(new FeedbackMessageTemplate
            {
                key = "too_short",
                template = "Short",
                display_duration_seconds = 0.2f
            });
            catalog.RegisterTemplate(new FeedbackMessageTemplate
            {
                key = "too_long",
                template = "Long",
                display_duration_seconds = 60.0f
            });

            Assert.Equal(1.0f, catalog.GetDisplayDuration("too_short"));
            Assert.Equal(15.0f, catalog.GetDisplayDuration("too_long"));
        }

        [Fact]
        public void FeedbackService_EmitsResolvedMessage_WithSafeFormatting()
        {
            var catalog = FeedbackMessageCatalogLoader.CreateDefaultContainer();
            var service = new FeedbackService(new FeedbackMessageCatalog(catalog.messages));

            ResolvedFeedbackMessage? received = null;
            service.OnFeedbackEmitted += msg => received = msg;

            var evt = new FeedbackEvent("trade_success", new object[] { "100 Scrap", "20 Fuel" });
            bool emitted = service.Emit(evt);

            Assert.True(emitted);
            Assert.NotNull(received);
            Assert.Equal("trade_success", received.Key);
            Assert.Equal("success", received.Category);
            Assert.Equal(FeedbackSeverity.Success, received.Severity);
            Assert.Equal("Trade settled. Received 100 Scrap for 20 Fuel.", received.FormattedText);
        }

        [Fact]
        public void FeedbackService_SeparatesDeveloperDiagnostics_FromPlayerMessages()
        {
            var service = new FeedbackService();

            ResolvedFeedbackMessage? playerMessage = null;
            ResolvedFeedbackMessage? diagMessage = null;
            service.OnFeedbackEmitted += msg => playerMessage = msg;
            service.OnDiagnosticEmitted += msg => diagMessage = msg;

            // Diagnostic key: missing_id
            var diagEvt = new FeedbackEvent("missing_id", new object[] { "item_unknown" });
            bool emitted = service.Emit(diagEvt);

            Assert.True(emitted);
            Assert.Null(playerMessage); // Did NOT leak into player toast stream!
            Assert.NotNull(diagMessage); // Was safely routed to diagnostics!
            Assert.True(diagMessage.IsDiagnosticOnly);
        }

        [Fact]
        public void FeedbackDeduplicator_SuppressesSpam_AllowsEscalation()
        {
            var dedupe = new FeedbackDeduplicator(cooldownSeconds: 5.0f);

            // First event: warning at t=1.0s -> not suppressed
            Assert.False(dedupe.ShouldSuppress("food_low", FeedbackSeverity.Warning, 1.0f));
            dedupe.Record("food_low", FeedbackSeverity.Warning, 1.0f);

            // Second identical event at t=2.0s -> suppressed!
            Assert.True(dedupe.ShouldSuppress("food_low", FeedbackSeverity.Warning, 2.0f));

            // Escalation to Critical at t=3.0s -> NOT suppressed (escalation bypass)!
            Assert.False(dedupe.ShouldSuppress("food_low", FeedbackSeverity.Critical, 3.0f));
            dedupe.Record("food_low", FeedbackSeverity.Critical, 3.0f);

            // Third identical event at t=4.0s (Critical) -> suppressed!
            Assert.True(dedupe.ShouldSuppress("food_low", FeedbackSeverity.Critical, 4.0f));

            // Fourth event after cooldown at t=9.0s -> not suppressed!
            Assert.False(dedupe.ShouldSuppress("food_low", FeedbackSeverity.Critical, 9.0f));
        }

        [Fact]
        public void FeedbackDeduplicator_TransitionTracker_FiresOnlyOnThresholdCross()
        {
            var dedupe = new FeedbackDeduplicator();

            // Entering alert: false -> true
            Assert.True(dedupe.EvaluateTransition("starvation", true));

            // Still in alert next tick: true -> true (no re-fire)
            Assert.False(dedupe.EvaluateTransition("starvation", true));
            Assert.False(dedupe.EvaluateTransition("starvation", true));

            // Recovered: true -> false (cleared)
            Assert.False(dedupe.EvaluateTransition("starvation", false));

            // Entering alert again: false -> true (fires again!)
            Assert.True(dedupe.EvaluateTransition("starvation", true));
        }
    }
}
