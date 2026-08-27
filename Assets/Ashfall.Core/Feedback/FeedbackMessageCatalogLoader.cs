using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Feedback
{
    /// <summary>
    /// Engine-agnostic loader for user-facing feedback messages from JSON data authority.
    /// </summary>
    public static class FeedbackMessageCatalogLoader
    {
        public const string FileName = "feedback_messages.json";

        public static FeedbackMessageContainer LoadContainer(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return CreateDefaultContainer();

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return CreateDefaultContainer();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return CreateDefaultContainer();

            try
            {
                var container = json.Deserialize<FeedbackMessageContainer>(raw);
                if (container != null && container.messages != null && container.messages.Count > 0)
                {
                    return container;
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "<root>", ex);
            }

            try
            {
                var list = CatalogLocator.LoadWrappedList<FeedbackMessageTemplate>(raw, SystemTextJsonSerializer.Options);
                if (list != null && list.Count > 0)
                {
                    return new FeedbackMessageContainer { schema_version = 1, messages = list };
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "<root>", ex);
            }

            return CreateDefaultContainer();
        }

        public static FeedbackMessageCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var container = LoadContainer(dataDir, fileIO, json);
            return new FeedbackMessageCatalog(container.messages);
        }

        public static FeedbackMessageContainer CreateDefaultContainer()
        {
            var container = new FeedbackMessageContainer { schema_version = 1 };
            var list = container.messages;

            // Success (10)
            Add(list, "quest_completed", "success", "success", "Quest completed! You've earned {0} reputation and {1} resources.", 2, 4.0f);
            Add(list, "survivor_recruited", "success", "success", "{0} has joined your bunker!", 1, 3.5f);
            Add(list, "resource_gained", "success", "success", "You've gained {0} {1}.", 2, 3.0f);
            Add(list, "technology_unlocked", "success", "success", "New technology unlocked: {0}.", 1, 4.0f);
            Add(list, "relationship_improved", "success", "success", "Your relationship with {0} has improved.", 1, 3.5f);
            Add(list, "bunker_upgraded", "success", "success", "Bunker upgraded! Capacity increased by {0}.", 1, 4.0f);
            Add(list, "medical_treatment_success", "success", "success", "Medical treatment successful. {0}'s condition improved.", 1, 3.5f);
            Add(list, "trade_success", "success", "success", "Trade completed! Received {0} in exchange for {1}.", 2, 3.5f);
            Add(list, "alliance_formed", "success", "success", "Alliance formed with {0}.", 1, 4.0f);
            Add(list, "expedition_success", "success", "success", "Expedition returned safely with {0} resources.", 1, 4.0f);

            // Failure (10)
            Add(list, "quest_failed", "failure", "error", "Quest failed. You lost {0} reputation and {1} morale.", 2, 4.0f);
            Add(list, "survivor_lost", "failure", "error", "{0} has died. Their skills are lost to the bunker.", 1, 4.5f);
            Add(list, "resource_lost", "failure", "error", "You've lost {0} {1}.", 2, 3.0f);
            Add(list, "technology_lost", "failure", "error", "Technology {0} was damaged and is now unusable.", 1, 4.0f);
            Add(list, "relationship_damaged", "failure", "error", "Your relationship with {0} has been damaged.", 1, 3.5f);
            Add(list, "bunker_damaged", "failure", "error", "Bunker took damage! Repairs needed.", 0, 4.0f);
            Add(list, "medical_failure", "failure", "error", "Medical treatment failed. {0}'s condition worsened.", 1, 3.5f);
            Add(list, "trade_failed", "failure", "error", "Trade failed. {0} took your resources without delivering.", 1, 4.0f);
            Add(list, "alliance_broken", "failure", "error", "Alliance with {0} has been broken.", 1, 4.0f);
            Add(list, "expedition_failed", "failure", "error", "Expedition failed. No resources recovered.", 0, 4.0f);

            // Warning (10)
            Add(list, "low_food", "warning", "warning", "Food supplies are critically low! Prioritize rationing.", 0, 4.5f);
            Add(list, "low_medical", "warning", "warning", "Medical supplies are running out. Assign scavengers to find more.", 0, 4.5f);
            Add(list, "low_fuel", "warning", "warning", "Fuel reserves are at {0}%. Send an expedition soon.", 1, 4.5f);
            Add(list, "high_radiation", "warning", "warning", "Radiation levels are elevated. Avoid outdoor activities.", 0, 4.5f);
            Add(list, "low_morale", "warning", "warning", "Morale is dangerously low. Survivors are restless.", 0, 4.5f);
            Add(list, "bunker_deteriorating", "warning", "warning", "Bunker structures are deteriorating. Assign laborers to repairs.", 0, 4.5f);
            Add(list, "disease_outbreak", "warning", "warning", "A disease is spreading. Quarantine infected survivors immediately.", 0, 5.0f);
            Add(list, "raider_activity", "warning", "warning", "Raider activity detected in Sector {0}. Stay alert.", 1, 4.5f);
            Add(list, "storm_approaching", "warning", "warning", "A storm is approaching in {0} hours. Prepare the bunker.", 1, 4.5f);
            Add(list, "power_critical", "warning", "warning", "Power levels are critical. Generator needs immediate attention.", 0, 5.0f);

            // Error (10)
            Add(list, "invalid_action", "error", "error", "Invalid action. Please try again.", 0, 3.0f);
            Add(list, "insufficient_resources", "error", "error", "Insufficient {0} to complete this action.", 1, 3.5f);
            Add(list, "missing_id", "error", "error", "Error: Missing ID '{0}'. Check your data files.", 1, 4.0f);
            Add(list, "system_overload", "error", "error", "System overload detected. Please wait and try again.", 0, 4.0f);
            Add(list, "file_not_found", "error", "error", "File not found: {0}. Check your installation.", 1, 4.0f);
            Add(list, "corrupt_data", "error", "error", "Corrupt data detected in {0}. The file may be damaged.", 1, 4.5f);
            Add(list, "permission_denied", "error", "error", "Permission denied. You don't have access to this action.", 0, 3.5f);
            Add(list, "network_error", "error", "error", "Network error. Unable to connect to {0}.", 1, 4.0f);
            Add(list, "out_of_bounds", "error", "error", "Error: Value out of bounds. Check your inputs.", 0, 3.5f);
            Add(list, "invalid_input", "error", "error", "Invalid input. Please enter a valid value.", 0, 3.0f);

            // Confirmation (10)
            Add(list, "delete_survivor", "confirmation", "warning", "Are you sure you want to exile {0}? This cannot be undone.", 1, 5.0f);
            Add(list, "abandon_quest", "confirmation", "warning", "Are you sure you want to abandon this quest? Progress will be lost.", 0, 5.0f);
            Add(list, "use_medicine", "confirmation", "warning", "Are you sure you want to use {0} medicine on {1}? This cannot be undone.", 2, 5.0f);
            Add(list, "scavenge_dangerous", "confirmation", "warning", "Are you sure you want to send an expedition to this dangerous location?", 0, 5.0f);
            Add(list, "trade_with_faction", "confirmation", "warning", "Are you sure you want to trade with {0}? Their reputation is questionable.", 1, 5.0f);
            Add(list, "upgrade_bunker", "confirmation", "warning", "Are you sure you want to upgrade the bunker? This will cost {0} resources.", 1, 5.0f);
            Add(list, "start_expedition", "confirmation", "warning", "Are you sure you want to start this expedition? Survivors will be at risk.", 0, 5.0f);
            Add(list, "accept_alliance", "confirmation", "warning", "Are you sure you want to accept this alliance? It may have hidden costs.", 0, 5.0f);
            Add(list, "use_technology", "confirmation", "warning", "Are you sure you want to use this unstable technology? It may cause damage.", 0, 5.0f);
            Add(list, "close_bunker", "confirmation", "warning", "Are you sure you want to close the bunker? Survivors outside will be at risk.", 0, 5.0f);

            // Progress (10)
            Add(list, "quest_progress", "progress", "info", "Quest progress: {0}% complete. {1} remaining.", 2, 3.0f);
            Add(list, "construction_progress", "progress", "info", "Construction progress: {0}% complete. {1} days remaining.", 2, 3.0f);
            Add(list, "training_progress", "progress", "info", "Training progress: {0}% complete. {1} survivors remaining.", 2, 3.0f);
            Add(list, "expedition_progress", "progress", "info", "Expedition progress: {0} days elapsed. {1} days remaining. Distance: {2} km.", 3, 3.5f);
            Add(list, "medical_progress", "progress", "info", "Medical treatment: {0}% complete. {1} remaining.", 2, 3.0f);
            Add(list, "repair_progress", "progress", "info", "Repair progress: {0}% complete. {1} systems remaining.", 2, 3.0f);
            Add(list, "ration_progress", "progress", "info", "Rationing: {0} days of supplies remaining. {1} days until restock.", 2, 3.0f);
            Add(list, "morale_progress", "progress", "info", "Morale: {0}/100. {1} survivors affected.", 2, 3.0f);
            Add(list, "relationship_progress", "progress", "info", "Relationship with {0}: {1}/100.", 2, 3.0f);
            Add(list, "resource_progress", "progress", "info", "{0}: {1}/{2} available.", 3, 3.0f);

            // Reward (10)
            Add(list, "reputation_gained", "reward", "success", "You've gained {0} reputation with {1}.", 2, 3.5f);
            Add(list, "resource_reward", "reward", "success", "You've earned {0} {1}.", 2, 3.0f);
            Add(list, "item_reward", "reward", "success", "You've received: {0}.", 1, 3.0f);
            Add(list, "technology_reward", "reward", "success", "New technology unlocked: {0}.", 1, 4.0f);
            Add(list, "skill_reward", "reward", "success", "{0} has learned a new skill: {1}.", 2, 3.5f);
            Add(list, "morale_boost", "reward", "success", "Morale increased by {0} points.", 1, 3.0f);
            Add(list, "health_reward", "reward", "success", "{0}'s health improved by {1} points.", 2, 3.0f);
            Add(list, "faction_reward", "reward", "success", "Your alliance with {0} has strengthened.", 1, 3.5f);
            Add(list, "experience_reward", "reward", "success", "{0} gained {1} experience points.", 2, 3.0f);
            Add(list, "bonus_reward", "reward", "success", "Bonus reward: {0}.", 1, 3.5f);

            // Penalty (10)
            Add(list, "reputation_lost", "penalty", "warning", "You've lost {0} reputation with {1}.", 2, 3.5f);
            Add(list, "resource_penalty", "penalty", "warning", "You've lost {0} {1}.", 2, 3.0f);
            Add(list, "item_penalty", "penalty", "warning", "You've lost: {0}.", 1, 3.0f);
            Add(list, "technology_damaged", "penalty", "warning", "Technology {0} was damaged and is now unusable.", 1, 4.0f);
            Add(list, "skill_penalty", "penalty", "warning", "{0} lost the skill: {1}.", 2, 3.5f);
            Add(list, "morale_penalty", "penalty", "warning", "Morale decreased by {0} points.", 1, 3.0f);
            Add(list, "health_penalty", "penalty", "warning", "{0}'s health decreased by {1} points.", 2, 3.0f);
            Add(list, "faction_penalty", "penalty", "warning", "Your alliance with {0} has weakened.", 1, 3.5f);
            Add(list, "experience_penalty", "penalty", "warning", "{0} lost {1} experience points.", 2, 3.0f);
            Add(list, "time_penalty", "penalty", "warning", "You've lost {0} days due to delays.", 1, 3.5f);

            // Status (10)
            Add(list, "bunker_status", "status", "info", "Bunker: {0}/{1} capacity. {2}% structural integrity.", 3, 3.5f);
            Add(list, "survivor_status", "status", "info", "Survivors: {0}/{1} alive. {2} injured.", 3, 3.5f);
            Add(list, "food_status", "status", "info", "Food: {0}/{1} days remaining. {2}% waste.", 3, 3.0f);
            Add(list, "water_status", "status", "info", "Water: {0}/{1} liters remaining. {2}% clean.", 3, 3.0f);
            Add(list, "medical_status", "status", "info", "Medical: {0}/{1} supplies remaining. {2} patients.", 3, 3.0f);
            Add(list, "fuel_status", "status", "info", "Fuel: {0}/{1} liters remaining. {2}% efficiency.", 3, 3.0f);
            Add(list, "power_status", "status", "info", "Power: {0}/{1}%. Generator status: {2}.", 3, 3.0f);
            Add(list, "morale_status", "status", "info", "Morale: {0}/100. {1} survivors affected.", 2, 3.0f);
            Add(list, "radiation_status", "status", "info", "Radiation: {0} mSv/hr outside. {1} mSv/hr inside.", 2, 3.5f);
            Add(list, "weather_status", "status", "info", "Weather: {0}. Visibility: {1} km. Temperature: {2}°C.", 3, 3.5f);

            // Alert (10)
            Add(list, "storm_alert", "alert", "critical", "ALERT: Radiation storm approaching in {0} hours!", 1, 5.0f);
            Add(list, "raider_alert", "alert", "critical", "ALERT: Raiders detected near Sector {0}!", 1, 5.0f);
            Add(list, "disease_alert", "alert", "critical", "ALERT: Disease outbreak detected! Quarantine required.", 0, 5.0f);
            Add(list, "fire_alert", "alert", "critical", "ALERT: Fire in {0}! Evacuate if necessary.", 1, 5.0f);
            Add(list, "intruder_alert", "alert", "critical", "ALERT: Intruder detected in the bunker!", 0, 5.0f);
            Add(list, "power_alert", "alert", "critical", "ALERT: Power failure in {0}! Emergency protocols activated.", 1, 5.0f);
            Add(list, "radiation_alert", "alert", "critical", "ALERT: Radiation leak detected! Seal affected areas immediately.", 0, 5.0f);
            Add(list, "food_alert", "alert", "critical", "ALERT: Food storage compromised! Check for contamination.", 0, 5.0f);
            Add(list, "water_alert", "alert", "critical", "ALERT: Water filtration system failing! Repairs needed.", 0, 5.0f);
            Add(list, "medical_alert", "alert", "critical", "ALERT: Medical bay at capacity! Prioritize critical cases.", 0, 5.0f);

            // Hint (10)
            Add(list, "scavenge_hint", "hint", "info", "Tip: Scavenge in areas with low radiation for better results.", 0, 4.0f);
            Add(list, "ration_hint", "hint", "info", "Tip: Equal rationing maintains morale, but priority rationing saves lives.", 0, 4.0f);
            Add(list, "medical_hint", "hint", "info", "Tip: Assign your best medics to critical cases to improve survival rates.", 0, 4.0f);
            Add(list, "trade_hint", "hint", "info", "Tip: Trade with factions you trust, but always verify their claims.", 0, 4.0f);
            Add(list, "expedition_hint", "hint", "info", "Tip: Send experienced survivors on dangerous expeditions.", 0, 4.0f);
            Add(list, "morale_hint", "hint", "info", "Tip: Small celebrations boost morale more than large ones.", 0, 4.0f);
            Add(list, "repair_hint", "hint", "info", "Tip: Assign laborers to repair bunker structures to prevent deterioration.", 0, 4.0f);
            Add(list, "radiation_hint", "hint", "info", "Tip: Use protective gear when radiation levels are elevated.", 0, 4.0f);
            Add(list, "faction_hint", "hint", "info", "Tip: Maintain good relationships with multiple factions to avoid isolation.", 0, 4.0f);
            Add(list, "resource_hint", "hint", "info", "Tip: Stockpile resources before winter to avoid shortages.", 0, 4.0f);

            // Spoiler (10)
            Add(list, "major_reveal", "spoiler", "warning", "SPOILER WARNING: This action will reveal major plot points. Continue?", 0, 5.0f);
            Add(list, "ending_spoiler", "spoiler", "warning", "SPOILER: This choice affects the game's ending. Are you sure?", 0, 5.0f);
            Add(list, "faction_spoiler", "spoiler", "warning", "SPOILER: This choice may permanently alter faction relationships.", 0, 5.0f);
            Add(list, "survivor_spoiler", "spoiler", "warning", "SPOILER: This action may result in a survivor's death.", 0, 5.0f);
            Add(list, "technology_spoiler", "spoiler", "warning", "SPOILER: This technology has hidden consequences. Proceed with caution.", 0, 5.0f);
            Add(list, "quest_spoiler", "spoiler", "warning", "SPOILER: This quest has branching paths with different outcomes.", 0, 5.0f);
            Add(list, "world_spoiler", "spoiler", "warning", "SPOILER: This choice may permanently change the game world.", 0, 5.0f);
            Add(list, "secret_spoiler", "spoiler", "warning", "SPOILER: You're about to uncover a hidden secret. Continue?", 0, 5.0f);
            Add(list, "ending_choice", "spoiler", "warning", "SPOILER: This is a major story choice. Think carefully before proceeding.", 0, 5.0f);
            Add(list, "final_consequence", "spoiler", "warning", "SPOILER: This action has irreversible consequences. Are you certain?", 0, 5.0f);

            // Time Pressure (10)
            Add(list, "storm_countdown", "time_pressure", "critical", "HURRY! Storm arrives in {0} hours. {1} tasks remaining.", 2, 4.5f);
            Add(list, "raid_countdown", "time_pressure", "critical", "HURRY! Raiders attack in {0} minutes. Prepare defenses now.", 1, 4.5f);
            Add(list, "medical_emergency", "time_pressure", "critical", "HURRY! {0} is critical. {1} minutes until irreversible damage.", 2, 5.0f);
            Add(list, "power_failure", "time_pressure", "critical", "HURRY! Power failure in {0} minutes. Activate backup generator.", 1, 4.5f);
            Add(list, "food_shortage", "time_pressure", "critical", "HURRY! Food runs out in {0} days. Send scavengers immediately.", 1, 4.5f);
            Add(list, "water_contamination", "time_pressure", "critical", "HURRY! Water contaminated. {0} hours until unusable.", 1, 4.5f);
            Add(list, "radiation_spike", "time_pressure", "critical", "HURRY! Radiation spike detected. {0} minutes to find shelter.", 1, 4.5f);
            Add(list, "siege_imminent", "time_pressure", "critical", "HURRY! Siege begins in {0} hours. Fortify the bunker now.", 1, 4.5f);
            Add(list, "expedition_timeout", "time_pressure", "critical", "HURRY! Expedition deadline in {0} hours. Recall team if overdue.", 1, 4.5f);
            Add(list, "trade_deadline", "time_pressure", "critical", "HURRY! Trade deadline in {0} minutes. Meet at the rendezvous point.", 1, 4.5f);

            // Resource Warning (10)
            Add(list, "food_low", "resource_warning", "warning", "WARNING: Food supplies at {0}%. Assign scavengers to find more.", 1, 4.0f);
            Add(list, "water_low", "resource_warning", "warning", "WARNING: Water supplies at {0}%. Filter repairs needed.", 1, 4.0f);
            Add(list, "medical_low", "resource_warning", "warning", "WARNING: Medical supplies at {0}%. Prioritize critical cases.", 1, 4.0f);
            Add(list, "fuel_low", "resource_warning", "warning", "WARNING: Fuel at {0}%. Send expedition to scavenge.", 1, 4.0f);
            Add(list, "scrap_low", "resource_warning", "warning", "WARNING: Scrap metal at {0}%. Construction projects delayed.", 1, 4.0f);
            Add(list, "technology_low", "resource_warning", "warning", "WARNING: Technology at {0}%. Research projects stalled.", 1, 4.0f);
            Add(list, "ammo_low", "resource_warning", "warning", "WARNING: Ammunition at {0}%. Assign survivors to scavenge weapons.", 1, 4.0f);
            Add(list, "medicine_low", "resource_warning", "warning", "WARNING: Medicine at {0}%. Disease risk increases.", 1, 4.0f);
            Add(list, "fuel_critical", "resource_warning", "critical", "CRITICAL: Fuel at {0}%. Bunker operations at risk.", 1, 4.5f);
            Add(list, "food_critical", "resource_warning", "critical", "CRITICAL: Food at {0}%. Starvation imminent without action.", 1, 4.5f);

            // Health Warning (10)
            Add(list, "radiation_high", "health_warning", "critical", "DANGER: Radiation levels at {0} mSv/hr. Seek shelter immediately.", 1, 5.0f);
            Add(list, "injury_critical", "health_warning", "critical", "DANGER: {0} is critically injured. Medical attention required NOW.", 1, 5.0f);
            Add(list, "sickness_spreading", "health_warning", "critical", "DANGER: Disease spreading. Quarantine infected survivors.", 0, 5.0f);
            Add(list, "starvation_imminent", "health_warning", "critical", "DANGER: {0} is starving. Feed them immediately or they will die.", 1, 5.0f);
            Add(list, "dehydration_imminent", "health_warning", "critical", "DANGER: {0} is dehydrated. Provide water immediately or they will die.", 1, 5.0f);
            Add(list, "radiation_sickness", "health_warning", "critical", "DANGER: {0} has radiation sickness. Immediate medical treatment required.", 1, 5.0f);
            Add(list, "hypothermia_risk", "health_warning", "critical", "DANGER: Hypothermia risk high. Provide warmth immediately.", 0, 5.0f);
            Add(list, "heatstroke_risk", "health_warning", "critical", "DANGER: Heatstroke risk high. Provide cooling immediately.", 0, 5.0f);
            Add(list, "mental_break", "health_warning", "critical", "DANGER: {0} is having a mental breakdown. Provide support immediately.", 1, 5.0f);
            Add(list, "poisoning_risk", "health_warning", "critical", "DANGER: {0} has been poisoned. Provide antidote immediately.", 1, 5.0f);

            // Morale Warning (10)
            Add(list, "morale_critical", "morale_warning", "critical", "CRITICAL: Morale at {0}/100. Survivors may abandon the bunker.", 1, 5.0f);
            Add(list, "starvation_morale", "morale_warning", "warning", "Morale dropping: Survivors are hungry and restless.", 0, 4.0f);
            Add(list, "death_morale", "morale_warning", "warning", "Morale dropping: A survivor has died. Community in mourning.", 0, 4.5f);
            Add(list, "isolation_morale", "morale_warning", "warning", "Morale dropping: Bunker feels isolated. Survivors crave connection.", 0, 4.0f);
            Add(list, "fear_morale", "morale_warning", "warning", "Morale dropping: Radiation storms and raids increase fear.", 0, 4.0f);
            Add(list, "hope_low", "morale_warning", "warning", "Morale low: Survivors have lost hope for the future.", 0, 4.0f);
            Add(list, "celebration_needed", "morale_warning", "warning", "Morale low: Survivors need a celebration to boost spirits.", 0, 4.0f);
            Add(list, "leadership_doubt", "morale_warning", "warning", "Morale low: Survivors question leadership decisions.", 0, 4.0f);
            Add(list, "community_divided", "morale_warning", "warning", "Morale low: Survivors are divided and distrustful.", 0, 4.0f);
            Add(list, "survival_fatigue", "morale_warning", "warning", "Morale low: Survivors are tired of constant struggle.", 0, 4.0f);

            // Relationship (10)
            Add(list, "relationship_improved", "relationship", "info", "Your relationship with {0} has improved to {1}/100.", 2, 3.5f);
            Add(list, "relationship_damaged", "relationship", "info", "Your relationship with {0} has worsened to {1}/100.", 2, 3.5f);
            Add(list, "relationship_hostile", "relationship", "warning", "Your relationship with {0} is now hostile. Avoid contact.", 1, 4.0f);
            Add(list, "relationship_allies", "relationship", "info", "Your relationship with {0} is now allied. Trade and cooperation possible.", 1, 3.5f);
            Add(list, "relationship_trusting", "relationship", "info", "Your relationship with {0} is now trusting. They may share secrets.", 1, 3.5f);
            Add(list, "relationship_respected", "relationship", "info", "{0} now respects you. They may offer valuable information.", 1, 3.5f);
            Add(list, "relationship_fearful", "relationship", "warning", "{0} fears you. They may avoid or betray you.", 1, 4.0f);
            Add(list, "relationship_dependent", "relationship", "info", "{0} is now dependent on you. They may follow your lead.", 1, 3.5f);
            Add(list, "relationship_manipulated", "relationship", "warning", "You have manipulated {0}. They may seek revenge.", 1, 4.0f);
            Add(list, "relationship_betrayed", "relationship", "warning", "{0} feels betrayed. Trust has been broken.", 1, 4.0f);

            // Faction (10)
            Add(list, "faction_allies", "faction", "info", "You are now allied with {0}. Trade and cooperation possible.", 1, 3.5f);
            Add(list, "faction_enemies", "faction", "warning", "You are now enemies with {0}. Avoid their territory.", 1, 4.0f);
            Add(list, "faction_neutral", "faction", "info", "Your relationship with {0} is neutral. Proceed with caution.", 1, 3.5f);
            Add(list, "faction_trade_blocked", "faction", "warning", "Trade with {0} is now blocked. Find another partner.", 1, 4.0f);
            Add(list, "faction_support_gained", "faction", "info", "{0} now supports you. They may provide aid.", 1, 3.5f);
            Add(list, "faction_support_lost", "faction", "warning", "{0} has withdrawn support. You are on your own.", 1, 4.0f);
            Add(list, "faction_raid_imminent", "faction", "critical", "{0} is planning a raid. Fortify your defenses.", 1, 5.0f);
            Add(list, "faction_trade_improved", "faction", "info", "Trade terms with {0} have improved.", 1, 3.5f);
            Add(list, "faction_trade_worsened", "faction", "warning", "Trade terms with {0} have worsened.", 1, 4.0f);
            Add(list, "faction_alliance_broken", "faction", "warning", "Your alliance with {0} has been broken.", 1, 4.0f);

            // World State (10)
            Add(list, "storm_approaching", "world_state", "info", "The weather forecast predicts a radiation storm in {0} hours.", 1, 4.0f);
            Add(list, "season_changing", "world_state", "info", "The season is changing to {0}. Prepare for {1} conditions.", 2, 4.0f);
            Add(list, "radiation_increasing", "world_state", "warning", "Radiation levels are increasing across the wasteland.", 0, 4.0f);
            Add(list, "settlement_destroyed", "world_state", "warning", "The settlement at {0} has been destroyed. Survivors may seek refuge here.", 1, 4.5f);
            Add(list, "raider_activity_increasing", "world_state", "warning", "Raider activity is increasing in Sector {0}.", 1, 4.0f);
            Add(list, "trade_route_disrupted", "world_state", "warning", "The trade route to {0} has been disrupted by raiders.", 1, 4.0f);
            Add(list, "water_source_contaminated", "world_state", "warning", "The water source at {0} has been contaminated.", 1, 4.5f);
            Add(list, "food_source_depleted", "world_state", "warning", "The food source at {0} has been depleted.", 1, 4.0f);
            Add(list, "technology_scavenged", "world_state", "info", "A new technology cache has been scavenged at {0}.", 1, 4.0f);
            Add(list, "radiation_storm_passed", "world_state", "info", "The radiation storm has passed. Radiation levels are returning to normal.", 0, 4.0f);

            // System (10)
            Add(list, "save_success", "system", "info", "Game saved successfully.", 0, 2.5f);
            Add(list, "save_failed", "system", "error", "Failed to save game. Check your storage device.", 0, 4.0f);
            Add(list, "load_success", "system", "info", "Game loaded successfully.", 0, 2.5f);
            Add(list, "load_failed", "system", "error", "Failed to load game. File may be corrupted.", 0, 4.0f);
            Add(list, "backup_created", "system", "info", "Backup created successfully.", 0, 2.5f);
            Add(list, "backup_failed", "system", "error", "Failed to create backup. Check your storage device.", 0, 4.0f);
            Add(list, "update_available", "system", "info", "Update available! Version {0} is ready to install.", 1, 3.5f);
            Add(list, "update_failed", "system", "error", "Failed to install update. Check your connection.", 0, 4.0f);
            Add(list, "crash_recovered", "system", "warning", "Game recovered from crash. Some progress may be lost.", 0, 4.5f);
            Add(list, "performance_warning", "system", "warning", "Performance warning: {0}% CPU usage. Optimize your settings.", 1, 4.0f);

            return container;
        }

        private static void Add(List<FeedbackMessageTemplate> list, string key, string category, string severity, string template, int paramCount, float duration)
        {
            list.Add(new FeedbackMessageTemplate
            {
                key = key,
                category = category,
                severity = severity,
                template = template,
                parameter_count = paramCount,
                display_duration_seconds = duration
            });
        }
    }
}
