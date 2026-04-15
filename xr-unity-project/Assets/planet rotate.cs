using UnityEngine;

public class planetrotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float angle = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame 
    void Update()
    {
        transform.Rotate(transform.up, angle * Time.deltaTime, Space.World);
       
    }
}
