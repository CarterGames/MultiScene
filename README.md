# Unity Multi-Scene Workflow
A multi-scene workflow/tool for using multiple scenes in one go in Unity. This is very much a work in progress and far from perfect. But it is how I manage working with multiple scenes together, so having scenes additive to your active. I started this project at work out of curiosity as I found it near to impossible to find information on how to actually makes games that use more than 1 scene per scene/level to operate. 

This repository is my answer to the issue, that I use in my projects currently. It uses a script from my <a href="https://github.com/JonathanMCarter/com.cartergames.tools.jtools">JTools Library</a> <code>SceneElly</code> to get references between scenes and a version of it provided with the repository. This script has methods such as <code>GetComponentFromAllScenes<T>()</code> which can get an instance of a script in any active scene, not just the "Active" scene. 
  
## How To Install ##
Download the latest version via the packages section, clone the repository or just download the repository as a .zip. If downloading a package, you'll just import them into project via the "import custom package" option in Unity. 
  
## How do I use it?
  ### Summary
The currently workflow involves a scriptable object known as a <code>Scene Group</code> which can be used define how scenes are loaded using the <code>MultiSceneManager</code>. Traditional <code>Awake()</code>, <code>OnEnable()</code> & <code>Start()</code> methods don't work as true awake and start for all the scenes you load using the manager. They run whenever the scene they are in is loaded, which is okay until you need to reference between scenes. Because of this we've added out own version implementing <code>IMultiSceneAwake</code>, <code>IMultiSceneEnable</code> & <code>IMultiSceneStart</code> interfaces and having their methods calls run once all the scenes in the scene group have been loaded.

### Preparation
To use the multi scene system, you will need a <code>Scene Group</code> with the necessary scenes you want to load. Once done, you'll need to add the Multi Scene Manager to a GameObject in the scene you want to load from. Normally this will be the scene you have as the base scene in the <code>Scene Group</code> already. 

### Loading Scenes
The Multi-Scene Manager will automatically load the scene group that is assigned in the inspector of the script. Once all the scenes are loaded the listeners will be called in the normal order. 

### Interfaces
The multi-scene manager calls all implementations of each interface in the normal order of Awake, Enable, Start. Each interface has one method such as <code>OnMultiSceneAwake()</code> which is invoked on said call.
  
### Scene Group Inspector
Scene groups can be created via the right-click create menu, under Multi-Scene/Scene Group. When first created you should see a grouping like this.
  
![scenegroup1](https://carter.games/git/multiscene/SceneGroup-01.png)
  
You start be adding the base scene, this is the scene that Unity considers the "Active" scene and is what all object will default spawn to unless you tell them otherwise.
  
![scenegroup2](https://carter.games/git/multiscene/SceneGroup-02.png)
  
Once you've setup the base scene you can define all the scenes to load in an additive form, these will all load at the same time using the Multi Scene Manager in the order defined in the group.
  
![scenegroup4](https://carter.games/git/multiscene/SceneGroup-04.png)

## Issues & Bug Reporting
If you find any issues with the code base or a bug in the project, please do add an issue on GitHub and I will try to resolve it. But remember this is a personal project, so it may be slow xD

## Contribute
At this time I'm not really looking for extra contributors to the project, as at the end of the day this is a learning experience for me. You are welcome to make your own fork of the project and work on our version should you wish. 
