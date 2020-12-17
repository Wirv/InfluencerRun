using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using TMPro;

public enum Direzione
{
    Nord = 0,
    Ovest = -1,
    Est = 1,
    Sud = 2,
    None,
    sinistra,
    destra
}

public class GameFlow : MonoBehaviourPunCallbacks
{
    public static GameFlow instance;
    PhotonView PV;
    [SerializeField] public static Direzione direction = Direzione.Nord;
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
    public static bool start = false;

    private int countdown = 3;
    [SerializeField] TMP_Text countDownTXT;

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

        StartCoroutine(contoAllaRovescia());
        if(PhotonNetwork.IsMasterClient)
        StartCoroutine(spawnGround());
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
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
        CalculateNextGround(60);
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
                if (start == true)
                    return "Rock";
                else return "Obstacle";
                break;

            case 3:
                return "Trap";
                break;
            default:
                return "Obstacle";
                break;
        }
    }

    private void CalculateNextGround(float x)
    {
        switch (direction)
        {
            case Direzione.Nord:
                nextGroundSpawn.z += x;
                break;

            case Direzione.Ovest:
                nextGroundSpawn.x -= x;
                break;

            case Direzione.Est:
                nextGroundSpawn.x += x;
                break;

            case Direzione.Sud:
                nextGroundSpawn.z -= x;
                break;
        }
    }

    IEnumerator contoAllaRovescia()
    {

        countDownTXT.text = countdown.ToString();

        yield return new WaitForSeconds(1);

        countdown -= 1;

        if (countdown == 0) 
        {
            Destroy(countDownTXT.gameObject);
            start = true;
        }
        else
        {
            StartCoroutine(contoAllaRovescia());
        }
    }
    
    IEnumerator spawnGround()
    {
        yield return new WaitForSeconds(1);

        if(counter == 10)
        {
            CalculateNextGround(Z);
            counter += 1;
            IncrocioinGame = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Incrocio"), new Vector3(nextGroundSpawn.x, nextGroundSpawn.y, nextGroundSpawn.z), SpawnRotation()).gameObject;
        
            Z = 0;
            CalculateNextGround(Z);
        }

        if (counter < 10)
        {
            Spawning();        
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

    void Spawning()
    {
        GameObject ground;

        ground = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ground"), nextGroundSpawn, SpawnRotation());
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
            SpawnTraps();
        }

        if (RandSpawnTrap() == 0)
        {
            SpawnSpeedBoost();
        }

        Z = 20;
        CalculateNextGround(Z);

        ground = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ground"), nextGroundSpawn, SpawnRotation());
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

        CalculateNextGround(Z);

    }

    private Quaternion SpawnRotation()
    {
        switch (direction)
        {
            case Direzione.Nord:
                return Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case Direzione.Ovest:
                return Quaternion.Euler(new Vector3(0, -90, 0));
                break;

            case Direzione.Est:
                return Quaternion.Euler(new Vector3(0, 90, 0));
                break;

            case Direzione.Sud:
                return Quaternion.Euler(new Vector3(0, 180, 0));
                break;
            default:
                return Quaternion.Euler(new Vector3(0, 0, 0));
                break;
        }
    }

    private void SpawnSpeedBoost()
    {
        switch (direction)
        {
            case Direzione.Nord:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x - 8, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 2.75f, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x - 2.75f, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 8, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                break;

            case Direzione.Ovest:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 0.15f, 0.5f, nextGroundSpawn.z - 8), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 0.15f, 0.5f, nextGroundSpawn.z + 2.75f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 0.15f, 0.5f, nextGroundSpawn.z - 2.75f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 0.15f, 0.5f, nextGroundSpawn.z + 8), Quaternion.identity);
                break;

            case Direzione.Est:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 0.15f, 0.5f, nextGroundSpawn.z - 8), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 0.15f, 0.5f, nextGroundSpawn.z + 2.75f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 0.15f, 0.5f, nextGroundSpawn.z - 2.75f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 0.15f, 0.5f, nextGroundSpawn.z + 8), Quaternion.identity);
                break;

            case Direzione.Sud:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x - 8, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 2.75f, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x - 2.75f, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 8, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                break;
            default:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x - 8, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 2.75f, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x - 2.75f, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SpeedBoost"), new Vector3(nextGroundSpawn.x + 8, 0.5f, nextGroundSpawn.z + 0.15f), Quaternion.identity);
                break;
        }
    }

    private void SpawnTraps()
    {
        
        switch (direction)
        {
            case Direzione.Nord:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x - 8, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x + 2.75f, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x - 2.75f, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x + 8, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                break;

            case Direzione.Ovest:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x, Obstacles[3].position.y, nextGroundSpawn.z - 8), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x, Obstacles[3].position.y, nextGroundSpawn.z + 2.75f), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x, Obstacles[3].position.y, nextGroundSpawn.z - 2.75f), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x, Obstacles[3].position.y, nextGroundSpawn.z + 8), SpawnRotation());
                break;

            case Direzione.Est:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x, Obstacles[3].position.y, nextGroundSpawn.z -8), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x, Obstacles[3].position.y, nextGroundSpawn.z +2.75f), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x, Obstacles[3].position.y, nextGroundSpawn.z -2.75f), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x, Obstacles[3].position.y, nextGroundSpawn.z +8), SpawnRotation());
                break;

            case Direzione.Sud:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x - 8, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x + 2.75f, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x - 2.75f, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x + 8, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                break;
            default:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x - 8, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x + 2.75f, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x - 2.75f, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Trap"), new Vector3(nextGroundSpawn.x + 8, Obstacles[3].position.y, nextGroundSpawn.z), SpawnRotation());
                break;
        }
    }
}
