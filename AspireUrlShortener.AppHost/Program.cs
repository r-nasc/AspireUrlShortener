var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("urlshortener-postgres")
    .WithPgAdmin(pgAdmin => pgAdmin.WithLifetime(ContainerLifetime.Persistent))
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var postgresDb = postgres.AddDatabase("urlshortener-postgres-db");

var redis = builder.AddRedis("urlshortener-redis")
    .WithLifetime(ContainerLifetime.Persistent);

var api = builder.AddProject<Projects.AspireUrlShortener_Api>("urlshortener-api")
    .WithReference(postgresDb)
    .WithReference(redis)
    .WaitFor(postgresDb)
    .WaitFor(redis)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("react", "../AspireUrlShortener.Web")
    .WithReference(api)
    .WaitFor(api)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "VITE_PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
