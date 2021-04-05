using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    #region Variables
    Vector3 playerPos;
    Vector3 camPos;
    Rigidbody rb;
    Camera camara;
    GameObject enemigos;
    Renderer rend;
    bool vivo;
    bool auto = true;
    bool ready;
    float escala;
    int currentLevel;
    int bestLevel;

    [SerializeField]
    float jumpPower;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float fallSpeed;
    [SerializeField]
    float maxFall;
    [SerializeField]
    float maxJump;
    [SerializeField]
    float maxAltura;
    [SerializeField]
    float minAltura;
    [SerializeField]
    int maxLevel;

    [SerializeField]
    GameObject deathScreen;
    [SerializeField]
    GameObject nextLevelButton;
    [SerializeField]
    GameObject textoInstruccion;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    Transform botonesLvl;
#endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camara = Camera.main;
        escala = -camara.transform.position.z;
        playerPos = transform.position;
        camPos = camara.transform.position;
        PlayerData data = Saves.control.Load();
        bestLevel = data.level;
        rend = GetComponent<Renderer>();
        rend.material.color = ColorParse(data.color);
        CargaBotones();        
    }

    Color ColorParse(ColorSerial color)
    {
        Color colorSerial;
        colorSerial.r = color.r;
        colorSerial.g = color.g;
        colorSerial.b = color.b;
        colorSerial.a = color.a;
        return colorSerial;
    }

    void CargaBotones()
    {
        for (int i = 0; i < bestLevel; i++)
        {
            botonesLvl.GetChild(i).gameObject.SetActive(true);
        }
    }

	// Update is called once per frame
	void Update () {
        Movimiento();
	}

    void Movimiento()
    {
        Fall();
        if (vivo)
        {
            Move();
            if (Input.GetMouseButtonDown(0))
            {
                Jump();
            }
            if (transform.position.y > maxAltura || transform.position.y < -minAltura)
            {
                Muerte(false);
            }
        }
        else if (auto)
        {
            if (Input.GetMouseButtonDown(0) && ready && transform.position.x == -1.5f)
            {
                vivo = true;
                auto = false;
                ready = false;
                textoInstruccion.SetActive(false);
                EnemyScript[] enemigosAux = enemigos.GetComponentsInChildren<EnemyScript>();
                for (int i = 0; i < enemigosAux.Length; i++)
                {
                    if (enemigosAux[i])
                    {
                        enemigosAux[i].SetActivo(true);
                    }
                }
            }
            if (transform.position.x <= -1.5f)
            {
                Move();
            }            
            if (transform.position.x >= -1.5f)
            {
                Vector3 aux = rb.velocity;
                aux.x = 0;
                rb.velocity = aux;
                Vector2 aux2 = transform.position;
                aux2.x = -1.5f;
                transform.position = aux2;
                if (!ready)
                {
                    menu.SetActive(true);
                }
            }
            if (transform.position.y <= -0.97f)
            {
                Jump();
            }            
        }        
        if (rb.velocity.y >= maxJump)
        {
            Vector3 aux = rb.velocity;
            aux.y = maxJump;
            rb.velocity = aux;
        }        
    }

    void Jump()
    {
        Vector3 aux = rb.velocity;
        aux.y = jumpPower * escala * Time.deltaTime;
        rb.velocity = aux;        
        //rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    void Fall()
    {
        Vector3 aux = rb.velocity;
        aux.y -= fallSpeed * escala * Time.deltaTime;
        if (aux.y <= -maxFall)
        {
            aux.y = -maxFall;
        }
        rb.velocity = aux;
    }

    void Move()
    {
        Vector3 aux = rb.velocity;
        aux.x = moveSpeed * escala * Time.deltaTime;
        rb.velocity = aux;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Muerte(true);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Finish")
        {
            if (vivo)
            {
                Win();
            }
        }
    }

    void Muerte(bool choco)
    {
        vivo = false;
        if (!choco)
        {
            rb.velocity = Vector3.zero;
        }
        camara.GetComponent<CameraScript>().SetFollow(false);
        //rb.useGravity = true;
        deathScreen.SetActive(true);
    }

    void Win()
    {
        vivo = false;
        deathScreen.SetActive(true);        
        if (currentLevel < maxLevel)
        {
            nextLevelButton.SetActive(true);
            if (currentLevel == bestLevel)
            {
                bestLevel = currentLevel + 1;
                Saves.control.Save(bestLevel, rend.material.color);
            }
        }
        CargaBotones();
    }

    public void CambiarColor(Image imagen)
    {
        rend.material.color = imagen.color;
        Saves.control.Save(bestLevel, imagen.color);
    }

    public void Play(int level)
    {
        if (level <= maxLevel)
        {
            if (level == 0)
            {
                LoadLevel(bestLevel);
                currentLevel = bestLevel;
            }
            else
            {
                LoadLevel(level);
                currentLevel = level;
            }
            auto = true;
            ready = true;
            camara.GetComponent<CameraScript>().SetFollow(true);
            textoInstruccion.SetActive(true);
            nextLevelButton.SetActive(false);
        }        
    }

    public void Replay(bool listo)
    {
        rb.velocity = Vector3.zero;
        transform.position = playerPos;
        camara.transform.position = camPos;
        auto = true;
        ready = listo;        
        if (listo)
        {
            textoInstruccion.SetActive(true);
        }
        camara.GetComponent<CameraScript>().SetFollow(true);
        nextLevelButton.SetActive(false);
        if (currentLevel > 5)
        {
            LoadLevel(currentLevel);
        }
    }

    public void LoadLevel(int level)
    {
        if (enemigos)
        {
            Destroy(enemigos);
        }
        enemigos = Instantiate(Resources.Load<GameObject>("Levels/" + "Level " + level ), Vector3.zero, Quaternion.identity);
        currentLevel = level;
    }

    public void NextLevel()
    {
        LoadLevel(currentLevel + 1);
        Replay(true);
        nextLevelButton.SetActive(false);
    }
}
