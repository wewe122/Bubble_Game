using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    private int numOfEnemies;
    private int numOfBubbles;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        numOfBubbles = 0;

    }

    // Update is called once per frame
    void Update()
    {
        numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        numOfBubbles = GameObject.FindGameObjectsWithTag("Enemy_Trapped").Length;

        if(numOfBubbles == 0 && numOfEnemies == 0) 
        {
            StartCoroutine(DisplayCanvas());
        }
    }
    IEnumerator DisplayCanvas()
    {
        yield return new WaitForSeconds(1);
        canvas.SetActive(true);
    }
}
