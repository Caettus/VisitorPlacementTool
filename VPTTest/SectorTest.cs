using VPTLogic;

namespace VPTTest;


[TestClass]
public class SectorTest
{
    [TestMethod]
    public void PlaceInRow_Should_PlaceVisitorsInFirstAvailableRow()
    {
        // Arrange

        Group groupA = new Group();
        {
            groupA.ChangeContainsChild(true);
            groupA.ChangeChildCount(1);
            groupA.ChangeAdultCount(2);
            groupA.ChangeContainsAdult(true);
        }
        Group groupB = new Group();
        {
            groupB.ChangeContainsChild(true);
            groupB.ChangeChildCount(1);
            groupB.ChangeAdultCount(5);
            groupB.ChangeContainsAdult(true);
        }

        Tournament tournament = new Tournament();
        Sector sectorA = new Sector(2, 3, 'A');
        tournament.SectorsList.Add(sectorA);
        groupA.AddGroupCountToVisitorsList(1, 2);
        groupB.AddGroupCountToVisitorsList(1, 5);
        tournament.Groups.Add(groupA);
        tournament.Groups.Add(groupB);
        

        // Act
        tournament.PlaceInSector(groupA);
        tournament.PlaceInSector(groupB);
        

        // Assert
        foreach (var group in tournament.Groups)
        {
            Console.WriteLine($"{group.GroupId} - {group.VisitorsList.Count()}");
            foreach (var visitor in group.VisitorsList)
            {
                Console.WriteLine($"    {visitor.Name} - {visitor.Adult} - {visitor.AssignedSeat}");
            }
            Console.WriteLine();
        }
        Assert.IsTrue(sectorA.RowsList[0].CheckIfFull());
        Assert.IsFalse(sectorA.RowsList[1].CheckIfFull());
    }
}