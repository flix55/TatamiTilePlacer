using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class TatamiPlacerTool : EditorWindow
{

    [MenuItem("Tools/TatamiPlacer")] 
    public static void OpenGrim() => GetWindow<TatamiPlacerTool>();
    public float radius = 2f;
    public int spawnCount = 10;
    public GameObject spawnPrefabs = null;
    
    private int assetVersion;
    private string bannerVersion;
    Color bannerColor;
    string bannerText;
    GUIStyle stylePopup;
    private float _pointOfssetSelction;
    
    float rotationHotKeyAmount = 0;

    private bool modeSelectObject;
    
    private SerializedProperty _propRadius;
    private SerializedProperty _propSpawnCount;
    private SerializedProperty _propspawnPrefabs;
    private SerializedObject so;

    private GameObject[] prefabs;
    private Texture[] textures;
    private Material[] materials;
    
    private bool _showIconToogle;
    private Vector2 _mousePositionOnScreen;

    private void FindThePrefabInFolder()
    {
        string[] guidsPrefabs = AssetDatabase.FindAssets("t:prefab", new[] {"Assets/Prefabs/Blocks/TilesMouvement"} ); // guids = global unique identifier
        IEnumerable<string> pathsPrefabs = guidsPrefabs.Select(AssetDatabase.GUIDToAssetPath);
        prefabs = pathsPrefabs.Select(AssetDatabase.LoadAssetAtPath<GameObject>).ToArray();

        string[] guidsTextures = AssetDatabase.FindAssets("t:texture", new[] {"Assets/Script/Editor/TatamiPlacer"} );
        IEnumerable<string> pathsTexture = guidsTextures.Select(AssetDatabase.GUIDToAssetPath);
        textures = pathsTexture.Select(AssetDatabase.LoadAssetAtPath<Texture>).ToArray();
        
        string[] guidsMat = AssetDatabase.FindAssets("t:material", new[] {"Assets/Script/Editor/TatamiPlacer/Material"} );
        IEnumerable<string> pathsMat = guidsMat.Select(AssetDatabase.GUIDToAssetPath);
        materials = pathsMat.Select(AssetDatabase.LoadAssetAtPath<Material>).ToArray();
    }
    
    private void OnEnable()
    {
        _pointOfssetSelction = 0.5f;
        so = new SerializedObject(this);
        _propRadius = so.FindProperty("radius");
        _propSpawnCount = so.FindProperty("spawnCount");
        _propspawnPrefabs = so.FindProperty("spawnPrefabs");
        FindThePrefabInFolder();
        
        bannerColor = new Color(0.55f, 0.7f, 1f);
        bannerVersion = assetVersion.ToString();
        bannerVersion = bannerVersion.Insert(1, ".");
        bannerColor = new Color(0.55f, 0.7f, 1f);
        bannerText = "Tatami Placer Tool" ;
        spawnPrefabs = prefabs[0];
        
        SceneView.duringSceneGui += OnSceneGUI;
        whichMode = 3;
    }

    private void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI; //close window
    
    int whichMode = 0;
    string[] selStrings = {"Place", "Erase", "replace", "Disable"};
    // draws on the custom window
    void OnGUI()
    {
        so.Update();
        // select wich type od mode 
        
        GUILayout.BeginVertical("Box");
        whichMode = GUILayout.SelectionGrid(whichMode, selStrings, 2);
        GUILayout.EndVertical();
        
        GUILayout.Space(10);
        CustomButtons();

        if (EditorApplication.isPlaying)
        {
            if (upsideDown == true)
            {
                Debug.LogError("flip le du bon barre avant de play");
            }
            EditorGUILayout.LabelField("Playing");
        }
        else
        {
            EditorGUILayout.LabelField("Not playing");
        }
       FixGUI();
    }

    void CustomButtons()
    {
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        if (GUILayout.Button(textures[4],GUILayout.Width(40),GUILayout.Height(40))) // rotate tile
        {
            rotateTile();
            Repaint();
        }
        if (GUILayout.Button(textures[2],GUILayout.Width(40),GUILayout.Height(40))) // flip canvas
        {
            Flip();
        } 
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        if (GUILayout.Button(textures[7],GUILayout.Width(40),GUILayout.Height(40))) // place
        {
            whichMode = 0;
        }
        if (GUILayout.Button(textures[1],GUILayout.Width(40),GUILayout.Height(40))) // erase
        {
            whichMode = 1;
        }
        if (GUILayout.Button(textures[3],GUILayout.Width(40),GUILayout.Height(40))) // replace
        {
            whichMode = 2;
        }
        if (GUILayout.Button(textures[0],GUILayout.Width(40),GUILayout.Height(40))) // disable
        {
            whichMode = 3;
        }
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        if (GUILayout.Button(textures[5],GUILayout.Width(40),GUILayout.Height(40))) // 1 scene
        {
            LayoutUtility.LoadLayout("Assets/Editor/Layouts/MY LAYOUT.wlt");
        }
        if (GUILayout.Button(textures[6],GUILayout.Width(40),GUILayout.Height(40))) // 2 scene
        {
            LayoutUtility.LoadLayout("Assets/Editor/Layouts/MY LAYOUTtatami.wlt");
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        if (GUILayout.Button("save",GUILayout.Width(15),GUILayout.Height(15))) // 1 scene
        {
            LayoutUtility.SaveLayout("Assets/Editor/Layouts/MY LAYOUTtatami.wlt");
        }
        GUILayout.EndHorizontal();
    }

    void Modes()
    {
        switch (whichMode)
        {
            case 0:
                modeSelectObject = false;
                break;
            case 1:
                modeSelectObject = true;
                break;
            case 2:
                modeSelectObject = true;
                break;
        }
    }

    void FixGUI()
    {
        if (so.ApplyModifiedProperties())
        {
            SceneView.RepaintAll(); // for less lag
        }
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            GUI.FocusControl(null); // fix the blue hightlight
            Repaint();
        }        
    }
    
    void HotKey()
    {
        bool holdingShift = (Event.current.modifiers & EventModifiers.Shift) == 0;
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R && holdingShift == false)
        {
            rotateTile();
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha1)
        {
            whichMode = 0;
            Repaint();
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha2)
        {
            whichMode = 1;
            Repaint();
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha3)
        {
            whichMode = 2;
            Repaint();
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha4)
        {
            whichMode = 3;
            Repaint();
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha5)
        {
            Flip();
            Repaint();
        }
    }

    void rotateTile()
    {
        rotationHotKeyAmount += 90;
        if (rotationHotKeyAmount >= 360)
        {
            rotationHotKeyAmount = 0;
        }
        rotationHotKeyAmount = Mathf.Round(rotationHotKeyAmount);
    }
    private void ShowIcon()
    {
        //GUI.Button(new Rect(8, 8, 64, 64), "hello world");  //just for test    
        Rect rect = new Rect(_mousePositionOnScreen.x,_mousePositionOnScreen.y - 64,64,64); // y coord increase to down and the x coord increase on the right of the screen
        foreach (var prefab in prefabs)
        {
            Texture icon = AssetPreview.GetAssetPreview(prefab);
            
            if (GUI.Button(rect, new GUIContent(icon)))
            {
                spawnPrefabs = prefab;
                Repaint();
                _showIconToogle = false;
                rotationHotKeyAmount = 0;
            }
            rect.x += rect.width + 2;
        }
    }
    
    private void GUIOnTheSceneDrawStuff()
    {
        Handles.BeginGUI();
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.G)
        {
            _showIconToogle = !_showIconToogle;
            _mousePositionOnScreen = Event.current.mousePosition;
        }
        if(_showIconToogle == true){ShowIcon();}
        Handles.EndGUI();
    }

    private void AnnoyingFix(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseMove)
        {
            sceneView.Repaint(); // to refesh the window so the blue highlight of the numbers wont be jank
        }
    }

    void RayCastPlacement(SceneView sceneView)
    {
        //Ray ray = new Ray(camTF.position, camTF.forward); // gets the cam center for placing the objects
        Transform camTF = sceneView.camera.transform;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit raycastHit;
        Plane hPlane = new Plane(Vector3.up, Vector3.zero); // idk how to make a plane
        float distance = 20f;
        if (spawnPrefabs == null) {return;}
        if (Physics.Raycast(ray, out raycastHit,distance) && _showIconToogle == false) // raycast
        {
            Vector3 selectionPosition = raycastHit.transform.position;
            Vector3 fixedSelction = raycastHit.point + (raycastHit.normal * _pointOfssetSelction);
            if (modeSelectObject == true)
            {
                renderGizmosHandles(raycastHit,selectionPosition);
                renderThePreviewMesh(raycastHit,selectionPosition);
            }
            else
            {
                renderGizmosHandles(raycastHit,selectionPosition);
                renderThePreviewMesh(raycastHit,fixedSelction);
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
            {
                if (modeSelectObject == true)
                {
                    if (whichMode == 1)
                    {
                        EraseTile(selectionPosition);
                    }

                    if (whichMode == 2)
                    {
                        ReplaceTile(selectionPosition);
                    }
                    sceneView.Repaint();
                    Repaint();
                }
                else
                {
                    SpawnObjet(fixedSelction);
                    sceneView.Repaint();
                    Repaint();
                }
            }
        }
    }
    
    private void EraseTile(Vector3 selectionPosition)
    {
         GameObject objcetToDelete = ExtensionMethods.positionToGameObject(selectionPosition);
         if(objcetToDelete == null){return;}
         Undo.DestroyObjectImmediate (objcetToDelete);
         DestroyImmediate(objcetToDelete);
    }
    private void ReplaceTile(Vector3 selectionPosition)
    {
        GameObject objcetToDelete = ExtensionMethods.positionToGameObject(selectionPosition);
        Undo.DestroyObjectImmediate (objcetToDelete);
        DestroyImmediate(objcetToDelete);
        SpawnObjet(selectionPosition);
    }
    
    void SpawnObjet(Vector3 hitSpawn) // I round the hitspawn to the grid with the right rotation and we got a game baby
    {
        if(spawnPrefabs == null){ return;}
        GameObject spawnThing = (GameObject)PrefabUtility.InstantiatePrefab(spawnPrefabs); // ??
        GameObject TileBellowObject = GameObject.Find("TilesBellow");
        GameObject TileUpObject = GameObject.Find("TilesTop");
        Undo.RegisterCreatedObjectUndo(spawnThing, "Spawn prefabs");
        spawnThing.transform.position = hitSpawn.Round();
        spawnThing.transform.rotation = RightRotationOfTheTile();
        spawnThing.transform.parent = upsideDown == true ? TileBellowObject.transform : TileUpObject.transform;
        // do the thing for the changing the prefab depending on the tile
    }
    
    void renderGizmosHandles(RaycastHit raycastHit,Vector3 positionToSpawnThings)
    {
        Handles.color = Color.black;
        Handles.DrawAAPolyLine(5f,positionToSpawnThings,positionToSpawnThings + raycastHit.normal);
        Handles.DrawWireDisc(positionToSpawnThings,raycastHit.normal,1f);
    }
    
    void renderThePreviewMesh(RaycastHit raycastHit,Vector3 positionToSpawnThings)
    {
        // render the previewMesh
        if(spawnPrefabs.GetComponent<MeshFilter>() == null){return;}
        Mesh mesh = spawnPrefabs.GetComponent<MeshFilter>().sharedMesh; // getting the cube from the filter of the prefab
        
        MeshFilter filters = spawnPrefabs.GetComponent<MeshFilter>();
        Matrix4x4 childtopose = filters.transform.localToWorldMatrix;
        //Matrix4x4 childToWorldMtx = poset * childtopose;
            
        Material mat = null;
        PickMaterialOnMode(mat);
        Graphics.DrawMeshNow(mesh,positionToSpawnThings.Round(),RightRotationOfTheTile());// not optimisse but its ok
    }

    void PickMaterialOnMode(Material mat)
    {
        if (whichMode == 1)
        {
            materials[0].SetPass(1);
            mat = materials[1];
        }
        else if (whichMode == 2)
        {
            materials[0].SetPass(3);
            mat = materials[2];
        }
        else if (whichMode == 3)
        {
            materials[0].SetPass(0);
            mat = materials[0];
        }
        else
        {
            materials[0].SetPass(0);
            mat = materials[0];
        }
    }

    private bool upsideDown;
    void Flip()
    {
        // gameobject find is not the best way to call a object... maybe calling it with customs scripts ?
        GameObject flipParentCenter = GameObject.Find("Center"); // maybe in enable window to call that ?
        upsideDown = !upsideDown;
        if(flipParentCenter == null){return;}
        var centerPos = flipParentCenter.transform.position;
        var centerRotation = flipParentCenter.transform.eulerAngles;
        if (upsideDown == true)
        {
            flipParentCenter.transform.position = new Vector3(centerPos.x,centerPos.y -2,centerPos.z);
            flipParentCenter.transform.eulerAngles = new Vector3(centerRotation.x,centerRotation.y,centerRotation.z + 180);
        }
        else
        {
            flipParentCenter.transform.position = new Vector3(centerPos.x,centerPos.y +2,centerPos.z);
            flipParentCenter.transform.eulerAngles = new Vector3(centerRotation.x,centerRotation.y,centerRotation.z - 180);
        }
    }

    public Quaternion RightRotationOfTheTile()
    {
        Quaternion rot = Quaternion.LookRotation(Vector3.up) * Quaternion.Euler(90f,0f,0f);
        rot = rot * Quaternion.Euler(0, rotationHotKeyAmount, 0);
        return rot;
    }

    // draws the stufs on the scene view
    void OnSceneGUI(SceneView sceneView) 
    {
        GUIOnTheSceneDrawStuff();
        Modes();
        AnnoyingFix(sceneView);
        HotKey();
        if(whichMode == 3){return;}
        RayCastPlacement(sceneView);   
    }
}
