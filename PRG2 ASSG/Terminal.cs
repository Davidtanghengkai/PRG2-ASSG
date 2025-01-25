using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG
{
    public class Terminal
    {
        private string terminalName;
        private Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
        private Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
        private Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
        private Dictionary<string, double> gateFees = new Dictionary<string, double>();
        private Dictionary<string, Airline> airlines1;

        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get ; set ; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        public Dictionary<string, double> GateFees { get; set; }


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
