# RustDesk Configurer 🖥️🛠️

**RustDesk Configurer** is a C# GUI application (.NET Framework 4.7.2) that wraps the installation and configuration of the [RustDesk](https://rustdesk.com/) remote desktop client. It creates a seamless deployment experience by automating the download, installation, and connection setup for end-users.

## 🚀 Features

* **Smart Initialization:** Automatically accepts Terms of Service and prepares the environment upon launch.
* **Auto-Update & Repair:** Detects if RustDesk is already installed; performs a fresh install or repairs/updates existing versions by comparing local versions with the latest GitHub release.
* **Silent Deployment:** Handles the downloading and installation of the client in the background.
* **Resilient Configuration:** Fetches connection settings (Host/Key) from a remote server, with a built-in interactive fallback to local backup settings if the server is offline.


## 🛠️ Configuration & Build

**Important:** You must define the configuration sources before compiling the application.

### Method 1: Visual Studio (Recommended)
1. Open the solution in **Visual Studio**.
2. Navigate to **Project > Properties > Resources**.
3. Edit the values for the following resources:
   * **`SERVER_ENDPOINT_URL`**: The URL that returns your config string (Primary method).
   * **`DEFAULT_CONFIG_STRING`**: The hardcoded configuration string (Backup method).

### Method 2: Manual Edit
Alternatively, you can open and edit the `Properties\Resources.resx` file directly with a text or XML editor.

> 💡 **Recommendation:** We suggest prioritizing the **`SERVER_ENDPOINT_URL`** method. This allows you to update server settings (IP, Key) centrally without needing to recompile and redistribute the application to clients.

## ⚙️ How It Works

The application logic is divided into two distinct phases:

### 1. Initialization (Startup)
* **Terms of Service:** Upon opening, the user is prompted to accept the RustDesk License.
* **Configuration Retrieval:** The app immediately attempts to fetch the configuration string (Host/Key) from the `SERVER_ENDPOINT_URL`.
    * *Success:* The client downloads the configuration string from the endpoint.
    * *Failure:* The user is asked to Retry. If they cancel, they are offered the option to use the **Preconfigured Backup Data** (`DEFAULT_CONFIG_STRING`).
* **Mode Detection:** The app checks if RustDesk is already installed to determine if it should run in "Install" or "Repair" mode.

### 2. Installation (User Action)
Once the user clicks **Install/Repair**:
* **Version Check:** The app queries the official RustDesk GitHub API to find the latest version.
* **Download & Install:** If the local version is older or missing, the app downloads the latest release and performs a **silent installation**.
* **Apply Settings:** Finally, the application applies the configuration string retrieved during the startup phase, linking the client to your server.

## 📋 Requirements

* **OS:** Windows 7 SP1 or higher.
* **Framework:** .NET Framework 4.7.2.
* **Privileges:** Administrator rights are required to install the service.
* **Internet Connection:** Required for fetching the configuration and downloading the installer.

---

## ⚖️ Disclaimer

This application is an unofficial wrapper and is not affiliated with the RustDesk development team. RustDesk is open-source software; please ensure you comply with their licensing terms.
