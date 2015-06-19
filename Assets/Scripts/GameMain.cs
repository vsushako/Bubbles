using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMain : MonoBehaviour {
	public GameObject _poolManagerGameObject;
	public GameObject _scorePanel;
	Text _scoreText;

	public GameObject _timePanel;
	Text _timeText;
	
	private PoolObjectManager _poolManager;
	private IInput _input;
	public int _times = 5;
	public float _time = 2;

	// Use this for initialization
	void Start () 
	{
		_poolManager = (PoolObjectManager) _poolManagerGameObject.GetComponent("PoolObjectManager");

		#if  (UNITY_IPHONE || UNITY_ANDROID)
		_input = new FingerTouch();
		#else
		_input = new MouseTouch();
		#endif

		InvokeRepeating("createNewBubble", _time, _time);

		if (_scorePanel != null)
			_scoreText = _scorePanel.GetComponent<Text>();

		if (_timePanel != null)
			_timeText = _timePanel.GetComponent<Text>();
	}

	void createNewBubble()
	{
		_poolManager.Pop("Bubble");
		if ( --_times == 0 )
		{
			_times = 5;
			_time -= .1f;

			if (_time <= 0)
				_time = 0.1f;

			CancelInvoke();
			InvokeRepeating("createNewBubble", _time, _time);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		Stats.time += Time.deltaTime;

		if (_timeText != null)
			_timeText.text = Mathf.RoundToInt( Stats.time ).ToString();

		if (_scoreText != null)
			_scoreText.text = Stats.score.ToString();

		// Получаем клики / прикосновения
		_input.Check();
		int touchCount = _input.Touches.Count;

		if ( touchCount > 0)
		{
			// Проверяем все клики
			for (int i = 0; i < touchCount; i++)
			{
				// Переводим точку касания
				Vector3 wp = Camera.main.ScreenToWorldPoint(_input.Touches[i].Position);
				Vector2 touchPos  = new Vector2(wp.x, wp.y);
				Collider[] obj = Physics.OverlapSphere( touchPos, 1 );

				int objLength = obj.Length;

				if ( objLength == 0 )
					continue;

				// Если хоть по одному попали убираем его
				for(int j = 0; j < objLength; j++ )
					if ( obj[j] != null && obj[j].gameObject.tag == "fallingObject" )
					{
						// отправляем объекту что по нему жмакнули
						IFallingObject script = (IFallingObject)obj[j].GetComponent(typeof(IFallingObject));
						if ( script != null)
							script.onTouch();				
					}
			}
		}
	}
}
