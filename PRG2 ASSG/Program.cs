using System.Collections.Generic;
using PRG2_ASSG;


class program
{
    static Terminal terminal = new Terminal();

    static void Main()
    {
        LoadFiles();
        Console.WriteLine("\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n0. Exit\r\nPlease select your option");
        byte number = Convert.ToByte(Console.ReadLine());
        if (number == 2) 
        {
            ListAllBoardingGates();
        }
    }



//Feature #1
static void LoadFiles()
    {
        
        List<Airline> Airplanelist = new List<Airline>();
        using (StreamReader sr = new StreamReader("airlines.csv"))
        {
            string s = sr.ReadLine();

            while ((s = sr.ReadLine()) != null)
            {
                string[] airplane = s.Split(',');
                Airline a = new Airline(airplane[0], airplane[1]);
                terminal.AddAirline(a);
                Airplanelist.Add(new Airline(airplane[0], airplane[1]));

            }

        }

        List<BoardingGate> BoardingGateList = new List<BoardingGate>();
        using (StreamReader sr = new StreamReader("boardinggates.csv"))
        {
            string x = sr.ReadLine();

            while ((x = sr.ReadLine()) != null)
            {
                string[] gateperm = x.Split(',');
                BoardingGate b = new BoardingGate(gateperm[0], Convert.ToBoolean(gateperm[1]), Convert.ToBoolean(gateperm[2]), Convert.ToBoolean(gateperm[3]));
                terminal.AddBoardingGate(b);

                BoardingGateList.Add(new BoardingGate(gateperm[0], Convert.ToBoolean(gateperm[1]), Convert.ToBoolean(gateperm[2]), Convert.ToBoolean(gateperm[3])));
            }

        }
    }

//Feature #2
static void LoadFlights()
{
    Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
    try
    {
        using (StreamReader reader = new StreamReader("flights.csv"))
        {
            string headerLine = reader.ReadLine(); 
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                if (values.Length >= 5)
                {
                    string flightNumber = values[0].Trim();
                    string origin = values[1].Trim();
                    string destination = values[2].Trim();
                    string timeString = values[3].Trim();
                    string status = values[4].Trim();

                    if (DateTime.TryParseExact(timeString, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expectedTime))
                    {
                        Flight flight = new Flight(flightNumber, origin, destination, status, expectedTime);
                        flights[flightNumber] = flight; 
                    }
                }
            }
        }
    }
}





//Feature #4
    static void ListAllBoardingGates()
    {
        Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================");

        Console.WriteLine($"{"Gate Name",-20} {"DDJB",-15} {"CFFT",-10} {"LWTT",-5}");

        // Access the boardingGates from the terminal object (which is already initialized)
        foreach (var gate in terminal.BoardingGates.Values)
        {
            // For each gate, print the details along with the assigned flight number (if any)
            Console.WriteLine($"{gate.GateName,-20} {gate.SupportsDDJB,-15} {gate.SupportsCFFT,-10} {gate.SupportsLWTT,-5}");
        }
    }
    static void ListAirlines()
    {
        Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================\r\nAirline Code Airline Name\r\nSQ Singapore Airlines\r\nMH Malaysia Airlines\r\nJL Japan Airlines\r\nCX Cathay Pacific\r\nQF Qantas Airways\r\nTR AirAsia\r\nEK Emirates\r\nBA British Airways\r\nEnter Airline Code: SQ\r\n=============================================\r\nList of Flights for Singapore Airlines\r\n=============================================");
        string code = Console.ReadLine();

        Console.WriteLine($"{"Flight Number",-25} {"Airline Name",-20} {"Origin",-15} {"Destination",-10} {"Expected",-5}");

        foreach (var airline in terminal.Airlines.Values)
        { 
            if (airline.) {

        }
    }
    
}

