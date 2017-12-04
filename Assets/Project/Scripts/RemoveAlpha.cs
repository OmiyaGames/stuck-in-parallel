using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare40 {
	public class RemoveAlpha : MonoBehaviour {

		// After camera is rendered, this clears alpha channel of it's
		// render texture to pure white.
		[SerializeField]
		Material mat = null;

		void OnPostRender() {
			GL.PushMatrix ();
			GL.LoadOrtho ();
			for (var i = 0; i < mat.passCount; ++i) {
				mat.SetPass (i);
				GL.Begin (GL.QUADS);
				GL.Vertex3 (0, 0, 0.1f);
				GL.Vertex3 (1, 0, 0.1f);
				GL.Vertex3 (1, 1, 0.1f);
				GL.Vertex3 (0, 1, 0.1f);
				GL.End ();
			}
			GL.PopMatrix ();
		}

	}
}
