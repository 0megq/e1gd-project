using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpPow;
    [SerializeField] float sens;
    
    private Rigidbody rb;
    private Vector3 move;
    private bool onGround;
    private Camera cam;
    private float xRot = 0f;
    private float yRot = 0f;
    private float mouseX;
    private float mouseY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        yRot += mouseX;

        cam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        this.transform.localRotation = Quaternion.Euler(0f, yRot, 0f);

        this.transform.position += move.x * speed * this.transform.right;
        this.transform.position += move.y * speed * this.transform.forward;
    }

    void OnLook(InputValue val){
        mouseX = val.Get<Vector2>().x * sens;
        mouseY = val.Get<Vector2>().y * sens;
    }

    void OnQuit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void OnMove(InputValue val){
        move = val.Get<Vector2>();
    }

    void OnJump(){
        if(onGround){
            rb.AddForce(Vector3.up * jumpPow, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            onGround = false;
        }
    }
}
