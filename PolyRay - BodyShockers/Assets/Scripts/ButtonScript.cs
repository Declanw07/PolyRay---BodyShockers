using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    void OnTouchDown()
    {

        Debug.Log("Button Touch Down");
//        Gameobject spawnHandler = Gameobject.Find("SpawnHandlerObject");
//        ObjectSpawnScript other = (ObjectSpawnScript)spawnHandler.GetComponent(typeof(ObjectSpawnScript));
//        other.Spawn();

    }

    void OnTouchUp()
    {

        Debug.Log("Button Touch Up");

    }

    void OnTouchStay()
    {

        Debug.Log("Button Touch Stay");

    }

    void OnTouchExit()
    {

        Debug.Log("Button Touch Exit");

    }

}
