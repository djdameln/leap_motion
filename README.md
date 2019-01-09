## Leap Motion Game for Upper Extremity Rehabilitation

This repository contains the source code and a compiled version of a serious video game that aims to improve functional movement outcome of patients suffering upper extremity injuries using the Leap Motion Controller. The game was developed during the masterwork project of the High Tech Systems and Materials master honours programme at the University of Groningen. 

## Prerequisites

To play the game, you will need access to a Leap Motion Desktop Controller (https://www.leapmotion.com/product/desktop-controller/), and a windows computer. 

To download the required Leap Motion drivers (see instructions below), you will need to make a Leap Motion Developer account (https://developer.leapmotion.com). 

To download the Unity Editor needed to modify the source code and build the game from source, a Unity ID is required (https://unity3d.com).

## Instructions on how to use the software

Start by downloading and installing the Leap Motion Orion SDK. The installer can be downloaded from https://developer.leapmotion.com/get-started. The game was developed using version 4.0 of the Leap Motion Orion SDK, and has not been tested with older versions. To prevent compatibility issues it is recommended to use the same version. To this date, the Orion SDK is only available for windows, therefore the game does not run on other systems.

Save the source code and a compiled version of the game to your computer, by cloning or downloading this GitHub repository.
    
To start the compiled game, open the 'Build' folder and launch the windows executable 'LeapMotionExergame.exe'. Make sure that the Leap Motion Controller is plugged in and connected to your computer (as indicated by the green Leap Motion icon in the windows taskbar).
    
The source code can be viewed and modified using the Unity Editor. Download the latest version of the Unity Editor from https://store.unity.com. To load the source code of the game in Unity, launch the Unity Editor and choose 'Open Project'. Select the folder downloaded from GitHub in the file browser and click 'Open'. All assets and project settings will now be imported in a new Unity project that contains the full implementation of the rehabilitation game. The script 'HandTracker.sc' contains code that can be used to extract kinematics of the hands from the Leap Motion Controller.
