namespace GhostTracker.GhostManager;

public static class GhostCache
{
    public static Ghost[] Ghosts { get; } = [
        new Ghost { Id = 1, Name = "Slimer", Type = "Ectoplasmic", Age = 50, DateOfDead = new DateTime(1975, 6, 20), HauntLocation = "Sedgewick Hotel", Appearance = "Green blob with a big appetite", DangerLevel = 3, Abilities = "Eating, Making mess" },
        new Ghost { Id = 2, Name = "Lady in White", Type = "Spectral", Age = 100, DateOfDead = new DateTime(1925, 10, 31), HauntLocation = "Shandor Library", Appearance = "Pale woman in white dress", DangerLevel = 4, Abilities = "Invisibility, Wailing" },
        new Ghost { Id = 3, Name = "Marshmallow Man", Type = "Manifestation", Age = 35, DateOfDead = new DateTime(1990, 5, 5), HauntLocation = "Downtown", Appearance = "Giant marshmallow", DangerLevel = 5, Abilities = "Size alteration, Resilience" },
        new Ghost { Id = 4, Name = "Screaming Specter", Type = "Banshee", Age = 250, DateOfDead = new DateTime(1775, 11, 2), HauntLocation = "Old Cemetery", Appearance = "Hooded figure", DangerLevel = 4, Abilities = "Screaming, Summoning smaller spirits" },
        new Ghost { Id = 5, Name = "Phantom of the Opera", Type = "Wraith", Age = 150, DateOfDead = new DateTime(1875, 3, 14), HauntLocation = "Abandoned Theater", Appearance = "Masked figure", DangerLevel = 4, Abilities = "Illusions, Music manipulation" },
        new Ghost { Id = 6, Name = "Poltergeist Phil", Type = "Poltergeist", Age = 60, DateOfDead = new DateTime(1965, 7, 20), HauntLocation = "Suburban House", Appearance = "Invisible force", DangerLevel = 3, Abilities = "Moving objects, Causing disturbances" },
        new Ghost { Id = 7, Name = "Vaporous Vlad", Type = "Ethereal", Age = 400, DateOfDead = new DateTime(1625, 8, 15), HauntLocation = "Ancient Ruins", Appearance = "Mist-like form", DangerLevel = 5, Abilities = "Teleportation, Phasing" },
        new Ghost { Id = 8, Name = "Haunting Harriet", Type = "Revenant", Age = 200, DateOfDead = new DateTime(1825, 11, 10), HauntLocation = "Deserted Mansion", Appearance = "Victorian woman", DangerLevel = 4, Abilities = "Possession, Shadow manipulation" },
        new Ghost { Id = 9, Name = "Wail of Woe", Type = "Phantom", Age = 300, DateOfDead = new DateTime(1725, 2, 28), HauntLocation = "Seaside Tavern", Appearance = "Seafaring ghost", DangerLevel = 4, Abilities = "Weather control, Crying" },
        new Ghost { Id = 10, Name = "Spectral Steve", Type = "Apparition", Age = 75, DateOfDead = new DateTime(1945, 4, 22), HauntLocation = "War Memorial", Appearance = "Soldier’s ghost", DangerLevel = 3, Abilities = "Creating illusions, Camouflage" }
    ];
}

public record Ghost
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Type { get; init; }
    public int Age { get; set; }
    public DateTime DateOfDead { get; set; }
    public string HauntLocation { get; set; }
    public string Appearance { get; set; }
    public int DangerLevel { get; set; }
    public string Abilities { get; set; }
    public bool Online { get; set; }
}
