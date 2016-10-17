using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using LitJson;

//using UnityEngine.Experimental.Networking;

public class JsonParser {
	static List<String> dataArray = new List<string>();

	//Retrieves the Finnish Title from the JsonData Object which is recieved from the UIHandler class and stored them in a list then finally returns the list to the UIHandler class
	public static List<String> GetData (JsonData itemData){
		dataArray.Clear ();
		for (int i = 0; i < itemData["data"].Count; i++) {
			String currStr = itemData ["data"] [i] ["title"]["fi"].ToString();
			dataArray.Add (currStr);
		}
		return dataArray;
	}
}
