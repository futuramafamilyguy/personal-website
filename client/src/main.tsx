import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import "./index.css";
import App from "./App.tsx";

const isMacOs = () => {
  const platform = window.navigator.platform;
  return ["Macintosh", "MacIntel", "MacPPC", "Mac68K"].includes(platform);
};

const root = document.getElementById("root")!;
const portal = document.getElementById("portal")!;

if (isMacOs()) {
  root.style.fontFamily = "'Segoe UI', sans-serif";
  portal.style.fontFamily = "'Segoe UI', sans-serif";
}

ReactDOM.createRoot(root!).render(
  <React.StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </React.StrictMode>
);
