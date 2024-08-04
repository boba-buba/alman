using DbAccess.Models;
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

    #endregion
}