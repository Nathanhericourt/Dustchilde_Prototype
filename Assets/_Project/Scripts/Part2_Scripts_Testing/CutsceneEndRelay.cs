using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneEndRelay : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private UnityEvent onCutsceneStopped;

    private bool fired;

    private void Awake()
    {
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }
    }

    private void OnEnable()
    {
        if (director != null)
        {
            director.stopped += HandleStopped;
        }
    }

    private void OnDisable()
    {
        if (director != null)
        {
            director.stopped -= HandleStopped;
        }
    }

    private void Update()
    {
        if (fired || director == null || director.state != PlayState.Playing)
        {
            return;
        }

        // Wrap Mode Hold never fires stopped; treat reaching the last frame as the end.
        if (director.duration > 0d && director.time >= director.duration - 0.05d)
        {
            Fire();
        }
    }

    private void HandleStopped(PlayableDirector _)
    {
        Fire();
    }

    private void Fire()
    {
        if (fired) return;

        fired = true;
        onCutsceneStopped?.Invoke();
    }
}
