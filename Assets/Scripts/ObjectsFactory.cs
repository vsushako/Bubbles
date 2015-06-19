using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsFactory : MonoBehaviour, IObjectsFactory {

	public Dictionary<string, GameObject> _prefab;

	// Объекты для текстур
	public GameObject _textureGameObject;
	private ITextureManager _texture;

	public string _url;
	public int _version = 1;

	// Создаем обекты 
	public GameObject create(string name, PoolObjectManager pool)
	{
		if (!_prefab.ContainsKey(name))
			return null;

		GameObject newgo = (GameObject)Instantiate (_prefab[name]);
		newgo.name = name;

		// При создании передаем ссылку на пул и на генератор текстур
		IFallingObject script = (IFallingObject)newgo.GetComponent(typeof(IFallingObject));
		script.SetPoolManager(pool);
		
		if (_texture != null)
			script.SetTextureManager(_texture);
		
		return newgo;
	}

	// загрузка из бандла
	IEnumerator DownloadAndCache ()
	{
		while (!Caching.ready)
			yield return null;
		
		using(WWW www = WWW.LoadFromCacheOrDownload (_url, _version))
		{
			yield return www;
			if (www.error != null)
				Debug.LogError("WWW download had an error:" + www.error);

			AssetBundle bundle = www.assetBundle;
			Object o = bundle.mainAsset;
			// Добавляем объект
			_prefab.Add(o.name, (GameObject)o);
			// Очищаем память
			bundle.Unload(false);
		} 
	}

	void Awake()
	{
		_url = @"file://" + @Application.dataPath + @"\Cache\New Resource.unity3d";

		_prefab = new Dictionary<string, GameObject>();
		StartCoroutine (DownloadAndCache());

		if (_textureGameObject != null)
			_texture = (ITextureManager)_textureGameObject.GetComponent(typeof(ITextureManager));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
