using Godot;
public partial class Snowball : RigidBody2D
{
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _Entered(Rid BodyRid,TileMapLayer Body,int BodyShapeIndex,int LocalShapeIndex)
	{
		if (Body is TileMapLayer)
		{
			Vector2I CollidedTileCords = Body.GetCoordsForBodyRid(BodyRid);
			TileData tileData = Body.GetCellTileData(CollidedTileCords);

			if (tileData != null)
        		{
        		    Variant SlipperyVariant = tileData.GetCustomData("icicle");
        		    int icicleid = (int)SlipperyVariant;
					if (icicleid != 0){
						Body.SetCell(CollidedTileCords);
						Global.CurrentWorld.SpawnIcicle((Vector2I)Body.MapToLocal(CollidedTileCords),icicleid-1);
						QueueFree();
					}


				}
		}
		QueueFree();
		// else{
		// 	QueueFree();
		// }
	}
}
