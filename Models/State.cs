namespace WorkflowEngine.Models;

public class State
{
    public string Id { get; set; } = "";
    public bool IsStart { get; set; }
    public bool IsEnd { get; set; }
    public bool Active { get; set; } = true;
    public string? Info { get; set; }
}