// src/index.js
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap/dist/js/bootstrap.bundle.min.js" // Add this line to include Bootstrap JavaScript
import React from "react"
import { createRoot } from "react-dom/client"
import "./index.css"
import App from "./App"
import reportWebVitals from "./reportWebVitals"
const container = document.getElementById("root")
const root = createRoot(container)

root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
)

reportWebVitals()