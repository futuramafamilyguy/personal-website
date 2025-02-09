import { createContext, ReactNode, useContext, useState } from "react";

type UpdateYearContextType = (year: number) => void;

const YearContext = createContext<number | undefined>(undefined);
const YearUpdateContext = createContext<UpdateYearContextType>(() => {});

export function useYear() {
  return useContext(YearContext);
}
export function useYearUpdate() {
  return useContext(YearUpdateContext);
}

interface YearProviderProps {
  children: ReactNode;
}

export const YearProvider = ({ children }: YearProviderProps) => {
  const [year, setYear] = useState<number>(() => {
    let currentDate = new Date();
    let currentYear = currentDate.getFullYear();

    return currentYear - 1;
  });

  const updateYear: UpdateYearContextType = (newYear: number) => {
    setYear(newYear);
  };

  return (
    <YearContext.Provider value={year}>
      <YearUpdateContext.Provider value={updateYear}>
        {children}
      </YearUpdateContext.Provider>
    </YearContext.Provider>
  );
};
