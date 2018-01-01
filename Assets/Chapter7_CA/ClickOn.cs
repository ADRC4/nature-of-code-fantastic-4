using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOn : MonoBehaviour {

    [SerializeField]
    private Material white;
    [SerializeField]
    private Material black;

    private MeshRenderer myRend;

    //s;dlfkjg
	// Use this for initialization
	void Start () {
        myRend = GetComponent<MeshRenderer>();
	}
	
	public void ClickMe()
    {
        myRend.material = black;
    }
}
