using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorController : MonoBehaviour {

	public static AudioSource audio;
	//0 = regular play, 1 = editing, 2 = testing custom level
	public static int editingMode = 2;

	private static bool loaded;
	public static bool Loaded
	{
		get
		{
			return loaded;
		}
		set
		{
			loaded = value;
		}
	}

	public Slider currentTime;
	public Slider volume;
	public Slider pitch;
	public GameObject mainMenuGUI;
	public GameObject editorGUI;
	public Stage stage;
	public EnemySpawner spawner;
	private bool sliderLocked;
	private static bool wasLoaded;
	public static bool inMenu = true;
	public bool placingEntity;
	public GameObject linePrefab;
	private LineRenderer horizontalPlaceLine, verticalPlaceLine;

	// Use this for initialization
	void Awake () {
		loaded = false;
		audio = GetComponent<AudioSource>();
	}

	void Start()
	{
		horizontalPlaceLine = Instantiate(linePrefab).GetComponent<LineRenderer>();
		verticalPlaceLine = Instantiate(linePrefab).GetComponent<LineRenderer>();
	}

	public void HideLines()
	{
		horizontalPlaceLine.gameObject.SetActive(false);
		verticalPlaceLine.gameObject.SetActive(false);
	}

	public void LoadSong()
	{
		StartCoroutine(StageFileManager.LoadStageSong(StageFileManager.audioFilePath, audio));

		editingMode = 1;
	}

	public void LockSlider()
	{
		sliderLocked = true;
	}

	public void UnlockSlider()
	{
		sliderLocked = false;
		spawner.BeginLevel();
	}

	public void OnVolumeChanged()
	{
		//audio.volume = volume.value;
	}

	public void OnSpeedChanged()
	{
		audio.pitch = pitch.value;
	}

	public void ToEditor()
	{
		mainMenuGUI.SetActive(false);
		editorGUI.SetActive(true);
		inMenu = false;
	}

	public void ToMenu()
	{
		mainMenuGUI.SetActive(true);
		editorGUI.SetActive(false);
		inMenu = true;
	}

	public void DisableEditor()
	{
		mainMenuGUI.SetActive(false);
		editorGUI.SetActive(false);
		editingMode = 0;
		inMenu = false;
	}

	public void OnAdd()
	{
		placingEntity = true;
	}

	public void PlaceEntity()
	{
		Vector3 m = Input.mousePosition;
		m = Camera.main.ScreenToWorldPoint(m);
		m.z = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(editingMode != 0)
		{
			if (!wasLoaded && Loaded)
			{
				audio.volume = Options.musicVolume;
				stage.song = audio;
				Stage.loadingSong = false;
			}
			wasLoaded = loaded;
			if(sliderLocked)
				audio.time = currentTime.value * audio.clip.length;
			if(audio.isPlaying && !sliderLocked)
			{
				currentTime.value = audio.time / audio.clip.length;
			}

			if(Input.GetKeyDown(KeyCode.Escape))
			{
				placingEntity = false;
			}

			if(Input.GetKeyDown(KeyCode.Space))
			{
				if (audio.isPlaying)
				{
					audio.Stop();
					editingMode = 1;
				}
				else
				{
					audio.time = currentTime.value * audio.clip.length;
					audio.Play();
					editingMode = 2;
					spawner.BeginLevel();
				}
			}

			if(placingEntity)
			{
				verticalPlaceLine.gameObject.SetActive(true);
				horizontalPlaceLine.gameObject.SetActive(true);

				Vector3 m = Input.mousePosition;
				m = Camera.main.ScreenToWorldPoint(m);
				m.z = 0;
				verticalPlaceLine.SetPositions(new Vector3[]{m + new Vector3(0, -100, 0), m + new Vector3(0, 100, 0)});
				horizontalPlaceLine.SetPositions(new Vector3[]{m + new Vector3(-100, 0, 0), m + new Vector3(100, 0, 0)});
			}
			else
			{
				HideLines();
			}
		}
	}
}
