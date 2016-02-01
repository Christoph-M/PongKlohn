using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour {

	private MasterScript masterScript;

	void Start() {
		masterScript = this.GetComponent<MasterScript> ();
	}

	public IEnumerator LoadMenu(int menuL, int menuUL)
	{
		masterScript.LoadScene (this.GetScene(menuL));

		yield return new WaitUntil(() => SceneManager.GetSceneByName(this.GetScene(menuL)).isLoaded);

		masterScript.UnloadScene (this.GetScene(menuUL));
	}

	public IEnumerator StartGame(int sceneL, int sceneUL) {
		masterScript.SetInMatch(true);

		masterScript.LoadScene (this.GetScene(sceneL));

		yield return new WaitUntil(() => SceneManager.GetSceneByName(this.GetScene(sceneL)).isLoaded);

		masterScript.UnloadScene (this.GetScene(sceneUL));
	}

	public IEnumerator EndGame(int scene) {
		masterScript.SetInMatch(false);

		masterScript.LoadScene (this.GetScene(scene));

		yield return new WaitUntil(() => SceneManager.GetSceneByName(this.GetScene(scene)).isLoaded);

		masterScript.UnloadScene (this.GetScene((int)MasterScript.Scene.gameScene));
	}
	
	public void LoadMatchSelection()
	{
//		SceneManager.LoadScene (Scenes.UIScene, LoadSceneMode.Single);
//		Tools.LoadMachtElements(List Players);
		//SetActiveUI(UIs.MatchCharUI);??
		//return UIs.MatchCharUI;??
	}
	
	public void LoadMacht(int player1 , int player2, int inputp1 , int inputp2)
	{
//		SceneManager.LoadScene (Scenes.GameScene, LoadSceneMode.Single);
//		Tools.LoadMachtElements(player1 , Player2 ,kristals1 ,kristal2 ,input1 ,input2);
		//SetActiveUI(UIs.MachtUI);??
		//return UIs.MachtUI;??	
	}
	
	public void LoadStorryModeCharacterSelection(int Player1, int input, int kristall)
	{
//		SceneManager.LoadScene (Scenes.UIScene, LoadSceneMode.Single);
//		Tools.LoadMachtElements(player1 , Player2);
		//SetActiveUI(UIs.SinglePlayerCharUI);??
		//return UIs.SinglePlayerCharUI;??	
	}
	
	public void LoadStorryModeMap()
	{
//		SceneManager.LoadScene (Scenes.UIScene, LoadSceneMode.Single);
//		Tools.LoadMachtElements(player1,Kristal1);
		//SetActiveUI(UIs.SinglePlayerMapUI);??
		//return UIs.SinglePlayerMapUI;??	
	}
	
	public void LoadStorryModeMatch(int Player1, int Player2)
	{
//		SceneManager.LoadScene (Scenes.GameScene, LoadSceneMode.Single);
//		Tools.LoadMachtElements(player1 , Player2 ,kristals1 ,kristal2 ,input1 ,input2);
		//SetActiveUI(UIs.SinglePlayerUI);??
		//return UIs.SinglePlayerUI;??
	}
	
	public void LoadStorryModeGameOver(int Player1, int Player2)
	{
//		SceneManager.LoadScene (Scenes.GameScene, LoadSceneMode.Single);
//		Tools.LoadMachtElements(player1 , Player2);
		//SetActiveUI(UIs.SinglePlayerGameOverUI);??
		//return UIs.SinglePlayerGameOverUI;??
	}
	
	public void LoadMatchGameOver(int Player1, int Player2)
	{
//		SceneManager.LoadScene (Scenes.GameScene, LoadSceneMode.Single);
//		Tools.LoadMachtElements(player1 , Player2 );
		//SetActiveUI(UIs.MachtGameOverUI);??
		//return UIs.MachtGameOverUI;??
	}

	public string GetScene(int i) {
		return masterScript.scenes [i];
	}
	
	enum UIs
	{
		StartUI ,MainUI ,SinglePlayerCharUI ,SinglePlayerUI ,SinglePlayerMapUI ,SinglePlayerGameOverUI ,MatchCharUI,MachtUI ,MachtGameOverUI
	};
	
	enum Scenes
	{
		MasterScene,UIScene,GameScene
	};
}
