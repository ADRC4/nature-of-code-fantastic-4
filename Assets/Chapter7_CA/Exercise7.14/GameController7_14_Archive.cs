using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController7_14_Archive : MonoBehaviour
{
    public GameObject box;

    List<GameObject> _boxes = new List<GameObject>();

    IEnumerator AnimateFlocking()
    {
        var vehicles = new Vehicles();

        yield return new WaitForSeconds(0.25f);
    }

    private void OnGUI()
    {

        GUILayout.BeginArea(new Rect(20, 20, 200, 200));

        if (GUILayout.Button("Animate Flocking"))
        {
            StartCoroutine(AnimateFlocking());
        }

        GUILayout.EndArea();
    }
}
