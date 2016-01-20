using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler {
	private List<SceneState> startState;
	private List<SceneState> matchState;
	private List<SceneState> campaignState;
	private List<SceneState> tournamentState;

	public SceneHandler() {
		startState = new List<SceneState> { new SceneState (1, 0),
											new SceneState (1, 1) };

		matchState = new List<SceneState> { new SceneState (1, 2, -1, -1, 0, 1),
											new SceneState (2, 0) };

		campaignState = new List<SceneState> { new SceneState (0),
											   new SceneState (1) };

		tournamentState = new List<SceneState> { new SceneState (0),
												 new SceneState (1) };
	}
}
