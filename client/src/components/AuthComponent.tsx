import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";

import { useAuthUpdate } from "../contexts/AuthContext";
import { debouncedLogin, debouncedLogout, makeDebouncedRequest } from "../personalWebsiteApi";

const AuthComponent: React.FC = () => {
  const [message, setMessage] = useState("");
  const updateIsLoggedIn = useAuthUpdate();
  const location = useLocation();

  const adminLogin = () => {
    makeDebouncedRequest(debouncedLogin, {
      url: "/login",
      method: "post",
    })
      .then((response) => {
        if (response.status === 200) {
          localStorage.setItem('isLoggedIn', 'true');
          updateIsLoggedIn(true);
          setMessage("Admin login successful");
        } else {
          setMessage("Admin login failed");
        }
      })
      .catch((error: any) => {
        console.error("Failed to log in as admin:", error);
        setMessage("Admin login failed");
      });
  };

  const adminLogout = () => {
    makeDebouncedRequest(debouncedLogout, {
      url: "/logout",
      method: "post",
    })
      .then(() => {
        localStorage.removeItem('isLoggedIn');
        updateIsLoggedIn(false);
        setMessage("Admin logout successful");
      })
      .catch((error: any) => {
        console.error("Admin logout failed:", error);
        setMessage("Admin logout failed");
      });
  };

  useEffect(() => {
    if (location.pathname === "/logout") {
      adminLogout();
    } else if (location.pathname === "/login") {
      adminLogin();
    }
  }, [location.pathname]);

  return <p>{message}</p>;
};

export default AuthComponent;
