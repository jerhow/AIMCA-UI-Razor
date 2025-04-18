# AI Medical Coding Assistant - Web UI

## Overview

This project is a web-based user interface. Its primary purpose is to demonstrate the functionality of a [backend API](https://github.com/jerhow/AI-medical-coding-assistant) designed for AI-assisted medical coding (specifically ICD-10-CM code searching based on diagnostic descriptions).

The UI allows users to input a search query and a maximum number of results, sends an authenticated request to the backend API, and displays the structured results returned by the API.

## What It Does

* Simple web interface for querying the medical coding API.
* Accepts user input for diagnostic query text and maximum results.
* Authenticates with the backend API using a Bearer token passed in the request header.
* Sends POST requests with a JSON payload to the API.
* Parses and displays structured JSON results from the API in a table format.
* Deployable to Azure App Service.

## Technologies Used

* **.NET 8:** Framework runtime
* **ASP.NET Core 8 Razor Pages:** Web UI framework
* **C#:** Programming language
* **HTML5 + CSS3 + Bootstrap 5:** Front-end structure and custom styling
* **Azure App Service:** Target deployment platform

## Prerequisites

* .NET 8 SDK
* Azure App Service
* Access to the running backend **AI Medical Coding Assistant API** endpoint ([separate repo](https://github.com/jerhow/AI-medical-coding-assistant))
* A valid **Bearer Token** for authenticating with the API

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
