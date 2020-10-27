using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SongGenerator : MonoBehaviour
{

    //Start() setting:
    public List<Grado> gradoList = new List<Grado>();
    public List<Escala> escalaList = new List<Escala>();
    
    //Song:
    public Escala escala;
    public int tempo;
    public int notaBase = 0;
    public List<int> acordePrimeraList = new List<int>();
    public List<int> acordeTerceraList = new List<int>();
    public List<int> acordeQuintaList = new List<int>();
    public List<int> acordeSeptimaList = new List<int>();
    public int tonicTension = 0;
    public int medianteTension = 0;
    public int antiTension = 0;

    public Display display;
    public MusicPlayer musicPlayer;
    
    public List<Notation> melodyList = new List<Notation>();
    public List<Notation> chordList = new List<Notation>();
    
    private void Start()
    {
        for (var i = 0; i < 7; i++)
        {
            var grado = new Grado();
            gradoList.Add(grado);
        }

        gradoList[0].gradoName = GradoName.tonica;          // "I"     // Tonica
        gradoList[1].gradoName = GradoName.supertonica;     // "II";   // Supertonica
        gradoList[2].gradoName = GradoName.mediante;        // "III";  // Mediante
        gradoList[3].gradoName = GradoName.subdominante;    // "IV";   // Subdominante
        gradoList[4].gradoName = GradoName.dominante;       // "V";    // Dominante
        gradoList[5].gradoName = GradoName.submediante;     // "VI";   // Submediante
        gradoList[6].gradoName = GradoName.sensible;        // "VII";  // Sensible
        //gradoList[7].gradoName = GradoName.tonicaplus;          // "I+";
        
        for (var i = 0; i < 7; i++)
        {
            var escala = new Escala();
            escalaList.Add(escala);
        }

        escalaList[0].name = "jonico";
        escalaList[0].semiTonos = new List<int>(){0,2,2,1,2,2,2,1}; //1
        escalaList[1].name = "dorico";
        escalaList[1].semiTonos = new List<int>(){0,2,1,2,2,2,1,2};
        escalaList[2].name = "frigio";
        escalaList[2].semiTonos = new List<int>(){0,1,2,2,2,1,2,2};
        escalaList[3].name = "lidio";
        escalaList[3].semiTonos = new List<int>(){0,2,2,2,1,2,2,1};
        escalaList[4].name = "mixolidio";
        escalaList[4].semiTonos = new List<int>(){0,2,2,1,2,2,1,2};
        escalaList[5].name = "eolico";
        escalaList[5].semiTonos = new List<int>(){0,2,1,2,2,1,2,2};
        escalaList[6].name = "locrio";
        escalaList[6].semiTonos = new List<int>(){0,1,2,2,1,2,2,2};


    }

    public void RunProgram()
    {
        musicPlayer.Stop();
        acordePrimeraList.Clear();
        acordeTerceraList.Clear();
        acordeQuintaList.Clear();
        acordeSeptimaList.Clear();
        melodyList.Clear();
        chordList.Clear();
        
        tempo = Random.Range(50, 200);
        PickScale();
        GenerateChords();
        GenerateMelody();
        display.RunDisplay();
    }

    private void PickScale()
    {
        //var scaleArray = Enum.GetValues(typeof(scaleType));
        notaBase = Random.Range(0, 12);
        escala = escalaList[Random.Range(0, escalaList.Count)];
        int semitonoIndex = 0;
        for (int i = 0; i < gradoList.Count; i++)
        {
            semitonoIndex += escala.semiTonos[i];
            gradoList[i].semitono = semitonoIndex;
        }
        
        SetGradoTension(1, false, true);    // II Supertonica
        SetGradoTension(3, false, false);    // IV Subdominante
        SetGradoTension(6, true, true);     // VII Sensible

    }

    private void SetGradoTension(int index, bool leadingTone, bool isTonicTension)
    {
        var grado = gradoList[index];
        if (leadingTone)
        {
            index++; //using the semitone needed to go higher pitch to the tonic
        }
        var tension = 25;
        if (escala.semiTonos[index] == 1)
        {
            tension = 50;
        }

        if (isTonicTension)
        {
            grado.medianteTension = tension;
        }
        else
        {
            grado.medianteTension = tension;
        }
    }
    

    void GenerateChords()
    {
        tonicTension = 0;
        medianteTension = 0;
        antiTension = 0;
        int time = 0;
        for (var i = 0; i < 8; i++)
        {
            if (i == 7)
                tonicTension = 200;
            var includeSeptima = Random.Range(0, 100) > 75;
            
            var primera = ReturnPrimera(includeSeptima);
            CreateNotationChord(primera, NoteLenght.half, time);
            var tercera = TwoGradosAbove(primera);
            CreateNotationChord(tercera, NoteLenght.half, time);
            var quinta = TwoGradosAbove(tercera);
            CreateNotationChord(quinta, NoteLenght.half, time);
            var septima = TwoGradosAbove(quinta);
            
            acordePrimeraList.Add(primera); //TODO: remove
            acordeTerceraList.Add(tercera); //TODO: remove
            acordeQuintaList.Add(quinta); //TODO: remove
           
            if (includeSeptima)
            {
                medianteTension += gradoList[septima].medianteTension;
                tonicTension += gradoList[septima].tonicTension;
                acordeSeptimaList.Add(septima); //TODO: remove
                CreateNotationChord(septima, NoteLenght.half, time);
            }
            else
            {
                acordeSeptimaList.Add(-1);
            }

            time += 8;

            medianteTension += gradoList[primera].medianteTension + gradoList[tercera].medianteTension + gradoList[quinta].medianteTension;
            tonicTension += gradoList[primera].tonicTension + gradoList[tercera].tonicTension + gradoList[quinta].tonicTension;
            
            if (ContainsGrado(primera, tercera, quinta, septima, includeSeptima, GradoName.mediante))
                medianteTension = 0;
            if (ContainsGrado(primera, tercera, quinta, septima, includeSeptima, GradoName.tonica))
                tonicTension = 0;
        }
    }
    


    int ReturnPrimera(bool includeSeptima)
    {
        var includeTonic = Random.Range(0, 100) < tonicTension;
        var includeMediante = Random.Range(0, 100) < medianteTension;

        var primera = Random.Range(0, gradoList.Count);
        var tercera = TwoGradosAbove(primera);
        var quinta = TwoGradosAbove(tercera);
        var septima = TwoGradosAbove(quinta);
        if (!includeSeptima)
            septima = -1;
        for (int i = 0; i < 1000; i++) //TODO instead of randomly picking and checking if it fits, start a chord using the notes you need
        {
            if(i == 999)
                Debug.LogError("ReturnPrimera loop reached 999 iterations");
            
            tercera = TwoGradosAbove(primera);
            quinta = TwoGradosAbove(tercera);
            septima = TwoGradosAbove(quinta);
            if (!includeSeptima)
                septima = -1;
            if (includeTonic)
            {
                if (!ContainsGrado(primera, tercera, quinta, septima, includeSeptima, GradoName.tonica))
                {
                    primera = Random.Range(0, gradoList.Count);
                    continue;
                }
            }
            if (includeMediante)
            {
                if (!ContainsGrado(primera, tercera, quinta, septima, includeSeptima, GradoName.mediante))
                {
                    primera = Random.Range(0, gradoList.Count);
                    continue;
                }
            }
            break;
        }
        //Debug.Log($"Creating chord --- includeTonic: {includeTonic.ToString()} tonicTension: {tonicTension} --- includeMediante: {includeMediante.ToString()} medianteTension: {medianteTension} --- chords {primera}, {tercera}, {quinta}, {septima}");
        return primera;
    }

    bool ContainsGrado(int primera, int tercera, int quinta, int septima, bool includeSeptima,  GradoName gradoName)
    {
        if (gradoList[primera].gradoName == gradoName)
            return true;
        if (gradoList[tercera].gradoName == gradoName)
            return true;
        if (gradoList[quinta].gradoName == gradoName)
            return true;
        if (includeSeptima && gradoList[septima].gradoName == gradoName)
            return true;
        return false;
    }

    int TwoGradosAbove(int index)
    {
        index += 2;
        if (index > gradoList.Count-1)
        {
            index -= gradoList.Count;
        }
        return index;
    }


    void GenerateMelody()
    {
        var currentTime = 0;
        for (int i = 0; i < acordePrimeraList.Count; i++)
        {
            int chordNotes = 3;
            if (acordeSeptimaList[i] != -1)
                chordNotes++;
            var startTone = Random.Range(0, chordNotes);
            int startPitch = 0;
            if (startTone == 0)
                startPitch = acordePrimeraList[i];
            else if (startTone == 1)
                startPitch = acordeTerceraList[i];
            else if (startTone == 2)
                startPitch = acordeQuintaList[i];
            else if (startTone == 3)
                startPitch = acordeSeptimaList[i];

            var startLenght = CreateNoteLenght((int) NoteLenght.half);
            CreateNotation(startPitch, startLenght, currentTime);
            currentTime += (int) startLenght;
            
            NoteLenght noteLenght;
            for (var j = (int)NoteLenght.half - (int)startLenght; j > 0; j -= (int)noteLenght)
            {
                var pitch = Random.Range(0, gradoList.Count);
                noteLenght = CreateNoteLenght(j);
                CreateNotation(pitch, noteLenght, currentTime);
                currentTime += (int) noteLenght;
            }
        }
    }

    void CreateNotation(int pitch, NoteLenght noteLenght, int time)
    {
        var noteObject = display.GetPurpleNote();
        var notation = noteObject.GetComponent<Notation>();
        notation.pitch = pitch;
        notation.noteLenght = noteLenght;
        notation.time = time;
        melodyList.Add(notation);
    }
    
    void CreateNotationChord(int pitch, NoteLenght noteLenght, int time) //TODO: REMOVE AND MERGE WITH ABOVE
    {
        var noteObject = display.GetGreenNote();
        var notation = noteObject.GetComponent<Notation>();
        notation.pitch = pitch;
        notation.noteLenght = noteLenght;
        notation.time = time;
        chordList.Add(notation);
    }
    NoteLenght CreateNoteLenght(int remainingSpace)
    {
        if (remainingSpace == (int)NoteLenght.sixteenth)
        {
            return NoteLenght.sixteenth;
        }
        
        if (Random.Range(0, 100) < 20)
        {
            if (remainingSpace == (int) NoteLenght.half)
                remainingSpace = (int)NoteLenght.quarterHalfOne;
            
            var value = Random.Range((int)NoteLenght.eightHalf, remainingSpace-1);

            if (value == 3)
                return NoteLenght.eightHalf;
            else if(value == 4)
                return NoteLenght.quarter;
            else if(value == 5)
                return NoteLenght.quarterOne;
            else if(value == 6)
                return NoteLenght.quarterHalf;
            else if(value == 7)
                return NoteLenght.quarterHalfOne;
        }
        
        return NoteLenght.eight;
    }
    
    

}


public class Escala
{
    public string name;
    public List<int> semiTonos = new List<int>();
}

public class Grado
{
    public int semitono;
    public string name;
    public int tonicTension = 0;
    public int medianteTension = 0;
    public GradoName gradoName;
}


public enum GradoName
{
    tonica,
    supertonica,
    mediante,
    subdominante,
    dominante,
    submediante,
    sensible,
    tonicaplus
}

public enum NoteLenght
{
    sixteenth = 1,
    eight = 2,
    eightHalf =3,
    quarter = 4,
    quarterOne = 5,
    quarterHalf = 6,
    quarterHalfOne = 7,
    half = 8,
    whole = 16
}
