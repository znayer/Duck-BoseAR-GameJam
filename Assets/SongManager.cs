using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum Direction{
		North, East, South, West
};

public class SongManager : MonoBehaviour {

	public AudioClip song;
	public double BPM; 

	private AudioSource player;
	private double BeatDuration;
	private int BeatCount = 0;

	private Direction[] sequence;

	public Transform head;
	public AudioSource[] voicecues;

	// Orbs
	public Material lightMaterial;
	public Material defMaterial;
	public Renderer rightMark;
	public Renderer leftMark;
	public Renderer upMark;
	public Renderer downMark;
	public Text scoreText;
	private int score = 0;
	
	private int delay;

	private AudioSource sfx;
	public AudioClip[] dings;
	public AudioClip dong;
	// Use this for initialization
	void Awake () {
		sequence = new Direction[4];
		player = gameObject.AddComponent<AudioSource>();
		sfx = gameObject.AddComponent<AudioSource>();
	}

	void OnEnable(){
		player.clip = song;
		BeatDuration = song.frequency * (60/BPM);
		player.Play();
		scoreText.text = "Score: 0";
	}
	
	// Update is called once per frame
	void Update () {
		if((player.timeSamples - delay) / BeatDuration > BeatCount){
			rightMark.material = defMaterial;
	        leftMark.material = defMaterial;
	        upMark.material = defMaterial;
	        downMark.material = defMaterial;
			TickBeat();
			BeatCount++;
		}
	}

	void TickBeat(){
		int phase = BeatCount % 12;
		if (phase < 4){
			// Nothing
			//print("rest");
		}else if(phase < 8){
			int dex = phase % 4;
			sequence[dex] = (Direction)Enum.GetValues(typeof(Direction)).GetValue(UnityEngine.Random.Range(0,4));
			switch(sequence[dex]){
			case Direction.North:
				voicecues[(int)Direction.North].Play();
				upMark.material = lightMaterial;
				break;
			case Direction.East:
				voicecues[(int)Direction.East].Play();
				rightMark.material = lightMaterial;
				break;
			case Direction.South:
				voicecues[(int)Direction.South].Play();
				downMark.material = lightMaterial;
				break;
			case Direction.West:
				voicecues[(int)Direction.West].Play();
				leftMark.material = lightMaterial;
				break;
			}
		}else{
			int dex = phase % 4;
			Direction input;
			float elevation = head.eulerAngles.x;
 			elevation = (elevation > 180) ? elevation - 360 : elevation;
 			float azimuth = head.eulerAngles.y;
 			azimuth = (azimuth > 180) ? azimuth - 360 : azimuth;
			if (elevation < -20f){
				input = Direction.North;
			}else if (elevation > 20f){
				input = Direction.South;
			}else if (azimuth > 0){
				input = Direction.East;
			}else{
				input = Direction.West;
			}
			if(input == sequence[dex]){
				sfx.clip = dings[dex];
				sfx.Play();
				switch(input){
					case Direction.North:
						upMark.material = lightMaterial;
						break;
					case Direction.East:
						rightMark.material = lightMaterial;
						break;
					case Direction.South:
						downMark.material = lightMaterial;
						break;
					case Direction.West:
						leftMark.material = lightMaterial;
						break;
				}
				score += 100;
				scoreText.text = "Score: " + score.ToString();
			}else{
				sfx.clip = dong;
				sfx.Play();
			}
		}
		if (phase == 7){
			delay = song.frequency / 8;
		}else if (phase == 0){
			delay = -song.frequency / 32;
		}
	}
}
