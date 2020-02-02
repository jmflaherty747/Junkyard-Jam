using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    #region vars
    float speed = 5;
    public bool isMoving;
    Grid gm;
    [SerializeField] bool active = false;
    [SerializeField] public int length = 1;
    [Tooltip("0=Up,1=Left,2=Down,3=Right")] [SerializeField] public int direction;

    public int gas = 0;
    public int tires = 0;
    public bool battery = false;
    public Text carUiText;
    #endregion

    #region movement functions
    public IEnumerator MoveRight(Vector3 nextPos)
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
            if (direction == 1)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z] = 4;

                for (int i = 1; i < length; i++)
                {
                    gm.grid[(int)transform.position.x + i][(int)transform.position.z] = 1;
                }
            }
            if (direction == 3)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z] = 4;

                for (int i = 1; i < length; i++)
                {
                    gm.grid[(int)transform.position.x - i][(int)transform.position.z] = 1;
                }
            }
        }
    }

    public IEnumerator MoveLeft(Vector3 nextPos)
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
            if (direction == 1)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z] = 4;

                for (int i = 1; i < length; i++)
                {
                    gm.grid[(int)transform.position.x + i][(int)transform.position.z] = 1;
                }
            }
            if (direction == 3)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z] = 4;

                for (int i = 1; i < length; i++)
                {
                    gm.grid[(int)transform.position.x - i][(int)transform.position.z] = 1;
                }
            }
        }
    }

    public IEnumerator MoveUp(Vector3 nextPos)
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
            if (direction == 0)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z] = 3;

                for (int i = 1; i < length; i++)
                {
                    gm.grid[(int)transform.position.x][(int)transform.position.z - i] = 1;
                }
            }
            if (direction == 2)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z] = 3;

                for (int i = 1; i < length; i++)
                {
                    gm.grid[(int)transform.position.x][(int)transform.position.z + i] = 1;
                }
            }
        }
    }

    public IEnumerator MoveDown(Vector3 nextPos)
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
            if (direction == 0)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z] = 3;

                for (int i = 1; i < length; i++)
                {
                    gm.grid[(int)transform.position.x][(int)transform.position.z - i] = 1;
                }
            }
            if (direction == 2)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z] = 3;

                for (int i = 1; i < length; i++)
                {
                    gm.grid[(int)transform.position.x][(int)transform.position.z + i] = 1;
                }
            }
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

        if (direction == 0)
        {
            gm.grid[(int)transform.position.x][(int)transform.position.z] = 3;

            for (int i = 1; i < length; i++)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z - i] = 1;
            }
        }
        else
        if (direction == 1)
        {
            gm.grid[(int)transform.position.x][(int)transform.position.z] = 4;

            for (int i = 1; i < length; i++)
            {
                gm.grid[(int)transform.position.x + i][(int)transform.position.z] = 1;
            }
        }
        else
        if (direction == 2)
        {
            gm.grid[(int)transform.position.x][(int)transform.position.z] = 3;

            for (int i = 1; i < length; i++)
            {
                gm.grid[(int)transform.position.x][(int)transform.position.z + i] = 1;
            }
        }
        else
        if (direction == 3)
        {
            gm.grid[(int)transform.position.x][(int)transform.position.z] = 4;

            for (int i = 1; i < length; i++)
            {
                gm.grid[(int)transform.position.x - i][(int)transform.position.z] = 1;
            }
        }
    }

    public void UpdateUi()
    {
        carUiText.text = "Gas: " + gas.ToString() + "\n" +
                        "Tires: " + tires.ToString() + "\n" +
                        "Battery: " + battery.ToString();
    }
}