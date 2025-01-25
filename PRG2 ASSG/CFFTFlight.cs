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
        public CFFTFlight(string FlightNumber, string Origin, string Destination, string Status, DateTime ExpectedTime) : base(FlightNumber, Origin, Destination, Status, ExpectedTime)
        {
            Fees = 150;
        }
        public virtual double CalculateFees()
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
