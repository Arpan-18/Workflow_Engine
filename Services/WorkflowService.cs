using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

public class WorkflowService
{
    readonly Dictionary<string, WorkflowDef> defs = new();
    readonly Dictionary<string, WorkflowInstance> inst = new();

    public bool Add(WorkflowDef def, out string err)
    {
        err = "";

        if (def.States.GroupBy(s => s.Id).Any(g => g.Count() > 1))
        {
            err = "State IDs must be unique.";
            return false;
        }

        if (def.Actions.GroupBy(a => a.Id).Any(g => g.Count() > 1))
        {
            err = "Action IDs must be unique.";
            return false;
        }

        if (def.States.Count(s => s.IsStart) != 1)
        {
            err = "One and only one start state required.";
            return false;
        }

        var ids = def.States.Select(s => s.Id).ToHashSet();
        foreach (var act in def.Actions)
        {
            if (!ids.Contains(act.To) || act.From.Any(f => !ids.Contains(f)))
            {
                err = $"Invalid state ref in action '{act.Id}'.";
                return false;
            }
        }

        defs[def.Id] = def;
        return true;
    }

    public WorkflowDef? GetDef(string id)
    {
        return defs.TryGetValue(id, out var d) ? d : null;
    }

    public List<WorkflowDef> AllDefs() => defs.Values.ToList();
    public List<WorkflowInstance> AllInst() => inst.Values.ToList();

    public WorkflowInstance? GetInst(string id)
    {
        return inst.TryGetValue(id, out var i) ? i : null;
    }

    public WorkflowInstance? Start(string defId)
    {
        if (!defs.TryGetValue(defId, out var def))
            return null;

        var start = def.States.First(s => s.IsStart && s.Active);
        var i = new WorkflowInstance
        {
            WorkflowId = defId,
            Current = start.Id
        };

        inst[i.Id] = i;
        return i;
    }

    public bool Act(string instId, string actId, out string err)
    {
        err = "";

        if (!inst.TryGetValue(instId, out var i))
        {
            err = "Instance not found.";
            return false;
        }

        var def = defs[i.WorkflowId];
        var cur = i.Current;

        var a = def.Actions.FirstOrDefault(x => x.Id == actId);
        if (a == null)
        {
            err = "Action not found.";
            return false;
        }

        if (!a.Enabled)
        {
            err = "Action disabled.";
            return false;
        }

        if (!a.From.Contains(cur))
        {
            err = $"Cannot do '{actId}' from '{cur}'.";
            return false;
        }

        var s = def.States.FirstOrDefault(x => x.Id == cur);
        if (s?.IsEnd == true)
        {
            err = "Already at final state.";
            return false;
        }

        i.Current = a.To;
        i.History.Add(new ActionLog { ActionId = actId, Time = DateTime.UtcNow });
        return true;
    }
}
