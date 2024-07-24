import { Route, Routes } from "react-router-dom";

import logo from "./assets/face.png";
import AuthComponent from "./components/AuthComponent";
import DisableTracking from "./components/DisableTracking";
import NavBar from "./components/NavBar/NavBar";
import { AuthProvider } from "./contexts/AuthContext";
import AboutPage from "./pages/AboutPage/AboutPage";
import ComingSoonPage from "./pages/ComingSoonPage/ComingSoonPage";
import LetterboxcPage from "./pages/LetterboxcPage/LetterboxcPage";
import SessionsPage from "./pages/SessionsPage/SessionsPage";
import StatsPage from "./pages/StatsPage/StatsPage";
import { MenuItem } from "./types/MenuItem";

import "bootstrap/dist/css/bootstrap.min.css";
import "./reset.css";

const App: React.FC = () => {
  const menuItems: MenuItem[] = [
    { label: "About", link: "/about" },
    { label: "Letterboxc", link: "/letterboxc" },
    { label: "Sessions", link: "/sessions" },
    { label: "Coming Soon", link: "/coming-soon" },
  ];

  return (
    <>
      <NavBar logoSrc={logo} menuItems={menuItems} />
      {/* space dedicated to the navbar when it's at the top of the page */}
      <div style={{ marginTop: "60px" }}>
        <AuthProvider>
          <Routes>
            <Route path="/" element={<AboutPage />} />
            <Route path="/about" element={<AboutPage />} />
            <Route path="/letterboxc" element={<LetterboxcPage />} />
            <Route path="/sessions" element={<SessionsPage />} />
            <Route path="/coming-soon" element={<ComingSoonPage />} />
            <Route path="/stats" element={<StatsPage />} />
            <Route path="/disable-tracking" element={<DisableTracking />} />
            <Route path="/login" element={<AuthComponent />} />
            <Route path="/logout" element={<AuthComponent />} />
          </Routes>
        </AuthProvider>
      </div>
    </>
  );
};

export default App;
