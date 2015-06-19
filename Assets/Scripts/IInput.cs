using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IInput
{
	IList<Touch> Touches { get; }

	void Check();
}
