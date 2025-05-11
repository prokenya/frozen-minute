using Godot;
using System;

public partial class Icicle : RigidBody2D
{
	private Sprite2D sprite;
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("%Sprite2D");
	}
	
	public void setFrame(int Frame){
		sprite.Frame = Frame;
	}
	private void _Entered(Rid BodyRid,Node2D Body,int BodyShapeIndex,int LocalShapeIndex)
	{
		if (Body is TileMapLayer)
		{	
			TileMapLayer tilemap = Body as TileMapLayer;
			Vector2I CollidedTileCords = tilemap.GetCoordsForBodyRid(BodyRid);
			TileData tileData = tilemap.GetCellTileData(CollidedTileCords);

			if (tileData != null)
        		{
        		    bool ice = (bool)tileData.GetCustomData("ice");
					if(ice){
						// tilemap.SetCell(CollidedTileCords, -1);
						// tilemap.EraseCell(CollidedTileCords);
						tilemap.SetCellsTerrainConnect([CollidedTileCords],0,-1,false);
						// tilemap.SetCellsTerrainConnect([CollidedTileCords - new Vector2I(1,)],0,-1,true);

					}
					else{
						QueueFree();
					}
					

				}
		}
		else if (Body.IsInGroup("player")){
			Global.gui._Exit();
		}
		else{
			QueueFree();
		}
	}

}
