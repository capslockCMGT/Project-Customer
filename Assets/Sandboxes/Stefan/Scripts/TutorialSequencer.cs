using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequencer : MonoBehaviour
{
    //this is just an example, you can create as many fields as you want
    [SerializeField] Image panel1;

    void Interface(GameObject playerCar)
    {

        PlayerController[] players = playerCar.GetComponentsInChildren<PlayerController>();

        PlayerController driver = players.FirstOrDefault(p => p.Player == 0);
        PlayerController passenger = players.FirstOrDefault(p => p.Player == 1);

        // DRIVER INPUTS 

        driver.BrakePressed.AddListener(() => {
            //write your code here

            //just an example
            FadePanelAfterSeconds(panel1, 5, 0);

            //write your code here
            //this stops the input to run the code above again
            driver.BrakePressed.RemoveAllListeners();
        });

        driver.GasPressed.AddListener(() =>
        {
            
            driver.GasPressed.RemoveAllListeners();
        });

        driver.ItemGrab.AddListener(() =>
        {

            driver.ItemGrab.RemoveAllListeners();
        });

        driver.ItemInteract.AddListener(() =>
        {

            driver.ItemInteract.RemoveAllListeners();
        });


        // PASSENGER INPUTS

        passenger.BrakePressed.AddListener(() => {
            //write your code here

            //write your code here
            //this stops the input to run the code above again
            passenger.BrakePressed.RemoveAllListeners();
        });

        passenger.GasPressed.AddListener(() =>
        {

            passenger.GasPressed.RemoveAllListeners();
        });

        passenger.ItemGrab.AddListener(() =>
        {

            passenger.ItemGrab.RemoveAllListeners();
        });

        passenger.ItemInteract.AddListener(() =>
        {

            passenger.ItemInteract.RemoveAllListeners();
        });
    }

    IEnumerator GetPlayerCar(Action<GameObject> onGet)
    {
        GameObject car;
        do
        {
            yield return null;
            car = GameManager.Instance.PlayerCar;
        } while (car == null);
        onGet(car);   
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="panel">the image you want to fade</param>
    /// <param name="fadeTime">how much time it takes to fade the image</param>
    /// <param name="bufferTime">how much time passes after calling the function before it starts to fade the panel</param>
    void FadePanelAfterSeconds(Image panel, float fadeTime, float bufferTime)
    {
        StartCoroutine(DoFade(panel, fadeTime, bufferTime));
    }

    IEnumerator DoFade(Image panel, float fadeTime, float bufferTime)
    {
        yield return new WaitForSeconds(bufferTime);
        float currTime = fadeTime;
        Color startClrPanel = panel.color;
        TextMeshProUGUI text = panel.GetComponentInChildren<TextMeshProUGUI>();
        Color startClrTxt = text.color;

        while(currTime > 0)
        {
            panel.color = new Color(startClrPanel.r, startClrPanel.g, startClrPanel.b, currTime / fadeTime);
            text.color = new Color(startClrTxt.r, startClrTxt.g, startClrTxt.b, currTime / fadeTime);

            currTime -= Time.deltaTime;
            yield return null;
        }
        panel.color = new Color(startClrPanel.r, startClrPanel.g, startClrPanel.b, 0);
        panel.color = new Color(startClrTxt.r, startClrTxt.g, startClrTxt.b, 0);
        panel.gameObject.SetActive(false);
    }


    void Start()
    {
        StartCoroutine(GetPlayerCar(Interface));
    }
}
