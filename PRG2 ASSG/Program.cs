using PRG2_ASSG;




class program
{
    static Terminal terminal = new Terminal();

    static void Main()
    {
        LoadFiles();
        Console.Write("\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n0. Exit\r\nPlease select your option");
        byte number = Convert.ToByte(Console.ReadLine());
        if (number == 2) 
        {
            ListAllBoardingGates();
        }
    }




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



    static void ListAllBoardingGates()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");

        Console.WriteLine($"{"Gate Name",-20} {"DDJB",-15} {"CFFT",-10} {"LWTT",-5}");

        // Access the boardingGates from the terminal object (which is already initialized)
        foreach (var gate in terminal.BoardingGates.Values)
        {
            // For each gate, print the details along with the assigned flight number (if any)
            Console.WriteLine($"{gate.GateName,-20} {gate.SupportsDDJB,-15} {gate.SupportsCFFT,-10} {gate.SupportsLWTT,-5}");
        }
    }
    
}

