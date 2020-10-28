using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public SongGenerator songGenerator;
    
    public List<NotePlayer> notePlayerList = new List<NotePlayer>();
    public float currentTime;
    public bool playingSong;
    
    public List<AudioClip> clipList = new List<AudioClip>();

    public List<Notation> notationList =new List<Notation>();

    public GameObject notePlayerPrefab;

    public void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var notePlayerObject = Instantiate(notePlayerPrefab);
            notePlayerObject.transform.SetParent(gameObject.transform);
            var notePlayer = notePlayerObject.GetComponent<NotePlayer>();
            notePlayerList.Add(notePlayer);
        }
        
    }

    public void Play()
    {
        if (songGenerator.melodyList.Count == 0)
            return;
        
        Stop();
        notationList.AddRange(songGenerator.melodyList);
        notationList.AddRange(songGenerator.chordList);
        playingSong = true;
    }

    public void Stop()
    {
        foreach (var notation in notationList)
        {
            notation.playedNote = false;
        }
        notationList.Clear();
        currentTime = 0;
        foreach (var notePlayer in notePlayerList)
        {
            StopSound(notePlayer);
        }
        playingSong = false;
    }

    void Update()
    {
        if (!playingSong)
            return;
        
        currentTime += Time.deltaTime*5;
        foreach (var notation in notationList)
        {
            if(notation.playedNote)
                continue;
            if (notation.time > currentTime)
                continue;
            
            notation.playedNote = true;
            PlaySound((int)notation.noteLenght, notation.exactNote, notation.gameObject.transform.position);
        }

        foreach (var notePlayer in notePlayerList)
        {
            if (!notePlayer.playingSound)
                continue;
            if (notePlayer.finishTime < currentTime)
            {
                StopSound(notePlayer);
            }

        }
    }

    void SetLightPosition(Light light, float x, float y)
    {
        light.gameObject.transform.position = new Vector3(x, y, light.transform.position.z);
    }

    void StopSound(NotePlayer notePlayer)
    {
        notePlayer.gameObject.name = "muted";
        notePlayer.audioSource.Stop();
        SetLightPosition(notePlayer.light, -10,-10);
        notePlayer.playingSound = false;
    }

    void PlaySound(float time, ExactNote note, Vector3 noteposition)
    {
        foreach (var notePlayer in notePlayerList)
        {
            if(notePlayer.playingSound)
                continue;
            notePlayer.audioSource.clip = clipList[(int)note];
            notePlayer.finishTime = time + currentTime;
            notePlayer.playingSound = true;
            notePlayer.gameObject.name = $"playing {note.ToString()}";
            notePlayer.audioSource.Play();
            SetLightPosition(notePlayer.light, noteposition.x, noteposition.y);
            return;
        }

    }

}

