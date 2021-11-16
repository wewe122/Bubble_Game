using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangDerction : MonoBehaviour
{
    public bool isVisible;
    public Vector2 direction;//כל אובייקט של חץ אדום מקבל כיוון שונה על מנת לשנות את כיוון הבועה (למטה,למעלה,ימינה,שמאלה)

    void Awake()
    {
        GetComponent<SpriteRenderer> ().enabled = isVisible;
    }
}
