using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Controls;

public static class PositionsControl
{
    private static BusinessEntity<Position, IPositionBase> BusinessLog { get; set; } = new BusinessEntity<Position, IPositionBase>();

    public static IReadOnlyList<IPositionBase> GetPositions() => 
        BusinessLog.GetEntities();
    

    public static ReturnCode AddPositions(IReadOnlyList<IPositionBase> positions) => 
        BusinessLog.AddEntities(positions);
    

    public static ReturnCode UpdatePositions(IReadOnlyList<IPositionBase> positions) => 
        BusinessLog.UpdateEntities(positions);
    

    public static ReturnCode DeletePositions(IList<int> positionsIds) => 
        BusinessLog.DeleteEntities(positionsIds);

    //TODO atomic somehow or like one transaction
    public static ReturnCode SavePositions(IReadOnlyList<IPositionBase> positionsToSave, IList<int> positionsIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (positionsIdsToDelete.Count > 0)
        {
            retCode = DeletePositions(positionsIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IPositionBase)}'s.");
                return retCode;
            }
        }

        var positionsFromDb = BusinessLog.GetEntities();
        int dbCount = positionsFromDb.Count;
        int difference = positionsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedPositions = positionsToSave.Where(pos => pos.InGroup(positionsFromDb)).ToList();

            retCode = BusinessLog.UpdateEntities(updatedPositions);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IPositionBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newPositions = positionsToSave.Where(pos => !pos.InGroup(positionsFromDb)).ToList();
            retCode = AddPositions(newPositions);
        }
        return retCode;
    }
}


public static class PositonBaseExtensions
{
    public static bool DbEquals(this IPositionBase item,  IPositionBase other)
    {
        if (item.Id == other.Id) { return true; }
        return false;
    }

    public static bool InGroup(this  IPositionBase item, IReadOnlyList<IPositionBase> group)
    {
        foreach (var groupItem in group)
        {
            if (item.DbEquals(groupItem))
                return true;
        }
        return false;
    }
}
