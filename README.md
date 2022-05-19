![(PSD) MS Banner Template](https://user-images.githubusercontent.com/33253710/169396164-002bc6fc-e749-4119-a0da-3a49fb7e5f64.jpg)


# Multi-Scene
> Version 0.1.3


A library to help developers create Unity games that split elements of the game into multiple scenes.


## Features
- Cross scene referencing
- Multi scene management
- Extensions with extra functionality

### To-Do:
- Further editor tools
- Optimisation extension for better scene loading
- More scene loading options
- Additional attributes for more flexiability

### Limitations
- At present scenes don't wait for the last group to be fully unloaded before the new group is loaded in. Be aware of this when using this system in your games.

## How To Install
Download the latest version of the project via the packages section of this repo & un-zip the downloaded files. Then either:
1. Run the .unitypackage and proceed to follow the steps in import window in Unity
2. Open your Unity project, right-click in the project window and select the "Import Custom Package" option. Select the package and follow the steps in import window

## Basic Setup
The setup your project for Multi-scene, choose a scene to be the main or active scene. This is known as the ```base``` scene which is the scene the system loads first and the default scene for any object spawning, referencing etc. Its recommended for this scene to be a manager scene for your game or something similar, but it can just be an empty scene with no other functionality if you wish.

![Multi Scene Manager](https://user-images.githubusercontent.com/33253710/153851111-d00e0f43-0578-4de9-a95e-042e1e5b42f0.png)

Then follow the following steps:
1. Add the ```MultiSceneManager``` script to a gameObject in the ```base``` scene
2. Create a ```Scene Group``` asset via the "Create Asset Menu" and add the name of the base scene to it

![Create Asset Menu Location](https://user-images.githubusercontent.com/33253710/153851235-8a93d193-cba0-40ec-a9e5-dc18f2a2795b.png)

3. Add the names of any additional scene in the additive section of the asset

![Scene Group Example](https://user-images.githubusercontent.com/33253710/153851320-70bf2ac1-a930-406f-a6a8-c8bc51b159eb.png)

4. Assign the asset to the ```MultiSceneManager```
This is all the setup the package needs to run a multi scene setup. 



### Cross Scene Referencing
Unity has no native support for referencing between active scenes, so the package contains a solution to this issue. While systems such as injection can get around the issue, the solution I've provided is more akin to the ```FindObjectsOfType``` & ```GetComponent``` styles of referencing that all will be familiar with. The only difference with this is that. The ```MultiSceneElly``` class provides the functionality for this by searching all active scenes or a specific scene that you provide for the component of the type you are looking for. Its import to note that this will work whenever called, but you will need to be sure the scene is loaded when you call for it. 

### Awake, OnEnable or Start But MultiScene
To ensure logic runs when all scenes are loaded in a group, you can implement an interface. For farmiliarity these are called ```IMultiSceneAwake```, ```IMultiSceneEnable``` & ```IMultiSceneStart```. When implemented any logic in the implementation method will run once all scene have been loading by the ```MultiSceneManager```. Alternatively you can listen to the before & post events on the ```MultiSceneManager``` and achieve the same result. 

#### Ordering
If you are using any of ```IMultiSceneAwake```, ```IMultiSceneEnable``` or ```IMultiSceneStart``` you can also define their execution order just like in the scripting execution order in the porject settings. This is done by adding the ```[MultiSceneOrdered]``` attribute to any of the previously mentioned interface implementation methods. The define the order itself, just add a number in brackets like so: ```[MultiSceneOrdered(-10)]```. The order runs with negative numbers first, then any with no ordering followed by any with a positive number. 

# Extensions
While the core library will handle most essential logic, there are some extension packages which act as extra features that you can choose to use if your project requires them. These are standalone and will always require the core library to function. Currently there are the following extensions:

### URP Cameras
URP Cameras adds support for URP camera stacking between scenes with some basic additional options added in such as following the base camera transform. 

### Do Not Destroy
This adds support for getting scripts that are in the Do Not Destroy scene, handy if you want persistent logic that you don't want to be static. 
