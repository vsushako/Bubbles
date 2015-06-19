using UnityEngine;
using System.Collections;

public interface IObjectsFactory
{
	GameObject create(string name, PoolObjectManager pool);		
}