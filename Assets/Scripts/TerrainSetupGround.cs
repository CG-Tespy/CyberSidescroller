using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSetupGround : MonoBehaviour {

    public GameObject groundPrefab;
    public GameObject wallPrefab;
    public GameObject ceilingPrefab;

    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    // Use this for initialization
    void Start()
    {
        MakeGround();//Make Floor
        MakeWall();//Make Wall
        MakeCeiling();//Make Ceiling
        SyncTerrain();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MakeGround()
    {
        GameObject ground =                 Instantiate(groundPrefab) as GameObject;
        ground.transform.SetParent(gameObject.transform, false);//working
        ground.transform.localScale =       new Vector3(spriteRenderer.size.x - 0.001f, 
                                                    0.003f, transform.localScale.z);
        ground.transform.localPosition =    new Vector3(0.0f, (spriteRenderer.size.y / 2) + 0.001f, 
                                                        0.0f);//working


    }
    void MakeWall()
    {
        GameObject ground =                 Instantiate(wallPrefab) as GameObject;
        ground.transform.SetParent(gameObject.transform, false);//working
        ground.transform.localScale =       new Vector3((spriteRenderer.size.x + 0.001f), 
                                                        (spriteRenderer.size.y - 0.03f), 0.0f);//works
        ground.transform.localPosition =    new Vector3(0.0f, 0.0f, 0.0f);//working

    }
    void MakeCeiling()
    {
        GameObject ground =                 Instantiate(ceilingPrefab) as GameObject;
        ground.transform.SetParent(gameObject.transform);//working
        ground.transform.localScale =       new Vector3((spriteRenderer.size.x - 0.001f), 
                                                        0.005f, transform.localScale.z);//works
        ground.transform.localPosition =    new Vector3(0.0f, ((spriteRenderer.size.y / 2) * -1.0f) - 0.001f , 
                                                        0.0f);//working

    }
    //The rest of terrain is just Ceiling so nothing happens

    void SyncTerrain()
    {
        // To make sure that the terrain collides with everything it should, sync the labels and tags.
        GameObject terrain =            null;
        foreach (Transform child in transform)
        {
            terrain =                   child.gameObject;
            terrain.layer =             gameObject.layer;
            //terrain.tag =               gameObject.tag;
        }
    }
}
