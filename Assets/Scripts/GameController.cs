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

    // Die Spiel-Logik (kennt Unity nicht)
    private SlotEngine engine = new SlotEngine();

    // Läuft einmal beim Start, setzt die Anzeigen auf die Startwerte
    void Start()
    {
        UpdateUI();
    }

    // Wird aufgerufen, wenn der SPIN-Button geklickt wird
    public void Spin()
    {
        SlotEngine.SpinResult result = engine.Spin(); // Operator sagt der Engine "dreh!"

        // Check, ob genug Guthaben für Einsatz da war
        if (!result.Played)
        {
            winText.text = "Nicht genug Guthaben!";
            return;
        }

        // Anzeige aktualisieren
        reel1Symbol.text = result.Symbols[0]; // s1
        reel2Symbol.text = result.Symbols[1]; // s2
        reel3Symbol.text = result.Symbols[2]; // s3 

        // Gewinnprüfung-Ergebnis anzeigen
        winText.text = result.IsWin ? "GEWINN! +" + result.Payout : "Kein Gewinn";

        UpdateUI(); // Anzeigen aktualisieren
    }

    // Schreibt Guthaben und Einsatz in die Anzeigen
    private void UpdateUI()
    {
        balanceText.text = "Guthaben: " + engine.Balance;
        betText.text = "Einsatz: " + engine.Bet;
    }
}