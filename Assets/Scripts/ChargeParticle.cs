using UnityEngine;

public class ChargeParticle : MonoBehaviour
{
    [SerializeField] private Rigidbody2D particleRigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private string particleOrientation;

    private Vector2 direction;


    private void Start()
    {
        direction = Player.Instance.particleDirection;
    }


    private void FixedUpdate()
    {
        particleRigidBody.linearVelocity =direction * 20;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Magnetic Field"))
        {
            MagneticField magneticField = collision.GetComponent<MagneticField>();
            string fieldOrientation = magneticField.GetFieldOrientation();
            if (particleOrientation == fieldOrientation)
            {
                magneticField.ChangeFieldOrientation();
                Destroy(gameObject);
            }
            else
            {
                magneticField.ChangeFieldOrientation();
                ChangeOrientation();
            }

        }
        else if (collision.CompareTag("Neutral Field"))
        {
            ChangeOrientation();
        }
        else if (collision.gameObject.name == "Object") { 
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Door"))
        {
            Door door = collision.GetComponent<Door>();
            string doorOrientaion = door.GetDoorOrientaion();
            door.ChangeDoorOrientation();
            if (particleOrientation == doorOrientaion)
            {

                Destroy(gameObject);
            }

        }
        else if (collision.gameObject.name == "Terrain")
        {
            Destroy(gameObject);
        }
    }

    public string GetParticleOrientation() { 
        return particleOrientation;
    }
    public void ChangeOrientation() {
        if (particleOrientation == "North")
        {
            particleOrientation = "South";
            spriteRenderer.color = new Color(0.208f, 0.757f, 0.843f, 0.992f);
        }
        else {
            particleOrientation = "North";
            spriteRenderer.color = new Color(0.851f, 0.341f, 0.388f, 1.0f);


        }
    }
}
