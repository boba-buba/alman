namespace Alman.SharedDefinitions;

public enum ContractType
{
    Precontract = 0,
    MotherCapital,
    OrdinaryContract,
    StaffChild
}

public enum ReturnCode
{
    OK = 0,
    NOT_FOUND_IN_DB,
    SAVE_CTX_ERR,
    NOT_IMPL,
    NOT_SUPPORTED,
    ERR,


}

public enum ChildState
{
    Inactive = 0,
    Active
}

public enum StaffMemberState
{
    Inactive = 0,
    Active
}

public enum WayOfPaying
{
    Cash = 0,
    Transfer
}

public enum WayOfPaingForActivity
{
    Subscription = 0,
    EveryLesson
}


public enum WasPaid
{
    False = 0,
    True
}

