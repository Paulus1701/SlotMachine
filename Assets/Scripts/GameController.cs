using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    // Verweise auf die 9 Symbol-Texte, zeilenweise Reel1..Reel9 (im Inspector zugewiesen)
    [SerializeField] private TMP_Text[] cells;
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

        // Anzeige aktualisieren: alle 9 Symbole in die 9 Zellen schreiben
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].text = result.Grid[i];
        }

        // Gewinnprüfung-Ergebnis anzeigen
        winText.text = result.WinningLines > 0
            ? "GEWINN! " + result.WinningLines + " Linie(n) +" + result.Payout
            : "Kein Gewinn";

        UpdateUI(); // Anzeigen aktualisieren
    }

    // Schreibt Guthaben und Einsatz in die Anzeigen
    private void UpdateUI()
    {
        balanceText.text = "Guthaben: " + engine.Balance;
        betText.text = "Einsatz: " + engine.Bet;
    }
}