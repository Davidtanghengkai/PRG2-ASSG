using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG
{
    public abstract class Flight 
    {
        private string flightNumber;
        private string origin;
        private string destination;
        private DateTime expectedTime;
        private string status;

        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }


        public Flight(string flightNumber, string origin, string destination, string status, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            Status = status;
            ExpectedTime = expectedTime;
        }

        public virtual double CalculateFees()
        {
            return 0;
        }

        public override string ToString()
        {
            return $"Flight {FlightNumber}: {Origin} -> {Destination}, Time: {ExpectedTime}, Status: {Status}";
        }
    }























}
}
