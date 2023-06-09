namespace VPTLogic;

public class Sector
{
    public char SectorLetter { get; private set; }
    public List<Row> RowsList { get; private set; }
    public int TotalSeats { get; private set; }
    public bool FrontSeatsFull { get; private set; }
    
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
    public bool CheckIfFrontSeatsFull()
    {
        if (RowsList[0].CheckIfFull())
        {
            FrontSeatsFull = true;
        }
        else
        {
            FrontSeatsFull = false;
        }
        return FrontSeatsFull;
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

    public bool CheckIfFull()
    {
        bool full = true;
        foreach (var row in RowsList)
        {
            foreach (var seat in row.SeatsList)
            {
                if (!seat.Occupied)
                {
                    full = false;
                    break;
                }
            }
        }
        return full;
    }
    #endregion
}