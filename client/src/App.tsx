import { Route, Routes } from "react-router-dom";

import NavBar from "./components/NavBar/NavBar";
import AboutPage from "./pages/AboutPage/AboutPage";
import ComingSoonPage from "./pages/ComingSoonPage/ComingSoonPage";
import LetterboxcPage from "./pages/LetterboxcPage/LetterboxcPage";

import "bootstrap/dist/css/bootstrap.min.css";
import "./reset.css";
import { MenuItem } from "./types/MenuItem";

import logo from "./assets/allen-icon.jpeg";

const App: React.FC = () => {
  const menuItems: MenuItem[] = [
    { label: "About", link: "/about" },
    { label: "Letterboxc", link: "/letterboxc" },
    { label: "Coming Soon", link: "/coming-soon" },
  ];

  return (
    <>
      <NavBar logoSrc={logo} menuItems={menuItems} />
      {/* space dedicated to the navbar when it's at the top of the page */}
      <div style={{ marginTop: "60px" }}>
        <Routes>
          <Route path="/" element={<AboutPage />} />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/letterboxc" element={<LetterboxcPage />} />
          <Route path="/coming-soon" element={<ComingSoonPage />} />
        </Routes>
      </div>
    </>
  );
};

export default App;
