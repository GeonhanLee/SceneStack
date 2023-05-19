# SceneStack
SceneStack is a URP CameraStack-like SceneManagement System for Multi-Scene Workflow.  


## Features
- Multi-scene management
- URP Camera Stacking support
  - Find and add all overlay cameras across scene to the base camera.
  - Removes annoying warnings by referencing overlay cameras from different scenes.

## Usage
### UI scene
todo
## Overview
### SceneStackSOManager
`SceneStackSOManager` Reserializes all `SceneStackSO` by `ReserializeAllSceneStackSO()`.
The method is called when below occurs.
1. `ExitingEditMode`
2. `OnPreprocessBuild`
3. `OnPostprocessAllAssets` : when SceneAsset is imported / deleted / moved

`ReserializeAllSceneStackSO()` automatically saves dirtied `SceneAssetSO`s when called.
This is because `AssetDatabase.ForceReserializeAssets()` does not reserialize any unsaved change.

## Todo
- async scene loading

## Credits & Similar Projects
Heavily inspired by [Eflatun.SceneReference](https://github.com/starikcetin/Eflatun.SceneReference)