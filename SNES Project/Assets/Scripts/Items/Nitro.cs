using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nitro : MonoBehaviour
{
    [SerializeField] private float nitroDuration = 5.0f;
    [SerializeField] private float nitroSpeed = 10.0f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            CarController player = collider.GetComponent<CarController>();
            if (player != null)
            {
                StartCoroutine(ApplyNitroEffect(player));
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator ApplyNitroEffect(CarController player)
    {
        Debug.Log($"Speed Before Nitro: {player.currentSpeed}");
        
        player.ModifySpeed(nitroSpeed);
        
        yield return new WaitForSeconds(nitroDuration);
        
        Debug.Log($"Speed After Nitro: {player.currentSpeed}");
        player.ModifySpeed(-nitroSpeed);

    }
}