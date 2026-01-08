import { useEffect, useState } from "react";

export const useIsMobile = (breakpoint = 768) => {
  const query = `(max-width: ${breakpoint}px)`;
  const [isMobile, setIsMobile] = useState(
    () => window.matchMedia(query).matches
  );

  useEffect(() => {
    const media = window.matchMedia(query);
    const handler = (e: MediaQueryListEvent) => setIsMobile(e.matches);

    media.addEventListener("change", handler);
    return () => media.removeEventListener("change", handler);
  }, [query]);

  return isMobile;
};
