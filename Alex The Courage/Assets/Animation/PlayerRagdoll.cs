using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private Rigidbody[] _ragdollRigidbodies;
    private Animator _animator;
    private CharacterController _characterController;
    private PlayerMovement _playerMovement;  // Assuming your player movement script is called PlayerMovement
    private AudioSource _audioSource;
    public bool IsRagdollEnabled { get; private set; } = false;

    // Start is called before the first frame update
    void Awake()
    {
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _playerMovement = GetComponent<PlayerMovement>();  // Assuming your player movement script is called PlayerMovement
        _audioSource = GetComponent<AudioSource>();

        foreach (var rb in _ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
        _animator.enabled = true;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Collision detected with enemy.");
            EnableRagdoll();
        }
    }

    public void EnableRagdoll()
    {
        _animator.enabled = false;
        _characterController.enabled = false;
        _playerMovement.enabled = false;  // Disable player movement
        _audioSource.enabled = false;  // Disable audio source

        foreach (var rb in _ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }
        IsRagdollEnabled = true;
        Debug.Log("Ragdoll Enabled");
    }

    public void RefreshRagdoll()
    {
        if (IsRagdollEnabled)
        {
            // Re-enable animator and character controller
            _animator.enabled = true;
            _characterController.enabled = true;
            _playerMovement.enabled = true;  // Re-enable player movement script
            _audioSource.enabled = true;  // Re-enable audio source if you want

            // Make all ragdoll Rigidbodies kinematic again
            foreach (var rb in _ragdollRigidbodies)
            {
                rb.isKinematic = true;
            }

            IsRagdollEnabled = false;
            Debug.Log("Ragdoll Refreshed");
        }
    }
}
