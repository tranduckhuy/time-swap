
<h1 align="center">
  <br>
  <a href="https://swap-time.vercel.app/"><img src="/frontend/public/assets/imgs/logo-1.png" alt="TimeSwap" width="200"></a>
  <br>
</h1>

<h4 align="center">Frontend Setup Guide</h4>

<p align="center">
  <a href="#prerequisites">Prerequisites</a> •
  <a href="#installation-steps">Installation Steps</a> •
  <a href="#additional-commands">Additional Commands</a>
</p>

## Prerequisites

Make sure you have the following installed before proceeding:

[Git](https://git-scm.com/)

[Node.js](https://nodejs.org/en) (Latest LTS version recommended)

## Installation Steps

### Command Line
```bash
# Clone this repository
$ git clone https://github.com/tranduckhuy/time-swap/

# Navigate to the frontend directory
$ cd time-swap/frontend

# Install dependencies
$ npm install
```
> **Note**
> If you're using Linux Bash for Windows, [see this guide](https://www.howtogeek.com/261575/how-to-run-graphical-linux-desktop-applications-from-windows-10s-bash-shell/) or use `node` from the command prompt.

### Create a .env file
Inside the frontend folder, create a file named .env and add the following variables:
```properties
API_AUTH_BASE_URL='YOUR AUTH API BASE URL GOES HERE'
API_BASE_URL='YOUR API BASE URL GOES HERE'
API_GPT_URL='YOUR API CHAT GPT URL GOES HERE'
AUTH_CLIENT_URL='YOUR DEPLOYED PAGE URL GOES HERE'
CHAT_GPT_KEY='YOUR CHAT GPT API KEY GOES HERE'
CHAT_GPT_ORG_KEY='YOUR CHAT GPT ORGANIZATION KEY GOES HERE'
```
> **Note**
> Make sure to replace the placeholder values with your actual configuration.

### Start the development server 
```bash
# Start server
$ npm start
```
> **Note**
> This will launch the frontend at <http://localhost:4200/> by default.

## Additional Commands
* Build the project for production:
```bash
# Build the project
$ npm run build
```

---

<p align="center">Now your frontend is up and running! 🚀</p>
