### Requirements

* Install Appium - https://appium.io/docs/en/2.11/quickstart/
    * After installing JDK 9, and Android Studio set the ANDROID_HOME and JAVA_HOME directories to the installed paths
    * From Android Studio, install Android SDK Command Line from Android Studio
* Google Cloud Platform (GCP) account


### Setup

#### Test that appium works

* Run appium server

    ```
    # on Terminal instance 1
    appium
    ```

* Run Android virtual device
    * I had to enable developer settings in the emulated device
    * `adb devices` should return your emulated

    ```
    adb devices
    List of devices attached
    emulator-5554   device
    ```

* Install TodayTix APK on the emulated/connected device
    * If installing onto the emulated device, you can use the `adb install-multiple <multiple paths to apks>`/`adb install <path to apk>` commands

* Test that it works as expected

    ```
    # on Terminal instance 2
    cd LotteryBot\appiumtest
    dotnet test
      Determining projects to restore...
      All projects are up-to-date for restore.
      appiumtest -> C:\Users\Drum\OneDrive\workarea\programming\2024\LotteryBot\LotteryBot\appiumtest\bin\Debug\net8.0\appiumtest.dll
    Test run for C:\Users\Drum\OneDrive\workarea\programming\2024\LotteryBot\LotteryBot\appiumtest\bin\Debug\net8.0\appiumtest.dll (.NETCoreApp,Version=v8.0)
    Microsoft (R) Test Execution Command Line Tool Version 17.8.0 (x64)
    Copyright (c) Microsoft Corporation.  All rights reserved.

    Starting test execution, please wait...
    A total of 1 test files matched the specified pattern.

    Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: 2 s - appiumtest.dll (net8.0)
    ```


#### Setup GCP Gmail API Credentials

* Go to Google Cloud Platform (GCP)
* Enable GCP Gmail API in API and Services
* Go to Credentials > Create Credentials > OAuth Client ID
* Give it appropriate permissions to read the emails and metadata
* Create the credentials
* Download credentials client_secret.json (It will be named something like client_secret_123456789-blahblahblahblah.apps.googleusercontent.com.json, rename it to client_secret.json)
* Put the client_secret.json inside `LotteryBot\LotteryBot\credentials\client_secret.json`



#### Running the lottery bot

* Make sure that you have your emulated device running
    ```
    # from LotteryBot/LotteryBot
    dotnet run
    ```

### Next steps

#### Sign-up
* Click "Account" in bottom right corner
* Type in email address
* Click Continue
* Type "First name" & "Last name"
* Click "Send link"
* Check email for subject "Your one-time access code: xxxxxx" and extract the code from the subject
* Type in code into TodayTix
* Reject "App activity usage" by clicking "Rejact all"
* Click on "Search"
* Click on "Search events in London"
* Type "Harry Potter"
* Click "Harry Potter And The Cursed Child"
* Click Set Alert
...