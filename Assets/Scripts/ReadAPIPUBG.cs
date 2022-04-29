using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;

public class ReadAPIPUBG : MonoBehaviour
{
    //CONSTRAINTS
    public readonly int TOURNAMENTS_IN_SCREEN = 5;

    //ATTRIBUTES
    private readonly string pugbTournamentsURL = "https://api.pubg.com/tournaments";
    private readonly string APIKey = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiI4OWFkMzZkMC1hOTRhLTAxM2EtYzBlZi0zMzVmZTRmOTlhMjkiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNjUxMTY4NjI1LCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6ImRldmVsb3BlcnBvc2l0In0.ShIyWSUMEeujssvagzq3kjfhTNDlpbPqJhN5IaGsY-w ";

    private TextMeshProUGUI loadingTxt;

    public GameObject prefabTournament;
    public GameObject container;
    public GameObject btnUP;
    public GameObject btnDown;
    public JSONNode jsonFileInfo;



    // Start is called before the first frame update
    void Start()
    {
        loadingTxt = GameObject.Find("Loading").GetComponent<TextMeshProUGUI>();
        StartCoroutine(GetTournamentsList());
    }

    IEnumerator GetTournamentsList()
    {
               

        UnityWebRequest tournamentsRequest = UnityWebRequest.Get(pugbTournamentsURL);
        tournamentsRequest.SetRequestHeader("Authorization", "Bearer " + APIKey);
        tournamentsRequest.SetRequestHeader("accept", "application/vnd.api+json");

        loadingTxt.text = "Loading info...";

        yield return tournamentsRequest.SendWebRequest();       
        
        
        //Validate if there is a error in the process of GET
        if (tournamentsRequest.isNetworkError || tournamentsRequest.isHttpError)
        {
            loadingTxt.text = "Request Error...try again";
            Debug.LogError(tournamentsRequest.error);
            yield break;
        }

        //Download the complete JSON info
        JSONNode jsonNode = JSON.Parse(tournamentsRequest.downloadHandler.text);
        Debug.Log(tournamentsRequest.downloadHandler.text);
        
        JSONNode dataNode = jsonNode["data"];
        jsonFileInfo = dataNode;

        //Read info of each tournament
        for (int i = 0; i < TOURNAMENTS_IN_SCREEN; i++)
            ReadInfoTournament(dataNode[i]["id"], dataNode[i]["attributes"]["createdAt"], i);

        btnDown.SetActive(true);
        container.SetActive(true);
        Destroy(loadingTxt.gameObject);
    }

    public void ReadInfoTournament(string id, string creation, int index)
    {
        container.transform.GetChild(index).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ID: " + id;
        container.transform.GetChild(index).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = creation;        
    }

}
