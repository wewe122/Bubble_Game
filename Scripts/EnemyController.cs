using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public int EnemySpeed; //מהירות האויב
    public Vector2 direction; //כיוון האויב
    public string nextLevelName; // שם השלב הבא
    public static int numOfEnemies; //מס האויבים בשלב זה
    
    private Rigidbody2D rb2d;
    private const float KILLING_VELOCITY = 5.0f;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void OnTriggerEnter2D(Collider2D other)//פונקצייה לבדיקת טריגרים עבור אובייקט האויב
    {

        switch (other.tag)
        {
            case "PointEnd":// שתי נקודות שמסומנות כטריגר שנמצאות
            {               //בסוף כל פלטפורמה במידה והאויב מתנגש באחת מהם הוא משנה כיוון
                Flip();
                break;
            }
            case "Enemy_Trapped"://הריסת אובייקט האויב לאחר פגיעה בבועה
            {
                Destroy(this.gameObject);
                break;
            }
        }

    }
    void OnCollisionEnter2D(Collision2D other)//פונקצייה למפגש של שתי אובייקטים עם רכיבים פיסיקלים
    {
        if (other.collider.tag == "Wrecking_ball")
        {
            if (Mathf.Abs(other.rigidbody.velocity.x) > KILLING_VELOCITY )
            {
                Destroy(this.gameObject);
            }
            else
            {
                Physics2D.IgnoreCollision(other.collider, this.GetComponent<Collider2D>());
            }
        }
    }

    void Flip()
    {
        direction = -direction;//שמירת שינוי כיוון הבועה
        Vector2 newScale = transform.localScale;//וקטור חדש על מנת לשנות את כיוון האויב
        newScale.x = newScale.x * -1;//שינוי הכיוון
        transform.localScale = newScale;//משנה את הכיוון של האויב

    }

    void FixedUpdate()
    {
        rb2d.velocity = new Vector2(direction.x * EnemySpeed, rb2d.velocity.y);//x-הוספה מהירות לאויב בציר ה 
    }

}
