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
                        terminal.AddFlight(flight); // Adds flight to terminal
                    }
                }
            }
        }
    }
}
//Feature #3
static void ListFlights()
{
    Console.WriteLine("=============================================\r\nList of Flights\r\n=============================================");
    Console.WriteLine($"{"Flight Number",-15} {"Origin",-15} {"Destination",-15} {"Expected Time",-15} {"Status",-10}");

    foreach (var flight in terminal.Flights.Values) // Access flights from terminal
    {
        Console.WriteLine($"{flight.FlightNumber,-15} {flight.Origin,-15} {flight.Destination,-15} {flight.ExpectedTime.ToString("h:mm tt"),-15} {flight.Status,-10}");
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
//Feature #5
static void AssignGate()
{
    // Step 1: Prompt the user for the Flight Number
    Console.WriteLine("Enter the Flight Number:");
    string flightNumber = Console.ReadLine().Trim();

    // Step 2: Check if the flight exists
    if (!terminal.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight not found. Please try again.");
        return;
    }
    Flight selectedFlight = terminal.Flights[flightNumber];
    // Display basic information of the selected flight
    Console.WriteLine("\nSelected Flight Details:");
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime.ToString("h:mm tt")}");
    Console.WriteLine($"Status: {selectedFlight.Status}");
    // Step 3: Prompt the user for the Boarding Gate
    string gateName;
    while (true)
    {
        Console.WriteLine("\nEnter the Boarding Gate:");
        gateName = Console.ReadLine().Trim();

        // Step 4: Check if the boarding gate exists
        if (!terminal.BoardingGates.ContainsKey(gateName))
        {
            Console.WriteLine("Boarding Gate not found. Please try again.");
            continue;
        }
        BoardingGate selectedGate = terminal.BoardingGates[gateName];
        // Check if the boarding gate is already assigned to another flight
        if (selectedGate.AssignedFlight != null)
        {
            Console.WriteLine($"Boarding Gate {gateName} is already assigned to Flight {selectedGate.AssignedFlight.FlightNumber}. Please choose another gate.");
            continue;
        }
        // Step 5: Assign the boarding gate to the flight
        selectedGate.AssignedFlight = selectedFlight;
        break;
    }
    // Step 6: Display the selected flight and boarding gate
    Console.WriteLine("\nAssignment Details:");
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Boarding Gate: {gateName}");
    // Step 7: Prompt the user to update the flight status
    Console.WriteLine("\nWould you like to update the status of the flight? [Y/N]");
    string updateStatus = Console.ReadLine().Trim().ToUpper();
    if (updateStatus == "Y")
    {
        Console.WriteLine("Select the new status:");
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");
        string statusChoice = Console.ReadLine().Trim();

        switch (statusChoice)
        {
            case "1":
                selectedFlight.Status = "Delayed";
                break;
            case "2":
                selectedFlight.Status = "Boarding";
                break;
            case "3":
                selectedFlight.Status = "On Time";
                break;
            default:
                Console.WriteLine("Invalid choice. Setting status to 'On Time'.");
                selectedFlight.Status = "On Time";
                break;
        }
    }
    else
    {
        selectedFlight.Status = "On Time";
    }   
    Console.WriteLine($"\nBoarding Gate {gateName} has been successfully assigned to Flight {selectedFlight.FlightNumber}.");
}
//Feature #6
static void CreateFlight()
{
    while (true)
    {
        Console.WriteLine("\nEnter the Flight Number:");
        string flightNumber = Console.ReadLine().Trim();
        Console.WriteLine("Enter the Origin:");
        string origin = Console.ReadLine().Trim();
        Console.WriteLine("Enter the Destination:");
        string destination = Console.ReadLine().Trim();
        Console.WriteLine("Enter the Expected Departure/Arrival Time (e.g., 10:00 AM):");
        string timeString = Console.ReadLine().Trim();

        if (!DateTime.TryParseExact(timeString, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expectedTime))
        {
            Console.WriteLine("Invalid time format. Please try again.");
            continue;
        }

        //Prompt the user for additional information (Special Request Code)
        Console.WriteLine("Would you like to enter a Special Request Code? [Y/N]");
        string addSpecialRequest = Console.ReadLine().Trim().ToUpper();

        string specialRequestCode = null;
        if (addSpecialRequest == "Y")
        {
            Console.WriteLine("Enter the Special Request Code:");
            specialRequestCode = Console.ReadLine().Trim();
        }

        Flight newFlight = new Flight(flightNumber, origin, destination, "On Time", expectedTime);
        //Add the Flight object to the Dictionary
        if (terminal.Flights.ContainsKey(flightNumber))
        {
            Console.WriteLine($"Flight {flightNumber} already exists. Please enter a unique Flight Number.");
            continue;
        }
        terminal.Flights[flightNumber] = newFlight;

        //Append the new Flight information to the flights.csv file
        try
        {
            using (StreamWriter writer = new StreamWriter("flights.csv", true))
            {
                string flightLine = $"{flightNumber},{origin},{destination},{expectedTime.ToString("h:mm tt")},On Time";
                if (!string.IsNullOrEmpty(specialRequestCode))
                {
                    flightLine += $",{specialRequestCode}";
                }
                writer.WriteLine(flightLine);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to flights.csv: {ex.Message}");
        }

        Console.WriteLine("\nWould you like to add another flight? [Y/N]");
        string addAnother = Console.ReadLine().Trim().ToUpper();

        if (addAnother != "Y")
        {
            break;
        }
    }
    Console.WriteLine("\nFlight(s) have been successfully added.");
}
// Feature #7
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


