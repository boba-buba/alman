namespace Alman.SharedDefinitions;

/// <summary>
/// Types of the possible contract that a child can have.
/// </summary>
public enum ContractType
{
    Precontract = 0,
    MotherCapital,
    OrdinaryContract,
    StaffChild
}

/// <summary>
/// Return code for indication success or failure of the operations.
/// </summary>
public enum ReturnCode
{
    OK = 0,
    NOT_FOUND_IN_DB,
    SAVE_CTX_ERR,
    NOT_IMPL,
    NOT_SUPPORTED,
    ERR,
}

/// <summary>
/// Flag that indicates whether the child attends the kindergarten or no.
/// </summary>
public enum ChildState
{
    Inactive = 0,
    Active
}

/// <summary>
/// Flag that indicates whether the staff member is still working or no.
/// </summary>
public enum StaffMemberState
{
    Inactive = 0,
    Active
}

//Basically states wether the sum was paid or not, therefore default value is 0 that means its not paid
/// <summary>
/// Differnet ways of the paying for something. 0 means it was not paid.
/// </summary>
public enum WayOfPaying
{
    NotPaid = 0,
    Cash,
    Transfer
}

/// <summary>
///  Differnet ways of the paying for activity.
/// </summary>
public enum WayOfPaingForActivity
{
    Subscription = 1,
    EveryLesson
}

/// <summary>
/// Flagthat indicates if it was paid for the expense, activity, etc.
/// </summary>
public enum WasPaid
{
    False = 0,
    True
}

/// <summary>
/// Various user permissions, mainly for distinguishing between the users that can manage other users and those who can not.
/// </summary>
public enum UserPermissions
{
    None = 0,
    ManageUsers = 1,
}
