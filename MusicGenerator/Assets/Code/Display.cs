using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    public SongGenerator songGenerator;

    public Text tempoText;
    public Text escalaText;
    public Text notaBaseText;
    
    public List<GameObject> gradoMarkers = new List<GameObject>();
    
    public List<GameObject> greenNoteList = new List<GameObject>();
    public GameObject greenNotePrefab;
    public int greenNoteIndex = 0;
    
    public List<GameObject> purpleNoteList = new List<GameObject>();
    public GameObject purpleNotePrefab;
    public int purpleNoteIndex = 0;

    public List<string> noteNamesList = new List<string>(){"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};
    
    public List<Sprite> spriteList = new List<Sprite>();
    
    public void RunDisplay()
    {
        HideNotes();
        greenNoteIndex = 0;
        purpleNoteIndex = 0;
        tempoText.text = $"Tempo: {songGenerator.tempo}";
        escalaText.text = $"Escala: {songGenerator.escala.name}";
        notaBaseText.text = $"Nota base: {noteNamesList[songGenerator.notaBase]}";
        
        for (var i = 0; i < songGenerator.chordList.Count; i++) // green chords
        {
            var notation = songGenerator.chordList[i];
            notation.spriteRenderer.sprite = spriteList[notation.pitch];
            var greenNote = notation.gameObject;
            float yPosition = songGenerator.gradoList[notation.pitch].semitono + songGenerator.notaBase;
            greenNote.transform.position = new Vector3(notation.time/2, yPosition/2,-2);
            notation.noteCube.transform.localScale = new Vector3((float)notation.noteLenght/2, notation.noteCube.transform.localScale.y, 1);
            greenNote.name = $"chord note:  pitch: {notation.pitch} --- length: {(int)notation.noteLenght}";
        }
        
        for (var i = 0; i < songGenerator.melodyList.Count; i++)// purple melody
        {
            var notation = songGenerator.melodyList[i];
            var purpleNote = notation.gameObject;
            float yPosition = songGenerator.gradoList[notation.pitch].semitono + songGenerator.notaBase;
            purpleNote.transform.position = new Vector3(notation.time/2, yPosition/2 + 6 ,-2);
            notation.noteCube.transform.localScale = new Vector3((float)notation.noteLenght/2, notation.noteCube.transform.localScale.y, 1);
            purpleNote.name = $"melody note:  pitch: {notation.pitch} --- length: {(int)notation.noteLenght}";
        }

        for (var i = 0; i < songGenerator.gradoList.Count; i++) // cyan tonalidad
        {
            float yPosition = songGenerator.gradoList[i].semitono + songGenerator.notaBase;
            var tonalidadMark = gradoMarkers[i].transform.position;
            gradoMarkers[i].transform.position = new Vector3(tonalidadMark.x, yPosition/2, -1);
        }
    }
    
    public GameObject GetPurpleNote()
    {
        var purpleNote = GetNote(purpleNoteIndex, purpleNoteList, purpleNotePrefab);
        purpleNoteIndex++;
        return purpleNote;
    }
    
    public GameObject GetGreenNote()
    {
        var greenNote = GetNote(greenNoteIndex, greenNoteList, greenNotePrefab);
        greenNoteIndex++;
        return greenNote;
    }

    public GameObject GetNote(int index, List<GameObject> noteList, GameObject prefab)
    {
        GameObject note;
        if (noteList.Count > index)
        {
            note = noteList[index];
        }
        else
        {
            note = Instantiate(prefab);
            noteList.Add(note);
        }
        
        return note;
    }

    void HideNotes()
    {
        for (var i = 0; i < greenNoteList.Count; i++)
        {
            greenNoteList[i].transform.position = new Vector3(0, 0, 1);
        }
        for (var i = 0; i < purpleNoteList.Count; i++)
        {
            purpleNoteList[i].transform.position = new Vector3(0, 0, 1);
        }
    }



}
