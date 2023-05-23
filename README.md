# SceneStack
SceneStack is a URP CameraStack-like SceneManagement System for Multi-Scene Workflow.  


## Features
- Multi-scene management
- URP Camera Stacking support
  - Find and add all overlay cameras across scene to the base camera.
  - Removes annoying warnings by referencing overlay cameras from different scenes.

## Guide

### Configure SceneStack
Create `SceneStackSO` using the menu `Assets > Create > SceneStack > Create SceneStack`.

Assign your scene to the base scene field in the inspector.  
You can also add overlay scenes as a stack.

### CameraStack
Overlay cameras in scenes included in the SceneStack will be added to base camera when SceneStack is loaded.

The order of cameras in camera stack is equivalent to the order of belonged scene in the SceneStack.

### Load SceneStack in editor mode
You can load your `SceneStack` in editor mode by clicking the button on `SceneStackSO`, or right click the `SceneStackSO` in the project window and select `Open Scene Stack` menu.

### Load SceneStack in runtime
You can load `SceneStack` using a static class, `SceneStackLoader` in runtime.
```cs
public void ExampleLoadSceneStackSO(SceneStackSO so)
{
  SceneStackLoader.LoadSceneStack(so);
}
public void ExampleLoadSceneStack(SceneStack ss)
{
  SceneStackLoader.LoadSceneStack(ss);
}
```

### SceneStackCanvasSorter 
The raycast order of canvas across multiple scenes with same sorting order is not guranteed.

`SceneStackCanvasSorter` sets the `sortingOrder` of a canvas to the index of a scene which belongs to.  
Add `SceneStackCanvasSorter` component to your canvas object.

### SceneStackCameraSorter
Camera is sorted in the camera stack with an index of  belonged scene.  
However, you can sort multiple camera in the same scene with `SceneStackCameraSorter` component.  

Add `SceneStackCameraSorter` component to your Camera object and modify Sorting Order in the inspector window. The lower sorting order is rendered first.

### SceneStackSOManager
`SceneStackSOManager` Reserializes all `SceneStackSO` by `ReserializeAllSceneStackSO()`.

The method is called when below occurs.  
1. `ExitingEditMode`
2. `OnPreprocessBuild`
3. `OnPostprocessAllAssets` : when SceneAsset is imported / deleted / moved

`ReserializeAllSceneStackSO()` automatically saves dirtied `SceneAssetSO`s when called.  
This is because `AssetDatabase.ForceReserializeAssets()` does not reserialize any unsaved change.

### SceneStackWarningSuppressor
`SceneStackWarningSuppressor` removes annoying warnings on editor by removing cross scene references when saving or starting playmode.  

It also selects and deselects camera when entering/exiting playmode to remove the error message below.  
https://forum.unity.com/threads/indexoutofrangeexception-in-urp.1306230

## Usage

### UI scene
You can manage your UI camera & canvas on a per-scene basis.  
Create a UI camera with Render Type - Overlay and Culling Mask - UI.  
Create a canvas with Screen Space - Camera and set Render Camera to your UI camera.

## Limits & To-Do
The list below is not currently supported.
- multiple base camera (e.g. split screen, render texture)
- async scene loading
- check if scene is in build setting

## Credits & Similar Projects
Heavily inspired by [Eflatun.SceneReference](https://github.com/starikcetin/Eflatun.SceneReference)
