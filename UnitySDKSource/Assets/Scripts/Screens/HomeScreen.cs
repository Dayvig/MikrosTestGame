using MikrosClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : ScreenBase
{
	public Text mikrosUsernameText;

	private void Start()
	{
		ShowMikrosUsername();
	}
	private void OnEnable()
	{
		screenTime = Time.time;
	}

	/// <summary>
	/// Launch Mikros SSO panel.
	/// Creates an SSORequest object which allows special characters. Attaches the OnAuthSuccess method to it to be accessed later.
	/// Sends a sign-in request to the AuthenticationController, which displays a Sign-in from the Authentication Panel.
	/// </summary>
	public void OnClickMikrosSignin()
	{
		MikrosSSORequest mikrosSSORequest = MikrosSSORequest.Builder().EnableUsernameSpecialCharacters(true).MikrosUserAction(OnAuthSuccess).Create();
		MikrosManager.Instance.AuthenticationController.LaunchSignin(mikrosSSORequest);
	}

	/// <summary>
	/// Callback for handling auth success from Mikros SSO.
	/// Adds a new user if none exists. Shows the username if it does.
	/// </summary>
	/// <param name="mikrosUser">Mikros user instance.</param>
	private void OnAuthSuccess(MikrosUser mikrosUser)
	{
		Instruction.Instance.HideLoader();
		Instruction.Instance.ShowPopup(mikrosUser == null ? "Signed out" : ("Auth Success: " + mikrosUser?.Email));
		if (!Equals(mikrosUser, null))
		{
			ScreenManager.Instance.AddNewMikrosUser(mikrosUser);
			if (!PlayerPrefs.HasKey(mikrosUser?.Email))
			{
				PlayerPrefs.SetString(mikrosUser.Email, Utility.GetRandomAlphanumericSequence());
			}
			ShowMikrosUsername();
		}
	}

	/// <summary>
	/// Show the Mikros user's username.
	/// </summary>
	private void ShowMikrosUsername()
	{
		mikrosUsernameText.text = MikrosManager.Instance.AuthenticationController.MikrosUser?.Email;
		
	}
}