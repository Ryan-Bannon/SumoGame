using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    //Controls the audiosource for the click sound effect
    public AudioSource clickSource;
    //Controls the click sound effect
    public AudioClip clickSE;
    //The audiosource for the menuMusic
    public AudioSource menuMusicSource;
    //The menuMusic audio
    public AudioClip menuMusic;
    //Stores the volume of the menuMusic at the start (used to set the volume back after it is lowered by the HTP screen)
    private float normalMMusicVol;
    //The image that shows up for the how to play screen
    public GameObject howToPlayScreen;
    //The image that shows up for the power up screen
    public GameObject powerupScreen;
    //The image that shows up for the controls screen
    public GameObject controlsScreen;
    //Controls how much the sound changes by when you open the HTP menu
    public float soundPercentChange;

    void Start()
    {
        normalMMusicVol = menuMusicSource.volume;
    }
    public void Lvl1()
    {
        clickSource.PlayOneShot(clickSE);
        SceneManager.LoadScene("Lvl1");
    }
    public void Lvl2()
    {
        clickSource.PlayOneShot(clickSE);
        SceneManager.LoadScene("Lvl2");
    }
    public void BackToMenu()
    {
        clickSource.PlayOneShot(clickSE);
        menuMusicSource.volume = normalMMusicVol;
        howToPlayScreen.SetActive(false);
    }
    public void OpenHowToPlay()
    {
        clickSource.PlayOneShot(clickSE);
        menuMusicSource.volume *=  (soundPercentChange/100);
        howToPlayScreen.SetActive(true);

    }
    public void OpenPowerUpScreen()
    {
        clickSource.PlayOneShot(clickSE);
        powerupScreen.SetActive(true);
    }
    public void OpenControlsScreen()
    {
        clickSource.PlayOneShot(clickSE);
        controlsScreen.SetActive(true);
    }
    public void BackToHowToPlay()
    {
        clickSource.PlayOneShot(clickSE);
        if(powerupScreen.activeSelf)
            powerupScreen.SetActive(false);
        else
            controlsScreen.SetActive(false);
    }
}
