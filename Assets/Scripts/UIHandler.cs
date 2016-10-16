using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using LitJson;
using UnityEngine.Networking;

public class UIHandler : MonoBehaviour {
	
	public Text programInput, ErrorMessage;
	public GameObject ContentBox, TextContainer, firstPos;
	List<String> dataArray = new List<string>();
	int currentOffset = 0;
	int currentLimit = 10;
	string JsonString;

	// Use this for initialization
	void Start(){
	}
	public void AddItems() {
		for (int i = 0; i < 10; i++) {
			if (i == 0 && ContentBox.transform.childCount <= 1){
				GameObject TContainer = Instantiate (TextContainer, firstPos.transform.position, Quaternion.identity,ContentBox.transform) as GameObject;
				TContainer.GetComponentInChildren<Text> ().text = dataArray [i].ToString ();
			} else {
				Vector3 pos = new Vector3 (ContentBox.transform.GetChild (ContentBox.transform.childCount - 1).transform.position.x, ContentBox.transform.GetChild (ContentBox.transform.childCount - 1).transform.position.y
					- (TextContainer.GetComponent<RectTransform> ().rect.height * 2.5f), 0);
				GameObject TContainer = Instantiate (TextContainer, pos, 
					Quaternion.identity, ContentBox.transform) as GameObject;
				TContainer.GetComponentInChildren<Text> ().text = dataArray [i].ToString ();
			}
		}
		ContentBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, ContentBox.GetComponent<RectTransform>().rect.height + 495);
	}

	private String Url;
	private String program;
	public void SearchBtnPressed(){
		foreach (Transform child in ContentBox.transform) {
			if (child.tag != "firstPos") {
				Destroy (child.gameObject);
			}
		}
		ContentBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 0);
		currentOffset = 0;
		program = programInput.text.ToString ();
		Search ();
	}
	public void Search(){
		Url = "http://external.api.yle.fi/v1/programs/items.json?type=" + program + "&offset=" + currentOffset.ToString() + "&limit=" +
			currentLimit +"&app_id=2621b415&app_key=4c4114300a84dd6a177d498085429dc2";
		print (Url);
		StartCoroutine (GetText ());
	}

	IEnumerator GetText () {
		UnityWebRequest www = UnityWebRequest.Get (Url);
		yield return www.Send ();
		try {
			if (www.isError) {
				print (www.error);
			} else {
				//Json fetched
				JsonString = www.downloadHandler.text.ToString ();
				JsonData itemData = JsonMapper.ToObject (JsonString);
				dataArray.Clear ();
				dataArray = JsonFetcher.GetData (itemData);
				AddItems ();
				currentOffset += currentLimit;
			}

		} catch (Exception ex) {
			ErrorMessage.text = ex.Message;
			ErrorMessage.gameObject.SetActive(true);
			StartCoroutine (DisableErrorText());
		}
	}

	public void ScrollerValueChanged(Scrollbar ScrollVert){
		if (ScrollVert.value == 0) {
			Search ();
		}
	}

	IEnumerator DisableErrorText(){
		yield return new WaitForSeconds (3);
		ErrorMessage.gameObject.SetActive (false);
	}
}
