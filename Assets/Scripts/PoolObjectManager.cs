using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolObjectManager : MonoBehaviour
{
	public Dictionary<string, List<dynamic>> _objects;
	// Объекты для фабрики
	public GameObject _factoryGameObject;
	private IObjectsFactory _factory;

	public void Pop( string name )
	{
		// Если нет запрашевоемого очереди создаем ее
 		if ( !_objects.ContainsKey( name ) )
			_objects.Add( name, new List<dynamic>() );

		List<dynamic> pool = _objects[ name ];
		GameObject go;

		// проверяем есть ли необходимые объекты
		if ( pool.Count > 0 )
		{
			// Получаем объект из пула
			go = (GameObject)pool[0];

			// Удаляем его там
			pool.RemoveAt(0);

			// Делаем его активным
			go.SetActive(true);
		}
		else
		{
			// Создаем объект
			go = _factory.create( name, this );
		}
	}

	public void Push(GameObject obj)
	{
		// Проверяем есть ли такой тип
		if ( !_objects.ContainsKey( obj.name ) )
		{
			_objects.Add( obj.name, new List<dynamic>() );
		}

		// Добавляем в стек
		_objects[obj.name].Add(obj);
	}

	void Awake()
	{
		// Объявляем основные переменные
		_factory = (IObjectsFactory)_factoryGameObject.GetComponent(typeof(IObjectsFactory));
		_objects = new Dictionary<string, List<dynamic>>();
	}

}
