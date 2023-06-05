namespace VPTLogic;

public class Tournament
{
    public List<Sector> SectorsList { get; private set; }
    public List<Group> Groups { get; private set; }
    public char[] sectorLetters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    public int MaxVisitors { get; private set; }
    public int VisitorsAmount { get; private set; }
    public DateOnly SignupDeadline { get; private set; }
    public Tournament()
    {
        SectorsList = new List<Sector>();
        Groups = new List<Group>();
        SignupDeadline = DateOnly.FromDateTime(DateTime.Now);
    }
    
    
    public bool CreateSectors()
    {
        Random r = new Random();
        int amount = r.Next (1, 24);
        
        for (int i = 0; i < amount; i++)
        {
            int RowsLength = r.Next(3, 11);
            int RowsAmount = r.Next(1, 4);
            Sector sector = new Sector(RowsAmount, RowsLength, sectorLetters[i]);
            SectorsList.Add(sector);
        }
        foreach (var sector in SectorsList)
        {
            sector.CountTotalSeats();
        }
        CountMaxVisitors();
        return true;
    }

    public void CreateVisitors()
    {
        int groupedVisitors = 0;
        Random random = new Random();
        //Check VisitorsAmount
        int minVisitors = Convert.ToInt32(MaxVisitors * .8);
        int maxVisitors = Convert.ToInt32(MaxVisitors * 1.3);
        int visitorsAmount = random.Next(minVisitors, maxVisitors);
        VisitorsAmount = visitorsAmount;
        
        while (groupedVisitors != visitorsAmount)
        {
            Group group = new Group();
            int groupSize = 0;
            
            if (visitorsAmount - groupedVisitors >= 16)
            {
                groupSize = random.Next(1, 17);
            }
            else
            {
                groupSize = random.Next(1, visitorsAmount - groupedVisitors);
            }
            
            for (int i = 0; i < groupSize; i++)
            {
                Visitor visitor = new Visitor();
                group.VisitorsList.Add(visitor);
            }
            
            groupedVisitors += group.VisitorsList.Count();
            Groups.Add(group);
        }
    }

    
    public void CheckGroups()
    {   
        for (int i = Groups.Count - 1; i >= 0; i--)
        {
            Groups[i].CountVisitors();
            
            var group = Groups[i];

            if (group.AdultCount < 2 && group.ChildCount >= GetRowsSeatsListCount())
            {
                Groups.RemoveAt(i);
            }
            else if (!group.ContainsAdult)
            {
                Groups.RemoveAt(i);
            }
            // else if (!group.CheckSignUpDate())
            // {
            //     Groups.RemoveAt(i);
            // }
        }
    }
    
    #region Counting
    public void CountMaxVisitors()
    {
        int maxVisitors = 0;
        foreach (var sector in SectorsList)
        {
            maxVisitors += sector.TotalSeats;
        }
        MaxVisitors = maxVisitors;
    }

    public int GetRowsSeatsListCount()
    {
        int rowSeats = 0;
        foreach (var sector in SectorsList)
        {
            foreach (var row in sector.RowsList)
            {
                rowSeats = row.SeatsList.Count();
            }
        }
        return rowSeats;
    }
    #endregion
    
    #region Placement
    public void PlaceGroups()
    {
        OrderGroupsByChildrenCount();
        foreach (var group in Groups)
        {
            PlaceInSector(group);
        }
    }
    public void PlaceInSector(Group group)
    {
        group.OrderGroupByAge();
        foreach (Sector sector in SectorsList)
        {
            sector.CheckIfFrontSeatsFull();
            if (group.ContainsChild && !sector.FrontSeatsFull && !group.CheckIfChildrenSeated())
            {
                PlaceChildrenInSector(sector, group);
            }
            //hij moet hier naar de volgende sector gaan in plaats van gewoon de kinderen ergens anders neer te stoppen.
            else if (!sector.CheckIfFull())
            {
                sector.PlaceInRow(group);
            }
        }
    }
    public void PlaceChildrenInSector(Sector sector, Group group)
    {
        if (sector.RowsList[0].SeatsLeft > group.ChildCount)
        {
            sector.RowsList[0].PlaceVisitors(group);
            group.CheckIfChildrenSeated();
            group.CheckIfGroupSeated();
            if (group.ChidrenSeated && !group.IsPlaced)
            {
                sector.PlaceInRow(group);
            }
        }
    }
    #endregion

    #region Ordering

    public void OrderGroupsByChildrenCount()
    {
        var orderGroups = Groups.OrderByDescending(g => g.ChildCount);
        Groups = orderGroups.ToList();
    }

    #endregion
}