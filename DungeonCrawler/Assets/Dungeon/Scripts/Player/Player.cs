﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public struct LikeControlScheme
{
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Up;
    public KeyCode Down;
}

public class Player : MonoBehaviour
{
    int layerMask = 1 << 8;
    public List<GameObject> Items;
    public LikeControlScheme[] ControlSchemes;
    public PlayerData PlayerData;

    private bool IsOnExit;
    public int CurrentHP;
    public int CurrentMP;

    void Start()
    {
        CurrentHP = PlayerData.MaxHP*PlayerData.Str;
        CurrentMP = PlayerData.MaxMP*PlayerData.Int;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOnExit && Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("You Win");
        }
    }

    public bool Movement()
    {
        CreatingLight();
        foreach (var ControlScheme in ControlSchemes)
        {
            if (Move(ControlScheme.Left, Vector3.left)) return true;
            if (Move(ControlScheme.Right, Vector3.right)) return true;
            if (Move(ControlScheme.Up, Vector3.up)) return true;
            if (Move(ControlScheme.Down, Vector3.down)) return true;
            // return false;
        }
        return false;
    }

    private bool Move(KeyCode kc, Vector3 dir)
    {
        if (Input.GetKeyDown(kc))
        {
            if (!CanMove(dir))
                return false;
            transform.localPosition += dir;
            return true;
        }
        return false;
    }

    private bool CanMove(Vector3 myVector)
    {
        if (Physics2D.OverlapPoint(transform.localPosition + (myVector*0.5f) + Vector3.one*0.5f, layerMask))
            // if (Physics2D.Raycast(transform.localPosition + Vector3.up * 0.5f, myVector, 0.5f))
        {
            // Debug.Log("Nie moszna");
            return false;
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hearth")
        {
            CurrentHP += 50;
            if (CurrentHP > PlayerData.MaxHP)
                CurrentHP = PlayerData.MaxHP*PlayerData.Str;
            other.gameObject.SetActive(false);
        }

        if (other.tag == "Enemy")
        {
            CurrentHP -= 30;
            if (CurrentHP < 0)
                Debug.Log("- 30");
            other.gameObject.SetActive(false);
        }
        if (other.tag == "Exit")
        {
            IsOnExit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            IsOnExit = false;
        }
    }

    private void CreatingLight()
    {
        for (int x = -2; x < 2; x++)
        {
            for (int y = -2; y < 2; y++)
            {
                Collider2D[] hitColliders =
                   Physics2D.OverlapPointAll(new Vector3(transform.position.x - x + 0.5f,
                       transform.position.y - y + 0.5f,
                       0));

                if (hitColliders != null)
                {
                    // Gizmos.DrawSphere(new Vector3(transform.position.x - x + 0.5f, transform.position.y - y + 0.5f, 0), 0.2f);
                    foreach (var d in hitColliders)
                    {
                        d.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                //Gizmos.DrawSphere(new Vector3(x, y, 0), 0.2f);
            }
        }
    }
}