namespace MemoAccount.Services.Repository;

public class ActionResult<TVal>(TVal? res, ActionStatus status, string? msg = null) : ActionResult(status, msg)
    where TVal : class
{
    public TVal? Result => res;

    public new static ActionResult<TVal> Error(string? msg = null) => new(default, ActionStatus.Error, msg);
    public new static ActionResult<TVal> Success(TVal res) => new(res, ActionStatus.Success);
}

public class ActionResult(ActionStatus status, string? msg = null)
{
    public ActionStatus Status => status;
    public string? ErrorMessage => msg;

    public static ActionResult Error(string? msg = null) => new(ActionStatus.Error, msg);
    public static ActionResult Success => new(ActionStatus.Success);
}