﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemies;
    public GameObject levelup;
    public GameObject player;
    void Start()
    {
        //if(!GlobalStats.justLoaded)
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject e in enemies)
        {
            //Debug.Log("looking for id :" + e.GetComponent<Combatant>().id);
            if (GlobalStats.killedEnemies.Contains(e.GetComponent<Combatant>().id))
            {
                //Debug.Log("desry");
                if (e.GetComponent<Combatant>().id == 3) e.GetComponent<Unlock>().unlock();
                Destroy(e);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalStats.perkPoints > 0)
        {
            levelup.SetActive(true);
        }
        else
        {
            Debug.Log("no perks");
            levelup.SetActive(false);
        }
        levelup.GetComponent<Transform>().position = new Vector3(player.GetComponent<Transform>().position.x - 13f, player.GetComponent<Transform>().position.y + 7, player.GetComponent<Transform>().position.z);
    }
}
