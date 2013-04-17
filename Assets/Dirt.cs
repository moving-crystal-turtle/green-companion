using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dirt : MonoBehaviour
{
	HashSet<GameObject> adjacentDirtObjects;
	
	public static float EXTENT = 1.0f;
	
	float lastLeechTime;

	Dictionary<Nutrient, int> nutrients;
	
	bool plantable;
	
	GameObject plantObject;
	
	Dirt()
	{
		adjacentDirtObjects = new HashSet<GameObject>();
		plantable = false;
	}
	
	public void AddAdjacentDirtObject(GameObject adjacentDirtObject)
	{
		adjacentDirtObjects.Add(adjacentDirtObject);
	}
	
	public float Consume(Nutrient nutrient, int quantity)
	{		
		nutrients[nutrient] = nutrients[nutrient] - quantity;
		
		if (nutrients[nutrient] < 0)
		{
			float unavailableQuantity = -nutrients[nutrient];
			nutrients[nutrient] = 0;
			
			return quantity - unavailableQuantity;
		}
		
		return quantity;
	}
	
	public Dictionary<Nutrient, int> GetNutrients()
	{
		return nutrients;
	}
	
	public HashSet<GameObject> GetAdjacentDirtObjects()
	{
		return adjacentDirtObjects;
	}
	
	public GameObject GetPlantObject()
	{
		return plantObject;
	}
	
	public bool IsPlantable()
	{
		return plantable;	
	}
	
	public void SetPlantObject(GameObject plantObject)
	{
		this.plantObject = plantObject;

		Vector3 plantPosition = transform.position;
		plantPosition.z = -2.0f;
		plantObject.transform.position = plantPosition;
	}
	
	public float Provide(Nutrient nutrient, int quantity)
	{
		nutrients[nutrient] = nutrients[nutrient] + quantity;
		
		if (nutrients[nutrient] > 100)
		{
			float unusableQuantity = nutrients[nutrient] - 100;
			nutrients[nutrient] = 100;
			
			return quantity - unusableQuantity;
		}
		
		return quantity;
	}
	
	public void SetPlantable()
	{
		if (!plantable)
		{
			plantable = true;
			
			/*Vector3 position = transform.position;
			position.z = -9.0f;
			Instantiate(GameObject.Find("Lamp"), position, Quaternion.identity);*/
		}
	}

	void Start()
	{
		lastLeechTime = Time.timeSinceLevelLoad;
		nutrients = new Dictionary<Nutrient, int>();
		
		nutrients[Nutrient.H2O] = 100;
		nutrients[Nutrient.N] = 100;
	}
	
	void Update()
	{
		if (Time.timeSinceLevelLoad - lastLeechTime > 1.0f)
		{
			lastLeechTime = Time.timeSinceLevelLoad;
			
			List<Nutrient> keys = new List<Nutrient>(nutrients.Keys);
			foreach (Nutrient nutrient in keys)
			{
				nutrients[nutrient] -= 1;
				if (nutrients[nutrient] < 0)
				{
					nutrients[nutrient] = 0;
				}
			}
		}
	}
}
