import { createContext, ReactNode, useContext, useState } from "react";

const ViewFavoriteContext = createContext<boolean>(false);
const ViewFavoriteUpdateContext = createContext<Function>(() => {});

export function useViewFavorite() {
  return useContext(ViewFavoriteContext);
}
export function useViewFavoriteUpdate() {
  return useContext(ViewFavoriteUpdateContext);
}

interface ViewFavoriteProviderProps {
  children: ReactNode;
}

export const ViewFavoriteProvider = ({
  children,
}: ViewFavoriteProviderProps) => {
  const [viewFavorite, setViewFavorite] = useState(false);

  const toggleViewFavorite = () => {
    setViewFavorite((prevViewFavorite) => !prevViewFavorite);
  };

  return (
    <ViewFavoriteContext.Provider value={viewFavorite}>
      <ViewFavoriteUpdateContext.Provider value={toggleViewFavorite}>
        {children}
      </ViewFavoriteUpdateContext.Provider>
    </ViewFavoriteContext.Provider>
  );
};
