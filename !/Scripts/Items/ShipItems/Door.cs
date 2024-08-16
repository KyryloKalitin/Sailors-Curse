using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : UntakeableItem
{
    public override void Interact()
    {
        SceneManager.LoadScene("IslandScene");
    }
}
