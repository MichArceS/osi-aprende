using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MangerCamaraTerremoto : MonoBehaviour
{
    public Vector2 targetPosition; // La posici�n a la que se mover� la c�mara
    public float speed = 2f; // Velocidad de movimiento

    void Start()
    {
        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        while (true)
        {
            // Mueve la c�mara hacia la posici�n objetivo
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), speed * Time.deltaTime);

            // Si llega a la posici�n objetivo, termina la coroutine
            if (Vector3.Distance(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z)) < 0.1f)
            {
                Debug.Log("Reached target position.");
                yield break; // Termina la coroutine
            }

            yield return null; // Espera un frame
        }
    }
}
