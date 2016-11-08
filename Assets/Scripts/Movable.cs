﻿using UnityEngine;
using System.Collections;

public class Movable : TinyObject
{

	protected bool facingRight = true;

	protected void Flip()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		facingRight = theScale.x > 0;
	}

	protected bool ShouldFlip(float h)
	{
		return h < 0 && facingRight || h > 0 && !facingRight;
	}

	protected void FaceRight()
	{
		if (!facingRight)
			Flip();
	}

	protected void FaceLeft()
	{
		if (facingRight)
			Flip();
	}
}