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
    [SerializeField] int length = 1;
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

    private void Update()
    {
        if (!active)
        {
            return;
        }

        if (!isMoving && Input.GetKey(KeyCode.W))
        {
            Vector3 nextPos = Vector3.zero;
            if (direction == 0)
                nextPos = transform.position + new Vector3(0, 0, 1);
            if (direction == 2)
                nextPos = new Vector3(transform.position.x, 0, transform.position.z + length);
            if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0)
            {
                if (direction == 0)
                    gm.grid[(int)transform.position.x][(int)transform.position.z - (length - 1)] = 0;
                if (direction == 2)
                    gm.grid[(int)transform.position.x][(int)transform.position.z] = 0;
                StartCoroutine(MoveUp(nextPos));
                isMoving = true;
            }
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.A))
        {
            var nextPos = transform.position - new Vector3(1, 0, 0);
            if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0)
            {
                if (direction == 1)
                    gm.grid[(int)transform.position.x + (length - 1)][(int)transform.position.z] = 0;
                else
                    gm.grid[(int)transform.position.x][(int)transform.position.z] = 0;
                StartCoroutine(MoveLeft(nextPos));
                isMoving = true;
            }
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.S))
        {
            var nextPos = transform.position - new Vector3(0, 0, 1);
            if (gm.grid[(int)nextPos.x][(int)nextPos.z] == 0)
            {
                if (direction == 2)
                    gm.grid[(int)transform.position.x][(int)transform.position.z + (length - 1)] = 0;
                else
                    gm.grid[(int)transform.position.x][(int)transform.position.z] = 0;
                StartCoroutine(MoveDown(nextPos));
                isMoving = true;
            }
        }
        else
        if (!isMoving && Input.GetKey(KeyCode.D))
        {
            var nextPos = transform.position + new Vector3(1, 0, 0);
            if (gm.grid[(int) nextPos.x][(int) nextPos.z] == 0)
            {
                if (direction == 3)
                    gm.grid[(int)transform.position.x - (length - 1)][(int)transform.position.z] = 0;
                else
                    gm.grid[(int)transform.position.x][(int)transform.position.z] = 0;
                StartCoroutine(MoveRight(nextPos));
                isMoving = true;
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