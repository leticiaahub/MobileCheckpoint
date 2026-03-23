using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config do player lane/ pulo/ speed
    [Header("Player Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float stepSpeed;
    [SerializeField] private float currentLane = 0;
    [SerializeField] private float laneLimit = 2;
    [SerializeField] private float jump = 2.0f;
    

    // config  do controle
    [Header("Controller Settings")]
    [SerializeField] private float gravity = 0f;
    [SerializeField] private Vector3 movement;
     private bool noChao;
    [SerializeField] private GameObject painel;
    [SerializeField] private GameObject painelDie;
  

    // vida
    [Header("Vida Settings")]
    [SerializeField] private Slider slid;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float vidaAtual;
    [SerializeField] private float drainSpeed = 20f;

    private Vector3 nowPos;
    private CharacterController control;



    private void Start()
    {
        Time.timeScale = 1f; // pra reiniciar com o tempo certo

        control = GetComponent<CharacterController>();

        nowPos = transform.position;

        
        vidaAtual = maxHealth; //start com a vida max
        if (slid != null)
        {
            slid.maxValue = maxHealth;
            slid.value = maxHealth;
        }

    }

    private void Update()
    {


         if ( vidaAtual > 0)
         {
             vidaAtual -= drainSpeed * Time.deltaTime;
             if (slid!= null) slid.value = vidaAtual;
         }
         else
         {
             Morrer(); 
         }

         movement.z = speed;


         Vector3 targetPosition = new Vector3(currentLane, transform.position.y, transform.position.z);
         Vector3 diff = targetPosition - transform.position;
         movement.x = diff.x * stepSpeed;


         noChao = control.isGrounded;


         if (noChao && movement.y < 0)
         {
             movement.y = -2f;
         }


        if (Input.GetButtonDown("Jump") && noChao)
        {
            Jump();
        }

        else
        {

            movement.y += gravity * Time.deltaTime;
        }

         control.Move(movement * Time.deltaTime);

        
    }

    
    public void RestoreHealth (float amount)
    {
        vidaAtual += amount;
        if (vidaAtual > maxHealth) vidaAtual = maxHealth;
        if (slid != null) slid.value = vidaAtual;
    }

    public void ChangeLane(int direction)
    {
        if (direction < 0)
        {
            if (currentLane > -laneLimit)
            {
                currentLane += direction;
            }
        }
        else if (direction > 0)
        {
            if (currentLane < laneLimit)
            {
                currentLane += direction;
            }
        }     

}

  

    private void OnControllerColliderHit (ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Morrer();
        }

        if (hit.gameObject.CompareTag("End"))
        {
            painel.SetActive(true);
            Time.timeScale = 0f;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Caixa"))
        {
            RestoreHealth(20f);
            Destroy(other.gameObject);
        }
    }

    void Morrer()
    {

        Time.timeScale = 0f;
        painelDie.SetActive(true);
    }


    public void Jump()
{
    if (noChao == true)
    {
            movement.y = jump;
    }
}

    public void Caindo()
    {
        
        if (noChao == false)
        {
            movement.y = -20f; 
        }
    }


}