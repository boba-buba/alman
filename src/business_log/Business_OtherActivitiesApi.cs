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

public interface IAlmanEntitiesMapper<TEntity, TIface> 
    where TEntity : class, TIface
    where TIface : class
{
    public IMapper EntityMapper { get; set; }
}

public class OtherActivitiesMapper : IAlmanEntitiesMapper<OtherActivity, IOtherActivityBase>
{
    public IMapper EntityMapper { get; set; }
    public OtherActivitiesMapper()
    {
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<IStaffMemberBase, StaffMember>());
        EntityMapper = config.CreateMapper();
    }
}

public class BusinessOtherApi<TEntity, TIface>
    where TEntity : class, TIface
    where TIface : class
{
    IAlmanEntitiesMapper<TEntity, TIface> EntityMapper { get; set; }
    public BusinessOtherApi(IAlmanEntitiesMapper<TEntity, TIface> mapper)
    {
        EntityMapper = mapper;
    }

    public IReadOnlyList<TIface> GetEntities()
    {
        var db = new DbOther();
        return db.GetEntitiesGen<TEntity>(ent => true);
    }

    public ReturnCode AddEntities(IReadOnlyList<TIface> newEntities)
    {
        var db = new DbOther();
        Collection<TEntity> entities = new Collection<TEntity>();

        foreach (var entity in newEntities)
        {
            entities.Add(EntityMapper.EntityMapper.Map<TEntity>(entity));
        }

        return db.AddEntitiesGen(entities);
    }
}
