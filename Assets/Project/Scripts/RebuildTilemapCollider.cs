using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace LudumDare40 {
	public class RebuildTilemapCollider : MonoBehaviour {

		[SerializeField]
		Grid grid;
		[SerializeField]
		TilemapCollider2D[] colliders;

		// Use this for initialization
		/*
		void Start () {
			for(int index = 0; index < colliders.Length; ++index) {
				Tilemap map = colliders [index].GetComponent<Tilemap> ();
				Destroy (colliders [index]);

				BoundsInt bounds = map.cellBounds;
				foreach (Vector3Int pos in bounds.allPositionsWithin) {
					TileBase tile = map.GetTile<TileBase> (pos);
					if (tile != null) {
						BoxCollider2D replaceCollider = gameObject.AddComponent<BoxCollider2D> ();
						// Not compiling for some reason?
						//replaceCollider.bounds.center = new Vector2((pos.x * grid.cellSize.x), (pos.y * grid.cellSize.y));
						//replaceCollider.bounds.size = new Vector2(grid.cellSize.x, grid.cellSize.y);
					}
				}
			}
		}
		*/

		[ContextMenu("Get Colliders")]
		void GetColliders() {
			grid = GetComponent<Grid> ();
			colliders = GetComponentsInChildren<TilemapCollider2D> (true);
		}
	}
}
