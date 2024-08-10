using DbAccess.Models;
using DatabaseAccess;
using Alman.SharedDefinitions;
using Alman.SharedModels;
//using Alman.Models;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace DbAccessUnitTests;

public partial class DbAccessModel_UnitTests
{
    #region OtherActivities
    [Theory]
    [InlineData("AddOtherActivity_ReadOtherActivity_MustPass_1.db", "Position")]
    [InlineData("AddOtherActivity_ReadOtherActivity_MustPass_2.db", "Имя")]
    public void AddOtherActivity_ReadOtherActivity_MustPass(string dbName, string positionName)
    {
        //Arrange
        var db = new DbConnection(dbName);
        db.DeleteDb(dbName);
        int expectedId = 1;
        var otherAct = new OtherActivity {OtherName = positionName };
        //Act
        db.AddItems([otherAct]);

        var otherActFromDb = db.GetItems<OtherActivity>(ch => true).Single();
        //Assert
        Assert.Equal(expectedId, otherActFromDb.Id);
        Assert.Equal(positionName, otherActFromDb.OtherName);
    }


    #endregion
}