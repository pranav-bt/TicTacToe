using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int Width;
    [SerializeField] public int Length;
    [SerializeField] private Tile Tiles;
    void Start()
    {
        for(int x = 0; x<Width; x++)
        {
            for(int y= 0; y<Length; y++)
            {
                var TileObject = Instantiate(Tiles, new Vector3(x,y), Quaternion.identity) as Tile;
                TileObject.name = $"Tile{y}{x}";

                if((x%2 == 0 && y%2 == 0) || (x%2!=0 && y%2!=0))
                {
                    TileObject.IsAlternate = false;
                }

                TileObject.SetColor();
                TileObject.Row = y;
                TileObject.Column = x;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
