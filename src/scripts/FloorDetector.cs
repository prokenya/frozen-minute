using Godot;
using System;

public partial class FloorDetector : Area2D
{
	public float Friction;
	public override void _Ready()
	{
	}

	private void _BodyShapeEntered(Rid BodyRid,TileMapLayer Body,int BodyShapeIndex,int LocalShapeIndex){
		if (Body is TileMapLayer)
		{
			Vector2I CollidedTileCords = Body.GetCoordsForBodyRid(BodyRid);
			TileData tileData = Body.GetCellTileData(CollidedTileCords);

			if (tileData != null)
        		{
        		    Variant SlipperyVariant = tileData.GetCustomData("Slippery");
        		    float Slippery = (float)SlipperyVariant;

					if (Slippery == 0){Friction = 0.5f;}
					else{Friction = Slippery;}
					Global.player.SlipFriction = Friction;

				}
		}
	}
}
