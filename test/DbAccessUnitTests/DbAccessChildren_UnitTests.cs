using DbAccess.Models;
using DatabaseAccess;
using Alman.SharedDefinitions;
//using Alman.Models;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace DbAccessUnitTests;

public partial class DbAccessModel_UnitTests
{

    #region Children
    [Theory]
    [InlineData("AddChild_ReadChild_MustPass_1.db", "FirstName", "LastName", 1, ChildState.Active)]
    [InlineData("AddChild_ReadChild_MustPass_2.db", "Имя", "Фамилия", 1, ChildState.Active)]
    public void AddChild_ReadChild_MustPass(string dbName, string firstName, string lastName, int childGroup, ChildState childState)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedId = 1;
        //Act
        var child = new Child { ChildName = firstName, ChildLastName = lastName, ChildGroup = childGroup, ChildState = (int)childState };
        db.AddChildren([ child ]);
        
        var childFromDb = db.GetChildren(ch => true).Single();
        //Assert
        Assert.Equal(expectedId, child.ChildId);
        Assert.Equal(child.ChildName, childFromDb.ChildName);
        Assert.Equal(child.ChildLastName, childFromDb.ChildLastName);
        Assert.Equal(child.ChildGroup, childFromDb.ChildGroup);
        Assert.Equal(child.ChildState, childFromDb.ChildState);

    }

    [Theory]
    [InlineData("AddTwoChildren_ReadTwoChildren_MustPass_1.db", "FirstName", "LastName", "SecondFirstName", "SecondLastName")]
    [InlineData("AddTwoChildren_ReadTwoChildren_MustPass_2.db", "Имя", "Фамилия", "Имя2", "Фамилия2")]
    public void AddTwoChildren_ReadTwoChildren_MustPass(string dbName, string firstName, string lastName, string secondFirstName, string secondLastName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 2;
        //Act
        var children = new Child[] { new Child { ChildName = firstName, ChildLastName = lastName }, new Child { ChildName = secondFirstName, ChildLastName = secondLastName } };
        db.AddChildren(children);
        
        var childrenFromDb = db.GetChildren(ch => true);

        //Assert
        Assert.Equal(expectedCount, childrenFromDb.Count);
    }

    [Theory]
    [InlineData("AddTheSameChildTwice_ReadTwoChildrenWithDifferentIds_MustPass_1.db", "FirstName", "LastName")]
    [InlineData("AddTheSameChildTwice_ReadTwoChildrenWithDifferentIds_MustPass_2.db", "Имя", "Фамилия")]
    public void AddTheSameChildTwice_ReadTwoChildrenWithDifferentIds_MustPass(string dbName, string firstName, string lastName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 2;
        //Act
        var children = new Child[] { new Child { ChildName = firstName, ChildLastName = lastName }, new Child { ChildName = firstName, ChildLastName = lastName } };
        db.AddChildren(children);

        var childrenFromDb = db.GetChildren(ch => true);

        //Assert
        Assert.Equal(expectedCount, childrenFromDb.Count);
        Assert.Equal(1, childrenFromDb.First().ChildId);
        Assert.Equal(2, childrenFromDb.Last().ChildId);
    }


    [Theory]
    [InlineData("AddTwoChildren_ReadTwoChildren_MustPass_1.db", "FirstName", "LastName", "SecondFirstName", "SecondLastName")]
    [InlineData("AddTwoChildren_ReadTwoChildren_MustPass_2.db", "Имя", "Фамилия", "Имя2", "Фамилия2")]
    public void GetChildrenWithFilter_MustPAss(string dbName, string firstName, string lastName, string secondFirstName, string secondLastName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var children = new Child[] { new Child { ChildName = firstName, ChildLastName = lastName }, new Child { ChildName = secondFirstName, ChildLastName = secondLastName } };
        db.AddChildren(children);

        var childrenFromDb = db.GetChildren(ch => ch.ChildName == firstName);
        //Assert
        Assert.Equal(expectedCount, childrenFromDb.Count);
    }

    [Theory]
    [InlineData("ChangeContractTypeForChild_MustPass_1.db", ContractType.MotherCapital, ContractType.OrdinaryContract)]
    public void ChangeContractTypeForChild_MustPass(string dbName, ContractType oldContract, ContractType newContract)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);

        //Act
        var child = new Child { ChildName = "name", ChildLastName = "lastname" ,ChildContract = (int)oldContract };
        db.AddChildren([child]);

        var childFromDb = db.GetChildren(ch => true).Single();
        //Assert
        Assert.Equal((int)oldContract, childFromDb.ChildContract);

        //Act update
        childFromDb.ChildContract = (int)newContract;
        var ret_code = db.UpdateChildren([childFromDb]);

        //Assert update
        Assert.Equal(ReturnCode.OK, ret_code);
        Assert.Equal((int)newContract, db.GetChildById(1).ChildContract);

    }


    [Theory]
    [InlineData("DeleteChildren_MustPass_1.db", "FirstName", "LastName", "SecondFirstName", "SecondLastName")]
    [InlineData("DeleteChildren_MustPass_2.db", "Имя", "Фамилия", "Имя2", "Фамилия2")]
    public void DeleteChildren_MustPass(string dbName, string firstName, string lastName, string secondFirstName, string secondLastName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 0;

        //Act
        var children = new Child[] { new Child { ChildName = firstName, ChildLastName = lastName }, new Child { ChildName = secondFirstName, ChildLastName = secondLastName } };
        db.AddChildren(children);

        db.DeleteChildren(children);

        //Assert
        Assert.Equal(expectedCount, db.GetChildren(ch => true).Count);
    }

    [Theory]
    [InlineData("DeleteChildrenWIthDependencies_MustPass_1.db", "Name", "surname", 2, 2024, 3000)]
    [InlineData("DeleteChildrenWIthDependencies_MustPass_2.db", "Имя", "Фамилия", 2, 2024, 3000)]
    public void DeleteChildrenWIthDependencies_MustPass(string dbName, string firstName, string lastName, int month, int year, int sum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedAfterAdding = 1;
        int expectedAfterDeleteing = 0;

        //Act
        var child = new Child { ChildName = firstName, ChildLastName = lastName };
        child.ContractFees.Add(new ContractFee { Cfmonth = month, Cfyear = year, CfsumPaid = sum });

        db.AddChildren([child]);

        //Assert
        var childrenFromDb = db.GetChildren(ch => true);
        var contractFees = db.GetContractFees(cf => true);
        Assert.Equal(expectedAfterAdding, childrenFromDb.Count);
        Assert.Equal(expectedAfterAdding, contractFees.Count);

        //Act delete
        var children = db.GetChildren(ch => true);
        db.DeleteChildren(children);

        //Assert
        Assert.Equal(expectedAfterDeleteing, db.GetChildren(ch=>true).Count);
        Assert.Equal(expectedAfterDeleteing, db.GetContractFees(cf => true).Count);

    }
    #endregion


    #region Activities

    [Theory]
    [InlineData("AddActvity_ReadActvity_MustPass_1.db", "Name", 800)]
    [InlineData("AddActvity_ReadActvity_MustPass_2.db", "Имя", 900)]
    public void AddActvity_ReadActvity_MustPass(string dbName, string name, int price)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedId = 1;
        int expectedCount = 1;
        //Act
        var activity = new Activity { ActivityName = name, ActivityPrice = price };
        db.AddActvities([activity]);

        var actvitiesFromDb = db.GetActivities(act => true);
        //Assert
        Assert.Equal(expectedCount, actvitiesFromDb.Count);
        Assert.Equal(expectedId, actvitiesFromDb[0].ActivityId);
        Assert.Equal(name, actvitiesFromDb.First().ActivityName);
        Assert.Equal(price, actvitiesFromDb.First().ActivityPrice);
    }


    [Theory]
    [InlineData("AddTwoActvities_ReadTwoActivities_MustPass_1.db", "Name", "SecondName")]
    [InlineData("AddTwoActvities_ReadTwoActivities_MustPass_2.db", "Имя", "Имя2")]
    public void AddTwoActvities_ReadTwoActivities_MustPass(string dbName, string firstName, string secondName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 2;
        //Act
        var activities = new Activity[] { new Activity { ActivityName = firstName }, new Activity { ActivityName = secondName } };
        db.AddActvities(activities);

        var activitiesFromDb = db.GetActivities(act => true);

        //Assert
        Assert.Equal(expectedCount, activitiesFromDb.Count);
    }


    [Theory]
    [InlineData("GetActivitiesWithFilter_MustPass_1.db", "FirstName", "SecondFirstName")]
    [InlineData("GetActivitiesWithFilter_MustPass_2.db", "Имя", "Имя2")]
    public void GetActivitiesWithFilter_MustPass(string dbName, string firstName, string secondName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var activities = new Activity[] { new Activity {ActivityName = firstName}, new Activity {ActivityName = secondName} };
        db.AddActvities(activities);

        var activitiesFromDb = db.GetActivities(act => act.ActivityName == firstName);
        //Assert
        Assert.Equal(expectedCount, activitiesFromDb.Count);
    }


    [Theory]
    [InlineData("ChangeActvityPrice_MustPass_1.db", 700, 800)]
    public void ChangeActvityPrice_MustPass(string dbName, int oldPrice, int newPrice)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedId = 1;

        //Act
        var actvity = new Activity { ActivityName = "name", ActivityPrice = oldPrice };
        db.AddActvities([actvity]);

        var activityFromDb = db.GetActivities(act => true).Single();
        
        //Assert
        Assert.Equal(oldPrice, activityFromDb.ActivityPrice);

        //Act update
        //var activityFromDbToChange = db.GetActivities(act => true).First();
        activityFromDb.ActivityPrice = newPrice;
        db.UpdateActvities([activityFromDb]);

        //Assert update
        Assert.Equal(newPrice, db.GetActivities(act => act.ActivityId == expectedId).Single().ActivityPrice);

    }


    [Theory]
    [InlineData("DeleteChildren_MustPass_1.db", "FirstName", "SecondFirstName")]
    [InlineData("DeleteChildren_MustPass_2.db", "Имя", "Имя2")]
    public void DeleteActvities_MustPass(string dbName, string firstName, string secondFirstName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 0;

        //Act
        var activities = new Activity[] { new Activity { ActivityName = firstName }, new Activity { ActivityName = secondFirstName } };
        db.AddActvities(activities);

        var activitiesFromDb = db.GetActivities(act => true);
        db.DeleteActvities(activities);

        //Assert
        Assert.Equal(expectedCount, db.GetActivities(act => true).Count);
    }


    [Theory]
    [InlineData("DeleteActvitiesWIthDependencies_MustPass_1.db", "Name", "surname", "Activity", 4, 2024)]
    [InlineData("DeleteActvitiesWIthDependencies_MustPass_2.db", "Имя", "Фамилия", "занятие", 4, 2024)]
    public void DeleteActvitiesWIthDependencies_MustPass(string dbName, string firstName, string lastName, string actName, int month, int year)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedAfterAdding = 1;
        int expectedAfterDeleteing = 0;

        //Act
        var activity = new Activity { ActivityName = actName, ActivityPrice = 700 };
        db.AddActvities([activity]);
        //Assert
        var activities = db.GetActivities(act => true);
        Assert.Equal(expectedAfterAdding, activities.Count);

        var child = new Child { ChildName = firstName, ChildLastName = lastName, ChildContract = (int)ContractType.OrdinaryContract };
        child.YearMonthActivities.Add(new YearMonthActivity { YmactivityId = 1,  Month = month, Year = year });

        db.AddChildren([child]);

        //Assert
        var childrenFromDb = db.GetChildren(ch => true);
        Assert.Equal(expectedAfterAdding, childrenFromDb.Count);

        var yearMonthActivities = db.GetYearMonthActivities(act => true);
        
        Assert.Equal(expectedAfterAdding, yearMonthActivities.Count);


        //Act delete
        var actvitiesToDel = db.GetActivities(act => true);
        db.DeleteActvities(actvitiesToDel);

        //Assert
        Assert.Equal(expectedAfterDeleteing, db.GetActivities(act => true).Count);
        Assert.Equal(expectedAfterDeleteing, db.GetYearMonthActivities(act => true).Count);

    }
    #endregion

    #region PreContracts
    [Theory]
    [InlineData("AddPrecontract_ReadPrecontract_MustPass_1.db", 19)]
    [InlineData("AddPrecontract_ReadPrecontract_MustPass_2.db", 1000)]
    public void AddPrecontract_ReadPrecontract_MustPass(string dbName, int sum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        
        var precontract = new Precontract { PchildId = 1, Psum  = sum };
        db.AddChildren([child]);
        db.AddPrecontracts([precontract]);

        var precontractsFromDb = db.GetPrecontracts(pr => true);
        //Assert
        Assert.Equal(expectedCount, precontractsFromDb.Count);
        Assert.Equal(precontract.PchildId, precontractsFromDb.First().PchildId);
    }


    [Theory]
    [InlineData("AddTwoPrecontracts_ReadTwoPrecontracts_MustPass_1.db", 19, 200)]
    [InlineData("AddTwoPrecontracts_ReadTwoPrecontracts_MustPass_2.db", 1000, 600)]
    public void AddTwoPrecontracts_ReadTwoPrecontracts_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 2;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var precontract = new Precontract { PchildId = 1, Psum = firstSum };
        var secondPrecontract = new Precontract { PchildId = 2, Psum = secondSum };

        db.AddChildren([child, child2]);
        db.AddPrecontracts([precontract, secondPrecontract]);

        var precontractsFromDb = db.GetPrecontracts(pr => true);
        //Assert
        Assert.Equal(expectedCount, precontractsFromDb.Count);
        Assert.Equal(precontract.PchildId, precontractsFromDb.First().PchildId);
        Assert.Equal(secondPrecontract.PchildId, precontractsFromDb.Last().PchildId);
    }


    [Theory]
    [InlineData("GetPrecontractsWithFilter_MustPass_1.db", 500, 700, 500)]
    [InlineData("GetPrecontractsWithFilter_MustPass_2.db", 200, 800, 600)]
    public void GetPrecontractsWithFilter_MustPass(string dbName, int firstSum, int secondSum, int condition)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var precontract = new Precontract { PchildId = 1, Psum = firstSum };
        var secondPrecontract = new Precontract { PchildId = 2, Psum = secondSum };

        db.AddChildren([child, child2]);
        db.AddPrecontracts([precontract, secondPrecontract]);

        var precontractsFromDb = db.GetPrecontracts(pr => pr.Psum > condition);
        //Assert
        Assert.Equal(expectedCount, precontractsFromDb.Count);
        Assert.Equal(secondSum, precontractsFromDb.First().Psum);
    }

    // originally it was PSum that i wanted to change, but parts of primary key cannot be changed unless you delete the constraint
    [Theory]
    [InlineData("ChangePrecontractSum_MustPass_1.db", "Comment1", "Comment2")]
    [InlineData("ChangePrecontractSum_MustPass_2.db", "Коментарий1", "Коментарий2")]
    public void ChangePrecontractComment_MustPass(string dbName, string firstComment, string secondComment)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        //Act
        var child = new Child { ChildLastName = "second", ChildName = "First" };
        child.Precontracts.Add(new Precontract { Psum = 900, Pcomment = firstComment });

        db.AddChildren([child]);
        //Assert
        Assert.Equal(1, db.GetChildren(ch => true).Count);
        Assert.Equal(1, db.GetPrecontracts(pr => true).Count);

        //Act update
        var precontract = db.GetPrecontracts(pr => pr.PchildId == 1).Single();
        precontract.Pcomment = secondComment;
        var ret_code = db.UpdatePrecontracts([precontract]);

        //Aassert
        Assert.Equal(ReturnCode.OK, ret_code);
        Assert.Equal(secondComment, db.GetPrecontracts(pr => true).Single().Pcomment);
    }

    [Theory]
    [InlineData("DeletePrecontracts_MustPass_1.db", 19, 200)]
    [InlineData("DeletePrecontracts_MustPass_2.db", 1000, 600)]
    public void DeletePrecontracts_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 0;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var precontract = new Precontract { PchildId = 1, Psum = firstSum };
        var secondPrecontract = new Precontract { PchildId = 2, Psum = secondSum };

        db.AddChildren([child, child2]);
        db.AddPrecontracts([precontract, secondPrecontract]);

        var precontracts = db.GetPrecontracts(pr => true);
        db.DeletePrecontracts(precontracts);

        //Assert
        Assert.Equal(expectedCount, db.GetPrecontracts(pr => true).Count);
    }

    [Theory]
    [InlineData("DeletePrecontracts_WithFilter_MustPass_1.db", 19, 200)]
    [InlineData("DeletePrecontracts_WithFilter_MustPass_2.db", 600, 1000)]
    public void DeletePrecontracts_WithFilter_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var precontract = new Precontract { PchildId = 1, Psum = firstSum };
        var secondPrecontract = new Precontract { PchildId = 2, Psum = secondSum };

        db.AddChildren([child, child2]);
        db.AddPrecontracts([precontract, secondPrecontract]);

        var precontracts = db.GetPrecontracts(pr => pr.Psum > firstSum);
        db.DeletePrecontracts(precontracts);

        //Assert
        Assert.Equal(expectedCount, db.GetPrecontracts(pr => true).Count);
    }

    #endregion

    #region ContractFees
    [Theory]
    [InlineData("AddContractFee_ReadContractFee_MustPass_1.db", -19)]
    [InlineData("AddContractFee_ReadContractFee_MustPass_2.db", 1000)]
    public void AddContractFee_REadContractFee_MustPass(string dbName, int sum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };

        var contractFee = new ContractFee { CfchildId = 1, CfsumPaid = sum, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year};
        db.AddChildren([child]);
        db.AddContractFees([contractFee]);

        var contractsFromDb = db.GetContractFees(pr => true);
        //Assert
        Assert.Equal(expectedCount, contractsFromDb.Count);
        Assert.Equal(contractFee.CfchildId, contractsFromDb[0].CfchildId);
        Assert.Equal(contractFee.CfsumPaid, contractsFromDb[0].CfsumPaid);
    }


    [Theory]
    [InlineData("AddTwoContractFees_ReadTwoContractFees_MustPass_1.db", 19, 200)]
    [InlineData("AddTwoContractFees_ReadTwoContractFees_MustPass_2.db", 1000, 600)]
    public void AddTwoContractFees_ReadTwoContractFees_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 2;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var contractFee = new ContractFee { CfchildId = 1, CfsumPaid = firstSum, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year };
        var secondContractFee = new ContractFee { CfchildId = 2, CfsumPaid = secondSum, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year };

        db.AddChildren([child, child2]);
        db.AddContractFees([contractFee, secondContractFee]);

        var contractsFromDb = db.GetContractFees(pr => true);
        //Assert
        Assert.Equal(expectedCount, contractsFromDb.Count);
        Assert.Equal(contractFee.CfchildId, contractsFromDb[0].CfchildId);
        Assert.Equal(secondContractFee.CfchildId, contractsFromDb[1].CfchildId);
    }

    [Theory]
    [InlineData("GetContractFeesWithFilter_MustPass_1.db", 500, 700, 500)]
    [InlineData("GetContractFeesWithFilter_MustPass_2.db", 200, 800, 600)]
    public void GetContractFeesWithFilter_MustPass(string dbName, int firstSum, int secondSum, int condition)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var contractFee = new ContractFee { CfchildId = 1, CfsumPaid = firstSum, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year };
        var secondContractFee = new ContractFee { CfchildId = 2, CfsumPaid = secondSum, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year };

        db.AddChildren([child, child2]);
        db.AddContractFees([contractFee, secondContractFee]);

        var contractsFromDb = db.GetContractFees(pr => pr.CfsumPaid > condition);
        //Assert
        Assert.Equal(expectedCount, contractsFromDb.Count);
        Assert.Equal(secondSum, contractsFromDb[0].CfsumPaid);
    }


    [Theory]
    [InlineData("ChangeContractFeeSum_MustPass_1.db", 400, 700)]
    [InlineData("ChangeContractFeeSum_MustPass_2.db", 500, 600)]
    public void ChangeContractFeeSum_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        //Act
        var child = new Child { ChildLastName = "second", ChildName = "First" };
        child.ContractFees.Add(new ContractFee { CfsumPaid = firstSum, Cfyear = DateTime.Now.Year, Cfmonth = DateTime.Now.Month });
            
        db.AddChildren([child]);
        //Assert
        Assert.Equal(1, db.GetChildren(ch => true).Count);
        Assert.Equal(1, db.GetContractFees(pr => true).Count);

        //Act update
        var contractFee = db.GetContractFees(pr => pr.CfchildId == 1).Single();
        contractFee.CfsumPaid = secondSum;
        var ret_code = db.UpdateContractFees([contractFee]);

        //Aassert
        Assert.Equal(ReturnCode.OK, ret_code);
        Assert.Equal(secondSum, db.GetContractFees(pr => true).Single().CfsumPaid);
    }

    [Theory]
    [InlineData("DeleteContractFees_MustPass_1.db", 19, 200)]
    [InlineData("DeleteContractFees_MustPass_2.db", 1000, 600)]
    public void DeleteContractFees_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 0;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var contractFee = new ContractFee { CfchildId = 1, CfsumPaid = firstSum, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year };
        var secondContractFee = new ContractFee { CfchildId = 2, CfsumPaid = secondSum, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year };

        db.AddChildren([child, child2]);
        db.AddContractFees([contractFee, secondContractFee]);

        var contractFees = db.GetContractFees(pr => true);
        db.DeleteContractFees(contractFees);

        //Assert
        Assert.Equal(expectedCount, db.GetContractFees(pr => true).Count);
    }


    [Theory]
    [InlineData("DeleteContractFees_WithFilter_MustPass_1.db", 19, 200)]
    [InlineData("DeleteContractFees_WithFilter_MustPass_2.db", 600, 1000)]
    public void DeleteContractFees_WithFilter_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var contractFee = new ContractFee { CfchildId = 1, CfsumPaid = firstSum, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year };
        var secondContractFee = new ContractFee { CfchildId = 2, CfsumPaid = secondSum, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year };

        db.AddChildren([child, child2]);
        db.AddContractFees([contractFee, secondContractFee]);

        var contarctFeesFromDb = db.GetContractFees(pr => pr.CfsumPaid > firstSum);
        db.DeleteContractFees(contarctFeesFromDb);

        //Assert
        Assert.Equal(expectedCount, db.GetContractFees(pr => true).Count);
    }

    [Theory]
    [InlineData("DeleteContractFees_WithFilter_MustPass_1.db")]
    public void AddContract_GetContractById_MustPass(string dbName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };

        var contractFee = new ContractFee { CfchildId = 1, CfsumPaid = 800, Cfmonth = DateTime.Now.Month, Cfyear = DateTime.Now.Year };
        db.AddChildren([child]);
        db.AddContractFees([contractFee]);

        var contractFeeFromDb = db.GetContractFeeById(DateTime.Now.Year, DateTime.Now.Month, 1);
        //Assert 
        Assert.Equal(contractFee.CfsumPaid, contractFeeFromDb.CfsumPaid);
    }

    #endregion

    #region YearMonthActvities
    [Theory]
    [InlineData("AddYearMonthAct_ReadYearMonthAct_MustPass_1.db", 19)]
    [InlineData("AddYearMonthAct_ReadYearMonthAct_MustPass_2.db", 1000)]
    public void AddYearMonthAct_ReadYearMonthAct_MustPass(string dbName, int sum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var activity = new Activity { ActivityName = "Doctor", ActivityPrice = sum };
        var yearMonthActivity = new YearMonthActivity { YmactivityId = 1, YmchildId = 1, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = sum };

        db.AddChildren([child]);
        db.AddActvities([activity]);
        db.AddYearMonthActivities([yearMonthActivity]);

        var yearMonthACtivitiesFromDb = db.GetYearMonthActivities(act => true);
        //Assert

        Assert.Equal(expectedCount, yearMonthACtivitiesFromDb.Count);
        Assert.Equal(yearMonthActivity.YmchildId, yearMonthACtivitiesFromDb[0].YmchildId);
        Assert.Equal(yearMonthActivity.YmactivitySum, yearMonthACtivitiesFromDb[0].YmactivitySum);
    }


    [Theory]
    [InlineData("AddTwoYearMonthActs_ReadTwoYearMonthActs_MustPass_1.db", 19, 200)]
    [InlineData("AddTwoYearMonthActs_ReadTwoYearMonthActs_MustPass_2.db", 1000, 600)]
    public void AddTwoYearMonthActs_ReadTwoYearMonthActs_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 2;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var activity = new Activity { ActivityName = "Doctor", ActivityPrice = firstSum };
        var yearMonthActivity = new YearMonthActivity { YmactivityId = 1, YmchildId = 1, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = firstSum };

        var activity2 = new Activity { ActivityName = "Doctor2", ActivityPrice = secondSum };
        var yearMonthActivity2 = new YearMonthActivity { YmactivityId = 2, YmchildId = 2, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = secondSum };

        db.AddChildren([child, child2]);
        db.AddActvities([activity, activity2]);
        db.AddYearMonthActivities([yearMonthActivity, yearMonthActivity2]);


        var ymaFromDb = db.GetYearMonthActivities(pr => true);
        //Assert
        Assert.Equal(expectedCount, ymaFromDb.Count);
        Assert.Equal(yearMonthActivity.YmchildId, ymaFromDb[0].YmchildId);
        Assert.Equal(yearMonthActivity2.YmchildId, ymaFromDb[1].YmchildId);
        Assert.Equal(yearMonthActivity.YmactivityId, ymaFromDb[0].YmactivityId);
        Assert.Equal(yearMonthActivity2.YmactivityId, ymaFromDb[1].YmactivityId);
    }

    [Theory]
    [InlineData("GetYearMonthActsWithFilter_MustPass_1.db", 500, 700, 500)]
    [InlineData("GetYearMonthActsWithFilter_MustPass_2.db", 200, 800, 600)]
    public void GetYearMonthActsWithFilter_MustPass(string dbName, int firstSum, int secondSum, int condition)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var activity = new Activity { ActivityName = "Doctor", ActivityPrice = firstSum };

        var yearMonthActivity = new YearMonthActivity { YmactivityId = 1, YmchildId = 1, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = firstSum };
        var yearMonthActivity2 = new YearMonthActivity { YmactivityId = 1, YmchildId = 2, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = secondSum };

        db.AddChildren([child, child2]);
        db.AddActvities([activity]);
        db.AddYearMonthActivities([yearMonthActivity, yearMonthActivity2]);

        var ymActsFromDb = db.GetYearMonthActivities(pr => pr.YmactivitySum > condition);
        //Assert
        Assert.Equal(expectedCount, ymActsFromDb.Count);
        Assert.Equal(secondSum, ymActsFromDb[0].YmactivitySum);
    }

    [Theory]
    [InlineData("ChangeYearMonthActivitySum_MustPass_1.db", 400, 700)]
    [InlineData("ChangeYearMonthActivitySum_MustPass_2.db", 500, 600)]
    public void ChangeYearMonthActivitySum_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        var activity = new Activity { ActivityName = "Doctor", ActivityPrice = firstSum };
        db.AddActvities([activity]);
        //Act

        var child = new Child { ChildLastName = "second", ChildName = "First" };
        child.YearMonthActivities.Add(new YearMonthActivity { Year = DateTime.Now.Year, Month = DateTime.Now.Month, YmactivityId = 1, YmwasPaid = firstSum });

        db.AddChildren([child]);
        //Assert
        Assert.Equal(1, db.GetChildren(ch => true).Count);
        Assert.Equal(1, db.GetYearMonthActivities(pr => true).Count);

        //Act update
        var ymAct = db.GetYearMonthActivities(pr => pr.YmchildId == 1).Single();
        ymAct.YmactivitySum = secondSum;
        var ret_code = db.UpdateYearMonthActivities([ymAct]);

        //Aassert
        Assert.Equal(ReturnCode.OK, ret_code);
        Assert.Equal(secondSum, db.GetYearMonthActivities(pr => true).Single().YmactivitySum);
    }

    [Theory]
    [InlineData("DeleteYearMonthActivities_MustPass_1.db", 19, 200)]
    [InlineData("DeleteYearMonthActivities_MustPass_2.db", 1000, 600)]
    public void DeleteYearMonthActivities_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 0;
        var activity = new Activity { ActivityName = "Doctor", ActivityPrice = firstSum };
        db.AddActvities([activity]);
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var yearMonthActivity = new YearMonthActivity { YmactivityId = 1, YmchildId = 1, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = firstSum };
        var yearMonthActivity2 = new YearMonthActivity { YmactivityId = 1, YmchildId = 2, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = secondSum };

        db.AddChildren([child, child2]);
        db.AddYearMonthActivities([yearMonthActivity, yearMonthActivity2]);

        var ymActsFromDb = db.GetYearMonthActivities(pr => true);
        db.DeleteYearMonthActivities(ymActsFromDb);

        //Assert
        Assert.Equal(expectedCount, db.GetYearMonthActivities(pr => true).Count);
    }


    [Theory]
    [InlineData("DeleteYearMonthActivities_WithFilter_MustPass_1.db", 19, 200)]
    [InlineData("DeleteYearMonthActivities_WithFilter_MustPass_2.db", 600, 1000)]
    public void DeleteYearMonthActivities_WithFilter_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        var activity = new Activity { ActivityName = "Doctor", ActivityPrice = firstSum };
        db.AddActvities([activity]);

        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var yearMonthActivity = new YearMonthActivity { YmactivityId = 1, YmchildId = 1, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = firstSum };
        var yearMonthActivity2 = new YearMonthActivity { YmactivityId = 1, YmchildId = 2, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = secondSum };

        db.AddChildren([child, child2]);
        db.AddYearMonthActivities([yearMonthActivity, yearMonthActivity2]);

        var ymActsFromDb = db.GetYearMonthActivities(pr => pr.YmactivitySum > firstSum);
        db.DeleteYearMonthActivities(ymActsFromDb);

        //Assert
        Assert.Equal(expectedCount, db.GetYearMonthActivities(pr => true).Count);
    }

    [Theory]
    [InlineData("DeleteContractFees_WithFilter_MustPass_1.db")]
    public void AddYearMonthACtivity_GetYearMonthActivityById_MustPass(string dbName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectdCount = 1;
        var activity = new Activity { ActivityName = "Doctor", ActivityPrice = 700 };
        db.AddActvities([activity]);
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };

        var yearMonthActivity = new YearMonthActivity { YmactivityId = 1, YmchildId = 1, Month = DateTime.Now.Month, Year = DateTime.Now.Year, YmactivitySum = 800 };
        db.AddChildren([child]);
        db.AddYearMonthActivities([yearMonthActivity]);

        var ymActsFromDb = db.GetYearMonthActivitiesById(DateTime.Now.Year, DateTime.Now.Month, 1);
        //Assert 
        Assert.Equal(expectdCount, ymActsFromDb.Count);
        Assert.Equal(yearMonthActivity.YmactivitySum, ymActsFromDb[0].YmactivitySum);
    }

    #endregion

    #region YearSubs
    [Theory]
    [InlineData("AddYearSub_ReadYearSub_MustPass_1.db")]
    public void AddYearSub_ReadYearSub_MustPass(string dbName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var yearSub = new YearSub { YchildId = 1 , Yyear = DateTime.Now.Year };

        db.AddChildren([child]);
        db.AddYearSubs([yearSub]);

        var yearSubsFromDb = db.GetYearSubs(act => true);
        //Assert

        Assert.Equal(expectedCount, yearSubsFromDb.Count);
        Assert.Equal(yearSub.YchildId, yearSubsFromDb[0].YchildId);
        Assert.Equal(yearSub.Yyear, yearSubsFromDb[0].Yyear);
    }

    [Theory]
    [InlineData("AddTwoYearSubs_ReadTwoYearSubs_MustPass_1.db", 2024, 2023)]
    [InlineData("AddTwoYearSubs_ReadTwoYearSubs_MustPass_2.db", 2025, 2000)]
    public void AddTwoYearSubs_ReadTwoYearSubs_MustPass(string dbName, int firstYear, int secondYear)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 2;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var yearSub = new YearSub { YchildId = 1, Yyear = firstYear };
        var yearSub2 = new YearSub { YchildId = 2, Yyear = secondYear };
      
        db.AddChildren([child, child2]);
        db.AddYearSubs([yearSub, yearSub2]);


        var ymaFromDb = db.GetYearSubs(pr => true);
        //Assert
        Assert.Equal(expectedCount, ymaFromDb.Count);
        Assert.Equal(yearSub.YchildId, ymaFromDb[0].YchildId);
        Assert.Equal(yearSub2.YchildId, ymaFromDb[1].YchildId);
        Assert.Equal(yearSub.Yyear, ymaFromDb[0].Yyear);
        Assert.Equal(yearSub2.Yyear, ymaFromDb[1].Yyear);
    }


    [Theory]
    [InlineData("GetYearSubsWithFilter_MustPass_1.db", 2019, 2028, 2020)]
    [InlineData("GetYearSubsWithFilter_MustPass_2.db", 2002, 2010, 2007)]
    public void GetYearSubsWithFilter_MustPass(string dbName, int firstYear, int secondYear, int condition)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var yearSub = new YearSub { YchildId = 1, Yyear = firstYear };
        var yearSub2 = new YearSub { YchildId = 2, Yyear = secondYear };

        db.AddChildren([child, child2]);
        db.AddYearSubs([yearSub, yearSub2]);


        var ymActsFromDb = db.GetYearSubs(pr => pr.Yyear > condition);
        //Assert
        Assert.Equal(expectedCount, ymActsFromDb.Count);
        Assert.Equal(secondYear, ymActsFromDb[0].Yyear);
    }


    [Theory]
    [InlineData("ChangeYearSub_MustPass_1.db", 400, 700)]
    [InlineData("ChangeYearSub_MustPass_2.db", 500, 600)]
    public void ChangeYearSub_MustPass(string dbName, int firstSum, int secondSum)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
      
        //Act

        var child = new Child { ChildLastName = "second", ChildName = "First" };
        child.YearSubs.Add(new YearSub { Yyear = DateTime.Now.Year, Yapril = firstSum, YaprilPayment = 1 });

        db.AddChildren([child]);
        //Assert
        Assert.Equal(1, db.GetChildren(ch => true).Count);
        Assert.Equal(1, db.GetYearSubs(pr => true).Count);

        //Act update
        var ymAct = db.GetYearSubs(pr => pr.YchildId == 1).Single();
        ymAct.Yapril = secondSum;
        var ret_code = db.UpdateYearSubs([ymAct]);

        //Aassert
        Assert.Equal(ReturnCode.OK, ret_code);
        Assert.Equal(secondSum, db.GetYearSubs(pr => true).Single().Yapril);
    }


    [Theory]
    [InlineData("DeleteYearSubs_MustPass_1.db", 19, 200)]
    [InlineData("DeleteYearSubs_MustPass_2.db", 1000, 600)]
    public void DeleteYearSubs_MustPass(string dbName, int firstYear, int secondYear)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 0;
        
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var yearSub = new YearSub { YchildId = 1, Yyear = firstYear };
        var yearSub2 = new YearSub { YchildId = 2, Yyear = secondYear };


        db.AddChildren([child, child2]);
        db.AddYearSubs([yearSub, yearSub2]);


        var ymActsFromDb = db.GetYearSubs(pr => true);
        db.DeleteYearSubs(ymActsFromDb);

        //Assert
        Assert.Equal(expectedCount, db.GetYearSubs(pr => true).Count);
    }


    [Theory]
    [InlineData("DeleteYearSubs_WithFilter_MustPass_1.db", 2003, 2006)]
    [InlineData("DeleteYearSubs_WithFilter_MustPass_2.db", 2006, 2023)]
    public void DeleteYearSubs_WithFilter_MustPass(string dbName, int firstYear, int secondYear)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
        int expectedCount = 1;
       
        
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };
        var child2 = new Child { ChildLastName = "name", ChildName = "second" };

        var yearSub = new YearSub { YchildId = 1, Yyear = firstYear };
        var yearSub2 = new YearSub { YchildId = 2, Yyear = secondYear };

        
        db.AddChildren([child, child2]);
        db.AddYearSubs([yearSub, yearSub2]);

        var ymActsFromDb = db.GetYearSubs(pr => pr.Yyear > firstYear);
        db.DeleteYearSubs(ymActsFromDb);

        //Assert
        Assert.Equal(expectedCount, db.GetYearSubs(pr => true).Count);
        Assert.Equal(firstYear, db.GetYearSubs(pr => true)[0].Yyear);
    }


    [Theory]
    [InlineData("DeleteContractFees_WithFilter_MustPass_1.db")]
    public void AddYearSub_GetYearSubById_MustPass(string dbName)
    {
        //Arrange
        var db = new DbChildren(dbName);
        db.DeleteDb(dbName);
       
        //Act
        var child = new Child { ChildLastName = "name", ChildName = "first" };

        var yearSub = new YearSub { YchildId = 1, Yyear = 2024 };
        var yearSub2 = new YearSub { YchildId = 1, Yyear = 2023 };

        db.AddChildren([child]);
        db.AddYearSubs([yearSub, yearSub2]);

        var ymActsFromDb = db.GetChildYearSubById(2024, 1);
        //Assert 
        Assert.Equal(yearSub.Yyear, ymActsFromDb.Yyear);
    }

    #endregion


}