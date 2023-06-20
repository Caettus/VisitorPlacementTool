// See https://aka.ms/new-console-template for more information

using VPTLogic;

Console.WriteLine("Hello, World!");

Tournament tournament = new Tournament();
tournament.CreateSectors();
tournament.CreateVisitors();
tournament.CheckGroups();
tournament.PlaceGroups();


foreach (var sector in tournament.SectorsList)
{
    foreach(var row in sector.RowsList)
    {
        foreach(var seat in row.SeatsList)
        {
            Console.WriteLine($"{seat.Code}");
        }
    }
}

foreach (var group in tournament.Groups)
{
    Console.WriteLine($"{group.GroupId} - {group.VisitorsList.Count()}");
    foreach (var visitor in group.VisitorsList)
    {
        Console.WriteLine($"    {visitor.Name} - {visitor.Adult} - {visitor.AssignedSeat}");
    }
    Console.WriteLine();
}

Console.WriteLine($"{tournament.Groups.Count()} groups");
Console.WriteLine($"{tournament.VisitorsAmount} visitors");
Console.WriteLine($"Er zijn maximaal {tournament.MaxVisitors} Seats");
Console.WriteLine($"Er zijn: {tournament.SectorsList.Count()} Sectoren");