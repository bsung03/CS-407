﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path;

    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance = 1;

    private int currentWaypoint = 0;

    private bool searchingForPlayer = false;

    public bool moving = true;
    GameObject[] potentialPlayers;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {

            potentialPlayers = GameObject.FindGameObjectsWithTag("Player");
            if (potentialPlayers.Length > 0)
            {
                GameObject player = potentialPlayers[0];
                target = player.transform;
            }
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());

    }

    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
           // return false;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            //return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);
      
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
       
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            potentialPlayers = GameObject.FindGameObjectsWithTag("Player");
            print("Potential Length: " + potentialPlayers.Length.ToString());
            if (potentialPlayers.Length > 0)
            {
                GameObject player = potentialPlayers[0];
                target = player.transform;
            }
            else
            {
                potentialPlayers = GameObject.FindGameObjectsWithTag("roll");
                if (potentialPlayers.Length > 0)
                {
                    GameObject player = potentialPlayers[0];
                    target = player.transform;
                }
            }
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            pathIsEnded = true;
            //print("PATH ENDED");
           
            try
            {
                this.GetComponent<BullManager>().SendMessage("StopWalking");
            }
            catch (Exception e)
            {

            }
            try
            {
                this.GetComponent<SkeletonBoss>().SendMessage("StopWalking");
            }
            catch (Exception e)
            {

            }
            return;
        }
        pathIsEnded = false;

        if(moving)
        {
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            dir *= speed * Time.fixedDeltaTime;

            rb.AddForce(dir, fMode);
           

            float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (dist < nextWaypointDistance)
            {
                currentWaypoint++;
                return;
            }
        }


    }
}
