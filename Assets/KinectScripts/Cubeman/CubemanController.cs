using UnityEngine;
using System;
using System.Collections;

public class CubemanController : MonoBehaviour 
{
	public double Threshold = 0.2;
	public bool MoveVertically = false;
	public bool MirroredMovement = false;

	public Vector3 HandRotation;
	public Vector3 SpineRotation;
	public Vector3 AvoidRotation;

	bool LeftPanch_temp = false;
	bool LeftKnee_temp = false;
	bool LeftUpper_temp = false;
	bool RightPanch_temp = false;
	bool RightKnee_temp = false;
	bool RightUpper_temp = false;
	bool RightAvoid_temp = false;
	bool LeftAvoid_temp = false;

	public bool LeftPanch_detected = false;
	public bool LeftKnee_detected = false;
	public bool LeftUpper_detected = false;
	public bool RightPanch_detected = false;
	public bool RightKnee_detected = false;
	public bool RightUpper_detected = false;
	public bool RightAvoid_detected = false;
	public bool LeftAvoid_detected = false;
	public bool DoubleUpper_detected = false;

	//public GameObject debugText;
	
	public GameObject Hip_Center;
	public GameObject Spine;
	public GameObject Shoulder_Center;
	public GameObject Head;
	public GameObject Shoulder_Left;
	public GameObject Elbow_Left;
	public GameObject Wrist_Left;
	public GameObject Hand_Left;
	public GameObject Shoulder_Right;
	public GameObject Elbow_Right;
	public GameObject Wrist_Right;
	public GameObject Hand_Right;
	public GameObject Hip_Left;
	public GameObject Knee_Left;
	public GameObject Ankle_Left;
	public GameObject Foot_Left;
	public GameObject Hip_Right;
	public GameObject Knee_Right;
	public GameObject Ankle_Right;
	public GameObject Foot_Right;

	public LineRenderer SkeletonLine;
	
	private GameObject[] bones; 
	private LineRenderer[] lines;
	private int[] parIdxs;
	
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Vector3 initialPosOffset = Vector3.zero;
	private uint initialPosUserID = 0;
	
	
	void Start () 
	{
		//store bones in a list for easier access
		bones = new GameObject[] {
			Hip_Center, Spine, Shoulder_Center, Head,  // 0 - 3
			Shoulder_Left, Elbow_Left, Wrist_Left, Hand_Left,  // 4 - 7
			Shoulder_Right, Elbow_Right, Wrist_Right, Hand_Right,  // 8 - 11
			Hip_Left, Knee_Left, Ankle_Left, Foot_Left,  // 12 - 15
			Hip_Right, Knee_Right, Ankle_Right, Foot_Right  // 16 - 19
		};

		parIdxs = new int[] {
			0, 0, 1, 2,
			2, 4, 5, 6,
			2, 8, 9, 10,
			0, 12, 13, 14,
			0, 16, 17, 18
		};
		
		// array holding the skeleton lines
		lines = new LineRenderer[bones.Length];
		
		if(SkeletonLine)
		{
			for(int i = 0; i < lines.Length; i++)
			{
				lines[i] = Instantiate(SkeletonLine) as LineRenderer;
				lines[i].transform.parent = transform;
			}
		}
		
		initialPosition = transform.position;
		initialRotation = transform.rotation;
		//transform.rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () 
	{
		KinectManager manager = KinectManager.Instance;

		// get 1st player
		uint playerID = manager != null ? manager.GetPlayer1ID() : 0;
		
		if(playerID <= 0)
		{
			// reset the pointman position and rotation
			if(transform.position != initialPosition)
			{
				transform.position = initialPosition;
			}
			
			if(transform.rotation != initialRotation)
			{
				transform.rotation = initialRotation;
			}
			
			for(int i = 0; i < bones.Length; i++) 
			{
				bones[i].gameObject.SetActive(true);
				
				bones[i].transform.localPosition = Vector3.zero;
				bones[i].transform.localRotation = Quaternion.identity;
				
				if(SkeletonLine)
				{
					lines[i].gameObject.SetActive(false);
				}
			}
			
			return;
		}

		// set the user position in space
		Vector3 posPointMan = manager.GetUserPosition(playerID);
		posPointMan.z = !MirroredMovement ? -posPointMan.z : posPointMan.z;
		
		// store the initial position
		if(initialPosUserID != playerID)
		{
			initialPosUserID = playerID;
			initialPosOffset = transform.position - (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));
		}
		
		transform.position = initialPosOffset + (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));

		// update the local positions of the bones
		for(int i = 0; i < bones.Length; i++) 
		{
			if(bones[i] != null)
			{
				int joint = MirroredMovement ? KinectWrapper.GetSkeletonMirroredJoint(i): i;
				
				if(manager.IsJointTracked(playerID, joint))
				{
					bones[i].gameObject.SetActive(true);
					
					Vector3 posJoint = manager.GetJointPosition(playerID, joint);
					posJoint.z = !MirroredMovement ? -posJoint.z : posJoint.z;

					Quaternion rotJoint = manager.GetJointOrientation(playerID, joint, !MirroredMovement);
					rotJoint = initialRotation * rotJoint;

					posJoint -= posPointMan;

					if(MirroredMovement)
					{
						posJoint.x = -posJoint.x;
						posJoint.z = -posJoint.z;
					}

					bones[i].transform.localPosition = posJoint;
					bones[i].transform.rotation = rotJoint;
				}
				else
				{
					bones[i].gameObject.SetActive(false);
				}
			}	
		}

		if(SkeletonLine)
		{
			for(int i = 0; i < bones.Length; i++) 
			{
				bool bLineDrawn = false;

				if(bones[i] != null)
				{
					if(bones[i].gameObject.activeSelf)
					{
						Vector3 posJoint = bones[i].transform.position;

						int parI = parIdxs[i];
						Vector3 posParent = bones[parI].transform.position;

						if(bones[parI].gameObject.activeSelf)
						{
							lines[i].gameObject.SetActive(true);
							
							//lines[i].SetVertexCount(2);
							lines[i].SetPosition(0, posParent);
							lines[i].SetPosition(1, posJoint);

							bLineDrawn = true;
						}
					}
				}	

				if(!bLineDrawn)
				{
					lines[i].gameObject.SetActive(false);
				}
			}
		}
		//ここから-----------------------------------------------------------------------------------------------

			//左手パンチ　＝　(6 Wrist_Left　左手首) - (4 Shoulder_Left	左肩) 0.5
		Vector3 LeftPanchVector = bones [6].transform.position - bones [4].transform.position;
		float len_LeftPanchVector = LeftPanchVector.sqrMagnitude;
			//左アッパー
		float len_LeftUpper = bones [6].transform.position.y - bones [4].transform.position.y;		
			//左ひざ打ち　＝　（14 Hip_Left）－（13 Knee_Left	左ひざ）
		float len_LeftKneeVector = bones [12].transform.position.y - bones [13].transform.position.y;

			//右手パンチ　＝　(10 Wrist_Right　右手首) - (8 Shoulder_Right　右肩)
		Vector3 RightPanchVector = bones [10].transform.position - bones [8].transform.position;
		float len_RightPanchVector = RightPanchVector.sqrMagnitude;
			//右アッパー
		float len_RightUpper = bones [10].transform.position.y - bones [8].transform.position.y;
			//右ひざ打ち　＝　（16 Hip_Right）－（17 Knee_Right）
		float len_RightKneeVector = bones [16].transform.position.y - bones [17].transform.position.y;

		//左パンチ＆左アッパー
		if (len_LeftUpper < 0.1) {
			if (len_LeftPanchVector < 0.5) {	//パンチ準備動作
				LeftPanch_temp = true;
				LeftPanch_detected = false;
			} else if (len_LeftPanchVector >= 0.5) {
				if (LeftPanch_temp == true) {
					LeftPanch_detected = true;		//パンチの検出
					Debug.Log ("Left_Panch_detected:" + LeftPanch_detected);
					LeftPanch_temp = false;
				} else {
					LeftPanch_detected = false;
					LeftPanch_temp = false;
				}
			} else {
				LeftPanch_detected = false;
				LeftPanch_temp = false;
			}
		} else {
			LeftPanch_detected = false;
			LeftPanch_temp = false;
			if (len_LeftUpper < 0.2) {
				LeftUpper_temp = true;
				LeftUpper_detected = false;
			} else {
				if (LeftUpper_temp == true) {
					LeftUpper_detected = true;		//アッパーの検出
					Debug.Log ("Left_Upper_detected:" + LeftUpper_detected);
					LeftUpper_temp = false;
				} else {
					LeftUpper_detected = false;
				}
			}
		}


		//左ひざ
		if (len_LeftKneeVector > 0.5) {
			LeftKnee_temp = true;
			LeftKnee_detected = false;
		} else {
			if (LeftKnee_temp == true) {
				LeftKnee_detected = true;
				LeftKnee_temp = false;
				Debug.Log ("Left_Knee_detected:"+LeftKnee_detected);

			} else {
				LeftKnee_detected = false;
			}
		}


		//右パンチ＆右アッパー
		if (len_RightUpper < 0.1) {
			if (len_RightPanchVector < 0.5) {	//パンチ準備動作
				RightPanch_temp = true;
				RightPanch_detected = false;
			} else if (len_RightPanchVector >= 0.5) {
				if (RightPanch_temp == true) {
					RightPanch_detected = true;		//パンチの検出
					Debug.Log ("Right_Panch_detected:" + RightPanch_detected);
					RightPanch_temp = false;
				} else {
					RightPanch_detected = false;
					RightPanch_temp = false;
				}
			} else {
				RightPanch_detected = false;
				RightPanch_temp = false;
			}
		} else {
			RightPanch_detected = false;
			RightPanch_temp = false;
			if (len_RightUpper < 0.2) {
				RightUpper_temp = true;
				RightUpper_detected = false;
			} else {
				if (RightUpper_temp == true) {
					RightUpper_detected = true;		//アッパーの検出
					Debug.Log ("Right_Upper_detected:" + RightUpper_detected);
					RightUpper_temp = false;
				} else {
					RightUpper_detected = false;
				}
			}
		}

		//右ひざ
		if (len_RightKneeVector > 0.5) {
			RightKnee_temp = true;
			RightKnee_detected = false;
		} else {
			if (RightKnee_temp == true) {
				RightKnee_detected = true;
				RightKnee_temp = false;
				Debug.Log ("Right_Knee_detected:"+RightKnee_detected);
			} else {
				RightKnee_detected = false;
			}
		}

		//kimisui---------------------------------------------------------------------------------
		Vector3 LeftHandPosition = bones[7].transform.position;		//左手のポジション
		Vector3 RightHandPosition = bones[11].transform.position;	//右手のポジション
		Vector3 v1 = LeftHandPosition - RightHandPosition;
		float v1radX = Mathf.Atan2(v1.y, v1.z);
		float v1DegX = v1radX * Mathf.Rad2Deg;	//X軸回転角
		float v1radY = Mathf.Atan2(v1.z, v1.x);
		float v1DegY = v1radY * Mathf.Rad2Deg;	//Y軸回転角
		float v1radZ = Mathf.Atan2(v1.y, v1.x);
		float v1DegZ = v1radZ * Mathf.Rad2Deg;	//Z軸回転角
		//Debug.Log("X:"+v1DegX);
		HandRotation = new Vector3(v1DegX, v1DegY, v1DegZ);

		Vector3 HipCenterPosition = bones[0].transform.position;		//腰のポジション
		Vector3 ShoulderCenterPosition = bones[2].transform.position;		//肩の中心のポジション
		Vector3 v2 = ShoulderCenterPosition - HipCenterPosition;
		float v2radX = Mathf.Atan2(v2.y, v2.z);
		float v2DegX = v2radX * Mathf.Rad2Deg;	//X軸回転角
		float v2DegX2 = v2DegX - 90f;			//９０度回転
		//Debug.Log("X:"+v2DegX2);
		SpineRotation = new Vector3 (v2DegX2,0,0);
		//-----------------------------------------------------------------------------------------

		//AvoidCube用------------------------------------------------------------------------------
		Vector3 v3 = bones[3].transform.position - bones[0].transform.position;		//3 Head - 0 HipCenter
		float v3radZ = Mathf.Atan2(v3.y, v3.x);
		float v3DegZ = v3radZ * Mathf.Rad2Deg;	//z軸回転
		//Debug.Log("AvoidRotation"+v3DegZ);
		AvoidRotation = new Vector3 (0,0,v3DegZ);

		//右回避
		if (v3DegZ > 80) {
			RightAvoid_temp = true;
			RightAvoid_detected = false;
		} else {
			if (RightAvoid_temp == true) {
				RightAvoid_detected = true;
				RightAvoid_temp = false;
				Debug.Log ("Right_Aviod_detected:"+RightAvoid_detected);

			} else {
				RightAvoid_detected = false;
			}
		}

		if (v3DegZ < 100) {
			LeftAvoid_temp = true;
			LeftAvoid_detected = false;
		} else {
			if (LeftAvoid_temp == true) {
				LeftAvoid_detected = true;
				LeftAvoid_temp = false;
				Debug.Log ("Left_Aviod_detected:"+LeftAvoid_detected);

			} else {
				LeftAvoid_detected = false;
			}
		}

		//------------------------------------------------------------------------------------------

		//DoubleUpper(ゲーム中断)用------------------------------------------------------------------

		if (len_RightUpper > 0.2 && len_LeftUpper > 0.2) {
			DoubleUpper_detected = true;
		} else {
			DoubleUpper_detected = false;
		}
			
		//----------------------------------------------------------------------------------------------
	}

}
