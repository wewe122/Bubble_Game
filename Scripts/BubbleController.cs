using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    // Start is called before the first frame update
    public float BubbleSpeed;//מהירות הבועה לאחר הירי
    public float ShootSpeed;//מהירות ירי הבועה
    public float EnemyTrapedSpeed;//מהירות הבועה אחרי לכידת האויב
    public float TimrToWait;//זמן המתנה ליצירת הבועה
    public int LayerNunberID;//מעביר את הבועה לשכבת הריחוף
    public Vector2 direction;//כיוון הבועה
    public Sprite graybubble;//תמונה חדשה לאבוייקט הבועה
    public Sprite clearbubble;//תמונה חדשה לאבוייקט הבועה
    public Sprite enemyTraped;//תמונה חדשה לאבוייקט הבועה שלוכדת את האויב

    float bubbleOffsetY;//משתנה שיחזיק את גודל הבועה
    float ScreenTopY;//מסך y-נקודה העליונה של
    float ScreenBottomY;//מסך y-נקודה התחתונה של
    float OriginalSpeed;
    Vector2 OriginalScale;
    Rigidbody2D rb2d;
   
    bool IsBubbleOfBundleFinish;
    bool CheckIfAllowUseControllShift;
    Coroutine bubbleOfBundleStop;//IEnumerator אובייקט שיחזיק לי את פעולת 
    Coroutine destroyBubbleOverTimeStop;//IEnumerator אובייקט שיחזיק לי את פעולת
    void Start()
    {
        
        OriginalSpeed = BubbleSpeed;//משתנה לשמירת מהירות הבועה
        OriginalScale = transform.localScale;//וקטור ששמור את גודל הבועה המקורי
        
        bubbleOffsetY = GetComponent<SpriteRenderer>().bounds.extents.y;// y-גודל של חצי מהבועה בציר ב 
        ScreenTopY = Camera.main.ViewportToWorldPoint(new Vector2(0,1)).y;// (y=1) מחזיר את הנקודה מעולם המשחק לעולם האמיתי
        ScreenBottomY = Camera.main.ViewportToWorldPoint(new Vector2(0,0)).y;//(y=0) מחזיר את הנקודה מעולם המשחק לעולם האמיתי

        rb2d = GetComponent<Rigidbody2D> ();
        
        bubbleOfBundleStop = StartCoroutine(BubbleOfBundle());
        StartCoroutine(AnimateBubble());
        destroyBubbleOverTimeStop = StartCoroutine(DestroyBubbleOverTime());
    }
    
    void Update()
    {
        if(transform.position.y > ScreenTopY + bubbleOffsetY)//חצי גודל של הבועה + y בדיקה אם הבועה חרגה מהנקודה העליונה 
        {
            transform.position = new Vector2(transform.position.x, ScreenBottomY - bubbleOffsetY);//במידה וכן חרגה מחזיר אותה לנקודה התחתונה פחות החצי שלה
        }                                                                                         //בשביל שלא תתחיל כל הבועה במסך
        
        if(Input.GetKeyDown(KeyCode.Space) && IsBubbleOfBundleFinish && CheckIfAllowUseControllShift)//נלחץ space-בדיקה אם כפתור ה
        {                                                                                            //ובדיקה אם הבועה נוצרה ואם אפשר לשלוט בכיוון הבועה
             
             ControllPositionBubble();
        }
    }

    void ControllPositionBubble()//ללמעלה space משנה את כיוון הבועה לאחר לחיצת הכפתור
    {
        CheckIfAllowUseControllShift = false;
        this.direction = Vector2.up;

    }
    IEnumerator AnimateBubble()//מייצר אנימציה של הבועה כמו לב פועם
    {
        Vector2 smallScale = OriginalScale / 1.05f;//מקטין את גודל הבועה

        while(true)
        {
            if(IsBubbleOfBundleFinish==true)
            {
                transform.localScale = smallScale;

                yield return new WaitForSeconds(0.2f);

                transform.localScale = OriginalScale;
             
                yield return new WaitForSeconds(0.2f);
            }
            else 
            {
                 yield return null;
            }
            

        }
    }

    IEnumerator BubbleOfBundle()//תהליך היווצרות הבועה
    {                           //הגודל של הבועה יגיע לגודל שנקבע בסוף הלולאה
                                //i=1 : כש
        IsBubbleOfBundleFinish = false;
        BubbleSpeed = ShootSpeed;//מהירות עבור ירי הבועה לפני היווצרותה
        for(int i = 3; i > 0; i--)
        {
            transform.localScale = OriginalScale / i;
            yield return new WaitForSeconds(TimrToWait);
            BubbleSpeed = BubbleSpeed / 2;//הקטנת מהירות הירי
        }

        BubbleSpeed = OriginalSpeed;//החזרת המהירות למהירות המקורית שנקבעה לאחר היווצרות הבועה
        IsBubbleOfBundleFinish = true;//נוצרה הבועה
        CheckIfAllowUseControllShift = true;//רשאי לשנות לה כיוון
        gameObject.layer = LayerNunberID;
       
        
    }

    IEnumerator DestroyBubbleOverTime()// (sprite) מחליף את התמונה  של הבועה עבור  בועה שנוצרה 
    {
        yield return new WaitForSeconds(4f);

        GetComponent<SpriteRenderer>().sprite = graybubble; 

        yield return new WaitForSeconds(4f);

         GetComponent<SpriteRenderer>().sprite = clearbubble;

         yield return new WaitForSeconds(4f);

         DestroyBubble();//לבסוף משמיד את האובייקט

    }

    void FixedUpdate()
    {
        rb2d.velocity = direction * BubbleSpeed;//הוספת מהירות לבועה
    }

    void OnTriggerEnter2D(Collider2D other)//טריגרים שנקבעו עבור הבועה
    {
        if(other.CompareTag("changeDerction"))
        {
            direction = other.GetComponent<ChangDerction>().direction;//שינוי כיוון הבועה לפי מה שנקבע באובייקטים של החצים
        }

        if(other.CompareTag("Player"))
        {
            DestroyBubble();//במידה והבועה פוגעת בשחקן אז אובייקט הבועה נהרס
        }


        if(other.CompareTag("Wall"))
        {
            TermaniteTheCoroutine();//במידה והבועה מתנגשת בקיר מפסיקים את היווצרותה
        }
                                    //במידה והבועה פוגעת באויב אז
        if(other.CompareTag("Enemy"))//(sprite) לוכד את האויב עם תמונה חדשה שמוחלפת באובייקט האוייב
        {
            TermaniteTheCoroutine();
            StopCoroutine(destroyBubbleOverTimeStop);//sprite-מפסיק את פעולת חילוף ה
            GetComponent<SpriteRenderer>().sprite = enemyTraped;//(sprite) שינוי אובייקט האויב לאחר הריסתו לתמונה חדשה 
           
            BubbleSpeed = EnemyTrapedSpeed;//מהירות הבועה משתנה למהירות הבועה שנקבעה עבור הבועה הלוכדת
            gameObject.layer = LayerNunberID;
            gameObject.tag = "Enemy_Trapped";
        }
    
    }

    void TermaniteTheCoroutine()
    {
        StopCoroutine(bubbleOfBundleStop);//הפסקת תהליך היווצרות הבועה
        IsBubbleOfBundleFinish = true; 
        BubbleSpeed = OriginalSpeed;//חוזר למהירות הרגילה
        direction = Vector2.up;//הבועה ממשיכה בכיוון העליון
        transform.localScale = OriginalScale;//מחזיר לגודל המקורית
       
        
    }

    void DestroyBubble()
    {
        Destroy(this.gameObject);//הריסת האובייקט
    }
}
