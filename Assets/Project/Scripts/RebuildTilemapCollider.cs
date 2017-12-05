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

		// BUG FIX: TileMapCollider2D does not work on builds for an unknown reason.
		#if !UNITY_EDITOR
		void Start () {
			for(int index = 0; index < colliders.Length; ++index) {
				Tilemap map = colliders [index].GetComponent<Tilemap> ();
				colliders [index].enabled = false;

				BoundsInt bounds = map.cellBounds;
				foreach (Vector3Int pos in bounds.allPositionsWithin) {
					TileBase tile = map.GetTile<TileBase> (pos);
					if (tile != null) {
						BoxCollider2D replaceCollider = map.gameObject.AddComponent<BoxCollider2D> ();
						replaceCollider.offset = new Vector2(
							(pos.x * grid.cellSize.x) + (grid.cellSize.x / 2f),
							(pos.y * grid.cellSize.y) + (grid.cellSize.y / 2f));
		 				replaceCollider.size = new Vector2(grid.cellSize.x, grid.cellSize.y);
					}
				}
			}
		}
		#else
		IEnumerator Start () {
			for(int index = 0; index < colliders.Length; ++index) {
				colliders [index].enabled = false;
				yield return null;
				yield return null;
				colliders [index].enabled = true;
			}
		}
		#endif

		[ContextMenu("Get Colliders")]
		void GetColliders() {
			grid = GetComponent<Grid> ();
			colliders = GetComponentsInChildren<TilemapCollider2D> (true);
		}
	}
}
