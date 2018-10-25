using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

  public SpriteRenderer sprite;
  public LayerMask groundLayer;
  public Transform groundCheck;
  public float groundCheckRadius;
  private bool groundCollision;
  private bool doubleJumped;
  public CanvasGroup canvasYouWin;
  private int lives = 3;
  public Text LivesText;

  void Start () {
    SetLivesText ();
  }

  void FixedUpdate () {
    groundCollision = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer);
  }

  void Update () {
    var rigidBody = GetComponent<Rigidbody2D> ();
    var transform = GetComponent<Transform> ();
    if (Input.GetKey ("d")) {
      sprite.flipX = false;
      if (Input.GetKey ("left shift")) {
        rigidBody.velocity = new Vector2 (10, rigidBody.velocity.y);
      } else {
        rigidBody.velocity = new Vector2 (5, rigidBody.velocity.y);
      }
    }
    if (Input.GetKey ("a")) {
      sprite.flipX = true;
      if (Input.GetKey ("left shift")) {
        rigidBody.velocity = new Vector2 (-10, rigidBody.velocity.y);
      } else {
        rigidBody.velocity = new Vector2 (-5, rigidBody.velocity.y);
      }
    }

    if (groundCollision) {
      doubleJumped = false;
    }
    
    if (Input.GetKeyDown ("space") && groundCollision) {
      rigidBody.velocity = new Vector2 (rigidBody.velocity.x, 10);
    }

    if (Input.GetKeyDown ("space") && !groundCollision && !doubleJumped) {
      rigidBody.velocity = new Vector2 (rigidBody.velocity.x, 10);
      doubleJumped = true;
    }
    if (transform.position.y < -10) {
      lives -= 1;
      SetLivesText ();
      transform.position = new Vector2 (-12, -3);
    }
    if (transform.position.y <= -5) {
      if (transform.position.x >= 24) {
        if (transform.position.x <= 26)
        transform.position = new Vector2 (41, -7);
      }
    }
    if (lives == 0) {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  }  
    
  public void OnTriggerEnter2D (Collider2D other) {
    if (other.name == "EnemyDamage") {
      lives -= 1;
      SetLivesText ();
      transform.position = new Vector2 (-12, -3);
    } else if (other.name == "Exit") {
      canvasYouWin.alpha = 1;
      lives = 100;
    }
  }
  void SetLivesText () {
    LivesText.text = "Lives: " + lives.ToString ();
  }
}