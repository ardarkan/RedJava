using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class KarakterKontrol : MonoBehaviour
{
    public Sprite[] beklemeAnim;
    public Sprite[] ziplamaAnim;
    public Sprite[] yurumeAnim;
    public Text canText;
    public Image deathImage;
    int can = 20;
    


    int beklemeAnimSayac;
    int ziplamaAnimSayac;
    int kosmaAnimSayac;

    SpriteRenderer spriteRendere;
    Rigidbody2D fizik;
    Vector3 vec;
    Vector3 kameraSonPos;
    Vector3 kameraIlkPos;
    float horizontal = 0;
    float beklemeAnimZaman = 0;
    float yurumeAnimZaman = 0;
    float deathImageSayac = 0;
    float anaMenuyeDonSayac = 0;
    
    bool ziplacount = true;
    GameObject kamera;
    
    
    void Start()
        

    {
       
        Time.timeScale = 1;
        PlayerPrefs.DeleteAll();
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("kacincilevel"))
        {
            PlayerPrefs.SetInt("kacincilevel", SceneManager.GetActiveScene().buildIndex);
        }
        
        spriteRendere = GetComponent<SpriteRenderer>();
        fizik = GetComponent<Rigidbody2D>();
        kamera = GameObject.FindGameObjectWithTag("MainCamera");
        kameraIlkPos = kamera.transform.position - transform.position;
        canText.text = "CAN  " + can;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            if (ziplacount)
            {
                fizik.AddForce(new Vector2(0, 500));
                ziplacount = false;
            }
            

        }
       
    }
    void FixedUpdate()
    {
        
        karakterHareket();
        Animasyon();
        if (can <= 0)
        {
            Time.timeScale = 0.4f;
            canText.enabled = false;
            deathImageSayac += 0.03f;
            deathImage.color = new Color(0, 0, 0, deathImageSayac);
            anaMenuyeDonSayac += Time.deltaTime;
            if (anaMenuyeDonSayac > 1)
            {
                SceneManager.LoadScene("AnaMenu");
            }
            
        }
        if(transform.position.y<=-10){
                SceneManager.LoadScene("AnaMenu");
            }
    }
    void LateUpdate()
    {
        kameraKontrol();
    }
    void karakterHareket()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vec = new Vector3(horizontal * 10, fizik.velocity.y, 0);
        fizik.velocity = vec;

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        ziplacount = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "kursun")
        {
            can--;
            canText.text = "CAN  " + can;
        }
        if (collision.gameObject.tag == "dusman")
        {
            can-=10;
            canText.text = "CAN  " + can;
        }
        if (collision.gameObject.tag == "saw")
        {
            can-=10;
            canText.text = "CAN  " + can;
        }
        if (collision.gameObject.tag == "nextLevel")
        {   
            if(SceneManager.GetActiveScene().buildIndex==3){
                SceneManager.LoadScene("AnaMenu");
            }
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
        }
    }

    void kameraKontrol()
    {
        kameraSonPos = kameraIlkPos + transform.position;
        kamera.transform.position = Vector3.Lerp(kamera.transform.position,kameraSonPos,0.08f);
    }
    void Animasyon()
    {
        if (ziplacount)
        {
            if (horizontal == 0)
            {
                beklemeAnimZaman += Time.deltaTime;
                if (beklemeAnimZaman > 0.05f)
                {
                    spriteRendere.sprite = beklemeAnim[beklemeAnimSayac++];
                    if (beklemeAnimSayac == 12)
                    {
                        beklemeAnimSayac = 0;
                    }
                    beklemeAnimZaman = 0;
                }

            }
            else if (horizontal > 0)
            {

                transform.localRotation = Quaternion.Euler(0, 0, 0);
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.05f)
                {
                    spriteRendere.sprite = yurumeAnim[kosmaAnimSayac++];
                    if (kosmaAnimSayac == 17)
                    {
                        kosmaAnimSayac = 0;
                    }
                    yurumeAnimZaman = 0;
                }
            }
            else if (horizontal < 0)
            {

                transform.localRotation = Quaternion.Euler(0, 180, 0);
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.05f)
                {
                    spriteRendere.sprite = yurumeAnim[kosmaAnimSayac++];
                    if (kosmaAnimSayac == 17)
                    {
                        kosmaAnimSayac = 0;
                    }
                    yurumeAnimZaman = 0;
                }


            }
        }
        else
        {
            if (fizik.velocity.y>0)
            {
                spriteRendere.sprite = ziplamaAnim[0];

            }
            else
            {
                spriteRendere.sprite = ziplamaAnim[1];
            }
            if (horizontal > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if(horizontal < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
