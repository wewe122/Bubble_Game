using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)//פונקצייה למפגש של שתי אובייקטים עם רכיבים פיסיקלים
    {
        if (other.collider.tag == "Player")
            this.GetComponent<Rigidbody2D>().AddForce(other.rigidbody.velocity, ForceMode2D.Impulse);
    }
}
