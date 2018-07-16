using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateConent : MonoBehaviour {

    public GameObject prefab;
    public static string filterItems;
    public Sprite[] prefabList;

    public int numberToCreate;
    // Use this for initialization
    void Start () {
        Populate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Populate()
    {
        GameObject newObj;
        for(int i = 0; i < numberToCreate; i++)
        {
            //prefab.getc<Image>().sprite = prefabList[i];
            newObj = (GameObject)Instantiate(prefab, transform);
        }
    }
}
