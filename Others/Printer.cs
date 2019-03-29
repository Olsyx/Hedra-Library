using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour {
    public void Print(string str) {
        Debug.Log(str);
    }

    public void Say(string str) {
        Debug.Log(name + ": " + str);
    }
}
