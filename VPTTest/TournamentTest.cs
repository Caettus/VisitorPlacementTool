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
    public void CreateVisitors_Test()
    {
        //Arrange
        Tournament tournament = new Tournament();
        //Act
        tournament.CreateVisitors();
        //Assert
        
    }
}