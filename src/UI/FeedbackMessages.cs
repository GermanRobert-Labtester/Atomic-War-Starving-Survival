// SPDX-License-Identifier: MIT
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
            {"quest_completed", "Task closed. Standing {0}. Stores {1}."},
            {"survivor_recruited", "{0} has joined the shelter."},
            {"resource_gained", "Received {0} {1}."},
            {"technology_unlocked", "Research available: {0}."},
            {"relationship_improved", "Standing with {0} improved."},
            {"bunker_upgraded", "Shelter expanded. Capacity +{0}."},
            {"medical_treatment_success", "Treatment took. {0} is better."},
            {"trade_success", "Trade settled. Received {0} for {1}."},
            {"alliance_formed", "Accord struck with {0}."},
            {"expedition_success", "Team returned. Salvage: {0}."}
        };

        private static readonly Dictionary<string, string> _failureMessages = new Dictionary<string, string>
        {
            {"quest_failed", "Task failed. Standing −{0}. Morale −{1}."},
            {"survivor_lost", "{0} is dead. Their work dies with them."},
            {"resource_lost", "Lost {0} {1}."},
            {"technology_lost", "{0} is damaged and unusable."},
            {"relationship_damaged", "Standing with {0} worsened."},
            {"bunker_damaged", "Shelter took damage. Repairs needed."},
            {"medical_failure", "Treatment failed. {0} is worse."},
            {"trade_failed", "Trade failed. {0} took the goods and left."},
            {"alliance_broken", "Accord with {0} is broken."},
            {"expedition_failed", "Expedition failed. Nothing came back."}
        };

        private static readonly Dictionary<string, string> _warningMessages = new Dictionary<string, string>
        {
            {"low_food", "Food is critically low. Tighten rations."},
            {"low_medical", "Medical stores running out. Send someone to find more."},
            {"low_fuel", "Fuel at {0}%. Send a team soon."},
            {"high_radiation", "Outdoor radiation is high. Keep people inside."},
            {"low_morale", "Morale is low. People are restless."},
            {"bunker_deteriorating", "Shelter fabric is failing. Put people on repairs."},
            {"disease_outbreak", "Sickness is spreading. Quarantine the infected."},
            {"raider_activity", "Armed movement in Sector {0}."},
            {"storm_approaching", "Storm in {0} hours. Seal the intake."},
            {"power_critical", "Power is critical. The generator needs work now."}
        };

        private static readonly Dictionary<string, string> _errorMessages = new Dictionary<string, string>
        {
            {"invalid_action", "That action isn't available."},
            {"insufficient_resources", "Not enough {0}."},
            {"missing_id", "Missing ID '{0}'."},
            {"system_overload", "System busy. Wait and try again."},
            {"file_not_found", "File not found: {0}."},
            {"corrupt_data", "Corrupt data in {0}."},
            {"permission_denied", "You don't have access to that."},
            {"network_error", "Cannot reach {0}."},
            {"out_of_bounds", "Value out of range."},
            {"invalid_input", "That value isn't valid."}
        };

        private static readonly Dictionary<string, string> _confirmationMessages = new Dictionary<string, string>
        {
            {"delete_survivor", "Exile {0}? This cannot be undone."},
            {"abandon_quest", "Abandon this task? Progress will be lost."},
            {"use_medicine", "Use {0} on {1}? The dose is spent."},
            {"scavenge_dangerous", "Send a team to this dangerous site?"},
            {"trade_with_faction", "Trade with {0}? Their word is thin."},
            {"upgrade_bunker", "Expand the shelter? Cost: {0}."},
            {"start_expedition", "Send this expedition? The team will be at risk."},
            {"accept_alliance", "Accept this accord? It may cost more later."},
            {"use_technology", "Use this unstable equipment? It may damage the shelter."},
            {"close_bunker", "Seal the hatch? Anyone still outside is on their own."}
        };

        private static readonly Dictionary<string, string> _progressMessages = new Dictionary<string, string>
        {
            {"quest_progress", "Task: {0}% done. {1} remaining."},
            {"construction_progress", "Build: {0}% done. {1} days remaining."},
            {"training_progress", "Training: {0}% done. {1} people remaining."},
            {
                "expedition_progress",
                "Expedition: {0} days out. {1} days left. Distance: {2} km."
            },
            {"medical_progress", "Treatment: {0}% done. {1} remaining."},
            {"repair_progress", "Repair: {0}% done. {1} systems remaining."},
            {"ration_progress", "Rations: {0} days left. Restock in {1} days."},
            {"morale_progress", "Morale: {0}/100. {1} people affected."},
            {"relationship_progress", "Standing with {0}: {1}/100."},
            {"resource_progress", "{0}: {1}/{2} available."}
        };

        private static readonly Dictionary<string, string> _rewardMessages = new Dictionary<string, string>
        {
            {"reputation_gained", "Standing with {1} +{0}."},
            {"resource_reward", "Received {0} {1}."},
            {"item_reward", "Received: {0}."},
            {"technology_reward", "Research available: {0}."},
            {"skill_reward", "{0} learned {1}."},
            {"morale_boost", "Morale +{0}."},
            {"health_reward", "{0}'s health +{1}."},
            {"faction_reward", "Accord with {0} holds firmer."},
            {"experience_reward", "{0} gained {1} experience."},
            {"bonus_reward", "Extra: {0}."}
        };

        private static readonly Dictionary<string, string> _penaltyMessages = new Dictionary<string, string>
        {
            {"reputation_lost", "Standing with {1} −{0}."},
            {"resource_penalty", "Lost {0} {1}."},
            {"item_penalty", "Lost: {0}."},
            {"technology_damaged", "{0} is damaged and unusable."},
            {"skill_penalty", "{0} lost {1}."},
            {"morale_penalty", "Morale −{0}."},
            {"health_penalty", "{0}'s health −{1}."},
            {"faction_penalty", "Accord with {0} is thinner."},
            {"experience_penalty", "{0} lost {1} experience."},
            {"time_penalty", "Lost {0} days to delay."}
        };

        private static readonly Dictionary<string, string> _statusMessages = new Dictionary<string, string>
        {
            {"bunker_status", "Shelter: {0}/{1} capacity. {2}% integrity."},
            {"survivor_status", "People: {0}/{1} alive. {2} injured."},
            {"food_status", "Food: {0}/{1} days left. {2}% waste."},
            {"water_status", "Water: {0}/{1} litres left. {2}% clean."},
            {"medical_status", "Medical: {0}/{1} supplies left. {2} patients."},
            {"fuel_status", "Fuel: {0}/{1} litres left. {2}% efficiency."},
            {"power_status", "Power: {0}/{1}%. Generator: {2}."},
            {"morale_status", "Morale: {0}/100. {1} people affected."},
            {"radiation_status", "Radiation: {0} mSv/h outside. {1} mSv/h inside."},
            {"weather_status", "Weather: {0}. Visibility: {1} km. {2}°C."}
        };

        private static readonly Dictionary<string, string> _alertMessages = new Dictionary<string, string>
        {
            {"storm_alert", "ALERT: Fallout storm in {0} hours."},
            {"raider_alert", "ALERT: Armed group near Sector {0}."},
            {"disease_alert", "ALERT: Sickness in the shelter. Quarantine."},
            {"fire_alert", "ALERT: Fire in {0}."},
            {"intruder_alert", "ALERT: Intruder inside."},
            {"power_alert", "ALERT: Power failure in {0}."},
            {"radiation_alert", "ALERT: Radiation leak. Seal the rooms."},
            {"food_alert", "ALERT: Food stores compromised. Check for contamination."},
            {"water_alert", "ALERT: Water filter failing. Needs repair."},
            {"medical_alert", "ALERT: Infirmary full. Critical cases first."}
        };

        private static readonly Dictionary<string, string> _hintMessages = new Dictionary<string, string>
        {
            {"scavenge_hint", "Low-radiation ground yields cleaner salvage."},
            {"ration_hint", "Equal rations keep morale. Unequal rations keep people alive."},
            {"medical_hint", "Put the best medic on the worst case."},
            {"trade_hint", "Trade with people you trust. Count the goods anyway."},
            {"expedition_hint", "Send experienced people on the hard roads."},
            {"morale_hint", "A small meal lifts the room more than a speech."},
            {"repair_hint", "Put hands on repairs before the rooms fail."},
            {"radiation_hint", "Kit up when the outdoor dose climbs."},
            {"faction_hint", "Don't owe everything to one faction."},
            {"resource_hint", "Stock before winter. The road closes."}
        };

        private static readonly Dictionary<string, string> _spoilerMessages = new Dictionary<string, string>
        {
            {"major_reveal", "Once you open this, you cannot put it back. Continue?"},
            {"ending_spoiler", "This choice decides who is left standing. Continue?"},
            {"faction_spoiler", "This may settle standing with a faction for good."},
            {"survivor_spoiler", "Someone may not come back from this."},
            {"technology_spoiler", "This equipment has costs that are not on the label."},
            {"quest_spoiler", "This task forks. The other path closes."},
            {"world_spoiler", "The valley will not look the same after this."},
            {"secret_spoiler", "You are about to read something that was meant to stay closed. Continue?"},
            {"ending_choice", "This is a lasting choice. Be sure."},
            {"final_consequence", "This cannot be undone. Continue?"}
        };

        private static readonly Dictionary<string, string> _timePressureMessages = new Dictionary<string, string>
        {
            {"storm_countdown", "Storm in {0} hours. {1} tasks still open."},
            {"raid_countdown", "Attack in {0} minutes. Get people on the hatch."},
            {"medical_emergency", "{0} is critical. {1} minutes before it will not reverse."},
            {"power_failure", "Power fails in {0} minutes. Start the backup."},
            {"food_shortage", "Food runs out in {0} days. Send a team."},
            {"water_contamination", "Water is contaminated. {0} hours until it is unusable."},
            {"radiation_spike", "Radiation spike. {0} minutes to get under cover."},
            {"siege_imminent", "Siege in {0} hours. Bar the hatch."},
            {"expedition_timeout", "Expedition overdue in {0} hours. Recall them."},
            {"trade_deadline", "Trade window closes in {0} minutes."}
        };

        private static readonly Dictionary<string, string> _resourceWarnings = new Dictionary<string, string>
        {
            {"food_low", "WARNING: Food at {0}%. Send someone for more."},
            {"water_low", "WARNING: Water at {0}%. The filter needs work."},
            {"medical_low", "WARNING: Medical stores at {0}%. Critical cases first."},
            {"fuel_low", "WARNING: Fuel at {0}%. Send a team."},
            {"scrap_low", "WARNING: Scrap at {0}%. Builds will wait."},
            {"technology_low", "WARNING: Research stock at {0}%. The queue is stalling."},
            {"ammo_low", "WARNING: Ammunition at {0}%. Send people for more."},
            {"medicine_low", "WARNING: Medicine at {0}%. Sickness will run."},
            {"fuel_critical", "CRITICAL: Fuel at {0}%. Shelter systems at risk."},
            {"food_critical", "CRITICAL: Food at {0}%. People will starve without a run."}
        };

        private static readonly Dictionary<string, string> _healthWarnings = new Dictionary<string, string>
        {
            {"radiation_high", "DANGER: Radiation {0} mSv/h. Get under cover."},
            {"injury_critical", "DANGER: {0} is critically injured. Get them to Medical."},
            {"sickness_spreading", "DANGER: Sickness spreading. Quarantine the infected."},
            {"starvation_imminent", "DANGER: {0} is starving. Feed them or they die."},
            {"dehydration_imminent", "DANGER: {0} is dehydrated. Water, or they die."},
            {
                "radiation_sickness",
                "DANGER: {0} has radiation sickness. Treat them now."
            },
            {"hypothermia_risk", "DANGER: Hypothermia risk. Get them warm."},
            {"heatstroke_risk", "DANGER: Heatstroke risk. Get them cool."},
            {"mental_break", "DANGER: {0} is breaking. Someone stay with them."},
            {"poisoning_risk", "DANGER: {0} is poisoned. Get the antidote."}
        };

        private static readonly Dictionary<string, string> _moraleWarnings = new Dictionary<string, string>
        {
            {"morale_critical", "CRITICAL: Morale at {0}/100. People may walk."},
            {"starvation_morale", "Morale dropping. People are hungry and pacing."},
            {"death_morale", "Morale dropping. Someone died. The room knows."},
            {"isolation_morale", "Morale dropping. The shelter feels cut off."},
            {"fear_morale", "Morale dropping. Storms and raids are wearing people down."},
            {"hope_low", "Morale low. People have stopped talking about later."},
            {"celebration_needed", "Morale low. A shared meal would help more than a speech."},
            {"leadership_doubt", "Morale low. People are questioning the roster."},
            {"community_divided", "Morale low. The room is splitting."},
            {"survival_fatigue", "Morale low. People are tired of the work."}
        };

        private static readonly Dictionary<string, string> _relationshipMessages = new Dictionary<string, string>
        {
            {"relationship_improved", "Standing with {0} is {1}/100."},
            {"relationship_damaged", "Standing with {0} fell to {1}/100."},
            {"relationship_hostile", "Standing with {0} is hostile. Keep clear."},
            {"relationship_allies", "Standing with {0} is allied. Trade is possible."},
            {"relationship_trusting", "Standing with {0} is trusting. They may talk."},
            {"relationship_respected", "{0} respects the shelter. They may pass word."},
            {"relationship_fearful", "{0} is afraid. They may keep clear — or turn."},
            {"relationship_dependent", "{0} is depending on the shelter."},
            {"relationship_manipulated", "{0} knows they were used. They may not forget."},
            {"relationship_betrayed", "{0} feels betrayed. Trust is gone."}
        };

        private static readonly Dictionary<string, string> _factionMessages = new Dictionary<string, string>
        {
            {"faction_allies", "Allied with {0}. Trade is open."},
            {"faction_enemies", "Enemies with {0}. Stay off their roads."},
            {"faction_neutral", "Standing with {0} is even. Watch the terms."},
            {"faction_trade_blocked", "Trade with {0} is blocked."},
            {"faction_support_gained", "{0} will send aid — for now."},
            {"faction_support_lost", "{0} has withdrawn support."},
            {"faction_raid_imminent", "{0} is planning a raid. Bar the hatch."},
            {"faction_trade_improved", "Trade terms with {0} improved."},
            {"faction_trade_worsened", "Trade terms with {0} worsened."},
            {"faction_alliance_broken", "Accord with {0} is broken."}
        };

        private static readonly Dictionary<string, string> _worldStateMessages = new Dictionary<string, string>
        {
            {"storm_approaching", "Fallout storm due in {0} hours."},
            {"season_changing", "Season turning to {0}. Expect {1}."},
            {"radiation_increasing", "Outdoor radiation is climbing."},
            {
                "settlement_destroyed",
                "The settlement at {0} is gone. People may come here."
            },
            {"raider_activity_increasing", "Armed traffic is rising in Sector {0}."},
            {"trade_route_disrupted", "The road to {0} is disrupted."},
            {"water_source_contaminated", "The water at {0} is contaminated."},
            {"food_source_depleted", "The food cache at {0} is empty."},
            {"technology_scavenged", "A cache of parts was found at {0}."},
            {"radiation_storm_passed", "The storm has passed. Outdoor dose is falling."}
        };

        private static readonly Dictionary<string, string> _systemMessages = new Dictionary<string, string>
        {
            {"save_success", "Save written."},
            {"save_failed", "Save failed. Check storage."},
            {"load_success", "Save loaded."},
            {"load_failed", "Load failed. The file may be damaged."},
            {"backup_created", "Backup written."},
            {"backup_failed", "Backup failed. Check storage."},
            {"update_available", "Update available: version {0}."},
            {"update_failed", "Update failed."},
            {"crash_recovered", "Recovered after a crash. Some progress may be missing."},
            {"performance_warning", "Machine load {0}%. Drop settings if it stutters."}
        };

        public static Ashfall.Core.Feedback.IFeedbackService? Service { get; set; }

        public static bool Emit(Ashfall.Core.Feedback.FeedbackEvent evt)
        {
            return Service?.Emit(evt) ?? false;
        }

        public static bool Emit(string key, params object[] args)
        {
            return Service?.Emit(new Ashfall.Core.Feedback.FeedbackEvent(key, args)) ?? false;
        }

        /// <summary>
        /// Gets a success message by key.
        /// </summary>
        public static string GetSuccessMessage(string key, params object[] args)
        {
            if (Service?.Catalog != null && Service.Catalog.TryGetTemplate("success", key, out var tmpl) && tmpl != null)
            {
                return Ashfall.Core.Feedback.FeedbackMessageCatalog.SafeFormat(tmpl.template, args);
            }
            if (_successMessages.TryGetValue(key, out var message))
            {
                return string.Format(message, args);
            }
            return "Done.";
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
            return "That didn't take. Check the inputs.";
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
            return "Caution.";
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
            return "Something went wrong.";
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
            return "Continue?";
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
            return "Progress: {0}%.";
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
            return "Received a reward.";
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
            return "A cost was taken. Check the ledger.";
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
            return "Status: steady.";
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
            return "ALERT: Something needs attention.";
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
            return "Look around before you move.";
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
            return "This may not go back in the box.";
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
            return "Time is short.";
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
            return "WARNING: Stores are critical.";
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
            return "DANGER: Health is critical.";
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
            return "WARNING: Morale is critically low.";
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
            return "Standing updated.";
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
            return "Faction standing updated.";
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
            return "The valley shifted.";
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
            return "System updated.";
        }
    }
}
