using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private static int boardWidth = 28;
    private static int boardHeight = 36;

    private bool didStartDeath = false;

    public int totalPellets = 0;
    public int score = 0;
    public int pacManLifes = 3;

    public AudioClip backgroundAudioNormal;
    public AudioClip backgroundAudioFrightened;
    public AudioClip backgroundAudioPacMenDeath;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    //gather all objects into the board to use later
    //like pieces put into the game box
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;

            if (o.name != "PacMan" && o.tag != "Pellet" && o.tag != "Ghost" && o.tag != "GhostHome")
            {
                board[(int)pos.x, (int)pos.y] = o;
            }
        }
    }

    public void StartDeath()
    {
        if (!didStartDeath)
        {
            didStartDeath = true;

            //stops ghost movement
            GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (GameObject ghost in o)
            {
                ghost.transform.GetComponent<Ghost>().canMove = false;
            }

            //stops PacMan movement
            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<PacMan>().canMove = false;
            pacMan.transform.GetComponent<Animator>().enabled = false;

            //stops music
            transform.GetComponent<AudioSource>().Stop();

            StartCoroutine(ProcessDeathAfter(2));
        }
    }

    //IEnumerator prevents game locking up
    //ex: breaking a loop after a specific time
    IEnumerator ProcessDeathAfter(float delay)
    {
        //yield prevents locking up the game
        yield return new WaitForSeconds(delay);

        //invisible ghosts!!!
        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
        }

        //1.9 is how long the audio clip is
        StartCoroutine(ProcessDeathAnimation(1.9f));
    }

    IEnumerator ProcessDeathAnimation(float delay)
    {
        GameObject pacMan = GameObject.Find("PacMan");

        //prevents weird animation either by scale or rotation
        pacMan.transform.localScale = new Vector3(1, 1, 1);
        pacMan.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //stes new animation and makes sure PacMan is not facing a wall
        pacMan.transform.GetComponent<Animator>().runtimeAnimatorController = pacMan.transform.GetComponent<PacMan>().deathAnimation;
        pacMan.transform.GetComponent<Animator>().enabled = true;

        //play death animation sound
        transform.GetComponent<AudioSource>().clip = backgroundAudioPacMenDeath;
        transform.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(delay);

        StartCoroutine(ProcessRestart(2));
    }

    IEnumerator ProcessRestart(float delay)
    {
        //invisible PacMan!!!
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

        //death animation sound does not loop
        transform.GetComponent<AudioSource>().Stop();

        yield return new WaitForSeconds(delay);

        Restart();
    }

    public void Restart()
    {
        pacManLifes -= 1;
        
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<PacMan>().Restart();

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().Restart();
        }

        //reseting background audio
        transform.GetComponent<AudioSource>().clip = backgroundAudioNormal;
        transform.GetComponent<AudioSource>().Play();

        didStartDeath = true;
    }
}
