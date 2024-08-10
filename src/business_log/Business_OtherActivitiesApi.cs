using Alman.SharedDefinitions;
using Alman.SharedModels;
using AutoMapper;
using DatabaseAccess;
using DbAccess.Models;
using System.Collections.ObjectModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business;


public class OtherActivitiesMapper : IAlmanEntitiesMapper<OtherActivity, IOtherActivityBase>
{
    public IMapper EntityMapper { get; set; }
    public OtherActivitiesMapper()
    {
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<IStaffMemberBase, StaffMember>());
        EntityMapper = config.CreateMapper();
    }
}


