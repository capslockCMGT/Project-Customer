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
    [SerializeField] Image panelGoalDriver1;
    [SerializeField] Image panelGoalDriver2;
    [SerializeField] Image panelStickDriver;
    [SerializeField] Image panelGrabDriver1;
    [SerializeField] Image panelGrabDriver2;
    [SerializeField] Image panelGrabDriver3;
    [SerializeField] Image panelGrabDriver4;
    [SerializeField] Image panelGasDriver;
    [SerializeField] Image panelGoalPassenger;
    [SerializeField] Image panelStickPassenger;
    [SerializeField] Image panelGrabPassenger1;
    [SerializeField] Image panelGrabPassenger2;
    [SerializeField] Image panelGrabPassenger3;
    [SerializeField] Image panelEndDriver;
    [SerializeField] Image panelEndPassenger;
    [SerializeField] GameManager gameManager;


    int orderdriver = 1;
    int orderpassenger = 1;
    int passengerReady = 0;
    int driverReady = 0;


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
            FadePanelAfterSeconds(panelGoalDriver1, 2, 2, () =>
            {

                panelGoalDriver2.gameObject.SetActive(true);
                FadePanelAfterSeconds(panelGoalDriver2, 2, 2, () =>
                {
                    panelStickDriver.gameObject.SetActive(true);
                    orderdriver += 1;
                    Debug.Log(orderdriver);
                });
            });

            //write your code here
            //this stops the input to run the code above again
            driver.BrakePressed.RemoveAllListeners();

        });



        driver.GasPressed.AddListener(() =>
        {
            if (orderdriver >= 6)
            {

                FadePanelAfterSeconds(panelGasDriver, 1, 1, () =>
                {
                    panelEndDriver.gameObject.SetActive(true);
                }); ;
                driver.GasPressed.RemoveAllListeners();
                orderdriver += 1;
                Debug.Log(orderdriver);
            }
        });

        driver.ItemGrab.AddListener(() =>
        {
            if (orderdriver >= 3)
            {
                FadePanelAfterSeconds(panelGrabDriver1, 1, 1, () =>
                {
                    panelGrabDriver2.gameObject.SetActive(true);
                }); ;

                driver.ItemGrab.RemoveAllListeners();
                orderdriver += 1;

            }
        });

        driver.ItemInteract.AddListener(() =>
        {
            if (orderdriver >= 4)
            {

                FadePanelAfterSeconds(panelGrabDriver2, 1, 1, () =>
                {
                    panelGrabDriver3.gameObject.SetActive(true);
                });
                driver.ItemInteract.RemoveAllListeners();
                orderdriver += 1;

            }
        });

        driver.UpdateRightJoystick.AddListener(onJoystickRightDriver);

        void onJoystickRightDriver(Vector2 v)
        {
            //your code here
            //example
            if (v != Vector2.zero && orderdriver >= 2)
            {


                FadePanelAfterSeconds(panelStickDriver, 1, 1, () =>
                {
                    panelGrabDriver1.gameObject.SetActive(true);
                });
                driver.UpdateRightJoystick.RemoveListener(onJoystickRightDriver);
                orderdriver += 1;

            }

        }

        driver.UpdateLeftJoystick.AddListener(onJoystickLeftDriver);
        void onJoystickLeftDriver(Vector2 v)
        {
            //your code here
            //example
            if (v != Vector2.zero && orderdriver >= 5)
            {
                FadePanelAfterSeconds(panelGrabDriver3, 1, 1, () =>
                {
                    panelGasDriver.gameObject.SetActive(true);

                });
                driver.UpdateLeftJoystick.RemoveListener(onJoystickLeftDriver);
                orderdriver += 1;

            }
        }

        // PASSENGER INPUTS

        passenger.BrakePressed.AddListener(() =>
        {



            //write your code here
            FadePanelAfterSeconds(panelGoalPassenger, 1, 1, () =>
            {
                panelStickPassenger.gameObject.SetActive(true);
            });

            //write your code here

            //this stops the input to run the code above again
            passenger.BrakePressed.RemoveAllListeners();
            orderpassenger++;


        });

        passenger.UpdateRightJoystick.AddListener(onJoystickRightPassenger);

        void onJoystickRightPassenger(Vector2 v)
        {
            //your code here
            //example
            if (v != Vector2.zero && orderpassenger >= 2)
            {
                FadePanelAfterSeconds(panelStickPassenger, 2, 2, () =>
                {
                    panelGrabPassenger1.gameObject.SetActive(true);
                });

                passenger.UpdateRightJoystick.RemoveListener(onJoystickRightPassenger);
                orderpassenger++;

            }
        }

        //passenger.GasPressed.AddListener(() =>
        //{

        //    passenger.GasPressed.RemoveAllListeners();
        //});

        passenger.ItemGrab.AddListener(() =>
        {
            if (orderpassenger >= 3)
            {
                FadePanelAfterSeconds(panelGrabPassenger1, 1, 1, () =>
                {
                    panelGrabPassenger2.gameObject.SetActive(true);
                });
                passenger.ItemGrab.RemoveAllListeners();
                orderpassenger++;

            }
        });

        passenger.ItemInteract.AddListener(() =>
        {
            if (orderpassenger >= 4)
            {
                FadePanelAfterSeconds(panelGrabPassenger2, 1, 1, () =>
                {
                    panelGrabPassenger3.gameObject.SetActive(true);
                    FadePanelAfterSeconds(panelGrabPassenger3, 2, 2, () =>
                    {
                        panelEndPassenger.gameObject.SetActive(true);
                    }); ;
                });
                passenger.ItemInteract.RemoveAllListeners();
                orderpassenger++;

            }
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

    private void FixedUpdate()
    {
        if (orderdriver >= 7)
        {
            driverReady += 1;
        }
        if (orderpassenger >= 4)
        {
            passengerReady += 1;
        }

        if (driverReady >= 1 && passengerReady >= 1)
        {
            gameManager.FinishLevel();
        }
    }
}
