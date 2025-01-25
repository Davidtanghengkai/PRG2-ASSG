﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG
{
    public class LWTTFlight : Flight
    {
        private double fees = 500;

        public double Fees { get; set; }
        public LWTTFlight(string FlightNumber, string Origin, string Destination, string Status, DateTime ExpectedTime) : base(FlightNumber, Origin, Destination, Status, ExpectedTime)
        {
            Fees = 500;
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
