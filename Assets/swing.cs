using UnityEngine;
using System.Collections;
 
 
 
public class swing : MonoBehaviour
{
 
    public float min=1f;
    public float max=2f;
    // Use this for initialization
    void Start () {
       
        min=transform.position.z;
        max=transform.position.z+1;
   
    }
   
    // Update is called once per frame
    void Update () {
       
       
        transform.position =new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time / 16 ,max-min)+min); //Time.time
       
    }
}