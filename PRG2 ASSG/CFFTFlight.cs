using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG
{
    internal class CFFTFlight : Flight
    {
        private double fees;

        public double Fees { get; set; }
        public CFFTFlight(string FlightNumber, string Origin, string Destination, DateTime ExpectedTime, string Status) : base(FlightNumber, Origin, Destination,ExpectedTime, Status)
        {
            Fees = 150;
        }
        public override double CalculateFees()
        {
            if (Origin == "Singapore")
            {
                fees += 800 + 300;
            }
            else
            {
                fees += 500 + 300;
            }
            return fees;
        }
    }
}
