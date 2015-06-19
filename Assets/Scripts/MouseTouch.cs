using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseTouch : IInput
{	
	private readonly List<Touch> _touches = new List<Touch>();
	public IList<Touch> Touches { get { return _touches; } }
	
	// Update is called once per frame
	public void Check () 
	{
		// Очищаем список кликов
		_touches.Clear();

		// Если не левой клавишей то мимо
		if (!Input.GetMouseButton(0)) 
			return;

		// Добавляем клик
		var touch = new Touch
		{
			Id = 1,
			Position = new Vector3(
				Input.mousePosition.x, 
				Input.mousePosition.y, 
				Input.mousePosition.z)
		};

		_touches.Add(touch);
	}
}
