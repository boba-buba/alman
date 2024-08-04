using Alman.SharedDefinitions;
using Alman.SharedModels;
using DatabaseAccess;
using DbAccess.Models;
using System.Collections.ObjectModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business;

public static class BusinessPositionsApi
{
    public static IReadOnlyList<IPositionBase> GetPositions()
    {
        var db = new DbStaff();
        return db.GetPositions(pos => true);
    }

    public static ReturnCode AddPositions(IReadOnlyList<IPositionBase> newPositions)
    {
        var db = new DbStaff();
        Collection<Position> positions = new Collection<Position>();
        foreach (var newPosition in newPositions)
        {
            positions.Add(new Position
            {
                PositionName = newPosition.PositionName,
                PositionSalary = newPosition.PositionSalary,
            });
        }

        return db.AddPositions(positions);
    }

    public static ReturnCode UpdatePositions(IReadOnlyList<IPositionBase> updatedPositions)
    {
        var db = new DbStaff();
        var positionsToUpdate = db.GetPositions(pos => true);
        

        foreach (var position in positionsToUpdate)
        {
            var updatedPosition = updatedPositions.Single(pos => pos.PositionId == position.PositionId);
            position.PositionName = updatedPosition.PositionName;
            position.PositionSalary = updatedPosition.PositionSalary;
        }
        return db.UpdatePositions(positionsToUpdate);
    }

    public static ReturnCode DeletePositions(IList<int> positionsIds)
    {
        var db = new DbStaff();
        var positionsToDelete = db.GetPositions(pos => positionsIds.Contains(pos.PositionId));
        return db.DeletePositions(positionsToDelete);
    }


}
