using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using WorkflowEngine.Models;
using WorkflowEngine.Services;

var b = WebApplication.CreateBuilder(args);
b.Services.AddSingleton<WorkflowService>();
b.Services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = null);

var app = b.Build();
var svc = app.Services.GetRequiredService<WorkflowService>();

app.MapPost("/workflows", (WorkflowDef def) =>
{
    return svc.Add(def, out var err) ? Results.Ok("Created") : Results.BadRequest(err);
});

app.MapGet("/workflows", () => Results.Ok(svc.AllDefs()));

app.MapGet("/workflows/{id}", (string id) =>
{
    var def = svc.GetDef(id);
    return def is not null ? Results.Ok(def) : Results.NotFound("Not found");
});

app.MapPost("/instances/{wid}", (string wid) =>
{
    var inst = svc.Start(wid);
    return inst is not null ? Results.Ok(inst) : Results.NotFound("Workflow not found");
});

app.MapPost("/instances/{iid}/actions/{aid}", (string iid, string aid) =>
{
    return svc.Act(iid, aid, out var err) ? Results.Ok("Done") : Results.BadRequest(err);
});

app.MapGet("/instances", () => Results.Ok(svc.AllInst()));

app.MapGet("/instances/{id}", (string id) =>
{
    var inst = svc.GetInst(id);
    return inst is not null ? Results.Ok(inst) : Results.NotFound("Not found");
});

app.Run();
