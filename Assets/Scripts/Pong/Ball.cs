﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public static float SPEED_MAX = 60;
    public float speed = 30;
    public Sprite sprite;
    public float _distance;

    private float _speed;
    private Eyex  _eye;

	void Start () {

        _speed = speed;

        GetComponent<Rigidbody2D>().velocity = Vector2.right * _speed;

        GameObject go = GameObject.Find("eye");
        _eye = (Eyex)go.GetComponent(typeof(Eyex));
	}

    float HitFactor(Vector2 Ball_Pos, Vector2 Racket_Pos, float Racket_Height) {
        return ((Ball_Pos.y - Racket_Pos.y) / Racket_Height);
    }

    bool isNear(Vector3 v3Pos, float distance)
    {
        v3Pos = v3Pos - transform.position;
        return ((v3Pos.x > -distance && v3Pos.y > -distance) && (v3Pos.x < distance && v3Pos.y < distance));
    }

    bool isNear(float x, float y, float distance) {
        Vector3 v3Pos = new Vector3(x, y, 0);
        v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
        v3Pos = v3Pos - transform.position;
        return ((v3Pos.x > -distance && v3Pos.y > -distance) && (v3Pos.x < distance && v3Pos.y < distance));
    }

    bool isNear(float x1, float y1, float x2, float y2) {
        Vector3 position = transform.position;
        return ((x1 < position.x && y1 > position.y) && (x2 > position.x && y2 < position.y));
    }

    void Update() {

        if (isNear(-5f, 33f, 5f, -37f)) { // center
            GetComponent<SpriteRenderer>().sprite = sprite;
        } else if (isNear(-50f, 33f, -35, -37f)) { // left
            GetComponent<SpriteRenderer>().sprite = sprite;
        } else if (isNear(35f, 33f, 50f, -37f)) { // right
            GetComponent<SpriteRenderer>().sprite = sprite;
        } else if (isNear(_eye.transform.position, _distance)) {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;
            GetComponent<TrailRenderer>().Clear();
        }

    }

    void OnCollisionEnter2D(Collision2D col) {

        if (col.gameObject.name == "racket_left")
        {
            _speed = Mathf.Min(_speed + 7f, SPEED_MAX);

            float y = HitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
            Vector2 dir = new Vector2(1, y).normalized;
            GetComponent<Rigidbody2D>().velocity = dir * _speed;
            GetComponent<TrailRenderer>().material.SetColor("_EmissionColor", new Color(0f, 240f, 210f, 1f));
        }
        if (col.gameObject.name == "racket_right")
        {
            _speed = Mathf.Min(_speed + 7f, SPEED_MAX);

            float y = HitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
            Vector2 dir = new Vector2(-1, y).normalized;
            GetComponent<Rigidbody2D>().velocity = dir * _speed;
            GetComponent<TrailRenderer>().material.SetColor("_EmissionColor", new Color(255f, 0f, 100f, 1f));
        }
        if (col.gameObject.name == "wall_right") {
            GetComponent<Rigidbody2D>().position = new Vector3(0, 0, 0);
            GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
            GetComponent<TrailRenderer>().Clear();
            GameObject go = GameObject.Find("score_left");
            ScoreManager sm = (ScoreManager)go.GetComponent(typeof(ScoreManager));
            sm.Increment();
            _speed = speed;
        }
        if (col.gameObject.name == "wall_left") {
            GetComponent<Rigidbody2D>().position = new Vector3(0, 0, 0);
            GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            GetComponent<TrailRenderer>().Clear();
            GameObject go = GameObject.Find("score_right");
            ScoreManager sm = (ScoreManager)go.GetComponent(typeof(ScoreManager));
            sm.Increment();
            _speed = speed;
        }
    }

}