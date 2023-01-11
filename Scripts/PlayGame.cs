using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Versioning;
using System;
using UnityEngine.UI;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class PlayGame : MonoBehaviour
{
    public ChangeUIText changeUIText;

    public GameObject missile;
    public GameObject missileAI;
    //GameObject[] missiles = new GameObject[99];
    //GameObject currentMissile;

    public int playerScore = 0;
    public int AIScore = 0;

    //public int[,] placements = new int[10, 10];
    //public int[,] placementsAI = new int[10, 10];

    //public Text playerScoreUpdateText;
    //public Text AIScoreUpdateText;

    public GameObject playerHit;
    public GameObject playerMiss;
    //instantiate could go at the end of ai turn
    //ai turn does not have to go in update
    //can be its own function that calls itself to
    // go again if hit

    // Start is called before the first frame update
    void Start()
    {
        // instantiate first missile
        // all others will be given to player at 
        // the end of ai turn or if they hit successfully
        missile = Instantiate(missile, new Vector3(0, 0, 0), Quaternion.identity);
        Material newMat = Resources.Load("red", typeof(Material)) as Material;
        missile.GetComponent<Renderer>().material = newMat;

        //playerHit = GameObject.Find("Hit");
        //playerMiss = GameObject.Find("Miss");

        //accessUIText();

        //currentMissile = missile;

        for (int i = 1; i < 11; i++)
        {
            for (int j = 1; j < 11; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(.9f, .1f, .9f);
                cube.transform.position = new Vector3(j, 0, i);
                //cube.GetComponent<MeshRenderer>().material = Resources.Load<Material>("White");
                cube.name = "TilePlayer";

                GameObject coordinates = new GameObject();
                TMPro.TextMeshPro t = coordinates.AddComponent<TMPro.TextMeshPro>();
                t.text = "[" + j + ", " + i + "]";
                t.transform.position = new Vector3((j + 9.6f), .1f, (i - 2.3f));
                t.transform.localEulerAngles = new Vector3(90, 0, 0);
                t.color = Color.black;
                t.fontSize = 2.35f;

            }
        }

        //ai
        for (int i = -1; i > -11; i--)
        {
            for (int j = -1; j > -11; j--)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(.9f, .1f, .9f);
                cube.transform.position = new Vector3(j, 0, (i + 11f));
                //cube.GetComponent<MeshRenderer>().material = Resources.Load<Material>("White");
                cube.name = "TileAI";

                GameObject coordinates = new GameObject();
                TMPro.TextMeshPro t = coordinates.AddComponent<TMPro.TextMeshPro>();
                t.text = "[" + (j * -1f) + ", " + (i * -1f) + "]";
                t.transform.position = new Vector3(((j * -1) - 11f + 9.6f), .1f, ((i * -1) - 2.3f));
                t.transform.localEulerAngles = new Vector3(90, 0, 0);
                t.color = Color.black;
                t.fontSize = 2.35f;

            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        playerTurn();

        if (playerScore >= 17)
        {
            SceneManager.LoadScene(3);
        }
    }

    /*void accessUIText()
    {
        changeUIText = GameObject.FindObjectOfType < ChangeUIText > ();
    }*/
    /*
    public void setPlacements(int j, int i, int tileValue)
    {
        placements[j, i] = tileValue;
    }

    public void setAIPlacements(int j, int i, int tileValue)
    {
        placementsAI[j, i] = tileValue;

    }*/

    public void playerTurn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(ray, out hitInfo);
        int x = (int)hitInfo.point.x;
        int z = (int)hitInfo.point.z;
        float shipX = x;
        float shipZ = z;
        float y = .5f;
        int playerPlacementX = (int)shipX;
        int playerPlacementZ = (int)shipZ;
        //placementsAI[(j + 1) * (-1), (i + 1) * (-1)] = -1;
        if (hit && hitInfo.transform.name == "TileAI")
        {
            missile.layer = LayerMask.NameToLayer("Ignore Raycast");
            if (shipZ < 1)
            {
                shipZ = 1;
                if (shipX > -1)
                {
                    shipX = -1;
                    playerPlacementX = -1;
                }
                else if (shipX < -10)
                {
                    shipX = -10;
                    playerPlacementX = 9;
                }
                missile.transform.position = new Vector3(shipX, y, shipZ);
                playerPlacementZ = 1;
                debug((playerPlacementX * (-1)) - 1, playerPlacementZ - 1);
                missileCheck((playerPlacementX * (-1)) - 1, playerPlacementZ - 1);
            }
            else if (shipZ > 10)
            {
                shipZ = 10;
                if (shipX > -1)
                {
                    shipX = -1;
                    playerPlacementX = -1;
                }
                else if (shipX < -10)
                {
                    shipX = -10;
                    playerPlacementX = 9;
                }
                missile.transform.position = new Vector3(shipX, y, shipZ);
                playerPlacementZ = 9;
                debug((playerPlacementX * (-1)) - 1, playerPlacementZ - 1);
                missileCheck((playerPlacementX * (-1)) - 1, playerPlacementZ - 1);
            }

            else
            {
                if (shipX > -1)
                {
                    shipX = -1;
                    playerPlacementX = -1;
                }
                else if (shipX < -10)
                {
                    shipX = -10;
                    playerPlacementX = 9;
                }
                missile.transform.position = new Vector3(shipX, y, shipZ);
                debug((playerPlacementX * (-1)) - 1, playerPlacementZ - 1);
                missileCheck((playerPlacementX * (-1)) - 1, playerPlacementZ - 1);
            }
        }
    }

    public void debug(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            string toString = "";
            toString += x;
            toString += " ";
            toString += y;
            toString += " ";
            UnityEngine.Debug.Log(toString);
            /*Material newMat = Resources.Load("green", typeof(Material)) as Material;
            missile.GetComponent<Renderer>().material = newMat;
            missile = Instantiate(missile, new Vector3(0, 0, 0), Quaternion.identity);
            newMat = Resources.Load("white", typeof(Material)) as Material;
            missile.GetComponent<Renderer>().material = newMat;*/
            //currentMissile = missile;
            
        }
    }
    
    public void AITurn()
    {
        int x = UnityEngine.Random.Range(0, 10);
        int y = UnityEngine.Random.Range(0, 10);

        while(ArrayManager.placements[x, y] == -2)
        {
            x = UnityEngine.Random.Range(0, 10);
            y = UnityEngine.Random.Range(0, 10);
        }

        if (ArrayManager.placements[x, y] != -1)
        {
            AIScore += 1;
            changeUIText.UpdateAIScore(AIScore);
            if (AIScore >= 17)
            {
                SceneManager.LoadScene(4);
            }
            // place correct AI missile
            missileAI = Instantiate(missile, new Vector3((float)(x + 1) , .5f, (float)(y + 1)), Quaternion.identity);
            Material newMat = Resources.Load("green", typeof(Material)) as Material;
            missileAI.GetComponent<Renderer>().material = newMat;
            //AIScoreUpdateText.text = "AI Score: " + AIScore;

            // to prevent ai from choosing the same location
            ArrayManager.placements[x, y] = -2;
            
            AITurn();
        }
        else
        {
            // place incorrect AI missile
            missileAI = Instantiate(missile, new Vector3((float)(x + 1), .5f, (float)(y + 1)), Quaternion.identity);
            Material newMat = Resources.Load("red", typeof(Material)) as Material;
            missileAI.GetComponent<Renderer>().material = newMat;
            
            // give player their next missile
            missile = Instantiate(missile, new Vector3(0, 0, 0), Quaternion.identity);
            newMat = Resources.Load("red", typeof(Material)) as Material;
            missile.GetComponent<Renderer>().material = newMat;
        }
    }

    public void missileCheck(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ArrayManager.placementsAI[x, y] != -1)
            {
                playerMiss.SetActive(false);
                playerHit.SetActive(true);
                playerScore += 1;
                changeUIText.UpdatePlayerScore(playerScore);
                //playerScoreUpdateText.text = "Player Score: " + playerScore;

                Material newMat = Resources.Load("green", typeof(Material)) as Material;
                missile.GetComponent<Renderer>().material = newMat;
                missile = Instantiate(missile, new Vector3(0, 0, 0), Quaternion.identity);
                newMat = Resources.Load("red", typeof(Material)) as Material;
                missile.GetComponent<Renderer>().material = newMat;
            }
            else
            {
                playerHit.SetActive(false);
                playerMiss.SetActive(true);
                //Material newMat = Resources.Load("red", typeof(Material)) as Material;
                //missile.GetComponent<Renderer>().material = newMat;
                AITurn();
            }
        }
    }
}
