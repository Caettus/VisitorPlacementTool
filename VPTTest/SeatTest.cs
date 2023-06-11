using VPTLogic;

namespace VPTTest;

[TestClass]
public class SeatTest
{
    [TestMethod]
    public void SetOccupiedTest()
    {
        // Arrange
        Visitor visitor = new Visitor();
        Seat seat = new Seat(1, "A1");

        // Act
        seat.SetOccupied(visitor);

        // Assert
        Assert.AreEqual(visitor, seat.SeatedVisitor);
        Assert.IsTrue(seat.Occupied);
    }
    
    [TestMethod]
    public void SetUnOccupiedTest()
    {
        // Arrange
        Visitor visitor = new Visitor();
        Seat seat = new Seat(1, "A1");
        seat.SetOccupied(visitor);

        // Act
        seat.SetUnoccupied();

        // Assert
        Assert.IsNull(seat.SeatedVisitor);
        Assert.IsFalse(seat.Occupied);
    }
    
}