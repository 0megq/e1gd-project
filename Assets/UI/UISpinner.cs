using UnityEngine;

public class UISpinner : MonoBehaviour
{

    public float spinSpeed = 100f;
    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}
