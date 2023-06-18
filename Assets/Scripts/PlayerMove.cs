using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool isMove = false;

    LineController line;

    [SerializeField]
    private float speed;

    public void Update()
    {
        if (!isMove)
            return;

        //int index = 0;
        //Vector3 destination = line.points[index].position;

        //Vector3 pos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        //transform.position = pos;

        //float distance = Vector3.Distance(transform.position, destination);
        
        //if(distance <= 0.05)
        //    if(index < line.points.Count - 1)
        //        index++;
    }

    public void SetLine(LineController _line) => line = _line;
}
