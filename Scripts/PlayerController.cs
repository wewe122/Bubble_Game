using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    public int PlayerSpeed;//מהירות השחקן
    public int JumpForce;//משתנה להוספה כח לקפיצהה
    private bool isGround;
    public float RadiusGround;
    public float shootdelay;//משתנה להשהיית הבועה
    public float blinkscount;//כמה היבהובים
    public float timetowaitforblink;//זמן המתנה לכל היבהוב
    
    public Transform playerposition;//מיקום חדש עבור השחקן לאחר שנפסל
    public LayerMask WhatIsGround;//שכבת האדמה שכל הפלטפורמות יושבות תחתיו
    public Transform[] groundPoints;//מערך של שלושה נקודות שנמצאות מתחת לרגליים של השחקן
    public GameObject BubblePrefab;//אובייקט הבועה
    public Transform BubblePrefabPoint;//נקודה התחלתית שהבועה מתחילה את היווצרותה

    
    bool canShoot;
    Rigidbody2D rb2d;
    Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        canShoot=true; //מאתחל את השחקן אם אפשרות לירות
        direction = Vector2.right;//מאתחל את השחקן כשהוא מסובב ימינה
        rb2d = GetComponent<Rigidbody2D> ();
    }
    // Update is called once per frame
    void Update()
    {
        isGround = CheckIsGrounded();//בודק אם השחקן על פלטפורמה כלשהי על מנת שיוכל לקפוץ שוב בעזרת הכפתור חץ למעלה

        if(isGround && Input.GetKeyDown("up"))
        {
            rb2d.AddForce(new Vector2 (0,JumpForce),ForceMode2D.Impulse);//הוספת כח של קפיצה לשחקן
        }

        if(Input.GetKeyDown(KeyCode.LeftControl) && canShoot)//בדיקה של אם השחקן יכול לירות בועה 
        {
            StartCoroutine(ShootBubble());
        }




    }
    void OnCollisionEnter2D(Collision2D other)//פונקצייה למפגש של שתי אובייקטים עם רכיבים פיסיקלים
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            killtheplayer();//הורגים את השחקן כאשר פגע באויב 
        }
    }
   
    void killtheplayer()
    {
        StartCoroutine(bringbacktheplayer());
    }

    IEnumerator bringbacktheplayer()
    {
        canShoot = false;
        this.transform.position = playerposition.position;//מחזירים את השחקן לאחר שנהרס בנקודה אחרת 

        for(int i = 0; i < blinkscount; i++)//יוצרים היבהובים שמדמה כאילו השחקן נפסל
        {
            this.GetComponent<SpriteRenderer>().enabled = false;

            yield return new WaitForSeconds(timetowaitforblink);// ממתינים 0.2 שניות

            this.GetComponent<SpriteRenderer>().enabled = true;

            yield return new WaitForSeconds(timetowaitforblink);
            
        }
        
        canShoot = true;//מחזירים את האפשרות לירי
    }




    IEnumerator ShootBubble()// shootdelay פונקציה שמשהה את פעולת הירי של השחקן עם המשתנה 
    {
        GameObject bubble = Instantiate(BubblePrefab,BubblePrefabPoint.position,Quaternion.identity);//יצירת אובייקט הבועה
        bubble.GetComponent<BubbleController>().direction = direction;//הבועה מקבלת את כיוון הירי שלה לפי כיוון השחקן

        canShoot=false;
        yield return new WaitForSeconds(shootdelay);//השהייה כל 0.4 שניות
        canShoot=true;

    }


    bool CheckIsGrounded()
    {
        for(int i=0; i < groundPoints.Length; i++ )//מערך של שלושה נקודות שנמצאות ברגליים של השחקן
        {
            if(Physics2D.OverlapCircle(groundPoints[i].position,RadiusGround,WhatIsGround))//בדיקה עבור כל נקודה אם לפחות אחת נמצאת על פלטפורמה כלשהי 
            {                                                                              //(ground)שנמצאת תחת שכבת אדמה
                return true;
            }
            
        }
        return false;
    }
    void FixedUpdate()//פונקציה לעדכונים הפיסקלים של המשחק
    {
        float inputX = Input.GetAxis("Horizontal");//x-תזוזת השחקן בציר ה
        rb2d.velocity = new Vector2(inputX * PlayerSpeed, rb2d.velocity.y);//x-הוספת כח לשחקן בציר ה 

        //השחקן נע בין 1- ל1 
        //x-שינוי כיוון של השחקן לפי ציר ה 
        if(inputX < 0)
        {
            direction = Vector2.left;//כשהשחקן רוצה לנוע שמאלה זה בצד השלילי של הציר
        }
        
        else if(inputX > 0)
        {
            direction = Vector2.right;//כשהשחקן רוצה לנוע ימינה זה בצד החיובי של הציר
        }

        transform.localScale = new Vector2(direction.x, transform.localScale.y);
    }
}
