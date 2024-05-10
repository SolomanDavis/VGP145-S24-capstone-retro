using UnityEngine;
using UnityEngine.Splines;

public class EnemyPathfinding : MonoBehaviour
{
    // Control value for the speed of the enemy along the path
    [SerializeField] float speedAlongPath;

    // Interpolation factor for the enemy's position along the path
    private float _interpFactor = 0.0f;

    // ENTRANCE PATHFINDING

    // Container containing splines that define predefined paths
    [SerializeField] SplineContainer SplineContainer;

    // Chosen path to follow
    private int _chosenPath = 0;

    // HOVER PATHFINDING
    public Transform HoverLocation { get; set; }

    public enum PathfindingState
    {
        Entrance,
        Hover,
        Dive,
    }
    [SerializeField] private PathfindingState _state = PathfindingState.Hover;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: ZA - Estelle - DEBUG - Replace with actual rank grid slots
        HoverLocation = new GameObject().transform;
        HoverLocation.position = Vector3.zero;
    }

    // ChoosePath sets the path index for the enemy to follow
    public void ChoosePath(int pathIndex)
    {
        _chosenPath = pathIndex;
    }

    // SetState changes the current state of this enemy's pathfinding
    public void SetState(PathfindingState state)
    {
        _state = state;

        // Reset interpolation factor between states to offer best transitions
        _interpFactor = 0;
    }

    // TODO: ZA - Estelle - DEBUG STATE CHANGER - REMOVE
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetState(PathfindingState.Entrance);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetState(PathfindingState.Hover);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetState(PathfindingState.Dive);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPosition = Vector3.zero;
        Vector3 newDirection = Vector3.up;

        switch (_state)
        {
            case PathfindingState.Entrance:
                CalculateEntrancePath(out newPosition, out newDirection);
                break;
            case PathfindingState.Hover:
                CalculateHoverPath(out newPosition, out newDirection);
                break;
            case PathfindingState.Dive:
                CalculateDivePath(out newPosition, out newDirection);
                break;
        }

        transform.position = newPosition;
        transform.up = newDirection;
    }

    private void CalculateEntrancePath(out Vector3 position, out Vector3 direction)   
    {
        // Calculate current interpolation factor along path
        CalculateInterpolationFactor();

        // Get the transformation matrix that will allow positioning a point
        // on the spline in world space
        Matrix4x4 splineTransformMatrix = SplineContainer.transform.localToWorldMatrix;

        Spline spline = SplineContainer.Splines[_chosenPath];

        // Find local position on spline and apply transformation matrix
        Vector3 localSplinePosition = spline.EvaluatePosition(_interpFactor);
        position = splineTransformMatrix.MultiplyPoint3x4(localSplinePosition);

        // Find normalized local direction via tangent off spline and apply transformation matrix
        Vector3 forwardDirection = spline.EvaluateTangent(_interpFactor);
        direction = splineTransformMatrix.MultiplyVector(forwardDirection).normalized;
    }

    private void CalculateHoverPath(out Vector3 position, out Vector3 direction)
    {
        // Calculate current interpolation factor along path
        CalculateInterpolationFactor();

        // Interpolate the current position to the hover location
        position = Vector3.Lerp(transform.position, HoverLocation.position, _interpFactor);

        // Determine if moving towards hover location or actually hovering
        if (Vector2.Distance(transform.position, HoverLocation.position) < 0.5f)
        {
            direction = Vector3.up;
        } else
        {
            direction = -Vector2.MoveTowards(transform.position, HoverLocation.position, 1);
        }
    }

    private void CalculateDivePath(out Vector3 position, out Vector3 direction)
    {
        position = Vector3.zero;
        direction = Vector3.zero;
    }

    // Calculate current interpolation factor according to time and speed
    private void CalculateInterpolationFactor()
    {
        _interpFactor += Time.deltaTime * speedAlongPath;
        _interpFactor = Mathf.Clamp01(_interpFactor);
    }
}
