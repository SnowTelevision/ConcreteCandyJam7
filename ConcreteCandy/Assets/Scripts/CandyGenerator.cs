using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyGenerator : MonoBehaviour {

    // Use this for initialization
    public GameObject Candy;
    public int MaxCandy;
    void Start()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        for (int i = 0; i < MaxCandy; ++i)
        {
            GameObject candy = (GameObject)Instantiate(Candy);
           // candy.GetComponent<SpriteRenderer>().color = starColors[i % starColors.Length];
            candy.transform.position = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
            candy.GetComponent<Candies>().speed = (1f * Random.value + 0.5f);
            candy.transform.parent = transform;
        }

    }


    // Update is called once per frame
    void Update () {
		
	}
}
