namespace VPTLogic;

public class Row
{
    public List<Seat> SeatsList { get; private set; }
    public string Code { get; private set; }
    public int RowNumber { get; private set; }
    public bool Full { get; private set; }
    public int SeatsLeft { get; private set; }
    
    
    public bool CreateSeats(int length)
    {
        for (int i = 0; i < length; i++)
        {
            Seat seat = new Seat(SeatsList.Count + 1, Code);
            SeatsList.Add(seat);
        }
        return true;
    }
    
    public Row(int rowNumber, char sectorLetter)
    {
        RowNumber = rowNumber;
        Code = sectorLetter.ToString() + RowNumber.ToString();
        SeatsList = new List<Seat>();
    }

    public void PlaceVisitors(Group group)
    {
        foreach (Visitor visitor in group.VisitorsList)
        {
            if (!visitor.Seated)
            { 
                PlaceInSeats(visitor, group);
            }
        }
    }

    public void UnplaceVisitors(Group group)
    {
        foreach (Visitor visitor in group.VisitorsList)
        {
            if (visitor.Seated)
            {
                UnplaceFromSeat(visitor);
            }
        }
    }

    private void UnplaceFromSeat(Visitor visitor)
    {
        foreach (Seat seat in SeatsList)
        {
            if (seat.Occupied)
            {
                seat.SetUnoccupied();
                visitor.UnplaceVisitor();
                break;
            }
        }
    }

    private void PlaceInSeats(Visitor visitor, Group group)
    {
            foreach (Seat seat in SeatsList)
            {
                if (!seat.Occupied)
                {
                    seat.SetOccupied(visitor);
                    visitor.PlaceVisitor(seat.Code);
                    break;
                }
            }
    }

    public bool CheckIfFull()
    {
        Full = true;
        foreach (Seat seat in SeatsList)
        {
            if (!seat.Occupied)
            {
                Full = false;
            }
        }
        CountSeatsLeft();
       return Full;
    }

    public int CountSeatsLeft()
    {
        int seatsLeft = 0;
        foreach (Seat seat in SeatsList)
        {
            if (!seat.Occupied)
            {
                seatsLeft++;
            }
        }
        SeatsLeft = seatsLeft;
        return seatsLeft;
    }
}