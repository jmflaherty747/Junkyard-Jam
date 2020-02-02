using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region vars
    float speed = 5;
    bool isMoving;
    Grid gm;
    [Tooltip("0=none,1-4=Wheel,5=Battery,6-11=Canister")] [SerializeField] int heldItem; //0=none,1-4=Wheel,5=Battery,6-11=Canister


    public GameObject[] cars;
    public GameObject carUi;
    public Text carUiText;
    public GameObject car;

    #endregion

    #region movement functions
    IEnumerator MoveRight(Vector3 nextPos)
    {
        var speedVector = new Vector3(speed, 0, 0);
        transform.position += (speedVector * Time.deltaTime);
        if (transform.position.x < nextPos.x)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(MoveRight(nextPos));
        }
        else
        {
            transform.position = nextPos;
            isMoving = false;
        }
    }

    IEnumerator MoveLeft(Vector3 nextPos)
    {
        var speedVector = new Vector3(speed, 0, 0);
        transform.position -= (speedVector * Time.deltaTime);
        if (transform.position.x > nextPos.x)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(MoveLeft(nextPos));
        }
        else
        {
            transform.position = nextPos;
            isMoving = false;
        }
    }

    IEnumerator MoveUp(Vector3 nextPos)
    {
        var speedVector = new Vector3(0, 0, speed);
        transform.position += (speedVector * Time.deltaTime);
        if (transform.position.z < nextPos.z)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(MoveUp(nextPos));
        }
        else
        {
            transform.position = nextPos;
            isMoving = false;
        }
    }

    IEnumerator MoveDown(Vector3 nextPos)
    {
        var speedVector = new Vector3(0, 0, speed);
        transform.position -= (speedVector * Time.deltaTime);
        if (transform.position.z > nextPos.z)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(MoveDown(nextPos));
        }
        else
        {
            transform.position = nextPos;
            isMoving = false;
        }
    }
    #endregion

    private void OnValidate()
    {
        if (speed <= 0)
            speed = 1;
    }

    private void Start()
    {
        gm = FindObjectOfType<Grid>();
        Debug.Log(gm.ToString());

        cars = GameObject.FindGameObjectsWithTag("car");
    }

    private void Update()
    {
        car = CheckCloseCollision();
        Car carComp = null;
        if (car)
        {
            carComp = car.GetComponent<Car>();
            carComp.UpdateUi();
            carUi.SetActive(true);
        }
        else
        {
            carUi.SetActive(false);
        }
        
        if (!isMoving && Input.GetKey(KeyCode.W))
        {
            if (carComp)
                if ((carComp.direction == 0 || carComp.direction == 2) && carComp.tires == 4 && carComp.battery && carComp.gas >= 1)
                {
                    Vector3 nextPos = Vector3.zero;
                    if (carComp.direction == 0)
                        nextPos = transform.position + new Vector3(0, 0, 1);
                    if (carComp.direction == 2)
                        nextPos = new Vector3(carComp.transform.position.x, 0, carComp.transform.position.z + carComp.length);
                    if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0)
                    {
                        if (carComp.direction == 0)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[(int)transform.position.x][(int)transform.position.z - i] = 0;
                            }
                        }
                        if (carComp.direction == 2)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[(int)transform.position.x][(int)transform.position.z + i] = 0;
                            }
                        }
                        nextPos = transform.position + new Vector3(0, 0, 1);
                        StartCoroutine(carComp.MoveUp(nextPos));
                        carComp.isMoving = true;
                        carComp.gas--;
                        nextPos = transform.position + new Vector3(0, 0, 1);
                        StartCoroutine(MoveUp(nextPos));
                        isMoving = true;
                    }
                }
            if (!car || (carComp.direction == 1 || carComp.direction == 3))
            {
                var nextPos = transform.position + new Vector3(0, 0, 1);
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0 || gm.grid[(int)nextPos.x][(int)nextPos.z] == 4)
                {
                    StartCoroutine(MoveUp(nextPos));
                    isMoving = true;
                }
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5 && gm.grid[(int)nextPos.x][(int)nextPos.z] <= 12)
                {
                    if (heldItem >= 1 && heldItem <= 4 && gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5 && gm.grid[(int)nextPos.x][(int)nextPos.z] <= 8)
                    {
                        while (heldItem < 4 && gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5)
                        {
                            heldItem++;
                            gm.grid[(int)nextPos.x][(int)nextPos.z]--;
                        }
                        if (gm.grid[(int)nextPos.x][(int)nextPos.z] < 5)
                            gm.grid[(int)nextPos.x][(int)nextPos.z] = 0;
                    }
                    else
                    {
                    int buffer = heldItem + 4;
                    if (heldItem == 0)
                        buffer = 0;
                    heldItem = gm.grid[(int)nextPos.x][(int)nextPos.z] - 4;
                    gm.grid[(int)nextPos.x][(int)nextPos.z] = buffer;
                    }
                    isMoving = true;
                    StartCoroutine(Stall());
                }
            }
            Debug.Log(gm.ToString());
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.A))
        {
            if (carComp)
                if ((carComp.direction == 1 || carComp.direction == 3) && carComp.tires == 4 && carComp.battery && carComp.gas >= 1)
                {
                    Vector3 nextPos = Vector3.zero;
                    if (carComp.direction == 1)
                        nextPos = transform.position - new Vector3(1, 0, 0);
                    if (carComp.direction == 3)
                        nextPos = new Vector3(carComp.transform.position.x - carComp.length, 0, carComp.transform.position.z);
                    if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0)
                    {
                        if (carComp.direction == 1)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[(int)transform.position.x + i][(int)transform.position.z] = 0;
                            }
                        }
                        if (carComp.direction == 3)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[(int)transform.position.x - i][(int)transform.position.z] = 0;
                            }
                        }
                        nextPos = transform.position - new Vector3(1, 0, 0);
                        StartCoroutine(carComp.MoveLeft(nextPos));
                        carComp.isMoving = true;
                        carComp.gas--;
                        nextPos = transform.position - new Vector3(1, 0, 0);
                        StartCoroutine(MoveLeft(nextPos));
                        isMoving = true;
                    }
                }
            if (!car || (carComp.direction == 0 || carComp.direction == 2))
            {
                var nextPos = transform.position - new Vector3(1, 0, 0);
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0 || gm.grid[(int)nextPos.x][(int)nextPos.z] == 3)
                {
                    StartCoroutine(MoveLeft(nextPos));
                    isMoving = true;
                }
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5 && gm.grid[(int)nextPos.x][(int)nextPos.z] <= 12)
                {
                    if (heldItem >= 1 && heldItem <= 4 && gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5 && gm.grid[(int)nextPos.x][(int)nextPos.z] <= 8)
                    {
                        while (heldItem < 4 && gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5)
                        {
                            heldItem++;
                            gm.grid[(int)nextPos.x][(int)nextPos.z]--;
                        }
                        if (gm.grid[(int)nextPos.x][(int)nextPos.z] < 5)
                            gm.grid[(int)nextPos.x][(int)nextPos.z] = 0;
                    }
                    else
                    {
                        int buffer = heldItem + 4;
                        if (heldItem == 0)
                            buffer = 0;
                        heldItem = gm.grid[(int)nextPos.x][(int)nextPos.z] - 4;
                        gm.grid[(int)nextPos.x][(int)nextPos.z] = buffer;
                    }
                    isMoving = true;
                    StartCoroutine(Stall());
                }
            }
            Debug.Log(gm.ToString());
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.S))
        {
            if (carComp)
                if ((carComp.direction == 0 || carComp.direction == 2) && carComp.tires == 4 && carComp.battery && carComp.gas >= 1)
                {
                    Vector3 nextPos = Vector3.zero;
                    if (carComp.direction == 0)
                        nextPos = new Vector3(carComp.transform.position.x, 0, carComp.transform.position.z - carComp.length);
                    if (carComp.direction == 2)
                        nextPos = transform.position - new Vector3(0, 0, 1);
                    if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0)
                    {
                        if (carComp.direction == 0)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[(int)transform.position.x][(int)transform.position.z - i] = 0;
                            }
                        }
                        if (carComp.direction == 2)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[(int)transform.position.x][(int)transform.position.z + i] = 0;
                            }
                        }
                        nextPos = transform.position - new Vector3(0, 0, 1);
                        StartCoroutine(carComp.MoveDown(nextPos));
                        carComp.isMoving = true;
                        carComp.gas--;
                        nextPos = transform.position - new Vector3(0, 0, 1);
                        StartCoroutine(MoveDown(nextPos));
                        isMoving = true;
                    }
                }
            if (!car || (carComp.direction == 1 || carComp.direction == 3))
            {
                var nextPos = transform.position - new Vector3(0, 0, 1);
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0 || gm.grid[(int)nextPos.x][(int)nextPos.z] == 4)
                {
                    StartCoroutine(MoveDown(nextPos));
                    isMoving = true;
                }
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5 && gm.grid[(int)nextPos.x][(int)nextPos.z] <= 12)
                {
                    if (heldItem >= 1 && heldItem <= 4 && gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5 && gm.grid[(int)nextPos.x][(int)nextPos.z] <= 8)
                    {
                        while (heldItem < 4 && gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5)
                        {
                            heldItem++;
                            gm.grid[(int)nextPos.x][(int)nextPos.z]--;
                        }
                        if (gm.grid[(int)nextPos.x][(int)nextPos.z] < 5)
                            gm.grid[(int)nextPos.x][(int)nextPos.z] = 0;
                    }
                    else
                    {
                        int buffer = heldItem + 4;
                        if (heldItem == 0)
                            buffer = 0;
                        heldItem = gm.grid[(int)nextPos.x][(int)nextPos.z] - 4;
                        gm.grid[(int)nextPos.x][(int)nextPos.z] = buffer;
                    }
                    isMoving = true;
                    StartCoroutine(Stall());
                }
            }
            Debug.Log(gm.ToString());
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.D))
        {
            if (carComp)
                if ((carComp.direction == 1 || carComp.direction == 3) && carComp.tires == 4 && carComp.battery && carComp.gas >= 1)
                {
                    Vector3 nextPos = Vector3.zero;
                    if (carComp.direction == 1)
                        nextPos = new Vector3(carComp.transform.position.x + carComp.length, 0, carComp.transform.position.z);
                    if (carComp.direction == 3)
                        nextPos = transform.position + new Vector3(1, 0, 0);
                    if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0 || gm.grid[(int)nextPos.x][(int)nextPos.z] == 13)
                    {
                        if (carComp.direction == 1)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[(int)transform.position.x + i][(int)transform.position.z] = 0;
                            }
                        }
                        if (carComp.direction == 3)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[(int)transform.position.x - i][(int)transform.position.z] = 0;
                            }
                        }
                        nextPos = transform.position + new Vector3(1, 0, 0);
                        StartCoroutine(carComp.MoveRight(nextPos));
                        carComp.isMoving = true;
                        carComp.gas--;
                        nextPos = transform.position + new Vector3(1, 0, 0);
                        StartCoroutine(MoveRight(nextPos));
                        isMoving = true;
                    }
                }
            if (!car || (carComp.direction == 0 || carComp.direction == 2))
            {
                var nextPos = transform.position + new Vector3(1, 0, 0);
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0 || gm.grid[(int)nextPos.x][(int)nextPos.z] == 3)
                {
                    StartCoroutine(MoveRight(nextPos));
                    isMoving = true;
                }
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5 && gm.grid[(int)nextPos.x][(int)nextPos.z] <= 12)
                {
                    if (heldItem >= 1 && heldItem <= 4 && gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5 && gm.grid[(int)nextPos.x][(int)nextPos.z] <= 8)
                    {
                        while (heldItem < 4 && gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5)
                        {
                            heldItem++;
                            gm.grid[(int)nextPos.x][(int)nextPos.z]--;
                        }
                        if (gm.grid[(int)nextPos.x][(int)nextPos.z] < 5)
                            gm.grid[(int)nextPos.x][(int)nextPos.z] = 0;
                    }
                    else
                    {
                        int buffer = heldItem + 4;
                        if (heldItem == 0)
                            buffer = 0;
                        heldItem = gm.grid[(int)nextPos.x][(int)nextPos.z] - 4;
                        gm.grid[(int)nextPos.x][(int)nextPos.z] = buffer;
                    }
                    isMoving = true;
                    StartCoroutine(Stall());
                }
            }
            Debug.Log(gm.ToString());
        }
    }

    IEnumerator Stall()
    {
        yield return new WaitForSeconds(0.25f);
        isMoving = false;
    }

    GameObject CheckCloseCollision()
    {
        foreach (GameObject car in cars)
        {
            if (transform.position.x == car.transform.position.x &&
            transform.position.z == car.transform.position.z)
            {
                return car;
            }
        }
        return null;
    }
}