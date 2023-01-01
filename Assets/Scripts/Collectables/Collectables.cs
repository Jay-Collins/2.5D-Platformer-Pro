using System;
using UnityEngine;


public class Collectables : MonoBehaviour
{
    private enum collectables {orb, health}
    [SerializeField] private collectables _collectableType;

    public static Action<int> orbCollected;

    [SerializeField] private int _orbValue;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        switch (_collectableType)
        {
            case collectables.orb:
                orbCollected(_orbValue);
                UIManager.instance.UpdateOrbs();
                gameObject.SetActive(false);
                break;
            case collectables.health:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
