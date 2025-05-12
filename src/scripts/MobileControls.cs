using Godot;
using System;

public partial class MobileControls : HBoxContainer
{
    private void _LeftDown(){
    	SimulateAction("ui_left",true);
    }
    private void _LeftUp(){
    	SimulateAction("ui_left",false);
    }
    private void _RightDown(){
    	SimulateAction("ui_right",true);
    }
    private void _RightUp(){
    	SimulateAction("ui_right",false);

    }
    private void _Down(){
    	SimulateAction("ui_down",true);

    }
    private void _Up(){
    	SimulateAction("ui_down",false);

    }
	private void _Jump(){
    	SimulateAction("ui_accept",true);

    }
	private void _JumpUp(){
    	SimulateAction("ui_accept",false);

    }
	public async void _Back()
	{
	    SimulateAction("ui_cancel", true);
	    await ToSignal(GetTree(), "process_frame");
	    SimulateAction("ui_cancel", false);
	}
	private void SimulateAction(string actionName, bool pressed)
	{
		// GD.Print($"{actionName} -> {pressed}");
	    var ev = new InputEventAction
	    {
	        Action = actionName,
	        Pressed = pressed
	    };
	    Input.ParseInputEvent(ev);
	}
}
