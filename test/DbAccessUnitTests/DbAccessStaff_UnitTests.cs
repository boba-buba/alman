/*using DbAccess.Models;
using DatabaseAccess;
using Alman.SharedDefinitions;
//using Alman.Models;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace DbAccessUnitTests;

public partial class DbAccessModel_UnitTests
{

    #region Positions

    [Theory]
    [InlineData("AddPosition_ReadPosition_MustPass_1.db", "Position", 5000)]
    [InlineData("AddPosition_ReadPosition_MustPass_2.db", "Имя", 1)]
    public void AddPosition_ReadPosition_MustPass(string dbName, string positionName, int salary)
    {
        //Arrange
        var db = new DbStaff(dbName);
        db.DeleteDb(dbName);
        int expectedId = 1;
        var position = new Position { PositionName = positionName, PositionSalary = salary };
        //Act
        db.AddPositions([position]);

        var positionFromDb = db.GetPositions(ch => true).Single();
        //Assert
        Assert.Equal(expectedId, position.PositionId);
        Assert.Equal(positionName, positionFromDb.PositionName);
        Assert.Equal(salary, positionFromDb.PositionSalary);
    }

    [Theory]
    [InlineData("AddTwoPositions_ReadTwoPositions_MustPass_1.db", "Position", 5000, "Position", 5000)]
    [InlineData("AddTwoPositions_ReadTwoPositions_MustPass_2.db", "Имя", 1, "Имя", 1)]
    public void AddTwoPositions_ReadTwoPositions_MustPass(string dbName, string positionName, int salary, string positionName2, int salary2)
    {
        //Arrange
        var db = new DbStaff(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 2;
        //Act
        var positions = new List<Position> { new Position { PositionName = positionName, PositionSalary = salary}, new Position { PositionName = positionName2, PositionSalary = salary2} };
        db.AddPositions(positions);
        var positionsFromDb = db.GetPositions(ch => true);

        //Assert
        Assert.Equal(expectedCount, positionsFromDb.Count);
    }



    #endregion
}*/