using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TutorialSequencer : MonoBehaviour
{
    //this is just an example, you can create as many fields as you want
    [SerializeField] Image panel1driver;
    [SerializeField] Image panel2driver;
    [SerializeField] Image panel3driver;
    [SerializeField] Image panel4driver;
    [SerializeField] Image panel5driver;
    [SerializeField] Image panel6driver;
    [SerializeField] Image panel7driver;
    [SerializeField] Image panel8driver;
    [SerializeField] Image panel1passenger;
    [SerializeField] Image panel2passenger;
    [SerializeField] Image panel3passenger;
    [SerializeField] Image panel4passenger;
    [SerializeField] Image panel5passenger;


    void Interface(GameObject playerCar)
    {

        PlayerController[] players = playerCar.GetComponentsInChildren<PlayerController>();

        PlayerController driver = players.FirstOrDefault(p => p.Player == 0);
        PlayerController passenger = players.FirstOrDefault(p => p.Player == 1);

        // DRIVER INPUTS 

        driver.BrakePressed.AddListener(() =>
        {
            //write your code here

            //just an example
            FadePanelAfterSeconds(panel1driver, 1, 1, () =>
            {
                panel2driver.gameObject.SetActive(true);
                panel3driver.gameObject.SetActive(true);
                FadePanelAfterSeconds(panel2driver, 1, 1);
            });

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

        void onJoystickRightDriver(Vector2 v)
        {
            //your code here
            //example
            if(v != Vector2.zero)
            FadePanelAfterSeconds(panel5passenger, 1, 1);
            driver.UpdateRightJoystick.RemoveListener(onJoystickRightDriver);
        }

        void onJoystickLeftDriver(Vector2 v)
        {
            //your code here
            //example
            if(v != Vector2.zero)
            FadePanelAfterSeconds(panel5passenger, 1, 1);
            driver.UpdateLeftJoystick.RemoveListener(onJoystickLeftDriver);
        }

        // PASSENGER INPUTS

        passenger.BrakePressed.AddListener(() =>
        {
            //write your code here
            FadePanelAfterSeconds(panel1passenger, 1, 1);
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


        void onJoystickRightPassenger(Vector2 v)
        {
            //your code here
            //example
            if(v != Vector2.zero)
            FadePanelAfterSeconds(panel5passenger, 1, 1);


            driver.UpdateRightJoystick.RemoveListener(onJoystickRightPassenger);
        }

        void onJoystickLeftPassenger(Vector2 v)
        {
            //your code here
            //example
            if(v != Vector2.zero)
            FadePanelAfterSeconds(panel5passenger, 1, 1);
            driver.UpdateLeftJoystick.RemoveListener(onJoystickLeftPassenger);
        }
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
    void FadePanelAfterSeconds(Image panel, float fadeTime, float bufferTime, Action onComplete = null)
    {
        StartCoroutine(DoFade(panel, fadeTime, bufferTime, onComplete));
    }

    IEnumerator DoFade(Image panel, float fadeTime, float bufferTime, Action onComplete = null)
    {
        yield return new WaitForSeconds(bufferTime);
        float currTime = fadeTime;
        Color startClrPanel = panel.color;
        TextMeshProUGUI text = panel.GetComponentInChildren<TextMeshProUGUI>();
        Color startClrTxt = text.color;

        while (currTime > 0)
        {
            panel.color = new Color(startClrPanel.r, startClrPanel.g, startClrPanel.b, currTime / fadeTime);
            text.color = new Color(startClrTxt.r, startClrTxt.g, startClrTxt.b, currTime / fadeTime);

            currTime -= Time.deltaTime;
            yield return null;
        }
        panel.color = new Color(startClrPanel.r, startClrPanel.g, startClrPanel.b, 0);
        panel.color = new Color(startClrTxt.r, startClrTxt.g, startClrTxt.b, 0);
        panel.gameObject.SetActive(false);

        onComplete?.Invoke();
    }


    void Start()
    {
        StartCoroutine(GetPlayerCar(Interface));
    }
}
