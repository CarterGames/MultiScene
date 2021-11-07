# Multi Scene Workflow

# Summary
The currently workflow involves a scriptable object known as a Scene Group which can be used define how scenes are loaded using the Multi-Scene Manager. 

Traditional Awake(), OnEnable() & Start() methods don't work as true awake and start for all the scenes you load using more than one scene. They run whenever the scene they are in is loaded, which is okay until you need to reference between scenes. as they will just initiate when they themselves are loaded. So for example if you were to have a reference in **Scene A** which is loaded first that needs something in **Scene B** which is loaded second. The reference would fail as **Scene B** isn't loaded yet and therefore can't be found because on **Scene A** was loaded at the time the reference call was made.  Because of this I added out own version implementing IMultiSceneAwake, IMultiSceneEnable & IMultiSceneStart interfaces and having their methods calls run once all the scenes in the scene group have been loaded. More on this in a bit.

## Installation

Download the latest version via the packages section, clone the repository or just download the repository as a .zip. If downloading a package, you'll just import them into project via the "import custom package" option in Unity.

## Things To Note

Just so you are aware of it. Using multi-scenes like this will cause a few issues. The majority of your game will work just fine. Though you will find that some bits play funny such as:

- **Lighting -** Mostly when real-time, lighting plays a bit weird with a multi-scene setup. Its based to bake lighting if possible. You'll find it works okay as is, but in builds the lighting can go a bit funny and dark.
- **Physics -** Will work just find no matter which scene the has the objects in, as far as I know physics from one scene should be able to interact with physics from another perfectly fine. However you may get issues if the player loads before the platform it is loading to is ready under the player etc. So set the load order accordingly.

# Setup & Usage

To setup your project to use this system you will need to split your scenes into more than one scene per scene to really benefit from the system. Through you can just have single scenes if that works better. Then create a scene group via the create asset menu. 

## Namespaces

The codebase is split into several namespaces, most IDE's will auto fill the namespaces for you, but you can also seem them below if needed:

```csharp
// Holds the core codebase
Multiscene.Core

// Contains any editor code in the core library.
Multiscene.Core.Editor 
```

## Scene Group
The scene has a custom inspector to help with the setup of each one, which is already super simple! You first add the string name of your base scene. This is the scene Unity will set to be the active scene when all the scenes have loaded. The active scene is the scene where all objects are instantiated to 

Once set you can then add as many extra or additive scenes as you like. I find it best to have the management scripts in the base/active scene and then have the environment, UI and any other elements of the game in additive scenes. The order of the additive scenes doesn't really matter unless you really want specific scenes to load in a certain order. 

## Loading Scenes
Loading scenes work very much like you would with the normal SceneManager. The multi-scene manager has the option to load scenes on Awake() which I personally use a lot as it saves calling for it manually. Its best to have an instance of the Multi-scene manager is every base scene for ease of testing and general use. There are a few bits in the inspector

- **Load On Awake** - This tells the manager to load the scenes when you press play.
- **Scenes To Load** - Is the scene group that the manager will load when called to unless otherwise changed in code.
- **Before Scene Loaded** - This runs before the first scene in a group is loaded.
- **Post Scene Loaded** - This runs after all scenes in a group have loaded and after all the interfaces have been called.

## Opening Scenes In The Editor

It is really easy to load scenes additively in the editor. Simply drag and drop the scene file in the inspector and the scene will load. The active scene will display its name in bold to indicate it is the main active scene. you can change which scene this is by double clicking the scene you want to be the active scene or by right clicking on the scene name and selecting "Set Active Scene" from the options. 

## Cross Scene References

Obviously, the biggest issue with more than one scene is referencing. Now I've seen some rather overkill examples of this like a signalling system that is pretty much a version of injection. My current solution is much more basic called SceneElly (short for "scene element"). Below is reference to each method and what it does:

### SceneElly

### Move Objects

### MoveObjectToScene
```csharp
MoveObjectToScene(GameObject obj, string scene)
MoveObjectToScene(GameObject obj, int scene)
```
Moves the object entered to the scene requested using the scene name to define which scene to send to. String is used for scene name, int is used for scene position in Hierarchy.

**Returns Bool -** Whether it successful or not.

### MoveObjectsToScene
```csharp
MoveObjectsToScene(List<GameObject> obj, string scene)
MoveObjectsToScene(List<GameObject> obj, int scene)
```
Moves the objects entered to the scene requested using the scene name to define which scene to send to. String is used for scene name, int is used for scene position in Hierarchy.

**Returns Bool -** Whether it successful or not.

### MoveObjectToSceneByBuildIndex
``` csharp
MoveObjectToSceneByBuildIndex(GameObject, int buildIndex)
```
Moves the object entered to the scene requested using the build index to define which scene send to.

**Returns Bool -** Whether it successful or not

### MoveObjectsToSceneByBuildIndex
``` csharp
MoveObjectsToSceneByBuildIndex(List<GameObject>, int buildIndex)
```
Moves the objects entered to the scene requested using the build index to define which scene send to.

**Returns Bool -** Whether it successful or not

### Get Objects
### GetComponentFromScene
```csharp
GetComponentFromScene<T>()
GetComponentFromScene<T>(string scene)
GetComponentFromScene<T>(int scene)
GetComponentFromScene<T>(Scene scene)
```
Gets the component requested from the active scene if a scene is not defined. otherwise from the scene specified if valid.

**Returns T -** The first found component of the type entered.
            
### GetComponentsFromScene
```csharp
GetComponentsFromScene<T>()
GetComponentsFromScene<T>(string scene)
GetComponentsFromScene<T>(int scene)
GetComponentsFromScene<T>(Scene scene)
```
Gets the component requested from the active scene if a scene is not defined. otherwise from the scene specified if valid.

**Returns List of T -** The found components of the type entered.

### GetComponentFromSceneByBuildIndex
```csharp
GetComponentFromSceneByBuildIndex<T>(int scene)
```
Gets the component requested from the scene of the build index entered

**Returns T -** The first found component of the type entered.
            
### GetComponentsFromSceneByBuildIndex
``` csharp
GetComponentsFromSceneByBuildIndex<T>(int scene)
```
Gets the components requested from the scene of the build index entered
  
**Returns List of T -** The found components of the type entered.

### GetComponentFromScenes
``` csharp
GetComponentFromScenes<T>(List<Scene> scenes)
```
Gets the components requested from the scenes entered.

**Returns T -** The first found component of the type entered.

### GetComponentsFromScenes
``` csharp
GetComponentsFromScenes<T>(List<Scene> scenes)
```
Gets the components requested from the scenes entered.

**Returns List of T -** The found components of the type entered.

### GetComponentFromAllScenes
```csharp
GetComponentFromAllScenes<T>()
```
Gets the components requested from all active scenes.
            
**Returns T -** The first found component of the type entered.
            
### GetComponentsFromAllScenes
``` csharp
GetComponentsFromAllScenes<T>()
```
Gets the components requested from all active scenes.
            
**Returns List of T -** The found components of the type entered.
            
        
The usage of **SceneElly** is like so:

```csharp
[SerializeField] private List<CarSlot> cachedMergeBoardSlots;

public void OnMultiSceneAwake()
{
	cachedMergeBoardSlots = SceneElly.GetComponentsFromAllScenes<CarSlot>();
}
```

Wait a minute...... that's not Awake() xD. That's because in the example above I'm searching across all active scenes using the [GetComponentsFromAllScenes]() method. This method has to go through each scene and as we could be calling it from a scene that is loaded ahead of others we use the MultiSceneAwake listener to ensure all scenes are loaded before the reference is made. Most of the time you will want to be using the inferfaces to implement these when using a multi scene setup with this system. However there are occasions where you can get away with it being in the normal Awake() method. Such as when the object you are getting is in a scene that loads ahead of the scene in question, or is in the same scene. In which case you can do the following:

```csharp
[SerializeField] private List<CarSlot> cachedMergeBoardSlots;

private void Awake()
{
	// Gets fromm the "active" scene, not the scene it is in, unless it is the active scene xD
	cachedMergeBoardSlots = SceneElly.GetComponentsFromScene<CarSlot>();

	// Gets fromm the requested scene, which can be the scene it is in.
	// NOTE: the scene MUST be loaded for the reference to work xD if in doubt use the MultiSceneAwake to ensure this.
	cachedMergeBoardSlots = SceneElly.GetComponentsFromScene<CarSlot>("UI Scene");
}
```

Just to note. I'd recommend avoiding using the All Scenes methods where possible as obviously it is more of a performance hit. Not a noticeable hit, but it is searching every scene you have active, rather than the one you want and doing it a lot can add up xD

## Multi-Scene Listeners

As you have already seem by now, the system uses a selection of interfaces to provide a way around reference issues when loading scenes for the first time. These are only called when the scenes are loaded via the multi scene manager. The order the interfaces gets called is the same as you'd expcet with Awake being first, followed by Enable and then Start:

### IMultiSceneAwake

Is called first out of the interfaces to be called once all scenes are loaded from the multi-scene manager.
```csharp
void OnMultiSceneAwake();
```

### IMultiSceneEnable

Is called second out of the interfaces to be called once all scenes are loaded from the multi-scene manager. 
```csharp
void OnMultiSceneEnable();
```

### IMultiSceneStart

Is called last out of the interfaces to be called once all scenes are loaded from the multi-scene manager. 
```csharp
void OnMultiSceneStart();
```


## Multi Scene Manager

The multi-scene manager is still in its early days. There are a few public methods that you can use should you wish. I however find it easier to just use the awake load option as 99% of the time that is all I need at the moment. The current methods in the manager are:
    
### IsSceneLoaded
```csharp
IsSceneLoaded(string sceneName)
```
Checks whether or not a scene with the name entered is currently loaded.

**Returns: Bool**
    
### IsSceneInGroup
```csharp   
IsSceneInGroup(SceneGroup group, string sceneName)
```
Checks whether or not a scene with the name entered is in the scene group entered.
    
**Returns: Bool**
    
### GetActiveSceneNames
```csharp
GetActiveSceneNames()
```
Gets all the active scene names and returns them.
    
**Returns: List of strings**
    
### SetGroup
```csharp
SetGroup(SceneGroup group)
```
Sets the active scene group to the group you enter into the method.
    
### UnloadAllActiveScenes
Unloads all the active scenes. Not much of a use for this at runtime, but may be handy for editor tools.
    
### UnloadAllAdditiveScenes
Unloads all the additive scenes but leaves the base scene loaded. 
    
### LoadScenes
```csharp
LoadScenes();
LoadScenes(SceneGroup groupToLoad)
```
Loads the scenes either from the scene group entered or from the group currently in the inspector of the multi-scene manager. Loading scenes will override the current scenes that are active.
    
### LoadScenes
```csharp
LoadScenesKeepBase(SceneGroup group)
```
Loads the scenes from the scene group but doesn't change the base scene as long as the base scene is the same in both scene groups. 
    

## Base Multi-Scene Loader

The base multi-scene loader is a base class that you can inherit from to load scene groups from interactable elements in your game. The base class is super simple and has the following
```csharp
loadGroup // Is the scene group that is loaded by the LoadSceneGroup() method if not overridden. 
LoadSceneGroup() // Default logic has the method load the scene group in the normal way. Override to add your own logic here. 
```

# Extensions

Off of the core system there are a few extensions which are separate packages that add some extra stuff that you might not always need into your project. These require the core codebase to work and are:

## URP Cameras
```csharp
MultiScene.Extensions.URP
```
Adds functionality to allow you to use camera stacking across multiple scenes with the URP or HDRP in theory. Note that you can't save the stacking changes. This is a work in progress as more features may be needed that I haven't found yet.

## Do Not Destroy
```csharp
MultiScene.Extensions.DoNotDestroy
```
Adds support for getting components in the persistent scene Do Not Destroy using a spy and an accessor to find the spy.
