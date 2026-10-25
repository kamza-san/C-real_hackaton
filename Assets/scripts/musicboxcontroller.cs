using UnityEngine;

[RequireComponent(typeof(ItemTable))]
public class MusicBox : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private ItemTable table;

    private void Awake()
    {
        table = GetComponent<ItemTable>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        table.OnInteracted += StopMusic;
    }

    private void OnDisable()
    {
        table.OnInteracted -= StopMusic;
    }

    private void StopMusic()
    {
        if (LoopManager.IsDone(gameObject.name) && audioSource != null)
        {
            audioSource.Stop();
        }
        
    }
}