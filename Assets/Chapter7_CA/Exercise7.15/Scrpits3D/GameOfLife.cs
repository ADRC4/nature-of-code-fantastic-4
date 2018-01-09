using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOfLife : MonoBehaviour {
	public int size = 20;
	public int Ysize;
    
	public bool GUI = true;

	bool[,,] world;
	GameObject[,,] GOS;
	public int[,,] worldInt;

	[Header ("Default, Deleted, OneGen, Alive, Created")]
	public Material[] transparentsMats = new Material[5];
	public Material[] opaquesMats = new Material[5];
	public Material[] usedMats = new Material[5];
	public bool useTransparentsMats = true;


	public bool use2D = true;
	public bool useTime = false;

	public bool threated = false;
	
	public int gen = 0;

	public int _w = 2;
	public int _x = 3;
	public int _y = 3;
	public int _z = 3;

	bool color = true;
	bool play = false;
	bool playOnce = false;
	float lastPlayCall = 0;
	float playFrequency = 1;

	bool needTransparentsMats = false;

	Text fpsText;
	Text genText;
	Text errorText;
	InputField WXYZText;
	InputField sizeText;
	Slider slider;

	Job myJob;

	void Start () {
		if (GUI)
			playFrequency = 1;
		else
			playFrequency = 0.2f;
		if (GUI)
			play = false;
		else
			play = true;
		playOnce = false;
		/*
		 * GUI
		 */
		if (GUI) {
			fpsText = GameObject.Find ("FPSText").GetComponent<Text> ();
			genText = GameObject.Find ("GenText").GetComponent<Text> ();
			errorText = GameObject.Find ("Error").GetComponent<Text> ();
			sizeText = GameObject.Find ("Size").GetComponent<UnityEngine.UI.InputField> ();
			WXYZText = GameObject.Find ("WXYZ").GetComponent<UnityEngine.UI.InputField> ();
			slider = GameObject.Find ("Slider").GetComponent<Slider> ();
			slider.value = 1f;
			errorText.text = "";
			WXYZText.text = _w.ToString () + _x.ToString () + _y.ToString () + _z.ToString ();
			sizeText.text = size.ToString ();
		}
		/*
		 * Set size
		 */
		gen = 0;
		if (GUI)
			size = PlayerPrefs.GetInt ("size");
		else
			size = 20;
		if(size == 0)
			size = 20;
		/*
		 * Define mod
		 */
		ChangeMats(false);
		Ysize = size;
		if(useTime) {
			ChangeMats(true);
			needTransparentsMats = true;
		}
		if(use2D) {
			Ysize = 1;
			ChangeMats(true);
		}
		/*
		 * Generate world
		 */
		world = new bool[size,Ysize,size];
		worldInt = new int[size,Ysize,size];
		GOS = new GameObject[size,Ysize,size];
		
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < Ysize; y++) {
				for(int z = 0; z < size; z++) {
					GOS[x,y,z] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					GOS[x,y,z].transform.position = new Vector3(x,y,z);
					GOS[x,y,z].name = x.ToString()+","+y.ToString()+","+z.ToString();
					worldInt[x,y,z] = 0;
				}
			}
		}
		//Central cube
		if (!use2D && !useTime) {
			worldInt [(int)size / 2, (int)Ysize / 2, (int)size / 2] = 3;
			worldInt [(int)size / 2, (int)Ysize / 2, (int)size / 2+1] = 3;
			worldInt [(int)size / 2, (int)Ysize / 2, (int)size / 2-1] = 3;
		}
		if(useTime)
			worldInt[(int)size/2,size-1,(int)size/2] = 3;
		/*
		 * Camera
		 */
		//Position
		Camera.main.transform.position = new Vector3(size/2, size/2, size*1.5f);
		if(use2D)
			Camera.main.transform.position = new Vector3(size, size/4, size);
		if(useTime)
			Camera.main.transform.position = new Vector3(size, size*1.5f, size);
		//Rotation
		Camera.main.transform.LookAt(new Vector3((int)size/2,(int)Ysize/2,(int)size/2));
		if(useTime)
			Camera.main.transform.LookAt(new Vector3((int)size/2,size-1,(int)size/2));
		//MouseLook
		Destroy(Camera.main.GetComponent<MouseLook>());
		var mouse = Camera.main.gameObject.AddComponent<MouseLook>();
		mouse.minimumY = -90f;
		mouse.maximumY = 90f;
		mouse.sensitivityX = 5f;
		mouse.sensitivityY = 5f;
		mouse.enabled = false;
		/*
		 * Threat
		 */
		myJob = null;
		/*
		 * Apply changes
		 */
		SyncWorlds();
		ApplyColor();
		ActualizeGUI ();
	}

	void Update () {
		if(GUI)
			fpsText.text = "FPS: "+((int)(1.0f / Time.smoothDeltaTime)).ToString();
		if (!GUI && gen == 12)
			Reset();

		if(lastPlayCall + playFrequency < Time.time) {
			Play();
			lastPlayCall = Time.time;
		}

		if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !EventSystem.current.IsPointerOverGameObject()) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				Vector3 v;
				if(useTransparentsMats && Input.GetMouseButtonDown(0))
					v = hit.transform.position+hit.normal;
				else
					v = hit.transform.position;
				if(v.x >= size || v.y >= Ysize || v.z >= size || v.x < 0 || v.y < 0 || v.z < 0)
					return;
				if(useTime && ((int) v.y) != size - 1)
					return;
				if(Input.GetMouseButtonDown(0))
					worldInt[(int)v.x,(int)v.y,(int)v.z] = 3;
				else
					worldInt[(int)v.x,(int)v.y,(int)v.z] = 0;
				SyncWorlds((int)v.x,(int)v.y,(int)v.z);
				ApplyColor((int)v.x,(int)v.y,(int)v.z);
			}
		}
	}

	public void Play () {
		if (useTime && needTransparentsMats && play) {
			needTransparentsMats = false;
			ChangeMats(false);
			ApplyColor();
		}
		if (threated)
			PlayThreat ();
		else
			PlayNormal ();
	}

	public void Size (int s, int Ys) {
		int prevSize = size;
		int YprevSize = Ysize;
		size = s;
		Ysize = Ys;
		for(int x = 0; x < prevSize; x++) {
			for(int y = 0; y < YprevSize; y++) {
				for(int z = 0; z < prevSize; z++) {
					Destroy(GOS[x,y,z]);
					worldInt[x,y,z] = 0;
					world[x,y,z] = false;
				}
			}
		}
		Start();
	}

	public void Size (int s) {
		Size(s, Ysize);
	}

	public void Resize () {
		int s = System.Int32.Parse(GameObject.Find ("Size").GetComponent<UnityEngine.UI.InputField>().text);
		PlayerPrefs.SetInt("size", s);
		GameObject.Find ("Error").GetComponent<UnityEngine.UI.Text>().text = "Restart needed";
	}

	public void ApplyColor() {
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < Ysize; y++) {
				for(int z = 0; z < size; z++) {
					ApplyColor(x,y,z);
				}
			}
		}
	}

	public void ApplyColor(int x, int y, int z) {
		if(worldInt[x,y,z] == 0) {
			GOS[x,y,z].GetComponent<Renderer>().material = usedMats[0];
			if(useTransparentsMats) {
				GOS[x,y,z].layer = 2;
				GOS[x,y,z].GetComponent<MeshRenderer>().enabled = false;
			} else {
				if(use2D) {
					GOS[x,y,z].layer = 0;
					GOS[x,y,z].GetComponent<MeshRenderer>().enabled = true;
				} else if(x == 0 || y == 0 || z == 0 || x == size-1 || y == size-1 || z == size-1) {
					//Keep sides actives
					GOS[x,y,z].layer = 0;
					GOS[x,y,z].GetComponent<MeshRenderer>().enabled = true;
				} else {
					//Disable the inside
					GOS[x,y,z].layer = 2;
					GOS[x,y,z].GetComponent<MeshRenderer>().enabled = false;
				}
			}
		} else {
			if(useTransparentsMats) {
				GOS[x,y,z].layer = 0;
				GOS[x,y,z].GetComponent<MeshRenderer>().enabled = true;
				if(color)
					GOS[x,y,z].GetComponent<Renderer>().material = usedMats[worldInt[x,y,z]];
				else
					GOS[x,y,z].GetComponent<Renderer>().material = usedMats[3];
			} else {
				if(use2D) {
					GOS[x,y,z].layer = 0;
					GOS[x,y,z].GetComponent<MeshRenderer>().enabled = true;
					if(color) {
						Material mat = usedMats[worldInt[x,y,z]];
						GOS[x,y,z].GetComponent<Renderer>().material = mat;
					} else
						GOS[x,y,z].GetComponent<Renderer>().material = usedMats[3];
				} else if(x == 0 || y == 0 || z == 0 || x == size-1 || y == size-1 || z == size-1) {
					//Keep sides actives
					GOS[x,y,z].layer = 0;
					GOS[x,y,z].GetComponent<MeshRenderer>().enabled = true;
					if(color)
						GOS[x,y,z].GetComponent<Renderer>().material = usedMats[worldInt[x,y,z]];
					else
						GOS[x,y,z].GetComponent<Renderer>().material = usedMats[3];
				} else {
					//Disable the inside
					GOS[x,y,z].layer = 2;
					GOS[x,y,z].GetComponent<MeshRenderer>().enabled = false;
				}
			}						
		}
	}

	
	public void Slider () {
		playFrequency = slider.value;
		lastPlayCall = Time.time + 1;
	}
	
	public void Reset () {
		Size(size);
	}

	public void WXYZ () {
		string wxyz = GameObject.Find ("WXYZ").GetComponent<UnityEngine.UI.InputField>().text;
		_w = (int) System.Char.GetNumericValue(wxyz[0]);
		_x = (int) System.Char.GetNumericValue(wxyz[1]);
		_y = (int) System.Char.GetNumericValue(wxyz[2]);
		_z = (int) System.Char.GetNumericValue(wxyz[3]);
	}

	public void Next() {
		playOnce = true;
		Play();
	}

	public void ChangeMats (bool useOpaque) {
		useTransparentsMats = !useOpaque;
		if(useTransparentsMats)
			usedMats = transparentsMats;
		else
			usedMats = opaquesMats;
		ActualizeGUI();
	}

	public void ChangeMats () {
		ChangeMats(useTransparentsMats);
		ApplyColor();
	}

	public void ChangeDimension () {
		use2D = !use2D;
		if (use2D)
			useTime = false;
		ActualizeGUI();
		Reset ();
	}

	public void UseTime () {
		useTime = !useTime;
		if (useTime)
			use2D = false;
		ActualizeGUI();
		Reset();
	}

	public void ChangeColor () {
		color = !color;
		ActualizeGUI();
	}

	public void ChangePlay () {
		play = !play;
		ActualizeGUI();
	}

	public void ChangeThread () {
		threated = !threated;
		ActualizeGUI ();
	}

	public void ActualizeGUI () {
		if (GUI) {
			if (play)
				GameObject.Find ("PlayButtonText").GetComponent<UnityEngine.UI.Text> ().text = "Pause";
			else
				GameObject.Find ("PlayButtonText").GetComponent<UnityEngine.UI.Text> ().text = "Play";

			if (useTime)
				GameObject.Find ("TimeButtonText").GetComponent<UnityEngine.UI.Text> ().text = "No history";
			else
				GameObject.Find ("TimeButtonText").GetComponent<UnityEngine.UI.Text> ().text = "History";

			if (use2D)
				GameObject.Find ("2DButtonText").GetComponent<UnityEngine.UI.Text> ().text = "3D";
			else
				GameObject.Find ("2DButtonText").GetComponent<UnityEngine.UI.Text> ().text = "2D";

			if (useTransparentsMats)
				GameObject.Find ("MatsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "Opaque";
			else
				GameObject.Find ("MatsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "Transparent";
			
			if (color)
				GameObject.Find ("ColorButtonText").GetComponent<UnityEngine.UI.Text> ().text = "No color";
			else
				GameObject.Find ("ColorButtonText").GetComponent<UnityEngine.UI.Text> ().text = "Color";
			
			if (threated)
				GameObject.Find ("ThreatedButtonText").GetComponent<UnityEngine.UI.Text> ().text = "Normal";
			else
				GameObject.Find ("ThreatedButtonText").GetComponent<UnityEngine.UI.Text> ().text = "Multithreaded";
		}
	}

	public void SyncWorlds () {
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < Ysize; y++) {
				for(int z = 0; z < size; z++) {
					SyncWorlds(x,y,z);
				}
			}
		}
	}

	public void SyncWorlds (int x, int y, int z) {
		if(worldInt[x,y,z] == 0)
			world[x,y,z] = false;
		else
			world[x,y,z] = true;
	}
	
	public void SyncWorldsTime () {
		for(int x = 0; x < size; x++) {
			for(int z = 0; z < size; z++) {
				if(worldInt[x,size-1,z] == 0)
					world[x,size-1,z] = false;
				else
					world[x,size-1,z] = true;
			}
		}
	}

	void PlayNormal () {
		myJob = null;
		if (play || playOnce) {
			gen ++;
			if(GUI)
				genText.text = "Generation " + gen.ToString ();
			if (useTime)
				PlayTime ();
			else
				PlayNoTime ();
			ApplyColor ();
			playOnce = false;
		}
	}

	void PlayThreat () {
		if(myJob == null) {
			myJob = new Job();
			myJob.world = world;
			myJob.worldInt = worldInt;
			myJob.size = size;
			myJob.Ysize = Ysize;
			myJob.useTime = useTime;
			myJob.Start();
		} else {
			if(myJob.IsDone && (play || playOnce)) {
				ApplyColor();
				myJob._w = _w;
				myJob._x = _x;
				myJob._y = _y;
				myJob._z = _z;
				myJob.color = color;
				myJob.Start();
				gen ++;
				if(GUI)
					genText.text = "Generation " + gen.ToString ();
				playOnce = false;
			}
		}
	}
	
	void PlayTime () {
		GoDown();
		for(int x = 0; x < size; x++) {
			for(int z = 0; z < size; z++) {
				int neighbors = Neighbors(x,size-1,z);
				if(_y <= neighbors && neighbors <= _z) {
					if(!world[x,size-1,z])
						worldInt[x,size-1,z] = 4;
					else
						worldInt[x,size-1,z] = 3;
					
				} else if(_w <= neighbors && neighbors <= _x) {
					if(world[x,size-1,z])
						worldInt[x,size-1,z] = 3;
					else
						worldInt[x,size-1,z] = 0;
				} else
					worldInt[x,size-1,z] = 0;
			}
		}
		SyncWorldsTime();
		for(int x = 0; x < size; x++) {
			for(int z = 0; z < size; z++) {
				int neighbors = Neighbors(x,size-1,z);
				if(world[x,size-1,z]) {
					if(!((_y <= neighbors && neighbors <= _z) || (_w <= neighbors && neighbors <= _x)))
						worldInt[x,size-1,z] -= 2;
				}
			}
		}
	}
	
	void GoDown() {
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < size-1; y++) {
				for(int z = 0; z < size; z++) {
					worldInt[x,y,z] = worldInt[x,y+1,z];
				}
			}
		}
	}
	
	void PlayNoTime () {
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < Ysize; y++) {
				for(int z = 0; z < size; z++) {
					int neighbors = Neighbors(x,y,z);
					if(_y <= neighbors && neighbors <= _z) {
						if(!world[x,y,z])
							worldInt[x,y,z] = 4;
						else
							worldInt[x,y,z] = 3;
						
					} else if(_w <= neighbors && neighbors <= _x) {
						if(world[x,y,z])
							worldInt[x,y,z] = 3;
						else
							worldInt[x,y,z] = 0;
					} else
						worldInt[x,y,z] = 0;
				}
			}
		}
		SyncWorlds();
		if(color) {
			for(int x = 0; x < size; x++) {
				for(int y = 0; y < Ysize; y++) {
					for(int z = 0; z < size; z++) {
						int neighbors = Neighbors(x,y,z);
						if(world[x,y,z]) {
							if(!((_y <= neighbors && neighbors <= _z) || (_w <= neighbors && neighbors <= _x)))
								worldInt[x,y,z] -= 2;
						}
					}
				}
			}
		}
	}

	int Neighbors (int x, int y, int z) {
		int neighbors = 0;
		
		if(x != 0 && y != 0 && z != 0)
			neighbors += Convert.ToInt16(world[x-1, y-1,z-1]);
		if(x != 0 && y != 0)
			neighbors += Convert.ToInt16(world[x-1, y-1,z]);
		if(x != 0 && y != 0 && z != size-1)
			neighbors += Convert.ToInt16(world[x-1, y-1,z+1]);
		
		
		if(x != 0 && z != 0)
			neighbors += Convert.ToInt16(world[x-1, y,z-1]);
		if(x != 0)
			neighbors += Convert.ToInt16(world[x-1, y,z]);
		if(x != 0 && z != size-1)
			neighbors += Convert.ToInt16(world[x-1, y,z+1]);
		
		
		if(x != 0 && y != Ysize-1 && z != 0)
			neighbors += Convert.ToInt16(world[x-1, y+1,z-1]);
		if(x != 0 && y != Ysize-1)
			neighbors += Convert.ToInt16(world[x-1, y+1,z]);
		if(x != 0 && y != Ysize-1 && z != size-1)
			neighbors += Convert.ToInt16(world[x-1, y+1,z+1]);
		
		
		
		if(y != 0 && z != 0)
			neighbors += Convert.ToInt16(world[x, y-1,z-1]);
		if(y != 0)
			neighbors += Convert.ToInt16(world[x, y-1,z]);
		if(y != 0 && z != size-1)
			neighbors += Convert.ToInt16(world[x, y-1,z+1]);
		
		
		if(z != 0)
			neighbors += Convert.ToInt16(world[x, y,z-1]);
		
		if(z != size-1)
			neighbors += Convert.ToInt16(world[x, y,z+1]);
		
		
		if(y != Ysize-1 && z != 0)
			neighbors += Convert.ToInt16(world[x, y+1,z-1]);
		if(y != Ysize-1)
			neighbors += Convert.ToInt16(world[x, y+1,z]);
		if(y != Ysize-1 && z != size-1)
			neighbors += Convert.ToInt16(world[x, y+1,z+1]);
		
		
		if(x != size-1 && y != 0 && z != 0)
			neighbors += Convert.ToInt16(world[x+1, y-1,z-1]);
		if(x != size-1 && y != 0)
			neighbors += Convert.ToInt16(world[x+1, y-1,z]);
		if(x != size-1 && y != 0 && z != size-1)
			neighbors += Convert.ToInt16(world[x+1, y-1,z+1]);
		
		
		if(x != size-1 && z != 0)
			neighbors += Convert.ToInt16(world[x+1, y,z-1]);
		if(x != size-1)
			neighbors += Convert.ToInt16(world[x+1, y,z]);
		if(x != size-1 && z != size-1)
			neighbors += Convert.ToInt16(world[x+1, y,z+1]);
		
		
		if(x != size-1 && y != Ysize-1 && z != 0)
			neighbors += Convert.ToInt16(world[x+1, y+1,z-1]);
		if(x != size-1 && y != Ysize-1)
			neighbors += Convert.ToInt16(world[x+1, y+1,z]);
		if(x != size-1 && y != Ysize-1 && z != size-1)
			neighbors += Convert.ToInt16(world[x+1, y+1,z+1]);
		return neighbors;
	}
}
