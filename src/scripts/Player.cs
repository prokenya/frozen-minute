using Godot;
using System;

public partial class Player : CharacterBody2D
{	
	const float MIN_MOVE_SPEED = 2.0f;
	public int JumpVelocity = -300;
	public int Speed = 200;
	public float SlipFriction = 1;
	public enum PlayerState 
	{
    Idle,
	lookForward,
    Walking,
	Jump,
    Falling,
	}
	public Camera2D Camera;

	private PlayerState currentState = PlayerState.Idle;
	private Timer CoyoteTimer;
	private bool CanJump = true;
	private bool JumpPressed = false;
	private AnimatedSprite2D sprite;
	private PackedScene scene = ResourceLoader.Load<PackedScene>("res://src/scenes/Snowball.tscn");

	private Timer ReloadTimer;
    public override void _Ready()
    {
		Global.player = this;
		sprite = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
		CoyoteTimer = GetNode<Timer>("%CoyoteTimer");
		ReloadTimer = GetNode<Timer>("%ReloadTimer");
		Camera =  GetNode<Camera2D>("%Camera2D");
    }
	public override void _PhysicsProcess(double delta)
	{
		HandleJump();
		if (!IsOnFloor())
		{
			Velocity += GetGravity() * (float)delta;
		}

		if (Input.IsActionPressed("ui_down")){
			ChangeState(PlayerState.lookForward);
			Position = new Vector2(Position.X,Position.Y + 1);
		}
		float direction = Input.GetAxis("ui_left", "ui_right");
		float targetSpeed = direction * Speed;

		Velocity = new Vector2(
			Mathf.Lerp(Velocity.X, targetSpeed, SlipFriction),
			Velocity.Y
		);

		if (direction != 0)
		{
			ChangeState(PlayerState.Walking);
			sprite.FlipH = direction > 0;
		}
		else
		{
			if (currentState != PlayerState.lookForward)
				ChangeState(PlayerState.Idle);
		}
		if (Mathf.Abs(Velocity.X) < MIN_MOVE_SPEED)
		{Velocity = new Vector2(0, Velocity.Y);}
		// else{
		// 	Velocity = new Vector2(Mathf.MoveToward(Velocity.X,0,Speed), Velocity.Y);
		// 	if (currentState != PlayerState.lookForward){
		// 	ChangeState(PlayerState.Idle);
		// 	}


		MoveAndSlide();
		HandleAnimation();
	}

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if (Input.IsActionJustPressed("LeftMouse") && ReloadTimer.IsStopped()){
			ThrowSnowball();
			ReloadTimer.Start();
		}
    }
	public void ThrowSnowball(){
		Vector2 ScreenCenter = GetViewport().GetVisibleRect().Size / 2;
		Vector2 MousePosition = GetGlobalMousePosition();
		Vector2 Direction = -(GlobalPosition - MousePosition).Normalized();
		Snowball instance = (Snowball)scene.Instantiate();
		instance.LinearVelocity = Direction * 500;
		instance.GlobalPosition = GlobalPosition - new Vector2(0,17);
		Global.CurrentWorld.AddChild(instance);
	}

	private void HandleJump()
	{
		if (!IsOnFloor())
		{
			if (CoyoteTimer.IsStopped() && CanJump){CoyoteTimer.Start();}
		}
		else{CoyoteTimer.Stop();CanJump = true;}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			JumpPressed = true;
		}
		if (Input.IsActionJustReleased("ui_accept"))
		{
			JumpPressed = false;
		}

		if (CanJump && JumpPressed)
		{
			Velocity = new Vector2(Velocity.X, JumpVelocity);
			CoyoteTimer.Stop();
			CanJump = false;
		}
	}
	private void ChangeState(PlayerState newState) {
    currentState = newState;
	}
	private void HandleAnimation(){
	switch (currentState) {
        case PlayerState.Idle:
			sprite.Frame = 0;
            break;
        case PlayerState.Walking:
			sprite.Play("Walk");
            break;
		case PlayerState.lookForward:
			sprite.Play("Forward");
			break;
    }
	}
	private void _CoyoteTimeout(){
		CanJump = false;
	}


}


