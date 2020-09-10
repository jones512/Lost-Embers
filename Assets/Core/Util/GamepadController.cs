using UnityEngine;
using System.Collections;


//controller names taken from unity
//"PLAYSTATION(R)3 Controller"
//"Controller Xbox 360"
//"Controller (Xbox One For Windows)"
//Controller (Xbox One For Windows)

public class GamepadController{
    //map controllers like this
    //
    //	Y
    //X	  B
    //	A
    //
    //

    public enum directionGamepadEnum{
        Up,
        Down,
        Left,
        Right,
        None
    }

    public const float DEADZONE = 0.65f;
    public const float DEADZONEJOYSTICK = 0.25f;


    //int is the ID of the controller who sent the button (imagine having 4 gamepads connected, we need to know who sent the delegate)
    public System.Action<int, bool> EvntButtonPadUpPressed;
    public System.Action<int, bool> EvntButtonPadDownPressed;
    public System.Action<int, bool> EvntButtonPadLeftPressed;
    public System.Action<int, bool> EvntButtonPadRightPressed;
    public System.Action<int, bool> EvntButtonAPressed;
    public System.Action<int, bool> EvntButtonBPressed;
    public System.Action<int, bool> EvntButtonXPressed;
    public System.Action<int, bool> EvntButtonYPressed;
    public System.Action<int, bool> EvntButtonR1Pressed;
    public System.Action<int, bool> EvntButtonL1Pressed;
	public System.Action<int, bool> EvntButtonR2Pressed;
	public System.Action<int, bool> EvntButtonL2Pressed;
    public System.Action<int, bool> EvntButton5Pressed;
    public System.Action<int, bool> EvntButtonStickLeftPressed;
    public System.Action<int, bool> EvntButtonStickRightPressed;
    public System.Action<int, bool> EvntButtonStartPressed; //usually enter key
    public System.Action<int, bool> EvntButtonSelectPressed; //usually space key

    //to use with a continous button press, like moving the character on screen with a constant rate
    //this will be either the left stick or the 4 control buttons
    public System.Action<float, int, bool> EvntButtonPadUpPressedJoystick;
    public System.Action<float, int, bool> EvntButtonPadDownPressedJoystick;
    public System.Action<float, int, bool> EvntButtonPadLeftPressedJoystick;
    public System.Action<float, int, bool> EvntButtonPadRightPressedJoystick;
    public System.Action<float, float, int> EvntButtonPadAxisLeftHorizontal; //x, y, index joystick
    public System.Action<float, float, int> EvntButtonPadAxisLeftVertical; //x, y, index joystick
    public System.Action<float, float, int> EvntButtonPadAxisLeft; //x, y, index joystick
    public System.Action<float, int, bool> EvntButtonButtonBPressedJoystick;
    public System.Action<float, int, bool> EvntButtonButtonAPressedJoystick;
    public System.Action<float, int, bool> EvntButtonButtonXPressedJoystick;
    public System.Action<float, int, bool> EvntButtonButtonYPressedJoystick;

    //neutral stuff
    public System.Action<int> EvntButtonNoDirectionPressed; //none of the directions were pressed
    public System.Action<int> EvntButtonNoDirectionPressedLeftRight;
    public System.Action<int> EvntButtonNoDirectionPressedUpDown;


    #region controller types, vars and getters
    public enum ControllerType { WiiRemote = 0, WiiRemoteHorizontal, WiiClassic, WiiuGamepad, WiiUProController, Keyboard, KeyboardJoystick, X360, PS3, none };
  

    public struct Controllers{
        public ControllerType controllerType;
        public bool movedAnalog;
        public int controllerID;
        public int channelID;

        public void Init() {
            movedAnalog = false;
            controllerID = 0;
            channelID = -1;
            controllerType = ControllerType.none;
        }

        public void Destroy() { 
        }
    }

    Controllers[] _controllersList;
    bool _useContinuousInput = false;

    public Controllers[] GetControllersList(){
        return _controllersList;
    }

    public int GetTotalControllersConnected(){
        int totalControllers = 0;

		for (int i = 0; i < _controllersList.Length; i++) {
			if (_controllersList [i].controllerType != ControllerType.none) {
				totalControllers++;
			}
		}
             
        return totalControllers;
    }

	public float GetDeadZoneJoystickValue(){
		return DEADZONEJOYSTICK;
	}

	public float GetDeadZoneValue(){
		return DEADZONE;
	}

    public void SetContinuousInput(bool _value) {
        _useContinuousInput = _value;
    }

    #endregion



    #region init and destroy
    public void InitAllControllers(int maxcontrollers){
#if UNITY_EDITOR || UNITY_STANDALONE
        _controllersList = new Controllers[maxcontrollers];
        
        //we expect 4 controller max
        for (int i = 0; i < maxcontrollers; i++)
        {
            _controllersList[i] = new Controllers();
            _controllersList[i].Init();
		}

		//Debug.Log(Input.GetJoystickNames()[0] + " is connected");

#else //any console
        _controllersList = new Controllers[maxcontrollers];
        
        //we expect 4 controller max
        for (int i = 0; i < maxcontrollers; i++)
        {
            _controllersList[i] = new Controllers();
			//_controllersList[i].controllerID = -1;
            _controllersList[i].Init();
		}
#endif
    }

    public void DestroyControllers(){

        if (_controllersList != null) {
            for(int i = 0; i < _controllersList.Length; i++) {
                _controllersList[i].Destroy();
            }
            System.Array.Clear(_controllersList, 0, _controllersList.Length);
        }
        
        _controllersList = null;
    }
    #endregion



    //use this on menus
    public void SetControllerConnected(){

		//wiiu
#if UNITY_CAFE
        if (_controllersList[0].controllerType != ControllerType.WiiuGamepad)
        {
            _controllersList[0].controllerType = ControllerType.WiiuGamepad;
            _controllersList[0].controllerID = 0;
            _controllersList[0].channelID = -1;
        }

        //get the controllers connected, and we are expecting 4 channels max
        for (int channelIndex = 0; channelIndex < 4; ++channelIndex)
        {
            WiiUInput.DisableAimingMode((uint)channelIndex);
            WiiUInput.DisableDPD((uint)channelIndex);

            WiiUDevType type = WiiUDevType.Unknown;
            WiiUErrorStatus status = WiiUInput.Probe((uint)channelIndex, out type);

            //start in position 1 of our controllers array, we expect gamepad on position 0
            if ( (channelIndex+1) < _controllersList.Length)
            {
                if (status != WiiUErrorStatus.NoController && (type == WiiUDevType.Core || type == WiiUDevType.MotionPlus))
                {
                    if (_controllersList[(channelIndex + 1)].controllerType != ControllerType.WiiRemoteHorizontal)
                    {
                        //Debug.Log("we dont have the WiiRemoteHorizontal, adding to list=" + controllerIndex);
                        _controllersList[(channelIndex + 1)].controllerType = ControllerType.WiiRemoteHorizontal;
                        _controllersList[(channelIndex + 1)].controllerID = (channelIndex + 1);
                        _controllersList[(channelIndex + 1)].channelID = channelIndex;
                        break;
                    }
                }
                else if (status != WiiUErrorStatus.NoController && (type == WiiUDevType.ProController))
                {
                    if (_controllersList[(channelIndex + 1)].controllerType != ControllerType.WiiUProController)
                    {
                        //Debug.Log("we dont have the WiiUProController, adding to list=" + controllerIndex);
                        _controllersList[(channelIndex + 1)].controllerType = ControllerType.WiiUProController;
                        _controllersList[(channelIndex + 1)].controllerID = (channelIndex + 1);
                        _controllersList[(channelIndex + 1)].channelID = channelIndex;
                        break;
                    }
                }
                else if (status != WiiUErrorStatus.NoController && (type == WiiUDevType.Classic || type == WiiUDevType.MotionPlusClassic))
                {
                    if (_controllersList[(channelIndex + 1)].controllerType != ControllerType.WiiClassic)
                    {
                        //Debug.Log("we dont have the WiiClassic, adding to list=" + controllerIndex);
                        _controllersList[(channelIndex + 1)].controllerType = ControllerType.WiiClassic;
                        _controllersList[(channelIndex + 1)].controllerID = (channelIndex + 1);
                        _controllersList[(channelIndex + 1)].channelID = channelIndex;
                        break;
                    }
                }
                else if (status == WiiUErrorStatus.NoController)
                {
                    if (_controllersList[(channelIndex + 1)].controllerType != ControllerType.none)
                    {
                        //Debug.Log("detected NoController");
                        //Debug.Log("we have an empty controller, adding to list=" + controllerIndex);
                        _controllersList[(channelIndex + 1)].controllerType = ControllerType.none;
                        _controllersList[(channelIndex + 1)].controllerID = (channelIndex + 1);
                        _controllersList[(channelIndex + 1)].channelID = channelIndex;
                        break;
                    }
                }
            }
        }
#endif


#if UNITY_EDITOR || UNITY_STANDALONE
        _controllersList[0].controllerID = 0;

        _controllersList[0].controllerType = ControllerType.Keyboard;

        for (int i = 1; i < _controllersList.Length; i++) {
            _controllersList[i].controllerID = i;
            _controllersList[i].controllerType = ControllerType.none;
        }
#endif
    }



    #region all control types
    void UpdateAnalogLeftStick(int index, bool continuousDetection, Vector2 input){
        Vector2 inputLeft = input;

        if (continuousDetection)
        {
			if (inputLeft.magnitude < DEADZONEJOYSTICK) {
				inputLeft = Vector2.zero;

				if (EvntButtonPadAxisLeft != null)
					EvntButtonPadAxisLeft (0, 0, index);

				if (EvntButtonPadAxisLeftHorizontal != null)
					EvntButtonPadAxisLeftHorizontal (0f, 0f, index);

                if (EvntButtonPadAxisLeftVertical != null)
                    EvntButtonPadAxisLeftVertical(0f, 0f, index);
			} else {
				inputLeft = inputLeft.normalized * ((inputLeft.magnitude - DEADZONEJOYSTICK) / (1 - DEADZONEJOYSTICK));

				if (EvntButtonPadAxisLeft != null)
					EvntButtonPadAxisLeft (inputLeft.x, inputLeft.y, index);

                if (Mathf.Sign(inputLeft.x) == 1) {
                    if (EvntButtonPadAxisLeftHorizontal != null)
                        EvntButtonPadAxisLeftHorizontal(1f, 0f, index);
                } else if (Mathf.Sign(inputLeft.x) == -1) {
                    if (EvntButtonPadAxisLeftHorizontal != null)
                        EvntButtonPadAxisLeftHorizontal(-1f, 0f, index);
                }

				if (Mathf.Sign (inputLeft.x) == 0) {
					if (EvntButtonPadAxisLeftHorizontal != null)
						EvntButtonPadAxisLeftHorizontal (0f, 0f, index);
				}


                if (Mathf.Sign(inputLeft.y) == 1) {
                    if (EvntButtonPadAxisLeftVertical != null)
                        EvntButtonPadAxisLeftVertical(0, 1f, index);
                } else if (Mathf.Sign(inputLeft.y) == -1) {
                    if (EvntButtonPadAxisLeftVertical != null)
                        EvntButtonPadAxisLeftVertical(0, -1f, index);
                }


                if (Mathf.Sign(inputLeft.y) == 0) {
                    if (EvntButtonPadAxisLeftVertical != null)
                        EvntButtonPadAxisLeftVertical(0f, 0f, index);
                }
			}
        }
        else
        {
			if (inputLeft.magnitude < DEADZONE) {
				_controllersList [index].movedAnalog = false;
				inputLeft = Vector2.zero;
			} else {
				inputLeft = inputLeft.normalized * ((inputLeft.magnitude - DEADZONE) / (1 - DEADZONE));

				if (Mathf.Abs (inputLeft.x) > DEADZONE) {
					if (Mathf.Sign (inputLeft.x) == 1) {
						if (!_controllersList [index].movedAnalog) {
							if (EvntButtonPadRightPressed != null)
								EvntButtonPadRightPressed (index, true);

							_controllersList [index].movedAnalog = true;
						}
					} else if (Mathf.Sign (inputLeft.x) == -1) {
						if (!_controllersList [index].movedAnalog) {
							if (EvntButtonPadLeftPressed != null)
								EvntButtonPadLeftPressed (index, true);

							_controllersList [index].movedAnalog = true;
						}
					}
				} else if (Mathf.Abs (inputLeft.y) > DEADZONE) {
					if (Mathf.Sign (inputLeft.y) == 1) {
						if (!_controllersList [index].movedAnalog) {
							if (EvntButtonPadUpPressed != null)
								EvntButtonPadUpPressed (index, true);

							_controllersList [index].movedAnalog = true;
						}
					} else if (Mathf.Sign (inputLeft.y) == -1) {
						if (!_controllersList [index].movedAnalog) {
							if (EvntButtonPadDownPressed != null)
								EvntButtonPadDownPressed (index, true);

							_controllersList [index].movedAnalog = true;
						}
					}
				}
			}//end else
        }
    }

    void UpdateWiiuGamepad(int index, bool continuousDetection)
    {
#if UNITY_CAFE
        WiiUGamePad pad = WiiUInput.GetGamePad();
        //WiiUGamePad pad = controllersList[index].controlPad as WiiUGamePad;

		if (!pad.GetButton (WiiUGamePadButton.ButtonLeft) && !pad.GetButton (WiiUGamePadButton.ButtonRight)
		          && !pad.GetButton (WiiUGamePadButton.ButtonUp) && !pad.GetButton (WiiUGamePadButton.ButtonDown)) {
			if (EvntButtonNoDirectionPressed != null)
				EvntButtonNoDirectionPressed (index);

			UpdateAnalogLeftStick (index, continuousDetection, pad.leftStick);
		}

		if (pad.GetButton (WiiUGamePadButton.ButtonLeft)) {
			if (EvntButtonPadLeftPressedJoystick != null)
				EvntButtonPadLeftPressedJoystick (1.0f, index);
		}

		if (pad.GetButton (WiiUGamePadButton.ButtonRight)) {
			if (EvntButtonPadRightPressedJoystick != null)
				EvntButtonPadRightPressedJoystick (1.0f, index);
		}

		if (pad.GetButton (WiiUGamePadButton.ButtonUp)) {
			if (EvntButtonPadUpPressedJoystick != null)
				EvntButtonPadUpPressedJoystick (1.0f, index);
		}

		if (pad.GetButton (WiiUGamePadButton.ButtonDown)) {
			if (EvntButtonPadDownPressedJoystick != null)
				EvntButtonPadDownPressedJoystick (1.0f, index);
		}

		if (pad.GetButton (WiiUGamePadButton.ButtonA)) { //cancel button
			if (EvntButtonAPressedJoystick != null)
				EvntButtonAPressedJoystick (index);
		}

		if (pad.GetButton (WiiUGamePadButton.ButtonB)) { //accept button
			if (EvntButtonBPressedJoystick != null)
				EvntButtonBPressedJoystick (index);
		}



        //non-continuous detection
		if (pad.GetButtonDown (WiiUGamePadButton.ButtonLeft)) {
			if (EvntButtonPadLeftPressed != null)
				EvntButtonPadLeftPressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonRight)) {
			if (EvntButtonPadRightPressed != null)
				EvntButtonPadRightPressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonUp)) {
			if (EvntButtonPadUpPressed != null)
				EvntButtonPadUpPressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonDown)) {
			if (EvntButtonPadDownPressed != null)
				EvntButtonPadDownPressed (index);
		}
       
		if (pad.GetButtonDown (WiiUGamePadButton.ButtonA)) { //accept button
			if (EvntButtonAPressed != null)
				EvntButtonAPressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonB)) { //cancel button
			if (EvntButtonBPressed != null)
				EvntButtonBPressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonY)) {
			if (EvntButtonYPressed != null)
				EvntButtonYPressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonX)) {
			if (EvntButtonXPressed != null)
				EvntButtonXPressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonL)) {
			if (EvntButtonL1Pressed != null)
				EvntButtonL1Pressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonR)) {
			if (EvntButtonR1Pressed != null)
				EvntButtonR1Pressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonPlus)) { //start
			if (EvntButtonStartPressed != null)
				EvntButtonStartPressed (index);
		}

		if (pad.GetButtonDown (WiiUGamePadButton.ButtonMinus)) { //select
			if (EvntButtonSelectPressed != null)
				EvntButtonSelectPressed (index);
		}
#endif
    }

    void UpdateWiiuClassic(int index, bool continuousDetection)
    {
        #if UNITY_CAFE
        WiiUClassic pad = WiiUInput.GetClassic((uint)_controllersList[index].channelID);
  
        if (!pad.GetButton(WiiUButton.ClassicLeft) && !pad.GetButton(WiiUButton.ClassicRight)
            && !pad.GetButton(WiiUButton.ClassicUp) && !pad.GetButton(WiiUButton.ClassicDown))
        {
            if (EvntButtonNoDirectionPressed != null)
                EvntButtonNoDirectionPressed(index);

            UpdateAnalogLeftStick(index, continuousDetection, pad.leftStick);
        }


        if (pad.GetButton(WiiUButton.ClassicLeft))
        {
            if (EvntButtonPadLeftPressedJoystick != null)
                EvntButtonPadLeftPressedJoystick(1.0f, index);
        }

        if (pad.GetButton(WiiUButton.ClassicRight))
        {
            if (EvntButtonPadRightPressedJoystick != null)
                EvntButtonPadRightPressedJoystick(1.0f, index);
        }

        if (pad.GetButton(WiiUButton.ClassicUp))
        {
            if (EvntButtonPadUpPressedJoystick != null)
                EvntButtonPadUpPressedJoystick(1.0f, index);
        }

        if (pad.GetButton(WiiUButton.ClassicDown))
        {
            if (EvntButtonPadDownPressedJoystick != null)
                EvntButtonPadDownPressedJoystick(1.0f, index);
        }

        if (pad.GetButton(WiiUButton.ClassicA)) //cancel button
        {
            if (EvntButtonAPressedJoystick != null)
                EvntButtonAPressedJoystick(index);
        }

        if (pad.GetButton(WiiUButton.ClassicB)) //accept button
        {
            if (EvntButtonBPressedJoystick != null)
                EvntButtonBPressedJoystick(index);
        }


        //non-continuous detection
        if (pad.GetButtonDown(WiiUButton.ClassicLeft))
        {
            if (EvntButtonPadLeftPressed != null)
                EvntButtonPadLeftPressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicRight))
        {
            if (EvntButtonPadRightPressed != null)
                EvntButtonPadRightPressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicUp))
        {
            if (EvntButtonPadUpPressed != null)
                EvntButtonPadUpPressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicDown))
        {
            if (EvntButtonPadDownPressed != null)
                EvntButtonPadDownPressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicA)) //cancel button
        {
            if (EvntButtonAPressed != null)
                EvntButtonAPressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicB)) //accept button
        {
            if (EvntButtonBPressed != null)
                EvntButtonBPressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicY))
        {
            if (EvntButtonYPressed != null)
                EvntButtonYPressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicX))
        {
            if (EvntButtonXPressed != null)
                EvntButtonXPressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicL))
        {
            if (EvntButtonL1Pressed != null)
                EvntButtonL1Pressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicR))
        {
            if (EvntButtonR1Pressed != null)
                EvntButtonR1Pressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicPlus)) //start
        {
            if (EvntButtonStartPressed != null)
                EvntButtonStartPressed(index);
        }

        if (pad.GetButtonDown(WiiUButton.ClassicMinus)) //select
        {
            if (EvntButtonSelectPressed != null)
                EvntButtonSelectPressed(index);
        }
#endif

    }

    void UpdateWiiuRemote(int index, bool isHorizontal, bool continuousDetection)
    {
        #if UNITY_CAFE
        WiiURemote pad = WiiUInput.GetRemote((uint)_controllersList[index].channelID);
 
        //horizontal buttonA becomes button2, and buttonB becomes button1
        //vertical buttonA becomes buttonA, and buttonB still buttonB
        //other buttons get changed as well

        if (!pad.GetButton(WiiUButton.ButtonLeft) && !pad.GetButton(WiiUButton.ButtonRight)
            && !pad.GetButton(WiiUButton.ButtonUp) && !pad.GetButton(WiiUButton.ButtonDown))
        {
            if (EvntButtonPadAxisLeftHorizontal != null)
                EvntButtonPadAxisLeftHorizontal(0f, 0f, index);

            if (EvntButtonNoDirectionPressed != null)
                EvntButtonNoDirectionPressed(index);
        }


        if (isHorizontal)
        {
            if (pad.GetButton(WiiUButton.ButtonUp))
            {
                if (EvntButtonPadLeftPressedJoystick != null)
                    EvntButtonPadLeftPressedJoystick(1.0f, index);
            }

            if (pad.GetButton(WiiUButton.ButtonDown))
            {
                if (EvntButtonPadRightPressedJoystick != null)
                    EvntButtonPadRightPressedJoystick(1.0f, index);
            }

            if (pad.GetButton(WiiUButton.ButtonRight))
            {
                if (EvntButtonPadUpPressedJoystick != null)
                    EvntButtonPadUpPressedJoystick(1.0f, index);
            }

            if (pad.GetButton(WiiUButton.ButtonLeft))
            {
                if (EvntButtonPadDownPressedJoystick != null)
                    EvntButtonPadDownPressedJoystick(1.0f, index);
            }

            if (pad.GetButton(WiiUButton.Button2)) //cancel button
            {
                if (EvntButtonAPressedJoystick != null)
                    EvntButtonAPressedJoystick(index);
            }

            if (pad.GetButton(WiiUButton.Button1)) //accept button
            {
                if (EvntButtonBPressedJoystick != null)
                    EvntButtonBPressedJoystick(index);
            }


            //non-continuous detection
            if (pad.GetButtonDown(WiiUButton.ButtonUp)) //left
            {
                if (EvntButtonPadLeftPressed != null)
                    EvntButtonPadLeftPressed(index);
            }

            if (pad.GetButtonDown(WiiUButton.ButtonDown)) //right
            {
                if (EvntButtonPadRightPressed != null)
                    EvntButtonPadRightPressed(index);
            }

            if (pad.GetButtonDown(WiiUButton.ButtonRight)) //up
            {
                if (EvntButtonPadUpPressed != null)
                    EvntButtonPadUpPressed(index);
            }

            if (pad.GetButtonDown(WiiUButton.ButtonLeft)) //down
            {
                if (EvntButtonPadDownPressed != null)
                    EvntButtonPadDownPressed(index);
            }

            if (pad.GetButtonDown(WiiUButton.Button2)) //cancel button
            {
                if (EvntButtonAPressed != null)
                    EvntButtonAPressed(index);
            }

            if (pad.GetButtonDown(WiiUButton.Button1)) //accept button
            {
                if (EvntButtonBPressed != null)
                    EvntButtonBPressed(index);
            }
        }



        if (pad.GetButtonDown(WiiUButton.ButtonPlus)) //start
        {
            if (EvntButtonStartPressed != null)
                EvntButtonStartPressed(index);
        }


        if (pad.GetButtonDown(WiiUButton.ButtonMinus)) //select
        {
            if (EvntButtonSelectPressed != null)
                EvntButtonSelectPressed(index);
        }
        #endif
    }

    void UpdateWiiuProController(int index, bool continuousDetection)
    {
        #if UNITY_CAFE
        WiiUProController pad = WiiUInput.GetProController((uint)_controllersList[index].channelID);

        if (!pad.GetButton(WiiUProControllerButton.ButtonLeft) && !pad.GetButton(WiiUProControllerButton.ButtonRight)
            && !pad.GetButton(WiiUProControllerButton.ButtonUp) && !pad.GetButton(WiiUProControllerButton.ButtonDown))
        {
            if (EvntButtonNoDirectionPressed != null)
                EvntButtonNoDirectionPressed(index);

            UpdateAnalogLeftStick(index, continuousDetection, pad.leftStick);
        }

        if (pad.GetButton(WiiUProControllerButton.ButtonLeft))
        {
            if (EvntButtonPadLeftPressedJoystick != null)
                EvntButtonPadLeftPressedJoystick(1.0f, index);
        }

        if (pad.GetButton(WiiUProControllerButton.ButtonRight))
        {
            if (EvntButtonPadRightPressedJoystick != null)
                EvntButtonPadRightPressedJoystick(1.0f, index);
        }

        if (pad.GetButton(WiiUProControllerButton.ButtonUp))
        {
            if (EvntButtonPadUpPressedJoystick != null)
                EvntButtonPadUpPressedJoystick(1.0f, index);
        }

        if (pad.GetButton(WiiUProControllerButton.ButtonDown))
        {
            if (EvntButtonPadDownPressedJoystick != null)
                EvntButtonPadDownPressedJoystick(1.0f, index);
        }

        if (pad.GetButton(WiiUProControllerButton.ButtonA)) //cancel button
        {
            if (EvntButtonAPressedJoystick != null)
                EvntButtonAPressedJoystick(index);
        }

        if (pad.GetButton(WiiUProControllerButton.ButtonB)) //accept button
        {
            if (EvntButtonBPressedJoystick != null)
                EvntButtonBPressedJoystick(index);
        }


        //non-continuous detection
        if (pad.GetButtonDown(WiiUProControllerButton.ButtonLeft))
        {
            if (EvntButtonPadLeftPressed != null)
                EvntButtonPadLeftPressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonRight))
        {
            if (EvntButtonPadRightPressed != null)
                EvntButtonPadRightPressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonUp))
        {
            if (EvntButtonPadUpPressed != null)
                EvntButtonPadUpPressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonDown))
        {
            if (EvntButtonPadDownPressed != null)
                EvntButtonPadDownPressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonA)) //cancel button
        {
            if (EvntButtonAPressed != null)
                EvntButtonAPressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonB)) //accept button
        {
            if (EvntButtonBPressed != null)
                EvntButtonBPressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonY))
        {
            if (EvntButtonYPressed != null)
                EvntButtonYPressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonX))
        {
            if (EvntButtonXPressed != null)
                EvntButtonXPressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonL))
        {
            if (EvntButtonL1Pressed != null)
                EvntButtonL1Pressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonR))
        {
            if (EvntButtonR1Pressed != null)
                EvntButtonR1Pressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonPlus)) //start
        {
            if (EvntButtonStartPressed != null)
                EvntButtonStartPressed(index);
        }

        if (pad.GetButtonDown(WiiUProControllerButton.ButtonMinus)) //select
        {
            if (EvntButtonSelectPressed != null)
                EvntButtonSelectPressed(index);
        }
#endif
    }

    
    //wired to a xbox controller
    void UpdateGamepadWindows(int index, bool continuousDetection) {

        Vector2 inputLeft;

        inputLeft.x = Input.GetAxis("Horizontal");
        inputLeft.y = Input.GetAxis("Vertical");

        if (inputLeft.x != 0f || inputLeft.y != 0f) {
            UpdateAnalogLeftStick(index, continuousDetection, inputLeft);
        } else if (inputLeft.x <= 0.1f || inputLeft.y <= 0.1f) { //no main input, then check control pad axis
            Vector2 inputPadLeft;
            inputPadLeft.x = Input.GetAxis("PadLeftRight");
            inputPadLeft.y = Input.GetAxis("PadUpDown");
            UpdateAnalogLeftStick(index, continuousDetection, inputPadLeft);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton1)) {
            //button X
            if (EvntButtonBPressed != null)
                EvntButtonBPressed(index, true);
        }

		if (Input.GetKeyUp(KeyCode.JoystickButton1)) {
            //button X
            if (EvntButtonBPressed != null)
				EvntButtonBPressed(index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton0)) {
            //button O
            if (EvntButtonAPressed != null)
				EvntButtonAPressed(index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton0)) {
			if (EvntButtonAPressed != null)
				EvntButtonAPressed(index, false);
		}

        if (Input.GetKeyDown(KeyCode.JoystickButton2)) {
            //button []
            if (EvntButtonXPressed != null)
                EvntButtonXPressed(index, true);
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton2)) {
            if (EvntButtonXPressed != null)
                EvntButtonXPressed(index, false);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton3)) {
            //button triangle
            if (EvntButtonYPressed != null)
                EvntButtonYPressed(index, true);
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton3)) {
            if (EvntButtonYPressed != null)
                EvntButtonYPressed(index, false);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton4)) {
            if (EvntButtonL1Pressed != null)
                EvntButtonL1Pressed(index, true);
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton4)) {
            if (EvntButtonL1Pressed != null)
                EvntButtonL1Pressed(index, false);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton5)) {
            if (EvntButtonR1Pressed != null)
                EvntButtonR1Pressed(index, true);
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton5)) {
            if (EvntButtonR1Pressed != null)
                EvntButtonR1Pressed(index, false);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6)) {
            if (EvntButtonSelectPressed != null)
                EvntButtonSelectPressed(index, true);
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton6)) {
            if (EvntButtonSelectPressed != null)
                EvntButtonSelectPressed(index, false);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton7)) {
            if (EvntButtonStartPressed != null)
                EvntButtonStartPressed(index, true);
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton7)) {
            if (EvntButtonStartPressed != null)
                EvntButtonStartPressed(index, false);
        }


        //if we want the rear bumpers (R2 and L2)
        //R values are negative
        //L values are positive
        //Vector2 inputR2L2;
        //inputR2L2.x = Input.GetAxis("BackTriggers");
        //inputR2L2.y = Input.GetAxis("BackTriggers");

        //Debug.Log("input right stickx " + inputR2L2.x);
        //Debug.Log("input right sticky " + inputR2L2.y);

    }
		
	//wired to a ps3 controller
    void UpdateGamePadMac(int index, bool continuousDetection) {

        Vector2 inputLeft;
        inputLeft.x = Input.GetAxis("Horizontal");
        inputLeft.y = Input.GetAxis("Vertical");

        if (inputLeft.x != 0f || inputLeft.y != 0f) {
            UpdateAnalogLeftStick(index, continuousDetection, inputLeft);
        }

		if (Input.GetKeyDown(KeyCode.JoystickButton0)) {
			if (EvntButtonSelectPressed != null)
				EvntButtonSelectPressed(index, true);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton3)) {
			if (EvntButtonStartPressed != null)
				EvntButtonStartPressed(index, true);
		}



		//directions
		if (Input.GetKeyDown(KeyCode.JoystickButton4)) {
			if (EvntButtonPadUpPressed != null)
				EvntButtonPadUpPressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton4)) {
			if (EvntButtonPadUpPressed != null)
				EvntButtonPadUpPressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton5)) {
			if (EvntButtonPadRightPressed != null)
				EvntButtonPadRightPressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton5)) {
			if (EvntButtonPadRightPressed != null)
				EvntButtonPadRightPressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton6)) {
			if (EvntButtonPadDownPressed != null)
				EvntButtonPadDownPressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton6)) {
			if (EvntButtonPadDownPressed != null)
				EvntButtonPadDownPressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton7)) {
			if (EvntButtonPadLeftPressed != null)
				EvntButtonPadLeftPressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton7)) {
			if (EvntButtonPadLeftPressed != null)
				EvntButtonPadLeftPressed (index, false);
		}



		//buttons
		if (Input.GetKeyDown(KeyCode.JoystickButton8)) {
			if (EvntButtonL2Pressed != null)
				EvntButtonL2Pressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton8)) {
			if (EvntButtonL2Pressed != null)
				EvntButtonL2Pressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton9)) {
			if (EvntButtonR2Pressed != null)
				EvntButtonR2Pressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton9)) {
			if (EvntButtonR2Pressed != null)
				EvntButtonR2Pressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.Joystick1Button10)) {
			if (EvntButtonL1Pressed != null)
				EvntButtonL1Pressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton10)) {
			if (EvntButtonL1Pressed != null)
				EvntButtonL1Pressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton11)) {
			if (EvntButtonR1Pressed != null)
				EvntButtonR1Pressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton11)) {
			if (EvntButtonR1Pressed != null)
				EvntButtonR1Pressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton12)) {
			//triangle button
			if (EvntButtonYPressed != null)
				EvntButtonYPressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton12)) {
			//triangle button
			if (EvntButtonYPressed != null)
				EvntButtonYPressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton13)) {
			//O button
			if (EvntButtonBPressed != null)
				EvntButtonBPressed (index, true);		
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton13)) {
			//O button
			if (EvntButtonBPressed != null)
				EvntButtonBPressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton14)) {
			//x button
			if (EvntButtonAPressed != null)
				EvntButtonAPressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton14)) {
			//x button
			if (EvntButtonAPressed != null)
				EvntButtonAPressed (index, false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton15)) {
			//[] button
			if (EvntButtonXPressed != null)
				EvntButtonXPressed (index, true);
		}

		if (Input.GetKeyUp(KeyCode.JoystickButton15)) {
			//[] button
			if (EvntButtonXPressed != null)
				EvntButtonXPressed (index, false);
		}
	}

    void UpdateKeyboard(int index, bool continuousDetection)
    {	
        //we are using 0 as the ID because the keyboard is "single player"
		if (Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.LeftArrow)) {
			if (EvntButtonPadRightPressedJoystick != null)
				EvntButtonPadRightPressedJoystick (1.0f, index, true);
		} else if (Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow)) {
			if (EvntButtonPadLeftPressedJoystick != null)
                EvntButtonPadLeftPressedJoystick(-1.0f, index, true);
		}


		if (Input.GetKey (KeyCode.UpArrow) && !Input.GetKey (KeyCode.DownArrow)) {
			if (EvntButtonPadUpPressedJoystick != null)
                EvntButtonPadUpPressedJoystick(1.0f, index, true);
		} else if (Input.GetKey (KeyCode.DownArrow) && !Input.GetKey (KeyCode.UpArrow)) {
			if (EvntButtonPadDownPressedJoystick != null)
                EvntButtonPadDownPressedJoystick(-1.0f, index, true);
		}


        //buttons released/non-pressed
		if (!Input.GetKey (KeyCode.UpArrow) && !Input.GetKey (KeyCode.DownArrow)) {
			if (EvntButtonNoDirectionPressedUpDown != null)
                EvntButtonNoDirectionPressedUpDown(index);
		}

		if (!Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow)) {
			if (EvntButtonNoDirectionPressedLeftRight != null)
                EvntButtonNoDirectionPressedLeftRight(index);
		}

        //no input, check analog joystick
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)
            && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)){
            
            if (EvntButtonNoDirectionPressed != null)
                EvntButtonNoDirectionPressed(index);
		}

        //keyboard
        //non-continuos detection (one press, useful for menus)
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (EvntButtonSelectPressed != null)
                EvntButtonSelectPressed(index, true);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			if (EvntButtonSelectPressed != null)
                EvntButtonSelectPressed(index, false);
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (EvntButtonPadUpPressed != null)
                EvntButtonPadUpPressed(index, true);
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (EvntButtonPadDownPressed != null)
                EvntButtonPadDownPressed(index, true);
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			if (EvntButtonPadLeftPressed != null)
                EvntButtonPadLeftPressed(index, true);
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			if (EvntButtonPadRightPressed != null)
                EvntButtonPadRightPressed(index, true);
		}

		if (Input.GetKeyDown (KeyCode.Z)) { //button A
			if (EvntButtonAPressed != null)
                EvntButtonAPressed(index, true);
		}

		if (Input.GetKeyDown (KeyCode.X)) { //button B
			if (EvntButtonBPressed != null)
                EvntButtonBPressed(index, true);
		}
		if (Input.GetKeyUp (KeyCode.X)) { //button B
			if (EvntButtonBPressed != null)
                EvntButtonBPressed(index, false);
		}

		if (Input.GetKeyDown (KeyCode.C)) { //button X
			if (EvntButtonXPressed != null)
                EvntButtonXPressed(index, true);
		}

		if (Input.GetKeyDown (KeyCode.V)) { //button Y
			if (EvntButtonYPressed != null)
                EvntButtonYPressed(index, true);
		}

		if (Input.GetKeyDown (KeyCode.A)) { //button L
			if (EvntButtonL1Pressed != null)
                EvntButtonL1Pressed(index, true);
		}

		if (Input.GetKeyDown (KeyCode.S)) { //button R
			if (EvntButtonR1Pressed != null)
                EvntButtonR1Pressed(index, true);
		}

		if (Input.GetKeyDown (KeyCode.Return)) { //x button
			if (EvntButtonStartPressed != null)
                EvntButtonStartPressed(index, true);
		}


//		if (!Input.GetKey (KeyCode.UpArrow) && !Input.GetKey (KeyCode.DownArrow)
//		    && !Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow)
//			&& !Input.GetKey (KeyCode.Z) && !Input.GetKey (KeyCode.X)
//			&& !Input.GetKey (KeyCode.C) && !Input.GetKey (KeyCode.V)
//			&& !Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.S)){
//
//
//			#if UNITY_EDITOR && !UNITY_EDITOR_OSX
//		UpdateGamepadWindows(index, continuousDetection);
//			#elif UNITY_EDITOR_OSX
//			UpdateGamePadMac (index, continuousDetection);
//			#elif UNITY_STANDALONE_OSX
//		UpdateGamePadMac(index, continuousDetection);
//			#elif !UNITY_STANDALONE_OSX && UNITY_STANDALONE
//		UpdateGamepadWindows(index, continuousDetection);
//			#endif
//		}
        
    }

    void UpdateJoystick(int index, bool continuousDetection)
    {
#if UNITY_EDITOR && !UNITY_EDITOR_OSX
        UpdateGamepadWindows(index, continuousDetection);
#elif UNITY_EDITOR_OSX
		UpdateGamePadMac(index, continuousDetection);
#elif UNITY_STANDALONE_OSX
        UpdateGamePadMac(index, continuousDetection);
#elif !UNITY_STANDALONE_OSX && UNITY_STANDALONE
        UpdateGamepadWindows(index, continuousDetection);
#endif
    }

    #endregion



    #region main update
    public void UpdateControllerInput(){
		for (int i = 0; i < _controllersList.Length; i++) {
            switch (_controllersList[i].controllerType) {
                case ControllerType.WiiRemoteHorizontal:
                    UpdateWiiuRemote(_controllersList[i].controllerID, true, _useContinuousInput);
                    break;

                case ControllerType.WiiClassic:
                    UpdateWiiuClassic(_controllersList[i].controllerID, _useContinuousInput);
                    break;

                case ControllerType.WiiuGamepad:
                    UpdateWiiuGamepad(_controllersList[i].controllerID, _useContinuousInput);
                    break;

                case ControllerType.WiiUProController:
                    UpdateWiiuProController(_controllersList[i].controllerID, _useContinuousInput);
                    break;

                case ControllerType.Keyboard:
                    UpdateKeyboard(i, _useContinuousInput);
                    break;

                case ControllerType.KeyboardJoystick:
                    UpdateJoystick(i, _useContinuousInput);
                    break;

                default:
                    break;
            }
		}
    }
    #endregion

}



