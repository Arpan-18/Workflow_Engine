namespace WorkflowEngine.Models;

public class WorkflowDef
{
    public string Id { get; set; } = "";
    public List<State> States { get; set; } = new();
    public List<ActionDef> Actions { get; set; } = new();
}
