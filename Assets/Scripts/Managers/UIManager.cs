using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private TMP_Text _orbsText;
    
    public void UpdateOrbs()
    {
        _orbsText.text = "Orbs: " + PlayerMovement.orbs;
    }
}
