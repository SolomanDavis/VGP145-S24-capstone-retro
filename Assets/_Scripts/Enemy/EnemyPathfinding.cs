using System.Linq;
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
    private SplineContainer _splineContainer;

    // Chosen path to follow
    private int _chosenPath = 0;

    // HOVER PATHFINDING
    public Transform HoverLocation { get; set; }

    private bool _isPaused = false;

    public enum PathfindingState
    {
        Entrance,
        Hover,
        Dive,
    }
    [SerializeField] private PathfindingState _state = PathfindingState.Entrance; // Starting state: Entrance

    public PathfindingState State
    {
        get { return _state; }       
    }

    private void Awake()
    {
        CanvasManager.Instance.GamePaused += () => _isPaused = true;
        CanvasManager.Instance.GameUnpaused += () => _isPaused = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_splineContainer == null)
        {
            _splineContainer = FindObjectOfType<SplineContainer>();
        }

        _chosenPath = Random.Range(0, _splineContainer.Splines.Count());
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
        if (_isPaused)
            return;

        // Detect Entrance -> Hover state change
        if (_state == PathfindingState.Entrance)
        {
            BezierKnot endpoint = _splineContainer.Splines[_chosenPath].Last();
            if (Vector2.Distance(transform.position, SplineLocalToWorldPoint(endpoint.Position)) < 0.5f)
            {
                SetState(PathfindingState.Hover);
            }
        }

        // TODO: ZA - Detect Hover -> Dive state change
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isPaused)
            return;

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

    // CalculateEntrancePath calculates the next position and direction for the enemy in an Entrance state
    // The enemy will move along the chosen path until it reaches the end of the path
    private void CalculateEntrancePath(out Vector3 position, out Vector3 direction)   
    {
        // Calculate current interpolation factor along path
        CalculateInterpolationFactor();

        Spline spline = _splineContainer.Splines[_chosenPath];

        // Find local position on spline and apply transformation matrix
        Vector3 localSplinePosition = spline.EvaluatePosition(_interpFactor);
        position = SplineLocalToWorldPoint(localSplinePosition);

        // Find normalized local direction via tangent off spline and apply transformation matrix
        Vector3 forwardDirection = spline.EvaluateTangent(_interpFactor);
        direction = SplineLocalToWorldVector(forwardDirection).normalized;
    }

    private Vector3 SplineLocalToWorldPoint(Vector3 localPoint)
    {
        // Get the transformation matrix that will allow positioning a point
        // on the spline in world space
        Matrix4x4 splineTransformMatrix = _splineContainer.transform.localToWorldMatrix;
        return splineTransformMatrix.MultiplyPoint3x4(localPoint);
    }

    private Vector3 SplineLocalToWorldVector(Vector3 localVector)
    {
        // Get the transformation matrix that will allow positioning a point
        // on the spline in world space
        Matrix4x4 splineTransformMatrix = _splineContainer.transform.localToWorldMatrix;
        return splineTransformMatrix.MultiplyVector(localVector);
    }

    // CalculateHoverPath calculates the next position and direction for the enemy in a Hover state
    // The enemy will move towards the hover location and then hover in place, sticking to the assigned hover location
    private void CalculateHoverPath(out Vector3 position, out Vector3 direction)
    {
        if (HoverLocation == null)
        {
            Debug.LogError("No hover location assigned to enemy pathfinding script by enemy manager");
            position = Vector3.zero;
            direction = Vector3.up;
            return;
        }

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
            direction = Vector2.MoveTowards(transform.position, HoverLocation.position, 0.1f);
        }
    }

    // CalculateDivePath calculates the next position and direction for the enemy in a Dive state
    // TODO: to be implemented
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
