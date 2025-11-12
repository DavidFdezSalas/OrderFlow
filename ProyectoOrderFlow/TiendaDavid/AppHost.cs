var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithLifetime(ContainerLifetime.Persistent).WithHostPort(51197);
var postgresdb = postgres.AddDatabase("identitydb");

//var cache = builder.AddRedis("cache");


var identityApi = builder.AddProject<Projects.TiendaDavid_Identity>("tiendadavid-identity").WithReference(postgresdb);
builder.AddNpmApp("tiendadavid-react", "../TiendaDavid.React", "dev").WithReference(identityApi).WithHttpEndpoint(port: 5173, env: "PORT").WithExternalHttpEndpoints().PublishAsDockerFile();


builder.Build().Run();