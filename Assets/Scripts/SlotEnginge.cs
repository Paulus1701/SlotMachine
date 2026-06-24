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

    // Ergebnis eines Drehs, bündelt alles, was die UI danach braucht
    public class SpinResult
    {
        public string[] Symbols;
        public bool IsWin;
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

        // Zufalssymbol je Walze, erst in Variablen, damit man vergleichen kann
        string s1 = symbols[random.Next(symbols.Length)];
        string s2 = symbols[random.Next(symbols.Length)];
        string s3 = symbols[random.Next(symbols.Length)];
        result.Symbols = new string[] { s1, s2, s3 };

        // Gewinnprüfung, wenn alle drei gleich
        if (s1 == s2 && s2 == s3)
        {
            result.IsWin = true;
            result.Payout = Bet * winMultiplier;
            Balance += result.Payout;
        }

        return result;
    }
}