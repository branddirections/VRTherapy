using System.Collections;
using UnityEngine;
using SocketIO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;
//using UnityEditor;

public class VRTConnect : MonoBehaviour
{


	public GameObject playWithoutAppButton;
	public int selectedShootMode = 1;
	public GameObject exercisesFolder;
	public GameObject homeScreenFolder;
	public GameObject paperPlane;
	public GameObject laserPointer;
    public GameObject cameraReticle;
	public GameObject connectionSign;
	public GameObject signText;
	public JSONObject receivedExerciseSettings;
    public bool explodeObject = true;
	public bool explodeMoveToArm = false;
	public bool explodeDisappear = false;
	public bool soundOn = true; 
	public int exerciseId;
	
	
	private string playerDeviceId = "";
	private string receiverId = "";
	private int score = 0;
	private int pointsGoal = 0;
	private TextMesh textMesh;
	private SocketIOComponent VRTConnection;
	private string deviceIdentifier;
	private bool isRegisteringDevice = false;
	private bool showSignFlag;
	private string textSignMessage;
	private string deviceModel = "";
	private string deviceName = "";
	private string bundleId = "";
	private string appVersion = "";
	private string availableExercises = "";
	private string appName = "";
	public bool isChangingGameState = false;


	public void Start(){
		setupVRTherapy();

		//PlayWithoutApp(144097);
		

	}

	public void Update(){
		// CHECK TO DISPLAY SIGN ON MAIN THREAT
		if (showSignFlag) {
			textMesh.text = textSignMessage;
			showSignFlag = false;
		}
	}


	/// YOU CAN ADD INDIVIDUAL CODE BELOW DEPENDING ON WHAT YOU WANT TO DO WITH THE RECEIVED SETTINGS VALUE
	/// OPTION 1: Create a public GameObject and Drag it into the script which you can:
	/// paperPlane.gameObject.SetActive(true);

	/// OPTION 2: Target a specific public variable of that gameObject by: 
	/// VRTherapy.VRTSearchObjectSpawner gameScript = exerciseGO.GetComponent<VRTherapy.VRTSearchObjepawner>();

	/// OPTION 3: Find a gameObject Directly in the Execise Folder as one of its childs
	/// GameObject settingGameObject = exercisesFolder.transform.Find("-enter name here-").gameObject;

	public void setupShootMode(int shootModeId){
		// SETUP SHOOT MODES OF THIS EXERCISE 
			selectedShootMode = getIntegerOfSetting(shootModeId);

		    paperPlane.gameObject.SetActive(false);
			laserPointer.gameObject.SetActive(false);
			cameraReticle.gameObject.SetActive(false);
			
			if (selectedShootMode == 0) { paperPlane.gameObject.SetActive(true); } // PAPER PLANE
			else if (selectedShootMode == 1){ laserPointer.gameObject.SetActive(true);} // LASER POINTER
			else if (selectedShootMode == 2){ cameraReticle.gameObject.SetActive(true); } // LOOK 3 SECONDS
	}

	public void setupSelectionMode(int selectionModeId){
		// SETUP SHOOT MODES OF THIS EXERCISE 
		/// START SETTING: OBJECT SELECTION TYPE - EXPLODE, DISAPPEAR, MOVETOPLAYER
			    int selectedExplosionMode = getIntegerOfSetting(selectionModeId);

				// Reset Everything	
				explodeObject = false;
				explodeDisappear = false;
				explodeMoveToArm = false;
				// Set parameters
				if (selectedExplosionMode == 0) { explodeObject = true; }
				else if (selectedExplosionMode == 1){ explodeDisappear = true;}
			    else{ explodeMoveToArm = true;	}
		
	}

	public void UpdateExercise(SocketIOEvent e)
	{
		
	

		
		// NEW EMPTY EXERCISE
		if (exerciseId == 484892){ 

			setupShootMode(9260310);  // Paper Plane, Laser Pointer, Look 3 Seconds
			setupSelectionMode(9260310); // Explode, Disappear, Fall down

			GameObject exerciseFolder = exercisesFolder.transform.Find(exerciseId.ToString()).gameObject;
			
			/// STEP 1: ATTACH A EMPTY VRT EXERCISE SCRIPT AS A CHILD TO YOUR EXERCISE
			VRTExercise MyExerciseScript = exerciseFolder.GetComponent<VRTExercise>();
		
			/// STEP 2: RESET EXERCISE & REMOVE ALL PREVIOUS OBJECTS THAT WERE CREATED IN THE LAST EXERCISE SESSION 
			MyExerciseScript.resetExercise();
			
						// OPTIONAL TIP: FIND ANOTHER GAME OBJECT BY NAME IN YOUR EXERCISE FOLDER
						/* string objectName = "MyObject";
						GameObject shootGeneratorGameObject = exerciseFolder.transform.Find(objectName).gameObject; */
					
						// FIND A ELEMENT OR SCRIPT ATTACHED TO A GAME OBJECT IN YOUR EXERCISE FOLDER
						// VRTExercise MyExerciseScript = exerciseFolder.GetComponent<VRTExercise>();
		
			/// STEP 3: NOW APPLY THE SETTINGS YOU WANT TO CHANGE WHEN YOUR RECEIVE AN UPDATE FROM THE VR THERAPY APP

			/*	/// LEFT RIGHT TOP BOTTOM ANGLES
				int leftAngle = getAngleOfSetting(3843528, "left"); // top
				int rightAngle = getAngleOfSetting(3843528, "right"); // bottom
				
				/// ON/OFF SWITCH
		     	bool trueOrFalseSetting = getBoolOfSetting(5457290);

		     	/// INTEGER OF SETTING LIKE SINGLE SELECTION, STEPPER VALUE, SLIDER VALUE
				int sliderValue = getIntegerOfSetting(9167277); // -> Returns the number selected in the slider
				int stepperValue = getIntegerOfSetting(9167277); // -> Returns the number selected in the stepper
				int singleSelectionIndex = getIntegerOfSetting(9167277); // -> Returns the index selected in the slider. Remember the first selected item has potition 0, second item 1 etc.
				
				/// MULTIPLE SELECTION
				arr multipleSelectedIndexes = getMultipleSelectedIndexesOfSetting(9167277) -> Returns an array of boolean values that are true (selected) or false (not selected)

	*/
			/// STEP 4: CALL RELOAD EXERCISE FLAG NOW IN YOUR EXERCISE SCRIPT
			MyExerciseScript.reloadExercise();



		}  
	    


		if (exerciseId == 144097){ 


			
			setupShootMode(9167277);  // Paper Plane, Laser Pointer, Look 3 Seconds
			setupSelectionMode(6656914); // Explode, Disappear, Fall down


 			GameObject exerciseFolder = exercisesFolder.transform.Find(exerciseId.ToString()).gameObject;
			GameObject shootGeneratorGameObject = exerciseFolder.transform.Find("VRTSearchObjectsGenerator").gameObject;
			VRTherapy.VRTSearchObjectSpawner VRTSearchObjectGenerator = shootGeneratorGameObject.GetComponent<VRTherapy.VRTSearchObjectSpawner>();
			/// REMOVE ALL EXISTING OBJECTS TO START EMPTY
			VRTSearchObjectGenerator.resetExercise();

			/// POINTS GOAL TO REACH = AMOUNT OF OBJECTS TO BE SPAWNED: ID 5209649
			VRTSearchObjectGenerator.AmountOfExplodableObjects = getIntegerOfSetting(5209649);
			pointsGoal = getIntegerOfSetting(5209649);
			
			/// SETTING: BALLOONS OR CANDIES
				int selectedShootElement = getIntegerOfSetting(9386955);
				// Reset Everything	
				VRTSearchObjectGenerator.searchBalloons = false;
				VRTSearchObjectGenerator.searchCandies = false;
				// Set parameters
				if(selectedShootElement == 0) { VRTSearchObjectGenerator.searchBalloons = true;}
				else if(selectedShootElement == 1){VRTSearchObjectGenerator.searchCandies = true;}
				
			///  SETTING: SEARCH OBJECTS OF COLOR
			int selectedShootElementColor = getIntegerOfSetting(1034301);
			
			// Reset Everything	
				VRTSearchObjectGenerator.findMagentaObjects = false;
				VRTSearchObjectGenerator.findYellowObjects = false;
				VRTSearchObjectGenerator.findBlueObjects = false;
			// Set parameters	
			if (selectedShootElementColor == 0) { VRTSearchObjectGenerator.findMagentaObjects = true; }
			if (selectedShootElementColor == 1) { VRTSearchObjectGenerator.findYellowObjects = true; }
			if (selectedShootElementColor == 2) { VRTSearchObjectGenerator.findBlueObjects = true; }

			/// DISTANCE: ID 5974305  MIN_SPAWN_RADIUS MAX_SPAWN_RADIUS
				int objectDistance = getIntegerOfSetting(5974305);

				float minDistance = 8f;
				float maxDistance = 16f;
				if (objectDistance == 0) { minDistance = 5f; maxDistance = 8f; }
				else if (objectDistance == 1) { minDistance = 8f; maxDistance = 16f; }
				else if (objectDistance == 2) { minDistance = 12f; maxDistance = 20f; }
				else if (objectDistance == 3) { minDistance = 10f; maxDistance = 10f; }
				VRTSearchObjectGenerator.MIN_SPAWN_RADIUS = minDistance;
				VRTSearchObjectGenerator.MAX_SPAWN_RADIUS = maxDistance;
			

			/// LEFT RIGHT OBJECT DISTRIBUTION BY ANGLE: ID 8510378
			VRTSearchObjectGenerator.LeftAngle = getAngleOfSetting(3843528, "left");
			VRTSearchObjectGenerator.RightAngle = getAngleOfSetting(3843528, "right");
			
			/// GENERATE DISTRACTION OBJECTS: ID 5457290
	     	bool showDistractionObjects = getBoolOfSetting(5457290);
		    if (showDistractionObjects) { VRTSearchObjectGenerator.AmountOfDistractionObjects = 20; }else{ VRTSearchObjectGenerator.AmountOfDistractionObjects = 0; }

			// All Values are loaded: RELOAD THE GAME SCRIPT VIA RELOAD FLAG
			VRTSearchObjectGenerator.reloadExercise();
		
		}
	}





	/// DONT CHANGE ANYTHING BELOW UNLESS YOU KNOW WHAT YOU ARE DOING :) !

	public void SoundSetting(SocketIOEvent e)
	{
		Debug.Log("Sound is " + e.data.GetField("soundOn") + e.data.GetField("soundOn").ToString());
		soundOn = false; 
		if(e.data.GetField("soundOn").ToString().Equals("true")) { soundOn = true; }
		Debug.Log("Sound setting changes to: " + soundOn);

	}

	public void StartExercise(SocketIOEvent e)
	{
		exerciseId = int.Parse(e.data.GetField("gameId").ToString());
		playWithoutAppButton.gameObject.SetActive(false);
		exerciseStarted(exerciseId);
		receivedExerciseSettings = e.data;
		UpdateExercise(e);
	}

    public void PlayWithoutApp(int exerciseId){
    	if(!isChangingGameState){
    	isChangingGameState = true; 
		exerciseStarted(exerciseId);
		}
	}

	public void exerciseStarted(int exerciseId){

		Sequence mySequence = DOTween.Sequence();
		//mySequence.Append(homeScreenFolder.transform.DOMoveY(-10, 2));
		mySequence.Append(homeScreenFolder.transform.DOScale(0, 1));
		mySequence.AppendCallback(finishedChangingGameState);
		

		foreach (Transform child in exercisesFolder.gameObject.transform){ 
		if (child.name == exerciseId.ToString()){child.gameObject.SetActive(true);
		 }else{	child.gameObject.SetActive(false);	}
	}
	}

	public void StopExercise(SocketIOEvent e){
		playWithoutAppButton.gameObject.SetActive(true);
		exerciseStopped();
	}

	public void StopWithoutApp(int exerciseId){
		if(!isChangingGameState){
    	isChangingGameState = true; 
		exerciseStopped();
		}
	}

	public void exerciseStopped(){
		
		Sequence mySequence = DOTween.Sequence();
		//mySequence.Append(homeScreenFolder.transform.DOMoveY(0, 1));
		mySequence.Append(homeScreenFolder.transform.DOScale(1, 1));
		mySequence.AppendCallback(finishedChangingGameState);
	
		ResetScore();
		foreach (Transform child in exercisesFolder.gameObject.transform){
				child.gameObject.SetActive(false);
		}

	}

	private void finishedChangingGameState(){
		isChangingGameState = false;
	}

	public void PauseExercise(SocketIOEvent e){
	Debug.Log("Pause exercise with ID: " + e.data.GetField("gameId"));
	}

	public int getIntegerOfSetting(int settingId) 
	{ return getIntSettingOnPos(0, settingId); }

    public int getAngleOfSetting(int settingId, string angleType) // angleType: left, right, top, bottom
	{ 	int angle = 0; if(angleType == "right" || angleType == "bottom") { angle = 1; }
		return getIntSettingOnPos(angle, settingId);
	}

    public bool getBoolOfSetting(int settingId) { //bool showDistractionObjects = getBooleanSeetingOf(5457290)
	int receivedValue = getIntSettingOnPos(0, settingId);
	bool booleanValue = false; if (receivedValue == 1) { booleanValue = true; }
	return booleanValue;
    }

	public ArrayList getMultipleSelectedIndexesOfSetting(int settingId){ // ArrayList with boolean values 
      ArrayList arr = new ArrayList();
	  for (int i = 0; i < parseSetting(settingId).Length; i++) { if (getIntSettingOnPos(i, settingId) == 1) { arr.Add((bool)true); } else { arr.Add((bool)false); } }
	  return arr; 
	}

	public string[] parseSetting(int settingId){
		return receivedExerciseSettings.GetField((settingId.ToString())).str.Split(new string[] { "," }, System.StringSplitOptions.None);
	}

	public int getIntSettingOnPos(int index, int settingId){
	return int.Parse(parseSetting(settingId)[index]);
	}

	public void ResetScore(){
		score = 0;
	}

	public void IncreaseScoreBy(int points){
	score = score + points;
    Dictionary<string, string> data = new Dictionary<string, string>();
	data["playerDeviceId"] = playerDeviceId;
	data["trainerDeviceId"] = receiverId;
	data["pointsIncrease"] = points.ToString();
	data["exerciseId"] = exerciseId.ToString();
	data["pointsGoal"] = pointsGoal.ToString();
	VRTConnection.Emit("increasePoints", new JSONObject(data));
	}

	public void MessageToGame(SocketIOEvent e)
	{
		if (e.data == null) { return; }
		if (e.data.GetField("gameId") != null && e.data.GetField("task") != null){
			JSONObject j = e.data;
			string task = j.GetField("task").str;

			if (task.Equals("start")){ StartExercise(e);}
			else if (task.Equals("stop")){ StopExercise(e);}
			else if (task.Equals("pause")){ PauseExercise(e);}
			else if (task.Equals("sound")){ SoundSetting(e);}
			else if (task.Equals("update")){ receivedExerciseSettings = e.data; UpdateExercise(e);}
		}
	}

	public void ConnectToApp(SocketIOEvent e){
		if (e.data == null) { return; }
		receiverId = e.data.GetField("senderId").str;

		Dictionary<string, string> data = new Dictionary<string, string>();
		data["receiverId"] = receiverId;
		data["senderId"] = playerDeviceId;
		data["bundleId"] = bundleId; 
		data["bundleVersion"] = appVersion; 
		data["appName"] = appName; 
		data["availableExercises"] = availableExercises; 
		data["message"] = "connected";

        showSign("Connected!\nStart Exercise via Trainer App.\nDevice ID: " + playerDeviceId);

		VRTConnection.Emit("messageFromGame", new JSONObject(data));
		Debug.Log("sending confirmation of connection out");
		Debug.Log("Connecting to app with id of user: " + e.data.GetField("senderId").str);
	}

	public void OpenConnection(SocketIOEvent e){
		Debug.Log("[VRTherapy] CONNECTION TO VRT SERVER IS ESTABLISHED: " + e.name + " " + e.data);
		connectUserToServer();
	}

	public void ConnectionError(SocketIOEvent e){
		Debug.Log("Error received: " + e.name + " " + e.data);
	}

	public void ConnectionClosed(SocketIOEvent e){
		Debug.Log("Close received: " + e.name + " " + e.data);
		connectUserToServer();
	}


	public void showSign(string m) {
		textSignMessage = m;
		showSignFlag = true;
  	}

	public void hideConnectionSign() {
		connectionSign.gameObject.SetActive(false);
	}

	public void connectUserToServer() {
		if (playerDeviceId.Equals(""))
		{
			// NO DEVICE ID EXISTS
			if (!isRegisteringDevice)
			{	registerDevice(); }
		} else {
            showSign("Connecting...\nWaiting for trainer app\nDevice ID: " + playerDeviceId);
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["userId"] = playerDeviceId;

			data["bundleId"] = bundleId; 
			data["bundleVersion"] = appVersion; 
			data["appName"] = appName; 
			
			data["availableExercises"] = availableExercises; 
			
			Debug.Log("Id:" +  bundleId + " app version:" + appVersion + " AVAILABLE exercises: " + availableExercises);

			Debug.Log("[VRTherapy] DEVICE IS REGISTERED WITH DEVICE ID : " + playerDeviceId);
			VRTConnection.Emit("connectUser", new JSONObject(data));
		}
	}

	public void registerDevice() {
        showSign("Setting up device...");
		isRegisteringDevice = true;
		Debug.Log("[VRTherapy] REGISTERING NEW DEVICE WITH DEVICE ID : " + deviceIdentifier);
		Dictionary<string, string> data = new Dictionary<string, string>();
		data["deviceIdentifier"] = deviceIdentifier;
		data["deviceModel"] = deviceModel;
		data["deviceName"] = deviceName;
		VRTConnection.Emit("registerDevice", new JSONObject(data));
	}

	public void registerDeviceResponse(SocketIOEvent data) {
		JSONObject response = data.data;
			string responseType = response.GetField("responseType").str;
		
		if (response.GetField("responseType") != null){
			string deviceId = response.GetField("deviceId").str;
	        Debug.Log("[VRTherapy] Register Device Response received: " + responseType + " - device ID: " + deviceId);

			if (responseType.Equals("ok")){
				PlayerPrefs.SetString("VrDeviceId2", deviceId);
				playerDeviceId = deviceId;
				connectUserToServer();
			    showSign("To connect this device with\nthe VR Therapy iOS App\nenter this connection code:\n" + playerDeviceId);

			}else if (responseType.Equals("error")){
                 showSign("Device could not connect.\nDevice ID: " + playerDeviceId);

			}else if (responseType.Equals("exists")){
				PlayerPrefs.SetString("VrDeviceId2", deviceId);
				playerDeviceId = deviceId;
                connectUserToServer();
                showSign("To connect this device with\nthe VR Therapy iOS App\nenter this connection code:\n" + playerDeviceId);
			}

			isRegisteringDevice = false;
		}
	}

	public void setupVRTherapy(){

		// bundleId = PlayerSettings.applicationIdentifier; 
		// appVersion = PlayerSettings.bundleVersion;
		// appName = PlayerSettings.productName;
		
		 availableExercises = "";
		foreach (Transform child in exercisesFolder.gameObject.transform){ availableExercises += child.name + ","; }
	
		receivedExerciseSettings = new JSONObject(new Dictionary<string, string>());
		textMesh = signText.GetComponent<TextMesh>();
		deviceModel = SystemInfo.deviceModel;
		deviceName = SystemInfo.deviceName;
		playerDeviceId = PlayerPrefs.GetString("VrDeviceId2", "");
        deviceIdentifier = SystemInfo.deviceUniqueIdentifier;

		VRTConnection = FindObjectOfType<SocketIOComponent>();
		VRTConnection.On("open", OpenConnection);
		VRTConnection.On("error", ConnectionError);
		VRTConnection.On("close", ConnectionClosed);
		VRTConnection.On("messageToGame", MessageToGame);
		VRTConnection.On("connectToApp", ConnectToApp);
		VRTConnection.On("registerDeviceResponse", registerDeviceResponse);
		        
		foreach (Transform child in exercisesFolder.gameObject.transform)	{
		child.gameObject.SetActive(false);
		}
	}




}