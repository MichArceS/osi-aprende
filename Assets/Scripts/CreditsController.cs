using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    private float speed = 200f;
    private Vector2 init_position;

    void Start()
    {
        init_position = transform.GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        if (transform.GetComponent<RectTransform>().anchoredPosition.y >= 4588)
        {
            transform.GetComponent<RectTransform>().anchoredPosition = init_position;
        }
        else transform.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, Time.deltaTime * speed);
    }
}
