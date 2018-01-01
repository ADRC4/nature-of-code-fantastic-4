using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOn : MonoBehaviour {

    [SerializeField]
    private Material white;
    [SerializeField]
    private Material black;

    private MeshRenderer myRend;

    [HideInInspector]
    public bool currentlySelected = false;

	// Use this for initialization
	void Start () {
        myRend = GetComponent<MeshRenderer>();
	}
	
	public void ClickMe()
    {
        if(currentlySelected == false)
        {
            myRend.material = white;
        }
        else
        {
            myRend.material = black;
        }
    }
}
