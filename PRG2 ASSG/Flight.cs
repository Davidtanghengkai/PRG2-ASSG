using System;

namespace PRG2_ASSG
{
    public class Flight
    {
        // Properties
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        // Constructor to initialize the properties
        public Flight(string flightNumber, string origin, string destination, string status, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            Status = status;
            ExpectedTime = expectedTime;
        }

        // Method to calculate fees, which can be overridden in derived classes
        public virtual double CalculateFees()
        {
            return 0;  // Default implementation (you can change it in derived classes)
        }

        // ToString method for printing flight details
        public override string ToString()
        {
            return $"Flight {FlightNumber}, Origin {Origin}, Destination {Destination}, Time {ExpectedTime}, Status {Status}";
        }
    }
}
