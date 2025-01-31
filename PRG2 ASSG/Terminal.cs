using PRG2_ASSG;

public class Terminal
{
    // Initialize airlines and boarding gates dictionaries in constructor
    public Terminal()
    {
        Airlines = new Dictionary<string, Airline>();
        BoardingGates = new Dictionary<string, BoardingGate>();
        Flights = new Dictionary<string, Flight>(); // Initialize flights dictionary
    }

    private string terminalName = "Changi Airport Terminal 5";
    private Dictionary<string, Airline> airlines; // Will be initialized in constructor
    private Dictionary<string, BoardingGate> boardingGates; // Will be initialized in constructor
    private Dictionary<string, double> gateFees = new Dictionary<string, double>();

    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; set; } // Initialized in constructor
    public Dictionary<string, BoardingGate> BoardingGates { get; set; } // Initialized in constructor
    public Dictionary<string, Flight> Flights { get; private set; } // Expose flights dictionary

    public bool AddAirline(Airline airline)
    {
        if (!Airlines.ContainsKey(airline.Code))
        {
            Airlines[airline.Code] = airline;
            return true;
        }
        return false;
    }

    public bool AddBoardingGate(BoardingGate gate)
    {
        if (!BoardingGates.ContainsKey(gate.GateName))
        {
            BoardingGates[gate.GateName] = gate;
            return true;
        }
        return false;
    }

    public bool AddFlight(Flight flight) // Add this method to populate flights
    {
        if (!Flights.ContainsKey(flight.FlightNumber))
        {
            Flights[flight.FlightNumber] = flight;
            return true;
        }
        return false;
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        foreach (var airline in Airlines.Values)
        {
            if (airline.Flights.ContainsKey(flight.FlightNumber))
            {
                return airline;
            }
        }
        return null;
    }
}