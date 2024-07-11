using DatabaseAccess;
using DbAccess.Models;

namespace DbAccessUnitTests
{
    public class DbAccessModel_UnitTests
    {

        private void AddChild(string dbName)
        {
            using var ctx = new DbAccess.Models.AlmanContext(dbName);
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
            ctx.Add(new Child
            {
                ChildName = "name",
                ChildLastName = "LastName",
                ChildStartMonth = 5,
                ChildStartYear = 2023,
                ChildGroup = 1,
                ChildContract = (int)AlmanDefinitions.ContractType.Precontract,
                ChildState = (int)AlmanDefinitions.ChildState.Active
            });
            ctx.SaveChanges();

        }


        [Fact]
        public void AddChild_OneChildWithSuchNameInDb()
        {
            // Arrange
            string testDb = "AddChild_OneChildWithSuchNameInDb.db";
            using var ctx = new DbAccess.Models.AlmanContext(testDb);
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
            ctx.Add(new Child { 
                ChildName = "name", 
                ChildLastName = "LastName", 
                ChildStartMonth = 5, 
                ChildStartYear = 2023, 
                ChildGroup = 1, 
                ChildContract = (int)AlmanDefinitions.ContractType.Precontract, 
                ChildState = (int)AlmanDefinitions.ChildState.Active });
            // Act
            ctx.SaveChanges();

            // Assert
            var child = ctx.Children.Where(x => x.ChildLastName == "LastName" && x.ChildName == "name").SingleOrDefault();
            Assert.NotNull(child);
        }


        [Fact]
        public void ChangeChildState_MustPass()
        {
            //Arrrange
            string testDb = "ChangeChildState_MustPass.db";
            using var ctx = new DbAccess.Models.AlmanContext(testDb);
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
            AddChild(testDb);
            //Act
            Child ch = ctx.Children.Where(ch => ch.ChildLastName == "LastName").Single();
            ch.ChildState = (int)AlmanDefinitions.ChildState.Inactive;
            ctx.SaveChanges();
            //Assert
            var child = ctx.Children.Where(ch => ch.ChildLastName == "LastName").Single();
            Assert.NotNull(child);
            Assert.Equal(child.ChildState, (int)AlmanDefinitions.ChildState.Inactive);

        }
    }
}