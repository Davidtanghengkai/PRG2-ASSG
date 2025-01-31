using PRG2_ASSG;

public class Terminal
{
    // Initialize airlines and boarding gates dictionaries in constructor
    public Terminal()
    {
        Airlines = new Dictionary<string, Airline>();
        BoardingGates = new Dictionary<string, BoardingGate>();
    }

    private string terminalName = "Changi Airport Terminal 5";
    private Dictionary<string, Airline> airlines; // Will be initialized in constructor
    private Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
    private Dictionary<string, BoardingGate> boardingGates; // Will be initialized in constructor
    private Dictionary<string, double> gateFees = new Dictionary<string, double>();

    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; set; } // Initialized in constructor
    public Dictionary<string, BoardingGate> BoardingGates { get; set; } // Initialized in constructor

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
