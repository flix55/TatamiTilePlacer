using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    // to prevent the boilerplate garbage
    // extension method
    public static Vector3 Round( this Vector3 v )
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }

    public static Vector3 Round(this Vector3 v, float size)
    {
        return  (v / size).Round() * size;
    }

    public static GameObject positionToGameObject(Vector3 boxPosition)
    {
        GameObject tile  = null;
        Collider[] colliders = Physics.OverlapBox(boxPosition , new Vector3(0.1f,0.1f,0.1f));
        foreach (var gameobjectColider in colliders)
        {
            if (gameobjectColider.GetComponent<IndividualTile>() != null)
            {
                tile = gameobjectColider.gameObject;
            }
        }
        return tile;
    }
    
    /*private void RadiuControle()
    {
        bool holdingAlt = (Event.current.modifiers & EventModifiers.Alt) == 0;
        bool holdingShift = (Event.current.modifiers & EventModifiers.Shift) == 0;// modifier is a bit field, or a enum flag, example at 3:14
    
        // change radius size with scroolwheel
        if (Event.current.type == EventType.ScrollWheel  && holdingAlt == false && holdingShift == false)
        {
            float scrollDir = Mathf.Sign(Event.current.delta.y); // scrollwhell movement 
            so.Update();
            _propRadius.floatValue *= 1f + scrollDir * 0.1f;
            so.ApplyModifiedProperties();
            Repaint(); // updates editor window
            Event.current.Use(); // consume the event, don,t let it fall through
        }
    }*/ // archive
    
}
