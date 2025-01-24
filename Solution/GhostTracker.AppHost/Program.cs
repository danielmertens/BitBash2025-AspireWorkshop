var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var ghostManagerApi = builder.AddProject<Projects.GhostTracker_GhostManager>("ghostmanagerapi")
    .WithReference(rabbitmq);
var pathfinderApi = builder.AddProject<Projects.GhostTracker_PathFinderApi>("pathfinderapi")
    .WithReference(rabbitmq);

var bff = builder.AddProject<Projects.GhostTracker_Bff>("bff")
    .WithExternalHttpEndpoints()
    .WithReference(ghostManagerApi)
    .WithReference(pathfinderApi);

for (int i = 1; i < 6; i++)
{
    builder.AddProject<Projects.GhostTracker_Transmitter>($"GhostTracker-transmitter-{i}")
        .WithReference(ghostManagerApi)
        .WithReference(pathfinderApi)
        .WithEnvironment("GhostId", i.ToString());
}

builder.AddNpmApp("react", "../GhostTracker.React")
    .WithReference(bff)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddProject<Projects.GhostTracker_Transmitter_RabbitMQ>("ghosttracker-transmitter-rabbitmq")
    .WithReference(rabbitmq)
    .WithEnvironment("GhostId", "9");

builder.Build().Run();
