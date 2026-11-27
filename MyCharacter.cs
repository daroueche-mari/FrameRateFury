using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class MyCharacter
{
    public string Name {  get; set; }
    public int Score { get; set; }


    public MyCharacter(string name, int score) {  Name = name; Score = score; }



    public static List<MyCharacter> mylistChar = new List<MyCharacter>
    {
        new MyCharacter("PlayerOne", 0),
        new MyCharacter("PlayerTwo", 0)
    };




}
