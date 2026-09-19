// SPDX-License-Identifier: MIT
// Plan 167 + Plan 219: bounded production seams over canonical owners.
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Culture;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Ashfall.Core.Underground;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // -----------------------------------------------------------------
        // Plan 167 — Underground Tunnel Network
        // -----------------------------------------------------------------

        /// <summary>
        /// Read-only projection of discovered subterranean passages from the
        /// authoritative WastelandMapSystem. No separate map graph is kept here.
        /// </summary>
        public IReadOnlyList<TunnelSegment> GetDiscoveredTunnelSegments()
        {
            SetupWorld();
            return _world?.WastelandMap?.GetDiscoveredTunnels() ?? Array.Empty<TunnelSegment>();
        }

        /// <summary>
        /// Evaluates whether an underground transit route is clear between two nodes.
        /// </summary>
        public (bool CanTraverse, float TravelTimeHours, string Reason) CanTraverseTunnel(string fromNodeId, string toNodeId)
        {
            SetupWorld();
            return _world?.WastelandMap == null
                ? (false, 0f, "World map unavailable.")
                : _world.WastelandMap.CanTraverseTunnel(fromNodeId, toNodeId);
        }

        /// <summary>
        /// Discovers a tunnel segment and updates the map presentation.
        /// </summary>
        public bool DiscoverTunnelSegment(string segmentId)
        {
            if (string.IsNullOrWhiteSpace(segmentId)) return false;
            SetupWorld();
            if (_world?.WastelandMap == null) return false;

            bool discovered = _world.WastelandMap.DiscoverTunnel(segmentId.Trim());
            if (discovered)
            {
                _worldDirty = true;
                _mapPanel?.RefreshView();
            }
            return discovered;
        }

        /// <summary>
        /// Repairs a damaged or collapsed tunnel segment using shelter scrap.
        /// </summary>
        public bool RepairTunnelSegment(string segmentId, float repairAmount = 50f)
        {
            if (string.IsNullOrWhiteSpace(segmentId)) return false;
            SetupWorld();
            SetupInventory();
            if (_world?.WastelandMap == null) return false;

            // Spend 1 scrap if available; repair proceeds even if improvised
            if (_inventory?.Inventory != null && _inventory.Inventory.CountById("scrap") > 0)
            {
                var bill = new InventoryBill();
                bill.AddCost("scrap", 1);
                _inventory.Inventory.TryExecuteTransaction(bill);
            }

            bool repaired = _world.WastelandMap.RepairTunnel(segmentId.Trim(), repairAmount);
            if (repaired)
            {
                _worldDirty = true;
                _mapPanel?.RefreshView();
            }
            return repaired;
        }

        // -----------------------------------------------------------------
        // Plan 219 — Survivor Photography & Documentation System
        // -----------------------------------------------------------------

        /// <summary>
        /// Returns all authored documentation items created by a specific survivor.
        /// </summary>
        public IReadOnlyList<DocumentationItem> GetSurvivorDocumentation(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return Array.Empty<DocumentationItem>();
            SetupCulturalArchive();
            var archive = EnsureCulturalArchive();
            return archive.Documentation.GetDocumentationByAuthor(survivorId.Trim());
        }

        /// <summary>
        /// Returns all public documentation items in the shelter archive.
        /// </summary>
        public IReadOnlyList<DocumentationItem> GetPublicDocumentation()
        {
            SetupCulturalArchive();
            var archive = EnsureCulturalArchive();
            return archive.Documentation.GetPublicDocumentation();
        }

        /// <summary>
        /// Returns all photo albums belonging to a specific survivor.
        /// </summary>
        public IReadOnlyList<PhotoAlbum> GetSurvivorPhotoAlbums(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return Array.Empty<PhotoAlbum>();
            SetupCulturalArchive();
            var archive = EnsureCulturalArchive();
            return archive.Documentation.GetAlbumsForOwner(survivorId.Trim());
        }

        /// <summary>
        /// Creates an authored photograph, consumes photographic film if required,
        /// registers a milestone in the cultural chronicle, and emits an attributed morale delta.
        /// </summary>
        public bool CreateSurvivorPhotograph(
            string authorId,
            string title,
            string cameraUsed = "camera",
            IEnumerable<string>? subjects = null,
            string locationId = "loc_holdfast",
            float composition = 70f,
            float lighting = 70f,
            string description = "",
            bool isPublic = true,
            bool requireFilm = false)
        {
            if (string.IsNullOrWhiteSpace(authorId)) return false;
            SetupSurvivors();
            SetupCulturalArchive();
            var survivor = _survivors?.Needs.Get(authorId.Trim());
            if (_survivors == null || survivor == null || !survivor.IsAliveState) return false;

            var archive = EnsureCulturalArchive();
            var result = archive.TryCreatePhotograph(
                authorId.Trim(),
                title,
                cameraUsed,
                subjects,
                locationId,
                composition,
                lighting,
                _simDay,
                description,
                isPublic,
                requireFilm,
                out var photo);

            if (result.Status != ActionResult.StatusKind.Success || photo == null)
                return false;

            _survivors.Needs.ApplyAttributedDelta(
                authorId.Trim(),
                NeedKind.Morale,
                photo.Quality * 0.04f,
                "culture.photography");

            MarkCulturalArchiveDirty();
            return true;
        }

        /// <summary>
        /// Creates an authored sketch and applies an attributed morale delta to the artist.
        /// </summary>
        public bool CreateSurvivorSketch(
            string authorId,
            string title,
            string subject = "shelter_life",
            string medium = "charcoal",
            float artisticQuality = 70f,
            float accuracy = 60f,
            float hoursSpent = 2f,
            string description = "",
            bool isPublic = true)
        {
            if (string.IsNullOrWhiteSpace(authorId)) return false;
            SetupSurvivors();
            SetupCulturalArchive();
            var survivor = _survivors?.Needs.Get(authorId.Trim());
            if (_survivors == null || survivor == null || !survivor.IsAliveState) return false;

            var archive = EnsureCulturalArchive();
            var result = archive.TryCreateSketch(
                authorId.Trim(),
                title,
                subject,
                medium,
                artisticQuality,
                accuracy,
                hoursSpent,
                _simDay,
                description,
                isPublic,
                out var sketch);

            if (result.Status != ActionResult.StatusKind.Success || sketch == null)
                return false;

            _survivors.Needs.ApplyAttributedDelta(
                authorId.Trim(),
                NeedKind.Morale,
                sketch.Quality * 0.03f,
                "culture.sketching");

            MarkCulturalArchiveDirty();
            return true;
        }

        /// <summary>
        /// Creates an authored written record (chronicle, journal entry, essay) and applies attributed morale.
        /// </summary>
        public bool CreateSurvivorWrittenRecord(
            string authorId,
            string title,
            string recordType = "chronicle",
            string content = "",
            float writingQuality = 75f,
            bool isPublic = true)
        {
            if (string.IsNullOrWhiteSpace(authorId)) return false;
            SetupSurvivors();
            SetupCulturalArchive();
            var survivor = _survivors?.Needs.Get(authorId.Trim());
            if (_survivors == null || survivor == null || !survivor.IsAliveState) return false;

            var archive = EnsureCulturalArchive();
            var result = archive.TryCreateWrittenRecord(
                authorId.Trim(),
                title,
                recordType,
                content,
                writingQuality,
                _simDay,
                isPublic,
                out var record);

            if (result.Status != ActionResult.StatusKind.Success || record == null)
                return false;

            _survivors.Needs.ApplyAttributedDelta(
                authorId.Trim(),
                NeedKind.Morale,
                record.Quality * 0.035f,
                "culture.chronicle");

            MarkCulturalArchiveDirty();
            return true;
        }

        /// <summary>
        /// Creates a photo album for a survivor to organize photographs.
        /// </summary>
        public bool CreateSurvivorPhotoAlbum(
            string ownerId,
            string albumName,
            string theme = "shelter",
            bool isShared = true)
        {
            if (string.IsNullOrWhiteSpace(ownerId)) return false;
            SetupSurvivors();
            SetupCulturalArchive();
            var survivor = _survivors?.Needs.Get(ownerId.Trim());
            if (_survivors == null || survivor == null || !survivor.IsAliveState) return false;

            var archive = EnsureCulturalArchive();
            var album = archive.Documentation.CreateAlbum(ownerId.Trim(), albumName, theme, _simDay, isShared);
            if (album != null)
            {
                MarkCulturalArchiveDirty();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a photograph to an existing photo album.
        /// </summary>
        public bool AddPhotoToSurvivorAlbum(string albumId, string photoDocumentationId)
        {
            if (string.IsNullOrWhiteSpace(albumId) || string.IsNullOrWhiteSpace(photoDocumentationId))
                return false;

            SetupCulturalArchive();
            var archive = EnsureCulturalArchive();
            bool added = archive.Documentation.AddPhotoToAlbum(albumId.Trim(), photoDocumentationId.Trim(), _simDay);
            if (added)
            {
                MarkCulturalArchiveDirty();
            }
            return added;
        }

        /// <summary>
        /// Shares a document with the shelter, boosting community-wide morale.
        /// </summary>
        public float ShareSurvivorDocumentation(string documentationId)
        {
            if (string.IsNullOrWhiteSpace(documentationId)) return 0f;
            SetupCulturalArchive();
            SetupSurvivors();
            var archive = EnsureCulturalArchive();
            var result = archive.TryShareDocumentation(documentationId.Trim(), _simDay, out float boost);
            if (result.Status != ActionResult.StatusKind.Success || boost <= 0f)
                return 0f;

            var roster = _survivors?.RosterState;
            if (roster != null)
            {
                for (int i = 0; i < roster.Count; i++)
                {
                    var s = roster[i];
                    if (s != null && s.IsAliveState && _survivors != null)
                    {
                        _survivors.Needs.ApplyAttributedDelta(
                            s.Id,
                            NeedKind.Morale,
                            boost,
                            "culture.shared_documentation");
                    }
                }
            }

            MarkCulturalArchiveDirty();
            return boost;
        }
    }
}
