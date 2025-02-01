using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG
{
    public class NORMFlight : Flight
    {
        private double fees = 500;

        public double Fees { get; set; }
        public NORMFlight(string FlightNumber, string Origin, string Destination, DateTime ExpectedTime, string Status) : base(FlightNumber, Origin, Destination, ExpectedTime, Status)
        {
            Fees =0;
        }
        public override double  CalculateFees()
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
