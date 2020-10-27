using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public SongGenerator songGenerator;

    public Light light;
    public AudioSource audioSource;
    public float currentTime;
    public float nextNoteAt;
    public int melodyIndex;
    
    public List<AudioClip> clipList = new List<AudioClip>();

    
    public void Play()
    {
        if (songGenerator.melodyList.Count == 0)
            return;

        Stop();
        StartCoroutine(PlaySong());
    }

    public void Stop()
    {
        melodyIndex = 0;
        currentTime = 0;
        nextNoteAt = 0;
        audioSource.Stop();
    }

    IEnumerator PlaySong()
    {
        while (true)
        {
            if (melodyIndex >= songGenerator.melodyList.Count)
            {
                break;
            }

            currentTime += Time.deltaTime*5;
            if (currentTime >= nextNoteAt)
            {
                melodyIndex++;
                var notation = songGenerator.melodyList[melodyIndex];

                light.transform.position = new Vector3(notation.gameObject.transform.position.x,
                    notation.gameObject.transform.position.y, light.transform.position.z);
                nextNoteAt += (int)notation.noteLenght;
                audioSource.Stop();
                var grado = songGenerator.gradoList[notation.pitch];
                var note = songGenerator.gradoList[notation.pitch].semitono + songGenerator.notaBase;
                if (note > 11)
                    note -= 11;
                audioSource.clip = clipList[note];
                audioSource.Play();
            }
            yield return null;
        }
    }


}
