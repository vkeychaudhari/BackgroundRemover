# Background Remover 🖼️➡️✨

A WPF application that removes backgrounds from images using Python's `rembg` library.

<img width="1074" height="845" alt="image" src="https://github.com/user-attachments/assets/d05c1175-296b-401a-b06c-a964d24a3a1c" />

## Features

- Browse and select images (JPG, JPEG, PNG)
- Remove backgrounds with one click
- View original and processed images side-by-side
- Automatic Python environment detection
- Clean UI with progress feedback

## Prerequisites

- [Python 3.10+](https://www.python.org/downloads/)
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download)

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/vkeychaudhari/BackgroundRemover.git

2. Install Python dependencies:
   ```bash 
   pip install rembg pillow

3. Build and run the WPF application in Visual Studio

## Usage

1. Click "Browse Image" to select an image

2. Click "Remove Background" to process

3. View the result in the bottom panel

4. Find the processed image saved as [originalname]_nobg.png in the same folder

## How It Works

The application:

1. Uses WPF for the user interface

2. Calls a Python script (bgRemover.py) for background removal

3. Automatically detects Python installations

4. Provides visual feedback during processing

## Python Script

1. The core functionality is in Scripts/bgRemover.py which uses:

2. rembg for background removal

3. Pillow for image processing

4. Default u2net model (can be changed to other supported models)
