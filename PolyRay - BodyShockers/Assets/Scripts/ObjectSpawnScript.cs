using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectSpawnScript : MonoBehaviour {

    public GameObject characterPrefab;
    public Transform spawnPos;
    public int characterCount;
    private Text txtRef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTouchDown()
    {
    //    Debug.Log("Spawning Character Prefab");
    //    Spawn();

    }

    void OnTouchStay()
    {
        Debug.Log("Spawning Prefab");
        Spawn();

    }

    public void Spawn()
    {

        GameObject character = (GameObject)Instantiate(characterPrefab, spawnPos.position, Random.rotation);
        characterCount++;
        txtRef = GameObject.Find("CountText").GetComponent<Text>();
        txtRef.text = "Character Count: " + characterCount;

 //       Color randColor = new Color(Random.value, Random.value, Random.value, 1.0f);
 //       character.GetComponent<Renderer>().material.color = randColor;

    }
}
