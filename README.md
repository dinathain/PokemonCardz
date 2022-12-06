<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#description">Description</a></li>
    <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
  </ol>
</details>

<!-- DESCRIPTION -->
## Description

This solution is comprised of 2 projects:
- <b>PokemonCardz</b>
   - This project is a .NET 6 MVC Web Application that contains:
      - Data access layer (this can be found in the Repositories folder
      - Service layer (this contains some minor validation and cleaning of data whilst calling the relevant repositories - can be found in the Services folders)
      - API (this is the single CardsController used to handle requests from the UI)
      - UI (this can be found in the Views folder)
- <b>PokemonCardzTests</b>
   - This project is a C# unit test project built with xUnit and Moq.
   

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

In order to run the web application, you will need:
- .NET 6.0 SDK
- Visual Studio

### Installation

In order to run the application, do the following:
- Set the 'PokemonCardz' project (web application) to be the Startup Project in Visual Studio
- Run the application

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- UNIT TESTS -->
## Unit tests

- The project named <b>PokemonCardzTests</b> is a unit test project built with xUnit and Moq.
- The scope of these tests currently test the Service layer of the application
- In order to ensure that the code is unit testable, I have used dependency injection and used Moq as the framework for mocking those dependencies

<p align="right">(<a href="#readme-top">back to top</a>)</p>