


using PRG2_ASSG;

static void loadfiles()
{
     List<Airline> Airplanelist = new List<Airline>();
    using (StreamReader sr = new StreamReader("airplanes.csv"))
    {
        string s = sr.ReadLine();

        while ((s = sr.ReadLine()) != null)
        {
            string[] airplane = s.Split(',');
            
            Airplanelist.Add(new Airline(airplane[0], airplane[1]));
        }
        
    }

    List<BoardingGate> BoardingGateList = new List<BoardingGate>();
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        string x = sr.ReadLine();

        while ((x = sr.ReadLine()) != null)
        {
            string[] gateperm = x.Split(',');
            
            BoardingGateList.Add(new BoardingGate(gateperm[0], Convert.ToBoolean(gateperm[1]), Convert.ToBoolean(gateperm[2]), Convert.ToBoolean(gateperm[3])));
        }

    }
}