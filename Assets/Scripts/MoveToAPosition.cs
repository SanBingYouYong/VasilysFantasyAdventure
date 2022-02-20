using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveToAPosition : MonoBehaviour
{
    private int counter = 0;
    private System.Random rand;
    private Vector3 targetPosition;
    private Vector3 direction = new Vector3(0,1,0);
    public int speedFactor = 300;
    // Start is called before the first frame update
    void Start()
    { 
        rand = new System.Random();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(counter == speedFactor){
            float y = (float)(rand.NextDouble() * 5 + 3.3);
            targetPosition = new Vector3(transform.position.x, y, 0);
            direction = targetPosition - transform.position;
            counter = 0;
        }
        transform.Translate(direction/speedFactor);
        counter++;
    }
}
