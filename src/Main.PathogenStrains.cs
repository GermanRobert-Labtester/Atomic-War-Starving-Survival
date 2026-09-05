using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Flagship XI — Plan 155 host wiring: attaches the fictional strain layer to
    /// the canonical disease engine, wires the read-only radiation dose query,
    /// persists cure projects as the "pathogen_strains" envelope section, and
    /// ticks mutations + cure research from the medical_disease day owner.
    /// </summary>
    public partial class Main : Control
    {
        private PathogenStrainSystem? _pathogenStrains;

        private void SetupPathogenStrains()
        {
            if (_pathogenStrains != null) return;

            SetupDisease();
            SetupSurvivors();
            if (_disease == null)
            {
                GD.Print("[Ashfall Godot] Pathogen strains: disease engine offline, strains idle.");
                return;
            }

            var catalog = PathogenStrainCatalogLoader.Load(_dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            _pathogenStrains = new PathogenStrainSystem(catalog, _disease.Engine)
            {
                // Read-only coupling: the radiation authority owns dose; strains
                // only read it for abstract severity/mutation pressure.
                RadiationDoseQuery = id => _survivors?.RadStateFor(id)?.RadiationDose ?? 0f
            };
            _pathogenStrains.AttachStrains();
            _pathogenStrains.BindEngineHooks();

            var save = PathogenStrainSaveStore.TryLoad();
            if (save != null)
            {
                _pathogenStrains.RestoreState(PathogenStrainSaveCodec.FromSaveState(save));
                // Re-attach after restore: cured strains are already registered,
                // but a pre-strain save loaded fresh needs the definitions back.
                GD.Print("[Ashfall Godot] Pathogen strain state restored.");
            }

            _pathogenStrains.OnStrainMutation += (survivorId, fromId, toId) =>
            {
                GD.Print($"[Ashfall Godot] {survivorId}: {fromId} has shifted into {toId}.");
            };
        }

        /// <summary>
        /// Player-facing cure project start. The research is fictional and
        /// abstract (reagents + time + labor); no procedures are modeled.
        /// </summary>
        public string StartPathogenCureProject(string strainId, int day)
        {
            SetupPathogenStrains();
            if (_pathogenStrains == null) return "Research bench is offline.";
            if (_pathogenStrains.IsCureUnlocked(strainId)) return "That course of treatment is already worked out.";

            var bill = new InventoryBill();
            bill.AddCost("antibiotics", 2);
            bill.AddCost("medical_kit", 1);
            bill.AddCost("chemicals", 3);

            using var tx = _inventory.Inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid)
            {
                tx.Cancel();
                return "Not enough materials: 2 antibiotics, 1 medical kit, 3 chemicals.";
            }
            if (!_pathogenStrains.StartCureProject(strainId, day))
            {
                tx.Cancel();
                return "That strain is already under study, or unknown to the bench.";
            }
            tx.TryCommit();
            return $"Research underway on {strainId}. It wants steady work and time.";
        }

        private void SavePathogenStrains()
        {
            if (_pathogenStrains == null) return;
            CaptureSection("pathogen_strains",
                PathogenStrainSaveStore.TryCapturePersisted(
                    PathogenStrainSaveCodec.ToSaveState(_pathogenStrains.CaptureState())));
        }
    }
}
