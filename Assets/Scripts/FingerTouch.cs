using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FingerTouch : IInput
{
	private readonly List<Touch> _touches = new List<Touch>();
	public IList<Touch> Touches { get { return _touches; } }

	// Update is called once per frame
	public void Check () 
	{
		// Очищаем список прикосновений
		_touches.Clear();

		// Проходим по всем прикосновениям
		foreach (var touch in Input.touches)
		{

			if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
				continue;

			// Добавляем прикосновения
			_touches.Add(new Touch
			{
				Id = touch.fingerId,
				Position = touch.position
			});
		}
	}
}
