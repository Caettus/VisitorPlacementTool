using VPTLogic;

namespace VPTTest;

[TestClass]
public class RowTest
{
    [TestMethod]
    public void CreateSeatsTest()
    {
        // Arrange
        Row row = new Row(1, 'A');
        // Act
        row.CreateSeats(5);
        // Assert
        Assert.AreEqual(5, row.SeatsList.Count());
    }
    
    [TestMethod]
    public void PlaceVisitorsTest()
    {
        // Arrange
        Row row = new Row(1, 'A');
        Group group = new Group();
        {
            group.ChangeContainsChild(true);
            group.ChangeChildCount(1);
            group.ChangeAdultCount(2);
            group.ChangeContainsAdult(true);
        }
        group.AddGroupCountToVisitorsList(1, 2);
        // Act
        row.CreateSeats(3);
        row.PlaceVisitors(group);
        // Assert
        Assert.AreEqual(3, row.SeatsList.Count(x => x.Occupied));
    }
    
    [TestMethod]
    public void UnplaceVisitorsTest()
    {
        // Arrange
        Row row = new Row(1, 'A');
        Group group = new Group();
        {
            group.ChangeContainsChild(true);
            group.ChangeChildCount(1);
            group.ChangeAdultCount(2);
            group.ChangeContainsAdult(true);
        }
        group.AddGroupCountToVisitorsList(1, 2);
        // Act
        row.CreateSeats(3);
        row.PlaceVisitors(group);
        row.UnplaceVisitors(group);
        // Assert
        Assert.AreEqual(0, row.SeatsList.Count(x => x.Occupied));
    }
    
    [TestMethod]
    public void CheckIfFullTest()
    {
        // Arrange
        Row row = new Row(1, 'A');
        Group group = new Group();
        {
            group.ChangeContainsChild(true);
            group.ChangeChildCount(1);
            group.ChangeAdultCount(2);
            group.ChangeContainsAdult(true);
        }
        group.AddGroupCountToVisitorsList(1, 2);
        // Act
        row.CreateSeats(3);
        row.PlaceVisitors(group);
        // Assert
        Assert.IsTrue(row.CheckIfFull());
    }
}