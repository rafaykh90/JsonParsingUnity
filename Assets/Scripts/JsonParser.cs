using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using LitJson; //LitJson is a JSON library for parsing JSON

/// <summary>
/// Json parser.
/// </summary>
public class JsonParser {
	static List<String> dataArray = new List<string>();

	/// <summary>
	/// Retrieves the Finnish Title from the JsonData Object which is recieved from the UIHandler class 
	/// and stores them in a list then finally returns the list to the UIHandler class
	/// </summary>
	/// <returns>List of titles</returns>
	/// <param name="itemData">JsonData Object</param>
	public static List<String> GetData (JsonData itemData){
		dataArray.Clear ();
		for (int i = 0; i < itemData["data"].Count; i++) {
			String currStr = itemData ["data"] [i] ["title"]["fi"].ToString();
			dataArray.Add (currStr);
		}
		return dataArray;
	}
}
