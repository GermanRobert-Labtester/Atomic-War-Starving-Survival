// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Culture
{
    public enum DocumentationType
    {
        Photograph = 0,
        Sketch = 1,
        WrittenRecord = 2,
        AudioRecording = 3,
        VideoRecording = 4
    }

    public enum DocumentationSubjectType
    {
        Person = 0,
        Place = 1,
        Event = 2,
        Object = 3,
        DailyLife = 4,
        SignificantMoment = 5
    }

    [Serializable]
    public sealed class PhotographDetails
    {
        public string CameraUsed { get; set; } = string.Empty;
        public List<string> Subjects { get; set; } = new List<string>();
        public string LocationId { get; set; } = string.Empty;
        public float Composition { get; set; } = 50f;
        public float Lighting { get; set; } = 50f;
        public string Moment { get; set; } = "candid";
    }

    [Serializable]
    public sealed class SketchDetails
    {
        public string Medium { get; set; } = "pencil";
        public float ArtisticQuality { get; set; } = 50f;
        public float Accuracy { get; set; } = 50f;
        public float TimeSpentHours { get; set; } = 1f;
    }

    [Serializable]
    public sealed class WrittenRecordDetails
    {
        public string RecordType { get; set; } = "journal_entry";
        public string Content { get; set; } = string.Empty;
        public int WordCount { get; set; } = 0;
        public float WritingQuality { get; set; } = 50f;
        public string IntendedAudience { get; set; } = "shelter";
    }

    [Serializable]
    public sealed class DocumentationItem
    {
        public string DocumentationId { get; set; } = string.Empty;
        public DocumentationType Type { get; set; } = DocumentationType.Photograph;
        public string AuthorId { get; set; } = string.Empty;
        public DocumentationSubjectType SubjectType { get; set; } = DocumentationSubjectType.DailyLife;
        public string SubjectId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CreatedDay { get; set; } = 1;
        public float Quality { get; set; } = 50f;
        public float SentimentalValue { get; set; } = 50f;
        public bool IsPublic { get; set; } = true;
        public List<string> Tags { get; set; } = new List<string>();

        // Specialized type-specific details
        public PhotographDetails? Photo { get; set; }
        public SketchDetails? Sketch { get; set; }
        public WrittenRecordDetails? Record { get; set; }
    }

    [Serializable]
    public sealed class PhotoAlbum
    {
        public string AlbumId { get; set; } = string.Empty;
        public string AlbumName { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public List<string> PhotoIds { get; set; } = new List<string>();
        public string Theme { get; set; } = "shelter";
        public int CreatedDay { get; set; } = 1;
        public int LastUpdatedDay { get; set; } = 1;
        public bool IsShared { get; set; } = true;
    }

    [Serializable]
    public sealed class DocumentationEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public string AuthorId { get; set; } = string.Empty;
        public string DocumentationId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Significance { get; set; } = "minor";
        public float MoraleEffect { get; set; } = 0f;
    }

    [Serializable]
    public sealed class DocumentationState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<DocumentationItem> Items { get; set; } = new List<DocumentationItem>();
        public List<PhotoAlbum> Albums { get; set; } = new List<PhotoAlbum>();
        public List<DocumentationEvent> Events { get; set; } = new List<DocumentationEvent>();
    }

    /// <summary>
    /// Plan 219 — Survivor Photography & Documentation System.
    /// Manages visual and textual records created by survivors (photos, sketches, chronicles),
    /// photo albums, documentary sharing, quality scoring, and community morale impact.
    /// </summary>
    public sealed class DocumentationSystem
    {
        private readonly DocumentationState _state;

        public event Action<DocumentationItem>? OnDocumentationCreated;
        public event Action<PhotoAlbum>? OnPhotoAlbumCreated;
        public event Action<string, string>? OnPhotoAddedToAlbum;
        public event Action<DocumentationItem, float>? OnDocumentationShared;

        public int TotalItemsCount => _state.Items.Count;
        public int TotalAlbumsCount => _state.Albums.Count;

        public DocumentationSystem(DocumentationState? state = null)
        {
            _state = state ?? new DocumentationState();
        }

        public DocumentationItem CreatePhotograph(
            string authorId,
            string title,
            string cameraUsed,
            IEnumerable<string>? subjects,
            string locationId,
            float composition,
            float lighting,
            int currentDay,
            string description = "",
            bool isPublic = true)
        {
            if (string.IsNullOrWhiteSpace(authorId)) throw new ArgumentNullException(nameof(authorId));

            float quality = Math.Clamp((composition + lighting) * 0.5f, 0f, 100f);
            float sentimental = Math.Clamp(quality * 0.8f + 10f, 0f, 100f);

            var photo = new DocumentationItem
            {
                DocumentationId = $"doc_p_{_state.NextSequence++}",
                Type = DocumentationType.Photograph,
                AuthorId = authorId.Trim(),
                SubjectType = DocumentationSubjectType.SignificantMoment,
                SubjectId = locationId ?? string.Empty,
                Title = string.IsNullOrWhiteSpace(title) ? "Photograph" : title.Trim(),
                Description = description ?? string.Empty,
                CreatedDay = Math.Max(1, currentDay),
                Quality = quality,
                SentimentalValue = sentimental,
                IsPublic = isPublic,
                Tags = new List<string> { "photo", cameraUsed ?? "camera" },
                Photo = new PhotographDetails
                {
                    CameraUsed = cameraUsed ?? "camera",
                    Subjects = subjects != null ? new List<string>(subjects) : new List<string>(),
                    LocationId = locationId ?? string.Empty,
                    Composition = composition,
                    Lighting = lighting,
                    Moment = "candid"
                }
            };

            _state.Items.Add(photo);

            var ev = new DocumentationEvent
            {
                EventId = $"dev_{_state.NextSequence++}",
                EventType = "photo_taken",
                Day = currentDay,
                AuthorId = authorId,
                DocumentationId = photo.DocumentationId,
                Description = $"{authorId} photographed '{photo.Title}'.",
                Significance = quality >= 80f ? "major" : "moderate",
                MoraleEffect = quality * 0.04f
            };
            _state.Events.Add(ev);

            OnDocumentationCreated?.Invoke(photo);
            return photo;
        }

        public DocumentationItem CreateSketch(
            string authorId,
            string title,
            string subject,
            string medium,
            float artisticQuality,
            float accuracy,
            float hoursSpent,
            int currentDay,
            string description = "",
            bool isPublic = true)
        {
            if (string.IsNullOrWhiteSpace(authorId)) throw new ArgumentNullException(nameof(authorId));

            float quality = Math.Clamp((artisticQuality * 0.6f) + (accuracy * 0.4f), 0f, 100f);
            float sentimental = Math.Clamp(quality * 0.7f + 15f, 0f, 100f);

            var sketch = new DocumentationItem
            {
                DocumentationId = $"doc_s_{_state.NextSequence++}",
                Type = DocumentationType.Sketch,
                AuthorId = authorId.Trim(),
                SubjectType = DocumentationSubjectType.Object,
                SubjectId = subject ?? string.Empty,
                Title = string.IsNullOrWhiteSpace(title) ? "Sketch" : title.Trim(),
                Description = description ?? string.Empty,
                CreatedDay = Math.Max(1, currentDay),
                Quality = quality,
                SentimentalValue = sentimental,
                IsPublic = isPublic,
                Tags = new List<string> { "sketch", medium ?? "pencil" },
                Sketch = new SketchDetails
                {
                    Medium = medium ?? "pencil",
                    ArtisticQuality = artisticQuality,
                    Accuracy = accuracy,
                    TimeSpentHours = Math.Max(0.5f, hoursSpent)
                }
            };

            _state.Items.Add(sketch);

            var ev = new DocumentationEvent
            {
                EventId = $"dev_{_state.NextSequence++}",
                EventType = "sketch_created",
                Day = currentDay,
                AuthorId = authorId,
                DocumentationId = sketch.DocumentationId,
                Description = $"{authorId} sketched '{sketch.Title}'.",
                Significance = quality >= 80f ? "major" : "minor",
                MoraleEffect = quality * 0.03f
            };
            _state.Events.Add(ev);

            OnDocumentationCreated?.Invoke(sketch);
            return sketch;
        }

        public DocumentationItem CreateWrittenRecord(
            string authorId,
            string title,
            string recordType,
            string content,
            float writingQuality,
            int currentDay,
            bool isPublic = true)
        {
            if (string.IsNullOrWhiteSpace(authorId)) throw new ArgumentNullException(nameof(authorId));

            int words = string.IsNullOrWhiteSpace(content) ? 0 : content.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            float quality = Math.Clamp(writingQuality, 0f, 100f);
            float sentimental = Math.Clamp(quality * 0.6f + 20f, 0f, 100f);

            var doc = new DocumentationItem
            {
                DocumentationId = $"doc_w_{_state.NextSequence++}",
                Type = DocumentationType.WrittenRecord,
                AuthorId = authorId.Trim(),
                SubjectType = DocumentationSubjectType.DailyLife,
                SubjectId = recordType ?? "chronicle",
                Title = string.IsNullOrWhiteSpace(title) ? "Record" : title.Trim(),
                Description = string.IsNullOrWhiteSpace(content) ? string.Empty : (content.Length > 80 ? content.Substring(0, 80) + "..." : content),
                CreatedDay = Math.Max(1, currentDay),
                Quality = quality,
                SentimentalValue = sentimental,
                IsPublic = isPublic,
                Tags = new List<string> { "record", recordType ?? "journal" },
                Record = new WrittenRecordDetails
                {
                    RecordType = recordType ?? "journal_entry",
                    Content = content ?? string.Empty,
                    WordCount = words,
                    WritingQuality = quality,
                    IntendedAudience = isPublic ? "shelter" : "private"
                }
            };

            _state.Items.Add(doc);

            var ev = new DocumentationEvent
            {
                EventId = $"dev_{_state.NextSequence++}",
                EventType = "record_written",
                Day = currentDay,
                AuthorId = authorId,
                DocumentationId = doc.DocumentationId,
                Description = $"{authorId} penned record '{doc.Title}'.",
                Significance = quality >= 80f ? "major" : "moderate",
                MoraleEffect = quality * 0.035f
            };
            _state.Events.Add(ev);

            OnDocumentationCreated?.Invoke(doc);
            return doc;
        }

        public PhotoAlbum CreateAlbum(string ownerId, string albumName, string theme, int currentDay, bool isShared = true)
        {
            if (string.IsNullOrWhiteSpace(ownerId)) throw new ArgumentNullException(nameof(ownerId));

            var album = new PhotoAlbum
            {
                AlbumId = $"alb_{_state.NextSequence++}",
                AlbumName = string.IsNullOrWhiteSpace(albumName) ? "Album" : albumName.Trim(),
                OwnerId = ownerId.Trim(),
                Theme = theme ?? "shelter",
                CreatedDay = Math.Max(1, currentDay),
                LastUpdatedDay = Math.Max(1, currentDay),
                IsShared = isShared
            };

            _state.Albums.Add(album);

            var ev = new DocumentationEvent
            {
                EventId = $"dev_{_state.NextSequence++}",
                EventType = "album_created",
                Day = currentDay,
                AuthorId = ownerId,
                DocumentationId = album.AlbumId,
                Description = $"{ownerId} created photo album '{album.AlbumName}'.",
                Significance = "moderate",
                MoraleEffect = 2.0f
            };
            _state.Events.Add(ev);

            OnPhotoAlbumCreated?.Invoke(album);
            return album;
        }

        public bool AddPhotoToAlbum(string albumId, string photoDocumentationId, int currentDay)
        {
            var album = _state.Albums.FirstOrDefault(a => string.Equals(a.AlbumId, albumId, StringComparison.OrdinalIgnoreCase));
            if (album == null) return false;

            var photo = _state.Items.FirstOrDefault(i => string.Equals(i.DocumentationId, photoDocumentationId, StringComparison.OrdinalIgnoreCase));
            if (photo == null || photo.Type != DocumentationType.Photograph) return false;

            if (!album.PhotoIds.Contains(photoDocumentationId))
            {
                album.PhotoIds.Add(photoDocumentationId);
                album.LastUpdatedDay = currentDay;
                OnPhotoAddedToAlbum?.Invoke(albumId, photoDocumentationId);
                return true;
            }

            return false;
        }

        public float ShareDocumentation(string documentationId, int currentDay)
        {
            var doc = _state.Items.FirstOrDefault(i => string.Equals(i.DocumentationId, documentationId, StringComparison.OrdinalIgnoreCase));
            if (doc == null) return 0f;

            doc.IsPublic = true;
            float shelterMoraleBoost = MathF.Round(doc.Quality * 0.05f + doc.SentimentalValue * 0.03f, 1);

            var ev = new DocumentationEvent
            {
                EventId = $"dev_{_state.NextSequence++}",
                EventType = "documentation_shared",
                Day = currentDay,
                AuthorId = doc.AuthorId,
                DocumentationId = doc.DocumentationId,
                Description = $"{doc.AuthorId} shared '{doc.Title}' with the shelter.",
                Significance = doc.Quality >= 75f ? "major" : "moderate",
                MoraleEffect = shelterMoraleBoost
            };
            _state.Events.Add(ev);

            OnDocumentationShared?.Invoke(doc, shelterMoraleBoost);
            return shelterMoraleBoost;
        }

        public IReadOnlyList<DocumentationItem> GetDocumentationByAuthor(string authorId)
        {
            if (string.IsNullOrWhiteSpace(authorId)) return Array.Empty<DocumentationItem>();
            return _state.Items.Where(i => string.Equals(i.AuthorId, authorId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IReadOnlyList<DocumentationItem> GetPublicDocumentation()
        {
            return _state.Items.Where(i => i.IsPublic).ToList();
        }

        public PhotoAlbum? GetAlbum(string albumId)
        {
            return _state.Albums.FirstOrDefault(a => string.Equals(a.AlbumId, albumId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<PhotoAlbum> GetAlbumsForOwner(string ownerId)
        {
            if (string.IsNullOrWhiteSpace(ownerId)) return Array.Empty<PhotoAlbum>();
            return _state.Albums.Where(a => string.Equals(a.OwnerId, ownerId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public DocumentationState CaptureState()
        {
            var captured = new DocumentationState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Items = new List<DocumentationItem>(_state.Items.Count),
                Albums = new List<PhotoAlbum>(_state.Albums.Count),
                Events = new List<DocumentationEvent>(_state.Events.Count)
            };

            foreach (var item in _state.Items)
            {
                var copy = new DocumentationItem
                {
                    DocumentationId = item.DocumentationId,
                    Type = item.Type,
                    AuthorId = item.AuthorId,
                    SubjectType = item.SubjectType,
                    SubjectId = item.SubjectId,
                    Title = item.Title,
                    Description = item.Description,
                    CreatedDay = item.CreatedDay,
                    Quality = item.Quality,
                    SentimentalValue = item.SentimentalValue,
                    IsPublic = item.IsPublic,
                    Tags = new List<string>(item.Tags)
                };

                if (item.Photo != null)
                {
                    copy.Photo = new PhotographDetails
                    {
                        CameraUsed = item.Photo.CameraUsed,
                        Subjects = new List<string>(item.Photo.Subjects),
                        LocationId = item.Photo.LocationId,
                        Composition = item.Photo.Composition,
                        Lighting = item.Photo.Lighting,
                        Moment = item.Photo.Moment
                    };
                }

                if (item.Sketch != null)
                {
                    copy.Sketch = new SketchDetails
                    {
                        Medium = item.Sketch.Medium,
                        ArtisticQuality = item.Sketch.ArtisticQuality,
                        Accuracy = item.Sketch.Accuracy,
                        TimeSpentHours = item.Sketch.TimeSpentHours
                    };
                }

                if (item.Record != null)
                {
                    copy.Record = new WrittenRecordDetails
                    {
                        RecordType = item.Record.RecordType,
                        Content = item.Record.Content,
                        WordCount = item.Record.WordCount,
                        WritingQuality = item.Record.WritingQuality,
                        IntendedAudience = item.Record.IntendedAudience
                    };
                }

                captured.Items.Add(copy);
            }

            foreach (var a in _state.Albums)
            {
                captured.Albums.Add(new PhotoAlbum
                {
                    AlbumId = a.AlbumId,
                    AlbumName = a.AlbumName,
                    OwnerId = a.OwnerId,
                    PhotoIds = new List<string>(a.PhotoIds),
                    Theme = a.Theme,
                    CreatedDay = a.CreatedDay,
                    LastUpdatedDay = a.LastUpdatedDay,
                    IsShared = a.IsShared
                });
            }

            foreach (var e in _state.Events)
            {
                captured.Events.Add(new DocumentationEvent
                {
                    EventId = e.EventId,
                    EventType = e.EventType,
                    Day = e.Day,
                    AuthorId = e.AuthorId,
                    DocumentationId = e.DocumentationId,
                    Description = e.Description,
                    Significance = e.Significance,
                    MoraleEffect = e.MoraleEffect
                });
            }

            return captured;
        }

        public void RestoreState(DocumentationState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Items.Clear();
            _state.Albums.Clear();
            _state.Events.Clear();

            if (state.Items != null)
            {
                foreach (var item in state.Items)
                {
                    var restored = new DocumentationItem
                    {
                        DocumentationId = item.DocumentationId,
                        Type = item.Type,
                        AuthorId = item.AuthorId,
                        SubjectType = item.SubjectType,
                        SubjectId = item.SubjectId,
                        Title = item.Title,
                        Description = item.Description,
                        CreatedDay = item.CreatedDay,
                        Quality = item.Quality,
                        SentimentalValue = item.SentimentalValue,
                        IsPublic = item.IsPublic,
                        Tags = new List<string>(item.Tags ?? Enumerable.Empty<string>())
                    };

                    if (item.Photo != null)
                    {
                        restored.Photo = new PhotographDetails
                        {
                            CameraUsed = item.Photo.CameraUsed,
                            Subjects = new List<string>(item.Photo.Subjects ?? Enumerable.Empty<string>()),
                            LocationId = item.Photo.LocationId,
                            Composition = item.Photo.Composition,
                            Lighting = item.Photo.Lighting,
                            Moment = item.Photo.Moment
                        };
                    }

                    if (item.Sketch != null)
                    {
                        restored.Sketch = new SketchDetails
                        {
                            Medium = item.Sketch.Medium,
                            ArtisticQuality = item.Sketch.ArtisticQuality,
                            Accuracy = item.Sketch.Accuracy,
                            TimeSpentHours = item.Sketch.TimeSpentHours
                        };
                    }

                    if (item.Record != null)
                    {
                        restored.Record = new WrittenRecordDetails
                        {
                            RecordType = item.Record.RecordType,
                            Content = item.Record.Content,
                            WordCount = item.Record.WordCount,
                            WritingQuality = item.Record.WritingQuality,
                            IntendedAudience = item.Record.IntendedAudience
                        };
                    }

                    _state.Items.Add(restored);
                }
            }

            if (state.Albums != null)
            {
                foreach (var a in state.Albums)
                {
                    _state.Albums.Add(new PhotoAlbum
                    {
                        AlbumId = a.AlbumId,
                        AlbumName = a.AlbumName,
                        OwnerId = a.OwnerId,
                        PhotoIds = new List<string>(a.PhotoIds ?? Enumerable.Empty<string>()),
                        Theme = a.Theme,
                        CreatedDay = a.CreatedDay,
                        LastUpdatedDay = a.LastUpdatedDay,
                        IsShared = a.IsShared
                    });
                }
            }

            if (state.Events != null)
            {
                foreach (var e in state.Events)
                {
                    _state.Events.Add(new DocumentationEvent
                    {
                        EventId = e.EventId,
                        EventType = e.EventType,
                        Day = e.Day,
                        AuthorId = e.AuthorId,
                        DocumentationId = e.DocumentationId,
                        Description = e.Description,
                        Significance = e.Significance,
                        MoraleEffect = e.MoraleEffect
                    });
                }
            }
        }
    }
}
