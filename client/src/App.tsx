import { Navigate, Route, Routes } from "react-router-dom";

import AuthComponent from "./components/AuthComponent";
import BlogContainer from "./components/Blog/BlogContainer/BlogContainer";
import PostContainer from "./components/Blog/PostContainer/PostContainer";
import DisableTracking from "./components/DisableTracking";
import MobileNavBar from "./components/MobileNavBar/MobileNavBar";
import NavBar from "./components/NavBar/NavBar";
import { AuthProvider } from "./contexts/AuthContext";
import { useIsMobile } from "./hooks/useIsMobile";
import AboutPage from "./pages/AboutPage/AboutPage";
import BlogLayout from "./pages/BlogLayout/BlogLayout";
import BoxOfficePage from "./pages/BoxOfficePage/BoxOfficePage";
import LetterboxPage from "./pages/LetterboxPage/LetterboxPage";
import OperationKinoPage from "./pages/OperationKinoPage/OperationKinoPage";
import QstFmPage from "./pages/QstFmPage/QstFmPage";
import { NavItem } from "./types/NavItem";

import "bootstrap/dist/css/bootstrap.min.css";
import "./reset.css";

const App: React.FC = () => {
  const navItems: NavItem[] = [
    { label: "about", path: "/about" },
    { label: "letterbox", path: "/letterbox" },
    { label: "blog", path: "/blog" },
    { label: "operation kino", path: "/operation-kino" },
  ];
  const CURRENT_YEAR = new Date().getFullYear();

  const isMobile = useIsMobile();

  return (
    <>
      {isMobile ? (
        <MobileNavBar navItems={navItems} />
      ) : (
        <NavBar
          logoSrc={"https://cdn.allenmaygibson.com/images/static/2011.png"}
          navItems={navItems}
        />
      )}
      {/* space dedicated to the navbar when it's at the top of the page */}
      <div style={isMobile ? {} : { marginTop: "60px" }}>
        <AuthProvider>
          <Routes>
            <Route path="/" element={<AboutPage />} />
            <Route path="/about" element={<AboutPage />} />
            <Route path="letterbox">
              <Route
                index
                element={<Navigate to={`${CURRENT_YEAR}`} replace />}
              />
              <Route path=":year" element={<LetterboxPage />} />
              <Route path=":year/focus" element={<LetterboxPage />} />
            </Route>
            <Route element={<BlogLayout />}>
              <Route path="/blog" element={<BlogContainer />} />
              <Route path="/blog/:slug" element={<PostContainer />} />
            </Route>
            <Route path="/operation-kino" element={<OperationKinoPage />} />
            <Route
              path="/operation-kino/focus"
              element={<OperationKinoPage />}
            />
            <Route path="/qst-fm" element={<QstFmPage />} />
            <Route path="/box-office" element={<BoxOfficePage />} />
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
