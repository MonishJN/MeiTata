using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class RedAnimation : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;


    public void PlayerTree(Vector2 movement) {
        playerAnimator.SetFloat("xInput", movement.x);
        playerAnimator.SetFloat("yInput", movement.y);
        
    }
    public void OnGameOver()
    {
        playerAnimator.Play("Red GameOver");

    }
    public void OnGameOverAnimationComplete()
    {
        Destroy(transform.parent.gameObject);
        GameManager.Instance.LevelCompleted();
    }
    public void OnShoot() {
        playerAnimator.SetTrigger("shoot");
        StartCoroutine(ReturnToIdle());
    }
    IEnumerator ReturnToIdle()
    {
        // Wait for 0.1 seconds (or whatever feels right for the "pop")
        yield return new WaitForSeconds(0.3f);

        // Force the animator to go straight to Idle, ignoring all arrows
        playerAnimator.Play("Player Tree");
    }
}
