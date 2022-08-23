using MikrosClient;
using MikrosClient.Analytics;
using MikrosClient.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Script for testing of sample app with Mikros SDK.
/// </summary>
public class AuthenticationScreen : ScreenBase
{
	//[SerializeField] private Text mikrosUsernameText;
	[SerializeField] private GameObject authButtons;
	[SerializeField] private InputField signinUsernameEmailInput, signinPasswordInput;
	[SerializeField] private InputField signupUsernameInput, signupEmailInput, signupPasswordInput;
	[SerializeField] private GameObject signoutButton;
	[SerializeField] private Text suggestionText;
	[SerializeField] private Toggle enableSpecialCharacterToggle;
	private bool enableSpecialCharacterUsername;

    private void OnEnable()
    {
		screenTime = Time.time;
		enableSpecialCharacterToggle.onValueChanged.AddListener(HandleToggleChange);
		SetToggleStatus(PlayerPrefs.GetInt(Constants.specialCharacterUsername, 1) == 1);
	}

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	private void Start()
	{
		ShowSignOutButton();
	}
		
	/// <summary>
	/// Show sign out button based on if signed in or not
	/// </summary> 
	public void ShowSignOutButton()
	{
		signoutButton.SetActive(!Equals(MikrosManager.Instance.AuthenticationController.MikrosUser, null));
	}

	

	/// <summary>
	/// Perform Mikros Signin.
	/// </summary>
	/// <param name="username">Username.</param>
	/// <param name="email">Email.</param>
	/// <param name="password">Password.</param>
	/// <param name="callback">Signin success callback.</param>
	private void MikrosSignin(string username, string email, string password, UnityAction callback = null)
	{
		SigninRequest.Builder()
			.Username(username)
			.Email(email)
			.Password(password)
			.Create(signinRequest =>
			{
				Debug.Log("All input correct.");

				MikrosManager.Instance.AuthenticationController.Signin(signinRequest, successResponse =>
				{
					callback?.Invoke();
					OnAuthSuccess(successResponse);
				});
			},
			RequestBuilderFailure);
	}

	/// <summary>
	/// Perform Mikros Signup.
	/// </summary>
	/// <param name="username">Username.</param>
	/// <param name="email">Email.</param>
	/// <param name="password">Password.</param>
	/// <param name="callback">Signup success callback.</param>
	private void MikrosSignup(string username, string email, string password, UnityAction callback = null)
	{
		SignupRequest.Builder()
			.Username(username)
			.Email(email)
			.Password(password)
			.EnableUsernameSpecialCharacters(enableSpecialCharacterUsername)
			.Create(signupRequest =>
			{
				Debug.Log("All input correct.");

				MikrosManager.Instance.AuthenticationController.Signup(signupRequest, successResponse =>
				{
					callback?.Invoke();
					OnAuthSuccess(successResponse);
				});
			},
			RequestBuilderFailure);
	}

	/// <summary>
	/// Perform Mikros Signout.
	/// </summary>
	/// <param name="callback">Signout success callback.</param>
	private void MikrosSignout(UnityAction callback = null)
	{
		MikrosManager.Instance.AuthenticationController.Signout(() =>
		{
			callback?.Invoke();
			OnAuthSuccess(null);
		},
		RequestBuilderFailure);
	}

	/// <summary>
	/// Handle API error.
	/// </summary>
	/// <param name="customExceptions">Exception occured.</param>
	private void RequestBuilderFailure(MikrosException customExceptions)
	{
		Instruction.Instance.HideLoader();
		Instruction.Instance.ShowPopup(customExceptions.Message);
	}

	/// <summary>
	/// Callback for handling auth success from Mikros SSO.
	/// </summary>
	/// <param name="mikrosUser">Mikros user instance.</param>
	private void OnAuthSuccess(MikrosUser mikrosUser)
	{
		Instruction.Instance.HideLoader();
		Instruction.Instance.ShowPopup(mikrosUser == null ? "Signed out" : ("Auth Success: " + mikrosUser?.Email));
		if(!Equals(mikrosUser, null))
		{
			ScreenManager.Instance.AddNewMikrosUser(mikrosUser);
			if (!PlayerPrefs.HasKey(mikrosUser?.Email))
			{
				PlayerPrefs.SetString(mikrosUser.Email, Utility.GetRandomAlphanumericSequence());
			}
		}
		ShowSignOutButton();
	}

	/// <summary>
	/// Button click tasks for Signin.
	/// </summary>
	/// <param name="signinPanel">UI panel to activate after signin success.</param>
	public void OnClickSignin(GameObject signinPanel)
	{
		Instruction.Instance.ShowLoader();
		string username = "", email = "";
		(username, email) = Utils.AuthUtils.DetermineUsernameOrEmail(signinUsernameEmailInput.text, true);
		MikrosSignin(username, email, signinPasswordInput.text, () =>
		{
			signinPanel.SetActive(false);
			signoutButton.SetActive(true);
			authButtons.SetActive(true);
		});
	}

	/// <summary>
	/// Button click tasks for Signup.
	/// </summary>
	/// <param name="signupPanel">UI panel to activate after signup success.</param>
	public void OnClickSignup(GameObject signupPanel)
	{
		Instruction.Instance.ShowLoader();
		MikrosSignup(signupUsernameInput.text, signupEmailInput.text, signupPasswordInput.text, () =>
		{
			signupPanel.SetActive(false);
			signoutButton.SetActive(true);
			authButtons.SetActive(true);
		});
	}

	/// <summary>
	/// Button click tasks for Signout.
	/// </summary>
	/// <param name="userChoicePanel">UI panel to activate after signout success.</param>
	public void OnClickSignout(GameObject userChoicePanel)
	{
		Instruction.Instance.ShowLoader();
		MikrosSignout(() =>
		{
			Instruction.Instance.HideLoader();
			signoutButton.SetActive(false);
			userChoicePanel.SetActive(true);
		});
	}
	/// <summary>
	/// Handle toggle change events.
	/// </summary>
	/// <param name="isOn">Status of toggle.</param>
	private void HandleToggleChange(bool isOn)
	{
		enableSpecialCharacterUsername = isOn;
		PlayerPrefs.SetInt(Constants.specialCharacterUsername, isOn ? 1 : 0);
	}
	/// <summary>
	/// Set toggle status without arising event.
	/// </summary>
	/// <param name="isOn">Status of toggle.</param>
	public void SetToggleStatus(bool isOn)
	{
		enableSpecialCharacterToggle.isOn = isOn;
	}

	private void OnDisable()
    {
		enableSpecialCharacterToggle.onValueChanged.RemoveAllListeners();
	}
}