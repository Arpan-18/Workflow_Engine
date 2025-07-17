namespace WorkflowEngine.Models;

public class ActionDef
{
    public string Id { get; set; } = "";
    public bool Enabled { get; set; } = true;
    public List<string> From { get; set; } = new();
    public string To { get; set; } = "";
}