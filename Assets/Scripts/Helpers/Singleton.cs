using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;
    void Awake()
    {
        // check if instance already exists
        if (instance == null)
        {
            // if not, set instance to this
            instance = this;
        }
        // if instance already exists and it's not this
        else if (instance != this)
        {
            // then destroy this, enforcing singleton pattern
            Destroy(gameObject);
        }
        // set this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
}
