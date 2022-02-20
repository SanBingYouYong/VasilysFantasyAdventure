using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleControl : MonoBehaviour
{
    public int hp = 10;
    public int magicPower = 0;
    public int signal = 0;

    // level length control

    public int mode=0;//Mode of game; 0, countExceed; 1, holdAbove
    public int planDuration=10;
    public int requirement=50;//mode 1, number of sets; mode 2, percentage
    private int counter = 0;

    // current level setttings: include threshold signals, magicPowerRequired
    private int thresholdSignal = 80;       // needs implemetation
    private int magicPowerRequired;    // needs implemetation

    // sync
    private float timer;
    public float detectionTime = 1.0f;

    private bool battled = false;

    public overlay overlay;

    private void Start()
    {
        //hp = 10;
        //signal = 0;
        //magicPower = 0;

        // Dynamic values from templates
        overlay = GameObject.Find("overlay_go").GetComponent<overlay>();
        var i = overlay.choice;
        if (overlay.templates[i][3] == "countExceed")
        {
            mode = 0;
        }
        else if (overlay.templates[i][3] == "holdAbove")
        {
            mode = 1;
        }
        planDuration = int.Parse(overlay.templates[i][1]);
        requirement = int.Parse(overlay.templates[i][4]);
        thresholdSignal = int.Parse(overlay.templates[i][2]);

        countMagicRequired(mode);

        Debug.Log("You need to suck 40 signals!");

       // A Start
    }
    private void countMagicRequired(int mode)
    {
        if (mode == 0)
        {
            magicPowerRequired = requirement * planDuration * thresholdSignal / 100;
        }
        else
        {
            magicPowerRequired = requirement* planDuration * thresholdSignal;
        }
    }
    private void Update()
    {
        // A End -> B Start
        if (overlay.started)
        {
            timer += Time.deltaTime;
        }
        // detect user signal every detection time period.
        if (timer > detectionTime)
        {
            MagicPowerSuck(thresholdSignal);

            timer = timer - detectionTime;
            counter += 1;
            //Time.timeScale = scrollBar
        }
        if (counter >= planDuration && !battled)
        {
            battled = true;
            Battle(magicPowerRequired);
        }

        
    }
   
    public void AdjustSignal(float newSignal)
    {
        signal = (int) Mathf.Round(newSignal*100);
    }
    public void Battle(int magicPowerRequired)
    {
        GameObject.Find("SoldiersFront").transform.position -= new Vector3(0f, 5f, 0f);
        StartCoroutine(waitBattle());
        
    }

    IEnumerator waitBattle()
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(4);
        if (magicPower >= magicPowerRequired)
        {
            Debug.Log("You successfully killed the enemy!");
            // enemy beaten
            GameObject.Find("SoldiersFront").transform.position += new Vector3(0f, 5f, 0f);

            // play animation NEEDS IMPLEMENT

            magicPower = 0;
        }
        else
        {
            hp -= 1;
            magicPower = 0;
            Debug.Log("You loss!, hp-1");

            // lose HP
        }
        deathDetection();
        //Wait for 2 seconds
        yield return new WaitForSeconds(2);
    }

    public void MagicPowerSuck(int threshold)
    {
        if (signal >= threshold)
        {
            magicPower += threshold;
            this.GetComponent<Move>().newMPBallneeded = true;
            Debug.Log("Now you have" + magicPower);
        }
    }
    public void deathDetection()
    {
        if (hp <= 0)
        {
            Debug.Log("You are dead!");

            // gameover! NEEDS IMPLEMENT
            
            overlay.dead = true;
        }
    }
}
