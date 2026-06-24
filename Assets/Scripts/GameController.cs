using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    //Verweis für die Symbol-Texte der drei Walzen (im Inspector zugewiesen)
    [SerializeField] private TMP_Text reel1Symbol;
    [SerializeField] private TMP_Text reel2Symbol;
    [SerializeField] private TMP_Text reel3Symbol;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text betText;
    
    // Die möglichen Symbole einer Walze
    private string[] symbols = { "7", "$", "X", "A", "K" };

    private int balance = 1000;     // aktuelles Guthaben
    private int bet = 10;           // Einsatz pro Dreh 
    private int winMultiplier = 10; // Auszahlung = Einsatz * Multiplier
    
    // Läuft einmal beim Start, setzt die Anzeigen auf die Startwerte
    void Start()
    {
        UpdateUI();
    }
    
    // Wird aufgerufen, wenn der SPIN-Button geklickt wird
    public void Spin() 
    {
        // Check, ob genug Guthaben für Einsatz da ist
        if (balance < bet)
        {
            winText.text = "Nicht genug Guthaben!";
            return;
        }
        
        balance -= bet; // Einsatz von Balance abziehen
        
        // Zufalssymbol je Walze, erst in Variablen, damit man vergleichen kann
        
        string s1 = symbols[Random.Range(0, symbols.Length)];
        string s2 = symbols[Random.Range(0, symbols.Length)];
        string s3 = symbols[Random.Range(0, symbols.Length)];
        
        // Anzeige aktualisieren
        reel1Symbol.text = s1;
        reel2Symbol.text = s2;
        reel3Symbol.text = s3;
        
        // Gewinnprüfung, wenn alle drei gleich
        if (s1 == s2 && s2 == s3)
        {
            int payout = bet * winMultiplier;
            balance += payout;
            winText.text = "GEWINN! +" + payout;
        }
        else
        {
            winText.text = "Kein Gewinn";
        }

        UpdateUI(); // Anzeigen aktualisieren
    }
    
    // Schreibt Guthaben und Einsatz in die Anzeigen
    private void UpdateUI()
    {
        balanceText.text = "Guthaben: " + balance;
        betText.text = "Einsatz: " + bet;
    }

}
