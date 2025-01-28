using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private float destroyAfterTime = 10.20f; // portal destroy time
    private Transform exitPoint;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime()); 
    }

    public void SetExitPoint(Transform exit)
    {
        exitPoint = exit;
    }

    private void OnTriggerEnter(Collider other) // if collision happened call teleport func
    {
        if (other.CompareTag("Player"))
        {   
            Teleport(other.transform);
        }
    }

    private void Teleport(Transform player) //teleport player to exit pint
    {
        player.position = exitPoint.position;
    }


    private IEnumerator DestroyAfterTime()  //destroy object after time
    {
        yield return new WaitForSeconds(destroyAfterTime);
        Destroy(gameObject);
    }
}

