using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nitro : MonoBehaviour
{
    [SerializeField] private float nitroDuration = 5.0f; 
    [SerializeField] private float nitroSpeed = 10.0f; 
    
    private bool isNitroActive = true;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            CarController carController = collider.GetComponent<CarController>();
            if (carController != null)
            {
                isNitroActive = false;
                GetComponent<Renderer>().enabled = false;
                StartCoroutine(ActivateNitro(carController));
            }
        }
    }

    private IEnumerator ActivateNitro(CarController carController)
    {
        carController.SetNitroSpeed(nitroSpeed); 
        float remainingTime = nitroDuration;
        while (remainingTime > 0)
        {
            Debug.Log("nitro duration " + remainingTime);
            yield return new WaitForSeconds(1.0f);
            remainingTime -= 1.0f;
        }
        carController.ResetSpeed();
        
        Destroy(gameObject);
    }
}