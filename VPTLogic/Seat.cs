namespace VPTLogic;

public class Seat
{
    public int FollowNumber { get; private set; }
    public string Code { get; private set; }
    public bool Occupied { get; private set; }
    public Visitor SeatedVisitor { get; private set;}
    public Seat(int seatNumber, string rowCode)
    {
        FollowNumber = seatNumber;
        Code = rowCode + "-" + FollowNumber.ToString();
    }
    
    public void SetOccupied(Visitor visitor)
    {
        SeatedVisitor = visitor;
        Occupied = true;
    }
}