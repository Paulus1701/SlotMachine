public class SlotEngine
{
    // SlotEngine rechnet nur und gibt "Zettel / Infos" zurück, ohne irgendeinen Text anzuzeigen

    // Die möglichen Symbole einer Walze
    private string[] symbols = { "7", "$", "X", "A", "K" };
    private System.Random random = new System.Random();
    private int winMultiplier = 10; // Auszahlung = Einsatz * Multiplier

    // Guthaben und Einsatz: von außen lesbar, nur intern änderbar
    public int Balance { get; private set; } = 1000; // aktuelles Guthaben; Von außen sieht sie aus wie ein Feld, aber sie steuert den Zugriff: get; heißt „jeder darf sie lesen", private set; heißt „nur diese Klasse darf sie ändern", = 1000 ist der Startwert.
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
        public bool Played; // false, wenn zu wenig Guthaben
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

        // 9 Zufallssymbole erzeugen (zeilenweise)
        string[] grid = new string[9];
        for (int i = 0; i < 9; i++)
        {
            grid[i] = symbols[random.Next(symbols.Length)];
        }
        result.Grid = grid;

        // Jede Payline prüfen, ob die drei Symbole gleich sind
        int winningLines = 0;
        foreach (int[] line in paylines)
        {
            if (grid[line[0]] == grid[line[1]] && grid[line[1]] == grid[line[2]]) // Eine Payline ist eine Liste von den 3 Nummern, die oben eingeführt wurden new int[] {0, 1, 2}, // obere Reihe. Jede einzelne von ihnen ist ein Gewinn, wenn die Symbole gleich sind. Es fragt also, "sind die drei Symbole auf dieser Linie gleich?" und das für jeder der 5 Linien
            {
                winningLines++;
            }
        }
        result.WinningLines = winningLines;

        // Auszahlung: pro Gewinnlinie Einsatz * Multiplikator
        if (winningLines > 0)
        {
            result.Payout = winningLines * Bet * winMultiplier;
            Balance += result.Payout;
        }

        return result;
    }
}