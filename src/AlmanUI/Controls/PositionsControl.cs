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

public static class PositionsControl
{
    public static IReadOnlyList<IPositionBase> GetPositions() => 
        BusinessPositionsApi.GetPositions();
    

    public static ReturnCode AddPositions(IReadOnlyList<IPositionBase> positions) => 
        BusinessPositionsApi.AddPositions(positions);
    

    public static ReturnCode UpdatePositions(IReadOnlyList<IPositionBase> positions) => 
        BusinessPositionsApi.UpdatePositions(positions);
    

    public static ReturnCode DeletePositions(IList<int> positionsIds) => 
        BusinessPositionsApi.DeletePositions(positionsIds);

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

        var positionsFromDb = BusinessPositionsApi.GetPositions();
        int dbCount = positionsFromDb.Count;
        int difference = positionsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedPositions = positionsToSave.Where(pos => pos.InGroup(positionsFromDb)).ToList();

            retCode = BusinessPositionsApi.UpdatePositions(updatedPositions);
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
        if (item.PositionId == other.PositionId) { return true; }
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
