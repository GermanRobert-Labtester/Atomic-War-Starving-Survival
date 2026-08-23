using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// Catalog of Holdfast-specific NPCs with hostile elements, faction interactions,
    /// and creative writing for the District 8 expansion.
    /// </summary>
    public sealed class HoldfastNpcDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string FactionId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string[] DialogueFragments { get; set; } = Array.Empty<string>();
        public string[] HostileActions { get; set; } = Array.Empty<string>();
        public string[] TrustBuildingRequirements { get; set; } = Array.Empty<string>();
        public float BaseTrust { get; set; } = 0f;
        public bool IsCompanion { get; set; } = false;
        public string[] CompanionFlags { get; set; } = Array.Empty<string>();
        public string[] CompanionRequirements { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Immutable-after-load Holdfast NPC catalog. Mutable during load only.
    /// </summary>
    public sealed class HoldfastNpcCatalog
    {
        private readonly Dictionary<string, HoldfastNpcDefinition> _byId =
            new Dictionary<string, HoldfastNpcDefinition>(StringComparer.Ordinal);
        private readonly List<HoldfastNpcDefinition> _order = new List<HoldfastNpcDefinition>();

        public int Count => _order.Count;
        public bool IsValid => _order.Count > 0;

        public static HoldfastNpcCatalog Empty() => new HoldfastNpcCatalog();

        public void Register(HoldfastNpcDefinition npc)
        {
            if (npc == null || string.IsNullOrEmpty(npc.Id) || _byId.ContainsKey(npc.Id)) return;
            _byId[npc.Id] = npc;
            _order.Add(npc);
        }

        public HoldfastNpcDefinition? GetById(string id)
            => string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var n) ? n : null);

        public bool Contains(string id) => GetById(id) != null;

        public IReadOnlyList<HoldfastNpcDefinition> All()
        {
            var list = new List<HoldfastNpcDefinition>(_order);
            list.Sort((a, b) => string.Compare(a.Id, b.Id, StringComparison.Ordinal));
            return list;
        }
    }

    /// <summary>
    /// Engine-agnostic loader for holdfast_npcs.json with load-time validation.
    /// </summary>
    public static class HoldfastNpcCatalogLoader
    {
        public const string FileName = "holdfast_npcs.json";
        public const int CurrentSchemaVersion = 1;

        public static HoldfastNpcCatalog Load(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            var catalog = new HoldfastNpcCatalog();
            string path = files.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                catalog.Register(CreateDefaultNpcs());
                return catalog;
            }

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                catalog.Register(CreateDefaultNpcs());
                return catalog;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var root = json.Deserialize<HoldfastNpcCatalogRoot>(raw);
                if (root == null || root.npcs == null)
                {
                    catalog.Register(CreateDefaultNpcs());
                    return catalog;
                }

                if (root.schema_version > CurrentSchemaVersion)
                {
                    catalog.Register(CreateDefaultNpcs());
                    return catalog;
                }

                var seen = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < root.npcs.Count; i++)
                {
                    var npc = root.npcs[i];
                    if (npc == null) continue;

                    if (string.IsNullOrEmpty(npc.Id))
                    {
                        continue;
                    }

                    if (!seen.Add(npc.Id))
                    {
                        continue;
                    }

                    catalog.Register(npc);
                }
            }
            catch
            {
                catalog.Register(CreateDefaultNpcs());
            }

            return catalog;
        }

        private static void Register(this HoldfastNpcCatalog catalog, HoldfastNpcDefinition npc)
        {
            if (npc != null && !string.IsNullOrEmpty(npc.Id) && !catalog.Contains(npc.Id))
            {
                catalog._byId[npc.Id] = npc;
                catalog._order.Add(npc);
            }
        }

        private static HoldfastNpcDefinition[] CreateDefaultNpcs()
        {
            return new HoldfastNpcDefinition[]
            {
                new HoldfastNpcDefinition
                {
                    Id = "npc_cael_ormund",
                    DisplayName = "Registrar-General Cael Ormund",
                    FactionId = "faction_the_office",
                    Role = "Registrar",
                    Description = "A civil servant who believes bureaucracy is the only thing that hasn't rotted. His voice is polite, his pencil is precise, and his patience is thinner than the paper he writes on.",
                    DialogueFragments = new string[]
                    {
                        "The discrepancy is noted.",
                        "I am not collecting you. I am scheduling you.",
                        "The Schedule is complete. Yours is not.",
                        "Paperwork is the only thing that keeps the ice from claiming us all.",
                        "You are living in a facility that authenticated for fourteen. The fourteen did not arrive.",
                        "Under Continuity Reconstruction Order 12-C, unallocated occupants constitute a labor reserve.",
                        "I will be back in thirty days to review the roster."
                    },
                    HostileActions = new string[]
                    {
                        "Files you as a labor reserve",
                        "Sends auditors to your bunker",
                        "Confiscates unlisted survivors",
                        "Freezes your Ice Road access",
                        "Issues levy orders with impossible terms",
                        "Marks your bunker for audit",
                        "Demands perfect paperwork"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Complete census forms accurately",
                        "Honor levy orders",
                        "Share survivor occupations",
                        "Provide accurate location data",
                        "File paperwork on time",
                        "Accept the numbered apartment in Block C",
                        "Do not question the Schedule"
                    },
                    BaseTrust = 0.1f,
                    IsCompanion = false
                },
                new HoldfastNpcDefinition
                {
                    Id = "npc_edor_vale",
                    DisplayName = "Clerk Edor Vale",
                    FactionId = "faction_the_office",
                    Role = "Census Clerk",
                    Description = "A junior enumerator who knows his score is too low to matter. He reads the census form twice because he's afraid of making a mistake, and his fear makes him dangerous.",
                    DialogueFragments = new string[]
                    {
                        "Most people want it read again. That's all right.",
                        "There isn't a time limit on understanding it.",
                        "The birth year is written twice. Once correctly.",
                        "I can wait. The ice can't.",
                        "The pencil hovers over the blank spaces like a judge's gavel.",
                        "Your name is in a column you were never meant to see.",
                        "I will file it. I will not sign it."
                    },
                    HostileActions = new string[]
                    {
                        "Waits silently at your hatch",
                        "Files incomplete returns",
                        "Marks your bunker for audit",
                        "Notes your survivors' occupations",
                        "Records your survivors as allocated",
                        "Refuses to acknowledge you in future"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Hear the form read aloud",
                        "Confirm or deny occupations",
                        "Allow him to wait near your hatch",
                        "Show Sela's card if you have it",
                        "Do not joke about the birth year",
                        "Accept his corrections"
                    },
                    BaseTrust = 0.3f,
                    IsCompanion = true,
                    CompanionFlags = new string[] { "companion_edor" },
                    CompanionRequirements = new string[] { "trust_edor_above_zero" }
                },
                new HoldfastNpcDefinition
                {
                    Id = "npc_leva_quist",
                    DisplayName = "Shift Lead Leva Quist",
                    FactionId = "hydro_barons",
                    Role = "Plant Foreman",
                    Description = "A municipal engineer who was already at the plant when the Exchange happened. She calls the allocated 'the indoors' and the unlisted 'the ones who actually do the work.'",
                    DialogueFragments = new string[]
                    {
                        "Motion: that we keep running. Carried.",
                        "The membranes don't care about your feelings.",
                        "I need four people on the outfall by morning or the indoors freeze in their numbers.",
                        "The count is short. The discrepancy is noted.",
                        "The evaporation is confirmed. The plant's failure is inevitable.",
                        "I will not shut the plant to spite Ormund. I have done the 48-hour math.",
                        "The resin drums are a countdown. The plant's failure is inevitable."
                    },
                    HostileActions = new string[]
                    {
                        "Poisons your water supply",
                        "Sabotages your steam connections",
                        "Overcharges for critical repairs",
                        "Refuses to share medical supplies",
                        "Blames you for plant failures",
                        "Evaporates your resin",
                        "Schedules you for outfall shifts"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Deliver brass fittings",
                        "Repair membrane systems",
                        "Share medical supplies",
                        "Work outfall shifts",
                        "Provide iodine crystals",
                        "Accept her minutes as gospel",
                        "Do not question her authority"
                    },
                    BaseTrust = 0.4f,
                    IsCompanion = true,
                    CompanionFlags = new string[] { "companion_leva" },
                    CompanionRequirements = new string[] { "trust_leva_above_zero" }
                },
                new HoldfastNpcDefinition
                {
                    Id = "npc_yara_holm",
                    DisplayName = "Cutter Yara Holm",
                    FactionId = "faction_the_cutters",
                    Role = "Ice Pilot",
                    Description = "A harbor ice-pilot who speaks in distances and warnings. She calls the road 'lit' or 'dark' as if they were moral words, and she will not guide you onto ice she has marked dark.",
                    DialogueFragments = new string[]
                    {
                        "I don't open it for you. I open it.",
                        "If it's dark, you wait.",
                        "If you don't wait, I write the accident in the book and I don't fetch you.",
                        "The ice groans beneath your boots.",
                        "Yara's rule is absolute: dark means do not cross.",
                        "The beacon is a contract.",
                        "Dark ice is a death sentence."
                    },
                    HostileActions = new string[]
                    {
                        "Marks your ice as unsafe",
                        "Sabotages your waystation",
                        "Steals your beacon oil",
                        "Leaves you stranded in a blizzard",
                        "Charges exorbitant passage fees",
                        "Withdraws her light from your section of the road",
                        "Springs traps on the ice"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Relight dark beacons",
                        "Provide lamp oil",
                        "Work ice road maintenance",
                        "Share navigation charts",
                        "Honor Cutter rules",
                        "Do not walk marked-dark ice",
                        "Accept her ledger as law"
                    },
                    BaseTrust = 0.5f,
                    IsCompanion = true,
                    CompanionFlags = new string[] { "companion_yara" },
                    CompanionRequirements = new string[] { "trust_yara_above_zero" }
                },
                new HoldfastNpcDefinition
                {
                    Id = "npc_halden_mire",
                    DisplayName = "Sparks Halden Mire",
                    FactionId = "faction_the_fleet",
                    Role = "Radioman",
                    Description = "A Fleet radioman who has listened for a stand-up order for five years. He speaks in radio procedure and dead air. He wants to be told the wait is over, or to be told it is not, in writing.",
                    DialogueFragments = new string[]
                    {
                        "I can hear you. That is not the same as a stand-up.",
                        "Say again.",
                        "I need a stand-up.",
                        "The Fleet does not forget.",
                        "The tender is still upright. The ice has come up to the Plimsoll mark and stopped.",
                        "Some waits do not end.",
                        "The Fleet's stand-up authentication is the same family as D/9's."
                    },
                    HostileActions = new string[]
                    {
                        "Demands authentication before boarding",
                        "Refuses to share radio frequencies",
                        "Charges for safe passage",
                        "Blames you for Fleet delays",
                        "Takes your survivors as crew",
                        "Denies boarding to unauthenticated parties",
                        "Accelerates Ormund's arrival"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Provide clean water",
                        "Share radio frequencies",
                        "Work on the tender",
                        "Honor Fleet protocols",
                        "Provide engine parts",
                        "Show Sole's paper if owned",
                        "Accept that some waits do not end"
                    },
                    BaseTrust = 0.2f,
                    IsCompanion = true,
                    CompanionFlags = new string[] { "companion_mire" },
                    CompanionRequirements = new string[] { "trust_mire_above_zero" }
                },
                new HoldfastNpcDefinition
                {
                    Id = "npc_ivy_corrigan",
                    DisplayName = "Ice Pilot Ivy Corrigan",
                    FactionId = "faction_the_cutters",
                    Role = "Lamplighter",
                    Description = "A Cutter who won't cross Kilometre 19. She confirms the post exists but won't say why it's significant. Her refusal carries a warning that echoes in the silence.",
                    DialogueFragments = new string[]
                    {
                        "I won't cross it.",
                        "I confirm the post exists.",
                        "I won't say why it's significant.",
                        "The ledger in Sector 4 stops here.",
                        "Yara's rule is absolute.",
                        "The silence is heavier than the ash on your boots."
                    },
                    HostileActions = new string[]
                    {
                        "Refuses to acknowledge you in future",
                        "Marks the ice as unsafe",
                        "Leaves you stranded",
                        "Charges exorbitant passage fees",
                        "Withdraws her light from your section of the road"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Carry oil south",
                        "Do not ask her to come north",
                        "Bring a receipt",
                        "Accept her refusal as law",
                        "Do not question her authority"
                    },
                    BaseTrust = 0.6f,
                    IsCompanion = false
                },
                new HoldfastNpcDefinition
                {
                    Id = "npc_margit_sole",
                    DisplayName = "Margit Sole",
                    FactionId = "faction_the_office",
                    Role = "Archivist",
                    Description = "A Continuity archivist who files but does not sign. She has the Schedule in her drawer and the truth in her eyes. She will not stand down a ship, but she will file your return.",
                    DialogueFragments = new string[]
                    {
                        "She files it. She does not sign it.",
                        "The Order is real. The unlisted are a legal fiction.",
                        "I will not sign it.",
                        "The Schedule is complete. Yours is not.",
                        "Your name is in a column you were never meant to see.",
                        "I will file your return. I will not vouch for it."
                    },
                    HostileActions = new string[]
                    {
                        "Files your return incompletely",
                        "Marks your bunker for audit",
                        "Notes your survivors as allocated",
                        "Refuses to sign anything",
                        "Accelerates Ormund's arrival"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Carry a copy of Ormund's levy north-to-south",
                        "Do not question her filing",
                        "Accept her refusal to sign",
                        "Trust her with your return",
                        "Do not demand signatures"
                    },
                    BaseTrust = 0.7f,
                    IsCompanion = false
                },
                new HoldfastNpcDefinition
                {
                    Id = "npc_colonel_voss",
                    DisplayName = "Colonel Rurik Voss",
                    FactionId = "faction_central_garrison",
                    Role = "Military Commander",
                    Description = "A garrison commander who sees the reconstruction pool as a prize to be claimed. He wants the levy column for his own purposes, and he will intercept it if he can.",
                    DialogueFragments = new string[]
                    {
                        "The reconstruction pool is a prize.",
                        "I want the levy column.",
                        "I will intercept it if I can.",
                        "The Order is a weapon.",
                        "Voss wants the reconstruction pool.",
                        "The Fleet pad still does not authenticate."
                    },
                    HostileActions = new string[]
                    {
                        "Intercepts levy columns",
                        "Conscripts your survivors",
                        "Blames you for plant failures",
                        "Demands tribute",
                        "Accelerates Ormund's arrival",
                        "Intensifies rivalry with Ormund"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Do not let him intercept your levy",
                        "Do not give him your survivors",
                        "Do not blame him for failures",
                        "Do not demand tribute",
                        "Accept his military authority"
                    },
                    BaseTrust = 0.0f,
                    IsCompanion = false
                },
                new HoldfastNpcDefinition
                {
                    Id = "npc_sela_renn",
                    DisplayName = "Sela Renn",
                    FactionId = "faction_unlisted",
                    Role = "Dependent",
                    Description = "A survivor who was part of Allocation 12. She carries her father's improvised kit and knows the truth about the Schedule. She will choose whether to be claimed as an allocated dependent or to remain unlisted.",
                    DialogueFragments = new string[]
                    {
                        "They have a school. They have iodine. They have my father's number in a drawer.",
                        "That isn't the same as having him.",
                        "The Cluster claims me as Halvard's dependent.",
                        "Iodine, school, a number.",
                        "I choose.",
                        "I will hear it. I will choose."
                    },
                    HostileActions = new string[]
                    {
                        "Disappears survivors",
                        "Corrupts your records",
                        "Makes you question reality",
                        "Claims her as allocated dependent",
                        "Takes her away from you"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Let her hear the claim",
                        "Do not force her to choose",
                        "Accept her choice",
                        "Do not question her decision",
                        "Trust her with her future"
                    },
                    BaseTrust = 0.8f,
                    IsCompanion = true,
                    CompanionFlags = new string[] { "companion_sela" },
                    CompanionRequirements = new string[] { "sela_present_in_shelter" }
                },
                new HoldfastNpcDefinition
                {
                    Id = "npc_wren",
                    DisplayName = "Wren",
                    FactionId = "faction_the_office",
                    Role = "Student",
                    Description = "A child born after the Exchange who has never known hunger in the Sector 4 sense. She sits in the back of the Cluster school and does not speak, but she notices everything.",
                    DialogueFragments = new string[]
                    {
                        "The children's sums are neat.",
                        "The ideology is complete.",
                        "Wren's eyes are wide.",
                        "I will record what you tell her.",
                        "The sum is corrected.",
                        "The ideology is complete."
                    },
                    HostileActions = new string[]
                    {
                        "Records what you tell her",
                        "Files reports on your behavior",
                        "Corrupts your records",
                        "Makes you question reality",
                        "Breaks your trust"
                    },
                    TrustBuildingRequirements = new string[]
                    {
                        "Sit the lesson",
                        "Correct a sum or let it stand",
                        "Record what you tell her",
                        "Accept her recording",
                        "Do not question her presence"
                    },
                    BaseTrust = 0.9f,
                    IsCompanion = false
                }
            };
        }
    }

    /// <summary>Schema-envelope root for holdfast_npcs.json.</summary>
    public sealed class HoldfastNpcCatalogRoot
    {
        public int schema_version { get; set; } = 1;
        public List<HoldfastNpcDefinition> npcs { get; set; } = new List<HoldfastNpcDefinition>();
    }
}
