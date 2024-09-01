using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;

/// <summary>
/// API for managing the data in StaffMembersViewModel.
/// </summary>
public class StaffMembersControl : ControlBase<StaffMember, IStaffMemberBase>
{
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <param name="itemsIdsToDelete">Ids of the items that must be deleted from database.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>
    public static ReturnCode SaveItems(IReadOnlyList<IStaffMemberBase> itemsToSave, IList<int> itemsIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (itemsIdsToDelete.Count > 0)
        {
            retCode = DeleteItems(itemsIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IStaffMemberBase)}'s.");
                return retCode;
            }
        }

        var staffMembersFromDb = GetItems();
        int dbCount = staffMembersFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedStaffMembers = itemsToSave.Where(m => m.InGroup(staffMembersFromDb)).ToList();
            retCode = UpdateItems(updatedStaffMembers);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IStaffMemberBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newStaffMembers = itemsToSave.Where(m => !m.InGroup(staffMembersFromDb)).ToList();
            retCode = AddItems(newStaffMembers);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IStaffMemberBase)}");
            }
        }
        return retCode;
    }
}


public static class StaffMemberExtensions
{
    public static bool DbEquals(this IStaffMemberBase item, IStaffMemberBase other)
    {
        if (item.Id != other.Id) { return false; }
        return true;
    }
     
    public static bool InGroup(this IStaffMemberBase item, IReadOnlyList<IStaffMemberBase> group)
    {
        if (group is null || group.Count == 0) { return false; }
        var itemFromGroup = group.SingleOrDefault(m => m.DbEquals(item));
        if (itemFromGroup is null) { return false; }
        return true;
    }
}
