namespace Alman.SharedModels;

/// <summary>
/// Common model Expense entity.
/// </summary>
public interface IExpenseBase : IIdentifier
{
    /// <summary>
    /// Name of the axpense.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Month in which the expanse occured.
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Year in which the expense occurred.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Sum that was paid for the expense.
    /// </summary>
    public int ExpenseSum { get; set;}

    /// <summary>
    /// The way in which the expense was paid (Cash or money transfer).
    /// </summary>
    public int WayOfPaying { get; set; }

}
