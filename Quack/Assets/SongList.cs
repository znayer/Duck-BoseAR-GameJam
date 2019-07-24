using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongList : MonoBehaviour {

	[System.Serializable]
	public struct Song{
		public AudioClip clip;
		public double bpm;
		public double intro;
	}

	public Song[] Songs;
	private int selection = -1;

	public void PlayCurrentSelection(){
		if (selection < 0){
			return;
		}

		SongManager.song = Songs[selection].clip;
		SongManager.BPM = Songs[selection].bpm;
		SongManager.introTime = Songs[selection].intro;

		SceneManager.LoadScene("Main");
	}

	public void SelectSong(int songdex){
		selection = songdex;
	}

}
