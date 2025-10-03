<a id="readme-top"></a>

[![Contributors][contributors-shield]][contributors-url]
[![Unlicense License][license-shield]][license-url]
[![Last Commit][last-commit-shield]][last-commit-url]

<div align="center">

  <h1 align="center">CONSISTENT SYSTEM</h1>

  <p align="center">
    <br />
    <a href="https://github.com/lazarnagulov/consistent-system/issues/new?labels=bug">Report Bug</a>
  </p>
</div>

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installation-steps">Installation Steps</a></li>
      </ul>
    </li>
    <li><a href="#project-details">Project Details</a></li>
    <li><a href="#theoretical-background">Theoretical Background</a></li>
  </ol>
</details>


## About The Project

This project simulates a **distributed temperature monitoring system** built with **WCF (Windows Communication Foundation)**.  
It demonstrates distributed consistency, replication, and synchronization principles in a sensor-based environment.

Main features:
- **Three independent sensors**, each with its own local SQLite database.
- Sensors generate temperature readings every **1–10 seconds (random interval)**.
- A **client application** communicates with sensors and validates data:
  - At least **two sensors** must report within ±5 of the global average for a reading to be valid.
  - Otherwise, the system triggers a **sensor alignment**.
- Automatic **alignment every 1 minute**:
  - All sensors update their last reading to the **average of the most recent values**.
  - During alignment, client reads are **blocked until completion**.
- The project explores concepts of **quorum-based replication** and the **CAP theorem** in distributed systems.

<br/>

## Built With

This project is built using the following technologies:

[![C#][CSharp-img]][CSharp-url]  
[![.NET][DotNet-img]][DotNet-url]  
[![WCF][WCF-img]][WCF-url]  
[![SQLite][SQLite-img]][SQLite-url]  

</br>

## Getting Started

Follow these steps to set up and run the project locally.

### Installation Steps

1. Clone the repository
   ```sh
   git clone https://github.com/lazarnagulov/consistent-system.git
   cd consistent-system
   ```
2. Open the solution in Visual Studio
Configure the WCF Service Project as the startup project.

3. Run the solution

- Start the WCF service hosting the sensors.
- Start the client application to query sensors and validate readings.
- Start the sensor aligner application.

</br>

## Project Details

- WCF Service Layer – hosts three independent temperature sensors, each writing to its own SQLite database.
- Client Application – queries sensors, validates data, and triggers alignment if required.
- Automatic Synchronization – every 1 minute, all sensors align their last value to the average.
</br>
## Theoretical Background

- Quorum-Based Replication – ensures consistency by requiring agreement from multiple sensors before accepting a value as correct.
- CAP Theorem – demonstrates the trade-offs between consistency, availability, and partition tolerance applied to distributed sensor systems.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

[CSharp-img]: https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white
[CSharp-url]: https://learn.microsoft.com/en-us/dotnet/csharp/

[DotNet-img]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=white
[DotNet-url]: https://dotnet.microsoft.com/

[WCF-img]: https://img.shields.io/badge/WCF-Windows%20Communication%20Foundation-0078D4?logo=windows&logoColor=white
[WCF-url]: https://learn.microsoft.com/en-us/dotnet/framework/wcf/

[SQLite-img]: https://img.shields.io/badge/Database-SQLite-003B57?logo=sqlite&logoColor=white
[SQLite-url]: https://www.sqlite.org/

[contributors-shield]: https://img.shields.io/github/contributors/lazarnagulov/consistent-system.svg?style=for-the-badge
[contributors-url]: https://github.com/lazarnagulov/consistent-system/graphs/contributors
[license-shield]: https://img.shields.io/github/license/lazarnagulov/consistent-system.svg?style=for-the-badge
[license-url]: https://github.com/lazarnagulov/consistent-system/blob/main/LICENSE
[last-commit-shield]: https://img.shields.io/github/last-commit/lazarnagulov/consistent-system?style=for-the-badge
[last-commit-url]: https://github.com/lazarnagulov/consistent-system/commits/main
