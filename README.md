# Unity Multi-Scene Workflow
This readme is still a work in progress, so the information on this page is not complete yet, I'm working on it xD

A multi-scene workflow/tool for using multiple scenes in one go in Unity. This is very much a work in progress and far from perfect. But it is how I manage working with multiple scenes together, so having scenes additive to your active. I started this project at work out of curiosity as I found it near to impossible to find information on how to actually makes games that use more than 1 scene per scene/level to operate. 

This repository is my answer to the issue, that I use in my projects currently. It uses a script from my <a href="https://github.com/JonathanMCarter/com.cartergames.tools.jtools">JTools Library</a> <code>SceneElly</code> to get references between scenes and a version of it provided with the repository. This script has methods such as <code>GetComponentFromAllScenes<T>()</code> which can get an instance of a script in any active scene, not just the "Active" scene. 
  
## How To Install ##
Download the latest version via the packages section, clone the repository or just download the repository as a .zip. If downloading a package, you'll just import them into project via the "import custom package" option in Unity. 
  
## How do I use it?
  ### Summary
The currently workflow involves a scriptable object known as a <code>Scene Group</code> which can be used define how scenes are loaded using the <code>MultiSceneManager</code>. Traditional <code>Awake()</code> & <code>Start()</code> methods don't work as true awake and start for all the scenes you load using the manager. They run whenever the scene they are in is loaded, which is okay until you need to reference between scenes. Because of this we've added out own version implementing <code>IMultiSceneAwake</code> & <code>IMultiSceneStart</code> interfaces and having their methods calls run once all the scenes in the scene group have been loaded.
  
### Breakdown
Coming Soon!
