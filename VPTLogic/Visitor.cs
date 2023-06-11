namespace VPTLogic;

public class Visitor
{
    public int VisitorId { get; private set; }
    public string Name { get; private set; }
    public bool Adult { get; private set; }
    public DateTime SignupDate { get; private set; }
    public bool Seated { get; private set; }
    public string AssignedSeat { get; private set; }

    public Visitor()
    {
        VisitorId = GenerateNewId();
        GenerateName();
        GenerateAdultStatus();
        GenerateSignupDate();
    }
    
    
    private static int lastGeneratedId = 0;

    private static int GenerateNewId()
    {
        lastGeneratedId++;
        return lastGeneratedId;
    }

    public void GenerateName()
    {
        Random r = new Random();
        int len = r.Next(3, 7);
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string name = "";
        name += consonants[r.Next(consonants.Length)].ToUpper();
        name += vowels[r.Next(vowels.Length)];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            name += consonants[r.Next(consonants.Length)];
            b++;
            name += vowels[r.Next(vowels.Length)];
            b++;
        }

        Name = name;
    }
    
    public void GenerateAdultStatus()
    {
        Random random = new Random();
        int age = random.Next(4, 65);
        if (age >= 12)
        {
            Adult = true;
        }
    }
    
    public void GenerateSignupDate()
    {
        Random random = new Random();
        int days = random.Next(1, 365);
        SignupDate = DateTime.Now.AddDays(-days);
    }

    public void PlaceVisitor(string seatCode)
    {
        AssignedSeat = seatCode;
        Seated = true;
    }
    
    public void UnplaceVisitor()
    {
        AssignedSeat = null;
        Seated = false;
    }
    
    #region test methods
    public void ChangeAdultStatus(bool adult)
    {
        Adult = adult;
    }
    #endregion
}