using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using LitJson;

//using UnityEngine.Experimental.Networking;

public class JsonFetcher {
	static List<String> dataArray = new List<string>();

	public static List<String> GetData (JsonData itemData){
		dataArray.Clear ();
		for (int i = 0; i < itemData["data"].Count; i++) {
			String currStr = itemData ["data"] [i] ["title"]["fi"].ToString();
			dataArray.Add (currStr);
		}
		return dataArray;
	}
}
