using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inv_Inventory : MonoBehaviour
{       
    // Un elenco con tutti i pulsanti dell'inventario
    [SerializeField] List<Button> buttons = new List<Button>();   
    // Tutti gli oggetti della cartella Risorse
    [SerializeField] List<GameObject> resourceItems = new List<GameObject>();
    [SerializeField] GameObject buttonsPath;
    // I nomi degli oggetti raccolti
    [SerializeField] List<string> inventoryItems = new List<string>();
    // L'oggetto attualmente equipaggiato
    GameObject itemInArm;
    // Il punto di spawn dell'oggetto
    [SerializeField] Transform itemPoint;
    [SerializeField] Transform[] itemPositions;
    // Un campo di testo per i messaggi di avviso dell'inventario (Text)
    [SerializeField] TMP_Text warning;
    // L'elenco degli oggetti raccolti dal giocatore
    [SerializeField] List<GameObject> playerItems = new List<GameObject>();
    GameObject itemPosition;


    private void Start()
    {
        // Caricamento di tutti gli oggetti dell'inventario presenti nella cartella Risorse
        GameObject[] objArr = Resources.LoadAll<GameObject>("Items");
        
        // Compilazione dell'elenco dei possibili articoli d'inventario
        resourceItems.AddRange(objArr);
        // Esaminare tutti i pulsanti dell'inventario e memorizzarli nell'elenco.
        foreach(Transform child in buttonsPath.transform)
        {
            buttons.Add(child.GetComponent<Button>());
        }
    }
    private void Update()
    {
        // Abilitazione/disabilitazione del cursore del mouse ogni volta che il giocatore preme Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void AddItem(Sprite img, string itemName, GameObject obj)
    {        
        // Se l'inventario è pieno, viene emesso un messaggio di avvertimento e il metodo viene interrotto.
        if (inventoryItems.Count >= buttons.Count)
        {
            warning.text = "Full Inventory!";
            Invoke("WarningUpdate", 1f);
            return;
        }
        // Se il giocatore ha già una copia dell'oggetto, viene emesso un messaggio di avvertimento e il metodo viene interrotto.
        if (inventoryItems.Contains(itemName))
        {
            warning.text = "You already have " + itemName;
            Invoke("WarningUpdate", 1f);
            return;
        }
        // Aggiunta del nome dell'articolo all'inventario
        inventoryItems.Add(itemName);
        // Ottenere il pulsante libero successivo e il suo componente Immagine
        var buttonImage = buttons[inventoryItems.Count - 1].GetComponent<Image>();
        // Impostazione dell'immagine del pulsante con l'immagine dell'oggetto raccolto
        buttonImage.sprite = img;
        // Distruggere l'oggetto raccolto
        Destroy(obj);
    }
    // Un metodo che cancella tutti i messaggi di avviso
    void WarningUpdate()
    {
        warning.text = "";
    }
    // Questo metodo viene richiamato ogni volta che viene premuto un pulsante
    public void UseItem(int itemPos)
    {           
        // Se si preme un pulsante a cui non è collegato alcun elemento, la funzione viene interrotta.
        if (inventoryItems.Count <= itemPos) return;
        // Memorizzazione del nome dell'oggetto collegato a questo pulsante in una variabile
        string item = inventoryItems[itemPos];
        // Richiamo del metodo che equipaggia l'oggetto dall'inventario e passaggio del nome dell'oggetto come inventario
        GetItemFromInventory(item);
    }
    // Questo metodo equipaggia l'oggetto. Viene richiamato dal metodo UseItem 
    public void GetItemFromInventory(string itemName)
    {
        // Ricerca dell'oggetto con il nome specificato nell'elenco di tutti gli oggetti
        var resourceItem = resourceItems.Find(x => x.name == itemName);
        // Se l'oggetto con tale nome non esiste nella cartella delle risorse, la funzione viene interrotta.
        if (resourceItem == null) return;

        //  Cercare l'oggetto con il nome specificato nell'elenco degli oggetti dei giocatori
        var putFind = playerItems.Find(x => x.name == itemName);

        // Se tale elemento non esiste, allora
        if (putFind == null)
        {
            // Se il giocatore ha già un oggetto equipaggiato, lo disabilitiamo.
            if (itemInArm != null)
            {
                itemInArm.SetActive(false);
            }
            // Verifica della parte del corpo a cui l'oggetto deve essere collegato
            var pos = resourceItem.GetComponent<Inv_ItemPosition>().positon;
            if (pos == Inv_ItemPosition.ItemPos.Head)
            {
                itemPoint.position = itemPositions[0].position;
                itemPosition = itemPositions[0].gameObject;
            }
            else if (pos == Inv_ItemPosition.ItemPos.Spine)
            {
                itemPoint.position = itemPositions[1].position;
                itemPosition = itemPositions[1].gameObject;
            }
            else
            {
                itemPoint.position = itemPositions[2].position;
                itemPosition = itemPositions[2].gameObject;
            }
            // Creazione dell'oggetto nel punto precedentemente definito
            var newItem = Instantiate(resourceItem, itemPoint);
            // Spostamento di questo oggetto nella Gerarchia del giocatore 
            newItem.transform.parent = itemPosition.transform;
            // Assegnare un nome al nuovo oggetto
            newItem.name = itemName;
            // Aggiunta dell'oggetto all'elenco degli oggetti presenti nell'inventario del giocatore
            playerItems.Add(newItem);
            // Dire all'unità che l'inventario itemInArm è uguale all'oggetto appena equipaggiatoТ (in altre parole, si ricorda l'oggetto attualmente equipaggiato)
            itemInArm = newItem;
        }
        // Se questo elemento esiste già nella scena, allora
        else
        {
            // Se questo oggetto è l'oggetto già equipaggiato, è sufficiente cambiare il suo stato
            {
                putFind.SetActive(!putFind.activeSelf);
            }
            // Se si tratta di un altro oggetto, si disabilita semplicemente l'oggetto attualmente equipaggiato e si abilita quello nuovo
            else
            {
                itemInArm.SetActive(false);
                putFind.SetActive(true);
                itemInArm = putFind;
            }
        }
    }
}
