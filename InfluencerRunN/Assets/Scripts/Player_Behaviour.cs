using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviourPun, IPunObservable
{
    PhotonView PV;

    public Transform posDesignata;
    public Transform posPrecedente;
    public static Player_Behaviour instance;
    public Rigidbody rb;
    public Animator myAnim;
    public bool jump = false;
    public bool movement = false;
    public bool slide = false;
    public float force;
    public float speed = 8;
    public float forceJumpUp = 2;
    public float forceJumpDown = -2;
    public float timingJup = 1;
    public float timingJdown = .5f;
    public float timingSlideIn = 1;
    public float timingSlideOut = 0.20f;
    public bool GameOver = false;
    public CamMove camera;
    public int posizioneGara = 1;

    private Vector2 startTouch, swipeDelta;
    private bool tap = false, isDraging = false;

    public Vector3 networkPosition;
    public Quaternion networkRotation;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        PV = gameObject.GetComponent<PhotonView>();

    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            posDesignata = camera.PosC.transform;
            startTouch = swipeDelta = Vector2.zero;
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    public Vector2 SwipeDelta { get { return swipeDelta; } }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;

        if (!GameOver && PV.IsMine)
        {
            Debug.Log("move");
            Move();
            MoveTouch();
        }

    }

    
    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, posDesignata.transform.position, force * Time.deltaTime);

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && jump == false && movement == false && slide==false)
        {
            if (posDesignata == camera.PosD.transform)
            {
                posDesignata = camera.PosC.transform;

                movement = true;
                StartCoroutine(StopMovement());
            }
            else if (posDesignata == camera.PosC.transform)
            {
                posDesignata = camera.PosS.transform;
                movement = true;
                StartCoroutine(StopMovement());
            }

        }

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && jump == false && movement == false && slide == false)
        {
            if (posDesignata == camera.PosS.transform)
            {
                posDesignata = camera.PosC.transform;

                movement = true;
                StartCoroutine(StopMovement());
            }
            else if (posDesignata == camera.PosC.transform)
            {
                posDesignata = camera.PosD.transform;
                movement = true;
                StartCoroutine(StopMovement());
            }

        }


        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && jump == false && movement == false && slide==false)
        {
            jump = true;
            posPrecedente = posDesignata;
            posDesignata = camera.Jumping(posDesignata);
            StartCoroutine(StopJump());
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && jump == false && movement == false && slide == false)
        {
            slide = true;
            
            StartCoroutine(StopSlide());
        }

    }

    private void MoveTouch()
    {
        #region StandaloneInputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            Reset();
        }
        #endregion

        #region MobileInputs
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }
        }
        #endregion

        //Calcolare la distanza
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }


        if (swipeDelta.magnitude > 125)
        {
            //Che Direzione?
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Sinistra o destra
                if (x < 0 && jump == false && movement == false && slide == false)
                {
                    //sinistra
                    if (posDesignata == camera.PosD.transform)
                    {
                        posDesignata = camera.PosC.transform;

                        movement = true;
                        StartCoroutine(StopMovement());
                    }
                    else if (posDesignata == camera.PosC.transform)
                    {
                        posDesignata = camera.PosS.transform;
                        movement = true;
                        StartCoroutine(StopMovement());
                    }
                }
                else if( x > 0 && jump == false && movement == false && slide == false)
                {
                    //destra
                    if (posDesignata == camera.PosS.transform)
                    {
                        posDesignata = camera.PosC.transform;

                        movement = true;
                        StartCoroutine(StopMovement());
                    }
                    else if (posDesignata == camera.PosC.transform)
                    {
                        posDesignata = camera.PosD.transform;
                        movement = true;
                        StartCoroutine(StopMovement());
                    }
                }
            }
            else
            {
                //Sopra o sotto
                if (y < 0 && jump == false && movement == false && slide == false)
                {
                    //sotto
                    slide = true;

                    StartCoroutine(StopSlide());
                }
                else if(y > 0 && jump == false && movement == false && slide == false)
                {
                    //sopra
                    jump = true;
                    posPrecedente = posDesignata;
                    posDesignata = camera.Jumping(posDesignata);
                    StartCoroutine(StopJump());
                }
            }

            Reset();
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }

    IEnumerator StopJump()
    {
        yield return new WaitForSeconds(timingJup);
        posDesignata = posPrecedente;
        yield return new WaitForSeconds(timingJdown);
        jump = false;
    }

    IEnumerator StopSlide()
    {
        myAnim.Play("PlayerSlide");
        yield return new WaitForSeconds(timingSlideIn);
        myAnim.Play("PlayerSlideOut");
        yield return new WaitForSeconds(timingSlideOut);
        slide = false;
    }

    IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(.5f);

        movement = false;
    }

    IEnumerator GOver()
    {
        GameOver = true;
        yield return new WaitForSeconds(1);

        GameFlow.instance.Menu.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Obstacle")
        {

            Destroy(collision.gameObject);
            if (PV.IsMine)
            {
                force = 400;
                StartCoroutine(Hit());
            }
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log(other.gameObject.name);
            if(posizioneGara > 1)
                other.gameObject.transform.parent.GetComponent<Player_Behaviour>().posizioneGara -= 1;
            
            if(posizioneGara < 4)
                posizioneGara += 1;

            for (int i = 0; i < transform.parent.transform.childCount; i++)
            {
                if (transform.parent.transform.GetChild(i).gameObject.name == "Destroyer")
                {
                    if(posizioneGara == 4)
                        transform.parent.transform.GetChild(i).gameObject.SetActive(true);
                    else
                        transform.parent.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

        }
    }

    public IEnumerator Hit()
    {
        yield return new WaitForSeconds(.2f);
        force = 500;

    }

    public IEnumerator SpeedBoost(float boostValue, float duration)
    {
        float oldSpeed = force;
        force += boostValue;
        if (PV.IsMine)
        {
            GameFlow.instance.imaginetest.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        force = oldSpeed;
        if (PV.IsMine)
        {
            GameFlow.instance.imaginetest.SetActive(false);
        }
    }
}
