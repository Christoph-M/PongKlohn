using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneState {
	private int activeScene = -1;
	private int activeUI = -1;
	private int nextScene = -1;
	private int altScene = -1;

	private int player1 = -1;
	private int player2 = -1;
	private int crystal1 = -1;
	private int crystal2 = -1;

	public SceneState(int aScene, int aUI = -1, int nextS = -1, int altS = -1, int p1 = -1, int p2 = -1, int c1 = -1, int c2 = -1){
		activeScene = aScene;
		activeUI = aUI;
		nextScene = nextS;
		altScene = altS;
		player1 = p1;
		player2 = p2;
		crystal1 = c1;
		crystal2 = c2;
	}

	public void SetState(int aScene, int aUI, int p1, int p2, int c1, int c2, int nextS, int altS) {
		activeScene = aScene;
		activeUI = aUI;
		nextScene = nextS;
		altScene = altS;
		player1 = p1;
		player2 = p2;
		crystal1 = c1;
		crystal2 = c2;
	}

	public void SetStateP1(int aScene, int aUI, int p1, int c1, int nextS, int altS) {
		activeScene = aScene;
		activeUI = aUI;
		nextScene = nextS;
		altScene = altS;
		player1 = p1;
		crystal1 = c1;
	}

	public void SetStateP2(int aScene, int aUI, int p2, int c2, int nextS, int altS) {
		activeScene = aScene;
		activeUI = aUI;
		nextScene = nextS;
		altScene = altS;
		player2 = p2;
		crystal2 = c2;
	}
}
