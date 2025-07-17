namespace WorkflowEngine.Models;

public class WorkflowInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string WorkflowId { get; set; } = "";
    public string Current { get; set; } = "";

    public List<ActionLog> History { get; set; } = new();
}

public class ActionLog
{
    public string ActionId { get; set; } = "";
    public DateTime Time { get; set; } = DateTime.UtcNow;
}