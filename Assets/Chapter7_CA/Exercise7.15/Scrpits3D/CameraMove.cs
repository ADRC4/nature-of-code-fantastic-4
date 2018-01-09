using UnityEngine;
using System.Collections;


[RequireComponent (typeof (MouseLook))]
[AddComponentMenu("Camera-Control/Camera Move")]
public class CameraMove : MonoBehaviour {

	public bool restriction = true;
	public float speed = 0.1f;
	float timeSinceLastKey = 0f;
	public Vector3 min = new Vector3(-19,1,-19);
	public Vector3 max = new Vector3(19,30,19);
	public bool noRigidbody = true;
	public int maxGameObjects = 200;
	public UnityEngine.UI.InputField speedField;
	public bool shortcuts = true;
	int size;
	GameOfLife game;
	public GameObject TextPopup;
	public bool rotate = false;

	void Start() {
		game = GetComponent<GameOfLife>();
		size = PlayerPrefs.GetInt("size");
	}
	void Update () {
		if(rotate)
			transform.RotateAround(new Vector3((int)game.size/2,(int)game.Ysize/2,(int)game.size/2), Vector3.up, 20 * Time.deltaTime);
		if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null || !TextPopup.activeSelf) {
			if(timeSinceLastKey + 0.2 < Time.time && shortcuts) {
				if(Input.GetAxis("Jump") == 1) {
					GetComponent<MouseLook>().enabled = !GetComponent<MouseLook>().enabled;
					timeSinceLastKey = Time.time;
				}
				if(Input.GetAxis("Play") == 1) {
					game.ChangePlay();
					timeSinceLastKey = Time.time;
				}
				if(Input.GetAxis("Reset") == 1) {
					game.Reset();
					timeSinceLastKey = Time.time;
				}
				if(Input.GetAxis("Next") == 1) {
					game.Next();
					timeSinceLastKey = Time.time;
				}
				if(Input.GetKey(KeyCode.RightShift)) {
					rotate = !rotate;
					timeSinceLastKey = Time.time;
				}
			}
			if(noRigidbody && speedField != null) {
			transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))*Mathf.Exp(transform.position.magnitude/100)/1000*size*System.Int32.Parse(speedField.text));
				if(restriction) {
					Vector3 position = transform.position;
					if(position.x > max.x)
						position.x = max.x;
					if(position.x < min.x)
						position.x = min.x;

					if(position.y > max.y)
						position.y = max.y;
					if(position.y < min.y)
						position.y = min.y;

					if(position.z > max.z)
						position.z = max.z;
					if(position.z < min.z)
						position.z = min.z;
					transform.position = position;
				}
			} else if(!noRigidbody) {
				GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))*transform.position.magnitude/1000*size*System.Int32.Parse(speedField.text),ForceMode.Impulse);
			}
//			if(GameObject.FindObjectsOfType(gameObject.GetType()).Length > maxGameObjects)
//				Debug.Break();
		}
	}
}
