public class SlotEngine
{
    // SlotEngine rechnet nur und gibt "Zettel / Infos" zurück, ohne irgendeinen Text anzuzeigen

    // Die Paytable: jedes Symbol mit Häufigkeit (Weight) und Multiplikator. Selten = hoher Multiplikator.
    private Symbol[] paytable = new Symbol[]
    {
        new Symbol("7", 1, 50), // selten, zahlt am meisten
        new Symbol("$", 2, 20),
        new Symbol("X", 3, 10),
        new Symbol("A", 4, 5),
        new Symbol("K", 5, 2),  // häufig, zahlt am wenigsten
    };

    private System.Random random = new System.Random();

    // Guthaben und Einsatz: von außen lesbar, nur intern änderbar
    public int Balance { get; private set; } = 1000; // aktuelles Guthaben
    public int Bet { get; private set; } = 10;       // Einsatz pro Dreh

    // 5 Paylines als Index-Tripel (Index = Reihe*3 + Spalte, zeilenweise)
    private int[][] paylines = new int[][]
    {
        new int[] {0, 1, 2}, // obere Reihe
        new int[] {3, 4, 5}, // mittlere Reihe
        new int[] {6, 7, 8}, // untere Reihe
        new int[] {0, 4, 8}, // Diagonale oben-links -> unten-rechts
        new int[] {6, 4, 2}, // Diagonale unten-links -> oben-rechts
    };

    // Ergebnis eines Drehs, bündelt alles, was die UI danach braucht
    public class SpinResult
    {
        public string[] Grid;      // 9 Symbole, zeilenweise
        public int WinningLines;   // wie viele Linien gewonnen haben
        public int Payout;
        public bool Played;        // false, wenn zu wenig Guthaben
    }

    // Wählt ein Symbol gewichtet aus: höheres Weight = häufiger
    // Zieht ein zufälliges Los aus dem Topf und gibt das Symbol zurück, dem dieses Los gehört
    private Symbol PickRandomSymbol()
    {
        // Alle Gewichte zusammenzählen (hier 1+2+3+4+5 = 15)
        int totalWeight = 0;
        foreach (Symbol s in paytable) totalWeight += s.Weight;

        // Eine Zahl 0..totalWeight-1 ziehen
        int roll = random.Next(totalWeight);

        // Durchlaufen, bis roll in den Bereich eines Symbols fällt
        int cumulative = 0;
        /*
         * roll ist eine zufällige Zahl von 0 bis 14.
         * cumulative erhöht sich in jedem Durchlauf der foreach Schleife um das Gewicht des jeweiligen Symbols (fängt an mit weight = +1 und geht zum Schluss bis weight = +5)
         * Wenn man Symbol 7 haben möchte, muss man muss roll = 0 bekommen, weil wenn man Symbol 7 bekommt, dann ist cumulative = 1 und das einzige, was kleiner ist als 1 ist 0, weshalb es eine Chance von 1/15 hat, weil roll mit 14/15 eine Zahl von 1-14 generiert hätte.
         */
        foreach (Symbol s in paytable)
        {
            cumulative += s.Weight;
            if (roll < cumulative) return s;
        }
        return paytable[paytable.Length - 1]; // Sicherheits-Fallback
    }
    
    // Gibt die Paytable nach außen, damit die UI sie anzeigen kann
    public Symbol[] GetPaytable()
    {
        return paytable;
    }
    
    // Schreibt den Gewinn dem Guthaben gut (von der UI nach der Animation aufgerufen)
    public void Collect(int payout)
    {
        Balance += payout;
    }
    
    // Setzt den Einsatz – aber nur, wenn das Guthaben reicht
    public void SetBet(int amount)
    {
        if (amount <= Balance)
        {
            Bet = amount;
        }
    }

    public SpinResult Spin()
    {
        SpinResult result = new SpinResult();

        // Check, ob genug Guthaben für Einsatz da ist
        if (Balance < Bet)
        {
            result.Played = false;
            return result;
        }

        result.Played = true;
        Balance -= Bet; // Einsatz von Balance abziehen

        // 9 Symbole gewichtet ziehen
        Symbol[] grid = new Symbol[9];
        for (int i = 0; i < 9; i++)
        {
            grid[i] = PickRandomSymbol();
        }

        // Namen fürs Anzeigen in den Zettel schreiben
        result.Grid = new string[9];
        for (int i = 0; i < 9; i++)
        {
            result.Grid[i] = grid[i].Name;
        }

        // Jede Payline prüfen; Auszahlung nach Multiplikator des Symbols
        int winningLines = 0;
        int totalPayout = 0;
        foreach (int[] line in paylines)
        {
            Symbol a = grid[line[0]];
            Symbol b = grid[line[1]];
            Symbol c = grid[line[2]];
            if (a.Name == b.Name && b.Name == c.Name)
            {
                winningLines++;
                totalPayout += Bet * a.Multiplier; // Multiplikator dieses Symbols
            }
        }

        result.WinningLines = winningLines;
        result.Payout = totalPayout;
        // Gewinn wird NICHT sofort gutgeschrieben, sondern erst nach der Animation (Collect); Davor wurde Balance direkt geändert und noch vor Ende der Walzenanimation angezeigt.

        return result;
    }
}