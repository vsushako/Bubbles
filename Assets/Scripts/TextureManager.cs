using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureManager : MonoBehaviour, ITextureManager {

	int x = 0;
	Color[] _pixels;
	Dictionary<int, List<Texture2D>> _textures;
	float _r;
	float _g;
	float _b;
	int _size = 32;
	int _maxSize = 256;
	int _minSize = 32;
	
	// Use this for initialization
	void Awake () 
	{
		_textures = new Dictionary<int, List<Texture2D>>();

		// В самом начале создаем первые текстуры
		for(int i = _minSize; i <= _maxSize; i *= 2)
			_textures.Add(i, new List<Texture2D>(){ createTexture(i) });

		_pixels = new Color[_size * _size ];

		// Генерим цвет
		_r = Random.Range(0f, 1f);
		_g = Random.Range(0f, 1f);
		_b = Random.Range(0f, 1f);
	}

	void createNextTexture()
	{
		// обнуляемся
		x = 0;
		_size *= 2;
		// Если размер текстуры больше максимального делаем сначала
		if (_size > _maxSize)
			_size = _minSize;

		// Проверяем есть ли старые текстуры и убираем их
		if (_textures.ContainsKey(_size) && _textures[_size].Count > 1)
		{
			Destroy(_textures[_size][0]);
			_textures[_size].RemoveAt(0);
		}

		// Генерим цвет
		_r = Random.Range(0f, 1f);
		_g = Random.Range(0f, 1f);
		_b = Random.Range(0f, 1f);

		_pixels = new Color[_size * _size ];
	}

	Texture2D createTexture(int size)
	{
		var texture = new Texture2D(size, size, TextureFormat.ARGB32, true);

		float r = Random.Range(0f, 1f);
		float g = Random.Range(0f, 1f);
		float b = Random.Range(0f, 1f);

		for (int i = 0; i < size; i++ )
		{
			for (int j = 0; j < size; j++)
			{
				texture.SetPixel(i, j, new Color(r, g, b) );
			}
			// Генерим градиент
			r += 0.005f;
			g += 0.005f;
			b += 0.005f;
		}

		texture.Apply();
		return texture;
	}

	public Texture2D getTexture(int size)
	{
		// отдаем последнюю текстуру
		return  _textures[size][_textures[size].Count - 1];
	}

	// Update is called once per frame
	void Update () 
	{
		// сохряняем в структуре
		if ( x == _size * _size )
		{
			// Создаем новую текстуру
			Texture2D texture = new Texture2D( _size, _size );
			// SetPixels быстрее чем SetPixel
			texture.SetPixels( _pixels );
			texture.Apply();
			_textures[ _size ].Add( texture );

			// Переходим к следующей
			createNextTexture();
		}
		else
		{
			// Проставляем 16 пикселей, что бы не нагружать
			for (int i = 0; i < 16; i++)
			{
				_pixels[x++] = new Color(_r, _g, _b);
				if (x % _size == 0)
				{
					// Генерим градиент
					_r += 0.005f;
					_g += 0.005f;
					_b += 0.005f;
				}
			}
		}
	}
}
