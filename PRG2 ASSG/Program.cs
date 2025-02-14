﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PRG2_ASSG
{
    class Program
    {
        static Terminal terminal = new Terminal();

        static void Main()
        {
            LoadFiles();
            AssignFlightsToBoardingGates();
            while (true)
            {
                Console.WriteLine("\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n8. DIsplay Total Airline Fee\r\n0. Exit\r\nPlease select your option");

                try
                {
                    byte number = Convert.ToByte(Console.ReadLine());
                    if (number == 0) break;
                    else if (number == 1) ListFlights();
                    else if (number == 2) ListAllBoardingGates();
                    else if (number == 3) AssignGate();
                    else if (number == 4) CreateFlight();
                    else if (number == 5) ListAirlines();
                    else if (number == 6) ModifyFlightDetails();
                    else if (number == 7) DisplayScheduledFlights();
                    else if (number == 8) DisplayTotal();
                    else Console.WriteLine("Invalid option. Please try again.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 0 and 8.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 0 and 8.");
                }
            }
        }
        //F1/2 DONE
        static void LoadFiles()
        {
            try
            {
                using (StreamReader sr = new StreamReader("airlines.csv"))
                {
                    string s = sr.ReadLine();
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] airplane = s.Split(',');
                        if (airplane.Length >= 2)
                        {
                            Airline a = new Airline(airplane[0], airplane[1]);
                            terminal.AddAirline(a);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File 'airlines.csv' not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            try
            {
                using (StreamReader sr = new StreamReader("boardinggates.csv"))
                {
                    string x = sr.ReadLine();
                    while ((x = sr.ReadLine()) != null)
                    {
                        string[] gateperm = x.Split(',');
                        if (gateperm.Length >= 4)
                        {
                            BoardingGate b = new BoardingGate(gateperm[0], Convert.ToBoolean(gateperm[1]), Convert.ToBoolean(gateperm[2]), Convert.ToBoolean(gateperm[3]));
                            terminal.AddBoardingGate(b);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File 'boardinggates.csv' not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            try
            {
                using (StreamReader sr = new StreamReader("flights.csv"))
                {
                    string x = sr.ReadLine();
                    while ((x = sr.ReadLine()) != null)
                    {
                        string[] Flights = x.Split(',');
                        if (Flights.Length >= 4)
                        {
                            if (Flights[Flights.Length-1] =="")
                            {
                                NORMFlight c = new NORMFlight(Flights[0], Flights[1], Flights[2], Convert.ToDateTime($"{DateTime.Today.ToString("yyyy-MM-dd")} {Flights[3]}"), "NORM");
                                terminal.AddFlight(c);
                            }
                            else if (Flights[Flights.Length - 1] == "LWTT")
                            {
                                LWTTFlight c = new LWTTFlight(Flights[0], Flights[1], Flights[2], Convert.ToDateTime($"{DateTime.Today.ToString("yyyy-MM-dd")} {Flights[3]}"), "LWTT");
                                terminal.AddFlight(c);
                            }
                            else if (Flights[Flights.Length - 1] == "CFFT")
                            {
                                CFFTFlight c = new CFFTFlight(Flights[0], Flights[1], Flights[2], Convert.ToDateTime($"{DateTime.Today.ToString("yyyy-MM-dd")} {Flights[3]}"), "CFFT");
                                terminal.AddFlight(c);
                            }
                            else if (Flights[Flights.Length - 1] == "DDJB")
                            {
                                DDJBFlight c = new DDJBFlight(Flights[0], Flights[1], Flights[2], Convert.ToDateTime($"{DateTime.Today.ToString("yyyy-MM-dd")} {Flights[3]}"), "DDJB");
                                terminal.AddFlight(c);
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File 'flights.csv' not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
        //F3 DONE
        static void ListFlights()
        {
            Console.WriteLine("=============================================\r\nList of Flights\r\n=============================================");
            Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-20} {"Destination",-20} {"Expected Departure",-5}");

            foreach (var flight in terminal.Flights.Values)
            {
                string Datenow = flight.ExpectedTime.ToString("dd / MM / yyyy h: mm tt");
                Console.WriteLine($"{flight.FlightNumber,-15} {terminal.Airlines[flight.FlightNumber.Substring(0, 2)].Name,-25} {flight.Origin,-20} {flight.Destination,-20} {Datenow,-5}");
            }
        }
        //F4 DONE
        static void ListAllBoardingGates()
        {
            Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================");
            Console.WriteLine($"{"Gate Name",-20} {"DDJB",-15} {"CFFT",-10} {"LWTT",-5}");

            foreach (var gate in terminal.BoardingGates.Values)
            {
                Console.WriteLine($"{gate.GateName,-20} {gate.SupportsDDJB,-15} {gate.SupportsCFFT,-10} {gate.SupportsLWTT,-5}");
            }
        }
        //F5 DONE
        static void AssignGate()
        {
            int retryCount = 0;
            const int maxRetries = 3;

            while (retryCount < maxRetries)
            {
                Console.WriteLine("Enter the Flight Number:");
                string flightNumber = Console.ReadLine().Trim();

                if (!terminal.Flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine("Flight not found. Please try again.");
                    retryCount++;
                    continue;
                }

                Flight selectedFlight = terminal.Flights[flightNumber];
                Console.WriteLine("\nSelected Flight Details:");
                Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
                Console.WriteLine($"Origin: {selectedFlight.Origin}");
                Console.WriteLine($"Destination: {selectedFlight.Destination}");
                Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime.ToString("h:mm tt")}");
                Console.WriteLine($"Status: {selectedFlight.Status}");

                string gateName;
                while (true)
                {
                    Console.WriteLine("\nEnter the Boarding Gate:");
                    gateName = Console.ReadLine().Trim();

                    if (!terminal.BoardingGates.ContainsKey(gateName))
                    {
                        Console.WriteLine("Boarding Gate not found. Please try again.");
                        continue;
                    }

                    BoardingGate selectedGate = terminal.BoardingGates[gateName];
                    if (selectedGate.Flight != null)
                    {
                        Console.WriteLine($"Boarding Gate {gateName} is already assigned to Flight {selectedGate.Flight.FlightNumber}. Please choose another gate.");
                        continue;
                    }

                    selectedGate.Flight = selectedFlight;
                    break;
                }

                Console.WriteLine("\nAssignment Details:");
                Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
                Console.WriteLine($"Boarding Gate: {gateName}");

                Console.WriteLine("\nWould you like to update the status of the flight? [Y/N]");
                string updateStatus = Console.ReadLine().Trim().ToUpper();
                if (updateStatus == "Y")
                {
                    Console.WriteLine("Select the new status:");
                    Console.WriteLine("1. Delayed");
                    Console.WriteLine("2. Boarding");
                    Console.WriteLine("3. On Time");
                    string statusChoice = Console.ReadLine().Trim();

                    switch (statusChoice)
                    {
                        case "1":
                            selectedFlight.Status = "Delayed";
                            break;
                        case "2":
                            selectedFlight.Status = "Boarding";
                            break;
                        case "3":
                            selectedFlight.Status = "On Time";
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Setting status to 'On Time'.");
                            selectedFlight.Status = "On Time";
                            break;
                    }
                }
                else
                {
                    selectedFlight.Status = "On Time";
                }

                Console.WriteLine($"\nBoarding Gate {gateName} has been successfully assigned to Flight {selectedFlight.FlightNumber}.");
                break;
            }

            if (retryCount >= maxRetries)
            {
                Console.WriteLine("Maximum retries reached. Exiting...");
            }
        }

        static void CreateFlight()
        {
            while (true)
            {
                // Input Flight Details
                Console.Write("\nEnter the Flight Number: ");
                string flightNumber = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(flightNumber))
                {
                    Console.WriteLine("Flight Number cannot be empty. Please try again.");
                    continue;
                }

                // Check for Duplicate Flight
                if (terminal.Flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine($"Flight {flightNumber} already exists. Please enter a unique Flight Number.");
                    continue;
                }

                Console.Write("Enter the Origin: ");
                string origin = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(origin))
                {
                    Console.WriteLine("Origin cannot be empty. Please try again.");
                    continue;
                }

                Console.Write("Enter the Destination: ");
                string destination = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(destination))
                {
                    Console.WriteLine("Destination cannot be empty. Please try again.");
                    continue;
                }

                Console.Write("Enter the Expected Departure/Arrival Time (e.g., 10:00 AM):  ");
                string timeString = Console.ReadLine().Trim();
                if (!DateTime.TryParseExact(timeString, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expectedTime))
                {
                    Console.WriteLine("Invalid date and time format. Please try again.");
                    continue;
                }



                // Handle Special Request Code
                Console.WriteLine("Would you like to enter a Special Request Code? [Y/N]");
                string addSpecialRequest = Console.ReadLine().Trim().ToUpper();
                string specialRequestCode = "No code";
                if (addSpecialRequest == "Y")
                {
                    Console.WriteLine("Enter the Special Request Code:");
                    specialRequestCode = Console.ReadLine().Trim();
                }

                // Create Flight Object
                Flight newFlight;
                switch (specialRequestCode)
                {
                    case "LWTT":
                        newFlight = new LWTTFlight(flightNumber, origin, destination, expectedTime, specialRequestCode);
                        break;
                    case "DDJB":
                        newFlight = new DDJBFlight(flightNumber, origin, destination, expectedTime, specialRequestCode);
                        break;
                    case "CFFT":
                        newFlight = new CFFTFlight(flightNumber, origin, destination, expectedTime, specialRequestCode);
                        break;
                    default:
                        newFlight = new NORMFlight(flightNumber, origin, destination, expectedTime, specialRequestCode);
                        break;
                }

                // Add Flight to Terminal
                terminal.Flights[flightNumber] = newFlight;

                // Write Flight to CSV
                try
                {
                    using (StreamWriter writer = new StreamWriter("flights.csv", true))
                    {
                        string flightLine = $"{flightNumber},{origin},{destination},{expectedTime.ToString("h:mm tt")}";
                        if (specialRequestCode != "No code")
                        {
                            flightLine += $",{specialRequestCode}";
                        }
                        writer.WriteLine(flightLine);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to flights.csv: {ex.Message}");
                }

                // Ask to Add Another Flight
                Console.WriteLine("\nWould you like to add another flight? [Y/N]");
                string addAnother = Console.ReadLine().Trim().ToUpper();
                if (addAnother == "N")
                {
                    break;
                }
            }

            Console.WriteLine("\nFlight(s) have been successfully added.");
        }

        static void ListAirlines()
        {
            Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================\r\nAirline Code Airline Name\r\nSQ Singapore Airlines\r\nMH Malaysia Airlines\r\nJL Japan Airlines\r\nCX Cathay Pacific\r\nQF Qantas Airways\r\nTR AirAsia\r\nEK Emirates\r\nBA British Airways\r\nEnter Airline Code: ");
            string code = Console.ReadLine();
            Console.WriteLine($"\r\n=============================================\r\nList of Flights for {terminal.Airlines[code].Name}\r\n=============================================");
            Console.WriteLine($"{"Flight Number",-25} {"Airline Name",-20} {"Origin",-15} {"Destination",-15} {"Expected",-5}");

            foreach (var airline in terminal.Flights.Values)
            {
                if (airline.FlightNumber.Substring(0, 2) == code)
                {
                    Console.WriteLine($"{airline.FlightNumber,-25} {terminal.Airlines[airline.FlightNumber.Substring(0, 2)].Name,-20} {airline.Origin,-15} {airline.Destination,-15} {airline.ExpectedTime,-5} ");
                }
            }
        }

        static void ModifyFlightDetails()
        {
            Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================");

            // List all available airlines
            foreach (var airline in terminal.Airlines.Values)
            {
                Console.WriteLine($"Airline Code {airline.Code}, Airline Name {airline.Name}");
            }

            string airlineCode;
            // Input validation for airline code

            Console.Write("Enter Airline Code: ");
            airlineCode = Console.ReadLine().Trim().ToUpper();
            Airline selectedAirline = terminal.Airlines[airlineCode];
            Console.WriteLine($"=============================================\r\nList of Flights for {selectedAirline.Name}\r\n=============================================");
            Console.WriteLine($"{"Flight Number",-15} {"Origin",-15} {"Destination",-15} {"Expected Departure/Arrival Time",-30}");
            foreach (var airline in terminal.Flights.Values)
            {
                if (airline.FlightNumber.Substring(0, 2) == airlineCode)
                {
                    Console.WriteLine($"{airline.FlightNumber,-25} {airline.Origin,-20} {airline.Destination,-15} {airline.ExpectedTime,-10} {airline.Status,-5}");

                }

            }





            // List flights for the selected airline
            foreach (var flight in terminal.Flights.Values.Where(f =>
            {
                var airline = terminal.GetAirlineFromFlight(f);
                return airline != null && airline.Code == airlineCode;
            }))
            {
                Console.WriteLine($"{flight.FlightNumber,-15} {flight.Origin,-15} {flight.Destination,-15} {flight.ExpectedTime,-30}");
            }


            string flightNumber;
            // Input validation for selecting a flight to modify or delete
            while (true)
            {
                Console.Write("Choose an existing Flight to modify or delete: ");
                flightNumber = Console.ReadLine().ToUpper();
                if (string.IsNullOrEmpty(flightNumber) || !terminal.Flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine("Invalid Flight Number. Please try again.");
                }
                else
                {
                    break;
                }
            }

            Flight selectedFlight = terminal.Flights[flightNumber];

            string option;
            // Menu to choose whether to modify or delete
            while (true)
            {
                Console.WriteLine("1. Modify Flight");
                Console.WriteLine("2. Delete Flight");
                Console.Write("Choose an option: ");
                option = Console.ReadLine();
                if (option != "1" && option != "2")
                {
                    Console.WriteLine("Invalid option. Please choose 1 or 2.");
                }
                else
                {
                    break;
                }
            }

            if (option == "1")
            {
                string modifyOption;
                // Choose which aspect of the flight to modify
                while (true)
                {
                    Console.WriteLine("1. Modify Basic Information");
                    Console.WriteLine("2. Modify Status");
                    Console.WriteLine("3. Modify Special Request Code");
                    Console.WriteLine("4. Modify Boarding Gate");
                    Console.Write("Choose an option: ");
                    modifyOption = Console.ReadLine();
                    if (modifyOption != "1" && modifyOption != "2" && modifyOption != "3" && modifyOption != "4")
                    {
                        Console.WriteLine("Invalid option. Please choose 1, 2, 3, or 4.");
                    }
                    else
                    {
                        break;
                    }
                }

                // Modify basic flight information (Origin, Destination, Expected Time)
                if (modifyOption == "1")
                {
                    Console.Write("Enter new Origin: ");
                    selectedFlight.Origin = Console.ReadLine();
                    Console.Write("Enter new Destination: ");
                    selectedFlight.Destination = Console.ReadLine();
                    while (true)
                    {
                        Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newExpectedTime))
                        {
                            selectedFlight.ExpectedTime = newExpectedTime;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format. Please try again.");
                        }
                    }
                }
                // Modify the status of the flight
                else if (modifyOption == "2")
                {
                    while (true)
                    {


                        Console.WriteLine("Select the new status:");
                        Console.WriteLine("1. Delayed");
                        Console.WriteLine("2. Boarding");
                        Console.WriteLine("3. On Time");
                        string statusChoice = Console.ReadLine().Trim();



                        if (statusChoice == "1")
                        {
                            selectedFlight.Status = "Delayed";
                            break;
                        }
                        else if (statusChoice == "2")
                        {
                            selectedFlight.Status = "Boarding";
                            break;
                        }
                        else if (statusChoice == "3")
                        {
                            selectedFlight.Status = "On Time";
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input. Choose 1,2 or 3");
                        }




                    }
                }


                // Modify the special request code (handling different types of flights)
                else if (modifyOption == "3")
                {
                    Console.Write("Enter new Special Request Code: ");
                    string specialRequestCode = Console.ReadLine();
                    if (specialRequestCode == "CFFT")
                    {
                        selectedFlight = new CFFTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status);
                    }
                    else if (specialRequestCode == "DDJB")
                    {
                        selectedFlight = new DDJBFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status);
                    }
                    else if (specialRequestCode == "LWTT")
                    {
                        selectedFlight = new LWTTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status);
                    }
                    else
                    {
                        selectedFlight = new NORMFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status);
                    }
                    terminal.Flights[flightNumber] = selectedFlight;
                }
                // Modify the boarding gate for the flight
                else if (modifyOption == "4")
                {
                    while (true)
                    {
                        Console.Write("Enter new Boarding Gate: ");
                        string gateName = Console.ReadLine().ToUpper();

                        if (string.IsNullOrEmpty(gateName) || !terminal.BoardingGates.ContainsKey(gateName))
                        {
                            Console.WriteLine("Invalid Boarding Gate. Please try again.");
                        }
                        else
                        {
                            // Assuming you have a property named "GateName" in BoardingGate.
                            BoardingGate selectedGate = terminal.BoardingGates[gateName];

                            // Check if Flight property exists on BoardingGate
                            if (selectedGate != null)
                            {
                                // Set the Flight for the BoardingGate
                                selectedGate.Flight = selectedFlight;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Boarding Gate not found.");
                            }
                        }
                    }
                }

                Console.WriteLine("Flight updated!");
            }
            else if (option == "2")
            {
                // Confirm before deleting the flight
                while (true)
                {
                    Console.Write("Are you sure you want to delete this flight? (Y/N): ");
                    string confirm = Console.ReadLine().ToUpper();
                    if (confirm == "Y")
                    {
                        terminal.Flights.Remove(flightNumber);
                        Console.WriteLine("Flight deleted!");
                        return;
                    }
                    else if (confirm == "N")
                    {
                        Console.WriteLine("Flight deletion canceled.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter Y or N.");
                    }
                }
            }

            // Display updated flight details
            Console.WriteLine("=============================================\r\nUpdated Flight Details\r\n=============================================");
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Airline Name: {selectedAirline.Name}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Destination: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime}");
            Console.WriteLine($"Status: {selectedFlight.Status}");
            Console.WriteLine($"Special Request Code: {(selectedFlight is CFFTFlight ? "CFFT" : selectedFlight is DDJBFlight ? "DDJB" : selectedFlight is LWTTFlight ? "LWTT" : "None")}");
            BoardingGate? gate = terminal.BoardingGates.Values.FirstOrDefault(g => g.Flight == selectedFlight);
            Console.WriteLine($"Boarding Gate: {(gate != null ? gate.GateName : "None")}");
        }



        static void DisplayScheduledFlights()
        {
            var sortedFlights = terminal.Flights.Values.OrderBy(flight => flight.ExpectedTime).ToList();
            Console.WriteLine("=============================================");
            Console.WriteLine("Scheduled Flights (Ordered by Earliest First)");
            Console.WriteLine("=============================================");
            Console.WriteLine($"{"Flight Number",-15} {"Airline",-20} {"Origin",-20} {"Destination",-20} {"Expected Time",-15} {"Status",-10} {"Boarding Gate",-15}");
            Console.WriteLine(new string('-', 120));

            foreach (var flight in sortedFlights)
            {
                string airlineCode = flight.FlightNumber.Substring(0, 2);
                string airlineName = terminal.Airlines.ContainsKey(airlineCode) ? terminal.Airlines[airlineCode].Name : "Unknown Airline";
                string boardingGate = "Unassigned";

                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                    {
                        boardingGate = gate.GateName;
                        break;
                    }
                }

                Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime.ToString("h:mm tt"),-15} {flight.Status,-10} {boardingGate,-15}");
            }
        }
        static void AssignFlightsToBoardingGates()
        {
            int unassigned = 0;
            int total = terminal.Flights.Count;
            Queue<Flight> unassignedFlights = new Queue<Flight>();
            Dictionary<string,Flight> printall = new Dictionary<string, Flight>();
            Dictionary<Flight,string> opv = new Dictionary<Flight,string>();
            List<BoardingGate> boardedgates = new List<BoardingGate>();

            // Separate assigned and unassigned flights

            foreach (var flight in terminal.Flights.Values)
            {
                bool counter = false;
                foreach (var board in terminal.BoardingGates.Values)
                {
                    if (board.Flight != null)
                    {
                        if (board.Flight == flight)
                        {
                            counter= true;
                            boardedgates.Add(board);
                            printall.Add(board.GateName, flight);
                            opv.Add(flight, board.GateName);
                            
                        }
    
                    }
                }
                if (counter == false)
                {
                    unassignedFlights.Enqueue(flight);
                    unassigned++;
                }
            }
            int x = unassignedFlights.Count;
            for (int i = 0; i < x; i++)
            {
                var flight = unassignedFlights.Dequeue();              
                
                if (flight.Status == "CFFT")
                {
                    foreach (var item in terminal.BoardingGates.Values)
                    {
                        if (boardedgates.Contains(item) == false)
                        {
                            if (item.SupportsCFFT == true)
                            {
                                item.Flight = flight;
                                boardedgates.Add(item);

                                try  
                                { 
                                    printall.Add(item.GateName, flight);
                                    opv.Add(flight, item.GateName); 
                                }
                                catch { }
                                break;
                            }
                        }
                        
                    }
                }
                else if (flight.Status == "LWTT")
                {
                    foreach (var item in terminal.BoardingGates.Values)
                    {
                        if (boardedgates.Contains(item) == false)
                        {
                            if (item.SupportsLWTT == true)
                            {
                                item.Flight = flight;
                                boardedgates.Add(item);
                                try
                                {
                                    printall.Add(item.GateName, flight);
                                    opv.Add(flight, item.GateName);
                                }
                                catch { }
                                break;
                            }
                        }
                    }
                }
                else if (flight.Status == "DDJB")
                {
                    foreach (var item in terminal.BoardingGates.Values)
                    {
                        if (boardedgates.Contains(item) == false)
                        {
                            if (item.SupportsDDJB == true)
                            {
                                item.Flight = flight;
                                boardedgates.Add(item);
                                try
                                {
                                    printall.Add(item.GateName, flight);
                                    opv.Add(flight, item.GateName);
                                }
                                catch { }
                                break;
                                
                            }
                        }
                    }
                }
                else 
                {
                    foreach (var item in terminal.BoardingGates.Values)
                    {
                        if (boardedgates.Contains(item) == false)
                        {
                            item.Flight = flight;
                            boardedgates.Add(item);
                            try
                            {
                                printall.Add(item.GateName, flight);
                                opv.Add(flight, item.GateName);
                            }
                            catch { }
                            break;

                        }
                    }
                }

                
                    
                

            }
            int counters = 0;
            Console.WriteLine($"{"Flight Number",-15} {"Origin",-20} {"Destination",-20} {"Expected Departure/Arrival time",-35} {"Special Request Code", - 25} {"Boarding Gate"}");
            foreach (var test in printall.Values)
            {
                Console.WriteLine($"{test.FlightNumber,-15} {test.Origin,-20} {test.Destination,-20} {test.ExpectedTime,-35} {test.Status,-25} {opv[test]}");
                counters++;
            }


        }


        static void DisplayTotal()
        {
    // Check if all flights have been assigned boarding gates
    bool allFlightsAssigned = true;
    foreach (var flight in terminal.Flights.Values)
    {
        bool isAssigned = false;
        foreach (var gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
            {
                isAssigned = true;
                break;
            }
        }
        if (!isAssigned)
        {
            allFlightsAssigned = false;
            break;
        }
    }

    if (!allFlightsAssigned)
    {
        Console.WriteLine("Not all flights have been assigned boarding gates. Please ensure all flights are assigned before running this feature.");
        return;
    }

    // Dictionary to store the total fees and discounts per airline
    Dictionary<string, (decimal subtotalFees, decimal subtotalDiscounts)> airlineTotals = new Dictionary<string, (decimal, decimal)>();

    // Iterate through each airline
    foreach (var airline in terminal.Airlines.Values)
    {
        decimal subtotalFees = 0;
        decimal subtotalDiscounts = 0;
        int flightCount = 0;
        int earlyLateFlights = 0;
        int specificOriginFlights = 0;
        int noSpecialRequestFlights = 0;

        // Retrieve all flights for the current airline
        var airlineFlights = terminal.Flights.Values.Where(f => f.FlightNumber.Substring(0, 2) == airline.Code).ToList();

        foreach (var flight in airlineFlights)
        {
            decimal flightFee = 0;

            // Check if the flight is arriving or departing
            if (flight.Destination == "SIN")
            {
                flightFee += 500; // Arriving flight fee
            }
            else if (flight.Origin == "SIN")
            {
                flightFee += 800; // Departing flight fee
            }

            // Add boarding gate base fee
            flightFee += 300;

            // Add special request code fee
            if (flight.Status == "DDJB")
            {
                flightFee += 300;
            }
            else if (flight.Status == "CFFT")
            {
                flightFee += 150;
            }
            else if (flight.Status == "LWTT")
            {
                flightFee += 500;
            }

            // Count flights for discounts
            flightCount++;

            // Check for early/late flights
            if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
            {
                earlyLateFlights++;
            }

            // Check for specific origin flights
            if (flight.Origin == "DXB" || flight.Origin == "BKK" || flight.Origin == "NRT")
            {
                specificOriginFlights++;
            }

            // Check for flights with no special request code
            if (flight.Status == "NORM")
            {
                noSpecialRequestFlights++;
            }

            subtotalFees += flightFee;
        }

        // Calculate discounts
        // Discount for every 3 flights
        subtotalDiscounts += (flightCount / 3) * 350;

        // Discount for early/late flights
        subtotalDiscounts += earlyLateFlights * 110;

        // Discount for specific origin flights
        subtotalDiscounts += specificOriginFlights * 25;

        // Discount for flights with no special request code
        subtotalDiscounts += noSpecialRequestFlights * 50;

        // Additional discount for airlines with more than 5 flights
        if (flightCount > 5)
        {
            subtotalDiscounts += subtotalFees * 0.03m;
        }

        // Store the subtotal fees and discounts for the airline
        airlineTotals[airline.Code] = (subtotalFees, subtotalDiscounts);
    }

    // Display the total fees and discounts per airline
    Console.WriteLine("=============================================");
    Console.WriteLine("Total Fees and Discounts per Airline");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Code",-15} {"Subtotal Fees",-15} {"Subtotal Discounts",-15} {"Final Total",-15}");
    Console.WriteLine(new string('-', 60));

    decimal totalFeesAllAirlines = 0;
    decimal totalDiscountsAllAirlines = 0;

    foreach (var airlineCode in airlineTotals.Keys)
    {
        var (subtotalFees, subtotalDiscounts) = airlineTotals[airlineCode];
        decimal finalTotal = subtotalFees - subtotalDiscounts;

        Console.WriteLine($"{airlineCode,-15} {subtotalFees,-15:C2} {subtotalDiscounts,-15:C2} {finalTotal,-15:C2}");

        totalFeesAllAirlines += subtotalFees;
        totalDiscountsAllAirlines += subtotalDiscounts;
    }

    // Display the overall totals
    Console.WriteLine(new string('-', 60));
    Console.WriteLine($"{"Total",-15} {totalFeesAllAirlines,-15:C2} {totalDiscountsAllAirlines,-15:C2} {totalFeesAllAirlines - totalDiscountsAllAirlines,-15:C2}");
    Console.WriteLine(new string('-', 60));

    // Calculate and display the percentage of discounts over the final total
    decimal finalTotalAllAirlines = totalFeesAllAirlines - totalDiscountsAllAirlines;
    decimal discountPercentage = (totalDiscountsAllAirlines / finalTotalAllAirlines) * 100;

    Console.WriteLine($"Percentage of discounts over final total: {discountPercentage:F2}%");
        }
}
}
