using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor;
# endif

public class dusmanKontrol : MonoBehaviour
{
    bool aradakiMesafeyiBirKereAl = true;
    bool ilerimiGerimi = true;

    GameObject[] gidilecekNoktalar;
    GameObject karakter;
    Vector3 aradakiMesafe;
    int aradakiMesafeSayacı = 0;
    RaycastHit2D ray;
    public LayerMask layermask;
    int hiz;
    public Sprite onTaraf;
    public Sprite arkaTaraf;
    SpriteRenderer spriteRenderer;
    public GameObject kursun;
    float atesZamani = 0;
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();


        karakter = GameObject.FindGameObjectWithTag("Player");
        gidilecekNoktalar = new GameObject[transform.childCount];
        for (int i = 0; i < gidilecekNoktalar.Length; i++)
        {
            gidilecekNoktalar[i] = transform.GetChild(0).gameObject;
            gidilecekNoktalar[i].transform.SetParent(transform.parent);
        }
    }


    void FixedUpdate()
    {

        beniGordumu();
        if (ray.collider.tag==("Player"))
        {
            Debug.Log("goruyor");
            hiz = 10;
            spriteRenderer.sprite = onTaraf;
            atesEt();
        }
        else
        {
            Debug.Log("gormuyor");
            hiz = 4;
            spriteRenderer.sprite = arkaTaraf;
        }
        noktalaraGit();
        

    }
    void atesEt()
    {
        atesZamani += Time.deltaTime;
        if (atesZamani > Random.Range(0.2f, 1))
        {
            Instantiate(kursun, transform.position, Quaternion.identity);
            atesZamani = 0;
        }
        Destroy(GameObject.FindGameObjectWithTag("kursun"), 1.5f);


    }
    void beniGordumu()
    {
        Vector3 rayYonum = karakter.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position,rayYonum,1000,layermask);
        Debug.DrawLine(transform.position, ray.point, Color.red);


    }

    void noktalaraGit()
    {
        if (aradakiMesafeyiBirKereAl)
        {
            aradakiMesafe = (gidilecekNoktalar[aradakiMesafeSayacı].transform.position - transform.position).normalized;
            aradakiMesafeyiBirKereAl = false;

        }
        float mesafe = Vector3.Distance(transform.position, gidilecekNoktalar[aradakiMesafeSayacı].transform.position);
        transform.position += aradakiMesafe * Time.deltaTime * hiz;
        if (mesafe < 0.5f)
        {
            aradakiMesafeyiBirKereAl = true;
            if (aradakiMesafeSayacı == gidilecekNoktalar.Length - 1)
            {
                ilerimiGerimi = false;
            }
            else if (aradakiMesafeSayacı == 0)
            {
                ilerimiGerimi = true;

            }
            if (ilerimiGerimi)
            {
                aradakiMesafeSayacı++;
            }
            else
            {
                aradakiMesafeSayacı--;
            }


        }

    }
    public Vector2 getYon()
    {
        return (karakter.transform.position - transform.position).normalized;

    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }



    }
# endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(dusmanKontrol))]
[System.Serializable]
class dusmanKontrolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        dusmanKontrol script = (dusmanKontrol)target; //testere adlı scripte ulaşıyor
        if (GUILayout.Button("Create", GUILayout.Width(100)))
        {
            GameObject yeniObje = new GameObject();
            yeniObje.transform.parent = script.transform;
            yeniObje.transform.position = script.transform.position;
            yeniObje.name = script.transform.childCount.ToString();
        }
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layermask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onTaraf"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("arkaTaraf"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("kursun"));
        serializedObject.ApplyModifiedProperties();//
        serializedObject.Update();//inspectorda kontrol etmek için bu 3 satırı yazdık. Normalde public olarak tanımladığımız değişkenleri inspectorda kontrol edebiliyorduk fakat unity editor kullandığımız için bu 3 satırı yazdık.
    }
}
#endif