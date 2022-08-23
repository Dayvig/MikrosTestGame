using MikrosClient;
using MikrosClient.Analytics;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnitTestConstants;

namespace Tests
{
    public class AuthTestSuite
    {
        #region Mikros Sign-in Test-case parameters

        private static object signinUsername = new object[] { "naskar3", "", "unknown" };
        private static object signinEmail = new object[] { "naskar3@test.com", "", "unknown@test.com" };
        private static object signinPassword = new object[] { "abcd1234", "wrong_password", "" };

        #endregion Mikros Sign-in Test-case parameters

        #region Mikros Sign-up Test-case parameters

        private static object signupUsername = new object[] { "naskar3", "shit", "", "naskar69", "@#$naskar05#$#", "" };
        private static object signupEmail = new object[] { "naskar3@test.com", "shit@test.com", "", "naskar69@test.com", "naskar156@test.com" };
        private static object signupPassword = new object[] { "abcd1234", "wrong", "" };
        private static object enableUsernameSpecialCharacters = new object[] { true, false };

        #endregion Mikros Sign-up Test-case parameters

        /// <summary>
        /// This is called every time before any test method executes
        /// Hence Mikros Initialization is done here, which is necessary for any Mikros SDK related operation
        /// </summary>
        [SetUp]
        public void Setup()
        {
            //MikrosManager.Instance.InitializeMikrosSDK(); // Initializing Mikros
        }

        /// <summary>
        /// Test Mikros Sign-in
        /// <param name="username">The username of the person signing in</param>
        /// <param name="email">email used to sign in</param>
        /// <param name="password">password used to sign in</param>
        /// </summary>
        [UnityTest]
        public IEnumerator SigninTest([ValueSourceAttribute(nameof(signinUsername))] string username, [ValueSourceAttribute(nameof(signinEmail))] string email, [ValueSourceAttribute(nameof(signinPassword))] string password)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isSigninProcessed = false;
            MikrosUser mikrosUser = null;
            SigninRequest.Builder()
                .Username(username)
                .Email(email)
                .Password(password)
                .Create(signinRequest =>
                {
                    MikrosManager.Instance.AuthenticationController.Signin(signinRequest, response =>
                    {
                        isSigninProcessed = true;
                        mikrosUser = response;
                    },
                    failure =>
                    {
                        isSigninProcessed = true;
                        LogAssert.Expect(LogType.Error, TestConstants.MikrosError + failure.Message);
                    });
                },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            // Testing sign-in request parameters validation
            Assert.IsFalse(string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email)); // Checking if both email and username are blank or not
            Assert.IsTrue(!string.IsNullOrEmpty(password)); // Checking if password is blank or not

            while (!isSigninProcessed)
            {
                yield return null;
            }

            // Testing to check if user data is returned in response for the sign-in request
            Assert.IsTrue(mikrosUser != null);
        }

        /// <summary>
        /// Test Mikros Sign-up
        /// <param name="username">The username of the person signing up</param>
        /// <param name="email">The email used to sign up</param>
        /// <param name="password">The password used to sign up</param>
        /// </summary>
        [UnityTest]
        public IEnumerator SignupTest([ValueSourceAttribute(nameof(signupUsername))] string username, [ValueSourceAttribute(nameof(signupEmail))] string email, [ValueSourceAttribute(nameof(signupPassword))] string password , [ValueSourceAttribute(nameof(enableUsernameSpecialCharacters))] bool enableUsernameSpecialCharacters)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isSignupProcessed = false;
            MikrosUser mikrosUser = null;
            SignupRequest.Builder()
                .Username(username)
                .Email(email)
                .Password(password)
                .EnableUsernameSpecialCharacters(enableUsernameSpecialCharacters)
                .Create(signupRequest =>
                {
                    MikrosManager.Instance.AuthenticationController.Signup(signupRequest, response =>
                    {
                        isSignupProcessed = true;
                        mikrosUser = response;
                    },
                    failure =>
                    {
                        isSignupProcessed = true;
                        LogAssert.Expect(LogType.Error, TestConstants.MikrosError + failure.Message);
                    });
                },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            // Testing sign-up request parameters validation
            Assert.IsTrue(!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(email)); // Checking if any of email or username is blank or not
            Assert.IsTrue(!string.IsNullOrEmpty(password)); // Checking if password is blank or not

            while (!isSignupProcessed)
            {
                yield return null;
            }

            // Testing to check if user data is returned in response for the sign-in request
            Assert.IsTrue(mikrosUser != null);
        }

        /// <summary>
        /// Test Mikros Sign-out after successful Sign-in
        /// <param name="username">The username of the person signing in</param>
        /// <param name="email">email used to sign in</param>
        /// <param name="password">password used to sign in</param>
        /// </summary>
        [UnityTest]
        public IEnumerator SignoutAfterSigninTest([ValueSourceAttribute(nameof(signinUsername))] string username, [ValueSourceAttribute(nameof(signinEmail))] string email, [ValueSourceAttribute(nameof(signinPassword))] string password)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isSigninProcessed = false;
            MikrosUser mikrosUser = null;
            MikrosException mikrosException = null;
            SigninRequest.Builder()
                .Username(username)
                .Email(email)
                .Password(password)
                .Create(signinRequest =>
                {
                    MikrosManager.Instance.AuthenticationController.Signin(signinRequest, response =>
                    {
                        isSigninProcessed = true;
                        mikrosUser = response;
                    },
                    failure =>
                    {
                        mikrosException = failure;
                        isSigninProcessed = true;
                        LogAssert.Expect(LogType.Error, TestConstants.MikrosError + failure.Message);
                    });
                },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            // Testing sign-in request parameters validation
            Assert.IsFalse(string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email)); // Checking if both email and username are blank or not
            Assert.IsTrue(!string.IsNullOrEmpty(password)); // Checking if password is blank or not

            while (!isSigninProcessed)
            {
                yield return null;
            }

            if (mikrosUser != null)
            {
                bool isSignoutProcessed = false;
                bool isSignoutCompleted = false;
                MikrosManager.Instance.AuthenticationController.Signout(() =>
                {
                    isSignoutCompleted = true;
                    isSignoutProcessed = true;
                },
                onFailure =>
                {
                    isSignoutProcessed = true;
                    Assert.Fail(onFailure.Message);
                });

                while (!isSignoutProcessed)
                {
                    yield return null;
                }

                // Testing to check if sign out is success
                Assert.IsTrue(isSignoutCompleted);
            }
            else
            {
                // Failing test if Success status code is not returned in response for the sign-in request
                Assert.Fail("Error: " + mikrosException.Message);
            }
        }

        /// <summary>
        /// Test Mikros Sign-out after successful Sign-up
        /// <param name="username">The username of the person signing up</param>
        /// <param name="email">The email used to sign up</param>
        /// <param name="password">The password used to sign up</param>
        /// </summary>
        [UnityTest]
        public IEnumerator SignoutAfterSignupTest([ValueSourceAttribute(nameof(signupUsername))] string username, [ValueSourceAttribute(nameof(signupEmail))] string email, [ValueSourceAttribute(nameof(signupPassword))] string password)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isSignupProcessed = false;
            MikrosUser mikrosUser = null;
            MikrosException mikrosException = null;
            SignupRequest.Builder()
                .Username(username)
                .Email(email)
                .Password(password)
                .Create(signupRequest =>
                {
                    MikrosManager.Instance.AuthenticationController.Signup(signupRequest, response =>
                    {
                        isSignupProcessed = true;
                        mikrosUser = response;
                    },
                    failure =>
                    {
                        mikrosException = failure;
                        isSignupProcessed = true;
                        LogAssert.Expect(LogType.Error, TestConstants.MikrosError + failure.Message);
                    });
                },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            // Testing sign-up request parameters validation
            Assert.IsTrue(!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(email)); // Checking if any of email or username is blank or not
            Assert.IsTrue(!string.IsNullOrEmpty(password)); // Checking if password is blank or not

            while (!isSignupProcessed)
            {
                yield return null;
            }

            if (mikrosUser != null)
            {
                bool isSignoutProcessed = false;
                bool isSignoutCompleted = false;
                MikrosManager.Instance.AuthenticationController.Signout(() =>
                {
                    isSignoutCompleted = true;
                    isSignoutProcessed = true;
                },
                onFailure =>
                {
                    isSignoutProcessed = true;
                    Assert.Fail(onFailure.Message);
                });

                while (!isSignoutProcessed)
                {
                    yield return null;
                }

                // Testing to check if sign out is success
                Assert.IsTrue(isSignoutCompleted);
            }
            else
            {
                // Failing test if Success status code is not returned in response for the sign-up request
                Assert.Fail("Error: " + mikrosException.Message);
            }
        }
    }
}