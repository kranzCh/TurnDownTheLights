# TurnDownTheLights

TurnDownTheLights is a Windows utility designed to quickly turn off RGB lighting on ASUS Aura compatible devices and turn off your monitors. This can be useful to reduce distractions or save power.

## Features

*   **ASUS Aura Sync Control:** Turns off RGB lights on all connected Aura Sync compatible devices.
*   **Monitor Control:** Turns off all connected monitors.
*   **Automatic Exit:** The application exits when the mouse is clicked.
*   **Secondary Screen Blackout:** The main application window is designed to cover secondary screens, effectively blacking them out while the primary monitor (where the app is not shown) can be turned off by the monitor control feature.

## Prerequisites

*   **Operating System:** Windows
*   **Framework:** .NET Framework (inspect `.csproj` or `TurnDownTheLights.sln` for specific version)
*   **ASUS Aura SDK:** Must be installed for RGB lighting control to work.
*   **Multiple Monitors:** The screen blackout feature is most effective with multiple monitors.

## Usage

1.  **Run the application:** Execute `TurnDownTheLights.exe`.
2.  **Lights Off & Monitors Off:** Upon launch, the application will automatically:
    *   Turn off Aura RGB lights.
    *   Turn off all monitors.
    *   Display a black window covering secondary screens.
3.  **Exit:** Click the mouse anywhere to close the application. When the application exits, it releases control of Aura devices, which may cause them to revert to their previous lighting state or a default state.

## Building the Project

This project is a C# Windows Forms application.
1.  Open `TurnDownTheLights.sln` with Visual Studio.
2.  Build the solution (typically Ctrl+Shift+B or via the Build menu).
3.  The executable will be located in a subfolder like `bin/Debug/` or `bin/Release/`.

## How It Works

*   **Aura Control:** Uses the `AuraServiceLib` to connect to the ASUS Aura SDK, enumerate connected devices, and set their light colors to off (0).
*   **Monitor Control:** Uses `SendMessage` P/Invoke call with `user32.dll` to send a system command (`SC_MONITORPOWER`) to turn off monitors.
*   **Window Management:** The main form is sized and positioned to cover all secondary screens. Mouse movement on these screens will re-trigger the monitor off command. A mouse click anywhere will exit the application.
*   **Cursor Initialization:** The mouse cursor is centered on the primary screen on startup to avoid immediate exit if the cursor is over a secondary screen where the main form is displayed.
