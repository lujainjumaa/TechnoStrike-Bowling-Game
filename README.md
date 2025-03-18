# 🎳 TechnoStrike – Interactive Bowling Simulation  

**TechnoStrike** is an innovative **bowling simulation** that combines real-world physics with virtual gameplay using **Unity** and **Raspberry Pi with Python** for motion tracking.  

---

## Logo  
![Technostrike Logo](./images/Technostrike_logo.png)  

---

## 🏗 Project Overview  

### 🔹 **How It Works**  
1. A real **physical ball** is thrown, and a **camera** captures its movement.  
2. A **Python script (running on Raspberry Pi)** processes the ball's position using **Color Masking**.  
3. The data (position, velocity) is sent to **Unity via an API**.  
4. In **Unity**, the ball is simulated with real physics and interacts with **3D bowling pins**.  

---

## 🛠 Technologies Used  

### 🎮 Unity (C#)  
- **Physics Engine** to simulate bowling ball movement.  
- **3D models** created with **Blender**.  

### 🐍 Python (Raspberry Pi)  
- **OpenCV** for **Color Masking** & ball tracking.  
- **Flask API** to send data to Unity.  

---

## 🚀 Getting Started  

### 🔹 **Requirements**  
The current version of **TechnoStrike** is compatible with the following versions of the Unity Editor:  
- **Unity 2021.3** and later  

### 🔹 **Downloading the Project**  
1. Clone or download this repository to a workspace on your drive.  
2. Click the **⤓ Code** button on this page to get the URL to clone with Git, or click **Download ZIP** to get a copy of this repository that you can extract.  

### 🔹 **Opening the Project in Unity**  
1. In the **Projects** tab, click **Add**.  
2. Browse to the folder where you downloaded the repository and click **Select Folder**.  
3. Verify the project has been added as **TechnoStrike-Bowling-Game**, and click on it to open the project.  

---

### 🔹 **Python Setup**  
To set up the Python environment for the **Raspberry Pi**:

1. **Install Python**:
   - Download and install Python from [python.org](https://www.python.org/).
   - Ensure you check the box to **Add Python to PATH** during installation.

2. **Install OpenCV**:
   OpenCV is required for image processing and ball tracking. To install OpenCV, follow these steps:
   1. Open a **terminal** (Command Prompt on Windows, Terminal on macOS/Linux, or SSH into your Raspberry Pi).
   2. Run the following command to install OpenCV:
      ```bash
      pip install opencv-python
      ```

3. **Install Flask**:
   Flask is used to create the API for communication with Unity. To install Flask, follow these steps:
   1. Open a **terminal** (if not already open).
   2. Run the following command to install Flask:
      ```bash
      pip install flask
      ```

4. **Run the Python Script**:
   - Start the ball tracking and API server:
     ```bash
     python main.py
     ```
---

## 🎯 Features  

- ✔️ **Real-time ball tracking**  
- ✔️ **Interactive 3D environment**  
- ✔️ **Lightweight API for Unity-Python communication**  
- ✔️ **Scalable for small or large spaces**  

---

## 📝 License  

This project is licensed under the **MIT License** – see the [LICENSE](./LICENSE) file for details.  

---

## 📢 Contact  

For questions, suggestions, or contributions, feel free to reach out!  

📧 **Email1**: lujainjumaa2@gmail.com
📧 **Email2**: dana.joukhadar3@gmail.com