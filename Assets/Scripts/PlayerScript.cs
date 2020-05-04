using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour
{


    public float jumpPower = 10.0f;
    public float moveSpeed = 5.0f;
    public float fallMultiplier = 1.5f;

    Rigidbody2D myRigidBody;
    private bool isGrounded = true;
    private bool isDead = false;
    private bool win = false;

    private Animator animation;
    private SoundScript sounds;
    private Scene scene;

    private GameObject instruction;

    void Start()
    {
        instruction = GameObject.FindGameObjectWithTag("Instructions");

        // checks current scene
        scene = SceneManager.GetActiveScene();

        // Makes instructions visible
        if (scene.name == "level1")
        {
            instruction.SetActive(true);
        }

        Time.timeScale = 2f;
        myRigidBody = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        sounds = GetComponent<SoundScript>();

        StartGame();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    void StartGame()
    {
        StartCoroutine(StartFromTheStart());
    }


    void FixedUpdate()
    {
        // Checks if no death and no victory or if position is at zero and starts moving
        if (((!isDead) && !win) || myRigidBody.position.x == 0)
        {
            isDead = false;
            myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);

            // Jumps when Space is pressed
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                if (scene.name == "level1")
                {
                    instruction.SetActive(false);
                }
                
                Jump();
            }

            // If player starts falling, increases falling speed for smoother gameplay
            if (myRigidBody.velocity.y < 0)
            {

                myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
            }
        }
    }

    public void Jump()
    {
        // Prevents from double jump
        if ((!isGrounded) && myRigidBody.rotation != 0) return;

        sounds.SoundPlay("jump");

        // Plays Animation, stops if on ground, but animation still going
        animation.SetTrigger("Jump");
        if (isGrounded) animation.StopPlayback();


        myRigidBody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
    }


    public void YouDied()
    {
        // You died
        isDead = true;
        sounds.SoundPlay("death");

        StartGame();
    }


    public void Stop()
    {
        // Stops all player movement 
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }


    IEnumerator StartFromTheStart()
    {
        // Stops, waits and goes to starting position
        Stop();

        yield return new WaitForSeconds(2f);
        isDead = false;
        transform.position = new Vector2(0, 0.015f);
        yield return new WaitForSeconds(0.5f);

        // Allows player to move
        myRigidBody.constraints = RigidbodyConstraints2D.None;
    }

    IEnumerator Victory()
    {
        Stop();
        win = true;

        // Actions based on which scene is active
        if (scene.name == "level1")
        {
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(2);
        }
        else if (scene.name == "level2")
        {
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(0);
        }
    }

    // Activated when to BoxColliders2D touches each other
    void OnCollisionEnter2D(Collision2D gameObj)
    {
        // actions based on what player touches
        if ((gameObj.collider.tag == "Ground"))
        {
            // checks if landing on top of platform
            if ((myRigidBody.transform.position.y - gameObj.transform.position.y) > 0.5)
            {
                isGrounded = true;
            }
            else
            {
                YouDied();
            }
        }

        if (gameObj.collider.tag == "Spike")
        {
            YouDied();
        }

        if (gameObj.collider.CompareTag("Finish"))
        {
            StartCoroutine("Victory");
        }
    }

    // Active while touching ground 
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    // Actives when not touching ground aka doing the jump
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
