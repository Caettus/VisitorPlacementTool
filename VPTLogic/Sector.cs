namespace VPTLogic;

public class Sector
{
    public char SectorLetter { get; private set; }
    public List<Row> RowsList { get; private set; }
    public int TotalSeats { get; private set; }
    public bool FrontSeatsFull { get; private set; }
    public bool BackSeatsFull { get; private set; }
    public int SeatsLeft { get; private set; }
    public bool Full { get; private set; }
    
    public Sector(int rowCount, int rowLength, char sectorLetter)
    {
        SectorLetter = sectorLetter;
        RowsList = new List<Row>();
        CreateRows(rowCount, rowLength);
    }

    private bool CreateRows(int rowCount, int rowLength)
    {
        bool rowsHaveBeenCreated = false;

        for (int i = 0; i < rowCount; i++)
        {
            Row row = new Row(i + 1, SectorLetter);
            row.CreateSeats(rowLength);
            RowsList.Add(row);
        }

        return rowsHaveBeenCreated;
    }
    
    public void PlaceInRow(Group group)
    {
            foreach (var row in RowsList)
            {
                if (!row.CheckIfFull())
                {
                    row.PlaceVisitors(group);
                }
                else
                {
                    continue;
                }
            } 
    }

    #region Counting and Checking Methods
    public bool CheckIfFrontSeatsAreTaken()
    {
        FrontSeatsFull = false;
        if (RowsList[0].CheckIfFull())
        {
            FrontSeatsFull = true;
        }
        return FrontSeatsFull;
    }

    public bool CheckIfBackRowSeatsAreTaken()
    {
        BackSeatsFull = true;
        if (RowsList.Count() > 1)
        {
            for (int i = 1; i < RowsList.Count; i++)
            {
                if (!RowsList[i].CheckIfFull())
                {
                    BackSeatsFull = false;
                    break;
                }
            }
        }
        else
        {
            BackSeatsFull = true;
        }
        return BackSeatsFull;
    }

    public bool CheckIfFull()
    {
        Full = true;
        foreach (var row in RowsList)
        {
            row.CheckIfFull();
            if (!row.Full)
            {
                Full = false;
                break;
            }
        }
        return Full;
    }

    public void CountTotalSeats()
    {
        int totalSeats = 0;

        foreach (var row in RowsList)
        {
            totalSeats += row.SeatsList.Count();
        }

        TotalSeats = totalSeats;
    }

    public int CountSeatsLeft()
    {
        int seatsLeft = 0;
        foreach (Row row in RowsList)
        {
            row.CountSeatsLeft();
            seatsLeft += row.SeatsLeft;
        }
        SeatsLeft = seatsLeft;
        return seatsLeft;
    }

    #endregion
}