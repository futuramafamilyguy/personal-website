import { createContext, ReactNode, useContext, useEffect, useState } from "react";

const AuthContext = createContext<boolean>(false);
const AuthUpdateContext = createContext<Function>(() => {});

export function useAuth() {
  return useContext(AuthContext);
}
export function useAuthUpdate() {
  return useContext(AuthUpdateContext);
}

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [isLoggedIn, setIsLoggedIn] = useState(
    localStorage.getItem("isLoggedIn") !== null
  );

  useEffect(() => {
    const loginStatus = localStorage.getItem("isLoggedIn");
    setIsLoggedIn(loginStatus !== null);
  }, []);

  const updateIsLoggedIn = (updatedIsLoggedIn: boolean) => {
    setIsLoggedIn(updatedIsLoggedIn);
  };

  return (
    <AuthContext.Provider value={isLoggedIn}>
      <AuthUpdateContext.Provider value={updateIsLoggedIn}>
        {children}
      </AuthUpdateContext.Provider>
    </AuthContext.Provider>
  );
};
