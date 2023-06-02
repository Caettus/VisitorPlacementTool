namespace VPTLogic;

public class Group
{
    public int GroupId { get; private set; }
    public List<Visitor> VisitorsList { get; private set; }
    public int AdultCount { get; private set; }
    public int ChildCount { get; private set; }
    public bool ContainsAdult { get; private set; }
    public bool IsPlaced { get; private set; }

    public Group()
    {
        VisitorsList = new List<Visitor>();
    }

    public bool CheckIfGroupContainsAdult()
    {
        bool containsAdult = false;
        foreach (var visitor in VisitorsList)
        {
            if (visitor.Adult)
            {
                ContainsAdult = true;
                containsAdult = true;
                break;
            }
        }

        return containsAdult;
    }

    public void CountVisitors()
    {
        int adults = 0;
        foreach (var visitor in VisitorsList)
        {
            if (visitor.Adult)
            {
                AdultCount++;
            }
            else if (!visitor.Adult)
            {
                ChildCount++;
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
}