using UnityEngine;
using DG.Tweening;
public class IndividualTile : MonoBehaviour 
{
    [Header("boolen that change overtime")]
    // maybe change that for enums
    public bool walkable;
    public bool curent;
    public bool notClickable;

    public enum TilesType
    {
        TileNormal,
        TileMagnetizer,
        TileDemagnetizer,
        TileRamp,
        TileLadderTile
    };
    public TilesTypeEnumClass.tilesTypeEnum tilesTypeEnum;
    public TilesTypeEnumClass.tilesSkinTypeEnum tilesSkinTypeEnum;
    public int id;
    [Space(10)]
    [Header("Adjustable Parameters")]
    public float colorSwitchDirection = 0.3f;
    public Ease easeForTheTile;

    private Color _oldColor;
    private Color _colorToLerp;
    private Renderer _materialRender;

    private void Awake()
    {
        _oldColor = GetComponent<Renderer>().material.color;
        _materialRender = GetComponent<Renderer>();
    }

    public void ColorChange()
    {
        if (curent && !notClickable )
        {
            _materialRender.material.DOColor(Color.red, colorSwitchDirection).SetEase(easeForTheTile);
        }
        else if (walkable && !notClickable )
        {
            _materialRender.material.DOColor(Color.green, colorSwitchDirection).SetEase(easeForTheTile);
        }
        else if (notClickable)
        {
            _materialRender.material.DOColor(Color.cyan, colorSwitchDirection).SetEase(easeForTheTile);
        }
        else
        {
            _materialRender.material.DOColor(_oldColor, colorSwitchDirection).SetEase(easeForTheTile);
        }
    }
    public void ResetVariable()
    {
        walkable = false;
        curent = false;
        notClickable = false;
        ColorChange();
    }
}
