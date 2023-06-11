using VPTLogic;

namespace VPTTest;

[TestClass]
public class Visitortest
{
    [TestMethod]
    public void PlaceVisitorTest()
    {
        // Arrange
        var visitor = new Visitor();
        var seatCode = "A1";

        // Act
        visitor.PlaceVisitor(seatCode);

        // Assert
        Assert.AreEqual(seatCode, visitor.AssignedSeat);
        Assert.IsTrue(visitor.Seated);
    }
    
    [TestMethod]
    public void UnplaceVisitorTest()
    {
        // Arrange
        var visitor = new Visitor();
        visitor.ChangeAssignedSeat("A1");
        visitor.ChangeSeatedStatus(true);

        // Act
        visitor.UnplaceVisitor();

        // Assert
        Assert.IsNull(visitor.AssignedSeat);
        Assert.IsFalse(visitor.Seated);
    }
}