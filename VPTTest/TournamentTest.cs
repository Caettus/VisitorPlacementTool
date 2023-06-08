using Microsoft.VisualStudio.TestTools.UnitTesting;
using VPTLogic;

namespace VPTTest;

[TestClass]
public class TournamentTest
{
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

}