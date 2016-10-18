using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using LitJson;
using UnityEngine.Networking;

/// <summary>
/// The purpose of this class is to get JSON string from the API server and perform UI action in the scene
/// </summary>
public class UIHandler : MonoBehaviour {
	
	public Text UserInputField, ErrorMessage;
	public GameObject ContentBox, TextContainer, firstPos;
	List<String> dataArray = new List<string>();
	int currentOffset = 0;
	int currentLimit = 10;
	string JsonString;
	private String UserInput;

	/// <summary>
	/// OnClick Event for Search Button - Called from the scene
	/// </summary>
	public void SearchBtnPressed(){
		//Remove all items from scrollview to populate with new data.
		foreach (Transform child in ContentBox.transform) {
			if (child.tag != "firstPos") {
				Destroy (child.gameObject);
			}
		}
		ContentBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 0);
		currentOffset = 0;
		UserInput = UserInputField.text.ToString ();
		Search ();
	}

	/// <summary>
	/// This method creates a URL string and calls the 
	/// coroutine to get JSON data from the api server.
	/// </summary>
	public void Search(){
			string Url = "http://external.api.yle.fi/v1/programs/items.json?q=" + UserInput + "&type=program&offset=" + currentOffset.ToString() + "&limit=" +
			currentLimit +"&app_id=2621b415&app_key=4c4114300a84dd6a177d498085429dc2";
		print (Url);
		StartCoroutine (GetText (Url));
	}


	/// <summary>
	/// This Coroutine gets the JSON text from the server 
	/// and call the AddItems method
	/// </summary>
	IEnumerator GetText (string Url) {
		UnityWebRequest www = UnityWebRequest.Get (Url);
		yield return www.Send ();
		try {
			if (www.isError) {
				ErrorMessage.text = www.error;
				ErrorMessage.gameObject.SetActive(true);
				StartCoroutine (DisableErrorText());
			} else {
				//Json fetched
				JsonString = www.downloadHandler.text.ToString ();
				JsonData itemData = JsonMapper.ToObject (JsonString); //JsonMapper is a method from LitJson library which converts string into parsable JsonData object
				dataArray.Clear ();
				dataArray = JsonParser.GetData (itemData); //GetData method from the JsonParser script is called to get the desired data
				AddItems ();
				currentOffset += currentLimit; //Offset Update to get the next 10 items
			}

		} catch (Exception ex) {
			ErrorMessage.text = ex.Message;
			ErrorMessage.gameObject.SetActive(true);
			StartCoroutine (DisableErrorText());
		}
	}

	/// <summary>
	/// Adding Titles to the Scrollable List and Resize the scrollview container
	/// </summary>
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

	/// <summary>
	/// Checks if the vertical scrollbar is at the bottom 
	/// then it will call the Search method to get the Next 10 items.
	/// </summary>
	public void ScrollerValueChanged(Scrollbar ScrollVert){
		if (ScrollVert.value == 0) {
			Search ();
		}
	}


	/// <summary>
	/// If an error occurs in the Search or if the data is not find in the directory, an error message will be displayed in the the scene. 
	/// This coroutine is used to disable the error text after 3 seconds.
	/// </summary>
	IEnumerator DisableErrorText(){
		yield return new WaitForSeconds (3);
		ErrorMessage.gameObject.SetActive (false);
	}
}
