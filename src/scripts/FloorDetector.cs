using Godot;
using System;

public partial class FloorDetector : Area2D
{
	public float Friction;
	public override void _Ready()
	{
	}

	private void _BodyShapeEntered(Rid BodyRid,Node2D Body,int BodyShapeIndex,int LocalShapeIndex){
		if (Body is TileMapLayer)
		{
			TileMapLayer tilemap = Body as TileMapLayer;
			Vector2I CollidedTileCords = tilemap.GetCoordsForBodyRid(BodyRid);
			TileData tileData = tilemap.GetCellTileData(CollidedTileCords);

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
