using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion V — Mega-Location: Blacksite Echo (The Listening Post).
    /// Pre-war domestic intelligence hub. Sub-basement of a collapsed government
    /// ministry. The analysts starved to death in the dark, listening to the
    /// world end on battery-powered headsets. NPC_BlackOps deserters remain.
    /// </summary>
    public class Location_BlacksiteEcho
    {
        public const string LocationId = "location_blacksite_echo";
        public const string DisplayName = "Blacksite Echo";
        public const int TravelHours = 12;
        public const int DangerLevel = 9;
        public const float BaseRads = 45f;

        // ── Special loot ──────────────────────────────────────────────
        public const string Loot_SuppressedVz61 = "weapon_suppressed_vz61_skorpion";
        public const string Loot_EncryptedDrive = "item_encrypted_drive";
        public const string Loot_SchematicAmmoReloader = "sch_ammo_reloader";
        public const int Ammo_SuppressedVz61 = 40;

        // ── Outpost ───────────────────────────────────────────────────
        public const string OutpostModuleId = "radio_array_outpost";
        public const float OutpostIntelReliabilityBonus = 0.20f;

        // ── Props ─────────────────────────────────────────────────────
        public const string Prop_ServerRack = "server_rack";
        public const string Prop_SoundproofingPanel = "soundproofing_panel";
        public const string Prop_FilingCabinet = "filing_cabinet";

        public event Action<string> OnBlackOpsEncounter;
        public event Action<string> OnSpecialLootFound;
        public event Action<string> OnOutpostEstablished;

        private readonly System.Random _rng;
        private readonly HashSet<string> _searchedProps = new HashSet<string>();
        private bool _suppressedWeaponFound;
        private bool _encryptedDriveFound;
        private bool _schematicFound;
        private bool _outpostEstablished;

        public bool IsSuppressedWeaponFound => _suppressedWeaponFound;
        public bool IsEncryptedDriveFound => _encryptedDriveFound;
        public bool IsOutpostEstablished => _outpostEstablished;

        public Location_BlacksiteEcho(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(7000);
        }

        public List<string> SearchProp(string propId, string survivorId, bool hasLockpick)
        {
            if (_searchedProps.Contains(propId)) return null;
            _searchedProps.Add(propId);

            var loot = new List<string>();

            switch (propId)
            {
                case Prop_ServerRack:
                    loot.Add("electronic_scrap");
                    loot.Add("copper_wire_10m_of_10m");
                    loot.Add("vacuum_tube");
                    if (!_schematicFound && _rng.NextDouble() < 0.30f)
                    {
                        _schematicFound = true;
                        loot.Add(Loot_SchematicAmmoReloader);
                        OnSpecialLootFound?.Invoke(Loot_SchematicAmmoReloader);
                    }
                    break;

                case Prop_SoundproofingPanel:
                    loot.Add("insulation_wrap");
                    break;

                case Prop_FilingCabinet:
                    if (hasLockpick)
                    {
                        loot.Add("sealed_government_document");
                        if (!_encryptedDriveFound && _rng.NextDouble() < 0.40f)
                        {
                            _encryptedDriveFound = true;
                            loot.Add(Loot_EncryptedDrive);
                            OnSpecialLootFound?.Invoke(Loot_EncryptedDrive);
                        }
                    }
                    break;
            }

            // BlackOps encounter chance
            if (_rng.NextDouble() < 0.40f)
                OnBlackOpsEncounter?.Invoke(survivorId);

            return loot;
        }

        public bool FindSuppressedWeapon(string survivorId)
        {
            if (_suppressedWeaponFound) return false;
            if (_rng.NextDouble() < 0.25f)
            {
                _suppressedWeaponFound = true;
                OnSpecialLootFound?.Invoke(Loot_SuppressedVz61);
                return true;
            }
            return false;
        }

        public bool EstablishOutpost(string survivorId)
        {
            if (_outpostEstablished) return false;
            _outpostEstablished = true;
            OnOutpostEstablished?.Invoke(survivorId);
            return true;
        }

        public BlacksiteEchoSave CaptureState()
        {
            var props = new string[_searchedProps.Count];
            _searchedProps.CopyTo(props);
            return new BlacksiteEchoSave
            {
                SearchedProps = props,
                SuppressedWeaponFound = _suppressedWeaponFound,
                EncryptedDriveFound = _encryptedDriveFound,
                SchematicFound = _schematicFound,
                OutpostEstablished = _outpostEstablished
            };
        }

        public void RestoreState(BlacksiteEchoSave save)
        {
            _searchedProps.Clear();
            _suppressedWeaponFound = false;
            _encryptedDriveFound = false;
            _schematicFound = false;
            _outpostEstablished = false;
            if (save == null) return;
            if (save.SearchedProps != null)
                for (int i = 0; i < save.SearchedProps.Length; i++)
                    if (!string.IsNullOrEmpty(save.SearchedProps[i]))
                        _searchedProps.Add(save.SearchedProps[i]);
            _suppressedWeaponFound = save.SuppressedWeaponFound;
            _encryptedDriveFound = save.EncryptedDriveFound;
            _schematicFound = save.SchematicFound;
            _outpostEstablished = save.OutpostEstablished;
        }
    }

    [Serializable]
    public class BlacksiteEchoSave
    {
        public string[] SearchedProps;
        public bool SuppressedWeaponFound;
        public bool EncryptedDriveFound;
        public bool SchematicFound;
        public bool OutpostEstablished;
    }
}
