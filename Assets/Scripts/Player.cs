using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region vars
    float speed = 5;
    bool isMoving;
    Grid gm;
    int heldItem; //0=none,1-4=Wheel,5=Battery,6-11=Canister


    public GameObject[] cars;
    public GameObject carUi;
    public Text carUiText;
    GameObject car;

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
                    var nextPos = transform.position + new Vector3(0, 0, 1);
                    StartCoroutine(carComp.MoveUp(nextPos));
                    carComp.isMoving = true;
                    carComp.gas--;
                    StartCoroutine(MoveUp(nextPos));
                    isMoving = true;
                }
            if (!car)
            {
                var nextPos = transform.position + new Vector3(0, 0, 1);
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0 || gm.grid[(int)nextPos.x][(int)nextPos.z] == 4)
                {
                    StartCoroutine(MoveUp(nextPos));
                    isMoving = true;
                }
                if (gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5)
                {
                    int buffer = heldItem + 4;
                    heldItem = gm.grid[(int)nextPos.x][(int)nextPos.z] - 4;
                    gm.grid[(int)nextPos.x][(int)nextPos.z] = buffer;
                }
            }
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.A))
        {
            var nextPos = transform.position - new Vector3(1, 0, 0);
            if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0 || gm.grid[(int)nextPos.x][(int)nextPos.z] == 3)
            {
                StartCoroutine(MoveLeft(nextPos));
                isMoving = true;
            }
            if (gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5)
            {
                int buffer = heldItem + 4;
                heldItem = gm.grid[(int)nextPos.x][(int)nextPos.z] - 4;
                gm.grid[(int)nextPos.x][(int)nextPos.z] = buffer;
            }
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.S))
        {
            var nextPos = transform.position - new Vector3(0, 0, 1);
            if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0 || gm.grid[(int)nextPos.x][(int)nextPos.z] == 4)
            {
                StartCoroutine(MoveDown(nextPos));
                isMoving = true;
            }
            if (gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5)
            {
                int buffer = heldItem + 4;
                heldItem = gm.grid[(int)nextPos.x][(int)nextPos.z] - 4;
                gm.grid[(int)nextPos.x][(int)nextPos.z] = buffer;
            }
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.D))
        {
            var nextPos = transform.position + new Vector3(1, 0, 0);
            if (gm.grid[(int) nextPos.x][(int) nextPos.z] == 0 || gm.grid[(int)nextPos.x][(int)nextPos.z] == 3)
            {
                StartCoroutine(MoveRight(nextPos));
                isMoving = true;
            }
            if (gm.grid[(int)nextPos.x][(int)nextPos.z] >= 5)
            {
                int buffer = heldItem + 4;
                heldItem = gm.grid[(int)nextPos.x][(int)nextPos.z] - 4;
                gm.grid[(int)nextPos.x][(int)nextPos.z] = buffer;
            }
        }


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