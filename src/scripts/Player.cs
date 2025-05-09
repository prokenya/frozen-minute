using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public int JumpVelocity = -300;
	public int Speed = 100;

	private bool _isFreezed;
	public bool IsFreezed
	{
	    get { return _isFreezed; }
	    set
	    {
	        _isFreezed = value;
			if (_isFreezed){ProcessMode = ProcessModeEnum.Disabled;}
			else{ProcessMode = ProcessModeEnum.Inherit;}
	    }
	}


	public enum PlayerState 
	{
    Idle,
	lookForward,
    Walking,
    Jumping,
	}
	private PlayerState currentState = PlayerState.Idle;

	private Timer CoyoteTimer;
	private bool CanJump = true;
	private AnimatedSprite2D sprite;

    public override void _Ready()
    {
		Global.player = this;
		sprite = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
		CoyoteTimer = GetNode<Timer>("%CoyoteTimer");
    }
	public override void _PhysicsProcess(double delta)
	{

		if (!IsOnFloor())
		{
			Velocity += GetGravity() * (float)delta;
			if (CoyoteTimer.IsStopped() && CanJump)
				CoyoteTimer.Start();
		}else{CoyoteTimer.Stop();CanJump = true;}

		if (Input.IsActionJustPressed("ui_accept") && CanJump)
		{
			Velocity = new Vector2(Velocity.X, JumpVelocity);
			CoyoteTimer.Stop();
			CanJump = false;

		}

		if (Input.IsActionPressed("ui_down")){
			ChangeState(PlayerState.lookForward);
		}
		float direction = Input.GetAxis("ui_left","ui_right");
		if (direction != 0){
			ChangeState(PlayerState.Walking);
			sprite.FlipH = direction >0;
			Velocity = new Vector2(direction * Speed,Velocity.Y);
		}
		else{
			Velocity = new Vector2(Mathf.MoveToward(Velocity.X,0,Speed), Velocity.Y);
			if (currentState != PlayerState.lookForward){
			ChangeState(PlayerState.Idle);
			}

		}

		MoveAndSlide();
		HandleAnimation();
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


