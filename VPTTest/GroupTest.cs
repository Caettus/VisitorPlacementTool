using Microsoft.VisualStudio.TestTools.UnitTesting;
using VPTLogic;

namespace VPTTest;

[TestClass]

public class GroupTest
{
    [TestMethod]
    public void OrderGroupByAgeTrueTest()
    {
        // Arrange
        Group group = new Group();
        Visitor visitor1 = new Visitor();
        visitor1.ChangeAdultStatus(true);
        Visitor visitor2 = new Visitor();
        visitor2.ChangeAdultStatus(false);
        Visitor visitor3 = new Visitor();
        visitor3.ChangeAdultStatus(true);
        Visitor visitor4 = new Visitor();
        visitor4.ChangeAdultStatus(false);
        group.VisitorsList.Add(visitor1);
        group.VisitorsList.Add(visitor2);
        group.VisitorsList.Add(visitor3);
        group.VisitorsList.Add(visitor4);
        // Act
        group.OrderGroupByAge();
        // Assert
        Assert.IsFalse(group.VisitorsList[0].Adult);
        Assert.IsFalse(group.VisitorsList[1].Adult);
        Assert.IsTrue(group.VisitorsList[2].Adult);
        Assert.IsTrue(group.VisitorsList[3].Adult);
    }

    [TestMethod]
    public void OrderGroupByAgeFalseTest()
    {
        // Arrange
        Group group = new Group();
        Visitor visitor1 = new Visitor();
        visitor1.ChangeAdultStatus(true);
        Visitor visitor2 = new Visitor();
        visitor2.ChangeAdultStatus(false);
        Visitor visitor3 = new Visitor();
        visitor3.ChangeAdultStatus(true);
        Visitor visitor4 = new Visitor();
        visitor4.ChangeAdultStatus(false);
        group.VisitorsList.Add(visitor1);
        group.VisitorsList.Add(visitor2);
        group.VisitorsList.Add(visitor3);
        group.VisitorsList.Add(visitor4);
        // Act
        // Assert
        Assert.IsTrue(group.VisitorsList[0].Adult);
        Assert.IsFalse(group.VisitorsList[1].Adult);
        Assert.IsTrue(group.VisitorsList[2].Adult);
        Assert.IsFalse(group.VisitorsList[3].Adult);
    }

    [TestMethod]
    public void CountVisitorsTest()
    {
        // Arrange
        Group group = new Group();
        Visitor visitor1 = new Visitor();
        visitor1.ChangeAdultStatus(true);
        Visitor visitor2 = new Visitor();
        visitor2.ChangeAdultStatus(false);
        Visitor visitor3 = new Visitor();
        visitor3.ChangeAdultStatus(true);
        Visitor visitor4 = new Visitor();
        visitor4.ChangeAdultStatus(false);
        group.VisitorsList.Add(visitor1);
        group.VisitorsList.Add(visitor2);
        group.VisitorsList.Add(visitor3);
        group.VisitorsList.Add(visitor4);
        // Act
        group.CountVisitors();
        // Assert
        Assert.AreEqual(4, group.VisitorsList.Count);
    }
    
}