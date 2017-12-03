using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LudumDare40 {
	public class SetupTilemaps : MonoBehaviour {
		[System.Serializable]
		public struct TileMapSet {
			public Tilemap originalWithCollision;
			public Tilemap originalNoCollision;
			public Tilemap oppositeWithCollision;
			public Tilemap oppositeNoCollision;
		}
		[System.Serializable]
		public struct TileSet {
			public TileBase withCollision;
			public TileBase noCollision;
		}
		[SerializeField]
		TileMapSet maps;
		[SerializeField]
		TileSet tiles;

		// Use this for initialization
		void Start () {
			BoundsInt bounds = maps.originalWithCollision.cellBounds;
			foreach (Vector3Int pos in bounds.allPositionsWithin) {
				TileBase originalTile = maps.originalWithCollision.GetTile<TileBase> (pos);
				if (originalTile != null) {
					if (originalTile == tiles.withCollision) {
						// Leave the tile on the original map
						// Add a no-collision tile on the opposite map
						maps.oppositeNoCollision.SetTile(pos, tiles.noCollision);
					} else if (originalTile == tiles.noCollision) {
						// Remove the tile on the original map
						maps.originalWithCollision.SetTile(pos, null);

						// Add a no-collision tile on the original map
						maps.originalNoCollision.SetTile(pos, tiles.noCollision);

						// Add a collision tile on the opposite map
						maps.oppositeWithCollision.SetTile(pos, tiles.withCollision);
					}
				}
			}

			// Activate all maps
			maps.originalWithCollision.gameObject.SetActive (true);
			maps.originalNoCollision.gameObject.SetActive (true);
			maps.oppositeWithCollision.gameObject.SetActive (true);
			maps.oppositeNoCollision.gameObject.SetActive (true);
		}

		[ContextMenu("Optimize")]
		void OptimizeTileMaps() {
			maps.originalWithCollision.CompressBounds ();
			maps.originalNoCollision.CompressBounds ();
			maps.oppositeWithCollision.CompressBounds ();
			maps.oppositeNoCollision.CompressBounds ();
		}
	}
}