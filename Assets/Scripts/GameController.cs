using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    //Verweis für die Symbol-Texte der drei Walzen (im Inspector zugewiesen)
    [SerializeField] private TMP_Text reel1Symbol;
    [SerializeField] private TMP_Text reel2Symbol;
    [SerializeField] private TMP_Text reel3Symbol;
    [SerializeField] private TMP_Text winText;
    
    // Die möglichen Symbole einer Walze
    private string[] symbols = { "7", "$", "X", "A", "K" };
    
    // Wird aufgerufen, wenn der SPIN-Button geklickt wird
    public void Spin() 
    {
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
            winText.text = "GEWINN!";
        }
        else
        {
            winText.text = "Kein Gewinn";
        }
    }

}
