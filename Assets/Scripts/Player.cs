using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region vars

    float speed = 5;
    bool isMoving;
    Grid gm;
    bool fixing = false;
    [Tooltip("0=none,1-4=Wheel,5=Battery,6-11=Canister")] [SerializeField] int heldItem; //0=none,1-4=Wheel,5=Battery,6-11=Canister


    public GameObject[] cars;
    public GameObject carUi;
    public Text carUiText;
    public GameObject fixUi;
    public Text fixUiText;
    public GameObject itemUi;
    public GameObject tireUi;
    public Text tireUiText;
    public GameObject batteryUi;
    public GameObject gasUi;
    public Text gasUiText;
    public GameObject car;
    MeshRenderer ren;

    #endregion
    
    IEnumerator Move(Vector3 nextPos)
    {
        var mov = Vector3.Normalize(nextPos - transform.position) * speed * Time.deltaTime;
        if (Vector3.Distance(nextPos, transform.position) > Vector3.Magnitude(mov))
        {
            transform.position += mov;
            yield return new WaitForEndOfFrame();
            StartCoroutine(Move(nextPos));
        }
        else
        {
            transform.position = nextPos;
            isMoving = false;
        }
    }

    private void Start()
    {
        gm = FindObjectOfType<Grid>();
        //Debug.Log(gm.ToString());
        ren = GetComponent<MeshRenderer>();
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
            ren.enabled = false;
        }
        else
        {
            carUi.SetActive(false);
            ren.enabled = true;
        }

        if (carComp && !isMoving && Input.GetKey(KeyCode.Space))
        {
            fixing = !fixing;
            isMoving = true;
            StartCoroutine(Stall());
        }

        fixUi.SetActive(fixing);

        if (heldItem == 0)
        {
            fixUiText.text = "Up: Remove Tire" + "\n" +  "Down: Remove Battery";
            itemUi.SetActive(false);
        }
        if (heldItem >= 1 && heldItem <= 4)
        {
            fixUiText.text = "Right: Add Tire" + "\n" + "Left: Remove Tire";
            itemUi.SetActive(true);
            tireUi.SetActive(true);
            tireUiText.text = "" + heldItem;
            batteryUi.SetActive(false);
            gasUi.SetActive(false);

        }
        if (heldItem == 5)
        {
            fixUiText.text = "Right: Add Battery";
            itemUi.SetActive(true);
            tireUi.SetActive(false);
            batteryUi.SetActive(true);
            gasUi.SetActive(false);
        }
        if (heldItem >=6 && heldItem <= 11)
        {
            fixUiText.text = "Right: Add Gas" + "\n" + "Left: Remove Gas";
            itemUi.SetActive(true);
            tireUi.SetActive(false);
            batteryUi.SetActive(false);
            gasUi.SetActive(true);
            gasUiText.text = "" + (heldItem - 6);
        }

        if (!isMoving && Input.GetKey(KeyCode.W))
        {
            if (carComp)
            {
                if (!fixing)
                {
                    if ((carComp.direction == 0 || carComp.direction == 2) && carComp.tires == 4 && carComp.battery && carComp.gas >= 1)
                    {
                        Vector3 nextPos = Vector3.zero;
                        if (carComp.direction == 0)
                            nextPos = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z) + 1);
                        if (carComp.direction == 2)
                            nextPos = new Vector3(Mathf.RoundToInt(carComp.transform.position.x), 0, Mathf.RoundToInt(carComp.transform.position.z) + carComp.length);
                        if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 0)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z + (i * (carComp.direction - 1)))] = 0;
                            }
                            nextPos = transform.position + new Vector3(0, 0, 1);
                            StartCoroutine(carComp.Move(nextPos));
                            carComp.isMoving = true;
                            carComp.gas--;
                            nextPos = transform.position + new Vector3(0, 0, 1);
                            StartCoroutine(Move(nextPos));
                            isMoving = true;
                        }
                    }
                }
                if (fixing)
                {
                    if (heldItem == 0 && carComp.tires > 0)
                    {
                        heldItem = 1;
                        carComp.tires--;
                        isMoving = true;
                        StartCoroutine(Stall());
                    }
                }
            }
            if (!car || ((carComp.direction == 1 || carComp.direction == 3) && !fixing))
            {
                var nextPos = transform.position + new Vector3(0, 0, 1);
                if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 0 || gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 4)
                {
                    StartCoroutine(Move(nextPos));
                    isMoving = true;
                }
                if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] <= 15)
                {
                    if (heldItem >= 1 && heldItem <= 4 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] <= 8)
                    {
                        while (heldItem < 4 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5)
                        {
                            heldItem++;
                            gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)]--;
                        }
                        if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] < 5)
                            gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] = 0;
                    }
                    else
                    {
                    int buffer = heldItem + 4;
                    if (heldItem == 0)
                        buffer = 0;
                    heldItem = gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] - 4;
                    gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] = buffer;
                    }
                    isMoving = true;
                    StartCoroutine(Stall());
                }
            }
            //Debug.Log(gm.ToString());
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.A))
        {
            if (carComp)
            {
                if (!fixing)
                {
                    if ((carComp.direction == 1 || carComp.direction == 3) && carComp.tires == 4 && carComp.battery && carComp.gas >= 1)
                    {
                        Vector3 nextPos = Vector3.zero;
                        if (carComp.direction == 1)
                            nextPos = transform.position - new Vector3(1, 0, 0);
                        if (carComp.direction == 3)
                            nextPos = new Vector3(carComp.transform.position.x - carComp.length, 0, carComp.transform.position.z);
                        if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 0)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[Mathf.RoundToInt(transform.position.x + (i * (2 - carComp.direction)))][Mathf.RoundToInt(transform.position.z)] = 0;
                            }
                            nextPos = transform.position - new Vector3(1, 0, 0);
                            StartCoroutine(carComp.Move(nextPos));
                            carComp.isMoving = true;
                            carComp.gas--;
                            nextPos = transform.position - new Vector3(1, 0, 0);
                            StartCoroutine(Move(nextPos));
                            isMoving = true;
                        }
                    }
                }
                if (fixing)
                {
                    if (heldItem >= 1 && heldItem < 4 && carComp.tires > 0)
                    {
                        heldItem++;
                        carComp.tires--;
                        isMoving = true;
                        StartCoroutine(Stall());
                    }

                    if (heldItem >= 6 && heldItem < 11 && carComp.gas > 0)
                    {
                        heldItem++;
                        carComp.gas--;
                        isMoving = true;
                        StartCoroutine(Stall());
                    }
                }
            }
            if (!car || ((carComp.direction == 0 || carComp.direction == 2) && !fixing))
            {
                var nextPos = transform.position - new Vector3(1, 0, 0);
                if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 0 || gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 3)
                {
                    StartCoroutine(Move(nextPos));
                    isMoving = true;
                }
                if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] <= 15)
                {
                    if (heldItem >= 1 && heldItem <= 4 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] <= 8)
                    {
                        while (heldItem < 4 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5)
                        {
                            heldItem++;
                            gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)]--;
                        }
                        if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] < 5)
                            gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] = 0;
                    }
                    else
                    {
                        int buffer = heldItem + 4;
                        if (heldItem == 0)
                            buffer = 0;
                        heldItem = gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] - 4;
                        gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] = buffer;
                    }
                    isMoving = true;
                    StartCoroutine(Stall());
                }
            }
            //Debug.Log(gm.ToString());
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.S))
        {
            if (carComp)
            {
                if (!fixing)
                {
                    if ((carComp.direction == 0 || carComp.direction == 2) && carComp.tires == 4 && carComp.battery && carComp.gas >= 1)
                    {
                        Vector3 nextPos = Vector3.zero;
                        if (carComp.direction == 0)
                            nextPos = new Vector3(carComp.transform.position.x, 0, carComp.transform.position.z - carComp.length);
                        if (carComp.direction == 2)
                            nextPos = transform.position - new Vector3(0, 0, 1);
                        if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 0)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z + (i * (carComp.direction - 1)))] = 0;
                            }
                            nextPos = transform.position - new Vector3(0, 0, 1);
                            StartCoroutine(carComp.Move(nextPos));
                            carComp.isMoving = true;
                            carComp.gas--;
                            nextPos = transform.position - new Vector3(0, 0, 1);
                            StartCoroutine(Move(nextPos));
                            isMoving = true;
                        }
                    }
                }
                if (fixing)
                {
                    if (heldItem == 0 && carComp.battery == true)
                    {
                        heldItem = 5;
                        carComp.battery = false;
                        isMoving = true;
                        StartCoroutine(Stall());
                    }
                }
            }
            if (!car || ((carComp.direction == 1 || carComp.direction == 3) && !fixing))
            {
                var nextPos = transform.position - new Vector3(0, 0, 1);
                if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 0 || gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 4)
                {
                    StartCoroutine(Move(nextPos));
                    isMoving = true;
                }
                if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] <= 15)
                {
                    if (heldItem >= 1 && heldItem <= 4 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] <= 8)
                    {
                        while (heldItem < 4 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5)
                        {
                            heldItem++;
                            gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)]--;
                        }
                        if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] < 5)
                            gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] = 0;
                    }
                    else
                    {
                        int buffer = heldItem + 4;
                        if (heldItem == 0)
                            buffer = 0;
                        heldItem = gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] - 4;
                        gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] = buffer;
                    }
                    isMoving = true;
                    StartCoroutine(Stall());
                }
            }
            //Debug.Log(gm.ToString());
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.D))
        {
            if (carComp)
            {
                if (!fixing)
                {
                    if ((carComp.direction == 1 || carComp.direction == 3) && carComp.tires == 4 && carComp.battery && carComp.gas >= 1 && transform.position != new Vector3(7, 0, 4))
                    {
                        Vector3 nextPos = Vector3.zero;
                        if (carComp.direction == 1)
                            nextPos = new Vector3(carComp.transform.position.x + carComp.length, 0, carComp.transform.position.z);
                        if (carComp.direction == 3)
                            nextPos = transform.position + new Vector3(1, 0, 0);
                        if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 0 || gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 16)
                        {
                            for (int i = 0; i < carComp.length; i++)
                            {
                                gm.grid[Mathf.RoundToInt(transform.position.x + (i * (2 - carComp.direction)))][Mathf.RoundToInt(transform.position.z)] = 0;
                            }
                            nextPos = transform.position + new Vector3(1, 0, 0);
                            StartCoroutine(carComp.Move(nextPos));
                            carComp.isMoving = true;
                            carComp.gas--;
                            nextPos = transform.position + new Vector3(1, 0, 0);
                            StartCoroutine(Move(nextPos));
                            isMoving = true;
                        }
                    }
                }
                if (fixing)
                {
                    if (heldItem >= 1 && heldItem <= 4 && carComp.tires < 4)
                    {
                        heldItem--;
                        carComp.tires++;
                        isMoving = true;
                        StartCoroutine(Stall());
                    }

                    if (heldItem == 5 && !carComp.battery)
                    {
                        heldItem = 0;
                        carComp.battery = true;
                        isMoving = true;
                        StartCoroutine(Stall());
                    }

                    if (heldItem > 6 && heldItem <= 11 && carComp.gas < 5)
                    {
                        heldItem--;
                        carComp.gas++;
                        isMoving = true;
                        StartCoroutine(Stall());
                    }
                }
            }
            if (!car || ((carComp.direction == 0 || carComp.direction == 2) && !fixing))
            {
                var nextPos = transform.position + new Vector3(1, 0, 0);
                if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 0 || gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] == 3)
                {
                    StartCoroutine(Move(nextPos));
                    isMoving = true;
                }
                if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] <= 15)
                {
                    if (heldItem >= 1 && heldItem <= 4 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] <= 8)
                    {
                        while (heldItem < 4 && gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] >= 5)
                        {
                            heldItem++;
                            gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)]--;
                        }
                        if (gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] < 5)
                            gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] = 0;
                    }
                    else
                    {
                        int buffer = heldItem + 4;
                        if (heldItem == 0)
                            buffer = 0;
                        heldItem = gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] - 4;
                        gm.grid[Mathf.RoundToInt(nextPos.x)][Mathf.RoundToInt(nextPos.z)] = buffer;
                    }
                    isMoving = true;
                    StartCoroutine(Stall());
                }
            }
            //Debug.Log(gm.ToString());
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
            if (Mathf.Abs(transform.position.x - car.transform.position.x) <= 0.25f &&
            Mathf.Abs(transform.position.z - car.transform.position.z) <= 0.25f)
            {
                return car;
            }
        }
        return null;
    }
}