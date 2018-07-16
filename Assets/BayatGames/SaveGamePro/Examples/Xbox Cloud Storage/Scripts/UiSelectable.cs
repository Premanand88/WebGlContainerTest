using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiSelectable : MonoBehaviour {
	public Button btn;

	public float time;

	// Use this for initialization
	void OnEnable () {
		//btn.Select ();
		btn.Select ();
		btn.OnSelect (null);
	}
	
	// Update is called once per frame
	void Update () {
		if (EventSystem.current.currentSelectedGameObject == null) {
			time += Time.deltaTime;
			if (time > 1.5f) {
				time = 0;
				EventSystem.current.SetSelectedGameObject (btn.gameObject);
			}
		}
	}



	public void CloseTab(){
		btn.Select ();

	}
}
