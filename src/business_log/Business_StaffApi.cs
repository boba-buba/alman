using Alman.SharedDefinitions;
using Alman.SharedModels;
using AutoMapper;
using DatabaseAccess;
using DbAccess.Models;
using System.Diagnostics;
namespace Business;


public class StaffMembersMapper
{
    public IMapper StaffMemberMapper { get; private set; }
    public StaffMembersMapper()
    {
        MapperConfiguration YearSubConfig = new MapperConfiguration(cfg => cfg.CreateMap<IStaffMemberBase, StaffMember>());
        StaffMemberMapper = YearSubConfig.CreateMapper();
    }

}
public static class BusinessStaffMembersApi
{
    public static IReadOnlyList<IStaffMemberBase> GetStaffMembers()
    {
        var db = new DbStaff();
        return db.GetStaffMembers(m => true);
    }

    public static IReadOnlyList<IStaffMemberBase> GetStaffMembersByFilter(Func<IStaffMemberBase, bool> filter)
    {
        var db = new DbStaff();
        return db.GetStaffMembers(filter);
    }

    public static ReturnCode AddStaffMembers(IReadOnlyList<IStaffMemberBase> newMembers)
    {
        var db = new DbStaff();
        var members = new List<StaffMember>();
        var mapper = new StaffMembersMapper();
        foreach (var member in newMembers)
        {
            members.Add(mapper.StaffMemberMapper.Map<StaffMember>(member));
        }
        return db.AddStaffMembers(members);
    }

    public static ReturnCode UpdateStaffMembers(IReadOnlyList<IStaffMemberBase> updatedMembers)
    {
        var db = new DbStaff();
        var membersFromDb = db.GetStaffMembers(m => true);
        var mapper = new StaffMembersMapper();

        foreach (var updatedMember in updatedMembers)
        {
            var memberFromDb = membersFromDb.SingleOrDefault(m => m.StaffMemberId == updatedMember.StaffMemberId);
            if (memberFromDb is null)
            {
                Debug.WriteLine($"No such {nameof(IStaffMemberBase)} in DB");
                return ReturnCode.NOT_FOUND_IN_DB;
            }
            mapper.StaffMemberMapper.Map(updatedMember, memberFromDb);
        }
        return db.UpdateStaffMembers(membersFromDb);
    }
    
    public static ReturnCode DeleteStaffMembers(IList<int> membersIdsToDelete)
    {
        var db = new DbStaff();
        var membersToDelete = db.GetStaffMembers(m => membersIdsToDelete.Contains(m.StaffMemberId));
        return db.DeleteStaffMembers(membersToDelete);
    }
}
