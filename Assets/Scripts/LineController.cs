using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public LineRenderer lr;
    public List<Transform> points;

    private void Awake()
    {
        lr.positionCount = 0;

        points = new List<Transform>();
    }
    public void ChangePoint(float _length, GameObject dotPrefab, Transform setParent)
    {
        //Debug.Log("before : " + points.Count);
        Vector3 p0 = points[points.Count - 2].transform.position;
        Vector3 p1 = points[points.Count - 1].transform.position;
        float Distance = Vector3.Distance(p1, p0);
        if (Distance <= _length) return;

        // 프레임 단위로 그려지기 때문에 유저가 프레임 단위보다 빠른 속도로 선을 그으면
        // 부자연스럽게 선이 그어지기 때문에 해당부분을 보완하는 부분
        // 중간에 채워야할 포인트 수
        int re = (int)(Distance / _length); 

            GameObject dot;
            
        if(re > 1)
                for (int i = 0; i < re; i++)
                {
                    dot = Instantiate(dotPrefab, new Vector3(0, 0.025f, 0), Quaternion.identity, setParent);
                    dot.transform.position = Vector3.Lerp(p0, p1, (i+1) / ((float)(re + 1)));
                    lr.positionCount++;
                    points.Insert(points.Count - 1, dot.transform);
                    //Debug.Log("Result : " + re);
                    Debug.Log(p0);
            }

    }
    public void AddPoint(Transform point)
    {
        lr.positionCount++;
        points.Add(point);
    }

    private void LateUpdate()
    {
        if (points.Count >= 2)
        {
            for(int i = 0; i < points.Count; i++)
            {
                lr.SetPosition(i, points[i].position);
            }
        }
    }
}
