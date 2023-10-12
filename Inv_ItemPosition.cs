using UnityEngine;

public class Inv_ItemPosition : MonoBehaviour
{
    //Selezionare la parte del corpo in cui si vuole che l'oggetto appaia
    public enum ItemPos
    {
        Head,
        Spine,
        RightArm
    }
    public ItemPos positon;
}
