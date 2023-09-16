
using System.Linq;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{

    private class BoneTransform
    {
        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }
    }

    private enum EnemyState
    {
        Idle, Ragdoll, StandingUp, ResettingBones
    }

    //[SerializeField]
    //private Camera _camera;

    [SerializeField]
    private string _standUpStateName;

    [SerializeField]
    private string _standUpClipName;

    [SerializeField]
    private float _timeToResetBones;

    private Rigidbody[] _ragdollRigidbodies;
    private EnemyState _currentState = EnemyState.Idle;
    private Animator _animator;  // Added Animator
    private AIController _AIController;
    private UnityEngine.AI.NavMeshAgent _navAgent;
    private float _timeToWakeUp;
    private Transform _hipsBone;

    private BoneTransform[] _standUpBoneTransforms;
    private BoneTransform[] _ragdollBoneTransforms;
    private Transform[] _bones;
    private float _elapsedResetBonesTime;

    // Start is called before the first frame update
    void Awake()
    {
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();  // Get the Animator component
    
        _AIController = GetComponent<AIController>();
        _navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _hipsBone = _animator.GetBoneTransform(HumanBodyBones.Hips);

        _bones = _hipsBone.GetComponentsInChildren<Transform>();
        _standUpBoneTransforms = new BoneTransform[_bones.Length];
        _ragdollBoneTransforms = new BoneTransform[_bones.Length];

        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
        {
            _standUpBoneTransforms[boneIndex] = new BoneTransform();
            _ragdollBoneTransforms[boneIndex] = new BoneTransform();
        }

        PopulateAnimationStartBoneTransforms(_standUpClipName, _standUpBoneTransforms);

        DisableRagdoll();
    }
    
    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
                IdleBehaviour();
                break;
            case EnemyState.Ragdoll:
                RagdollBehaviour();
                break;
            case EnemyState.StandingUp:
                StandingUpBehaviour();
                break;
            case EnemyState.ResettingBones:
                ResettingBonesBehaviour();
                break;
        }
    }
   
    private void DisableRagdoll()
    {
        
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        _animator.enabled = true;  // Disable Animator    
        _AIController.enabled = true;
        _navAgent.enabled = true;
        
    }

    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        EnableRagdoll();

        Rigidbody hitRigidbody = _ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();

        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

        _currentState = EnemyState.Ragdoll;
        _timeToWakeUp = Random.Range(1, 3);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
        // Check if the enemy collides with a GameObject that has the tag "Cube"
        if (collision.gameObject.tag == "KnockDownCube")
        {
            Vector3 force = new Vector3(0, 5, 10); // Arbitrary force
            Vector3 hitPoint = collision.contacts[0].point; // Point of contact
            TriggerRagdoll(force, hitPoint);
        }
    }

    private void EnableRagdoll()
    {
        
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }
       
        _animator.enabled = false;  // Disable Animator    
        _AIController.enabled = false;
        _navAgent.enabled = false;


    }

    private void IdleBehaviour()
    {
        //Vector3 direction = _camera.transform.position - transform.position;
        // direction.y = 0;
        //direction.Normalize();

        // Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 20 * Time.deltaTime);

        if (_navAgent.velocity.magnitude > 0) // Check if the enemy is actually moving
        {
            _animator.SetBool("IsMoving", true); // Set IsMoving to true
        }
        else
        {
            _animator.SetBool("IsMoving", false); // Set IsMoving to false
        }
       
    }

    private void RagdollBehaviour()
    {
        _timeToWakeUp -= Time.deltaTime;

        if (_timeToWakeUp <= 0)
        {
            AlignRotationToHips();
            AlignPositionToHips();

            PopulateBoneTransforms(_ragdollBoneTransforms);

            _currentState = EnemyState.ResettingBones;
            _elapsedResetBonesTime = 0;


        }
        
    }
    private void StandingUpBehaviour()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_standUpStateName) == false)
        {
            _currentState = EnemyState.Idle;
        }
    }

    private void ResettingBonesBehaviour()
    {
        _elapsedResetBonesTime += Time.deltaTime;
        float elapsedPercentage = _elapsedResetBonesTime / _timeToResetBones;

        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
        {
            // Debug statements for Vector3.Lerp
            Debug.Log("Vector Start: " + _ragdollBoneTransforms[boneIndex].Position);
            Debug.Log("Vector End: " + _standUpBoneTransforms[boneIndex].Position);
            Debug.Log("Vector Lerp Ratio/Scalar: " + elapsedPercentage);

            _bones[boneIndex].localPosition = Vector3.Lerp(
                _ragdollBoneTransforms[boneIndex].Position,
                _standUpBoneTransforms[boneIndex].Position,
                elapsedPercentage);

            // Debug statements for Quaternion.Lerp
            Debug.Log("Quaternion Start: " + _ragdollBoneTransforms[boneIndex].Rotation);
            Debug.Log("Quaternion End: " + _standUpBoneTransforms[boneIndex].Rotation);
            Debug.Log("Quaternion Lerp Ratio/Scalar: " + elapsedPercentage);

            _bones[boneIndex].localRotation = Quaternion.Lerp(
                _ragdollBoneTransforms[boneIndex].Rotation,
                _standUpBoneTransforms[boneIndex].Rotation,
                elapsedPercentage);
        }

        if (elapsedPercentage >= 1)
        {
            _currentState = EnemyState.StandingUp;
            DisableRagdoll();

            _animator.Play(_standUpStateName);
        }
    }

    private void AlignRotationToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        Quaternion originalHipsRotation = _hipsBone.rotation;

        Vector3 desiredDirection = _hipsBone.up * -1;
        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, desiredDirection);
        transform.rotation *= fromToRotation;

        _hipsBone.position = originalHipsPosition;
        _hipsBone.rotation = originalHipsRotation;
    }

    private void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        transform.position = _hipsBone.position;

        Vector3 positionOffset = _standUpBoneTransforms[0].Position;
        positionOffset.y = 0;
        positionOffset = transform.rotation * positionOffset;
        transform.position -= positionOffset;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }

        _hipsBone.position = originalHipsPosition;
    }

    private void PopulateBoneTransforms(BoneTransform[] boneTransforms)
    {
        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
        {
            boneTransforms[boneIndex].Position = _bones[boneIndex].localPosition;
            boneTransforms[boneIndex].Rotation = _bones[boneIndex].localRotation;
        }
    }

    private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms)
    {
        Vector3 positionBeforeSampling = transform.position;
        Quaternion rotationBeforeSampling = transform.rotation;

        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                clip.SampleAnimation(gameObject, 0);
                PopulateBoneTransforms(_standUpBoneTransforms);
                break;
            }
        }

        transform.position = positionBeforeSampling;
        transform.rotation = rotationBeforeSampling;
    }
}
