namespace VPTLogic;

public class Tournament
{
    public List<Sector> SectorsList { get; set; }
    public List<Group> Groups { get; set; }
    public char[] sectorLetters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    public int MaxVisitors { get; set; }
    //dit moet public zijn want: 1 individuele specifieke test runnen is blijkbaar exact hetzelfde als het totale programma runnen
    public int VisitorsAmount { get; private set; }
    public int SeatsLeft { get; private set; }
    public Tournament()
    {
        SectorsList = new List<Sector>();
        Groups = new List<Group>();
    }
    
    #region Create Methods
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
    
    #endregion
    
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
    
    public void CheckIfGroupsSeated()
    {
        foreach (Group group in Groups)
        {
            group.DefaultCheck();
            if (!group.IsPlaced)
            {
                TryPlaceInSector(group);
            }
            if (group.AdultsSeated && !group.ChildrenSeated)
            {
                group.ResetSeatedStatus(group);
                UnplaceGroup(group);
            }
            else if (!group.AdultsSeated && group.ChildrenSeated)
            {
                group.ResetSeatedStatus(group);
                UnplaceGroup(group);    
            }
        }
    }

    #region Counting
    private void CountMaxVisitors()
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

    public int CountSeatsLeft()
    {
        SeatsLeft = 0;
        foreach ( Sector sector in SectorsList)
        {
            sector.CountSeatsLeft();
            SeatsLeft += sector.SeatsLeft;
        }
        return SeatsLeft;
    }
    #endregion

    #region Placement

    public void PlaceGroups()
    {
        foreach (Group group in Groups)
        {
            group.DefaultCheck();
            while (!group.IsPlaced)
            {
                CountSeatsLeft();
                if (SeatsLeft >= group.VisitorsList.Count())
                {
                    group.OrderGroupByAge();
                    group.DefaultCheck();
                    if (!TryPlaceInSector(group))
                    {
                        break;
                    }
                    CountSeatsLeft();
                }
                else { break; }
            }
            if(group.IsPlaced)
            {
                continue;
            }

        }
    }


    private bool TryPlaceInSector(Group group)
    {
        foreach (Sector sector in SectorsList)
        {
            if (sector.SeatsLeft >= group.VisitorsList.Count())
            {
                sector.PlaceVisitors(sector, group);
                group.DefaultCheck();
                if (!group.IsPlaced)
                {
                    continue;
                }
                return true;
            }
        }
        return false;
    }


    private void UnplaceGroup(Group group)
    {
        foreach (Sector sector in SectorsList)
        {
            foreach (Row row in sector.RowsList)
            {
                row.UnplaceVisitors(group);
            }
        }
    }



    #endregion

    #region Ordering

    private void OrderGroupsByChildrenCount()
    {
        var orderGroups = Groups.OrderByDescending(g => g.ChildCount);
        Groups = orderGroups.ToList();
    }

    #endregion
}