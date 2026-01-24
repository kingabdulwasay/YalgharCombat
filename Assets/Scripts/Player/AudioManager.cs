using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource walk;
    public AudioSource run;
    public AudioSource knife;
    public AudioSource cut;
    public AudioSource swing;
    public AudioSource damage;

    void Update()
    {
        if (IsRunning())
        {
            PlayRun();
        }
        else if (IsWalking())
        {
            PlayWalk();
        }
        else
        {
            StopAll();
        }
    }

    bool IsWalking()
    {
        return Input.GetKey(KeyCode.W) ||
               Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) ||
               Input.GetKey(KeyCode.D);
    }

    bool IsRunning()
    {
        return IsWalking() && Input.GetKey(KeyCode.LeftShift);
    }

    void PlayWalk()
    {
        if (run.isPlaying) run.Stop();
        if (!walk.isPlaying) walk.Play();
    }

    void PlayRun()
    {
        if (walk.isPlaying) walk.Stop();
        if (!run.isPlaying) run.Play();
    }

    void StopAll()
    {
        walk.Stop();
        run.Stop();
    }

    public void ThrowKnife(){
        knife.Play();
    }

    public void Cut(){
        cut.Play();
    }

    public void Damage(){
        damage.Play();
    }

    public void Swing(){
        swing.Play();
    }
}
