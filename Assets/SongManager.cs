using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
	public AudioClip[] voicecues;
	private AudioSource voice;
	private int delay;
	// Use this for initialization
	void Start () {
		sequence = new Direction[4];
		player = gameObject.AddComponent<AudioSource>();
		player.clip = song;
		BeatDuration = song.frequency * (60/BPM);
		player.Play();
		voice = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if((player.timeSamples - delay) / BeatDuration > BeatCount){
			TickBeat();
			BeatCount++;
		}
	}

	void TickBeat(){
		int phase = BeatCount % 12;
		if (phase < 4){
			print("rest");
		}else if(phase < 8){
			int dex = phase % 4;
			sequence[dex] = (Direction)Enum.GetValues(typeof(Direction)).GetValue(UnityEngine.Random.Range(0,4));
			switch(sequence[dex]){
			case Direction.North:
				voice.clip = voicecues[(int)Direction.North];
				voice.panStereo = 0.0f;
				voice.Play();
				break;
			case Direction.East:
				voice.clip = voicecues[(int)Direction.East];
				voice.panStereo = 1.0f;
				voice.Play();
				break;
			case Direction.South:
				voice.clip = voicecues[(int)Direction.South];
				voice.panStereo = 0.0f;
				voice.Play();
				break;
			case Direction.West:
				voice.clip = voicecues[(int)Direction.West];
				voice.panStereo = -1.0f;
				voice.Play();
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
			print(input == sequence[dex]);
		}
		if (phase == 7){
			delay = song.frequency / 8;
		}else if (phase == 0){
			delay = 0;
		}
	}
}
