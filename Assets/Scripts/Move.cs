using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour

{

    private Vector3 randomPosition = new Vector3(0, 0, 0);
    private Transform mpBallTransform;
    // Start is called before the first frame update
    private float movement;
    public float speed = 10f;
    public Transform[] Target;
    //private int i = 0;
    public Vector3 P;
    public GameObject MPBall;
    public GameObject mpball_GO;
    public bool mpballGetting = false;
    public bool newMPBallneeded = false;

    // test
    int counter = 0;
    // Update is called once per frame
    void Update()
    {
        //movement = Time.deltaTime * speed;
        //transform.Translate(movement, 0, 0);
        //  counter++;
        // GenerateRandomPosition();
        if (mpballGetting == true)
            MPMotion();
        if (newMPBallneeded)
        {
            MPSetup();
        }
    }
    private void Start()
    {
        // MPBall = Resources.Load("pixil-frame-0_single") as GameObject;

        MPSetup();

    }
    void MPSetup()

    {
        MPBall = Resources.Load("pixil-frame-0-1-single") as GameObject;
        var wizard = GameObject.Find("Wizard_0");
        //ballpre = Resources.Load("pixil - frame - 0") as GameObject;

        Vector3 P = wizard.transform.position;
        GenerateRandomPosition();

        mpball_GO = Instantiate(MPBall);
        mpball_GO.transform.SetParent(wizard.transform, false);
        mpball_GO.transform.localPosition = randomPosition;
        //mpball_GO.transform.position = randomPosition;
        mpball_GO.name = "MPBall";


        mpballGetting = true;
        newMPBallneeded = false;

    }
    void GenerateRandomPosition()
    {
        randomPosition = new Vector3(Random.Range(P.x-7f, P.x+7f), Random.Range(P.y-6f, P.y+8f), 0f);

    }
    void MPMotion()
    {
        //Vector3 newP = new Vector3(P.x, P.y + 1.2f, P.z);
        //mpball_GO.transform.localPosition = Vector3.MoveTowards(mpball_GO.transform.localPosition, newP, movement * 0.1f);
        //Debug.Log(mpball_GO.transform.localPosition);
        //Debug.Log(P);

        if (mpball_GO.transform.localPosition != new Vector3(0f, 0f, 0f))
        {
            var distancePos = -mpball_GO.transform.localPosition;
            mpball_GO.transform.Translate(distancePos * 0.05f);
        }
        if (mpball_GO.transform.localPosition == new Vector3(0f, 0f, 0f))
        {
            // NEEDS IMPLEMNT: MP ++
            Destroy(mpball_GO);
            Debug.Log("MP GAIN!");
            mpballGetting = false;
        }
    }
}
