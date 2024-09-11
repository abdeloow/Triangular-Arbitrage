# Cross-Exchange Trading Aggregator (C# Demo)

## Overview

This repository contains a C# demo of the core logic used in a larger Cross-Exchange Trading Aggregator project, which was developed for a customer. The project demonstrates key functionalities like triangular arbitrage detection, leveraging live market data from cryptocurrency exchanges such as Binance and Poloniex.

Due to confidentiality, only the core components of the system are presented here, excluding the React-based user interface and other proprietary features. I have received explicit permission from the contractor to publish this demo for review, and the customer has approved this release.
## Purpose

The goal of this demo is to:

- Showcase my expertise in handling cryptocurrency financial data and performing trade simulations.
- Demonstrate triangular arbitrage detection, using graph-based algorithms for identifying profitable trade cycles.
- Present a portion of the backend architecture, including service orchestration, exchange interactions, and algorithm design.

This project was developed under a contract with a customer, so only the backend logic is included for review.

## Technologies Used

- C# 12.0
- .NET 8.0
- RESTful API Integration (Binance and Poloniex)
- JSON Serialization with Newtonsoft.Json
- Graph Algorithms for market data analysis
- Task-based Asynchronous Programming for API calls

## Architecture & Design Patterns

- **Graph-based Market Representation**: The market data is modeled as a graph, where currencies are nodes and trading pairs are edges, enabling efficient cycle detection for arbitrage opportunities.

- **Separation of Concerns**:
  - **Triangular Arbitrage Detector**: Responsible for detecting potential arbitrage opportunities using market data.
  - **Trading Service**: Handles communication with the exchange clients and organizes the arbitrage detection logic.
  - **Exchange Helper**: Manages exchange-specific data like pair formatting and fee calculations.

- **Design Patterns**:
  - **Factory Pattern**: Used for creating exchange-specific clients (e.g., BinanceClient, PoloniexClient).
  - **Repository Pattern**: Utilized for managing the exchange data and retrieving the latest market rates.
  - **Strategy Pattern**: Applied for handling exchange-specific fee and pricing calculations within the TriangularTrade class.

- **REST API Scheme**:
  - Binance and Poloniex clients consume RESTful APIs to retrieve ticker data. Each exchange follows a different schema, but both offer endpoints for fetching trading pairs and market prices.
  - **HTTP Methods**: GET requests are primarily used to retrieve data from the exchanges.
  - **Response Parsing**: Data is fetched in JSON format and deserialized using Newtonsoft.Json into respective exchange-specific models.

## How to Run

1. **Clone the repository**:
   ```bash
   git clone https://github.com/abdeloow/Triangular-Arbitrage.git
2.   **Set up the environment**:
    Install the required packages:
    dotnet restore
3. **Run the demo**
    cd ConsoleApp1
    dotnet run
    The demo simulates triangular arbitrage detection by fetching live market data and identifying profitable trading cycles.

## Full Platform

The full platform for the Cross-Exchange Trading Aggregator includes:

    Real-time trade execution across multiple exchanges.
    Advanced fee handling and slippage calculation.
    Comprehensive user interface built with React (not included in this repository).

## Confidentiality Notice

This repository showcases only a portion of the larger customer project. Certain business logic and proprietary features have been omitted due to confidentiality. The full version includes additional trade execution capabilities and user interaction components.

For more information, please reach out to me directly -> dihbaouri@outlook.com.
