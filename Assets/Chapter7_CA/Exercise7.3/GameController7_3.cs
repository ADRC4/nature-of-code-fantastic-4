using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController7_3 : MonoBehaviour {

    public GUISkin UISkin;
    public GameObject box;

    List<GameObject> _boxes = new List<GameObject>();

    int generation;

    IEnumerator AnimateCellularRow()
    {
        var cellular = new CellularAutomata1d7_3();
        var length = cellular.Cells.Length;
        int genCount = 20;


        for (int i = 0; i < genCount; i++)
        {


            for (int j = 0; j < length; j++)
            {
                if (cellular.Cells[j] == 1)
                {

                    AddBoxToGrid(i, j, length);

                }
            }
            generation++;
            cellular.NextGeneration();
            yield return new WaitForSeconds(0.25f);
        }


    }


    void AddBoxToGrid(int layer, int row, int rowCount)
    {
        float size = 1.02f;
        float halfRow = rowCount * 0.5f;


        var rotation = Quaternion.Euler(0, layer * 0, 0);
        var pos = new Vector3(
            (row - halfRow) * size,
            0.5f + layer * size,
            layer*size
            );

        var pos2 = new Vector3(
            layer * size,
            0.5f + layer * size,
            (row - halfRow) * size
            );

        var pos3 = new Vector3(
            (row - halfRow) * size,
            0.5f + layer * size,
            -layer * size
            );

        var pos4 = new Vector3(
            -layer * size,
            0.5f + layer * size,
            (row - halfRow) * size
            );

        pos = rotation * pos;
        var instance = Instantiate(box, pos, rotation);
        _boxes.Add(instance);

        var instance2 = Instantiate(box, pos2, rotation);
        _boxes.Add(instance2);

        var instance3 = Instantiate(box, pos3, rotation);
        _boxes.Add(instance3);
        layer++;

        var instance4 = Instantiate(box, pos4, rotation);
        _boxes.Add(instance4);
        layer++;
    }

    private void OnGUI()
    {
        var cellular = new CellularAutomata1d7_3();

        int genCount = 20;


        GUI.skin = UISkin;

        GUILayout.BeginArea(new Rect(20, 20, 200, 200));

        if (GUILayout.Button("Animate Cellular Row"))
        {

            StartCoroutine(AnimateCellularRow());
        }
        if (generation == genCount)
        {
            foreach (var instance in _boxes)
                Destroy(instance);

            generation = 0;

            StartCoroutine(AnimateCellularRow());
        }
        GUILayout.EndArea();
    }
}
