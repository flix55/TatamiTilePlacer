using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TilePimperSystem : MonoBehaviour
{
    public TileScriptableObject[] _tileScriptableObject; // make a array of scriptable object
    private IndividualTile[] _allTheTile;
    public Boo.Lang.List<IndividualTile> listOfBoarderTiles = new Boo.Lang.List<IndividualTile>();
    
    // fuck, it's trash
    private float _miroireValuesTileZ;
    private float _miroireValuesTileY;
    
    private Vector3 _miroireVector3Tile;
    private bool _IsMiroirTile;
    
    private int[,] matrixTileSkinBridge = new int[3,3]{ 
        {1, 0, 1}, 
        {0, 0, 0}, 
        {1, 0, 1} 
    };
    
    private int[,] matrixTest = new int[3,3]{ 
        {3, 1, 3}, 
        {0, 0, 0}, 
        {0, 0, 0} 
    };

    private int[,] matrixTileSkinBar = new int[3,3]{ 
        {1, 0, 0}, 
        {0, 0, 0}, 
        {1, 0, 0} 
    };
        
    private int[,] matrixTileSkin1Side = new int[3,3]{ 
        {0, 0, 1}, 
        {0, 0, 0}, 
        {1, 0, 0} 
    };
    
    private int[,] matrixTileSkin2Side = new int[3,3]{ 
        {0, 0, 0}, 
        {0, 0, 0}, 
        {1, 0, 1} 
    };
    
    private int[,] matrixResult = new int[3,3]{ 
        {0, 0, 0}, 
        {0, 0, 0}, 
        {0, 0, 0} 
    };
    
    private int[,] matrixSubstraction = new int[3,3]{ 
        {0, 0, 0}, 
        {0, 0, 0}, 
        {0, 0, 0} 
    };

    /// <summary>
    /// setup
    /// </summary>

    private void Start()
    {
        PimpeAllTheTiles();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PimpeAllTheTiles();
        }
    }

    public void PimpSelectedTile(IndividualTile tile)
    {
        FindNeibhours(tile);
    }
    public void PimpAdjacentTiles(IndividualTile tile)
    {
        
    }

    public void PimpeAllTheTiles()
    {
        _allTheTile = FindObjectsOfType<IndividualTile>();
        LoopForAllTheTile();
        
        ListOfAllTheBoarders();
        ChooseTypeTile();
    }
    
    /// <summary>
    /// // coners
    /// </summary>

    private void ListOfAllTheBoarders()
    {
        listOfBoarderTiles.Clear();
        for (int i = 0; i < _allTheTile.Length; i++)
        {
            if (_allTheTile[i].tilesSkinTypeEnum == TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide1
            || _allTheTile[i].tilesSkinTypeEnum == TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBar
            || _allTheTile[i].tilesSkinTypeEnum == TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBridge
            || _allTheTile[i].tilesSkinTypeEnum == TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide2)
            {
                _allTheTile[i].id = i;
                // maybe add them to a dictionary 
                listOfBoarderTiles.Add(_allTheTile[i]);
            }
        }
    }

    private void ResetTheMatrixSystem()
    {
        matrixResult = new int[3,3]
        {
            {0,0,0},
            {0,0,0},
            {0,0,0}
        };
        matrixSubstraction = new int[3,3]
        {
            {0,0,0},
            {0,0,0},
            {0,0,0}
        };
    }

    private void ResetConers()
    {
        GameObject[] coners = GameObject.FindGameObjectsWithTag("Coner");
        for (int i = 0; i < coners.Length; i++)
        {
            Destroy(coners[i]);
        }
    }
    

    private void ChooseTypeTile()
    {
        ResetConers();
        for (int i = 0; i < listOfBoarderTiles.Count; i++)
        {
            ResetTheMatrixSystem();
            if (listOfBoarderTiles[i].tilesSkinTypeEnum == TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBridge)
            {
                //matrixTileSkinBridge
                MatrixSystem(matrixTileSkinBridge, listOfBoarderTiles[i].id, listOfBoarderTiles[i]);
                
                //instatiate(wiht rotation of the tile) depending on the result above
                PlaceCornerPieaces(listOfBoarderTiles[i], matrixResult);
            }
            else if(listOfBoarderTiles[i].tilesSkinTypeEnum == TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBar)
            {
                //matrixTileSkinBar
                PlaceCornerPieaces(listOfBoarderTiles[i], matrixTileSkinBar);
            }
            else if (listOfBoarderTiles[i].tilesSkinTypeEnum == TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide1)
            {
                //matrixTileSkin1Side
                PlaceCornerPieaces(listOfBoarderTiles[i], matrixTileSkin1Side);
            }
            else if (listOfBoarderTiles[i].tilesSkinTypeEnum == TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide2)
            {
                //matrixTileSkin2Side
                PlaceCornerPieaces(listOfBoarderTiles[i], matrixTileSkin2Side);
            }
        }
        
    }
    GameObject emptyGameobject;

    private void PlaceCornerPieaces(IndividualTile tile, int[,] finalMatrix)
    {
        for (int i = 0; i < _tileScriptableObject.Length; i++)
        {
            if (_tileScriptableObject[i].typeOfTiles == tile.tilesTypeEnum)
            {
                Quaternion rot = tile.transform.rotation;
                    if (finalMatrix[0, 0] == 1)
                    {
                        /*emptyGameobject = _tileScriptableObject[i].conersStraight[0];
                        GameObject conerPiece = Instantiate(emptyGameobject,tile.transform.position,rot);
                        conerPiece.transform.parent = tile.transform;*/
                        instatiateCornerPiece(tile, 0,i, rot,0);
                    }
                    if (finalMatrix[0, 0] == 2)
                    {
                        instatiateCornerPiece(tile, 0,i, rot,1);
                    }
                
                    if (finalMatrix[0, 2] == 1)
                    {
                        instatiateCornerPiece(tile, 1,i, rot,0);
                    }
                    if (finalMatrix[0, 0] == 2)
                    {
                        instatiateCornerPiece(tile, 1,i, rot,1);
                    }
                
                    if (finalMatrix[2, 0] == 1)
                    {
                        instatiateCornerPiece(tile, 2,i, rot,0);
                    }
                    if (finalMatrix[2, 0] == 2)
                    {
                        instatiateCornerPiece(tile, 2,i, rot,1);
                    }
                
                    if (finalMatrix[2, 2] == 1)
                    {
                        instatiateCornerPiece(tile, 3,i, rot,0);
                    }
                    if (finalMatrix[2, 2] == 2)
                    {
                        instatiateCornerPiece(tile, 3,i, rot,1);
                    }
            }
        }
    }

    private void instatiateCornerPiece(IndividualTile tile, int whichPosition,int i, Quaternion rot,int conerType)
    {
        if (conerType == 0)
        {
            emptyGameobject = _tileScriptableObject[i].conersStraight[whichPosition];
        }
        else
        {
            emptyGameobject = _tileScriptableObject[i].conersStraight[whichPosition];
        }
        GameObject conerPiece =  Instantiate(emptyGameobject,tile.transform.position, rot);
        conerPiece.transform.parent = tile.transform;
        conerPiece.transform.tag = "Coner";
    }
    
    // needs to know which coners to discard
    public List<Vector3> ConvertMatrixIntonArrayDirection(int[,] matrixTileSet)
    {
        List<Vector3> listOfDirection = new List<Vector3>();

        if (matrixTileSet[0, 0] == 1) 
            listOfDirection.Add(Vector3.left + Vector3.back);
        if (matrixTileSet[0, 1] == 1) 
            listOfDirection.Add(Vector3.back);
        if (matrixTileSet[0, 2] == 1) 
            listOfDirection.Add(Vector3.left + Vector3.forward);
        
        if (matrixTileSet[1, 0] == 1) 
            listOfDirection.Add(Vector3.back);
        if (matrixTileSet[1, 1] == 1) 
            listOfDirection.Add(Vector3.zero);
        if (matrixTileSet[1, 2] == 1) 
            listOfDirection.Add(Vector3.forward);
        
        if (matrixTileSet[2, 0] == 1) 
            listOfDirection.Add(Vector3.back + Vector3.right);
        if (matrixTileSet[2, 1] == 1) 
            listOfDirection.Add(Vector3.right);
        if (matrixTileSet[2, 2] == 1) 
            listOfDirection.Add(Vector3.right + Vector3.forward);
        
        return listOfDirection;
    }

    public int[,] ConvertDirectionIntoMatrix(Vector3 dir)
    {
        int [,] temporyMatrix = new int[3,3];
        if (dir == new Vector3(-1, 0, -1))
            temporyMatrix[0, 0] = 1;
        if (dir == new Vector3(-1, 0, 0))
            temporyMatrix[0, 1] = 1;
        if (dir == new Vector3(-1, 0, 1))
            temporyMatrix[0, 2] = 1;
        
        if (dir == new Vector3(0, 0, -1))
            temporyMatrix[1, 0] = 1;
        if (dir == new Vector3(0, 0, 0))
            temporyMatrix[1, 1] = 1;
        if (dir == new Vector3(0, 0, 1))
            temporyMatrix[1, 2] = 1;
        
        if (dir == new Vector3(1, 0, -1))
            temporyMatrix[2, 0] = 1;
        if (dir == new Vector3(1, 0, 0))
            temporyMatrix[2, 1] = 1;
        if (dir == new Vector3(1, 0, 1))
            temporyMatrix[2, 2] = 1;
        
        return temporyMatrix;
    }
    
    private void MatrixSystem(int[,] matrixTileSet, int id, IndividualTile tile)
    {
        //temporaire
        matrixResult = matrixTileSet;
        
        List<Vector3> vecDirectionArrays = ConvertMatrixIntonArrayDirection(matrixTileSet);
        for (int i = 0; i < vecDirectionArrays.Count; i++)
        {
            bool inside = ColiderWithOtherTile(tile.transform.position, vecDirectionArrays[i]);
            if (inside == true)
            {
                // demande de l'aide lol
                /*matrixResult = ConvertDirectionIntoMatrix(vecDirectionArrays[i]);
                int x = matrixTileSet.GetUpperBound(0);
                int y = matrixTest.GetUpperBound(1);
                
                for (int e = 0; e <= x; e++)
                {
                    for (int j = 0; j <= y; j++)
                    {
                        Debug.Log(matrixEmpty[i, j] = matrixTileSet[i, j] + matrixTest[i, j]);
                    }
                }*/
            }
        }
        ReverseMatrixRule();
        
    }
    
    void ReverseMatrixRule()
    {
        // matrix subastractiion if there's one
    }
    
    /// <summary>
    ///  tile placement for the skin
    /// </summary>
    /// <param name="tile"></param>

    
    private void LoopForAllTheTile()
    {
        for (int i = 0; i < _allTheTile.Length; i++)
        {
            FindNeibhours(_allTheTile[i]);
        }
    }
    private void FindNeibhours(IndividualTile tile)
    {
        int[] tileCode = TileIdSetCode(tile.transform.position,false);
        CheckIsUpSideDown(tile);
        CheckFor2side(tile, tileCode);
        CheckFor1side(tile, tileCode);
        CheckForBridgeSide(tile, tileCode);
        CheckFor0SideAndBridgesBarTiles(tile, tileCode);
    }

    private void CheckIsUpSideDown(IndividualTile tile)
    {
        Vector3 eulerOfTheTile = tile.transform.eulerAngles;
        if (eulerOfTheTile == new Vector3(eulerOfTheTile.x, eulerOfTheTile.y, 180))
        {
            _miroireVector3Tile =new Vector3(0,0,0);
            _miroireValuesTileZ = 180;
            _IsMiroirTile = true;
        }
        else
        {
            _miroireValuesTileZ = 0;
            _IsMiroirTile = false;
        }
    }
    
    private void ChangeSkinScriptableObject(IndividualTile tile,int numberOnTheSkinIndex)
    {
        for (int i = 0; i < _tileScriptableObject.Length; i++)
        {
            if (_tileScriptableObject[i].typeOfTiles == tile.tilesTypeEnum)
            {
                int[] tileCodeHeight = TileIdSetCode(tile.transform.position,true);
                tileCodeHeight = new[] {tileCodeHeight[4], tileCodeHeight[5]}; // take the last two degits

                int[] arrayOfCompareDown = {0,1};
                int[] arrayOfCompareFDownUp = {1,1};
                
                //make sure that the tiles bellow don<t fuck up
                if (_IsMiroirTile == false)
                {
                    arrayOfCompareDown[0] = 0;
                    arrayOfCompareDown[1] = 1;
                    arrayOfCompareFDownUp[0] = 1;
                    arrayOfCompareFDownUp[1] = 1;
                }
                else
                {
                    arrayOfCompareDown[0] = 1;
                    arrayOfCompareDown[1] = 0;
                    arrayOfCompareFDownUp[0] = 1;
                    arrayOfCompareFDownUp[1] = 1;
                }
                
                //change it to prefabs 
                if (arrayOfCompareDown.SequenceEqual(tileCodeHeight) || arrayOfCompareFDownUp.SequenceEqual(tileCodeHeight))
                {
                    tile.GetComponent<MeshFilter>().sharedMesh = _tileScriptableObject[i].meshsHeightVariants[numberOnTheSkinIndex];
                }
                else
                {
                    tile.GetComponent<MeshFilter>().sharedMesh = _tileScriptableObject[i].meshs[numberOnTheSkinIndex];
                }
            }
        }
    }

    private void CheckFor0SideAndBridgesBarTiles(IndividualTile tile,int[] tileCode) // 1 empty side and none and full side 
    {
        int[] arrayOfCompareF = {1, 1, 1, 0};
        int[] arrayOfCompareB = {1, 1, 0, 1};
        int[] arrayOfCompareR = {1, 0, 1, 1};
        int[] arrayOfCompareL = {0, 1, 1, 1};
        
        int[] arrayOfCompareNone = {0, 0, 0, 0};
        int[] arrayOfCompareAll = {1, 1, 1, 1};
        
        //string word = TileIdSetCode(tile.transform.position, true).Select(i => i.ToString()).Aggregate((i, j) => i + j);
        //Debug.Log(word);
        //change for prefabs

        if (arrayOfCompareF.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 3);
            _miroireValuesTileY = _IsMiroirTile == true ? 90 : 90;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBar;
        }
        else if (arrayOfCompareB.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 3);
            _miroireValuesTileY = _IsMiroirTile == true ? -90 : -90;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBar;
        }
        else if (arrayOfCompareR.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 3);
            _miroireValuesTileY = _IsMiroirTile == true ? 180 : 180;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBar;
        }
        else if (arrayOfCompareL.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 3);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBar;
        }
        else if (arrayOfCompareNone.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 0);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileSide0;
        }
        else if (arrayOfCompareAll.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 0);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileSide0;
        }
    }

    private void CheckForBridgeSide(IndividualTile tile,int[] tileCode)
    {
        int[] arrayOfCompareFB = {1, 1, 0, 0};
        int[] arrayOfCompareRL = {0, 0, 1, 1};
        
        if (arrayOfCompareFB.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 4);
            _miroireValuesTileY = _IsMiroirTile == true ? 90 : 90;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBridge;
        }
        else if (arrayOfCompareRL.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 4);
            _miroireValuesTileY = _IsMiroirTile == true ? 0 : 0;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderBridge;
        }
    }

    private void CheckFor1side(IndividualTile tile,int[] tileCode)
    {
        int[] arrayOfCompareTL = {1, 0, 1, 0}; // defaulkt
        int[] arrayOfCompareBL = {0, 1, 1, 0};
        int[] arrayOfCompareTR = {1, 0, 0, 1};
        int[] arrayOfCompareBR = {0, 1, 0, 1};
        if (arrayOfCompareBL.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 1);
            _miroireValuesTileY = _IsMiroirTile == true ? -270 : 0;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide1;
        }
        else if (arrayOfCompareTL.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 1);
            _miroireValuesTileY = _IsMiroirTile == true ? -180 : 90;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide1;
        }
        else if (arrayOfCompareTR.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 1);
            _miroireValuesTileY = _IsMiroirTile == true ? -90 : 180;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide1;
        }
        else if (arrayOfCompareBR.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 1);
            _miroireValuesTileY = _IsMiroirTile == true ? 0 : 270;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide1;
        }
    }

    private void CheckFor2side(IndividualTile tile,int[] tileCode)
    {
        int[] arrayOfCompareF = {1, 0, 0, 0};
        int[] arrayOfCompareB = {0, 1, 0, 0};
        int[] arrayOfCompareR = {0, 0, 1, 0};
        int[] arrayOfCompareL = {0, 0, 0, 1};
        if (arrayOfCompareF.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 2);
            _miroireValuesTileY = _IsMiroirTile == true ? -90 : 90;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide2;
        }
        else if (arrayOfCompareB.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 2);
            _miroireValuesTileY = _IsMiroirTile == true ? -270 : 270;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide2;
        }
        else if (arrayOfCompareR.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 2);
            _miroireValuesTileY = _IsMiroirTile == true ? 180 : 0;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide2;
        }
        else if (arrayOfCompareL.SequenceEqual(tileCode))
        {
            ChangeSkinScriptableObject(tile, 2);
            _miroireValuesTileY = _IsMiroirTile == true ? 0 : 180;
            tile.transform.rotation = Quaternion.Euler(0, _miroireValuesTileY, _miroireValuesTileZ);
            tile.tilesSkinTypeEnum = TilesTypeEnumClass.tilesSkinTypeEnum.TileBorderSide2;
        }
    }
    
    // maybe made a new flexible script for future stuff // refactor
    // maybe the tilesetcode should give you the information about the rotation of the tile so that it is easier to identefy upsidedown tiles
    private int[] TileIdSetCode(Vector3 tileposition, bool AsHeight) // as height as a enum
    {
        var code = AsHeight == true ? new []{0,0,0,0,0,0}: new []{0,0,0,0};
        bool forward = ColiderWithOtherTile(tileposition, Vector3.back);
        code[0] = forward == true ? 1 : 0;
        bool back = ColiderWithOtherTile(tileposition, Vector3.forward);
        code[1] = back == true ? 1 : 0;
        bool right = ColiderWithOtherTile(tileposition, Vector3.right);
        code[2] = right == true ? 1 : 0;
        bool left = ColiderWithOtherTile(tileposition, Vector3.left);
        code[3] = left == true ? 1 : 0;
        
        if (AsHeight)
        {
            bool up = ColiderWithOtherTile(tileposition, Vector3.up);
            code[4] = up == true ? 1 : 0;
            bool down = ColiderWithOtherTile(tileposition, Vector3.down);
            code[5] = down == true ? 1 : 0;
        }
        return code;
    }

    public bool ColiderWithOtherTile(Vector3 origineTilePosition,Vector3 vectorDirection)
    {
        var _colliders = Physics.OverlapBox(origineTilePosition + vectorDirection, new Vector3(0.25f,0.25f,0.25f));
        bool isInsideColider;
        if (_colliders.Length == 0)
        {
            isInsideColider = false;
        }
        else
        {
            isInsideColider = true;    
        }
        return isInsideColider;
    }
}
