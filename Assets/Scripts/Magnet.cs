using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] public float rightEdge;
    [SerializeField] public float leftEdge;
    [SerializeField] public float topEdge;
    [SerializeField] public float bottomEdge;

    [SerializeField] private Movement movement;
    
    public MagnetType type;

    [SerializeField] private float time = 0f;

    private enum Movement { 
        MovesX,
        MovesY,
        DontMove
    }
    public enum MagnetType { 
        VerticalMagnet,
        HorizontalMagnet
    }
    private void Awake()
    {

    }

    private void Update()
    {
        if (movement != Movement.DontMove) {
            if (time % 4 >= 2)
            {
                transform.position = transform.position + new Vector3(4 * Time.deltaTime, 0, 0);
                time += Time.deltaTime;
            }
            else
            {
                transform.position = transform.position + new Vector3(-4 * Time.deltaTime, 0, 0);
                time += Time.deltaTime;
            }
        }
    }

}
