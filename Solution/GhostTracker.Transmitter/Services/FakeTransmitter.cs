﻿using GhostTracker.Transmitter.ApiClients;
using GhostTracker.Transmitter.Interfaces;
using GhostTracker.Transmitter.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace GhostTracker.Transmitter.Services;

public class FakeTransmitter : ITransmitter
{
    private Image<Rgba32>? _mapMask = null;
    private readonly LinkedList<GhostLocation> _memory = [];
    private readonly GhostManagerApiClient _ghostManagerApiClient;
    private readonly PathFinderApiClient _pathFinderApiClient;
    private bool _transmitterInitialized = false;

    // Constants
    private const int SPEED = 30;
    private const int SPAWN_EDGEOFFSET_WIDTH = 80;
    private const int SPAWN_EDGEOFFSET_HEIGHT = 50;

    public FakeTransmitter(GhostManagerApiClient ghostManagerApiClient,
        PathFinderApiClient pathFinderApiClient)
    {
        _ghostManagerApiClient = ghostManagerApiClient;
        _pathFinderApiClient = pathFinderApiClient;
    }

    public async Task BringOnline(int ghostId)
    {
        await Initialize();
        await _ghostManagerApiClient.BringOnline(ghostId);
    }

    public async Task TakeOffline(int ghostId)
    {
        await _ghostManagerApiClient.TakeOffline(ghostId);
    }

    public async Task TransmitLocation(int ghostId)
    {
        if (!_transmitterInitialized) return;

        GhostLocation newLocation;
        if (!_memory.Any())
        {
            newLocation = CreateFirstLocation();
        }
        else
        {
            newLocation = GenerateNextLocation(_memory.Last());
        }

        _memory.AddLast(newLocation);
        await _pathFinderApiClient.TransmitLocation(ghostId, newLocation);
    }

    private GhostLocation GenerateNextLocation(GhostLocation location)
    {
        var vect = new Vector2(location.Heading.X, location.Heading.Y);
        while (true)
        {
            // Rotate heading slightly randomly. (Perlin noise)
            vect = Vector2.Transform(vect,
                Matrix3x2.CreateRotation((float)(Random.Shared.NextDouble() * 2 - 1)));

            var newX = (int)(location.Coordinate.X + vect.X * SPEED);
            var newY = (int)(location.Coordinate.Y + vect.Y * SPEED);

            if (newX > 0 && newY > 0
                && newX < _mapMask.Width && newY < _mapMask.Height
                && _mapMask[newX, newY] == Rgba32.ParseHex("000000"))
            {
                return new GhostLocation
                {
                    Coordinate = new() { X = newX, Y = newY },
                    Heading = new() { X = vect.X, Y = vect.Y }
                };
            }
        }
    }

    private GhostLocation CreateFirstLocation()
    {
        while (true)
        {
            var x = Random.Shared.Next(SPAWN_EDGEOFFSET_WIDTH, _mapMask.Width - (SPAWN_EDGEOFFSET_WIDTH * 2));
            var y = Random.Shared.Next(SPAWN_EDGEOFFSET_HEIGHT, _mapMask.Height - (SPAWN_EDGEOFFSET_HEIGHT * 2));

            var vect = new Vector2(
                (float)(Random.Shared.NextDouble() * 2 - 1),
                (float)(Random.Shared.NextDouble() * 2 - 1));
            vect = Vector2.Normalize(vect);

            var color = _mapMask[x, y];
            if (color == Rgba32.ParseHex("000000"))
            {
                return new GhostLocation()
                {
                    Coordinate = new Coordinate { X = x, Y = y },
                    Heading = new Heading
                    {
                        X = vect.X,
                        Y = vect.Y
                    }
                };
            }
        }
    }

    private async Task Initialize()
    {
        _mapMask = await Image.LoadAsync<Rgba32>("./Assets/street_map_mask.jpeg");
        _transmitterInitialized = true;
    }
}
