public class Symbol
{
    public string Name;     // das angezeigte Zeichen, z.B. "7"
    public int Weight;      // höher = kommt häufiger vor
    public int Multiplier;  // Auszahlung pro Gewinnlinie = Einsatz * Multiplier

    public Symbol(string name, int weight, int multiplier)
    {
        Name = name;
        Weight = weight;
        Multiplier = multiplier;
    }
}