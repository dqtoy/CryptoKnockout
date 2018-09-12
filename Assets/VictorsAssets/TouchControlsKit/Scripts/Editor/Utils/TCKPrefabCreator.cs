﻿/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace TouchControlsKit.Inspector
{
    public static class TCKPrefabCreator
    {
        const string k_MainGOName = "_TCKCanvas";
        const string k_MenuAbbrev = "GameObject/UI/Touch Controls Kit/";
        const string k_NameAbbrev = "TouchControlsKit";

        static GameObject tckUIobj;
        static GameObject buttonObj, touchpadObj, steeringWheelObj;
        static GameObject joystickMainObj, joystickBackgrObj, joystickImageObj;
        static GameObject dpadMainObj, dpadArrowUPobj, dpadArrowDOWNobj, dpadArrowLEFTobj, dpadArrowRIGHTobj;

        static readonly Color32 defaultColor = new Color32( 255, 255, 255, 165 );

        static string spritesPath;


        // Get EditorPath
        private static void UpdateSpritesPath<T>( GameObject source ) where T : MonoBehaviour
        {
            string assetPath = AssetDatabase.GetAssetPath( MonoScript.FromMonoBehaviour( source.GetComponent<T>() ) );
            const string endFolder = "/Scripts";

            if( assetPath.Contains( endFolder ) )
            {
                int endIndex = assetPath.IndexOf( endFolder, 0 );
                string between = assetPath.Substring( 0, endIndex );
                spritesPath = between + "/Content/Sprites/";
            }
        }


        // CreateTCKInput [MenuItem]
        [MenuItem( k_MenuAbbrev + "TCK Canvas" )]
        private static void CreateTouchManager()
        {
            TCKInput.CheckUIEventSystem();

            if( tckUIobj == null )
            {
                TCKInput tckInputObj = Object.FindObjectOfType<TCKInput>();
                tckUIobj = ( tckInputObj != null ) ? tckInputObj.gameObject : null;
            }          

            if( tckUIobj != null )
            {
                UpdateSpritesPath<TCKInput>( tckUIobj );
                return;
            }                

            tckUIobj = new GameObject( k_MainGOName, typeof( Canvas ), typeof( GraphicRaycaster ), typeof( CanvasScaler ), typeof( TCKInput ) );
            tckUIobj.layer = LayerMask.NameToLayer( "UI" );

            Transform camTransform = new GameObject( "tckUICamera", typeof( GuiCamera ) ).transform;
            camTransform.parent = tckUIobj.transform;
            camTransform.localPosition = Vector3.zero;

            UpdateSpritesPath<GuiCamera>( camTransform.gameObject );

            float maxCameraDepth = -1f;
            Array.ForEach( Object.FindObjectsOfType<Camera>(), cam => maxCameraDepth = Mathf.Max( cam.depth, maxCameraDepth ) );

            Camera camera = camTransform.GetComponent<Camera>();
            camera.useOcclusionCulling = false;
            camera.orthographic = true;
            camera.allowMSAA = false;
            camera.allowHDR = false;
            camera.cullingMask = 32;
            camera.depth = ++maxCameraDepth;
            camera.orthographicSize = 100f;
            camera.nearClipPlane = -.25f;
            camera.farClipPlane = .25f;
            camera.renderingPath = RenderingPath.Forward;
            camera.clearFlags = CameraClearFlags.Depth;

            Canvas canvas = tckUIobj.GetComponent<Canvas>();            
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.pixelPerfect = true;

            CanvasScaler canvasScaler = tckUIobj.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;            
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            canvasScaler.referenceResolution = new Vector2( 1920, 1080 );

            MarkActiveSceneDirty();
        }

        [MenuItem( k_MenuAbbrev + "TCK Canvas", true )]
        private static bool ValidateCreateTouchManager()
        {
            return ( Object.FindObjectOfType<TCKInput>() == null );
        }

        // CreateButton [MenuItem]
        [MenuItem( k_MenuAbbrev + "Button" )]
        private static void CreateButton()
        {
            CreateTouchManager();
            SetupController<TCKButton>( ref buttonObj, tckUIobj.transform, "Button" + Object.FindObjectsOfType<TCKButton>().Length, true );

            TCKButton btn = buttonObj.GetComponent<TCKButton>();
            btn.baseImage = buttonObj.GetComponent<Image>();
            btn.baseRect = buttonObj.transform as RectTransform;
            btn.normalSprite = btn.baseImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "ButtonNormal.png" );
            btn.pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "ButtonPressed.png" );
            btn.identifier = buttonObj.name;
            btn.baseRect.sizeDelta = Vector2.one * 128;            
            buttonObj.transform.localScale = Vector3.one;
            btn.baseRect.anchoredPosition = RandomPos;

            MarkActiveSceneDirty();
        }

        // CreateJoystick [MenuItem]
        [MenuItem( k_MenuAbbrev + "Joystick" )]
        private static void CreateJoystick()
        {
            CreateTouchManager();
            SetupController<TCKJoystick>( ref joystickMainObj, tckUIobj.transform, "Joystick" + Object.FindObjectsOfType<TCKJoystick>().Length, true );

            TCKJoystick joy = joystickMainObj.GetComponent<TCKJoystick>();            
            joy.baseImage = joystickMainObj.GetComponent<Image>();
            joy.baseRect = joystickMainObj.transform as RectTransform;
            joy.baseImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "Touchzone.png" );

            SetupController<TCKJoystick>( ref joystickBackgrObj, joystickMainObj.transform, "JoystickBack", false );
            SetupController<TCKJoystick>( ref joystickImageObj, joystickBackgrObj.transform, "Joystick", false );

            joy.backgroundImage = joystickBackgrObj.GetComponent<Image>();
            joy.backgroundRT = joystickBackgrObj.transform as RectTransform;
            joy.backgroundRT.sizeDelta = Vector2.one * 160f; 
            joy.joystickImage = joystickImageObj.GetComponent<Image>();            
            joy.joystickRT = joystickImageObj.transform as RectTransform;
            joy.joystickRT.anchorMin = Vector2.zero;
            joy.joystickRT.anchorMax = Vector2.one;
            joy.joystickRT.sizeDelta = Vector2.zero;
            joy.backgroundImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "JoystickBack.png" );
            joy.joystickImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "Joystick.png" );
            joy.baseRect.sizeDelta = new Vector2( 500f, 450f );            
            joy.identifier = joystickMainObj.name;
            joystickMainObj.transform.localScale = Vector3.one;
            joy.baseRect.anchoredPosition = RandomPos;

            MarkActiveSceneDirty();
        }

        // CreateTouchpad [MenuItem]
        [MenuItem( k_MenuAbbrev + "Touchpad" )]
        private static void CreateTouchpad()
        {
            CreateTouchManager();
            SetupController<TCKTouchpad>( ref touchpadObj, tckUIobj.transform, "Touchpad" + Object.FindObjectsOfType<TCKTouchpad>().Length, true );

            TCKTouchpad tpd = touchpadObj.GetComponent<TCKTouchpad>();
            tpd.baseImage = touchpadObj.GetComponent<Image>();
            tpd.baseRect = touchpadObj.transform as RectTransform;
            tpd.baseImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "Touchzone.png" );
            tpd.identifier = touchpadObj.name;
            tpd.baseRect.sizeDelta = new Vector2( 630f, 430f );

            touchpadObj.transform.localScale = Vector3.one;
            tpd.baseRect.anchoredPosition = RandomPos;

            MarkActiveSceneDirty();
        }

        // CreateDPad [MenuItem]
        [MenuItem( k_MenuAbbrev + "DPad" )]
        private static void CreateDPad()
        {
            CreateTouchManager();
            SetupController<TCKDPad>( ref dpadMainObj, tckUIobj.transform, "DPad" + Object.FindObjectsOfType<TCKDPad>().Length, true );

            TCKDPad dpad = dpadMainObj.GetComponent<TCKDPad>();
            dpad.baseImage = dpadMainObj.GetComponent<Image>();
            dpad.baseRect = dpadMainObj.transform as RectTransform;
            dpad.normalSprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "ArrowNormal.png" );
            dpad.pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "ArrowPressed.png" );
            dpad.normalColor = dpad.pressedColor = defaultColor;
            dpad.identifier = dpadMainObj.name;
            dpad.baseRect.sizeDelta = Vector2.one * 380f;
            dpad.baseImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "Touchzone.png" );

            SetupController<TCKDPadArrow>( ref dpadArrowUPobj, dpadMainObj.transform, "ArrowUP", true, EArrowType.UP );
            SetupController<TCKDPadArrow>( ref dpadArrowDOWNobj, dpadMainObj.transform, "ArrowDOWN", true, EArrowType.DOWN );
            SetupController<TCKDPadArrow>( ref dpadArrowLEFTobj, dpadMainObj.transform, "ArrowLEFT", true, EArrowType.LEFT );
            SetupController<TCKDPadArrow>( ref dpadArrowRIGHTobj, dpadMainObj.transform, "ArrowRIGHT", true, EArrowType.RIGHT );

            dpadArrowUPobj.GetComponent<Image>().sprite = dpad.normalSprite;
            dpadArrowDOWNobj.GetComponent<Image>().sprite = dpad.normalSprite;
            dpadArrowLEFTobj.GetComponent<Image>().sprite = dpad.normalSprite;
            dpadArrowRIGHTobj.GetComponent<Image>().sprite = dpad.normalSprite;

            dpadMainObj.transform.localScale = Vector3.one;
            dpad.baseRect.anchoredPosition = RandomPos;

            MarkActiveSceneDirty();
        }

        // CreateSteeringWheel [MenuItem]
        [MenuItem( k_MenuAbbrev + "Steering Wheel" )]
        private static void CreateSteeringWheel()
        {
            CreateTouchManager();
            SetupController<TCKSteeringWheel>( ref steeringWheelObj, tckUIobj.transform, "SteeringWheel" + Object.FindObjectsOfType<TCKSteeringWheel>().Length, true );

            TCKSteeringWheel sw = steeringWheelObj.GetComponent<TCKSteeringWheel>();
            sw.baseImage = steeringWheelObj.GetComponent<Image>();
            sw.baseRect = steeringWheelObj.transform as RectTransform;
            sw.baseImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( spritesPath + "SteeringWheel.png" );
            sw.identifier = steeringWheelObj.name;
            sw.baseRect.sizeDelta = Vector2.one * 385f;

            steeringWheelObj.transform.localScale = Vector3.one;
            sw.baseRect.anchoredPosition = RandomPos;

            MarkActiveSceneDirty();
        }


        // SetupController<Generic>
        private static void SetupController<TComp>(
            ref GameObject myGO, Transform myParent, string myName, bool needMyComponent,
            EArrowType arrowType = EArrowType.none ) where TComp : MonoBehaviour
        {
            myGO = new GameObject( myName, typeof( Image ) );
            myGO.GetComponent<Image>().color = defaultColor;
            myGO.layer = LayerMask.NameToLayer( "UI" );
            myGO.transform.localScale = Vector3.one;
            myGO.transform.SetParent( myParent );

            if( needMyComponent ) {
                myGO.AddComponent<TComp>();
            }                

            if( arrowType != EArrowType.none )
            {
                myGO.GetComponent<TCKDPadArrow>().arrowType = arrowType;
                CalcDPadSizeAndPos( dpadMainObj.transform as RectTransform, myGO.transform as RectTransform, arrowType );
            }
        }

        // CalcDPadSizeAndPos
        private static void CalcDPadSizeAndPos( RectTransform mainRect, RectTransform childRect, EArrowType arrowType )
        {
            childRect.sizeDelta = mainRect.sizeDelta / 3.4f;
            childRect.pivot = new Vector2( 0f, .5f );

            Vector2 size = childRect.sizeDelta;

            switch( arrowType )
            {
                case EArrowType.UP:
                    childRect.anchoredPosition = new Vector2( 0f, -size.y );                    
                    childRect.anchorMin = childRect.anchorMax = new Vector2( .5f, 1f );
                    childRect.rotation = Quaternion.Euler( 0f, 0f, 90f );
                    break;

                case EArrowType.DOWN:
                    childRect.anchoredPosition = new Vector2( 0f, size.y );                    
                    childRect.anchorMin = childRect.anchorMax = new Vector2( .5f, 0f );
                    childRect.rotation = Quaternion.Euler( 0f, 0f, 270f );
                    break;

                case EArrowType.LEFT:
                    childRect.anchoredPosition = new Vector2( size.x, 0f );                    
                    childRect.anchorMin = childRect.anchorMax = new Vector2( 0f, .5f );
                    childRect.rotation = Quaternion.Euler( 0f, 0f, 180f );
                    break;

                case EArrowType.RIGHT:
                    childRect.anchoredPosition = new Vector2( -size.x, 0f );                    
                    childRect.anchorMin = childRect.anchorMax = new Vector2( 1f, .5f );
                    childRect.rotation = Quaternion.identity;
                    break;
            }
        }

        // RandomPos
        private static Vector2 RandomPos { get { return new Vector2( Random.Range( -350f, 350f ), Random.Range( -220f, 220f ) ); } }


        // Mark ActiveSceneDirty
        private static void MarkActiveSceneDirty()
        {
            EditorSceneManager.MarkSceneDirty( SceneManager.GetActiveScene() );
        }
    };
}