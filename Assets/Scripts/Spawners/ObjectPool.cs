//From https://blogs.msdn.microsoft.com/dave_crooks_dev_blog/2014/07/21/object-pooling-for-unity3d/
/*
 * @Author: David Crook
 *
 * Use the object pools to help reduce object instantiation time and performance
 * with objects that are frequently created and used.
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmptyPoolMode { NullIfEmptyPool, RecycleOldest } //recycle oldest isn't used yet

public class ObjectPool
{
    //the list of objects.
    private List<GameObject> pooledObjects;

    //sample of the actual object to store.
    //used if we need to grow the list.
    private GameObject pooledObj;

    //maximum number of objects to have in the list.
    private int maxPoolSize;

    //initial and default number of objects to have in the list.
    private int initialPoolSize;

    //pool mode determins what to do if there are no objects in Pool.
    private EmptyPoolMode emptyPoolMode;

    /// <summary>
    /// Constructor for creating a new Object Pool.
    /// </summary>
    /// <param name="obj">Game Object for this pool</param>
    /// <param name="initialPoolSize">Initial and default size of the pool.</param>
    /// <param name="maxPoolSize">Maximum number of objects this pool can contain.</param>
    /// <param name="shouldShrink">Should this pool shrink back to the initial size.</param>
    public ObjectPool(GameObject obj, int initialPoolSize, int maxPoolSize, bool shouldShrink) : this(obj, initialPoolSize, maxPoolSize, shouldShrink, EmptyPoolMode.NullIfEmptyPool)
    {

    }

    /// <summary>
    /// Constructor for creating a new Object Pool.
    /// </summary>
    /// <param name="obj">Game Object for this pool</param>
    /// <param name="initialPoolSize">Initial and default size of the pool.</param>
    /// <param name="maxPoolSize">Maximum number of objects this pool can contain.</param>
    /// <param name="shouldShrink">Should this pool shrink back to the initial size.</param>
    public ObjectPool(GameObject obj, int initialPoolSize, int maxPoolSize, bool shouldShrink, EmptyPoolMode emptyPoolMode)
    {
        //instantiate a new list of game objects to store our pooled objects in.
        pooledObjects = new List<GameObject>();
        
        //create and add an object based on initial size.
        for (int i = 0; i < initialPoolSize; i++)
        {

            //instantiate and create a game object with useless attributes.
            //these should be reset anyways.
            GameObject go = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;

            //make sure the object isn't active.
            go.SetActive(false);

            //add the object too our list.
            pooledObjects.Add(go);

            //Don't destroy on load, so
            //we can manage centrally.
            GameObject.DontDestroyOnLoad(go);

            this.emptyPoolMode = emptyPoolMode;

            //store our other variables that are useful.
            this.maxPoolSize = maxPoolSize;
            this.pooledObj = obj;
            this.initialPoolSize = initialPoolSize;

            //are we supposed to shrink?
            if (shouldShrink)
            {

            }
        }
    }

    /// <summary>
    /// Returns an active object from the object pool without resetting any of its values.
    /// You will need to set its values and set it inactive again when you are done with it.
    /// </summary>
    /// <returns>Game Object of requested type if it is available, otherwise null.</returns>
    public GameObject GetObject()
    {
        //iterate through all pooled objects.
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            ///Pure HACK need to figure out how to solve this. PROBLEM
            ///PROBLEM: When a game object that is set to DONotDestroyOnLoad becomes a child of another object, it then can become destroyed. This messes with the PooledObjects
            ///because pooledObjects thinks it has members but when the scene is reloaded the members are NULL instead of the game object which should have not been destroyed
            ///BUT was destroyed because it became a parent of the game object.....
            ///This right here is a hack to get around this.
            if (!this.pooledObjects[i])
            {
                pooledObjects[i] = GameObject.Instantiate(pooledObj, Vector3.zero, Quaternion.identity) as GameObject;
            }
            
            //look for the first one that is inactive.
            if (pooledObjects[i].activeSelf == false)
            {
                //set the object to active.
                pooledObjects[i].SetActive(true);
                //return the object we found.
                return pooledObjects[i];
            }
        }
        //if we make it this far, we obviously didn't find an inactive object.
        //so we need to see if we can grow beyond our current count.
        if (this.maxPoolSize > this.pooledObjects.Count)
        {
            //Instantiate a new object.
            GameObject nObj = GameObject.Instantiate(pooledObj, Vector3.zero, Quaternion.identity) as GameObject;
            //set it to active since we are about to use it.
            nObj.SetActive(true);
            //add it to the pool of objects
            pooledObjects.Add(nObj);
            //return the object to the requestor.
            return nObj;
        }

        //if we made it this far obviously we didn't have any inactive objects
        //we also were unable to grow, so return null as we can't return an object.
        if (emptyPoolMode != EmptyPoolMode.NullIfEmptyPool)
            return null;
        else
            return null;
    }
}
