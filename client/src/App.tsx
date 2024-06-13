import { Route, Routes } from "react-router-dom";

import NavBar from "./components/NavBar";
import AboutPage from "./pages/AboutPage/AboutPage";
import LetterboxcPage from "./pages/LetterboxcPage/LetterboxcPage";

import "bootstrap/dist/css/bootstrap.min.css";
import "./reset.css";

function App() {
  return (
    <>
      <NavBar />
      <Routes>
        <Route path="/" element={<AboutPage />} />
        <Route path="/about" element={<AboutPage />} />
        <Route path="/letterboxc" element={<LetterboxcPage />} />
      </Routes>
    </>
  );
}

export default App;
