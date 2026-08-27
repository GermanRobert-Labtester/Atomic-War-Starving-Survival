using System;
using System.Collections.Generic;

namespace Ashfall.Core.Random
{
    /// <summary>Canonical stream IDs for domain-specific simulation RNG streams.</summary>
    public static class CampaignStreamIds
    {
        public const string Weather = "weather";
        public const string Combat = "combat";
        public const string Disease = "disease";
        public const string Greenhouse = "greenhouse";
        public const string Expedition = "expedition";
        public const string Narrative = "narrative";
        public const string Economy = "economy";
        public const string Radio = "radio";
        public const string Social = "social";
        public const string MoralChoice = "moral_choice";
        public const string Shelter = "shelter";
        public const string DutyRoster = "duty_roster";
        public const string Muster = "muster";
        public const string Foundry = "foundry";
        public const string Maritime = "maritime";
        public const string Psychology = "psychology";
        public const string Events = "events";
    }

    /// <summary>Domain-isolated deterministic RNG stream derived from a campaign master seed.</summary>
    public interface ICampaignRngStream
    {
        string StreamId { get; }
        int MasterSeed { get; }
        int DerivedBaseSeed { get; }
        ISeededRng Rng { get; }
        int Position { get; }
        ISeededRng Fork(int day = 0, int actionIndex = 0);
        int ForkSeed(int day = 0, int actionIndex = 0);
        void RestorePosition(int position);
    }

    public sealed class CampaignRngStream : ICampaignRngStream
    {
        private readonly int _masterSeed;
        private readonly string _streamId;
        private readonly int _derivationVersion;
        private readonly int _derivedBaseSeed;
        private ISeededRng _rng;
        private int _position;

        public string StreamId => _streamId;
        public int MasterSeed => _masterSeed;
        public int DerivedBaseSeed => _derivedBaseSeed;
        public int Position => _position;

        public ISeededRng Rng
        {
            get
            {
                _position++;
                return _rng;
            }
        }

        public CampaignRngStream(int masterSeed, string streamId, int derivationVersion = 1)
        {
            _masterSeed = masterSeed;
            _streamId = streamId ?? throw new ArgumentNullException(nameof(streamId));
            _derivationVersion = derivationVersion;
            _derivedBaseSeed = DeriveSeed(masterSeed, streamId, derivationVersion, day: 0, actionIndex: 0);
            _rng = new SeededRng(_derivedBaseSeed);
            _position = 0;
        }

        public static int DeriveSeed(int masterSeed, string streamId, int version = 1, int day = 0, int actionIndex = 0)
        {
            if (version == 0)
            {
                // Version 0: legacy compatibility mappings
                return streamId switch
                {
                    CampaignStreamIds.MoralChoice => 2026,
                    CampaignStreamIds.Radio => 2026,
                    CampaignStreamIds.Economy => 2026,
                    _ => unchecked(1986 + day * 31 + actionIndex)
                };
            }

            // Version 1: standard StableHash derivation
            int hash = StableHash.Of(streamId);
            unchecked
            {
                int s = masterSeed * 31337 + hash * 1009 + day * 37 + actionIndex;
                return s != 0 ? s : 1986;
            }
        }

        public ISeededRng Fork(int day = 0, int actionIndex = 0)
        {
            int seed = ForkSeed(day, actionIndex);
            return new SeededRng(seed);
        }

        public int ForkSeed(int day = 0, int actionIndex = 0)
        {
            return DeriveSeed(_masterSeed, _streamId, _derivationVersion, day, actionIndex);
        }

        public void RestorePosition(int position)
        {
            _rng = new SeededRng(_derivedBaseSeed);
            _position = 0;
            for (int i = 0; i < position; i++)
            {
                _rng.Next(0, int.MaxValue);
                _position++;
            }
        }
    }

    /// <summary>Coordinator managing named campaign RNG streams derived from a single master seed.</summary>
    public interface ICampaignRngManager
    {
        int MasterSeed { get; }
        int DerivationVersion { get; }
        ICampaignRngStream GetStream(string streamId);
        ISeededRng Fork(string streamId, int day = 0, int actionIndex = 0);
        string FormatDiagnostics(string streamId, int day = 0, int actionIndex = 0);
        Dictionary<string, int> CapturePositions();
        void RestorePositions(Dictionary<string, int>? positions);
    }

    public sealed class CampaignRngManager : ICampaignRngManager
    {
        public const int DefaultMasterSeed = 1986;
        public const int CurrentDerivationVersion = 1;

        private readonly int _masterSeed;
        private readonly int _derivationVersion;
        private readonly Dictionary<string, CampaignRngStream> _streams =
            new Dictionary<string, CampaignRngStream>(StringComparer.Ordinal);

        public int MasterSeed => _masterSeed;
        public int DerivationVersion => _derivationVersion;

        public CampaignRngManager(int masterSeed = DefaultMasterSeed, int derivationVersion = CurrentDerivationVersion)
        {
            _masterSeed = masterSeed;
            _derivationVersion = derivationVersion;
        }

        public ICampaignRngStream GetStream(string streamId)
        {
            if (string.IsNullOrWhiteSpace(streamId))
                throw new ArgumentException("Stream ID must not be empty.", nameof(streamId));

            if (!_streams.TryGetValue(streamId, out var stream))
            {
                stream = new CampaignRngStream(_masterSeed, streamId, _derivationVersion);
                _streams[streamId] = stream;
            }
            return stream;
        }

        public ISeededRng Fork(string streamId, int day = 0, int actionIndex = 0)
        {
            return GetStream(streamId).Fork(day, actionIndex);
        }

        public string FormatDiagnostics(string streamId, int day = 0, int actionIndex = 0)
        {
            var stream = GetStream(streamId);
            int forkSeed = stream.ForkSeed(day, actionIndex);
            return $"[RNG_STREAM] id='{streamId}' master={_masterSeed} derived={forkSeed} day={day} action={actionIndex} pos={stream.Position}";
        }

        public Dictionary<string, int> CapturePositions()
        {
            var map = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var kv in _streams)
            {
                map[kv.Key] = kv.Value.Position;
            }
            return map;
        }

        public void RestorePositions(Dictionary<string, int>? positions)
        {
            if (positions == null) return;
            foreach (var kv in positions)
            {
                if (_streams.TryGetValue(kv.Key, out var stream))
                {
                    stream.RestorePosition(kv.Value);
                }
            }
        }
    }
}
