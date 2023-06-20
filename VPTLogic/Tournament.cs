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
    public bool BackRowSeatsTaken { get; private set; }
    public bool FrontRowSeatsTaken { get; private set; }
    public bool TournamentFull { get; private set; }
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
    #endregion
    
    #region Placement
    public void PlaceGroups()
    {
        OrderGroupsByChildrenCount();
        
        foreach (var group in Groups)
        {
            CountSeatsLeft();
            while (!group.IsPlaced)
            {
                if(SeatsLeft >= group.VisitorsList.Count())
                {
                    group.OrderGroupByAge();
                    group.DefaultCheck();
                    if(!TryPlaceInSector(group))
                    {
                        break;
                    }
                    CountSeatsLeft();
                }
                else
                {
                    break;
                }
            }

        }
    }
    
    private Sector FindSuitableSectorForChildren(Group group)
    {
        foreach (Sector sector in SectorsList)
        {
            sector.CheckIfFrontSeatsAreTaken();
            if (group.ContainsChild && !sector.FrontSeatsFull && sector.RowsList[0].SeatsLeft >= group.ChildCount)
            {
                return sector;
            }
        }
        return null;
    }

    private bool TryPlaceInSector(Group group)
    {
        bool groupCanBePlaced = true;
        // while loop to make sure all group members are placed - if there aren't enough seats a group will be skipped
        while (!group.IsPlaced && groupCanBePlaced)
        {
            DefaultTournamentCheck();
            Sector suitableSectorForChildren = FindSuitableSectorForChildren(group);

            if (suitableSectorForChildren != null)
            {
                PlaceChildrenInSector(suitableSectorForChildren, group);
            }
            else if (!group.ContainsChild)
            {
                foreach (var sector in SectorsList)
                {
                    group.DefaultCheck();
                    // if sector is full skip to next sector and check if unseated group members fit in the sector
                    if (!sector.CheckIfFull())
                    {
                        // try placing the group in the sector
                        sector.PlaceInRow( group);

                        // if group is not yet placed, place group in next sector
                        if (!group.IsPlaced)
                        {
                            continue;
                        }
                        break;
                    }
                }
            }
            group.DefaultCheck();   
            // if no group members are placed in any sector, group cannot be placed
            if (group.UnseatedGroupMembers == group.VisitorsList.Count())
            {
                groupCanBePlaced = false;
            }
        }
        return groupCanBePlaced;
    }



    private void PlaceChildrenInSector(Sector sector, Group group)
    {
        if (sector.RowsList[0].SeatsLeft >= group.ChildCount)
        {
            sector.RowsList[0].PlaceVisitors(group);
            group.CheckIfVisitorSeated();

            sector.CountSeatsLeft();
            int seatsAvailable = sector.SeatsLeft;

            if (seatsAvailable >= group.AdultCount)
            {
                group.CheckIfGroupSeated();
                if (group.ChildrenSeated && !group.IsPlaced)
                {
                    sector.PlaceInRow(group);
                    group.CheckIfGroupSeated();
                    if (!group.IsPlaced)
                    {
                        foreach (Row row in sector.RowsList)
                        {
                            row.UnplaceVisitors(group);
                            group.ResetSeatedStatus();
                        }
                    }
                }
            }

            group.CheckAdultsLeft();
            if (group.AdultsLeft > seatsAvailable)
            {
                foreach (Row row in sector.RowsList)
                {
                    row.UnplaceVisitors(group);
                    group.ResetSeatedStatus();
                }
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

    private int CountSeatsLeft()
    {
        int seatsLeft = 0;
        foreach (var sector in SectorsList)
        {
            seatsLeft += sector.SeatsLeft;
        }
        return seatsLeft;
    }


    private void DefaultTournamentCheck()
    {
        CheckIfFull();
        CheckIfFrontSeatsTaken();
        CheckIfBackSeatsTaken();
    }

    private bool CheckIfBackSeatsTaken()
    {
        BackRowSeatsTaken = true;
        foreach (var sector in SectorsList)
        {
            if (!sector.CheckIfBackRowSeatsAreTaken())
            {
                BackRowSeatsTaken = false;
                break;
            }
        }
        return BackRowSeatsTaken;
    }

    private bool CheckIfFrontSeatsTaken()
    {
        FrontRowSeatsTaken = true;
        foreach (var sector in SectorsList)
        {
            if (!sector.FrontSeatsFull)
            {
                FrontRowSeatsTaken = false;
                break;
            }
        }
        return FrontRowSeatsTaken;
    }

    private bool CheckIfFull()
    {
        TournamentFull = true;
        foreach (var sector in SectorsList)
        {
            if (!sector.Full)
            {
                TournamentFull = false;
                break;
            }
        }
        return TournamentFull;
    }

    #endregion
}