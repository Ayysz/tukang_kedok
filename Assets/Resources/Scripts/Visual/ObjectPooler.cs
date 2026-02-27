using System.Collections.Generic;
using UnityEngine;



	public static class ObjectPooler
	{
		public static Dictionary<string, Component> poolLookup = new Dictionary<string, Component>();
		public static Dictionary<string, Queue<Component>> poolDictionary = new Dictionary<string, Queue<Component>>();

		public static void EnqueueObject<T>(T item, string name) where T : Component
		{
			if (!item.gameObject.activeSelf) { return; }
			//item.transform.position = Vector2.zero;
			poolDictionary[name].Enqueue(item);
			item.gameObject.SetActive(false);
		}
		public static T DequeueObject<T>(string key) where T : Component
		{
			if (!poolDictionary.ContainsKey(key))
			{
				//Debug.LogError($"Pool dictionary does not contain key: {key}");
				return null;
			}
			
			if (poolDictionary[key].TryDequeue(out var item))
			{
				item.gameObject.SetActive(true);
				//Debug.Log($"Dequeued {key} from pool");
				return (T)item;
			}
			
			//Debug.Log($"Pool {key} is empty, creating new instance");
			return (T)EnqueueNewInstanceLangsungPake(poolLookup[key], key);
		}
		public static T EnqueueNewInstance<T>(T item, string key) where T : Component
		{
			T newInstance = Object.Instantiate(item);
			newInstance.gameObject.SetActive(false);
			poolDictionary[key].Enqueue(newInstance);
			return newInstance;
		}
		public static T EnqueueNewInstanceLangsungPake<T>(T item, string key) where T : Component
		{
			T newInstance = Object.Instantiate(item);
			newInstance.gameObject.SetActive(true);
			newInstance.transform.position = Vector2.zero;
			return newInstance;
		}
		public static void Startup()
		{
			poolDictionary.Clear();
			poolLookup.Clear();
			//Debug.Log("ObjectPooler Startup");
		}
		public static void SetupPool<T>(T pooledItemprefab, int poolSize, string dictionaryEntry) where T : Component
		{
			if (poolDictionary.ContainsKey(dictionaryEntry))
			{
				//Debug.LogError("Pool already exists: " + dictionaryEntry);
				return;
			}
			poolDictionary.Add(dictionaryEntry, new Queue<Component>());
			poolLookup.Add(dictionaryEntry, pooledItemprefab);
			for (int i = 0; i < poolSize; i++)
			{
				T pooledInstance = Object.Instantiate(pooledItemprefab);
				pooledInstance.gameObject.SetActive(false);
				poolDictionary[dictionaryEntry].Enqueue((T)pooledInstance);
			}
		}
		
		public static void SetupPool<T>(T pooledItemprefab, int poolSize, string dictionaryEntry,float damageMultiplier) where T : Component
		{
			poolDictionary.Add(dictionaryEntry, new Queue<Component>());
			poolLookup.Add(dictionaryEntry, pooledItemprefab);
			for (int i = 0; i < poolSize; i++)
			{
				T pooledInstance = Object.Instantiate(pooledItemprefab);
				pooledInstance.gameObject.SetActive(false);
				poolDictionary[dictionaryEntry].Enqueue((T)pooledInstance);
			}
		}
	}


