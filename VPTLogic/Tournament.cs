namespace VPTLogic
{
    public class Tournament
    {
        #region Properties

        public List<Sector> SectorsList { get; set; }
        public List<Group> Groups { get; set; }
        public char[] sectorLetters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public int MaxVisitors { get; set; }
        public int VisitorsAmount { get; private set; }
        public int SeatsLeft { get; private set; }
        private bool BackRowSeatsTaken { get; set; }
        private bool FrontRowSeatsTaken { get; set; }
        private bool Full { get; set; }

        #endregion

        #region Constructors

        public Tournament()
        {
            SectorsList = new List<Sector>();
            Groups = new List<Group>();
        }

        #endregion

        #region Public Methods

        public bool CreateSectors()
        {
            Random r = new Random();
            int amount = r.Next(1, 24);

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
            }
        }

        public void PlaceVisitors()
        {
            OrderGroupsBySize();
            OrderGroupsByChildrenCount();
            PlaceGroups();
        }

        #endregion

        #region Private Methods

        private void CountMaxVisitors()
        {
            int maxVisitors = 0;
            foreach (var sector in SectorsList)
            {
                maxVisitors += sector.TotalSeats;
            }
            MaxVisitors = maxVisitors;
        }

        private int GetRowsSeatsListCount()
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

        private void PlaceGroups()
        {
            foreach (var group in Groups)
            {
                CountSeatsLeft();
                while (!group.IsPlaced)
                {
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
                    else
                    {
                        break;
                    }
                    DefaultTournamentChecks();
                }
                if (Full)
                {
                    break;
                }
            }
        }

        private bool TryPlaceInSector(Group group)
        {
            bool groupCanBePlaced = true;

            while (!group.IsPlaced && groupCanBePlaced)
            {
                DefaultTournamentChecks();
                foreach (var sector in SectorsList)
                {
                    group.DefaultCheck();

                    if (!sector.CheckIfFull())
                    {
                        sector.PlaceVisitors(sector, group);

                        if (!group.IsPlaced)
                        {
                            continue;
                        }
                        break;
                    }
                }
                if (group.UnseatedGroupMembers == group.VisitorsList.Count())
                {
                    groupCanBePlaced = false;
                }
            }
            return groupCanBePlaced;
        }

        private void OrderGroupsBySize()
        {
            var orderedGroupsOnSize = Groups.OrderByDescending(x => x.VisitorsList.Count());
            Groups = orderedGroupsOnSize.ToList();
        }

        private void OrderGroupsByChildrenCount()
        {
            var orderedGroupsOnChildrenCount = Groups.OrderByDescending(x => x.ChildCount);
            Groups = orderedGroupsOnChildrenCount.ToList();
        }

        private int CountSeatsLeft()
        {
            SeatsLeft = 0;
            foreach (var sector in SectorsList)
            {
                sector.CountSeatsLeft();
                SeatsLeft += sector.SeatsLeft;
            }
            return SeatsLeft;
        }

        private void DefaultTournamentChecks()
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
                if (!sector.CheckIfBackRowSeatsFull())
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
                if (!sector.CheckIfFrontSeatsFull())
                {
                    FrontRowSeatsTaken = false;
                    break;
                }
            }
            return FrontRowSeatsTaken;
        }

        private bool CheckIfFull()
        {
            Full = true;
            foreach (var sector in SectorsList)
            {
                if (!sector.Full)
                {
                    Full = false;
                    break;
                }
            }
            return Full;
        }

        #endregion
    }
}
