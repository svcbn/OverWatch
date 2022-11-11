using UnityEngine;
using System.Collections;

public class TurretRotate : MonoBehaviour
{
	//turret looks player by rotation
	public Transform turretRotatePivot;
	Transform lookTarget;
	private Quaternion lookRotation;
	private Vector3 lookDir;
	float lookRotSpeed = 10;

	//laser looks player by rotation
	private Quaternion lookRotation2;
	private Vector3 lookDir2;
	float lookRotSpeed2 = 10;
	public Transform laserPivot;

	//turret move
	public GameObject PylonElement;
	public Transform targetPylonElement;

	//turret move
	public GameObject TurretSelf;
	public Transform targetTurretSelf;

	//Gun move
	public GameObject GunLeftOriginPos;
	public GameObject GunRightOriginPos;
	public Transform targetGunLeftPos;
	public Transform targetGunRightPos;

	//pivot for rotation
	public Transform rotationPivot;
	public Transform turretKnockback;
	public Transform gunKnockbackLeft;
	public Transform gunKnockbackRight;

	//position of gun fire
	public Transform muzzleLeft;
	public Transform muzzleRight;

	//Line Renderer on/off
	[SerializeField]
	private LineRenderer line;

	//bullet prefab
	public GameObject turretBullet;
	GameObject turretBulletLeft;
	GameObject turretBulletRight;

	//Gun Fire
	float currentTime;
	float fireDelay = 0.256f;

	//TurretBullet effect when fire FLASH
	//public ParticleSystem turretBulletFire;
	public GameObject explosion;

	//Fire Alternativley
	bool alternateFire = true;

	//bool for Iron Man 
	bool first = true;


	// Use this for initialization
	void Start()
	{
		//turret Rotate target is PLAYER!
		lookTarget = GameObject.Find("Player").transform;
	}
    
    // Update is called once per frame
    void Update()
	{
		//BEFORE SHOOTING
		if(first)
        {
			StartCoroutine(IronManSequence());
		}
		//SHOOTING
		if (!first)
		{
			currentTime += Time.deltaTime;

			if (currentTime > fireDelay)
			{
				//SHOOTING LEFT
				if (alternateFire)
				{
					turretBulletLeft = Instantiate(turretBullet, muzzleLeft.position, muzzleLeft.rotation);

					GameObject flash = Instantiate(explosion);
					flash.transform.position = muzzleLeft.position;
					Destroy(flash, 0.2f);


					//left gun knockback 
					StartCoroutine(GunRotSequenceLeft());
					
					alternateFire = false;
					GameObject.Destroy(turretBulletLeft, 1.5f);
				}
				//SHOOTING RIGHT
				else
				{
					turretBulletRight = Instantiate(turretBullet, muzzleRight.position, muzzleRight.rotation);

					GameObject flash = Instantiate(explosion);
					flash.transform.position = muzzleRight.position;
					Destroy(flash, 0.2f);

					//right gun knockback 
					StartCoroutine(GunRotSequenceRight());

					alternateFire = true;
					GameObject.Destroy(turretBulletRight, 1.5f);
				}

				//turret knockback
				StartCoroutine(TurretRotSequence());
				currentTime = 0;
			}
		}
	}

    private void LateUpdate()
    {

		if (!first)
		{
			//find the direction of player
			lookDir = (lookTarget.position - turretRotatePivot.transform.position).normalized;
			//prevent turret from looking player up or down, making turret rotates only left or right
			lookDir.y = 0;
			//create the rotation 
			lookRotation = Quaternion.LookRotation(lookDir);
			//rotate over time according to speed until we are in the required rotation
			turretRotatePivot.transform.rotation = Quaternion.Slerp(turretRotatePivot.transform.rotation, lookRotation, Time.deltaTime * lookRotSpeed);
		}
	}

	void laserPointPlayer(Transform turretRotatePivot2, Transform lookTarget2)
    {
		//find the direction of player
		lookDir2 = (lookTarget2.position - turretRotatePivot2.transform.position).normalized;
		//create the rotation 
		lookRotation2 = Quaternion.LookRotation(lookDir2);
		//rotate over time according to speed until we are in the required rotation
		turretRotatePivot2.transform.rotation = Quaternion.Slerp(turretRotatePivot2.transform.rotation, lookRotation2, Time.deltaTime * lookRotSpeed2);
	}

    //Turret Iron Man Sequence Coroutine motivated by Iron Man
    IEnumerator IronManSequence()
    {
		yield return StartCoroutine(PylonElementDown(targetPylonElement, 0.5f));
		yield return StartCoroutine(TurretUp(targetTurretSelf,0.5f));
		yield return StartCoroutine(GunForward(targetGunLeftPos, targetGunRightPos, 0.5f));

		//enable laser beam
		line.enabled = true;
		//line points player
		//laserPointPlayer(laserPivot, lookTarget);

		//change bool false to start SHOOTING
		first = false;
	}

	IEnumerator PylonElementDown(Transform targetPosition, float duration)
	{
		float time = 0;
		//Vector3 startPosition = TurretSelf.transform.position;
		while (time < duration)
		{
			PylonElement.transform.position = Vector3.Lerp(PylonElement.transform.position, targetPosition.position, time / duration);
			time += Time.deltaTime;
			yield return null;
		}
		PylonElement.transform.position = targetPosition.position;
	}

	IEnumerator TurretUp(Transform targetPosition, float duration)
	{
		float time = 0;
		//Vector3 startPosition = TurretSelf.transform.position;
		while (time < duration)
		{
			TurretSelf.transform.position = Vector3.Lerp(TurretSelf.transform.position, targetPosition.position, time / duration);
			time += Time.deltaTime;
			yield return null;
		}
		TurretSelf.transform.position = targetPosition.position;
	}

	IEnumerator GunForward(Transform GunLeftTargetPos, Transform GunRightTargetPos, float duration)
	{
		float time = 0;
		//Vector3 startPosition = TurretSelf.transform.position;
		while (time < duration)
		{
			GunLeftOriginPos.transform.position = Vector3.Lerp(GunLeftOriginPos.transform.position, GunLeftTargetPos.position, time / duration);
			GunRightOriginPos.transform.position = Vector3.Lerp(GunRightOriginPos.transform.position, GunRightTargetPos.position, time / duration);

			time += Time.deltaTime;
			yield return null;
		}
		GunLeftOriginPos.transform.position = GunLeftTargetPos.position;
		GunRightOriginPos.transform.position = GunRightTargetPos.position;
	}



	//Sequence Coroutine for Turret and Gun Knockback (used rotation)
	IEnumerator TurretRotSequence()
    {
		yield return StartCoroutine(RotateFunction(turretKnockback, 5, new Vector3(0, 0, 1)));
		yield return StartCoroutine(RotateBackFunction(turretKnockback, 5, new Vector3(0, 0, -1)));
	}

	IEnumerator GunRotSequenceLeft()
	{
		yield return StartCoroutine(GunRotateFunctionLeft(gunKnockbackLeft, 5, new Vector3(0, 0, 1)));
		yield return StartCoroutine(GunRotateBackFunctionLeft(gunKnockbackLeft, 5, new Vector3(0, 0, -1)));
	}

	IEnumerator GunRotSequenceRight()
	{
		yield return StartCoroutine(GunRotateFunctionRight(gunKnockbackRight, 5, new Vector3(0, 0, 1)));
		yield return StartCoroutine(GunRotateBackFunctionRight(gunKnockbackRight, 5, new Vector3(0, 0, -1)));
	}




	//Turret Knockback
	IEnumerator RotateFunction(Transform pivotPos, float degree, Vector3 vectorOption)
	{
		float timeSinceStarted = 0f;
		while (true)
		{
			timeSinceStarted += Time.deltaTime * 1f;
			pivotPos.transform.Rotate(vectorOption, degree * Time.deltaTime);

			// If the object has arrived, stop the coroutine
			if (timeSinceStarted >= 0.1f)
			{
				yield break;
			}

			// Otherwise, continue next frame
			yield return null;
		}
	}

	IEnumerator RotateBackFunction(Transform pivotPos, float degree, Vector3 vectorOption)
	{
		float timeSinceStarted = 0f;
		while (true)
		{
			timeSinceStarted += Time.deltaTime * 1f;
			pivotPos.transform.Rotate(vectorOption, degree * Time.deltaTime);

			// If the object has arrived, stop the coroutine
			if (timeSinceStarted >= 0.1f)
			{
				yield break;
			}

			// Otherwise, continue next frame
			yield return null;
		}
	}

	//Gun Knockback
	//Gun Left
	IEnumerator GunRotateFunctionLeft(Transform pivotPos, float degree, Vector3 vectorOption)
	{
		float timeSinceStarted = 0f;
		while (true)
		{
			timeSinceStarted += Time.deltaTime * 1f;
			pivotPos.transform.Rotate(vectorOption, degree * Time.deltaTime);

			// If the object has arrived, stop the coroutine
			if (timeSinceStarted >= 0.1f)
			{
				yield break;
			}

			// Otherwise, continue next frame
			yield return null;
		}
	}

	IEnumerator GunRotateBackFunctionLeft(Transform pivotPos, float degree, Vector3 vectorOption)
	{
		float timeSinceStarted = 0f;
		while (true)
		{
			timeSinceStarted += Time.deltaTime * 1f;
			pivotPos.transform.Rotate(vectorOption, degree * Time.deltaTime);

			// If the object has arrived, stop the coroutine
			if (timeSinceStarted >= 0.1f)
			{
				yield break;
			}

			// Otherwise, continue next frame
			yield return null;
		}
	}



	//Gun Right
	IEnumerator GunRotateFunctionRight(Transform pivotPos, float degree, Vector3 vectorOption)
	{
		float timeSinceStarted = 0f;
		while (true)
		{
			timeSinceStarted += Time.deltaTime * 1f;
			pivotPos.transform.Rotate(vectorOption, degree * Time.deltaTime);

			// If the object has arrived, stop the coroutine
			if (timeSinceStarted >= 0.1f)
			{
				yield break;
			}

			// Otherwise, continue next frame
			yield return null;
		}
	}

	IEnumerator GunRotateBackFunctionRight(Transform pivotPos, float degree, Vector3 vectorOption)
	{
		float timeSinceStarted = 0f;
		while (true)
		{
			timeSinceStarted += Time.deltaTime * 1f;
			pivotPos.transform.Rotate(vectorOption, degree * Time.deltaTime);

			// If the object has arrived, stop the coroutine
			if (timeSinceStarted >= 0.1f)
			{
				yield break;
			}

			// Otherwise, continue next frame
			yield return null;
		}
	}

}
