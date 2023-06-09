namespace VPTLogic;

public class Group
{
    public int GroupId { get; private set; }
    public List<Visitor> VisitorsList { get; private set; }
    public int AdultCount { get; private set; }
    public int ChildCount { get; private set; }
    public bool ContainsAdult { get; private set; }
    public bool IsPlaced { get; private set; }
    public bool ContainsChild { get; private set; }
    public bool ChidrenSeated { get; private set; }

    
    private static int groupIdCounter = 1;
    public Group()
    {
        GroupId = groupIdCounter++;
        VisitorsList = new List<Visitor>();
    }

    public void OrderGroupByAge()
    {
        var ordernedVisitorsList = VisitorsList.OrderBy(v => v.Adult);
        VisitorsList = ordernedVisitorsList.ToList();
    }
    
    public void CountVisitors()
    {
        int adults = 0;
        foreach (var visitor in VisitorsList)
        {
            if (visitor.Adult)
            {
                AdultCount++;
                ContainsAdult = true;
            }
            else if (!visitor.Adult)
            {
                ChildCount++;
                ContainsChild = true;
            }
        }
    }
    public bool CheckIfChildrenSeated()
    {
        ChidrenSeated = true;
        foreach (var visitor in VisitorsList)
        {
            if (!visitor.Adult && !visitor.Seated)
            {
                ChidrenSeated = false;
            }
            else if (!visitor.Adult && visitor.Seated)
            {
                ChidrenSeated = true;
            }
        }
        return ChidrenSeated;
    }

    public void CheckIfGroupSeated()
    {
        IsPlaced = true;
        foreach (var visitor in VisitorsList)
        {
            if (!visitor.Seated)
            {
                IsPlaced = false;
            }
        }
    }
    
    //DateTime.Today is de signup deadline
    // public bool CheckSignUpDate()
    // {
    //     bool correctSignupDate = true;
    //     for (int i = VisitorsList.Count - 1; i >= 0; i--)
    //     {
    //         var visitor = VisitorsList[i];
    //         if (visitor.SignupDate != DateTime.Today)
    //         {
    //             correctSignupDate = false;
    //         }
    //     }
    //     return correctSignupDate;
    // }
    
    #region test methods
    public void ChangeAdultCount(int amount)
    {
        AdultCount += amount;
    }
    
    public void ChangeChildCount(int amount)
    {
        ChildCount += amount;
    }
    
    public void ChangeContainsAdult(bool value)
    {
        ContainsAdult = value;
    }
    public void ChangeContainsChild(bool value)
    {
        ContainsChild = value;
    }

    public void AddGroupCountToVisitorsList(int childCount, int adultCount)
    {
        if (childCount != 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                Visitor visitor = new Visitor();
                visitor.ChangeAdultStatus(false);
                VisitorsList.Add(visitor);
            }
        }

        if (adultCount != 0)
        {
            for (int i = 0; i < adultCount; i++)
            {
                Visitor visitor = new Visitor();
                visitor.ChangeAdultStatus(true);
                VisitorsList.Add(visitor);
            }
        }
    }
    #endregion
}