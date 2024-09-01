using Alman.SharedDefinitions;
using Alman.SharedModels;
using AutoMapper;
using DbAccess;
using DbAccess.Models;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Business;

/// <summary>
/// Provides the functionality of a mapper between interface <typeparamref name="TIface"/> and database model class <typeparamref name="TEntity"/>
/// </summary>
/// <typeparam name="TEntity">Type of the database model entity that must implement <typeparamref name="TIface"/></typeparam>
/// <typeparam name="TIface">Interface that every class that can be mapped to the <typeparamref name="TEntity"/> must implement</typeparam>
public interface IAlmanEntitiesMapper<TEntity, TIface>
    where TEntity : class, TIface
    where TIface : class
{
    /// <summary>
    /// Provides functionality to create instance of <typeparamref name="TEntity"/> from <typeparamref name="TIface"/> and back.
    /// </summary>
    public IMapper EntityMapper { get; set; }
}


/// <summary>
/// Class that represents the mapper between interface <typeparamref name="TIface"/> and database model class <typeparamref name="TEntity"/>
/// </summary>
/// <typeparam name="TEntity">Type of the database model entity that must implement <typeparamref name="TIface"/></typeparam>
/// <typeparam name="TIface">Interface that every class that can be mapped to the <typeparamref name="TEntity"/> must implement</typeparam>
public class BusinessEntityMapper<TEntity, TIface> : IAlmanEntitiesMapper<TEntity, TIface>
    where TEntity : class, TIface
    where TIface : class
{

    public IMapper EntityMapper { get; set; }

    /// <summary>
    /// ctor that creates the mapper.
    /// </summary>
    public BusinessEntityMapper()
    {
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<TIface, TEntity>());
        EntityMapper = config.CreateMapper();
    }
}

/// <summary>
/// Functionality to dup the database to the excel fil.
/// </summary>
public static class BusinessDbDump
{
    /// <summary>
    /// Start the dumping process.
    /// </summary>
    /// <returns>Task that must result into the filename where the </returns>
    public static async Task<string> DumpDb()
    {
        var db = new DbConnection();
        var res = await db.ExportToExcel();
        
        return res;
    }
}

/// <summary>
/// Business API for every database table (entity)
/// </summary>
/// <typeparam name="TEntity">Type of the database model entity that must implement <typeparamref name="TIface"/></typeparam>
/// <typeparam name="TIface">Interface that every class that can be mapped to the <typeparamref name="TEntity"/> must implement</typeparam>
public class BusinessEntity<TEntity, TIface>
    where TEntity : class, TIface, IIdentifier, IDeleteDependable, new()
    where TIface : class, IIdentifier
{
    /// <summary>
    /// Mapper between <typeparamref name="TIface"/> and <typeparamref name="TEntity"/>
    /// </summary>
    BusinessEntityMapper<TEntity, TIface> EntityMapper { get; set; }

    /// <summary>
    /// ctor.
    /// </summary>
    public BusinessEntity()
    {
        EntityMapper = new BusinessEntityMapper<TEntity, TIface>();
    }

    /// <summary>
    /// Get the whole table.
    /// </summary>
    /// <returns>Read-only list of rows. </returns>
    public IReadOnlyList<TIface> GetItems()
    {
        var db = new DbConnection();
        return db.GetItems<TEntity>(ent => true);
    }

    /// <summary>
    /// Get some rows from the table.
    /// </summary>
    /// <param name="filter"> Condition by which the rows are chosen.</param>
    /// <returns></returns>
    public IReadOnlyList<TIface> GetItemsByFilter(Func<TIface, bool> filter)
    {
        var db = new DbConnection();
        return db.GetItems<TEntity>(filter);
    }

    /// <summary>
    /// Aff new rows to the table.
    /// </summary>
    /// <param name="newEntities"> Read-only of new rows.</param>
    /// <returns>ReturnCode.OK if successful, error otherwise. </returns>
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

    /// <summary>
    /// Delete rows from the table.
    /// </summary>
    /// <param name="ids"> ids of the rows to delete. </param>
    /// <returns>ReturnCode.OK if successful, error otherwise. </returns>
    public ReturnCode DeleteItems(IList<int> ids)
    {
        var db = new DbConnection();
        var entitiesToDelete = db.GetItems<TEntity>(ent => ids.Contains(ent.Id));
        return db.DeleteItems(entitiesToDelete);
    }

    /// <summary>
    /// Update rows in the table.
    /// </summary>
    /// <param name="updatedEntities">Read-only list of the changed rows.</param>
    /// <returns>ReturnCode.OK if successful, error otherwise. </returns>
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

    /// <summary>
    /// Get row by roe id from the table.
    /// </summary>
    /// <param name="id">Id of the row.</param>
    /// <returns>Row or null if not found.</returns>
    public TIface? GetItemById(int id)
    {
        var db = new DbConnection();
        return db.GetItemById<TEntity>(id);
    }
}
