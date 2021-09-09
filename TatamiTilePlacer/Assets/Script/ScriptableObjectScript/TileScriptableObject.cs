using UnityEngine;


[CreateAssetMenu(fileName = "New TileScriptableObject", menuName = "My ScritableObject/TileScriptableObject")]
public class TileScriptableObject : ScriptableObject
{
    [Header("Tile Type")] 
    public TilesTypeEnumClass.tilesTypeEnum typeOfTiles;
    [Header("Mesh and Prefabs")]
    public Mesh[] meshs;
    public Mesh[] meshsHeightVariants;
    public GameObject[] conersStraight;
    public GameObject[] conersCruve;
    [Header("Prop")]
    [Range(0, 100)] public float chanceToGetProp;
    // maybe add the prop in the parent object
    public GameObject[] props;
}
