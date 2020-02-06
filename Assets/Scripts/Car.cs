using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    #region vars
    float speed = 5;
    public bool isMoving;
    Grid gm;
    [SerializeField] public int length = 1;
    [Tooltip("0=Up,1=Left,2=Down,3=Right")] [SerializeField] public int direction;

    public int gas = 0;
    public int tires = 0;
    public bool battery = false;
    public Text carUiText;
    #endregion

    public IEnumerator Move(Vector3 nextPos)
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
            AddToGrid();
        }
    }

    private void Start()
    {
        gm = FindObjectOfType<Grid>();
        AddToGrid();
        carUiText = GameObject.FindWithTag("carText").GetComponent<Text>();
    }

    public void UpdateUi()
    {
        carUiText.text = "Gas: " + gas.ToString() + "\n" +
                        "Tires: " + tires.ToString() + "\n" +
                        "Battery: " + battery.ToString();
    }

    private void AddToGrid()
    {
        gm.grid[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z)] = 3 + (direction % 2);

        if (direction == 0 || direction == 2)
        {
            for (int i = 1; i < length; i++)
            {
                gm.grid[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z + (i * (direction - 1)))] = 1;
            }
        }
        else
        if (direction == 1 || direction == 3)
        {
            for (int i = 1; i < length; i++)
            {
                gm.grid[Mathf.RoundToInt(transform.position.x + (i * (2 - direction)))][Mathf.RoundToInt(transform.position.z)] = 1;
            }
        }
    }
}