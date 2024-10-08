using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
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
    bool passengerReady = false;
    bool driverReady = false;


    void Interface(GameObject playerCar)
    {

        PlayerController[] players = playerCar.GetComponentsInChildren<PlayerController>();

        PlayerController driver = players.FirstOrDefault(p => p.Player == 0);
        PlayerController passenger = players.FirstOrDefault(p => p.Player == 1);

        //waits untill both plazers pressed ready to leave tutorial
        StartCoroutine(WaitForReady());

        // DRIVER INPUTS 
        //SKIP TUTORIAL
        driver.OptionPress.AddListener(() =>
        {
            driverReady = true;
            driver.OptionPress.RemoveAllListeners();
            Debug.Log("options" + driverReady);


        });

        driver.BrakePressed.AddListener(() =>
        {

            if (orderdriver == 1)
            {
                FadePanelAfterSeconds(panelGoalDriver1, 2, 2, () =>
                {

                    panelGoalDriver2.gameObject.SetActive(true);
                    FadePanelAfterSeconds(panelGoalDriver2, 2, 2, () =>
                    {
                        panelStickDriver.gameObject.SetActive(true);
                        orderdriver += 1;

                    });
                });

                //this stops the input to run the code above again

            }
            else if (orderdriver >= 7)
            {
                driverReady = true;
                driver.BrakePressed.RemoveAllListeners();
                Debug.Log(driverReady);
            }



        });



        driver.GasPressed.AddListener(() =>
        {
            if (orderdriver >= 6)
            {

                FadePanelAfterSeconds(panelGasDriver, 1, 1, () =>
                {
                    panelEndDriver.gameObject.SetActive(true);
                    orderdriver++;
                });
                driver.GasPressed.RemoveAllListeners();


            }
        });

        driver.ItemGrab.AddListener(() =>
        {
            if (orderdriver >= 3)
            {
                FadePanelAfterSeconds(panelGrabDriver1, 1, 1, () =>
                {
                    panelGrabDriver2.gameObject.SetActive(true);
                    orderdriver++;
                }); ;

                driver.ItemGrab.RemoveAllListeners();


            }
        });

        driver.ItemInteract.AddListener(() =>
        {
            if (orderdriver >= 4)
            {

                FadePanelAfterSeconds(panelGrabDriver2, 1, 1, () =>
                {
                    panelGrabDriver3.gameObject.SetActive(true);
                    orderdriver++;
                });
                driver.ItemInteract.RemoveAllListeners();


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
                    orderdriver++;
                });
                driver.UpdateRightJoystick.RemoveListener(onJoystickRightDriver);


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
                    orderdriver++;
                });
                driver.UpdateLeftJoystick.RemoveListener(onJoystickLeftDriver);


            }
        }

        // PASSENGER INPUTS

        passenger.OptionPress.AddListener(() =>
        {
            passengerReady = true;
            passenger.OptionPress.RemoveAllListeners();
            Debug.Log(passengerReady);

        });

        passenger.BrakePressed.AddListener(() =>
        {



            //write your code here
            if (orderpassenger == 1)
            {
                FadePanelAfterSeconds(panelGoalPassenger, 1, 1, () =>
                {
                    panelStickPassenger.gameObject.SetActive(true);
                    orderpassenger++;
                });

                //write your code here

                //this stops the input to run the code above again

                orderpassenger++;
            }
            else if (orderpassenger >= 4)
            {
                passengerReady = true;
                passenger.BrakePressed.RemoveAllListeners();
                Debug.Log(passengerReady);
            }


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
                    orderpassenger++;
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
                    orderpassenger++;
                });
                passenger.ItemGrab.RemoveAllListeners();


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
                        orderpassenger++;
                    }); ;
                });
                passenger.ItemInteract.RemoveAllListeners();


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
        CanvasGroup group = panel.GetComponent<CanvasGroup>();

        while (currTime > 0)
        {
            group.alpha = currTime / fadeTime;

            currTime -= Time.deltaTime;
            yield return null;
        }
        group.alpha = 0;
        panel.gameObject.SetActive(false);

        onComplete?.Invoke();
    }


    void Start()
    {
        StartCoroutine(GetPlayerCar(Interface));
    }


    IEnumerator WaitForReady()
    {
        yield return new WaitUntil(() => driverReady && passengerReady);
        GameManager.Instance.FinishLevel();
    }
}
