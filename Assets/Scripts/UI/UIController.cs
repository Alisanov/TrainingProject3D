using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text healthLabel;
    [SerializeField] private InventoryInterface inventory;
    [SerializeField] private SoundInterface sound;
    [SerializeField] private Text levelEnding;
    [SerializeField] private Vector3 startPositon;
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private AudioSource audioSource2D;
    [SerializeField] private AudioClip saveGame;
    private void Awake()
    {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.AddListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.RemoveListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }
    private void Start()
    {
        OnHealthUpdated();
        
        inventory.gameObject.SetActive(false);
        sound.gameObject.SetActive(false);
        levelEnding.gameObject.SetActive(false);

        sound.PlayMusic();
        sound.UpdateSetting();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isShowing = inventory.gameObject.activeSelf;
            inventory.gameObject.SetActive(!isShowing);
            //sound.gameObject.SetActive(isShowing);
            inventory.Refresh();
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            bool isShowing = sound.gameObject.activeSelf;
            sound.gameObject.SetActive(!isShowing);
            //inventory.gameObject.SetActive(isShowing);
        }    
    }

    private void OnHealthUpdated()
    {
        string message = "ЗДОРОВЬЕ: " + Managers.Player.health + "/" + Managers.Player.maxHealth;
        healthLabel.text = message;
    }

    private void OnLevelComplete()
    {
        StartCoroutine(CompleteLevel());
    }

    private IEnumerator CompleteLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.color = Color.green;
        levelEnding.text = "Уровень пройден!";

        yield return new WaitForSeconds(2);
        Managers.Mission.GoToNext();
    }

    private void OnLevelFailed()
    {
        StartCoroutine(FailedLevel());
    }
    private IEnumerator FailedLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.color = Color.red;
        levelEnding.text = "Уровень не пройден!";

        yield return new WaitForSeconds(2);

        Managers.Player.Respawn();
        Managers.Mission.RestartCurrent();
    }

    public void SaveGame()
    {

        Managers.Player.UpdatePosition(player.transform.position);
        Managers.Data.SaveGameState();
        audioSource2D.PlayOneShot(saveGame);
    }

    public void LoadGame()
    {
        Managers.Data.LoadGameState();
        audioSource2D.PlayOneShot(saveGame);
    }
    private void OnGameComplete()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.color = Color.green;
        levelEnding.text = "Ты прошел игру!";
    }

    private void OnApplicationQuit()
    {
        Managers.Audio.SaveData();
    }

}
