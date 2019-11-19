﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject enemy;
    public GameObject stateManager;

    private Combatant playerScript;
    private Combatant enemyScript;
    private Enemy enemyMethods;
    private StateManager sm;

    private int enemyCurrentElement;
    private int playerCurrentElement;
    private int enemyCurrentAction; // attack->0, defent->1, reload->2, repair->3
    private int playerCurrentAction;


    void Start()
    {
        playerScript = player.GetComponent<Combatant>();
        enemyScript = enemy.GetComponent<Combatant>();
        enemyMethods = enemy.GetComponent<Enemy>();
        sm = stateManager.GetComponent<StateManager>();
        playerScript.health = GlobalStats.health;
        playerScript.elementLevels = GlobalStats.elements;
        playerScript.ammo = 1;
        playerScript.shields = 1;

    }

    // Update is called once per frame

    public void setPlayerFire()
    {
        playerCurrentElement = 0;
        player.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void setPlayerEarth()
    {
        playerCurrentElement = 1;
        player.GetComponent<SpriteRenderer>().color = Color.yellow;

    }
    public void setPlayerMetal()
    {
        playerCurrentElement = 2;
        player.GetComponent<SpriteRenderer>().color = Color.gray;

    }
    public void setPlayerWater()
    {
        playerCurrentElement = 3;
        player.GetComponent<SpriteRenderer>().color = Color.blue;

    }
    public void setPlayerPlant()
    {
        playerCurrentElement = 4;
        player.GetComponent<SpriteRenderer>().color = Color.green;

    }

    public void setPlayerAttack()
    {
        playerCurrentAction = 0;
    }
    public void setPlayerDefend()
    {
        playerCurrentAction = 1;
    }
    public void setPlayerReload()
    {
        playerCurrentAction = 2;
    }
    public void setPlayerRepair()
    {
        playerCurrentAction = 3;
    }

    public int getPlayerAmmo()
    {
        Debug.Log("Ammo: " + playerScript.ammo);
        return playerScript.ammo; 
    }

    public int getPlayerShields()
    {
        return playerScript.shields;
    }

    public void getOutcome()
    {
        int tempEDmgTaken = 0;
        int tempPDmgTaken = 0;
        enemyCurrentElement = enemyMethods.getElement();
        enemyCurrentAction = enemyMethods.getAction();
        if (enemyCurrentElement == 0) enemy.GetComponent<SpriteRenderer>().color = Color.red;
        if (enemyCurrentElement == 1) enemy.GetComponent<SpriteRenderer>().color = Color.yellow;
        if (enemyCurrentElement == 2) enemy.GetComponent<SpriteRenderer>().color = Color.gray;
        if (enemyCurrentElement == 3) enemy.GetComponent<SpriteRenderer>().color = Color.blue;
        if (enemyCurrentElement == 4) enemy.GetComponent<SpriteRenderer>().color = Color.green;

        if (playerCurrentAction == 0) {
            tempEDmgTaken = playerScript.elementLevels[playerCurrentElement] * multiplier(playerCurrentElement, enemyCurrentElement);
            playerScript.ammo -= 1;
        }
        if (enemyCurrentAction == 0)
        {
            tempPDmgTaken = enemyScript.elementLevels[enemyCurrentElement] * multiplier(enemyCurrentElement, playerCurrentElement);
            enemyScript.ammo -= 1;
        }
        if (playerCurrentAction == 1) {
            tempPDmgTaken -= playerScript.elementLevels[playerCurrentElement] * multiplier(playerCurrentElement, enemyCurrentElement);
            if (tempPDmgTaken < 0) {
                tempPDmgTaken = 0;
            }
            playerScript.shields -= 1;
        }
        if (enemyCurrentAction == 1)
        {
            tempEDmgTaken -= enemyScript.elementLevels[enemyCurrentElement] * multiplier(enemyCurrentElement, playerCurrentElement);
            if (tempEDmgTaken < 0)
            {
                tempEDmgTaken = 0;
            }
            enemyScript.shields -= 1;
        }
        if (playerCurrentAction == 2) {
            playerScript.ammo += 1;
        }
        if (enemyCurrentAction == 2) {
            enemyScript.ammo += 1;
        }
        if (playerCurrentAction == 3)
        {
            playerScript.shields += 1;
        }
        if (enemyCurrentAction == 3)
        {
            enemyScript.shields += 1;
        }

        sm.animate(playerCurrentElement,playerCurrentAction,enemyCurrentElement,enemyCurrentAction);

        playerScript.health -= tempPDmgTaken;
        enemyScript.health -= tempEDmgTaken;
        if(enemyScript.health < 1)
        {
            GlobalStats.health = playerScript.health;
            SceneManager.LoadScene(sceneBuildIndex: GlobalStats.lastScene);
        }
    }

    //returns what to multiply e1 by
    private int multiplier(int e1, int e2) {
        if (elementCompare(e1, e2) == 1) return 2;
        else return 1;
    }

    //returns -1 if enemy beats player, 0 if they're the same, and 1 if player beats enemy
    private int elementCompare(int p, int e) {
        if (p == e) return 0;
        if ((((p + 2) % 5) == e) || (((p - 1) % 5) == e)) return 1;
        else return -1;
    }

}