using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BayatGames.SaveGamePro;
using BayatGames.SaveGamePro.Networking;
using System;
using System.Text;
using System.Xml.Linq;

namespace BayatGames.SaveGamePro.Examples
{
    [Serializable]
    public class SaveItems
    {
        public string itemName;
        public Vector3 pos;
        public Quaternion rot;
        public Vector3 scale;
        public string tag;
    }

    [Serializable]
    public class SaveItemsList
    {
        public List<SaveItems> saveList;
    }

    /// <summary>
    /// Web cloud save example.
    /// </summary>
    public class WebCloudSave : MonoBehaviour
    {

        /// <summary>
        /// The identifier.
        /// </summary>
        public string identifier = "helloWorld";
        public List<GameObject> prefabList;

        /// <summary>
        /// The secret key.
        /// </summary>
        public string secretKey;
        public string userName;
        public string password;

        /// <summary>
        /// The URL.
        /// </summary>
        public string url;

        /// <summary>
        /// The default value.
        /// </summary>
        public string defaultValue = "Hello, World!";

        /// <summary>
        /// The save button.
        /// </summary>
        public Button saveButton;

        /// <summary>
        /// The load button.
        /// </summary>
        public Button loadButton;

        /// <summary>
        /// The clear button.
        /// </summary>
        public Button clearButton;
        public GameObject WorldManager;

        /// <summary>
        /// The username input field.
        /// </summary>
        public InputField usernameInputField;

        /// <summary>
        /// The password input field.
        /// </summary>
        public InputField passwordInputField;

        /// <summary>
        /// The data input field.
        /// </summary>
        public InputField dataInputField;

        /// <summary>
        /// Save the data.
        /// </summary>
        public void Save()
        {
            StartCoroutine("DoSave");
        }

        public void Start()
        {
            StringBuilder result = new StringBuilder();
            foreach (XElement level1Element in XElement.Load(@"\ProjectFiles\Contanify\WebGlContainer\Assets\Config\ConfigXML.xml").Elements("Key"))
            {
                secretKey =  (level1Element.Value).Trim();
            }
            foreach (XElement level1Element in XElement.Load(@"\ProjectFiles\Contanify\WebGlContainer\Assets\Config\ConfigXML.xml").Elements("username"))
            {
                userName = (level1Element.Value).Trim();
            }
            foreach (XElement level1Element in XElement.Load(@"\ProjectFiles\Contanify\WebGlContainer\Assets\Config\ConfigXML.xml").Elements("password"))
            {
                password = (level1Element.Value).Trim().Replace('^','&');
            }
            Debug.Log(secretKey);
        }


        IEnumerator DoSave()
        {
            Debug.Log("Saving...");

            // Disable save button.
            saveButton.interactable = false;
            //SaveGameWeb web = new SaveGameWeb(url, secretKey, usernameInputField.text, passwordInputField.text);
            SaveGameWeb web = new SaveGameWeb(url, secretKey, userName, password);
            SaveItemsList saveDataList =  new SaveItemsList();
            List<SaveItems> saveList = new List<SaveItems>();
            string saveJsonData = string.Empty;
            Transform[] ts = WorldManager.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts)
            {
                if (t.gameObject.tag.Contains("Placed"))
                {
                    SaveItems saveData = new SaveItems();
                    saveData.itemName = t.name;
                    saveData.pos = t.transform.position;
                    saveData.rot = t.transform.rotation;
                    saveData.scale = t.transform.localScale;
                    saveData.tag = t.tag;
                    saveList.Add(saveData);
                }
            }
            saveDataList.saveList = saveList;
            saveJsonData = JsonUtility.ToJson(saveDataList);
            yield return StartCoroutine(web.Save(identifier, saveJsonData));// dataInputField.text));

            // Enable save button.
            saveButton.interactable = true;
#if UNITY_2017_1_OR_NEWER
            if ( web.Request.isHttpError || web.Request.isNetworkError )
			{
				Debug.LogError ( "Save Failed" );
				Debug.LogError ( web.Request.error );
				Debug.LogError ( web.Request.downloadHandler.text );
			}
			else
			{
				Debug.Log ( "Save Successful" );
				Debug.Log ( "Response: " + web.Request.downloadHandler.text );
			}
#else
            if (web.Request.isError)
            {
                Debug.LogError("Save Failed");
                Debug.LogError(web.Request.error);
                Debug.LogError(web.Request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Save Successful");
                Debug.Log("Response: " + web.Request.downloadHandler.text);
            }
#endif
        }

        /// <summary>
        /// Load the data.
        /// </summary>
        public void Load()
        {
            StartCoroutine("DoLoad");
        }

        IEnumerator DoLoad()
        {
            Debug.Log("Loading...");

            // Disable load button.
            loadButton.interactable = false;
            // SaveGameWeb web = new SaveGameWeb(url, secretKey, usernameInputField.text, passwordInputField.text);
            SaveGameWeb web = new SaveGameWeb(url, secretKey, userName, password);
            yield return StartCoroutine(web.Download(identifier));

            // Enable load button.
            loadButton.interactable = true;
#if UNITY_2017_1_OR_NEWER
            if ( web.Request.isHttpError || web.Request.isNetworkError )
			{
                Debug.LogError("Load Failed");
                Debug.LogError(web.Request.error);
                Debug.LogError(web.Request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Load Successful");
                Debug.Log("Response: " + web.Request.downloadHandler.text);
                //dataInputField.text = web.Load<string>(defaultValue);
                SaveItemsList loadItems = new SaveItemsList();
                loadItems = JsonUtility.FromJson<SaveItemsList>(web.Load<string>(defaultValue)) ;
                Debug.Log(loadItems.saveList[0].tag);
                foreach(SaveItems item in loadItems.saveList)
                {
                    foreach(GameObject prefab in prefabList)
                    {
                        if (prefab.tag == item.tag)
                        {
                            GameObject tileAdded = (GameObject)Instantiate(prefab, item.pos, item.rot);
                            //objLoad.PushItem(tileAdded);
                            //if (scrollList != null)
                            //    scrollList.TryTransferItemToOtherShop(cartItem);
                            tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
                        }
                    }
                }
            }
#else
            if (web.Request.isError)
            {
                Debug.LogError("Load Failed");
                Debug.LogError(web.Request.error);
                Debug.LogError(web.Request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Load Successful");
                Debug.Log("Response: " + web.Request.downloadHandler.text);
                dataInputField.text = web.Load<string>(defaultValue);
            }
#endif
        }

        /// <summary>
        /// Clear the user data.
        /// </summary>
        public void Clear()
        {
            StartCoroutine("DoClear");
        }

        IEnumerator DoClear()
        {
            Debug.Log("Clearing...");

            // Disable clear button.
            clearButton.interactable = false;
            SaveGameWeb web = new SaveGameWeb(url, secretKey, usernameInputField.text, passwordInputField.text);
            yield return StartCoroutine(web.Clear());

            // Enable clear button.
            clearButton.interactable = true;
#if UNITY_2017_1_OR_NEWER
            if (web.Request.isHttpError || web.Request.isNetworkError)
            {
                Debug.LogError("Clear Failed");
                Debug.LogError(web.Request.error);
            }
            else
            {
                Debug.Log("Clear Successful");
                Debug.Log("Response: " + web.Request.downloadHandler.text);
            }
#else
            if (web.Request.isError)
            {
                Debug.LogError("Clear Failed");
                Debug.LogError(web.Request.error);
            }
            else
            {
                Debug.Log("Clear Successful");
                Debug.Log("Response: " + web.Request.downloadHandler.text);
            }
#endif
        }

    }

}