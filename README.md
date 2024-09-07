# Blogging Site CMS

**This project is currently in progress and will not have full functionality. Thank you for your understanding.**

## Table of Contents
- [Blogging Site CMS](#blogging-site-cms)
    - [Table of Contents](#table-of-contents)
    - [Summary](#summary)
    - [Tech Stack](#tech-stack)
    - [Installation](#installation)
        - [Prerequisites](#prerequisites)
    - [Setup Instructions](#setup-instructions)
    - [Usage](#usage)
    - [Features](#features)
    - [API Documentation](#api-documentation)
    - [Architecture](#architecture)

## Summary

Blogging Site CMS is a full-stack application that will allow users to create and manage their own blog posts. It provides a Markdown editor, that allows users to nicely format their posts, and upload their own images.

## Tech Stack

**Frontend**: React

**Backend**: ASP.NET Core

**Database**: Microsoft SQL Server

**Authentication:** ASP.NET Core Identity (Session)

## Installation

### Prerequisites

- Docker Desktop
- Docker Compose

## Setup Instructions

- Clone the repository by running the command `git clone https://github.com/brendokht/blogging-site-cms.git`
- Create a `.env` file in the root of the project folder in the following format
    
    ```
    ASPNET_CERT_PASSWORD=your-certificate-password
    MSSQL_SA_PASSWORD=your-sa-password
    DOCKER_CONNECTION_STRING=Data Source=database.server,1433;Initial Catalog=BloggingSiteCMS;User Id=SA;Password=your-sa-password;TrustServerCertificate=True
    ```
    
- Create a dotnet developer certificate by running the following command `dotnet dev-certs https -ep "C:\Users\your-username\.aspnet\https\bloggingsitecms.pfx" -p your-certificate-password`

## Usage

- Build and start the application by running `docker-compose up --build`
- Access the frontend by going to `https://localhost:5173`
    - You can also access the backend via Swagger by going to `https://localhost:8081/swagger/index.html`
        - This is the recommended way of creating an account of type *Admin*, allowing you to see extra administrator features
- To stop the application, you can run `docker-compose down`, or stop it via Docker Desktop

## Features

- Post Management: Add, update, draft and publish blog posts written in Markdown.
- Authentication: Register and login via ASP.NET Core Identity using Sessions.
- Responsive Design: Anything and everything on this site is responsive and can be done hassle free on the smallest and largest devices.
- Browsing specific categories and keywords (tags): Get right to what you are specifically looking for by searching keywords or going through any categories you want.

## API Documentation

- **TBA**

## Architecture

- The architecture I went for on this application is based off of the first architecture I learned while in school, a **3 Tier Architecture**, with **5 Logical Tiers**, slightly modified to separate concerns with Viewmodels and DTOs (Data Transfer Objects).
