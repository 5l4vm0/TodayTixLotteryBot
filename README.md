# TodayTix Lottery Bot

## About The Project
TodayTix Lottery Bot is an application designed to automatically sign up for TodayTix lottery tickets, increasing your chances of winning a lottery ticket for theatre play. It utilises Appium to interact with the TodayTix mobile app and automates the ticket application process on an Android device.

This project aims to simplify and automate the ticket application process for the user, reducing the manual effort required to enter multiple lotteries.

https://github.com/user-attachments/assets/ff6c7823-000d-41bc-adf6-f3922eed7cc1


[YoutubeLink](https://youtu.be/k57s3IfegSQ)


## Architecture and Design
The project is made up of two key components:
* Appium Test Automation: Utilises Appium to automate interactions with the TodayTix Android app on a virtual or connected Android device.
* Google Cloud Platform (GCP) Integration: Uses GCP's Gmail API to fetch relevant emails and metadata related to TodayTix lottery results.

The bot runs on a virtual device using Appium for automation. It installs the TodayTix APK on the device and proceeds to interact with the app to sign up for lottery tickets. Google Cloud Platform (GCP) is used to manage email access via the Gmail API, allowing the bot to handle sign up codes related to the TodayTix account sign up process.

## Built With
* Appium: Mobile app automation framework.
* .NET Core: The main development framework.
* GCP: Google Cloud Platform integration for managing Gmail API.
* Android Studio: For Android device emulation and SDK management.

## Getting Started
Follow these steps to get a local copy up and running.

### Prerequisites

* Install JDK 9
* Install Android Studio
    * After installing JDK 9 and Android Studio, set the ANDROID_HOME and JAVA_HOME environment variables to the installed paths
    * From Android Studio, install Android SDK Command Line from Android Studio
* Install [Appium](https://appium.io/docs/en/2.11/quickstart/)
* Google Cloud Platform (GCP) account
    * Go to Google Cloud Platform (GCP)
    * Enable GCP Gmail API in API and Services
    * Go to Credentials > Create Credentials > OAuth Client ID
    * Give it appropriate permissions to read the emails and metadata
    * Create the credentials
    * Download credentials client_secret.json (It will be named something like client_secret_123456789-blahblahblahblah.apps.googleusercontent.com.json, rename it to client_secret.json)
    * Put the client_secret.json inside `LotteryBot\LotteryBot\credentials\client_secret.json`
* Install TodayTix APK on an emulated or connected device using adb.  
    * If installing onto the emulated device, you can use the `adb install-multiple <multiple paths to apks>`/`adb install <path to apk>` commands

### Running Tests

To test if Appium is correctly configured:

1. Run the Appium server:

    ```
    # on Terminal instance 1
    appium
    ```

2. Run Android virtual device
    * Need to enable developer settings in the emulated device
    * `adb devices` should return your emulated

    ```
    adb devices
    List of devices attached
    emulator-5554   device
    ```

4. Run the test script

    ```
    # on Terminal instance 2
    cd LotteryBot\appiumtest
    dotnet test
    ```

## Running the lottery bot

To run the bot and start automating ticket signups:
1. Ensure the virtual or physical device is running.
2. To configure the lottery bot with your specific preferences, you'll need to modify certain values in the `Program.cs` file:
    * `_originalEmailAddress`: Set this to your email address. 
    * `_targetSingUpAmount`: Set this to the number of times you want the bot to sign up for the lottery.
    * `_attemptEmailNum`: Specify the starting email number that the bot should start with.
    * `_showName`: Enter the name of the show you want to sign up for in the lottery.
    * `_numberOfTicketsToWin`: Set the number of tickets you wish to sign up for.
2. Execute through
    ```
    dotnet run
    ```

## Limitations
* The bot currently works only on Android devices via an Appium connection.
* Google Cloud Platform integration is limited to Gmail API for retrieving lottery results, and no other services are utilised.
* Requires manual APK installation on emulated/connected Android devices.


## Roadmap
Future improvements for the bot include:
* Enhancing error handling and stability of the bot during sign-up processes.
* Expanding to support more platforms and iOS devices.
* Check for winning lottery email automatically
