using UnityEngine;
using System.Collections;

public class BubbleController : MonoBehaviour, IFallingObject {

	// Коэффициент скорости передвижения
	float _speed = .5f;
	public int _radius;
	float _hSize;
	public float _wSize;
	PoolObjectManager _pool;
	public ITextureManager _texture;

	public void SetPoolManager(PoolObjectManager pool)
	{
		_pool = pool;
	}

	// Изменяем коэффициент скорости
	public void setSpeed(int speed) 
	{
		_speed = speed;
	}

	// Устанавливаем размер экрана
	public void setScreenSize(float hSize, float wSize)
	{
		_hSize = hSize;
		_wSize = wSize;
	}

	// Добавляем случайный цвет
	public void SetRandomColor()
	{
		renderer.material.color = new Color(
			Random.Range(0f,1f), 
			Random.Range(0f,1f), 
			Random.Range(0f,1f), 1f);
	}

	// Добавляем случайную текстуру
	public void SetRandomTexture()
	{
		// если мы не передали при создании текстуру то делаем просто рандомный цвет
		if (_texture == null)
		{
			SetRandomColor();
			return;
		}

		// Определяем размер объекта
		Texture2D newTexture;

		if ( _radius < _wSize / 4 )
			newTexture = _texture.getTexture(32);
		else if (_radius < _wSize / 3)
			newTexture = _texture.getTexture(64);
		else if (_radius < _wSize / 2)
			newTexture = _texture.getTexture(128);
		else
			newTexture = _texture.getTexture(256);

		// Если текстура поменялась добаляем ее
		if ( renderer.material.mainTexture != newTexture )
			renderer.material.mainTexture = newTexture;
	}

	// Добавляем размер
	public void SetRandomScale(int max)
	{
		// Получаем радиус
		_radius = Random.Range(1, max);
		// Изменяем значения
		transform.localScale = new Vector3(_radius, _radius);
	}

	// Изменяем положение
	public void setPosition(float x, float y)
	{
		// Задаем новую позицию
		transform.position = new Vector2( x, y );
	}

	// Добавляем менеджер текстур
	public void SetTextureManager(ITextureManager tm)
	{
		_texture = tm;
	}

	void OnEnable() 
	{
		// увеличиваем скорость пи новом появлении
		_speed += .1f;
		// Делаем рандомный размер
		SetRandomScale( (int) _wSize );
		// Делаем заливку
		SetRandomTexture();
		// Ставим верхнюю рандомную позицию экрана
		setPosition( 
		            - Random.Range( 
		               - (_wSize - _radius / 2), 
		               (_wSize - _radius / 2) ), 
		            _hSize + _radius / 2 );
	}

	void Awake()
	{
		_hSize = Camera.main.orthographicSize;
		_wSize = (float)Screen.width / Screen.height * Camera.main.orthographicSize;
	}

	public void onTouch()
	{
		if (_pool)
			_pool.Push(this.gameObject);

		Stats.score += Mathf.RoundToInt( _wSize - _radius );

		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Передвигаем объект
		transform.Translate(
			Vector3.down 
			* _speed 
			* (_wSize - _radius) 
			* Time.deltaTime);

		// Если он упал ниже границы делаем неактивным и возвращаем в стек
		if (transform.position.y < - ( _hSize + _radius / 2 ))
		{
			if (_pool)
 				_pool.Push(this.gameObject);

			gameObject.SetActive(false);
		}
	}
}
