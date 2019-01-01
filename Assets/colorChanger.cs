using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class colorChanger : MonoBehaviour {
    public Color In0;
    public Color In1;
    public Color Out0;
    public Color Out1;

    Material _mat;

    void OnEnable()
    {
        Shader shader = Shader.Find("Sprites/Default");
        if (_mat == null)
            _mat = new Material(shader);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        _mat.SetColor("_In0", In0);
        _mat.SetColor("_Out0", Out0);
        _mat.SetColor("_In1", In1);
        _mat.SetColor("_Out1", Out1);
        

        Graphics.Blit(src, dst, _mat);
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
