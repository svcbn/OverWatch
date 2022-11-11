using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotFire : MonoBehaviour
{
	//rotating bot's gun(gun knockback)
	public Transform gunKnockbackLeft;
	public Transform gunKnockbackRight;

	//position of gun fire
	public Transform muzzleLeftBot;
	public Transform muzzleRightBot;

    //bullet prefab
    public GameObject botBullet;
    GameObject botBulletLeft;
    GameObject botBulletRight;

    //Gun Fire
    float currentTime;
	float fireDelay = 0.256f;

	//Fire Alternativley
	bool alternateFire = true;

	//Patrol
	public Transform[] waypoints;
	public Transform botBody;
	private int speed = 5;
	private int waypointIndex;
	private float dist;

	//Explosion effect(bot below)
	public GameObject botFlame;
	public Transform f1;
	public Transform f2;
	public Transform f3;
	public Transform f4;

	// Start is called before the first frame update
	void Start()
    {
		waypointIndex = 0;
		botBody.transform.LookAt(waypoints[waypointIndex].position);
	}

    // Update is called once per frame
    void Update()
    {

		//continuous flame below Bot
		GameObject flame = Instantiate(botFlame);
		flame.transform.position = f1.position;
		flame.transform.position = f2.position;
		flame.transform.position = f3.position;
		flame.transform.position = f4.position;
		Destroy(flame, 0.5f);


		dist = Vector3.Distance(botBody.transform.position, waypoints[waypointIndex].position);
		if (dist < 1f)
		{
			IncreaseIndex();
		}
		botBody.transform.Translate(Vector3.forward * speed * Time.deltaTime);

		currentTime += Time.deltaTime;

		if (true)
		{
			if (currentTime > fireDelay)
			{
				//SHOOTING LEFT
				if (alternateFire)
				{
					botBulletLeft = Instantiate(botBullet, muzzleLeftBot.position, muzzleLeftBot.rotation);

					StartCoroutine(GunRotSequenceLeft());

					alternateFire = false;
					GameObject.Destroy(botBulletLeft, 1f);
				}
				//SHOOTING RIGHT
				else
				{
					botBulletRight = Instantiate(botBullet, muzzleRightBot.position, muzzleRightBot.rotation);

					StartCoroutine(GunRotSequenceRight());

					alternateFire = true;
					GameObject.Destroy(botBulletRight, 1f);
				}

				currentTime = 0;
			}
		}
	}






	//Patrol
	void IncreaseIndex()
	{
		waypointIndex++;
		if (waypointIndex >= waypoints.Length)
		{
			waypointIndex = 0;
		}

		botBody.transform.LookAt(waypoints[waypointIndex].position);
	}






	//Gun Knockback Rotate Automation sequence
	IEnumerator GunRotSequenceLeft()
	{
		yield return StartCoroutine(GunRotateFunctionLeft(gunKnockbackLeft, 30, new Vector3(-1, 0, 0)));
		yield return StartCoroutine(GunRotateBackFunctionLeft(gunKnockbackLeft, 30, new Vector3(1, 0, 0)));
	}

	IEnumerator GunRotSequenceRight()
	{
		yield return StartCoroutine(GunRotateFunctionRight(gunKnockbackRight, 50, new Vector3(-1, 0, 0)));
		yield return StartCoroutine(GunRotateBackFunctionRight(gunKnockbackRight, 50, new Vector3(1, 0, 0)));
	}






	//Left Gun Rotate Co
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






	//Right Gun Rotate Co
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
