using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    bool activo;    

    [SerializeField]
    bool upDown;
    [SerializeField]
    bool square;
    [SerializeField]
    bool upOrDown;
    [SerializeField]
    bool rotate;
    [SerializeField]
    bool clockwise;
    [SerializeField]
    bool movingLeft;
    [SerializeField]
    bool movingRight;
    [SerializeField]
    bool subiendo;
    [SerializeField]
    float maxAltura;
    [SerializeField]
    float minAltura;
    [SerializeField]
    float maxX;
    [SerializeField]
    float minX;
    [SerializeField]
    float speed;

    //void Awake()
    //{
    //    //if (square)
    //    //{
    //    //    float posicionX = transform.position.x;
    //    //    float posicionY = transform.position.y;
    //    //    minAltura += posicionY;
    //    //    maxAltura += posicionY;
    //    //    maxX += posicionX;
    //    //    minX += posicionX;
    //    //}
    //}
	
	// Update is called once per frame
	void Update () {

        if (activo)
        {
            Comportamiento();
        }
	}

    void Comportamiento()
    {

        float posicionX = transform.position.x;
        float posicionY = transform.position.y;

        if (upDown)
        {
            if (subiendo)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                if (transform.position.y >= maxAltura)
                {
                    SetY(maxAltura);
                    subiendo = false; 
                }
            }
            else
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                if (transform.position.y <= minAltura)
                {
                    SetY(minAltura);
                    subiendo = true;
                }
            }
        }
        else if (square)
        {
            if (movingRight)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (posicionX >= maxX)
                {
                    if (!clockwise)
                    {
                        subiendo = true;                        
                    }
                    movingRight = false;
                }
            }
            else if (subiendo)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                if (posicionY >= maxAltura)
                {
                    if (!clockwise)
                    {
                        movingLeft = true;
                    }
                    else
                    {
                        movingRight = true;
                    }
                    subiendo = false;
                }
            }
            else if (movingLeft)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (posicionX <= minX)
                {
                    movingLeft = false;
                    if (clockwise)
                    {
                        subiendo = true;
                    }
                }
            }
            else if (!subiendo && !movingLeft && !movingRight)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                if (posicionY <= minAltura)
                {
                    if (!clockwise)
                    {
                        movingRight = true;
                    }
                    else
                    {
                        movingLeft = true;
                    }
                }
            }
        }
        else if (upOrDown)
        {
            if (subiendo)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                if (transform.position.y >= maxAltura)
                {
                    SetY(minAltura);
                }
            }
            else
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                if (transform.position.y <= minAltura)
                {
                    SetY(maxAltura);
                }
            }
        }
        else if (rotate)
        {
            if (clockwise)
            {
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
            }
            else
            {
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }

    void SetY(float y)
    {
        Vector3 aux = transform.position;
        aux.y = y;
        transform.position = aux;
    }

    public void SetActivo(bool bol)
    {
        activo = bol;
    }
}
