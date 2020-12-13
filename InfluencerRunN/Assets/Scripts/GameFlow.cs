using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class GameFlow : MonoBehaviourPunCallbacks
{
    public static GameFlow instance;
    PhotonView PV;
    public Transform groundObj;
    private Vector3 nextGroundSpawn;
    public Transform[] Obstacles;
    private int randSecondSpawn;
    public GameObject PlayerContainer;
    public Transform Incrocio;
    public GameObject IncrocioinGame;
    public GameObject Menu;
    public GameObject PointsPanel;
    public Text PointsTXT, PointsMenuTXT;
    public static float Z = 20;
    public static bool changeDir = false;
    public static int DirGrades = 0;
    public bool move = true;
    private int counter = 0;
    private float points = 0;
    [SerializeField] public List<SpawnPoints> spawnPoints = new List<SpawnPoints>();
    private bool spawned = false;
    private int numberPlayers;
    Player[] allPlayers;

    private void Awake()
    {
        instance = this;
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        nextGroundSpawn.z += 80;
        Menu.SetActive(false);
        allPlayers = PhotonNetwork.PlayerList;

        foreach(Player p in allPlayers)
        {
            if (p != PhotonNetwork.LocalPlayer)
                numberPlayers++;
        }
        for(int i = 0; i < allPlayers.Length; i++)
        {
            if(allPlayers[i] == PhotonNetwork.LocalPlayer)
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerContainer"), spawnPoints[i].gameObject.transform.position, Quaternion.identity);
        }

        if(PhotonNetwork.IsMasterClient)
        StartCoroutine(spawnGround());
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine && PhotonNetwork.IsMasterClient)
        {
            points += Time.deltaTime * 2;
            PointsTXT.text = ((int)points).ToString();

            if (Menu.activeInHierarchy)
            {
                PointsMenuTXT.text = PointsTXT.text;
                PointsPanel.SetActive(false);
            }
            else
            {
                PointsPanel.SetActive(true);
            }

            if (changeDir)
            {
                StartCoroutine(changeDirMovement());
            }
        }
    }

    IEnumerator changeDirMovement()
    {
        changeDir = false;
        
        yield return new WaitForSeconds(1);
        counter = 0;
    }

    private Vector3 Rand(GameObject obj)
    {
        int x = Random.Range(0, obj.transform.childCount);

        return obj.transform.GetChild(x).transform.position;
    }

    private int RandZ()
    {
        int randZ = Random.Range(-1, 2);
        if (randZ == -1) randZ = -5;
        if (randZ == 1) randZ = 5;

        return randZ;
    }

    private int RandSpawnTrap()
    {
        int rand = Random.Range(0, 6);
        if (rand == 0 || rand == 1 || rand == 2) rand = 0;
        else rand = 1;

        return rand;
    }

    private string RO()
    {
        int i = Random.Range(0, Obstacles.Length -1);

        switch (i)
        {
            case 0:
                return "Bricks";
                break;

            case 1:
                return "Obstacle";
                break;

            case 2:
                return "Rock";
                break;

            case 3:
                return "Trap";
                break;
            default:
                return "Obstacle";
                break;
        }
    }

    
    IEnumerator spawnGround()
    {
        yield return new WaitForSeconds(1);

        if(counter == 10)
        {
            counter += 1;
            IncrocioinGame = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Incrocio"), new Vector3(nextGroundSpawn.x, nextGroundSpawn.y, nextGroundSpawn.z + 15), Incrocio.rotation).gameObject;

            Z = 90;
            nextGroundSpawn.z += Z;
        }

        if (counter < 10)
        {
            GameObject ground;

            ground = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ground"), nextGroundSpawn, groundObj.rotation);
            counter += 1;
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            if (randSecondSpawn == 1)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            }
            if (RandSpawnTrap() == 0)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(-8, Obstacles[3].position.y, nextGroundSpawn.z), Obstacles[3].rotation);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(2.75f, Obstacles[3].position.y, nextGroundSpawn.z), Obstacles[3].rotation);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(-2.75f, Obstacles[3].position.y, nextGroundSpawn.z), Obstacles[3].rotation);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(8, Obstacles[3].position.y, nextGroundSpawn.z), Obstacles[3].rotation);
            }

            
            Z = 20;
            nextGroundSpawn.z += Z;

            ground = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ground"), nextGroundSpawn, groundObj.rotation);
            counter += 1;
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            if (randSecondSpawn == 1)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", RO()), Rand(ground), Quaternion.identity);
            }

            nextGroundSpawn.z += Z;
        }
        
            
        StartCoroutine(spawnGround());
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Launcher.instance.StartGame();
    }
}
