using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Feedback Messages System
    /// Provides standardized feedback messages for all game systems.
    /// </summary>
    public partial class FeedbackMessages : Node
    {
        private static readonly Dictionary<string, string> _successMessages = new Dictionary<string, string>
        {
            {"quest_completed", "Quest completed! You've earned {0} reputation and {1} resources."},
            {"survivor_recruited", "{0} has joined your bunker!"},
            {"resource_gained", "You've gained {0} {1}."},
            {"technology_unlocked", "New technology unlocked: {0}."},
            {"relationship_improved", "Your relationship with {0} has improved."},
            {"bunker_upgraded", "Bunker upgraded! Capacity increased by {0}."},
            {"medical_treatment_success", "Medical treatment successful. {0}'s condition improved."},
            {"trade_success", "Trade completed! Received {0} in exchange for {1}."},
            {"alliance_formed", "Alliance formed with {0}."},
            {"expedition_success", "Expedition returned safely with {0} resources."}
        };

        private static readonly Dictionary<string, string> _failureMessages = new Dictionary<string, string>
        {
            {"quest_failed", "Quest failed. You lost {0} reputation and {1} morale."},
            {"survivor_lost", "{0} has died. Their skills are lost to the bunker."},
            {"resource_lost", "You've lost {0} {1}."},
            {"technology_lost", "Technology {0} was damaged and is now unusable."},
            {"relationship_damaged", "Your relationship with {0} has been damaged."},
            {"bunker_damaged", "Bunker took damage! Repairs needed."},
            {"medical_failure", "Medical treatment failed. {0}'s condition worsened."},
            {"trade_failed", "Trade failed. {0} took your resources without delivering."},
            {"alliance_broken", "Alliance with {0} has been broken."},
            {"expedition_failed", "Expedition failed. No resources recovered."}
        };

        private static readonly Dictionary<string, string> _warningMessages = new Dictionary<string, string>
        {
            {"low_food", "Food supplies are critically low! Prioritize rationing."},
            {"low_medical", "Medical supplies are running out. Assign scavengers to find more."},
            {"low_fuel", "Fuel reserves are at {0}%. Send an expedition soon."},
            {"high_radiation", "Radiation levels are elevated. Avoid outdoor activities."},
            {"low_morale", "Morale is dangerously low. Survivors are restless."},
            {"bunker_deteriorating", "Bunker structures are deteriorating. Assign laborers to repairs."},
            {"disease_outbreak", "A disease is spreading. Quarantine infected survivors immediately."},
            {"raider_activity", "Raider activity detected in Sector {0}. Stay alert."},
            {"storm_approaching", "A storm is approaching in {0} hours. Prepare the bunker."},
            {"power_critical", "Power levels are critical. Generator needs immediate attention."}
        };

        private static readonly Dictionary<string, string> _errorMessages = new Dictionary<string, string>
        {
            {"invalid_action", "Invalid action. Please try again."},
            {"insufficient_resources", "Insufficient {0} to complete this action."},
            {"missing_id", "Error: Missing ID '{0}'. Check your data files."},
            {"system_overload", "System overload detected. Please wait and try again."},
            {"file_not_found", "File not found: {0}. Check your installation."},
            {"corrupt_data", "Corrupt data detected in {0}. The file may be damaged."},
            {"permission_denied", "Permission denied. You don't have access to this action."},
            {"network_error", "Network error. Unable to connect to {0}."},
            {"out_of_bounds", "Error: Value out of bounds. Check your inputs."},
            {"invalid_input", "Invalid input. Please enter a valid value."}
        };

        private static readonly Dictionary<string, string> _confirmationMessages = new Dictionary<string, string>
        {
            {"delete_survivor", "Are you sure you want to exile {0}? This cannot be undone."},
            {"abandon_quest", "Are you sure you want to abandon this quest? Progress will be lost."},
            {"use_medicine", "Are you sure you want to use {0} medicine on {1}? This cannot be undone."},
            {"scavenge_dangerous", "Are you sure you want to send an expedition to this dangerous location?"},
            {"trade_with_faction", "Are you sure you want to trade with {0}? Their reputation is questionable."},
            {"upgrade_bunker", "Are you sure you want to upgrade the bunker? This will cost {0} resources."},
            {"start_expedition", "Are you sure you want to start this expedition? Survivors will be at risk."},
            {"accept_alliance", "Are you sure you want to accept this alliance? It may have hidden costs."},
            {"use_technology", "Are you sure you want to use this unstable technology? It may cause damage."},
            {"close_bunker", "Are you sure you want to close the bunker? Survivors outside will be at risk."}
        };

        private static readonly Dictionary<string, string> _progressMessages = new Dictionary<string, string>
        {
            {"quest_progress", "Quest progress: {0}% complete. {1} remaining."},
            {"construction_progress", "Construction progress: {0}% complete. {1} days remaining."},
            {"training_progress", "Training progress: {0}% complete. {1} survivors remaining."},
            {
                "expedition_progress",
                "Expedition progress: {0} days elapsed. {1} days remaining. Distance: {2} km."
            },
            {"medical_progress", "Medical treatment: {0}% complete. {1} remaining."},
            {"repair_progress", "Repair progress: {0}% complete. {1} systems remaining."},
            {"ration_progress", "Rationing: {0} days of supplies remaining. {1} days until restock."},
            {"morale_progress", "Morale: {0}/100. {1} survivors affected."},
            {"relationship_progress", "Relationship with {0}: {1}/100."},
            {"resource_progress", "{0}: {1}/{2} available."}
        };

        private static readonly Dictionary<string, string> _rewardMessages = new Dictionary<string, string>
        {
            {"reputation_gained", "You've gained {0} reputation with {1}."},
            {"resource_reward", "You've earned {0} {1}."},
            {"item_reward", "You've received: {0}."},
            {"technology_reward", "New technology unlocked: {0}."},
            {"skill_reward", "{0} has learned a new skill: {1}."},
            {"morale_boost", "Morale increased by {0} points."},
            {"health_reward", "{0}'s health improved by {1} points."},
            {"faction_reward", "Your alliance with {0} has strengthened."},
            {"experience_reward", "{0} gained {1} experience points."},
            {"bonus_reward", "Bonus reward: {0}."}
        };

        private static readonly Dictionary<string, string> _penaltyMessages = new Dictionary<string, string>
        {
            {"reputation_lost", "You've lost {0} reputation with {1}."},
            {"resource_penalty", "You've lost {0} {1}."},
            {"item_penalty", "You've lost: {0}."},
            {"technology_damaged", "Technology {0} was damaged and is now unusable."},
            {"skill_penalty", "{0} lost the skill: {1}."},
            {"morale_penalty", "Morale decreased by {0} points."},
            {"health_penalty", "{0}'s health decreased by {1} points."},
            {"faction_penalty", "Your alliance with {0} has weakened."},
            {"experience_penalty", "{0} lost {1} experience points."},
            {"time_penalty", "You've lost {0} days due to delays."}
        };

        private static readonly Dictionary<string, string> _statusMessages = new Dictionary<string, string>
        {
            {"bunker_status", "Bunker: {0}/{1} capacity. {2}% structural integrity."},
            {"survivor_status", "Survivors: {0}/{1} alive. {2} injured."},
            {"food_status", "Food: {0}/{1} days remaining. {2}% waste."},
            {"water_status", "Water: {0}/{1} liters remaining. {2}% clean."},
            {"medical_status", "Medical: {0}/{1} supplies remaining. {2} patients."},
            {"fuel_status", "Fuel: {0}/{1} liters remaining. {2}% efficiency."},
            {"power_status", "Power: {0}/{1}%. Generator status: {2}."},
            {"morale_status", "Morale: {0}/100. {1} survivors affected."},
            {"radiation_status", "Radiation: {0} mSv/hr outside. {1} mSv/hr inside."},
            {"weather_status", "Weather: {0}. Visibility: {1} km. Temperature: {2}°C."}
        };

        private static readonly Dictionary<string, string> _alertMessages = new Dictionary<string, string>
        {
            {"storm_alert", "ALERT: Radiation storm approaching in {0} hours!"},
            {"raider_alert", "ALERT: Raiders detected near Sector {0}!"},
            {"disease_alert", "ALERT: Disease outbreak detected! Quarantine required."},
            {"fire_alert", "ALERT: Fire in {0}! Evacuate if necessary."},
            {"intruder_alert", "ALERT: Intruder detected in the bunker!"},
            {"power_alert", "ALERT: Power failure in {0}! Emergency protocols activated."},
            {"radiation_alert", "ALERT: Radiation leak detected! Seal affected areas immediately."},
            {"food_alert", "ALERT: Food storage compromised! Check for contamination."},
            {"water_alert", "ALERT: Water filtration system failing! Repairs needed."},
            {"medical_alert", "ALERT: Medical bay at capacity! Prioritize critical cases."}
        };

        private static readonly Dictionary<string, string> _hintMessages = new Dictionary<string, string>
        {
            {"scavenge_hint", "Tip: Scavenge in areas with low radiation for better results."},
            {"ration_hint", "Tip: Equal rationing maintains morale, but priority rationing saves lives."},
            {"medical_hint", "Tip: Assign your best medics to critical cases to improve survival rates."},
            {"trade_hint", "Tip: Trade with factions you trust, but always verify their claims."},
            {"expedition_hint", "Tip: Send experienced survivors on dangerous expeditions."},
            {"morale_hint", "Tip: Small celebrations boost morale more than large ones."},
            {"repair_hint", "Tip: Assign laborers to repair bunker structures to prevent deterioration."},
            {"radiation_hint", "Tip: Use protective gear when radiation levels are elevated."},
            {"faction_hint", "Tip: Maintain good relationships with multiple factions to avoid isolation."},
            {"resource_hint", "Tip: Stockpile resources before winter to avoid shortages."}
        };

        private static readonly Dictionary<string, string> _spoilerMessages = new Dictionary<string, string>
        {
            {"major_reveal", "SPOILER WARNING: This action will reveal major plot points. Continue?"},
            {"ending_spoiler", "SPOILER: This choice affects the game's ending. Are you sure?"},
            {"faction_spoiler", "SPOILER: This choice may permanently alter faction relationships."},
            {"survivor_spoiler", "SPOILER: This action may result in a survivor's death."},
            {"technology_spoiler", "SPOILER: This technology has hidden consequences. Proceed with caution."},
            {"quest_spoiler", "SPOILER: This quest has branching paths with different outcomes."},
            {"world_spoiler", "SPOILER: This choice may permanently change the game world."},
            {"secret_spoiler", "SPOILER: You're about to uncover a hidden secret. Continue?"},
            {"ending_choice", "SPOILER: This is a major story choice. Think carefully before proceeding."},
            {"final_consequence", "SPOILER: This action has irreversible consequences. Are you certain?"}
        };

        private static readonly Dictionary<string, string> _timePressureMessages = new Dictionary<string, string>
        {
            {"storm_countdown", "HURRY! Storm arrives in {0} hours. {1} tasks remaining."},
            {"raid_countdown", "HURRY! Raiders attack in {0} minutes. Prepare defenses now."},
            {"medical_emergency", "HURRY! {0} is critical. {1} minutes until irreversible damage."},
            {"power_failure", "HURRY! Power failure in {0} minutes. Activate backup generator."},
            {"food_shortage", "HURRY! Food runs out in {0} days. Send scavengers immediately."},
            {"water_contamination", "HURRY! Water contaminated. {0} hours until unusable."},
            {"radiation_spike", "HURRY! Radiation spike detected. {0} minutes to find shelter."},
            {"siege_imminent", "HURRY! Siege begins in {0} hours. Fortify the bunker now."},
            {"expedition_timeout", "HURRY! Expedition deadline in {0} hours. Recall team if overdue."},
            {"trade_deadline", "HURRY! Trade deadline in {0} minutes. Meet at the rendezvous point."}
        };

        private static readonly Dictionary<string, string> _resourceWarnings = new Dictionary<string, string>
        {
            {"food_low", "WARNING: Food supplies at {0}%. Assign scavengers to find more."},
            {"water_low", "WARNING: Water supplies at {0}%. Filter repairs needed."},
            {"medical_low", "WARNING: Medical supplies at {0}%. Prioritize critical cases."},
            {"fuel_low", "WARNING: Fuel at {0}%. Send expedition to scavenge."},
            {"scrap_low", "WARNING: Scrap metal at {0}%. Construction projects delayed."},
            {"technology_low", "WARNING: Technology at {0}%. Research projects stalled."},
            {"ammo_low", "WARNING: Ammunition at {0}%. Assign survivors to scavenge weapons."},
            {"medicine_low", "WARNING: Medicine at {0}%. Disease risk increases."},
            {"fuel_critical", "CRITICAL: Fuel at {0}%. Bunker operations at risk."},
            {"food_critical", "CRITICAL: Food at {0}%. Starvation imminent without action."}
        };

        private static readonly Dictionary<string, string> _healthWarnings = new Dictionary<string, string>
        {
            {"radiation_high", "DANGER: Radiation levels at {0} mSv/hr. Seek shelter immediately."},
            {"injury_critical", "DANGER: {0} is critically injured. Medical attention required NOW."},
            {"sickness_spreading", "DANGER: Disease spreading. Quarantine infected survivors."},
            {"starvation_imminent", "DANGER: {0} is starving. Feed them immediately or they will die."},
            {"dehydration_imminent", "DANGER: {0} is dehydrated. Provide water immediately or they will die."},
            {
                "radiation_sickness",
                "DANGER: {0} has radiation sickness. Immediate medical treatment required."
            },
            {"hypothermia_risk", "DANGER: Hypothermia risk high. Provide warmth immediately."},
            {"heatstroke_risk", "DANGER: Heatstroke risk high. Provide cooling immediately."},
            {"mental_break", "DANGER: {0} is having a mental breakdown. Provide support immediately."},
            {"poisoning_risk", "DANGER: {0} has been poisoned. Provide antidote immediately."}
        };

        private static readonly Dictionary<string, string> _moraleWarnings = new Dictionary<string, string>
        {
            {"morale_critical", "CRITICAL: Morale at {0}/100. Survivors may abandon the bunker."},
            {"starvation_morale", "Morale dropping: Survivors are hungry and restless."},
            {"death_morale", "Morale dropping: A survivor has died. Community in mourning."},
            {"isolation_morale", "Morale dropping: Bunker feels isolated. Survivors crave connection."},
            {"fear_morale", "Morale dropping: Radiation storms and raids increase fear."},
            {"hope_low", "Morale low: Survivors have lost hope for the future."},
            {"celebration_needed", "Morale low: Survivors need a celebration to boost spirits."},
            {"leadership_doubt", "Morale low: Survivors question leadership decisions."},
            {"community_divided", "Morale low: Survivors are divided and distrustful."},
            {"survival_fatigue", "Morale low: Survivors are tired of constant struggle."}
        };

        private static readonly Dictionary<string, string> _relationshipMessages = new Dictionary<string, string>
        {
            {"relationship_improved", "Your relationship with {0} has improved to {1}/100."},
            {"relationship_damaged", "Your relationship with {0} has worsened to {1}/100."},
            {"relationship_hostile", "Your relationship with {0} is now hostile. Avoid contact."},
            {"relationship_allies", "Your relationship with {0} is now allied. Trade and cooperation possible."},
            {"relationship_trusting", "Your relationship with {0} is now trusting. They may share secrets."},
            {"relationship_respected", "{0} now respects you. They may offer valuable information."},
            {"relationship_fearful", "{0} fears you. They may avoid or betray you."},
            {"relationship_dependent", "{0} is now dependent on you. They may follow your lead."},
            {"relationship_manipulated", "You have manipulated {0}. They may seek revenge."},
            {"relationship_betrayed", "{0} feels betrayed. Trust has been broken."}
        };

        private static readonly Dictionary<string, string> _factionMessages = new Dictionary<string, string>
        {
            {"faction_allies", "You are now allied with {0}. Trade and cooperation possible."},
            {"faction_enemies", "You are now enemies with {0}. Avoid their territory."},
            {"faction_neutral", "Your relationship with {0} is neutral. Proceed with caution."},
            {"faction_trade_blocked", "Trade with {0} is now blocked. Find another partner."},
            {"faction_support_gained", "{0} now supports you. They may provide aid."},
            {"faction_support_lost", "{0} has withdrawn support. You are on your own."},
            {"faction_raid_imminent", "{0} is planning a raid. Fortify your defenses."},
            {"faction_trade_improved", "Trade terms with {0} have improved."},
            {"faction_trade_worsened", "Trade terms with {0} have worsened."},
            {"faction_alliance_broken", "Your alliance with {0} has been broken."}
        };

        private static readonly Dictionary<string, string> _worldStateMessages = new Dictionary<string, string>
        {
            {"storm_approaching", "The weather forecast predicts a radiation storm in {0} hours."},
            {"season_changing", "The season is changing to {0}. Prepare for {1} conditions."},
            {"radiation_increasing", "Radiation levels are increasing across the wasteland."},
            {
                "settlement_destroyed",
                "The settlement at {0} has been destroyed. Survivors may seek refuge here."
            },
            {"raider_activity_increasing", "Raider activity is increasing in Sector {0}."},
            {"trade_route_disrupted", "The trade route to {0} has been disrupted by raiders."},
            {"water_source_contaminated", "The water source at {0} has been contaminated."},
            {"food_source_depleted", "The food source at {0} has been depleted."},
            {"technology_scavenged", "A new technology cache has been scavenged at {0}."},
            {"radiation_storm_passed", "The radiation storm has passed. Radiation levels are returning to normal."}
        };

        private static readonly Dictionary<string, string> _systemMessages = new Dictionary<string, string>
        {
            {"save_success", "Game saved successfully."},
            {"save_failed", "Failed to save game. Check your storage device."},
            {"load_success", "Game loaded successfully."},
            {"load_failed", "Failed to load game. File may be corrupted."},
            {"backup_created", "Backup created successfully."},
            {"backup_failed", "Failed to create backup. Check your storage device."},
            {"update_available", "Update available! Version {0} is ready to install."},
            {"update_failed", "Failed to install update. Check your connection."},
            {"crash_recovered", "Game recovered from crash. Some progress may be lost."},
            {"performance_warning", "Performance warning: {0}% CPU usage. Optimize your settings."}
        };

        /// <summary>
        /// Gets a success message by key.
        /// </summary>
        public static string GetSuccessMessage(string key, params object[] args)
        {
            if (_successMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Success! Operation completed.";
        }

        /// <summary>
        /// Gets a failure message by key.
        /// </summary>
        public static string GetFailureMessage(string key, params object[] args)
        {
            if (_failureMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Operation failed. Check your inputs.";
        }

        /// <summary>
        /// Gets a warning message by key.
        /// </summary>
        public static string GetWarningMessage(string key, params object[] args)
        {
            if (_warningMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Warning: Proceed with caution.";
        }

        /// <summary>
        /// Gets an error message by key.
        /// </summary>
        public static string GetErrorMessage(string key, params object[] args)
        {
            if (_errorMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Error: Something went wrong.";
        }

        /// <summary>
        /// Gets a confirmation message by key.
        /// </summary>
        public static string GetConfirmationMessage(string key, params object[] args)
        {
            if (_confirmationMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Are you sure you want to proceed?";
        }

        /// <summary>
        /// Gets a progress message by key.
        /// </summary>
        public static string GetProgressMessage(string key, params object[] args)
        {
            if (_progressMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Progress: {0}% complete.";
        }

        /// <summary>
        /// Gets a reward message by key.
        /// </summary>
        public static string GetRewardMessage(string key, params object[] args)
        {
            if (_rewardMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "You've earned a reward!";
        }

        /// <summary>
        /// Gets a penalty message by key.
        /// </summary>
        public static string GetPenaltyMessage(string key, params object[] args)
        {
            if (_penaltyMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Penalty incurred. Check your status.";
        }

        /// <summary>
        /// Gets a status message by key.
        /// </summary>
        public static string GetStatusMessage(string key, params object[] args)
        {
            if (_statusMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Status: Normal.";
        }

        /// <summary>
        /// Gets an alert message by key.
        /// </summary>
        public static string GetAlertMessage(string key, params object[] args)
        {
            if (_alertMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "ALERT: Important update available!";
        }

        /// <summary>
        /// Gets a hint message by key.
        /// </summary>
        public static string GetHintMessage(string key, params object[] args)
        {
            if (_hintMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Tip: Check your surroundings for clues.";
        }

        /// <summary>
        /// Gets a spoiler message by key.
        /// </summary>
        public static string GetSpoilerMessage(string key, params object[] args)
        {
            if (_spoilerMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "SPOILER WARNING: This action may reveal major plot points.";
        }

        /// <summary>
        /// Gets a time pressure message by key.
        /// </summary>
        public static string GetTimePressureMessage(string key, params object[] args)
        {
            if (_timePressureMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "HURRY! Time is running out!";
        }

        /// <summary>
        /// Gets a resource warning by key.
        /// </summary>
        public static string GetResourceWarning(string key, params object[] args)
        {
            if (_resourceWarnings.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "WARNING: Resource levels are critical!";
        }

        /// <summary>
        /// Gets a health warning by key.
        /// </summary>
        public static string GetHealthWarning(string key, params object[] args)
        {
            if (_healthWarnings.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "DANGER: Health critical!";
        }

        /// <summary>
        /// Gets a morale warning by key.
        /// </summary>
        public static string GetMoraleWarning(string key, params object[] args)
        {
            if (_moraleWarnings.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "WARNING: Morale is dangerously low!";
        }

        /// <summary>
        /// Gets a relationship message by key.
        /// </summary>
        public static string GetRelationshipMessage(string key, params object[] args)
        {
            if (_relationshipMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Relationship status updated.";
        }

        /// <summary>
        /// Gets a faction message by key.
        /// </summary>
        public static string GetFactionMessage(string key, params object[] args)
        {
            if (_factionMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Faction relationship updated.";
        }

        /// <summary>
        /// Gets a world state message by key.
        /// </summary>
        public static string GetWorldStateMessage(string key, params object[] args)
        {
            if (_worldStateMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "World state updated.";
        }

        /// <summary>
        /// Gets a system message by key.
        /// </summary>
        public static string GetSystemMessage(string key, params object[] args)
        {
            if (_systemMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "System status updated.";
        }
    }
}