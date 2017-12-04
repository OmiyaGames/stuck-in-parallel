using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare40 {
	public class RenderTextureGenerator : MonoBehaviour {

		static readonly Dictionary<Universe, Textures> CachedTextures = new Dictionary<Universe, Textures>();
		static int LastScreenWidth = 0;
		static int LastScreenHeight = 0;

		[System.Serializable]
		public class Set {
			public Universe forColor;
			public Camera renderCamera;
			public Camera maskCamera;
			public RawImage renderImage;
			public RawImage maskImage;
		}

		struct Textures {
			public RenderTexture render;
			public RenderTexture mask;
		}

		[SerializeField]
		Set greenLayer;
		[SerializeField]
		Set purpleLayer;
		[SerializeField]
		Set orangeLayer;

		Set[] allSets = null;

		// Use this for initialization
		void Awake () {
			allSets = new Set[] { greenLayer, purpleLayer, orangeLayer };
			if (CachedTextures.Count <= 0) {
				PopulateCachedTextures (allSets);
				LastScreenWidth = Screen.width;
				LastScreenHeight = Screen.height;
			} else if (IsWindowDimensionDifferent == true) {
				LastScreenWidth = Screen.width;
				LastScreenHeight = Screen.height;
			}
			foreach (Set layer in allSets) {
				UpdateSet (layer);
			}
		}
		
		// Update is called once per frame
		void Update () {
			if (IsWindowDimensionDifferent == true) {
				PopulateCachedTextures (allSets);
				foreach (Set layer in allSets) {
					UpdateSet (layer);
				}
				LastScreenWidth = Screen.width;
				LastScreenHeight = Screen.height;
			}
		}
		static void PopulateCachedTextures(Set[] allSets) {
			foreach (Set layer in allSets) {
				if (CachedTextures.ContainsKey (layer.forColor) == true) {
					CachedTextures [layer.forColor].render.Release ();
					CachedTextures [layer.forColor].render.width = Screen.width;
					CachedTextures [layer.forColor].render.height = Screen.height;
					CachedTextures [layer.forColor].mask.Release ();
					CachedTextures [layer.forColor].mask.width = Screen.width;
					CachedTextures [layer.forColor].mask.height = Screen.height;
				} else {
					Textures textureSet = new Textures ();
					textureSet.render = NewTexture ("Render, " + layer.forColor.ToString ());
					textureSet.mask = NewTexture ("Render, " + layer.forColor.ToString ());
					CachedTextures.Add (layer.forColor, textureSet);
				}
			}
		}
		static bool IsWindowDimensionDifferent {
			get {
				return (Screen.width != LastScreenWidth) || (Screen.height != LastScreenHeight);
			}
		}
		static void UpdateSet(Set textureSet) {
			textureSet.renderCamera.gameObject.SetActive (true);
			textureSet.renderCamera.targetTexture = CachedTextures[textureSet.forColor].render;

			textureSet.maskCamera.gameObject.SetActive (true);
			textureSet.maskCamera.targetTexture = CachedTextures[textureSet.forColor].mask;

			textureSet.renderImage.gameObject.SetActive (true);
			textureSet.renderImage.texture = CachedTextures[textureSet.forColor].render;

			textureSet.maskImage.gameObject.SetActive (true);
			textureSet.maskImage.texture = CachedTextures[textureSet.forColor].mask;
		}
		static RenderTexture NewTexture(string name) {
			RenderTexture texture = new RenderTexture (Screen.width, Screen.height, 0, RenderTextureFormat.Default);
			texture.name = name;
			texture.useMipMap = false;
			texture.autoGenerateMips = false;
			return texture;
		}
	}
}