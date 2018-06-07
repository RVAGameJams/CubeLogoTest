using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorMode { Full, Flat, Mono }

[ExecuteInEditMode]
public class Cube : MonoBehaviour {

	public Vector3 HSV;
	public ColorMode ColorMode;
	public Texture[] Textures = new Texture[6];
	MeshRenderer[] quads = new MeshRenderer[6];
	MaterialPropertyBlock props;

	void Start () {
		quads[0] = transform.Find("Front").GetComponent<MeshRenderer>();
		quads[1] = transform.Find("Right").GetComponent<MeshRenderer>();
		quads[2] = transform.Find("Top").GetComponent<MeshRenderer>();
		quads[3] = transform.Find("Back").GetComponent<MeshRenderer>();
		quads[4] = transform.Find("Left").GetComponent<MeshRenderer>();
		quads[5] = transform.Find("Bottom").GetComponent<MeshRenderer>();

		props = new MaterialPropertyBlock();
	}
	
	void Update() {
		if (quads[0] == null || props == null) { Start(); }
		for (int i = 0; i < quads.Length; i++) {
			quads[i].GetPropertyBlock(props);
			if (Textures[i] != null) props.SetTexture("_ColorMap", Textures[i]);
			props.SetFloat("_Hue", HSV.x);
			props.SetFloat("_Sat", HSV.y);
			props.SetFloat("_Val", HSV.z);
			props.SetFloat("_AddLight", ColorMode == ColorMode.Flat ? 1f : ColorMode == ColorMode.Mono ? -1 : 0.1f);
			quads[i].SetPropertyBlock(props);
		}
	}
}
