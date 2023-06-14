using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTool : MonoBehaviour
{
    [Header("Dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;
    [SerializeField] private float dotSize;
    [SerializeField] private bool dotActive;

    [Header("Lines")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform lineParent;
    [SerializeField] private float lineSize;
    [SerializeField] private bool lineActive;
    [SerializeField] private float lineLength;

    [Header("Speed")]
    [SerializeField] private float Speed;

    private LineController currentLine;
    //public PlayerMove playerMove;

    public Vector3 worldMousePosition;

    public static bool isMove;
    private bool isLine;
    public GameObject playerObject;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        #region GetMouseButtonDown

        if (Input.GetMouseButtonDown(0) && !isMove) // MouseDown
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Player")
                    isLine = true;

            }
        }

        #endregion

        #region GetMouseButton

        if (Input.GetMouseButton(0) && !isMove) // GetMouseButton
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Plane")
                {
                    if (!isLine)
                        return;

                    worldMousePosition = hit.point;

                    //worldMousePosition.y = 0.05f;

                    if (currentLine == null)
                    {
                        currentLine = LineSet(linePrefab, lineActive, lineSize);
                    }

                    if (!CanAppend(worldMousePosition)) return; // 라인 길이 

                    GameObject dotted;

                    if (currentLine.lr.positionCount == 0)
                    {
                        dotted = InstantiateDotSet(dotPrefab, dotActive, dotSize, new Vector3(playerObject.transform.position.x, worldMousePosition.y, playerObject.transform.position.z));
                        currentLine.AddPoint(dotted.transform);
                    }
                    else
                    {
                        dotted = InstantiateDotSet(dotPrefab, dotActive, dotSize, worldMousePosition);
                        currentLine.AddPoint(dotted.transform);

                        currentLine.ChangePoint(lineLength, dotPrefab, dotParent);
                    }
                }
            }
        }

        #endregion

        #region Runtime
        if (currentLine != null) // Runtime 라인 조절
        {
            currentLine.lr.startWidth = lineSize;
            currentLine.transform.gameObject.SetActive(lineActive);
        }

        if (currentLine != null) // Runtime 도트 조절
        {
            foreach(var dot in currentLine.points)
            {

                dot.transform.localScale = new Vector3(dotSize, dotSize, dotSize);
                dot.transform.gameObject.SetActive(dotActive);
            }
        }
        #endregion

        #region GetMouseButtonUp

        if (Input.GetMouseButtonUp(0) && !isMove && isLine) // Mouse Up
        {
            isLine = false;
            isMove = true;
        }

        //if (isMove)
        //    playerMove.MoveOn(currentLine, Speed, isMove);

        #endregion
    }

    // 포인트 생성
    public GameObject InstantiateDotSet(GameObject _obj, bool _act, float _size, Vector3 _pos) 
    {

        dotPrefab.transform.localScale = new Vector3(_size, _size, _size);

        _obj.SetActive(_act);

        GameObject dot = Instantiate(dotPrefab, _pos, Quaternion.identity, dotParent);

        return dot;
    }

    // 라인렌더러 포지션 생성
    private LineController LineSet(GameObject _obj, bool _act, float size)
    {

        _obj.GetComponent<LineController>().lr.startWidth = size;

        _obj.SetActive(_act);

        LineController Line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent).GetComponent<LineController>();

        return Line;
    }

    // 라인렌더러 한 포지션 당 한계점
    public bool CanAppend(Vector3 pos)
    {
        if (currentLine.lr.positionCount == 0) return true;

        return Vector3.Distance(currentLine.lr.GetPosition(currentLine.lr.positionCount - 1), pos) >= lineLength;
    }
}
