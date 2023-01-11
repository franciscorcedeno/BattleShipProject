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

public class CreateBoard : MonoBehaviour
{

    GameObject[] ships = new GameObject[5];
    GameObject[] shipsPreSelect = new GameObject[5];
    GameObject selectedShip;
    GameObject selectedPreSelect;
    //public int[,] placements = new int[10, 10];
    //public int[,] playerShipsLocation = new int[11, 11];
    //public int[,] aiShipsLocation = new int[11, 11];
    //public int[,] placementsAI = new int[10, 10];
    // vertical bool must be outside of update otherwise it gets set to true again
    public bool vertical = true;
    public int lengthShip = 0;

    // check if ship was placed
    bool mrvikPlaced = false;
    bool sterePlaced = false;
    bool adSergPlaced = false;
    bool iverPlaced = false;
    bool kuzPlaced = false;

    private PlayGame playGame;

    // How to count winning
    // Each ship has a length
    // Detect the square that the player clicked to set the ship
    // Knowing ships length and direction you can tell on which squares it was placed
    // Use an array to store which squares has a ship
    // this can be used for missile detection
    // total size of array is total amount needed to win



    // Start is called before the first frame update
    void Start()
    {
        /*for (int i = 0; i < 11; i++)
        {
            for(int j = 0; j < 11; j++)
            {
                playerShipsLocation[]
            }
            
        }*/

        //accessPlayGame();

        selectedShip = new GameObject();
        //store ships in global array
        ships[0] = GameObject.Find("206MRVikhrIFQ");
        ships[1] = GameObject.Find("Steregushchiy");
        ships[2] = GameObject.Find("AdmiralSergeyGorshkov");
        ships[3] = GameObject.Find("IverHuitfeldt");
        ships[4] = GameObject.Find("AdmiralKuznetsov");

        //shipPreSelect
        shipsPreSelect[0] = GameObject.Find("206MRVikhrlFQPreSelect");
        shipsPreSelect[1] = GameObject.Find("SteregushchiyPreSelect");
        shipsPreSelect[2] = GameObject.Find("AdmiralSergeyGorshkovPreSelect");
        shipsPreSelect[3] = GameObject.Find("IverHuitfeldtPreSelect");
        shipsPreSelect[4] = GameObject.Find("AdmiralKuznetsovPreSelect");

        //set ship false
        ships[0].SetActive(false);
        ships[1].SetActive(false);
        ships[2].SetActive(false);
        ships[3].SetActive(false);
        ships[4].SetActive(false);

        //set preselects false
        shipsPreSelect[0].SetActive(false);
        shipsPreSelect[1].SetActive(false);
        shipsPreSelect[2].SetActive(false);
        shipsPreSelect[3].SetActive(false);
        shipsPreSelect[4].SetActive(false);

        //player
        for (int i = 1; i < 11; i++)
        {
            for (int j = 1; j < 11; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(.9f, .1f, .9f);
                cube.transform.position = new Vector3(j, 0, i);
                //cube.GetComponent<MeshRenderer>().material = Resources.Load<Material>("White");
                cube.name = "TilePlayer";
                //placements[j - 1, i - 1] = -1;

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
                //placementsAI[(j + 1) * (-1), (i + 1) * (-1)] = -1;

                GameObject coordinates = new GameObject();
                TMPro.TextMeshPro t = coordinates.AddComponent<TMPro.TextMeshPro>();
                t.text = "[" + (j * -1f) + ", " + (i * -1f) + "]";
                t.transform.position = new Vector3(((j * -1) - 11f + 9.6f), .1f, ((i * -1) - 2.3f));
                t.transform.localEulerAngles = new Vector3(90, 0, 0);
                t.color = Color.black;
                t.fontSize = 2.35f;

            }

        }

        placeAIShips();
    }

    public void AIDirection()
    {
        int x = UnityEngine.Random.Range(1, 3);
        if (x == 1)
        {
            vertical = true;
        }
        else if (x == 2)
        {
            vertical = false;
        }
    }

    public bool AICollision(int x, int y, int index)
    {
        for (int i = 0; i < lengthShip; i++)
        {
            //UnityEngine.Debug.Log(y);
            if (vertical)
            {
                //allow reposition if space is empty or previous position of ship you are trying to place
                if (ArrayManager.placementsAI[x, y + i] != -1)
                {
                    return true;
                }
            }
            else
            {
                if (ArrayManager.placementsAI[x + i, y] != -1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void placeAIShips()
    {
        //Random rnd = new Random();
        int x = -1;
        int y = -1;
        bool AIMrvikPlaced = false;
        bool AISterePlaced = false;
        bool AIAdSergPlaced = false;
        bool AIIverPlaced = false;
        bool AIKuzPlaced = false;

        //mrvik
        while(!AIMrvikPlaced)
        {
            AIDirection();
            lengthShip = 2;
            //using the length of the ship and direction
            //max random int will be bottom cube of preSelect
            if (vertical)
            {
                x = UnityEngine.Random.Range(0, 10);
                y = UnityEngine.Random.Range(0, 9);
            }
            else
            {
                x = UnityEngine.Random.Range(0, 9);
                y = UnityEngine.Random.Range(0, 10);
            }
            //UnityEngine.Dubug.Log(x);
            //UnityEngine.Dubug.Log(y);
            if (!AICollision(x, y, 0))
            {
                for(int i = 0; i < lengthShip; i++)
                {
                    if (vertical)
                    {
                        ArrayManager.placementsAI[x, y + i] = 0;
                    }
                    else
                    {
                        ArrayManager.placementsAI[x + i, y] = 0;
                    }
                }
                /*for (int i = 0; i < 10; i++)
                {
                    string toString = "";
                    for (int j = 0; j < 10; j++)
                    {
                        toString += placementsAI[j, i];
                    }
                    UnityEngine.Debug.Log(toString);
                }*/
                AIMrvikPlaced = true;
            }
            
        }

        //stere
        while (!AISterePlaced)
        {
            AIDirection();
            lengthShip = 3;
            if (vertical)
            {
                x = UnityEngine.Random.Range(0, 10);
                y = UnityEngine.Random.Range(0, 8);
            }
            else
            {
                x = UnityEngine.Random.Range(0, 8);
                y = UnityEngine.Random.Range(0, 10);
            }
            if (!AICollision(x, y, 1))
            {
                for (int i = 0; i < lengthShip; i++)
                {
                    if (vertical)
                    {
                        ArrayManager.placementsAI[x, y + i] = 1;
                    }
                    else
                    {
                        ArrayManager.placementsAI[x + i, y] = 1;
                    }
                }
                AISterePlaced = true;
            }

        }

        //adserg
        while (!AIAdSergPlaced)
        {
            AIDirection();
            lengthShip = 3;
            if (vertical)
            {
                x = UnityEngine.Random.Range(0, 10);
                y = UnityEngine.Random.Range(0, 8);
            }
            else
            {
                x = UnityEngine.Random.Range(0, 8);
                y = UnityEngine.Random.Range(0, 10);
            }
            if (!AICollision(x, y, 2))
            {
                for (int i = 0; i < lengthShip; i++)
                {
                    if (vertical)
                    {
                        ArrayManager.placementsAI[x, y + i] = 2;
                    }
                    else
                    {
                        ArrayManager.placementsAI[x + i, y] = 2;
                    }
                }
                AIAdSergPlaced = true;
            }
        }

        //iver
        while (!AIIverPlaced)
        {
            AIDirection();
            lengthShip = 4;
            if (vertical)
            {
                x = UnityEngine.Random.Range(0, 10);
                y = UnityEngine.Random.Range(0, 7);
            }
            else
            {
                x = UnityEngine.Random.Range(0, 7);
                y = UnityEngine.Random.Range(0, 10);
            }
            
            if (!AICollision(x, y, 3))
            {
                for (int i = 0; i < lengthShip; i++)
                {
                    if (vertical)
                    {
                        ArrayManager.placementsAI[x, y + i] = 3;
                    }
                    else
                    {
                        ArrayManager.placementsAI[x + i, y] = 3;
                    }
                }
                AIIverPlaced = true;
            }
        }

        //kuz
        while (!AIKuzPlaced)
        {
            AIDirection();
            lengthShip = 5;
            if (vertical)
            {
                x = UnityEngine.Random.Range(0, 10);
                y = UnityEngine.Random.Range(0, 6);
            }
            else
            {
                x = UnityEngine.Random.Range(0, 6);
                y = UnityEngine.Random.Range(0, 10);
            }
            if (!AICollision(x, y, 4))
            {
                for (int i = 0; i < lengthShip; i++)
                {
                    if (vertical)
                    {
                        ArrayManager.placementsAI[x, y + i] = 4;
                    }
                    else
                    {
                        ArrayManager.placementsAI[x + i, y] = 4;
                    }
                }
                AIKuzPlaced = true;
            }
        }

        for (int i = 0; i < 10; i++)
        {
            string toString = "";
            for (int j = 0; j < 10; j++)
            {
                toString += ArrayManager.placementsAI[j, i];
            }
            UnityEngine.Debug.Log(toString);
        }

        // leave lengthship ready for player
        lengthShip = 0;
        // leave verical ready for player to use
        vertical = true;
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(ray, out hitInfo);
        int x = (int)hitInfo.point.x;
        int z = (int)hitInfo.point.z;
        float shipX = x;
        float shipZ = z;
        float y = .1f;
        int playerPlacementX = (int)shipX;
        int playerPlacementZ = (int)shipZ;
        // index of ship
        int index = -1;
        if (hit && hitInfo.transform.name == "TilePlayer")
        {
            if(selectedPreSelect != null)
            {
                selectedPreSelect.SetActive(true);
                selectedPreSelect.layer = LayerMask.NameToLayer("Ignore Raycast");

                //Press R to rotate
                if (Input.GetKeyDown(KeyCode.R) && vertical)
                {
                    vertical = false;
                    selectedPreSelect.transform.rotation = Quaternion.Euler(0, 90f, 0);
                }
                else if (Input.GetKeyDown(KeyCode.R) && !vertical)
                {
                    vertical = true;
                    selectedPreSelect.transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                // To check that ships that get placed on even number of squares
                // gets placed correctly
                if (selectedPreSelect == shipsPreSelect[0] || selectedPreSelect == shipsPreSelect[3])
                {

                    if (vertical)
                    {
                        if (selectedPreSelect == shipsPreSelect[0])
                        {
                            lengthShip = 2;
                            // index of ship
                            index = 0;
                            if (shipZ < 1)
                            {
                                shipZ = 1f + .4F;
                                if (shipX < 1)
                                {
                                    shipX = 1f;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 1;
                                //addMrvikY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else if (shipZ > 9)
                            {
                                shipZ = 9f + .4f;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 9;
                                //UnityEngine.Debug.Log(playerPlacementZ);
                                //addMrvikY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                shipZ = z + .4f;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                //UnityEngine.Debug.Log(playerPlacementZ);
                                //playerPlacementZ = (int)(shipZ - .4f);
                                //addMrvikY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }
                        else if (selectedPreSelect == shipsPreSelect[3])
                        {
                            lengthShip = 4;
                            index = 3;
                            if (shipZ < 3)
                            {
                                shipZ = 2f + .4F;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 1;
                                //addIverY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else if (shipZ > 8)
                            {
                                shipZ = 8f + .4f;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 7;
                                //UnityEngine.Debug.Log(shipZ);
                                //UnityEngine.Debug.Log(playerPlacementZ);
                                //addIverY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                shipZ = z + .4f;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = z - 1;
                                //addIverY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }
                    }

                    else
                    {
                        if (selectedPreSelect == shipsPreSelect[0])
                        {
                            lengthShip = 2;
                            index = 0;
                            if (shipX < 1)
                            {
                                shipX = 1f + .35F;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 1;
                                //addMrvikX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);

                            }
                            else if (shipX > 9)
                            {
                                shipX = 9f + .35f;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 9;
                                //addMrvikX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                shipX = x + .35f;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                //addMrvikX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }
                        if (selectedPreSelect == shipsPreSelect[3])
                        {
                            lengthShip = 4;
                            index = 3;
                            if (shipX < 2)
                            {
                                shipX = 2f + .35F;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 1;
                                //addIverX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else if (shipX > 8)
                            {
                                shipX = 8f + .35f;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 7;
                                //addIverX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                shipX = x + .35f;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = x - 1;
                                //addIverX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }

                    }

                    /*if (Input.GetMouseButtonDown(0))
                    {
                        selectedShip.SetActive(true);
                        selectedShip.transform.position = selectedPreSelect.transform.position;
                        selectedShip.transform.rotation = selectedPreSelect.transform.rotation;
                    }*/

                }
                else
                {
                    if (vertical)
                    {
                        if (selectedPreSelect == shipsPreSelect[1])
                        {
                            lengthShip = 3;
                            index = 1;
                            if (shipZ < 3)
                            {
                                shipZ = 2;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 1;
                                //addAdSergY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else if (shipZ > 8)
                            {
                                shipZ = 9;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 8;
                                //UnityEngine.Debug.Log(playerPlacementZ);
                                //addAdSergY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = z - 1;
                                //UnityEngine.Debug.Log(shipZ);
                                //UnityEngine.Debug.Log(playerPlacementZ);
                                //addAdSergY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }
                        else if (selectedPreSelect == shipsPreSelect[2])
                        {
                            lengthShip = 3;
                            index = 2;
                            if (shipZ < 2)
                            {
                                shipZ = 2;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 1;
                                //addAdSergY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else if (shipZ > 8)
                            {
                                shipZ = 9;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 8;
                                //addAdSergY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = z - 1;
                                //addAdSergY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }
                        else if (selectedPreSelect == shipsPreSelect[4])
                        {
                            lengthShip = 5;
                            index = 4;
                            if (shipZ < 4)
                            {
                                shipZ = 3;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 1;
                                //addKuzY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else if (shipZ > 8)
                            {
                                shipZ = 8;
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = 6;
                                //addKuzY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                if (shipX < 1)
                                {
                                    shipX = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementZ = z - 2;
                                //addKuzY(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }
                    }
                    else
                    {
                        if (selectedPreSelect == shipsPreSelect[1])
                        {
                            lengthShip = 3;
                            index = 1;
                            if (shipX < 2)
                            {
                                shipX = 2;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 1;
                                //addStereX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else if (shipX > 9)
                            {
                                shipX = 9;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 8;
                                //addStereX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = x - 1;
                                //addStereX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }
                        else if (selectedPreSelect == shipsPreSelect[2])
                        {
                            lengthShip = 3;
                            index = 2;
                            if (shipX < 2)
                            {
                                shipX = 2;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 1;
                                //addAdSergX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else if (shipX > 9)
                            {
                                shipX = 9;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 8;
                                //addAdSergX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = x - 1;
                                //addAdSergX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }
                        else if (selectedPreSelect == shipsPreSelect[4])
                        {
                            lengthShip = 5;
                            index = 4;
                            if (shipX < 3)
                            {
                                shipX = 3;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 1;
                                //addKuzX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else if (shipX > 8)
                            {
                                shipX = 8;
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = 6;
                                //addKuzX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                            else
                            {
                                if (shipZ < 1)
                                {
                                    shipZ = 1;
                                }
                                selectedPreSelect.transform.position = new Vector3(shipX, y, shipZ);
                                playerPlacementX = x - 2;
                                //addKuzX(playerPlacementX, playerPlacementZ);
                                placeShip(playerPlacementX, playerPlacementZ, index);
                            }
                        }
                    }

                    /*if (Input.GetMouseButtonDown(0))
                    {
                        selectedShip.SetActive(true);
                        selectedShip.transform.position = selectedPreSelect.transform.position;
                        selectedShip.transform.rotation = selectedPreSelect.transform.rotation;
                    }*/
                }
                /*
                if (Input.GetMouseButtonDown(0))
                {
                    //if (!collision(playerPlacementX, playerPlacementZ))
                    //{
                    selectedShip.SetActive(true);
                    selectedShip.transform.position = selectedPreSelect.transform.position;
                    selectedShip.transform.rotation = selectedPreSelect.transform.rotation;
                    //}

                }*/
            }
        }
        
        /*if (mrvikPlaced && sterePlaced && adSergPlaced && iverPlaced && kuzPlaced)
        {
            for (int i = 0; i < 10; i++)
            {
                string toString = "";
                for (int j = 0; j < 10; j++)
                {
                    toString += placements[j, i];
                }
                UnityEngine.Debug.Log(toString);
            }
            mrvikPlaced = false;
        }*/  
            
    }

    // Lower limits pass as 0 for some reason so this check
    // is to set them to 1
    // ships would get placed one unity above on unit in
    // the direction they faced for some reason so now i
    // also check incase it is more than 10

    /*void accessPlayGame()
    {
        playGame = GameObject.FindObjectOfType<PlayGame>();
    }*/

    public int checkLimits(int x)
    {
        if (x <= 0)
        {
            x = 1;
            return x;
        }
        return x;
    }

    // resets the tiles with value of given index to -1
    public void reposition(int index)
    {
        for (int i=0; i <10; i++)
        {
            for (int j=0; j<10; j++)
            {
                if(ArrayManager.placements[j, i] == index)
                {
                    ArrayManager.placements[j, i] = -1;
                }
            }
        }
    }

    public bool collision(int x, int y, int index)
    {
        for (int i = 0; i < lengthShip; i++)
        {
            //UnityEngine.Debug.Log(y);
            if (vertical)
            {
                //allow reposition if space is empty or previous position of ship you are trying to place
                if (ArrayManager.placements[x, y + i] != -1 && ArrayManager.placements[x, y + i] != index)
                {
                    return true;
                }
            }
            else
            {
                if (ArrayManager.placements[x + i, y] != -1 && ArrayManager.placements[x + i, y] != index)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void placeShipVisually()
    {
        //if (!collision(playerPlacementX, playerPlacementZ))
        //{
        selectedShip.SetActive(true);
        selectedShip.transform.position = selectedPreSelect.transform.position;
        selectedShip.transform.rotation = selectedPreSelect.transform.rotation;
        //}
    }

    // calculates value of tiles
    public void placeShip(int x, int y, int index)
    {
        if (Input.GetMouseButtonDown(0))
        {

            int checkedX = checkLimits(x) - 1;
            int checkedY = checkLimits(y) - 1;
            if (!collision(checkedX, checkedY, index))
            {
                if (index == 0)
                {
                    mrvikPlaced = true;
                }
                if (index == 1)
                {
                    sterePlaced = true;
                }
                if (index == 2)
                {
                    adSergPlaced = true;
                }
                if (index == 3)
                {
                    iverPlaced = true;
                }
                if (index == 4)
                {
                    kuzPlaced = true;
                }
            }
            if (vertical)
            {
                UnityEngine.Debug.Log(collision(checkedX, checkedY, index));
                if (!collision(checkedX, checkedY, index))
                {
                    //string toString = "";
                    reposition(index);
                    for (int i = 0; i < lengthShip; i++)
                    {
                        ArrayManager.placements[checkedX, checkedY + i] = index;
                        /*int testY = checkedY + i;
                        toString += checkedX;
                        toString += " ";
                        toString += testY;
                        toString += " ";
                        UnityEngine.Debug.Log(toString);*/
                    }
                    //UnityEngine.Debug.Log(toString);
                    placeShipVisually();
                }
            }
            else
            {
                if (!collision(checkedX, checkedY, index))
                {
                    //string toString = "";
                    reposition(index);
                    for (int i = 0; i < lengthShip; i++)
                    {
                        ArrayManager.placements[checkedX + i, checkedY] = index;
                        /*int testX = checkedX + i;
                        toString += testX;
                        toString += " ";
                        toString += checkedY;
                        toString += " ";*/
                    }
                    //UnityEngine.Debug.Log(toString);
                    placeShipVisually();
                }
            }
        }
    }

    /*
    public void addMrvikX(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!collision(x, y))
            {
                mrvikPlaced = true;
                int index = 0;
                reposition(index);
                placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
                placements[checkLowerLimit(x), checkLowerLimit(y) - 1] = index;
            }
                
        }
        
    }

    public void addStereX(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            sterePlaced = true;
            int index = 1;
            reposition(index);
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x), checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) + 1, checkLowerLimit(y) - 1] = index;
        }
    }

    public void addAdSergX(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            adSergPlaced = true;
            int index = 2;
            reposition(index);
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x), checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) + 1, checkLowerLimit(y) - 1] = index;
        }
    }

    public void addIverX(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            iverPlaced = true;
            int index = 3;
            reposition(index);
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x), checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) + 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) + 2, checkLowerLimit(y) - 1] = index;
        }
    }

    public void addKuzX(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            kuzPlaced = true;
            int index = 4;
            reposition(index);
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x), checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) + 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) + 2, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) + 3, checkLowerLimit(y) - 1] = index;
        }
    }

    public void addMrvikY(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            mrvikPlaced = true;
            /*string testString = "";
            testString = x + " " + y;
            UnityEngine.Debug.Log(testString);
            testString = x + " " + y;
            UnityEngine.Debug.Log(testString);
            end star comment here //
            // index of ship
            int index = 0;
            reposition(index);
            /*for (int i=0; i <10; i++)
            {
                //string toString = "";
                for (int j=0; j<10; j++)
                {
                    //UnityEngine.Debug.Log(placements[j, i] + " ");
                    // since ship is getting replaced remove its placement from all other board tiles
                    // Check to see which are 0 since that is the value for this ship
                    //toString += placements[j, i];
                    // checks for index of ship to reset the tiles where it was placed
                    if(placements[j, i] == index)
                    {
                        placements[j, i] = -1;
                    }
                }
                //UnityEngine.Debug.Log(toString);
            }end star comment here //
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y)] = index;
        }

    }

    public void addStereY(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            sterePlaced = true;
            int index = 1;
            reposition(index);
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y)] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) + 1] = index;
        }
    }

    public void addAdSergY(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            adSergPlaced = true;
            int index = 2;
            reposition(index);
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y)] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) + 1] = index;
        }
    }

    public void addIverY(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            iverPlaced = true;
            int index = 3;
            reposition(index);
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y)] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) + 1] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) + 2] = index;
        }
    }

    public void addKuzY(int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            kuzPlaced = true;
            int index = 4;
            reposition(index);
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) - 1] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y)] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) + 1] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) + 2] = index;
            placements[checkLowerLimit(x) - 1, checkLowerLimit(y) + 3] = index;
        }
    }
    */

    public void mrvikButton()
    {
        // if statement avoids no instance of an object when trying to setActive(false)
        // when a button is clicked for the first time there is nothing to setActive(false)
        if (selectedPreSelect != null)
        {
            selectedPreSelect.transform.rotation = Quaternion.Euler(0, 0, 0);
            selectedPreSelect.SetActive(false);
        }
        vertical = true;
        selectedPreSelect = shipsPreSelect[0];
        selectedShip = ships[0];
        /*shipsPreSelect[0].SetActive(true);
        shipsPreSelect[1].SetActive(false);
        shipsPreSelect[2].SetActive(false);
        shipsPreSelect[3].SetActive(false);
        shipsPreSelect[4].SetActive(false);*/
    }

    public void stereButton()
    {
        if (selectedPreSelect != null)
        {
            selectedPreSelect.transform.rotation = Quaternion.Euler(0, 0, 0);
            selectedPreSelect.SetActive(false);
        }
        vertical = true;
        selectedPreSelect = shipsPreSelect[1];
        selectedShip = ships[1];
        /*
        shipsPreSelect[0].SetActive(false);
        shipsPreSelect[1].SetActive(true);
        shipsPreSelect[2].SetActive(false);
        shipsPreSelect[3].SetActive(false);
        shipsPreSelect[4].SetActive(false);*/
    }

    public void adsergButton()
    {
        if (selectedPreSelect != null)
        {
            selectedPreSelect.transform.rotation = Quaternion.Euler(0, 0, 0);
            selectedPreSelect.SetActive(false);
        }
        vertical = true;
        selectedPreSelect = shipsPreSelect[2];
        selectedShip = ships[2];
        /*
        shipsPreSelect[0].SetActive(false);
        shipsPreSelect[1].SetActive(false);
        shipsPreSelect[2].SetActive(true);
        shipsPreSelect[3].SetActive(false);
        shipsPreSelect[4].SetActive(false);*/
    }

    public void iverButton()
    {
        if (selectedPreSelect != null)
        {
            selectedPreSelect.transform.rotation = Quaternion.Euler(0, 0, 0);
            selectedPreSelect.SetActive(false);
        }
        vertical = true;
        selectedPreSelect = shipsPreSelect[3];
        selectedShip = ships[3];
        /*
        shipsPreSelect[0].SetActive(false);
        shipsPreSelect[1].SetActive(false);
        shipsPreSelect[2].SetActive(false);
        shipsPreSelect[3].SetActive(true);
        shipsPreSelect[4].SetActive(false);*/
    }

    public void kuzButton()
    {
        if (selectedPreSelect != null)
        {
            selectedPreSelect.transform.rotation = Quaternion.Euler(0, 0, 0);
            selectedPreSelect.SetActive(false);
        }
        vertical = true;
        selectedPreSelect = shipsPreSelect[4];
        selectedShip = ships[4];
        /*
        shipsPreSelect[0].SetActive(false);
        shipsPreSelect[1].SetActive(false);
        shipsPreSelect[2].SetActive(false);
        shipsPreSelect[3].SetActive(false);
        shipsPreSelect[4].SetActive(true);*/
    }

    public void StartGame()
    {
        if (mrvikPlaced && sterePlaced && adSergPlaced && iverPlaced && kuzPlaced)
        {
            /*for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    playGame.setPlacements(j, i, placements[j, i]);
                    playGame.setAIPlacements(j, i, placementsAI[j, i]);
                }
            }*/
            SceneManager.LoadScene(2);
        }
    }

}
