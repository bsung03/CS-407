﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public double experience;
    public int level;
    public double expThreshold;

    //Each index of this array corresponds to how much the respective stat in the stats array should be incremented by in the level up function
    //This way we can take care of levelling up just with a loop
    public int[] increments = new int[] { 5, 5, 1, 1, 2, 5 };

    // Stats in order of index: Health, Max Health, Attack Power, Attack Speed, Movement Speed, Shield
    public int[] stats = new int[] { 100, 100, 5, 2, 4, 20 };

    private BoxCollider2D boxCollider;

    private Vector3 moveDelta;

    public float moveSpeed = 5f;

    private RaycastHit2D hit;

    

    // Start is called before the first frame update
    void Start()
    {
       level = 1;
       expThreshold = 30;
       experience = 0;
       boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDelta = new Vector3(x,y,0);

        if(moveDelta.x < 0){
            transform.localScale = Vector3.one;
        }else if(moveDelta.x > 0){
            transform.localScale = new Vector3(-1,1,1);
        }


        //movement
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveSpeed * moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null)
        {
            transform.Translate(0,moveDelta.y * Time.deltaTime * moveSpeed, 0);
        }
        else
        {
            Debug.Log("blocker");
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x,0), Mathf.Abs( moveSpeed * moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime * moveSpeed, 0,0);
        }
        else
        {
            Debug.Log("blocker");
        }



        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveSpeed * moveDelta.y * Time.deltaTime), LayerMask.GetMask("collect"));
        if (hit.collider == null)
        {

        }
        else
        {
 
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveSpeed * moveDelta.x * Time.deltaTime), LayerMask.GetMask("collect"));
        if (hit.collider == null)
        {
           
        }
        else
        {

        }

        //Level ups
        if (experience >= expThreshold) {
            experience -= expThreshold;
            levelUp();
        }
    }

    public double adjustThreshold() {
        return expThreshold * 1.3;
    }

    public void levelUp() {
        //increment player's level
        level++;
        //Loop through the stats array and increment each one by its corresponding index in the increment array
        for (int i = 0; i < stats.Length; i++) {
            stats[i] += increments[i];
        }

        //Set a new exp threshold
        expThreshold = adjustThreshold();
    }
}
