### Requirements

* Install Appium - https://appium.io/docs/en/2.11/quickstart/
    * After installing JDK 9, and Android Studio set the ANDROID_HOME and JAVA_HOME directories to the installed paths
    * From Android Studio, install Android SDK Command Line from Android Studio

### Setup

* Run appium server

    ```
    # on Terminal instance 1
    appium
    ```

* Run Android virtual device (or connect your real device)
    * I had to enable developer settings in the emulated device
    *  s `adb devices` should return your emulated/connected

    ```
    adb devices
    List of devices attached
    emulator-5554   device
    ```

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