using UnityEngine;

public class PreassurePlate : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Movable"))
        {
            var distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance <= 0.05)
            {
                if (other.GetComponent<Rigidbody>())
                    other.GetComponent<Rigidbody>().isKinematic = true;
                if (_renderer is not null)
                    _renderer.material.color = Color.blue;
            }
        }
    }
}
