using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare40 {
	public class RenderTextureGenerator : MonoBehaviour {
		[System.Serializable]
		public struct Set {
			public Camera renderCamera;
			public Camera maskCamera;
			public RawImage renderImage;
			public RawImage maskImage;
			public RenderTexture renderTexture;
			public RenderTexture maskTexture;
		}

		[SerializeField]
		Set greenLayer;
		[SerializeField]
		Set purpleLayer;
		[SerializeField]
		Set orangeLayer;

		// Use this for initialization
		void Awake () {
			CreateSet (ref greenLayer);
			CreateSet (ref purpleLayer);
			CreateSet (ref orangeLayer);
		}
		
		// Update is called once per frame
		void Update () {
			UpdateSet (ref greenLayer);
			UpdateSet (ref purpleLayer);
			UpdateSet (ref orangeLayer);
		}

		static void UpdateSet(ref Set textureSet) {
			if (textureSet.renderTexture == null) {
				CreateSet (ref textureSet);
			} else if ((textureSet.renderTexture.width != Screen.width) || (textureSet.renderTexture.width != Screen.height)) {
				textureSet.renderTexture.Release ();
				textureSet.maskTexture.Release ();
				CreateSet (ref textureSet);
			}
		}
		static void CreateSet(ref Set textureSet) {
			textureSet.renderTexture = new RenderTexture (Screen.width, Screen.height, 16);
			textureSet.maskTexture = new RenderTexture (Screen.width, Screen.height, 16);

			textureSet.renderCamera.gameObject.SetActive (true);
			textureSet.renderCamera.targetTexture = textureSet.renderTexture;

			textureSet.maskCamera.gameObject.SetActive (true);
			textureSet.maskCamera.targetTexture = textureSet.maskTexture;

			textureSet.renderImage.gameObject.SetActive (true);
			textureSet.renderImage.texture = textureSet.renderTexture;

			textureSet.maskImage.gameObject.SetActive (true);
			textureSet.maskImage.texture = textureSet.maskTexture;
		}
	}
}