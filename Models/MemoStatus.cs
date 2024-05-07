using System.ComponentModel;

namespace MemoAccount.Models;

public enum MemoStatus
{
    [Description("Открыта")]
    Open,
    [Description("Закрыта")]
    Closed
}