using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    //Verweis für die Symbol-Texte der drei Walzen (im Inspector zugewiesen)
    [SerializeField] private TMP_Text reel1Symbol;
    [SerializeField] private TMP_Text reel2Symbol;
    [SerializeField] private TMP_Text reel3Symbol;
    
    // Die möglichen Symbole einer Walze
    private string[] symbols = { "7", "$", "X", "A", "K" };
    
    // Wird aufgerufen, wenn der SPIN-Button geklickt wird
    public void Spin() 
    {
        reel1Symbol.text = symbols[Random.Range(0, symbols.Length)];
        reel2Symbol.text = symbols[Random.Range(0, symbols.Length)];
        reel3Symbol.text = symbols[Random.Range(0, symbols.Length)];
    }

}
