import { Route, Routes } from "react-router-dom";

import NavBar from "./components/NavBar";
import AboutPage from "./pages/AboutPage";
import PicturesPage from "./pages/PicturesPage";

import "bootstrap/dist/css/bootstrap.min.css";

function App() {
  return (
    <>
      <NavBar />
      <Routes>
        <Route path="/" element={<AboutPage />} />
        <Route path="/about" element={<AboutPage />} />
        <Route path="/pictures" element={<PicturesPage />} />
      </Routes>
    </>
  );
}

export default App;
