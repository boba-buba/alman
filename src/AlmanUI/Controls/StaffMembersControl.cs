using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Controls;

public static class StaffMembersControl
{
    public static IReadOnlyList<IStaffMemberBase> GetStaffMembers() =>
        BusinessStaffMembersApi.GetStaffMembers();

    public static IReadOnlyList<IStaffMemberBase> GetStaffMembersByFilter(Func<IStaffMemberBase, bool> filter) =>
        BusinessStaffMembersApi.GetStaffMembersByFilter(filter);

    public static ReturnCode AddStaffMembers(IReadOnlyList<IStaffMemberBase> members) =>
        BusinessStaffMembersApi.AddStaffMembers(members);

    public static ReturnCode DeleteStaffMembers(IList<int> membersIdsToDelete) =>
        BusinessStaffMembersApi.DeleteStaffMembers(membersIdsToDelete);

    public static ReturnCode UpdateStaffMembers(IReadOnlyList<IStaffMemberBase> members) =>
        BusinessStaffMembersApi.UpdateStaffMembers(members);

    public static ReturnCode SaveStaffMembers(IReadOnlyList<IStaffMemberBase> membersToSave, IList<int> memberIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (memberIdsToDelete.Count > 0)
        {
            retCode = DeleteStaffMembers(memberIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IStaffMemberBase)}'s.");
                return retCode;
            }
        }

        var staffMembersFromDb = GetStaffMembers();
        int dbCount = staffMembersFromDb.Count;
        int difference = membersToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedStaffMembers = membersToSave.Where(m => m.InGroup(staffMembersFromDb)).ToList();
            retCode = UpdateStaffMembers(updatedStaffMembers);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IStaffMemberBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newStaffMembers = membersToSave.Where(m => !m.InGroup(staffMembersFromDb)).ToList();
            retCode = AddStaffMembers(newStaffMembers);
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
        if (item.StaffMemberId != other.StaffMemberId) { return false; }
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
