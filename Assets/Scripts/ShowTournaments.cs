using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTournaments : MonoBehaviour
{
    private ReadAPIPUBG infoAPI;
    private int counter = 0; 

    private void Start()
    {
        infoAPI = FindObjectOfType<ReadAPIPUBG>();
    }

    void Refresh()
    {
        string[] tournaments = new string[infoAPI.jsonFileInfo.Count];

        if (tournaments.Length - (counter * infoAPI.TOURNAMENTS_IN_SCREEN) > (infoAPI.TOURNAMENTS_IN_SCREEN-1))
        {            
            for (int i = 0; i < infoAPI.TOURNAMENTS_IN_SCREEN; i++)
            {
                infoAPI.ReadInfoTournament(infoAPI.jsonFileInfo[counter * infoAPI.TOURNAMENTS_IN_SCREEN + i]["id"],
                                                                infoAPI.jsonFileInfo[counter * infoAPI.TOURNAMENTS_IN_SCREEN + i]["attributes"]["createdAt"], i);

            }
        }
        else
        {
            if (tournaments.Length - (counter * (infoAPI.TOURNAMENTS_IN_SCREEN)) > 0 || tournaments.Length - (counter * (infoAPI.TOURNAMENTS_IN_SCREEN)) < infoAPI.TOURNAMENTS_IN_SCREEN)
            {
                for (int i = 0; i < tournaments.Length - (counter * (infoAPI.TOURNAMENTS_IN_SCREEN)); i++)
                {
                    infoAPI.ReadInfoTournament(infoAPI.jsonFileInfo[counter * infoAPI.TOURNAMENTS_IN_SCREEN + i]["id"],
                                                                infoAPI.jsonFileInfo[counter * infoAPI.TOURNAMENTS_IN_SCREEN + i]["attributes"]["createdAt"], i);

                }
                for(int i= tournaments.Length - (counter * (infoAPI.TOURNAMENTS_IN_SCREEN)); i< infoAPI.TOURNAMENTS_IN_SCREEN; i++)
                {
                    infoAPI.container.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                    infoAPI.container.transform.GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }

            }
        }

    }

    public void nextSetOfTournaments()
    {
        float operation = (float)infoAPI.jsonFileInfo.Count / infoAPI.TOURNAMENTS_IN_SCREEN;

        //Debug.Log(counter);

        if (counter + 1 == Mathf.CeilToInt(operation) - 1) infoAPI.btnDown.SetActive(false);
        if (counter + 1 < Mathf.CeilToInt(operation)) 
        {
            if(!infoAPI.btnUP.activeInHierarchy) infoAPI.btnUP.SetActive(true);
            counter += 1;
            infoAPI.btnUP.GetComponent<ShowTournaments>().counter = counter;
            Refresh();
        }
    }

    public void previousSetOfTournaments()
    {
        if (counter -1 == 0) infoAPI.btnUP.SetActive(false);
        if (counter - 1 > -1)
        {
            if (!infoAPI.btnDown.activeInHierarchy) infoAPI.btnDown.SetActive(true);
            counter -= 1;
            infoAPI.btnDown.GetComponent<ShowTournaments>().counter = counter;
            Refresh();
        }
        
    }
    
}
