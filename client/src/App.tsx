import { Route, Routes } from "react-router-dom";

import logo from "./assets/face.png";
import DisableTracking from "./components/DisableTracking/DisableTracking";
import NavBar from "./components/NavBar/NavBar";
import AboutPage from "./pages/AboutPage/AboutPage";
import ComingSoonPage from "./pages/ComingSoonPage/ComingSoonPage";
import LetterboxcPage from "./pages/LetterboxcPage/LetterboxcPage";
import StatsPage from "./pages/StatsPage/StatsPage";
import { MenuItem } from "./types/MenuItem";

import "bootstrap/dist/css/bootstrap.min.css";
import "./reset.css";

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
          <Route path="/stats" element={<StatsPage />} />
          <Route path="/disable-tracking" element={<DisableTracking />} />
        </Routes>
      </div>
    </>
  );
};

export default App;
