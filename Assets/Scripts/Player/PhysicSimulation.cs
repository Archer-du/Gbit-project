

//private void PhysicSimulation()
//{
//	//onGroundCheckBack = Physics2D.Raycast(transform.position, Vector2.down, lowerDistance, groundLayer);
//	//onGroundCheckFore = Physics2D.Raycast(new Vector2(transform.position.x + 1, transform.position.y), Vector2.down, lowerDistance, groundLayer);

//	verticalVelocity -= gravity * gravityScale * Time.deltaTime;

//	if (Physics2D.OverlapBox(feet.position, feet.localScale, 0, filter, result) > 0 && verticalVelocity < 0)
//	{
//		verticalVelocity = 0;
//		Vector2 surface = Physics2D.ClosestPoint(transform.position, result[0]) + Vector2.up * box.size.y / 2;
//		transform.position = new Vector3(transform.position.x, surface.y, 0);
//		state.onGround = true;
//	}
//	else
//	{
//		state.onGround = false;
//	}
//	if (state.isblocked)
//	{
//		horizontalVelocity = 0;
//	}

//	transform.Translate(Vector2.right * Time.deltaTime * horizontalVelocity * x);
//	transform.Translate(Vector2.up * Time.deltaTime * verticalVelocity);
//}
