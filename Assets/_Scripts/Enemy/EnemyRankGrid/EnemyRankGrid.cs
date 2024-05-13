using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRankGrid : MonoBehaviour
{
    [SerializeField] private EnemyRankGridSlot _gridSlotPrefab;

    private float _spacing = 2.0f;

    public enum EnemyRank
    {
        Light,
        Medium,
        Heavy,
    }

    // 40 slots for the grid - 2x10 Light, 1x10 Medium, 1x6 Heavy
    // Rows will always be 10 slots maximum. If there are less than 10 slots, the remaining slots will be null.
    // Null slots will be placed evenly in the grid at either end of the row.
    [SerializeField] private int _lightSlots = 20;
    [SerializeField] private int _mediumSlots = 10;
    [SerializeField] private int _heavySlots = 6;
    [SerializeField] private float _maxSlotsPerRow = 10.0f;

    private EnemyRankGridSlot[,] _gridSlots;

    // Start is called before the first frame update
    void Start()
    {
        int[] gridSlotTypes = new int[] { _lightSlots, _mediumSlots, _heavySlots };

        // Calculate the number of rows needed for the grid rounded up to the nearest multiple of 10
        int totalRows = (int) Math.Ceiling((_lightSlots + _mediumSlots + _heavySlots) / 10.0f);
        _gridSlots = new EnemyRankGridSlot[totalRows, (int) _maxSlotsPerRow];

        int slotIndex = 0;

        for (int i = 0; i < gridSlotTypes.Length; ++i)
        {
            int numSlotsInRow = gridSlotTypes[i];
            int numRowsPerSlotType = (int) Math.Ceiling(numSlotsInRow / _maxSlotsPerRow);

            Debug.Log("ZA - numSlotsInRow: " + numSlotsInRow + ", numRowsPerSlotType: " + numRowsPerSlotType);

            for (int row = 0; i < numRowsPerSlotType; ++row)
            {
                Debug.Log("ZA - row: " + row);

                for (int column = 0; column < _maxSlotsPerRow; ++column)
                {
                    Debug.Log("ZA - slotIndex: " + slotIndex + ", row: " + row + ", column: " + column);

                    _gridSlots[row, column] = InstantiateGridSlot(slotIndex, row, column);

                    if (_gridSlots[row, column] != null)
                        Debug.Log("ZA - _gridSlots[slotIndex]: " + _gridSlots[row, column].name);

                    ++slotIndex;
                }
            }
        }
    }

    private EnemyRankGridSlot InstantiateGridSlot(int index, int row, int column)
    {
        if (index < 0 || index >= (_lightSlots + _mediumSlots + _heavySlots)) return null;

        Vector2 position = new Vector2(column * _spacing, row * _spacing);
        EnemyRankGridSlot gridSlot = Instantiate(_gridSlotPrefab, position, Quaternion.identity, transform);
        gridSlot.name = "GridSlot" + index;

        return gridSlot;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
