using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>One selectable branch of a Muster questline.</summary>
    public class ApproachOption
    {
        public QuestApproach approach;
        public string label = string.Empty;
        public string description = string.Empty;
        public string endingKey = string.Empty;
    }

    /// <summary>Data-defined questline entry for the Muster escalation layer.</summary>
    public class MusterQuestlineDefinition
    {
        public string questlineId = string.Empty;
        public string factionId = string.Empty;
        public int windowStartDay;
        public List<ApproachOption> approaches = new List<ApproachOption>();
    }

    /// <summary>Serialized state of the Muster system (save/load safe).</summary>
    public class MusterState
    {
        public string systemId = MusterSystem.SystemId;
        public int escalationDay = -1;
        public bool musterTriggered;
        public List<MusterRecord> records = new List<MusterRecord>();
    }

    /// <summary>One questline's selection record.</summary>
    public class MusterRecord
    {
        public string questlineId = string.Empty;
        public string factionId = string.Empty;
        public bool resolved;
        public string selectedApproach = string.Empty;
        public string endingKey = string.Empty;
        public int resolvedDay;
    }

    /// <summary>
    /// Day-180+ escalation orchestrator for Expansion 06. Owns the questline
    /// catalog, approach selection (validated against the questline's offered
    /// branches), the Muster trigger flag, and ending-key resolution for the
    /// Section XII epilogue matrix. Engine-agnostic; hosts only present it.
    /// </summary>
    public class MusterSystem : IApproachQuestline
    {
        public const string SystemId = "muster_system";

        private readonly MusterState _state;
        private readonly List<MusterQuestlineDefinition> _catalog = new List<MusterQuestlineDefinition>();

        public event Action<MusterRecord> OnQuestlineResolved;
        public event Action<MusterState> OnStateChanged;

        public MusterSystem(MusterState state = null!)
        {
            _state = state ?? new MusterState();
            if (_state.systemId != SystemId) _state.systemId = SystemId;
            if (_state.records == null) _state.records = new List<MusterRecord>();
            PopulateFoundingCatalog();
        }

        public MusterState State => _state;
        public IReadOnlyList<MusterQuestlineDefinition> Catalog => _catalog;
        public bool MusterTriggered => _state.musterTriggered;
        public int EscalationDay => _state.escalationDay;
        public bool IsResolved => _state.records.Exists(r => r.questlineId == QuestlineId && r.resolved);
        public QuestApproach? SelectedApproach
        {
            get
            {
                var r = FindRecord(QuestlineId);
                if (r == null || string.IsNullOrEmpty(r.selectedApproach)) return null;
                return Enum.TryParse(r.selectedApproach, out QuestApproach parsed)
                    ? (QuestApproach?)parsed
                    : null;
            }
        }
        public string QuestlineId => "quest_the_muster_uprising";

        // ── Catalog ────────────────────────────────────────────────────

        public void RegisterQuestline(MusterQuestlineDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.questlineId)) return;
            if (_catalog.Exists(q => q.questlineId == def.questlineId)) return;
            _catalog.Add(def);
        }

        public MusterQuestlineDefinition? FindDefinition(string questlineId)
        {
            foreach (var q in _catalog)
                if (q.questlineId == questlineId) return q;
            return null;
        }

        /// <summary>True once the sector clock passes the Muster's opening day.</summary>
        public void SetEscalationDay(int day)
        {
            if (day < 0 || day == _state.escalationDay) return;
            _state.escalationDay = day;
            _state.musterTriggered = day >= MusterOpeningDay;
            RaiseChanged();
        }

        // ── Approach selection (IApproachQuestline) ────────────────────

        public bool SelectApproach(QuestApproach approach) =>
            SelectApproachFor(QuestlineId, approach);

        /// <summary>Select an approach on any registered questline. Rejects
        /// unregistered questlines, approaches the questline does not offer,
        /// and already-resolved questlines.</summary>
        public bool SelectApproachFor(string questlineId, QuestApproach approach)
        {
            var def = FindDefinition(questlineId);
            if (def == null) return false;
            ApproachOption? option = null;
            for (int i = 0; i < def.approaches.Count; i++)
                if (def.approaches[i].approach == approach) { option = def.approaches[i]; break; }
            if (option == null) return false;

            var record = GetOrCreateRecord(def);
            if (record.resolved) return false;

            record.selectedApproach = approach.ToString();
            record.resolved = true;
            record.endingKey = option.endingKey;
            record.resolvedDay = _state.escalationDay >= 0 ? _state.escalationDay : 0;
            OnQuestlineResolved?.Invoke(record);
            RaiseChanged();
            return true;
        }

        public string ResolveEndingKey()
        {
            var r = FindRecord(QuestlineId);
            return r != null && r.resolved ? r.endingKey : string.Empty;
        }

        /// <summary>Ending key for any registered questline (Section XII matrix).</summary>
        public string EndingKeyFor(string questlineId)
        {
            var r = FindRecord(questlineId);
            return r != null && r.resolved ? r.endingKey : string.Empty;
        }

        /// <summary>True when any questline resolved to this matrix key.</summary>
        public bool EndingKeyForAny(string endingKey)
        {
            for (int i = 0; i < _state.records.Count; i++)
            {
                var r = _state.records[i];
                if (r.resolved && r.endingKey == endingKey) return true;
            }
            return false;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public MusterState CaptureState()
        {
            var copy = new MusterState
            {
                systemId = _state.systemId,
                escalationDay = _state.escalationDay,
                musterTriggered = _state.musterTriggered
            };
            var ordered = new List<MusterRecord>(_state.records);
            ordered.Sort((a, b) => string.CompareOrdinal(a.questlineId, b.questlineId));
            for (int i = 0; i < ordered.Count; i++)
            {
                var r = ordered[i];
                copy.records.Add(new MusterRecord
                {
                    questlineId = r.questlineId,
                    factionId = r.factionId,
                    resolved = r.resolved,
                    selectedApproach = r.selectedApproach,
                    endingKey = r.endingKey,
                    resolvedDay = r.resolvedDay
                });
            }
            return copy;
        }

        public void RestoreState(MusterState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.escalationDay = saved.escalationDay;
            _state.musterTriggered = saved.musterTriggered;
            _state.records.Clear();
            if (saved.records != null)
            {
                for (int i = 0; i < saved.records.Count; i++)
                {
                    var r = saved.records[i];
                    if (r == null || string.IsNullOrEmpty(r.questlineId)) continue;
                    _state.records.Add(new MusterRecord
                    {
                        questlineId = r.questlineId,
                        factionId = r.factionId,
                        resolved = r.resolved,
                        selectedApproach = r.selectedApproach,
                        endingKey = r.endingKey,
                        resolvedDay = r.resolvedDay
                    });
                }
            }
            RaiseChanged();
        }

        // ── Internals ──────────────────────────────────────────────────

        public const int MusterOpeningDay = 260;

        private void PopulateFoundingCatalog()
        {
            var muster = new MusterQuestlineDefinition
            {
                questlineId = QuestlineId,
                factionId = "faction_deserter_coalition",
                windowStartDay = MusterOpeningDay
            };
            muster.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.A,
                label = "The Amnesty Campaign",
                description = "Build the case. Testimony, provenance, a petition Harven can sign without losing the war.",
                endingKey = "the_amnesty"
            });
            muster.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.B,
                label = "The Standing Ground",
                description = "Arm the Coalition and hold. Counter-raids escalate; survival to Day 320 is a real fight.",
                endingKey = "the_open_muster"
            });
            muster.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.C,
                label = "Nobody Stays",
                description = "Empty the ground. Route rallied members out of Sector 4 in small groups; become a corridor, not a camp.",
                endingKey = "the_corridor"
            });
            muster.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.D,
                label = "The Blood Price",
                description = "Report the Coalition's location to Harven for restored standing and a resupply. Members rallied are lost.",
                endingKey = "the_blood_price"
            });
            RegisterQuestline(muster);

            var hydro = new MusterQuestlineDefinition
            {
                questlineId = "quest_the_rate_card_war",
                factionId = "faction_hydro_barons",
                windowStartDay = 180
            };
            hydro.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.A,
                label = "Undercut",
                description = "Ally with the Grain Exchange to flood the barter market with brine-salt substitute until the rate card is renegotiated downward.",
                endingKey = "the_rate_card_revised"
            });
            hydro.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.B,
                label = "Audit",
                description = "Bring the Cold Count's instruments to Unit 4. The water tests clean; the filters' safety margin does not.",
                endingKey = "the_rate_card_revised"
            });
            hydro.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.C,
                label = "Seize",
                description = "Take Desalination Unit 4 by force before the patrols do. The queue ends; the thirsty season begins.",
                endingKey = "the_administrator"
            });
            hydro.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.D,
                label = "Broker",
                description = "Bring the Tally in to formalize a three-way rotation backed by an enforceable contract.",
                endingKey = "the_rate_card_revised"
            });
            RegisterQuestline(hydro);

            RegisterQuestline(new MusterQuestlineDefinition
            {
                questlineId = "quest_the_unsigned_order",
                factionId = "faction_the_tally",
                windowStartDay = 185
            });

            // Section V — the six silent currents, wired with their questlines
            // and approach forks (or deliberate forklessness).
            var coldCount = new MusterQuestlineDefinition
            {
                questlineId = "quest_four_names_on_the_roster",
                factionId = "faction_cold_count",
                windowStartDay = 180
            };
            coldCount.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.A,
                label = "Sustain Them",
                description = "Supply power and shielding on a regular schedule; the only path that lets the provenance run complete before Day 300.",
                endingKey = "the_measured_truth"
            });
            coldCount.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.B,
                label = "Extract and Leave",
                description = "Trade up front for a partial reading, then stop supplying. The broadcast fires caveated and Garrison hears it as far less credible.",
                endingKey = "the_measured_truth_contested"
            });
            RegisterQuestline(coldCount);

            RegisterQuestline(new MusterQuestlineDefinition
            {
                questlineId = "quest_the_second_winter",
                factionId = "faction_the_provisioned",
                windowStartDay = 190
            });

            var longWalk = new MusterQuestlineDefinition
            {
                questlineId = "quest_the_eleven_month_circuit",
                factionId = "faction_long_walk",
                windowStartDay = 185
            };
            longWalk.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.A,
                label = "Escort",
                description = "Guard a leg of the circuit through the dangerous stretch. No payment; fresher intelligence on the next pass.",
                endingKey = string.Empty
            });
            longWalk.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.B,
                label = "Resupply Only",
                description = "Trade water and footwear for goods and a report. Sustainable, permanently at arm's length.",
                endingKey = string.Empty
            });
            RegisterQuestline(longWalk);

            var scavengerGuild = new MusterQuestlineDefinition
            {
                questlineId = "quest_the_second_color_ledger",
                factionId = "faction_scavenger_guild",
                windowStartDay = 190
            };
            scavengerGuild.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.A,
                label = "Apprentice",
                description = "Take the Guild's training for claimed-site routing. Richest salvage first, in exchange for a hard yield cap.",
                endingKey = string.Empty
            });
            scavengerGuild.approaches.Add(new ApproachOption
            {
                approach = QuestApproach.B,
                label = "Freelance",
                description = "Salvage wherever you want. One over-stripped claimed site blacklists the shelter permanently.",
                endingKey = string.Empty
            });
            RegisterQuestline(scavengerGuild);

            RegisterQuestline(new MusterQuestlineDefinition
            {
                questlineId = "quest_nothing_to_offer",
                factionId = "faction_iron_raiders",
                windowStartDay = 200
            });
        }

        /// <summary>True when a current's Section V questline is wired in the
        /// catalog (the data-side is_active flip may then stand).</summary>
        public bool IsCurrentWired(string factionId)
        {
            for (int i = 0; i < _catalog.Count; i++)
                if (_catalog[i].factionId == factionId) return true;
            return false;
        }

        private MusterRecord GetOrCreateRecord(MusterQuestlineDefinition def)
        {
            var r = FindRecord(def.questlineId);
            if (r != null) return r;
            r = new MusterRecord { questlineId = def.questlineId, factionId = def.factionId };
            _state.records.Add(r);
            return r;
        }

        public MusterRecord? FindRecord(string questlineId)
        {
            for (int i = 0; i < _state.records.Count; i++)
                if (_state.records[i].questlineId == questlineId) return _state.records[i];
            return null;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
