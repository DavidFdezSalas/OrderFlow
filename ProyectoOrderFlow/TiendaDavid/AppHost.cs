var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("identitydb");


builder.AddProject<Projects.TiendaDavid_Identity>("tiendadavid-identity").WithReference(postgresdb);

builder.Build().Run();