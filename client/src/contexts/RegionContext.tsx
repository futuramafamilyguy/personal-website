import { createContext, ReactNode, useContext, useState } from "react";

type UpdateRegionContextType = (region: string) => void;

const RegionContext = createContext<string | undefined>(undefined);
const RegionUpdateContext = createContext<UpdateRegionContextType>(() => {});

export function useRegion() {
  return useContext(RegionContext);
}
export function useRegionUpdate() {
  return useContext(RegionUpdateContext);
}

interface RegionProviderProps {
  children: ReactNode;
}

export const RegionProvider = ({ children }: RegionProviderProps) => {
  const [Region, setRegion] = useState<string>(() => "Auckland");

  const updateRegion: UpdateRegionContextType = (newRegion: string) => {
    setRegion(newRegion);
  };

  return (
    <RegionContext.Provider value={Region}>
      <RegionUpdateContext.Provider value={updateRegion}>
        {children}
      </RegionUpdateContext.Provider>
    </RegionContext.Provider>
  );
};
