using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor;
# endif

public class testere : MonoBehaviour
{
    bool aradakiMesafeyiBirKereAl = true;
    bool ilerimiGerimi = true;

    GameObject[] gidilecekNoktalar;
    Vector3 aradakiMesafe;
    int aradakiMesafeSayacı = 0;
    
    void Start()
    {
        gidilecekNoktalar = new GameObject[transform.childCount];
        for(int i = 0; i < gidilecekNoktalar.Length; i++)
        {
            gidilecekNoktalar[i] = transform.GetChild(0).gameObject;
            gidilecekNoktalar[i].transform.SetParent(transform.parent);
        }
    }

    
    void FixedUpdate()
    {
        
        transform.Rotate(0, 0, 5);
        noktalaraGit() ;

      
    }

    void noktalaraGit()
    {
        if (aradakiMesafeyiBirKereAl)
        {
            aradakiMesafe = (gidilecekNoktalar[aradakiMesafeSayacı].transform.position - transform.position).normalized;
            aradakiMesafeyiBirKereAl = false;

        }
        float mesafe = Vector3.Distance(transform.position, gidilecekNoktalar[aradakiMesafeSayacı].transform.position);
        transform.position += aradakiMesafe * Time.deltaTime * 10;
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


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }

        for (int i = 0; i < transform.childCount-1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i+1).transform.position);
        }



    }
# endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(testere))]
[System.Serializable]
class testereEditor:Editor
{
    public override void OnInspectorGUI()
    {
        testere script = (testere)target; //testere adlı scripte ulaşıyor
        if (GUILayout.Button("Create",GUILayout.Width(100)))
        {
            GameObject yeniObje = new GameObject();
            yeniObje.transform.parent = script.transform;
            yeniObje.transform.position = script.transform.position;
            yeniObje.name = script.transform.childCount.ToString();
        }

    }
}
#endif