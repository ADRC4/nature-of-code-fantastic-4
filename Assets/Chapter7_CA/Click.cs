using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour {

    [SerializeField]
    private LayerMask clickablesLayer;

    private List<GameObject> selectedObjects;

    private void Start()
    {
        selectedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update () {
		
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, clickablesLayer))
            {
                ClickOn clickOnScript = rayHit.collider.GetComponent<ClickOn>();
                if (Input.GetKey("left ctrl"))
                {
                    if(clickOnScript.currentlySelected == false)
                    {
                        selectedObjects.Add(rayHit.collider.gameObject);
                        clickOnScript.currentlySelected = true;
                        clickOnScript.ClickMe();
                    }
                    else
                    {
                        selectedObjects.Remove(rayHit.collider.gameObject);
                        clickOnScript.currentlySelected = false;
                        clickOnScript.ClickMe();
                    }
                }
                else
                {
                    if(selectedObjects.Count > 0)
                    {
                        foreach (GameObject obj in selectedObjects)
                        {
                            obj.GetComponent<ClickOn>().currentlySelected = false;
                            obj.GetComponent<ClickOn>().ClickMe();
                        }

                        selectedObjects.Clear();
                    }

                    selectedObjects.Add(rayHit.collider.gameObject);
                    clickOnScript.currentlySelected = true;
                    clickOnScript.ClickMe();
                }
                    
                }
            }
        }
	}
}
