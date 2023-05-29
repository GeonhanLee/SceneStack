# SceneStack
SceneStack is a multi-scene management system with URP CameraStack support.

## Features
- Multi-scene management
- URP Camera Stacking support
  - Find and add all overlay cameras across scene to the base camera.
  - Removes annoying cross-Scene reference warnings by referencing overlay cameras from another scene.

## Usage

### UI Scene
Using SceneStack, UI camera & canvas can be managed in separate scenes.  
![image](https://github.com/GeonhanLee/SceneStack/assets/37390116/38aef70c-28f4-4048-8aa0-afe2c46d43d3)

1. Create a UI camera with Render Type - Overlay and Culling Mask - UI.  
2. Create a canvas with Screen Space - Camera and set Render Camera to your UI camera.

## Guide

### Getting started
Add `using Malcha.SceneStack;`

### Configure SceneStack in editor mode
Create `SceneStackSO` using the menu `Assets > Create > SceneStack > Create SceneStack`.  
![image](https://github.com/GeonhanLee/SceneStack/assets/37390116/6264f746-0f93-4531-bea2-a0a42909dc17)  
Assign your scene to the base scene field in the inspector.  
You can also add overlay scenes as a stack.  
![image](https://github.com/GeonhanLee/SceneStack/assets/37390116/8a0d46a3-46af-4584-8293-681138b33655)

### Configure SceneStack in runtime
You can create your own `SceneStack` without `SceneStackSO`.
```cs
SceneStack stack = new SceneStack("BaseScene");
// stack.baseScene = new SceneData("BaseScene"); is also ok.
stack.overlayScenes = new List<SceneData>
{
  new SceneData("Assets/SampleScenes/UIOverlaySceneA.unity"),
  new SceneData("UIOverlaySceneB"),
  new SceneData("SampleScenes/UIOverlaySceneC")
};
```
You can fill the class `SceneData`'s constructor with the name or path of the scene as [`SceneManager.LoadScene`](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html).

### CameraStack
Overlay cameras in scenes included in the `SceneStack` will be added to base camera when `SceneStack` is loaded.  
![image](https://github.com/GeonhanLee/SceneStack/assets/37390116/0248418f-a991-4af3-b7f9-7b2cb9a20f30)  
The order of cameras in camera stack is equivalent to the order of belonged scene in the `SceneStack`.  
![image](https://github.com/GeonhanLee/SceneStack/assets/37390116/1411dbb5-b0b1-42fe-ac17-cd0bd152754d)  

### Load SceneStack in editor mode
You can load your `SceneStack` in editor mode by clicking the button on `SceneStackSO`,  
![image](https://github.com/GeonhanLee/SceneStack/assets/37390116/77394d96-bf7d-49b8-9ccc-574b21f8f91f)  
or right click the `SceneStackSO` in the project window and select `Open Scene Stack` menu.  
![image](https://github.com/GeonhanLee/SceneStack/assets/37390116/b6523835-5d5b-42db-a471-8521164c9769)


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
![image](https://github.com/GeonhanLee/SceneStack/assets/37390116/0697f7f1-9691-4556-a163-89c74ba821d1)

### SceneStackCameraSorter
Camera is sorted in the camera stack with an index of  belonged scene.  
However, you can sort multiple camera in the same scene with `SceneStackCameraSorter` component.  

Add `SceneStackCameraSorter` component to your Camera object and modify Sorting Order in the inspector window. The lower sorting order is rendered first.  
![image](https://github.com/GeonhanLee/SceneStack/assets/37390116/71b3e9dc-b32f-402a-b6e4-2e404dd4f2a1)

## Overview

### SceneStackSOManager
`SceneStackSOManager` Reserializes all `SceneStackSO` by `ReserializeAllSceneStackSO()`.

The method is called when below occurs.  
1. `ExitingEditMode`
2. `OnPreprocessBuild`
3. `OnPostprocessAllAssets` : when SceneAsset is imported / deleted / moved

`ReserializeAllSceneStackSO()` automatically saves dirtied `SceneAssetSO` when called.  
This is because `AssetDatabase.ForceReserializeAssets()` serializes before the modification is applied.

### SceneStackWarningSuppressor
`SceneStackWarningSuppressor` removes annoying warnings on the editor by removing cross scene references when entering/exiting playmode.  

It also selects and deselects camera when entering/exiting playmode to remove the error message below.  
https://forum.unity.com/threads/indexoutofrangeexception-in-urp.1306230


## Limits & To-Do
The list below is not currently supported.
- Multiple base camera (e.g. split screen, render texture)
- Async scene loading
- Check if scene is in build setting

## Credits & Similar Projects
`SceneReference` and `ReserializeAllSceneStackSO()` is based and inspired by [Eflatun.SceneReference](https://github.com/starikcetin/Eflatun.SceneReference).
- Unlike `Eflatun.SceneReference`, SceneStack doesn't generate a Scene GUID to Path Map. Instead, SceneStack reserializes all `SceneStackSO`.
- SceneStack doesn't provide SceneReference API to user.

## License
MIT License. Refer to the [LICENSE.md](./LICENSE.md) file.