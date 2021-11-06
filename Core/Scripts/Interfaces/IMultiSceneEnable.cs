/*
 * 
 *  Multi-Scene Workflow
 *							  
 *	Multi-Scene Enable Interface
 *      A interface to implement when you want logic to run second once a scene group has loaded via the multi-scene manager.
 *			
 *  Written by:
 *      Jonathan Carter
 *		
 *	Last Updated: 05/11/2021 (d/m/y)							
 * 
 */

namespace MultiScene.Core
{
    public interface IMultiSceneEnable
    {
        void OnMultiSceneEnable();
    }
}