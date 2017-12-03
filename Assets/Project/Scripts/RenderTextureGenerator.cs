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
			CreateSet (ref greenLayer, "Green");
			CreateSet (ref purpleLayer, "Purple");
			CreateSet (ref orangeLayer, "Orange");
		}
		
		// Update is called once per frame
		void Update () {
			UpdateSet (ref greenLayer, "Green");
			UpdateSet (ref purpleLayer, "Purple");
			UpdateSet (ref orangeLayer, "Orange");
		}

		static void UpdateSet(ref Set textureSet, string name) {
			if (textureSet.renderTexture == null) {
				CreateSet (ref textureSet, name);
			} else if ((textureSet.renderTexture.width != Screen.width) || (textureSet.renderTexture.width != Screen.height)) {
				textureSet.renderTexture.Release ();
				textureSet.maskTexture.Release ();
				CreateSet (ref textureSet, name);
			}
		}
		static void CreateSet(ref Set textureSet, string name) {
			textureSet.renderTexture = NewTexture("Render, " + name);
			textureSet.maskTexture = NewTexture("Mask, " + name);

			textureSet.renderCamera.gameObject.SetActive (true);
			textureSet.renderCamera.targetTexture = textureSet.renderTexture;

			textureSet.maskCamera.gameObject.SetActive (true);
			textureSet.maskCamera.targetTexture = textureSet.maskTexture;

			textureSet.renderImage.gameObject.SetActive (true);
			textureSet.renderImage.texture = textureSet.renderTexture;

			textureSet.maskImage.gameObject.SetActive (true);
			textureSet.maskImage.texture = textureSet.maskTexture;
		}
		static RenderTexture NewTexture(string name) {
			RenderTexture texture = new RenderTexture (Screen.width, Screen.height, 0, RenderTextureFormat.Default);
			texture.name = name;
			return texture;
		}
	}
}