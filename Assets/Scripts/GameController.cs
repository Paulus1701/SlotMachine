using UnityEngine;
using TMPro;
using System.Collections;

public class GameController : MonoBehaviour
{
    // Verweise auf die 9 Symbol-Texte, zeilenweise Reel1..Reel9 (im Inspector zugewiesen)
    [SerializeField] private TMP_Text[] cells;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text betText;
    [SerializeField] private TMP_Text paytableText;
    [SerializeField] private GameObject betPanel;

    // Die Spiel-Logik (kennt Unity nicht)
    private SlotEngine engine = new SlotEngine();

    // true, während die Walzen drehen (verhindert Doppel-Klicks)
    private bool isSpinning = false;

    // Läuft einmal beim Start, setzt die Anzeigen auf die Startwerte
    void Start()
    {
        UpdateUI();
        ShowPaytable();
    }

    // Wird aufgerufen, wenn der SPIN-Button geklickt wird
    public void Spin()
    {
        if (isSpinning) return;          // läuft schon ein Dreh? dann nichts tun
        StartCoroutine(SpinRoutine());   // die Animation starten
    }
    
    // Wird vom Einsatz-Button aufgerufen (Wert im Inspector eingestellt)
    public void SetBet(int amount)
    {
        if (isSpinning) return;   // während eines Drehs nicht ändern
        engine.SetBet(amount);
        UpdateUI();               // neue Einsatz-Anzeige
        betPanel.SetActive(false);// Nach der Auswahl zuklappen 
    }
    
    // Öffnet/schließt die Einsatz-Auswahl
    public void ToggleBetPanel()
    {
        betPanel.SetActive(!betPanel.activeSelf); //Setz das Panel auf das Gegenteil seines jetzigen Zustands; Bei Klick, während es zu ist auf machen und anders rum
    }

    // Der zeitliche Ablauf eines Drehs (Coroutine)
    private IEnumerator SpinRoutine()
    {
        SlotEngine.SpinResult result = engine.Spin(); // Ergebnis steht sofort fest

        // Check, ob genug Guthaben für Einsatz da war
        if (!result.Played)
        {
            winText.text = "Nicht genug Guthaben!";
            yield break;                 // Coroutine sofort beenden
        }

        isSpinning = true;
        winText.text = "";               // alte Meldung weg
        UpdateUI();                      // Einsatz wurde abgezogen -> Guthaben sofort zeigen

        // Walzen rattern lassen: ca. 1 Sekunde lang alle 0,05s zufällige Symbole
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].text = RandomSymbolName();
            }
            yield return new WaitForSeconds(0.05f); // hier 0,05s pausieren, dann weiter
            elapsed += 0.05f;
        }

        // Endergebnis anzeigen
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].text = result.Grid[i];
        }

        // Gewinnprüfung-Ergebnis anzeigen
        // Gewinnprüfung-Ergebnis anzeigen
        string lineWord = result.WinningLines == 1 ? "Linie" : "Linien";
        winText.text = result.WinningLines > 0
            ? "GEWINN! " + result.WinningLines + " " + lineWord + " +" + result.Payout
            : "Kein Gewinn";
        
        engine.Collect(result.Payout); // Gewinn erst JETZT gutschreiben
        UpdateUI();                    // Guthaben jetzt mit Gewinn anzeigen
        isSpinning = false;
    }

    // Ein zufälliges Symbol nur für die Dreh-Animation
    private string RandomSymbolName()
    {
        Symbol[] pt = engine.GetPaytable();
        return pt[Random.Range(0, pt.Length)].Name;
    }

    // Baut die Gewinntabelle aus der Paytable der Engine (eine Quelle)
    private void ShowPaytable()
    {
        string text = "GEWINNTABELLE\n";
        foreach (Symbol s in engine.GetPaytable())
        {
            text += s.Name + "  x" + s.Multiplier + "\n";
        }
        paytableText.text = text;
    }

    // Schreibt Guthaben und Einsatz in die Anzeigen
    private void UpdateUI()
    {
        balanceText.text = "Guthaben: " + engine.Balance;
        betText.text = "Einsatz: " + engine.Bet;
    }
}