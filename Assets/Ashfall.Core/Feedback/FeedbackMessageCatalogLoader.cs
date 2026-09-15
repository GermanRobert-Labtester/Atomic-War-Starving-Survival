// SPDX-License-Identifier: MIT
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
                    ValidateContainer(container);
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
                    var container = new FeedbackMessageContainer { schema_version = 1, messages = list };
                    ValidateContainer(container);
                    return container;
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "<root>", ex);
            }

            var fallback = CreateDefaultContainer();
            ValidateContainer(fallback);
            return fallback;
        }

        public static void ValidateContainer(FeedbackMessageContainer container)
        {
            if (container?.messages == null) return;
            foreach (var msg in container.messages)
            {
                if (msg == null) continue;
                if (msg.display_duration_seconds <= 0f)
                    msg.display_duration_seconds = 3.0f;
                else
                    msg.display_duration_seconds = Math.Clamp(msg.display_duration_seconds, 1.0f, 15.0f);
            }
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
            Add(list, "quest_completed", "success", "success", "Task closed. Standing {0}. Stores {1}.", 2, 4.0f);
            Add(list, "survivor_recruited", "success", "success", "{0} has joined the shelter.", 1, 3.5f);
            Add(list, "resource_gained", "success", "success", "Received {0} {1}.", 2, 3.0f);
            Add(list, "technology_unlocked", "success", "success", "Research available: {0}.", 1, 4.0f);
            Add(list, "relationship_improved", "success", "success", "Standing with {0} improved.", 1, 3.5f);
            Add(list, "bunker_upgraded", "success", "success", "Shelter expanded. Capacity +{0}.", 1, 4.0f);
            Add(list, "medical_treatment_success", "success", "success", "Treatment took. {0} is better.", 1, 3.5f);
            Add(list, "trade_success", "success", "success", "Trade settled. Received {0} for {1}.", 2, 3.5f);
            Add(list, "alliance_formed", "success", "success", "Accord struck with {0}.", 1, 4.0f);
            Add(list, "expedition_success", "success", "success", "Team returned. Salvage: {0}.", 1, 4.0f);

            // Failure (10)
            Add(list, "quest_failed", "failure", "error", "Task failed. Standing −{0}. Morale −{1}.", 2, 4.0f);
            Add(list, "survivor_lost", "failure", "error", "{0} is dead. Their work dies with them.", 1, 4.5f);
            Add(list, "resource_lost", "failure", "error", "Lost {0} {1}.", 2, 3.0f);
            Add(list, "technology_lost", "failure", "error", "{0} is damaged and unusable.", 1, 4.0f);
            Add(list, "relationship_damaged", "failure", "error", "Standing with {0} worsened.", 1, 3.5f);
            Add(list, "bunker_damaged", "failure", "error", "Shelter took damage. Repairs needed.", 0, 4.0f);
            Add(list, "medical_failure", "failure", "error", "Treatment failed. {0} is worse.", 1, 3.5f);
            Add(list, "trade_failed", "failure", "error", "Trade failed. {0} took the goods and left.", 1, 4.0f);
            Add(list, "alliance_broken", "failure", "error", "Accord with {0} is broken.", 1, 4.0f);
            Add(list, "expedition_failed", "failure", "error", "Expedition failed. Nothing came back.", 0, 4.0f);

            // Warning (10)
            Add(list, "low_food", "warning", "warning", "Food is critically low. Tighten rations.", 0, 4.5f);
            Add(list, "low_medical", "warning", "warning", "Medical stores running out. Send someone to find more.", 0, 4.5f);
            Add(list, "low_fuel", "warning", "warning", "Fuel at {0}%. Send a team soon.", 1, 4.5f);
            Add(list, "high_radiation", "warning", "warning", "Outdoor radiation is high. Keep people inside.", 0, 4.5f);
            Add(list, "low_morale", "warning", "warning", "Morale is low. People are restless.", 0, 4.5f);
            Add(list, "bunker_deteriorating", "warning", "warning", "Shelter fabric is failing. Put people on repairs.", 0, 4.5f);
            Add(list, "disease_outbreak", "warning", "warning", "Sickness is spreading. Quarantine the infected.", 0, 5.0f);
            Add(list, "raider_activity", "warning", "warning", "Armed movement in Sector {0}.", 1, 4.5f);
            Add(list, "storm_approaching", "warning", "warning", "Storm in {0} hours. Seal the intake.", 1, 4.5f);
            Add(list, "power_critical", "warning", "warning", "Power is critical. The generator needs work now.", 0, 5.0f);

            // Error (10)
            Add(list, "invalid_action", "error", "error", "That action isn't available.", 0, 3.0f);
            Add(list, "insufficient_resources", "error", "error", "Not enough {0}.", 1, 3.5f);
            Add(list, "missing_id", "error", "error", "Missing ID '{0}'.", 1, 4.0f);
            Add(list, "system_overload", "error", "error", "System busy. Wait and try again.", 0, 4.0f);
            Add(list, "file_not_found", "error", "error", "File not found: {0}.", 1, 4.0f);
            Add(list, "corrupt_data", "error", "error", "Corrupt data in {0}.", 1, 4.5f);
            Add(list, "permission_denied", "error", "error", "You don't have access to that.", 0, 3.5f);
            Add(list, "network_error", "error", "error", "Cannot reach {0}.", 1, 4.0f);
            Add(list, "out_of_bounds", "error", "error", "Value out of range.", 0, 3.5f);
            Add(list, "invalid_input", "error", "error", "That value isn't valid.", 0, 3.0f);

            // Confirmation (10)
            Add(list, "delete_survivor", "confirmation", "warning", "Exile {0}? This cannot be undone.", 1, 5.0f);
            Add(list, "abandon_quest", "confirmation", "warning", "Abandon this task? Progress will be lost.", 0, 5.0f);
            Add(list, "use_medicine", "confirmation", "warning", "Use {0} on {1}? The dose is spent.", 2, 5.0f);
            Add(list, "scavenge_dangerous", "confirmation", "warning", "Send a team to this dangerous site?", 0, 5.0f);
            Add(list, "trade_with_faction", "confirmation", "warning", "Trade with {0}? Their word is thin.", 1, 5.0f);
            Add(list, "upgrade_bunker", "confirmation", "warning", "Expand the shelter? Cost: {0}.", 1, 5.0f);
            Add(list, "start_expedition", "confirmation", "warning", "Send this expedition? The team will be at risk.", 0, 5.0f);
            Add(list, "accept_alliance", "confirmation", "warning", "Accept this accord? It may cost more later.", 0, 5.0f);
            Add(list, "use_technology", "confirmation", "warning", "Use this unstable equipment? It may damage the shelter.", 0, 5.0f);
            Add(list, "close_bunker", "confirmation", "warning", "Seal the hatch? Anyone still outside is on their own.", 0, 5.0f);

            // Progress (10)
            Add(list, "quest_progress", "progress", "info", "Task: {0}% done. {1} remaining.", 2, 3.0f);
            Add(list, "construction_progress", "progress", "info", "Build: {0}% done. {1} days remaining.", 2, 3.0f);
            Add(list, "training_progress", "progress", "info", "Training: {0}% done. {1} people remaining.", 2, 3.0f);
            Add(list, "expedition_progress", "progress", "info", "Expedition: {0} days out. {1} days left. Distance: {2} km.", 3, 3.5f);
            Add(list, "medical_progress", "progress", "info", "Treatment: {0}% done. {1} remaining.", 2, 3.0f);
            Add(list, "repair_progress", "progress", "info", "Repair: {0}% done. {1} systems remaining.", 2, 3.0f);
            Add(list, "ration_progress", "progress", "info", "Rations: {0} days left. Restock in {1} days.", 2, 3.0f);
            Add(list, "morale_progress", "progress", "info", "Morale: {0}/100. {1} people affected.", 2, 3.0f);
            Add(list, "relationship_progress", "progress", "info", "Standing with {0}: {1}/100.", 2, 3.0f);
            Add(list, "resource_progress", "progress", "info", "{0}: {1}/{2} available.", 3, 3.0f);

            // Reward (10)
            Add(list, "reputation_gained", "reward", "success", "Standing with {1} +{0}.", 2, 3.5f);
            Add(list, "resource_reward", "reward", "success", "Received {0} {1}.", 2, 3.0f);
            Add(list, "item_reward", "reward", "success", "Received: {0}.", 1, 3.0f);
            Add(list, "technology_reward", "reward", "success", "Research available: {0}.", 1, 4.0f);
            Add(list, "skill_reward", "reward", "success", "{0} learned {1}.", 2, 3.5f);
            Add(list, "morale_boost", "reward", "success", "Morale +{0}.", 1, 3.0f);
            Add(list, "health_reward", "reward", "success", "{0}'s health +{1}.", 2, 3.0f);
            Add(list, "faction_reward", "reward", "success", "Accord with {0} holds firmer.", 1, 3.5f);
            Add(list, "experience_reward", "reward", "success", "{0} gained {1} experience.", 2, 3.0f);
            Add(list, "bonus_reward", "reward", "success", "Extra: {0}.", 1, 3.5f);

            // Penalty (10)
            Add(list, "reputation_lost", "penalty", "warning", "Standing with {1} −{0}.", 2, 3.5f);
            Add(list, "resource_penalty", "penalty", "warning", "Lost {0} {1}.", 2, 3.0f);
            Add(list, "item_penalty", "penalty", "warning", "Lost: {0}.", 1, 3.0f);
            Add(list, "technology_damaged", "penalty", "warning", "{0} is damaged and unusable.", 1, 4.0f);
            Add(list, "skill_penalty", "penalty", "warning", "{0} lost {1}.", 2, 3.5f);
            Add(list, "morale_penalty", "penalty", "warning", "Morale −{0}.", 1, 3.0f);
            Add(list, "health_penalty", "penalty", "warning", "{0}'s health −{1}.", 2, 3.0f);
            Add(list, "faction_penalty", "penalty", "warning", "Accord with {0} is thinner.", 1, 3.5f);
            Add(list, "experience_penalty", "penalty", "warning", "{0} lost {1} experience.", 2, 3.0f);
            Add(list, "time_penalty", "penalty", "warning", "Lost {0} days to delay.", 1, 3.5f);

            // Status (10)
            Add(list, "bunker_status", "status", "info", "Shelter: {0}/{1} capacity. {2}% integrity.", 3, 3.5f);
            Add(list, "survivor_status", "status", "info", "People: {0}/{1} alive. {2} injured.", 3, 3.5f);
            Add(list, "food_status", "status", "info", "Food: {0}/{1} days left. {2}% waste.", 3, 3.0f);
            Add(list, "water_status", "status", "info", "Water: {0}/{1} litres left. {2}% clean.", 3, 3.0f);
            Add(list, "medical_status", "status", "info", "Medical: {0}/{1} supplies left. {2} patients.", 3, 3.0f);
            Add(list, "fuel_status", "status", "info", "Fuel: {0}/{1} litres left. {2}% efficiency.", 3, 3.0f);
            Add(list, "power_status", "status", "info", "Power: {0}/{1}%. Generator: {2}.", 3, 3.0f);
            Add(list, "morale_status", "status", "info", "Morale: {0}/100. {1} people affected.", 2, 3.0f);
            Add(list, "radiation_status", "status", "info", "Radiation: {0} mSv/h outside. {1} mSv/h inside.", 2, 3.5f);
            Add(list, "weather_status", "status", "info", "Weather: {0}. Visibility: {1} km. {2}°C.", 3, 3.5f);

            // Alert (10)
            Add(list, "storm_alert", "alert", "critical", "ALERT: Fallout storm in {0} hours.", 1, 5.0f);
            Add(list, "raider_alert", "alert", "critical", "ALERT: Armed group near Sector {0}.", 1, 5.0f);
            Add(list, "disease_alert", "alert", "critical", "ALERT: Sickness in the shelter. Quarantine.", 0, 5.0f);
            Add(list, "fire_alert", "alert", "critical", "ALERT: Fire in {0}.", 1, 5.0f);
            Add(list, "intruder_alert", "alert", "critical", "ALERT: Intruder inside.", 0, 5.0f);
            Add(list, "power_alert", "alert", "critical", "ALERT: Power failure in {0}.", 1, 5.0f);
            Add(list, "radiation_alert", "alert", "critical", "ALERT: Radiation leak. Seal the rooms.", 0, 5.0f);
            Add(list, "food_alert", "alert", "critical", "ALERT: Food stores compromised. Check for contamination.", 0, 5.0f);
            Add(list, "water_alert", "alert", "critical", "ALERT: Water filter failing. Needs repair.", 0, 5.0f);
            Add(list, "medical_alert", "alert", "critical", "ALERT: Infirmary full. Critical cases first.", 0, 5.0f);

            // Hint (10)
            Add(list, "scavenge_hint", "hint", "info", "Low-radiation ground yields cleaner salvage.", 0, 4.0f);
            Add(list, "ration_hint", "hint", "info", "Equal rations keep morale. Unequal rations keep people alive.", 0, 4.0f);
            Add(list, "medical_hint", "hint", "info", "Put the best medic on the worst case.", 0, 4.0f);
            Add(list, "trade_hint", "hint", "info", "Trade with people you trust. Count the goods anyway.", 0, 4.0f);
            Add(list, "expedition_hint", "hint", "info", "Send experienced people on the hard roads.", 0, 4.0f);
            Add(list, "morale_hint", "hint", "info", "A small meal lifts the room more than a speech.", 0, 4.0f);
            Add(list, "repair_hint", "hint", "info", "Put hands on repairs before the rooms fail.", 0, 4.0f);
            Add(list, "radiation_hint", "hint", "info", "Kit up when the outdoor dose climbs.", 0, 4.0f);
            Add(list, "faction_hint", "hint", "info", "Don't owe everything to one faction.", 0, 4.0f);
            Add(list, "resource_hint", "hint", "info", "Stock before winter. The road closes.", 0, 4.0f);

            // Spoiler (10)
            Add(list, "major_reveal", "spoiler", "warning", "Once you open this, you cannot put it back. Continue?", 0, 5.0f);
            Add(list, "ending_spoiler", "spoiler", "warning", "This choice decides who is left standing. Continue?", 0, 5.0f);
            Add(list, "faction_spoiler", "spoiler", "warning", "This may settle standing with a faction for good.", 0, 5.0f);
            Add(list, "survivor_spoiler", "spoiler", "warning", "Someone may not come back from this.", 0, 5.0f);
            Add(list, "technology_spoiler", "spoiler", "warning", "This equipment has costs that are not on the label.", 0, 5.0f);
            Add(list, "quest_spoiler", "spoiler", "warning", "This task forks. The other path closes.", 0, 5.0f);
            Add(list, "world_spoiler", "spoiler", "warning", "The valley will not look the same after this.", 0, 5.0f);
            Add(list, "secret_spoiler", "spoiler", "warning", "You are about to read something that was meant to stay closed. Continue?", 0, 5.0f);
            Add(list, "ending_choice", "spoiler", "warning", "This is a lasting choice. Be sure.", 0, 5.0f);
            Add(list, "final_consequence", "spoiler", "warning", "This cannot be undone. Continue?", 0, 5.0f);

            // Time Pressure (10)
            Add(list, "storm_countdown", "time_pressure", "critical", "Storm in {0} hours. {1} tasks still open.", 2, 4.5f);
            Add(list, "raid_countdown", "time_pressure", "critical", "Attack in {0} minutes. Get people on the hatch.", 1, 4.5f);
            Add(list, "medical_emergency", "time_pressure", "critical", "{0} is critical. {1} minutes before it will not reverse.", 2, 5.0f);
            Add(list, "power_failure", "time_pressure", "critical", "Power fails in {0} minutes. Start the backup.", 1, 4.5f);
            Add(list, "food_shortage", "time_pressure", "critical", "Food runs out in {0} days. Send a team.", 1, 4.5f);
            Add(list, "water_contamination", "time_pressure", "critical", "Water is contaminated. {0} hours until it is unusable.", 1, 4.5f);
            Add(list, "radiation_spike", "time_pressure", "critical", "Radiation spike. {0} minutes to get under cover.", 1, 4.5f);
            Add(list, "siege_imminent", "time_pressure", "critical", "Siege in {0} hours. Bar the hatch.", 1, 4.5f);
            Add(list, "expedition_timeout", "time_pressure", "critical", "Expedition overdue in {0} hours. Recall them.", 1, 4.5f);
            Add(list, "trade_deadline", "time_pressure", "critical", "Trade window closes in {0} minutes.", 1, 4.5f);

            // Resource Warning (10)
            Add(list, "food_low", "resource_warning", "warning", "WARNING: Food at {0}%. Send someone for more.", 1, 4.0f);
            Add(list, "water_low", "resource_warning", "warning", "WARNING: Water at {0}%. The filter needs work.", 1, 4.0f);
            Add(list, "medical_low", "resource_warning", "warning", "WARNING: Medical stores at {0}%. Critical cases first.", 1, 4.0f);
            Add(list, "fuel_low", "resource_warning", "warning", "WARNING: Fuel at {0}%. Send a team.", 1, 4.0f);
            Add(list, "scrap_low", "resource_warning", "warning", "WARNING: Scrap at {0}%. Builds will wait.", 1, 4.0f);
            Add(list, "technology_low", "resource_warning", "warning", "WARNING: Research stock at {0}%. The queue is stalling.", 1, 4.0f);
            Add(list, "ammo_low", "resource_warning", "warning", "WARNING: Ammunition at {0}%. Send people for more.", 1, 4.0f);
            Add(list, "medicine_low", "resource_warning", "warning", "WARNING: Medicine at {0}%. Sickness will run.", 1, 4.0f);
            Add(list, "fuel_critical", "resource_warning", "critical", "CRITICAL: Fuel at {0}%. Shelter systems at risk.", 1, 4.5f);
            Add(list, "food_critical", "resource_warning", "critical", "CRITICAL: Food at {0}%. People will starve without a run.", 1, 4.5f);

            // Health Warning (10)
            Add(list, "radiation_high", "health_warning", "critical", "DANGER: Radiation {0} mSv/h. Get under cover.", 1, 5.0f);
            Add(list, "injury_critical", "health_warning", "critical", "DANGER: {0} is critically injured. Get them to Medical.", 1, 5.0f);
            Add(list, "sickness_spreading", "health_warning", "critical", "DANGER: Sickness spreading. Quarantine the infected.", 0, 5.0f);
            Add(list, "starvation_imminent", "health_warning", "critical", "DANGER: {0} is starving. Feed them or they die.", 1, 5.0f);
            Add(list, "dehydration_imminent", "health_warning", "critical", "DANGER: {0} is dehydrated. Water, or they die.", 1, 5.0f);
            Add(list, "radiation_sickness", "health_warning", "critical", "DANGER: {0} has radiation sickness. Treat them now.", 1, 5.0f);
            Add(list, "hypothermia_risk", "health_warning", "critical", "DANGER: Hypothermia risk. Get them warm.", 0, 5.0f);
            Add(list, "heatstroke_risk", "health_warning", "critical", "DANGER: Heatstroke risk. Get them cool.", 0, 5.0f);
            Add(list, "mental_break", "health_warning", "critical", "DANGER: {0} is breaking. Someone stay with them.", 1, 5.0f);
            Add(list, "poisoning_risk", "health_warning", "critical", "DANGER: {0} is poisoned. Get the antidote.", 1, 5.0f);

            // Morale Warning (10)
            Add(list, "morale_critical", "morale_warning", "critical", "CRITICAL: Morale at {0}/100. People may walk.", 1, 5.0f);
            Add(list, "starvation_morale", "morale_warning", "warning", "Morale dropping. People are hungry and pacing.", 0, 4.0f);
            Add(list, "death_morale", "morale_warning", "warning", "Morale dropping. Someone died. The room knows.", 0, 4.5f);
            Add(list, "isolation_morale", "morale_warning", "warning", "Morale dropping. The shelter feels cut off.", 0, 4.0f);
            Add(list, "fear_morale", "morale_warning", "warning", "Morale dropping. Storms and raids are wearing people down.", 0, 4.0f);
            Add(list, "hope_low", "morale_warning", "warning", "Morale low. People have stopped talking about later.", 0, 4.0f);
            Add(list, "celebration_needed", "morale_warning", "warning", "Morale low. A shared meal would help more than a speech.", 0, 4.0f);
            Add(list, "leadership_doubt", "morale_warning", "warning", "Morale low. People are questioning the roster.", 0, 4.0f);
            Add(list, "community_divided", "morale_warning", "warning", "Morale low. The room is splitting.", 0, 4.0f);
            Add(list, "survival_fatigue", "morale_warning", "warning", "Morale low. People are tired of the work.", 0, 4.0f);

            // Relationship (10)
            Add(list, "relationship_improved", "relationship", "info", "Standing with {0} is {1}/100.", 2, 3.5f);
            Add(list, "relationship_damaged", "relationship", "info", "Standing with {0} fell to {1}/100.", 2, 3.5f);
            Add(list, "relationship_hostile", "relationship", "warning", "Standing with {0} is hostile. Keep clear.", 1, 4.0f);
            Add(list, "relationship_allies", "relationship", "info", "Standing with {0} is allied. Trade is possible.", 1, 3.5f);
            Add(list, "relationship_trusting", "relationship", "info", "Standing with {0} is trusting. They may talk.", 1, 3.5f);
            Add(list, "relationship_respected", "relationship", "info", "{0} respects the shelter. They may pass word.", 1, 3.5f);
            Add(list, "relationship_fearful", "relationship", "warning", "{0} is afraid. They may keep clear — or turn.", 1, 4.0f);
            Add(list, "relationship_dependent", "relationship", "info", "{0} is depending on the shelter.", 1, 3.5f);
            Add(list, "relationship_manipulated", "relationship", "warning", "{0} knows they were used. They may not forget.", 1, 4.0f);
            Add(list, "relationship_betrayed", "relationship", "warning", "{0} feels betrayed. Trust is gone.", 1, 4.0f);

            // Faction (10)
            Add(list, "faction_allies", "faction", "info", "Allied with {0}. Trade is open.", 1, 3.5f);
            Add(list, "faction_enemies", "faction", "warning", "Enemies with {0}. Stay off their roads.", 1, 4.0f);
            Add(list, "faction_neutral", "faction", "info", "Standing with {0} is even. Watch the terms.", 1, 3.5f);
            Add(list, "faction_trade_blocked", "faction", "warning", "Trade with {0} is blocked.", 1, 4.0f);
            Add(list, "faction_support_gained", "faction", "info", "{0} will send aid — for now.", 1, 3.5f);
            Add(list, "faction_support_lost", "faction", "warning", "{0} has withdrawn support.", 1, 4.0f);
            Add(list, "faction_raid_imminent", "faction", "critical", "{0} is planning a raid. Bar the hatch.", 1, 5.0f);
            Add(list, "faction_trade_improved", "faction", "info", "Trade terms with {0} improved.", 1, 3.5f);
            Add(list, "faction_trade_worsened", "faction", "warning", "Trade terms with {0} worsened.", 1, 4.0f);
            Add(list, "faction_alliance_broken", "faction", "warning", "Accord with {0} is broken.", 1, 4.0f);

            // World State (10)
            Add(list, "storm_approaching", "world_state", "info", "Fallout storm due in {0} hours.", 1, 4.0f);
            Add(list, "season_changing", "world_state", "info", "Season turning to {0}. Expect {1}.", 2, 4.0f);
            Add(list, "radiation_increasing", "world_state", "warning", "Outdoor radiation is climbing.", 0, 4.0f);
            Add(list, "settlement_destroyed", "world_state", "warning", "The settlement at {0} is gone. People may come here.", 1, 4.5f);
            Add(list, "raider_activity_increasing", "world_state", "warning", "Armed traffic is rising in Sector {0}.", 1, 4.0f);
            Add(list, "trade_route_disrupted", "world_state", "warning", "The road to {0} is disrupted.", 1, 4.0f);
            Add(list, "water_source_contaminated", "world_state", "warning", "The water at {0} is contaminated.", 1, 4.5f);
            Add(list, "food_source_depleted", "world_state", "warning", "The food cache at {0} is empty.", 1, 4.0f);
            Add(list, "technology_scavenged", "world_state", "info", "A cache of parts was found at {0}.", 1, 4.0f);
            Add(list, "radiation_storm_passed", "world_state", "info", "The storm has passed. Outdoor dose is falling.", 0, 4.0f);

            // System (10)
            Add(list, "save_success", "system", "info", "Save written.", 0, 2.5f);
            Add(list, "save_failed", "system", "error", "Save failed. Check storage.", 0, 4.0f);
            Add(list, "load_success", "system", "info", "Save loaded.", 0, 2.5f);
            Add(list, "load_failed", "system", "error", "Load failed. The file may be damaged.", 0, 4.0f);
            Add(list, "backup_created", "system", "info", "Backup written.", 0, 2.5f);
            Add(list, "backup_failed", "system", "error", "Backup failed. Check storage.", 0, 4.0f);
            Add(list, "update_available", "system", "info", "Update available: version {0}.", 1, 3.5f);
            Add(list, "update_failed", "system", "error", "Update failed.", 0, 4.0f);
            Add(list, "crash_recovered", "system", "warning", "Recovered after a crash. Some progress may be missing.", 0, 4.5f);
            Add(list, "performance_warning", "system", "warning", "Machine load {0}%. Drop settings if it stutters.", 1, 4.0f);

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
