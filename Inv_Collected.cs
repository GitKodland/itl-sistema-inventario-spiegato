using UnityEngine;

public class Inv_Collected : MonoBehaviour
{
    // Il nome dell'oggetto 
    public string name;
    // L'immagine (sprite) che sarà mostrata nell'inventario
    public Sprite image;
    // Un riferimento allo script dell'inventario
    private Inv_Inventory inventory;

    private void Start()
    {
        // Cercare un oggetto con lo script dell'inventario e memorizzarlo in una variabile
        inventory = FindObjectOfType<Inv_Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {                 
        // Ogni volta che l'oggetto viene prelevato, si richiama il metodo AddItem dallo script dell'inventario, passando il metodo
        // lo sprite dell'oggetto, il nome e l'oggetto raccolto dal giocatore
        inventory.AddItem(image, name, gameObject);
    }
}
