var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.GFSWeb>("gfsweb");

builder.Build().Run();
