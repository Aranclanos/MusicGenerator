using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notation : MonoBehaviour
{
    public int pitch;
    public NoteLenght noteLenght;
    public int time;
    public GameObject noteCube;
    public SpriteRenderer spriteRenderer;
    public ExactNote exactNote;

    public bool playedNote;
}

public enum ExactNote
{
    //4
    C4,
    Cm4,
    D4,
    Dm4,
    E4,
    F4,
    Fm4,
    G4,
    Gm4,
    A4,
    Am4,
    B4,
    
    //5
    C5,
    Cm5,
    D5,
    Dm5,
    E5,
    F5,
    Fm5,
    G5,
    Gm5,
    A5,
    Am5,
    B5,
    
    //6
    C6,
    Cm6,
    D6,
    Dm6,
    E6,
    F6,
    Fm6,
    G6,
    Gm6,
    A6,
    Am6,
    B6,
}
