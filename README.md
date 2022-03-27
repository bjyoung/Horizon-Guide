# Horizon Guide

Horizon Guide is a Windows application that adds a line overlay to the screen. It is useful for checking if a photograph or picture is level or not.

![Image Test](.\Data\DemoScreenshot2.png)

## How to install?

1. Visit the [repository](https://github.com/bjyoung/Horizon-Guide/releases) and download the latest HorizonGuideSetup.zip
1. Unzip and run the installer
1. Navigate to the installation folder and run 'Horizon Guide.exe'

## Setup Development Environment

1. Download [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
    - Having a Microsoft account is recommended
    - Skip the workloads section
1. Clone the [repository](https://github.com/bjyoung/Horizon-Guide)
    1. Open Visual Studio
    1. Choose the "clone a repository" option
    1. Get the project link (go to repo and click on 'Code')
    1. Paste link into Visual Studio for the 'repository location' and clone the repo
1. Setup project
    1. In Visual Studio, right-click on 'Horizon Guide.sln' > Open
    1. Follow Visual Studio Installer prompts to install required workloads
    1. Re-open Visual Studio and open 'Horizon Guide.sln' in the downloaded repo
    1. Go to Extensions > Manage Extensions
    1. Search for 'Microsoft Visual Studio Installer Projects 2022' and download it
    1. Close out of Visual Studio and follow the prompts
    1. Re-open the solution and right-click on HorizonGuideSetup > Reload
1. Setup markdown file editor
    1. Download [Visual Studio Code](https://code.visualstudio.com/download)
    2. Open Visual Studio Code and go to Extensions section (four squares icon in left bar)
    3. Search for the 'markdownlint' extension and install it

## Build Project

1. Right-click on HorizonGuideSetup > Build
2. The outputted installer is in ./HorizonGuideSetup/Debug/
    - Only need HorizonGuideSetup.msi to install the app

## Links

- Website: <https://www.brandonjamesyoung.com/HorizonGuide>
- Source: <https://github.com/bjyoung/Horizon-Guide>

## License

Horizon Guide

Copyright (c) 2022 Brandon Young

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
