import { Route, Routes } from "react-router-dom";

import AuthComponent from "./components/AuthComponent";
import BlogContainer from "./components/Blog/BlogContainer/BlogContainer";
import PostContainer from "./components/Blog/PostContainer/PostContainer";
import DisableTracking from "./components/DisableTracking";
import NavBar from "./components/NavBar/NavBar";
import { AuthProvider } from "./contexts/AuthContext";
import AboutPage from "./pages/AboutPage/AboutPage";
import BlogLayout from "./pages/BlogLayout/BlogLayout";
import BoxOfficePage from "./pages/BoxOfficePage/BoxOfficePage";
import WgpmFmPage from "./pages/WgpmFmPage/WgpmFmPage";
import LetterboxPage from "./pages/LetterboxPage/LetterboxPage";
import OperationKinoPage from "./pages/OperationKinoPage/OperationKinoPage";
import { MenuItem } from "./types/MenuItem";

import "bootstrap/dist/css/bootstrap.min.css";
import "./reset.css";

const App: React.FC = () => {
  const menuItems: MenuItem[] = [
    { label: "about", link: "/about" },
    { label: "letterbox", link: "/letterbox" },
    { label: "blog", link: "/blog" },
    { label: "operation kino", link: "/operation-kino" },
    { label: "WGPM FM", link: "/wgpm-fm" },
  ];

  return (
    <>
      <NavBar
        logoSrc={"https://cdn.allenmaygibson.com/images/static/2011.png"}
        menuItems={menuItems}
      />
      {/* space dedicated to the navbar when it's at the top of the page */}
      <div style={{ marginTop: "60px" }}>
        <AuthProvider>
          <Routes>
            <Route path="/" element={<AboutPage />} />
            <Route path="/about" element={<AboutPage />} />
            <Route path="/letterbox" element={<LetterboxPage />} />
            <Route element={<BlogLayout />}>
              <Route path="/blog" element={<BlogContainer />} />
              <Route path="/blog/:slug" element={<PostContainer />} />
            </Route>
            <Route path="/operation-kino" element={<OperationKinoPage />} />
            <Route path="/wgpm-fm" element={<WgpmFmPage />} />
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
