using Microsoft.VisualStudio.TestTools.UnitTesting;
using VPTLogic;

namespace VPTTest;

[TestClass]
public class TournamentTest
{
    // 1
    [TestMethod]
    public void CreateSectors_Test()
    {
        // Arrange
        Tournament tournament = new Tournament();
        // Act
        bool result = tournament.CreateSectors();
        // Assert
        Assert.IsTrue(result);
    }

    // 2
    [TestMethod]
    public void CreateVisitors_VisitorsAmountTest()
    {
        //Arrange
        Tournament tournament = new Tournament();
        tournament.MaxVisitors = 100;
        //Act
        tournament.CreateVisitors();
        //Assert
        bool isWithinRange = tournament.VisitorsAmount >= 80 && tournament.VisitorsAmount <= 130;
        Console.WriteLine("Actual Result: " + tournament.VisitorsAmount);
        Assert.IsTrue(isWithinRange, "The number of visitors is not within the expected range.");
    }
    
    // 3
    [TestMethod]
    public void CreateVisitors_GroupsAmountTest()
    {
        //Arrange
        Tournament tournament = new Tournament();
        tournament.MaxVisitors = 100;
        //Act
        tournament.CreateVisitors();
        //Assert
        bool isWithinRange = tournament.Groups.Count() >= 1 && tournament.Groups.Count() <= 100;
        Console.WriteLine("Actual Result: " + tournament.Groups.Count());
        Assert.IsTrue(isWithinRange, "The number of groups is not within the expected range.");
    }
    
    // 4
    [TestMethod]
    public void CreateVisitors_GroupedVisitorsTest()
    {
        // Arrange
        Tournament tournament = new Tournament();
        int maxVisitors = 100;

        // Act
        tournament.MaxVisitors = maxVisitors;
        tournament.CreateVisitors();

        // Assert
        int actualGroupedVisitors = 0;
        foreach (var group in tournament.Groups)
        {
            actualGroupedVisitors += group.VisitorsList.Count;
        }
        Assert.AreEqual(tournament.VisitorsAmount, actualGroupedVisitors, "Total amount of grouped visitors  should equal VisitorsAmount");
    }
    
    // 5
    [TestMethod]
    public void CheckGroups_RemoveGroupsTest()
    {
        // Arrange
        Tournament tournament = new Tournament();
        int group3ChildCount = tournament.GetRowsSeatsListCount() + 3;
        var group1 = new Group();
        {
            group1.ChangeAdultCount(0);
            group1.ChangeChildCount(10);
            group1.ChangeContainsAdult(false);
        }
        var group2 = new Group();
        {
            group2.ChangeAdultCount(10);
            group2.ChangeChildCount(0);
            group2.ChangeContainsAdult(false);
        }
        var group3 = new Group();
        {
            group3.ChangeAdultCount(0);
            group3.ChangeChildCount(group3ChildCount);
            group3.ChangeContainsAdult(true);
        }

        tournament.Groups.Add(group1);
        tournament.Groups.Add(group2);
        tournament.Groups.Add(group3);

        // Act
        tournament.CheckGroups();

        // Assert
        if (tournament.Groups.Contains(group1))
        {
            Console.WriteLine("Error: group1 was not removed.");
        }
        if (tournament.Groups.Contains(group2))
        {
            Console.WriteLine("Error: group2 was not removed.");
        }
        if (tournament.Groups.Contains(group3))
        {
            Console.WriteLine("Error: group3 was not removed.");
        }
    }

    // 6
    [TestMethod] 
    public void PlaceInSector_ChildrenNotPlacedTest()
    {
        // Arrange
        var sectorsList = new List<Sector>
        {
            new Sector(1, 1, 'A'),
            new Sector(1, 1, 'B'),
            new Sector(1, 1, 'C')
        };

        var group = new Group();
        group.ChangeContainsChild(true);
        group.ChangeChildCount(500);
        group.ChangeAdultCount(5);
        group.ChangeContainsAdult(true);

        Tournament tournament = new Tournament(); 

        // Act
        tournament.PlaceInSector(group);

        // Assert
        foreach (var sector in sectorsList)
        {
            Assert.IsFalse(sector.FrontSeatsFull);
            if (sector.FrontSeatsFull)
            {
                Console.WriteLine($"Front seats in sector {sector} are incorrectly marked as full.");
            }
        }
    }

    // // 7
    [TestMethod]
    public void PlaceInSector_ChildrenPlacedTest()
    {
        // Arrange
        Sector sector = new Sector(3, 10, 'A');

        Group newgroup = new Group();
        {
            newgroup.ChangeContainsChild(true);
            newgroup.ChangeChildCount(8);
            newgroup.ChangeAdultCount(4);
            newgroup.ChangeContainsAdult(true);
        }
        newgroup.AddGroupCountToVisitorsList(4, 8);

        Tournament tournament = new Tournament();
        tournament.SectorsList.Add(sector);
        tournament.Groups.Add(newgroup);


        // Act
        tournament.PlaceInSector(newgroup);
        sector.CheckIfFrontSeatsFull();

        // Assert
        Assert.IsTrue(sector.FrontSeatsFull);
    }
}