using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRankGrid : MonoBehaviour
{
    [SerializeField] private EnemyRankGridSlot _gridSlotPrefab;
    [SerializeField] private float _gridSpacing = 1.0f;

    // 40 slots for the grid - 2x10 Light, 1x10 Medium, 1x6 Heavy
    // Rows will always be 10 slots maximum. If there are less than 10 slots, the remaining slots will be null.
    // Null slots will be placed evenly in the grid at either end of the row.
    [SerializeField] private int _lightSlots = 20;
    [SerializeField] private int _mediumSlots = 10;
    [SerializeField] private int _heavySlots = 6;
    [SerializeField] private float _maxSlotsPerRow = 10.0f;

    private int[] _gridSlotTypes;
    private EnemyRankGridSlot[,] _gridSlots;

    private bool _isPaused = false;

    private void Awake()
    {
        CanvasManager.Instance.GamePaused += () => _isPaused = true;
        CanvasManager.Instance.GameUnpaused += () => _isPaused = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_gridSlotPrefab == null)
        {
            Debug.LogError("EnemyRankGrid: _gridSlotPrefab is null");
            return;
        }

        // Initialize numbers of slots for each rank used to build rank grid
        _gridSlotTypes = new int[] { _lightSlots, _mediumSlots, _heavySlots };

        // Calculate and initialize to the number of rows needed for the grid rounded up to the nearest multiple of 10
        int totalRows = (int) Math.Ceiling((_lightSlots + _mediumSlots + _heavySlots) / 10.0f);
        _gridSlots = new EnemyRankGridSlot[totalRows, (int)_maxSlotsPerRow];

        BuildRankGrid();
    }

    // BuildRankGrid builds the rank grid based on the number of slots for each rank
    private void BuildRankGrid()
    {
        int slotIndex = 0; // Track the index of each slot
        int rowOffset = 0; // Track the row offset within each rank type

        // Loop through each rank type
        for (int i = 0; i < _gridSlotTypes.Length; ++i)
        {
            int numSlotsForType = _gridSlotTypes[i]; // Number of slots for current rank type
            int numRowsPerSlotType = (int) Math.Ceiling(numSlotsForType / _maxSlotsPerRow); // Number of rows needed for current rank type

            // Loop through each row for the current rank type
            for (int row = 0; row < numRowsPerSlotType; ++row)
            {
                // Offset the starting column if there are not enough slots to fill the row to center the slots
                int colOffset = (_maxSlotsPerRow > numSlotsForType) ? (int) Math.Floor((_maxSlotsPerRow - numSlotsForType) / 2.0f) : 0;

                // Loop through each column for the current row
                for (int column = colOffset; column < _maxSlotsPerRow; ++column)
                {
                    // Instantiate
                    _gridSlots[rowOffset, column] = InstantiateGridSlot(slotIndex, rowOffset, column);
                    ++slotIndex;
                }
                ++rowOffset;
            }
        }
    }

    // InstantiateGridSlot instantiates a new grid slot at the specified row and column of the grid
    private EnemyRankGridSlot InstantiateGridSlot(int index, int row, int column)
    {
        if (index < 0 || index >= (_lightSlots + _mediumSlots + _heavySlots)) return null;

        Vector2 position = new Vector2(column * _gridSpacing, row * _gridSpacing);

        EnemyRankGridSlot gridSlot = Instantiate(_gridSlotPrefab, transform, false);

        gridSlot.transform.localPosition = position;
        gridSlot.transform.rotation = Quaternion.identity;
        gridSlot.name = "GridSlot" + index;

        return gridSlot;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPaused)
            return;

        // TODO: ZA - strafing effect
        // TODO: ZA - breathing effect
    }

    public EnemyRankGridSlot GetUnassignedGridSlot(EnemyRank enemyRank)
    {
        EnemyRankGridSlot gridSlot = null;

        switch (enemyRank)
        {
            case EnemyRank.Light:
                gridSlot = FindUnassignedGridSlot(0, _lightSlots);
                break;
            case EnemyRank.Medium:
                gridSlot = FindUnassignedGridSlot(_lightSlots, _lightSlots + _mediumSlots);
                break;
            case EnemyRank.Heavy:
                gridSlot = FindUnassignedGridSlot(_lightSlots + _mediumSlots, _lightSlots + _mediumSlots + _heavySlots);
                break;
        }

        if (gridSlot != null)
        {
            return gridSlot;
        }

        return null;
    }

    private EnemyRankGridSlot FindUnassignedGridSlot(int startIndex, int endIndex)
    {
        List<int> emptyGridSlots = new List<int>();

        int rowEnd = (endIndex % _maxSlotsPerRow == 0)
            ? endIndex
            : ((int) Math.Ceiling(endIndex / _maxSlotsPerRow) * (int)_maxSlotsPerRow);

        for (int i = startIndex; i < rowEnd; ++i)
        {
            EnemyRankGridSlot gridSlot = GetGridSlot(i);
            if (gridSlot != null && gridSlot.enemy == null)
            {
                emptyGridSlots.Add(i);
            }
        }

        if (emptyGridSlots.Count == 0)
        {
            Debug.LogWarning("No empty grid slots found for rank: " + startIndex + " to " + endIndex);
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, emptyGridSlots.Count);
        return GetGridSlot(emptyGridSlots[randomIndex]);
    }

    private EnemyRankGridSlot GetGridSlot(int slotIndex)
    {
        int row = slotIndex / (int)_maxSlotsPerRow;
        int col = slotIndex % (int)_maxSlotsPerRow;

        return _gridSlots[row, col];
    }
}
