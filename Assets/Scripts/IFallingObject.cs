using UnityEngine;
using System.Collections;

public interface IFallingObject 
{	
	void SetRandomColor();
	void SetRandomTexture();
	void SetRandomScale(int max);
	void setPosition(float x, float y);
	void setSpeed(int speed);
	void setScreenSize(float hSize, float wSize);
	void SetPoolManager(PoolObjectManager pool);
	void SetTextureManager(ITextureManager tm);
	void onTouch();
}
