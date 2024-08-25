using Alman.SharedDefinitions;
using Alman.SharedModels;
using AutoMapper;
using DatabaseAccess;
using DbAccess.Models;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Business;

public interface IAlmanEntitiesMapper<TEntity, TIface>
    where TEntity : class, TIface
    where TIface : class
{
    public IMapper EntityMapper { get; set; }
}


public class BusinessEntityMapper<TEntity, TIface> : IAlmanEntitiesMapper<TEntity, TIface>
    where TEntity : class, TIface
    where TIface : class
{
    public IMapper EntityMapper { get; set; }
    public BusinessEntityMapper()
    {
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<TIface, TEntity>());
        EntityMapper = config.CreateMapper();
    }
}


public class BusinessEntity<TEntity, TIface>
    where TEntity : class, TIface, IIdentifier, IDeleteDependable, new()
    where TIface : class, IIdentifier
{
    BusinessEntityMapper<TEntity, TIface> EntityMapper { get; set; }
    public BusinessEntity()
    {
        EntityMapper = new BusinessEntityMapper<TEntity, TIface>();
    }

    public IReadOnlyList<TIface> GetItems()
    {
        var db = new DbConnection();
        return db.GetItems<TEntity>(ent => true);
    }

    public IReadOnlyList<TIface> GetItemsByFilter(Func<TIface, bool> filter)
    {
        var db = new DbConnection();
        return db.GetItems<TEntity>(filter);
    }

    public ReturnCode AddItems(IReadOnlyList<TIface> newEntities)
    {
        var db = new DbConnection();
        Collection<TEntity> entities = new Collection<TEntity>();

        foreach (var entity in newEntities)
        {
            entities.Add(EntityMapper.EntityMapper.Map<TEntity>(entity));
        }

        return db.AddItems(entities);
    }

    public ReturnCode DeleteItems(IList<int> ids)
    {
        var db = new DbConnection();
        var entitiesToDelete = db.GetItems<TEntity>(ent => ids.Contains(ent.Id));
        return db.DeleteItems(entitiesToDelete);
    }

    public ReturnCode UpdateItems(IReadOnlyList<TIface> updatedEntities)
    {
        DbConnection db = new DbConnection();

        IReadOnlyList<TEntity> entitiesToUpdate = db.GetItems<TEntity>(ent => true);

        foreach (var updEntity in updatedEntities)
        {
            var entityToUpd = entitiesToUpdate.Single(ent => ent.Id == updEntity.Id); 
            EntityMapper.EntityMapper.Map(updEntity, entityToUpd);
        }
        return db.UpdateItems(entitiesToUpdate);
    }

    public TIface? GetItemById(int id)
    {
        var db = new DbConnection();
        return db.GetItemById<TEntity>(id);
    }
}
