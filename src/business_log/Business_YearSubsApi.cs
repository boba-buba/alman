using Alman.SharedDefinitions;
using Alman.SharedModels;
using AutoMapper;
using DatabaseAccess;
using DbAccess.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business;

public class EntityMapper
{
    public IMapper YearSubMapper { get; private set; }
    public EntityMapper() 
    {
        MapperConfiguration YearSubConfig = new MapperConfiguration(cfg => cfg.CreateMap<IYearSubBase, YearSub>());
        YearSubMapper = YearSubConfig.CreateMapper();
    }
    
}

public static class BusinessYearSubsApi
{
    public static IReadOnlyList<IYearSubBase> GetYearSubs()
    {
        var db = new DbChildren();
        return db.GetYearSubs(ys => true);
    }

    public static IReadOnlyList<IYearSubBase> GetYearSubsByFilter(Func<IYearSubBase, bool> filter)
    {
        var db = new DbChildren();
        return db.GetYearSubs(filter);
    }

    public static ReturnCode AddYearSubs(IReadOnlyList<IYearSubBase> newYearSubs)
    {
        var db = new DbChildren();
        Collection<YearSub> yearSubs = new Collection<YearSub>();
        var mapper = new EntityMapper();

        foreach (var yearSub in newYearSubs)
        {
            yearSubs.Add(mapper.YearSubMapper.Map<YearSub>(yearSub));
            /*yearSubs.Add(new YearSub
            {
                YchildId = yearSub.YchildId,
                Yyear = yearSub.Yyear,
                Yjanuary = yearSub.Yjanuary,
                Yfebruary = yearSub.Yfebruary,
                Ymarch = yearSub.Ymarch,
                Yapril = yearSub.Yapril,
                Ymay = yearSub.Ymay,
                Yjune = yearSub.Yjune,
                Yjuly = yearSub.Yjuly,
                Yaugust = yearSub.Yaugust,
                Yseptember = yearSub.Yseptember,
                Yoctober = yearSub.Yoctober,
                Ynovember = yearSub.Ynovember,
                Ydecember = yearSub.Ydecember
            });*/
        }
        return db.AddYearSubs(yearSubs);
    }
    
    public static ReturnCode UpdateYearSubs(IReadOnlyList<IYearSubBase> updatedYearSubs)
    {
        int year = updatedYearSubs[0].Yyear;
        var db = new DbChildren();
        var yearSubsFromDb = db.GetYearSubs(ys => ys.Yyear == year);
        var mapper = new EntityMapper();

        foreach (var updatedYearSub in  updatedYearSubs)
        {
            var yearSubFromDb = yearSubsFromDb.SingleOrDefault(ys => ys.YchildId == updatedYearSub.YchildId);
            if (yearSubFromDb is null) continue;
            mapper.YearSubMapper.Map(updatedYearSub, yearSubFromDb);
            
            /*yearSubFromDb.Yjanuary = updatedYearSub.Yjanuary;
            yearSubFromDb.Yfebruary = updatedYearSub.Yfebruary;
            yearSubFromDb.Ymarch = updatedYearSub.Ymarch;
            yearSubFromDb.Yapril = updatedYearSub.Yapril;
            yearSubFromDb.*/


        }
        return db.UpdateYearSubs(yearSubsFromDb);
    }
}
