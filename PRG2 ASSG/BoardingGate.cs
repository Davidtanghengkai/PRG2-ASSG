﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG
{
    public class BoardingGate
    {
        private string gateName;
        private bool supportsCFFT;
        private bool supportsDDJB;
        private bool supportsLWTT;
        private Flight assignedFlight;

        public string GateName { get; set; }

        public bool SupportsCFFT { get; set; }

        public bool SupportsDDJB { get; set; }

        public bool SupportsLWTT { get; set; }

        public Flight Flight { get; set; }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;

        }

        public double CalculateFees()
        {
            return 300; // Base fee for all boarding gates
        }

        public override string ToString()
        {
            return $"Gate {GateName}: CFFT({SupportsCFFT}), DDJB({SupportsDDJB}), LWTT({SupportsLWTT})";
        }


    }
}
