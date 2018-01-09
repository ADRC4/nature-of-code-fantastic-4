using UnityEngine;
using System.Collections;

public class LoadLevels : MonoBehaviour {

	void Start () {
		if (PlayerPrefs.GetInt ("size") == 0)
			ResetSize ();
	}
	public void Load () {
		Application.LoadLevel("Game");
	}

	public void ResetSize () {
		PlayerPrefs.SetInt("size",20);
	}
}
